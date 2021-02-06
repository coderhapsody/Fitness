<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="ManageAlerts.aspx.cs" Inherits="ManageAlerts" StylesheetTheme="Workspace" %>


<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Alerts
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <AjaxToolkit:ToolkitScriptManager ID="tsmAjaxToolkitScriptManager" runat="server" />
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 180px">
                                    Filter by</td>
                                <td style="width: 1px">
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFilter" runat="server" CssClass="dropdown">
                                        <asp:ListItem Selected="True" Value="0">Show All Alert(s)</asp:ListItem>
                                        <asp:ListItem Value="1">Show Only Active Alert(s)</asp:ListItem>
                                        <asp:ListItem Value="2">Show Only Inactive Alert(s)</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px">
                                    &nbsp;
                                </td>
                                <td style="width: 1px">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnRefresh" EnableViewState="false" CommandName="FormCommand" CommandArgument="Refresh"
                                        CssClass="button" runat="server" Text="Refresh" ToolTip="Show data with specified criteria"
                                        OnCommand="Buttons_Command" ValidationGroup="View" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnbAddNew" runat="server" CommandArgument="AddNew" CommandName="FormCommand"
                            EnableViewState="false" OnCommand="Buttons_Command" SkinID="AddNewButton" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" CommandArgument="Delete" CommandName="FormCommand" OnClientClick="return confirm('Delete marked row(s) ?');"
                            EnableViewState="false" OnCommand="Buttons_Command" SkinID="DeleteButton" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" AllowSorting="true"
                            Width="100%" AutoGenerateColumns="False" DataSourceID="sdsMaster" OnRowCreated="gvwMaster_RowCreated"
                            OnRowCommand="gvwMaster_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="ID" />
                                <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="Description" />
                                <asp:BoundField DataField="StartDate" HeaderText="StartDate" HeaderStyle-HorizontalAlign="Left"
                                    HtmlEncode="false" DataFormatString="{0:dddd, dd MMM yyyy}"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="StartDate" />
                                <asp:BoundField DataField="EndDate" HeaderText="EndDate" HeaderStyle-HorizontalAlign="Left"
                                    HtmlEncode="false" DataFormatString="{0:dddd, dd MMM yyyy}"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="EndDate" />
                                <asp:CheckBoxField DataField="Active" HeaderText="Active" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" SortExpression="Active" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# ((System.Data.DataRowView)Container.DataItem)["ID"] %>' />
                                    </ItemTemplate> 
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                .: No Data :.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                            OnSelected="sdsMaster_Selected" OnSelecting="sdsMaster_Selecting" SelectCommand="proc_GetAllAlert_Paged"
                            SelectCommandType="StoredProcedure" SortParameterName="OrderByClause">
                            <SelectParameters>
                                <asp:Parameter Name="PageIndex" Type="Int32" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Direction="InputOutput" Name="RecordCount" Type="Int32" />
                                <asp:Parameter Name="ShowOnlyActiveAlerts" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <pager:PagerV2_8 ID="pgrMaster" runat="server" OnCommand="Pager_Command" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <asp:ValidationSummary ID="vsSummary" runat="server" EnableViewState="false" ValidationGroup="AddEdit"
                CssClass="errorMessage" ToolTip="Validation error" />
            <table style="width: 100%">
                <tr>
                    <td style="width: 160px">
                        Description
                    </td>
                    <td style="width: 1px">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" ValidationGroup="AddEdit"
                            Width="400px" Columns="60" Rows="8" TextMode="MultiLine" MaxLength="2000"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvDescription" runat="server" CssClass="errorMessage"
                            ControlToValidate="txtDescription" ErrorMessage="<strong>Description</strong> must be specified"
                            ToolTip="Description must be specified" Text="*" SetFocusOnError="true" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Start Date
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td class="style3">                        
                        <ew:CalendarPopup ID="CalendarPopup1" runat="server" SkinID="Calendar">
                            <TextBoxLabelStyle CssClass="textbox" />
                        </ew:CalendarPopup>
<%--                        &nbsp;<ew:CustomValidator ID="cvlDateFrom" runat="server" ErrorMessage="<strong>Date</strong> is invalid"
                            EnableViewState="false" ControlToValidate="CalendarPopup1" ValidationGroup="View"
                            CssClass="errorMessage" ClientValidationFunction="ValidateDate" ValidateEmptyText="true" />--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 160px">
                        End Date
                    </td>
                    <td style="width: 1px">
                        :
                    </td>
                    <td>
                        <ew:CalendarPopup ID="CalendarPopup2" runat="server" SkinID="Calendar">
                            <TextBoxLabelStyle CssClass="textbox" />
                        </ew:CalendarPopup>
<%--                        &nbsp;<ew:CustomValidator ID="cvlDateTo" runat="server" ErrorMessage="<strong>Date</strong> is invalid"
                            EnableViewState="false" ControlToValidate="CalendarPopup2" ValidationGroup="View"
                            CssClass="errorMessage" ClientValidationFunction="ValidateDate" ValidateEmptyText="true" />--%>
                        </td>
                </tr>
                <tr>
                    <td style="width: 160px">
                        &nbsp;</td>
                    <td style="width: 1px">
                        &nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkInfinite" runat="server" Text="Set as infinite" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 160px">
                        &nbsp;
                    </td>
                    <td style="width: 1px">
                        &nbsp;
                    </td>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Text="Set as active alert" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 160px">
                        &nbsp;
                    </td>
                    <td style="width: 1px">
                        &nbsp;
                    </td>
                    <td>                    
                        <asp:Button ID="btnSave" runat="server" SkinID="SaveButton" EnableViewState="false"
                            CommandName="FormCommand" CommandArgument="Save" ValidationGroup="AddEdit" OnCommand="Buttons_Command" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" SkinID="CancelButton" CommandName="FormCommand"
                            CommandArgument="Cancel" OnClientClick="return confirm('Cancel current operation ?')"
                            EnableViewState="false" CausesValidation="false" OnCommand="Buttons_Command" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            if ($("#<%= chkInfinite.ClientID %>").is(":checked")) {
                $("#<%= CalendarPopup2.ClientID %>").hide();
            }
            else {
                $("#<%= CalendarPopup2.ClientID %>").show();
            }
            $("#<%= chkInfinite.ClientID %>").click(
                    function () {
                        chk = $("#<%= chkInfinite.ClientID %>");
                        if (chk.is(":checked")) {
                            $("#<%= CalendarPopup2.ClientID %>").hide();
                        }
                        else {
                            $("#<%= CalendarPopup2.ClientID %>").show();
                        }
                    });
        });                                                        
    </script>
</asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
        .style1
        {
            width: 160px;
            height: 41px;
        }
        .style2
        {
            width: 1px;
            height: 41px;
        }
        .style3
        {
            height: 41px;
        }
    </style>
</asp:Content>


