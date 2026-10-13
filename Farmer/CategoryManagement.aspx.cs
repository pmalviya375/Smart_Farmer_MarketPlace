using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class CategoryManagement : System.Web.UI.Page
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
                LoadCategories();
            }
        }
        private void LoadCategories()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                // ✅ ADO.NET Disconnected Model
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT CategoryID, CategoryName, Description, CreatedDate " +
                    "FROM Crop_Category WHERE FarmerID=@FID ORDER BY CreatedDate DESC", con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);

                DataTable dt = new DataTable();
                da.Fill(dt);   // 🔑 Disconnected — DataSet/DataTable ma data load

                gvCategories.DataSource = dt;
                gvCategories.DataBind();
            }
        }

        // ========== CREATE / UPDATE ==========
        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            string catName = txtCategoryName.Text.Trim();
            string desc = txtDescription.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string q;
                    if (string.IsNullOrEmpty(hfCategoryID.Value))
                    {
                        // INSERT
                        q = "INSERT INTO Crop_Category (CategoryName, Description, FarmerID) " +
                            "VALUES (@Name, @Desc, @FID)";
                    }
                    else
                    {
                        // UPDATE
                        q = "UPDATE Crop_Category SET CategoryName=@Name, Description=@Desc " +
                            "WHERE CategoryID=@ID AND FarmerID=@FID";
                    }

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Name", catName);
                    cmd.Parameters.AddWithValue("@Desc", desc);
                    cmd.Parameters.AddWithValue("@FID", farmerID);
                    if (!string.IsNullOrEmpty(hfCategoryID.Value))
                        cmd.Parameters.AddWithValue("@ID", hfCategoryID.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                ResetForm();
                LoadCategories();
                ClientScript.RegisterStartupScript(this.GetType(), "ok",
                    "alert('Category saved successfully!');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "err",
                    $"alert('Error: {ex.Message.Replace("'", "")}');", true);
            }
        }

        // ========== DELETE + EDIT LINK ==========
        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int catID = Convert.ToInt32(e.CommandArgument);
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            if (e.CommandName == "EditCat")
            {
                // Load category into form
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT CategoryName, Description FROM Crop_Category " +
                        "WHERE CategoryID=@ID AND FarmerID=@FID", con);
                    cmd.Parameters.AddWithValue("@ID", catID);
                    cmd.Parameters.AddWithValue("@FID", farmerID);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfCategoryID.Value = catID.ToString();
                        txtCategoryName.Text = dr["CategoryName"].ToString();
                        txtDescription.Text = dr["Description"] == DBNull.Value ? "" : dr["Description"].ToString();

                        lblFormTitle.Text = "Edit Category";
                        btnSaveCategory.Text = "Update Category";
                        btnCancelEdit.Visible = true;
                    }
                }
            }
            else if (e.CommandName == "DeleteCat")
            {
                // Check karo ke aa category ma koi crop chhe?
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand check = new SqlCommand(
                        "SELECT COUNT(*) FROM Crop WHERE CategoryID=@ID", con);
                    check.Parameters.AddWithValue("@ID", catID);

                    con.Open();
                    int cropCount = (int)check.ExecuteScalar();

                    if (cropCount > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "warn",
                            "alert('Cannot delete! This category has crops assigned to it. Delete crops first.');", true);
                        return;
                    }

                    // Delete
                    SqlCommand del = new SqlCommand(
                        "DELETE FROM Crop_Category WHERE CategoryID=@ID AND FarmerID=@FID", con);
                    del.Parameters.AddWithValue("@ID", catID);
                    del.Parameters.AddWithValue("@FID", farmerID);
                    del.ExecuteNonQuery();
                }

                LoadCategories();
                ClientScript.RegisterStartupScript(this.GetType(), "del",
                    "alert('Category deleted!');", true);
            }
        }

        // ========== CANCEL EDIT ==========
        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            hfCategoryID.Value = "";
            txtCategoryName.Text = "";
            txtDescription.Text = "";
            lblFormTitle.Text = "Add New Category";
            btnSaveCategory.Text = "Save Category";
            btnCancelEdit.Visible = false;
        }
    }
}