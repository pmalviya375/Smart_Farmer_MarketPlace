using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class Login : System.Web.UI.Page
    {
        // ADO.NET Connection String from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // State Management: Check if Buyer is already logged in
            if (Session["BuyerID"] != null)
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["registered"] == "true")
                {
                    pnlAlert.Visible = true;
                    pnlAlert.CssClass = "auth-alert-box alert-success";
                    lblAlert.Text = "Registration successful! Please login with your credentials.";
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                // ADO.NET Connected Architecture: SqlConnection and SqlCommand
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    // Parameterized query to prevent SQL Injection
                    string q = @"SELECT FarmerID, FullName, Email, MobileNumber, Village, Role 
                                 FROM Farmer_Registration 
                                 WHERE Email = @Email AND Password = @Password AND (Role = 'Buyer' OR Role IS NULL)";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        // State Management using Session
                        Session["BuyerID"] = dr["FarmerID"].ToString();
                        Session["BuyerName"] = dr["FullName"].ToString();
                        Session["BuyerEmail"] = dr["Email"].ToString();
                        Session["BuyerMobile"] = dr["MobileNumber"] == DBNull.Value ? "" : dr["MobileNumber"].ToString();
                        Session["BuyerAddress"] = dr["Village"] == DBNull.Value ? "" : dr["Village"].ToString();
                        Session["Role"] = "Buyer";

                        Response.Redirect("Dashboard.aspx");
                    }
                    else
                    {
                        pnlAlert.Visible = true;
                        pnlAlert.CssClass = "auth-alert-box alert-danger";
                        lblAlert.Text = "Invalid Email or Password. Please try again.";
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
