<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="MasterBillingType.aspx.cs" Inherits="MasterBillingType" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Billing Type
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="style1">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false" 
                            Text="Add New" SkinID="AddNewButton" onclick="lnbAddNew_Click"  />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false" 
                            Text="Delete" OnClientClick="return confirm('Delete marked row(s) ?')" 
                            SkinID="DeleteButton" onclick="lnbDelete_Click"  />
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
                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="AutoPayDay" HeaderText="Auto Pay Day" SortExpression="Description" HeaderStyle-HorizontalAlign="Left" />
                                <asp:CheckBoxField DataField="IsActive" HeaderText="Active" SortExpression="IsActive" HeaderStyle-HorizontalAlign="Left" />
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
                            SelectCommand="proc_GetAllBillingTypes" SelectCommandType="StoredProcedure">
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>

        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2">
                        Description</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox id="txtDescription" runat="server" CssClass="textbox" Width="300px" MaxLength="50" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator ID="rqvDescription" runat="server" ControlToValidate="txtDescription" EnableViewState="false" ErrorMessage="<b>Description</b> must be specified" ValidationGroup="AddEdit" CssClass="errorMessage" SetFocusOnError="true" />
                     </td>
                </tr>           
                <tr>
                    <td class="style2">
                        Auto Pay Day</td>
                    <td class="style3">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtAutoPayDay" runat="server" CssClass="textbox" 
                            MaxLength="2" ValidationGroup="AddEdit" Width="30px" />
                        &nbsp;<asp:RequiredFieldValidator ID="rqvAutoPayDay" runat="server" 
                            ControlToValidate="txtAutoPayDay" CssClass="errorMessage" 
                            EnableViewState="false" 
                            ErrorMessage="&lt;b&gt;Auto Pay Day&lt;/b&gt; must be specified" 
                            SetFocusOnError="true" ValidationGroup="AddEdit" />
                        <asp:RangeValidator ID="rnvAutoPayDay" runat="server"
                            ControlToValidate="txtAutoPayDay" CssClass="errorMessage"
                            EnableViewState="false"
                            ErrorMessage="&lt;b&gt;Auto Pay Day&lt;/b&gt; must be between 1 and 30" 
                            SetFocusOnError="true" ValidationGroup="AddEdit"
                            Type="Integer"
                            MinimumValue="1" MaximumValue="30" />

                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Text="Active" />  </td>
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
    </style>
</asp:Content>

