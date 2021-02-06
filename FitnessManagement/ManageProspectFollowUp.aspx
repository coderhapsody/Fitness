<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true" CodeFile="ManageProspectFollowUp.aspx.cs" Inherits="ManageProspectFollowUp" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Prospect Follow-up
    <asp:Label ID="lblProspectName" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            <asp:ScriptReference Path="~/js/Utils.js" />
        </Scripts>
    </telerik:RadScriptManager>    

    <table class="fullwidth">
        <tr>
            <td class="auto-style1">Date</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDatePicker runat="server" ID="dtpDate" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style1">Follow-up Via</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlFollowUpVia" Width="80px" />
            </td>
            <td>
                <asp:RequiredFieldValidator runat="server" ID="rqvFollowUpVia" ControlToValidate="ddlFollowUpVia" CssClass="errorMessage" SetFocusOnError="True" EnableViewState="False" ErrorMessage="<b>Follow Up via</b> must be specified" Text="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Follow-up By</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlFollowUpBy" DropDownHeight="200px" /></td>
            <td>
                <asp:RequiredFieldValidator runat="server" ID="rqvFollowUpVia0" ControlToValidate="ddlFollowUpBy" CssClass="errorMessage" SetFocusOnError="True" EnableViewState="False" ErrorMessage="&lt;b&gt;Follow Up By&lt;/b&gt; must be specified" Text="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Follow-up Outcome</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlOutcome" Width="150px" />
            </td>
            <td>
                <asp:RequiredFieldValidator runat="server" ID="rqvOutcome" ControlToValidate="ddlOutcome" CssClass="errorMessage" SetFocusOnError="True" EnableViewState="False" ErrorMessage="<b>Outcome</b> must be specified" Text="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Notes</td>
            <td class="auto-style2">:</td>
            <td>
                <telerik:RadTextBox runat="server" ID="txtResult" Width="100%" />
            </td>
            <td>
                <asp:RequiredFieldValidator runat="server" ID="rqvResult" ControlToValidate="txtResult" CssClass="errorMessage" SetFocusOnError="True" EnableViewState="False" ErrorMessage="<b>Notes</b> must be specified" Text="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style2"></td>
            <td>
                <telerik:RadButton runat="server" ID="btnAddFollowUp" Text="Add Follow Up" OnClick="btnAddFollowUp_Click"></telerik:RadButton>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

    <asp:Label ID="lblStatus" runat="server" EnableViewState="False" CssClass="errorMessage" />
    <br />

    <asp:GridView runat="server" ID="gvwMaster" Width="100%" DataSourceID="sdsMaster" AutoGenerateColumns="False" SkinID="GridViewDefaultSkin" OnRowCommand="grdMaster_ItemCommand" OnRowCreated="gvwMaster_RowCreated">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" SortExpression="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="FollowUpVia" HeaderText="FollowUpVia" SortExpression="FollowUpVia" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Consultant" HeaderText="Consultant" SortExpression="Consultant" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Result" HeaderText="Result" SortExpression="Result" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Outcome" HeaderText="Outcome" SortExpression="Outcome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ChangedWhen" HeaderText="Entry Date" SortExpression="ChangedWhen" DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/delete-item.png" runat="server" CausesValidation="False" OnClientClicking="DeleteConfirm" ID="btnDeleteRow" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetFollowUpsForProspect" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="" Name="ProspectID" QueryStringField="ProspectID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 140px;
        }
        .auto-style2 {
            width: 5px;
        }
    </style>
</asp:Content>

