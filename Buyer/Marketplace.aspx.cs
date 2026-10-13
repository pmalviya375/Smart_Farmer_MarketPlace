using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class Marketplace : System.Web.UI.Page
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
                if (!string.IsNullOrEmpty(Request.QueryString["search"]))
                {
                    txtSearch.Text = Request.QueryString["search"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    ViewState["Category"] = Request.QueryString["type"];
                }
                else
                {
                    ViewState["Category"] = "All";
                }

                UpdateChipStyles();
                LoadMarketplace();
            }
        }

        private void UpdateChipStyles()
        {
            string cat = ViewState["Category"] == null ? "All" : ViewState["Category"].ToString();
            chipAll.CssClass = "chip" + (cat == "All" ? " active" : "");
            chipCrop.CssClass = "chip" + (cat == "Crop" ? " active" : "");
            chipEquip.CssClass = "chip" + (cat == "Equipment" ? " active" : "");
            chipAnimal.CssClass = "chip" + (cat == "Animal" ? " active" : "");
        }

        // ==================== LOAD MARKETPLACE DATA ====================
        private void LoadMarketplace()
        {
            string category = ViewState["Category"] == null ? "All" : ViewState["Category"].ToString();
            string search = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                // Disconnected Model using SqlDataAdapter and DataTable
                // UNION: Crop + Equipment + Animal
                string q = @"
                    SELECT 'Crop' AS ItemType, c.CropID AS ItemID, c.CropName AS ItemName,
                           c.Quantity AS Price, c.Status, c.MainImage, c.DateAdded,
                           f.FullName AS SellerName, f.Village AS SellerVillage
                    FROM Crop c
                    INNER JOIN Farmer_Registration f ON c.FarmerID = f.FarmerID
                    WHERE ISNULL(c.Status,'') <> 'Sold'

                    UNION ALL

                    SELECT 'Equipment' AS ItemType, e.EquipmentID AS ItemID, e.EquipmentName AS ItemName,
                           e.AskingPrice AS Price, e.Status, e.MainImage, e.DateAdded,
                           f.FullName AS SellerName, f.Village AS SellerVillage
                    FROM Equipment e
                    INNER JOIN Farmer_Registration f ON e.FarmerID = f.FarmerID
                    WHERE ISNULL(e.Status,'') NOT IN ('Rented','Sold')

                    UNION ALL

                    SELECT 'Animal' AS ItemType, a.AnimalID AS ItemID, a.AnimalName AS ItemName,
                           a.ExpectedPrice AS Price, a.Status, a.MainImage, a.DateAdded,
                           f.FullName AS SellerName, f.Village AS SellerVillage
                    FROM Animal a
                    INNER JOIN Farmer_Registration f ON a.FarmerID = f.FarmerID
                    WHERE ISNULL(a.Status,'') <> 'Sold'
                ";

                string finalQ = "SELECT * FROM (" + q + ") AS x WHERE 1=1";

                if (category != "All")
                    finalQ += " AND x.ItemType = @Cat";

                if (!string.IsNullOrEmpty(search))
                    finalQ += " AND x.ItemName LIKE @Search";

                finalQ += " ORDER BY x.DateAdded DESC";

                SqlDataAdapter da = new SqlDataAdapter(finalQ, con);

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

        // ==================== HELPER METHODS ====================
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

        public string GetBadgeClass(string itemType)
        {
            return itemType == "Equipment" ? "badge-rent" : "badge-sale";
        }

        public string GetBadgeText(string itemType, string status)
        {
            if (itemType == "Equipment") return "Machinery";
            if (itemType == "Animal") return "Livestock";
            if (status == "Sold") return "Sold";
            return "Fresh Harvest";
        }

        public string GetPriceUnit(string itemType)
        {
            if (itemType == "Equipment") return "<small>/ day</small>";
            if (itemType == "Crop") return "<small>/ Qtl</small>";
            return "";
        }

        public string GetButtonStyle(string itemType)
        {
            return itemType == "Animal" ? "btn-view-outline" : "";
        }

        public string GetRating(object itemID)
        {
            int id = Convert.ToInt32(itemID);
            return (4.0 + (id % 10) / 10.0).ToString("0.0");
        }

        // ==================== EVENTS ====================
        protected void FilterChip_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["Category"] = btn.CommandArgument;
            UpdateChipStyles();
            LoadMarketplace();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadMarketplace();
        }
    }
}
