<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ProcessUnpaidBilling.aspx.cs" Inherits="ProcessUnpaidBilling" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .ui-tabs
        {
            font-size: 100%;
            width: 100%;
        }

        .auto-style1
        {
            width: 250px;
        }

        .auto-style2
        {
            width: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Process Unpaid Billing
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div id="tabs">
        <ul>
            <li><a href="#tab1">Generate Text File</a></li>
            <li><a href="#tab2">Upload Result</a></li>
        </ul>
        <div id="tab1">
            <table style="width: 100%">
                <tr>
                    <td class="auto-style1">Branch</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Find outstanding payment for invoice date</td>
                    <td class="auto-style2">:</td>
                    <td>From
                        <ew:CalendarPopup ID="calFromDate" runat="server" SkinID="Calendar">
                            <TextBoxLabelStyle CssClass="textbox" />
                        </ew:CalendarPopup>
                        to
                                                <ew:CalendarPopup ID="calToDate" runat="server" SkinID="Calendar">
                                                    <TextBoxLabelStyle CssClass="textbox" />
                                                </ew:CalendarPopup>

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
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button" OnClick="btnRefresh_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="button" OnClick="btnProcess_Click" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Literal ID="litResult" runat="server"></asp:Literal></td>
                </tr>
            </table>

            <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False" DataSourceID="sdsMaster" SkinID="GridViewDefaultSkin" Width="100%" AllowSorting="true">
                <Columns>
                    <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" SortExpression="InvoiceNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                    <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="InvoiceAmount" ReadOnly="True" SortExpression="InvoiceAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0.00}" />
                    <asp:BoundField DataField="PaidAmount" HeaderText="PaidAmount" ReadOnly="True" SortExpression="PaidAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0.00}" />
                    <asp:BoundField DataField="CustomerBarcode" HeaderText="CustomerBarcode" ReadOnly="True" SortExpression="CustomerBarcode" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" SortExpression="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="CustomerStatus" HeaderText="CustomerStatus" ReadOnly="True" SortExpression="CustomerStatus" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkProcess" runat="server" Checked="true" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_FindUnpaidInvoice" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="FromDate" Type="DateTime" />
                    <asp:Parameter Name="ToDate" Type="DateTime" />
                    <asp:Parameter Name="StatusWithComma" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

        </div>
        <div id="tab2">
            <h3 style="color: red;">Make sure you will upload the <b><u>accepted</u></b> billing data, not the rejected data.
            </h3>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px;">Result Accepted Text File</td>
                    <td style="width: 5px;">:</td>
                    <td>
                        <asp:FileUpload ID="fupResultFile" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnProcessResult" runat="server" Text="Process Result" CssClass="button" OnClick="btnProcessResult_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <br />
    <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
    <br />
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button"
        EnableViewState="false" OnClick="btnSave_Click" />
    &nbsp;    &nbsp;&nbsp;&nbsp;&nbsp;
    <input type="reset" value="Undo" class="button" />

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs();
        });
    </script>

</asp:Content>

