<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="RePrint.aspx.cs" Inherits="RePrint" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Re-Print Invoice
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <AjaxToolkit:ToolkitScriptManager ID="tsmScriptManager" runat="server" />
    <table class="style1">
        <tr>
            <td class="style2">
                Invoice No.
            </td>
            <td class="style3">
                :
            </td>
            <td>
                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>&nbsp;
                <asp:Hyperlink id="hypLookUpInvoice" NavigateUrl="#" runat="server"
                                        onclick="showPromptPopUp('PromptInvoice.aspx?', this.previousSibling.previousSibling.id, 550, 1000);">Look Up</asp:Hyperlink>
            &nbsp;
                <asp:RequiredFieldValidator ID="rqvInvoiceNo" runat="server" 
                    ControlToValidate="txtInvoiceNo" CssClass="errorMessage" 
                    EnableViewState="False" 
                    ErrorMessage="&lt;b&gt;Invoice Number&lt;/b&gt; must be specified" 
                    SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;
            </td>
            <td class="style3">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnReprint" runat="server" CssClass="button" EnableViewState="False"
                    Text="Re-Print This Invoice" OnClick="btnReprint_Click" />
            &nbsp;<asp:Label ID="lblStatus0" runat="server" EnableViewState="False"></asp:Label>
            </td>
        </tr>
    </table>        
    <asp:Button ID="btnDummy" runat="server" CausesValidation="False" CssClass="textbox"
        EnableViewState="False" Text="Cancel" Style="display: none;" />
    <asp:Panel ID="pnlAuth" runat="server" Style="display: block;" BorderStyle="Solid"
        BorderWidth="2px" BorderColor="Black" Width="300px" BackColor="White">
        <table style="width: 100%">
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    User Name
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    Password
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" Width="120px" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnPopupOK" runat="server" CausesValidation="False" CssClass="button"
                        EnableViewState="False" Text="Authorize" OnClick="btnPopupOK_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnPopupCancel" runat="server" CausesValidation="False" CssClass="button"
                        EnableViewState="False" Text="Cancel" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <AjaxToolkit:ModalPopupExtender ID="mopAuth" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="btnPopupCancel" DropShadow="True" PopupControlID="pnlAuth" PopupDragHandleControlID="pnlAuth"  
        TargetControlID="btnDummy">        
    </AjaxToolkit:ModalPopupExtender>    
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 160px;
        }
        .style3
        {
            width: 2px;
        }
    </style>
</asp:Content>
