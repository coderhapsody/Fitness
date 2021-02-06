<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ReportUnpaidContracts.aspx.cs" Inherits="ReportUnpaidContracts" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Unpaid Contracts
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">
                Branch</td>
            <td class="style2">
                :</td>
            <td>
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                <asp:Button ID="btnViewReport" runat="server" CssClass="button" 
                    Text="View Report" />
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var id = 0;
            $("#<%= btnViewReport.ClientID %>").click(function () {
                $("#<%= ddlBranch.ClientID %> option:selected").each(function () {
                    id = $(this).val();
                    //alert($(this).val());
                });
                showSimplePopUp('PrintPreview.aspx?RDL=UnpaidContract&BranchID=' + id);

                return false;
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
        .style1
        {
            width: 130px;
        }
        .style2
        {
            width: 4px;
        }
    </style>
</asp:Content>


