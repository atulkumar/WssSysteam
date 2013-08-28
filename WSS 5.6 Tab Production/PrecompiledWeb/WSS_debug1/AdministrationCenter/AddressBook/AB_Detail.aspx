<%@ page language="VB" autoeventwireup="false" validaterequest="false" inherits="AdministrationCenter_AddressBook_AB_Detail, App_Web_owb2nwqw" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="~/SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
    <title>Employee Information</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/jquery-1[1].2.6.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/CallViewShortCuts.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $(".txtPhnNumber").keydown(function(event) {
                if (event.shiftKey) {
                    event.preventDefault();
                }

                if (event.keyCode == 46 || event.keyCode == 8) {
                }
                else {
                    if (event.keyCode < 95) {
                        if (event.keyCode < 48 || event.keyCode > 57) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if (event.keyCode < 96 || event.keyCode > 105) {
                            event.preventDefault();
                        }
                    }
                }
            });
        });
    </script>

    <script type="text/javascript">

        var rand_no = Math.ceil(500 * Math.random())

        function wopen(url, name, w, h) {
            // Fudge factors for window decoration space.
            // In my tests these work well on all platforms & browsers.
            w += 32;
            h += 96;
            wleft = (screen.width - w) / 2;
            wtop = (screen.height - h) / 2;
            var win = window.open(url,
					name,
					'width=' + w + ', height=' + h + ', ' +
					'left=' + wleft + ', top=' + wtop + ', ' +
					'location=no, menubar=no, ' +
					'status=no, toolbar=no, scrollbars=yes, resizable=no');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }
        ///To open resizable comment window.
        function wopenComment(url, name, w, h) {
            // Fudge factors for window decoration space.
            // In my tests these work well on all platforms & browsers.
            w += 32;
            h += 96;
            wleft = (screen.width - w) / 2;
            wtop = (screen.height - h) / 2;
            var win = window.open(url,
				name,
				'width=' + w + ', height=' + h + ', ' +
				'left=' + wleft + ', top=' + wtop + ', ' +
				'location=no, menubar=no, ' +
				'status=no, toolbar=no, scrollbars=yes, resizable=yes');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }

        function RefreshAttachment() {
            document.Form1.submit();
        }

        function CheckLength() {
            var TDLength = document.getElementById('cpnlCallView_txtDesc').value.length;
            if (TDLength > 0) {
                if (TDLength > 2000) {
                    alert('The Call Description cannot be more than 2000 characters\n (Current Length :' + TDLength + ')');
                    return false;
                }
            }
            return true;
        }

        function KeyImage(a, b, c, d) {
            wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=329&CallNo=' + a + ' &CompId=' + b + '&tbname=' + c, 'Comment' + rand_no, 500, 450);
            return false;
        }

        function OpenAttach(CompanyID) {
            wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=C&CompanyID=' + CompanyID, 'Additional_Address' + rand_no, 460, 450);
            return false;
        }

        function SaveEdit(varImgValue) {
            if (varImgValue == 'Edit') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }
            if (varImgValue == 'Close') {
                window.close();
            }
            if (varImgValue == 'Save') {
                if (CheckLength() == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;
            }
            if (varImgValue == 'OK') {
                if (CheckLength() == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;
            }
            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset();
                }
            }
        }        			
    </script>

    <script type="text/javascript">
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";
        }
    </script>

    <!--<script type="text/javascript">
        $(document).ready(function() {

            // Define what happens when the textbox comes under focus
            // Remove the watermark class and clear the box
            $("#txtCurrentAddress3").focus(function() {

                $(this).filter(function() {

                    // We only want this to apply if there's not
                    // something actually entered
                    return $(this).val() == "" || $(this).val() == "Type here"

                }).removeClass("watermarkOn").val("");

            });

            // Define what happens when the textbox loses focus
            // Add the watermark class and default text
            $("#txtCurrentAddress3").blur(function() {

                $(this).filter(function() {

                    // We only want this to apply if there's not
                    // something actually entered
                    return $(this).val() == ""

                }).addClass("watermarkOn").val("Type here");

            });

        });
    </script>-->

    <style type="text/css">
        .watermarkOn
        {
            color: #CCCCCC;
            font-style: italic;
        }
    </style>
    <style type="text/css">
        .container-info2
        {
            width: 100%;
            margin-left: 10px;
        }
        .container-info
        {
            width: 100%;
        }
        .form-items
        {
            width: 280px;
            float: left;
            margin-bottom: 10px;
        }
        .form-item-textarea
        {
            width: 600px;
            float: left;
            margin-bottom: 10px;
        }
        .form-items span.FieldLabel, .form-item-textarea span.FieldLabel
        {
            float: left;
            width: 140px;
            line-height: 20px;
            height: 20px;
            display: block;
        }
        .clear
        {
            clear: both;
        }
    </style>
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td width="382">
                                                    <div>
                                                        <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BorderWidth="0px"
                                                            BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button><asp:ImageButton
                                                                ID="imgbtnSearch" TabIndex="1" runat="server" Height="0px" BorderWidth="0px"
                                                                Width="0px" AlternateText="." CommandName="submit" ImageUrl="../../Images/white.gif">
                                                            </asp:ImageButton>
                                                        <asp:Label ID="lblTitleLabelTaskView" runat="server" CssClass="TitleLabel">Information</asp:Label></div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="imgSave" AccessKey="A" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                        </asp:ImageButton>&nbsp;<asp:ImageButton ID="imgRefresh" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                            ToolTip="Refresh" AlternateText="" />&nbsp;<asp:ImageButton ID="imgClose" runat="server"
                                                                OnClientClick="tabClose();" ImageUrl="../../Images/s2close01.gif" AlternateText="Close Window">
                                                            </asp:ImageButton>&nbsp;
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../../Images/top_nav_back.gif" height="47">
                                        <div style="width: 150px">
                                            <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2204','../../');"
                                                alt="Video Help" src="../../Images/video_help.jpg" border="0">&nbsp;
                                            <img class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('0','../../');"
                                                alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;<img
                                                    class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                    src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;&nbsp;</div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
                            <cc1:CollapsiblePanel ID="cpnlPerInfo" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
                                Text="Personal Information" TitleBackColor="Transparent" TitleClickable="True"
                                TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="true" BorderColor="Indigo">
                                <div style="overflow: auto; width: 100%; bordercolor: Indigo; borderstyle: Solid;">
                                    <table cellspacing="0" cellpadding="0" width="100%">
                                        <tr>
                                            <td bordercolor="#f5f5f5">
                                                <div class="container-info2">
                                                    <div class="form-items">
                                                        <asp:Label ID="lblName" runat="server" CssClass="FieldLabel">Name</asp:Label><asp:TextBox
                                                            ID="txtName" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblEmpID" runat="server" CssClass="FieldLabel">Emp ID</asp:Label>
                                                        <asp:TextBox ID="txtEmpID" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblBloodGrp" runat="server" CssClass="FieldLabel">Blood Group</asp:Label>
                                                        <%--<asp:DropDownList ID="ddlBloodGrp" runat="server" Width="200px">
                                                            </asp:DropDownList>--%>
                                                        <asp:TextBox ID="txtBloodGrp" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblDOB" runat="server" CssClass="FieldLabel">Date of Birth</asp:Label>
                                                        <%--<ION:Customcalendar ID="dtDateOfBirth" runat="server" Width="200px" Height="16px" />--%>
                                                        <asp:TextBox ID="txtDateOfBirth" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblAge" runat="server" CssClass="FieldLabel">Age</asp:Label>
                                                        <asp:TextBox ID="txtAge" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblDOJ" runat="server" CssClass="FieldLabel">Date of Joining</asp:Label>
                                                        <%--<ION:Customcalendar ID="dtDateOfJoining" runat="server" Width="200px" Height="16px" />--%>
                                                        <asp:TextBox ID="txtDateOfJoining" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblGender" runat="server" CssClass="FieldLabel">Gender</asp:Label>
                                                        <%--<asp:DropDownList ID="ddlGender" runat="server" Width="200px">
                                                            </asp:DropDownList>--%>
                                                        <asp:TextBox ID="txtGender" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblEmail" runat="server" CssClass="FieldLabel">E-Mail</asp:Label>
                                                        <asp:TextBox ID="txtEmail" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblMaritalStatus" runat="server" CssClass="FieldLabel">Marital Status</asp:Label>
                                                        <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="120px" Height="24px">
                                                            <asp:ListItem>Married</asp:ListItem>
                                                            <asp:ListItem>Single</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%-- <asp:TextBox ID="txtMaritalStatus" runat="server" Width="200px"
                                                                MaxLength="100"></asp:TextBox>  --%>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblAnniversary" runat="server" CssClass="FieldLabel">Anniversary</asp:Label>
                                                        <telerik:RadDatePicker ID="dtAnniversary" runat="server" Width="120px" Height="16px"
                                                            DateInput-Enabled="false" ToolTip="dd/mm/yyyy">
                                                        </telerik:RadDatePicker>
                                                        <%--<ION:Customcalendar ID="dtAnniversary" runat="server" Width="120px" Height="16px" />--%>
                                                        <%--<asp:TextBox ID="txtAnniversary" runat="server" Width="200px"
                                                                MaxLength="100"></asp:TextBox>--%>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblBirthPlace" runat="server" CssClass="FieldLabel">Birth Place</asp:Label>
                                                        <asp:TextBox ID="txtBirthPlace" runat="server" Width="120px" MaxLength="45"></asp:TextBox>
                                                    </div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblMobNumber" runat="server" CssClass="FieldLabel">Mobile Number</asp:Label>
                                                        <asp:TextBox ID="txtMobNumber" runat="server" Width="120px" MaxLength="13" CssClass="txtPhnNumber"></asp:TextBox></div>
                                                    <div class="form-items">
                                                        <asp:Label ID="lblQualification" runat="server" CssClass="FieldLabel">Qualification</asp:Label>
                                                        <asp:TextBox ID="txtQualification" runat="server" Width="120px" MaxLength="45"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </cc1:CollapsiblePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:CollapsiblePanel ID="DetailedInfo" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
                                Text="Detailed Information" TitleBackColor="Transparent" TitleClickable="True"
                                TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="true" BorderColor="Indigo">
                                <div style="overflow: auto; width: 100%; bordercolor: Indigo; borderstyle: Solid;">
                                    <cc1:TabContainer runat="server" ID="TabContainer1" BackColor="#F5F5F5">
                                        <cc1:TabPanel runat="server" ID="TabPanel1" HeaderText="TabPanel1" Width="100%" BackColor="#F5F5F5">
                                            <HeaderTemplate>
                                                Details
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <div class="container-info">
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblVehicleNo" runat="server" CssClass="FieldLabel">Vehicle No.</asp:Label>
                                                                            <asp:TextBox ID="txtVehicleNo" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblVehicleInsurance" runat="server" CssClass="FieldLabel">Vehicle Insurance</asp:Label>
                                                                            <%--<asp:DropDownList ID="ddlVehicleInsurance" runat="server" Width="200px" CssClass="txtNoFocus">
                                                                            </asp:DropDownList>--%>
                                                                            <asp:TextBox ID="txtVehicleInsurance" runat="server" Width="120px" MaxLength="45"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblVehicleInsuranceExpiry" runat="server" CssClass="FieldLabel">Insurance Expiry Date</asp:Label>
                                                                            <telerik:RadDatePicker ID="dtVehicleInsuranceExpiry" runat="server" Width="120px"
                                                                                Height="16px" DateInput-Enabled="false" ToolTip="dd/mm/yyyy">
                                                                            </telerik:RadDatePicker>
                                                                            <%--<ION:Customcalendar ID="dtVehicleInsuranceExpiry" runat="server" Width="120px" Height="16px"/>--%>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblVehicleType" runat="server" CssClass="FieldLabel">Vehicle Type</asp:Label><asp:TextBox
                                                                                ID="txtVehicleType" runat="server" Width="120px" MaxLength="40"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblPassportNo" runat="server" CssClass="FieldLabel">Passport No.</asp:Label>
                                                                            <asp:TextBox ID="txtPassportNo" runat="server" Width="120px" MaxLength="8"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblPassportIssueDate" runat="server" CssClass="FieldLabel">Passport Issue Date</asp:Label>
                                                                            <telerik:RadDatePicker ID="dtPassportIssueDate" runat="server" Width="120px" Height="16px"
                                                                                DateInput-Enabled="false" ToolTip="dd/mm/yyyy">
                                                                            </telerik:RadDatePicker>
                                                                            <%--<ION:Customcalendar ID="dtPassportIssueDate" runat="server" Width="120px" Height="16px"/>--%>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblPassportExpiry" runat="server" CssClass="FieldLabel">Passport Expiry Date</asp:Label>
                                                                            <telerik:RadDatePicker ID="dtPassportExpiry" runat="server" Width="120px" Height="16px"
                                                                                DateInput-Enabled="false" ToolTip="dd/mm/yyyy">
                                                                            </telerik:RadDatePicker>
                                                                            <%--<ION:Customcalendar ID="dtPassportExpiry" runat="server" Width="120px" Height="16px"/>--%>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblPassportType" runat="server" CssClass="FieldLabel">Passport Type</asp:Label>
                                                                            <%--<asp:DropDownList ID="ddlPassportType" runat="server" Width="200px">
                                                                            </asp:DropDownList>--%>
                                                                            <asp:TextBox ID="txtPassportType" runat="server" Width="120px" MaxLength="18"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblPassportIssuedBy" runat="server" CssClass="FieldLabel">Passport Issued By</asp:Label>
                                                                            <asp:TextBox ID="txtPassportIssuedBy" runat="server" Width="120px" MaxLength="15"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblPanNo" runat="server" CssClass="FieldLabel">Pan No.</asp:Label><asp:TextBox
                                                                                ID="txtPanNo" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblFoodPreference" runat="server" CssClass="FieldLabel">Food Preference</asp:Label><asp:TextBox
                                                                                ID="txtFoodPreference" runat="server" Width="120px" MaxLength="95"></asp:TextBox>
                                                                        </div>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <div class="form-item-textarea">
                                                                            <asp:Label ID="lblCurrentAddress" runat="server" CssClass="FieldLabel">Current Address</asp:Label>
                                                                            <asp:TextBox ID="txtCurrentAddress" runat="server" Width="400px" TextMode="MultiLine"
                                                                                MaxLength="400"></asp:TextBox>                                                                            
                                                                        </div>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <div class="form-item-textarea">
                                                                            <asp:Label ID="lblPermanentAddress" runat="server" CssClass="FieldLabel">Permanent Address</asp:Label>
                                                                            <asp:TextBox ID="txtPermanentAddress" runat="server" Width="400px" TextMode="MultiLine"
                                                                                MaxLength="400"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabPanel2" HeaderText="TabPanel1" Width="100%" BackColor="#F5F5F5">
                                            <HeaderTemplate>
                                                Family Details
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0" cellpadding="0" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div class="container-info">
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblRelation" runat="server" CssClass="FieldLabel">Relation</asp:Label>
                                                                            <asp:DropDownList ID="ddlRelation" runat="server" Width="120px" CssClass="txtNoFocus"
                                                                                OnSelectedIndexChanged="ddlRelation_SelectedIndexChanged" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblRelationName" runat="server" CssClass="FieldLabel">Name</asp:Label>
                                                                            <asp:TextBox ID="txtRelationName" runat="server" Width="120px" MaxLength="18"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="txtRelationDOB" runat="server" CssClass="FieldLabel">DOB</asp:Label>
                                                                            <telerik:RadDatePicker ID="dtRelationDOB" runat="server" Width="120px" Height="16px"
                                                                                DateInput-Enabled="false" ToolTip="dd/mm/yyyy">
                                                                            </telerik:RadDatePicker>
                                                                            <%--<ION:Customcalendar ID="dtRelationDOB" runat="server" Width="120px" Height="16px" />--%>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblRelationCountry" runat="server" CssClass="FieldLabel">Country</asp:Label>
                                                                            <asp:TextBox ID="txtRelationCountry" runat="server" Width="120px" MaxLength="18"></asp:TextBox>
                                                                        </div>
                                                                        <span>
                                                                            <asp:TextBox ID="TempTextBox" runat="server" Visible="false"></asp:TextBox>
                                                                        </span>
                                                                        <div>
                                                                            <asp:DataGrid runat="Server" ID="grdEmpInfo" Width="100%" BorderWidth="1px" BorderStyle="None"
                                                                                CssClass="Grid" HorizontalAlign="Center" AutoGenerateColumns="False" CellPadding="1"
                                                                                OnItemCommand="EditRecord">
                                                                                <FooterStyle CssClass="GridFooter"></FooterStyle>
                                                                                <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                                <Columns>
                                                                                    <asp:BoundColumn DataField="Relation" HeaderText="Relation" HeaderStyle-Width="23.3%">
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Name" HeaderText="Name" HeaderStyle-Width="23.3%"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="DOB" HeaderText="Date of Birth" HeaderStyle-Width="23.3%">
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Country" HeaderText="Country" HeaderStyle-Width="23.3%">
                                                                                    </asp:BoundColumn>
                                                                                    <asp:ButtonColumn HeaderText="Edit" Text="Edit" HeaderStyle-Width="6.8%"></asp:ButtonColumn>
                                                                                    <asp:BoundColumn DataField="SNo" HeaderText="Relation" Visible="false"></asp:BoundColumn>
                                                                                </Columns>
                                                                            </asp:DataGrid>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabPanel3" HeaderText="TabPanel1" Width="100%" BackColor="#F5F5F5">
                                            <HeaderTemplate>
                                                Business Details
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0" cellpadding="0" width="100%">
                                                            <tr>
                                                                <td bordercolor="#f5f5f5">
                                                                    <div class="container-info">
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblFirstName" runat="server" CssClass="FieldLabel">First Name</asp:Label>
                                                                            <asp:TextBox ID="txtFirstName" runat="server" Width="120px" MaxLength="34" Enabled="false"></asp:TextBox></div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblMiddleName" runat="server" CssClass="FieldLabel">Middle Name</asp:Label>
                                                                            <asp:TextBox ID="txtMiddleName" runat="server" Width="120px" MaxLength="34" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblLastName" runat="server" CssClass="FieldLabel">Last Name</asp:Label>
                                                                            <asp:TextBox ID="txtLastName" runat="server" Width="120px" MaxLength="34" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblOfficialPhone" runat="server" CssClass="FieldLabel">Official Phone</asp:Label>
                                                                            <asp:TextBox ID="txtOfficialPhone" runat="server" Width="120px" MaxLength="16" CssClass="txtPhnNumber"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblNationality" runat="server" CssClass="FieldLabel">Current Nationality</asp:Label>
                                                                            <asp:TextBox ID="txtNationality" runat="server" Width="120px" MaxLength="40"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblCountry" runat="server" CssClass="FieldLabel">Country of Birth</asp:Label>
                                                                            <asp:TextBox ID="txtCountry" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblESIDetail" runat="server" CssClass="FieldLabel">ESI Details</asp:Label>
                                                                            <asp:TextBox ID="txtESIDetail" runat="server" Width="120px" MaxLength="150"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblPFDetail" runat="server" CssClass="FieldLabel">PF Details</asp:Label>
                                                                            <asp:TextBox ID="txtPFDetail" runat="server" Width="120px" MaxLength="150"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="FieldLabel">Designation/Level</asp:Label>
                                                                            <asp:TextBox ID="txtDesignation" runat="server" Width="120px" MaxLength="14" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblDepartment" runat="server" CssClass="FieldLabel">Department</asp:Label>
                                                                            <asp:TextBox ID="txtDepartment" runat="server" Width="120px" MaxLength="8" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblBusinessUnit" runat="server" CssClass="FieldLabel">Business Unit</asp:Label>
                                                                            <asp:TextBox ID="txtBusinessUnit" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-items">
                                                                            <asp:Label ID="lblReportingHead" runat="server" CssClass="FieldLabel">Reporting Head</asp:Label>
                                                                            <asp:TextBox ID="txtReportingHead" runat="server" Width="120px" MaxLength="100" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <div class="form-item-textarea">
                                                                            <asp:Label ID="lblSkillSet" runat="server" CssClass="FieldLabel">Skill Set</asp:Label>
                                                                            <asp:TextBox ID="txtSkillSet" runat="server" Width="400px" TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                                                                        </div>
                                                                        <div class="clear">
                                                                        </div>
                                                                        <div class="form-item-textarea">
                                                                            <asp:Label ID="lblRole" runat="server" CssClass="FieldLabel">Role & Responsibilities</asp:Label>
                                                                            <asp:TextBox ID="txtRole" runat="server" Width="400px" TextMode="MultiLine" MaxLength="4000"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                </div>
                            </cc1:CollapsiblePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlMsg" runat="server" CssClass="pnlmsg">
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
