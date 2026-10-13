<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="Marketplace.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.Marketplace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="page-header">
        <div>
            <h1>Farmer Marketplace</h1>
            <p>Connect farmer to farmer — buy and sell crops, equipment, and animals.</p>
        </div>
    </div>

    <!-- Category Chips + Search -->
    <div class="market-toolbar">
        <div class="category-chips">
            <asp:LinkButton ID="chipAll" runat="server" CssClass="chip active"
                OnClick="FilterChip_Click" CommandArgument="All">All Categories</asp:LinkButton>
            <asp:LinkButton ID="chipEquip" runat="server" CssClass="chip"
                OnClick="FilterChip_Click" CommandArgument="Equipment">Equipment</asp:LinkButton>
            <asp:LinkButton ID="chipAnimal" runat="server" CssClass="chip"
                OnClick="FilterChip_Click" CommandArgument="Animal">Animals</asp:LinkButton>
            <asp:LinkButton ID="chipCrop" runat="server" CssClass="chip"
                OnClick="FilterChip_Click" CommandArgument="Crop">Crops</asp:LinkButton>
        </div>

        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input"
            placeholder="🔍 Search by name" AutoPostBack="true"
            OnTextChanged="txtSearch_TextChanged" />
    </div>

    <!-- Empty state -->
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
        <div class="empty-msg">
            <h3>No items found 🌾</h3>
            <p style="margin-top:8px;">Try different keywords or category.</p>
        </div>
    </asp:Panel>

    <!-- Grid -->
    <asp:Panel ID="pnlGrid" runat="server">
        <div class="market-grid">
            <asp:Repeater ID="rptMarketplace" runat="server">
                <ItemTemplate>
                    <div class="market-card">
                        <div class="market-card-img">
                            <%# Eval("MainImage") == DBNull.Value || Eval("MainImage").ToString() == "" 
                                ? "<div class='no-img'>" + GetIcon(Eval("ItemType").ToString()) + "</div>"
                                : "<img src='" + ResolveUrl(Eval("MainImage").ToString()) + "' alt='item' />" %>

                            <span class='card-badge <%# GetBadgeClass(Eval("ItemType").ToString()) %>'>
                                <%# GetBadgeText(Eval("ItemType").ToString(), Eval("Status").ToString()) %>
                            </span>

                            <div class="card-heart" title="Add to wishlist">♡</div>
                        </div>

                        <div class="market-card-body">
                            <div class="card-title">
                                <span><%# Eval("ItemName") %></span>
                                <span class="card-rating">★ <%# GetRating(Eval("ItemID")) %></span>
                            </div>

                            <div class="card-location">
                                📍 <%# Eval("SellerVillage") %>
                            </div>

                            <div class="card-price-row">
                                <div class="card-price">
                                    ₹ <%# Eval("Price", "{0:N0}") %>
                                    <%# GetPriceUnit(Eval("ItemType").ToString()) %>
                                </div>
                                <div class="card-seller">
                                    SELLER
                                    <strong><%# Eval("SellerName") %></strong>
                                </div>
                            </div>

                            <a href='MarketplaceDetails.aspx?type=<%# Eval("ItemType") %>&id=<%# Eval("ItemID") %>'
                               class='btn-view <%# GetButtonStyle(Eval("ItemType").ToString()) %>'>
                                View Details
                            </a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="load-more-wrap">
            <asp:Button ID="btnLoadMore" runat="server" Text="Load More Items ⌄"
                CssClass="btn-load-more" OnClick="btnLoadMore_Click" />
        </div>
    </asp:Panel>
</asp:Content>
