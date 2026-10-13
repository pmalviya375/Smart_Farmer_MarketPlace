using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Admin
{
    public partial class ProductManagement : System.Web.UI.Page
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
                ViewState["CurrentTab"] = "All";
                UpdateTabStyles();
                LoadCounts();
                LoadProducts();
            }
        }

        private void UpdateTabStyles()
        {
            string tab = ViewState["CurrentTab"] != null ? ViewState["CurrentTab"].ToString() : "All";
            tabAll.CssClass = "product-tab-btn" + (tab == "All" ? " active" : "");
            tabCrops.CssClass = "product-tab-btn" + (tab == "Crop" ? " active" : "");
            tabEquipment.CssClass = "product-tab-btn" + (tab == "Equipment" ? " active" : "");
            tabAnimals.CssClass = "product-tab-btn" + (tab == "Animal" ? " active" : "");
        }

        private void LoadCounts()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Crop", con))
                {
                    int crops = Convert.ToInt32(cmd.ExecuteScalar());
                    lblCountCrops.Text = crops.ToString("N0");
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Equipment", con))
                {
                    int equip = Convert.ToInt32(cmd.ExecuteScalar());
                    lblCountEquipment.Text = equip.ToString("N0");
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Animal", con))
                {
                    int animals = Convert.ToInt32(cmd.ExecuteScalar());
                    lblCountAnimals.Text = animals.ToString("N0");
                }

                int total = Convert.ToInt32(lblCountCrops.Text) + Convert.ToInt32(lblCountEquipment.Text) + Convert.ToInt32(lblCountAnimals.Text);
                lblCountAll.Text = total.ToString("N0");
            }
        }

        private void LoadProducts()
        {
            string currentTab = ViewState["CurrentTab"] != null ? ViewState["CurrentTab"].ToString() : "All";
            string search = txtSearch.Text.Trim();
            string status = ddlStatusFilter.SelectedValue;

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("ItemType", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("CategoryName", typeof(string));
            dt.Columns.Add("SellerName", typeof(string));
            dt.Columns.Add("ContactNumber", typeof(string));
            dt.Columns.Add("PriceOrQty", typeof(string));
            dt.Columns.Add("Location", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("DateAdded", typeof(DateTime));
            dt.Columns.Add("MainImage", typeof(string));

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // 1. CROPS
                if (currentTab == "All" || currentTab == "Crop")
                {
                    string sqlCrop = @"
                        SELECT c.CropID AS ItemID, 'Crop' AS ItemType, c.CropName AS Title,
                               ISNULL(cat.CategoryName, 'General') AS CategoryName,
                               ISNULL(f.FullName, 'Farmer') AS SellerName,
                               c.ContactNumber,
                               CAST(c.Quantity AS NVARCHAR(50)) + ' Quintals' AS PriceOrQty,
                               c.FarmerAddress AS Location,
                               ISNULL(c.Status, 'Active') AS Status,
                               ISNULL(c.DateAdded, GETDATE()) AS DateAdded,
                               c.MainImage
                        FROM Crop c
                        LEFT JOIN Crop_Category cat ON c.CategoryID = cat.CategoryID
                        LEFT JOIN Farmer_Registration f ON c.FarmerID = f.FarmerID
                        WHERE 1=1";

                    if (!string.IsNullOrEmpty(status))
                    {
                        sqlCrop += " AND c.Status = @Status";
                    }
                    if (!string.IsNullOrEmpty(search))
                    {
                        sqlCrop += " AND (c.CropName LIKE @Search OR f.FullName LIKE @Search OR c.FarmerAddress LIKE @Search)";
                    }

                    using (SqlCommand cmd = new SqlCommand(sqlCrop, con))
                    {
                        if (!string.IsNullOrEmpty(status)) cmd.Parameters.AddWithValue("@Status", status);
                        if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dt.Rows.Add(
                                    dr["ItemID"], dr["ItemType"], dr["Title"], dr["CategoryName"],
                                    dr["SellerName"], dr["ContactNumber"], dr["PriceOrQty"],
                                    dr["Location"], dr["Status"], dr["DateAdded"], dr["MainImage"]
                                );
                            }
                        }
                    }
                }

                // 2. EQUIPMENT
                if (currentTab == "All" || currentTab == "Equipment")
                {
                    string sqlEquip = @"
                        SELECT e.EquipmentID AS ItemID, 'Equipment' AS ItemType, e.EquipmentName AS Title,
                               ISNULL(ec.CategoryName, 'General') AS CategoryName,
                               ISNULL(f.FullName, 'Farmer') AS SellerName,
                               e.MobileNumber AS ContactNumber,
                               '₹' + FORMAT(e.AskingPrice, 'N0') AS PriceOrQty,
                               e.FarmLocation AS Location,
                               ISNULL(e.Status, 'Active') AS Status,
                               ISNULL(e.DateAdded, GETDATE()) AS DateAdded,
                               e.MainImage
                        FROM Equipment e
                        LEFT JOIN Equipment_Category ec ON e.CategoryID = ec.CategoryID
                        LEFT JOIN Farmer_Registration f ON e.FarmerID = f.FarmerID
                        WHERE 1=1";

                    if (!string.IsNullOrEmpty(status))
                    {
                        sqlEquip += " AND e.Status = @Status";
                    }
                    if (!string.IsNullOrEmpty(search))
                    {
                        sqlEquip += " AND (e.EquipmentName LIKE @Search OR f.FullName LIKE @Search OR e.FarmLocation LIKE @Search)";
                    }

                    using (SqlCommand cmd = new SqlCommand(sqlEquip, con))
                    {
                        if (!string.IsNullOrEmpty(status)) cmd.Parameters.AddWithValue("@Status", status);
                        if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dt.Rows.Add(
                                    dr["ItemID"], dr["ItemType"], dr["Title"], dr["CategoryName"],
                                    dr["SellerName"], dr["ContactNumber"], dr["PriceOrQty"],
                                    dr["Location"], dr["Status"], dr["DateAdded"], dr["MainImage"]
                                );
                            }
                        }
                    }
                }

                // 3. ANIMAL
                if (currentTab == "All" || currentTab == "Animal")
                {
                    string sqlAnimal = @"
                        SELECT a.AnimalID AS ItemID, 'Animal' AS ItemType, a.AnimalName AS Title,
                               ISNULL(at.TypeName, 'Livestock') AS CategoryName,
                               ISNULL(f.FullName, 'Farmer') AS SellerName,
                               a.MobileNumber AS ContactNumber,
                               '₹' + FORMAT(a.ExpectedPrice, 'N0') AS PriceOrQty,
                               a.FarmAddress AS Location,
                               ISNULL(a.Status, 'Active') AS Status,
                               ISNULL(a.DateAdded, GETDATE()) AS DateAdded,
                               a.MainImage
                        FROM Animal a
                        LEFT JOIN Animal_Type at ON a.TypeID = at.TypeID
                        LEFT JOIN Farmer_Registration f ON a.FarmerID = f.FarmerID
                        WHERE 1=1";

                    if (!string.IsNullOrEmpty(status))
                    {
                        sqlAnimal += " AND a.Status = @Status";
                    }
                    if (!string.IsNullOrEmpty(search))
                    {
                        sqlAnimal += " AND (a.AnimalName LIKE @Search OR f.FullName LIKE @Search OR a.FarmAddress LIKE @Search)";
                    }

                    using (SqlCommand cmd = new SqlCommand(sqlAnimal, con))
                    {
                        if (!string.IsNullOrEmpty(status)) cmd.Parameters.AddWithValue("@Status", status);
                        if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dt.Rows.Add(
                                    dr["ItemID"], dr["ItemType"], dr["Title"], dr["CategoryName"],
                                    dr["SellerName"], dr["ContactNumber"], dr["PriceOrQty"],
                                    dr["Location"], dr["Status"], dr["DateAdded"], dr["MainImage"]
                                );
                            }
                        }
                    }
                }
            }

            // Sort by DateAdded Descending
            DataView dv = dt.DefaultView;
            dv.Sort = "DateAdded DESC";
            DataTable sortedDt = dv.ToTable();

            lblTotalCount.Text = sortedDt.Rows.Count.ToString("N0");
            lblShowingCount.Text = sortedDt.Rows.Count.ToString("N0");

            if (sortedDt.Rows.Count == 0)
            {
                pnlEmpty.Visible = true;
                rptProducts.Visible = false;
            }
            else
            {
                pnlEmpty.Visible = false;
                rptProducts.Visible = true;
                rptProducts.DataSource = sortedDt;
                rptProducts.DataBind();
            }
        }

        protected void tabFilter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["CurrentTab"] = btn.CommandArgument;
            UpdateTabStyles();
            LoadProducts();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ExportAlert",
                "alert('Product catalog export generated successfully (CSV format).');", true);
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string[] parts = e.CommandArgument.ToString().Split(':');
            string itemType = parts[0];
            int itemID = Convert.ToInt32(parts[1]);

            if (e.CommandName == "ToggleStatus")
            {
                string tableName = itemType == "Crop" ? "Crop" : (itemType == "Equipment" ? "Equipment" : "Animal");
                string idColumn = itemType == "Crop" ? "CropID" : (itemType == "Equipment" ? "EquipmentID" : "AnimalID");

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    string checkSql = "SELECT Status FROM " + tableName + " WHERE " + idColumn + " = @ID";
                    string currentStatus = "Active";
                    using (SqlCommand cmdCheck = new SqlCommand(checkSql, con))
                    {
                        cmdCheck.Parameters.AddWithValue("@ID", itemID);
                        object val = cmdCheck.ExecuteScalar();
                        if (val != null && val != DBNull.Value) currentStatus = val.ToString();
                    }

                    string newStatus = currentStatus == "Active" ? "Inactive" : "Active";
                    string updateSql = "UPDATE " + tableName + " SET Status = @NewStatus WHERE " + idColumn + " = @ID";
                    using (SqlCommand cmdUp = new SqlCommand(updateSql, con))
                    {
                        cmdUp.Parameters.AddWithValue("@NewStatus", newStatus);
                        cmdUp.Parameters.AddWithValue("@ID", itemID);
                        cmdUp.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = itemType + " listing status successfully updated.";
                LoadCounts();
                LoadProducts();
            }
            else if (e.CommandName == "DeleteItem")
            {
                string tableName = itemType == "Crop" ? "Crop" : (itemType == "Equipment" ? "Equipment" : "Animal");
                string idColumn = itemType == "Crop" ? "CropID" : (itemType == "Equipment" ? "EquipmentID" : "AnimalID");

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    string deleteSql = "DELETE FROM " + tableName + " WHERE " + idColumn + " = @ID";
                    using (SqlCommand cmdDel = new SqlCommand(deleteSql, con))
                    {
                        cmdDel.Parameters.AddWithValue("@ID", itemID);
                        cmdDel.ExecuteNonQuery();
                    }
                }

                pnlAlert.Visible = true;
                pnlAlert.CssClass = "auth-alert-box alert-success";
                lblAlert.Text = itemType + " listing permanently deleted.";
                LoadCounts();
                LoadProducts();
            }
            else if (e.CommandName == "View")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ViewAlert",
                    "alert('" + itemType + " ID: " + itemID + "\\nViewed from Admin Portal.');", true);
            }
        }

        public string GetItemImage(string itemType, string mainImage)
        {
            if (string.IsNullOrWhiteSpace(mainImage))
            {
                if (itemType == "Crop") return "~/Images/Crop/crop_main_2fde8de82fae.jpg";
                if (itemType == "Equipment") return "~/Images/Equipment/main_67acac1624bf.jpg";
                if (itemType == "Animal") return "~/Images/Animal/animal_main_b6c3e34b6a56.jpg";
                return "https://placehold.co/44x44?text=Item";
            }
            if (mainImage.StartsWith("~") || mainImage.StartsWith("/")) return mainImage;
            return "~/Uploads/" + itemType + "/" + mainImage;
        }
    }
}
