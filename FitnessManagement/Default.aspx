<%@ Page Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Welcome to <%= ConfigurationManager.AppSettings[ApplicationSettingKeys.ApplicationTitle]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server"> 
    <table style="width:89%">
        <tr>
            <td rowspan="6" style="width: 200px">
                <img alt="" src="images/little_monkey.png" /></td>
            <td class="style1">&nbsp;</td>
            <td class="style2">Client Browser</td>
            <td style="width: 3px">:</td>
            <td><asp:Label ID="lblBrowser" runat="server" EnableViewState="false" /></td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">Database Server</td>
            <td style="width: 3px">:</td>
            <td><asp:Label ID="lblDatabaseServer" runat="server" EnableViewState="False" /></td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">Database</td>
            <td style="width: 3px">:</td>
            <td><asp:Label ID="lblDatabase" runat="server" EnableViewState="False" /></td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">Security Provider</td>
            <td style="width: 3px">:</td>
            <td><asp:Label ID="lblSecurityProvider" runat="server" EnableViewState="False" /></td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">Server Version</td>
            <td style="width: 3px">:</td>
            <td><asp:Label ID="lblServerVersion" runat="server" EnableViewState="False" /></td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style2">User <%= Page.User.Identity.Name %> is active at branch:</td>
            <td style="width: 3px">:</td>
            <td><asp:BulletedList ID="bulBranch" runat="server" EnableViewState="false" 
                    BulletStyle="CustomImage" BulletImageUrl="~/images/user_green.png" /></td>
        </tr>
        </table>
                                   
    
    <hr /> 
    <my:alerts ID="viewActiveAlerts" runat="server" EnableViewState="false"  />

   
</asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
        .style1
        {
            width: 97px;
        }
        .style2
        {
            width: 192px;
        }
    </style>
</asp:Content>


