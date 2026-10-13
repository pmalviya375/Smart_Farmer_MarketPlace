using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smart_Farmer_MarketPlace.Farmer
{
    public partial class AnimalType : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["AgriTechConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FarmerName"] == null) { Response.Redirect("Login.aspx"); return; }
            if (!IsPostBack) LoadTypes();
        }

        private void LoadTypes()
        {
            int farmerID = Convert.ToInt32(Session["FarmerID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TypeID, TypeName, Description, CreatedDate " +
                    "FROM Animal_Type WHERE FarmerID=@FID ORDER BY CreatedDate DESC", con);
                da.SelectCommand.Parameters.AddWithValue("@FID", farmerID);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTypes.DataSource = dt;
                gvTypes.DataBind();
            }
        }

        protected void btnSaveType_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string q = string.IsNullOrEmpty(hfTypeID.Value)
                        ? "INSERT INTO Animal_Type (TypeName, Description, FarmerID) VALUES (@Name, @Desc, @FID)"
                        : "UPDATE Animal_Type SET TypeName=@Name, Description=@Desc WHERE TypeID=@ID AND FarmerID=@FID";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Name", txtTypeName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Desc", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@FID", farmerID);
                    if (!string.IsNullOrEmpty(hfTypeID.Value))
                        cmd.Parameters.AddWithValue("@ID", hfTypeID.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                ResetForm();
                LoadTypes();
                ClientScript.RegisterStartupScript(this.GetType(), "ok",
                    "alert('Type saved successfully!');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "err",
                    $"alert('Error: {ex.Message.Replace("'", "")}');", true);
            }
        }

        protected void gvTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int typeID = Convert.ToInt32(e.CommandArgument);
            int farmerID = Convert.ToInt32(Session["FarmerID"]);

            if (e.CommandName == "EditType")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT TypeName, Description FROM Animal_Type WHERE TypeID=@ID AND FarmerID=@FID", con);
                    cmd.Parameters.AddWithValue("@ID", typeID);
                    cmd.Parameters.AddWithValue("@FID", farmerID);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfTypeID.Value = typeID.ToString();
                        txtTypeName.Text = dr["TypeName"].ToString();
                        txtDescription.Text = dr["Description"] == DBNull.Value ? "" : dr["Description"].ToString();
                        lblFormTitle.Text = "Edit Animal Type";
                        btnSaveType.Text = "Update Type";
                        btnCancelEdit.Visible = true;
                    }
                }
            }
            else if (e.CommandName == "DeleteType")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Animal WHERE TypeID=@ID", con);
                    check.Parameters.AddWithValue("@ID", typeID);
                    con.Open();
                    int count = (int)check.ExecuteScalar();

                    if (count > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "warn",
                            "alert('Cannot delete! Animals exist in this type. Delete them first.');", true);
                        return;
                    }

                    SqlCommand del = new SqlCommand("DELETE FROM Animal_Type WHERE TypeID=@ID AND FarmerID=@FID", con);
                    del.Parameters.AddWithValue("@ID", typeID);
                    del.Parameters.AddWithValue("@FID", farmerID);
                    del.ExecuteNonQuery();
                }
                LoadTypes();
                ClientScript.RegisterStartupScript(this.GetType(), "del", "alert('Type deleted!');", true);
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e) => ResetForm();

        private void ResetForm()
        {
            hfTypeID.Value = "";
            txtTypeName.Text = "";
            txtDescription.Text = "";
            lblFormTitle.Text = "Add New Animal Type";
            btnSaveType.Text = "Save Type";
            btnCancelEdit.Visible = false;
        }
    }
}