<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true"
    CodeFile="MasterCustomer.aspx.cs" Inherits="MasterCustomer" StylesheetTheme="Workspace" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Customer
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:MultiView ID="mvwForm" runat="server">
        <asp:View ID="viwRead" runat="server">
            <table class="style1">
                <tr>
                    <td>
                        <table class="style1">
                            <tr>
                                <td class="auto-style2">Branch
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFindBranch" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Barcode
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFindBarcode" runat="server" CssClass="textbox" MaxLength="50"
                                        ValidationGroup="AddEdit" Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Name
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFindName" runat="server" CssClass="textbox" MaxLength="50" ValidationGroup="AddEdit"
                                        Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Parent Name</td>
                                <td style="width: 1px">:</td>
                                <td>
                                    <asp:TextBox ID="txtFindParentName" runat="server" CssClass="textbox" ValidationGroup="AddEdit"
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">Phone No. (Customer/Parent) </td>
                                <td style="width: 1px">:</td>
                                <td>
                                    <asp:TextBox ID="txtFindPhoneNo" runat="server" CssClass="textbox" ValidationGroup="AddEdit"
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td class="style3">&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnRefresh" runat="server" EnableViewState="false" OnClick="btnRefresh_Click"
                                        SkinID="RefreshButton" ValidationGroup="AddEdit" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false" 
                            onclick="lnbAddNew_Click" SkinID="AddNewButton" Text="Add New" 
                            Enabled="False" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false" 
                            onclick="lnbDelete_Click" 
                            OnClientClick="return confirm('Delete marked row(s) ?')" SkinID="DeleteButton" 
                            Text="Delete" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvwMaster" runat="server" SkinID="GridViewDefaultSkin" Width="100%"
                            AutoGenerateColumns="False" DataSourceID="sdsMaster" AllowPaging="True" AllowSorting="True"
                            OnRowCreated="gvwMaster_RowCreated" OnRowCommand="gvwMaster_RowCommand" OnRowDataBound="gvwMaster_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Barcode" HeaderText="Barcode" SortExpression="Barcode"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
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
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypChangeCC" runat="server" Text="Change Credit Card" NavigateUrl="#" ImageUrl="~/images/hand.png"
                                            ToolTip="View credit card change history" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypInvoiceHistory" runat="server" Text="Invoice History" NavigateUrl="#" ImageUrl="~/images/mail_16.png"
                                            ToolTip="View invoice history" />
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
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow"
                                            CommandArgument='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                            SelectCommand="proc_GetAllCustomers" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="BranchID" Type="Int32" />
                                <asp:Parameter Name="Barcode" Type="String" />
                                <asp:Parameter Name="Name" Type="String" />
                                <asp:Parameter Name="ParentName" Type="String" />
                                <asp:Parameter Name="PhoneNo" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="viwAddEdit" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2">Barcode
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="textbox" Width="120px" MaxLength="50"
                            ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator ID="rqvDescription" runat="server" ControlToValidate="txtBarcode"
                            EnableViewState="false" ErrorMessage="<b>Barcode</b> must be specified" ValidationGroup="AddEdit"
                            CssClass="errorMessage" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">First Name
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="textbox" MaxLength="50" ValidationGroup="AddEdit"
                            Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Last Name
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="textbox" MaxLength="50" ValidationGroup="AddEdit"
                            Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Surname
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtSurname" runat="server" CssClass="textbox" MaxLength="50" ValidationGroup="AddEdit"
                            Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Date of Birth
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <ew:CalendarPopup ID="calDate" runat="server" SkinID="Calendar">
                            <TextBoxLabelStyle CssClass="textbox" />
                        </ew:CalendarPopup>
                    </td>
                </tr>
                <tr>
                    <td class="style2">ID Card No.
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtIDCardNo" runat="server" CssClass="textbox" MaxLength="50" ValidationGroup="AddEdit"
                            Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2">Address
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textbox" MaxLength="500" ValidationGroup="AddEdit"
                            Width="500px" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Zip Code
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="textbox" MaxLength="5" ValidationGroup="AddEdit"
                            Width="80px" onkeydown="return NumbersOnly(event)" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2">Mailing Address
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtMailingAddress" runat="server" CssClass="textbox" MaxLength="500"
                            ValidationGroup="AddEdit" Width="500px" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Zip Code
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtMailingZipCode" runat="server" CssClass="textbox" MaxLength="5"
                            ValidationGroup="AddEdit" Width="80px" onkeydown="return NumbersOnly(event)" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2">Email
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" MaxLength="50" ValidationGroup="AddEdit"
                            Width="200px" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                            CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Email Address&lt;/b&gt; is invalid"
                            SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ValidationGroup="AddEdit"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style2">Phone
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="textbox" MaxLength="20" ValidationGroup="AddEdit"
                            Width="150px" onkeydown="return NumbersOnly(event)" />
                        &nbsp;&nbsp;&nbsp;&nbsp; Cell Phone:
                        <asp:TextBox ID="txtCellPhone" runat="server" CssClass="textbox" MaxLength="20" onkeydown="return NumbersOnly(event)"
                            ValidationGroup="AddEdit" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Home Branch
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:Label ID="lblHomeBranch" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Area
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlArea" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style2">School
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:TextBox ID="txtSchoolID" runat="server" style="display:none;"
                                ValidationGroup="AddEdit" Width="0px" />
                        <asp:TextBox ID="txtSchoolName" runat="server" CssClass="textbox" MaxLength="50" ReadOnly="true" ToolTip="Click Look Up to select a school"
                                ValidationGroup="AddEdit" Width="200px" />
                        &nbsp;<asp:HyperLink ID="HyperLink1" NavigateUrl="#" runat="server" onclick="showPromptPopUp('PromptSchool.aspx?callback1=cphMainContent_txtSchoolID&callback2=cphMainContent_txtSchoolName', null, 550, 900);">Look Up</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td class="style2" valign="top">Active Contract
                    </td>
                    <td class="style3" valign="top">:
                    </td>
                    <td>
                        <asp:GridView ID="gvwActiveContracts" runat="server" AutoGenerateColumns="False"
                            SkinID="GridViewDefaultSkin" Width="100%"
                            OnRowCommand="gvwActiveContracts_RowCommand" OnRowDataBound="gvwActiveContracts_RowDataBound">
                            <Columns>
                                <asp:CommandField />
                                <asp:BoundField DataField="ContractNo" HeaderText="ContractNo" HeaderStyle-HorizontalAlign="Left"
                                    ReadOnly="true" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DuesAmount" HeaderText="DuesAmount" HeaderStyle-HorizontalAlign="Right"
                                    ReadOnly="true" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0}" />
                                <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                                    ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PurchaseDate" HeaderText="PurchaseDate" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                                    ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" DataFormatString="{0:ddd, dd-MMM-yyyy}"
                                    ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="ExpiredDate" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("ExpiredDate")).ToString("ddd, dd-MMM-yyyy") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <ew:CalendarPopup ID="calExpiredDate" runat="server" SkinID="Calendar" SelectedDate='<%# Eval("ExpiredDate") %>'>
                                            <TextBoxLabelStyle CssClass="textbox" />
                                        </ew:CalendarPopup>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NextDuesDate" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# Convert.ToDateTime(Eval("NextDuesDate")).ToString("ddd, dd-MMM-yyyy") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <ew:CalendarPopup ID="calNextDuesDate" runat="server" SkinID="Calendar" SelectedDate='<%# Eval("NextDuesDate") %>'>
                                            <TextBoxLabelStyle CssClass="textbox" />
                                        </ew:CalendarPopup>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnbEditContract" runat="server" CommandName="EditContract" CommandArgument='<%# Eval("ContractNo") %>' Enabled='<%# Convert.ToString(Eval("Status")).ToUpper() != "CLOSED" && Convert.ToString(Eval("Status")).ToUpper() != "VOID"  %>'
                                            Text="Edit" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnbDeletePackage" runat="server" CommandName="DeleteContract" Visible="false"
                                            CommandArgument='<%# Eval("ContractNo") %>' Text="Delete" OnClientClick="return confirm('Are you sure want to delete this row?')" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnbSaveContract" runat="server" CommandName="SaveContract" CommandArgument='<%# Eval("ContractNo") %>'
                                            Text="Save" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnbCancelPackage" runat="server" CommandName="CancelContract"
                                            CommandArgument='<%# Eval("ContractNo") %>' Text="Cancel" OnClientClick="return confirm('Cancel current operation ?')" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Status" HeaderText="Contract Status" HeaderStyle-HorizontalAlign="Left" ReadOnly="true"
                                    ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="style2" valign="top">Partner
                    </td>
                    <td class="style3" valign="top">:
                    </td>
                    <td>
                        <asp:HiddenField ID="hidPartner" runat="server" />
                        <asp:TextBox ID="txtPartner" runat="server" CssClass="textbox" MaxLength="20" ValidationGroup="AddEdit"
                            Width="150px" />
                        <asp:HyperLink ID="hypLookUpPartner" NavigateUrl="#" runat="server" onclick="showPromptPopUp('PromptCustomer.aspx?BranchLocked=1', this.previousSibling.previousSibling.id, 550, 900);">Look Up</asp:HyperLink>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CustomValidator ID="cuvPartner" runat="server" ControlToValidate="txtPartner"
                            CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Partner&lt;/b&gt; is invalid"
                            SetFocusOnError="True" ValidationGroup="AddEdit" OnServerValidate="cuvPartner_ServerValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style2" valign="top">Parents
                    </td>
                    <td class="style3" valign="top">:
                    </td>
                    <td>
                        <asp:HyperLink ID="hypParent" runat="server" EnableViewState="False" NavigateUrl="#">Click here to modify information of Parents / Guardians / Pick Up Persons</asp:HyperLink>
                        <asp:GridView ID="gvwParents" runat="server" AutoGenerateColumns="False" DataSourceID="sdsParents"
                            SkinID="GridViewDefaultSkin" Width="700px">
                            <Columns>
                                <asp:BoundField DataField="Connection" HeaderText="Connection" ReadOnly="True" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="Connection" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="BirthDate" HeaderText="BirthDate" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="BirthDate" DataFormatString="{0:ddd, dd-MMM-yyyy}" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Phone1" HeaderText="Phone1" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="Phone1" />
                                <asp:BoundField DataField="Phone2" HeaderText="Phone2" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="Phone2" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sdsParents" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>"
                            SelectCommand="proc_GetCustomerParents" SelectCommandType="StoredProcedure" OnSelecting="sdsParents_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="CustomerBarcode" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2">Billing Type
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBillingType" runat="server" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="billing">
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td class="style2">Card Number
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCardNo" runat="server" CssClass="textbox" MaxLength="20" ValidationGroup="AddEdit"
                                        onkeydown="return NumbersOnly(event)" Width="150px" />
                                    &nbsp;<asp:DropDownList ID="ddlCreditCardType" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:CustomValidator ID="cuvCardNo" runat="server" ControlToValidate="txtCardNo"
                                        CssClass="errorMessage" EnableViewState="False" ErrorMessage="&lt;b&gt;Card Number&lt;/b&gt; must be specified if Billing type is set to Auto Payment"
                                        OnServerValidate="cuvCardNo_ServerValidate" SetFocusOnError="True"
                                        ValidationGroup="AddEdit" Display="Dynamic"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cuvCreditCardNo" runat="server" ControlToValidate="txtCardNo" CssClass="errorMessage" EnableClientScript="False" EnableViewState="False" ErrorMessage="&lt;b&gt;Credit Card Number&lt;/b&gt; is invalid" OnServerValidate="cuvCreditCardNo_ServerValidate" SetFocusOnError="True" ToolTip="Credit Card Number is invalid" ValidationGroup="AddEdit"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">Card Holder Name
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCardHolderName" runat="server" CssClass="textbox" MaxLength="50"
                                        ValidationGroup="AddEdit" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">Card Holder ID No.
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCardHolderID" runat="server" CssClass="textbox" MaxLength="20"
                                        onkeydown="return NumbersOnly(event)" ValidationGroup="AddEdit" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">Issued by Bank
                                </td>
                                <td class="style3">:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp; Card Expired Date:
                                    <ew:CalendarPopup ID="calExpiredDate" runat="server" SkinID="Calendar">
                                        <TextBoxLabelStyle CssClass="textbox" />
                                    </ew:CalendarPopup>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2">Photo
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:FileUpload ID="fupPhoto" runat="server" />
                        <asp:CheckBox ID="chkDeletePhoto" runat="server" Text="Delete current photo" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>
                        <asp:Image ID="imgPhoto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">Status
                    </td>
                    <td class="style3">:
                    </td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Large" />
                        &nbsp;<asp:HyperLink ID="hypViewStatusHistory" runat="server" NavigateUrl="#">View Status History</asp:HyperLink>
                        <asp:HiddenField ID="hidStatus" runat="server" />
                        <br />
                        <asp:Label ID="lblStatusNotes" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;
                    </td>
                    <td class="style3">&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" SkinID="SaveButton" EnableViewState="false"
                            OnClick="btnSave_Click" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" SkinID="CancelButton" EnableViewState="false"
                            ValidationGroup="AddEdit" CausesValidation="false" OnClientClick="return confirm('Are you sure want to cancel ?')"
                            OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var ddlBillingType = $("#<%= ddlBillingType.ClientID %>").get(0);
            if (ddlBillingType != null) {
                if (ddlBillingType.item(ddlBillingType.selectedIndex).text != "Manual Payment") {
                    $("#billing").show();
                }
                else {
                    $("#billing").hide();
                }
            }
        });

        $("#<%= ddlBillingType.ClientID %>").change(function () {
            var ddlBillingType = $("#<%= ddlBillingType.ClientID %>").get(0);
            if (ddlBillingType != null) {
                if (ddlBillingType.item(ddlBillingType.selectedIndex).text != "Manual Payment")
                    $("#billing").show();
                else
                    $("#billing").hide();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphHead">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 140px;
        }

        .style3 {
            width: 1px;
        }

        .style4 {
            width: 130px;
        }

        .auto-style1 {
            width: 131px;
        }

        .auto-style2 {
            width: 181px;
        }
    </style>
</asp:Content>
