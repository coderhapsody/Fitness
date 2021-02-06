<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="MasterTimeSlot.aspx.cs" Inherits="MasterTimeSlot" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 130px;
        }

        .auto-style2 {
            width: 2px;
        }

        .auto-style3 {
            width: 110px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Classes Time Slot
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <AjaxToolkit:ToolkitScriptManager ID="tsmScriptManager" runat="server" />
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="View1" runat="server">

            <table class="ui-accordion">
                <tr>
                    <td class="auto-style1">Branch</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" CssClass="dropdown" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Day of Week</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:DropDownList ID="ddlDayOfWeek" CssClass="dropdown" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:Button ID="btnRefresh" runat="server" Text="Submit" CssClass="button" OnClick="btnRefresh_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>

        </asp:View>
        <asp:View ID="View2" runat="server">

            <table class="ui-accordion">
                <tr>
                    <td class="auto-style3">Start Time</td>
                    <td class="auto-style2">:</td>
                    <td>Time Start:
                        <asp:TextBox ID="txtTimeStart" runat="server" Width="50px" />
                        <AjaxToolkit:MaskedEditExtender ID="mexTimeStart" runat="server" TargetControlID="txtTimeStart" Mask="99:99" MessageValidatorTip="true" MaskType="Time" ErrorTooltipEnabled="true" AcceptAMPM="false" InputDirection="LeftToRight" />
                        <AjaxToolkit:MaskedEditValidator ID="mevTimeStart" runat="server" ControlExtender="mexTimeStart" ControlToValidate="txtTimeStart" IsValidEmpty="false" EmptyValueMessage="Time Start must be specified" CssClass="errorMessage" ValidationGroup="TimeStartEnd" Display="None" />
                        <AjaxToolkit:ValidatorCalloutExtender ID="vceTimeStart" runat="server" TargetControlID="mevTimeStart" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Time End:
                        <asp:TextBox ID="txtTimeEnd" runat="server" Width="50px" />
                        <AjaxToolkit:MaskedEditExtender ID="mexTimeEnd" runat="server" TargetControlID="txtTimeEnd" Mask="99:99" MessageValidatorTip="true" MaskType="Time" ErrorTooltipEnabled="true" AcceptAMPM="false" InputDirection="LeftToRight" />
                        <AjaxToolkit:MaskedEditValidator ID="mevTimeEnd" runat="server" ControlExtender="mexTimeEnd" ControlToValidate="txtTimeStart" IsValidEmpty="false" EmptyValueMessage="Time End must be specified" CssClass="errorMessage" ValidationGroup="TimeStartEnd" Display="None" />
                        <AjaxToolkit:ValidatorCalloutExtender ID="vceTimeEnd" runat="server" TargetControlID="mevTimeEnd" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td style="font-weight: 700">
                        <asp:Button ID="btnSave" runat="server" Text="Save" EnableViewState="false" CssClass="button" OnClick="btnSave_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnDelete" runat="server" CssClass="button" EnableViewState="false" Text="Delete" OnClick="btnDelete_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:GridView ID="gvwData" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsTimeSlot" OnRowCreated="gvwData_RowCreated" AllowSorting="True">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                                <asp:BoundField DataField="StartTime" HeaderText="Time" SortExpression="StartTime" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsTimeSlot" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetTimeSlot" SelectCommandType="StoredProcedure" OnSelecting="sdsTimeSlot_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="BranchID" Type="Int32" />
                                <asp:Parameter Name="DayOfWeek" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>

        </asp:View>
    </asp:MultiView>
</asp:Content>

