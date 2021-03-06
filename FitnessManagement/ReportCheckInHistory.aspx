﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ReportCheckInHistory.aspx.cs" Inherits="ReportCheckInHistory" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Check-In History
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
            <td class="style1">From Date</td>
            <td class="style2">:</td>
            <td>
                <ew:CalendarPopup ID="calDateFrom" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="style1">To Date</td>
            <td class="style2">:</td>
            <td>
                <ew:CalendarPopup ID="calDateTo" runat="server" SkinID="Calendar">
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

                var dateFrom = moment($("#cphMainContent_calDateFrom_textBox").get(0).value, "DD/MM/YYYY");
                var dateTo = moment($("#cphMainContent_calDateTo_textBox").get(0).value, "DD/MM/YYYY");
                
                showSimplePopUp('PrintPreview.aspx?RDL=CheckInHistory&BranchID=' + id + '&FromDate=' + dateFrom.format("YYYY-MM-DD") + '&ToDate=' + dateTo.format("YYYY-MM-DD"));

                return false;
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 130px;
        }

        .style2 {
            width: 4px;
        }
    </style>
</asp:Content>
