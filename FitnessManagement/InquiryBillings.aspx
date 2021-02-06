<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="InquiryBillings.aspx.cs" Inherits="InquiryBillings" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 109px;
        }

        .auto-style2 {
            width: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Billing History
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">Branch</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown" /></td>
        </tr>
        <tr>
            <td class="auto-style1">Year</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown" /></td>
        </tr>
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style2"></td>
            <td>
                <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" OnClick="btnSearch_Click" /></td>
        </tr>
    </table>
    <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" SkinID="GridViewDefaultSkin" Width="100%" OnRowCreated="gvwMaster_RowCreated" OnRowDataBound="gvwMaster_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="BatchNo" HeaderText="BatchNo" SortExpression="BatchNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="BillingType" HeaderText="BillingType" SortExpression="BillingType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ProcessDate" HeaderText="ProcessDate" SortExpression="ProcessDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy HH:mm}" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:HyperLink ID="hypBillingDetail" runat="server" Text="Invoice History" NavigateUrl="#" ImageUrl="~/images/zoom.png" 
                        ToolTip="View invoice history" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_InquiryBillingHistory" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="ProcessYear" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

