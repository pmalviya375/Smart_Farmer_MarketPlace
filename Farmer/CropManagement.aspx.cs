using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;             


namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class CropManagement : System.Web.UI.Page
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
                LoadCategoriesDropdown();
                LoadCrops();
            }

        }
        // ========== READ ==========
        private void LoadCategoriesDropdown()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT CategoryID, CategoryName FROM Crop_Category WHERE FarmerID=@FID", con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlFilterCategory.DataSource = dt;
                ddlFilterCategory.DataTextField = "CategoryName";
                ddlFilterCategory.DataValueField = "CategoryID";
                ddlFilterCategory.DataBind();
                ddlFilterCategory.Items.Insert(0, new ListItem("All Categories", "0"));
            }
        }

        // READ with search + filter
        private void LoadCrops()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            string search = txtSearch.Text.Trim();
            int catFilter = Convert.ToInt32(ddlFilterCategory.SelectedValue);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string q = @"SELECT c.CropID, c.CropName, c.Quantity, c.DateAdded, c.Status,
                    c.MainImage, cat.CategoryName
             FROM Crop c
             INNER JOIN Crop_Category cat ON c.CategoryID = cat.CategoryID
             WHERE c.FarmerID = @FID";

                if (!string.IsNullOrEmpty(search))
                    q += " AND c.CropName LIKE @Search";

                if (catFilter > 0)
                    q += " AND c.CategoryID = @CatID";

                q += " ORDER BY c.DateAdded DESC";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);
                if (!string.IsNullOrEmpty(search))
                    da.SelectCommand.Parameters.AddWithValue("@Search", "%" + search + "%");
                if (catFilter > 0)
                    da.SelectCommand.Parameters.AddWithValue("@CatID", catFilter);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCrops.DataSource = dt;
                gvCrops.DataBind();
                lblCount.Text = dt.Rows.Count.ToString();
            }
        }

        // Search / Filter events (AutoPostBack!)
        protected void txtSearch_TextChanged(object sender, EventArgs e) => LoadCrops();
        protected void ddlFilterCategory_SelectedIndexChanged(object sender, EventArgs e) => LoadCrops();

        // Edit / Delete
        protected void gvCrops_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int cropID = Convert.ToInt32(e.CommandArgument);
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            if (e.CommandName == "EditCrop")
            {
                Response.Redirect("CropForm.aspx?id=" + cropID);
            }
            else if (e.CommandName == "DeleteCrop")
            {
                try
                {
                    // STEP 1: Fetch image paths PEHLA
                    string mainImg = null, img2 = null, img3 = null, img4 = null;

                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand fetchCmd = new SqlCommand(
                            "SELECT MainImage, Image2, Image3, Image4 FROM Crop " +
                            "WHERE CropID=@ID AND FarmerID=@FID", con);
                        fetchCmd.Parameters.AddWithValue("@ID", cropID);
                        fetchCmd.Parameters.AddWithValue("@FID", farmerID);

                        con.Open();
                        SqlDataReader dr = fetchCmd.ExecuteReader();
                        if (dr.Read())
                        {
                            mainImg = dr["MainImage"] == DBNull.Value ? null : dr["MainImage"].ToString();
                            img2 = dr["Image2"] == DBNull.Value ? null : dr["Image2"].ToString();
                            img3 = dr["Image3"] == DBNull.Value ? null : dr["Image3"].ToString();
                            img4 = dr["Image4"] == DBNull.Value ? null : dr["Image4"].ToString();
                        }
                        dr.Close();

                        // STEP 2: DB row delete
                        SqlCommand delCmd = new SqlCommand(
                            "DELETE FROM Crop WHERE CropID=@ID AND FarmerID=@FID", con);
                        delCmd.Parameters.AddWithValue("@ID", cropID);
                        delCmd.Parameters.AddWithValue("@FID", farmerID);
                        delCmd.ExecuteNonQuery();
                    }

                    // STEP 3: Physical images delete
                    DeleteImageFile(mainImg);
                    DeleteImageFile(img2);
                    DeleteImageFile(img3);
                    DeleteImageFile(img4);

                    LoadCrops();
                    ClientScript.RegisterStartupScript(this.GetType(), "del",
                        "alert('Crop deleted successfully!');", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "err",
                        $"alert('Error: {ex.Message.Replace("'", "")}');", true);
                }
            }

        }
        // 🗑️ Safe image delete helper
        private void DeleteImageFile(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath)) return;

                string fullPath = Server.MapPath(relativePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch
            {
                // Silent fail — file locked hoy shke, pan delete operation rokay nahi
            }
        }
    }
}
