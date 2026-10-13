<%@ Page Title="My Orders - Buyer" Language="C#" MasterPageFile="~/Buyer/Buyer.Master" AutoEventWireup="true" CodeBehind="MyOrders.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Buyer.MyOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- PAGE HEADER -->
    <div class="page-header">
        <div>
            <h1>My Orders & Purchase Requests</h1>
            <p>Track your submitted commodity, machinery, and livestock orders from farmers.</p>
        </div>
    </div>

    <!-- TABS -->
    <div class="order-tabs">
        <asp:LinkButton ID="tabAll" runat="server" CssClass="order-tab active"
            OnClick="Tab_Click" CommandArgument="All">
            All Orders <span class="count"><asp:Label ID="lblAllCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
        <asp:LinkButton ID="tabPending" runat="server" CssClass="order-tab"
            OnClick="Tab_Click" CommandArgument="Pending">
            Pending <span class="count"><asp:Label ID="lblPendingCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
        <asp:LinkButton ID="tabAccepted" runat="server" CssClass="order-tab"
            OnClick="Tab_Click" CommandArgument="Accepted">
            Confirmed <span class="count"><asp:Label ID="lblAcceptedCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
        <asp:LinkButton ID="tabCancelled" runat="server" CssClass="order-tab"
            OnClick="Tab_Click" CommandArgument="Cancelled">
            Cancelled <span class="count"><asp:Label ID="lblCancelledCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
    </div>

    <!-- EMPTY STATE -->
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
        <div class="empty-msg">
            <h3>No orders found in this category</h3>
            <p style="margin-top:8px;">Browse the marketplace to discover fresh crops, machinery, and farm livestock.</p>
            <div style="margin-top:16px;">
                <a href="Marketplace.aspx" class="hero-btn-primary" style="display:inline-block;">Explore Marketplace</a>
            </div>
        </div>
    </asp:Panel>

    <!-- ORDERS LIST REPEATER -->
    <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
        <ItemTemplate>
            <div class="order-card">
                <div class="order-thumb">
                    <%# Eval("MainImage") == DBNull.Value || Eval("MainImage").ToString() == "" 
                        ? GetIcon(Eval("ItemType").ToString())
                        : "<img src='" + ResolveUrl(Eval("MainImage").ToString()) + "' style='width:80px;height:80px;object-fit:cover;border-radius:10px;' alt='item' />" %>
                </div>

                <div class="order-info">
                    <h4><%# Eval("ItemName") %></h4>
                    <p>Order #ORD-<%# Eval("OrderID").ToString().PadLeft(5, '0') %> &bull;
                       <%# Eval("OrderDate", "{0:dd MMM yyyy}") %> &bull;
                       <strong><%# Eval("ItemType") %></strong></p>
                    <p><i class="fa-solid fa-user-tag" style="color:#15803d;"></i> Producer: <strong><%# Eval("SellerName") %></strong> &bull;
                       <i class="fa-solid fa-location-dot" style="color:#64748b;"></i> Location: <strong><%# Eval("SellerVillage") %></strong></p>
                    <p style="margin-top:6px;">
                        Offered Amount: <strong><i class="fa-solid fa-indian-rupee-sign" style="font-size:12px;"></i> <%# Eval("OfferedPrice", "{0:N0}") %></strong> &bull; 
                        <i class="fa-solid fa-phone" style="color:#64748b;"></i> Contact: <strong><%# Eval("ContactNumber") %></strong>
                    </p>
                </div>

                <div class="order-actions">
                    <span class='order-status status-<%# Eval("OrderStatus").ToString().ToLower() %>'>
                        <%# Eval("OrderStatus") %>
                    </span>

                    <asp:LinkButton ID="btnCancel" runat="server"
                        CssClass="btn-cancel-order"
                        CommandName="Cancel"
                        CommandArgument='<%# Eval("OrderID") %>'
                        Visible='<%# Eval("OrderStatus").ToString() == "Pending" %>'
                        OnClientClick="return confirm('Are you sure you want to cancel this order request?');">
                        Cancel Request
                    </asp:LinkButton>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
