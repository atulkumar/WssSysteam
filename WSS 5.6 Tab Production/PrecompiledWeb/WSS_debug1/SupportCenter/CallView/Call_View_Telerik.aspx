<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_Call_View_Telerik, App_Web_brwmhxgd" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html>
<head>
    <title>Call/Task View</title>
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

    <script type="text/javascript">
        //A Function to call on Page Load to set grid width according to screen size
        function onLoad() {
            var divCallView = document.getElementById('divCallView');
            if (document.body.clientWidth !=0)
            {
            divCallView.style.width = document.body.clientWidth - 30 + "px";
            }
        }

        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlCallTask_collapsible').cells[0].colSpan = "1";
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

        var globleID;
        var rand_no = Math.ceil(500 * Math.random())


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
            //alert(event.keyCode);
            //alert(id);
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

   
   function KeyImage(a, b, c, d, comp, CallNo)         
        {
              if (d == 0) //if comment is clicked

            {
                //var CallNo = document.Form1.txthiddenCallNo.value;
		        //var CompID = document.Form1.txtComp.value;              
		        wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&tbname=T&CompId=' + comp + '&CallNo=' + CallNo + '&TaskNo=' + a + '&ActionNo=0', 'Comment' + rand_no, 500, 450);
		        //wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=T&CallNo='+CallNo+'&CompID='+CompID, 'Comment' + rand_no, 500, 450);
            }
            else if (d == 1) //if Attachment is clicked
            {
                wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=' + c + '&TaskNo=' + a + '&CompId=' + comp + '&CallNo=' + CallNo, 'Attachment' + rand_no, 700, 240);
            }
            else if (d == 5)//call comment
            {
                wopen('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID=' + a + '&tbname=C&CallNo=' + CallNo + '&CompID=' + comp, 'Comment' + rand_no, 500, 450);
            }
            else if (d == 7) 
            {
                wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo=' + a + '&CompId=' + comp + '&CallNo=' + CallNo, 'Attachment' + rand_no, 700, 240);
            }
            else // if Attach form is clicked
            {
                var c;
                c = document.getElementById("txtcallNo").value;
                wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno=' + c + '&tno=' + a, 'AttachForms' + rand_no, 500, 450);
            }
        }


    function KeyCheckTaskEdit(nn, rowvalues, tableID,CompID,CallNo) 
        {

            globaldbclick = 1;
            document.Form1.txthiddenTaskNo.value = nn;
            document.Form1.txthidden.value = nn;
            document.Form1.txthiddenImage.value = 'Edit';
            document.Form1.txthiddentable.value = tableID;
           
         if (tableID == 'cpnlCallTask_dtgTask') 
            {
                OpenTask(nn);
            }
         else if (tableID == 'cpnlTaskAction_grdAction') 
            {
                wopen('Action_edit.aspx?ScrID=294&ACTIONNO=' + nn, 'Search' + rand_no, 430, 300);
            }
         else 
            {
              Form1.submit();
            }
        }
        
        function OpenW(a, b, c)
         {
            wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c, 'Search' + rand_no, 500, 450);
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
        
   function callrefresh() 
            {
             location.href = "../../SupportCenter/CallView/Call_View.aspx?ScrID=4";
            }
     
   function ShowAttachment(P) 
   {
 
           if (P == 1) 
          
           {
           var CompID = document.Form1.txtComp.value;
           var CallNo= document.Form1.txthiddenCallNo.value;
                
           window.open('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CompId=' + CompID + '&CallNo=' + CallNo, 'Attachments' + rand_no, "scrollBars=yes,resizable=No,width=800,height=550,status=no  ");
           }
            else if (P == -1) 
            {
             alert('Please select a call');
            }
            else 
            {
             alert('No attachment Uploaded');
            }
            return false;

        }

   function SaveEdit(varImgValue) 
   {
        
        if (varImgValue == 'View')
             {

                document.Form1.txthiddenImage.value = varImgValue;
                //document.Form1.txthiddenCallNo.value = 0;
                __doPostBack("upnlCallView", "");
            }
        if (varImgValue == 'Edit') 
            {

                //Security Block
               var TaskNo = document.Form1.txthiddenTaskNo.value;
               var CallNo =document.Form1.txthiddenCallNo.value;
			   var CompID =document.Form1.txtComp.value;
               var obj = document.getElementById("imgEdit")
               
               if (obj == null) 
                {
                    alert("You don't have access rights to edit record");
                    return false;
                }
               if (obj.disabled == true) 
                {
                    alert("You don't have access rights to edit record");
                    return false;
                }
                //End of Security Block
                if (document.Form1.txthiddenCallNo.value == 0 && document.Form1.txthiddenTaskNo.value == '') {
                    alert("Please select the Call");
                }
                else 
                {
                    if (document.Form1.txthiddentable.value == 'cpnlCallTask_dtgTask') 
                    {
                       if (document.Form1.txtCallStatus.value == 'CLOSED')
                        { 
                        alert('You cannot edit task for a Closed Call'); 
                        }
                        else
                        { 
                        wopen('Task_edit.aspx?ScrID=334&TASKNO=' + TaskNo+'&CompID='+CompID+'&CallNo='+CallNo+'&ReadOnly=0', 'Search' + rand_no, 440, 470); 
                        }
                    }
                    else {
                        var screenid = window.parent.getActiveTabDetails();
                        //document.Form1.txthiddenImage.value = varImgValue;
                        window.parent.OpenCallDetailInTab('Call# ' + CallNo, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&CallNumber=" + CallNo + "&CompId=" + CompID, 'Call' + CallNo, screenid);
                        //Form1.submit();
                        return false;
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
                Form1.submit();
                return false;
            }
            if (varImgValue == 'Search') {

                document.Form1.txthiddenImage.value = varImgValue;
                //Form1.submit(); 
                __doPostBack("upnlCallView", "")
                return false;
            }
            if (varImgValue == 'Monitor') {

                var CallStatus = document.Form1.txtCallStatus.value;            
                if (CallStatus == 'CLOSED') {
                    alert('You cannot set Monitor on CLOSED call');
                }
                else {
                    CallNo = document.Form1.txthiddenCallNo.value;
                    //CallNo = '<%=Session("PropCallNumber")%>';
                    if ((CallNo == '') || (CallNo == '0')) {
                        alert('Please select the Call');
                        return false;
                    }
                    else {
                        var CompID = document.Form1.txtComp.value;
                        wopen('../../CommunicationSetup/CommunicationSetupOnCall.aspx?CallNo=' + CallNo + '&Comp=' + CompID, 'CommunicationSetup' + rand_no, 800, 490);
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

            if (varImgValue == 'MyCall') {
                document.Form1.txthiddenImage.value = varImgValue;
                __doPostBack("upnlCallView", "")
                //Form1.submit(); 
                return false;
            }

            if (varImgValue == 'Save') {

                //Security Block
                var obj = document.getElementById("imgSave")
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
                __doPostBack("upnlCallView", "")
                //Form1.submit(); 
                return false;
            }

            if (varImgValue == 'Attach') {
                if (document.Form1.txthiddenCallNo.value == 0 && document.Form1.txthiddenTaskNo.value == '') {
                    alert("Please select the Call");
                    return false;
                }
                else {
                    //location.href="Call_Detail.aspx?ScrID=3&ID=0";
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                    return false;
                }
                //document.Form1.txthiddenImage.value=varImgValue;
                //Form1.submit(); 
            }

            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset();

                }

            }
        }


  function callrefresh() 
  {
            Form1.submit();
            document.Form1.txtComp.value = document.Form1.txtComp.value;
        }

   
   function  KeyCheck(TaskNo,CallNo, rowvalues, rowvaluescall, tableID, Comp, SuppComp) 
        
        {
            //TaskNo,CallNo,rowvalues,rowvaluescall,tableID,Comp,ActionNo
            //alert(CallNo);
            globleID = CallNo;
            document.Form1.txthidden.value = TaskNo;
            document.Form1.txthiddentable.value = tableID;
            document.Form1.txtrowvalues.value = rowvalues;
            document.Form1.txtrowvaluescall.value = rowvaluescall;
            document.Form1.txthiddenCallNo.value=CallNo;
			document.Form1.txthiddenTaskNo.value=TaskNo;
                     
            if (tableID == 'cpnlCallView_GrdAddSerach_ctl00') 
            {
                document.Form1.txtComp.value = Comp;
                document.Form1.txthiddenCallNo.value = CallNo;
            }
            else if (tableID == 'cpnlCallTask_dtgTask') 
            {
                document.Form1.txthiddenTaskNo.value = TaskNo;
            }

            var table;

            if (document.all) table = document.all[tableID];
            if (document.getElementById) table = document.getElementById(tableID);
//            if (table) {
//                for (var i = 1; i < table.rows.length; i++) {
//                    if (i % 2 == 0) {
//                        table.rows[i].style.backgroundColor = "#f5f5f5";
//                    }
//                    else {

//                        table.rows[i].style.backgroundColor = "#ffffff";
//                    }
//                }
//                table.rows[rowvalues].style.backgroundColor = "#d4d4d4";
//            }
            if (tableID == 'cpnlCallView_GrdAddSerach_ctl00') {

                document.Form1.txthiddensuppcomp.value = SuppComp;
                document.Form1.txthiddenImage.value = 'Select';
                __doPostBack("upnlCallTask", "")
                return false;
            }
        }

  function KeyCheck555(CallNo, rowvalues, tableID, Comp, CallOwner, TaskOwnerID) {
            var strscreen;
            strscreen = '463'
            document.Form1.txthiddenCallNo.value = CallNo;
            document.Form1.txthidden.value = CallNo;
            document.Form1.txthiddenImage.value = '';
            document.Form1.txthiddentable.value = tableID;
            document.Form1.txtComp.value = Comp;
            document.Form1.txtByWhom.value = CallOwner;

            if (tableID == 'cpnlCallTask_dtgTask') 
            {
                wopen('UserInfo.aspx?ScrID=334&ADDNO=' + TaskOwnerID + '&CALLOWNER=' + CallOwner, 'Search' + rand_no, 350, 500);
            }
            else 
            {
                wopen('UserInfo.aspx?ScrID=334&CALLNO=' + CallNo + '&CALLOWNER=' + CallOwner + '&COMP=' + Comp + '&ScreenID=' + strscreen, 'Search' + rand_no, 350, 500);
                return false;
            }
        }

   function OpenUserInfo(vartable, CallOwner, Comp, strscreen) 
        {
        wopen('UserInfo.aspx?ScrID=334&CALLNO=' + vartable + '&CALLOWNER=' + CallOwner + '&COMP=' + Comp + '&ScreenID=' + strscreen, 'Search' + rand_no, 350, 500);

        }

   function KeyCheck55(nn, rowvalues, tableID, Comp) {
       var screenid = window.parent.getActiveTabDetails();
            //Security Block
            var obj = document.getElementById("imgEdit")
            if (obj == null) 
            {
                alert("You don't have access rights to edit record");
                return false;
            }
            if (obj.disabled == true) 
            {
                alert("You don't have access rights to edit record");
                return false;
            }

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
                document.Form1.txtComp.value = Comp;
                window.parent.OpenCallDetailInTab('Call# ' + nn, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&CallNumber=" + nn + "&CompId=" + Comp, 'Call' + nn, screenid);
                //Form1.submit();
                return false;
            }
        }


   function OpenTask(TaskNo) 
        {
            var CS = document.Form1.txtCallStatus.value;
            if (CS == 'CLOSED') 
            {
                alert('Task cannot be edited for a Closed Call');
            }
            else 
            {
            var CallNo =document.Form1.txthiddenCallNo.value;
			var CompID =document.Form1.txtComp.value;
			
            wopen('Task_edit.aspx?ScrID=334&TASKNO=' + TaskNo+'&CompID='+CompID+'&CallNo='+CallNo, 'Search' + rand_no, 440, 470);
            }
        }

 function OpenVW(vartable) 
            {
         
            wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=463&TBLName=' + vartable, 'Search' + rand_no, 450, 500);
            return false;
            
            }

  function wopen(url, name, w, h) 
        {
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
			
				
    </script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(onLoad);     
    </script>

    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table id="table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td>
                                                    <div style="width: 120px">
                                                        <asp:Button ID="BtnGrdSearch" runat="server" Height="0" Width="0px" BorderWidth="0px">
                                                        </asp:Button><asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px"
                                                            Width="1px" AlternateText="." CommandName="submit" ImageUrl="~/Images/white.gif">
                                                        </asp:ImageButton>
                                                        <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel" Height="12px"> CALL/TASK</asp:Label>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <asp:UpdatePanel ID="ee" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" AlternateText="Add" ImageUrl="../../Images/s2Add01.gif">
                                                                </asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                                </asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" AlternateText="Edit" ImageUrl="../../Images/S2edit01.gif">
                                                                </asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" AlternateText="Search"
                                                                    ImageUrl="../../Images/s1search02.gif"></asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgAttachments" AccessKey="A" runat="server" AlternateText="View Attachments"
                                                                    ImageUrl="../../Images/ScreenHunter_075.bmp"></asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server" AlternateText="Close Call"
                                                                    ImageUrl="../../Images/CloseCall1.gif"></asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgMyCall" AccessKey="M" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                                    ToolTip="My Calls"></asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgMonitor" AccessKey="M" runat="server" AlternateText="Call Monitor"
                                                                    ImageUrl="../../Images/callmonitor.jpg"></asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                                                                    ToolTip="Close"></asp:ImageButton>&nbsp;
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                                <td style="width: 40px">
                                                    <asp:UpdateProgress ID="progress" runat="server">
                                                        <ProgressTemplate>
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="../../Images/ajax1.gif" Width="24"
                                                                Height="24" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                                <td align="center">
                                                    <div style="width: 130px">
                                                        <font face="Verdana" size="1"><strong>View&nbsp;
                                                            <asp:DropDownList ID="ddlstview" runat="server" Width="80px" Font-Names="Verdana"
                                                                Font-Size="XX-Small" AutoPostBack="False">
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="imgBtnViewPopup" runat="server" ImageUrl="../../Images/plus.gif">
                                                            </asp:ImageButton></strong></font></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../../Images/top_nav_back.gif" height="47">
                                        <div style="width: 150px">
                                            <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('4','../../');"
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
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlCallView" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" Height="294px" Width="100%"
                                                        BorderColor="Indigo" Visible="true" TitleCSS="test" CssClass="test" PanelCSS="panel"
                                                        TitleForeColor="black" TitleClickable="true" TitleBackColor="transparent" Text="Call View"
                                                        ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                        Draggable="False" BorderStyle="Solid" BorderWidth="1px">
                                                        <div id="divCallView" style="overflow: auto; width: 1056px; height: 250px">
                                                            <asp:Panel ID="cpnlCallTaskview" DefaultButton="btnCallTaskView" runat="server">
                                                                <asp:Button ID="btnCallTaskView" runat="server" Width="0px" Height="0px" BorderWidth="0px" />
                                                                <table id="table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                    width="100%" align="left" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Panel ID="pnl" runat="server" Width="42px">
                                                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="CHKC" runat="server" Width="18px" ToolTip="Comment Search" Font-Size="XX-Small"
                                                                                                            BorderWidth="0" Visible="false"></asp:CheckBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="CHKA" runat="server" Width="18px" ToolTip="Attachment Search" Font-Size="XX-Small"
                                                                                                            BorderWidth="0" Visible="false"></asp:CheckBox>
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
                                                                            <telerik:RadGrid ID="GrdAddSerach" runat="server" AllowFilteringByColumn="true" AutoGenerateColumns="false"
                                                                                AllowSorting="true" AllowPaging="true" Skin="Office2007">
                                                                                <SelectedItemStyle BackColor="#FFCC66"></SelectedItemStyle>
                                                                                <ItemStyle Font-Size="8pt"></ItemStyle>
                                                                                <AlternatingItemStyle Font-Size="8pt" />
                                                                                <ClientSettings>
                                                                                    <Selecting AllowRowSelect="True" />
                                                                                </ClientSettings>
                                                                                <HeaderStyle Font-Size="8pt" Font-Bold="true" ForeColor="Black" BorderColor="White"
                                                                                    BackColor="#E0E0E0"></HeaderStyle>
                                                                                <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                <MasterTableView DataKeyNames="CallNo" runat="server">
                                                                                    <Columns>
                                                                                        <telerik:GridTemplateColumn HeaderText="C" AllowFiltering="true" ShowFilterIcon="false">
                                                                                            <FilterTemplate>
                                                                                                <asp:CheckBox ID="chkC" Text="C" runat="server" AutoPostBack="true" OnCheckedChanged="abcd" />
                                                                                            </FilterTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Image ID="imgComm" runat="server" ImageUrl="../../Images/comment_Blank.gif">
                                                                                                </asp:Image>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="A" AllowFiltering="true" ShowFilterIcon="false">
                                                                                            <FilterTemplate>
                                                                                                <asp:CheckBox ID="chkA" Text="A" runat="server" AutoPostBack="true" OnCheckedChanged="abcd" />
                                                                                            </FilterTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Image ID="imgAtt" runat="server"></asp:Image>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                    </Columns>
                                                                                </MasterTableView>
                                                                                <PagerStyle Visible="False" Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                            </telerik:RadGrid>
                                                                            <!-- Panel for displaying Task Info -->
                                                                            <!-- Panel for displaying Action Info-->
                                                                            <!-- ***********************************************************************-->
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
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
                                                                        <asp:Label ID="off" Font-Size="8pt" Font-Names="Verdana" ForeColor="#0000C0" runat="server"
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
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlCallTask" runat="server">
                                                <ContentTemplate>
                                                    <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Height="66px" Width="100%"
                                                        BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                        TitleClickable="true" TitleBackColor="transparent" Text="Task View" ExpandImage="../../Images/ToggleDown.gif"
                                                        CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderStyle="Solid"
                                                        BorderWidth="0px">
                                                        <telerik:RadGrid ID="dtgTask1" runat="server" AllowFilteringByColumn="true" AllowSorting="true"
                                                            Width="800px" AllowPaging="true" Skin="Office2007">
                                                            <MasterTableView>
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn>
                                                                        <FooterTemplate>
                                                                            <asp:Panel ID="pnlTask" Width="1px" runat="server" DefaultButton="BtnGrdSearch1">
                                                                                <asp:Button ID="BtnGrdSearch1" runat="server" Height="0px" Width="0px" BorderWidth="0px" />
                                                                                <table style="border-collapse: collapse" width="670" border="0">
                                                                                    <tr align="left">
                                                                                        <td>
                                                                                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table style="border-collapse: collapse" cellspacing="0" cellpadding="0" align="left"
                                                                                    border="0">
                                                                                    <tr valign="top">
                                                                                        <td>
                                                                                            <asp:Image ID="imgHidTask" Width="180px" Height="18px" ImageUrl="../../Images/divider.gif"
                                                                                                runat="server"></asp:Image>
                                                                                            <asp:TextBox ID="TxtStatus_F" runat="server" Height="18px" CssClass="txtNoFocusFE"
                                                                                                Font-Size="XX-Small" Font-Names="Verdana" BorderWidth="1px" BorderStyle="Solid"
                                                                                                Visible="False">ASSIGNED</asp:TextBox>
                                                                                            <asp:TextBox ID="TxtTaskNo_F" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                BorderWidth="1px" BorderStyle="Solid" Visible="False" runat="server" Enabled="False"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="TxtSubject_F" runat="server" Width="187px" Height="18px" CssClass="txtNoFocusFE"
                                                                                                Font-Size="XX-Small" Font-Names="Verdana" BorderWidth="1px" BorderStyle="Solid"
                                                                                                MaxLength="950" TextMode="MultiLine"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <%--<uc1:CustomDDL ID="CDDLTaskType_F" runat="server" Width="68px"></uc1:CustomDDL>--%>
                                                                                            <telerik:RadComboBox ID="CDDLTaskType_F" AllowCustomText="True" runat="server" Width="68px"
                                                                                                DropDownWidth="150px" Height="68px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Task Type"
                                                                                                EnableTextSelection="true" EnableVirtualScrolling="true">
                                                                                            </telerik:RadComboBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <%--<uc1:CustomDDL ID="CDDLTaskOwner_F" runat="server" Width="77px"></uc1:CustomDDL>--%>
                                                                                            <telerik:RadComboBox ID="CDDLTaskOwner_F" AllowCustomText="True" runat="server" Width="77px"
                                                                                                DropDownWidth="150px" Height="77px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Task Owner"
                                                                                                EnableTextSelection="true" EnableVirtualScrolling="true">
                                                                                            </telerik:RadComboBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="DDLDependency_F" class="DDLFieldFE" Width="42px" runat="server">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>
                                                                                            <ION:Customcalendar ID="dtEstCloseDate" runat="server" Width="91px" Height="19px" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="TxtEstimatedHrs" runat="server" Width="30px" Height="12px" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:CheckBox ID="chkMandatory" runat="server" Width="27px" Height="18px" ToolTip="Action Mandatory"
                                                                                                AutoPostBack="False" Font-Size="XX-Small" Font-Names="Verdana" Checked="true">
                                                                                            </asp:CheckBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <%--<uc1:CustomDDL ID="CDDLPriority_F" runat="server" Width="63px"></uc1:CustomDDL>--%>
                                                                                            <telerik:RadComboBox ID="CDDLPriority_F" AllowCustomText="True" runat="server" Width="63px"
                                                                                                Height="63px" DropDownWidth="120px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Priority"
                                                                                                EnableTextSelection="true" EnableVirtualScrolling="true">
                                                                                            </telerik:RadComboBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                        <asp:Panel ID="pnlTask" Width="1px" runat="server" DefaultButton="BtnGrdSearch1">
                                                            <asp:Button ID="BtnGrdSearch1" runat="server" Height="0px" Width="0px" BorderWidth="0px" />
                                                            <table style="border-collapse: collapse" width="670" border="0">
                                                                <tr align="left">
                                                                    <td>
                                                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table style="border-collapse: collapse" cellspacing="0" cellpadding="0" align="left"
                                                                border="0">
                                                                <tr valign="top">
                                                                    <td>
                                                                        <asp:Image ID="imgHidTask" Width="180px" Height="18px" ImageUrl="../../Images/divider.gif"
                                                                            runat="server"></asp:Image>
                                                                        <asp:TextBox ID="TxtStatus_F" runat="server" Height="18px" CssClass="txtNoFocusFE"
                                                                            Font-Size="XX-Small" Font-Names="Verdana" BorderWidth="1px" BorderStyle="Solid"
                                                                            Visible="False">ASSIGNED</asp:TextBox>
                                                                        <asp:TextBox ID="TxtTaskNo_F" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                            BorderWidth="1px" BorderStyle="Solid" Visible="False" runat="server" Enabled="False"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TxtSubject_F" runat="server" Width="187px" Height="18px" CssClass="txtNoFocusFE"
                                                                            Font-Size="XX-Small" Font-Names="Verdana" BorderWidth="1px" BorderStyle="Solid"
                                                                            MaxLength="950" TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <%--<uc1:CustomDDL ID="CDDLTaskType_F" runat="server" Width="68px"></uc1:CustomDDL>--%>
                                                                        <telerik:RadComboBox ID="CDDLTaskType_F" AllowCustomText="True" runat="server" Width="68px"
                                                                            DropDownWidth="150px" Height="68px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                            DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Task Type"
                                                                            EnableTextSelection="true" EnableVirtualScrolling="true">
                                                                        </telerik:RadComboBox>
                                                                    </td>
                                                                    <td>
                                                                        <%--<uc1:CustomDDL ID="CDDLTaskOwner_F" runat="server" Width="77px"></uc1:CustomDDL>--%>
                                                                        <telerik:RadComboBox ID="CDDLTaskOwner_F" AllowCustomText="True" runat="server" Width="77px"
                                                                            DropDownWidth="150px" Height="77px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                            DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Task Owner"
                                                                            EnableTextSelection="true" EnableVirtualScrolling="true">
                                                                        </telerik:RadComboBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="DDLDependency_F" class="DDLFieldFE" Width="42px" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <ION:Customcalendar ID="dtEstCloseDate" runat="server" Width="91px" Height="19px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TxtEstimatedHrs" runat="server" Width="30px" Height="12px" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="chkMandatory" runat="server" Width="27px" Height="18px" ToolTip="Action Mandatory"
                                                                            AutoPostBack="False" Font-Size="XX-Small" Font-Names="Verdana" Checked="true">
                                                                        </asp:CheckBox>
                                                                    </td>
                                                                    <td>
                                                                        <%--<uc1:CustomDDL ID="CDDLPriority_F" runat="server" Width="63px"></uc1:CustomDDL>--%>
                                                                        <telerik:RadComboBox ID="CDDLPriority_F" AllowCustomText="True" runat="server" Width="63px"
                                                                            Height="63px" DropDownWidth="120px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                            DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Priority"
                                                                            EnableTextSelection="true" EnableVirtualScrolling="true">
                                                                        </telerik:RadComboBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </cc1:CollapsiblePanel>
                                                    <input type="hidden" runat="server" id="txtcallNo" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Panel ID="pnlMsg" runat="server">
                    </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server">
                    </asp:Panel>
                    <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <input type="hidden" name="txthiddenImage" style="width: 10px" />
                            <input type="hidden" name="txthidden" style="width: 10px" />
                            <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues" style="width: 10px" />
                            <input type="hidden" value="<%=introwvalues%>" name="txtrowvaluescall" style="width: 10px" />
                            <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" style="width: 10px" />
                            <input type="hidden" name="txthiddenTaskNo" style="width: 10px" />
                            <input type="hidden" value="<%=strhiddentable%>" name="txthiddentable" style="width: 10px" />
                            <input type="hidden" value="<%=mstrComp%>" name="txtComp" style="width: 10px" />
                            <input type="hidden" name="txtByWhom" style="width: 10px" />
                            <input type="hidden" name="txtHIDSize" style="width: 10px" />
                            <input type="hidden" value="<%=mstrsuppcomp%>" name="txthiddensuppcomp" style="width: 10px" />
                            <input type="hidden" value="<%=PropCallStatus%>" name="txtCallStatus" style="width: 10px" />
                            <asp:HiddenField ID="chkStateA" runat="server" />
                            <asp:HiddenField ID="chkStateC" runat="server" />
                            <asp:ListBox ID="lstError" runat="server" Width="702px" Font-Size="XX-Small" Font-Names="Verdana"
                                BorderWidth="0" BorderStyle="Groove" ForeColor="Red" Visible="false"></asp:ListBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
