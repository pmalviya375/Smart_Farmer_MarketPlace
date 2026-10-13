using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class MyOrders : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BuyerID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ViewState["Tab"] = "All";
                UpdateTabStyles();
                LoadOrderCounts();
                LoadOrders();
            }
        }

        private void UpdateTabStyles()
        {
            string tab = ViewState["Tab"] == null ? "All" : ViewState["Tab"].ToString();
            tabAll.CssClass = "order-tab" + (tab == "All" ? " active" : "");
            tabPending.CssClass = "order-tab" + (tab == "Pending" ? " active" : "");
            tabAccepted.CssClass = "order-tab" + (tab == "Accepted" ? " active" : "");
            tabCancelled.CssClass = "order-tab" + (tab == "Cancelled" ? " active" : "");
        }

        private void LoadOrderCounts()
        {
            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT 
                        COUNT(*) AS TotalAll,
                        SUM(CASE WHEN OrderStatus = 'Pending' THEN 1 ELSE 0 END) AS TotalPending,
                        SUM(CASE WHEN OrderStatus = 'Accepted' THEN 1 ELSE 0 END) AS TotalAccepted,
                        SUM(CASE WHEN OrderStatus = 'Cancelled' THEN 1 ELSE 0 END) AS TotalCancelled
                    FROM Orders
                    WHERE BuyerID = @BuyerID";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@BuyerID", buyerID);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblAllCount.Text = dr["TotalAll"] != DBNull.Value ? dr["TotalAll"].ToString() : "0";
                            lblPendingCount.Text = dr["TotalPending"] != DBNull.Value ? dr["TotalPending"].ToString() : "0";
                            lblAcceptedCount.Text = dr["TotalAccepted"] != DBNull.Value ? dr["TotalAccepted"].ToString() : "0";
                            lblCancelledCount.Text = dr["TotalCancelled"] != DBNull.Value ? dr["TotalCancelled"].ToString() : "0";
                        }
                    }
                }
            }
        }

        private void LoadOrders()
        {
            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            string tab = ViewState["Tab"] == null ? "All" : ViewState["Tab"].ToString();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT o.OrderID, o.ItemType, o.ItemID, o.ItemName, o.OfferedPrice, o.Message, 
                           o.ContactNumber, o.OrderStatus, o.OrderDate,
                           f.FullName AS SellerName, f.Village AS SellerVillage,
                           COALESCE(c.MainImage, e.MainImage, a.MainImage) AS MainImage
                    FROM Orders o
                    INNER JOIN Farmer_Registration f ON o.SellerID = f.FarmerID
                    LEFT JOIN Crop c ON o.ItemType = 'Crop' AND o.ItemID = c.CropID
                    LEFT JOIN Equipment e ON o.ItemType = 'Equipment' AND o.ItemID = e.EquipmentID
                    LEFT JOIN Animal a ON o.ItemType = 'Animal' AND o.ItemID = a.AnimalID
                    WHERE o.BuyerID = @BuyerID";

                if (tab != "All")
                {
                    sql += " AND o.OrderStatus = @Status";
                }

                sql += " ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@BuyerID", buyerID);
                if (tab != "All")
                {
                    da.SelectCommand.Parameters.AddWithValue("@Status", tab);
                }

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

        public string GetIcon(string itemType)
        {
            switch (itemType)
            {
                case "Crop": return "<i class='fa-solid fa-seedling' style='color:#15803d;'></i>";
                case "Equipment": return "<i class='fa-solid fa-tractor' style='color:#ea580c;'></i>";
                case "Animal": return "<i class='fa-solid fa-cow' style='color:#b45309;'></i>";
                default: return "<i class='fa-solid fa-box' style='color:#64748b;'></i>";
            }
        }

        protected void Tab_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["Tab"] = btn.CommandArgument;
            UpdateTabStyles();
            LoadOrders();
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                int orderID = Convert.ToInt32(e.CommandArgument);
                int buyerID = Convert.ToInt32(Session["BuyerID"]);

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "UPDATE Orders SET OrderStatus = 'Cancelled' WHERE OrderID = @OrderID AND BuyerID = @BuyerID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderID);
                        cmd.Parameters.AddWithValue("@BuyerID", buyerID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadOrderCounts();
                LoadOrders();

                ClientScript.RegisterStartupScript(this.GetType(), "cancelled",
                    "alert('Order request #ORD-" + orderID.ToString().PadLeft(5, '0') + " was cancelled successfully.');", true);
            }
        }
    }
}
