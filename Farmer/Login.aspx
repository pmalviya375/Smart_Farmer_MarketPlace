<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style>
        .sidebar, .main-content .topbar { display: none !important; }
        .main-content { margin-left: 0 !important; padding: 0 !important; }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-body" style="min-height:100vh;">
    <div class="auth-card">
        <div class="auth-logo">
            <h1>Welcome Back</h1>
            <p>Access your farming dashboard</p>
        </div>

        <div class="form-group">
            <label>Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="name@email.com" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                ControlToValidate="txtEmail" ErrorMessage="Email required"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label>Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
            <asp:RequiredFieldValidator ID="rfvPass" runat="server"
                ControlToValidate="txtPassword" ErrorMessage="Password required"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <div style="display:flex;justify-content:space-between;font-size:13px;margin-bottom:18px;">
            <label><asp:CheckBox ID="chkRemember" runat="server" /> Remember me</label>
            <a href="#" style="color:#2e7d32;text-decoration:none;">Forgot Password?</a>
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Login to Dashboard" 
            CssClass="btn-primary" OnClick="btnLogin_Click" />

        <div class="auth-footer">
            Don't have an account? <a href="Register.aspx">Register Now</a>
        </div>
    </div>
</div>
</asp:Content>
