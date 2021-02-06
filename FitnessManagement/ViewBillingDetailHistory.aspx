<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true" CodeFile="ViewBillingDetailHistory.aspx.cs" Inherits="ViewBillingDetailHistory" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Billing Detail
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False" DataSourceID="sdsMaster" SkinID="GridViewDefaultSkin" Width="100%" AllowPaging="true" AllowSorting="true" OnRowDataBound="gvwMaster_RowDataBound" >
        <Columns>
            <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" SortExpression="ContractNo" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" SortExpression="InvoiceNo" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="PaidStatus" HeaderText="PaidStatus" ReadOnly="True" SortExpression="PaidStatus" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="VoidReason" HeaderText="Payment Alert" ReadOnly="True" SortExpression="VoidReason" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode" SortExpression="CustomerCode" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" ReadOnly="True" SortExpression="CustomerName" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="PackageName" HeaderText="PackageName" SortExpression="PackageName" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="DuesAmount" HeaderText="DuesAmount" SortExpression="DuesAmount" HeaderStyle-HorizontalAlign="Right"  ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0}" />
            <asp:BoundField DataField="RejectionCode" HeaderText="RejectionCode" SortExpression="RejectionCode" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetBillingDetailInfo" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="BillingBatchNo" QueryStringField="BatchNo" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

