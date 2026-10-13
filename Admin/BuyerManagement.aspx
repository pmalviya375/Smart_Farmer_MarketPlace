<%@ Page Title="Buyer Management - AgriTech Admin" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BuyerManagement.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Admin.BuyerManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- PAGE HEADER -->
    <div class="page-header">
        <div>
            <h1>Buyer Management</h1>
            <p>View and manage all registered agricultural commodity buyers and traders in the system.</p>
        </div>
    </div>

    <!-- ALERT MESSAGE -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 16px;">
        <asp:Label ID="lblAlert" runat="server" />
    </asp:Panel>

    <!-- TOOLBAR -->
    <div class="admin-toolbar-card">
        <div class="admin-search-wrapper">
            <i class="fa-solid fa-magnifying-glass"></i>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                placeholder="Search buyers by name, phone, address..." AutoPostBack="true" 
                OnTextChanged="txtSearch_TextChanged" />
        </div>

        <div class="admin-toolbar-actions">
            <asp:LinkButton ID="btnFilter" runat="server" CssClass="btn-admin-tool" OnClick="btnFilter_Click">
                <i class="fa-solid fa-filter"></i> Filter
            </asp:LinkButton>
            <asp:LinkButton ID="btnExport" runat="server" CssClass="btn-admin-tool" OnClick="btnExport_Click">
                <i class="fa-solid fa-download"></i> Export
            </asp:LinkButton>
        </div>
    </div>

    <!-- DIRECTORY TABLE -->
    <div class="admin-table-container">
        <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
            <div class="empty-msg" style="padding: 40px;">
                <h3>No buyers found</h3>
                <p style="margin-top:8px;">Try modifying your search criteria.</p>
            </div>
        </asp:Panel>

        <asp:Repeater ID="rptBuyers" runat="server" OnItemCommand="rptBuyers_ItemCommand">
            <HeaderTemplate>
                <table class="admin-table">
                    <thead>
                        <tr>
                            <th>NAME</th>
                            <th>MOBILE NUMBER</th>
                            <th>ADDRESS</th>
                            <th>TOTAL ORDERS</th>
                            <th>STATUS</th>
                            <th style="text-align: right;">ACTIONS</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <div class="user-cell-flex">
                            <div class='initial-avatar <%# GetAvatarColorClass(Container.ItemIndex) %>'>
                                <%# GetInitials(Eval("FullName").ToString()) %>
                            </div>
                            <div>
                                <div class="user-name-text"><%# Eval("FullName") %></div>
                                <div style="font-size: 11.5px; color: #64748b;"><%# Eval("Email") %></div>
                            </div>
                        </div>
                    </td>
                    <td>
                        <strong><%# Eval("MobileNumber") %></strong>
                    </td>
                    <td>
                        <%# Eval("Village") %>
                    </td>
                    <td>
                        <strong><%# Eval("TotalOrders") %></strong>
                    </td>
                    <td>
                        <span class='status-badge-pill <%# Eval("Status").ToString() == "Active" ? "status-active" : "status-blocked" %>'>
                            <%# Eval("Status") %>
                        </span>
                    </td>
                    <td style="text-align: right;">
                        <div class="action-btn-group" style="justify-content: flex-end;">
                            <!-- View Details -->
                            <asp:LinkButton ID="btnView" runat="server" CssClass="action-icon-link" 
                                CommandName="View" CommandArgument='<%# Eval("FarmerID") %>' title="View Details">
                                <i class="fa-regular fa-eye"></i>
                            </asp:LinkButton>

                            <!-- Toggle Status (Block / Unblock) -->
                            <asp:LinkButton ID="btnToggleStatus" runat="server" 
                                CssClass='<%# Eval("Status").ToString() == "Active" ? "action-icon-link action-block" : "action-icon-link action-unblock" %>'
                                CommandName="ToggleStatus" CommandArgument='<%# Eval("FarmerID") %>'
                                title='<%# Eval("Status").ToString() == "Active" ? "Block Buyer" : "Unblock Buyer" %>'>
                                <i class='<%# Eval("Status").ToString() == "Active" ? "fa-solid fa-ban" : "fa-solid fa-check" %>'></i>
                            </asp:LinkButton>

                            <!-- Delete Buyer -->
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="action-icon-link action-delete"
                                CommandName="DeleteBuyer" CommandArgument='<%# Eval("FarmerID") %>'
                                OnClientClick="return confirm('Are you sure you want to delete this buyer?');" title="Delete Buyer">
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
                <asp:Label ID="lblTotalCount" runat="server" Text="0" /> buyers
            </div>
            <div class="pagination-controls">
                <asp:LinkButton ID="btnPrev" runat="server" CssClass="page-btn" OnClick="btnPrev_Click">&lt; Previous</asp:LinkButton>
                <span class="page-btn active">1</span>
                <asp:LinkButton ID="btnNext" runat="server" CssClass="page-btn" OnClick="btnNext_Click">Next &gt;</asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
