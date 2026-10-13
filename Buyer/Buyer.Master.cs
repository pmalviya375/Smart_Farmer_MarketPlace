using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class Buyer : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string currentPage = Path.GetFileName(Request.Path).ToLower();

                // State Management using Session
                if (Session["BuyerID"] != null)
                {
                    string name = Session["BuyerName"] != null ? Session["BuyerName"].ToString() : "Buyer";
                    lblBuyerName.Text = name;
                    lblAvatar.Text = name.Length > 0 ? name.Substring(0, 1).ToUpper() : "B";
                }
                else
                {
                    lblBuyerName.Text = "Guest";
                    lblAvatar.Text = "G";

                    // Only redirect protected pages, NEVER redirect Login or Register (avoids ERR_TOO_MANY_REDIRECTS)
                    if (currentPage != "login.aspx" && currentPage != "register.aspx")
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Clear buyer session upon logout
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}
