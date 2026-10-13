using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class OrderRequest : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ViewState["Tab"] = "Incoming";
                LoadCounts();
                LoadOrders();
            }
        }

        private void LoadCounts()
        {
            int myID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand c1 = new SqlCommand(
                    "SELECT COUNT(*) FROM Orders WHERE SellerID=@ID AND OrderStatus='Pending'", con);
                c1.Parameters.AddWithValue("@ID", myID);

                SqlCommand c2 = new SqlCommand(
                    "SELECT COUNT(*) FROM Orders WHERE BuyerID=@ID", con);
                c2.Parameters.AddWithValue("@ID", myID);

                con.Open();
                lblIncomingCount.Text = c1.ExecuteScalar().ToString();
                lblOutgoingCount.Text = c2.ExecuteScalar().ToString();
            }
        }

        private void LoadOrders()
        {
            int myID = Convert.ToInt32(Session["FarmerID"]);
            string tab = ViewState["Tab"].ToString();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string filter = tab == "Incoming"
                    ? "o.SellerID = @MyID"
                    : "o.BuyerID = @MyID";

                string q = $@"
                    SELECT o.OrderID, o.ItemType, o.ItemID, o.ItemName, o.OrderStatus,
                           o.OrderDate, o.ContactNumber, o.BuyerID, o.SellerID,
                           b.FullName AS BuyerName, s.FullName AS SellerName,
                           CASE o.ItemType
                               WHEN 'Crop' THEN (SELECT MainImage FROM Crop WHERE CropID=o.ItemID)
                               WHEN 'Equipment' THEN (SELECT MainImage FROM Equipment WHERE EquipmentID=o.ItemID)
                               WHEN 'Animal' THEN (SELECT MainImage FROM Animal WHERE AnimalID=o.ItemID)
                           END AS MainImage
                    FROM Orders o
                    INNER JOIN Farmer_Registration b ON o.BuyerID = b.FarmerID
                    INNER JOIN Farmer_Registration s ON o.SellerID = s.FarmerID
                    WHERE {filter}
                    ORDER BY o.OrderDate DESC
                ";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                da.SelectCommand.Parameters.AddWithValue("@MyID", myID);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlEmpty.Visible = true;
                    rptOrders.Visible = false;
                }
                else
                {
                    pnlEmpty.Visible = false;
                    rptOrders.Visible = true;
                    rptOrders.DataSource = dt;
                    rptOrders.DataBind();
                }
            }
        }

        public string GetIcon(string type)
        {
            switch (type)
            {
                case "Crop": return "🌾";
                case "Equipment": return "🚜";
                case "Animal": return "🐄";
                default: return "📦";
            }
        }

        protected void Tab_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["Tab"] = btn.CommandArgument;

            tabIncoming.CssClass = "order-tab";
            tabOutgoing.CssClass = "order-tab";
            btn.CssClass = "order-tab active";

            LoadOrders();
        }

        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            DataRowView row = (DataRowView)e.Item.DataItem;
            string status = row["OrderStatus"].ToString();
            if (status != "Pending")
                return;

            string tab = ViewState["Tab"] == null ? "Incoming" : ViewState["Tab"].ToString();
            LinkButton btnAccept = (LinkButton)e.Item.FindControl("btnAccept");
            LinkButton btnCancel = (LinkButton)e.Item.FindControl("btnCancel");

            // Seller (Incoming): Accept + Cancel
            // Buyer (Outgoing): Cancel only
            if (tab == "Incoming")
            {
                if (btnAccept != null) btnAccept.Visible = true;
                if (btnCancel != null) btnCancel.Visible = true;
            }
            else
            {
                if (btnCancel != null) btnCancel.Visible = true;
            }
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int orderID = Convert.ToInt32(e.CommandArgument);
            int myID = Convert.ToInt32(Session["FarmerID"]);
            string tab = ViewState["Tab"] == null ? "Incoming" : ViewState["Tab"].ToString();

            string newStatus = e.CommandName == "Accept" ? "Accepted"
                             : e.CommandName == "Cancel" ? "Cancelled"
                             : null;
            if (newStatus == null) return;

            // Only sellers can Accept
            if (e.CommandName == "Accept" && tab != "Incoming")
                return;

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string roleFilter = tab == "Incoming"
                        ? "SellerID=@MyID"
                        : "BuyerID=@MyID";

                    SqlCommand cmd = new SqlCommand(
                        $@"UPDATE Orders SET OrderStatus=@Status, RespondedDate=GETDATE()
                          WHERE OrderID=@ID AND {roleFilter} AND OrderStatus='Pending'", con);
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@ID", orderID);
                    cmd.Parameters.AddWithValue("@MyID", myID);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0 && newStatus == "Accepted")
                        UpdateItemStatusIfAccepted(orderID, con);
                }

                LoadCounts();
                LoadOrders();
                ClientScript.RegisterStartupScript(this.GetType(), "ok",
                    "alert('Order " + newStatus.ToLower() + " successfully!');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "err",
                    "alert('Error: " + ex.Message.Replace("'", "") + "');", true);
            }
        }

        private void UpdateItemStatusIfAccepted(int orderID, SqlConnection con)
        {
            SqlCommand getCmd = new SqlCommand(
                "SELECT ItemType, ItemID FROM Orders WHERE OrderID=@ID", con);
            getCmd.Parameters.AddWithValue("@ID", orderID);

            string type = null;
            int itemID = 0;

            using (SqlDataReader dr = getCmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    type = dr["ItemType"].ToString();
                    itemID = Convert.ToInt32(dr["ItemID"]);
                }
            }

            if (type == null) return;

            string table = type == "Crop" ? "Crop"
                         : type == "Equipment" ? "Equipment"
                         : "Animal";
            string idCol = type == "Crop" ? "CropID"
                         : type == "Equipment" ? "EquipmentID"
                         : "AnimalID";
            string statusVal = type == "Equipment" ? "Rented" : "Sold";

            SqlCommand upd = new SqlCommand(
                $"UPDATE {table} SET Status=@S WHERE {idCol}=@ID", con);
            upd.Parameters.AddWithValue("@S", statusVal);
            upd.Parameters.AddWithValue("@ID", itemID);
            upd.ExecuteNonQuery();
        }
    }
}
