<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkspace.master" AutoEventWireup="true" CodeFile="CheckInInstructor.aspx.cs" Inherits="CheckInInstructor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <script type="text/javascript">
        (function () {
            "use strict";

            var dlg = null;
            var pleasewait = null;
            var canCloseDialog = false;
            var canClosePleaseWait = false;
            $(document).ready(function () {
                $("fieldset").height(screen.height - 250);

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
                var barcode = $get("txtInstructor").value;
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
                
                checkInService.DoInstructorCheckIn(branchID, barcode, checkInSuccess, checkInFailed);
                return false;
            }

            function checkInSuccess(data) {
                var checkInService = new CheckInService.AjaxService();

                $get("txtInstructor").value = "";

                var result = "";
                if (data.Barcode != null) {
                    result = "<h1 style='font-size:xx-large; color:#bb0000;'>" + data.Barcode + "<h1>" +
                             "<h1 style='color:#00bb00;'>" + data.Name + "<h1>" +
                             "<h1 style='color:#0000bb;'>" + data.CheckInWhen.format("ddd, dd-MMM-yyyy HH:mm:ss") + "</h1>"
                }
                $get("checkinResult").innerHTML = result;
                canClosePleaseWait = true;
                $("#pleasewait").dialog("close");

                getCheckInHistory();

            }

            function getCheckInHistory() {
                var checkInService = new CheckInService.AjaxService();
                var branchID = $get("hidBranchID").value;
                checkInService.GetInstructorCheckInHistory(branchID, function (historyList) {
                    $get("history").innerHTML = "";
                    if (historyList != null) {
                        for (var index = 0; index < historyList.length; index++) {
                            var when = historyList[index].CheckInWhen.format("ddd, dd-MMM-yyyy HH:mm:ss");
                            var barcode = historyList[index].Barcode;
                            var name = historyList[index].Name;
                            var row = "<tr><td>" + barcode + "</td><td>" + name + "</td><td>" + when + "</td></tr>" +
                            $("#history").append(row);                            
                        }
                    }
                });
            }

            function checkInFailed(data) {
                alert("Error: " + data.message);
            }
        })();
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainTitle" runat="Server">
    Instructor Attendance
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:ScriptManager ID="scmScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="AjaxService.svc" />
        </Services>
    </asp:ScriptManager>
    <fieldset>
        <legend id="branchName"></legend>
        <div id="dialog" style="display: none; text-align: center;">
            <p>
                Select Branch:
            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown" />
            </p>
        </div>
        <div style="width: 100%;">
            <div style="width: 30%; float: left; display: inline;">
                <table cellpadding="3" cellspacing="3" width="100%">
                    <thead>
                        <tr>
                            <th style="text-align: left;">Barcode</th>
                            <th style="text-align: left;">Name</th>
                            <th style="text-align: left;">Date/Time</th>
                        </tr>
                    </thead>
                    <tbody id="history">
                    </tbody>
                </table>
            </div>
            <div style="float: left;">
                <div style="text-align: center">
                    <input type="text" id="txtInstructor" class="textbox" style="font-size: 50px;" />

                    <div id="checkinResult" />
                </div>
            </div>
        </div>

        <input type="hidden" id="hidBranchID" value="" />
        <input type="hidden" id="hidUserName" value="<%= Page.User.Identity.Name  %>" />
    </fieldset>
</asp:Content>

