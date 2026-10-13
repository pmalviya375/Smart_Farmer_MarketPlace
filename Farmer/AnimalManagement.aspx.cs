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
    public partial class AnimalManagement : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] == null) { Response.Redirect("Login.aspx"); return; }

            if (!IsPostBack)
            {
                LoadTypesDropdown();
                LoadAnimals();
            }
        }

        private void LoadTypesDropdown()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TypeID, TypeName FROM Animal_Type WHERE FarmerID=@FID", con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlFilterType.DataSource = dt;
                ddlFilterType.DataTextField = "TypeName";
                ddlFilterType.DataValueField = "TypeID";
                ddlFilterType.DataBind();
                ddlFilterType.Items.Insert(0, new ListItem("All Types", "0"));
            }
        }

        private void LoadAnimals()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            string search = txtSearch.Text.Trim();
            int typeFilter = Convert.ToInt32(ddlFilterType.SelectedValue);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                // SellerName = Farmer's FullName via JOIN
                string q = @"SELECT a.AnimalID, a.AnimalName, a.ExpectedPrice, a.DateAdded,
                                    a.Status, a.MainImage, at.TypeName,
                                    f.FullName AS SellerName
                             FROM Animal a
                             INNER JOIN Animal_Type at ON a.TypeID = at.TypeID
                             INNER JOIN Farmer_Registration f ON a.FarmerID = f.FarmerID
                             WHERE a.FarmerID = @FID";

                if (!string.IsNullOrEmpty(search))
                    q += " AND a.AnimalName LIKE @Search";

                if (typeFilter > 0)
                    q += " AND a.TypeID = @TypeID";

                q += " ORDER BY a.DateAdded DESC";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);
                if (!string.IsNullOrEmpty(search))
                    da.SelectCommand.Parameters.AddWithValue("@Search", "%" + search + "%");
                if (typeFilter > 0)
                    da.SelectCommand.Parameters.AddWithValue("@TypeID", typeFilter);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAnimals.DataSource = dt;
                gvAnimals.DataBind();
                lblCount.Text = dt.Rows.Count.ToString();
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) => LoadAnimals();
        protected void ddlFilterType_SelectedIndexChanged(object sender, EventArgs e) => LoadAnimals();

        protected void gvAnimals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int animalID = Convert.ToInt32(e.CommandArgument);
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            if (e.CommandName == "EditAnimal")
            {
                Response.Redirect("AnimalForm.aspx?id=" + animalID);
            }
            else if (e.CommandName == "DeleteAnimal")
            {
                try
                {
                    // STEP 1: Fetch image paths
                    string mainImg = null, img2 = null, img3 = null, img4 = null;

                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand fetchCmd = new SqlCommand(
                            "SELECT MainImage, Image2, Image3, Image4 FROM Animal " +
                            "WHERE AnimalID=@ID AND FarmerID=@FID", con);
                        fetchCmd.Parameters.AddWithValue("@ID", animalID);
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

                        // STEP 2: DB delete
                        SqlCommand delCmd = new SqlCommand(
                            "DELETE FROM Animal WHERE AnimalID=@ID AND FarmerID=@FID", con);
                        delCmd.Parameters.AddWithValue("@ID", animalID);
                        delCmd.Parameters.AddWithValue("@FID", farmerID);
                        delCmd.ExecuteNonQuery();
                    }

                    // STEP 3: Images delete
                    DeleteImageFile(mainImg);
                    DeleteImageFile(img2);
                    DeleteImageFile(img3);
                    DeleteImageFile(img4);

                    LoadAnimals();
                    ClientScript.RegisterStartupScript(this.GetType(), "del",
                        "alert('Animal deleted successfully!');", true);
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
                if (File.Exists(fullPath)) File.Delete(fullPath);
            }
            catch { }
        }
    }
}