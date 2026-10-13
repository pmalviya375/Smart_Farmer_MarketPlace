using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class Marketplace : System.Web.UI.Page
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
                ViewState["Category"] = "All";
                LoadMarketplace();
            }
        }

        // ==================== LOAD DATA ====================
        private void LoadMarketplace()
        {
            int myFarmerID = Convert.ToInt32(Session["FarmerID"]);
            string category = ViewState["Category"] == null ? "All" : ViewState["Category"].ToString();
            string search = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                // UNION: Crop + Equipment + Animal
                string q = @"
                    SELECT 'Crop' AS ItemType, c.CropID AS ItemID, c.CropName AS ItemName,
                           c.Quantity AS Price, c.Status, c.MainImage, c.DateAdded,
                           f.FullName AS SellerName, f.Village AS SellerVillage
                    FROM Crop c
                    INNER JOIN Farmer_Registration f ON c.FarmerID = f.FarmerID
                    WHERE c.FarmerID != @MyID AND ISNULL(c.Status,'') <> 'Sold'

                    UNION ALL

                    SELECT 'Equipment' AS ItemType, e.EquipmentID AS ItemID, e.EquipmentName AS ItemName,
                           e.AskingPrice AS Price, e.Status, e.MainImage, e.DateAdded,
                           f.FullName AS SellerName, f.Village AS SellerVillage
                    FROM Equipment e
                    INNER JOIN Farmer_Registration f ON e.FarmerID = f.FarmerID
                    WHERE e.FarmerID != @MyID AND ISNULL(e.Status,'') NOT IN ('Rented','Sold')

                    UNION ALL

                    SELECT 'Animal' AS ItemType, a.AnimalID AS ItemID, a.AnimalName AS ItemName,
                           a.ExpectedPrice AS Price, a.Status, a.MainImage, a.DateAdded,
                           f.FullName AS SellerName, f.Village AS SellerVillage
                    FROM Animal a
                    INNER JOIN Farmer_Registration f ON a.FarmerID = f.FarmerID
                    WHERE a.FarmerID != @MyID AND ISNULL(a.Status,'') <> 'Sold'
                ";

                string finalQ = "SELECT * FROM (" + q + ") AS x WHERE 1=1";

                if (category != "All")
                    finalQ += " AND x.ItemType = @Cat";

                if (!string.IsNullOrEmpty(search))
                    finalQ += " AND x.ItemName LIKE @Search";

                finalQ += " ORDER BY x.DateAdded DESC";

                SqlDataAdapter da = new SqlDataAdapter(finalQ, con);
                da.SelectCommand.Parameters.AddWithValue("@MyID", myFarmerID);

                if (category != "All")
                    da.SelectCommand.Parameters.AddWithValue("@Cat", category);

                if (!string.IsNullOrEmpty(search))
                    da.SelectCommand.Parameters.AddWithValue("@Search", "%" + search + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlEmpty.Visible = true;
                    pnlGrid.Visible = false;
                }
                else
                {
                    pnlEmpty.Visible = false;
                    pnlGrid.Visible = true;

                    rptMarketplace.DataSource = dt;
                    rptMarketplace.DataBind();
                }
            }
        }

        // ==================== HELPER METHODS (for markup) ====================
        public string GetIcon(string itemType)
        {
            switch (itemType)
            {
                case "Crop": return "🌾";
                case "Equipment": return "🚜";
                case "Animal": return "🐄";
                default: return "📦";
            }
        }

        public string GetBadgeClass(string itemType)
        {
            return itemType == "Equipment" ? "badge-rent" : "badge-sale";
        }

        public string GetBadgeText(string itemType, string status)
        {
            if (itemType == "Equipment") return "Rental";
            if (status == "Sold") return "Sold";
            return "For Sale";
        }

        public string GetPriceUnit(string itemType)
        {
            return itemType == "Equipment" ? "<small>/ day</small>" : "";
        }

        public string GetButtonStyle(string itemType)
        {
            return itemType == "Animal" ? "btn-view-outline" : "";
        }

        public string GetRating(object itemID)
        {
            int id = Convert.ToInt32(itemID);
            return (3.5 + (id % 20) / 10.0).ToString("0.0");
        }

        // ==================== EVENTS ====================
        protected void FilterChip_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["Category"] = btn.CommandArgument;

            // Reset all chips
            chipAll.CssClass = "chip";
            chipEquip.CssClass = "chip";
            chipAnimal.CssClass = "chip";
            chipCrop.CssClass = "chip";

            // Activate clicked
            btn.CssClass = "chip active";

            LoadMarketplace();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadMarketplace();
        }

        protected void btnLoadMore_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "info",
                "alert('All items loaded!');", true);
        }
    }
}