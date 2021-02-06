<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="InquiryMembership.aspx.cs" Inherits="InquiryMembership" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Membership Inquiry (as of today)        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
    </telerik:RadStyleSheetManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
        <%--<ClientEvents OnRequestStart="onRequestStart"></ClientEvents>--%>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadFilter1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
     <telerik:RadFilter runat="server" ID="RadFilter1" FilterContainerID="RadGrid1" ShowApplyButton="false"></telerik:RadFilter>
    <p>
        <asp:Button ID="btnExportToExcel" runat="server" Text="Export To Excel" CssClass="button" OnClick="btnExportToExcel_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Filter" CssClass="button" OnClick="btnApplyFilter_Click"  />
        </p>
    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowSorting="True" CellSpacing="0" DataSourceID="sdsMaster" GridLines="None" ShowGroupPanel="True" Skin="Metro" FilterMenu-EnableAjaxSkinRendering="true" EnableAjaxSkinRendering="true" GroupingSettings-CaseSensitive="false" >
        <ClientSettings AllowColumnsReorder="True" AllowDragToGroup="True" ReorderColumnsOnClient="True">
            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="400" />
        </ClientSettings>
        <MasterTableView AutoGenerateColumns="False" DataSourceID="sdsMaster" AllowFilteringByColumn="false" EnableHeaderContextFilterMenu="true" EnableHeaderContextMenu="true" EnableHeaderContextAggregatesMenu="true" AllowMultiColumnSorting="true">
            <CommandItemSettings ShowExportToWordButton="true" ShowExportToExcelButton="true"
                ShowExportToCsvButton="true" ShowExportToPdfButton="true"></CommandItemSettings>
            <RowIndicatorColumn Visible="False">
            </RowIndicatorColumn>
            <ExpandCollapseColumn Created="True" ButtonType="SpriteButton">
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridBoundColumn DataField="CustomerBarcode" FilterControlAltText="Filter CustomerBarcode column" HeaderText="CustomerBarcode" SortExpression="CustomerBarcode" UniqueName="CustomerBarcode">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Name" ReadOnly="True" SortExpression="Name" UniqueName="Name">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column" HeaderText="Status" ReadOnly="True" SortExpression="Status" UniqueName="Status">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="StartDate" DataType="System.DateTime" FilterControlAltText="Filter StartDate column" HeaderText="StartDate" SortExpression="StartDate" UniqueName="StartDate" DataFormatString="{0:ddd, dd-MMM-yyyy}">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EndDate" DataType="System.DateTime" FilterControlAltText="Filter EndDate column" HeaderText="EndDate" SortExpression="EndDate" UniqueName="EndDate">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column" HeaderText="Email" SortExpression="Email" UniqueName="Email">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Package" FilterControlAltText="Filter Package column" HeaderText="Package" SortExpression="Package" UniqueName="Package">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="HomePhone" FilterControlAltText="Filter HomePhone column" HeaderText="HomePhone" SortExpression="HomePhone" UniqueName="HomePhone">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ContractStatus" FilterControlAltText="Filter ContractStatus column" HeaderText="ContractStatus" SortExpression="ContractStatus" UniqueName="ContractStatus">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DuesAmount" DataType="System.Decimal" FilterControlAltText="Filter DuesAmount column" HeaderText="DuesAmount" ReadOnly="True" SortExpression="DuesAmount" UniqueName="DuesAmount" DataFormatString="{0:###,##0}">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ContractNo" FilterControlAltText="Filter ContractNo column" HeaderText="ContractNo" SortExpression="ContractNo" UniqueName="ContractNo">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="StatusMembership" FilterControlAltText="Filter StatusMembership column" HeaderText="StatusMembership" ReadOnly="True" SortExpression="StatusMembership" UniqueName="StatusMembership">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="BillingTypeDescription" FilterControlAltText="Filter BillingTypeDescription column" HeaderText="BillingTypeDescription" SortExpression="BillingTypeDescription" UniqueName="BillingTypeDescription">
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_InquiryMembership" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
</asp:Content>

