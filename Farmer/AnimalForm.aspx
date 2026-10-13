<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="AnimalForm.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.AnimalForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <a href="AnimalManagement.aspx" style="color:#555;text-decoration:none;font-size:13px;">
        ← Back to Livestock Management
    </a>

    <div class="page-header" style="margin-top:15px;">
        <div>
            <h1><asp:Label ID="lblPageTitle" runat="server" Text="List New Animal" /></h1>
            <p>Provide accurate details to list your livestock on the marketplace.</p>
        </div>
    </div>

    <div class="form-card">
        <asp:HiddenField ID="hfAnimalID" runat="server" />

        <!-- ===== BASIC INFO ===== -->
        <div class="form-section">
            <div class="form-section-title">🐄 Basic Information</div>

            <!-- Row: Animal Name + Type + Price -->
            <div class="form-row">
                <div class="form-group">
                    <label>Animal Name *</label>
                    <asp:TextBox ID="txtAnimalName" runat="server" CssClass="form-control"
                        placeholder="e.g. Sahiwal Cow" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server"
                        ControlToValidate="txtAnimalName" ErrorMessage="Animal name required"
                        CssClass="error-msg" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label>Animal Type *</label>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select species" Value="" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvType" runat="server"
                        ControlToValidate="ddlType" InitialValue=""
                        ErrorMessage="Please select a type" CssClass="error-msg" Display="Dynamic" />
                </div>
            </div>

            <div class="form-group">
                <label>Price Expected (₹) *</label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control"
                    placeholder="0.00" />
                <asp:RequiredFieldValidator ID="rfvPrice" runat="server"
                    ControlToValidate="txtPrice" ErrorMessage="Price is required"
                    CssClass="error-msg" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPrice" runat="server"
                    ControlToValidate="txtPrice"
                    ValidationExpression="^\d+(\.\d{1,2})?$"
                    ErrorMessage="Enter valid amount" CssClass="error-msg" Display="Dynamic" />
            </div>
        </div>

        <!-- ===== ANIMAL INFORMATION ===== -->
        <div class="form-section">
            <div class="form-section-title">📋 Animal Information</div>

            <!-- Row: Age + Breed -->
            <div class="form-row">
                <div class="form-group">
                    <label>Age</label>
                    <div style="display:grid;grid-template-columns:2fr 1fr;gap:8px;">
                        <asp:TextBox ID="txtAge" runat="server" CssClass="form-control"
                            placeholder="e.g. 2" TextMode="Number" />
                        <asp:DropDownList ID="ddlAgeUnit" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Years" Value="Years" />
                            <asp:ListItem Text="Months" Value="Months" />
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <label>Breed</label>
                    <asp:TextBox ID="txtBreed" runat="server" CssClass="form-control"
                        placeholder="e.g. Holstein Friesian" />
                </div>
            </div>

            <!-- Health Status -->
            <div class="form-group">
                <label>Health Status & Vaccination History</label>
                <asp:TextBox ID="txtHealth" runat="server" CssClass="form-control"
                    TextMode="MultiLine" Rows="4"
                    placeholder="Describe current health, recent vaccinations, or any existing conditions..." />
            </div>
        </div>

        <!-- ===== IMAGES ===== -->
        <div class="form-section">
            <div class="form-section-title">📷 Animal Photos</div>

            <small style="color:#888;font-size:12px;display:block;margin-bottom:8px;">
                Main image required. Additional 3 images optional. (Max 2MB each, JPG/PNG)
            </small>

            <div class="image-upload-grid">
                <!-- MAIN -->
                <div class="image-box main-box">
                    <span class="image-box-label">⭐ Main Image *</span>
                    <asp:FileUpload ID="fuMainImage" runat="server" CssClass="file-input"
                        accept="image/*" onchange="previewImage(this, 'imgMain')" />
                    <asp:Image ID="imgMain" runat="server" CssClass="image-preview" ClientIDMode="Static" />
                    <asp:Label ID="lblMainImage" runat="server" CssClass="remove-img" Visible="false" />
                    <asp:RequiredFieldValidator ID="rfvMainImg" runat="server"
                        ControlToValidate="fuMainImage" ErrorMessage="Main image is required"
                        CssClass="error-msg" Display="Dynamic" Enabled="false" />
                </div>

                <!-- IMAGE 2 -->
                <div class="image-box">
                    <span class="image-box-label">Image 2</span>
                    <asp:FileUpload ID="fuImage2" runat="server" CssClass="file-input"
                        accept="image/*" onchange="previewImage(this, 'img2')" />
                    <asp:Image ID="img2" runat="server" CssClass="image-preview" ClientIDMode="Static" />
                    <asp:Label ID="lblImage2" runat="server" CssClass="remove-img" Visible="false" />
                </div>

                <!-- IMAGE 3 -->
                <div class="image-box">
                    <span class="image-box-label">Image 3</span>
                    <asp:FileUpload ID="fuImage3" runat="server" CssClass="file-input"
                        accept="image/*" onchange="previewImage(this, 'img3')" />
                    <asp:Image ID="img3" runat="server" CssClass="image-preview" ClientIDMode="Static" />
                    <asp:Label ID="lblImage3" runat="server" CssClass="remove-img" Visible="false" />
                </div>

                <!-- IMAGE 4 -->
                <div class="image-box">
                    <span class="image-box-label">Image 4</span>
                    <asp:FileUpload ID="fuImage4" runat="server" CssClass="file-input"
                        accept="image/*" onchange="previewImage(this, 'img4')" />
                    <asp:Image ID="img4" runat="server" CssClass="image-preview" ClientIDMode="Static" />
                    <asp:Label ID="lblImage4" runat="server" CssClass="remove-img" Visible="false" />
                </div>
            </div>
        </div>

        <!-- ===== SELLER CONTACT ===== -->
        <div class="form-section">
            <div class="form-section-title orange">📍 Seller Contact</div>

            <div class="form-group">
                <label>Mobile Number *</label>
                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control"
                    placeholder="+91 10-digit number" />
                <asp:RequiredFieldValidator ID="rfvMobile" runat="server"
                    ControlToValidate="txtMobile" ErrorMessage="Mobile required"
                    CssClass="error-msg" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revMobile" runat="server"
                    ControlToValidate="txtMobile"
                    ValidationExpression="^[6-9]\d{9}$"
                    ErrorMessage="Enter valid 10-digit Indian mobile"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label>Farm Address *</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"
                    TextMode="MultiLine" Rows="3"
                    placeholder="Full address for potential buyers to visit..." />
                <asp:RequiredFieldValidator ID="rfvAddress" runat="server"
                    ControlToValidate="txtAddress" ErrorMessage="Address required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>
        </div>

        <!-- Actions -->
        <div class="form-actions">
            <a href="AnimalManagement.aspx" class="btn-outline">Cancel</a>
            <asp:Button ID="btnSave" runat="server" Text="💾 Submit Listing"
                CssClass="btn-green" OnClick="btnSave_Click" />
        </div>
    </div>

    <script>
        function previewImage(input, imgId) {
            var img = document.getElementById(imgId);
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    img.src = e.target.result;
                    img.classList.add('show');
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>

    </asp:Content>
