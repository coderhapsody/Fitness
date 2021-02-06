<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ReportProspectsByDate.aspx.cs" Inherits="ReportProspectsByDate" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 90px;
        }

        .auto-style2 {
            width: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Prospects by Date
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="auto-style1">Branch</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">From Date</td>
            <td class="auto-style2">:</td>
            <td>
                <ew:CalendarPopup ID="calDateFrom" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
                <tr>
            <td class="auto-style1">To Date</td>
            <td class="auto-style2">:</td>
            <td>
                <ew:CalendarPopup ID="calDateTo" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>
                <asp:Button ID="btnRefresh" runat="server" CssClass="button" EnableViewState="False" Text="Refresh" />
            </td>
        </tr>
    </table>    
    
    <script>
        $(function() {
            $("#<%=btnRefresh.ClientID%>").click(function(e) {
                e.preventDefault();
                var branchID = $("#<%=ddlBranch.ClientID%>").val();
                var dateFrom = moment($("#cphMainContent_calDateFrom_textBox").get(0).value, "DD/MM/YYYY");
                var dateTo = moment($("#cphMainContent_calDateTo_textBox").get(0).value, "DD/MM/YYYY");

                showSimplePopUp('PrintPreview.aspx?RDL=ProspectsByDate&BranchID=' + branchID + '&FromDate=' + dateFrom.format("YYYY-MM-DD") + '&ToDate=' + dateTo.format("YYYY-MM-DD"));
            });
        });
    </script>
</asp:Content>



