<%@ Page Title="Farmer Dashboard" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="farmer-dashboard-wrapper">
        
        <!-- 1. HERO BANNER -->
        <div class="farmer-hero-card">
            <div class="hero-left">
                <div class="hero-badge">
                    <i class="fa-solid fa-leaf"></i> Verified Farmer Hub
                </div>
                <h1 class="hero-title">
                    <asp:Label ID="lblGreeting" runat="server" Text="Welcome" />, 
                    <span class="farmer-highlight"><asp:Label ID="lblName" runat="server" /></span>! 🌾
                </h1>
                <p class="hero-subtitle">
                    Here is your live farm marketplace summary for 
                    <strong><i class="fa-regular fa-calendar-check"></i> <asp:Label ID="lblToday" runat="server" /></strong>.
                </p>

                <div class="hero-chips-bar">
                    <div class="hero-chip">
                        <i class="fa-solid fa-boxes-stacked"></i>
                        <span>Active Listings: <strong><asp:Label ID="lblTotalListings" runat="server" Text="0" /></strong></span>
                    </div>
                    <div class="hero-chip chip-alert">
                        <i class="fa-solid fa-bell"></i>
                        <span>Pending Inquiries: <strong><asp:Label ID="lblPendingHero" runat="server" Text="0" /></strong></span>
                    </div>
                </div>
            </div>

            <div class="hero-right">
                <div class="hero-actions">
                    <a href="CropForm.aspx" class="hero-btn-primary">
                        <i class="fa-solid fa-circle-plus"></i> Post New Crop
                    </a>
                    <a href="Marketplace.aspx" class="hero-btn-ghost">
                        <i class="fa-solid fa-store"></i> Browse Marketplace
                    </a>
                </div>
            </div>
        </div>

        <!-- 2. SECTION TITLE -->
        <div class="section-heading-bar">
            <div>
                <h2>Overview & Statistics</h2>
                <p>Live inventory counts and trade performance metrics</p>
            </div>
            <div class="heading-badge">
                <i class="fa-solid fa-arrows-rotate"></i> Real-time Sync
            </div>
        </div>

        <!-- 3. STATS / KPI GRID -->
        <div class="kpi-grid">
            <!-- Crop Stat -->
            <a href="CropManagement.aspx" class="kpi-card kpi-crop">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Crop Inventory</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-seedling"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblCropCount" runat="server" Text="0" /></h3>
                    <span class="kpi-unit">crops</span>
                </div>
                <div class="kpi-footer">
                    <span>Grains, seeds & veggies</span>
                    <span class="kpi-arrow">Manage <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Equipment Stat -->
            <a href="EquipmentManagement.aspx" class="kpi-card kpi-equip">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Machinery & Tools</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-tractor"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblEquipCount" runat="server" Text="0" /></h3>
                    <span class="kpi-unit">items</span>
                </div>
                <div class="kpi-footer">
                    <span>Tractors, tillers & tools</span>
                    <span class="kpi-arrow">Manage <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Animal Stat -->
            <a href="AnimalManagement.aspx" class="kpi-card kpi-animal">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Farm Livestock</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-paw"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblAnimalCount" runat="server" Text="0" /></h3>
                    <span class="kpi-unit">animals</span>
                </div>
                <div class="kpi-footer">
                    <span>Cattle, dairy & poultry</span>
                    <span class="kpi-arrow">Manage <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Pending Incoming -->
            <a href="OrderRequest.aspx" class="kpi-card kpi-pending">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Pending Inquiries</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-inbox"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblPendingIncoming" runat="server" Text="0" /></h3>
                    <span class="kpi-status-badge badge-pending-pulse">Action Needed</span>
                </div>
                <div class="kpi-footer">
                    <span>Buyers awaiting reply</span>
                    <span class="kpi-arrow">Respond <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- Accepted Orders -->
            <a href="OrderRequest.aspx" class="kpi-card kpi-accepted">
                <div class="kpi-card-header">
                    <span class="kpi-tag">Confirmed Deals</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-circle-check"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblAccepted" runat="server" Text="0" /></h3>
                    <span class="kpi-status-badge badge-success">Closed Deals</span>
                </div>
                <div class="kpi-footer">
                    <span>Agreed marketplace orders</span>
                    <span class="kpi-arrow">View <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>

            <!-- My Orders -->
            <a href="OrderRequest.aspx" class="kpi-card kpi-orders">
                <div class="kpi-card-header">
                    <span class="kpi-tag">My Buy Requests</span>
                    <div class="kpi-icon-wrap"><i class="fa-solid fa-cart-shopping"></i></div>
                </div>
                <div class="kpi-value-row">
                    <h3 class="kpi-value"><asp:Label ID="lblMyOrders" runat="server" Text="0" /></h3>
                    <span class="kpi-unit">requests</span>
                </div>
                <div class="kpi-footer">
                    <span>Orders sent to sellers</span>
                    <span class="kpi-arrow">Track <i class="fa-solid fa-arrow-right"></i></span>
                </div>
            </a>
        </div>

        <!-- 4. TWO-COLUMN DASHBOARD BODY -->
        <div class="dash-layout-grid">
            
            <!-- MAIN / LEFT COLUMN -->
            <div class="dash-main-col">
                
                <!-- RECENT ACTIVITY CARD -->
                <div class="dash-card">
                    <div class="dash-card-header">
                        <div class="header-left">
                            <div class="card-icon-circle"><i class="fa-solid fa-clock-rotate-left"></i></div>
                            <div>
                                <h3>Recent Market & Order Activity</h3>
                                <p>Live stream of incoming buyer offers and orders</p>
                            </div>
                        </div>
                        <a href="OrderRequest.aspx" class="dash-card-header-link">
                            View All <i class="fa-solid fa-arrow-right"></i>
                        </a>
                    </div>

                    <div class="dash-card-body p-0">
                        <asp:Panel ID="pnlNoActivity" runat="server" Visible="false">
                            <div class="dash-empty-state">
                                <div class="empty-icon-wrap">
                                    <i class="fa-regular fa-clipboard"></i>
                                </div>
                                <h4>No Trade Activity Yet</h4>
                                <p>When fellow farmers or buyers place requests for your listings, notifications will appear here instantly.</p>
                                <div class="empty-btn-group">
                                    <a href="CropForm.aspx" class="btn-subtle"><i class="fa-solid fa-plus"></i> List Your Harvest</a>
                                    <a href="Marketplace.aspx" class="btn-subtle-primary"><i class="fa-solid fa-store"></i> Browse Market</a>
                                </div>
                            </div>
                        </asp:Panel>

                        <div class="activity-stream">
                            <asp:Repeater ID="rptActivity" runat="server">
                                <ItemTemplate>
                                    <a href="OrderRequest.aspx" class="activity-stream-item">
                                        <div class="stream-icon-box stream-<%# Eval("ItemType").ToString().ToLower() %>">
                                            <i class='<%# Eval("ItemIconClass") %>'></i>
                                        </div>
                                        <div class="stream-info">
                                            <div class="stream-title-row">
                                                <span class="stream-title"><%# Eval("Title") %></span>
                                                <span class='stream-dir-badge badge-<%# Eval("DirectionCss") %>'>
                                                    <%# Eval("DirectionBadge") %>
                                                </span>
                                            </div>
                                            <div class="stream-meta">
                                                <span><i class="fa-regular fa-clock"></i> <%# Eval("TimeAgo") %></span>
                                                <span class="meta-dot">·</span>
                                                <span><%# Eval("PartnerDetail") %></span>
                                            </div>
                                        </div>
                                        <div class="stream-status">
                                            <span class='status-pill status-<%# Eval("StatusCss") %>'>
                                                <span class="status-dot"></span>
                                                <%# Eval("Status") %>
                                            </span>
                                            <i class="fa-solid fa-chevron-right stream-chevron"></i>
                                        </div>
                                    </a>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

                <!-- MY ACTIVE INVENTORY PREVIEW CARD -->
                <div class="dash-card">
                    <div class="dash-card-header">
                        <div class="header-left">
                            <div class="card-icon-circle"><i class="fa-solid fa-boxes-stacked"></i></div>
                            <div>
                                <h3>My Recent Inventory Listings</h3>
                                <p>Overview of your active crops, machinery, and livestock</p>
                            </div>
                        </div>
                        <div class="header-actions">
                            <a href="CropManagement.aspx" class="dash-card-header-link">Manage All <i class="fa-solid fa-arrow-right"></i></a>
                        </div>
                    </div>

                    <div class="dash-card-body p-0">
                        <asp:Panel ID="pnlNoListings" runat="server" Visible="false">
                            <div class="dash-empty-state">
                                <div class="empty-icon-wrap">
                                    <i class="fa-solid fa-seedling"></i>
                                </div>
                                <h4>No Active Listings Yet</h4>
                                <p>Start selling by adding your harvest crops, farm machinery, or animals to the marketplace.</p>
                                <div class="empty-btn-group">
                                    <a href="CropForm.aspx" class="btn-subtle-primary"><i class="fa-solid fa-plus"></i> Add Crop</a>
                                    <a href="EquipmentForm.aspx" class="btn-subtle"><i class="fa-solid fa-plus"></i> Add Equipment</a>
                                    <a href="AnimalForm.aspx" class="btn-subtle"><i class="fa-solid fa-plus"></i> Add Animal</a>
                                </div>
                            </div>
                        </asp:Panel>

                        <div class="listings-table-wrap">
                            <asp:Repeater ID="rptRecentListings" runat="server">
                                <ItemTemplate>
                                    <div class="listing-row">
                                        <div class="listing-img-box">
                                            <%# RenderItemImage(Eval("MainImage"), Eval("ItemType").ToString()) %>
                                        </div>
                                        <div class="listing-details">
                                            <div class="listing-title"><%# Eval("ItemName") %></div>
                                            <div class="listing-meta-row">
                                                <span class='type-chip <%# Eval("TypeBadgeClass") %>'>
                                                    <i class='<%# Eval("TypeIcon") %>'></i> <%# Eval("ItemType") %>
                                                </span>
                                                <span class="meta-dot">·</span>
                                                <span class="listing-date"><i class="fa-regular fa-calendar"></i> Added <%# Convert.ToDateTime(Eval("DateAdded")).ToString("dd MMM yyyy") %></span>
                                            </div>
                                        </div>
                                        <div class="listing-price-col">
                                            <div class="listing-val"><%# Eval("DisplayValue") %></div>
                                            <span class="stock-status-pill"><%# Eval("Status") %></span>
                                        </div>
                                        <div class="listing-actions">
                                            <a href='<%# Eval("EditUrl") %>' class="btn-icon-edit" title="Edit listing">
                                                <i class="fa-solid fa-pen-to-square"></i> Edit
                                            </a>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

            </div>

            <!-- SIDEBAR / RIGHT COLUMN -->
            <div class="dash-side-col">
                
                <!-- QUICK ACTIONS LAUNCHPAD -->
                <div class="dash-card">
                    <div class="dash-card-header">
                        <div class="header-left">
                            <div class="card-icon-circle action-circle"><i class="fa-solid fa-bolt"></i></div>
                            <div>
                                <h3>Quick Launchpad</h3>
                                <p>Direct shortcuts to farm tools</p>
                            </div>
                        </div>
                    </div>
                    <div class="dash-card-body">
                        <div class="quick-launch-grid">
                            <a href="CropForm.aspx" class="launch-item">
                                <div class="launch-icon bg-green"><i class="fa-solid fa-seedling"></i></div>
                                <div class="launch-text">
                                    <h4>Add New Crop</h4>
                                    <p>Post grains, fruits & vegetables</p>
                                </div>
                                <i class="fa-solid fa-chevron-right launch-arrow"></i>
                            </a>

                            <a href="EquipmentForm.aspx" class="launch-item">
                                <div class="launch-icon bg-orange"><i class="fa-solid fa-tractor"></i></div>
                                <div class="launch-text">
                                    <h4>Add Equipment</h4>
                                    <p>Sell or rent tractors & tools</p>
                                </div>
                                <i class="fa-solid fa-chevron-right launch-arrow"></i>
                            </a>

                            <a href="AnimalForm.aspx" class="launch-item">
                                <div class="launch-icon bg-brown"><i class="fa-solid fa-paw"></i></div>
                                <div class="launch-text">
                                    <h4>Add Livestock</h4>
                                    <p>Post cattle, poultry & dairy animals</p>
                                </div>
                                <i class="fa-solid fa-chevron-right launch-arrow"></i>
                            </a>

                            <a href="Marketplace.aspx" class="launch-item">
                                <div class="launch-icon bg-blue"><i class="fa-solid fa-store"></i></div>
                                <div class="launch-text">
                                    <h4>Browse Marketplace</h4>
                                    <p>Discover goods from other farmers</p>
                                </div>
                                <i class="fa-solid fa-chevron-right launch-arrow"></i>
                            </a>

                            <a href="OrderRequest.aspx" class="launch-item">
                                <div class="launch-icon bg-purple"><i class="fa-solid fa-file-invoice"></i></div>
                                <div class="launch-text">
                                    <h4>Order Requests</h4>
                                    <p>Review incoming orders & sales</p>
                                </div>
                                <i class="fa-solid fa-chevron-right launch-arrow"></i>
                            </a>

                            <a href="CategoryManagement.aspx" class="launch-item">
                                <div class="launch-icon bg-teal"><i class="fa-solid fa-tags"></i></div>
                                <div class="launch-text">
                                    <h4>Crop Categories</h4>
                                    <p>Manage your custom categories</p>
                                </div>
                                <i class="fa-solid fa-chevron-right launch-arrow"></i>
                            </a>
                        </div>
                    </div>
                </div>

                <!-- SMART ADVISORY CARD -->
                <div class="dash-card advisory-card">
                    <div class="dash-card-header">
                        <div class="header-left">
                            <div class="card-icon-circle tip-circle"><i class="fa-solid fa-lightbulb"></i></div>
                            <div>
                                <h3>Farmer Advisory</h3>
                                <p>Tips to boost buyer inquiries</p>
                            </div>
                        </div>
                    </div>
                    <div class="dash-card-body">
                        <div class="tip-item">
                            <div class="tip-icon"><i class="fa-solid fa-camera"></i></div>
                            <div class="tip-content">
                                <h5>Clear Pictures</h5>
                                <p>Listings with clear daylight photos receive up to <strong>3x more buyer inquiries</strong>.</p>
                            </div>
                        </div>
                        <div class="tip-item">
                            <div class="tip-icon"><i class="fa-solid fa-stopwatch"></i></div>
                            <div class="tip-content">
                                <h5>Fast Response</h5>
                                <p>Responding to pending buy requests within <strong>12 hours</strong> significantly increases deal conversion.</p>
                            </div>
                        </div>
                        <div class="tip-item">
                            <div class="tip-icon"><i class="fa-solid fa-scale-balanced"></i></div>
                            <div class="tip-content">
                                <h5>Fair Pricing</h5>
                                <p>Check current prices in the Marketplace to ensure competitive and profitable pricing.</p>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- NEED SUPPORT CARD -->
                <div class="support-card-banner">
                    <div class="support-content">
                        <div class="support-icon"><i class="fa-solid fa-headset"></i></div>
                        <div>
                            <h4>Need Marketplace Help?</h4>
                            <p>Have questions about orders, payments, or listing approval? We're here for you.</p>
                        </div>
                    </div>
                    <a href="CategoryManagement.aspx" class="btn-support">AgriTech Support Desk</a>
                </div>

            </div>

        </div>

        <div class="dash-footer-note">
            <span>🌾 AgriTech Smart Farmer Portal · Empowering Agriculture with Technology</span>
        </div>

    </div>
</asp:Content>
