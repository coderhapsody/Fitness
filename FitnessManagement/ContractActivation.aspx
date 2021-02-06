<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="ContractActivation.aspx.cs" Inherits="ContractActivation" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 120px;
        }
        .style2
        {
            width: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Contract Activation
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">
                Branch
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <asp:DropDownList ID="ddlFindBranch" runat="server" CssClass="dropdown">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Contract Date
            </td>
            <td class="style2">
                :
            </td>
            <td>
                From
                <ew:CalendarPopup ID="calContractDateFrom" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
                &nbsp;to
                <ew:CalendarPopup ID="calContractDateTo" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Customer Name
            </td>
            <td class="style2">
                :
            </td>
            <td>
                <asp:TextBox ID="txtCustomerFirstName" runat="server" CssClass="textbox" MaxLength="50"
                    ValidationGroup="AddEdit" Width="150px" />
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                                    <asp:Button ID="btnRefresh" runat="server" 
                    CssClass="button" EnableViewState="False"
                                        Text="Refresh" OnClick="btnRefresh_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnSave" runat="server" CssClass="button" EnableViewState="False" OnClientClick="return confirm('Are you sure want to save ?')"
                                        Text="Save" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False" 
        DataSourceID="sqldsMaster" SkinID="GridViewDefaultSkin" Width="100%" 
        AllowSorting="True" onrowcreated="gvwMaster_RowCreated" 
        onrowdatabound="gvwMaster_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                SortExpression="ContractNo" />
            <asp:BoundField DataField="Barcode" HeaderText="Barcode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                SortExpression="Barcode" />
            <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                ReadOnly="True" SortExpression="CustomerName" />
            <asp:BoundField DataField="Package" HeaderText="Package" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                SortExpression="Package" />
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
            <asp:BoundField DataField="PurchaseDate" HeaderText="PurchaseDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                SortExpression="PurchaseDate" />
            <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                SortExpression="EffectiveDate" />
            <asp:BoundField DataField="ActiveDate" HeaderText="ActiveDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                SortExpression="ActiveDate" />
            <asp:BoundField DataField="BillingType" HeaderText="BillingType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" 
                SortExpression="BillingType" />
            <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                SortExpression="Status" />
            <asp:TemplateField HeaderText="Activate" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:CheckBox ID="chkActivate" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            .: No Data :.
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:SqlDataSource ID="sqldsMaster" runat="server" 
        ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
        SelectCommand="proc_GetContractsForActivation" 
        SelectCommandType="StoredProcedure" onselecting="sqldsMaster_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="ContractDateFrom" Type="DateTime" />
            <asp:Parameter Name="ContractDateTo" Type="DateTime" />
            <asp:Parameter Name="CustomerName" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
