<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="CheckIn.aspx.cs" Inherits="CheckIn" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Check-In
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ScriptManager ID="scmScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="AjaxService.svc" />
        </Services>
    </asp:ScriptManager>
    <div style="text-align: center; width: 450px; float: left; margin-right: 10px;">
        <fieldset style="height: 450px;">
            <legend id="branchName"></legend>

            <asp:TextBox ID="txtBarcode" Font-Size="600%" runat="server" CssClass="textbox" Width="400px" />            
            <asp:HyperLink ID="hypLookUpCustomer" NavigateUrl="#" runat="server" onclick="showPromptPopUp('PromptCustomer.aspx?', this.previousSibling.previousSibling.id, 550, 900);">Look Up</asp:HyperLink>
            <div style="text-align: center; margin-top: 20px;">
                <table cellpadding="3" cellspacing="3" width="100%">
                    <thead>
                        <tr>
                            <th style="text-align: center;">Barcode</th>
                            <th style="text-align: center;">Name</th>
                            <th style="text-align: center;">Date/Time</th>
                        </tr>
                    </thead>
                    <tbody id="history">
                    </tbody>
                </table>
            </div>

        </fieldset>
    </div>

    <fieldset style="height: 450px;" id="fieldsetCheckInResult">
        <legend>Check In Result</legend>
        <table style="width: 100%;">
            <tr>
                <td style="width:33%;">Customer:<br />
                    <img id="foto" src="Photo/Customers/default.png" /></td>
                <td style="width:33%;">Pick Up Person 1:<br />
                    <img id="fotoPickUpPerson0" src="Photo/Customers/default.png" />
                    <div id="namaPickUpPerson0"></div>
                </td>
                <td style="width:33%;">Pick Up Person 2:<br />
                    <img id="fotoPickUpPerson1" src="Photo/Customers/default.png" />
                    <div id="namaPickUpPerson1"></div>
                </td>
            </tr>
        </table>

        <div style="text-align: center; width: 100%; text-wrap: normal;" id="checkinResult">
        </div>
    </fieldset>

    <div id="pleasewait" style="display: none; text-align: center;">
        Please wait
        <img src="images/wait.gif" />
    </div>

    <div id="dialog" style="display: none; text-align: center;">
        <p>
            Select Branch:
            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown" />
        </p>
    </div>


    <input type="hidden" id="hidBranchID" value="" />
    <input type="hidden" id="hidUserName" value="<%= Page.User.Identity.Name  %>" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        h1 {
            font-size: 180%;
        }

        h2 {
            font-size: 150%;
        }
    </style>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            $("fieldset").height(screen.height - 250);
        });

        var dlg = null;
        var pleasewait = null;
        var canCloseDialog = false;
        var canClosePleaseWait = false;
        $(function () {
            dlg = $("#dialog").dialog({
                draggable: true,
                resizable: false,
                show: 'Transfer',
                hide: 'explode',
                modal: true,
                width: 400,
                height: 200,
                closeOnEscape: false,
                buttons: {
                    "Select": function () {
                        var ddlBranch = $get("<%=ddlBranch.ClientID %>");
                        $get("hidBranchID").value = ddlBranch.item(ddlBranch.selectedIndex).value;
                        $get("branchName").innerHTML = ddlBranch.item(ddlBranch.selectedIndex).text;
                        canCloseDialog = true;
                        getCheckInHistory();
                        $(this).dialog("close");
                    }
                },
                dialogClose: function (event, ui) {

                },
                beforeClose: function (event, ui) {
                    return canCloseDialog;
                }
            });
            dlg.parent().appendTo(jQuery("form:first"));


            $("form").submit(formSubmit);
        });

        function formSubmit() {
            var barcode = $get("<%=txtBarcode.ClientID%>").value;
            var userName = $get("hidUserName").value;
            var branchID = $get("hidBranchID").value;

            var checkInService = new CheckInService.AjaxService();
            pleasewait = $("#pleasewait").dialog({
                draggable: false,
                resizable: false,
                modal: true,
                width: 400,
                height: 200,
                closeOnEscape: false,
                beforeClose: function (event, ui) {
                    return canClosePleaseWait;
                }
            });
            pleasewait.parent().appendTo(jQuery("form:first"));

            checkInService.DoCheckIn(branchID, barcode, userName, checkInSuccess, checkInFailed);

            $get("<%=txtBarcode.ClientID%>").value = "";

            return false;
        }

        function checkInSuccess(data) {
            var checkInService = new CheckInService.AjaxService();

            $("#checkinResult").get(0).innerHTML = "";

            var result = "";
            
            if (data.CustomerBarcode != null) {
                result = "<h1>" + data.CustomerBarcode + "<h1>" +
                         "<h1>" + data.CustomerName + "<h1>" +
                         "<h1>" + data.PackageName + "</h1>" +
                         "<h1  style='left:300px; color:" + data.CustomerStatusColor + "; background-color:" + data.CustomerStatusBackgroundColor + "'>CUSTOMER STATUS: " + data.CustomerStatus + "</h1>" +
                         "<h2 style='color:#00bb00'>AGE: " + data.Age + "</h2>";

                if (data.CustomerStatus.toUpperCase().indexOf("BILLING PROBLEM") >= 0) {                    
                    result += "<marquee><span style='font-weight:bold; font-size:22pt; color:red;'>ALERT !!!</span></marquee>";
                }
                
            }

            for (var index = 0; index < data.Messages.length; index++)
                result += "<p style='color:red;'><h1 style='color:#ff0000;'>" + data.Messages[index] + "</h1></p>";


            if (data.CustomerBarcode != null && data.CustomerName != null) {                
                result += "<p><a id='addnotes' href='#' onclick=\"showPromptPopUp('InputCustomerNotes.aspx?barcode=" + data.CustomerBarcode + "', null, 600, 900);\">Add notes for <b>" + data.CustomerBarcode + " - " + data.CustomerName + "</b></a></p>";
            }

            $("#checkinResult").append(result);




            if (data.IsPhotoExist)
                $("#foto").attr("src", "Photo/Customers/" + data.Photo + ".ashx?h=120");
            else
                $("#foto").attr("src", "Photo/Customers/default.png");

            for (index = 0; index < 2; index++) {
                $("#fotoPickUpPerson" + index).attr("src", "Photo/Persons/default.png");
                $("#namaPickUpPerson" + index).get(0).innerHTML = "";
            }

            if (data.PickUpPersons.length > 0) {
                for (index = 0; index < data.PickUpPersons.length; index++) {
                    $("#fotoPickUpPerson" + index).attr("src", "Photo/Persons/" + data.PickUpPhotos[index] + ".ashx?h=120");
                    $("#namaPickUpPerson" + index).get(0).innerHTML = data.PickUpPersons[index];
                }
            }

            canClosePleaseWait = true;
            $("#pleasewait").dialog("close");

            getCheckInHistory();

            $get("<%=txtBarcode.ClientID%>").focus();
        }

        function getCheckInHistory() {
            var checkInService = new CheckInService.AjaxService();
            var branchID = $get("hidBranchID").value;
            checkInService.GetCheckInHistory(branchID, function (historyList) {

                //$get("history").innerHTML = "";
                $("#history").empty();
                if (historyList != null) {
                    for (var index = 0; index < historyList.length; index++) {
                        var when = historyList[index].When.format("ddd, dd-MMM-yyyy HH:mm:ss");
                        var allowed = historyList[index].Allowed ? "Allowed " : "Not Allowed";
                        var row = "<tr><td>" + historyList[index].CustomerBarcode + "</td><td>" + historyList[index].CustomerName + "</td><td>" + when + "</td>" +
                                      "<td><a href='#' onclick='showPromptPopUp(\"InvoiceHistory.aspx?barcode=" + historyList[index].CustomerBarcode + "\", null, 600, 1100)'><img src='images/mail_16.png'/></a></td>" +
                                      "<td><a href='#' onclick='showPromptPopUp(\"InputCustomerNotes.aspx?barcode=" + historyList[index].CustomerBarcode + "&mode=read\", null, 600, 900)'><img src='images/NewDocumentHS.png'/></a></td>"
                        $("#history").append(row);
                    }
                }
            });
        }

        function checkInFailed(data) {
            alert("Error: " + data.message);
        }

    </script>
</asp:Content>
