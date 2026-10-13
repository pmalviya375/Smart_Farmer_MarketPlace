<%@ Page Title="" Language="C#" MasterPageFile="~/Farmer/Farmer.Master" AutoEventWireup="true" CodeBehind="AnimalType.aspx.cs" Inherits="Smart_Farmer_MarketPlace.Farmer.AnimalType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="page-header">
        <div>
            <h1>Animal Type Management</h1>
            <p>Define species like Cow, Buffalo, Goat, Sheep for your livestock listings.</p>
        </div>
    </div>

    <div class="card">
        <h3 style="font-size:16px;margin-bottom:15px;">
            <asp:Label ID="lblFormTitle" runat="server" Text="Add New Animal Type" />
        </h3>

        <asp:HiddenField ID="hfTypeID" runat="server" />

        <div class="form-row">
            <div class="form-group">
                <label>Animal Type Name</label>
                <asp:TextBox ID="txtTypeName" runat="server" CssClass="form-control"
                    placeholder="e.g. Cow, Buffalo, Goat, Sheep" />
                <asp:RequiredFieldValidator ID="rfvType" runat="server"
                    ControlToValidate="txtTypeName" ErrorMessage="Type name required"
                    CssClass="error-msg" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label>Description (Optional)</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"
                    placeholder="Short description" />
            </div>
        </div>

        <asp:Button ID="btnSaveType" runat="server" Text="Save Type"
            CssClass="btn-green" OnClick="btnSaveType_Click" />

        <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel"
            CssClass="btn-sm" style="background:#eee;color:#555;margin-left:8px;"
            OnClick="btnCancelEdit_Click" Visible="false" CausesValidation="false" />
    </div>

    <div class="card">
        <h3 style="font-size:16px;margin-bottom:15px;">All Animal Types</h3>

        <asp:GridView ID="gvTypes" runat="server"
            AutoGenerateColumns="false"
            CssClass="grid-table"
            GridLines="None"
            OnRowCommand="gvTypes_RowCommand"
            EmptyDataText="No types added yet.">

            <Columns>
                <asp:TemplateField HeaderText="Type Name">
                    <ItemTemplate>
                        🐾 <strong><%# Eval("TypeName") %></strong>
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
                            CommandName="EditType"
                            CommandArgument='<%# Eval("TypeID") %>'>
                            ✏️ Edit
                        </asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnDelete" runat="server"
                            CssClass="btn-sm btn-delete"
                            CommandName="DeleteType"
                            CommandArgument='<%# Eval("TypeID") %>'
                            OnClientClick="return confirm('Delete this type?');">
                            🗑 Delete
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
