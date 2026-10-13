using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class Farmer : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["FarmerName"] != null)
                {
                    string name = Session["FarmerName"].ToString();
                    lblTopName.Text = name;
                    lblAvatar.Text = name.Length > 0
                        ? name.Substring(0, 1).ToUpper()
                        : "F";
                }
                else
                {
                    lblTopName.Text = "Guest";
                    lblAvatar.Text = "G";
                }
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}
