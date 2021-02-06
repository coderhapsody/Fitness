<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="MasterPickUpPerson.aspx.cs" Inherits="MasterPickUpPerson" StyleSheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Pick Up Person
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:ScriptManager ID="scmScriptManager" runat="server" />
    <asp:MultiView ActiveViewIndex="0" ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="ui-accordion">
                <tr>                    
                    <td class="style1">
                        Branch</td>
                    <td class="style2">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlFindBranch" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Barcode</td>
                    <td class="style2">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtFindBarcode" runat="server" CssClass="textbox" 
                            MaxLength="50" ValidationGroup="AddEdit" Width="120px" />                        
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Name</td>
                    <td class="style2">
                        :</td>
                    <td>
                        <asp:TextBox ID="txtFindName" runat="server" CssClass="textbox" MaxLength="50" 
                            ValidationGroup="AddEdit" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;</td>
                    <td class="style2">
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="Button1" runat="server" CssClass="button" 
                            SkinID="RefreshButton" Text="Refresh" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvwMaster" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" DataSourceID="sdsMaster" 
                onrowcommand="gvwMaster_RowCommand" onrowcreated="gvwMaster_RowCreated" 
                SkinID="GridViewDefaultSkin" Width="100%" 
                onrowdatabound="gvwMaster_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderStyle-HorizontalAlign="Left" 
                        HeaderText="ID" ItemStyle-HorizontalAlign="Left" SortExpression="ID" />
                    <asp:BoundField DataField="Barcode" HeaderStyle-HorizontalAlign="Left" 
                        HeaderText="Barcode" ItemStyle-HorizontalAlign="Left" 
                        SortExpression="Barcode" />
                    <asp:BoundField DataField="Name" HeaderStyle-HorizontalAlign="Left" 
                        HeaderText="Name" ItemStyle-HorizontalAlign="Left" SortExpression="Name" />
                    <asp:BoundField DataField="Package" HeaderStyle-HorizontalAlign="Left" 
                        HeaderText="Package" ItemStyle-HorizontalAlign="Left" 
                        SortExpression="p.[Name]" />
                    <asp:BoundField DataField="Status" HeaderStyle-HorizontalAlign="Left" 
                        HeaderText="Status" ItemStyle-HorizontalAlign="Left" SortExpression="Status" />
                    <asp:BoundField DataField="ContractStatus" HeaderStyle-HorizontalAlign="Left" 
                        HeaderText="ContractStatus" ItemStyle-HorizontalAlign="Left" 
                        SortExpression="ContractStatus" />
                    <asp:BoundField DataField="EffectiveDate" 
                        DataFormatString="{0:ddd, dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" 
                        HeaderText="EffectiveDate" ItemStyle-HorizontalAlign="Left" 
                        SortExpression="EffectiveDate" />                    
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <a id="hypPerson" runat="server" href="#"><img src="images/user_green.png" style="border: 0px" /></a>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sdsMaster" runat="server" 
                ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" 
                onselecting="sdsMaster_Selecting" SelectCommand="proc_GetAllCustomers" 
                SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="Barcode" Type="String" />
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="ParentName" Type="String" DefaultValue="" ConvertEmptyStringToNull="false" />
                    <asp:Parameter Name="PhoneNo" Type="String" DefaultValue="" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
    </asp:MultiView>
</asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="cphHead">
    <style type="text/css">
        .style1
        {
            width: 120px;
        }
        .style2
        {
            width: 1px;
        }
    .style3
    {
        width: 107px;
    }
    </style>
</asp:Content>


