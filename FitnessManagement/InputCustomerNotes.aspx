<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true"
    CodeFile="InputCustomerNotes.aspx.cs" Inherits="InputCustomerNotes" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Notes for&nbsp;<asp:Literal ID="litCustomerName" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table style="width: 100%;" id="tblNotes" runat="server">
        <tr>
            <td>
                <asp:Label ID="lblDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Width="100%" CssClass="textbox"
                    Rows="5" ValidationGroup="Notes"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rqvNotes" runat="server" ControlToValidate="txtNotes"
                    EnableViewState="false" ErrorMessage="Notes cannot be empty" SetFocusOnError="true"
                    CssClass="errorMessage" ValidationGroup="Notes" />
                <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">Priority:
                <asp:CheckBox ID="chkShowCheckIn" runat="server" Text="Show at check-in" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <asp:Button ID="btnSave" runat="server" EnableViewState="False" ValidationGroup="Notes"
                    SkinID="SaveButton" Text="Save" OnClick="btnSave_Click" />

                <input type="button" onclick="window.close();" value="Close This Window" class="button" title="Close this window" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvwNotes" runat="server" SkinID="GridViewDefaultSkin" AllowPaging="True"
        AllowSorting="True" Width="100%" AutoGenerateColumns="False" DataSourceID="sdsMaster"
        OnRowCommand="gvwNotes_RowCommand" OnRowCreated="gvwNotes_RowCreated">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-HorizontalAlign="Left"
                SortExpression="ID">
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="CreatedWhen" HeaderText="Date" HeaderStyle-HorizontalAlign="Left"
                DataFormatString="{0:ddd, dd-MMM-yyyy HH:mm:ss}" ItemStyle-Width="130px" SortExpression="CreatedWhen">
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                <ItemStyle Width="130px"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Notes" SortExpression="Notes">
                <ItemTemplate>
	            <div style="word-break:break-all;">
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Notes") %>'></asp:Label>
	            </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle  Wrap="True" />
            </asp:TemplateField>
            <asp:BoundField DataField="IsShowed" HeaderText="Is Showed" ReadOnly="True" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-Width="100px" SortExpression="IsShowed">
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                <ItemStyle Width="100px"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                <ItemTemplate>
                    <asp:LinkButton ID="lnbToggle" runat="server" Text="Toggle Show" CommandName="Toggle"
                        CommandArgument='<%# Eval("ID") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="5px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                <ItemTemplate>
                    <asp:LinkButton ID="lnbDelete" runat="server" Text="Delete" CommandName="DeleteNote" Visible='<%# Request["CanDelete"]=="1" %>'
                        CommandArgument='<%# Eval("ID") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="5px"></ItemStyle>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            .: No Data :.
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
        SelectCommand="proc_GetCustomerNotes" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="CustomerBarcode" QueryStringField="barcode" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
