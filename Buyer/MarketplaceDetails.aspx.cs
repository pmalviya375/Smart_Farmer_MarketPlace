using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class MarketplaceDetails : System.Web.UI.Page
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
                string type = Request.QueryString["type"];
                string idStr = Request.QueryString["id"];

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(idStr))
                {
                    Response.Redirect("Marketplace.aspx");
                    return;
                }

                LoadItemDetails(type, Convert.ToInt32(idStr));
            }
        }

        private void LoadItemDetails(string type, int itemID)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string q = "";
                switch (type)
                {
                    case "Crop":
                        q = @"SELECT c.CropName AS ItemName, c.Quantity AS Price,
                                     c.MainImage, c.Image2, c.Image3, c.Image4,
                                     c.FarmerAddress AS Address, c.ContactNumber AS Mobile,
                                     ISNULL(c.CropName, '') AS Description,
                                     c.FarmerID AS SellerID,
                                     f.FullName AS SellerName, f.Village AS SellerVillage,
                                     f.CreatedDate AS SellerSince
                              FROM Crop c
                              INNER JOIN Farmer_Registration f ON c.FarmerID = f.FarmerID
                              WHERE c.CropID = @ID";
                        break;

                    case "Equipment":
                        q = @"SELECT e.EquipmentName AS ItemName, e.AskingPrice AS Price,
                                     e.MainImage, e.Image2, e.Image3, e.Image4,
                                     e.FarmLocation AS Address, e.MobileNumber AS Mobile,
                                     ISNULL(e.ConditionNotes, '') AS Description,
                                     e.FarmerID AS SellerID,
                                     f.FullName AS SellerName, f.Village AS SellerVillage,
                                     f.CreatedDate AS SellerSince
                              FROM Equipment e
                              INNER JOIN Farmer_Registration f ON e.FarmerID = f.FarmerID
                              WHERE e.EquipmentID = @ID";
                        break;

                    case "Animal":
                        q = @"SELECT a.AnimalName AS ItemName, a.ExpectedPrice AS Price,
                                     a.MainImage, a.Image2, a.Image3, a.Image4,
                                     a.FarmAddress AS Address, a.MobileNumber AS Mobile,
                                     ISNULL(a.HealthStatus, '') AS Description,
                                     a.FarmerID AS SellerID,
                                     f.FullName AS SellerName, f.Village AS SellerVillage,
                                     f.CreatedDate AS SellerSince
                              FROM Animal a
                              INNER JOIN Farmer_Registration f ON a.FarmerID = f.FarmerID
                              WHERE a.AnimalID = @ID";
                        break;

                    default:
                        Response.Redirect("Marketplace.aspx");
                        return;
                }

                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@ID", itemID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    Response.Redirect("Marketplace.aspx");
                    return;
                }

                lblItemName.Text = dr["ItemName"].ToString();
                lblPrice.Text = Convert.ToDecimal(dr["Price"]).ToString("N0");
                lblPriceUnit.Text = type == "Equipment" ? "/ day rental" : (type == "Crop" ? "/ Quintal" : "");
                lblTypeBadge.Text = type;

                lblDescription.Text = string.IsNullOrEmpty(dr["Description"].ToString())
                    ? "Freshly listed directly by local farmer. High quality agricultural produce / asset."
                    : dr["Description"].ToString();

                SetImage(imgMain, dr["MainImage"]);
                SetImage(imgThumb1, dr["MainImage"]);
                SetImage(imgThumb2, dr["Image2"]);
                SetImage(imgThumb3, dr["Image3"]);
                SetImage(imgThumb4, dr["Image4"]);

                string sellerName = dr["SellerName"].ToString();
                lblSellerName.Text = sellerName;
                lblSellerInitial.Text = string.IsNullOrEmpty(sellerName)
                    ? "F"
                    : sellerName.Substring(0, 1).ToUpper();
                lblSellerAddress.Text = dr["SellerVillage"].ToString();
                lblSellerSince.Text = "Member since " +
                    Convert.ToDateTime(dr["SellerSince"]).Year;

                lblBreadcrumbType.Text = type;
                lblBreadcrumbName.Text = dr["ItemName"].ToString();

                // Save to ViewState
                ViewState["ItemType"] = type;
                ViewState["ItemID"] = itemID;
                ViewState["ItemName"] = dr["ItemName"].ToString();
                ViewState["Price"] = dr["Price"].ToString();
                ViewState["SellerID"] = Convert.ToInt32(dr["SellerID"]);
                ViewState["SellerMobile"] = dr["Mobile"].ToString();
            }

            LoadSpecs(type, itemID);
        }

        private void LoadSpecs(string type, int itemID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Icon");
            dt.Columns.Add("Label");
            dt.Columns.Add("Value");

            if (type == "Equipment")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT UsageYears, UsageMonths, MakeModel, ConditionNotes
                          FROM Equipment WHERE EquipmentID=@ID", con);
                    cmd.Parameters.AddWithValue("@ID", itemID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dt.Rows.Add("<i class='fa-regular fa-calendar'></i>", "Usage", dr["UsageYears"] + " Yrs, " + dr["UsageMonths"] + " Mos");
                        dt.Rows.Add("<i class='fa-solid fa-gear'></i>", "Make & Model", dr["MakeModel"].ToString());
                        dt.Rows.Add("<i class='fa-solid fa-file-lines'></i>", "Condition", string.IsNullOrEmpty(dr["ConditionNotes"].ToString()) ? "Well maintained" : dr["ConditionNotes"].ToString());
                        dt.Rows.Add("<i class='fa-solid fa-tractor'></i>", "Category", "Agricultural Machinery");
                    }
                }
            }
            else if (type == "Crop")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT Quantity, CropName, DateAdded FROM Crop WHERE CropID=@ID", con);
                    cmd.Parameters.AddWithValue("@ID", itemID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dt.Rows.Add("<i class='fa-solid fa-seedling'></i>", "Produce", dr["CropName"].ToString());
                        dt.Rows.Add("<i class='fa-solid fa-box-archive'></i>", "Available Qty", dr["Quantity"] + " Quintals");
                        dt.Rows.Add("<i class='fa-solid fa-circle-check'></i>", "Quality", "Grade A Organic Produce");
                        dt.Rows.Add("<i class='fa-regular fa-calendar'></i>", "Harvest / Listed", Convert.ToDateTime(dr["DateAdded"]).ToString("dd MMM yyyy"));
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT Age, AgeUnit, Breed, HealthStatus
                          FROM Animal WHERE AnimalID=@ID", con);
                    cmd.Parameters.AddWithValue("@ID", itemID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dt.Rows.Add("<i class='fa-solid fa-cow'></i>", "Age", dr["Age"] + " " + dr["AgeUnit"]);
                        dt.Rows.Add("<i class='fa-solid fa-dna'></i>", "Breed", string.IsNullOrEmpty(dr["Breed"].ToString()) ? "Purebred" : dr["Breed"].ToString());
                        dt.Rows.Add("<i class='fa-solid fa-syringe'></i>", "Health Check", string.IsNullOrEmpty(dr["HealthStatus"].ToString()) ? "Healthy / Vaccinated" : dr["HealthStatus"].ToString());
                        dt.Rows.Add("<i class='fa-solid fa-shield-halved'></i>", "Verification", "AgriTech Certified Livestock");
                    }
                }
            }

            rptSpecs.DataSource = dt;
            rptSpecs.DataBind();
        }

        private void SetImage(Image img, object imgPath)
        {
            if (imgPath == null || imgPath == DBNull.Value || string.IsNullOrEmpty(imgPath.ToString()))
            {
                img.Visible = false;
            }
            else
            {
                img.ImageUrl = ResolveUrl(imgPath.ToString());
                img.Visible = true;
            }
        }

        protected void btnContactSeller_Click(object sender, EventArgs e)
        {
            string mobile = ViewState["SellerMobile"] == null
                ? "Not available"
                : ViewState["SellerMobile"].ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "contact",
                "alert('Farmer Direct Contact Number: " + mobile.Replace("'", "") + "');", true);
        }

        protected void btnRequestBuy_Click(object sender, EventArgs e)
        {
            if (ViewState["ItemType"] != null && ViewState["ItemID"] != null)
            {
                string type = ViewState["ItemType"].ToString();
                string id = ViewState["ItemID"].ToString();
                Response.Redirect("OrderRequest.aspx?type=" + Server.UrlEncode(type) + "&id=" + Server.UrlEncode(id));
            }
            else
            {
                Response.Redirect("Marketplace.aspx");
            }
        }
    }
}
