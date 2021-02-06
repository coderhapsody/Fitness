<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ReportCreditCardChangeHistory.aspx.cs" Inherits="ReportCreditCardChangeHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 110px;
        }
        .auto-style2 {
            width: 7px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Credit Card Change History
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
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
            <td class="auto-style1">Month</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdown" />                
            </td>
        </tr>
            <tr>
            <td class="auto-style1">Year</td>
            <td class="auto-style2">:</td>
            <td>
            <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown" />                
            </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>
                <asp:Button ID="btnViewReport" runat="server" CssClass="button"
                    Text="View Report" />
            </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
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

                $("#<%= ddlMonth.ClientID %> option:selected").each(function () {
                    month = $(this).val();
                });

                $("#<%= ddlYear.ClientID %> option:selected").each(function () {
                    year = $(this).val();
                });

                showSimplePopUp('PrintPreview.aspx?RDL=CreditCardChangeHistory&BranchID=' + id + '&Month=' + month + '&Year=' + year);

                return false;
            });
        });
    </script>
</asp:Content>

