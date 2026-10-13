using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class ContactUs : System.Web.UI.Page
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
                txtName.Text = Session["BuyerName"] != null ? Session["BuyerName"].ToString() : "";
                txtMobile.Text = Session["BuyerMobile"] != null ? Session["BuyerMobile"].ToString() : "";
                LoadMyQueries();
            }
        }

        private void LoadMyQueries()
        {
            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT QueryID, Subject, Message, QueryDate, AdminReply, ReplyDate, Status FROM ContactQueries WHERE SenderID = @SenderID AND SenderRole = 'Buyer' ORDER BY QueryDate DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@SenderID", buyerID);
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

            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            string name = Session["BuyerName"] != null ? Session["BuyerName"].ToString() : "Buyer";
            string email = Session["BuyerEmail"] != null ? Session["BuyerEmail"].ToString() : "";
            string mobile = Session["BuyerMobile"] != null ? Session["BuyerMobile"].ToString() : "";
            string subject = txtSubject.Text.Trim();
            string message = txtMessage.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = @"INSERT INTO ContactQueries 
                                  (SenderID, SenderName, SenderEmail, SenderMobile, SenderRole, Subject, Message, Status, QueryDate)
                                  VALUES (@SenderID, @SenderName, @SenderEmail, @SenderMobile, 'Buyer', @Subject, @Message, 'Pending', GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@SenderID", buyerID);
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
                lblAlert.Text = "Your inquiry has been submitted to AgriTech Admin! We will reply soon.";

                txtSubject.Text = "";
                txtMessage.Text = "";

                LoadMyQueries();
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Error sending inquiry: " + ex.Message;
            }
        }
    }
}
