<%@ Page Title="Order Management - AgriTech Admin" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderManagement.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Admin.OrderManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .order-tab-group {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
            flex-wrap: wrap;
        }
        .order-tab-btn {
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
        .order-tab-btn.active {
            background: #1b6d34;
            color: #fff;
            border-color: #1b6d34;
        }
        .badge-pending { background: #fef3c7; color: #b45309; }
        .badge-accepted { background: #dcfce7; color: #15803d; }
        .badge-completed { background: #e0e7ff; color: #4338ca; }
        .badge-rejected { background: #fee2e2; color: #b91c1c; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1>Marketplace Order & Inquiry Management</h1>
            <p>Track, oversee, and manage all marketplace transaction requests placed between buyers and farmers.</p>
        </div>
    </div>

    <!-- ALERT MESSAGE -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 16px;">
        <asp:Label ID="lblAlert" runat="server" />
    </asp:Panel>

    <!-- TABS -->
    <div class="order-tab-group">
        <asp:LinkButton ID="tabAll" runat="server" CssClass="order-tab-btn active" OnClick="tabFilter_Click" CommandArgument="All">
            <i class="fa-solid fa-list-check"></i> All Orders (<asp:Label ID="lblCountAll" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabPending" runat="server" CssClass="order-tab-btn" OnClick="tabFilter_Click" CommandArgument="Pending">
            <i class="fa-regular fa-clock"></i> Pending (<asp:Label ID="lblCountPending" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabAccepted" runat="server" CssClass="order-tab-btn" OnClick="tabFilter_Click" CommandArgument="Accepted">
            <i class="fa-regular fa-circle-check"></i> Accepted (<asp:Label ID="lblCountAccepted" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabCompleted" runat="server" CssClass="order-tab-btn" OnClick="tabFilter_Click" CommandArgument="Completed">
            <i class="fa-solid fa-check-double"></i> Completed (<asp:Label ID="lblCountCompleted" runat="server" Text="0" />)
        </asp:LinkButton>
        <asp:LinkButton ID="tabRejected" runat="server" CssClass="order-tab-btn" OnClick="tabFilter_Click" CommandArgument="Rejected">
            <i class="fa-regular fa-circle-xmark"></i> Rejected (<asp:Label ID="lblCountRejected" runat="server" Text="0" />)
        </asp:LinkButton>
    </div>

    <!-- TOOLBAR -->
    <div class="admin-toolbar-card">
        <div class="admin-search-wrapper">
            <i class="fa-solid fa-magnifying-glass"></i>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                placeholder="Search orders by item name, buyer name, contact number..." AutoPostBack="true" 
                OnTextChanged="txtSearch_TextChanged" />
        </div>

        <div class="admin-toolbar-actions">
            <asp:LinkButton ID="btnExport" runat="server" CssClass="btn-admin-tool" OnClick="btnExport_Click">
                <i class="fa-solid fa-download"></i> Export Orders
            </asp:LinkButton>
        </div>
    </div>

    <!-- ORDERS TABLE -->
    <div class="admin-table-container">
        <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
            <div class="empty-msg" style="padding: 40px; text-align: center;">
                <h3>No orders found</h3>
                <p style="margin-top:8px; color: #64748b;">No marketplace orders match the current criteria.</p>
            </div>
        </asp:Panel>

        <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
            <HeaderTemplate>
                <table class="admin-table">
                    <thead>
                        <tr>
                            <th>ORDER ID</th>
                            <th>ITEM DETAILS</th>
                            <th>BUYER</th>
                            <th>SELLER (FARMER)</th>
                            <th>OFFERED PRICE</th>
                            <th>STATUS</th>
                            <th>DATE</th>
                            <th style="text-align: right;">ACTIONS</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <strong style="color: #1e293b;">#ORD-<%# Eval("OrderID") %></strong>
                    </td>
                    <td>
                        <div>
                            <div class="user-name-text"><%# Eval("ItemName") %></div>
                            <span class="user-role-tag" style="background:#f1f5f9; color:#475569; font-size:10.5px;">
                                <%# Eval("ItemType") %>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div>
                            <strong><%# Eval("BuyerName") %></strong>
                            <div style="font-size: 11.5px; color: #64748b;"><%# Eval("ContactNumber") %></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <strong><%# Eval("SellerName") %></strong>
                            <div style="font-size: 11.5px; color: #64748b;"><%# Eval("SellerMobile") %></div>
                        </div>
                    </td>
                    <td>
                        <strong style="color: #1b6d34; font-size: 14px;">
                            <%# Eval("OfferedPrice") != DBNull.Value ? "₹" + Convert.ToDecimal(Eval("OfferedPrice")).ToString("N0") : "Negotiable" %>
                        </strong>
                    </td>
                    <td>
                        <span class='status-badge-pill <%# GetStatusClass(Eval("OrderStatus").ToString()) %>'>
                            <%# Eval("OrderStatus") %>
                        </span>
                    </td>
                    <td style="font-size: 12px; color: #64748b;">
                        <%# Convert.ToDateTime(Eval("OrderDate")).ToString("dd MMM yyyy") %>
                    </td>
                    <td style="text-align: right;">
                        <div class="action-btn-group" style="justify-content: flex-end;">
                            <!-- Mark Completed -->
                            <asp:LinkButton ID="btnComplete" runat="server" CssClass="action-icon-link action-unblock"
                                CommandName="SetStatus" CommandArgument='<%# Eval("OrderID") + ":Completed" %>'
                                title="Mark Completed">
                                <i class="fa-solid fa-check"></i>
                            </asp:LinkButton>

                            <!-- Delete Order -->
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="action-icon-link action-delete"
                                CommandName="DeleteOrder" CommandArgument='<%# Eval("OrderID") %>'
                                OnClientClick="return confirm('Are you sure you want to delete this order record?');" 
                                title="Delete Order">
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
                Showing <asp:Label ID="lblShowingCount" runat="server" Text="0" /> of 
                <asp:Label ID="lblTotalCount" runat="server" Text="0" /> orders
            </div>
        </div>
    </div>
</asp:Content>
