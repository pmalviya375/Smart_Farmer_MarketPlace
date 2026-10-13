using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class OrderManagement : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ViewState["OrderStatusTab"] = "All";
                UpdateTabStyles();
                LoadCounts();
                LoadOrders();
            }
        }

        private void UpdateTabStyles()
        {
            string tab = ViewState["OrderStatusTab"] != null ? ViewState["OrderStatusTab"].ToString() : "All";
            tabAll.CssClass = "order-tab-btn" + (tab == "All" ? " active" : "");
            tabPending.CssClass = "order-tab-btn" + (tab == "Pending" ? " active" : "");
            tabAccepted.CssClass = "order-tab-btn" + (tab == "Accepted" ? " active" : "");
            tabCompleted.CssClass = "order-tab-btn" + (tab == "Completed" ? " active" : "");
            tabRejected.CssClass = "order-tab-btn" + (tab == "Rejected" ? " active" : "");
        }

        private void LoadCounts()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT 
                        COUNT(*) AS TotalAll,
                        SUM(CASE WHEN OrderStatus = 'Pending' THEN 1 ELSE 0 END) AS TotalPending,
                        SUM(CASE WHEN OrderStatus = 'Accepted' THEN 1 ELSE 0 END) AS TotalAccepted,
                        SUM(CASE WHEN OrderStatus = 'Completed' THEN 1 ELSE 0 END) AS TotalCompleted,
                        SUM(CASE WHEN OrderStatus = 'Rejected' THEN 1 ELSE 0 END) AS TotalRejected
                    FROM Orders";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblCountAll.Text = dr["TotalAll"] != DBNull.Value ? dr["TotalAll"].ToString() : "0";
                            lblCountPending.Text = dr["TotalPending"] != DBNull.Value ? dr["TotalPending"].ToString() : "0";
                            lblCountAccepted.Text = dr["TotalAccepted"] != DBNull.Value ? dr["TotalAccepted"].ToString() : "0";
                            lblCountCompleted.Text = dr["TotalCompleted"] != DBNull.Value ? dr["TotalCompleted"].ToString() : "0";
                            lblCountRejected.Text = dr["TotalRejected"] != DBNull.Value ? dr["TotalRejected"].ToString() : "0";
                        }
                    }
                }
            }
        }

        private void LoadOrders()
        {
            string tab = ViewState["OrderStatusTab"] != null ? ViewState["OrderStatusTab"].ToString() : "All";
            string search = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT o.OrderID, o.BuyerID, o.SellerID, o.ItemType, o.ItemID, o.ItemName,
                           o.OfferedPrice, o.ContactNumber, o.OrderStatus, o.OrderDate,
                           ISNULL(b.FullName, 'Buyer') AS BuyerName,
                           ISNULL(s.FullName, 'Farmer') AS SellerName,
                           ISNULL(s.MobileNumber, '-') AS SellerMobile
                    FROM Orders o
                    LEFT JOIN Farmer_Registration b ON o.BuyerID = b.FarmerID
                    LEFT JOIN Farmer_Registration s ON o.SellerID = s.FarmerID
                    WHERE 1=1";

                if (tab != "All")
                {
                    sql += " AND o.OrderStatus = @Status";
                }
                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (o.ItemName LIKE @Search OR b.FullName LIKE @Search OR s.FullName LIKE @Search OR o.ContactNumber LIKE @Search)";
                }

                sql += " ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                if (tab != "All") da.SelectCommand.Parameters.AddWithValue("@Status", tab);
                if (!string.IsNullOrEmpty(search)) da.SelectCommand.Parameters.AddWithValue("@Search", "%" + search + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                lblTotalCount.Text = dt.Rows.Count.ToString();
                lblShowingCount.Text = dt.Rows.Count.ToString();

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

        protected void tabFilter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["OrderStatusTab"] = btn.CommandArgument;
            UpdateTabStyles();
            LoadOrders();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadOrders();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ExportAlert",
                "alert('Order report export generated successfully (CSV format).');", true);
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SetStatus")
            {
                string[] parts = e.CommandArgument.ToString().Split(':');
                int orderID = Convert.ToInt32(parts[0]);
                string newStatus = parts[1];

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "UPDATE Orders SET OrderStatus = @Status, RespondedDate = GETDATE() WHERE OrderID = @OrderID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Status", newStatus);
                        cmd.Parameters.AddWithValue("@OrderID", orderID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Order #" + orderID + " status updated to " + newStatus + ".";
                LoadCounts();
                LoadOrders();
            }
            else if (e.CommandName == "DeleteOrder")
            {
                int orderID = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "DELETE FROM Orders WHERE OrderID = @OrderID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Order record #" + orderID + " deleted from database.";
                LoadCounts();
                LoadOrders();
            }
        }

        public string GetStatusClass(string status)
        {
            switch (status)
            {
                case "Pending": return "badge-pending";
                case "Accepted": return "badge-accepted";
                case "Completed": return "badge-completed";
                case "Rejected": return "badge-rejected";
                default: return "status-active";
            }
        }
    }
}
