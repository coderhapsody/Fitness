<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true" CodeFile="PaymentManual.aspx.cs" Inherits="PaymentManual" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 130px;
        }

        .auto-style2 {
            width: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Payment
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ScriptManager ID="scmScriptManager" runat="server" />
    <div>
        <table style="width: 100%">
            <tr>
                <td class="auto-style1">Invoice No.</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="auto-style1">Employee</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblEmployee" runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="auto-style1">Invoice Date</td>
                <td class="auto-style2">:</td>
                <td>
                    <ew:CalendarPopup ID="calInvoiceDate" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>      
                </td>
            </tr>
            <tr>
                <td class="auto-style1">Payment Date</td>
                <td class="auto-style2">:</td>
                <td>
                    <ew:CalendarPopup ID="calPaymentDate" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>      
                </td>
            </tr>
            <tr>
                <td class="auto-style1">Customer</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblCustomer" runat="server"></asp:Label>

                </td>
            </tr>
            <tr style="display:none;">
                <td class="auto-style1">Invoice Type</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblInvoiceType" runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="auto-style1">Notes</td>
                <td class="auto-style2">:</td>
                <td>

                    <asp:Label ID="lblNotes" runat="server"></asp:Label>

                </td>
            </tr>
        </table>
        <asp:GridView ID="gvwInvoice" runat="server" SkinID="GridViewDefaultSkin"
            Width="100%" AutoGenerateColumns="false"
            OnRowDataBound="gvwInvoice_RowDataBound">
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
    </div>
    <asp:Panel ID="pnlAuth" runat="server" Style="display: block;" BorderStyle="Solid"
        BorderWidth="2px" BorderColor="Black" Width="300px" BackColor="White">
        <table style="width: 100%">
            <tr>
                <td>
                    Select Item to add:</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlItemType" runat="server" CssClass="dropdown">
                    </asp:DropDownList>
                    <AjaxToolkit:CascadingDropDown ID="ddlItemType_CascadingDropDown" runat="server" Category="ItemType" Enabled="True" PromptText="Select item type" ServiceMethod="GetItemTypes" ServicePath="AjaxWebService.asmx" TargetControlID="ddlItemType">
                    </AjaxToolkit:CascadingDropDown>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlItem" runat="server" CssClass="dropdown">
                    </asp:DropDownList>
                    <AjaxToolkit:CascadingDropDown ID="ddlItem_CascadingDropDown" runat="server" Category="Item" Enabled="True" LoadingText="Loading, please wait" ParentControlID="ddlItemType" PromptText="Select item type first" ServiceMethod="GetItemsByType" ServicePath="AjaxWebService.asmx" TargetControlID="ddlItem">
                    </AjaxToolkit:CascadingDropDown>                    
                    <br />
                    <asp:RequiredFieldValidator ID="rqvItemType" runat="server" ErrorMessage="Item Type must be specified" ControlToValidate="ddlItemType" ValidationGroup="Charge" Display="Dynamic" CssClass="errorMessage" /><br />
                    <asp:RequiredFieldValidator ID="rqvItem" runat="server" ErrorMessage="Item must be specified" ControlToValidate="ddlItem" ValidationGroup="Charge" Display="Dynamic" CssClass="errorMessage" />
                </td>
            </tr>
            <tr>
                <td class="style24">Quantity: <ew:NumericBox ID="txtQuantity" RealNumber="false" PositiveNumber="true"  runat="server" CssClass="textbox" Width="50px" /><br /><asp:RequiredFieldValidator ID="rqvQuantity" runat="server" ErrorMessage="Quantity must be specified" ControlToValidate="txtQuantity" ValidationGroup="Charge" CssClass="errorMessage" /> </td>
            </tr>
            <tr>
                <td class="style24">Unit Price: <ew:NumericBox ID="txtUnitPrice" RealNumber="false" PositiveNumber="true" runat="server" CssClass="textbox" /><br /><asp:RequiredFieldValidator ID="rqvUnitCharge" runat="server" ErrorMessage="Unit Price must be specified" ControlToValidate="txtUnitPrice" ValidationGroup="Charge" CssClass="errorMessage" /></td>
            </tr>
            <tr>
                <td class="style24">&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStatusItem" runat="server" EnableViewState="false" />
                    <br />
                    <asp:Button ID="btnPopupOK" runat="server" CausesValidation="true" CssClass="button"
                        EnableViewState="False" Text="Add" OnClick="btnPopupOK_Click" ValidationGroup="Charge" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnPopupCancel" runat="server" CausesValidation="False" CssClass="button"
                        EnableViewState="False" Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <AjaxToolkit:ModalPopupExtender ID="mopAuth" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="btnPopupCancel" DropShadow="True" PopupControlID="pnlAuth" PopupDragHandleControlID="pnlAuth"
        TargetControlID="btnAddCharge">
    </AjaxToolkit:ModalPopupExtender>
    <asp:Button ID="btnAddCharge" runat="server" CssClass="button" Text="Add Charge" />
    <br />
    <br />
    <div>
        <asp:UpdatePanel ID="updPayment" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="payment">
                    <table class="style1">
                        <tr style="font-weight: bold;">
                            <td class="style34">Payment Type:
                            </td>
                            <td class="style33">Amount:
                            </td>
                            <td class="style28">Approval Code:
                            </td>
                            <td class="style28">Notes:</td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style34">
                                <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="dropdown"
                                    ValidationGroup="Payment">
                                </asp:DropDownList>
                                <AjaxToolkit:CascadingDropDown ID="ddlPaymentType_CascadingDropDown" PromptText="Select payment type"
                                    Category="PaymentType" ServicePath="AjaxWebService.asmx" ServiceMethod="GetPaymentTypes"
                                    runat="server" Enabled="True" TargetControlID="ddlPaymentType">
                                </AjaxToolkit:CascadingDropDown>
                                <asp:DropDownList ID="ddlCreditCardType" runat="server" CssClass="dropdown"
                                    ValidationGroup="Payment">
                                </asp:DropDownList>
                                <AjaxToolkit:CascadingDropDown ID="ddlCreditCardType_CascadingDropDown" Category="CreditCardType"
                                    ParentControlID="ddlPaymentType" PromptText="" LoadingText="Loading, please wait"
                                    ServicePath="AjaxWebService.asmx" ServiceMethod="GetCreditCardTypesByPaymentType"
                                    runat="server" Enabled="True" TargetControlID="ddlCreditCardType">
                                </AjaxToolkit:CascadingDropDown>
                            </td>
                            <td class="style33">
                                <ew:NumericBox ID="txtPaymentAmount" runat="server" CssClass="textbox" Width="100px"
                                    RealNumber="true" PositiveNumber="true" ValidationGroup="Payment" />
                            </td>
                            <td class="style28">
                                <asp:TextBox ID="txtApprovalCode" runat="server" CssClass="textbox"
                                    Width="150px" ValidationGroup="Payment"></asp:TextBox>
                            </td>
                            <td class="style28">
                                <asp:TextBox ID="txtPaymentNotes" runat="server" CssClass="textbox"
                                    MaxLength="300" Width="250px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnAddPayment" runat="server" CssClass="button" Text="Add Payment"
                                    OnClick="btnAddPayment_Click" ValidationGroup="Payment" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style34">
                                <asp:RequiredFieldValidator ID="rqvBillingType" runat="server" ControlToValidate="ddlPaymentType"
                                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Payment Type&lt;/b&gt; must be specified"
                                    SetFocusOnError="True" ValidationGroup="Payment"></asp:RequiredFieldValidator>
                            </td>
                            <td class="style33">
                                <asp:RequiredFieldValidator ID="rqvBillingType0" runat="server" ControlToValidate="txtPaymentAmount"
                                    CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Amount&lt;/b&gt; must be specified"
                                    SetFocusOnError="True" ValidationGroup="Payment"></asp:RequiredFieldValidator>
                            </td>
                            <td class="style28">&nbsp;
                            </td>
                            <td class="style28">&nbsp;</td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvwPayment" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
                        AutoGenerateColumns="false" OnRowCreated="gvwPayment_RowCreated"
                        OnRowCommand="gvwPayment_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" />
                            <asp:BoundField DataField="PaymentType" HeaderText="PaymentType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="CreditCardType" HeaderText="CreditCardType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:###,##0.00}"
                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="ApprovalCode" HeaderText="ApprovalCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Notes" HeaderText="Notes" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnbDeleteItem" runat="server" Text="Delete" CommandName="DeletePayment"
                                        CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure want to delete this row?')" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            .: No Payment Data :.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <h4>Total Payment:
                        <asp:Label ID="lblTotalPayment" runat="server" />
                </h4>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAddPayment" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvwPayment" EventName="RowCommand" />
            </Triggers>
        </asp:UpdatePanel>

        <br />
        <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
        <br />
        <asp:Button ID="btnSave" runat="server" SkinID="SaveButton"
            EnableViewState="false" OnClick="btnSave_Click"
            ValidationGroup="ExistingMember" />
    </div>
</asp:Content>

