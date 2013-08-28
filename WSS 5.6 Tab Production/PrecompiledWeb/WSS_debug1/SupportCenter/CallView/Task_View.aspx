<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_Task_View, App_Web_brwmhxgd" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Task View</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/TaskViewShortCuts.js" type="text/javascript"></script>

    <%-- <script src="../../Images/Js/PageLoader.js" type="text/javascript"></script>--%>

    <script type="text/javascript">
        //A Function to call on Page Load to set grid width according to screen size
        function onLoad() {
            var divCallView = document.getElementById('divCallView');
           if(document.body.clientWidth !=0)
           {
            divCallView.style.width = document.body.clientWidth - 30 + "px";
            }
        }
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlTaskView_collapsible').cells[0].colSpan = "1";

        }
        //A Function is Called when we resize window
        window.onresize = onLoad;   
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
//        prm.add_initializeRequest(InitializeRequest);
//        prm.add_endRequest(EndRequest);
        prm.add_pageLoaded(onLoad);     
    </script>

    <script type="text/javascript">
        var gTaskStatus;   //Global variable for task status which stores the task status returned by AJAX
        var gblnAttachment = -1;
        gblnAttachment = '<%=intHIDAttach%>';

        ///***********************Task View AJAX**********************************////////
        var gtype;
        var xmlHttp;
        var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0;
        var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5") != -1) ? 1 : 0;
        var is_opera = ((navigator.userAgent.indexOf("Opera6") != -1) || (navigator.userAgent.indexOf("Opera/6") != -1)) ? 1 : 0;
        //netscape, safari, mozilla behave the same??? 
        var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0;

        function CallAjax() {

            var intCallNo = document.Form1.txthiddenCallNo.value;
            var intTaskNo = document.Form1.txtTask.value;
            var strComp = document.Form1.txtComp.value;
            var url = '../../AJAX Server/AjaxInfo.aspx?Type=FillSession&CallNo=' + intCallNo + '&TaskNo=' + intTaskNo + '&Comp=' + strComp + '&RKey=' + Math.random();
            xmlHttp = GetXmlHttpObject(stateChangeHandler);
            xmlHttp_Get(xmlHttp, url);

        }

        function stateChangeHandler() {
            if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete') {
                document.getElementById('imgAjax').style.display = 'none'; //src="../images/divider.gif";						
                var response = xmlHttp.responseXML;
                var info = response.getElementsByTagName("INFO");

                if (info.length > 0) {
                    var vTable = response.getElementsByTagName("TABLE");
                    var intT;
                    for (intT = 0; intT < vTable.length; intT++) {
                        var item = vTable[intT].getElementsByTagName("ITEM");
                        var objForm = document.Form1;
                        switch (intT) {
                            case 0:
                                {

                                    for (var inti = 0; inti < item.length; inti++) {

                                        gTaskStatus = item[inti].getAttribute("COL0");
                                        document.getElementById('taskStatus').value = gTaskStatus;
                                        //alert(document.getElementById('taskStatus').value);

                                        gblnAttachment = item[inti].getAttribute("COL1");

                                        if (gblnAttachment == 0) {
                                            document.getElementById('imgAttachments').title = "No Attachment Uploaded";
                                        }
                                        else if (gblnAttachment == 1) {
                                            document.getElementById('imgAttachments').title = "View Attachment";
                                        }
                                        else {
                                            document.getElementById('imgAttachments').title = "Select a Task to View Attachment";
                                        }
                                        document.getElementById('imgEdit').title = "Edit Call";
                                        document.getElementById('imgFWD').title = "Forward Task";
                                        document.getElementById('imgDelete').title = "Delete Task";
                                        document.getElementById('imgMonitor').title = "Set Task Monitor";
                                        document.getElementById('ImgActionView').title = "View Actions";
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


        ///**************************Task View AJAX end*********************************///////


        var globleID;
        var rand_no = Math.ceil(500 * Math.random())


        function KeyImage(a, b, c, d, comp, CallNo) {

            if (d == 0) //if comment is clicked
            {
                wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID=' + a + '&tbname=TASK&CallNo=' + CallNo + '&comp=' + comp, 'Comment' + rand_no, 500, 450);
            }
            else if (d == 2) //if Attachment is clicked
            {
                wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=T&TaskNo=' + a + '&CompID=' + comp + '&CallNo=' + CallNo, 'Attachment' + rand_no, 700, 240);
            }
            else 
            {
                wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno=' + CallNo + '&tno=' + a + '&CompID=' + comp, 'AttachForms' + rand_no, 500, 450);
            }
        }

        function OpenVW(varTable) 
        {
            wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrId=464&TBLName=' + varTable, 'Search' + rand_no, 500, 450);
            return false;
        }

        function formReload() 
        {
            self.location.href = 'Task_View.aspx';
        }

        function callrefresh() 
        {
            document.Form1.txthiddenImage.value = '';
            Form1.submit();
        }

        function ConfirmDelete(varImgValue) 
        {
            var cn = document.Form1.txthiddenCallNo.value;
            var tn = document.Form1.txtTask.value;
            var strTaskStatus = document.Form1.taskStatus.value;
            //if (gTaskStatus!=null) 
            if (strTaskStatus != null) {
                if (cn > 0 && tn > 0) {

                    //var ts=gTaskStatus;
                    var ts = strTaskStatus;
                    //	alert(ts);

                    if (ts != 'ASSIGNED') {
                        alert('Task is in ' + ts + ' status and you cannot delete it');
                    }
                    else {
                        var confirmed
                        confirmed = window.confirm("Delete,Are you sure you want to Delete the selected record ?");
                        if (confirmed == false) {
                            return false;
                        }

                        document.Form1.txthiddenImage.value = varImgValue;
                        document.Form1.txtrowvaluesCall.value = 0;
                        Form1.submit();
                    }
                    return false;

                }
            }
            else 
            {
                alert('Please select a row');
            }
            
            return false;
        }


        function SaveEdit(varImgValue) 
        {
               var TaskNo = document.Form1.txtTask.value;
               var CallNo =document.Form1.txthiddenCallNo.value;
			   var CompID =document.Form1.txtComp.value;

            if (varImgValue == 'View') 
            {

                document.Form1.txthiddenImage.value = varImgValue;
                document.Form1.txthiddenCallNo.value = 0;
                //Form1.submit(); 		
                __doPostBack("upnlTaskView", "");
            }

            if (varImgValue == 'ActionView') {
                //	alert(document.Form1.txthiddenCallNo.value);
                if (document.Form1.txthiddenCallNo.value == 0 || document.Form1.txtTask.value == 0) 
                {
                    alert("Please select a task");
                    return false;
                }
                else 
                {
                    intCallNo = document.Form1.txthiddenCallNo.value;
                    intTaskNo = document.Form1.txtTask.value;
                    strComp = document.Form1.txtComp.value;
                    wopen('ActionViewOnly.aspx?ScrID=539&intCallNo=' + intCallNo + '&intTaskNo=' + intTaskNo + '&strComp=' + strComp, 'ActionView' + rand_no, 800, 500);
                }
            }

            if (varImgValue == 'Edit') 
            {
                if (document.Form1.txthiddenCallNo.value == 0) 
                {
                    alert("Please select the row");
                    return false;
                }
                else {
                    //location.href="Call_Detail.aspx?ScrID=3&ID=0";
                    document.Form1.txthiddenImage.value = varImgValue;
                       //window.parent.OpenTabOnAddClick('Call Detail#'+,"SupportCenter/CallView/Call_Detail.aspx?CallNumber='++'&ScrID=3&ID=0&PageID=2", "3");
                    var screenid = window.parent.getActiveTabDetails();
                    window.parent.OpenCallDetailInTab('Call# ' + CallNo, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=2&CallNumber=" + CallNo + "&CompId=" + CompID, 'Call' + CallNo, screenid);
                   // Form1.submit();
                    return false;
                }

            }

            if (varImgValue == 'Close') {
                window.close();
            }

            if (varImgValue == 'Add') 
            {
                document.Form1.txthiddenImage.value = varImgValue;
                window.parent.OpenTabOnAddClick('Call Detail',"SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=-1&PageID=2", "3");
                //Form1.submit();
                return false;

            }

            if (varImgValue == 'ShowReleased') {
                //alert("ads");
                document.Form1.txthiddenImage.value = varImgValue;
                //Form1.submit(); 
                __doPostBack("upnlTaskView", "");
                return false;
            }

            if (varImgValue == 'Search') {
                document.Form1.txthiddenImage.value = varImgValue;
                __doPostBack("upnlTaskView", "");
                //Form1.submit(); 
                return false;
            }

            if (varImgValue == 'Select') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }

            if (varImgValue == 'Monitor') {
                //alert(gTaskStatus);
                var ts = gTaskStatus;
                if (ts == 'CLOSED') {
                    alert('You cannot set Monitor on a Closed Task');
                }
                else {
              
                    var cn = document.Form1.txthiddenCallNo.value;
                    var tn = document.Form1.txtTask.value;
                      var strComp = document.Form1.txtComp.value;
                    if (cn > 0 && tn > 0) {
                        window.open('../../CommunicationSetup/CommunicationSetupOnCall.aspx?&CallNo=' + cn + '&TaskNo=' + tn + '&Comp=' + strComp, 'Attachment', 'scrollBars=yes,resizable=No,width=800,height=600,status=no');
                    }
                    else {
                        alert('Please select a row');
                    }  
                }
                return false;
            }

            if (varImgValue == 'Save') {
                //Security Block
                var obj = document.getElementById("imgSave")
                if (obj == null) {
                    alert("You don't have access rights to save record");
                    return false;
                }

                if (obj.disabled == true) {
                    alert("You don't have access rights to save record");
                    return false;
                }
                //End of Security Block
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }

            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }

            if (varImgValue == 'CloseCall') {
                document.Form1.txthiddenImage.value = varImgValue;
                document.Form1.txtrowvaluesCall.value = 0;
                document.Form1.txthiddenCallNo.value = 0;
                document.Form1.txtTask.value = 0;
                //Form1.submit(); 
                __doPostBack("upnlTaskView", "");
                return false;
            }

            if (varImgValue == 'Fwd') {
                if (document.Form1.txtrowvalues.value == 0) {
                    alert("Please select the row");
                    return false;
                }
                else {
                    var ts = gTaskStatus;
                    if (ts == 'CLOSED') {
                        alert('You cannot forward a Closed Task');
                    }
                    else {
                    intCallNo = document.Form1.txthiddenCallNo.value;
                    intTaskNo = document.Form1.txtTask.value;
                    strComp = document.Form1.txtComp.value;
                    wopen('Task_Fwd.aspx?ScrID=340&CallNo=' + intCallNo + '&TASKNO=' + intTaskNo + '&CompID=' + strComp, 'Fwd' + rand_no, 400, 350);
//                        wopen('Task_Fwd.aspx?ScrID=340', 'Fwd' + rand_no, 400, 350);
                    }
                    return false;
                }

            }

            if (varImgValue == 'Attach') {

                var cn = document.Form1.txthiddenCallNo.value;
                var tn = document.Form1.txtTask.value;
                //	alert(cn+' df'+tn);
                if (cn > 0 && tn > 0) {

                    if (gblnAttachment == 1) {
                        wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CompID=' + document.Form1.txtComp.value + '&CallNo=' + cn, 'Attachments' + rand_no, 700, 240);
                    }
                    else {
                        alert('No Attachment Uploaded');
                    }
                    return false;
                }
                else {
                    alert('Please select a row');
                }

            }

            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset()
                    return false;
                }

            }
            return false;
        }




        function KeyCheck(nn, col, rowvalues, rowvaluescall, tableID, Comp) {

            globleID = col;
            document.Form1.txthiddenCallNo.value = col;
            document.Form1.txtTask.value = nn;
            document.Form1.txthiddenTable.value = tableID;
            document.Form1.txtrowvalues.value = rowvalues;
            document.Form1.txtrowvaluesCall.value = rowvaluescall;

            document.Form1.txtComp.value = Comp;

            var tableID = 'cpnlTaskView_GrdAddSerach'; //your datagrids id

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

            if (tableID == 'cpnlTaskView_GrdAddSerach') {
                document.Form1.txthiddenImage.value = 'Select';
                CallAjax();
                //setTimeout('Form1.submit();',100);
                //Form1.submit(); 
            }
        }


        function KeyCheck555(Adressbookno, suppowner) {
            //alert(Adressbookno);
            var strscreen;

            strscreen = '464'
            OpenUserInfo(Adressbookno, suppowner, strscreen);

            //Form1.submit(); 
            return false;

        }


        function OpenUserInfo(Adressbookno, supponer, strscreen) {
            //alert(varTable);
            //window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
            wopen('UserInfo.aspx?ADDNO=' + Adressbookno + '&CALLOWNER=' + supponer + '&ScreenID=' + strscreen, 'Search' + rand_no, 350, 500);
            //wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo='+a+'&CompID='+comp+'&CallNo='+CallNo ,'Attachment',800,550);

            //	wopen('AB_ViewColumns.aspx','UserView',500,450);
        }


        function KeyCheck55(nn, col, rowvalues, tableID, Comp) 
        {
            
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

            document.Form1.txthiddenCallNo.value = col;
            document.Form1.txtTask.value = nn;
            document.Form1.txthiddenImage.value = 'Edit';
            document.Form1.txthiddenTable.value = tableID;
            document.Form1.txtComp.value = Comp;

            if (tableID == 'cpnlTaskView_GrdAddSerach')
             {
               window.parent.OpenCallDetailInTab('Call# ' + col, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=2&CallNumber=" + col + "&CompId=" + Comp, 'Call' + col,screenid);
               // Form1.submit();
                return false;
                //OpenTask(nn,col); 
            }
            else if (tableID == 'cpnlTaskAction_grdAction') {
                wopen('Action_edit.aspx?ScrID=294&ACTIONNO=' + nn, 'Search001' + rand_no, 500, 450);
            }
            else {
                
             window.parent.OpenCallDetailInTab('Call# ' + col, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=2&CallNumber=" + col + "&CompId=" + Comp , 'Call' + col,screenid);
             // Form1.submit();
               return false;
            }

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
	//Function is used to close the Tab
    function tabClose() {
        
            window.parent.closeTab();
        }	
	
				
    </script>

    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 25%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="0px" Height="0px"
                                                        BorderWidth="0px" ImageUrl="~/Images/white.gif" CommandName="submit" AlternateText=".">
                                                    </asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelTaskView" runat="server" CssClass="TitleLabel">TASK VIEW</asp:Label>
                                                </td>
                                                <td style="width: 50%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:UpdatePanel ID="upnltop" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../../Images/s2Add01.gif"
                                                                    AlternateText="Add" ToolTip="New Call"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                                    AlternateText="Edit" ToolTip="Select a Task to Edit"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                                    AlternateText="Search" ToolTip="Search"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgAttachments" AccessKey="T" runat="server" ImageUrl="../../Images/ScreenHunter_075.bmp"
                                                                    AlternateText="Attachments" ToolTip="Select a Task to View Attachment"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgFWD" AccessKey="F" runat="server" ImageUrl="../../Images/Fwd.jpg"
                                                                    ToolTip="Forward"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server" ImageUrl="../../Images/CloseCall1.gif"
                                                                    AlternateText="CloseCall" ToolTip="Closed Tasks"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgShowReleased" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                                    ToolTip="Show only released tasks"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgMonitor" AccessKey="M" runat="server" ImageUrl="../../Images/callmonitor.jpg"
                                                                    AlternateText="Select a Task to Set Task Monitor"></asp:ImageButton>
                                                                <asp:ImageButton ID="ImgActionView" runat="server" ImageUrl="../../Images/torch2.gif"
                                                                    AlternateText="View Action" ToolTip="Select a Task to View Actions"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                                    AlternateText="Delete" ToolTip="Select a Task to Delete"></asp:ImageButton>
                                                                &nbsp;<img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;"
                                                                    onclick="javascript:location.reload(true);" />
                                                                &nbsp;
                                                                <asp:ImageButton ID="imgClose" runat="server" OnClientClick="tabClose();" ImageUrl="../../Images/s2close01.gif"
                                                                    AlternateText="Close Window"></asp:ImageButton>&nbsp;
                                                                <%--<img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                    style="cursor: hand;" />--%>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <img id="imgAjax" title="ajax" height="30" src="../../images/divider.gif" width="24px"
                                                        visible="false" />
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:UpdateProgress ID="progress" runat="server">
                                                        <ProgressTemplate>
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="../../Images/ajax1.gif" Width="24"
                                                                Height="24" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                                <td style="width: 10%;" nowrap="nowrap">
                                                    <font face="Verdana" size="1"><strong>View&nbsp;<asp:DropDownList ID="ddlstview"
                                                        runat="server" Width="80px" Font-Size="XX-Small" Font-Names="Verdana" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                        <asp:ImageButton ID="imgBtnViewPopup" runat="server" ImageUrl="../../Images/plus.gif">
                                                        </asp:ImageButton></strong></font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../../Images/top_nav_back.gif" height="47">
                                        <%-- <div style="width: 130px">--%>
                                        <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2210','../../');"
                                            alt="Video Help" src="../../Images/video_help.jpg" border="0">
                                        <%--<IMG id="imgAjax" title="ajax" height="30" src="../../images/divider.gif" width="30" height="47" >--%><img
                                            class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('5','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                        &nbsp;&nbsp;<%--</div>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnlTaskView" runat="server">
                                <ContentTemplate>
                                    <cc1:CollapsiblePanel ID="cpnlTaskView" runat="server" Width="100%" BorderStyle="Dotted"
                                        BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                        TitleForeColor="black" TitleClickable="true" TitleBackColor="transparent" Text="Task View"
                                        ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                        Draggable="False">
                                        <div id="divCallView" style="overflow: auto; width: 1054px; height: 370pt">
                                            <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
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
                                                                                <td>
                                                                                    <asp:CheckBox ID="CHKF" runat="server" Width="20px" ToolTip="Form Search" Font-Size="XX-Small"
                                                                                        BorderStyle="None" BorderWidth="0"></asp:CheckBox>
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
                                                        <asp:DataGrid ID="GrdAddSerach" runat="server" CssClass="Grid" Font-Names="Verdana"
                                                            BorderStyle="None" BorderWidth="1px" BorderColor="Silver" ForeColor="MidnightBlue"
                                                            AllowPaging="true" PagerStyle-Visible="False" CellPadding="0" GridLines="Horizontal"
                                                            HorizontalAlign="Left" PageSize="20" DataKeyField="TaskNo" AllowSorting="true">
                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                            <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                            </ItemStyle>
                                                            <HeaderStyle Font-Size="8pt" Font-Bold="true" ForeColor="Black" BackColor="#E0E0E0">
                                                            </HeaderStyle>
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
                                                                <asp:TemplateColumn HeaderText="F">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgform" runat="server"></asp:Image>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <PagerStyle Visible="False"></PagerStyle>
                                                        </asp:DataGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div>
                                            <table height="25">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="pg" Width="40px" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                            ForeColor="#0000C0" runat="server" Font-Bold="true">Page</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="CurrentPg" runat="server" Width="10px" Height="12px" Font-Size="X-Small"
                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="of" Font-Size="8pt" Font-Names="Verdana" ForeColor="#0000C0" runat="server"
                                                            Font-Bold="true">of</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="TotalPages" runat="server" Width="10px" Height="12px" Font-Size="X-Small"
                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="Firstbutton" runat="server" ImageUrl="../../Images/next9.jpg"
                                                            AlternateText="First" ToolTip="First"></asp:ImageButton>
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
                                                        <asp:TextBox ID="txtPageSize" runat="server" Width="24px" Height="14px" Font-Size="7pt"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="Button3" runat="server" Width="16px" Height="12pt" ToolTip="Change Paging Size"
                                                            Font-Size="7pt" BorderStyle="None" Text=">" ForeColor="Navy" Font-Bold="true">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblrecords" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                            ForeColor="MediumBlue" Font-Bold="true">Total Records</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="TotalRecods" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </cc1:CollapsiblePanel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                        <asp:ListBox ID="lstError" runat="server" Width="752px" Font-Size="XX-Small" Font-Names="Verdana"
                            BorderStyle="Groove" BorderWidth="0" ForeColor="Red" Visible="False"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                        <input type="hidden" value="<%=introwvalues%>" name="txtrowvaluesCall" />
                        <input type="hidden" value="<%=strhiddenTable%>" name="txthiddenTable" />
                        <input type="hidden" value="<%=mstrTaskNumber%>" name="txtTask" />
                        <input type="hidden" value="<%=mstrTaskStatus%>" id="taskStatus" />
                        <input type="hidden" value="<%=mstrComp%>" name="txtComp" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
