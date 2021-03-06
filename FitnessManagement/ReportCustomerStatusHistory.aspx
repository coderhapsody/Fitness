﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ReportCustomerStatusHistory.aspx.cs" Inherits="ReportMonthlyReport" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Customer Status History         
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
            <td class="style1">Start Date</td>
            <td class="style2">:</td>
            <td>From
                 <ew:CalendarPopup ID="calDateFrom" runat="server" SkinID="Calendar">
                     <TextBoxLabelStyle CssClass="textbox" />
                 </ew:CalendarPopup>
                &nbsp;
                To
                <ew:CalendarPopup ID="calDateTo" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="style1">Status</td>
            <td class="style2">:</td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropdown" />
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
            var customerStatusID = 0;
            $("#<%= btnViewReport.ClientID %>").click(function () {
                $("#<%= ddlBranch.ClientID %> option:selected").each(function () {
                    id = $(this).val();
                });


                $("#<%= ddlStatus.ClientID %> option:selected").each(function () {
                    customerStatusID = $(this).val();
                });

                var dateFrom = moment($("#cphMainContent_calDateFrom_textBox").get(0).value, "DD/MM/YYYY");
                var dateTo = moment($("#cphMainContent_calDateTo_textBox").get(0).value, "DD/MM/YYYY");
                
                
                showSimplePopUp('PrintPreview.aspx?RDL=CustomerStatusHistory&BranchID=' + id + '&StartDateFrom=' + dateFrom.format("YYYY-MM-DD") + '&StartDateTo=' + dateTo.format("YYYY-MM-DD") + '&CustomerStatusID=' + customerStatusID);

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
