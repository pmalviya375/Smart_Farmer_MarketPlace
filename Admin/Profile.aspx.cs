using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class Profile : System.Web.UI.Page
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
                LoadAdminProfile();
            }
        }

        private void LoadAdminProfile()
        {
            int adminID = Convert.ToInt32(Session["AdminID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT Email, FullName, MobileNumber FROM Admin WHERE AdminID = @AdminID";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminID);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtEmail.Text = dr["Email"].ToString();
                            txtFullName.Text = dr["FullName"].ToString();
                            txtMobile.Text = dr["MobileNumber"] == DBNull.Value ? "" : dr["MobileNumber"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            int adminID = Convert.ToInt32(Session["AdminID"]);
            string fullName = txtFullName.Text.Trim();
            string mobile = txtMobile.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "UPDATE Admin SET FullName = @FullName, MobileNumber = @Mobile WHERE AdminID = @AdminID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Mobile", mobile);
                        cmd.Parameters.AddWithValue("@AdminID", adminID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                Session["AdminName"] = fullName;

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Admin profile information updated successfully!";
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
            int adminID = Convert.ToInt32(Session["AdminID"]);
            string oldPass = txtOldPassword.Text.Trim();
            string newPass = txtNewPassword.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Verify old password
                    string checkSql = "SELECT COUNT(*) FROM Admin WHERE AdminID = @AdminID AND Password = @OldPass";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@AdminID", adminID);
                        checkCmd.Parameters.AddWithValue("@OldPass", oldPass);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count == 0)
                        {
                            pnlAlert.Visible = true;
                            pnlAlert.CssClass = "auth-alert-box alert-danger";
                            lblAlert.Text = "Incorrect current password. Please try again.";
                            return;
                        }
                    }

                    // Update to new password
                    string updateSql = "UPDATE Admin SET Password = @NewPass WHERE AdminID = @AdminID";
                    using (SqlCommand updateCmd = new SqlCommand(updateSql, con))
                    {
                        updateCmd.Parameters.AddWithValue("@NewPass", newPass);
                        updateCmd.Parameters.AddWithValue("@AdminID", adminID);
                        updateCmd.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Password changed successfully! Keep your new credentials safe.";

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
