<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="ExistingMemberCompleted.aspx.cs" Inherits="ExistingMemberCompleted"
    StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Non-Membership Invoicing
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td class="style1">
                            Branch
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Invoice No.
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Customer Barcode
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerBarcode" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Customer Name
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Notes
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblNotes" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Discount Value</td>
                        <td class="style4">
                            :</td>
                        <td>
                            <asp:Label ID="lblDiscountValue" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top; width: 50%">
                <table width="100%">
                    <tr>
                        <td class="style5">
                            Purchase Date
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblPurchaseDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            Sales
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblSales" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            Status
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblStatusInvoice" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvwInvoice" runat="server" SkinID="GridViewDefaultSkin" 
                    Width="100%" AutoGenerateColumns="false" 
                    onrowdatabound="gvwInvoice_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ItemBarcode" HeaderText="ItemBarcode" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:###,##0.00}"
                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" DataFormatString="{0:###,##0.00}"
                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Discount" HeaderText="Discount" DataFormatString="{0:###,##0.00}"
                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                        <asp:TemplateField HeaderText="Taxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Convert.ToBoolean(Eval("IsTaxable")) ? "Taxed" : "Not Taxed" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:###,##0.00}"
                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                </asp:GridView>
                <p>
                <strong>Total Before Tax:&nbsp;</strong><asp:Label ID="lblTotalBeforeTax" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <strong>Total Tax:&nbsp;</strong><asp:Label ID="lblTotalTax" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <strong>Total After Tax:&nbsp;</strong><asp:Label ID="lblTotalInvoice" runat="server" />
                </p>
                <br />
                <table class="ui-accordion">
                    <tr>
                        <td class="style6">
                            Payment No.
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblPaymentNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style6">
                            Payment Date
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblPaymentDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style6">
                            Status
                        </td>
                        <td class="style4">
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblStatusPayment" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvwPayment" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
        AutoGenerateColumns="false" OnRowCreated="gvwPayment_RowCreated">
        <Columns>
            <asp:BoundField DataField="PaymentType" HeaderText="PaymentType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
            <asp:BoundField DataField="CreditCardType" HeaderText="CreditCardType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
            <asp:BoundField DataField="ApprovalCode" HeaderText="ApprovalCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:###,##0.00}"
                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="Notes" HeaderText="Notes" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
        </Columns>
    </asp:GridView>
        <p>
        <strong>Total Payment:&nbsp;</strong><asp:Label ID="lblTotalPayment" runat="server" />
    </p>
    <br />
    <p>
        <asp:Button ID="btnPrint" runat="server" Text="Print Official Receipt" EnableViewState="false"
            CssClass="button" SkinID="ViewReportButton" />
        <asp:Button ID="btnClose" runat="server" Text="Close This Window" EnableViewState="false"
            CssClass="button" OnClientClick="window.close()" />
    </p>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1
        {
            width: 120px;
        }
        .style4
        {
            width: 1px;
        }
        .style5
        {
            width: 140px;
        }
        .style6
        {
            width: 130px;
        }
    </style>
</asp:Content>
