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
    public partial class AnimalForm : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;
        private const string IMG_FOLDER = "~/images/Animal/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] == null) { Response.Redirect("Login.aspx"); return; }

            if (!IsPostBack)
            {
                LoadTypeDropdown();

                if (Request.QueryString["id"] != null)
                {
                    hfAnimalID.Value = Request.QueryString["id"];
                    lblPageTitle.Text = "Edit Animal";
                    btnSave.Text = "💾 Update Listing";
                    LoadAnimalData(Convert.ToInt32(hfAnimalID.Value));
                    rfvMainImg.Enabled = false;
                }
                else
                {
                    rfvMainImg.Enabled = true;
                }
            }
        }

        private void LoadTypeDropdown()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TypeID, TypeName FROM Animal_Type WHERE FarmerID=@FID ORDER BY TypeName", con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlType.DataSource = dt;
                ddlType.DataTextField = "TypeName";
                ddlType.DataValueField = "TypeID";
                ddlType.DataBind();
                ddlType.Items.Insert(0, new ListItem("Select species", ""));
            }
        }

        private void LoadAnimalData(int animalID)
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT AnimalName, TypeID, Age, AgeUnit, Breed, ExpectedPrice,
                             HealthStatus, MobileNumber, FarmAddress,
                             MainImage, Image2, Image3, Image4
                      FROM Animal WHERE AnimalID=@ID AND FarmerID=@FID", con);
                cmd.Parameters.AddWithValue("@ID", animalID);
                cmd.Parameters.AddWithValue("@FID", farmerID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtAnimalName.Text = dr["AnimalName"].ToString();
                    ddlType.SelectedValue = dr["TypeID"].ToString();
                    txtAge.Text = dr["Age"].ToString();
                    ddlAgeUnit.SelectedValue = dr["AgeUnit"].ToString();
                    txtBreed.Text = dr["Breed"] == DBNull.Value ? "" : dr["Breed"].ToString();
                    txtPrice.Text = dr["ExpectedPrice"].ToString();
                    txtHealth.Text = dr["HealthStatus"] == DBNull.Value ? "" : dr["HealthStatus"].ToString();
                    txtMobile.Text = dr["MobileNumber"].ToString();
                    txtAddress.Text = dr["FarmAddress"].ToString();

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

        // ==================== SAVE ====================
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            bool isEdit = !string.IsNullOrEmpty(hfAnimalID.Value);

            int age = 0;
            int.TryParse(txtAge.Text.Trim(), out age);

            try
            {
                // Old image paths
                string oldMain = null, oldImg2 = null, oldImg3 = null, oldImg4 = null;
                if (isEdit)
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand c = new SqlCommand(
                            "SELECT MainImage, Image2, Image3, Image4 FROM Animal " +
                            "WHERE AnimalID=@ID AND FarmerID=@FID", con);
                        c.Parameters.AddWithValue("@ID", hfAnimalID.Value);
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
                string newMain = UploadImage(fuMainImage, "animal_main");
                string newImg2 = UploadImage(fuImage2, "animal_img2");
                string newImg3 = UploadImage(fuImage3, "animal_img3");
                string newImg4 = UploadImage(fuImage4, "animal_img4");

                string finalMain = newMain ?? oldMain;
                string finalImg2 = newImg2 ?? oldImg2;
                string finalImg3 = newImg3 ?? oldImg3;
                string finalImg4 = newImg4 ?? oldImg4;

                // Delete replaced old files
                if (newMain != null) DeleteImageFile(oldMain);
                if (newImg2 != null) DeleteImageFile(oldImg2);
                if (newImg3 != null) DeleteImageFile(oldImg3);
                if (newImg4 != null) DeleteImageFile(oldImg4);

                // Save
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string q = isEdit
                        ? @"UPDATE Animal SET 
                                AnimalName=@Name, TypeID=@TypeID, Age=@Age, AgeUnit=@AgeUnit,
                                Breed=@Breed, ExpectedPrice=@Price, HealthStatus=@Health,
                                MobileNumber=@Mobile, FarmAddress=@Addr,
                                MainImage=@MainImg, Image2=@Img2, Image3=@Img3, Image4=@Img4
                            WHERE AnimalID=@ID AND FarmerID=@FID"
                        : @"INSERT INTO Animal 
                                (AnimalName, TypeID, Age, AgeUnit, Breed, ExpectedPrice, HealthStatus,
                                 MobileNumber, FarmAddress, MainImage, Image2, Image3, Image4,
                                 FarmerID, Status)
                            VALUES (@Name, @TypeID, @Age, @AgeUnit, @Breed, @Price, @Health,
                                    @Mobile, @Addr, @MainImg, @Img2, @Img3, @Img4,
                                    @FID, 'Pending')";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Name", txtAnimalName.Text.Trim());
                    cmd.Parameters.AddWithValue("@TypeID", ddlType.SelectedValue);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@AgeUnit", ddlAgeUnit.SelectedValue);
                    cmd.Parameters.AddWithValue("@Breed", (object)txtBreed.Text.Trim() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Health", (object)txtHealth.Text.Trim() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@Addr", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@MainImg", (object)finalMain ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img2", (object)finalImg2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img3", (object)finalImg3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Img4", (object)finalImg4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FID", farmerID);
                    if (isEdit) cmd.Parameters.AddWithValue("@ID", hfAnimalID.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                string action = isEdit ? "updated" : "added";
                ClientScript.RegisterStartupScript(this.GetType(), "ok",
                    $"alert('Animal {action} successfully!');window.location='AnimalManagement.aspx';", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "err",
                    $"alert('Error: {ex.Message.Replace("'", "")}');", true);
            }
        }

        // ==================== IMAGE HELPERS ====================
        private string UploadImage(FileUpload fu, string prefix)
        {
            if (fu == null || !fu.HasFile) return null;

            try
            {
                string ext = Path.GetExtension(fu.FileName).ToLower();
                string[] allowed = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                if (Array.IndexOf(allowed, ext) < 0)
                    throw new Exception("Only JPG, PNG, GIF, WEBP allowed");

                if (fu.PostedFile.ContentLength > 2 * 1024 * 1024)
                    throw new Exception("Image size must be less than 2 MB");

                string fileName = prefix + "_" + Guid.NewGuid().ToString("N").Substring(0, 12) + ext;

                string folderPath = Server.MapPath(IMG_FOLDER);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);
                fu.SaveAs(fullPath);

                return IMG_FOLDER + fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("Image upload failed (" + prefix + "): " + ex.Message);
            }
        }

        private void DeleteImageFile(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath)) return;
                string fullPath = Server.MapPath(relativePath);
                if (File.Exists(fullPath)) File.Delete(fullPath);
            }
            catch { }
        }
    }
}