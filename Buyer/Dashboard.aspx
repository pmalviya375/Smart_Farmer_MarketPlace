<%@ Page Title="Buyer Dashboard - AgriTech" Language="C#" MasterPageFile="~/Buyer/Buyer.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Buyer.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="farmer-dashboard-wrapper">
        
        <!-- 1. HERO BANNER -->
        <div class="farmer-hero-card">
            <div class="hero-left">
                <div class="hero-badge">
                    <i class="fa-solid fa-leaf"></i> Verified Buyer Hub
                </div>
                <h1 class="hero-title">
                    <asp:Label ID="lblGreeting" runat="server" Text="Welcome" />, 
                    <span class="farmer-highlight"><asp:Label ID="lblBuyerName" runat="server" /></span>!
                </h1>
                <p class="hero-subtitle">
                    Here is your agricultural procurement and direct farmer trade summary for 
                    <strong><i class="fa-regular fa-calendar-check"></i> <asp:Label ID="lblToday" runat="server" /></strong>.
                </p>

                <div class="hero-chips-bar">
                    <div class="hero-chip">
                        <i class="fa-solid fa-cart-shopping"></i>
                        <span>Total Orders: <strong><asp:Label ID="lblTotalOrdersHero" runat="server" Text="0" /></strong></span>
                    </div>
                    <div class="hero-chip chip-alert">
                        <i class="fa-solid fa-clock"></i>
                        <span>Pending Inquiries: <strong><asp:Label ID="lblPendingHero" runat="server" Text="0" /></strong></span>
                    </div>
                </div>
            </div>

            <div class="hero-right">
                <div class="hero-actions">
                    <a href="Marketplace.aspx" class="hero-btn-primary">
                        <i class="fa-solid fa-store"></i> Browse Marketplace
                    </a>
                    <a href="OrderRequest.aspx" class="hero-btn-ghost">
                        <i class="fa-solid fa-cart-plus"></i> Submit Buy Request
                    </a>
                </div>
            </div>
        </div>

        <!-- 2. SECTION TITLE -->
        <div class="section-heading-bar">
            <div>
                <h2>Overview & Statistics</h2>
                <p>Live marketplace availability and trade performance metrics</p>
            </div>
            <div class="heading-badge">
                <i class="fa-solid fa-arrows-rotate"></i> Real-time Sync
            </div>
        </div>

        <!-- 3. KPI / STATS GRID (6 Cards Matching Farmer Dashboard) -->
        <div class="kpi-grid">
            <!-- Crop Stat -->
            <a href="Marketplace.aspx?type=Crop" class="kpi-card kpi-crop">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Available Crops</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-seedling"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblCropCount" runat="server" Text="0" /></h3>
                    <span class="kpi-unit">crops</span>
                </div>
                <div class="kpi-footer">
                    <span>Grains, pulses & veggies</span>
                    <span class="kpi-arrow">Browse <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Equipment Stat -->
            <a href="Marketplace.aspx?type=Equipment" class="kpi-card kpi-equip">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Farm Machinery</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-tractor"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblEquipCount" runat="server" Text="0" /></h3>
                    <span class="kpi-unit">machinery</span>
                </div>
                <div class="kpi-footer">
                    <span>Tractors & tools</span>
                    <span class="kpi-arrow">Browse <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Animal Stat -->
            <a href="Marketplace.aspx?type=Animal" class="kpi-card kpi-animal">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Farm Livestock</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-cow"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblAnimalCount" runat="server" Text="0" /></h3>
                    <span class="kpi-unit">livestock</span>
                </div>
                <div class="kpi-footer">
                    <span>Cattle, dairy & poultry</span>
                    <span class="kpi-arrow">Browse <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Pending Inquiries -->
            <a href="MyOrders.aspx" class="kpi-card kpi-pending">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Pending Offers</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-clock"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblPendingOrders" runat="server" Text="0" /></h3>
                    <span class="kpi-status-badge badge-pending-pulse">Awaiting Farmer</span>
                </div>
                <div class="kpi-footer">
                    <span>Under farmer review</span>
                    <span class="kpi-arrow">Track <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Confirmed Purchases -->
            <a href="MyOrders.aspx" class="kpi-card kpi-accepted">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Confirmed Purchases</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-circle-check"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblConfirmedOrders" runat="server" Text="0" /></h3>
                    <span class="kpi-status-badge badge-success">Deals Closed</span>
                </div>
                <div class="kpi-footer">
                    <span>Agreed farmer orders</span>
                    <span class="kpi-arrow">View <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Total Requests -->
            <a href="MyOrders.aspx" class="kpi-card kpi-orders">
                <div class="kpi-card-header">
                    <span class="kpi-tag">My Total Orders</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-cart-shopping"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblTotalOrders" runat="server" Text="0" /></h3>
                    <span class="kpi-unit">requests</span>
                </div>
                <div class="kpi-footer">
                    <span>Lifetime procurement</span>
                    <span class="kpi-arrow">View All <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>
        </div>

        <!-- 4. DASHBOARD CONTENT GRID (Recent Orders + Featured Marketplace) -->
        <div style="display:grid; grid-template-columns: 1.2fr 1fr; gap:24px; margin-top:20px;">
            
            <!-- LEFT: RECENT ORDERS -->
            <div class="info-card">
                <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:16px; border-bottom:1px solid #f1f5f9; padding-bottom:12px;">
                    <div>
                        <h3 style="font-size:16px; color:#0f172a;"><i class="fa-solid fa-clipboard-list" style="color:#15803d; margin-right:8px;"></i> Recent Order Requests</h3>
                        <p style="font-size:12.5px; color:#64748b;">Live status of your recent purchase inquiries</p>
                    </div>
                    <a href="MyOrders.aspx" style="color:#15803d; font-size:13px; font-weight:600; text-decoration:none;">View All <i class="fa-solid fa-arrow-right"></i></a>
                </div>

                <asp:Panel ID="pnlNoOrders" runat="server" Visible="false">
                    <div class="empty-msg" style="padding:24px;">
                        <p>No purchase requests yet. Browse the marketplace to connect with farmers!</p>
                    </div>
                </asp:Panel>

                <asp:Repeater ID="rptRecentOrders" runat="server">
                    <ItemTemplate>
                        <div style="display:flex; justify-content:space-between; align-items:center; padding:12px 0; border-bottom:1px solid #f1f5f9;">
                            <div>
                                <div style="font-weight:700; font-size:14px; color:#0f172a;">
                                    <%# Eval("ItemName") %> <span style="font-size:12px; color:#64748b; font-weight:normal;">(#ORD-<%# Eval("OrderID") %>)</span>
                                </div>
                                <div style="font-size:12px; color:#64748b; margin-top:3px;">
                                    Producer: <strong><%# Eval("SellerName") %></strong> &bull; <%# Eval("ItemType") %> &bull; 
                                    <strong><i class="fa-solid fa-indian-rupee-sign" style="font-size:11px;"></i> <%# Convert.ToDecimal(Eval("OfferedPrice")).ToString("N0") %></strong>
                                </div>
                            </div>
                            <span class='order-status status-<%# Eval("OrderStatus").ToString().ToLower() %>' style="font-size:11.5px; padding:4px 10px;">
                                <%# Eval("OrderStatus") %>
                            </span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- RIGHT: FEATURED MARKETPLACE PRODUCE & ASSETS -->
            <div class="info-card">
                <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:16px; border-bottom:1px solid #f1f5f9; padding-bottom:12px;">
                    <div>
                        <h3 style="font-size:16px; color:#0f172a;"><i class="fa-solid fa-store" style="color:#15803d; margin-right:8px;"></i> Featured in Marketplace</h3>
                        <p style="font-size:12.5px; color:#64748b;">Direct from farmers: Crops, Machinery & Livestock</p>
                    </div>
                    <a href="Marketplace.aspx" style="color:#15803d; font-size:13px; font-weight:600; text-decoration:none;">Explore <i class="fa-solid fa-arrow-right"></i></a>
                </div>

                <asp:Repeater ID="rptFeaturedProduce" runat="server">
                    <ItemTemplate>
                        <div style="display:flex; align-items:center; gap:12px; padding:10px 0; border-bottom:1px solid #f1f5f9;">
                            <div style="width:48px; height:48px; border-radius:8px; overflow:hidden; background:#f1f5f9; display:flex; align-items:center; justify-content:center; font-size:18px; flex-shrink:0;">
                                <%# Eval("MainImage") == DBNull.Value || Eval("MainImage").ToString() == "" 
                                    ? GetIcon(Eval("ItemType").ToString())
                                    : "<img src='" + ResolveUrl(Eval("MainImage").ToString()) + "' style='width:100%;height:100%;object-fit:cover;' />" %>
                            </div>
                            <div style="flex:1;">
                                <div style="font-weight:700; font-size:13.5px; color:#0f172a;">
                                    <%# Eval("ItemName") %>
                                </div>
                                <div style="font-size:12px; color:#64748b;">
                                    <i class="fa-solid fa-location-dot" style="font-size:11px;"></i> <%# Eval("SellerVillage") %> &bull; <i class="fa-solid fa-indian-rupee-sign" style="font-size:11px;"></i> <%# Convert.ToDecimal(Eval("Price")).ToString("N0") %>
                                </div>
                            </div>
                            <a href='MarketplaceDetails.aspx?type=<%# Eval("ItemType") %>&id=<%# Eval("ItemID") %>' 
                               class="btn-view" style="padding:6px 12px; font-size:12px; text-decoration:none;">
                                View
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>

    </div>
</asp:Content>
