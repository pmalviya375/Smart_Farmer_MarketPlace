using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Buyer
{
    public partial class OrderRequest : System.Web.UI.Page
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
                // Auto-fill Buyer details from Session
                txtFullName.Text = Session["BuyerName"] != null ? Session["BuyerName"].ToString() : "";
                txtPhone.Text = Session["BuyerMobile"] != null ? Session["BuyerMobile"].ToString() : "";
                txtAddress.Text = Session["BuyerAddress"] != null ? Session["BuyerAddress"].ToString() : "";

                string type = Request.QueryString["type"];
                string id = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(type))
                {
                    ListItem item = ddlItemType.Items.FindByValue(type);
                    if (item != null)
                    {
                        ddlItemType.ClearSelection();
                        item.Selected = true;
                    }
                }

                LoadItemsDropdown();

                if (!string.IsNullOrEmpty(id))
                {
                    ListItem targetItem = ddlItems.Items.FindByValue(id);
                    if (targetItem != null)
                    {
                        ddlItems.ClearSelection();
                        targetItem.Selected = true;
                        LoadItemDetails(Convert.ToInt32(id));
                    }
                }
                else if (ddlItems.Items.Count > 1)
                {
                    ddlItems.SelectedIndex = 1;
                    LoadItemDetails(Convert.ToInt32(ddlItems.SelectedValue));
                }
            }
        }

        private void LoadItemsDropdown()
        {
            string itemType = ddlItemType.SelectedValue;
            ddlItems.Items.Clear();
            ddlItems.Items.Add(new ListItem("-- Select Available Item --", ""));

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "";
                if (itemType == "Equipment")
                {
                    sql = "SELECT EquipmentID AS ID, EquipmentName + ' (Rs. ' + CAST(AskingPrice AS VARCHAR) + '/day)' AS DisplayName FROM Equipment WHERE ISNULL(Status,'') NOT IN ('Rented','Sold') ORDER BY EquipmentName";
                }
                else if (itemType == "Animal")
                {
                    sql = "SELECT AnimalID AS ID, AnimalName + ' (Rs. ' + CAST(ExpectedPrice AS VARCHAR) + ')' AS DisplayName FROM Animal WHERE ISNULL(Status,'') <> 'Sold' ORDER BY AnimalName";
                }
                else
                {
                    sql = "SELECT CropID AS ID, CropName + ' (Rs. ' + CAST(Quantity AS VARCHAR) + '/Qtl)' AS DisplayName FROM Crop WHERE ISNULL(Status,'') <> 'Sold' ORDER BY CropName";
                }

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ddlItems.Items.Add(new ListItem(dr["DisplayName"].ToString(), dr["ID"].ToString()));
                        }
                    }
                }
            }
        }

        private void LoadItemDetails(int itemID)
        {
            string itemType = ddlItemType.SelectedValue;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "";
                if (itemType == "Equipment")
                {
                    sql = @"SELECT e.EquipmentName AS ItemName, e.AskingPrice AS Price, e.MainImage,
                                   e.FarmerID AS SellerID, f.FullName AS SellerName, f.Village AS SellerVillage, f.MobileNumber AS SellerMobile
                            FROM Equipment e
                            INNER JOIN Farmer_Registration f ON e.FarmerID = f.FarmerID
                            WHERE e.EquipmentID = @ID";
                }
                else if (itemType == "Animal")
                {
                    sql = @"SELECT a.AnimalName AS ItemName, a.ExpectedPrice AS Price, a.MainImage,
                                   a.FarmerID AS SellerID, f.FullName AS SellerName, f.Village AS SellerVillage, f.MobileNumber AS SellerMobile
                            FROM Animal a
                            INNER JOIN Farmer_Registration f ON a.FarmerID = f.FarmerID
                            WHERE a.AnimalID = @ID";
                }
                else
                {
                    sql = @"SELECT c.CropName AS ItemName, c.Quantity AS Price, c.MainImage,
                                   c.FarmerID AS SellerID, f.FullName AS SellerName, f.Village AS SellerVillage, f.MobileNumber AS SellerMobile
                            FROM Crop c
                            INNER JOIN Farmer_Registration f ON c.FarmerID = f.FarmerID
                            WHERE c.CropID = @ID";
                }

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@ID", itemID);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string itemName = dr["ItemName"].ToString();
                            decimal price = Convert.ToDecimal(dr["Price"]);
                            string sellerName = dr["SellerName"].ToString();
                            string sellerVillage = dr["SellerVillage"].ToString();
                            string sellerMobile = dr["SellerMobile"].ToString();
                            int sellerID = Convert.ToInt32(dr["SellerID"]);
                            object img = dr["MainImage"];

                            lblSelectedItemName.Text = itemName;
                            lblItemTypeTag.Text = itemType + " Listing";
                            txtAskingPrice.Text = price.ToString("N0");
                            txtOfferPrice.Text = price.ToString("N0");

                            lblProducerName.Text = sellerName;
                            lblProducerVillage.Text = sellerVillage;
                            lblProducerMobile.Text = sellerMobile;

                            if (img != null && img != DBNull.Value && !string.IsNullOrEmpty(img.ToString()))
                            {
                                imgItemPreview.ImageUrl = ResolveUrl(img.ToString());
                                imgItemPreview.Style["display"] = "block";
                            }
                            else
                            {
                                imgItemPreview.Style["display"] = "none";
                            }

                            ViewState["CurrentSellerID"] = sellerID;
                            ViewState["CurrentItemName"] = itemName;
                        }
                    }
                }
            }
        }

        protected void ddlItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItemsDropdown();
            if (ddlItems.Items.Count > 1)
            {
                ddlItems.SelectedIndex = 1;
                LoadItemDetails(Convert.ToInt32(ddlItems.SelectedValue));
            }
            else
            {
                ClearDetails();
            }
        }

        protected void ddlItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlItems.SelectedValue))
            {
                LoadItemDetails(Convert.ToInt32(ddlItems.SelectedValue));
            }
            else
            {
                ClearDetails();
            }
        }

        private void ClearDetails()
        {
            lblSelectedItemName.Text = "Please select an item";
            lblItemTypeTag.Text = "";
            txtAskingPrice.Text = "";
            txtOfferPrice.Text = "";
            lblProducerName.Text = "-";
            lblProducerVillage.Text = "-";
            lblProducerMobile.Text = "-";
            imgItemPreview.Style["display"] = "none";
            ViewState["CurrentSellerID"] = null;
            ViewState["CurrentItemName"] = null;
        }

        protected void btnSubmitOffer_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (string.IsNullOrEmpty(ddlItems.SelectedValue) || ViewState["CurrentSellerID"] == null)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Please select a valid item to submit an order request.";
                return;
            }

            int buyerID = Convert.ToInt32(Session["BuyerID"]);
            int sellerID = Convert.ToInt32(ViewState["CurrentSellerID"]);
            string itemType = ddlItemType.SelectedValue;
            int itemID = Convert.ToInt32(ddlItems.SelectedValue);
            string itemName = ViewState["CurrentItemName"].ToString();
            decimal offerPrice;

            if (!decimal.TryParse(txtOfferPrice.Text.Trim(), out offerPrice))
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Please enter a valid numeric offer price.";
                return;
            }

            string contact = txtPhone.Text.Trim();
            string message = txtMessage.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Check for duplicate pending requests
                    string checkSql = @"SELECT COUNT(*) FROM Orders 
                                       WHERE BuyerID = @BuyerID AND ItemType = @ItemType AND ItemID = @ItemID AND OrderStatus = 'Pending'";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@BuyerID", buyerID);
                        checkCmd.Parameters.AddWithValue("@ItemType", itemType);
                        checkCmd.Parameters.AddWithValue("@ItemID", itemID);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            pnlAlert.Visible = true;
                            pnlAlert.CssClass = "auth-alert-box alert-danger";
                            lblAlert.Text = "You already have a pending order request for this item. Please track it in My Orders.";
                            return;
                        }
                    }

                    // ADO.NET Parameterized Insert
                    string insertSql = @"INSERT INTO Orders 
                                        (BuyerID, SellerID, ItemType, ItemID, ItemName, OfferedPrice, Message, ContactNumber, OrderStatus, OrderDate)
                                        VALUES (@BuyerID, @SellerID, @ItemType, @ItemID, @ItemName, @OfferedPrice, @Message, @ContactNumber, 'Pending', GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(insertSql, con))
                    {
                        cmd.Parameters.AddWithValue("@BuyerID", buyerID);
                        cmd.Parameters.AddWithValue("@SellerID", sellerID);
                        cmd.Parameters.AddWithValue("@ItemType", itemType);
                        cmd.Parameters.AddWithValue("@ItemID", itemID);
                        cmd.Parameters.AddWithValue("@ItemName", itemName);
                        cmd.Parameters.AddWithValue("@OfferedPrice", offerPrice);
                        cmd.Parameters.AddWithValue("@Message", string.IsNullOrEmpty(message) ? (object)DBNull.Value : message);
                        cmd.Parameters.AddWithValue("@ContactNumber", contact);

                        cmd.ExecuteNonQuery();
                    }
                }

                ClientScript.RegisterStartupScript(this.GetType(), "orderOk",
                    "alert('Order request submitted successfully to the farmer!'); window.location='MyOrders.aspx';", true);
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-danger";
                lblAlert.Text = "Error submitting order request: " + ex.Message;
            }
        }
    }
}
