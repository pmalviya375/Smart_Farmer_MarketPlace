<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="MarketplaceDetails.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.MarketplaceDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="breadcrumb">
        <a href="Marketplace.aspx">Marketplace</a> ›
        <asp:Label ID="lblBreadcrumbType" runat="server" Text="Machinery" /> ›
        <asp:Label ID="lblBreadcrumbName" runat="server" Text="Item" />
    </div>

    <div class="details-layout">

        <!-- ============ LEFT: GALLERY + SPECS + DESCRIPTION ============ -->
        <div>
            <!-- Gallery -->
            <div class="gallery-card">
                <div class="verified-badge">✅ Verified Listing</div>

                <asp:Image ID="imgMain" runat="server" CssClass="main-image"
                    AlternateText="Item photo" />

                <div class="thumb-strip">
                    <asp:Image ID="imgThumb1" runat="server" onclick="changeImage(this)"
                        AlternateText="Photo 1" />
                    <asp:Image ID="imgThumb2" runat="server" onclick="changeImage(this)"
                        AlternateText="Photo 2" />
                    <asp:Image ID="imgThumb3" runat="server" onclick="changeImage(this)"
                        AlternateText="Photo 3" />
                    <asp:Image ID="imgThumb4" runat="server" onclick="changeImage(this)"
                        AlternateText="Photo 4" />
                </div>
            </div>

            <!-- Specs -->
            <div class="info-card" style="margin-top:20px;">
                <h3 style="font-size:16px;margin-bottom:12px;">Key Specifications</h3>
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
                <h3 style="font-size:16px;margin-bottom:10px;">Description</h3>
                <p style="font-size:14px;color:#555;line-height:1.6;">
                    <asp:Label ID="lblDescription" runat="server" />
                </p>
            </div>
        </div>

        <!-- ============ RIGHT: PRICE + SELLER + CTA ============ -->
        <div>
            <div class="info-card">
                <h1><asp:Label ID="lblItemName" runat="server" /></h1>
                <div class="big-price">₹ <asp:Label ID="lblPrice" runat="server" /></div>

                <asp:LinkButton ID="btnRequestBuy" runat="server" CssClass="btn-order"
                    OnClick="btnRequestBuy_Click">
                    🛒 Request to Buy
                </asp:LinkButton>

                <asp:LinkButton ID="btnContactSeller" runat="server" CssClass="btn-secondary"
                    OnClick="btnContactSeller_Click">
                    Contact Seller
                </asp:LinkButton>

                <div class="deposit-note">
                    <span>ℹ️</span>
                    <span>Deposit of ₹500 required for confirmation.</span>
                </div>
            </div>

            <!-- Seller Info -->
            <div class="info-card seller-info">
                <h3>Seller Information</h3>

                <div style="display:flex;align-items:center;margin-top:12px;">
                    <div class="seller-avatar">
                        <asp:Label ID="lblSellerInitial" runat="server" />
                    </div>
                    <div>
                        <div style="font-weight:700;font-size:15px;">
                            <asp:Label ID="lblSellerName" runat="server" />
                        </div>
                        <div class="seller-rating">
                            ★ 4.8 (12 Reviews) · Trusted Seller
                        </div>
                    </div>
                </div>

                <div class="seller-row" style="margin-top:15px;">
                    📍 &nbsp;<asp:Label ID="lblSellerAddress" runat="server" />
                </div>

                <div class="seller-row">
                    ⏱ &nbsp;<asp:Label ID="lblSellerSince" runat="server" />
                </div>

                <a href="#" style="color:#2e7d32;font-weight:600;font-size:13px;text-decoration:none;display:block;margin-top:12px;">
                    View all listings from <asp:Label ID="lblSellerFirstName" runat="server" /> →
                </a>
            </div>
        </div>

    </div>

    <script>
        function changeImage(thumb) {
            var main = document.getElementById('<%= imgMain.ClientID %>');
            if (main && thumb.src) {
                main.src = thumb.src;
                // Visual active state
                var all = document.querySelectorAll('.thumb-strip img');
                all.forEach(function (t) { t.classList.remove('active'); });
                thumb.classList.add('active');
            }
        }
    </script>

</asp:Content>
