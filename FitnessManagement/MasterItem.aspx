<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="MasterItem.aspx.cs" Inherits="MasterItem" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Item
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
                                <td>
                                    <asp:TextBox ID="txtFindBarcode" runat="server" CssClass="textbox" 
                                        MaxLength="50" ValidationGroup="AddEdit" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    Description</td>
                                <td class="style5">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="txtFindDescription" runat="server" CssClass="textbox" 
                                        MaxLength="50" ValidationGroup="AddEdit" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    Item Type</td>
                                <td class="style5">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlFindItemType" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnRefresh" runat="server" EnableViewState="false" 
                                        onclick="btnRefresh_Click" SkinID="RefreshButton" 
                                        ValidationGroup="AddEdit" />
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
                        <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false" 
                            onclick="lnbAddNew_Click" SkinID="AddNewButton" Text="Add New" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false" 
                            onclick="lnbDelete_Click" 
                            OnClientClick="return confirm('Delete marked row(s) ?')" SkinID="DeleteButton" 
                            Text="Delete" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Account" HeaderText="Account" SortExpression="Account" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" SortExpression="UnitPrice" DataFormatString="{0:###,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
                            SelectCommand="proc_GetAllItems" SelectCommandType="StoredProcedure" 
                            onselecting="sdsMaster_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="Barcode" Type="String" />
                                <asp:Parameter Name="Description" Type="String" />
                                <asp:Parameter Name="ItemTypeID" Type="Int32" />
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
                        Barcode</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="textbox" MaxLength="50" 
                            ValidationGroup="AddEdit" Width="100px" />
                        <asp:RequiredFieldValidator ID="rqvBarcode" runat="server" 
                            ControlToValidate="txtBarcode" CssClass="errorMessage" EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;Barcode&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                     </td>
                </tr>           
                <tr>
                    <td class="style2">
                        Description</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" 
                            MaxLength="50" ValidationGroup="AddEdit" Width="300px" />
                        <asp:RequiredFieldValidator ID="rqvDescription" runat="server" 
                            ControlToValidate="txtDescription" CssClass="errorMessage" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;Description&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Account</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlAccount" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqvAccount" runat="server" 
                            ControlToValidate="ddlAccount" CssClass="errorMessage" EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;Account&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Item Type</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlItemType" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqvItemType" runat="server" 
                            ControlToValidate="ddlItemType" CssClass="errorMessage" EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;Item Type&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Standard Unit Price</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <ew:NumericBox ID="txtUnitPrice" runat="server" CssClass="textbox" MaxLength="50" 
                            ValidationGroup="AddEdit" Width="120px" PositiveNumber="True"  
                            TruncateLeadingZeros="True" />
                        <asp:RequiredFieldValidator ID="rqvUnitPrice" runat="server" 
                            ControlToValidate="txtUnitPrice" CssClass="errorMessage" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;Standard Unit Price&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Text="This item is active" />
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
            width: 110px;
        }
        .style5
        {
            width: 2px;
        }
    </style>
</asp:Content>

