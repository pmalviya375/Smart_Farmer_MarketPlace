<%@ Page Title="Register as Buyer - AgriTech" Language="C#" MasterPageFile="~/Buyer/Buyer.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Buyer.Register" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="server">
    <style>
        .sidebar, .main-content .topbar { display: none !important; }
        .main-content { margin-left: 0 !important; padding: 0 !important; }
    </style>
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-body" style="min-height: 100vh;">
        <div class="auth-card">
            <div class="auth-logo">
                <h1>AgriTech</h1>
                <p>Agricultural Procurement Platform</p>
                <span class="role-badge" style="margin-top: 8px;">ROLE: Registering as Buyer</span>
            </div>

            <!-- ALERT MESSAGE -->
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box">
                <asp:Label ID="lblAlert" runat="server" />
            </asp:Panel>

            <asp:ValidationSummary ID="vsRegister" runat="server" 
                CssClass="error-msg" HeaderText="Please correct the following errors:"
                DisplayMode="BulletList" ForeColor="#d32f2f" />

            <!-- Full Name -->
            <div class="form-group">
                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Enter full name" />
                <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                    ControlToValidate="txtFullName" ErrorMessage="Full Name is required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <!-- Email Address -->
            <div class="form-group">
                <label>Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="buyer@example.com" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail" ErrorMessage="Email is required"
                    CssClass="error-msg" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,}$"
                    ErrorMessage="Enter a valid email address"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <!-- Mobile Number -->
            <div class="form-group">
                <label>Mobile Number</label>
                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="10-digit mobile number" />
                <asp:RequiredFieldValidator ID="rfvMobile" runat="server" 
                    ControlToValidate="txtMobile" ErrorMessage="Mobile Number is required"
                    CssClass="error-msg" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revMobile" runat="server"
                    ControlToValidate="txtMobile"
                    ValidationExpression="^\d{10}$"
                    ErrorMessage="Enter a valid 10-digit mobile number"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <!-- Shipping / Warehouse Address -->
            <div class="form-group">
                <label>Delivery / Warehouse Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" 
                    TextMode="MultiLine" Rows="2" placeholder="Full address or delivery location" />
                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" 
                    ControlToValidate="txtAddress" ErrorMessage="Address is required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <!-- Password -->
            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Minimum 6 characters" />
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
                <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" placeholder="Re-enter password" />
                <asp:RequiredFieldValidator ID="rfvConfirm" runat="server" 
                    ControlToValidate="txtConfirm" ErrorMessage="Confirm Password is required"
                    CssClass="error-msg" Display="Dynamic" />
                <asp:CompareValidator ID="cvPass" runat="server"
                    ControlToValidate="txtConfirm" ControlToCompare="txtPassword"
                    ErrorMessage="Passwords do not match" CssClass="error-msg" Display="Dynamic" />
            </div>

            <asp:Button ID="btnRegister" runat="server" Text="Register as Buyer" 
                CssClass="btn-primary" OnClick="btnRegister_Click" />

            <div class="auth-footer">
                Already have an account? <a href="Login.aspx">Login Here</a>
            </div>
        </div>
    </div>
</asp:Content>
