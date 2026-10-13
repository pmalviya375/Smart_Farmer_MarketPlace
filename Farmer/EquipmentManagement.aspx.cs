using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class EquipmentManagement : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] == null) { Response.Redirect("Login.aspx"); return; }

            if (!IsPostBack)
            {
                LoadCategoriesDropdown();
                LoadEquipment();
            }
        }
        public string GetDuration(object yearsObj, object monthsObj)
        {
            int y = yearsObj == DBNull.Value ? 0 : Convert.ToInt32(yearsObj);
            int m = monthsObj == DBNull.Value ? 0 : Convert.ToInt32(monthsObj);

            if (y == 0 && m == 0) return "—";

            string result = "";
            if (y > 0) result += y + (y == 1 ? " Year" : " Years");
            if (m > 0)
            {
                if (result != "") result += ", ";
                result += m + (m == 1 ? " Month" : " Months");
            }
            return result;
        }

        private void LoadCategoriesDropdown()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT CategoryID, CategoryName FROM Equipment_Category WHERE FarmerID=@FID", con);
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

        private void LoadEquipment()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            string search = txtSearch.Text.Trim();
            int catFilter = Convert.ToInt32(ddlFilterCategory.SelectedValue);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string q = @"SELECT e.EquipmentID, e.EquipmentName, e.UsageYears, e.UsageMonths,
                    e.AskingPrice, e.DateAdded, e.MainImage, ec.CategoryName
             FROM Equipment e
             INNER JOIN Equipment_Category ec ON e.CategoryID = ec.CategoryID
             WHERE e.FarmerID = @FID";
                if (!string.IsNullOrEmpty(search))
                    q += " AND (e.EquipmentName LIKE @Search OR CAST(e.EquipmentID AS NVARCHAR) LIKE @Search)";

                if (catFilter > 0)
                    q += " AND e.CategoryID = @CatID";

                q += " ORDER BY e.DateAdded DESC";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);
                if (!string.IsNullOrEmpty(search))
                    da.SelectCommand.Parameters.AddWithValue("@Search", "%" + search + "%");
                if (catFilter > 0)
                    da.SelectCommand.Parameters.AddWithValue("@CatID", catFilter);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvEquipment.DataSource = dt;
                gvEquipment.DataBind();
                lblCount.Text = dt.Rows.Count.ToString();
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) => LoadEquipment();
        protected void ddlFilterCategory_SelectedIndexChanged(object sender, EventArgs e) => LoadEquipment();

        protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int equipID = Convert.ToInt32(e.CommandArgument);
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            if (e.CommandName == "EditEquip")
            {
                Response.Redirect("EquipmentForm.aspx?id=" + equipID);
            }
            else if (e.CommandName == "DeleteEquip")
            {
                try
                {
                    // STEP 1: Fetch image paths PEHLA
                    string mainImg = null, img2 = null, img3 = null, img4 = null;

                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand fetchCmd = new SqlCommand(
                            "SELECT MainImage, Image2, Image3, Image4 FROM Equipment " +
                            "WHERE EquipmentID=@ID AND FarmerID=@FID", con);
                        fetchCmd.Parameters.AddWithValue("@ID", equipID);
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
                            "DELETE FROM Equipment WHERE EquipmentID=@ID AND FarmerID=@FID", con);
                        delCmd.Parameters.AddWithValue("@ID", equipID);
                        delCmd.Parameters.AddWithValue("@FID", farmerID);
                        delCmd.ExecuteNonQuery();
                    }

                    // STEP 3: Physical images delete
                    DeleteImageFile(mainImg);
                    DeleteImageFile(img2);
                    DeleteImageFile(img3);
                    DeleteImageFile(img4);

                    LoadEquipment();
                    ClientScript.RegisterStartupScript(this.GetType(), "del",
                        "alert('Equipment deleted successfully!');", true);
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
            catch { }
        }

    }
}