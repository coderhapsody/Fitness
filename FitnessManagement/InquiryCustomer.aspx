<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="InquiryCustomer.aspx.cs" Inherits="InquiryCustomer" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Inquiry Customer
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="style1">Branch
            </td>
            <td class="style2">:
            </td>
            <td>
                <asp:DropDownList ID="ddlFindBranch" runat="server" CssClass="dropdown" />
            </td>
        </tr>
        <tr>
            <td class="style1">Barcode
            </td>
            <td class="style2">:
            </td>
            <td>
                <asp:TextBox ID="txtFindBarcode" runat="server" CssClass="textbox" MaxLength="50"
                    ValidationGroup="AddEdit" Width="120px" />
            </td>
        </tr>
        <tr>
            <td class="style1">First Name / Last Name
            </td>
            <td class="style2">:
            </td>
            <td>
                <asp:TextBox ID="txtFindName" runat="server" CssClass="textbox" MaxLength="50" ValidationGroup="AddEdit"
                    Width="200px" />
            </td>
        </tr>
        <tr>
            <td class="style1">Surname
            </td>
            <td class="style2">:
            </td>
            <td>
                <asp:TextBox ID="txtFindSurname" runat="server" CssClass="textbox" MaxLength="50"
                    ValidationGroup="AddEdit" Width="200px" />
            </td>
        </tr>
        <tr>
            <td class="style1">ID Card No.&nbsp;
            </td>
            <td class="style2">:
            </td>
            <td>
                <asp:TextBox ID="txtFindIDCardNo" runat="server" CssClass="textbox" MaxLength="50"
                    ValidationGroup="AddEdit" Width="200px" />
            </td>
        </tr>
        <tr>
            <td class="style1">Birth Date
            </td>
            <td class="style2">:
            </td>
            <td>From
                <ew:CalendarPopup ID="calFindBirthDateFrom" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
                &nbsp;
                <asp:CheckBox ID="chkUnknownBirthDateFrom" runat="server" Text="Unknown" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;to
                <ew:CalendarPopup ID="calFindBirthDateTo" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
                <asp:CheckBox ID="chkUnknownBirthDateTo" runat="server" Text="Unknown" />
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;
            </td>
            <td class="style2">&nbsp;
            </td>
            <td>
                <asp:Button ID="btnRefresh" runat="server" EnableViewState="false" OnClick="btnRefresh_Click"
                    SkinID="RefreshButton" ValidationGroup="AddEdit" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False" DataSourceID="sdsMaster"
        SkinID="GridViewDefaultSkin" Width="100%" AllowPaging="True" AllowSorting="True"
        OnRowCreated="gvwMaster_RowCreated" OnRowDataBound="gvwMaster_RowDataBound">
        <Columns>
            <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="DateOfBirth" HeaderText="DateOfBirth" SortExpression="DateOfBirth"
                DataFormatString="{0:dd-MMM-yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ContractStatus" HeaderText="ContractStatus" SortExpression="ContractStatus"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="StatusMembership" HeaderText="StatusMembership" SortExpression="StatusMembership"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
            <asp:BoundField DataField="NextDuesDate" HeaderText="NextDuesDate" SortExpression="NextDuesDate"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
            <asp:BoundField DataField="ActiveDate" HeaderText="ActiveDate" SortExpression="ActiveDate"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="HomePhone" HeaderText="HomePhone" SortExpression="HomePhone"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CellPhone1" HeaderText="CellPhone" SortExpression="CellPhone1"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:HyperLink ID="hypInvoiceHistory" runat="server" Text="Invoice History" NavigateUrl="#" ImageUrl="~/images/mail_16.png"
                        ToolTip="View invoice history" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:HyperLink ID="hypDetail" runat="server" Text="Detail" NavigateUrl="#" ImageUrl="~/images/zoom.png"
                        ToolTip="View detail" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:HyperLink ID="hypCheckInHistory" runat="server" Text="Check-in History" NavigateUrl="#" ImageUrl="~/images/list_components.gif"
                        ToolTip="View check-in history" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                <ItemTemplate>
                    <asp:HyperLink ID="hypNotes" runat="server" Text="Notes" NavigateUrl="#" ImageUrl="~/images/NewDocumentHS.png"
                        ToolTip="View notes" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="hypPrint" runat="server" Text="Print" NavigateUrl="#" ImageUrl="~/images/PrintHS.png"
                        ToolTip="Print" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
        SelectCommand="proc_InquiryCustomer" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
        <SelectParameters>
            <asp:Parameter Name="BranchID" Type="Int32" />
            <asp:Parameter Name="Barcode" Type="String" />
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Surname" Type="String" />
            <asp:Parameter Name="IDCardNo" Type="String" />
            <asp:Parameter Name="DateOfBirthFrom" Type="String" />
            <asp:Parameter Name="DateOfBirthTo" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 161px;
        }

        .style2 {
            width: 6px;
        }
    </style>
</asp:Content>
