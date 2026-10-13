<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="CropForm.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.CropForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="page-header">
        <div>
            <h1><asp:Label ID="lblPageTitle" runat="server" Text="Add New Crop" /></h1>
            <p>Enter the details of your harvest. Once added, this crop will be verified and listed in the marketplace for buyers to see.</p>
        </div>
    </div>

    <div class="form-card">
        <asp:HiddenField ID="hfCropID" runat="server" />

        <!-- ========== IMAGE UPLOAD SECTION ========== -->
<div class="form-group">
    <label>Crop Photos</label>
    <small style="color:#888;font-size:12px;display:block;margin-bottom:5px;">
        Main image required. Additional 3 images optional. (Max 2MB each, JPG/PNG)
    </small>

    <div class="image-upload-grid">

        <!-- MAIN IMAGE -->
        <div class="image-box main-box">
            <span class="image-box-label">⭐ Main Image *</span>
            <asp:FileUpload ID="fuMainImage" runat="server" CssClass="file-input"
                accept="image/*" onchange="previewImage(this, 'imgMain')" />
            <asp:Image ID="imgMain" runat="server" CssClass="image-preview" ClientIDMode="Static" />
            <asp:Label ID="lblMainImage" runat="server" CssClass="remove-img" Visible="false" />
            <asp:RequiredFieldValidator ID="rfvMainImg" runat="server"
                ControlToValidate="fuMainImage"
                ErrorMessage="Main image is required"
                CssClass="error-msg" Display="Dynamic"
                Enabled="false" />
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


        <!-- Crop Name -->
        <div class="form-group">

            <label>Crop Name</label>
            <asp:TextBox ID="txtCropName" runat="server" CssClass="form-control"
                placeholder="e.g., Premium Basmati Rice" />
            <asp:RequiredFieldValidator ID="rfvName" runat="server"
                ControlToValidate="txtCropName" ErrorMessage="Crop name is required"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <!-- Category Dropdown -->
        <div class="form-group">
            <label>Crop Category</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                <asp:ListItem Text="-- Select Category --" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvCat" runat="server"
                ControlToValidate="ddlCategory" InitialValue=""
                ErrorMessage="Please select a category" CssClass="error-msg" Display="Dynamic" />
        </div>

        <!-- Quantity + Contact -->
        <div class="form-row">
            <div class="form-group">
                <label>Quantity per Quintal</label>
                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"
                    placeholder="0.00" />
                <asp:RequiredFieldValidator ID="rfvQty" runat="server"
                    ControlToValidate="txtQuantity" ErrorMessage="Quantity is required"
                    CssClass="error-msg" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revQty" runat="server"
                    ControlToValidate="txtQuantity"
                    ValidationExpression="^\d+(\.\d{1,2})?$"
                    ErrorMessage="Enter a valid number (e.g., 450.00)"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label>Contact Number</label>
                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control"
                    placeholder="+91 10-digit number" />
                <asp:RequiredFieldValidator ID="rfvContact" runat="server"
                    ControlToValidate="txtContact" ErrorMessage="Contact is required"
                    CssClass="error-msg" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revContact" runat="server"
                    ControlToValidate="txtContact"
                    ValidationExpression="^[6-9]\d{9}$"
                    ErrorMessage="Enter valid 10-digit mobile number"
                    CssClass="error-msg" Display="Dynamic" />
            </div>
        </div>

        <!-- Address -->
        <div class="form-group">
            <label>Farmer Address</label>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"
                TextMode="MultiLine" Rows="3"
                placeholder="Enter full farm address including village, district, and state" />
            <asp:RequiredFieldValidator ID="rfvAddr" runat="server"
                ControlToValidate="txtAddress" ErrorMessage="Address is required"
                CssClass="error-msg" Display="Dynamic" />
        </div>

        <hr style="border:none;border-top:1px solid #eee;margin:25px 0;" />

        <asp:Button ID="btnSave" runat="server" Text="🌱 Add Crop to Inventory"
            CssClass="btn-green" OnClick="btnSave_Click"
            style="float:right;" />

        <a href="CropManagement.aspx" class="btn-sm"
           style="background:#eee;color:#555;padding:11px 20px;text-decoration:none;border-radius:8px;">
            Cancel
        </a>

        <div style="clear:both;"></div>
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
