<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true"
    CodeFile="PromptContract.aspx.cs" Inherits="PromptContract" StylesheetTheme="Workspace" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Look Up Contracts
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <AjaxToolkit:ToolkitScriptManager ID="tsmAjaxToolkitScriptManager" runat="server" />
    &nbsp;<table style="width: 100%">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 160px">
                            Branch
                        </td>
                        <td style="width: 1px">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px">
                            &nbsp;
                        </td>
                        <td style="width: 1px">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnRefresh" runat="server" CommandArgument="Refresh" CommandName="PromptEmployee"
                                CssClass="button" EnableViewState="False" Text="Refresh" OnClick="btnRefresh_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvwPrompt" runat="server" SkinID="GridViewDefaultSkin" AutoGenerateColumns="False"
                    Width="100%" AllowSorting="True" AllowPaging="True" DataKeyNames="ID" DataSourceID="sdsPrompt"
                    OnRowCreated="gvwPrompt_RowCreated" OnRowDataBound="gvwPrompt_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            InsertVisible="False" ReadOnly="True" />
                        <asp:BoundField DataField="Branch" SortExpression="Branch" HeaderText="Branch" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ContractNo" SortExpression="ContractNo" HeaderText="ContractNo"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Barcode" SortExpression="Barcode" HeaderText="Barcode"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ReadOnly="True" />
                        <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="CustomerName"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Package" SortExpression="Package" HeaderText="Package"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Date" HeaderText="EffectiveDate" SortExpression="EffectiveDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            DataFormatString="{0:dd-MMM-yyyy}" />
                        <asp:BoundField DataField="BillingType" HeaderText="BillingType" SortExpression="BillingType" />                        
                        <asp:TemplateField>
                            <ItemStyle Width="10px" />
                            <ItemTemplate>
                                <asp:HyperLink ID="hypSelect" runat="server" href="#">Select</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        .: No Data :.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="sdsPrompt" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                    SelectCommand="proc_GetPendingContracts" SelectCommandType="StoredProcedure" OnSelecting="sdsPrompt_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="BranchID" Type="Int32" />                        
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
