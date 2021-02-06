<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Change Password
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table style="width: 100%">
        <tr>
            <td style="width: 100px">
                <img alt="" src="images/Security2.png" /></td>
            <td>
                <asp:ChangePassword ID="changePasswordApp" runat="server">
                    <ChangePasswordTemplate>
                        <table cellpadding="1" cellspacing="0" style="border-collapse: collapse; width: 100%">
                            <tr>
                                <td>
                                    <table cellpadding="1">
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ID="vsChangePassword" runat="server" ValidationGroup="changePasswordApp"
                                                    CssClass="errorMessage" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 150px;">
                                                <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="CurrentPassword" CssClass="textbox" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                                    CssClass="errorMessage" ErrorMessage="<strong>Password</strong> is required."
                                                    SetFocusOnError="true" ForeColor="Red" ToolTip="Password is required." ValidationGroup="changePasswordApp">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 150px;">
                                                <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="NewPassword" CssClass="textbox" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                                    CssClass="errorMessage" ErrorMessage="<strong>New Password</strong> is required."
                                                    SetFocusOnError="true" ForeColor="Red" ToolTip="New Password is required." ValidationGroup="changePasswordApp">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 150px;">
                                                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ConfirmNewPassword" CssClass="textbox" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                                    CssClass="errorMessage" ErrorMessage="<strong>Confirm New Password</strong> is required."
                                                    SetFocusOnError="true" ForeColor="Red" ToolTip="Confirm New Password is required."
                                                    ValidationGroup="changePasswordApp">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                                    ControlToValidate="ConfirmNewPassword" CssClass="errorMessage" Display="Dynamic"
                                                    SetFocusOnError="true" ErrorMessage="The <strong>Confirm New Password</strong> must match the <strong>New Password</strong> entry."
                                                    ForeColor="Red" ValidationGroup="changePassword"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red;">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="ChangePasswordPushButton" CssClass="button" runat="server" CommandName="ChangePassword"
                                                    Text="Change Password" ValidationGroup="changePasswordApp" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ChangePasswordTemplate>
                    <SuccessTemplate>
                        <table cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                            <tr>
                                <td>
                                    <table cellpadding="0">
                                        <tr>
                                            <td align="center" colspan="2">
                                                Change Password Complete
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Your password has been changed!
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </SuccessTemplate>
                </asp:ChangePassword>
            </td>
        </tr>
    </table>
</asp:Content>
