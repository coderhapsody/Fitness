<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ManageUsers.aspx.cs" Inherits="ManageUsers" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Application Users
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="View1" runat="server">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 200px">
                                    Roles</td>
                                <td style="width: 1px">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlFindRoles" CssClass="dropdown" runat="server" />                                    
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    &nbsp;</td>
                                <td style="width: 1px">
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnRefresh" CssClass="button" runat="server" Text="Refresh" 
                                        oncommand="Buttons_Command" CommandArgument="Refresh" ToolTip="Show data with specified criteria"
                                        CommandName="FormCommand" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" CssClass="errorMessage" 
                            EnableViewState="False"></asp:Label>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" CausesValidation="False" 
                            CommandArgument="AddNew" CommandName="FormCommand" EnableViewState="False" 
                            oncommand="Buttons_Command" SkinID="AddNewButton"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Delete marked row(s) ?')"
                            CommandArgument="Delete" CommandName="FormCommand" EnableViewState="False" 
                            oncommand="Buttons_Command" SkinID="DeleteButton" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" 
                            Width="100%"  AllowSorting="true"
                            AutoGenerateColumns="false" onsorting="gvwMaster_Sorting" 
                            onrowcommand="gvwMaster_RowCommand" onrowcreated="gvwMaster_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="UserName" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CreationDate" SortExpression="CreationDate" HeaderText="CreationDate" HtmlEncode="false" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="LastLoginDate" SortExpression="LastLoginDate" HeaderText="LastLoginDate" HtmlEncode="false" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Locked" HeaderStyle-HorizontalAlign="Left" >                                    
                                    <ItemTemplate>
                                        <%# Convert.ToBoolean(Eval("IsLockedOut")) ? "Yes" : "No" %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnbResetPassword" runat="server" CommandName="ResetPassword" CommandArgument='<%# Eval("UserName") %>' Text="Reset Password" ToolTip="Reset password for this user" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/ManageFormAccess.aspx?UserName={0}" DataNavigateUrlFields="UserName" Text="User Access" Visible="false" />
                                <asp:TemplateField ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lnbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("UserName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
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
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 200px">
                        User Name</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" ValidationGroup="AddEdit"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvUserName" runat="server" 
                            CssClass="errorMessage" ControlToValidate="txtUserName" SetFocusOnError="true" 
                            EnableViewState="false" 
                            ErrorMessage="<strong>User Name</strong> is required" 
                            ToolTip="User Name is required" Display="Dynamic" 
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>                
                <tr>
                    <td style="width: 200px">
                        Role Name</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="dropdown" ValidationGroup="AddEdit">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqvRole" runat="server" CssClass="errorMessage" 
                            ControlToValidate="ddlRole" SetFocusOnError="true" EnableViewState="false" 
                            ErrorMessage="<strong>Role</strong> is required" 
                            ToolTip="Role is required" Display="Dynamic" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Home Branch</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlHomeBranch" runat="server" CssClass="dropdown" 
                            ValidationGroup="AddEdit">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqvHomeBranch" runat="server" 
                            ControlToValidate="ddlHomeBranch" CssClass="errorMessage" Display="Dynamic" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;strong&gt;Home Branch&lt;/strong&gt; is required" 
                            SetFocusOnError="true" ToolTip="Home Branch is required" 
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Barcode</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="textbox" 
                            ValidationGroup="AddEdit"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvBarcode" runat="server" 
                            ControlToValidate="txtBarcode" CssClass="errorMessage" Display="Dynamic" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;strong&gt;Barcode&lt;/strong&gt; is required" 
                            SetFocusOnError="true" ToolTip="Barcode is required" 
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Email</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" ValidationGroup="AddEdit"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvEmail" runat="server" 
                            CssClass="errorMessage" ControlToValidate="txtEmail" SetFocusOnError="true" 
                            EnableViewState="false" ErrorMessage="<strong>Email</strong> is required" 
                            ToolTip="Email is required" Display="Dynamic" 
                            ValidationGroup="AddEdit" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                            ControlToValidate="txtEmail" CssClass="errorMessage" Display="Dynamic" 
                            EnableViewState="False" 
                            ErrorMessage="&lt;strong&gt;Email&lt;/strong&gt; is invalid" 
                            SetFocusOnError="True" ToolTip="Email is invalid" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ValidationGroup="AddEdit"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Password</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" ValidationGroup="AddEdit" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvPassword" CssClass="errorMessage" 
                            runat="server" ControlToValidate="txtPassword" SetFocusOnError="true" 
                            EnableViewState="false" 
                            ErrorMessage="<strong>Password</strong> is required" 
                            ToolTip="Password is required" Display="Dynamic" 
                            ValidationGroup="AddEdit" />
                        <asp:CustomValidator ID="cuvPassword" runat="server" ControlToValidate="txtPassword"  ErrorMessage="Minimum <b>password</b> length is 6 characters" CssClass="errorMessage" OnServerValidate="cuvPassword_ServerValidate" ValidationGroup="AddEdit" ></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        Confirm New Password</td>
                    <td style="width: 1px">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="textbox" ValidationGroup="AddEdit" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvConfirmPassword" runat="server" 
                            CssClass="errorMessage" ControlToValidate="txtConfirmPassword" 
                            SetFocusOnError="true" EnableViewState="false" 
                            ErrorMessage="<strong>Confirm Password</strong> is required" 
                            ToolTip="Confirm Password is required" Display="Dynamic" 
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        &nbsp;</td>
                    <td style="width: 1px">
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" SkinID="SaveButton" EnableViewState="false" 
                            CommandName="FormCommand" CommandArgument="Save" 
                            ValidationGroup="AddEdit"  
                            oncommand="Buttons_Command" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" SkinID="CancelButton"
                            CommandName="FormCommand" CommandArgument="Cancel" OnClientClick="return confirm('Cancel current operation ?')" 
                            EnableViewState="false" CausesValidation="false" 
                            oncommand="Buttons_Command" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        &nbsp;</td>
                    <td style="width: 1px">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View3" runat="server">

            <table style="width: 100%">
                <tr>
                    <td style="width: 200px">
                        <img alt="" src="images/Security_Warning.png" />
                    </td>
                    <td>
                        <h3>Warning</h3>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    This option will reset the password for user: <asp:Label ID="lblResetUserName" runat="server" Font-Bold="true"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    After password reset, user must log in with new random password and change it to 
                                    desired one.</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDoReset" runat="server" CssClass="button" 
                                        CommandName="FormCommand" CommandArgument="DoResetPassword" Text="Reset" 
                                        EnableViewState="False" oncommand="Buttons_Command" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnCancelReset" runat="server" CssClass="button" CommandName="FormCommand"
                                        CommandArgument="CancelResetPassword" Text="Cancel" EnableViewState="False" 
                                        oncommand="Buttons_Command" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNewPassword" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </asp:View>
    </asp:MultiView>
</asp:Content>