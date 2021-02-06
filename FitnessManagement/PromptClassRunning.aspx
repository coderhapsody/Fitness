<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true" CodeFile="PromptClassRunning.aspx.cs" Inherits="PromptClassRunning" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 100px;
        }
        .auto-style2 {
            width: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Select Class Running        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="auto-style1">Date</td>
            <td class="auto-style2">:</td>
            <td><ew:CalendarPopup ID="calDate" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>                </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>
                <asp:Button ID="btnRefresh" runat="server" CssClass="button" EnableViewState="False" Text="Refresh" OnClick="btnRefresh_Click" />
            </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <asp:GridView ID="gvwData" runat="server" AutoGenerateColumns="False" DataKeyNames="ClassRunningID" DataSourceID="sdsPrompt" OnRowCreated="gvwData_RowCreated" SkinID="GridViewDefaultSkin" Width="100%" OnRowDataBound="gvwData_RowDataBound" >
        <Columns>
            <asp:BoundField DataField="ClassRunningID" HeaderText="ClassRunningID" InsertVisible="False" ReadOnly="True" SortExpression="ClassRunningID" />
            <asp:BoundField DataField="ClassName" HeaderText="ClassName" SortExpression="ClassName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="InstructorName" HeaderText="InstructorName" SortExpression="InstructorName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Level" HeaderText="Level" SortExpression="Level" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="TimeStart" HeaderText="TimeStart" SortExpression="TimeStart" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="TimeEnd" HeaderText="TimeEnd" SortExpression="TimeEnd" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="hypSelect" runat="server" Text="Select" NavigateUrl="#" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsPrompt" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_PromptClassRunning" SelectCommandType="StoredProcedure" OnSelecting="sdsPrompt_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="Date" Type="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

