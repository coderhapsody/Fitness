<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ReportFormFreeze.aspx.cs" Inherits="ReportFormFreeze" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 140px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Freeze Form
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table class="fullwidth">
        <tr>
            <td class="auto-style1">Barcode:</td>
            <td>
                <asp:TextBox runat="server" ID="txtBarcode" CssClass="textbox"></asp:TextBox>
                <asp:HyperLink ID="hypLookUpCustomer" NavigateUrl="#" runat="server" onclick="showPromptPopUp('PromptCustomer.aspx?', this.previousSibling.previousSibling.id, 550, 900);">Look Up</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Date:</td>
            <td>From 
                <ew:CalendarPopup ID="calDateFrom" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
                To
                <ew:CalendarPopup ID="calDateTo" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="auto-style1"></td>
            <td>
                <asp:Button runat="server" ID="btnViewForm" Text="View Form" CssClass="button" />
            </td>
        </tr>
    </table>
    
    <script>
        $(function() {
            $("#<%=btnViewForm.ClientID%>").click(function(e) {
                e.preventDefault();

                var barcode = $("#<%=txtBarcode.ClientID%>").val();

                if (barcode.length == 0) {
                    alert("Customer barcode must be specified.");
                    return;
                }

                var dateFrom = moment($("#cphMainContent_calDateFrom_textBox").get(0).value, "DD/MM/YYYY");
                var dateTo = moment($("#cphMainContent_calDateTo_textBox").get(0).value, "DD/MM/YYYY");
                
                showSimplePopUp('PrintPreview.aspx?RDL=FreezeForm&Barcode=' + barcode + "&FreezeDateFrom=" + dateFrom.format("YYYY-MM-DD") + "&FreezeDateTo=" + dateTo.format("YYYY-MM-DD"));
            });
        });
    </script>
</asp:Content>

