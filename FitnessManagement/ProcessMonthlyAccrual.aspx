<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ProcessMonthlyAccrual.aspx.cs" Inherits="ProcessMonthlyAccrual" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            width: 150px;
        }

        .auto-style3 {
            width: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Monthly Accrual Process
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="auto-style1">
        <tr>
            <td class="auto-style2">Branch</td>
            <td class="auto-style3">:</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlBranch" CssClass="dropdown" /></td>
        </tr>
        <tr>
            <td class="auto-style2">Last Accrual Month / Year</td>
            <td class="auto-style3">:</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMonth" CssClass="dropdown" />&nbsp;<asp:DropDownList runat="server" ID="ddlYear" CssClass="dropdown" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td>
                <asp:CheckBox ID="chkExcludeFirstAccrual" runat="server" Text="Exclude first accrual" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td>
                <asp:CheckBox ID="chkExcludeFinishAccrual" runat="server" Text="Exclude last accrual" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td>
                <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" OnClick="btnSearch_Click" />
                &nbsp;<asp:Button ID="btnProcess" runat="server" CssClass="button" Text="Process Accrual" OnClientClick="return confirm('This process is irreversible, are you sure want to start accrual process ?')" OnClick="btnProcess_Click" />
                &nbsp;&nbsp;&nbsp;
    <asp:Label runat="server" ID="lblStatus" EnableViewState="False"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvwMaster" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" OnRowCreated="gvwMaster_RowCreated" SkinID="GridViewDefaultSkin" OnRowDataBound="gvwMaster_RowDataBound" GridLines="Vertical" AllowSorting="True">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID" SortExpression="InvoiceID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" SortExpression="InvoiceNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="AccrualDate" HeaderText="AccrualDate" SortExpression="AccrualDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="TotalAmount" HeaderText="TotalAmount" SortExpression="TotalAmount" DataFormatString="{0:###,##0.00}"  HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="TotalAccrualPeriod" HeaderText="TotalAccrualPeriod" SortExpression="TotalAccrualPeriod" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="AccrualAmount" HeaderText="AccrualAmount" SortExpression="AccrualAmount" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="SumAccrualPeriod" HeaderText="SumAccrualPeriod" SortExpression="SumAccrualPeriod" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="SumAccrualAmount" HeaderText="SumAccrualAmount" SortExpression="SumAccrualAmount" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"  />
            <asp:BoundField DataField="CreatedWhen" HeaderText="CreatedWhen" SortExpression="CreatedWhen" DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
            <asp:BoundField DataField="CreatedWho" HeaderText="CreatedWho" SortExpression="CreatedWho" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hypAccrualHistory" Text="Accrual History" NavigateUrl="#"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:CheckBox ID="chkProcess" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetInvoiceAccrualSummary" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="AccrualMonth" Type="Int32" />
            <asp:Parameter Name="AccrualYear" Type="Int32" />
            <asp:Parameter Name="ExcludeFirstAccrual" Type="Boolean" />
            <asp:Parameter Name="ExcludeFinishAccrual" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

