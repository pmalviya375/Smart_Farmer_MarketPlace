<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="CropManagement.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.CropManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1>Crop Inventory Management</h1>
            <p>Manage your current stock, track yields, and update details for the marketplace.</p>
        </div>
        <div>
            <a href="CategoryManagement.aspx" class="btn-green" 
               style="background:#fff;color:#2e7d32;border:1px solid #2e7d32;margin-right:8px;">
                ⚙ Manage Categories
            </a>
            <a href="CropForm.aspx" class="btn-green">➕ Add New Crop</a>
        </div>
    </div>

    <div class="card">
        <!-- Toolbar -->
        <div class="toolbar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input"
                placeholder="🔍 Search crops..." AutoPostBack="true"
                OnTextChanged="txtSearch_TextChanged" />
            <asp:DropDownList ID="ddlFilterCategory" runat="server"
                CssClass="filter-input" AutoPostBack="true"
                OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged">
                <asp:ListItem Text="All Categories" Value="0" />
            </asp:DropDownList>
        </div>

        <!-- Grid -->
        <asp:GridView ID="gvCrops" runat="server"
            AutoGenerateColumns="false"
            CssClass="grid-table"
            GridLines="None"
            OnRowCommand="gvCrops_RowCommand"
            EmptyDataText="No crops found. Click 'Add New Crop' to get started!">

            <Columns>
                <asp:TemplateField HeaderText="Crop Name">
    <ItemTemplate>
        <div class="crop-name-cell">
            <%# Eval("MainImage") == DBNull.Value || Eval("MainImage").ToString() == "" 
                ? "<div class='crop-thumb'>🌾</div>"
                : "<img src='" + ResolveUrl(Eval("MainImage").ToString()) + "' class='crop-thumb' />" %>
            <strong><%# Eval("CropName") %></strong>
        </div>
    </ItemTemplate>
</asp:TemplateField>

                <asp:TemplateField HeaderText="Category">
                    <ItemTemplate>
                        <%# Eval("CategoryName") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Quantity (Quintals)">
                    <ItemTemplate>
                        <%# Eval("Quantity", "{0:N2}") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Date Added">
                    <ItemTemplate>
                        <%# Eval("DateAdded", "{0:dd MMM, yyyy}") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='badge badge-<%# Eval("Status").ToString().ToLower() %>'>
                            <%# Eval("Status") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="130">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server"
                            CssClass="btn-sm btn-edit"
                            CommandName="EditCrop"
                            CommandArgument='<%# Eval("CropID") %>'>
                            ✏️
                        </asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnDelete" runat="server"
                            CssClass="btn-sm btn-delete"
                            CommandName="DeleteCrop"
                            CommandArgument='<%# Eval("CropID") %>'
                            OnClientClick="return confirm('Delete this crop?');">
                            🗑
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <div class="pagination">
            <span>
                Showing <asp:Label ID="lblCount" runat="server" Text="0" /> entries
            </span>
        </div>
    </div>

</asp:Content>
