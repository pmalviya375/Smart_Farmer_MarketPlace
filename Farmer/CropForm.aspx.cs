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
    public partial class CropForm : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;
        private const string IMG_FOLDER = "~/images/Crop/";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] == null) { Response.Redirect("Login.aspx"); return; }

            if (!IsPostBack)
            {
                LoadCategoryDropdown();

                if (Request.QueryString["id"] != null)
                {
                    hfCropID.Value = Request.QueryString["id"];
                    lblPageTitle.Text = "Edit Crop";
                    btnSave.Text = "💾 Update Crop";
                    LoadCropData(Convert.ToInt32(hfCropID.Value));
                    rfvMainImg.Enabled = false;   // Edit mode — main image optional
                }
                else
                {
                    rfvMainImg.Enabled = true;    // Add mode — required
                }
            }
        }
        private void LoadCategoryDropdown()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT CategoryID, CategoryName FROM Crop_Category " +
                    "WHERE FarmerID=@FID ORDER BY CategoryName", con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
            }
        }

        private void LoadCropData(int cropID)
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT CropName, CategoryID, Quantity, ContactNumber, FarmerAddress,
                             MainImage, Image2, Image3, Image4
                      FROM Crop WHERE CropID=@ID AND FarmerID=@FID", con);
                cmd.Parameters.AddWithValue("@ID", cropID);
                cmd.Parameters.AddWithValue("@FID", farmerID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtCropName.Text = dr["CropName"].ToString();
                    ddlCategory.SelectedValue = dr["CategoryID"].ToString();
                    txtQuantity.Text = dr["Quantity"].ToString();
                    txtContact.Text = dr["ContactNumber"].ToString();
                    txtAddress.Text = dr["FarmerAddress"].ToString();

                    ShowExistingImages(dr);
                }
            }
        }

        private void ShowExistingImages(SqlDataReader dr)
        {
            if (dr["MainImage"] != DBNull.Value && !string.IsNullOrEmpty(dr["MainImage"].ToString()))
            {
                imgMain.ImageUrl = ResolveUrl(dr["MainImage"].ToString());
                imgMain.CssClass = "image-preview show";
                lblMainImage.Text = "✓ Current main image";
                lblMainImage.Visible = true;
            }
            if (dr["Image2"] != DBNull.Value && !string.IsNullOrEmpty(dr["Image2"].ToString()))
            {
                img2.ImageUrl = ResolveUrl(dr["Image2"].ToString());
                img2.CssClass = "image-preview show";
                lblImage2.Text = "✓ Current image 2";
                lblImage2.Visible = true;
            }
            if (dr["Image3"] != DBNull.Value && !string.IsNullOrEmpty(dr["Image3"].ToString()))
            {
                img3.ImageUrl = ResolveUrl(dr["Image3"].ToString());
                img3.CssClass = "image-preview show";
                lblImage3.Text = "✓ Current image 3";
                lblImage3.Visible = true;
            }
            if (dr["Image4"] != DBNull.Value && !string.IsNullOrEmpty(dr["Image4"].ToString()))
            {
                img4.ImageUrl = ResolveUrl(dr["Image4"].ToString());
                img4.CssClass = "image-preview show";
                lblImage4.Text = "✓ Current image 4";
                lblImage4.Visible = true;
            }
        }

        // ==================== SAVE (INSERT / UPDATE) ====================
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            bool isEdit = !string.IsNullOrEmpty(hfCropID.Value);

            try
            {
                // Old image paths (edit mode)
                string oldMain = null, oldImg2 = null, oldImg3 = null, oldImg4 = null;

                if (isEdit)
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand c = new SqlCommand(
                            "SELECT MainImage, Image2, Image3, Image4 FROM Crop " +
                            "WHERE CropID=@ID AND FarmerID=@FID", con);
                        c.Parameters.AddWithValue("@ID", hfCropID.Value);
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

                // Upload new images
                string newMain = UploadImage(fuMainImage, "crop_main");
                string newImg2 = UploadImage(fuImage2, "crop_img2");
                string newImg3 = UploadImage(fuImage3, "crop_img3");
                string newImg4 = UploadImage(fuImage4, "crop_img4");

                string finalMain = newMain ?? oldMain;
                string finalImg2 = newImg2 ?? oldImg2;
                string finalImg3 = newImg3 ?? oldImg3;
                string finalImg4 = newImg4 ?? oldImg4;

                // Delete replaced old files
                if (newMain != null) DeleteOldImage(oldMain);
                if (newImg2 != null) DeleteOldImage(oldImg2);
                if (newImg3 != null) DeleteOldImage(oldImg3);
                if (newImg4 != null) DeleteOldImage(oldImg4);

                // Save to DB
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string q = isEdit
                        ? @"UPDATE Crop SET 
                                CropName=@Name, CategoryID=@CatID, Quantity=@Qty,
                                ContactNumber=@Contact, FarmerAddress=@Addr,
                                MainImage=@MainImg, Image2=@Img2, Image3=@Img3, Image4=@Img4
                            WHERE CropID=@ID AND FarmerID=@FID"
                        : @"INSERT INTO Crop 
                                (CropName, CategoryID, Quantity, ContactNumber, FarmerAddress,
                                 MainImage, Image2, Image3, Image4, FarmerID, Status)
                            VALUES (@Name, @CatID, @Qty, @Contact, @Addr,
                                    @MainImg, @Img2, @Img3, @Img4, @FID, 'Pending')";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Name", txtCropName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CatID", ddlCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@Qty", Convert.ToDecimal(txtQuantity.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                    cmd.Parameters.AddWithValue("@Addr", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@MainImg", (object)finalMain ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img2", (object)finalImg2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img3", (object)finalImg3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img4", (object)finalImg4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FID", farmerID);
                    if (isEdit) cmd.Parameters.AddWithValue("@ID", hfCropID.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                string action = isEdit ? "updated" : "added";
                ClientScript.RegisterStartupScript(this.GetType(), "ok",
                    $"alert('Crop {action} successfully!');window.location='CropManagement.aspx';", true);
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

                // 3. Unique filename
                string fileName = prefix + "_" + Guid.NewGuid().ToString("N").Substring(0, 12) + ext;

                // 4. Save path
                string folderPath = Server.MapPath(IMG_FOLDER);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);
                fu.SaveAs(fullPath);

                // 5. Return virtual path for DB
                return IMG_FOLDER + fileName;   // e.g. ~/images/Crop/crop_main_abc123.jpg
            }
            catch (Exception ex)
            {
                throw new Exception("Image upload failed (" + prefix + "): " + ex.Message);
            }
        }

        private void DeleteOldImage(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath)) return;
                string full = Server.MapPath(relativePath);
                if (File.Exists(full)) File.Delete(full);
            }
            catch { /* silently ignore */ }
        }
}
}