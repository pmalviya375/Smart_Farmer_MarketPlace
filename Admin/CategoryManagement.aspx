<%@ Page Title="Category Management - AgriTech Admin" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CategoryManagement.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Admin.CategoryManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .cat-tab-group {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
        }
        .cat-tab-btn {
            padding: 8px 18px;
            border-radius: 8px;
            background: #fff;
            border: 1px solid #e2e8f0;
            color: #475569;
            font-weight: 600;
            font-size: 13px;
            text-decoration: none;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 6px;
        }
        .cat-tab-btn.active {
            background: #1b6d34;
            color: #fff;
            border-color: #1b6d34;
        }
        .add-cat-card {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 12px;
            padding: 20px;
            margin-bottom: 24px;
        }
        .add-cat-grid {
            display: grid;
            grid-template-columns: 1fr 1fr auto;
            gap: 16px;
            align-items: end;
        }
        @media (max-width: 768px) {
            .add-cat-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1>Marketplace Category Management</h1>
            <p>Define and manage catalog taxonomies across Crop Categories, Equipment Categories, and Livestock Types.</p>
        </div>
    </div>

    <!-- ALERT MESSAGE -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 16px;">
        <asp:Label ID="lblAlert" runat="server" />
    </asp:Panel>

    <!-- TABS -->
    <div class="cat-tab-group">
        <asp:LinkButton ID="tabCrops" runat="server" CssClass="cat-tab-btn active" OnClick="tabFilter_Click" CommandArgument="Crop">
            <i class="fa-solid fa-wheat-awn"></i> Crop Categories (<asp:Label ID="lblCountCropCats" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabEquipment" runat="server" CssClass="cat-tab-btn" OnClick="tabFilter_Click" CommandArgument="Equipment">
            <i class="fa-solid fa-tractor"></i> Equipment Categories (<asp:Label ID="lblCountEquipCats" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabAnimals" runat="server" CssClass="cat-tab-btn" OnClick="tabFilter_Click" CommandArgument="Animal">
            <i class="fa-solid fa-cow"></i> Livestock Types (<asp:Label ID="lblCountAnimalTypes" runat="server" Text="0" />)
        </asp:LinkButton>
    </div>

    <!-- ADD CATEGORY CARD -->
    <div class="add-cat-card">
        <h3 style="margin-bottom: 14px; font-size: 15px; font-weight: 700; color: #1e293b;">
            <i class="fa-solid fa-plus-circle" style="color: #1b6d34; margin-right: 6px;"></i> Add New <asp:Label ID="lblAddHeading" runat="server" Text="Crop Category" />
        </h3>
        <div class="add-cat-grid">
            <div>
                <label style="display: block; font-size: 12px; font-weight: 600; color: #475569; margin-bottom: 6px;">Category / Type Name *</label>
                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="e.g. Organic Grains, Harvesters, Dairy Cattle..." />
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtCategoryName" 
                    ErrorMessage="Name is required" CssClass="error-msg" Display="Dynamic" ValidationGroup="AddCat" />
            </div>
            <div>
                <label style="display: block; font-size: 12px; font-weight: 600; color: #475569; margin-bottom: 6px;">Description (Optional)</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Brief category description..." />
            </div>
            <div>
                <asp:Button ID="btnAddCategory" runat="server" Text="+ Add Category" CssClass="admin-btn-primary" 
                    OnClick="btnAddCategory_Click" ValidationGroup="AddCat" style="padding: 10px 24px;" />
            </div>
        </div>
    </div>

    <!-- CATEGORIES TABLE -->
    <div class="admin-table-container">
        <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
            <div class="empty-msg" style="padding: 40px; text-align: center;">
                <h3>No categories found</h3>
                <p style="margin-top:8px; color: #64748b;">Add your first category using the form above.</p>
            </div>
        </asp:Panel>

        <asp:Repeater ID="rptCategories" runat="server" OnItemCommand="rptCategories_ItemCommand">
            <HeaderTemplate>
                <table class="admin-table">
                    <thead>
                        <tr>
                            <th>CATEGORY / TYPE NAME</th>
                            <th>DESCRIPTION</th>
                            <th>TOTAL ASSOCIATED LISTINGS</th>
                            <th>CREATED DATE</th>
                            <th style="text-align: right;">ACTIONS</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <strong style="color: #0f172a; font-size: 14px;"><%# Eval("Name") %></strong>
                    </td>
                    <td>
                        <span style="color: #64748b; font-size: 13px;"><%# Eval("Description") %></span>
                    </td>
                    <td>
                        <span class="user-role-tag" style="background: #f1f5f9; color: #334155; font-weight: 700;">
                            <%# Eval("ItemCount") %> Listings
                        </span>
                    </td>
                    <td style="font-size: 12px; color: #64748b;">
                        <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("dd MMM yyyy") %>
                    </td>
                    <td style="text-align: right;">
                        <div class="action-btn-group" style="justify-content: flex-end;">
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="action-icon-link action-delete"
                                CommandName="DeleteCat" CommandArgument='<%# Eval("ID") %>'
                                OnClientClick="return confirm('Are you sure you want to delete this category? Associated listings may be affected.');" 
                                title="Delete Category">
                                <i class="fa-regular fa-trash-can"></i>
                            </asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <div class="admin-pagination-row">
            <div>
                Total: <asp:Label ID="lblTotalCount" runat="server" Text="0" /> categories
            </div>
        </div>
    </div>
</asp:Content>
