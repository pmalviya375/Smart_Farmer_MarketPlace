using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class EquipmentForm : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;
        private const string IMG_FOLDER = "~/images/Equipment/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] == null) { Response.Redirect("Login.aspx"); return; }

            if (!IsPostBack)
            {
                LoadCategoryDropdown();

                if (Request.QueryString["id"] != null)
                {
                    hfEquipmentID.Value = Request.QueryString["id"];
                    lblPageTitle.Text = "Edit Equipment";
                    btnSave.Text = "💾 Update Listing";
                    LoadEquipmentData(Convert.ToInt32(hfEquipmentID.Value));
                    rfvMainImg.Enabled = false;   // Edit mode — main image optional
                }
                else
                {
                    rfvMainImg.Enabled = true;    // Add mode — main image required
                }
            }
        }
        private void LoadCategoryDropdown()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT CategoryID, CategoryName FROM Equipment_Category WHERE FarmerID=@FID ORDER BY CategoryName", con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("Select equipment type", ""));
            }
        }

        private void LoadEquipmentData(int equipID)
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT EquipmentName, CategoryID, MakeModel, UsageYears, UsageMonths,
                             AskingPrice, ConditionNotes, MobileNumber, PostalCode, FarmLocation,
                             MainImage, Image2, Image3, Image4
                      FROM Equipment WHERE EquipmentID=@ID AND FarmerID=@FID", con);
                cmd.Parameters.AddWithValue("@ID", equipID);
                cmd.Parameters.AddWithValue("@FID", farmerID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtEquipmentName.Text = dr["EquipmentName"].ToString();
                    ddlCategory.SelectedValue = dr["CategoryID"].ToString();
                    txtMakeModel.Text = dr["MakeModel"].ToString();
                    txtUsageYears.Text = dr["UsageYears"].ToString();
                    txtUsageMonths.Text = dr["UsageMonths"].ToString();
                    txtPrice.Text = dr["AskingPrice"].ToString();
                    txtCondition.Text = dr["ConditionNotes"] == DBNull.Value ? "" : dr["ConditionNotes"].ToString();
                    txtMobile.Text = dr["MobileNumber"].ToString();
                    txtPostal.Text = dr["PostalCode"] == DBNull.Value ? "" : dr["PostalCode"].ToString();
                    txtLocation.Text = dr["FarmLocation"].ToString();

                    // Show existing images
                    ShowExistingImages(dr);
                }
            }
        }

        private void ShowExistingImages(SqlDataReader dr)
        {
            // Main
            if (dr["MainImage"] != DBNull.Value && !string.IsNullOrEmpty(dr["MainImage"].ToString()))
            {
                imgMain.ImageUrl = ResolveUrl(dr["MainImage"].ToString());
                imgMain.CssClass = "image-preview show";
                lblMainImage.Text = "✓ Current main image";
                lblMainImage.Visible = true;
            }
            // 2
            if (dr["Image2"] != DBNull.Value && !string.IsNullOrEmpty(dr["Image2"].ToString()))
            {
                img2.ImageUrl = ResolveUrl(dr["Image2"].ToString());
                img2.CssClass = "image-preview show";
                lblImage2.Text = "✓ Current image 2";
                lblImage2.Visible = true;
            }
            // 3
            if (dr["Image3"] != DBNull.Value && !string.IsNullOrEmpty(dr["Image3"].ToString()))
            {
                img3.ImageUrl = ResolveUrl(dr["Image3"].ToString());
                img3.CssClass = "image-preview show";
                lblImage3.Text = "✓ Current image 3";
                lblImage3.Visible = true;
            }
            // 4
            if (dr["Image4"] != DBNull.Value && !string.IsNullOrEmpty(dr["Image4"].ToString()))
            {
                img4.ImageUrl = ResolveUrl(dr["Image4"].ToString());
                img4.CssClass = "image-preview show";
                lblImage4.Text = "✓ Current image 4";
                lblImage4.Visible = true;
            }
        }

        // ==================== SAVE ====================
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            bool isEdit = !string.IsNullOrEmpty(hfEquipmentID.Value);

            // Safe parse
            int years = 0, months = 0;
            int.TryParse(txtUsageYears.Text.Trim(), out years);
            int.TryParse(txtUsageMonths.Text.Trim(), out months);

            try
            {
                // Old image paths (edit mode ma j) — naya upload nahi hoy to retain karva
                string oldMain = null, oldImg2 = null, oldImg3 = null, oldImg4 = null;

                if (isEdit)
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand c = new SqlCommand(
                            "SELECT MainImage, Image2, Image3, Image4 FROM Equipment " +
                            "WHERE EquipmentID=@ID AND FarmerID=@FID", con);
                        c.Parameters.AddWithValue("@ID", hfEquipmentID.Value);
                        c.Parameters.AddWithValue("@FID", farmerID);
                        con.Open();
                        SqlDataReader r = c.ExecuteReader();
                        if (r.Read())
                        {
                            oldMain = r["MainImage"] == DBNull.Value ? null : r["MainImage"].ToString();
                            oldImg2 = r["Image2"] == DBNull.Value ? null : r["Image2"].ToString();
                            oldImg3 = r["Image3"] == DBNull.Value ? null : r["Image3"].ToString();
                            oldImg4 = r["Image4"] == DBNull.Value ? null : r["Image4"].ToString();
                        }
                    }
                }

                // Upload new images (jo hoy to)
                string newMain = UploadImage(fuMainImage, "main");
                string newImg2 = UploadImage(fuImage2, "img2");
                string newImg3 = UploadImage(fuImage3, "img3");
                string newImg4 = UploadImage(fuImage4, "img4");

                string finalMain = newMain ?? oldMain;
                string finalImg2 = newImg2 ?? oldImg2;
                string finalImg3 = newImg3 ?? oldImg3;
                string finalImg4 = newImg4 ?? oldImg4;

                // Insert / Update DB
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string q = isEdit
                        ? @"UPDATE Equipment SET 
                                EquipmentName=@Name, CategoryID=@CatID, MakeModel=@Make,
                                UsageYears=@Years, UsageMonths=@Months, AskingPrice=@Price,
                                ConditionNotes=@Cond, MobileNumber=@Mobile, PostalCode=@Postal,
                                FarmLocation=@Location,
                                MainImage=@MainImg, Image2=@Img2, Image3=@Img3, Image4=@Img4
                            WHERE EquipmentID=@ID AND FarmerID=@FID"
                        : @"INSERT INTO Equipment 
                                (EquipmentName, CategoryID, MakeModel, UsageYears, UsageMonths, AskingPrice,
                                 ConditionNotes, MobileNumber, PostalCode, FarmLocation, 
                                 MainImage, Image2, Image3, Image4, FarmerID, Status)
                            VALUES (@Name, @CatID, @Make, @Years, @Months, @Price,
                                    @Cond, @Mobile, @Postal, @Location,
                                    @MainImg, @Img2, @Img3, @Img4, @FID, 'Available')";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Name", txtEquipmentName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CatID", ddlCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@Make", txtMakeModel.Text.Trim());
                    cmd.Parameters.AddWithValue("@Years", years);
                    cmd.Parameters.AddWithValue("@Months", months);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Cond", (object)txtCondition.Text.Trim() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@Postal", string.IsNullOrEmpty(txtPostal.Text.Trim()) ? (object)DBNull.Value : txtPostal.Text.Trim());
                    cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
                    cmd.Parameters.AddWithValue("@MainImg", (object)finalMain ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img2", (object)finalImg2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img3", (object)finalImg3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img4", (object)finalImg4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FID", farmerID);
                    if (isEdit) cmd.Parameters.AddWithValue("@ID", hfEquipmentID.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                string action = isEdit ? "updated" : "added";
                ClientScript.RegisterStartupScript(this.GetType(), "ok",
                    $"alert('Equipment {action} successfully!');window.location='EquipmentManagement.aspx';", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "err",
                    $"alert('Error: {ex.Message.Replace("'", "")}');", true);
            }
        }

        // ==================== IMAGE UPLOAD HELPER ====================
        private string UploadImage(FileUpload fu, string prefix)
        {
            if (fu == null || !fu.HasFile) return null;

            try
            {
                // 1. Validate extension
                string ext = Path.GetExtension(fu.FileName).ToLower();
                string[] allowed = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                if (Array.IndexOf(allowed, ext) < 0)
                    throw new Exception("Only JPG, PNG, GIF, WEBP allowed");

                // 2. Validate size (max 2MB)
                if (fu.PostedFile.ContentLength > 2 * 1024 * 1024)
                    throw new Exception("Image size must be less than 2 MB");

                // 3. Generate unique filename
                string fileName = prefix + "_" + Guid.NewGuid().ToString("N").Substring(0, 12) + ext;

                // 4. Save path
                string folderPath = Server.MapPath("~/images/Equipment/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);
                fu.SaveAs(fullPath);

                // 5. Return relative URL for DB storage
                return "~/images/Equipment/" + fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("Image upload failed (" + prefix + "): " + ex.Message);
            }
        }
    }
}
