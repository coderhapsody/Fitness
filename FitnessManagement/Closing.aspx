<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="Closing.aspx.cs" Inherits="Closing" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 70px;
        }
        .auto-style2 {
            width: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Monthly Closing        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="auto-style1">Branch</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown">
                </asp:DropDownList></td>  
        </tr>
        <tr>
            <td class="auto-style1">Year</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown">
                </asp:DropDownList></td>  
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" />
            </td>  
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>
                &nbsp;</td>  
        </tr>
        <tr>
            <td class="auto-style1">Month</td>
            <td class="auto-style2">:</td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdown">
                </asp:DropDownList>
            &nbsp;
                <asp:Button ID="btnProses" runat="server" Text="Process Closing" CssClass="button" OnClick="btnProses_Click"  />
                &nbsp;
                <asp:Button ID="btnUnProses" runat="server" Text="UnClosing" CssClass="button" OnClick="btnUnProses_Click"  />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" Width="100%" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="BranchName" HeaderText="Branch" ReadOnly="True" SortExpression="BranchName" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ClosingMonthName" HeaderText="Month" ReadOnly="True" SortExpression="ClosingMonthName" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ChangedWhen" HeaderText="Closing When" ReadOnly="True" SortExpression="ChangedWhen" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy HH:mm:ss}" />
            <asp:BoundField DataField="ChangedWho" HeaderText="User Name" ReadOnly="True" SortExpression="ChangedWho" HeaderStyle-HorizontalAlign="Left" />
        </Columns>
     <EmptyDataTemplate>
         .: No Data :.
     </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>

