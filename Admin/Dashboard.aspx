<%@ Page Title="Admin Dashboard - AgriTech" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- PAGE HEADER (MATCHING SCREENSHOT 2) -->
    <div class="page-header" style="margin-bottom:24px;">
        <div>
            <h1>Dashboard Overview</h1>
            <p>Monitor platform growth and user activity.</p>
        </div>
    </div>

    <!-- METRIC CARDS GRID (3 CARDS FROM SCREENSHOT 2) -->
    <div class="admin-metric-grid">
        <!-- Card 1: Total Farmers -->
        <a href="FarmerManagement.aspx" class="admin-metric-card" style="text-decoration:none;">
            <div class="admin-metric-icon icon-farmer">
                <i class="fa-solid fa-tractor"></i>
            </div>
            <div>
                <div class="admin-metric-lbl">Total Farmers</div>
                <div class="admin-metric-val">
                    <asp:Label ID="lblTotalFarmers" runat="server" Text="0" />
                </div>
            </div>
        </a>

        <!-- Card 2: Total Buyers -->
        <a href="BuyerManagement.aspx" class="admin-metric-card" style="text-decoration:none;">
            <div class="admin-metric-icon icon-buyer">
                <i class="fa-solid fa-cart-shopping"></i>
            </div>
            <div>
                <div class="admin-metric-lbl">Total Buyers</div>
                <div class="admin-metric-val">
                    <asp:Label ID="lblTotalBuyers" runat="server" Text="0" />
                </div>
            </div>
        </a>

        <!-- Card 3: Pending Contact Queries -->
        <a href="ContactManagement.aspx" class="admin-metric-card" style="text-decoration:none;">
            <div class="admin-metric-icon icon-query">
                <i class="fa-solid fa-circle-question"></i>
            </div>
            <div>
                <div class="admin-metric-lbl">Pending Contact Queries</div>
                <div class="admin-metric-val">
                    <asp:Label ID="lblPendingQueries" runat="server" Text="0" />
                </div>
            </div>
        </a>
    </div>

    <!-- PLATFORM ACTIVITY & OVERVIEW -->
    <div style="display:grid; grid-template-columns: 1.2fr 1fr; gap:24px; margin-top:10px;">
        
        <!-- RECENT REGISTRATIONS -->
        <div class="info-card">
            <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:16px; border-bottom:1px solid #f1f5f9; padding-bottom:12px;">
                <div>
                    <h3 style="font-size:16px; color:#0f172a;"><i class="fa-solid fa-users" style="color:#1b6d34; margin-right:8px;"></i> Recent User Registrations</h3>
                    <p style="font-size:12.5px; color:#64748b;">Newly onboarded farmers and commodity buyers</p>
                </div>
                <a href="FarmerManagement.aspx" style="color:#1b6d34; font-size:13px; font-weight:600; text-decoration:none;">View All Farmers →</a>
            </div>

            <asp:Repeater ID="rptRecentUsers" runat="server">
                <ItemTemplate>
                    <div style="display:flex; justify-content:space-between; align-items:center; padding:12px 0; border-bottom:1px solid #f1f5f9;">
                        <div class="user-cell-flex">
                            <div class='initial-avatar <%# GetAvatarColorClass(Container.ItemIndex) %>'>
                                <%# GetInitials(Eval("FullName").ToString()) %>
                            </div>
                            <div>
                                <div class="user-name-text"><%# Eval("FullName") %></div>
                                <div style="font-size:12px; color:#64748b; margin-top:2px;">
                                    <i class="fa-solid fa-phone" style="font-size:10.5px;"></i> <%# Eval("MobileNumber") %> &bull; <%# Eval("Village") %>
                                </div>
                            </div>
                        </div>

                        <div>
                            <span class='status-badge-pill <%# Eval("Role").ToString() == "Farmer" ? "status-active" : "status-pending" %>'>
                                <%# Eval("Role") %>
                            </span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- RECENT CONTACT INQUIRIES -->
        <div class="info-card">
            <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:16px; border-bottom:1px solid #f1f5f9; padding-bottom:12px;">
                <div>
                    <h3 style="font-size:16px; color:#0f172a;"><i class="fa-solid fa-comments" style="color:#1b6d34; margin-right:8px;"></i> Recent Inquiries</h3>
                    <p style="font-size:12.5px; color:#64748b;">Queries sent by farmers and buyers to admin</p>
                </div>
                <a href="ContactManagement.aspx" style="color:#1b6d34; font-size:13px; font-weight:600; text-decoration:none;">Manage Queries →</a>
            </div>

            <asp:Panel ID="pnlNoQueries" runat="server" Visible="false">
                <div class="empty-msg" style="padding:24px;">
                    <p>No contact queries pending at this moment.</p>
                </div>
            </asp:Panel>

            <asp:Repeater ID="rptRecentQueries" runat="server">
                <ItemTemplate>
                    <div style="padding:12px 0; border-bottom:1px solid #f1f5f9;">
                        <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:4px;">
                            <span style="font-weight:700; font-size:13.5px; color:#0f172a;"><%# Eval("SenderName") %> (<%# Eval("SenderRole") %>)</span>
                            <span class='status-badge-pill <%# Eval("Status").ToString() == "Pending" ? "status-blocked" : "status-active" %>' style="font-size:11px;">
                                <%# Eval("Status") %>
                            </span>
                        </div>
                        <div style="font-weight:600; font-size:13px; color:#1b6d34; margin-bottom:3px;"><%# Eval("Subject") %></div>
                        <div style="font-size:12px; color:#64748b; text-overflow:ellipsis; overflow:hidden; white-space:nowrap;">
                            <%# Eval("Message") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

    </div>
</asp:Content>
