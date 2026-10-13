using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class Register : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] != null)
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    // Check duplicate email
                    string checkQ = "SELECT COUNT(*) FROM Farmer_Registration WHERE Email=@Email";
                    SqlCommand checkCmd = new SqlCommand(checkQ, con);
                    checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    con.Open();
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        Response.Write("<script>alert('Email already registered!');</script>");
                        return;
                    }

                    string q = @"INSERT INTO Farmer_Registration 
                        (FullName, Email, MobileNumber, Village, Password, Role) 
                        VALUES (@FullName, @Email, @Mobile, @Village, @Password, 'Farmer')";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@Village", txtVillage.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    cmd.ExecuteNonQuery();
                }

                Response.Write("<script>alert('Registration Successful! Please login.');" +
                               "window.location='Login.aspx';</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message.Replace("'", "") + "');</script>");
            }
        }
    }
}