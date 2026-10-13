<%@ Page Title="Order Request - AgriTech" Language="C#" MasterPageFile="~/Buyer/Buyer.Master" AutoEventWireup="true" CodeBehind="OrderRequest.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Buyer.OrderRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .order-request-grid {
            display: grid;
            grid-template-columns: 2fr 1fr;
            gap: 24px;
            margin-top: 15px;
        }
        @media (max-width: 900px) {
            .order-request-grid { grid-template-columns: 1fr; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- PAGE HEADER -->
    <div class="page-header">
        <div>
            <h1>Submit Order Request</h1>
            <p>Send a direct purchase offer or booking request to the farmer for crops, equipment, or livestock.</p>
        </div>
    </div>

    <!-- ALERT MESSAGE -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 20px;">
        <asp:Label ID="lblAlert" runat="server" />
    </asp:Panel>

    <div class="order-request-grid">
        <!-- LEFT: ORDER PROPOSAL FORM -->
        <div class="info-card">
            <h3 style="font-size:17px; margin-bottom:16px; color:#1e293b; border-bottom:1px solid #f1f5f9; padding-bottom:10px;">
                <i class="fa-solid fa-file-pen" style="color:#15803d; margin-right:6px;"></i> Proposal Details
            </h3>

            <!-- Item Type Selection -->
            <div class="form-group">
                <label>Category / Item Type *</label>
                <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlItemType_SelectedIndexChanged">
                    <asp:ListItem Text="Crop Produce" Value="Crop" />
                    <asp:ListItem Text="Farm Equipment / Machinery" Value="Equipment" />
                    <asp:ListItem Text="Farm Livestock / Animals" Value="Animal" />
                </asp:DropDownList>
            </div>

            <!-- Item / Commodity Selection -->
            <div class="form-group">
                <label>Select Item / Commodity *</label>
                <asp:DropDownList ID="ddlItems" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlItems_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvItems" runat="server"
                    ControlToValidate="ddlItems" InitialValue="" Display="Dynamic"
                    ErrorMessage="Please select an item" CssClass="error-msg" />
            </div>

            <!-- Pricing & Offer -->
            <div style="display:grid; grid-template-columns:1fr 1fr; gap:16px;">
                <div class="form-group">
                    <label>Asking / Listed Price (Rs.)</label>
                    <asp:TextBox ID="txtAskingPrice" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8fafc" />
                </div>
                <div class="form-group">
                    <label>Your Offered Price (Rs.) *</label>
                    <asp:TextBox ID="txtOfferPrice" runat="server" CssClass="form-control" placeholder="Enter offer amount" />
                    <asp:RequiredFieldValidator ID="rfvOfferPrice" runat="server"
                        ControlToValidate="txtOfferPrice" Display="Dynamic"
                        ErrorMessage="Offer price is required" CssClass="error-msg" />
                </div>
            </div>

            <!-- Buyer Contact Information -->
            <h3 style="font-size:17px; margin-top:20px; margin-bottom:16px; color:#1e293b; border-bottom:1px solid #f1f5f9; padding-bottom:10px;">
                <i class="fa-solid fa-user-check" style="color:#15803d; margin-right:6px;"></i> Buyer Information
            </h3>

            <div style="display:grid; grid-template-columns:1fr 1fr; gap:16px;">
                <div class="form-group">
                    <label>Full Name *</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Your full name" />
                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server"
                        ControlToValidate="txtFullName" Display="Dynamic"
                        ErrorMessage="Full name is required" CssClass="error-msg" />
                </div>
                <div class="form-group">
                    <label>Contact Phone Number *</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="10-digit number" />
                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server"
                        ControlToValidate="txtPhone" Display="Dynamic"
                        ErrorMessage="Phone number is required" CssClass="error-msg" />
                </div>
            </div>

            <div class="form-group">
                <label>Delivery / Warehouse Address *</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Destination address for logistics..." />
                <asp:RequiredFieldValidator ID="rfvAddress" runat="server"
                    ControlToValidate="txtAddress" Display="Dynamic"
                    ErrorMessage="Delivery address is required" CssClass="error-msg" />
            </div>

            <div class="form-group">
                <label>Order Requirements & Notes</label>
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Specify any delivery schedule, moisture limit, packaging preferences..." />
            </div>

            <div style="display:flex; gap:12px; margin-top:20px;">
                <asp:Button ID="btnSubmitOffer" runat="server" Text="Submit Order Request" CssClass="btn-primary" style="flex:2;" OnClick="btnSubmitOffer_Click" />
                <a href="Marketplace.aspx" class="btn-secondary" style="flex:1; text-align:center; padding:12px; text-decoration:none;">Cancel</a>
            </div>
        </div>

        <!-- RIGHT: SELLER & SUMMARY PREVIEW -->
        <div>
            <div class="info-card seller-info">
                <h3>Selected Item & Producer</h3>

                <div style="margin-top:16px; text-align:center;">
                    <asp:Image ID="imgItemPreview" runat="server" style="width:100%; max-height:160px; object-fit:cover; border-radius:10px; margin-bottom:12px; display:none;" />
                    <div style="font-size:17px; font-weight:700; color:#0f172a;">
                        <asp:Label ID="lblSelectedItemName" runat="server" Text="Select an item from the form" />
                    </div>
                    <div style="font-size:13px; color:#64748b; margin-top:4px;">
                        <asp:Label ID="lblItemTypeTag" runat="server" />
                    </div>
                </div>

                <div class="seller-row" style="margin-top:20px;">
                    <i class="fa-solid fa-user-tag" style="color:#15803d;"></i> &nbsp;Producer: &nbsp;<strong><asp:Label ID="lblProducerName" runat="server" Text="-" /></strong>
                </div>

                <div class="seller-row">
                    <i class="fa-solid fa-location-dot" style="color:#64748b;"></i> &nbsp;Location: &nbsp;<asp:Label ID="lblProducerVillage" runat="server" Text="-" />
                </div>

                <div class="seller-row">
                    <i class="fa-solid fa-phone" style="color:#64748b;"></i> &nbsp;Contact: &nbsp;<asp:Label ID="lblProducerMobile" runat="server" Text="-" />
                </div>

                <div class="deposit-note" style="margin-top:20px;">
                    <span><i class="fa-solid fa-shield-halved" style="color:#15803d;"></i></span>
                    <span>Your request will be sent to the farmer immediately. You will be notified once the farmer accepts your deal.</span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
