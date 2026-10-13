using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class ContactUs : System.Web.UI.Page
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
                txtName.Text = Session["FarmerName"] != null ? Session["FarmerName"].ToString() : "";
                txtMobile.Text = Session["FarmerMobile"] != null ? Session["FarmerMobile"].ToString() : "";
                LoadMyQueries();
            }
        }

        private void LoadMyQueries()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT QueryID, Subject, Message, QueryDate, AdminReply, ReplyDate, Status FROM ContactQueries WHERE SenderID = @SenderID AND SenderRole = 'Farmer' ORDER BY QueryDate DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@SenderID", farmerID);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlNoQueries.Visible = true;
                    rptMyQueries.Visible = false;
                }
                else
                {
                    pnlNoQueries.Visible = false;
                    rptMyQueries.Visible = true;
                    rptMyQueries.DataSource = dt;
                    rptMyQueries.DataBind();
                }
            }
        }

        protected void btnSendQuery_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            string name = Session["FarmerName"].ToString();
            string email = Session["FarmerEmail"] != null ? Session["FarmerEmail"].ToString() : "";
            string mobile = Session["FarmerMobile"] != null ? Session["FarmerMobile"].ToString() : "";
            string subject = txtSubject.Text.Trim();
            string message = txtMessage.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = @"INSERT INTO ContactQueries 
                                  (SenderID, SenderName, SenderEmail, SenderMobile, SenderRole, Subject, Message, Status, QueryDate)
                                  VALUES (@SenderID, @SenderName, @SenderEmail, @SenderMobile, 'Farmer', @Subject, @Message, 'Pending', GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@SenderID", farmerID);
                        cmd.Parameters.AddWithValue("@SenderName", name);
                        cmd.Parameters.AddWithValue("@SenderEmail", email);
                        cmd.Parameters.AddWithValue("@SenderMobile", mobile);
                        cmd.Parameters.AddWithValue("@Subject", subject);
                        cmd.Parameters.AddWithValue("@Message", message);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Your message has been sent to the Admin! We will reply soon.";

                txtSubject.Text = "";
                txtMessage.Text = "";

                LoadMyQueries();
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Error sending message: " + ex.Message;
            }
        }
    }
}
