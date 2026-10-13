using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class Dashboard : System.Web.UI.Page
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
                LoadMetricCards();
                LoadRecentUsers();
                LoadRecentQueries();
            }
        }

        private void LoadMetricCards()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // 1. Total Farmers
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Farmer_Registration WHERE Role = 'Farmer'", con))
                {
                    int farmers = Convert.ToInt32(cmd.ExecuteScalar());
                    lblTotalFarmers.Text = farmers.ToString("N0");
                }

                // 2. Total Buyers
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Farmer_Registration WHERE Role = 'Buyer'", con))
                {
                    int buyers = Convert.ToInt32(cmd.ExecuteScalar());
                    lblTotalBuyers.Text = buyers.ToString("N0");
                }

                // 3. Pending Contact Queries
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ContactQueries WHERE Status = 'Pending'", con))
                {
                    int pending = Convert.ToInt32(cmd.ExecuteScalar());
                    lblPendingQueries.Text = pending.ToString("N0");
                }
            }
        }

        private void LoadRecentUsers()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT TOP 5 FullName, MobileNumber, Village, Role FROM Farmer_Registration ORDER BY CreatedDate DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptRecentUsers.DataSource = dt;
                rptRecentUsers.DataBind();
            }
        }

        private void LoadRecentQueries()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT TOP 5 SenderName, SenderRole, Subject, Message, Status FROM ContactQueries ORDER BY QueryDate DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlNoQueries.Visible = true;
                    rptRecentQueries.Visible = false;
                }
                else
                {
                    pnlNoQueries.Visible = false;
                    rptRecentQueries.Visible = true;
                    rptRecentQueries.DataSource = dt;
                    rptRecentQueries.DataBind();
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
