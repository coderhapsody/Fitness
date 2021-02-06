<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true" CodeFile="MasterParents.aspx.cs" Inherits="MasterParents" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
    .style1
    {
        width: 133px;
    }
    .style2
    {
        width: 3px;
    }
        .style3
        {
            height: 17px;
        }
        .style5
        {
            height: 17px;
            width: 130px;
        }
        .style6
        {
            width: 130px;
        }
        .style7
        {
            width: 2px;
        }
        .style8
        {
            height: 17px;
            width: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
        Parents / Guardians
        / Pick Up Persons 
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:MultiView ID="mvwParents" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td class="style1">
                        Barcode</td>
                    <td class="style2">
                        :</td>
                    <td>
                        <asp:Label ID="lblBarcode" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Name</td>
                    <td class="style2">
                        :</td>
                    <td>
                        <asp:Label ID="lblName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Date of Birth</td>
                    <td class="style2">
                        :</td>
                    <td>
                        <asp:Label ID="lblDateOfBirth" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;</td>
                    <td class="style2">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            <asp:LinkButton ID="lnbAddNew" runat="server" Text="Add New"  SkinID="AddNewButton"
                ValidationGroup="Read" EnableViewState="false" onclick="lnbAddNew_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnbDelete" runat="server" Text="Delete"  SkinID="DeleteButton"
                ValidationGroup="Read" EnableViewState="false" CausesValidation="false" 
                onclick="lnbDelete_Click" />
            <asp:GridView ID="gvwParent" runat="server" Width="100%" 
                AutoGenerateColumns="False" DataSourceID="sdsParents" 
                SkinID="GridViewDefaultSkin" onrowcommand="gvwParent_RowCommand" 
                onrowcreated="gvwParent_RowCreated" 
                onrowdatabound="gvwParent_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Connection" HeaderText="Connection" ReadOnly="True" 
                        SortExpression="Connection" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="BirthDate" HeaderText="BirthDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MMM-yyyy}"
                        SortExpression="BirthDate" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Phone1" HeaderText="Phone1"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="Phone1" />
                    <asp:BoundField DataField="Phone2" HeaderText="Phone2"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="Phone2" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate> 
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    .: No Parents / Guardians Information :.
                </EmptyDataTemplate>
            </asp:GridView>
            
            <asp:SqlDataSource ID="sdsParents" runat="server" 
                ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
                onselecting="sdsParents_Selecting" SelectCommand="proc_GetCustomerParents" 
                SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="CustomerBarcode" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <input id="btnClose" type="button" class="button" onclick="window.close();" value="Close this window" />    
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td class="style6">
                        Connection</td>
                    <td class="style7">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlConnection" runat="server" CssClass="dropdown">
                            <asp:ListItem Selected="True" Value="F">Father</asp:ListItem>
                            <asp:ListItem Value="M">Mother</asp:ListItem>
                            <asp:ListItem Value="G">Guardian</asp:ListItem>
                            <asp:ListItem Value="P">Pick Up Person</asp:ListItem>   
                            <asp:ListItem Value="O">Other</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        Name</td>
                    <td class="style8">
                        :</td>
                    <td class="style3">
                        <asp:TextBox ID="txtName" runat="server" CssClass="textbox" Width="250px" 
                            MaxLength="50" ValidationGroup="AddEdit"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtName" CssClass="errorMessage" 
                            ErrorMessage="&lt;b&gt;Name&lt;/b&gt; must be specified" SetFocusOnError="True" 
                            ValidationGroup="AddEdit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        Date of Birth</td>
                    <td class="style7">
                        :</td>
                    <td>
                        <ew:CalendarPopup ID="calDateOfBirth" runat="server" SkinID="Calendar">
                            <TextBoxLabelStyle CssClass="textbox" />
                        </ew:CalendarPopup>
                        &nbsp;<asp:CheckBox ID="chkUnknownBirthDate" runat="server" Text="Unknown" />
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        ID Card No.</td>
                    <td class="style7">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtIDCardNo" runat="server" CssClass="textbox" MaxLength="20" 
                            ValidationGroup="AddEdit" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        Email</td>
                    <td class="style7">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" Width="250px" 
                            MaxLength="50" ValidationGroup="AddEdit"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                            ControlToValidate="txtEmail" CssClass="errorMessage" EnableViewState="False" 
                            ErrorMessage="&lt;b&gt;Email Address&lt;/b&gt; is invalid" 
                            SetFocusOnError="True" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ValidationGroup="AddEdit"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        Phone 1</td>
                    <td class="style7">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtPhone1" runat="server" CssClass="textbox" Width="150px"  onkeydown="return NumbersOnly(event)" 
                            MaxLength="20" ValidationGroup="AddEdit"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        Phone 2</td>
                    <td class="style7">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtPhone2" runat="server" CssClass="textbox" Width="150px"  onkeydown="return NumbersOnly(event)" 
                            MaxLength="20" ValidationGroup="AddEdit"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        Photo</td>
                    <td class="style7">
                        :</td>
                    <td>
                        <asp:FileUpload ID="fupPhoto" runat="server" />
                        <asp:CheckBox ID="chkDeletePhoto" runat="server" Text="Delete current photo" />
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        &nbsp;</td>
                    <td class="style7">
                        &nbsp;</td>
                    <td>
                        <asp:Image ID="imgPhoto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        &nbsp;</td>
                    <td class="style7">
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        &nbsp;</td>
                    <td class="style7">
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" EnableViewState="false" 
                            onclick="btnSave_Click" SkinID="SaveButton" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="false" 
                            EnableViewState="false" onclick="btnCancel_Click" 
                            OnClientClick="return confirm('Are you sure want to cancel ?')" 
                            SkinID="CancelButton" ValidationGroup="AddEdit" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
    </asp:Content>

