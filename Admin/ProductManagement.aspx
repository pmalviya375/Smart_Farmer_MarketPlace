<%@ Page Title="Product Management - AgriTech Admin" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductManagement.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Admin.ProductManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .product-tab-group {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
        }
        .product-tab-btn {
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
        .product-tab-btn.active {
            background: #1b6d34;
            color: #fff;
            border-color: #1b6d34;
        }
        .product-thumb-img {
            width: 44px;
            height: 44px;
            border-radius: 8px;
            object-fit: cover;
            border: 1px solid #e2e8f0;
            background: #f8fafc;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1>Product & Listing Management</h1>
            <p>Monitor, activate, deactivate, and moderate all marketplace listings across Crops, Equipment, and Animals.</p>
        </div>
    </div>

    <!-- ALERT MESSAGE -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 16px;">
        <asp:Label ID="lblAlert" runat="server" />
    </asp:Panel>

    <!-- TABS: ALL / CROPS / EQUIPMENT / ANIMALS -->
    <div class="product-tab-group">
        <asp:LinkButton ID="tabAll" runat="server" CssClass="product-tab-btn active" OnClick="tabFilter_Click" CommandArgument="All">
            <i class="fa-solid fa-layer-group"></i> All Listings (<asp:Label ID="lblCountAll" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabCrops" runat="server" CssClass="product-tab-btn" OnClick="tabFilter_Click" CommandArgument="Crop">
            <i class="fa-solid fa-wheat-awn"></i> Crops (<asp:Label ID="lblCountCrops" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabEquipment" runat="server" CssClass="product-tab-btn" OnClick="tabFilter_Click" CommandArgument="Equipment">
            <i class="fa-solid fa-tractor"></i> Equipment (<asp:Label ID="lblCountEquipment" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabAnimals" runat="server" CssClass="product-tab-btn" OnClick="tabFilter_Click" CommandArgument="Animal">
            <i class="fa-solid fa-cow"></i> Livestock (<asp:Label ID="lblCountAnimals" runat="server" Text="0" />)
        </asp:LinkButton>
    </div>

    <!-- TOOLBAR -->
    <div class="admin-toolbar-card">
        <div class="admin-search-wrapper">
            <i class="fa-solid fa-magnifying-glass"></i>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                placeholder="Search products by title, seller name, location..." AutoPostBack="true" 
                OnTextChanged="txtSearch_TextChanged" />
        </div>

        <div class="admin-toolbar-actions">
            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" style="width: auto; padding: 7px 14px;" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                <asp:ListItem Text="All Statuses" Value="" />
                <asp:ListItem Text="Active" Value="Active" />
                <asp:ListItem Text="Inactive" Value="Inactive" />
            </asp:DropDownList>
            <asp:LinkButton ID="btnExport" runat="server" CssClass="btn-admin-tool" OnClick="btnExport_Click">
                <i class="fa-solid fa-download"></i> Export
            </asp:LinkButton>
        </div>
    </div>

    <!-- PRODUCTS TABLE -->
    <div class="admin-table-container">
        <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
            <div class="empty-msg" style="padding: 40px; text-align: center;">
                <h3>No listings found</h3>
                <p style="margin-top:8px; color: #64748b;">No marketplace items match your selected filter or search term.</p>
            </div>
        </asp:Panel>

        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
            <HeaderTemplate>
                <table class="admin-table">
                    <thead>
                        <tr>
                            <th>ITEM</th>
                            <th>TYPE</th>
                            <th>SELLER (FARMER)</th>
                            <th>PRICE / QTY</th>
                            <th>LOCATION</th>
                            <th>STATUS</th>
                            <th>DATE ADDED</th>
                            <th style="text-align: right;">ACTIONS</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <div style="display: flex; align-items: center; gap: 12px;">
                            <img src='<%# ResolveUrl(GetItemImage(Eval("ItemType").ToString(), Eval("MainImage").ToString())) %>' 
                                 class="product-thumb-img" alt="Thumbnail" onerror="this.src='https://placehold.co/44x44?text=Item';" />
                            <div>
                                <div class="user-name-text"><%# Eval("Title") %></div>
                                <div style="font-size: 11.5px; color: #64748b;">Category: <%# Eval("CategoryName") %></div>
                            </div>
                        </div>
                    </td>
                    <td>
                        <span class="user-role-tag" style="background:#eef2ff; color:#3730a3; font-weight:700;">
                            <%# Eval("ItemType") %>
                        </span>
                    </td>
                    <td>
                        <div>
                            <strong><%# Eval("SellerName") %></strong>
                            <div style="font-size: 11.5px; color: #64748b;"><%# Eval("ContactNumber") %></div>
                        </div>
                    </td>
                    <td>
                        <strong style="color: #1b6d34;"><%# Eval("PriceOrQty") %></strong>
                    </td>
                    <td>
                        <%# Eval("Location") %>
                    </td>
                    <td>
                        <span class='status-badge-pill <%# Eval("Status").ToString() == "Active" ? "status-active" : "status-blocked" %>'>
                            <%# Eval("Status") %>
                        </span>
                    </td>
                    <td style="font-size: 12px; color: #64748b;">
                        <%# Convert.ToDateTime(Eval("DateAdded")).ToString("dd MMM yyyy") %>
                    </td>
                    <td style="text-align: right;">
                        <div class="action-btn-group" style="justify-content: flex-end;">
                            <!-- View Details -->
                            <asp:LinkButton ID="btnView" runat="server" CssClass="action-icon-link" 
                                CommandName="View" CommandArgument='<%# Eval("ItemType") + ":" + Eval("ItemID") %>' title="View Details">
                                <i class="fa-regular fa-eye"></i>
                            </asp:LinkButton>

                            <!-- Toggle Status (Activate / Deactivate) -->
                            <asp:LinkButton ID="btnToggle" runat="server" 
                                CssClass='<%# Eval("Status").ToString() == "Active" ? "action-icon-link action-block" : "action-icon-link action-unblock" %>'
                                CommandName="ToggleStatus" CommandArgument='<%# Eval("ItemType") + ":" + Eval("ItemID") %>'
                                title='<%# Eval("Status").ToString() == "Active" ? "Deactivate Listing" : "Activate Listing" %>'>
                                <i class='<%# Eval("Status").ToString() == "Active" ? "fa-solid fa-ban" : "fa-solid fa-check" %>'></i>
                            </asp:LinkButton>

                            <!-- Delete Listing -->
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="action-icon-link action-delete"
                                CommandName="DeleteItem" CommandArgument='<%# Eval("ItemType") + ":" + Eval("ItemID") %>'
                                OnClientClick="return confirm('Are you sure you want to permanently delete this listing?');" title="Delete Listing">
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

        <!-- PAGINATION ROW -->
        <div class="admin-pagination-row">
            <div>
                Showing 1-<asp:Label ID="lblShowingCount" runat="server" Text="0" /> of 
                <asp:Label ID="lblTotalCount" runat="server" Text="0" /> listings
            </div>
            <div class="pagination-controls">
                <asp:LinkButton ID="btnPrev" runat="server" CssClass="page-btn" OnClick="btnPrev_Click">&lt; Previous</asp:LinkButton>
                <span class="page-btn active">1</span>
                <asp:LinkButton ID="btnNext" runat="server" CssClass="page-btn" OnClick="btnNext_Click">Next &gt;</asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
