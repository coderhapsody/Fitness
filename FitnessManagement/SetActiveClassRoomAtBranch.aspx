<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="SetActiveClassRoomAtBranch.aspx.cs" Inherits="SetActiveClassRoomAtBranch" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Setting Active Rooms at Branch
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ScriptManager ID="scmScriptManager" runat="server" />
    <table class="style1">
        <tr>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style2">Select Class</td>
                        <td class="style3">:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlItem" runat="server" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DataList ID="dlsBranches" runat="server">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkBranch" runat="server" Text='<%# Eval("Name") %>' Value='<%# Eval("ID") %>' />
                                <asp:TextBox ID="txtCapacity" runat="server" CssClass="textbox" Width="50px" />
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlItem" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <p>
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Save" SkinID="SaveButton"
                        EnableViewState="False" OnClick="btnSave_Click" />
                </p>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        Sys.Application.add_load(pageLoad);


        function pageLoad() {
            $get('<%= btnSave.ClientID %>').disabled = $get('<%= ddlItem.ClientID %>').selectedIndex == 0;
                    if ($get('<%= btnSave.ClientID %>').disabled)
                        $("#<%= dlsBranches.ClientID %>").hide();
                else
                    $("#<%= dlsBranches.ClientID %>").show();
            }

    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 130px;
        }

        .style3 {
            width: 1px;
        }
    </style>


</asp:Content>


