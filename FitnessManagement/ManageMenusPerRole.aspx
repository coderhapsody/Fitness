<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="ManageMenusPerRole.aspx.cs" Inherits="ManageMenusPerRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        #kiri {
            float: left;
            width: 300px;
        }

        #kanan {
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" Runat="Server">
    Menus per Role
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:ScriptManager ID="scmScriptManager" runat="server" />
    <div id="kiri">
        <asp:TreeView ID="tvwMenus" runat="server" OnSelectedNodeChanged="tvwMenus_SelectedNodeChanged" />
    </div>
    <div id="kanan">
        <p>
            <em>Select menu from left tree node and set the permission for specified roles.</em>
        </p>
        <asp:UpdateProgress ID="updProgress" runat="server" AssociatedUpdatePanelID="updRoles">
            <ProgressTemplate>
                Please wait...
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="updRoles" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:CheckBoxList ID="cblRoles" runat="server" RepeatLayout="Table" RepeatColumns="3" CellSpacing="5" CellPadding="5" />                    
                <p>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" />
            </p>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="tvwMenus" EventName="SelectedNodeChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        
    </div>
</asp:Content>

