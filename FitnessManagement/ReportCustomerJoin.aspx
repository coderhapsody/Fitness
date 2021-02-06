<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" StyleSheetTheme="Workspace" CodeFile="ReportCustomerJoin.aspx.cs" Inherits="ReportCustomerJoin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .style1 {
            width: 130px;
        }

        .style2 {
            width: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Customer Join
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">Branch</td>
            <td class="style2">:</td>
            <td>
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style1">From Join Date</td>
            <td class="style2">:</td>
            <td>
                <ew:CalendarPopup ID="calFromDate" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="style1">To Join Date</td>
            <td class="style2">:</td>
            <td>
                <ew:CalendarPopup ID="calToDate" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">&nbsp;</td>
            <td>
                <asp:Button ID="btnViewReport" runat="server" CssClass="button"
                    Text="View Report" />
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var id = 0;
            var month = 0;
            var year = 0;
            $("#<%= btnViewReport.ClientID %>").click(function () {
                $("#<%= ddlBranch.ClientID %> option:selected").each(function () {
                    id = $(this).val();
                });

                var fromDate = moment($("#cphMainContent_calFromDate_textBox").get(0).value, "DD/MM/YYYY");
                var toDate = moment($("#cphMainContent_calToDate_textBox").get(0).value, "DD/MM/YYYY");

                showSimplePopUp('PrintPreview.aspx?RDL=CustomerJoin&BranchID=' + id + '&FromDate=' + fromDate.format("YYYY-MM-DD") + '&ToDate=' + toDate.format("YYYY-MM-DD"));

                return false;
            });
        });
    </script>
</asp:Content>

