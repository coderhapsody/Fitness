<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true" CodeFile="PromptSchool.aspx.cs" Inherits="PromptSchool" StylesheetTheme="Workspace" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Look Up School
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <AjaxToolkit:ToolkitScriptManager ID="tsmAjaxToolkitScriptManager" runat="server" />
    &nbsp;<table style="width: 100%">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 160px">
                            School Name
                        </td>
                        <td style="width: 1px">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtFindSchoolName" runat="server" CssClass="flat" />
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
                        <asp:BoundField DataField="Name" HeaderText="School Name" SortExpression="Name" />                        
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
                    SelectCommand="proc_GetAllSchools" SelectCommandType="StoredProcedure" OnSelecting="sdsPrompt_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="SchoolName" Type="String" />                        
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>


