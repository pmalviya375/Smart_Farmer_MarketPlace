<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="EquipmentManagement.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.EquipmentManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="page-header">
        <div>
            <h1>Equipment Management</h1>
            <p>Manage and track your farm machinery and tools.</p>
        </div>
        <div>
            <a href="EquipmentCategory.aspx" class="btn-green"
               style="background:#fff;color:#2e7d32;border:1px solid #2e7d32;margin-right:8px;">
                ⚙ Manage Categories
            </a>
            <a href="EquipmentForm.aspx" class="btn-green">➕ Add New Equipment</a>
        </div>
    </div>

    <div class="card">
        <!-- Search -->
        <div class="toolbar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input"
                placeholder="🔍 Search equipment by name or ID..." AutoPostBack="true"
                OnTextChanged="txtSearch_TextChanged" />
            <asp:DropDownList ID="ddlFilterCategory" runat="server"
                CssClass="filter-input" AutoPostBack="true"
                OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged">
                <asp:ListItem Text="All Categories" Value="0" />
            </asp:DropDownList>
        </div>

        <!-- Grid -->
        <asp:GridView ID="gvEquipment" runat="server"
            AutoGenerateColumns="false"
            CssClass="grid-table"
            GridLines="None"
            OnRowCommand="gvEquipment_RowCommand"
            EmptyDataText="No equipment listed yet. Click 'Add New Equipment' to start!">

            <Columns>
               <asp:TemplateField HeaderText="Equipment Details">
    <ItemTemplate>
        <div class="equip-name-cell">
            <%# Eval("MainImage") == DBNull.Value || Eval("MainImage").ToString() == "" 
                ? "<div class='equip-thumb' style='background:#e8f5e9;'>🚜</div>"
                : "<img src='" + ResolveUrl(Eval("MainImage").ToString()) + "' class='equip-thumb' alt='equip' />" %>
            <div>
                <div style="font-weight:600;"><%# Eval("EquipmentName") %></div>
                <div class="equip-id-sub">ID: EQ-<%# Eval("EquipmentID").ToString().PadLeft(5, '0') %></div>
            </div>
        </div>
    </ItemTemplate>
</asp:TemplateField>

                <asp:TemplateField HeaderText="Usage Duration">
                    <ItemTemplate>
                        <%# GetDuration(Eval("UsageYears"), Eval("UsageMonths")) %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Price (₹)">
                    <ItemTemplate>
                        <span class="price-text">₹ <%# Eval("AskingPrice", "{0:N0}") %></span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Date Added">
                    <ItemTemplate>
                        <%# Eval("DateAdded", "{0:dd MMM yyyy}") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="130">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server"
                            CssClass="btn-sm btn-edit"
                            CommandName="EditEquip"
                            CommandArgument='<%# Eval("EquipmentID") %>'>
                            ✏️
                        </asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnDelete" runat="server"
                            CssClass="btn-sm btn-delete"
                            CommandName="DeleteEquip"
                            CommandArgument='<%# Eval("EquipmentID") %>'
                            OnClientClick="return confirm('Delete this equipment?');">
                            🗑
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <div class="pagination">
            <span>Showing <asp:Label ID="lblCount" runat="server" Text="0" /> entries</span>
        </div>
    </div>
</asp:Content>
