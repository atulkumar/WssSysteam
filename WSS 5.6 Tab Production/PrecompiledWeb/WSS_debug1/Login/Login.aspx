<%@ page language="VB" autoeventwireup="false" inherits="Login_Login, App_Web_63ldieft" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Login</title>
    <meta content="False" name="vs_snapToGrid" />
    <meta content="False" name="vs_showGrid" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>

    <script type="text/javascript" src="../images/js/ION.js"></script>

    <script type="text/javascript" src="../Images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../Images/Js/LoginShortCuts.js"></script>

    <script type="text/javascript">
        ///***********************Login AJAX**********************************////////
        var gtype;
        var xmlHttp;
        var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0;
        var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5") != -1) ? 1 : 0;
        var is_opera = ((navigator.userAgent.indexOf("Opera6") != -1) || (navigator.userAgent.indexOf("Opera/6") != -1)) ? 1 : 0;
        //netscape, safari, mozilla behave the same??? 
        var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0;

        function DisableDDL() {
            document.getElementById("ddlCompany").options.length = 1;
            document.getElementById("ddlRole").options.length = 1;
            document.getElementById('ddlCompany').disabled = true;
            document.getElementById('ddlRole').disabled = true;
            document.getElementById('lblError').innerText = '';

        }
        function ClearInfo() {
            document.getElementById("ddlCompany").options.length = 1;
            document.getElementById("ddlRole").options.length = 1;
            document.getElementById('ddlCompany').disabled = true;
            document.getElementById('ddlRole').disabled = true;
            document.getElementById('txtPassword').value = '';
        }

        function Focus(ID) {
            //	ID.select();
            //document.getElementById('lblError').innerText='';
        }
        function Encode(Data) {
            var arry = Data.split("");
            var strData = '';
            for (var i in arry) {
                strData += arry[i].charCodeAt(0) + ',';
            }
            return strData;
        }
        function GetCompany() {
            if (document.getElementById('txtPassword').value == '') {
                return;
            }
            document.getElementById('lblError').innerText = '';
            document.getElementById('txtCompName').value = '';
            document.getElementById('txtRole').value = '';
            document.getElementById('txtCompNameName').value = '';
            document.getElementById('txtRoleName').value = '';
            var strUser = document.getElementById('txtUserName').value;
            strUser = Encode(strUser);
            var pwd = document.getElementById('txtPassword').value;
            document.getElementById("ddlCompany").options.length = 1;
            document.getElementById("ddlRole").options.length = 1;
            if (strUser.length > 2 && pwd.length > 2) {
                var url = '../AJAX Server/GetLoginInfo.aspx?Type=COMP&Password=' + pwd + '&UserID=' + strUser + '&RKey=' + Math.random();
                gtype = 'COMP';
                xmlHttp = GetXmlHttpObject(stateChangeHandler);
                xmlHttp_Get(xmlHttp, url);

            }
            else {
                //Invalid user

                //document.getElementById('lblError').value='Invalid Username/or Password';
            }
        }
        function Getrole() {
            document.getElementById('lblError').innerText = '';
            document.getElementById('txtCompName').value = '';
            document.getElementById('txtRole').value = '';
            document.getElementById('txtCompNameName').value = '';
            document.getElementById('txtRoleName').value = '';

            document.getElementById("ddlRole").options.length = 1;
            var ddl = document.getElementById('ddlCompany');
            var strCompID;
            strCompID = ddl.options(ddl.options.selectedIndex).value;
            var strUserID;
            strUserID = document.getElementById('txtUserName').value;
            strUserID = Encode(strUserID);
            var url = '../AJAX Server/GetLoginInfo.aspx?Type=ROLE&UserID=' + strUserID + '&CompID=' + strCompID + '&RKey=' + Math.random();
            gtype = 'ROLE';
            xmlHttp = GetXmlHttpObject(stateChangeHandler);
            xmlHttp_Get(xmlHttp, url);
        }

        function FillHidden() {

            var ddlComp = document.getElementById('ddlCompany');
            var ddlRole = document.getElementById('ddlRole');
            document.getElementById('txtCompName').value = ddlComp.options(ddlComp.options.selectedIndex).value;
            document.getElementById('txtRole').value = ddlRole.options(ddlRole.options.selectedIndex).value;
            document.getElementById('txtCompNameName').value = ddlComp.options(ddlComp.options.selectedIndex).text;
            document.getElementById('txtRoleName').value = ddlRole.options(ddlRole.options.selectedIndex).text;

        }

        function stateChangeHandler() {
            if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete') {


                document.getElementById('imgAjax').style.display = 'none'; //src="../images/divider.gif";
                var response = xmlHttp.responseXML;
                //alert(xmlHttp.responseText);
                var info = response.getElementsByTagName("INFO");
                if (gtype == 'COMP')
                    document.getElementById("ddlCompany").options.length = 1;
                else
                    document.getElementById("ddlRole").options.length = 1;
                var item = info[0].getElementsByTagName("ITEM");
                if (info.length > 0) {
                    for (var i = 0; i < item.length; i++) {
                        //var nodeText = document.createTextNode(user[i].getAttribute("CompID"));
                        if (item[i].getAttribute("Name").length > 0 && item[i].getAttribute("ID").length > 0) {
                            var objForm = document.Form1;
                            var objNewOption = document.createElement("OPTION");
                            if (gtype == 'COMP') {
                                document.getElementById('ddlCompany').disabled = false;
                                document.getElementById('ddlCompany').focus();
                                document.getElementById("ddlCompany").options.add(objNewOption);
                            }
                            else {
                                document.getElementById('ddlRole').disabled = false;
                                document.getElementById('ddlRole').focus();
                                document.getElementById("ddlRole").options.add(objNewOption);
                            }

                            if (item[i].getAttribute("Name").length > 0)
                            { objNewOption.innerText = item[i].getAttribute("Name"); }
                            if (item[i].getAttribute("ID").length > 0)
                            { objNewOption.value = item[i].getAttribute("ID"); }
                        }
                        else {
                            if (gtype == 'COMP') {
                                document.getElementById('lblError').innerText = 'Invalid Username/or Password';
                                //alert('Invalid Username/or Password');
                            }
                            else {
                                var ddlComp = document.getElementById('ddlCompany');
                                var comp = ddlComp.options(ddlComp.options.selectedIndex).value;
                                if (comp != '') {
                                    document.getElementById('lblError').innerText = 'No Role assigned to ' + document.getElementById('txtUserName').value;
                                    //alert('No Role assigned to '+document.getElementById('txtUserName').value);
                                }
                            }
                        }
                    }
                    if (document.getElementById('ddlCompany').options.length == 2) {
                        if (gtype == 'COMP') {
                            document.getElementById('ddlCompany').options.selectedIndex = 1;
                            Getrole();
                        }
                        else {
                            if (document.getElementById('ddlRole').options.length == 2) {
                                document.getElementById("ddlRole").options.selectedIndex = 1;
                                document.getElementById("Imagebutton1").focus();
                            }
                        }
                        FillHidden();
                    }
                }

            }
            else {
                if (gtype == 'COMP') {
                    document.getElementById('ddlCompany').disabled = true;
                    document.getElementById('ddlRole').disabled = true;
                }
                else {
                    document.getElementById('ddlRole').disabled = true;
                }
                document.getElementById('imgAjax').src = "../images/ajax1.gif";
                document.getElementById('imgAjax').style.display = 'inline';
            }
        }


        function xmlHttp_Get(xmlhttp, url) {
            xmlhttp.open('GET', url, true);
            xmlhttp.send(null);

        }

        function GetXmlHttpObject(handler) {
            var objXmlHttp = null;    //Holds the local xmlHTTP object instance 
            if (is_ie) {
                var strObjName = (is_ie5) ? 'Microsoft.XMLHTTP' : 'Msxml2.XMLHTTP';
                try {
                    objXmlHttp = new ActiveXObject(strObjName);
                    objXmlHttp.onreadystatechange = handler;
                }
                catch (e) {
                    alert('IE detected, but object could not be created. Verify that active scripting and activeX controls are enabled');
                    return;
                }
            }
            else if (is_opera) {
                alert('Opera detected. The page may not behave as expected.');
                return;
            }
            else {
                objXmlHttp = new XMLHttpRequest();
                objXmlHttp.onload = handler;
                objXmlHttp.onerror = handler;
            }
            return objXmlHttp;
        }


        var gStatus;
        gStatus = 0;
        function CheckLoginStatus() {
            var strUser = document.getElementById('txtUserName').value;
            strUser = Encode(strUser);
            var pwd = document.getElementById('txtPassword').value;
            var ddlComp = document.getElementById('ddlCompany');
            var comp = ddlComp.options(ddlComp.options.selectedIndex).value;
            var ddlRole = document.getElementById('ddlRole');
            var role = ddlRole.options(ddlRole.options.selectedIndex).value;
            var url = '../AJAX Server/GetLoginInfo.aspx?Type=Status&Password=' + pwd + '&UserID=' + strUser + '&CompID=' + comp + '&Role=' + role + '&RKey=' + Math.random();
            xmlHttp = GetXmlHttpObject(CheckStatusHandler);
            xmlHttp_Get(xmlHttp, url);
            return true;
        }

        function CheckStatusHandler() {
            if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete') {
                document.getElementById('ddlCompany').disabled = false;
                document.getElementById('ddlRole').disabled = false;
                document.getElementById('txtUserName').disabled = false;
                document.getElementById('txtPassword').disabled = false;
                document.getElementById('imgAjax').style.display = 'none'; //src="../images/divider.gif";
                var response = xmlHttp.responseXML;
                //alert(xmlHttp.responseText);
                var info = response.getElementsByTagName("INFO");

                var item = info[0].getElementsByTagName("ITEM");
                if (info.length > 0) {

                    for (var i = 0; i < item.length; i++) {
                        //var nodeText = document.createTextNode(user[i].getAttribute("CompID"));
                        if (item[i].getAttribute("Name").length > 0 && item[i].getAttribute("ID").length > 0) {
                            if (item[i].getAttribute("Name") == 'OK') {
                                document.getElementById('ddlCompany').disabled = true;
                                document.getElementById('ddlRole').disabled = true;
                                document.getElementById('txtUserName').disabled = true;
                                document.getElementById('txtPassword').disabled = true;
                                //alert('OK');
                                gStatus = 1;
                                document.Form1.txthiddenImage.value = 'Submit';
                                document.Form1.submit();
                            }
                            else {
                                document.getElementById('lblError').innerText = item[i].getAttribute("Name");
                                gStatus = 0;
                            }

                        }
                        else {

                        }
                    }




                }

            }
            else {
                document.getElementById('imgAjax').src = "../images/ajax1.gif";
                document.getElementById('imgAjax').style.display = 'inline';
                document.getElementById('ddlCompany').disabled = true;
                document.getElementById('ddlRole').disabled = true;
                document.getElementById('txtUserName').disabled = true;
                document.getElementById('txtPassword').disabled = true;
            }
        }


        ///**************************Login AJAX end*********************************///////
        //this code is used to burst frames in browser window
        //this is need when we redirect from the right frame to login page to logout
        //
        if (window.location.href != window.top.location.href) {
            window.top.location.replace(window.location.href);
        }
        ///////////////////END/////////////////////////////////////////////



        function Login() {
            if (event.keyCode == 13) {
                SaveEdit('Submit');
            }
        }

        function SaveEdit(varImage) {
            if (varImage == 'Submit') {
                var ddlComp = document.getElementById('ddlCompany');
                var ddlRole = document.getElementById('ddlRole');
                var comp = ddlComp.options(ddlComp.options.selectedIndex).value;
                var role = ddlRole.options(ddlRole.options.selectedIndex).value;
                var pwd = document.getElementById('txtPassword').value;
                var uid = document.getElementById('txtUserName').value;

                if (uid == '') {
                    document.getElementById('lblError').innerText = 'Please enter username to Login';
                    document.getElementById('txtUserName').focus();
                    return false;
                }
                if (pwd == '') {
                    document.getElementById('lblError').innerText = 'Please enter password to Login';
                    document.getElementById('txtPassword').focus();
                    return false;
                }
                if (comp == '') {
                    document.getElementById('lblError').innerText = 'Please select company to Login';
                }
                else {
                    if (role == '') {
                        document.getElementById('lblError').innerText = 'Please select Role to Login';
                    }
                    else {
                        CheckLoginStatus(); //Use Ajax to find User Status
                        if (gStatus == 1) {

                            document.Form1.txthiddenImage.value = varImage;
                            document.Form1.submit();
                        }
                    }
                }


            }
            return false;
        }

        function OpenForGotPassword()
				{
				
					wopen('ForgotPassword.aspx', 'ForGotPassword',350,160);
					
				}

        function wopen(url, name, w, h) {
            w += 32;
            h += 96;
            wleft = (screen.width - w) / 2;
            wtop = (screen.height - h) / 2;
            var win = window.open(url,
					name,
					'width=' + w + ', height=' + h + ', ' +
					'left=' + wleft + ', top=' + wtop + ', ' +
					'location=no, menubar=no, ' +
					'status=no, toolbar=no, scrollbars=no, resizable=no');
            win.resizeTo(w, h);
            win.moveTo(wleft, wtop);
            win.focus();
        }

        function addToParentList(Afilename, TbName, strname) {
            if (Afilename != "" || Afilename != 'undefined') {
                varName = TbName + 'Name'
                document.getElementById(varName).value = strname;
                document.getElementById(TbName).value = Afilename;
                aa = Afilename;
            }
            else {
                document.Form1.txtAB_Type.value = aa;
            }
        }
        function Reset_onclick() {
            Form1.reset();
            document.Form1.txtUserName.focus();
            document.Form1.txtUserName.select();
            document.getElementById("ddlCompany").options.length = 1;
            document.getElementById("ddlRole").options.length = 1;
            document.getElementById('ddlCompany').disabled = true;
            document.getElementById('ddlRole').disabled = true;
            document.getElementById('lblError').innerText = '';
        }
        function Load() {
            document.Form1.txtUserName.focus();
            document.Form1.txtUserName.select();
            document.getElementById("ddlCompany").options.length = 1;
            document.getElementById("ddlRole").options.length = 1;
        }
    </script>

</head>
<body scroll="no" style="margin-left: 0px; margin-right: 0px; margin-top: 0px; margin-bottom: 0px;" onload="Load();">
    <form id="Form1" runat="server">
    <asp:PlaceHolder ID="placeholder1" runat="server">
        <table id="Table126" bordercolor="blue" height="100%" cellspacing="0" cellpadding="0"
            width="100%" border="0">
            <tr>
                <td height="30%" align="center">
                    <br />
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
            <tr valign="middle">
                <td valign="middle" align="center" height="90%">
                    <!-- **********************************************************************-->
                    <asp:Label ID="Label1" runat="server" BackColor="transparent" ForeColor="Red" Width="348px"
                        Font-Names="Verdana" Font-Size="XX-Small" Height="18px"></asp:Label>
                    &nbsp;
                    <table id="AutoNumber2" style="border-collapse: collapse" bordercolor="red" height="392"
                        cellspacing="0" cellpadding="0" width="683" background="../images/mainbackground.jpg"
                        border="0">
                        <tr>
                            <td valign="top" width="100%">
                                <font face="Verdana" size="1">&nbsp;&nbsp;&nbsp;</font>
                                <table id="AutoNumber3" style="border-collapse: collapse" bordercolor="#111111" cellspacing="0"
                                    cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td style="height: 10px" width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" style="height: 65px">
                                            <div align="center">
                                                <center>
                                                    <table id="AutoNumber4" style="border-collapse: collapse" bordercolor="#111111" cellspacing="0"
                                                        cellpadding="0" width="90%" border="0">
                                                        <tr>
                                                            <td width="15%">
                                                                <asp:Image ID="imgLogo" runat="server" Height="66" Width="76" BorderStyle="0"></asp:Image>
                                                            </td>
                                                            <td width="85%">
                                                                <table id="AutoNumber8" style="border-collapse: collapse" bordercolor="#111111" cellspacing="0"
                                                                    cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td width="100%">
                                                                            <asp:Label ID="lblPunchLineTitle" runat="server" CssClass="FieldLabel"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="100%">
                                                                            <asp:Label ID="lblPunchLineSubTitle" runat="server" Font-Size="Medium" Font-Names="Georgia"
                                                                                Width="500px" ForeColor="White" BackColor="transparent" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" height="19" style="height: 19px">
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblAccountHeading" runat="server" Font-Bold="true" BackColor="transparent"
                                                ForeColor="White" Width="615px" Font-Names="Georgia" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom" width="100%">
                                            <div align="center">
                                                <center>
                                                    <table id="AutoNumber5" style="border-collapse: collapse" bordercolor="#111111" cellspacing="0"
                                                        cellpadding="0" width="90%" border="0">
                                                        <tr>
                                                            <td width="5%" style="height: 5%">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img
                                                                    id="imgAjax" title="ajax" src="../images/divider.gif" width="24px" height="24px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="100%" style="height: 193px">
                                                                <table id="AutoNumber6" style="border-collapse: collapse" bordercolor="#111111" cellspacing="3"
                                                                    cellpadding="3" width="100%" border="0">
                                                                    <tr>
                                                                        <td align="right" width="20%">
                                                                            <b><font face="Verdana" color="#000000" size="1">User Name :&nbsp; </font></b>
                                                                        </td>
                                                                        <td width="80%" align="left">
                                                                            <asp:TextBox ID="txtUserName" TabIndex="2" runat="server" BorderStyle="Solid" Font-Size="XX-Small"
                                                                                Font-Names="Verdana" Width="123px" name="txtUserName" CssClass="txtNoFocus" Height="14px"
                                                                                BorderWidth="1px" MaxLength="36"></asp:TextBox>&nbsp;&nbsp;
                                                                            <asp:Label ID="lblError" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                Width="328px" ForeColor="Red" BackColor="transparent" Height="18px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" width="20%">
                                                                            <font face="Verdana" color="#000000" size="1"><b>Password</b></font><b><font face="Verdana"
                                                                                color="#000000" size="1"> :&nbsp; </font></b>
                                                                        </td>
                                                                        <td width="80%" align="left">
                                                                            <asp:TextBox ID="txtPassword" TabIndex="3" runat="server" BorderStyle="Solid" Font-Size="XX-Small"
                                                                                Font-Names="Verdana" Width="123px" CssClass="txtNoFocus" Height="14px" BorderWidth="1px"
                                                                                MaxLength="50" TextMode="Password"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" width="20%">
                                                                            <font face="Verdana" color="#000000" size="1"><b>Company</b></font><b><font face="Verdana"
                                                                                color="#000000" size="1"> :&nbsp; </font></b>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList Enabled="false" ID="ddlCompany" TabIndex="4" runat="server" Width="129px"
                                                                                CssClass="txtNoFocus">
                                                                            </asp:DropDownList>
                                                                            <input type="hidden" id="txtCompName" runat="server" />
                                                                            <input type="hidden" id="txtCompNameName" runat="server" />
                                                                            <%--<asp:textbox id="txtCompName" runat="server" BorderStyle="Solid" Font-Size="XX-Small" Font-Names=                 "Verdana" 	Width="0" name="txtCompName" CssClass="txtNoFocus" Height="18px" BorderWidth="1px"                 MaxLength="36" ></asp:textbox>
	<asp:textbox id="txtCompNameName" runat="server" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana" Width="0" name="txtCompNameName" CssClass="txtNoFocus" Height="18px" BorderWidth="1px" MaxLength="36" ReadOnly="true"></asp:textbox>--%>
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<!--<IMG class="PlusImageCSS" onclick="OpenComp('txtCompName');" alt="Company Name" src="../images/Plus.gif"
																						border="0">-->
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" width="20%">
                                                                            <font face="Verdana" color="#000000" size="1"><b>Role</b></font><b><font face="Verdana"
                                                                                color="#000000" size="1"> :&nbsp; </font></b>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList Enabled="false" ID="ddlRole" TabIndex="5" runat="server" Width="129px"
                                                                                CssClass="txtNoFocus">
                                                                            </asp:DropDownList>
                                                                            <input type="hidden" id="txtRole" runat="server" />
                                                                            <input type="hidden" id="txtRoleName" runat="server" />
                                                                            <%--<asp:textbox id="txtRole" runat="server" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana"
Width="0" name="txtRole" CssClass="txtNoFocus" Height="18px" BorderWidth="1px" MaxLength="36"></asp:textbox>
		<asp:textbox id="txtRoleName" runat="server" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana" Width="0" name="txtRoleName" CssClass="txtNoFocus" Height="18px" BorderWidth="1px" MaxLength="36"
ReadOnly="true"></asp:textbox>--%>
                                                                            <!--<IMG class="PlusImageCSS" onclick="OpenRole('txtRole');" alt="Role Name" src="../images/Plus.gif"
																						border="0">-->
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 20%">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="80%" align="left">
                                                                            <table style="border-color: #111111; border-collapse: collapse" id="AutoNumber7"
                                                                                cellspacing="0" cellpadding="0" border="0">
                                                                                <tr>
                                                                                    <td height="1" valign="top" align="left">
                                                                                        <asp:ImageButton ID="Imagebutton1" TabIndex="5" runat="server" Height="21" Width="64"
                                                                                            ImageUrl="../Images/bt_submit.gif"></asp:ImageButton>
                                                                                    </td>
                                                                                    <td height="1">
                                                                                        <a href="javascript:Reset_onclick()">
                                                                                            <img height="21" src="../images/bt_reset.gif" width="64" alt="" border="0" /></a>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 20%">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="80%" align="left">
                                                                            <table style="border-color: #111111; border-collapse: collapse" id="Table1" cellspacing="0"
                                                                                cellpadding="0" border="0">
                                                                                <tr>
                                                                                    <td style="display: block">
                                                                                        <asp:CheckBox ID="ChkRember" runat="server" />
                                                                                    </td>
                                                                                    <td style="display: block">
                                                                                        <asp:Label ID="Label2" runat="server" Text="Remember Me" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                            Width="100px"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                  <td style="display: block">
                                                                                      </td>
                                                                                      <td style="display: block">
                                                                                      <asp:LinkButton  ID="lbtnForgotpwd" runat="server"  Text="Forgot Password" Font-Size="XX-Small" Font-Names="Verdana"  ForeColor="Black" ></asp:LinkButton>   
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <font face="Verdana" size="1">Press Esc To Exit</font>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="bottom">
                <td height="10%" align="center">
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        <input type="hidden" name="txthiddenImage" />
    </asp:PlaceHolder>
    </form>
</body>
</html>
