<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ProcessBillingResult.aspx.cs" Inherits="ProcessBillingResult" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 146px;
        }

        .auto-style2 {
            width: 2px;
        }
        .auto-style3 {
            width: 159px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Process Billing Result        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" ActiveViewIndex="0" runat="server">
        <asp:View runat="server" ID="View1">
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
            <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" SkinID="GridViewDefaultSkin" Width="100%" OnRowCreated="gvwMaster_RowCreated" OnRowDataBound="gvwMaster_RowDataBound" OnRowCommand="gvwMaster_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="BatchNo" HeaderText="BatchNo" SortExpression="BatchNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="BillingType" HeaderText="BillingType" SortExpression="BillingType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ProcessDate" HeaderText="ProcessDate" SortExpression="ProcessDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy HH:mm}" />
                    <asp:BoundField DataField="ResultProcessDate" HeaderText="ResultProcessDate" SortExpression="ResultProcessDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="FileName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:HyperLink ID="hypFileName" runat="server" Text='<%# Eval("FileName") %>' NavigateUrl='<%# "~/billing/" + Eval("FileName") %>' Target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="lnbProcessResult" runat="server" ImageUrl="~/images/sync.png" CommandName="ProcessResult" CommandArgument='<%# Eval("BatchNo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                        <ItemTemplate>
                            <asp:HyperLink ID="hypBillingDetail" runat="server" Text="Invoice History" NavigateUrl="#" ImageUrl="~/images/zoom.png"
                                ToolTip="View invoice history" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_InquiryBillingResult" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="ProcessYear" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View runat="server" ID="View2">
            <table class="ui-accordion">
                <tr>
                    <td class="auto-style3">Batch No.</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:Label ID="lblBatchNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Billing Type</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:Label ID="lblBillingType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Process Date</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:Label ID="lblProcessDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">File Name</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:Label ID="lblFileName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Decline File Name</td>
                    <td class="auto-style2">:</td>
                    <td>
                        <asp:FileUpload ID="fupDecline" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="button" EnableViewState="False" Text="Submit" OnClick="btnSubmit_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="button" EnableViewState="False" OnClick="btnCancel_Click" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

</asp:Content>

