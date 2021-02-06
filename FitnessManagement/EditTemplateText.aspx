<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="EditTemplateText.aspx.cs" Inherits="EditTemplateText" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <link rel='stylesheet' href='CLEditor/jquery.cleditor.css' />
    <style type="text/css">
        .ui-tabs {
            font-size: 100%;
            width: 1050px;
        }
    </style>
    <script type='text/javascript' language='javascript' src='CLEditor/jquery.cleditor.min.js'></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Edit Template Text
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div id="tabs">
        <ul>
            <li><a href="#tab1">Presigning Contract</a></li>
            <li><a href="#tab2">Terms and Conditions</a></li>
            <li><a href="#tab3">Sales Receipt Footer Text</a></li>
        </ul>
        <div id="tab1">
            <asp:TextBox ID="txtPresigningNotice" runat="server" CssClass="textbox" Rows="25" TextMode="MultiLine" Font-Names="Courier New" Font-Size="10pt"
                ValidationGroup="AddEdit" Width="100%"></asp:TextBox>
        </div>
        <div id="tab2">
            <asp:TextBox ID="txtTermsConditions" TextMode="MultiLine" Rows="25" runat="server" Font-Names="Courier New" Font-Size="10pt"
                CssClass="textbox" ValidationGroup="AddEdit" MaxLength="4000" Width="100%"></asp:TextBox>
        </div>
        <div id="tab3">
            <asp:TextBox ID="txtReceiptFooterText" TextMode="MultiLine" Rows="25" runat="server" Font-Names="Courier New" Font-Size="10pt"
                CssClass="textbox" ValidationGroup="AddEdit" MaxLength="4000" Width="100%"></asp:TextBox>
        </div>
    </div>
    <br />
    <asp:Label ID="lblStatus" runat="server" EnableViewState="false" />
    <br />
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button"
        EnableViewState="false" OnClick="btnSave_Click" />
    &nbsp;    &nbsp;&nbsp;&nbsp;&nbsp;
    <input type="reset" value="Undo" class="button" />

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            $("#tabs").tabs();
            $("#<%= txtPresigningNotice.ClientID %>").cleditor({ width: 1000, height: 400 });
            $("#<%= txtTermsConditions.ClientID %>").cleditor({ width: 1000, height: 400 });
            $("#<%= txtReceiptFooterText.ClientID %>").cleditor({ width: 1000, height: 400 });
        });
    </script>

</asp:Content>
