<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="MasterEmployee.aspx.cs" Inherits="MasterEmployee" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Employee
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="style1">
                <tr>
                    <td>
                        <table class="style1">
                            <tr>
                                <td class="style4">
                                    Barcode</td>
                                <td class="style5">
                                    :</td>
                                <td style="margin-left: 40px">
                                    <asp:TextBox ID="txtFindBarcode" runat="server" CssClass="textbox" 
                                        MaxLength="50" ValidationGroup="AddEdit" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    Name</td>
                                <td class="style5">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="txtFindName" runat="server" CssClass="textbox" MaxLength="50" 
                                        ValidationGroup="AddEdit" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    Home Branch</td>
                                <td class="style5">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlFindHomeBranch" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnRefresh" runat="server" SkinID="RefreshButton" 
                                        Text="Refresh" OnClick="btnRefresh_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" 
                            Width="100%" AutoGenerateColumns="False" DataSourceID="sdsMaster" 
                            AllowPaging="True" AllowSorting="True" onrowcreated="gvwMaster_RowCreated" 
                            onrowcommand="gvwMaster_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate> 
                                </asp:TemplateField>
<%--                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
--%>                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
                            SelectCommand="proc_GetAllEmployees" SelectCommandType="StoredProcedure" 
                            onselecting="sdsMaster_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="Barcode" Type="String" />
                                <asp:Parameter Name="Name" Type="String" />
                                <asp:Parameter Name="HomeBranchID" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2">
                        User Name</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                     </td>
                </tr>           
                <tr>
                    <td class="style2">
                        Barcode</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="textbox" 
                            MaxLength="50" ValidationGroup="AddEdit" Width="100px" />
                        <asp:RequiredFieldValidator ID="rqvBarcode" runat="server" 
                            ControlToValidate="txtBarcode" CssClass="errorMessage" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;Barcode&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Home Branch</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlHomeBranch" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqvHomeBranch" runat="server" 
                            ControlToValidate="ddlHomeBranch" CssClass="errorMessage" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;Home Branch&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        First Name</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="textbox" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvFirstName" runat="server" 
                            ControlToValidate="txtFirstName" CssClass="errorMessage" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;First Name&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Last Name</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="textbox" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Email</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Phone</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="textbox" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Photo</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:FileUpload ID="fupPhoto" runat="server" Width="350px" CssClass="textbox" />
                        &nbsp;
                        <asp:CheckBox ID="chkDeletePhoto" runat="server" Text="Delete current photo" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <asp:Image ID="imgPhoto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Text="This employee is active" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkCanApproveDocument" runat="server" 
                            Text="Can approve document" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkCanEditActiveContract" runat="server" 
                            Text="Can edit active contract" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkCanReprint" runat="server" 
                            Text="Can reprint invoice" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" SkinID="SaveButton"  
                            EnableViewState="false" onclick="btnSave_Click" ValidationGroup="AddEdit"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" SkinID="CancelButton"  
                            EnableViewState="false"  ValidationGroup="AddEdit" CausesValidation="false"
                            OnClientClick="return confirm('Are you sure want to cancel ?')" 
                            onclick="btnCancel_Click" />
                        </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>    
</asp:Content>


<asp:Content ID="Content5" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
    .style1
    {
        width: 100%;
    }
        .style2
        {
            width: 140px;
        }
        .style3
        {
            width: 1px;
        }
        .style4
        {
            width: 125px;
        }
        .style5
        {
            width: 3px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#<%= chkDeletePhoto.ClientID %>").click(
                function () {
                    if (this.checked) {
                        $("#<%= fupPhoto.ClientID %>").hide("slow");
                        $("#<%= imgPhoto.ClientID %>").hide("slow");
                    }
                    else {
                        $("#<%= fupPhoto.ClientID %>").show("slow");
                        $("#<%= imgPhoto.ClientID %>").show("slow");
                    }
                });
        });
    </script>
</asp:Content>




