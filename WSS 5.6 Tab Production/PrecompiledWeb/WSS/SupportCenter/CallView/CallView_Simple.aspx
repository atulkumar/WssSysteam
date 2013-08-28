<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_CallView_Simple, App_Web_ixviuxgi" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Call View</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/CallViewShortCuts.js" type="text/javascript"></script>

    <script src="../../Images/Js/PageLoader.js" type="text/javascript"></script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">

    <script type="text/javascript">

        var gCallStatus;
        var gblnAttachment = -1;
        var rand_no = Math.ceil(500 * Math.random())
        gblnAttachment = '<%=intHIDAttach%>';


        ///***********************Call View AJAX**********************************////////
        var gtype;
        var xmlHttp;
        var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0;
        var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5") != -1) ? 1 : 0;
        var is_opera = ((navigator.userAgent.indexOf("Opera6") != -1) || (navigator.userAgent.indexOf("Opera/6") != -1)) ? 1 : 0;
        //netscape, safari, mozilla behave the same??? 
        var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0;

        function CallAjax() {
            var intCallNo = document.Form1.txthiddenCallNo.value;
            var intTaskNo = 0; //document.Form1.txtTask.value;
            var strComp = document.Form1.txtComp.value;
            var url = '../../AJAX Server/AjaxInfo.aspx?Type=FillCallViewSession&CallNo=' + intCallNo + '&Comp=' + strComp + '&RKey=' + Math.random();
            xmlHttp = GetXmlHttpObject(stateChangeHandler);
            xmlHttp_Get(xmlHttp, url);

        }



        function stateChangeHandler() {
            if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete') {
                document.getElementById('imgAjax').style.display = 'none'; //src="../images/divider.gif";
                var response = xmlHttp.responseXML;
                var info = response.getElementsByTagName("INFO");

                if (info.length > 0) {
                    var vtable = response.getElementsByTagName("TABLE");
                    var intT;
                    for (intT = 0; intT < vtable.length; intT++) {
                        var item = vtable[intT].getElementsByTagName("ITEM");
                        var objForm = document.Form1;
                        switch (intT) {
                            case 0:
                                {

                                    for (var inti = 0; inti < item.length; inti++) {

                                        gCallStatus = item[inti].getAttribute("COL0");
                                        gblnAttachment = item[inti].getAttribute("COL1");

                                        if (gblnAttachment == 0) {
                                            document.getElementById('imgAttachments').title = "No Attachment Uploaded";
                                        }
                                        else if (gblnAttachment == 1) {
                                            document.getElementById('imgAttachments').title = "View Attachment";
                                        }
                                        else {
                                            document.getElementById('imgAttachments').title = "Select a Call to View Attachment";
                                        }
                                        document.getElementById('imgEdit').title = "Edit Call";
                                        document.getElementById('imgMonitor').title = "Set Call Monitor";
                                        document.getElementById('imgTask').title = "View Task";
                                    }
                                    break;
                                } //case 0
                        } //switch
                    } //for loop
                }
            }
            else {
                document.getElementById('imgAjax').src = "../../images/ajax1.gif";
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


        ///**************************Call View AJAX end*********************************///////
        var globleID;

        function CheckLength() {
            var tdLength = document.getElementById('cpnlCallTask_TxtSubject_F').value.length;
            if (tdLength > 0) {
                if (tdLength > 1000) {
                    alert('The Task Subject cannot be more than 1000 characters\n (Current Length :' + tdLength + ')');
                    return false;
                }
            }
            return true;
        }

        function ChangeHeight(txt, id) {
            if (event.keyCode == 13) {
                document.Form1.submit();
                event.returnValue = false;
            }

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


        function KeyImage(a, b, c, d, comp, CallNo) {
            if (d == 0) //if comment is clicked
            {
                wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c, 'Comment' + rand_no, 500, 450);
            }
            else if (d == 1) //if Attachment is clicked
            {

                wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=' + c + '&ACTIONNO=' + a + '&TaskNo=' + a, 'Attachment' + rand_no, 700, 240);
            }
            else if (d == 5)//call comment
            {
                wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID=' + a + '&tbname=C&CallNo=' + CallNo + '&CompID=' + comp, 'Comment' + rand_no, 500, 450);
            }
            else if (d == 7) {

                wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo=' + a + '&CompID=' + comp + '&CallNo=' + CallNo, 'Attachment' + rand_no, 700, 240);
            }
            else // if Attach form is clicked
            {
                wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno=' + CallNo + '&tno=' + a, 'AttachForms' + rand_no, 500, 450);

            }
        }


        function KeyCheckTaskEdit(nn, rowvalues, tableID) {
            globaldbclick = 1;
            document.Form1.txthiddenTaskNo.value = nn;
            document.Form1.txthidden.value = nn;
            document.Form1.txthiddenImage.value = 'Edit';
            document.Form1.txthiddentable.value = tableID;

            //	alert(nn);
            if (tableID == 'cpnlCallTask_dtgTask') {
                OpenTask(nn);
            }
            else if (tableID == 'cpnlTaskAction_grdAction') {

                wopen('Action_edit.aspx?ScrID=294&ACTIONNO=' + nn, 'Search' + rand_no, 430, 300);
            }
            else {
                Form1.submit();
            }
        }
        function OpenW(a, b, c) {
            wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c, 'Search' + rand_no, 500, 450);
            //window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
            //popupWin = window.open('../../Search/Common/PopSearch.aspx?ID=select CUM_IN4_Address_No as ID ,CUM_VC25_First_Name as FirstName,CUM_VC25_Middle_Name as Middlename,CUM_VC25_Last_Name as LastName,CUM_VC25_Spouse_Name as SpouseName from TSL0071 where CUM_CH3_Type_id='+"'CUS'",'Search','Width=550px;Height=350px;dialogHide:true;help:no;scroll:yes');
            //window.open("AddAddress.aspx","ss","scrollBars=no,resizable=No,width=400,height=450,status=no  " );
        }
        function addToParentList(Afilename, TbName, strName) {

            if (Afilename != "" || Afilename != 'undefined') {
                varName = TbName + 'Name'
                //alert(Afilename);
                document.getElementById(TbName).value = Afilename;
                document.getElementById(varName).value = strName;
                aa = Afilename;
            }
            else {
                document.Form1.txtAB_Type.value = aa;
            }
        }
        function OpenAB(c) {
            var compType = '<%=session("propCompanyType")%>';
            var compID = '<%=session("propCompanyID")%>';
            var strQuery;
            if (compType == 'SCM') {
                strQuery = 'SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id ';
            }
            else {
                strQuery = 'SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id=' + compID + '  or um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation=' + "'SCM'" + ' ))';
            }
            wopen('../../Search/Common/PopSearch.aspx?ID=' + strQuery + '&tbname=' + c, 'Search' + rand_no, 500, 450);
        }

        function callrefresh() {

            location.href = "../../SupportCenter/CallView/Call_View.aspx?ScrID=4";
            //Form1.submit();
        }




        function SaveEdit(varImgValue) {

            if (varImgValue == 'View') {

                document.Form1.txthiddenImage.value = varImgValue;
                document.Form1.txthiddenCallNo.value = 0;
                __doPostBack("upnlGrdAddSearch", "");
                //Form1.submit(); 		

            }


            if (varImgValue == 'Tasks') {
                //	alert(document.Form1.txthiddenCallNo.value);
                if (document.Form1.txthiddenCallNo.value == 0 || document.Form1.txtComp.value == "") {
                    alert("Please select a Call");
                    return false;
                }
                else {
                    intCallNo = document.Form1.txthiddenCallNo.value;
                    strComp = document.Form1.txtComp.value;
                    wopen('Tasks.aspx?ScrID=832&intCallNo=' + intCallNo + '&strComp=' + strComp, 'ActionView' + rand_no, 870, 500);
                    return false;
                }
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

                if (document.Form1.txthiddenCallNo.value == 0 && document.Form1.txthiddenTaskNo.value == '') {
                    alert("Please select the row");
                }
                else {
                    if (document.Form1.txthiddentable.value == 'cpnlCallTask_dtgTask') {
                        var TASKNO = document.Form1.txthiddenTaskNo.value;
                        if (gCallStatus == 'CLOSED')
                        { alert('You cannot edit task for a Closed Call'); }
                        else
                        { wopen('Task_edit.aspx?ScrID=334&TASKNO=' + TASKNO, 'Search' + rand_no, 440, 470); }
                    }
                    else {
                        document.Form1.txthiddenImage.value = varImgValue;
                        var CallNo = document.Form1.txthiddenCallNo.value;
                        var Comp = document.Form1.txtComp.value;
                        var screenid = window.parent.getActiveTabDetails();
                        window.parent.OpenCallDetailInTab('Call# ' + CallNo, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=5&CallNumber=" + CallNo + "&CompId=" + Comp, 'Call' + CallNo,screenid);

                        //   Form1.submit();
                    }

                }

                return false;
            }

            if (varImgValue == 'Close') {
                window.close();
            }

            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }

            if (varImgValue == 'Add') {
                //location.href="Call_Detail.aspx?ScrID=3&ID=-1";
                document.Form1.txthiddenImage.value = varImgValue;
                window.parent.OpenTabOnAddClick('Call Entry', "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=-1&PageID=5", "3");

                // Form1.submit();
                return false;
            }
            if (varImgValue == 'Search') {
                document.Form1.txthiddenImage.value = varImgValue;
                //Form1.submit(); 
                __doPostBack("upnlGrdAddSearch", "");
                return false;
            }
            if (varImgValue == 'Monitor') {
                var CallStatus = gCallStatus;

                if (CallStatus == 'CLOSED') {
                    alert('You cannot set Monitor on CLOSED call');
                }
                else {
                    //CallNo=globleID;
                    CallNo = document.Form1.txthidden.value;
                    if (CallNo == 0) {
                        alert('Please select the call first.');
                        //CallNo = '<%=Session("PropCallNumber")%>';
                    }
                    //alert(CallNo);
                    if ((CallNo == '') || (CallNo == '0')) {
                        alert('Please select the call first.');
                        return false;
                    }
                    else {
                        TaskNo = document.Form1.txthidden.value;
                        wopen('../../CommunicationSetup/CommunicationSetupOnCall.aspx?CallNo=' + CallNo + '&Comp=' + document.Form1.txtComp.value, 'Attachment' + rand_no, 800, 490);
                        return false;
                    }
                }
                return false;
            }

            if (varImgValue == 'Select') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }

            if (varImgValue == 'Save') {

                //Security Block
                var obj = document.getElementById("imgtask")
                if (obj == null) {
                    alert("You don't have access rights to Save record");
                    return false;
                }

                if (obj.disabled == true) {
                    alert("You don't have access rights to Save record");
                    return false;
                }
                //End of Security Block
                if (CheckLength() == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;
            }

            if (varImgValue == 'CloseCall') {
                document.Form1.txthiddenImage.value = varImgValue;

                document.Form1.txtrowvaluescall.value = 0;
                __doPostBack("upnlGrdAddSearch", "");
                //Form1.submit(); 
                return false;
            }
            if (varImgValue == 'MyCall') {
                document.Form1.txthiddenImage.value = varImgValue;
                //Form1.submit(); 
                __doPostBack("upnlGrdAddSearch", "");
                return false;
            }

            if (varImgValue == 'Attach') {
                if (document.Form1.txthiddenCallNo.value == 0 && document.Form1.txthiddenTaskNo.value == '') {
                    alert("Please select the row");
                    return false;
                }
                else {
                    if (gblnAttachment == 1) {
                        wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CompID=' + document.Form1.txtComp.value + '&CallNo=' + document.Form1.txthiddenCallNo.value, 'Attachments' + rand_no, 800, 550);
                    }
                    else {
                        alert('No Attachment Uploaded');
                    }
                    return false;
                }
            }

            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset();

                }

            }
        }


        function callrefresh() {

            //Form1.submit();
            __doPostBack("upnlGrdAddSearch", "");
            document.Form1.txtComp.value = document.Form1.txtComp.value;
        }

        function KeyCheck(nn, rowvalues, rowvaluescall, tableID, Comp, SuppComp) {

            globleID = nn;



            document.Form1.txthidden.value = nn;
            document.Form1.txthiddentable.value = tableID;
            document.Form1.txtrowvalues.value = rowvalues;
            document.Form1.txtrowvaluescall.value = rowvaluescall;
            //alert(rowvaluescall);
            if (tableID == 'cpnlCallView_GrdAddSerach') {
                document.Form1.txtComp.value = Comp;
                document.Form1.txthiddenCallNo.value = nn;
            }
            else if (tableID == 'cpnlCallTask_dtgTask') {
                document.Form1.txthiddenTaskNo.value = nn;
            }
            //Form1.submit();

            //var tableID='cpnlCallView_GrdAddSerach'  //your datagrids id
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
                table.rows[rowvalues].style.backgroundColor = "#d4d4d4";
            }


            if (tableID == 'cpnlCallView_GrdAddSerach') {
                //	alert(SuppComp);
                document.Form1.txthiddensuppcomp.value = SuppComp;
                document.Form1.txthiddenImage.value = 'Select';
                CallAjax();
                //setTimeout('Form1.submit();',100);
                return false;
                //Form1.submit(); 
            }

        }

        function KeyCheck555(CallNo, rowvalues, tableID, Comp, CallOwner, TaskOwnerID) {




            //alert("asdf");
            var strscreen;
            strscreen = '463'

            document.Form1.txthiddenCallNo.value = CallNo;
            document.Form1.txthidden.value = CallNo;
            document.Form1.txthiddenImage.value = '';
            document.Form1.txthiddentable.value = tableID;
            document.Form1.txtComp.value = Comp;
            document.Form1.txtByWhom.value = CallOwner;

            if (tableID == 'cpnlCallTask_dtgTask') {
                //	alert();
                //OpenUserInfo(CallNo,CallOwner,Comp,strscreen);
                wopen('UserInfo.aspx?ScrID=334&ADDNO=' + TaskOwnerID + '&CALLOWNER=' + CallOwner, 'Search' + rand_no, 350, 500);
                //	OpenUserInfo(TaskOwnerID);
            }
            else {
                //alert();
                wopen('UserInfo.aspx?ScrID=334&CALLNO=' + CallNo + '&CALLOWNER=' + CallOwner + '&COMP=' + Comp + '&ScreenID=' + strscreen, 'Search' + rand_no, 350, 500);
                //	OpenUserInfo(CallNo,CallOwner,Comp,strscreen);
                //Form1.submit(); 
                return false;
            }


        }


        function OpenUserInfo(vartable, CallOwner, Comp, strscreen) {
            wopen('UserInfo.aspx?ScrID=334&CALLNO=' + vartable + '&CALLOWNER=' + CallOwner + '&COMP=' + Comp + '&ScreenID=' + strscreen, 'Search' + rand_no, 350, 500);
        }

        function KeyCheck55(nn, rowvalues, tableID, Comp) {
            var screenid = window.parent.getActiveTabDetails();
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

            document.Form1.txthidden.value = nn;
            document.Form1.txthiddenImage.value = 'Edit';
            document.Form1.txthiddentable.value = tableID;
            document.Form1.txtComp.value = Comp;

            if (tableID == 'cpnlCallTask_dtgTask') {
                document.Form1.txthiddenTaskNo.value = nn;
                OpenTask(nn);
            }
            else {
                document.Form1.txthiddenCallNo.value = nn;
                window.parent.OpenCallDetailInTab('Call# ' + nn, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=5&CallNumber=" + nn + "&CompId=" + Comp, 'Call' + nn, screenid);
                //Form1.submit();
                return false;
            }


        }

        function OpenTask(vartable) {
            //alert(vartable);
            //window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
            var CS = gCallStatus;
            if (CS == 'CLOSED') {
                alert('Task cannot be edited for a Closed Call');
            }
            else {
                wopen('Task_edit.aspx?ScrID=334&TASKNO=' + vartable, 'Search' + rand_no, 440, 470);
            }
        }

        function OpenVW(vartable) {
            //alert(vartable);
            //window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
            wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=799&TBLName=' + vartable, 'Search' + rand_no, 450, 500);
            //	wopen('AB_ViewColumns.aspx','UserView',500,450);
            return false;
        }


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
				
    </script>

    <script type="text/javascript">
        //A Function to call on Page Load to set grid width according to screen size
        function onLoad() {
            var divCallView = document.getElementById('divCallView');
            if (divCallView != null) {
                if (document.body.clientWidth > 0) {
                    divCallView.style.width = document.body.clientWidth - 30 + "px";
                }
            }
        }
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";
        }
        //A Function is Called when we resize window
        window.onresize = onLoad;     
    </script>

    <script type="text/javascript">
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        prm.add_pageLoaded(onLoad);
    </script>

    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td background="../../Images/top_nav_back.gif" height="47">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                            BackColor="#9FBEEB"></asp:Button><asp:ImageButton ID="imgbtnSearch" TabIndex="1"
                                                runat="server" Width="1px" Height="1px" ImageUrl="white.GIF" CommandName="submit"
                                                AlternateText="."></asp:ImageButton>
                                        <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel">CALL VIEW</asp:Label>
                                    </td>
                                    <td style="width: 55%; text-align: center;" nowrap="nowrap">
                                        <center>
                                            <asp:UpdatePanel ID="upnltop" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../../Images/s2Add01.gif"
                                                        AlternateText="New Call"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                        AlternateText="Select a Call to Edit"></asp:ImageButton>&nbsp;<asp:ImageButton ID="imgSearch"
                                                            AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif" AlternateText="Search">
                                                        </asp:ImageButton>&nbsp;<asp:ImageButton ID="imgAttachments" AccessKey="A" runat="server"
                                                            ImageUrl="../../Images/ScreenHunter_075.bmp" AlternateText="Select a Call to View Attachment">
                                                        </asp:ImageButton>&nbsp;<asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server"
                                                            ImageUrl="../../Images/CloseCall1.gif" AlternateText="Close Call"></asp:ImageButton>&nbsp;<asp:ImageButton
                                                                ID="imgMyCall" AccessKey="M" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                                ToolTip="My Calls"></asp:ImageButton>&nbsp;<asp:ImageButton ID="imgMonitor" AccessKey="M"
                                                                    runat="server" ImageUrl="../../Images/callmonitor.jpg" AlternateText="Select a Call to set Call Monitor">
                                                                </asp:ImageButton>&nbsp;<asp:ImageButton ID="imgTask" AccessKey="T" runat="server"
                                                                    ImageUrl="../../Images/torch2.gif" AlternateText="Select a Call to View Task">
                                                    </asp:ImageButton>&nbsp;
                                                    <img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;"
                                                        onclick="javascript:location.reload(true);" />&nbsp;
                                                        <asp:ImageButton ID="imgClose" runat="server" OnClientClick="tabClose();"
                                                        ImageUrl="../../Images/s2close01.gif" AlternateText="Close Window">
                                                    </asp:ImageButton>&nbsp;
                                                 <%--   <img src="../../Images/s2close01.gif" title="Close" alt="" onclick=""
                                                        />--%>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </center>
                                    </td>
                                    <td style="width: 40px" height="40">
                                        <img id="imgAjax" title="ajax" height="30" src="../../images/divider.gif" width="24"
                                            visible="false" />
                                    </td>
                                    <td style="width: 40px">
                                        <asp:UpdateProgress ID="progress" runat="server">
                                            <ProgressTemplate>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="../../Images/ajax1.gif" Width="24"
                                                    Height="24" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </td>
                                    <td style="width: 15%">
                                        <span><font face="Verdana" size="1"><strong>View&nbsp;
                                            <asp:DropDownList ID="ddlstview" runat="server" Width="80px" AutoPostBack="false"
                                                Font-Size="XX-Small" Font-Names="Verdana">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="imgBtnViewPopup" runat="server" ImageUrl="../../Images/plus.gif">
                                            </asp:ImageButton></strong></font></span>
                                    </td>
                                    <td style="width: 10%;" nowrap="nowrap">
                                    <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2206','../../');"
                                            alt="Video Help" src="../../Images/video_help.jpg" border="0">&nbsp;
                                        <img class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('845','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="100%">
                                            <asp:UpdatePanel ID="upnlGrdAddSearch" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" Width="100%" BorderWidth="0px"
                                                        BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                        ExpandImage="../../Images/ToggleDown.gif" Text="Call View" TitleBackColor="transparent"
                                                        TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                        Visible="true" BorderColor="Indigo">
                                                        <div id="divCallView" style="overflow: auto; width: 1056px; height: 470px">
                                                            <table id="table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                width="100%" align="left" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <table cellspacing="0" cellpadding="0" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Panel ID="pnl" runat="server">
                                                                                        <table cellspacing="0" cellpadding="0" border="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="CHKC" runat="server" Width="20px" ToolTip="Comment Search" Font-Size="XX-Small"
                                                                                                        BorderWidth="0"></asp:CheckBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="CHKA" runat="server" Width="20px" ToolTip="Attachment Search" Font-Size="XX-Small"
                                                                                                        BorderWidth="0"></asp:CheckBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                                <td nowrap="nowrap">
                                                                                    <asp:Panel ID="Panel1" runat="server">
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="left">
                                                                        <!--  **********************************************************************-->
                                                                        <asp:DataGrid ID="GrdAddSerach" runat="server" CssClass="Grid" Font-Names="Verdana"
                                                                            BorderColor="Silver" BorderStyle="None" BorderWidth="1px" ForeColor="MidnightBlue"
                                                                            AllowSorting="true" DataKeyField="CallNo" PageSize="20" HorizontalAlign="Left"
                                                                            GridLines="Horizontal" CellPadding="0" PagerStyle-Visible="False" AllowPaging="true">
                                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                            <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                                            </ItemStyle>
                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="true" ForeColor="Black" BorderColor="White"
                                                                                BackColor="#E0E0E0"></HeaderStyle>
                                                                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="C">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgComm" runat="server" ImageUrl="../../Images/comment_Blank.gif">
                                                                                        </asp:Image>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="A">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgAtt" runat="server"></asp:Image>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <PagerStyle Visible="False" Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                        </asp:DataGrid>
                                                                        <!-- ***********************************************************************-->
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="height: 20">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="pg" Height="12pt" Width="40px" Font-Names="Verdana" Font-Size="8pt"
                                                                            ForeColor="#0000C0" runat="server" Font-Bold="true">Page</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="CurrentPg" runat="server" Height="12px" Width="10px" Font-Size="X-Small"
                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="off" Font-Names="Verdana" Font-Size="8pt" ForeColor="#0000C0" runat="server"
                                                                            Font-Bold="true">of</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="TotalPages" runat="server" Height="12px" Width="10px" Font-Size="X-Small"
                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Firstbutton" runat="server" AlternateText="First" ImageUrl="../../Images/next9.jpg"
                                                                            ToolTip="First"></asp:ImageButton>
                                                                    </td>
                                                                    <td width="14">
                                                                        <asp:ImageButton ID="Prevbutton" runat="server" ImageUrl="../../Images/next99.jpg"
                                                                            ToolTip="Previous"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Nextbutton" runat="server" ImageUrl="../../Images/next9999.jpg"
                                                                            ToolTip="Next"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Lastbutton" runat="server" ImageUrl="../../Images/next999.jpg"
                                                                            ToolTip="Last"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPageSize" runat="server" Height="14px" Width="24px" Font-Size="7pt"
                                                                            MaxLength="3"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="Button3" runat="server" Height="12pt" Width="16px" ToolTip="Change Paging Size"
                                                                            Font-Size="7pt" Text=">" BorderStyle="None" ForeColor="Navy" Font-Bold="true">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblrecords" runat="server" Height="12pt" Font-Names="Verdana" Font-Size="8pt"
                                                                            ForeColor="MediumBlue" Font-Bold="true">Total Records</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="TotalRecods" runat="server" Height="12pt" Font-Names="Verdana" Font-Size="8pt"
                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="722px" Font-Names="Verdana" Font-Size="XX-Small"
                            BorderStyle="Groove" BorderWidth="0" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues" />
                        <input type="hidden" value="<%=introwvalues%>" name="txtrowvaluescall" />
                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                        <input type="hidden" name="txthiddenTaskNo" />
                        <input type="hidden" value="<%=strhiddentable%>" name="txthiddentable" />
                        <input type="hidden" value="<%=mstrComp%>" name="txtComp" />
                        <input type="hidden" name="txtByWhom" />
                        <input type="hidden" name="txtHIDSize" />
                        <input type="hidden" value="<%=mstrsuppcomp%>" name="txthiddensuppcomp" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
