<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="AnimalManagement.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.AnimalManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="page-header">
        <div>
            <h1>Animal Management</h1>
            <p>Manage and track your livestock listings.</p>
        </div>
        <div>
            <a href="AnimalType.aspx" class="btn-green"
               style="background:#fff;color:#2e7d32;border:1px solid #2e7d32;margin-right:8px;">
                ⚙ Manage Types
            </a>
            <a href="AnimalForm.aspx" class="btn-green">➕ Add New Animal</a>
        </div>
    </div>

    <div class="card">
        <!-- Search -->
        <div class="toolbar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input"
                placeholder="🔍 Search..." AutoPostBack="true"
                OnTextChanged="txtSearch_TextChanged" />
            <asp:DropDownList ID="ddlFilterType" runat="server"
                CssClass="filter-input" AutoPostBack="true"
                OnSelectedIndexChanged="ddlFilterType_SelectedIndexChanged">
                <asp:ListItem Text="All Types" Value="0" />
            </asp:DropDownList>
        </div>

        <!-- Grid -->
        <asp:GridView ID="gvAnimals" runat="server"
            AutoGenerateColumns="false"
            CssClass="grid-table"
            GridLines="None"
            OnRowCommand="gvAnimals_RowCommand"
            EmptyDataText="No animals listed yet. Click 'Add New Animal' to start!">

            <Columns>
                <asp:TemplateField HeaderText="Animal Name">
                    <ItemTemplate>
                        <div class="animal-name-cell">
                            <%# Eval("MainImage") == DBNull.Value || Eval("MainImage").ToString() == "" 
                                ? "<div class='animal-thumb'>🐄</div>"
                                : "<img src='" + ResolveUrl(Eval("MainImage").ToString()) + "' class='animal-thumb' alt='animal' />" %>
                            <div>
                                <div class="name-line"><%# Eval("AnimalName") %></div>
                                <div style="font-size:12px;color:#888;">
                                    <%# Eval("Status").ToString() == "Active" 
                                        ? "<span class='status-dot'></span>Active" 
                                        : "<span class='status-dot pending'></span>" + Eval("Status") %>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Price (₹)">
                    <ItemTemplate>
                        <span class="price-text">₹ <%# Eval("ExpectedPrice", "{0:N0}") %></span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Seller Name">
                    <ItemTemplate>
                        <%# Eval("SellerName") %>
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
                            CommandName="EditAnimal"
                            CommandArgument='<%# Eval("AnimalID") %>'>
                            ✏️
                        </asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnDelete" runat="server"
                            CssClass="btn-sm btn-delete"
                            CommandName="DeleteAnimal"
                            CommandArgument='<%# Eval("AnimalID") %>'
                            OnClientClick="return confirm('Delete this animal?');">
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
