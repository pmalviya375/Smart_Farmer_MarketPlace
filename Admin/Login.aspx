<%@ Page Title="AgriTech Admin - Login" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Admin.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .sidebar, .main-content .topbar { display: none !important; }
        .main-content { margin-left: 0 !important; padding: 0 !important; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-auth-wrapper">
        <div class="admin-login-card">
            
            <!-- BRAND / LOGO (MATCHING SCREENSHOT 1) -->
            <div class="admin-login-logo">
                <i class="fa-solid fa-leaf admin-leaf-icon"></i>
                <h1>AgriTech Admin</h1>
                <p>Secure system access</p>
            </div>

            <!-- ALERT MESSAGE -->
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 20px;">
                <asp:Label ID="lblAlert" runat="server" />
            </asp:Panel>

            <!-- EMAIL ADDRESS -->
            <div class="admin-field-group">
                <label>Email Address</label>
                <div class="admin-input-box">
                    <i class="fa-regular fa-envelope input-icon-lead"></i>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                        placeholder="admin@agritech.com" TextMode="Email" />
                </div>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail" ErrorMessage="Email is required" 
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <!-- PASSWORD -->
            <div class="admin-field-group">
                <div class="admin-field-header">
                    <label>Password</label>
                    <a href="#" class="admin-forgot-link" onclick="alert('Default credentials: admin@gmail.com / admin@123\nContact Super Admin to reset credentials.'); return false;">Forgot?</a>
                </div>
                <div class="admin-input-box">
                    <i class="fa-solid fa-lock input-icon-lead"></i>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" 
                        TextMode="Password" placeholder="••••••••" />
                    <i class="fa-regular fa-eye input-icon-trail" onclick="togglePassVisibility()"></i>
                </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword" ErrorMessage="Password is required" 
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <!-- SUBMIT BUTTON -->
            <asp:Button ID="btnLogin" runat="server" Text="Login →" 
                CssClass="admin-btn-primary" OnClick="btnLogin_Click" />

            <div style="margin-top: 20px; text-align: center; font-size: 12.5px; color: #64748b;">
                Default Credentials: <strong>admin@gmail.com</strong> / <strong>admin@123</strong>
            </div>

        </div>
    </div>

    <script type="text/javascript">
        function togglePassVisibility() {
            var passInput = document.getElementById('<%= txtPassword.ClientID %>');
            if (passInput) {
                if (passInput.type === "password") {
                    passInput.type = "text";
                } else {
                    passInput.type = "password";
                }
            }
        }
    </script>
</asp:Content>
