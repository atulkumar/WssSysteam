<%@ Page Language="VB" EnableEventValidation="false" AutoEventWireup="false" CodeFile="Call_Detail.aspx.vb"
    Inherits="SupportCenter_CallView_Call_Detail" MaintainScrollPositionOnPostback="true"
    ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CALL DETAIL</title>
     <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="javascript" type="text/javascript" src="../../DateControl/ION.js"></script>

    <script language="javascript" type="text/javascript" src="../calendar/popcalendar.js"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js"></script>

    <%--<script src="../../Images/Js/PageLoader.js" type="text/javascript"></script>--%>
    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>

    <script language="Javascript" type="text/javascript">

        var rand_no = Math.ceil(500 * Math.random())
        var varGlobal = 1;
        function RefreshAttachment() {
            document.Form1.submit();
        }

        function ShowDateTimePopup(sender, eventArgs) {
            var picker = $find("<%= DTCallStartDate.ClientID %>");
            var userChar = eventArgs.get_keyCharacter();
            if (userChar == '@') {
                picker.showPopup();
                eventArgs.set_cancel(true);
            }
            else if (userChar == '#') {
                picker.showTimePopup();
                eventArgs.set_cancel(true);
            }
        }
        function GoToCallCheck() {
            document.Form1.txthiddenImage.value = 'GoToCall';

        }
        function GoToCall(goToCallId) {
            var CompID;
            CompID = document.getElementById('cpnlCallView_DDLCustomer').options(document.getElementById('cpnlCallView_DDLCustomer').selectedIndex).value;
            var screenid = window.parent.getActiveTabDetails();
            document.Form1.hiddenGoToCall.value = document.getElementById('cpnlCallView_txtCallNumber').value
            window.parent.OpenCallDetailInTab('Call# ' + goToCallId, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&CallNumber=" + goToCallId + "&CompId=" + CompID, 'Call' + goToCallId, screenid);
            document.Form1.txthiddenImage.value = 'GoToCall';
            //Form1.submit();
            return false;
        }
        //        function changeCurrentTabName(callNo) {
        //            window.parent.changeTabName(callNo);
        //        
        //        }

        function GoToCallKeyPress() {

            if (event.keyCode == 13) {
                var CompID;
                CompID = document.getElementById('cpnlCallView_DDLCustomer').options(document.getElementById('cpnlCallView_DDLCustomer').selectedIndex).value;
                var screenid = window.parent.getActiveTabDetails();
                document.Form1.hiddenGoToCall.value = document.getElementById('cpnlCallView_txtCallNumber').value
                window.parent.OpenCallDetailInTab('Call# ' + goToCallId, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&CallNumber=" + goToCallId + "&CompId=" + CompID, 'Call' + goToCallId, screenid);
                document.Form1.txthiddenImage.value = 'GoToCall';
                Form1.submit();
                return false;
            }

        }
        function AddCallNo(CallNo) {
            document.getElementById('cpnlCallView_txtCallRef').value = CallNo;
        }

        function OpenCalls() {
            var CompID;
            CompID = document.getElementById('cpnlCallView_DDLCustomer').options(document.getElementById('cpnlCallView_DDLCustomer').selectedIndex).value;
            wopen('GetCallsPopup.aspx?CompID=' + CompID, 'Calls' + rand_no, 720, 500);
            return false;
        }

        function HandleEndChanging(sender, args) {
            var item = args.gert_item();
            alert(sender.get_text());
        }

        function ShowUserInfo(ID) {
            var Owner = '';
            if (ID == 'txtCallBy') {
                Owner = document.getElementById('cpnlCallView_txtCallBy').value;
            }
            else if (ID == 'DDLCoordinator') {
                Owner = document.getElementById('cpnlCallView_' + ID).options(document.getElementById('cpnlCallView_' + ID).selectedIndex).value;
                //	Owner=document.getElementById('cpnlCallView_CDDLCallOwner').options(document.getElementById('cpnlCallView_CDDLCallOwner').selectedIndex).value;
            }
            else {

                var combo = $find('<%=CDDLCallOwner.ClientID %>');
                var value = combo.get_value();
                Owner = value;
            }
            if (Owner == '') {
                alert('No User Selected');
            }
            else {
                wopen('UserInfo.aspx?ScrID=334&ADDNO=' + Owner, 'Search' + rand_no, 350, 500);
            }
        }

        function CheckLength() {
            try {

                var CDLength = document.getElementById('cpnlCallView_txtdescription').value.length;
                var tdLength = document.getElementById('cpnlCallTask_TxtSubject_F').value.length;

                if (CDLength > 0) {
                    if (CDLength > 2000) {
                        alert('The Call Description cannot be more than 2000 characters\n (Current Length :' + CDLength + ')');
                        return false;
                    }
                }
                if (tdLength > 0) {
                    if (tdLength > 1000) {
                        alert('The Task Subject cannot be more than 1000 characters \n(Current Length :' + tdLength + ')');
                        return false;
                    }
                }
                var ADLength = document.getElementById('cpnlTaskAction_Txtdescription_F').value.length;
                if (ADLength > 0) {
                    if (ADLength > 2000) {
                        alert('The Action Description cannot be more than 2000 characters \n(Current Length :' + ADLength + ')');
                        return false;
                    }
                }
            }
            catch (e) {
                return true;
            }
            return true;
        }

        /**********************AJAX for Project****************************************/

        var gtype;
        var xmlHttp;
        var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0;
        var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5") != -1) ? 1 : 0;
        var is_opera = ((navigator.userAgent.indexOf("Opera6") != -1) || (navigator.userAgent.indexOf("Opera/6") != -1)) ? 1 : 0;
        //netscape, safari, mozilla behave the same??? 
        var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0;


        var gPID;
        var gAID;
        var gTID;
        var gCID;


        function ProjectChange(PID, AID, TID, CID, compID) {
            //alert(PID);
            //document.getElementById('txtHIDAgreement')='';
            gPID = PID;
            gAID = AID;
            gTID = TID;
            gCID = CID;

            xmlHttp = null;
            var ddlProject = document.getElementById(gPID);
            var projectID = ddlProject.options(ddlProject.selectedIndex).value;
            var url = '../../AJAX Server/AjaxInfo.aspx?Type=Aggreement_Task_Owner&ProjectID=' + projectID + '&Rnd=' + Math.random() + '&CompanyID=' + compID; //+'&CompID='+'<%=Session("PropCAComp")%>';
            xmlHttp = GetXmlHttpObject(stateChangeHandler);
            xmlHttp_Get(xmlHttp, url);
        }

        function CompanyChange() {
            document.Form1.txtTaskOwnerHID.value = '';
            document.Form1.txtProjectHID.value = '';
        }

        function stateChangeHandler() {
            document.getElementById(gAID).options.length = 1;
            //            document.getElementById(gTID).options.length = 1;
            document.getElementById(gCID).options.length = 1;
            var combo = $find('<%=CDDLTaskType_F.ClientID %>');
            var items = combo.get_items();
            items.clear();
            if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete') {
                // document.getElementById('imgAjax').style.display = 'none'; //src="../images/divider.gif";
                var response = xmlHttp.responseXML;
                var info = response.getElementsByTagName("INFO");

                if (info.length > 0) {
                    var vtable = response.getElementsByTagName("TABLE");
                    var intT;
                    for (intT = 0; intT < vtable.length; intT++) {
                        var item = vtable[intT].getElementsByTagName("ITEM");
                        var objForm = document.Form1;
                        var RoleDataName = '';
                        var RoleDataID = '';
                        switch (intT) {
                            case 0:
                                {

                                    for (var inti = 0; inti < item.length; inti++) {
                                        var objNewOption = document.createElement("OPTION");
                                        document.getElementById(gAID).options.add(objNewOption);
                                        objNewOption.value = item[inti].getAttribute("COL0");
                                        objNewOption.innerText = item[inti].getAttribute("COL1");
                                        RoleDataName = RoleDataName + item[inti].getAttribute("COL1") + '^';
                                        RoleDataID = RoleDataID + item[inti].getAttribute("COL0") + '^';
                                    }
                                    document.Form1.txtAgreementHID.value = RoleDataName + '~' + RoleDataID;
                                    break;
                                } //case 0
                            case 1:
                                {
                                    for (var inti = 0; inti < item.length; inti++) {
                                        var objNewOption = document.createElement("OPTION");
                                        var comboTask = $find("<%= DDLTaskOwner.ClientID %>");
                                        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                                        comboItem.set_value(item[inti].getAttribute("COL0"));
                                        comboItem.set_text(item[inti].getAttribute("COL1"));
                                        comboTask.trackChanges();
                                        comboTask.get_items().add(comboItem);
                                        comboTask.commitChanges();
                                        // document.getElementById(gTID).options.add(objNewOption);
                                        //                                        objNewOption.value = item[inti].getAttribute("COL0");
                                        //                                        objNewOption.innerText = item[inti].getAttribute("COL1");

                                        var objNewOption1 = document.createElement("OPTION");
                                        document.getElementById(gCID).options.add(objNewOption1);
                                        //                                        var comboOwner = $find("<%= DDLCoordinator.ClientID %>");
                                        //                                        var comboItem1 = new Telerik.Web.UI.RadComboBoxItem();
                                        //                                        comboItem1.set_value(item[inti].getAttribute("COL0"));
                                        //                                        comboItem1.set_text(item[inti].getAttribute("COL1"));
                                        //                                        comboOwner.trackChanges();
                                        //                                        comboOwner.get_items().add(comboItem1);
                                        //                                        comboOwner.commitChanges();
                                        objNewOption1.value = item[inti].getAttribute("COL0");
                                        objNewOption1.innerText = item[inti].getAttribute("COL1");

                                        RoleDataName = RoleDataName + item[inti].getAttribute("COL1") + '^';
                                        RoleDataID = RoleDataID + item[inti].getAttribute("COL0") + '^';
                                    }
                                    document.Form1.txtTaskOwnerHID.value = RoleDataName + '~' + RoleDataID;
                                    document.Form1.txtProjectHID.value = RoleDataName + '~' + RoleDataID;

                                    break;
                                } //case 1
                            case 2:
                                {
                                    //alert(item.length );
                                    /*if (item.length ==0)
                                    {
                                    document.getElementById('txtHIDAgreement').value='';							
                                    }*/
                                    for (var inti = 0; inti < item.length; inti++) {
                                        document.getElementById(gAID).options.value = item[inti].getAttribute("COL0");
                                        //document.getElementById('txtHIDAgreement').value=item[inti].getAttribute("COL0");
                                    }
                                    break;

                                } //case 2
                        } //switch
                    } //for loop

                } //if

            } //
            else {
                //                document.getElementById('imgAjax').src = "../../images/ajax.gif";
                //                document.getElementById('imgAjax').style.display = 'inline';

            }

        } //function


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



        /***************************************************************************/
        var globalid;
        var globalSkil;
        var globalAddNo;
        var globalGrid;
        var globaldbclick = 0;

        function ChangeHeight(txt, id) {
            //alert(event.keyCode);
            //alert(id);
            /*	if (event.keyCode==13 )
            {
            //document.Form1.submit();
            event.returnValue=true;
            }*/

            var n = document.getElementById(id).value.length;
            if (n < 30) {
                document.getElementById(id).runtimeStyle.height = 18;
                document.Form1.txtHIDSize.value = "18";
            }
            if (n > 30 && n < 60) {
                document.getElementById(id).runtimeStyle.height = 30;
                document.Form1.txtHIDSize.value = "30";
            }
            if (n > 60 && n < 90) {
                document.getElementById(id).runtimeStyle.height = 42;
                document.Form1.txtHIDSize.value = "42";
            }
            if (n > 90 && n < 120) {
                document.getElementById(id).runtimeStyle.height = 55;
                document.Form1.txtHIDSize.value = "55";
            }
            if (n > 120 && n < 150) {
                document.getElementById(id).runtimeStyle.height = 68;
                document.Form1.txtHIDSize.value = "68";
            }
            if (n > 150 && n < 180) {
                document.getElementById(id).runtimeStyle.height = 81;
                document.Form1.txtHIDSize.value = "81";
            }
            if (n > 180 && n < 210) {
                document.getElementById(id).runtimeStyle.height = 94;
                document.Form1.txtHIDSize.value = "94";
            }
            if (n > 210 && n < 240) {
                document.getElementById(id).runtimeStyle.height = 107;
                document.Form1.txtHIDSize.value = "107";
            }
            if (n > 240 && n < 270) {
                document.getElementById(id).runtimeStyle.height = 120;
                document.Form1.txtHIDSize.value = "120";
            }
            if (n > 270) {
                document.getElementById(id).runtimeStyle.height = 133;
                document.Form1.txtHIDSize.value = "133";
            }
        }


        function ChangeHeightAction(txt, id) {
            //alert(event.keyCode);
            //alert(id);
            /*	if (event.keyCode==13 )
            {
            document.Form1.submit();
            event.returnValue=false;
            }*/

            var n = document.getElementById(id).value.length;
            if (n < 60) {
                document.getElementById(id).runtimeStyle.height = 18;
                document.Form1.txtHIDSizeAction.value = "18";
            }
            if (n > 60 && n < 120) {
                document.getElementById(id).runtimeStyle.height = 30;
                document.Form1.txtHIDSizeAction.value = "30";
            }
            if (n > 120 && n < 180) {
                document.getElementById(id).runtimeStyle.height = 42;
                document.Form1.txtHIDSizeAction.value = "42";
            }
            if (n > 180 && n < 240) {
                document.getElementById(id).runtimeStyle.height = 55;
                document.Form1.txtHIDSizeAction.value = "55";
            }
            if (n > 240 && n < 300) {
                document.getElementById(id).runtimeStyle.height = 68;
                document.Form1.txtHIDSizeAction.value = "68";
            }
            if (n > 300 && n < 360) {
                document.getElementById(id).runtimeStyle.height = 81;
                document.Form1.txtHIDSizeAction.value = "81";
            }
            if (n > 360 && n < 420) {
                document.getElementById(id).runtimeStyle.height = 94;
                document.Form1.txtHIDSizeAction.value = "94";
            }
            if (n > 420 && n < 480) {
                document.getElementById(id).runtimeStyle.height = 107;
                document.Form1.txtHIDSizeAction.value = "107";
            }
            if (n > 540 && n < 600) {
                document.getElementById(id).runtimeStyle.height = 120;
                document.Form1.txtHIDSizeAction.value = "120";
            }
            if (n > 600) {
                document.getElementById(id).runtimeStyle.height = 133;
                document.Form1.txtHIDSizeAction.value = "133";
            }
        }

        function OpenW(a, b, c) {

            if ('<%=session("PropCompanyType")%>' == 'SCM') {
                wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c, 'Search' + rand_no, 500, 450);
            }
            else {
                wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 ' +
								' where UDC.Company=CI_NU8_Address_Number and ProductCode=' + a + '   and UDCType=' + "'" + b + "'" +
								' and UDC.Company=<%=session("PropCompanyID")%>' +
								' union ' +
								' select Name as ID,Description,' + "'" + "'" + ' as Company from UDC' +
								' where   ProductCode=' + a + '  and UDCType=' + "'" + b + "'" +
								' and UDC.Company=0' +
								' &tbname=' + c, 'Search' + rand_no, 500, 450);
            }
            return false;
        }
        function formReload() {
            document.Form1.submit();
        }
        function OpenComp(c) {

            wopen('../../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type = ' + "'COM'" + ' &tbname=' + c, 'Search11' + rand_no, 500, 450);
            //wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,ab.CI_VC36_Name as Name,UL_VC8_Role_PK as Role from T010011 ab,T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'Search',500,450);
            return false;
        }

        function OpenUserInfo2(ADDNO) {
            var strscreen = '334';
            wopen('UserInfo.aspx?ScrID=334&ADDNO=' + ADDNO, 'Search' + rand_no, 350, 500);
            return false;
        }

        function OpenTMPL(TempNameID, TempTypeID) {
            if (window.confirm("Do you want to select Template?") == false) {
                return false;
            }
            var calltype = ' ';
            var customer = ' ';
            var tmptype = ' ';
            var tmpname = ' ';
            var prjID;
            var DDLPRJ = document.getElementById('cpnlCallView_DDLProject');
            prjID = DDLPRJ.options(DDLPRJ.options.selectedIndex).value;
            //calltype=document.getElementById('cpnlCallView_CDDLCallType_txtHIDName').value;

            var combo = $find('<%=CDDLCallType.ClientID %>');
            var value = combo.get_value();
            calltype = value;

            calltype = calltype.replace('&', '1_a_2_b_3_c');
            customer = document.getElementById('cpnlCallView_DDLCustomer').value;
            tmptype = document.getElementById(TempTypeID).value;
            tmpname = document.getElementById(TempNameID).value;

            if (tmptype == 'All Types') {
                tmptype = '';
            }

            if (((tmptype == 'CNT') || (tmptype == 'CAO')) && tmpname != '') {
                //alert(tmptype);

                if (tmptype == 'CNT') {
                    if (confirm("Call Data will be replaced and task from template will be added to the existing task list")) {

                        document.getElementById('txtChangeTemplate').value = 1;

                        wopen1('../../AdministrationCenter/Template/TemplateSearch.aspx?txtBox=' + TempNameID + '&CALLTYPE=' + calltype + '&CUST=' + customer + '&TYPE=' + tmptype + '&ProjectID=' + prjID, 'Search' + rand_no, 910, 500);

                    }
                }
                if (tmptype == 'CAO') {
                    if (confirm("Call Data will be replaced by using Call Only Template")) {

                        document.getElementById('txtChangeTemplate').value = 1;

                        wopen1('../../AdministrationCenter/Template/TemplateSearch.aspx?txtBox=' + TempNameID + '&CALLTYPE=' + calltype + '&CUST=' + customer + '&TYPE=' + tmptype + '&ProjectID=' + prjID, 'Search' + rand_no, 910, 500);

                    }
                }


            }
            else {
                //do not confirm
                document.getElementById('txtChangeTemplate').value = 1;
                wopen1('../../AdministrationCenter/Template/TemplateSearch.aspx?txtBox=' + TempNameID + '&CALLTYPE=' + calltype + '&CUST=' + customer + '&TYPE=' + tmptype + '&ProjectID=' + prjID, 'Search' + rand_no, 910, 500);

            }

            if (tmptype == 'TAO') {
                document.getElementById('txtChangeTemplate').value = 2;
            }
            return false;
        }




        function OpenW_Add_Address(param) {
            wopen('AB_Additional.aspx?ID=' + param, 'Additional_Address' + rand_no, 400, 450);
        }

        function OpenAttach() {

            var CallStatus = document.getElementById('cpnlCallView_CDDLStatus_DDL').value;
            var CallNo = document.getElementById('cpnlCallView_txtCallNumber').value;
            var CompID = document.getElementById('cpnlCallView_DDLCustomer').value;

            //					alert(CallStatus);
            //					alert(CallNo);
            //					alert(CompID);

            //					var CS='<%=Session("PropCallStatus")%>';
            if (CallStatus == 'CLOSED') {
                alert('You cannot attach file on a Closed Call');
                return false;
            }
            wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=C&CompanyID=' + CompID + '&CallNo=' + CallNo, 'Additional_Address' + rand_no, 460, 450);
            return false;
        }


        function ShowAttachment(P) {
            var CallNo = document.getElementById('cpnlCallView_txtCallNumber').value;
            var CompID = document.getElementById('cpnlCallView_DDLCustomer').value;

            if (P == 1) {
                window.open('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CompID=' + CompID + '&CallNo=' + CallNo, 'Attachments' + rand_no, "scrollBars=yes,resizable=No,width=800,height=550,status=no  ");
            }
            else if (P == -1) {
                alert('Call Not Saved Yet');
            }
            else {
                alert('No attachment Uploaded');
            }
            return false;
        }

        function templrefresh(a, b, TempType) {
            document.getElementById('cpnlCallView_DDLTemplType').value = TempType;
            document.getElementById('cpnlCallView_TxtTmplId').value = a;
            document.getElementById('cpnlCallView_TxtTmplName').value = b;
        }

        function wopen1(url, name, w, h) {
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
            return false;
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
					'status=no, toolbar=no, scrollbars=yes, resizable=yes');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
            return false;
        }

        function OpenWUdc_Search() {
            window.open("Udc_Home_Search.aspx", "ss", "scrollBars=no,resizable=No,width=400,height=450,status=yes");
        }
        //////////////////Check
        function addToParentList(Afilename, TbName, strName) {
            if (Afilename != "" || Afilename != 'undefined') {
                varName = TbName + 'Name'
                document.getElementById(TbName).value = Afilename;
                document.getElementById(varName).value = strName;
                aa = Afilename;
            }
            else {
                document.Form1.txtAB_Type.value = aa;
            }
        }

        function addToParentCtrl(value) {
            document.getElementById('ContactInfo_txtBr').value = Value;
        }

        function ContactKey(cc) {
            document.getElementById('ClpContact_Info_txtBr').value = cc;
        }

        function callrefresh() {
            Form1.submit();
        }

        function OpenTask(vartable, CompID, CallNo) {
            var CS = document.Form1.txtCallStatus.value; //
            //alert(CS);
            //var CS='<%=Session("PropCallStatus")%>';
            if (CS == 'CLOSED') {
                alert('You cannot edit Task for a Closed Call');
            }
            else {
                wopen('Task_edit.aspx?ScrID=334&TASKNO=' + vartable + '&CompID=' + CompID + '&CallNo=' + CallNo + '&ReadOnly=0', 'Search' + rand_no, 440, 470);
            }
        }

        function ForcedPostBack() {
            document.Form1.txthiddenImage.value = "forced";
        }

        function KeyCheckTaskEdit(nn, rowvalues, tableID, CompID, CallNo, TaskNo) {

            globaldbclick = 1;
            document.Form1.txthiddenCallNo.value = nn;
            document.Form1.txthidden.value = nn;
            document.Form1.txthiddenImage.value = 'Edit';
            document.Form1.txthiddentable.value = tableID;
            var Taskstatus = document.Form1.txtTaskStatus.value;

            if (tableID == 'cpnlCallTask_dtgTask') {
                OpenTask(nn, CompID, CallNo);
            }
            else if (tableID == 'cpnlTaskAction_grdAction') {
                var TS = Taskstatus;
                if (TS == 'CLOSED') {
                    alert('You cannot edit Action for a Closed Task');
                }
                else {
                    wopen('Action_edit.aspx?ScrID=294&CallFromScrID=3&ACTIONNO=' + nn + '&CompID=' + CompID + '&CallNo=' + CallNo + '&TaskNo=' + TaskNo, 'Search' + rand_no, 350, 400);
                }
            }
            else {
                Form1.submit();
            }
        }


        function KeyCheckAction(ActionNo, GridRowID, GridName, CompID, CallNo, TaskNo) {
            globalid = GridRowID;
            globalSkil = ActionNo;
            globalGrid = GridName;

            document.Form1.txthiddenSkil.value = ActionNo;
            document.Form1.txthidden.value = ActionNo;
            document.Form1.txtrowvalues.value = GridRowID;
            document.Form1.txthiddentable.value = GridName;
            document.Form1.txtComp.value = CompID;
            document.Form1.txtCallNo.value = CallNo;
            var Taskstatus = document.Form1.txtTaskStatus.value;

            var tableID = GridName  //your datagrids id
            var table;

            if (document.all) table = document.all[tableID];
            if (document.getElementById) table = document.getElementById(tableID);
            if (table) {
                for (var i = 1; i < table.rows.length; i++) {
                    if (i % 2 == 0) {
                        table.rows[i].style.backgroundColor = "#f5f5f5";
                    }
                    else {
                        table.rows[i].style.backgroundColor = "#ffffff";
                    }
                }
                table.rows[GridRowID].style.backgroundColor = "#d4d4d4";
            }
            if (tableID == 'cpnlCallTask_dtgTask') {
                document.Form1.txthiddenImage.value = 'Select';
                setTimeout('Form1.submit();', 700); //Commented by Atul
            }
        }


        function KeyCheckTask(TaskNo, GridRowID, GridName, TaskStatus, CompID, CallNo, CallStatus) {
            globalid = GridRowID;
            globalSkil = TaskNo;
            globalGrid = GridName;

            document.Form1.txthiddenSkil.value = TaskNo;
            document.Form1.txthidden.value = TaskNo;
            document.Form1.txtrowvalues.value = GridRowID;
            document.Form1.txthiddentable.value = GridName;
            document.Form1.txtComp.value = CompID;
            document.Form1.txtTaskStatus.value = TaskStatus;
            document.Form1.txtCallNo.value = CallNo;
            document.Form1.txtCallStatus.value = CallStatus;

            var tableID = GridName  //your datagrids id
            var table;

            if (document.all) table = document.all[tableID];
            if (document.getElementById) table = document.getElementById(tableID);
            if (table) {
                for (var i = 1; i < table.rows.length; i++) {
                    if (i % 2 == 0) {
                        table.rows[i].style.backgroundColor = "#f5f5f5";
                    }
                    else {
                        table.rows[i].style.backgroundColor = "#ffffff";
                    }
                }
                table.rows[GridRowID].style.backgroundColor = "#d4d4d4";
            }
            if (tableID == 'cpnlCallTask_dtgTask') {
                document.Form1.txthiddenImage.value = 'Select';
                setTimeout('Form1.submit();', 700); //Commented by Atul
            }
        }


        function KeyCheck55(aa, bb, cc) {

            document.Form1.txthiddenImage.value = 'Edit';
            document.Form1.txthiddenSkil.value = aa;
            document.Form1.txthidden.value = bb;
            //document.Form1.txthiddenGrid.value=dd;	
            SaveEdit('Edit');

        }

        function SaveEdit(varImgValue) {
       
            if (varImgValue == 'Swap') {
                document.Form1.txthiddenImage.value = varImgValue;
                document.Form1.submit();
                return false;
            }
            if (varImgValue == 'Edit') {

                //Security Block
                var obj = document.getElementById("imgEdit")
                if (obj == null) {
                    alert("You don't have access rights to edit record");
                    return false;
                }

                if (obj.disabled == true) {
                    alert("You don't have access rights to edit record");
                    return false;
                }
                //End of Security Block

                if (document.Form1.txthidden.value == 0) {
                    alert("Please select the row");
                }
                else {
                    document.Form1.txthiddenImage.value = varImgValue;
                    document.Form1.txthiddenSkil.value = globalSkil;
                    document.Form1.txthidden.value = globalAddNo;
                    document.Form1.txthiddenGrid.value = globalGrid;
                    Form1.submit();
                    return false;
                }
            }

            if (varImgValue == 'Close') {
                window.close();
                //document.Form1.txthiddenImage.value=varImgValue;
                //Form1.submit(); 
                return false;
            }

            if (varImgValue == 'Add') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }
            if (varImgValue == 'SendMail') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }

            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }

            if (varImgValue == 'Ok') {
                //Security Block
                var obj = document.getElementById("imgSave");
                if (obj == null) {
                    alert("You don't have access rights to Save record");
                    return false;
                }

                if (obj.disabled == true) {
                    alert("You don't have access rights to Save record");
                    return false;
                }
                //End of Security Block
                var CS = document.getElementById('cpnlCallView_CDDLStatus_DDL').value; //
                //var CS1='<%=Session("PropCallStatus")%>';
                var CS1 = document.Form1.txtCallStatus.value;
                if (CS == 'CLOSED' && CS1 == 'CLOSED') {
                    alert('You cannot save a Closed Call');
                    return false;
                }
                if (CheckLength() == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;

            }

            if (varImgValue == 'Save') {
                //Security Block
                var obj = document.getElementById("imgSave");
                 if (obj == null) {
                    alert("You don't have access rights to Save record");
                    return false;
                }

                if (obj.disabled == true) {
                    alert("You don't have access rights to Save record");
                    return false;
                }
                //End of Security Block

                var CS = document.getElementById('cpnlCallView_CDDLStatus_DDL').value; //
                //var CS1='<%=Session("PropCallStatus")%>';
                var CS1 = document.Form1.txtCallStatus.value;
                
                if (CS == 'CLOSED' && CS1 == 'CLOSED') {
                    alert('You cannot save a Closed Call');
                    return false;
                }
                if (CheckLength() == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    if (CS == 'CLOSED') {
                        var agree = confirm('All the tasks related to this call will automatically closed. \nDo you want to continue?');
                        if (agree) {
                            Form1.submit();
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        Form1.submit();
                    }
                }
                return false;
            }

            if (varImgValue == 'Attach') {

                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }


            if (varImgValue == 'CloseTask') {
                if (document.Form1.txtrowvalues.value == 0) {
                    alert("Please select the row");
                }
                else {
                    var strStatus;
                    var strStatus = document.Form1.txtTaskStatus.value;
                    //strStatus='<%=session("PropTaskStatus")%>';
                    //alert(strStatus);
                    if (strStatus == 'CLOSED') {
                        alert('Task is already closed');
                    }
                    else {
                        var confirmed
                        confirmed = window.confirm("Are you sure you want to Close the selected Task ?");
                        if (confirmed == true) {
                            document.Form1.txthiddenImage.value = varImgValue;
                            //document.Form1.txtrowvaluesCall.value =0;  
                            Form1.submit();
                        }
                    }

                }
                return false;
            }

            if (varImgValue == 'Fwd') {
                var TaskNo = document.Form1.txtTask.value;
                var CallNumber = document.Form1.txtCallNo.value;
                var CompanyID = document.Form1.txtComp.value;
                var TaskStatus = document.Form1.txtTaskStatus.value;

                if (TaskNo == '0') {
                    alert("Please select the row");
                }
                else {
                    //var CS='<%=Session("PropCallStatus")%>';
                    var CS = document.Form1.txtCallStatus.value;
                    if (CS == 'CLOSED') {
                        alert('You cannot Forward a Task for a Closed Call');
                    }
                    else {
                        var TS = TaskStatus;
                        if (TS == 'CLOSED') {
                            alert('You cannot Forward a Closed Task');
                        }
                        else {

                            wopen('Task_Fwd.aspx?ScrID=340&CompID=' + CompanyID + '&CallNo=' + CallNumber + '&TASKNO=' + TaskNo, 'FWD' + rand_no, 400, 250);
                        }
                    }
                }
                return false;
            }

            if (varImgValue == 'Help') {

                var leftPos;
                var topPos;
                leftPos = (screen.width / 2) - 250;
                topPos = (screen.height / 2) - 250;
                window.open('../../Help/WSSHelp.aspx?ScreenID=3', 'posA', 'overfilter="Alpha(opacity=75);";style=ScrollingSampStyle;toolbar=no, titlebar=no,width=500,height=555,top=' + topPos + ',left=' + leftPos);
                return false;
            }


            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset()
                }
                return false;
            }
        }


        function ConfirmDelete(varImgValue) {
            if (document.Form1.txthiddentable.value == 'cpnlCallTask_dtgTask') {
                var TaskStatus = document.Form1.txtTaskStatus.value;
                var TaskNo = document.Form1.txthidden.value;
                //alert(TaskNo);
                if (TaskNo == '0' || TaskNo == '') {
                    alert("Please select the row");
                }
                else {
                    var TS = TaskStatus;
                    //	alert(TS);
                    if (TS != 'ASSIGNED') {
                        alert('Task in ' + TS + ' status cannot be deleted');
                    }
                    else {
                        var confirmed
                        confirmed = window.confirm("Are you sure you want to Delete the selected Task ?");
                        if (confirmed == false) {
                            return false;
                        }
                        else {
                            document.Form1.txthiddenImage.value = varImgValue;
                            Form1.submit();
                            return false;
                        }
                    }
                }
            }
            else {
                if (document.Form1.txthiddenSkil.value == 0) {
                    alert("Please select the row");
                }
                else {
                    var TS = TaskStatus;
                    if (TS == 'CLOSED') {
                        alert('Action cannot be deleted for a Closed Task');
                    }
                    else {
                        var confirmed
                        confirmed = window.confirm("Are you sure you want to Delete the selected Action ?");
                        if (confirmed == false) {
                            return false;
                        }
                        else {
                            document.Form1.txthiddenImage.value = varImgValue;
                            Form1.submit();
                            return false;
                        }
                    }
                }
            }
            return false;
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

        function KeyImage(a, b, c, d, CallNo, TaskNo, CompID) {

            //					alert(CallNo);
            //					alert(TaskNo);
            //					alert(CompID);

            if (d == 0) //if comment is clicked
            {

                //if ('<%=session("PropCallNumber")%>' > 0 && '<%=session("PropCAComp")%>' > 0)
                wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID=' + a + ' and UDCType=' + "'" + b + "'" + ' &tbname=' + c + '&CallNo=' + CallNo + '&TaskNo=' + TaskNo + '&CompID=' + CompID + '&ActionNo=' + a, 'Comment' + rand_no, 500, 450);

            }
            else if (d == 1) //if Attachment is clicked
            {
                wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=' + c + '&ACTIONNO=' + a + '&TaskNo=' + TaskNo + '&CompID=' + CompID + '&CallNo=' + CallNo, 'Attachment' + rand_no, 700, 240);
            }
            else // if Attach form is clicked
            {
                wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno=' + CallNo + '&tno=' + a, 'AttachForms' + rand_no, 500, 450);
            }
            return false;
        }


        function OpenVW(vartable) {
            wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ID=' + vartable, 'Search' + rand_no, 500, 450);
        }

        function FP_swapImg() {//v1.0
            var doc = document, args = arguments, elm, n; doc.$imgSwaps = new Array(); for (n = 2; n < args.length;
						n += 2) {
                elm = FP_getObjectByID(args[n]); if (elm) {
                    doc.$imgSwaps[doc.$imgSwaps.length] = elm;
                    elm.$src = elm.src; elm.src = args[n + 1];
                }
            }
        }

        function FP_preloadImgs() {//v1.0
            var d = document, a = arguments; if (!d.FP_imgs) d.FP_imgs = new Array();
            for (var i = 0; i < a.length; i++) { d.FP_imgs[i] = new Image; d.FP_imgs[i].src = a[i]; }
        }

        function FP_getObjectByID(id, o) {//v1.0
            var c, el, els, f, m, n; if (!o) o = document; if (o.getElementById) el = o.getElementById(id);
            else if (o.layers) c = o.layers; else if (o.all) el = o.all[id]; if (el) return el;
            if (o.id == id || o.name == id) return o; if (o.childNodes) c = o.childNodes; if (c)
                for (n = 0; n < c.length; n++) { el = FP_getObjectByID(id, c[n]); if (el) return el; }
            f = o.forms; if (f) for (n = 0; n < f.length; n++) {
                els = f[n].elements;
                for (m = 0; m < els.length; m++) { el = FP_getObjectByID(id, els[n]); if (el) return el; }
            }
            return null;
        }

        function HideContents() {
            parent.document.all("SideMenu1").cols = "0,*";
            document.getElementById('imgHide').style.visibility = 'hidden';
            document.getElementById('imgShow').style.visibility = 'visible';
            return false;
        }

        function ShowContents() {
            document.getElementById('imgHide').style.visibility = 'visible';
            document.getElementById('imgShow').style.visibility = 'hidden';
            parent.document.all("SideMenu1").cols = "163,*";
            return false;
        }
        //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }					
						
    </script>

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlCallTask_collapsible').cells[0].colSpan = "1";
            var z = document.getElementById('cpnlTaskAction_collapsible').cells[0].colSpan = "1";
        } 
    </script>

    <%-- <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
    </script>--%>
    <table height="1%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server"
                                                        CommandName="submit" AlternateText="." ImageUrl="~/Images/white.gif" Width="1px"
                                                        Height="1px"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelCallDetail" runat="server" CssClass="TitleLabel">CALL DETAIL</asp:Label>
                                                </td>
                                                <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" AlternateText="Click to Add Call"
                                                            ToolTip="Click to Add Call" ImageUrl="../../Images/s2Add01.gif" Visible="False">
                                                        </asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" Visible="true" ImageUrl="../../Images/s1ok02.gif"
                                                            ToolTip="Ok"></asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton ID="imgReset" AccessKey="R"
                                                                runat="server" ImageUrl="../../Images/reset_20.gif" ToolTip="Reset"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgAttachments" AccessKey="T" runat="server" ImageUrl="../../Images/ScreenHunter_075.bmp"
                                                            ToolTip="View Attachment"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgFWD" AccessKey="F" runat="server" ImageUrl="../../Images/Fwd.jpg"
                                                            ToolTip="Task Forward"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                            ToolTip="Delete"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgCloseTask" runat="server" ImageUrl="../../Images/TaskClose.gif"
                                                            ToolTip="Select a Task to Close"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="ImgMail" AccessKey="L" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                            ToolTip="PushMail"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgClose" runat="server" OnClientClick="tabClose();" ImageUrl="../../Images/s2close01.gif"
                                                            AlternateText="Close Window"></asp:ImageButton>&nbsp;
                                                        <%-- <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />--%>
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:UpdateProgress ID="progress" runat="server">
                                                        <ProgressTemplate>
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="../../Images/ajax1.gif" Width="24"
                                                                Height="24" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 10%" background="../../Images/top_nav_back.gif" height="47" nowrap="nowrap">
                                        <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2203','../../');"
                                            alt="Video Help" src="../../Images/video_help.jpg" border="0">&nbsp;
                                        <img class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('3','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;
                                        <asp:ImageButton ID="Logout" runat="server" ImageUrl="../../Images/logoff.gif" ToolTip="Logout">
                                        </asp:ImageButton>&nbsp; &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td valign="top">
                                <!-- **********************************************************************-->
                                <asp:UpdatePanel ID="upnlCallView" runat="server">
                                    <ContentTemplate>
                                        <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" Width="100%" Height="232px"
                                            BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="true" Draggable="False"
                                            CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
                                            Text="Call View" TitleBackColor="transparent" TitleClickable="true" TitleForeColor="black"
                                            PanelCSS="panel" TitleCSS="test">
                                            <table id="Table3" cellspacing="0" cellpadding="0" border="0">
                                                <tr align="left">
                                                    <td valign="top" align="left">
                                                        <table style="width: 100%; height: 100px" bordercolor="#5c5a5b" bgcolor="#f5f5f5"
                                                            border="1">
                                                            <tr>
                                                                <td bordercolor="#f5f5f5" width="4">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 131px" bordercolor="#f5f5f5" width="131">
                                                                    <asp:Label ID="lblMiddleName4" runat="server" Height="18px" Width="72px" CssClass="FieldLabel">Customer*</asp:Label><br>
                                                                    <asp:DropDownList ID="DDLCustomer" runat="server" Width="110px" Font-Size="XX-Small"
                                                                        AutoPostBack="True" Height="20px" Font-Name="Verdana">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 128px" bordercolor="#f5f5f5" width="128">
                                                                    <asp:Label ID="lblMiddleName10" runat="server" Height="18px" Width="72px" CssClass="FieldLabel">SubCategory*</asp:Label><br>
                                                                    <asp:DropDownList ID="DDLProject" runat="server" Width="110px" CssClass="txtNoFocus">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td bordercolor="#f5f5f5" align="left">
                                                                    <asp:Label ID="lblMiddleName" runat="server" Height="18px" Width="72px" CssClass="FieldLabel">Call Type*</asp:Label><br>
                                                                    <%--<uc1:CustomDDL id="CDDLCallType" runat="server" width="110px"></uc1:CustomDDL>--%>
                                                                    <telerik:RadComboBox ID="CDDLCallType" AllowCustomText="True" runat="server" Width="110px"
                                                                        Height="150px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                        DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Select Call Type"
                                                                        EnableTextSelection="true" EnableVirtualScrolling="true">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bordercolor="#f5f5f5" width="4">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 131px" bordercolor="#f5f5f5" width="131">
                                                                    <asp:Label ID="Label7" runat="server" Width="72px" Height="18px" CssClass="FieldLabel">Priority*</asp:Label><br>
                                                                    <uc1:CustomDDL ID="CDDLPriority" runat="server" Width="110px"></uc1:CustomDDL>
                                                                </td>
                                                                <td style="width: 131px" bordercolor="#f5f5f5" width="131">
                                                                    <asp:Label ID="lblMiddleName2" runat="server" Height="18px" Width="112px" CssClass="FieldLabel">Requested By*</asp:Label><br>
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="CDDLCallOwner" Font-Names="Verdana" Font-Size="7pt" runat="server"
                                                                                    Width="110px" DropDownWidth="180px" Height="150px" AppendDataBoundItems="true"
                                                                                    DataTextField="Description" DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith"
                                                                                    NoWrap="true" EmptyMessage="Select User" AllowCustomText="True" EnableTextSelection="true"
                                                                                    EnableVirtualScrolling="true">
                                                                                </telerik:RadComboBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgCallRequestedBy" Style="cursor: hand" onclick="return ShowUserInfo('CDDLCallOwner');"
                                                                                    Width="15px" ImageUrl="../../Images/user.gif" AlternateText="Click to see User Info"
                                                                                    runat="server"></asp:Image>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="height: 35px" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="lblMiddleName6" runat="server" Height="18px" Width="90px" CssClass="FieldLabel">Entered By*</asp:Label><br>
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCallByName" runat="server" Height="16px" Width="110px" CssClass="txtNoFocus"
                                                                                    BorderWidth="1px" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                    MaxLength="8" ReadOnly="true" BackColor="#E5E5E5"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgCallEnteredBy" Style="cursor: hand" onclick="return ShowUserInfo('txtCallBy');"
                                                                                    Width="15px" ImageUrl="../../Images/user.gif" AlternateText="Click to see User Info"
                                                                                    runat="server"></asp:Image>
                                                                                <input type="hidden" id="txtCallBy" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br>
                                                        <table style="width: 100%; height: 210px" bordercolor="#5c5a5b" bgcolor="#f5f5f5"
                                                            border="1">
                                                            <tr>
                                                                <td style="height: 35px" bordercolor="#f5f5f5" width="4">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 128px; height: 35px" bordercolor="#f5f5f5" width="128">
                                                                    <asp:Label ID="Label10" runat="server" Height="16px" Width="112px" CssClass="FieldLabel">Coordinator</asp:Label><br>
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="DDLCoordinator" runat="server" Width="110px" CssClass="txtnofocus">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgCoordinator" Style="cursor: hand" onclick="return ShowUserInfo('DDLCoordinator');"
                                                                                    Width="15px" ImageUrl="../../Images/user.gif" AlternateText="Click to see User Info"
                                                                                    runat="server"></asp:Image>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="width: 128px; height: 35px" bordercolor="#f5f5f5" width="128">
                                                                    <asp:Label ID="Label15" runat="server" Height="16px" Width="100px" CssClass="FieldLabel">Knowledge DB</asp:Label><br>
                                                                    <uc1:CustomDDL ID="CDDLCallCategory" runat="server" Width="110px"></uc1:CustomDDL>
                                                                </td>
                                                                <td style="height: 35px" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label11" runat="server" Height="16px" Width="112px" CssClass="FieldLabel">Related Call</asp:Label><br>
                                                                    <asp:TextBox onkeypress="NumericOnly();" ID="txtCallRef" runat="server" Height="14px"
                                                                        Width="100px" CssClass="txtNoFocus" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" MaxLength="36"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgCalls" runat="server" ImageUrl="..\..\Images\plus.gif"></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bordercolor="#f5f5f5" width="4">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 131px" bordercolor="#f5f5f5" width="131">
                                                                    <asp:Label ID="Label12" runat="server" Width="72px" Height="16px" CssClass="FieldLabel">
																									Category</asp:Label><br>
                                                                    <uc1:CustomDDL ID="CDDLCategory" runat="server" Width="110px"></uc1:CustomDDL>
                                                                </td>
                                                                <td style="width: 128px" bordercolor="#f5f5f5" width="128">
                                                                    <asp:Label ID="Label13" runat="server" Height="16px" Width="72px" CssClass="FieldLabel">
																									Cause Code</asp:Label><br>
                                                                    <uc1:CustomDDL ID="CDDLCauseCode" runat="server" Width="110px"></uc1:CustomDDL>
                                                                </td>
                                                                <td bordercolor="#f5f5f5">
                                                                    <asp:Label ID="lblMiddleName12" runat="server" Height="16px" Width="72px" CssClass="FieldLabel">Reference</asp:Label><br>
                                                                    <asp:TextBox ID="txtReference" runat="server" Height="14px" Width="109px" CssClass="txtNoFocus"
                                                                        BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small"
                                                                        MaxLength="36"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bordercolor="#f5f5f5" width="4">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 131px" bordercolor="#f5f5f5" width="131">
                                                                    <asp:Label ID="Label4" runat="server" Height="16px" Width="104px" CssClass="FieldLabel">Registered Date</asp:Label><br>
                                                                    <asp:TextBox ID="txtCallDate" runat="server" Height="14px" Width="110px" CssClass="txtNoFocus"
                                                                        BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small"
                                                                        BackColor="#e5e5e5" ReadOnly="True" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 128px" bordercolor="#f5f5f5" width="128">
                                                                    <asp:Label ID="Label5" runat="server" Height="16px" Width="96px" CssClass="FieldLabel">
																											Template Type</asp:Label><br>
                                                                    <asp:DropDownList ID="DDLTemplType" runat="server" Width="110px" CssClass="txtnofocus">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label6" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Template</asp:Label><br>
                                                                    <asp:TextBox ID="TxtTmplName" runat="server" Height="14px" Width="100px" ToolTip="Click To Select Template"
                                                                        CssClass="txtNoFocus" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" ReadOnly="False" MaxLength="36"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgTemplate" runat="server" ImageUrl="..\..\Images\plus.gif"
                                                                        AlternateText="Click To Select Template"></asp:ImageButton>
                                                                    <asp:TextBox ID="TxtTmplId" runat="server" Height="0" Width="0" CssClass="txtNoFocus"
                                                                        BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small" ReadOnly="False"
                                                                        MaxLength="36"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bordercolor="#f5f5f5" width="4">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 131px" bordercolor="#f5f5f5" width="131">
                                                                    <asp:Label ID="lblMiddleName18" runat="server" Height="16px" Width="108px" CssClass="FieldLabel">Est Close Date*</asp:Label><br>
                                                                    <ION:Customcalendar ID="dtEstFinishDate" runat="server" Width="115px" Height="16px" />
                                                                </td>
                                                                <td style="width: 128px" bordercolor="#f5f5f5" width="128">
                                                                    <asp:Label ID="lblMiddleName14" runat="server" Height="16px" Width="104px" CssClass="FieldLabel">Estimated Hours</asp:Label><br>
                                                                    <asp:TextBox ID="txtTotalEstimatedHours" runat="server" Height="14px" Width="109px"
                                                                        CssClass="txtNoFocus" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" BackColor="#E5E5E5" ReadOnly="True" MaxLength="8"></asp:TextBox>
                                                                </td>
                                                                <td bordercolor="#f5f5f5">
                                                                    <asp:Label ID="lblMiddleName16" runat="server" Height="16px" Width="96px" CssClass="FieldLabel">Reported Hours</asp:Label><br>
                                                                    <asp:TextBox ID="txtTotalReportedHours" runat="server" Height="14px" Width="109px"
                                                                        CssClass="txtNoFocus" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" BackColor="#E5E5E5" ReadOnly="True" MaxLength="8"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bordercolor="#f5f5f5" width="4">
                                                                </td>
                                                                <td style="width: 131px" bordercolor="#f5f5f5" width="131">
                                                                    <asp:Label ID="Label8" runat="server" Height="16px" Width="72px" CssClass="FieldLabel">Agreement</asp:Label><br>
                                                                    <asp:DropDownList ID="DDLAgreement" Width="110px" runat="server" CssClass="txtnofocus">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 128px" bordercolor="#f5f5f5" width="128">
                                                                    <asp:Label ID="Label16" runat="server" Height="16px" Width="100px" CssClass="FieldLabel">Category Code 1</asp:Label><br>
                                                                    <asp:TextBox ID="txtCateCode1" runat="server" Height="14px" Width="109px" CssClass="txtNoFocus"
                                                                        BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small"
                                                                        MaxLength="36"></asp:TextBox>
                                                                </td>
                                                                <td bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label17" runat="server" Height="16px" Width="100px" CssClass="FieldLabel">Category Code 2</asp:Label><br>
                                                                    <asp:TextBox ID="txtCateCode2" runat="server" Height="14px" Width="109px" CssClass="txtNoFocus"
                                                                        BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small"
                                                                        MaxLength="36"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                    <td valign="top" align="left" width="258">
                                                        <table bordercolor="#5c5a5b" bgcolor="#f5f5f5" border="1" height="100px" width="100%">
                                                            <tr>
                                                                <td bordercolor="#f5f5f5">
                                                                    <table>
                                                                        <td bordercolor="#f5f5f5" width="143">
                                                                            <asp:Label ID="Label1" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Status*</asp:Label><br>
                                                                            <uc1:CustomDDL ID="CDDLStatus" runat="server" Width="110px"></uc1:CustomDDL>
                                                                        </td>
                                                                        <td bordercolor="#f5f5f5">
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="border-width: 0px; padding-right: 0px;">
                                                                                        <asp:Label ID="Label14" runat="server" Height="12px" Width="50px" CssClass="FieldLabel">Call No.</asp:Label><br>
                                                                                        <asp:TextBox ID="txtCallNumber" runat="server" Height="14px" Width="58px" CssClass="txtNoFocus"
                                                                                            BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                            ReadOnly="false" MaxLength="10"></asp:TextBox>
                                                                                    </td>
                                                                                    <td valign="bottom">
                                                                                        <asp:Button ID="btnGoToCall" Style="cursor: hand" runat="server" CssClass="txtNoFocus"
                                                                                            Text="Go" ForeColor="WhiteSmoke" BackColor="gray" ToolTip="Click to view Call">
                                                                                        </asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td bordercolor="#f5f5f5">
                                                                            <asp:Label ID="Label9" runat="server" Height="12px" Width="52px" CssClass="FieldLabel">Comm</asp:Label><br>
                                                                            &nbsp;&nbsp;
                                                                            <asp:ImageButton ID="imgComment" Height="12" ImageUrl="../../Images/comment_Blank.gif"
                                                                                AlternateText="Comment" runat="server"></asp:ImageButton>
                                                                        </td>
                                                                        <td bordercolor="#f5f5f5">
                                                                            <asp:Label ID="Label2" runat="server" Height="12px" Width="52px" CssClass="FieldLabel">
																										Attach</asp:Label><br>
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<img class="PlusImageCSS" id="imgAddAttachment" height="12"
                                                                                onclick="OpenAttach();" alt="Add Attachment" src="../../Images/Attach15_9.gif"
                                                                                border="0" runat="server">
                                                                        </td>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bordercolor="#f5f5f5" style="padding-top: 5px; padding-bottom: 4px">
                                                                    <asp:Label ID="Label18" Height="12px" runat="server" Width="85px" CssClass="FieldLabel">Received Date</asp:Label><br />
                                                                    <telerik:RadDateTimePicker ID="DTCallStartDate" Height="14px" Width="200px" runat="server"
                                                                        DateInput-DisplayDateFormat="yyyy-MMM-dd HH:mm tt">
                                                                        <DateInput ID="DateInput1" runat="server">
                                                                            <ClientEvents OnKeyPress="ShowDateTimePopup" />
                                                                        </DateInput>
                                                                        <DatePopupButton Visible="true" />
                                                                        <Calendar DayNameFormat="FirstLetter" FirstDayOfWeek="Default" UseColumnHeadersAsSelectors="False"
                                                                            UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <TimePopupButton Visible="true" />
                                                                    </telerik:RadDateTimePicker>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <table bordercolor="#5c5a5b" bgcolor="#f5f5f5" border="1" height="210px" width="100%">
                                                            <tr>
                                                                <td bordercolor="#f5f5f5">
                                                                    <table>
                                                                        <asp:Label ID="Label3" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Subject*</asp:Label><br>
                                                                        <asp:TextBox ID="txtSubject" runat="server" Width="320px" CssClass="txtNoFocus" BorderWidth="1px"
                                                                            BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="100"></asp:TextBox>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bordercolor="#f5f5f5">
                                                                    <table>
                                                                        <asp:Label ID="lblMiddleName21" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Description*</asp:Label><br />
                                                                        <asp:TextBox ID="txtDescription" runat="server" Width="320px" Height="142px" CssClass="txtNoFocus"
                                                                            BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small"
                                                                            MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </cc1:CollapsiblePanel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:UpdatePanel ID="upnlCallTask" runat="server">
                                    <ContentTemplate>
                                        <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Width="100%" Height="66px"
                                            BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="true" Draggable="False"
                                            CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
                                            Text="Task View" TitleBackColor="transparent" TitleClickable="true" TitleForeColor="black"
                                            PanelCSS="panel" TitleCSS="test">
                                            <table style="border-collapse: collapse" width="700" border="0">
                                                <tr align="left">
                                                    <td>
                                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="pnlTask" Width="1px" runat="server" DefaultButton="submitbutton">
                                                <table style="border-collapse: collapse" width="700" cellspacing="0" cellpadding="0"
                                                    align="left" border="0">
                                                    <tr valign="TOP">
                                                        <td>
                                                            <asp:Image ID="ImgHidTask" Width="185px" Height="18px" ImageUrl="../../Images/divider.gif"
                                                                runat="server"></asp:Image>
                                                            <asp:TextBox ID="TxtStatus_F" runat="server" Height="14px" CssClass="txtNoFocusFE"
                                                                Visible="False" BorderWidth="1px" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana">ASSIGNED</asp:TextBox>
                                                            <asp:TextBox ID="TxtTaskNo_F" Height="14px" Visible="False" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Size="XX-Small" Font-Names="Verdana" runat="server" Enabled="False"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtSubject_F" runat="server" Width="224px" Height="14px" CssClass="txtNoFocusFE"
                                                                BorderWidth="1px" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana"
                                                                TabIndex="1" MaxLength="950" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <%-- <uc1:CustomDDL ID="CDDLTaskType_F" runat="server" Width="70px"></uc1:CustomDDL>--%>
                                                            <telerik:RadComboBox ID="CDDLTaskType_F" AllowCustomText="True" runat="server" Width="68px"
                                                                DropDownWidth="150px" Height="68px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Task Type"
                                                                TabIndex="2" EnableTextSelection="true" EnableVirtualScrolling="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td>
                                                            <%--<asp:DropDownList ID="DDLTaskOwner" class="DDLFieldFE" Width="81px" runat="server"
                                                                                CssClass="txtnofocus">
                                                                            </asp:DropDownList>--%>
                                                            <telerik:RadComboBox ID="DDLTaskOwner" AllowCustomText="True" runat="server" Width="77px"
                                                                DropDownWidth="150px" Height="77px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Task Owner"
                                                                TabIndex="3" EnableTextSelection="true" EnableVirtualScrolling="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="DDLDependency_F" class="DDLFieldFE" Width="39px" runat="server"
                                                                CssClass="txtnofocus">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <ION:Customcalendar ID="dtEstStartDate" runat="server" Width="91px" Height="19px" />
                                                        </td>
                                                        <td>
                                                            <ION:Customcalendar ID="dtEstCloseDate" runat="server" Width="87px" Height="19px" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtEstimatedHrs" runat="server" Width="39px" Height="14px" class="txtNoFocusFE"
                                                                TabIndex="4" BorderWidth="1px" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana"></asp:TextBox>
                                                        </td>
                                                        <td align="center">
                                                            <asp:CheckBox ID="chkMandatory" runat="server" Width="29px" Height="14px" ToolTip="Action Mandatory"
                                                                TabIndex="5" Font-Size="XX-Small" Font-Names="Verdana" AutoPostBack="False" Checked="true">
                                                            </asp:CheckBox>
                                                        </td>
                                                        <td>
                                                            <%-- <uc1:CustomDDL ID="CDDLPriority_F" runat="server" Width="49px"></uc1:CustomDDL>--%>
                                                            <telerik:RadComboBox ID="CDDLPriority_F" AllowCustomText="True" runat="server" Width="63px"
                                                                Height="63px" DropDownWidth="120px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Priority"
                                                                TabIndex="6" EnableTextSelection="true" EnableVirtualScrolling="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <span style="display: none">
                                                    <asp:Button ID="submitbutton" runat="server" CommandName="submit" Height="0" Width="0" /></span>
                                            </asp:Panel>
                                        </cc1:CollapsiblePanel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="left">
                                <asp:UpdatePanel ID="upnlTaskAction" runat="server">
                                    <ContentTemplate>
                                        <cc1:CollapsiblePanel ID="cpnlTaskAction" runat="server" Width="100%" Height="34px"
                                            BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="true" Draggable="False"
                                            CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
                                            Text="Action View" TitleBackColor="transparent" TitleClickable="true" TitleForeColor="black"
                                            PanelCSS="panel" TitleCSS="test">
                                            <table style="border-collapse: collapse" width="700" border="0">
                                                <tr align="left">
                                                    <td>
                                                        <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="PnlAction" Width="1pt" runat="server" DefaultButton="Button1">
                                                <table bordercolor="#ff0066" cellspacing="0" cellpadding="0" border="0">
                                                    <tr valign="top" bordercolor="#0066ff">
                                                        <td>
                                                            <asp:Image ID="ImgHid" Width="120px" Height="18px" ImageUrl="../../Images/divider.gif"
                                                                runat="server"></asp:Image>
                                                            <asp:TextBox ID="TxtActionNo_F" Height="0px" Width="0" CssClass="txtNoFocusFE" Visible="False"
                                                                BorderWidth="0px" BorderStyle="Solid" runat="server" Enabled="False"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TxtDescription_F" runat="server" Height="14px" Width="411px" BorderWidth="1px"
                                                                BorderStyle="Solid" CssClass="txtNoFocusFE" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ION:Customcalendar ID="dtActionDate" runat="server" Width="100px" Height="14px" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:CheckBox ID="chkMandatoryHr" Height="18px" Width="33px" Font-Size="XX-Small"
                                                                Font-Names="Verdana" ToolTip="Hours Mandatory" runat="server" Checked="true">
                                                            </asp:CheckBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtUsedHr_F" runat="server" Height="14px" Width="30px" BorderWidth="1px"
                                                                BorderStyle="Solid" CssClass="txtNoFocusFE"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <uc1:CustomDDL ID="CDDLActionOwner_F" runat="server" Width="80px" Enabled="false">
                                                            </uc1:CustomDDL>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <span style="display: none">
                                                    <asp:Button ID="Button1" runat="server" CommandName="submit" Height="0" Width="0" /></span>
                                            </asp:Panel>
                                        </cc1:CollapsiblePanel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlMsg" runat="server">
                                        </asp:Panel>
                                        <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                                        <input id="TxtPriority_FName" name="TxtPriority_FName" runat="server" type="hidden" />
                                        <input id="TxtActionOwner_F" name="TxtActionOwner_F" runat="server" type="hidden" />
                                        <input type="hidden" id="txthidden" runat="server" />
                                        <!--Address Nuimber -->
                                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenSkil">
                                        <!--Skill -->
                                        <input type="hidden" name="txthiddenImage">
                                        <!-- Image Clicked-->
                                        <input type="hidden" name="txthiddenGrid">
                                        <!-- Image Clicked-->
                                        <input id="txtChangeTemplate" type="hidden" name="txtChangeTemplate" runat="server">
                                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo">
                                        <input type="hidden" value="<%=strhiddentable%>" name="txthiddentable">
                                        <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues">
                                        <input type="hidden" value="<%=mstrCallNumber%>" name="txtTask">
                                        <input id="txtHIDIndex" type="hidden" name="txthiddenIndex" runat="server">
                                        <input type="hidden" name="txtHIDSize">
                                        <input type="hidden" name="txtHIDSizeAction">
                                        <input type="hidden" name="txtAgreementHID">
                                        <input type="hidden" name="txtTaskOwnerHID"><input type="hidden" name="txtProjectHID">
                                        <input type="hidden" id="txtComp" name="txtComp" runat="server" />
                                        <input type="hidden" id="txtTaskStatus" name="txtTaskStatus" runat="server" />
                                        <input type="hidden" id="txtCallStatus" name="txtCallStatus" runat="server" />
                                        <input type="hidden" id="txtCallNo" name="txtCallNo" runat="server" />
                                        <input type="hidden" id="hiddenGoToCall" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
