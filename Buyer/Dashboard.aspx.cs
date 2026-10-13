using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class Dashboard : System.Web.UI.Page
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
                InitGreetings();
                LoadDashboardStats();
                LoadRecentOrders();
                LoadFeaturedMarketplaceProduce();
            }
        }

        private void InitGreetings()
        {
            int hour = DateTime.Now.Hour;
            string greeting = (hour < 12) ? "Good morning" : (hour < 17 ? "Good afternoon" : "Good evening");
            lblGreeting.Text = greeting;

            string buyerName = Session["BuyerName"] != null ? Session["BuyerName"].ToString() : "Buyer";
            lblBuyerName.Text = buyerName;
            lblToday.Text = DateTime.Now.ToString("dddd, dd MMM yyyy");
        }

        private void LoadDashboardStats()
        {
            int buyerID = Convert.ToInt32(Session["BuyerID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // 1. Available Crops Count
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Crop WHERE ISNULL(Status,'') <> 'Sold'", con))
                {
                    lblCropCount.Text = cmd.ExecuteScalar().ToString();
                }

                // 2. Available Equipment Count
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Equipment WHERE ISNULL(Status,'') NOT IN ('Rented','Sold')", con))
                {
                    lblEquipCount.Text = cmd.ExecuteScalar().ToString();
                }

                // 3. Available Animal Count
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Animal WHERE ISNULL(Status,'') <> 'Sold'", con))
                {
                    lblAnimalCount.Text = cmd.ExecuteScalar().ToString();
                }

                // 4. Buyer's Total Orders
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Orders WHERE BuyerID = @BuyerID", con))
                {
                    cmd.Parameters.AddWithValue("@BuyerID", buyerID);
                    string total = cmd.ExecuteScalar().ToString();
                    lblTotalOrders.Text = total;
                    lblTotalOrdersHero.Text = total;
                }

                // 5. Buyer's Pending Orders
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Orders WHERE BuyerID = @BuyerID AND OrderStatus = 'Pending'", con))
                {
                    cmd.Parameters.AddWithValue("@BuyerID", buyerID);
                    string pending = cmd.ExecuteScalar().ToString();
                    lblPendingOrders.Text = pending;
                    lblPendingHero.Text = pending;
                }

                // 6. Buyer's Confirmed Orders
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Orders WHERE BuyerID = @BuyerID AND OrderStatus = 'Accepted'", con))
                {
                    cmd.Parameters.AddWithValue("@BuyerID", buyerID);
                    lblConfirmedOrders.Text = cmd.ExecuteScalar().ToString();
                }
            }
        }

        private void LoadRecentOrders()
        {
            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT TOP 5 o.OrderID, o.ItemName, o.ItemType, o.OfferedPrice, o.OrderStatus, o.OrderDate,
                                 f.FullName AS SellerName
                    FROM Orders o
                    INNER JOIN Farmer_Registration f ON o.SellerID = f.FarmerID
                    WHERE o.BuyerID = @BuyerID
                    ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@BuyerID", buyerID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlNoOrders.Visible = true;
                    rptRecentOrders.Visible = false;
                }
                else
                {
                    pnlNoOrders.Visible = false;
                    rptRecentOrders.Visible = true;
                    rptRecentOrders.DataSource = dt;
                    rptRecentOrders.DataBind();
                }
            }
        }

        private void LoadFeaturedMarketplaceProduce()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string q = @"
                    SELECT TOP 2 'Crop' AS ItemType, CropID AS ItemID, CropName AS ItemName,
                           Quantity AS Price, MainImage, DateAdded, f.Village AS SellerVillage
                    FROM Crop c
                    INNER JOIN Farmer_Registration f ON c.FarmerID = f.FarmerID
                    WHERE ISNULL(c.Status,'') <> 'Sold'

                    UNION ALL

                    SELECT TOP 2 'Equipment' AS ItemType, EquipmentID AS ItemID, EquipmentName AS ItemName,
                           AskingPrice AS Price, MainImage, DateAdded, f.Village AS SellerVillage
                    FROM Equipment e
                    INNER JOIN Farmer_Registration f ON e.FarmerID = f.FarmerID
                    WHERE ISNULL(e.Status,'') NOT IN ('Rented','Sold')

                    UNION ALL

                    SELECT TOP 2 'Animal' AS ItemType, AnimalID AS ItemID, AnimalName AS ItemName,
                           ExpectedPrice AS Price, MainImage, DateAdded, f.Village AS SellerVillage
                    FROM Animal a
                    INNER JOIN Farmer_Registration f ON a.FarmerID = f.FarmerID
                    WHERE ISNULL(a.Status,'') <> 'Sold'
                    ORDER BY DateAdded DESC";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptFeaturedProduce.DataSource = dt;
                rptFeaturedProduce.DataBind();
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
    }
}
