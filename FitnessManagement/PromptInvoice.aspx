<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true"
    CodeFile="PromptInvoice.aspx.cs" Inherits="PromptInvoice" StylesheetTheme="Workspace" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Look Up Invoices
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <AjaxToolkit:ToolkitScriptManager ID="tsmAjaxToolkitScriptManager" runat="server" />
    &nbsp;<table style="width: 100%">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 160px">
                            Branch</td>
                        <td style="width: 1px">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px">
                            Invoice Type</td>
                        <td style="width: 1px">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInvoiceType" runat="server" CssClass="dropdown">
                                <asp:ListItem Selected="True" Value="F">Membership Invoicing</asp:ListItem>
                                <asp:ListItem Value="X">Non-Membership Invoice</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px">
                            Date</td>
                        <td style="width: 1px">
                            :</td>
                        <td>
                            From
                        <ew:CalendarPopup ID="calDateFrom" runat="server" SkinID="Calendar">
                            <TextBoxLabelStyle CssClass="textbox" />
                        </ew:CalendarPopup>
                        &nbsp;to
                        <ew:CalendarPopup ID="calDateTo" runat="server" SkinID="Calendar">
                            <TextBoxLabelStyle CssClass="textbox" />
                        </ew:CalendarPopup>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px">
                            &nbsp;
                        </td>
                        <td style="width: 1px">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnRefresh" runat="server" CommandArgument="Refresh" CommandName="PromptEmployee"
                                CssClass="button" EnableViewState="False" Text="Refresh" OnClick="btnRefresh_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvwPrompt" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="False"
                    Width="100%" AllowSorting="True" AllowPaging="True" DataKeyNames="ID" DataSourceID="sdsPrompt"
                    OnRowCreated="gvwPrompt_RowCreated" OnRowDataBound="gvwPrompt_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            InsertVisible="False" ReadOnly="True" />
                        <asp:BoundField DataField="InvoiceNo" SortExpression="InvoiceNo" HeaderText="InvoiceNo"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ContractNo" SortExpression="ContractNo" HeaderText="ContractNo"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EffectiveDate" SortExpression="EffectiveDate" HeaderText="EffectiveDate"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                        <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode" SortExpression="CustomerCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" ReadOnly="True" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            SortExpression="CustomerName" />
                        <asp:BoundField DataField="EmployeeCode" HeaderText="EmployeeCode" SortExpression="EmployeeCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EmployeeName" HeaderText="EmployeeName" ReadOnly="True" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            SortExpression="EmployeeName" />
                        <asp:BoundField DataField="InvoiceDate" HeaderText="InvoiceDate" SortExpression="InvoiceDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                        <asp:TemplateField>
                            <ItemStyle Width="10px" />
                            <ItemTemplate>
                                <asp:HyperLink ID="hypSelect" runat="server" href="#">Select</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        .: No Data :.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="sdsPrompt" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                    SelectCommand="proc_GetActiveInvoices" SelectCommandType="StoredProcedure" OnSelecting="sdsPrompt_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="BranchID" Type="Int32" />
                        <asp:Parameter Name="InvoiceType" Type="String" />
                        <asp:Parameter Name="DateFrom" Type="DateTime" />
                        <asp:Parameter Name="DateTo" Type="DateTime" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
