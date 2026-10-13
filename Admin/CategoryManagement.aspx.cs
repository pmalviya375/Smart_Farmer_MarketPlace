using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class CategoryManagement : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ViewState["CurrentCatType"] = "Crop";
                UpdateTabStyles();
                LoadCounts();
                LoadCategories();
            }
        }

        private void UpdateTabStyles()
        {
            string catType = ViewState["CurrentCatType"] != null ? ViewState["CurrentCatType"].ToString() : "Crop";
            tabCrops.CssClass = "cat-tab-btn" + (catType == "Crop" ? " active" : "");
            tabEquipment.CssClass = "cat-tab-btn" + (catType == "Equipment" ? " active" : "");
            tabAnimals.CssClass = "cat-tab-btn" + (catType == "Animal" ? " active" : "");

            if (catType == "Crop")
                lblAddHeading.Text = "Crop Category";
            else if (catType == "Equipment")
                lblAddHeading.Text = "Equipment Category";
            else
                lblAddHeading.Text = "Livestock Type";
        }

        private void LoadCounts()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Crop_Category", con))
                {
                    lblCountCropCats.Text = Convert.ToInt32(cmd.ExecuteScalar()).ToString();
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Equipment_Category", con))
                {
                    lblCountEquipCats.Text = Convert.ToInt32(cmd.ExecuteScalar()).ToString();
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Animal_Type", con))
                {
                    lblCountAnimalTypes.Text = Convert.ToInt32(cmd.ExecuteScalar()).ToString();
                }
            }
        }

        private void LoadCategories()
        {
            string catType = ViewState["CurrentCatType"] != null ? ViewState["CurrentCatType"].ToString() : "Crop";

            string sql = "";
            if (catType == "Crop")
            {
                sql = @"
                    SELECT c.CategoryID AS ID, c.CategoryName AS Name, ISNULL(c.Description, '-') AS Description,
                           ISNULL(c.CreatedDate, GETDATE()) AS CreatedDate,
                           (SELECT COUNT(*) FROM Crop cr WHERE cr.CategoryID = c.CategoryID) AS ItemCount
                    FROM Crop_Category c
                    ORDER BY c.CategoryName ASC";
            }
            else if (catType == "Equipment")
            {
                sql = @"
                    SELECT ec.CategoryID AS ID, ec.CategoryName AS Name, ISNULL(ec.Description, '-') AS Description,
                           ISNULL(ec.CreatedDate, GETDATE()) AS CreatedDate,
                           (SELECT COUNT(*) FROM Equipment eq WHERE eq.CategoryID = ec.CategoryID) AS ItemCount
                    FROM Equipment_Category ec
                    ORDER BY ec.CategoryName ASC";
            }
            else
            {
                sql = @"
                    SELECT at.TypeID AS ID, at.TypeName AS Name, ISNULL(at.Description, '-') AS Description,
                           ISNULL(at.CreatedDate, GETDATE()) AS CreatedDate,
                           (SELECT COUNT(*) FROM Animal an WHERE an.TypeID = at.TypeID) AS ItemCount
                    FROM Animal_Type at
                    ORDER BY at.TypeName ASC";
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                lblTotalCount.Text = dt.Rows.Count.ToString();

                if (dt.Rows.Count == 0)
                {
                    pnlEmpty.Visible = true;
                    rptCategories.Visible = false;
                }
                else
                {
                    pnlEmpty.Visible = false;
                    rptCategories.Visible = true;
                    rptCategories.DataSource = dt;
                    rptCategories.DataBind();
                }
            }
        }

        protected void tabFilter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["CurrentCatType"] = btn.CommandArgument;
            UpdateTabStyles();
            LoadCategories();
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            string catType = ViewState["CurrentCatType"] != null ? ViewState["CurrentCatType"].ToString() : "Crop";
            string name = txtCategoryName.Text.Trim();
            string desc = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(name)) return;

            string insertSql = "";
            if (catType == "Crop")
            {
                insertSql = "INSERT INTO Crop_Category (CategoryName, Description, FarmerID, CreatedDate) VALUES (@Name, @Desc, 1, GETDATE())";
            }
            else if (catType == "Equipment")
            {
                insertSql = "INSERT INTO Equipment_Category (CategoryName, Description, FarmerID, CreatedDate) VALUES (@Name, @Desc, 1, GETDATE())";
            }
            else
            {
                insertSql = "INSERT INTO Animal_Type (TypeName, Description, FarmerID, CreatedDate) VALUES (@Name, @Desc, 1, GETDATE())";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(insertSql, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Desc", string.IsNullOrEmpty(desc) ? (object)DBNull.Value : desc);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                txtCategoryName.Text = "";
                txtDescription.Text = "";
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "New " + (catType == "Animal" ? "Livestock Type" : "Category") + " added successfully.";
                LoadCounts();
                LoadCategories();
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Error adding category: " + ex.Message;
            }
        }

        protected void rptCategories_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCat")
            {
                string catType = ViewState["CurrentCatType"] != null ? ViewState["CurrentCatType"].ToString() : "Crop";
                int id = Convert.ToInt32(e.CommandArgument);

                string delSql = "";
                if (catType == "Crop")
                    delSql = "DELETE FROM Crop_Category WHERE CategoryID = @ID";
                else if (catType == "Equipment")
                    delSql = "DELETE FROM Equipment_Category WHERE CategoryID = @ID";
                else
                    delSql = "DELETE FROM Animal_Type WHERE TypeID = @ID";

                try
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand(delSql, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    pnlAlert.Visible = true;
                    pnlAlert.CssClass = "auth-alert-box alert-success";
                    lblAlert.Text = "Category successfully deleted.";
                    LoadCounts();
                    LoadCategories();
                }
                catch (Exception ex)
                {
                    pnlAlert.Visible = true;
                    pnlAlert.CssClass = "auth-alert-box alert-danger";
                    lblAlert.Text = "Unable to delete category: it may have linked items. (" + ex.Message + ")";
                }
            }
        }
    }
}
