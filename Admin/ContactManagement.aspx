<%@ Page Title="Contact Management - AgriTech Admin" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ContactManagement.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Admin.ContactManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- PAGE HEADER -->
    <div class="page-header">
        <div>
            <h1>Contact Inquiries Management</h1>
            <p>Manage, review, and reply directly to support queries and questions from farmers and buyers.</p>
        </div>
    </div>

    <!-- ALERT MESSAGE -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 16px;">
        <asp:Label ID="lblAlert" runat="server" />
    </asp:Panel>

    <!-- TABS -->
    <div class="order-tabs">
        <asp:LinkButton ID="tabAll" runat="server" CssClass="order-tab active"
            OnClick="Tab_Click" CommandArgument="All">
            All Inquiries <span class="count"><asp:Label ID="lblAllCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
        <asp:LinkButton ID="tabPending" runat="server" CssClass="order-tab"
            OnClick="Tab_Click" CommandArgument="Pending">
            Pending <span class="count"><asp:Label ID="lblPendingCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
        <asp:LinkButton ID="tabReplied" runat="server" CssClass="order-tab"
            OnClick="Tab_Click" CommandArgument="Replied">
            Replied <span class="count"><asp:Label ID="lblRepliedCount" runat="server" Text="0" /></span>
        </asp:LinkButton>
    </div>

    <!-- EMPTY STATE -->
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
        <div class="empty-msg" style="padding: 40px;">
            <h3>No inquiries found</h3>
            <p style="margin-top:8px;">No queries match the selected tab filter.</p>
        </div>
    </asp:Panel>

    <!-- QUERIES LIST -->
    <asp:Repeater ID="rptQueries" runat="server" OnItemCommand="rptQueries_ItemCommand">
        <ItemTemplate>
            <div class="info-card" style="margin-bottom:18px;">
                <div style="display:flex; justify-content:space-between; align-items:flex-start; margin-bottom:12px; border-bottom:1px solid #f1f5f9; padding-bottom:10px;">
                    <div>
                        <div style="display:flex; align-items:center; gap:10px;">
                            <span style="font-weight:700; font-size:15px; color:#0f172a;"><%# Eval("SenderName") %></span>
                            <span class='status-badge-pill <%# Eval("SenderRole").ToString() == "Farmer" ? "status-active" : "status-pending" %>' style="font-size:11px;">
                                <%# Eval("SenderRole") %>
                            </span>
                        </div>
                        <div style="font-size:12px; color:#64748b; margin-top:3px;">
                            <i class="fa-solid fa-phone" style="font-size:11px;"></i> <%# Eval("SenderMobile") %> &bull; 
                            <i class="fa-regular fa-envelope" style="font-size:11px;"></i> <%# Eval("SenderEmail") %> &bull;
                            <i class="fa-regular fa-clock" style="font-size:11px;"></i> Received: <%# Convert.ToDateTime(Eval("QueryDate")).ToString("dd MMM yyyy, hh:mm tt") %>
                        </div>
                    </div>

                    <span class='status-badge-pill <%# Eval("Status").ToString() == "Pending" ? "status-blocked" : "status-active" %>'>
                        <%# Eval("Status") %>
                    </span>
                </div>

                <div style="font-weight:700; font-size:14px; color:#1b6d34; margin-bottom:6px;">
                    Subject: <%# Eval("Subject") %>
                </div>

                <div style="font-size:13.5px; color:#334155; line-height:1.6; background:#f8fafc; padding:12px 14px; border-radius:8px; margin-bottom:14px;">
                    <%# Eval("Message") %>
                </div>

                <!-- If already replied, show reply -->
                <%# Eval("Status").ToString() == "Replied" ? @"
                    <div class='admin-reply-box'>
                        <div class='reply-header'><i class='fa-solid fa-shield-halved'></i> Admin Reply Sent on " + Convert.ToDateTime(Eval("ReplyDate")).ToString("dd MMM yyyy, hh:mm tt") + @":</div>
                        <div class='reply-content'>" + Eval("AdminReply") + @"</div>
                    </div>" : "" %>

                <!-- Reply Form for Pending Queries -->
                <asp:Panel ID="pnlReplyBox" runat="server" Visible='<%# Eval("Status").ToString() == "Pending" %>'>
                    <div style="margin-top:12px; border-top:1px solid #f1f5f9; padding-top:12px;">
                        <label style="font-size:12.5px; font-weight:700; color:#1e293b; display:block; margin-bottom:6px;">
                            Write Admin Response to <%# Eval("SenderName") %>:
                        </label>
                        <asp:TextBox ID="txtReplyMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" 
                            placeholder="Type your reply message here..." style="margin-bottom:10px;" />
                        <div style="text-align:right;">
                            <asp:Button ID="btnSendReply" runat="server" Text="Send Reply to User" CssClass="btn-primary" 
                                style="width:auto; padding:8px 20px; font-size:13px;"
                                CommandName="SendReply" CommandArgument='<%# Eval("QueryID") %>' />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
