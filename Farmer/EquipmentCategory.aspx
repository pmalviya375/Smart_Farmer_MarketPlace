<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="EquipmentCategory.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.EquipmentCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1>Equipment Category Management</h1>
            <p>Create and manage categories for your farm machinery listings.</p>
        </div>
    </div>

    <!-- ADD / EDIT FORM -->
    <div class="card">
        <h3 style="font-size:16px;margin-bottom:15px;">
            <asp:Label ID="lblFormTitle" runat="server" Text="Add New Equipment Category" />
        </h3>

        <asp:HiddenField ID="hfCategoryID" runat="server" />

        <div class="form-row">
            <div class="form-group">
                <label>Category Name</label>
                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control"
                    placeholder="e.g. Tractor, Pump, Harvester" />
                <asp:RequiredFieldValidator ID="rfvCat" runat="server"
                    ControlToValidate="txtCategoryName" ErrorMessage="Category name required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label>Description (Optional)</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"
                    placeholder="Short description" />
            </div>
        </div>

        <asp:Button ID="btnSaveCategory" runat="server" Text="Save Category"
            CssClass="btn-green" OnClick="btnSaveCategory_Click" />

        <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel"
            CssClass="btn-sm" style="background:#eee;color:#555;margin-left:8px;"
            OnClick="btnCancelEdit_Click" Visible="false" CausesValidation="false" />
    </div>

    <!-- LIST -->
    <div class="card">
        <h3 style="font-size:16px;margin-bottom:15px;">All Equipment Categories</h3>

        <asp:GridView ID="gvCategories" runat="server"
            AutoGenerateColumns="false"
            CssClass="grid-table"
            GridLines="None"
            OnRowCommand="gvCategories_RowCommand"
            EmptyDataText="No categories added yet.">

            <Columns>
                <asp:TemplateField HeaderText="Category Name">
                    <ItemTemplate>
                        <strong><%# Eval("CategoryName") %></strong>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <%# Eval("Description") == DBNull.Value || Eval("Description").ToString() == "" 
                            ? "<span style='color:#aaa;'>—</span>" 
                            : Eval("Description") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Created">
                    <ItemTemplate>
                        <%# Eval("CreatedDate", "{0:dd MMM yyyy}") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="160">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server"
                            CssClass="btn-sm btn-edit"
                            CommandName="EditCat"
                            CommandArgument='<%# Eval("CategoryID") %>'>
                            ✏️ Edit
                        </asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnDelete" runat="server"
                            CssClass="btn-sm btn-delete"
                            CommandName="DeleteCat"
                            CommandArgument='<%# Eval("CategoryID") %>'
                            OnClientClick="return confirm('Delete this category?');">
                            🗑 Delete
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
