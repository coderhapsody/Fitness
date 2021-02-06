<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="MasterPackage.aspx.cs" Inherits="MasterPackage" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Package
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="style1">
                <tr>
                    <td>
                        <table class="style1">
                            <tr>
                                <td class="style9">Name :</td>
                                <td class="style3">:</td>
                                <td>
                                    <asp:TextBox ID="txtFindName" runat="server" CssClass="textbox" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style9">&nbsp;</td>
                                <td class="style3">&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnRefresh" runat="server" EnableViewState="False"
                                        OnClick="btnRefresh_Click" SkinID="RefreshButton" Text="Refresh" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false"
                            OnClick="lnbAddNew_Click" SkinID="AddNewButton" Text="Add New" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false"
                            OnClick="lnbDelete_Click"
                            OnClientClick="return confirm('Delete marked row(s) ?')" SkinID="DeleteButton"
                            Text="Delete" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
                            AutoGenerateColumns="False" DataSourceID="sdsMaster" AllowPaging="True" AllowSorting="True"
                            OnRowCreated="gvwMaster_RowCreated" OnRowCommand="gvwMaster_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PackageDuesInMonth" HeaderText="Dues In Month" SortExpression="PackageDuesInMonth"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="StandardPrice" HeaderText="Standard Price" SortExpression="StandardPrice" DataFormatString="{0:###,##0}"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="IsActive" HeaderText="Is Active" SortExpression="IsActive"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="OpenEnd" HeaderText="Open End" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="OpenEnd" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnbDefineClass" runat="server" CommandName="DefineClass" Text="Class"
                                            CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow"
                                            CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                            SelectCommand="proc_GetAllPackages" SelectCommandType="StoredProcedure"
                            OnSelecting="sdsMaster_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="Name" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <table class="style1">
                            <tr>
                                <td class="style2">Name
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="textbox" Width="300px"
                                        MaxLength="50" ValidationGroup="AddEdit" />
                                    <asp:RequiredFieldValidator ID="rqvName" runat="server" ControlToValidate="txtName"
                                        EnableViewState="false"
                                        ErrorMessage="<b>Description</b> must be specified" ValidationGroup="AddEdit"
                                        CssClass="errorMessage" SetFocusOnError="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">Dues in Month
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDuesInMonth" runat="server" CssClass="dropdown">
                                        <asp:ListItem Text="1" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:CheckBox ID="chkOpenEnd" runat="server" Text="Open End" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">Freeze Fee Amount
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <ew:NumericBox ID="txtFreezeFee" runat="server" CssClass="textbox" Width="300px"
                                        MaxLength="50" ValidationGroup="AddEdit" />
                                    <asp:RequiredFieldValidator ID="rqvFreezeFee" runat="server" ControlToValidate="txtFreezeFee"
                                        EnableViewState="false"
                                        ErrorMessage="<b>Freeze fee</b> must be specified" ValidationGroup="AddEdit"
                                        CssClass="errorMessage" SetFocusOnError="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">&nbsp;
                                </td>
                                <td class="style3">&nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsActive" runat="server" Text="This package is active" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="style1">
                            <tr>
                                <td class="style8">&nbsp;</td>
                                <td class="style7">&nbsp;</td>
                                <td class="style7">&nbsp;</td>
                                <td class="style7">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style11">Item:&nbsp;
                                    <asp:DropDownList ID="ddlItem" runat="server" CssClass="dropdown"
                                        ValidationGroup="AddDetail">
                                    </asp:DropDownList><br />
                                    <asp:RequiredFieldValidator ID="rqvItem" runat="server"
                                        ControlToValidate="ddlItem" CssClass="errorMessage"
                                        EnableViewState="false"
                                        ErrorMessage="&lt;b&gt;Item&lt;/b&gt; must be specified" SetFocusOnError="true"
                                        ValidationGroup="AddDetail" />
                                </td>
                                <td class="style13">Quantity:
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="textbox" Width="50px"
                                        ValidationGroup="AddDetail"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="rqvQuantity" runat="server"
                                        ControlToValidate="txtQuantity" CssClass="errorMessage"
                                        EnableViewState="false"
                                        ErrorMessage="&lt;b&gt;Quantity&lt;/b&gt; must be specified"
                                        SetFocusOnError="true" ValidationGroup="AddDetail" />
                                </td>
                                <td class="style13">Unit Price :<asp:TextBox ID="txtUnitPrice" runat="server" CssClass="textbox"
                                    ValidationGroup="AddDetail" Width="100px"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="rqvUnitPrice" runat="server"
                                        ControlToValidate="txtUnitPrice" CssClass="errorMessage"
                                        EnableViewState="false"
                                        ErrorMessage="&lt;b&gt;Unit Price&lt;/b&gt; must be specified"
                                        SetFocusOnError="true" ValidationGroup="AddDetail" />
                                </td>
                                <td class="style13">
                                    <asp:Button ID="btnAddDetail" runat="server" CssClass="button"
                                        EnableViewState="False" Text="Add Detail" ValidationGroup="AddDetail"
                                        OnClick="btnAddDetail_Click" />
                                </td>
                                <td class="style14"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwDetail" runat="server" SkinID="GridViewDefaultSkin"
                            Width="800px" AutoGenerateColumns="False"
                            OnRowCommand="gvwDetail_RowCommand" OnRowCreated="gvwDetail_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ItemBarcode" HeaderText="ItemBarcode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0}" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnbDelete" runat="server" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' ToolTip="Delete this row" Text="Delete" OnClientClick="return confirm('Are you sure want to delete this row ?')" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessageDetail" runat="server" EnableViewState="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" EnableViewState="false"
                            OnClick="btnSave_Click" SkinID="SaveButton" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="false"
                            EnableViewState="false" OnClick="btnCancel_Click"
                            OnClientClick="return confirm('Are you sure want to cancel ?')"
                            SkinID="CancelButton" ValidationGroup="AddEdit" />
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit2" runat="server">

            <table class="ui-accordion">
                <tr>
                    <td class="style2">Package Name</td>
                    <td class="auto-style1">:</td>
                    <td>
                        <asp:Label ID="lblPackageName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;</td>
                    <td class="auto-style1">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <asp:CheckBoxList ID="cblClass" runat="server">
            </asp:CheckBoxList>
            <br />
            <asp:Button ID="btnSaveClassPackage" runat="server" Text="Save" CssClass="button" OnClick="btnSaveClassPackage_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCancelPackage" runat="server" CssClass="button" OnClick="btnCancelPackage_Click" Text="Cancel" />
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 140px;
        }

        .style3 {
            width: 1px;
        }

        .style7 {
            width: 231px;
        }

        .style8 {
            width: 235px;
        }

        .style9 {
            width: 120px;
        }

        .style11 {
            width: 235px;
            height: 24px;
        }

        .style13 {
            width: 231px;
            height: 24px;
            text-align: left;
        }

        .style14 {
            height: 24px;
        }

        .auto-style1 {
            width: 2px;
        }
    </style>
</asp:Content>
