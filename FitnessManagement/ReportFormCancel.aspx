<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ReportFormCancel.aspx.cs" Inherits="ReportFormCancel" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 130px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Cancellation Form
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
     <table class="fullwidth">
         <tr>
             <td class="auto-style1">Customer Barcode: </td>
             <td><asp:TextBox runat="server" ID="txtBarcode" CssClass="textbox"/> 
                 <asp:HyperLink ID="hypLookUpCustomer" NavigateUrl="#" runat="server" onclick="showPromptPopUp('PromptCustomer.aspx?', this.previousSibling.previousSibling.id, 550, 900);">Look Up</asp:HyperLink>
             </td>
         </tr>
         <tr>
             <td class="auto-style1"></td>
             <td><asp:Button runat="server" ID="btnViewForm" Text="View Form" CssClass="button" /> </td>
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
                showSimplePopUp('PrintPreview.aspx?RDL=CancellationForm&Barcode=' + barcode);
            });
        });
    </script>
</asp:Content>

