using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string currentPage = Path.GetFileName(Request.Path).ToLower();

                // State Management using Session
                if (Session["AdminID"] != null)
                {
                    string name = Session["AdminName"] != null ? Session["AdminName"].ToString() : "Admin";
                    lblAdminName.Text = name;
                    lblAvatar.Text = name.Length > 0 ? name.Substring(0, 1).ToUpper() : "A";
                }
                else
                {
                    lblAdminName.Text = "Guest";
                    lblAvatar.Text = "A";

                    // Allow Login.aspx without redirecting, redirect all protected admin pages
                    if (currentPage != "login.aspx")
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Clear admin session upon logout
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}
