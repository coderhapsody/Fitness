<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ViewActiveAlerts.ascx.cs" Inherits="UserControls_ViewActiveAlerts" %>

<asp:Repeater ID="repAlerts" runat="server" DataSourceID="sdsData">
    <HeaderTemplate>
        <img src="images/chat.png" style="float:left" alt="Chat icon" title="Current active alerts" />
        <h2>Today Alerts</h2>
        <table border="0" cellpadding="4" cellspacing="3" style="margin-left:50px; width:75%;">
    </HeaderTemplate>

    <ItemTemplate>
            <tr>
                <td style="width:200px;"><span style="font-style:italic; font-weight:bold;"><%# Convert.ToDateTime(Eval("StartDate")).ToLongDateString() %></span></td>
                <td><%# HttpUtility.HtmlEncode(Eval("Description")) %></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>            
    </ItemTemplate>

    <FooterTemplate>
        </table>
    </FooterTemplate>    
    
</asp:Repeater>
<asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
<asp:SqlDataSource ID="sdsData" runat="server" 
    ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
    onselecting="sdsAlert_Selecting" SelectCommand="proc_GetActiveAlerts" 
    SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:Parameter Name="Date" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
