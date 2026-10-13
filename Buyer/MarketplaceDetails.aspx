<%@ Page Title="Item Details - Marketplace" Language="C#" MasterPageFile="~/Buyer/Buyer.Master" AutoEventWireup="true" CodeBehind="MarketplaceDetails.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Buyer.MarketplaceDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Breadcrumb -->
    <div class="breadcrumb">
        <a href="Marketplace.aspx">Marketplace</a> ›
        <asp:Label ID="lblBreadcrumbType" runat="server" Text="Item" /> ›
        <asp:Label ID="lblBreadcrumbName" runat="server" Text="Details" />
    </div>

    <div class="details-layout">

        <!-- ============ LEFT: GALLERY + SPECS + DESCRIPTION ============ -->
        <div>
            <!-- Gallery -->
            <div class="gallery-card">
                <div class="verified-badge"><i class="fa-solid fa-circle-check"></i> Verified Producer Listing</div>

                <asp:Image ID="imgMain" runat="server" CssClass="main-image" AlternateText="Item photo" />

                <div class="thumb-strip">
                    <asp:Image ID="imgThumb1" runat="server" onclick="changeImage(this)" AlternateText="Photo 1" />
                    <asp:Image ID="imgThumb2" runat="server" onclick="changeImage(this)" AlternateText="Photo 2" />
                    <asp:Image ID="imgThumb3" runat="server" onclick="changeImage(this)" AlternateText="Photo 3" />
                    <asp:Image ID="imgThumb4" runat="server" onclick="changeImage(this)" AlternateText="Photo 4" />
                </div>
            </div>

            <!-- Specifications -->
            <div class="info-card" style="margin-top:20px;">
                <h3 style="font-size:16px; margin-bottom:12px;">Key Specifications</h3>
                <div class="specs-grid">
                    <asp:Repeater ID="rptSpecs" runat="server">
                        <ItemTemplate>
                            <div class="spec-box">
                                <div class="icon"><%# Eval("Icon") %></div>
                                <div class="label"><%# Eval("Label") %></div>
                                <div class="value"><%# Eval("Value") %></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <!-- Description -->
            <div class="info-card" style="margin-top:20px;">
                <h3 style="font-size:16px; margin-bottom:10px;">Item Description</h3>
                <p style="font-size:14px; color:#555; line-height:1.6;">
                    <asp:Label ID="lblDescription" runat="server" />
                </p>
            </div>
        </div>

        <!-- ============ RIGHT: PRICE + SELLER + BUY CTA ============ -->
        <div>
            <div class="info-card">
                <div style="display:flex; justify-content:space-between; align-items:flex-start;">
                    <h1><asp:Label ID="lblItemName" runat="server" /></h1>
                    <span class="role-badge" style="margin-bottom:0;"><asp:Label ID="lblTypeBadge" runat="server" Text="Item" /></span>
                </div>
                
                <div class="big-price">
                    <i class="fa-solid fa-indian-rupee-sign" style="font-size:22px;"></i> <asp:Label ID="lblPrice" runat="server" />
                    <small style="font-size:14px; color:#64748b; font-weight:normal;">
                        <asp:Label ID="lblPriceUnit" runat="server" />
                    </small>
                </div>

                <asp:LinkButton ID="btnRequestBuy" runat="server" CssClass="btn-order"
                    OnClick="btnRequestBuy_Click">
                    <i class="fa-solid fa-cart-shopping"></i> Request to Buy / Order
                </asp:LinkButton>

                <asp:LinkButton ID="btnContactSeller" runat="server" CssClass="btn-secondary"
                    OnClick="btnContactSeller_Click">
                    <i class="fa-solid fa-phone"></i> Contact Farmer
                </asp:LinkButton>

                <div class="deposit-note">
                    <span><i class="fa-solid fa-shield-halved" style="color:#15803d;"></i></span>
                    <span>Direct farmer procurement. Quality verified & protected through AgriTech.</span>
                </div>
            </div>

            <!-- Seller Information Card -->
            <div class="info-card seller-info">
                <h3>Farmer / Producer Information</h3>

                <div style="display:flex; align-items:center; margin-top:12px;">
                    <div class="seller-avatar">
                        <asp:Label ID="lblSellerInitial" runat="server" />
                    </div>
                    <div>
                        <div style="font-weight:700; font-size:15px;">
                            <asp:Label ID="lblSellerName" runat="server" />
                        </div>
                        <div class="seller-rating">
                            <i class="fa-solid fa-star" style="color:#eab308; font-size:11px;"></i> 4.9 &bull; Verified AgriTech Farmer
                        </div>
                    </div>
                </div>

                <div class="seller-row" style="margin-top:15px;">
                    <i class="fa-solid fa-location-dot" style="color:#64748b;"></i> &nbsp;<asp:Label ID="lblSellerAddress" runat="server" />
                </div>

                <div class="seller-row">
                    <i class="fa-regular fa-clock" style="color:#64748b;"></i> &nbsp;<asp:Label ID="lblSellerSince" runat="server" />
                </div>

                <a href="Marketplace.aspx" style="color:#15803d; font-weight:600; font-size:13px; text-decoration:none; display:block; margin-top:12px;">
                    Explore more listings from local farmers →
                </a>
            </div>
        </div>

    </div>

    <script type="text/javascript">
        function changeImage(thumb) {
            var main = document.getElementById('<%= imgMain.ClientID %>');
            if (main && thumb.src) {
                main.src = thumb.src;
                var all = document.querySelectorAll('.thumb-strip img');
                all.forEach(function (t) { t.classList.remove('active'); });
                thumb.classList.add('active');
            }
        }
    </script>
</asp:Content>
