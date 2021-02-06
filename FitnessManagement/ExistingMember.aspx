<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="ExistingMember.aspx.cs" Inherits="ExistingMember" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Non-Membership Invoicing
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <AjaxToolkit:ToolkitScriptManager ID="tsmScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="AjaxService.svc" />
        </Services>
    </AjaxToolkit:ToolkitScriptManager>
    <table class="style1">
        <tr>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style2">Branch
                        </td>
                        <td class="style3">:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown"
                                ValidationGroup="ExistingMember" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">Date
                        </td>
                        <td class="style3">:
                        </td>
                        <td>
                            <ew:CalendarPopup ID="calDate" runat="server" SkinID="Calendar">
                                <TextBoxLabelStyle CssClass="textbox" />
                            </ew:CalendarPopup>
                            <asp:CustomValidator ID="cuvDate" runat="server" ControlToValidate="calDate"
                                CssClass="errorMessage" EnableViewState="False" ErrorMessage="Cannot input back date"
                                OnServerValidate="cuvDate_ServerValidate" SetFocusOnError="True" ValidationGroup="FreshMember"
                                ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Customer</td>
                        <td class="style5">:
                        </td>
                        <td class="style6">
                            <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox"
                                Width="120px" ValidationGroup="ExistingMember"></asp:TextBox>
                            <asp:HyperLink ID="hypLookUpCustomer" NavigateUrl="#" runat="server"
                                onclick="showPromptPopUp('PromptCustomer.aspx?BranchID=' + document.getElementById('ddlBranch').value, this.previousSibling.previousSibling.id, 550, 900);">Look Up</asp:HyperLink>
                            &nbsp;<asp:RequiredFieldValidator ID="rqvCustomerCode" runat="server" ControlToValidate="txtCustomerCode"
                                CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Customer&lt;/b&gt; must be specified"
                                SetFocusOnError="True" ValidationGroup="ExistingMember"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Sales
                        </td>
                        <td class="style5">:
                        </td>
                        <td class="style6">
                            <asp:DropDownList ID="ddlSales" runat="server" CssClass="dropdown"
                                ValidationGroup="ExistingMember">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqvSales" runat="server" ControlToValidate="ddlSales"
                                CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Sales&lt;/b&gt; must be specified"
                                SetFocusOnError="True" ValidationGroup="ExistingMember"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" valign="top">Notes
                        </td>
                        <td class="style3" valign="top">:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" runat="server" CssClass="textbox" Width="400px" Rows="5"
                                TextMode="MultiLine" ValidationGroup="ExistingMember"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;&nbsp;                
            </td>
        </tr>
        <tr>
            <td>
                <div id="otherpurchase">
                    <asp:UpdatePanel ID="pnlOtherPurchase" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr style="font-weight: bold;">
                                    <td class="style23">Item Type:
                                    </td>
                                    <td class="style24">Item:
                                    </td>
                                    <td class="style21">Quantity:
                                    </td>
                                    <td class="style18">Unit Price:
                                    </td>
                                    <td class="style19">Discount:
                                    </td>
                                    <td class="style20">&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        <asp:DropDownList ID="ddlItemType" runat="server" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <AjaxToolkit:CascadingDropDown ID="ddlItemType_CascadingDropDown" PromptText="Select item type"
                                            Category="ItemType" ServicePath="AjaxWebService.asmx" ServiceMethod="GetItemTypes"
                                            runat="server" Enabled="True" TargetControlID="ddlItemType">
                                        </AjaxToolkit:CascadingDropDown>
                                    </td>
                                    <td class="style24">
                                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <AjaxToolkit:CascadingDropDown ID="ddlItem_CascadingDropDown" Category="Item" ParentControlID="ddlItemType"
                                            PromptText="Select item type first" LoadingText="Loading, please wait" ServicePath="AjaxWebService.asmx"
                                            ServiceMethod="GetItemsByType" runat="server" Enabled="True" TargetControlID="ddlItem">
                                        </AjaxToolkit:CascadingDropDown>
                                    </td>
                                    <td class="style21">
                                        <ew:NumericBox ID="txtQuantity" runat="server" CssClass="textbox" Width="50px" />
                                    </td>
                                    <td class="style18">
                                        <ew:NumericBox ID="txtUnitPrice" runat="server" CssClass="textbox" Width="120px" />
                                    </td>
                                    <td class="style19">
                                        <ew:NumericBox ID="txtDiscount" runat="server" CssClass="textbox" Width="50px" RealNumber="true" />
                                    </td>
                                    <td class="style20">
                                        <asp:CheckBox ID="chkIsTaxable" runat="server" Text="Taxable" Checked="True" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAddDetail" runat="server" CssClass="button" Text="Add Detail"
                                            OnClick="btnAddDetail_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="gvwOtherPurchase" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
                                AutoGenerateColumns="False" OnRowCommand="gvwOtherPurchase_RowCommand" OnRowCreated="gvwOtherPurchase_RowCreated">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ItemBarcode" HeaderText="ItemBarcode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:###,##0}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" DataFormatString="{0:###,##0}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Discount" HeaderText="Discount" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:CheckBoxField DataField="IsTaxable" HeaderText="Taxable" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnbDeleteItem" runat="server" Text="Delete" CommandName="DeleteItem"
                                                CommandArgument='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <h4>Total Invoice:
                        <asp:Label ID="lblTotalInvoice" runat="server" />
                            </h4>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddDetail" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvwOtherPurchase" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </td>
        </tr>
    </table>
    <hr />
    <div>
        <asp:Panel ID="pnlPayment" runat="server">
            <a id="link_payment" href="#">Payment</a>

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
        </asp:Panel>
        <br />
        <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
        <br />
        <asp:Button ID="btnSave" runat="server" SkinID="SaveButton"
            EnableViewState="false" OnClick="btnSave_Click"
            ValidationGroup="ExistingMember" />
    </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#payment").hide();


            $("#<%=ddlItem.ClientID%>").change(function() {
                var ajaxService = new CheckInService.AjaxService();
                var itemID = $(this).val();
                ajaxService.GetStdUnitPrice(itemID, function(unitPrice) {
                    $("#<%=txtUnitPrice.ClientID%>").val(unitPrice);
                });

            });
        });

        $("#link_payment").click(
        function () {
            $("#payment").toggle("slow");
        });
    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 120px;
        }

        .style3 {
            width: 1px;
        }

        .style4 {
            width: 120px;
            height: 21px;
        }

        .style5 {
            width: 1px;
            height: 21px;
        }

        .style6 {
            height: 21px;
        }

        .style18 {
            width: 130px;
        }

        .style19 {
            width: 59px;
        }

        .style20 {
            width: 70px;
        }

        .style21 {
            width: 60px;
        }

        .style23 {
            width: 90px;
        }

        .style24 {
            width: 195px;
        }

        .style28 {
            width: 161px;
        }

        .style33 {
            width: 156px;
        }

        .style34 {
            width: 293px;
        }
    </style>
</asp:Content>
