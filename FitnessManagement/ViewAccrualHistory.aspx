<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true" CodeFile="ViewAccrualHistory.aspx.cs" Inherits="ViewAccrualHistory" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Accrual History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
        <asp:GridView runat="server" ID="gvwMaster" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" OnRowCreated="gvwMaster_RowCreated" SkinID="GridViewDefaultSkin" OnRowDataBound="gvwMaster_RowDataBound" GridLines="Vertical" AllowSorting="True">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />            
            <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" SortExpression="InvoiceNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="AccrualDate" HeaderText="AccrualDate" SortExpression="AccrualDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="TotalAmount" HeaderText="TotalAmount" SortExpression="TotalAmount" DataFormatString="{0:###,##0.00}"  HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="TotalAccrualPeriod" HeaderText="TotalAccrualPeriod" SortExpression="TotalAccrualPeriod" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="AccrualAmount" HeaderText="AccrualAmount" SortExpression="AccrualAmount" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="SumAccrualPeriod" HeaderText="SumAccrualPeriod" SortExpression="SumAccrualPeriod" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="SumAccrualAmount" HeaderText="SumAccrualAmount" SortExpression="SumAccrualAmount" DataFormatString="{0:###,##0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"  />
            <asp:BoundField DataField="CreatedWhen" HeaderText="CreatedWhen" SortExpression="CreatedWhen" DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
            <asp:BoundField DataField="CreatedWho" HeaderText="CreatedWho" SortExpression="CreatedWho" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hypPrint" Text="Print" NavigateUrl="#" ImageUrl="images/PrintHS.png"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_ViewAccrualHistory" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="InvoiceID" Type="Int32" QueryStringField="InvoiceID" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>

