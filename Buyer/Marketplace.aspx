<%@ Page Title="Agri Marketplace - Buyer" Language="C#" MasterPageFile="~/Buyer/Buyer.Master" AutoEventWireup="true" CodeBehind="Marketplace.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Buyer.Marketplace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- PAGE HEADER -->
    <div class="page-header">
        <div>
            <h1>Agri Marketplace</h1>
            <p>Source crops, farm machinery, and livestock directly from verified farmers across regions.</p>
        </div>
    </div>

    <!-- Category Chips + Search -->
    <div class="market-toolbar">
        <div class="category-chips">
            <asp:LinkButton ID="chipAll" runat="server" CssClass="chip active"
                OnClick="FilterChip_Click" CommandArgument="All">All Categories</asp:LinkButton>
            <asp:LinkButton ID="chipCrop" runat="server" CssClass="chip"
                OnClick="FilterChip_Click" CommandArgument="Crop">
                <i class="fa-solid fa-seedling"></i> Crops
            </asp:LinkButton>
            <asp:LinkButton ID="chipEquip" runat="server" CssClass="chip"
                OnClick="FilterChip_Click" CommandArgument="Equipment">
                <i class="fa-solid fa-tractor"></i> Equipment
            </asp:LinkButton>
            <asp:LinkButton ID="chipAnimal" runat="server" CssClass="chip"
                OnClick="FilterChip_Click" CommandArgument="Animal">
                <i class="fa-solid fa-cow"></i> Livestock
            </asp:LinkButton>
        </div>

        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input"
            placeholder="Search crops, machinery, animals..." AutoPostBack="true"
            OnTextChanged="txtSearch_TextChanged" />
    </div>

    <!-- Empty state -->
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
        <div class="empty-msg">
            <h3><i class="fa-solid fa-seedling"></i> No listings found</h3>
            <p style="margin-top:8px;">Try different keywords or select another category above.</p>
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

                            <div class="card-heart" title="Add to wishlist"><i class="fa-regular fa-heart"></i></div>
                        </div>

                        <div class="market-card-body">
                            <div class="card-title">
                                <span><%# Eval("ItemName") %></span>
                                <span class="card-rating"><i class="fa-solid fa-star" style="color:#eab308; font-size:11px;"></i> <%# GetRating(Eval("ItemID")) %></span>
                            </div>

                            <div class="card-location">
                                <i class="fa-solid fa-location-dot" style="color:#64748b; font-size:11px;"></i> <%# Eval("SellerVillage") %> &bull; <span style="color:#64748b; font-size:12px;"><%# Eval("ItemType") %></span>
                            </div>

                            <div class="card-price-row">
                                <div class="card-price">
                                    <i class="fa-solid fa-indian-rupee-sign" style="font-size:13px;"></i> <%# Eval("Price", "{0:N0}") %>
                                    <%# GetPriceUnit(Eval("ItemType").ToString()) %>
                                </div>
                                <div class="card-seller">
                                    PRODUCER
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
    </asp:Panel>
</asp:Content>
