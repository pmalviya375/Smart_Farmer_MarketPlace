using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] != null)
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = @"SELECT AdminID, FullName, Email 
                                   FROM Admin 
                                   WHERE Email = @Email AND Password = @Password";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                int adminID = Convert.ToInt32(dr["AdminID"]);
                                string fullName = dr["FullName"].ToString();

                                // State Management via Session
                                Session["AdminID"] = adminID;
                                Session["AdminName"] = fullName;
                                Session["AdminEmail"] = dr["Email"].ToString();
                                Session["Role"] = "Admin";

                                Response.Redirect("Dashboard.aspx");
                            }
                            else
                            {
                                pnlAlert.Visible = true;
                                pnlAlert.CssClass = "auth-alert-box alert-danger";
                                lblAlert.Text = "Invalid Admin Credentials. Please use admin@gmail.com / admin@123.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Error during login: " + ex.Message;
            }
        }
    }
}
