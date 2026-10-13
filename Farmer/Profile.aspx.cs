using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class Profile : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadFarmerProfile();
            }
        }

        private void LoadFarmerProfile()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT Email, FullName, MobileNumber, Village FROM Farmer_Registration WHERE FarmerID = @FarmerID";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtEmail.Text = dr["Email"].ToString();
                            txtFullName.Text = dr["FullName"].ToString();
                            txtMobile.Text = dr["MobileNumber"] == DBNull.Value ? "" : dr["MobileNumber"].ToString();
                            txtVillage.Text = dr["Village"] == DBNull.Value ? "" : dr["Village"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            string fullName = txtFullName.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string village = txtVillage.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "UPDATE Farmer_Registration SET FullName = @FullName, MobileNumber = @Mobile, Village = @Village WHERE FarmerID = @FarmerID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Mobile", mobile);
                        cmd.Parameters.AddWithValue("@Village", village);
                        cmd.Parameters.AddWithValue("@FarmerID", farmerID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Update session variables
                Session["FarmerName"] = fullName;
                Session["FarmerMobile"] = mobile;
                Session["FarmerVillage"] = village;

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Farmer profile information updated successfully!";
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
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            string oldPass = txtOldPassword.Text.Trim();
            string newPass = txtNewPassword.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Verify old password
                    string checkSql = "SELECT COUNT(*) FROM Farmer_Registration WHERE FarmerID = @FarmerID AND Password = @OldPass";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@FarmerID", farmerID);
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
                    string updateSql = "UPDATE Farmer_Registration SET Password = @NewPass WHERE FarmerID = @FarmerID";
                    using (SqlCommand updateCmd = new SqlCommand(updateSql, con))
                    {
                        updateCmd.Parameters.AddWithValue("@NewPass", newPass);
                        updateCmd.Parameters.AddWithValue("@FarmerID", farmerID);
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
