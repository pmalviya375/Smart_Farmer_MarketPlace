<%@ Page Title="Admin Profile & Security - AgriTech" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Admin.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- PAGE HEADER -->
    <div class="page-header">
        <div>
            <h1>Admin Profile & Security</h1>
            <p>Manage your administrator account credentials, contact information, and password security.</p>
        </div>
    </div>

    <!-- ALERT MESSAGE -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="auth-alert-box" style="margin-bottom: 20px;">
        <asp:Label ID="lblAlert" runat="server" />
    </asp:Panel>

    <div style="display:grid; grid-template-columns: 1fr 1fr; gap:24px;">
        
        <!-- LEFT: EDIT PROFILE DETAILS -->
        <div class="info-card">
            <h3 style="font-size:16px; margin-bottom:16px; color:#0f172a; border-bottom:1px solid #f1f5f9; padding-bottom:10px;">
                <i class="fa-regular fa-id-card" style="color:#1b6d34; margin-right:6px;"></i> Administrator Details
            </h3>

            <div class="form-group">
                <label>Admin Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8fafc" />
                <span style="font-size:11.5px; color:#64748b;">Primary system login username</span>
            </div>

            <div class="form-group">
                <label>Full Name *</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Administrator name" />
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server"
                    ControlToValidate="txtFullName" ErrorMessage="Full Name is required"
                    CssClass="error-msg" Display="Dynamic" ValidationGroup="vgProfile" />
            </div>

            <div class="form-group">
                <label>Contact Mobile Number</label>
                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="10-digit number" />
            </div>

            <asp:Button ID="btnUpdateProfile" runat="server" Text="Save Profile Changes" 
                CssClass="btn-primary" OnClick="btnUpdateProfile_Click" ValidationGroup="vgProfile" />
        </div>

        <!-- RIGHT: CHANGE PASSWORD FORM -->
        <div class="info-card">
            <h3 style="font-size:16px; margin-bottom:16px; color:#0f172a; border-bottom:1px solid #f1f5f9; padding-bottom:10px;">
                <i class="fa-solid fa-lock" style="color:#1b6d34; margin-right:6px;"></i> Change Password
            </h3>

            <div class="form-group">
                <label>Current / Old Password *</label>
                <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter current password" />
                <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server"
                    ControlToValidate="txtOldPassword" ErrorMessage="Current password is required"
                    CssClass="error-msg" Display="Dynamic" ValidationGroup="vgPassword" />
            </div>

            <div class="form-group">
                <label>New Password *</label>
                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Minimum 6 characters" />
                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server"
                    ControlToValidate="txtNewPassword" ErrorMessage="New password is required"
                    CssClass="error-msg" Display="Dynamic" ValidationGroup="vgPassword" />
            </div>

            <div class="form-group">
                <label>Confirm New Password *</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Re-enter new password" />
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                    ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm password is required"
                    CssClass="error-msg" Display="Dynamic" ValidationGroup="vgPassword" />
                <asp:CompareValidator ID="cvPassword" runat="server"
                    ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword"
                    ErrorMessage="New passwords do not match" CssClass="error-msg" Display="Dynamic" ValidationGroup="vgPassword" />
            </div>

            <asp:Button ID="btnChangePassword" runat="server" Text="Update Password" 
                CssClass="btn-primary" OnClick="btnChangePassword_Click" ValidationGroup="vgPassword" />
        </div>

    </div>
</asp:Content>
