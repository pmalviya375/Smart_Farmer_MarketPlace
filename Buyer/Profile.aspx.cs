using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class Profile : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BuyerID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadBuyerProfile();
            }
        }

        private void LoadBuyerProfile()
        {
            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT Email, FullName, MobileNumber, Village FROM Farmer_Registration WHERE FarmerID = @BuyerID";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@BuyerID", buyerID);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtEmail.Text = dr["Email"].ToString();
                            txtFullName.Text = dr["FullName"].ToString();
                            txtMobile.Text = dr["MobileNumber"] == DBNull.Value ? "" : dr["MobileNumber"].ToString();
                            txtAddress.Text = dr["Village"] == DBNull.Value ? "" : dr["Village"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            string fullName = txtFullName.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string address = txtAddress.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "UPDATE Farmer_Registration SET FullName = @FullName, MobileNumber = @Mobile, Village = @Address WHERE FarmerID = @BuyerID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Mobile", mobile);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@BuyerID", buyerID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Update session variables
                Session["BuyerName"] = fullName;
                Session["BuyerMobile"] = mobile;
                Session["BuyerAddress"] = address;

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Buyer profile information updated successfully!";
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Error updating profile: " + ex.Message;
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            string oldPass = txtOldPassword.Text.Trim();
            string newPass = txtNewPassword.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Verify old password
                    string checkSql = "SELECT COUNT(*) FROM Farmer_Registration WHERE FarmerID = @BuyerID AND Password = @OldPass";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@BuyerID", buyerID);
                        checkCmd.Parameters.AddWithValue("@OldPass", oldPass);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count == 0)
                        {
                            pnlAlert.Visible = true;
                            pnlAlert.CssClass = "auth-alert-box alert-danger";
                            lblAlert.Text = "Incorrect current password. Please enter your correct current password.";
                            return;
                        }
                    }

                    // Update to new password
                    string updateSql = "UPDATE Farmer_Registration SET Password = @NewPass WHERE FarmerID = @BuyerID";
                    using (SqlCommand updateCmd = new SqlCommand(updateSql, con))
                    {
                        updateCmd.Parameters.AddWithValue("@NewPass", newPass);
                        updateCmd.Parameters.AddWithValue("@BuyerID", buyerID);
                        updateCmd.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Password changed successfully! Use your new password the next time you login.";

                txtOldPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Error changing password: " + ex.Message;
            }
        }
    }
}
