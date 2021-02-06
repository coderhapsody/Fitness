<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="MasterProspect.aspx.cs" Inherits="MasterProspect" StylesheetTheme="Workspace" Culture="id-ID" UICulture="id-ID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainTitle" runat="server">
    Prospect
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scmScriptManager"></asp:ScriptManager>
    <asp:MultiView runat="server" ID="mvwForm">
        <asp:View runat="server" ID="viwRead">
            <table class="fullwidth">
                <tr>
                    <td style="width: 120px">Branch</td>
                    <td style="width: 2px">:</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlBranch" Width="250px"></telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Search by</td>
                    <td>:</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlSearchBy" OnClientSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged">
                            <Items>
                                <telerik:DropDownListItem runat="server" Text="Entry Month/Year" Value="0" />
                                <telerik:DropDownListItem runat="server" Text="Prospect Name" Value="1" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr id="searchBy0">
                    <td style="width: 120px">Prospect Name</td>
                    <td style="width: 2px">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFindByProspectName" />
                    </td>
                </tr>
                <tr id="searchBy1">
                    <td style="width: 120px">Entry Month / Year</td>
                    <td style="width: 2px">:</td>
                    <td>
                        <telerik:RadMonthYearPicker runat="server" ID="mypMonthYear" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px"></td>
                    <td style="width: 2px"></td>
                    <td>
                        <telerik:RadButton ID="btnRefresh" runat="server" Text="Refresh" EnableViewState="False" OnClick="btnRefresh_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:LinkButton ID="lnbAddNew" runat="server" EnableViewState="false"
                Text="Add New" SkinID="AddNewButton" OnClick="lnbAddNew_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnbDelete" runat="server" EnableViewState="false"
                            Text="Delete" OnClientClick="return confirm('Delete marked row(s) ?')"
                            SkinID="DeleteButton" OnClick="lnbDelete_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

            <asp:UpdatePanel runat="server" ID="updProspect" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:GridView runat="server" ID="gvwMaster" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" SkinID="GridViewDefaultSkin" Width="100%" OnRowCommand="gvwMaster_RowCommand" OnRowCreated="gvwMaster_RowCreated" OnRowDataBound="gvwMaster_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Date" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Date" SortExpression="Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Phone1" HeaderText="Phone1" SortExpression="Phone1" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Consultant" HeaderText="Consultant" ReadOnly="True" SortExpression="Consultant" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="IdentityNo" HeaderText="IdentityNo" ReadOnly="True" SortExpression="IdentityNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="ProspectSource" HeaderText="ProspectSource" ReadOnly="True" SortExpression="ProspectSource" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hypFollowUp" runat="server" NavigateUrl="#" ToolTip="Follow Ups" ImageUrl="~/Images/DialHS.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbEdit" runat="server" SkinID="EditButton" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkDelete" runat="server" ToolTip="Mark this row to delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" SelectCommand="proc_GetAllProspects" SelectCommandType="StoredProcedure" OnSelecting="sdsMaster_Selecting">
                        <SelectParameters>
                            <asp:Parameter Name="BranchID" Type="Int32" />
                            <asp:Parameter Name="SearchBy" Type="Int32" />
                            <asp:Parameter Name="ProspectName" Type="String" />
                            <asp:Parameter Name="Month" Type="Int32" />
                            <asp:Parameter Name="Year" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <script>
                $(function () {
                    $("#searchBy0").hide();
                });

                function ddlSearchBy_SelectedIndexChanged(sender, args) {
                    var ddlSearchBy = $find("<%=ddlSearchBy.ClientID%>");
                    var value = ddlSearchBy.get_selectedItem().get_value();
                    if (value == 0) {
                        $("#searchBy0").hide();
                        $("#searchBy1").show();
                    }
                    else if (value == 1) {
                        $("#searchBy1").hide();
                        $("#searchBy0").show();
                    }

                }
            </script>
        </asp:View>
        <asp:View runat="server" ID="viwAddEdit">
            <table class="fullwidth">
                <tr>
                    <td style="width: 150px">Branch</td>
                    <td style="width: 3px">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" />
                        <asp:HiddenField runat="server" ID="hidBranchID" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">First Name</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator runat="server" ID="rqvFirstName" ControlToValidate="txtFirstName" EnableViewState="False" ErrorMessage="<b>First Name</b> must be specified" SetFocusOnError="True" ValidationGroup="AddEdit" CssClass="errorMessage" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">Last Name</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" ValidationGroup="AddEdit" /></td>
                </tr>
                <tr>
                    <td style="width: 150px">Entry Date</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="dtpDate" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">Identity No.</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtIdentityNo" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">Phone 1</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPhone1" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator runat="server" ID="rqvPhone1" ControlToValidate="txtPhone1" EnableViewState="False" ErrorMessage="<b>Phone number</b> must be specified" SetFocusOnError="True" ValidationGroup="AddEdit" CssClass="errorMessage" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">Phone 2</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPhone2" ValidationGroup="AddEdit" /></td>
                </tr>
                <tr>
                    <td style="width: 150px">Email</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmail" ValidationGroup="AddEdit" /></td>
                </tr>
                <tr>
                    <td style="width: 150px">Date of Birth</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="dtpDateOfBirth" ValidationGroup="AddEdit" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">Consultant</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlConsultant" DropDownHeight="100px" Width="250px" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator runat="server" ID="rqvConsultant" ControlToValidate="ddlConsultant" EnableViewState="False" ErrorMessage="<b>Consultant</b> must be specified" SetFocusOnError="True" ValidationGroup="AddEdit" CssClass="errorMessage" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px; height: 18px;">Source</td>
                    <td style="width: 3px; height: 18px;">:</td>
                    <td style="height: 18px">
                        <telerik:RadDropDownList runat="server" ID="ddlSource" ValidationGroup="AddEdit" />
                        <asp:RequiredFieldValidator runat="server" ID="rqvSource" ControlToValidate="ddlSource" EnableViewState="False" ErrorMessage="<b>Source</b> must be specified" SetFocusOnError="True" ValidationGroup="AddEdit" CssClass="errorMessage" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">Source Reference</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSourceRef" ValidationGroup="AddEdit" /></td>
                </tr>
                <tr>
                    <td style="width: 150px">&nbsp;</td>
                    <td style="width: 3px">&nbsp;</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkEnableFreeTrial" Text="Enable Free Trial" />
                        <div id="freetrial">
                            <div class="tableContainer" style="width: 500px;">
                                <div class="tableRow">
                                    <div class="tableCol" style="width: 50px;">From</div>
                                    <div class="tableCol" style="width: 5px;">:</div>
                                    <div class="tableCol" style="width: 100px;">
                                        <telerik:RadDatePicker runat="server" ID="dtpFreeTrialFrom"></telerik:RadDatePicker>
                                    </div>
                                    <div class="tableCol" style="width: 50px;"></div>
                                    <div class="tableCol" style="width: 50px;">To</div>
                                    <div class="tableCol" style="width: 5px;">:</div>
                                    <div class="tableCol" style="width: 100px;">
                                        <telerik:RadDatePicker runat="server" ID="dtpFreeTrialTo"></telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">Notes</td>
                    <td style="width: 3px">:</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtNotes" ValidationGroup="AddEdit" TextMode="MultiLine" Width="400px" Height="80px" Rows="5" /></td>
                </tr>
                <tr>
                    <td style="width: 150px">&nbsp;</td>
                    <td style="width: 3px">&nbsp;</td>
                    <td>
                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" SingleClick="True" SingleClickText="Saving"
                            EnableViewState="false" OnClick="btnSave_Click" ValidationGroup="AddEdit" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel"
                            EnableViewState="false" ValidationGroup="AddEdit" CausesValidation="false"
                            OnClientClicking="CancelConfirm"
                            OnClick="btnCancel_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblStatus" runat="server" CssClass="errorMessage" EnableViewState="false" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" runat="server">
    <script>
        $(function () {
            var chkEnableFreeTrial = $get("<%=chkEnableFreeTrial.ClientID%>");

            $("#<%=chkEnableFreeTrial.ClientID%>").click(function () {
                if ($get("<%=chkEnableFreeTrial.ClientID%>").checked)
                    $("#freetrial").show();
                else
                    $("#freetrial").hide();

            });

            if (chkEnableFreeTrial != null) {
                if ($get("<%=chkEnableFreeTrial.ClientID%>").checked)
                    $("#freetrial").show();
                else
                    $("#freetrial").hide();
            }
        })
    </script>
</asp:Content>


