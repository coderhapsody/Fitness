<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="ManageRoles.aspx.cs" Inherits="ManageRoles" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Application Roles
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ValidationSummary ID="vsSummary" runat="server" EnableViewState="false" CssClass="errorMessage" />
    <table style="width: 100%">
        <tr>
            <td>
                Enter new role:
                <asp:TextBox ID="txtRoleName" runat="server" CssClass="textbox" />
                <asp:RequiredFieldValidator ID="rqvRoleName" runat="server" ControlToValidate="txtRoleName"
                    CssClass="errorMessage" Text="*" EnableViewState="false" SetFocusOnError="true"
                    ErrorMessage="<strong>Role Name</strong> must be specified" />
                &nbsp;
                <asp:Button ID="btnSave" runat="server" EnableViewState="false" SkinID="SaveButton"
                    CommandArgument="Save" CommandName="FormCommand" OnCommand="Buttons_Command" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblStatus" runat="server" EnableViewState="false" CssClass="errorMessage" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="false"
                    Width="300px" OnRowCommand="gridRoles_RowCommand" 
                    onrowcreated="gvwMaster_RowCreated">
                    <Columns>
                        <asp:BoundField DataField="RoleName" HeaderStyle-HorizontalAlign="Left" HeaderText="Role Name" />
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteRow" CssClass="button"
                                    CommandArgument='<%# Eval("RoleName") %>' CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        .: No Data :.
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
