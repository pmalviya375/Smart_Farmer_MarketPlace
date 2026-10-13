using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class Dashboard : System.Web.UI.Page
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
                string name = Session["FarmerName"].ToString();
                lblName.Text = name;
                lblToday.Text = DateTime.Now.ToString("dddd, dd MMM yyyy");

                int hour = DateTime.Now.Hour;
                if (hour < 12)
                    lblGreeting.Text = "Good morning";
                else if (hour < 17)
                    lblGreeting.Text = "Good afternoon";
                else
                    lblGreeting.Text = "Good evening";

                LoadStats();
                LoadRecentActivity();
                LoadRecentListings();
            }
        }

        private void LoadStats()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                int cropCount = GetCount(con,
                    "SELECT COUNT(*) FROM Crop WHERE FarmerID=@ID", farmerID);

                int equipCount = GetCount(con,
                    "SELECT COUNT(*) FROM Equipment WHERE FarmerID=@ID", farmerID);

                int animalCount = GetCount(con,
                    "SELECT COUNT(*) FROM Animal WHERE FarmerID=@ID", farmerID);

                int pendingIncoming = GetCount(con,
                    "SELECT COUNT(*) FROM Orders WHERE SellerID=@ID AND OrderStatus='Pending'", farmerID);

                int acceptedOrders = GetCount(con,
                    @"SELECT COUNT(*) FROM Orders 
                      WHERE (SellerID=@ID OR BuyerID=@ID) AND OrderStatus='Accepted'", farmerID);

                int myOrders = GetCount(con,
                    "SELECT COUNT(*) FROM Orders WHERE BuyerID=@ID", farmerID);

                lblCropCount.Text = cropCount.ToString();
                lblEquipCount.Text = equipCount.ToString();
                lblAnimalCount.Text = animalCount.ToString();
                lblPendingIncoming.Text = pendingIncoming.ToString();
                lblAccepted.Text = acceptedOrders.ToString();
                lblMyOrders.Text = myOrders.ToString();

                int totalListings = cropCount + equipCount + animalCount;
                lblTotalListings.Text = totalListings.ToString();
                lblPendingHero.Text = pendingIncoming.ToString();
            }
        }

        private int GetCount(SqlConnection con, string sql, int farmerID)
        {
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@ID", farmerID);
                object result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        private void LoadRecentActivity()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string q = @"
                    SELECT TOP 6
                        o.OrderID, o.ItemName, o.ItemType, o.OrderStatus, o.OrderDate,
                        o.BuyerID, o.SellerID,
                        ISNULL(b.FullName, 'Buyer') AS BuyerName,
                        ISNULL(s.FullName, 'Seller') AS SellerName
                    FROM Orders o
                    LEFT JOIN Farmer_Registration b ON o.BuyerID = b.FarmerID
                    LEFT JOIN Farmer_Registration s ON o.SellerID = s.FarmerID
                    WHERE o.SellerID = @ID OR o.BuyerID = @ID
                    ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(q, con);
                da.SelectCommand.Parameters.AddWithValue("@ID", farmerID);

                DataTable src = new DataTable();
                da.Fill(src);

                if (src.Rows.Count == 0)
                {
                    pnlNoActivity.Visible = true;
                    rptActivity.Visible = false;
                    return;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("Title");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("ItemType");
                dt.Columns.Add("ItemIconClass");
                dt.Columns.Add("TimeAgo");
                dt.Columns.Add("PartnerDetail");
                dt.Columns.Add("DirectionBadge");
                dt.Columns.Add("DirectionCss");
                dt.Columns.Add("Status");
                dt.Columns.Add("StatusCss");

                foreach (DataRow row in src.Rows)
                {
                    int buyerID = Convert.ToInt32(row["BuyerID"]);
                    int sellerID = Convert.ToInt32(row["SellerID"]);
                    string itemName = row["ItemName"].ToString();
                    string itemType = row["ItemType"].ToString();
                    string status = row["OrderStatus"] == DBNull.Value
                        ? "Pending"
                        : row["OrderStatus"].ToString();
                    DateTime orderDate = Convert.ToDateTime(row["OrderDate"]);

                    string iconClass;
                    switch (itemType.ToLower())
                    {
                        case "crop": iconClass = "fa-solid fa-seedling"; break;
                        case "equipment": iconClass = "fa-solid fa-tractor"; break;
                        case "animal": iconClass = "fa-solid fa-paw"; break;
                        default: iconClass = "fa-solid fa-box"; break;
                    }

                    string title;
                    string partnerDetail;
                    string directionBadge;
                    string directionCss;

                    if (sellerID == farmerID)
                    {
                        directionBadge = "Incoming";
                        directionCss = "incoming";
                        partnerDetail = "Buyer: " + row["BuyerName"];
                        title = status == "Pending"
                            ? "New buy request for " + itemName + " from " + row["BuyerName"]
                            : status == "Accepted"
                                ? "You accepted order for " + itemName
                                : "Order cancelled for " + itemName;
                    }
                    else
                    {
                        directionBadge = "Outgoing";
                        directionCss = "outgoing";
                        partnerDetail = "Seller: " + row["SellerName"];
                        title = status == "Pending"
                            ? "You requested to buy " + itemName + " from " + row["SellerName"]
                            : status == "Accepted"
                                ? "Order accepted for " + itemName
                                : "Order cancelled for " + itemName;
                    }

                    dt.Rows.Add(title, itemName, itemType, iconClass, GetTimeAgo(orderDate),
                        partnerDetail, directionBadge, directionCss, status, status.ToLower());
                }

                pnlNoActivity.Visible = false;
                rptActivity.Visible = true;
                rptActivity.DataSource = dt;
                rptActivity.DataBind();
            }
        }

        private void LoadRecentListings()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string q = @"
                        SELECT TOP 5 * FROM (
                            SELECT 'Crop' AS ItemType, CropID AS ItemID, CropName AS ItemName,
                                   CAST(Quantity AS VARCHAR(50)) + ' kg' AS DisplayValue,
                                   MainImage, ISNULL(Status, 'Available') AS Status, DateAdded,
                                   'CropForm.aspx?id=' + CAST(CropID AS VARCHAR(10)) AS EditUrl
                            FROM Crop WHERE FarmerID = @ID

                            UNION ALL

                            SELECT 'Equipment' AS ItemType, EquipmentID AS ItemID, EquipmentName AS ItemName,
                                   '₹ ' + CAST(CAST(AskingPrice AS DECIMAL(10,0)) AS VARCHAR(50)) AS DisplayValue,
                                   MainImage, ISNULL(Status, 'Available') AS Status, DateAdded,
                                   'EquipmentForm.aspx?id=' + CAST(EquipmentID AS VARCHAR(10)) AS EditUrl
                            FROM Equipment WHERE FarmerID = @ID

                            UNION ALL

                            SELECT 'Animal' AS ItemType, AnimalID AS ItemID, AnimalName AS ItemName,
                                   '₹ ' + CAST(CAST(ExpectedPrice AS DECIMAL(10,0)) AS VARCHAR(50)) AS DisplayValue,
                                   MainImage, ISNULL(Status, 'Available') AS Status, DateAdded,
                                   'AnimalForm.aspx?id=' + CAST(AnimalID AS VARCHAR(10)) AS EditUrl
                            FROM Animal WHERE FarmerID = @ID
                        ) AS Combined
                        ORDER BY DateAdded DESC";

                    SqlDataAdapter da = new SqlDataAdapter(q, con);
                    da.SelectCommand.Parameters.AddWithValue("@ID", farmerID);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        pnlNoListings.Visible = true;
                        rptRecentListings.Visible = false;
                        return;
                    }

                    dt.Columns.Add("TypeBadgeClass");
                    dt.Columns.Add("TypeIcon");

                    foreach (DataRow r in dt.Rows)
                    {
                        string t = r["ItemType"].ToString();
                        if (t == "Crop")
                        {
                            r["TypeBadgeClass"] = "type-crop";
                            r["TypeIcon"] = "fa-solid fa-seedling";
                        }
                        else if (t == "Equipment")
                        {
                            r["TypeBadgeClass"] = "type-equip";
                            r["TypeIcon"] = "fa-solid fa-tractor";
                        }
                        else
                        {
                            r["TypeBadgeClass"] = "type-animal";
                            r["TypeIcon"] = "fa-solid fa-paw";
                        }
                    }

                    pnlNoListings.Visible = false;
                    rptRecentListings.Visible = true;
                    rptRecentListings.DataSource = dt;
                    rptRecentListings.DataBind();
                }
            }
            catch (Exception)
            {
                pnlNoListings.Visible = true;
                rptRecentListings.Visible = false;
            }
        }

        public string RenderItemImage(object imgObj, string itemType)
        {
            string emoji = "📦";
            if (itemType == "Crop") emoji = "🌾";
            else if (itemType == "Equipment") emoji = "🚜";
            else if (itemType == "Animal") emoji = "🐄";

            if (imgObj == null || imgObj == DBNull.Value || string.IsNullOrEmpty(imgObj.ToString()))
            {
                return "<div class='listing-placeholder-icon'>" + emoji + "</div>";
            }

            string url = ResolveUrl(imgObj.ToString());
            return "<img src='" + url + "' alt='" + itemType + "' class='listing-thumb' onerror=\"this.outerHTML='<div class=\\'listing-placeholder-icon\\'>" + emoji + "</div>';\" />";
        }

        private string GetTimeAgo(DateTime date)
        {
            TimeSpan span = DateTime.Now - date;

            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return (int)span.TotalMinutes + "m ago";
            if (span.TotalHours < 24) return (int)span.TotalHours + "h ago";
            if (span.TotalDays < 7) return (int)span.TotalDays + "d ago";
            return date.ToString("dd MMM yyyy");
        }
    }
}
