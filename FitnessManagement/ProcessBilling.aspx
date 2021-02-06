<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ProcessBilling.aspx.cs" Inherits="ProcessBilling" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 200px;
        }

        .auto-style2 {
            width: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Process Billing                
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="auto-style1">Branch</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlBranch" CssClass="dropdown" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Billing Type</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlBillingType" CssClass="dropdown" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Membership Type</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:CheckBoxList ID="cblMembershipType" runat="server" RepeatColumns="5" RepeatLayout="Table">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Customer Status</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:CheckBoxList ID="cblCustomerStatus" runat="server" RepeatColumns="5" RepeatLayout="Table">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Next Due Date</td>
            <td class="auto-style2">:</td>
            <td>From
                <ew:CalendarPopup ID="calFindDateFrom" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
                &nbsp;&nbsp;&nbsp;&nbsp; To
                <ew:CalendarPopup ID="calFindDateTo" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>
                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button" OnClick="btnRefresh_Click" />&nbsp;&nbsp;&nbsp;&nbsp; 
                    <asp:Button ID="btnProcessAll" runat="server" Text="Process All" CssClass="button" OnClick="btnProcessAll_Click" /></td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>

    <div>
        <asp:GridView ID="gvwBilling" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="false" Width="100%">
            <Columns>
                <asp:BoundField DataField="CustomerBarcode" HeaderText="CustomerBarcode" SortExpression="CustomerBarcode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Name" HeaderText="CustomerName" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" SortExpression="ContractNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="BillingTypeDescription" HeaderText="BillingType" SortExpression="BillingTypeDescription" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                <asp:BoundField DataField="NextDuesDate" HeaderText="NextDuesDate" SortExpression="NextDuesDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                <asp:BoundField DataField="StatusMembership" HeaderText="StatusMembership" SortExpression="StatusMembership" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="DuesAmount" HeaderText="DuesAmount" SortExpression="DuesAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,###0.00}" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkProcess" class="chkProcess" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

<%--    <script>
        $(document).ready(function () {
            $(".chkProcess").each(function () {
                $(this).attr("checked", "checked");
            });
        });
    </script>--%>
</asp:Content>

