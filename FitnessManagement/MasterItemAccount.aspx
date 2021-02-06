<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="MasterItemAccount.aspx.cs" Inherits="MasterItemAccount" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Item Account
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    
    <table class="style1">
        <tr>
            <td style="width:30%; vertical-align:top;">
                <p>
                <em>Account with asterisk sign (*) is inactive account</em>
                </p>
                <asp:TreeView ID="tvwAccount" runat="server" PopulateNodesFromClient="true" EnableClientScript="true"
                    onselectednodechanged="tvwAccount_SelectedNodeChanged" 
                    ontreenodepopulate="tvwAccount_TreeNodePopulate">
                </asp:TreeView>
            </td>
            <td style="vertical-align:top;">
                <p>
                <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false" 
                            Text="Add New" SkinID="AddNewButton" onclick="lnbAddNew_Click"  />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false" 
                            Text="Delete" OnClientClick="return confirm('Delete marked row(s) ?')" 
                            SkinID="DeleteButton" onclick="lnbDelete_Click"  />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                   </p>     
                <table class="style1">
                    <tr>
                        <td class="style7">
                            Account No.</td>
                        <td class="style5">
                            :</td>
                        <td class="style6">
                            <asp:TextBox ID="txtAccountNo" runat="server" CssClass="textbox" Width="100px" ValidationGroup="AddEdit"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqvAccountNo" runat="server" ErrorMessage="<b>Account Number</b> must be specified uniquely"  ControlToValidate="txtAccountNo" EnableViewState="false" SetFocusOnError="true"  ValidationGroup="AddEdit"  CssClass="errorMessage"/>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            Description</td>
                        <td class="style3">
                            :</td>
                        <td>
                            <asp:TextBox ID="txtAccountDescription" runat="server" CssClass="textbox" ValidationGroup="AddEdit"
                                Width="250px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rqvDescription" runat="server" ErrorMessage="<b>Description</b> must be specified"  ControlToValidate="txtAccountDescription" EnableViewState="false" SetFocusOnError="true"  ValidationGroup="AddEdit" CssClass="errorMessage" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            Parent Account</td>
                        <td class="style3">
                            :</td>
                        <td>
                            <asp:DropDownList ID="ddlParentAccount" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            &nbsp;</td>
                        <td class="style3">
                            &nbsp;</td>
                        <td>
                            <asp:CheckBox ID="chkActive" runat="server" Text="This account is active" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            &nbsp;</td>
                        <td class="style3">
                            &nbsp;</td>
                        <td>
                            <asp:CheckBox ID="chkCascade" runat="server" 
                                Text="Cascade account state to its child" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            &nbsp;</td>
                        <td class="style3">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style7">
                            &nbsp;</td>
                        <td class="style3">
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
                                SkinID="SaveButton" onclick="btnSave_Click" ValidationGroup="AddEdit" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
</asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style3
        {
            width: 7px;
        }
        .style5
        {
            width: 7px;
            height: 17px;
        }
        .style6
        {
            height: 17px;
        }
        .style7
        {
            width: 120px;
        }
    </style>
</asp:Content>


