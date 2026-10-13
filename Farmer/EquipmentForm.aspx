<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="EquipmentForm.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.EquipmentForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1><asp:Label ID="lblPageTitle" runat="server" Text="List New Equipment" /></h1>
            <p>Add details about your agricultural machinery to find buyers or renters in the marketplace.</p>
        </div>
    </div>

    <div class="form-card">
        <asp:HiddenField ID="hfEquipmentID" runat="server" />

        <!-- ===== SECTION 1: EQUIPMENT DETAILS ===== -->
        <div class="form-section">
            <div class="form-section-title">
                ⚙️ Equipment Details
            </div>

            <!-- Row: Category + Make/Model -->
            <div class="form-row">
                <!-- ========== IMAGE UPLOAD SECTION ========== -->
<div class="form-group">
    <label>Equipment Photos</label>
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
                Enabled="false" />  <!-- Edit mode ma disabled raakhishu -->
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


                <div class="form-group">
                    <label>Equipment Category *</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select equipment type" Value="" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCat" runat="server"
                        ControlToValidate="ddlCategory" InitialValue=""
                        ErrorMessage="Please select a category" CssClass="error-msg" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label>Make & Model *</label>
                    <asp:TextBox ID="txtMakeModel" runat="server" CssClass="form-control"
                        placeholder="e.g. John Deere 5050 D" />
                    <asp:RequiredFieldValidator ID="rfvMake" runat="server"
                        ControlToValidate="txtMakeModel" ErrorMessage="Make & Model is required"
                        CssClass="error-msg" Display="Dynamic" />
                </div>
            </div>

            <!-- Row: Equipment Name -->
            <div class="form-group">
                <label>Equipment Name / Title *</label>
                <asp:TextBox ID="txtEquipmentName" runat="server" CssClass="form-control"
                    placeholder="e.g. Mahindra 575 DI" />
                <asp:RequiredFieldValidator ID="rfvName" runat="server"
                    ControlToValidate="txtEquipmentName" ErrorMessage="Equipment name is required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>

            <!-- Row: Usage Duration (Years + Months) + Price -->
            <div class="form-row">
                <div class="form-group">
                    <label>Total Usage Duration</label>
                    <div class="inline-group">
                        <asp:TextBox ID="txtUsageYears" runat="server" CssClass="form-control"
                            placeholder="0" TextMode="Number" />
                        <asp:TextBox ID="txtUsageMonths" runat="server" CssClass="form-control"
                            placeholder="0" TextMode="Number" />
                        <div style="display:none;"></div>
                    </div>
                    <div style="display:flex;gap:12px;font-size:11px;color:#999;margin-top:4px;">
                        <span style="flex:1;">Years</span>
                        <span style="flex:1;">Months</span>
                    </div>
                </div>

                <div class="form-group">
                    <label>Asking Price (₹) *</label>
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

            <!-- Condition Notes -->
            <div class="form-group">
                <label>Condition & Notes</label>
                <asp:TextBox ID="txtCondition" runat="server" CssClass="form-control"
                    TextMode="MultiLine" Rows="4"
                    placeholder="Describe the current condition, any recent repairs, or specific features..." />
            </div>
        </div>

        <!-- ===== SECTION 2: CONTACT INFORMATION ===== -->
        <div class="form-section">
            <div class="form-section-title orange">
                📍 Contact Information
            </div>

            <!-- Mobile + Postal -->
            <div class="form-row">
                <div class="form-group">
                    <label>Mobile Number *</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control"
                        placeholder="Phone Number" />
                    <asp:RequiredFieldValidator ID="rfvMobile" runat="server"
                        ControlToValidate="txtMobile" ErrorMessage="Mobile is required"
                        CssClass="error-msg" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revMobile" runat="server"
                        ControlToValidate="txtMobile"
                        ValidationExpression="^[6-9]\d{9}$"
                        ErrorMessage="Enter valid 10-digit mobile number"
                        CssClass="error-msg" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label>Postal / ZIP Code</label>
                    <asp:TextBox ID="txtPostal" runat="server" CssClass="form-control"
                        placeholder="Enter postal code" />
                    <asp:RegularExpressionValidator ID="revPostal" runat="server"
                        ControlToValidate="txtPostal"
                        ValidationExpression="^\d{6}$"
                        ErrorMessage="Enter valid 6-digit PIN code"
                        CssClass="error-msg" Display="Dynamic" />
                </div>
            </div>

            <!-- Farm Location -->
            <div class="form-group">
                <label>Farm / Equipment Location *</label>
                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control"
                    TextMode="MultiLine" Rows="3"
                    placeholder="Full address where the equipment can be inspected..." />
                <asp:RequiredFieldValidator ID="rfvLocation" runat="server"
                    ControlToValidate="txtLocation" ErrorMessage="Location is required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>
        </div>

        <!-- Actions -->
        <div class="form-actions">
            <a href="EquipmentManagement.aspx" class="btn-outline">Cancel</a>
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
