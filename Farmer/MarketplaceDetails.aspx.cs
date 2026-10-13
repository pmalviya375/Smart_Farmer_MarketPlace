using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class MarketplaceDetails : System.Web.UI.Page
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
                lblDescription.Text = string.IsNullOrEmpty(dr["Description"].ToString())
                    ? "No description provided."
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
                lblSellerFirstName.Text = sellerName.Split(' ')[0];
                lblSellerAddress.Text = dr["SellerVillage"].ToString();
                lblSellerSince.Text = "Member since " +
                    Convert.ToDateTime(dr["SellerSince"]).Year;

                lblBreadcrumbType.Text = type;
                lblBreadcrumbName.Text = dr["ItemName"].ToString();

                ViewState["ItemType"] = type;
                ViewState["ItemID"] = itemID;
                ViewState["ItemName"] = dr["ItemName"].ToString();
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
                        dt.Rows.Add("📅", "Usage",
                            dr["UsageYears"] + " Years, " + dr["UsageMonths"] + " Months");
                        dt.Rows.Add("⚙️", "Model", dr["MakeModel"].ToString());
                        dt.Rows.Add("📝", "Notes",
                            string.IsNullOrEmpty(dr["ConditionNotes"].ToString())
                                ? "Good condition"
                                : dr["ConditionNotes"].ToString());
                        dt.Rows.Add("✅", "Condition", "Available");
                    }
                }
            }
            else if (type == "Crop")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT Quantity, CropName FROM Crop WHERE CropID=@ID", con);
                    cmd.Parameters.AddWithValue("@ID", itemID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dt.Rows.Add("🌾", "Crop", dr["CropName"].ToString());
                        dt.Rows.Add("📦", "Quantity / Price", dr["Quantity"].ToString());
                        dt.Rows.Add("✅", "Quality", "Farm Fresh");
                        dt.Rows.Add("🤝", "Deal Type", "Farmer to Farmer");
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
                        dt.Rows.Add("🐄", "Age", dr["Age"] + " " + dr["AgeUnit"]);
                        dt.Rows.Add("🧬", "Breed",
                            string.IsNullOrEmpty(dr["Breed"].ToString()) ? "-" : dr["Breed"].ToString());
                        dt.Rows.Add("💉", "Health",
                            string.IsNullOrEmpty(dr["HealthStatus"].ToString())
                                ? "Healthy"
                                : dr["HealthStatus"].ToString());
                        dt.Rows.Add("🤝", "Deal Type", "Farmer to Farmer");
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
                "alert('Seller contact: " + mobile.Replace("'", "") + "');", true);
        }

        protected void btnRequestBuy_Click(object sender, EventArgs e)
        {
            if (ViewState["SellerID"] == null || ViewState["ItemType"] == null)
            {
                Response.Redirect("Marketplace.aspx");
                return;
            }

            int myFarmerID = Convert.ToInt32(Session["FarmerID"]);
            int sellerID = Convert.ToInt32(ViewState["SellerID"]);
            string itemType = ViewState["ItemType"].ToString();
            int itemID = Convert.ToInt32(ViewState["ItemID"]);
            string itemName = ViewState["ItemName"].ToString();

            if (myFarmerID == sellerID)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "warn",
                    "alert('You cannot buy your own listing!');", true);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand check = new SqlCommand(
                        @"SELECT COUNT(*) FROM Orders 
                          WHERE BuyerID=@BID AND ItemType=@Type AND ItemID=@IID 
                            AND OrderStatus='Pending'", con);
                    check.Parameters.AddWithValue("@BID", myFarmerID);
                    check.Parameters.AddWithValue("@Type", itemType);
                    check.Parameters.AddWithValue("@IID", itemID);
                    con.Open();

                    if ((int)check.ExecuteScalar() > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "dup",
                            "alert('You already sent a request for this item!');", true);
                        return;
                    }

                    string contact = Session["FarmerMobile"] != null
                        ? Session["FarmerMobile"].ToString()
                        : "0000000000";

                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Orders 
                            (BuyerID, SellerID, ItemType, ItemID, ItemName, 
                             ContactNumber, OrderStatus, OrderDate)
                          VALUES (@BuyerID, @SellerID, @Type, @IID, @IName,
                                  @Contact, 'Pending', GETDATE())", con);

                    cmd.Parameters.AddWithValue("@BuyerID", myFarmerID);
                    cmd.Parameters.AddWithValue("@SellerID", sellerID);
                    cmd.Parameters.AddWithValue("@Type", itemType);
                    cmd.Parameters.AddWithValue("@IID", itemID);
                    cmd.Parameters.AddWithValue("@IName", itemName);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.ExecuteNonQuery();
                }

                ClientScript.RegisterStartupScript(this.GetType(), "ok",
                    "alert('Order request sent! Track it in Order Request page.');" +
                    "window.location='OrderRequest.aspx';", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "err",
                    "alert('Error: " + ex.Message.Replace("'", "") + "');", true);
            }
        }
    }
}
