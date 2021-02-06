<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true"
    CodeFile="ViewCustomerStatusHistory.aspx.cs" Inherits="ViewCustomerStatusHistory"
    StylesheetTheme="Workspace" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Customer Status History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
        DataSourceID="sdsMaster" OnRowCreated="gvwMaster_RowCreated" SkinID="GridViewDefaultSkin"
        Width="100%" EnableViewState="False" OnRowDataBound="gvwMaster_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CustomerStatus" HeaderText="CustomerStatus" SortExpression="CustomerStatus" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:ddd, dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="StartDate" HeaderText="StartDate" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                SortExpression="StartDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="EndDate" HeaderText="EndDate" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                SortExpression="EndDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="DocumentNo" HeaderText="DocumentNo" SortExpression="DocumentNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ApprovedDate" HeaderText="ApprovedDate" DataFormatString="{0:ddd, dd-MMM-yyyy HH:mm}"
                SortExpression="ApprovedDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ApproverName" HeaderText="ApproverName" ReadOnly="True"
                SortExpression="ApproverName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="IssuedBy" HeaderText="IssuedBy" ReadOnly="True" SortExpression="IssuedBy" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
        SelectCommand="proc_GetCustomerStatusHistory" 
        SelectCommandType="StoredProcedure" EnableViewState="False">
        <SelectParameters>
            <asp:QueryStringParameter Name="CustomerCode" QueryStringField="CustomerCode" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <p>
        <input type="button" id="btnClose" onclick="window.close()" value="Close this window"
            class="button" />
    </p>
</asp:Content>
