using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class Register : System.Web.UI.Page
    {
        // ADO.NET Connection String from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BuyerID"] != null)
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string address = txtAddress.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                // ADO.NET Connected Architecture
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Check if email already exists
                    string checkSql = "SELECT COUNT(*) FROM Farmer_Registration WHERE Email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            pnlAlert.Visible = true;
                            pnlAlert.CssClass = "auth-alert-box alert-danger";
                            lblAlert.Text = "This email is already registered. Please login or use a different email.";
                            return;
                        }
                    }

                    // ADO.NET Insert Operation using Parameterized SQL
                    string insertSql = @"INSERT INTO Farmer_Registration 
                                        (FullName, Email, MobileNumber, Village, Password, Role, CreatedDate)
                                        VALUES (@FullName, @Email, @Mobile, @Address, @Password, 'Buyer', GETDATE())";

                    using (SqlCommand insertCmd = new SqlCommand(insertSql, con))
                    {
                        insertCmd.Parameters.AddWithValue("@FullName", fullName);
                        insertCmd.Parameters.AddWithValue("@Email", email);
                        insertCmd.Parameters.AddWithValue("@Mobile", mobile);
                        insertCmd.Parameters.AddWithValue("@Address", address);
                        insertCmd.Parameters.AddWithValue("@Password", password);

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Response.Redirect("Login.aspx?registered=true");
                        }
                        else
                        {
                            pnlAlert.Visible = true;
                            pnlAlert.CssClass = "auth-alert-box alert-danger";
                            lblAlert.Text = "Unable to create account. Please try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Registration Error: " + ex.Message;
            }
        }
    }
}
