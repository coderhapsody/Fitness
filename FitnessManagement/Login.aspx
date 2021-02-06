<%@ Page Title="" Language="C#" MasterPageFile="~/MasterUnregisteredPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStyleSheets" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="Server">
    <img alt="" src="images/little_monkey.png" />
    <h2>Fitness Management System</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <AjaxToolkit:ToolkitScriptManager ID="tsmAjaxToolkitScriptManager" runat="server" />        
    <table style="width: 100%;" id="tblLogin">
        <tr>
            <td>
                <asp:Login ID="loginApp" runat="server" CssClass="mainContent" Width="100%" DisplayRememberMe="False"
                    RememberMeText="Remember me" DestinationPageUrl="~/Default.aspx"
                    OnLoggedIn="loginApp_LoggedIn" OnLoginError="loginApp_LoginError">
                    <LayoutTemplate>
                        <table cellpadding="1" cellspacing="0" style="border-collapse: collapse; margin-left: 100px;">
                            <tr>
                                <td>
                                    <table cellpadding="3" style="width: 100%;">
                                        <tr>
                                            <td align="left" colspan="2">
                                                <asp:ValidationSummary ID="vsLogin" runat="Server" EnableViewState="False" ValidationGroup="loginApp"
                                                    CssClass="errorMessage" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 100px;">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="UserName" runat="server" CssClass="textbox" Width="150px"></asp:TextBox>
                                                <AjaxToolkit:FilteredTextBoxExtender ID="UserName_FilteredTextBoxExtender" FilterMode="ValidChars" FilterType="Custom" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"
                                                    runat="server" Enabled="True" TargetControlID="UserName">
                                                </AjaxToolkit:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                    CssClass="errorMessage" ErrorMessage="<strong>User Name</strong> is required."
                                                    ForeColor="Red" ToolTip="User Name is required." ValidationGroup="loginApp">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 100px;">
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Password" runat="server" CssClass="textbox" TextMode="Password" Width="150px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                    CssClass="errorMessage" ErrorMessage="<strong>Password</strong> is required."
                                                    ForeColor="Red" ToolTip="Password is required." ValidationGroup="loginApp">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red;">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="LoginButton" runat="server" EnableViewState="false" CommandName="Login"
                                                    CssClass="button" Text="Log In" ValidationGroup="loginApp" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <LoginButtonStyle CssClass="flat" />
                    <TextBoxStyle CssClass="flat" Width="200px" />
                    <ValidatorTextStyle CssClass="errorMessage" />
                </asp:Login>
            </td>
        </tr>
    </table>
</asp:Content>

