<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="ManageFormAccess.aspx.cs" Inherits="ManageFormAccess" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Forms Security
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ScriptManager ID="scmScriptManager" runat="server" />
    <table class="ui-accordion">
        <tr>
            <td valign="top" width="250">
                <asp:Label ID="lblUserName" runat="server"></asp:Label>
            </td>
            <td valign="top">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="top" width="250">
                &nbsp;&nbsp;</td>
            <td valign="top">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="top" width="250">
                <asp:ListBox ID="lstMenu" runat="server" Width="200px" CssClass="noborder" 
                    AutoPostBack="true" onselectedindexchanged="lstMenu_SelectedIndexChanged">
                </asp:ListBox>
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:CheckBoxList ID="cblAccess" runat="server" Width="300px">
                            <asp:ListItem Value="A">Allow Add New</asp:ListItem>
                            <asp:ListItem Value="U">Allow Update</asp:ListItem>
                            <asp:ListItem Value="D">Allow Delete</asp:ListItem>
                            <asp:ListItem Value="R">Allow Read</asp:ListItem>
                        </asp:CheckBoxList>

                        <p>
                        <asp:Label ID="lblStatus" runat="server" EnableViewState="false" CssClass="errorMessage" />
                        <br />
                        <asp:Button ID="btnSave" runat="server"  Text="Save" CssClass="button" 
                                onclick="btnSave_Click" />
                        </p>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstMenu" 
                            EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                

            </td>
        </tr>
    </table>
</asp:Content>
