<%@ Page Title="Register - AgriTech" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" 
    AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.Register" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Register page ma master page no sidebar hide karvo */
        .sidebar, .main-content .topbar { display: none !important; }
        .main-content { margin-left: 0 !important; padding: 0 !important; }
    </style>
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="MainContent" runat="server">
<div class="auth-body" style="min-height:100vh;">
    <div class="auth-card">
        <div class="auth-logo">
            <h1>AgriTech</h1>
            <span class="role-badge">ROLE: Registering as Farmer</span>
        </div>

        <asp:ValidationSummary ID="vsRegister" runat="server" 
            CssClass="error-msg" HeaderText="Please fix below errors:"
            DisplayMode="BulletList" ForeColor="#d32f2f" />

        <!-- Full Name -->
        <div class="form-group">
            <label>Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Enter your name" />
            <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                ControlToValidate="txtFullName" ErrorMessage="Full Name is required"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <!-- Email -->
        <div class="form-group">
            <label>Email Address</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="name@email.com" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="Email is required"
                CssClass="error-msg" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                ControlToValidate="txtEmail"
                ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,}$"
                ErrorMessage="Enter valid email (e.g. name@email.com)"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <!-- Mobile -->
        <div class="form-group">
            <label>Mobile Number</label>
            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="+91 10-digit number" />
            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" 
                ControlToValidate="txtMobile" ErrorMessage="Mobile Number is required"
                CssClass="error-msg" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revMobile" runat="server"
                ControlToValidate="txtMobile"
                ValidationExpression="\d{10}$"
                ErrorMessage="Enter valid 10-digit Indian mobile number"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <!-- Village -->
        <div class="form-group">
            <label>Village / Address</label>
            <asp:TextBox ID="txtVillage" runat="server" CssClass="form-control" 
                TextMode="MultiLine" Rows="2" placeholder="Enter your full address" />
            <asp:RequiredFieldValidator ID="rfvVillage" runat="server" 
                ControlToValidate="txtVillage" ErrorMessage="Village/Address is required"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <!-- Password -->
        <div class="form-group">
            <label>Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
            <asp:RequiredFieldValidator ID="rfvPass" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="Password is required"
                CssClass="error-msg" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revPass" runat="server"
                ControlToValidate="txtPassword"
                ValidationExpression="^.{6,}$"
                ErrorMessage="Password must be at least 6 characters"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <!-- Confirm Password -->
        <div class="form-group">
            <label>Confirm Password</label>
            <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" />
            <asp:RequiredFieldValidator ID="rfvConfirm" runat="server" 
                ControlToValidate="txtConfirm" ErrorMessage="Confirm Password is required"
                CssClass="error-msg" Display="Dynamic" />
            <asp:CompareValidator ID="cvPass" runat="server"
                ControlToValidate="txtConfirm" ControlToCompare="txtPassword"
                ErrorMessage="Passwords do not match" CssClass="error-msg" Display="Dynamic" />
        </div>

        <asp:Button ID="btnRegister" runat="server" Text="Register Now" 
            CssClass="btn-primary" OnClick="btnRegister_Click" />

        <div class="auth-footer">
            Already have an account? <a href="Login.aspx">Login here</a>
        </div>
    </div>
</div>
</asp:Content>