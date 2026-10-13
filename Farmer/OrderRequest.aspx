<%@ Page Title="Order Requests" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="OrderRequest.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.OrderRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1>Order Requests</h1>
            <p>Manage incoming buyer requests and your outgoing orders.</p>
        </div>
    </div>

    <div class="order-tabs">
        <asp:LinkButton ID="tabIncoming" runat="server" CssClass="order-tab active"
            OnClick="Tab_Click" CommandArgument="Incoming">
            Incoming Requests <span class="count"><asp:Label ID="lblIncomingCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
        <asp:LinkButton ID="tabOutgoing" runat="server" CssClass="order-tab"
            OnClick="Tab_Click" CommandArgument="Outgoing">
            My Orders <span class="count"><asp:Label ID="lblOutgoingCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
    </div>

    <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
        <div class="empty-msg">
            <h3>No orders here</h3>
            <p style="margin-top:8px;">Orders will appear here once they are placed.</p>
        </div>
    </asp:Panel>

    <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand" OnItemDataBound="rptOrders_ItemDataBound">
        <ItemTemplate>
            <div class="order-card">
                <div class="order-thumb">
                    <%# Eval("MainImage") == DBNull.Value || Eval("MainImage").ToString() == "" 
                        ? GetIcon(Eval("ItemType").ToString())
                        : "<img src='" + ResolveUrl(Eval("MainImage").ToString()) + "' style='width:80px;height:80px;object-fit:cover;border-radius:10px;' alt='item' />" %>
                </div>

                <div class="order-info">
                    <h4><%# Eval("ItemName") %></h4>
                    <p>Order #ORD-<%# Eval("OrderID").ToString().PadLeft(5, '0') %> ·
                       <%# Eval("OrderDate", "{0:dd MMM yyyy}") %> ·
                       <%# Eval("ItemType") %></p>
                    <p>Buyer: <strong><%# Eval("BuyerName") %></strong> ·
                       Seller: <strong><%# Eval("SellerName") %></strong></p>
                    <p style="margin-top:6px;">Contact: <strong><%# Eval("ContactNumber") %></strong></p>
                </div>

                <div class="order-actions">
                    <span class='order-status status-<%# Eval("OrderStatus").ToString().ToLower() %>'>
                        <%# Eval("OrderStatus") %>
                    </span>

                    <asp:LinkButton ID="btnAccept" runat="server"
                        CssClass="btn-accept"
                        CommandName="Accept"
                        CommandArgument='<%# Eval("OrderID") %>'
                        Visible="false">
                        Accept
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnCancel" runat="server"
                        CssClass="btn-cancel-order"
                        CommandName="Cancel"
                        CommandArgument='<%# Eval("OrderID") %>'
                        Visible="false"
                        OnClientClick="return confirm('Cancel this order?');">
                        Cancel
                    </asp:LinkButton>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
