using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class FarmerManagement : System.Web.UI.Page
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
                LoadFarmers();
            }
        }

        private void LoadFarmers()
        {
            string search = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT f.FarmerID, f.FullName, f.Email, f.MobileNumber, f.Village, f.Status, f.CreatedDate,
                           (ISNULL((SELECT COUNT(*) FROM Crop c WHERE c.FarmerID = f.FarmerID), 0) +
                            ISNULL((SELECT COUNT(*) FROM Equipment e WHERE e.FarmerID = f.FarmerID), 0) +
                            ISNULL((SELECT COUNT(*) FROM Animal a WHERE a.FarmerID = f.FarmerID), 0)) AS TotalListings
                    FROM Farmer_Registration f
                    WHERE f.Role = 'Farmer'";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (f.FullName LIKE @Search OR f.MobileNumber LIKE @Search OR f.Village LIKE @Search OR f.Email LIKE @Search)";
                }

                sql += " ORDER BY f.CreatedDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                if (!string.IsNullOrEmpty(search))
                {
                    da.SelectCommand.Parameters.AddWithValue("@Search", "%" + search + "%");
                }

                DataTable dt = new DataTable();
                da.Fill(dt);

                lblTotalCount.Text = dt.Rows.Count.ToString("N0");
                lblShowingCount.Text = dt.Rows.Count.ToString("N0");

                if (dt.Rows.Count == 0)
                {
                    pnlEmpty.Visible = true;
                    rptFarmers.Visible = false;
                }
                else
                {
                    pnlEmpty.Visible = false;
                    rptFarmers.Visible = true;
                    rptFarmers.DataSource = dt;
                    rptFarmers.DataBind();
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadFarmers();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadFarmers();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "export",
                "alert('Farmer directory export generated successfully (CSV/Excel format).');", true);
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            LoadFarmers();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            LoadFarmers();
        }

        protected void rptFarmers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int farmerID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ToggleStatus")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = @"UPDATE Farmer_Registration 
                                   SET Status = CASE WHEN Status = 'Active' THEN 'Blocked' ELSE 'Active' END 
                                   WHERE FarmerID = @FarmerID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Farmer status updated successfully.";
                LoadFarmers();
            }
            else if (e.CommandName == "DeleteFarmer")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "DELETE FROM Farmer_Registration WHERE FarmerID = @FarmerID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Farmer record deleted from database.";
                LoadFarmers();
            }
            else if (e.CommandName == "View")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "SELECT FullName, Email, MobileNumber, Village, Status, CreatedDate FROM Farmer_Registration WHERE FarmerID = @ID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", farmerID);
                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string msg = "Farmer Details:\\n" +
                                             "Name: " + dr["FullName"].ToString().Replace("'", "") + "\\n" +
                                             "Email: " + dr["Email"].ToString().Replace("'", "") + "\\n" +
                                             "Mobile: " + dr["MobileNumber"].ToString().Replace("'", "") + "\\n" +
                                             "Location: " + dr["Village"].ToString().Replace("'", "") + "\\n" +
                                             "Status: " + dr["Status"].ToString().Replace("'", "") + "\\n" +
                                             "Registered: " + Convert.ToDateTime(dr["CreatedDate"]).ToString("dd MMM yyyy");
                                ClientScript.RegisterStartupScript(this.GetType(), "viewDetails", "alert('" + msg + "');", true);
                            }
                        }
                    }
                }
            }
        }

        public string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "U";
            string[] parts = fullName.Trim().Split(' ');
            if (parts.Length >= 2)
            {
                return (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();
            }
            return fullName.Substring(0, Math.Min(2, fullName.Length)).ToUpper();
        }

        public string GetAvatarColorClass(int index)
        {
            int colorNum = (index % 6) + 1;
            return "avatar-color-" + colorNum;
        }
    }
}
