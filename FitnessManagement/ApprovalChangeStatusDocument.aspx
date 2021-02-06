<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="ApprovalChangeStatusDocument.aspx.cs" Inherits="ApprovalChangeStatusDocument" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Approval Change Status
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">

    <asp:MultiView ID="mvwForm" ActiveViewIndex="0" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td>  
                        <table class="ui-accordion">
                            <tr>
                                <td class="style3">Branch</td>
                                <td class="style4">:</td>
                                <td>
                                    <asp:DropDownList ID="ddlFindBranch" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">Document Type</td>
                                <td class="style4">:</td>
                                <td>
                                    <asp:DropDownList ID="ddlFindDocumentType" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">Period</td>
                                <td class="style4">:</td>
                                <td>From Date
                                    <ew:CalendarPopup ID="calFindFromDate" runat="server" SkinID="Calendar">
                                        <TextBoxLabelStyle CssClass="textbox" />
                                    </ew:CalendarPopup>
                                    &nbsp;&nbsp;&nbsp; To Date
                                    <ew:CalendarPopup ID="calFindToDate" runat="server" SkinID="Calendar">
                                        <TextBoxLabelStyle CssClass="textbox" />
                                    </ew:CalendarPopup>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">Customer Barcode</td>
                                <td class="style4">:</td>
                                <td>
                                    <asp:TextBox ID="txtFindCustomerCode" runat="server" CssClass="textbox"
                                        MaxLength="50" ValidationGroup="AddEdit" Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">&nbsp;</td>
                                <td class="style4">&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="button"
                                        EnableViewState="False" Text="Refresh" OnClick="btnRefresh_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False"
                DataSourceID="sdsMaster" SkinID="GridViewDefaultSkin" Width="100%"
                OnRowCreated="gvwMaster_RowCreated" OnRowCommand="gvwMaster_RowCommand"
                AllowPaging="True" AllowSorting="True"
                OnRowDataBound="gvwMaster_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DocumentNo" HeaderText="DocumentNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="DocumentNo" />
                    <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="CustomerCode" />
                    <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="CustomerName" />
                    <asp:BoundField DataField="EmployeeCode" HeaderText="Issued By" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="EmployeeName" />
                    <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="DocumentType" />
                    <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="DocumentStatus" />
                    <asp:BoundField DataField="StartDate" HeaderText="StartDate" DataFormatString="{0:dddd, dd MMMM yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="StartDate" />
                    <asp:BoundField DataField="EndDate" HeaderText="EndDate" DataFormatString="{0:dddd, dd MMMM yyyy}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        SortExpression="EndDate" />
                    <asp:BoundField DataField="ApprovedDate" HeaderText="ApprovedDate"
                        SortExpression="ApprovedDate" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sdsMaster" runat="server"
                ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                SelectCommand="proc_GetAllChangeStatusDocuments"
                SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="BranchID" Type="Int32" />
                    <asp:Parameter Name="DocumentTypeID" Type="Int32" />
                    <asp:Parameter Name="DateFrom" Type="String" />
                    <asp:Parameter Name="DateTo" Type="String" />
                    <asp:Parameter Name="CustomerCode" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <table class="ui-accordion">
                <tr>
                    <td class="style1">Branch</td>
                    <td class="style2">:</td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">Document No.</td>
                    <td class="style2">:</td>
                    <td>
                        <asp:Label ID="lblDocumentNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">Document Type</td>
                    <td class="style2">:</td>
                    <td>
                        <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style1">Start Date</td>
                    <td class="style2">:</td>
                    <td>
                        <ew:CalendarPopup ID="calStartDate" runat="server" SkinID="Calendar">
                            <TextBoxLabelStyle CssClass="textbox" />
                        </ew:CalendarPopup>
                    </td>
                </tr>
                <tr>
                    <td class="style1">End Date</td>
                    <td class="style2">:</td>
                    <td>
                        <span id="enddate">
                            <ew:CalendarPopup ID="calEndDate" runat="server" SkinID="Calendar">
                                <TextBoxLabelStyle CssClass="textbox" />
                            </ew:CalendarPopup>
                        </span>
                        <asp:CheckBox ID="chkEndDate" runat="server" Text="Specify End Date" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">Customer</td>
                    <td class="style2">:</td>
                    <td>
                        <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="textbox" MaxLength="50"
                            ValidationGroup="AddEdit" Width="150px" />&nbsp;<asp:HyperLink
                                ID="hypPromptCustomer" NavigateUrl="#" runat="server"
                                onclick="showPromptPopUp('PromptCustomer.aspx?', this.previousSibling.previousSibling.id, 550, 900);">Look Up</asp:HyperLink>
                        &nbsp;
                        <asp:CustomValidator ID="cuvCustomerCode" runat="server"
                            ClientValidationFunction="ValidateCustomerCode"
                            ControlToValidate="txtCustomerCode" CssClass="errorMessage"
                            EnableViewState="False"
                            ErrorMessage="&lt;b&gt;Customer&lt;/b&gt; must be specified with valid customer code"
                            OnServerValidate="cuvCustomerCode_ServerValidate" SetFocusOnError="True"
                            ValidateEmptyText="True" ValidationGroup="AddEdit"></asp:CustomValidator>
                    </td>
                </tr>

                <tr>
                    <td class="style1" valign="top">Notes</td>
                    <td class="style2" valign="top">:</td>
                    <td>
                        <asp:TextBox ID="txtNotes" runat="server" Columns="80" CssClass="textbox"
                            MaxLength="50" Rows="8" TextMode="MultiLine" ValidationGroup="AddEdit" />
                        <br />
                        <asp:RequiredFieldValidator ID="rqvNotes" runat="server"
                            ControlToValidate="txtNotes" CssClass="errorMessage" EnableViewState="False"
                            ErrorMessage="&lt;b&gt;Notes&lt;/b&gt; must be specified"
                            SetFocusOnError="True" ValidationGroup="AddEdit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style1">Approval</td>
                    <td class="style2">:</td>
                    <td>
                        <asp:Label ID="lblApprovalStatus" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">&nbsp;</td>
                    <td class="style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="style1">&nbsp;</td>
                    <td class="style2">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">&nbsp;</td>
                    <td class="style2">&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" EnableViewState="false"
                            OnClick="btnSave_Click" SkinID="SaveButton" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="false"
                            EnableViewState="false" OnClick="btnCancel_Click"
                            OnClientClick="return confirm('Are you sure want to cancel ?')"
                            SkinID="CancelButton" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnApprove" runat="server" CssClass="button"
                            EnableViewState="false" OnClick="btnApprove_Click" Text="Approve"
                            OnClientClick="return confirm('Are you sure want to approve ?')"
                            ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                        <asp:Button ID="btnVoid" runat="server" CausesValidation="false" 
                            CssClass="button" EnableViewState="false" OnClick="btnVoid_Click"
                            OnClientClick="return confirm('Are you sure want to void ?')" Text="Void"
                            ValidationGroup="AddEdit" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

    <script language="javascript" type="text/javascript">
        function ValidateCustomerCode(source, arg) {
            arg.IsValid = $('#<%= txtCustomerCode.ClientID %>').get(0).value != "";
        }

        $(document).ready(function () {
            if ($("#<%= chkEndDate.ClientID %>").is(":checked")) {
                $("#enddate").show();
            }
            else {
                $("#enddate").hide();
            }
        });

        $("#<%= chkEndDate.ClientID %>").click(
            function () {
                $("#enddate").toggle();
            });

    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 140px;
        }

        .style2 {
            width: 2px;
        }

        .style3 {
            width: 130px;
        }

        .style4 {
            width: 1px;
        }
    </style>
</asp:Content>

