<%@ Page Title="Contact Support - Buyer" Language="C#" MasterPageFile="~/Buyer/Buyer.Master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Buyer.ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- PAGE HEADER -->
    <div class="page-header">
        <div>
            <h1>Contact Support & Admin</h1>
            <p>Need procurement support, contract inquiry, or platform assistance? Contact AgriTech admin directly.</p>
        </div>
    </div>

    <!-- ALERT MESSAGE -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 20px;">
        <asp:Label ID="lblAlert" runat="server" />
    </asp:Panel>

    <div class="contact-form-layout">
        <!-- LEFT: SEND QUERY FORM -->
        <div class="info-card">
            <h3 style="font-size:16px; margin-bottom:16px; color:#1e293b; border-bottom:1px solid #f1f5f9; padding-bottom:10px;">
                <i class="fa-solid fa-paper-plane" style="color:#15803d; margin-right:6px;"></i> Submit Support Request
            </h3>

            <div class="form-group">
                <label>Your Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8fafc" />
            </div>

            <div class="form-group">
                <label>Contact Mobile</label>
                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8fafc" />
            </div>

            <div class="form-group">
                <label>Subject / Topic *</label>
                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="e.g., Bulk trade query, delivery inquiry, invoice..." />
                <asp:RequiredFieldValidator ID="rfvSubject" runat="server"
                    ControlToValidate="txtSubject" ErrorMessage="Subject is required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label>Your Message / Query *</label>
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" 
                    placeholder="Provide details about your query or requirement..." />
                <asp:RequiredFieldValidator ID="rfvMessage" runat="server"
                    ControlToValidate="txtMessage" ErrorMessage="Message is required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <asp:Button ID="btnSendQuery" runat="server" Text="Send Message to Admin" 
                CssClass="btn-primary" OnClick="btnSendQuery_Click" />
        </div>

        <!-- RIGHT: PREVIOUS INQUIRIES & REPLIES -->
        <div class="info-card">
            <h3 style="font-size:16px; margin-bottom:16px; color:#1e293b; border-bottom:1px solid #f1f5f9; padding-bottom:10px;">
                <i class="fa-solid fa-clock-rotate-left" style="color:#15803d; margin-right:6px;"></i> My Past Inquiries & Admin Replies
            </h3>

            <asp:Panel ID="pnlNoQueries" runat="server" Visible="false">
                <div class="empty-msg" style="padding: 30px;">
                    <h3>No inquiries yet</h3>
                    <p style="margin-top:6px;">Your sent messages and admin replies will appear here.</p>
                </div>
            </asp:Panel>

            <asp:Repeater ID="rptMyQueries" runat="server">
                <ItemTemplate>
                    <div class="query-card">
                        <div class="query-header">
                            <span class="query-subject"><%# Eval("Subject") %></span>
                            <span class='status-badge-pill <%# Eval("Status").ToString() == "Pending" ? "status-pending" : "status-active" %>'>
                                <%# Eval("Status") %>
                            </span>
                        </div>
                        <div class="query-meta" style="margin-bottom:8px;">
                            Sent on: <%# Convert.ToDateTime(Eval("QueryDate")).ToString("dd MMM yyyy, hh:mm tt") %>
                        </div>
                        <div class="query-message">
                            <%# Eval("Message") %>
                        </div>

                        <!-- Admin Reply if available -->
                        <%# Eval("AdminReply") != DBNull.Value && !string.IsNullOrEmpty(Eval("AdminReply").ToString()) ? @"
                            <div class='admin-reply-box'>
                                <div class='reply-header'><i class='fa-solid fa-shield-halved'></i> AgriTech Admin Response:</div>
                                <div class='reply-content'>" + Eval("AdminReply") + @"</div>
                                <div style='font-size:11px; color:#166534; margin-top:4px;'>Replied: " + Convert.ToDateTime(Eval("ReplyDate")).ToString("dd MMM yyyy") + @"</div>
                            </div>" : @"
                            <div style='margin-top:10px; font-size:12px; color:#d97706;'>
                                <i class='fa-regular fa-clock'></i> Awaiting response from AgriTech Admin
                            </div>" %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
