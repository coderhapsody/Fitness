<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationMenus.ascx.cs" Inherits="UserControls_ApplicationMenus" %>
<asp:Menu ID="menuDefault" runat="server" EnableViewState="false"
    RenderingMode="Table" ToolTip="Application main menu" Orientation="Horizontal"
    DynamicVerticalOffset="3" DynamicHorizontalOffset="0"
    DynamicEnableDefaultPopOutImage="true" IncludeStyleBlock="False">
    <DynamicHoverStyle BackColor="#1A341B" ForeColor="White" Font-Bold="True" />
    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="4px" Width="200px" ForeColor="Black" CssClass="menuItem" />
    <DynamicMenuStyle BackColor="#DFEADC" BorderWidth="2px" BorderStyle="Solid" />
</asp:Menu>
