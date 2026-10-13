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
    public partial class Login : System.Web.UI.Page
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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string q = @"SELECT FarmerID, FullName, Role, MobileNumber FROM Farmer_Registration 
                                 WHERE Email=@Email AND Password=@Password";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        Session["FarmerID"] = dr["FarmerID"].ToString();
                        Session["FarmerName"] = dr["FullName"].ToString();
                        Session["Role"] = dr["Role"].ToString();
                        Session["FarmerMobile"] = dr["MobileNumber"] == DBNull.Value
                            ? ""
                            : dr["MobileNumber"].ToString();

                        Response.Redirect("Dashboard.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Invalid Email or Password');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message.Replace("'", "") + "');</script>");
            }
        }
    }
}