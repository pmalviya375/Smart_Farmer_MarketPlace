using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class ContactManagement : System.Web.UI.Page
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
                ViewState["Tab"] = "All";
                UpdateTabStyles();
                LoadCounts();
                LoadQueries();
            }
        }

        private void UpdateTabStyles()
        {
            string tab = ViewState["Tab"] == null ? "All" : ViewState["Tab"].ToString();
            tabAll.CssClass = "order-tab" + (tab == "All" ? " active" : "");
            tabPending.CssClass = "order-tab" + (tab == "Pending" ? " active" : "");
            tabReplied.CssClass = "order-tab" + (tab == "Replied" ? " active" : "");
        }

        private void LoadCounts()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT 
                        COUNT(*) AS TotalAll,
                        SUM(CASE WHEN Status = 'Pending' THEN 1 ELSE 0 END) AS TotalPending,
                        SUM(CASE WHEN Status = 'Replied' THEN 1 ELSE 0 END) AS TotalReplied
                    FROM ContactQueries";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblAllCount.Text = dr["TotalAll"] != DBNull.Value ? dr["TotalAll"].ToString() : "0";
                            lblPendingCount.Text = dr["TotalPending"] != DBNull.Value ? dr["TotalPending"].ToString() : "0";
                            lblRepliedCount.Text = dr["TotalReplied"] != DBNull.Value ? dr["TotalReplied"].ToString() : "0";
                        }
                    }
                }
            }
        }

        private void LoadQueries()
        {
            string tab = ViewState["Tab"] == null ? "All" : ViewState["Tab"].ToString();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT QueryID, SenderName, SenderEmail, SenderMobile, SenderRole, Subject, Message, QueryDate, AdminReply, ReplyDate, Status FROM ContactQueries";
                if (tab != "All")
                {
                    sql += " WHERE Status = @Status";
                }
                sql += " ORDER BY QueryDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                if (tab != "All")
                {
                    da.SelectCommand.Parameters.AddWithValue("@Status", tab);
                }

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlEmpty.Visible = true;
                    rptQueries.Visible = false;
                }
                else
                {
                    pnlEmpty.Visible = false;
                    rptQueries.Visible = true;
                    rptQueries.DataSource = dt;
                    rptQueries.DataBind();
                }
            }
        }

        protected void Tab_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["Tab"] = btn.CommandArgument;
            UpdateTabStyles();
            LoadQueries();
        }

        protected void rptQueries_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SendReply")
            {
                int queryID = Convert.ToInt32(e.CommandArgument);
                TextBox txtReply = (TextBox)e.Item.FindControl("txtReplyMessage");
                string replyText = txtReply != null ? txtReply.Text.Trim() : "";

                if (string.IsNullOrEmpty(replyText))
                {
                    pnlAlert.Visible = true;
                    pnlAlert.CssClass = "auth-alert-box alert-danger";
                    lblAlert.Text = "Please enter a reply message before submitting.";
                    return;
                }

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = @"UPDATE ContactQueries 
                                   SET AdminReply = @Reply, ReplyDate = GETDATE(), Status = 'Replied' 
                                   WHERE QueryID = @QueryID";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Reply", replyText);
                        cmd.Parameters.AddWithValue("@QueryID", queryID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = "Reply sent successfully to user!";

                LoadCounts();
                LoadQueries();
            }
        }
    }
}
