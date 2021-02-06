<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPrompt.master" AutoEventWireup="true" CodeFile="ChangeCreditCard.aspx.cs" Inherits="ChangeCreditCard" StylesheetTheme="Workspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 220px;
        }

        .auto-style3 {
            width: 4px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Change Credit Card Information        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <table class="ui-accordion">
        <tr>
            <td class="auto-style1">Customer</td>
            <td class="auto-style3">:</td>
            <td>
                <asp:Label ID="lblCustomer" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Credit Card No.</td>
            <td class="auto-style3">:</td>
            <td>
                <asp:TextBox ID="txtCreditCardNo" runat="server" CssClass="textbox" Width="200px" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rqvCreditCardNo" runat="server" ControlToValidate="txtCreditCardNo" CssClass="errorMessage" EnableViewState="False" ErrorMessage="Credit card number must be specified" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Credit Card Type</td>
            <td class="auto-style3">:</td>
            <td>
                <asp:DropDownList ID="ddlCreditCardType" runat="server" CssClass="dropdown">
                </asp:DropDownList>
            &nbsp;<asp:DropDownList ID="ddlBank" runat="server" CssClass="dropdown">
                </asp:DropDownList>
            &nbsp;<asp:RequiredFieldValidator ID="rqvBank" runat="server" ControlToValidate="ddlBank" CssClass="errorMessage" EnableViewState="False" ErrorMessage="Bank must be specified" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Card Holder Name</td>
            <td class="auto-style3">:</td>
            <td>
                <asp:TextBox ID="txtCardHolderName" runat="server" CssClass="textbox" Width="250px" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rqvCardHolderName" runat="server" ControlToValidate="txtCardHolderName" CssClass="errorMessage" EnableViewState="False" ErrorMessage="Card holder name must be specified" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Card Holder Identity No.</td>
            <td class="auto-style3">:</td>
            <td>
                <asp:TextBox ID="txtCardHolderIDNo" runat="server" CssClass="textbox" Width="200px" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rqvCardHolderIDNo" runat="server" ControlToValidate="txtCardHolderIDNo" CssClass="errorMessage" EnableViewState="False" ErrorMessage="Card holder identity number must be specified" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Card Expire Date</td>
            <td class="auto-style3">:</td>
            <td>
                <ew:CalendarPopup ID="calExpireDate" runat="server" SkinID="Calendar">
                    <TextBoxLabelStyle CssClass="textbox" />
                </ew:CalendarPopup>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">Reason</td>
            <td class="auto-style3">:</td>
            <td>
                <asp:TextBox ID="txtReason" runat="server" Columns="80" CssClass="textbox" Rows="5" TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rqvReason" runat="server" ControlToValidate="txtReason" CssClass="errorMessage" EnableViewState="False" ErrorMessage="Reason must be specified" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td><asp:Label ID="lblStatus" runat="server" EnableViewState="false" /></td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td>
                <asp:Button ID="btnSave" runat="server" CssClass="button" EnableViewState="False" Text="Save" OnClick="btnSave_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnClose" runat="server" CssClass="button" EnableViewState="False" Text="Close This Window" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvwMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsMaster" OnRowCreated="gvwMaster_RowCreated" SkinID="GridViewDefaultSkin" Width="100%">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="CustomerBarcode" HeaderText="CustomerBarcode" SortExpression="CustomerBarcode" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CreditCardType" HeaderText="Type" SortExpression="CreditCardType" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Bank" HeaderText="Bank" SortExpression="Bank" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CreditCardNo" HeaderText="CreditCardNo" SortExpression="CreditCardNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CreditCardHolderName" HeaderText="CardHolderName" SortExpression="CreditCardHolderName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CreditCardExpiredDate" HeaderText="ExpiredDate" SortExpression="CreditCardExpiredDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CreditCardIDNo" HeaderText="Card Holder Identity No." SortExpression="CreditCardIDNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ChangedWhen" HeaderText="UpdateWhen" SortExpression="ChangedWhen" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ChangedWho" HeaderText="UpdateBy" SortExpression="ChangedWho" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
        </Columns>

    </asp:GridView>
    <asp:SqlDataSource ID="sdsMaster" runat="server" ConnectionString="<%$ ConnectionStrings:FitnessConnectionString %>" OnSelecting="sdsMaster_Selecting" SelectCommand="proc_GetCreditCardChangeHistory" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="CustomerBarcode" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <script type="text/javascript">
        $("textarea").keypress(function (event) {
            // Check the keyCode and if the user pressed Enter (code = 13) 
            // disable it
            if (event.keyCode == 13) {
                event.preventDefault();
            }
        });

        $("[id$='btnClose']").click(function () {
            window.close();
        });
    </script>
</asp:Content>

