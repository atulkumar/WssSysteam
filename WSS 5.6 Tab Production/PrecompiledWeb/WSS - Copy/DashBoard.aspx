<%@ page language="VB" autoeventwireup="false" inherits="DashBoard, App_Web_n44rks5n" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WSS</title>
    <style type="text/css">
        /**
	 *	Basic Layout Theme
	 * 
	 *	This theme uses the default layout class-names for all classes
	 *	Add any 'custom class-names', from options: paneClass, resizerClass, togglerClass
	 */.ui-layout-pane
        {
            /* all 'panes' */
            background: #FFF;
            border: 1px solid #BBB;
            padding: 1px;
            overflow: auto;
            vertical-align: top;
        }
        .ui-layout-resizer
        {
            /* all 'resizer-bars' 
            background-color: #A7CF9F;*/
            background-color: #D1ECF6; /*background-image: url("Images/gradient_sidebar.png");*/
            vertical-align: top;
        }
        .ui-layout-toggler
        {
            /* all 'toggler-buttons' 
            background-color: #266421;*/
            background-color: #D6D6D6;
            color: Black;
            font-family: Comic Sans MS;
            background-image: url(     "Images/gradient1.jpg" );
            vertical-align: text-top;
        }
    </style>
    <style type="text/css">
        .header
        {
            position: relative;
            text-align: center;
            padding-bottom: 0px;
            padding-left: 0px;
            padding-right: 0px;
            overflow: hidden;
            font-weight: bold;
            padding-top: 0px;
        }
    </style>
    <style type="text/css">
        .style1
        {
            font-weight: bold;
            font-size: 12px;
            color: #ffffff;
            font-family: Arial, Helvetica, sans-serif;
        }
        .ajax__tab_body
        {
            margin-left: 0;
        }
        .content a:link, a:visited
        {
            background-color: #FFFFFF;
            text-decoration: none;
        }
    </style>
    <link rel="shortcut icon" href="Images/favicon.ico" />
    <link href="Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="Images/Js/jquery-1[1].2.6.js"></script>

    <script type="text/javascript" src="Images/Js/jquery.js"></script>

    <script type="text/javascript" src="Images/Js/jquery.ui.all.js"></script>

    <script type="text/javascript" src="Images/Js/jquery.layout.js"></script>

    <script type="text/javascript" src="images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="images/Js/FastPathShortCut.js"></script>

    <script type="text/javascript">
        function funcLoad() {
            //To set Height of screen according to screen
            var TabContainer1 = document.getElementById("TabContainer1_body");
            if (TabContainer1 != null) {
                var bodyheight = document.body.clientHeight;
                if (bodyheight > 0)
                    TabContainer1.style.height = bodyheight - 42;
            }
            var iFrameFirst = document.getElementById("TabContainer1_TabPanel1_Iframe0");
            if (iFrameFirst != null) {
                var bodyheight = document.body.clientHeight;
                if (bodyheight > 0)
                    iFrameFirst.style.height = bodyheight - 44;
            }
            var bodyheight = document.body.clientHeight;
            if (bodyheight > 0)
                tdmenu.style.height = bodyheight - 80;
            //To hide Role Dropdown and FastPath textbox on page load
            tblRole.style.display = "none";

            if (document.getElementById("imgSideClose").style.display == "block") {
                tblRole.style.display = "block";

            }
        }
        window.onresize = funcLoad;
        window.onload = funcLoad;
    </script>

    <script type="text/javascript">
        //A Function To Stop Refresh on F5
        function document.onkeydown() {
            if (event.keyCode == 116 || event.keyCode == 82) {
                event.keyCode = 0;
                event.cancelBubble = true;
                return false;
            }
        }
        //////////////Right Click/////////////////
        function click(e) {
            if (document.all) {
                if (event.button == 2 || event.button == 3) {
                    alert("Right Click is not Allowed");
                    return false;
                }
            }
            if ((document.layers) || (window.navigator.userAgent.toLowerCase().indexOf('gecko') != -1)) {
                if (e.which == 3) {
                    alert("Right Click is not Allowed");
                    return false;
                }
            }
        }

        if ((document.layers) || (window.navigator.userAgent.toLowerCase().indexOf('gecko') != -1)) {
            document.captureEvents(Event.MOUSEDOWN);
        }
        document.onmousedown = click;
        //////////////Right Click End/////////////////	
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; margin-bottom: 0px">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <script type="text/javascript">
        //To Show Text in the Status Bar
        window.status = " :: WSS ::";

        //Global Variables Declaration
        var popUpObj;
        var newTabPos = 2;
        var win;

        //To Enable Tab i.e add new tab after click on link
        function enableTab(tabHeaderText, srcLink, screenId, tabFrom) {

            //To Count number of tabs
            var count = $find('TabContainer1').get_tabs().length;

            var isTabOpen = false;
            var varscreenid;
            //If Condition to start loop from second tab because first tab will remain open.
            if (count > 1) {
                //For Loop to iterate between Tabs
                for (var i = 2; i <= count; i++) {
                    var tabHeader = 'TabContainer1_' + 'TabPanel' + i + '_tab';
                    if (tabHeader != null) {
                        var Header = document.getElementById(tabHeader);

                        if (Header != null) {
                            varscreenid = trim(document.getElementById('TabContainer1_' + 'TabPanel' + i + '_ScreenId').innerText);
                            var HeaderText = Header.childNodes(0).innerText;
                            var newHeaderText = trim(HeaderText.substr(0, HeaderText.length - 2));
                            //If Condition to check the page is already open or not on basis of screenId
                            if (trim(screenId) == varscreenid) {
                                isTabOpen = true;
                                $find('TabContainer1').set_activeTabIndex(i - 1);
                                break;
                            }
                        }
                    }
                }
            }

            //To Create New TabPanel if Page is already not open in any tab
            if (isTabOpen == false || count == 1) {
                CreateNewTabPanel('TabContainer1', 'TabPanel' + newTabPos, tabHeaderText, srcLink, newTabPos - 1, newTabPos, screenId, tabFrom);
                newTabPos++;
            }
        }

        function CreateNewTabPanel(tabContainerID, tabPanelID, headerText, srcLink, index, activeIndex, ScreenId, tabFrom) {
            //create header

            var header = document.createElement('span');
            header.id = "__tab_" + tabContainerID + tabPanelID;
            header.innerHTML = headerText;
            header.innerHTML += "&nbsp;<span onclick=removeAt('" + tabPanelID + "'," + index + ")>&nbsp;<font color=#000000 size=2 face=Verdana><b>x</b></font></span>";

            $get(tabContainerID + "_header").appendChild(header);

            var screenid = document.createElement('span');
            screenid.id = tabContainerID + "_" + tabPanelID + "_ScreenId";
            screenid.innerHTML = ScreenId;
            screenid.style.display = "none";
            $get(tabContainerID + "_header").appendChild(screenid);

            var tabPageFrom = document.createElement('span');
            tabPageFrom.id = tabContainerID + "_" + tabPanelID + "_tabFrom";
            tabPageFrom.innerHTML = tabFrom;
            tabPageFrom.style.display = "none";
            $get(tabContainerID + "_header").appendChild(tabPageFrom);

            //create content
            var body = document.createElement('div');
            body.id = tabContainerID + "_" + tabPanelID;
            body.style.display = "none";
            body.style.visibility = "hidden";

            body.cssClass = "ajax__tab_panel";
            $get(tabContainerID + "_body").appendChild(body);

            var tempIFrame = document.createElement('iframe');
            tempIFrame.setAttribute('id', 'RSIFrame' + activeIndex);
            tempIFrame.style.border = '0px';
            tempIFrame.style.width = '100%';


            //document.getElementById('Iframe0').height = hght;
            //tempIFrame.style.height = screen.height - 60;
            tempIFrame.style.height = document.body.clientHeight - 46;
            tempIFrame.setAttribute('src', srcLink);
            IFrameObj = $get(tabContainerID + "_" + tabPanelID).appendChild(tempIFrame);
            $create(AjaxControlToolkit.TabPanel, { "headerTab": $get(header.id) }, null, { "owner": tabContainerID }, $get(body.id));

            //To set the active tab
            var lastTab = $find('<%=TabContainer1.ClientID%>').getLastTab();
            $find('<%=TabContainer1.ClientID%>').set_activeTab(lastTab)

        }

        //To Close the tab after click on CLose button i.e  x
        function removeAt(tabPanelClientID, index) {
            //default case to set Tab on 0
            var activetab = $find('TabContainer1').get_activeTabIndex();
            $find('TabContainer1').set_activeTabIndex(0);
            var activeTabPanel = $find('TabContainer1').get_tabs()[index];
            var tabContainerID = "<%=TabContainer1.ClientID %>";
            var pos = index + 1
            var tabHeader = 'TabContainer1_' + 'TabPanel' + pos + '_tab';
            ////
            var tabPageFrom = 'TabContainer1_' + 'TabPanel' + pos + '_tabFrom';
            var tabPageScreenId = document.getElementById(tabPageFrom).innerText;

            ////
            var header = document.getElementById(tabHeader);
            $get(tabContainerID + "_header").removeChild(header);

            var body = document.getElementById('TabContainer1_' + 'TabPanel' + pos)
            $get(tabContainerID + "_body").removeChild(body);

            activeTabPanel.dispose();
            //To set the active tab index i.e 0
            if (tabPageScreenId == '-1') {
                if (activetab == index)
                    $find('TabContainer1').set_activeTabIndex(0);
                else
                    $find('TabContainer1').set_activeTabIndex(activetab);
            }
            else {
                setTabFocus(tabPageScreenId);
            }

        }

        // A Function to Close First Tab if it is opne throgh email
        function closeFirstTab() {
            var Iframe0 = document.getElementById('TabContainer1_TabPanel1_Iframe0');
            Iframe0.setAttribute('src', 'home.aspx');
            var tabHeader = "__tab_TabContainer1_TabPanel1";
            var header = document.getElementById(tabHeader);
            header.innerHTML = 'Home.aspx';
        }
        // A Function to Show HeaderText according to page when we open through Email.
        function openPageFromMail(tabHeaderText) {
            var tabHeader = "__tab_TabContainer1_TabPanel1";
            var header = document.getElementById(tabHeader);
            header.innerHTML = tabHeaderText;
            header.innerHTML += "&nbsp;<span onclick=closeFirstTab()>&nbsp;<font color=#000000 size=2 face=Verdana><b>x</b></font></span>";
        }

        // A genral function to trim whitespace from a variable
        function trim(s) {
            var l = 0; var r = s.length - 1;
            while (l < s.length && s[l] == ' ')
            { l++; }
            while (r > l && s[r] == ' ')
            { r -= 1; }
            return s.substring(l, r + 1);
        }

        // This function will be called by Pages to open call in new tab on Double Click
        function OpenCallDetailInTab(tabHeaderText, srcLink, screenId, tabFrom) {
            enableTab(tabHeaderText, srcLink, screenId, tabFrom);
        }
        //This function will be called by Pages to open SubCategory in new tab on Double Click
        function OpenTabOnDBClick(tabHeaderText, srcLink, screenId, tabFrom) {
            enableTab(tabHeaderText, srcLink, screenId, tabFrom);
        }
        //This function will be called by Pages to open SubCategory in new tab  on Add Click
        function OpenTabOnAddClick(tabHeaderText, srcLink, screenId) {
            enableTab(tabHeaderText, srcLink, screenId);
        }

        //        ///function to change the current tab Name
        //        function changeTabName(callNo) {

        //            var index = $find('TabContainer1').get_activeTabIndex();
        //            var TabScreenId = document.getElementById('TabContainer1_' + 'TabPanel' + index + '_ScreenId');
        //           var tabHeader = 'TabContainer1_' + 'TabPanel' + i + '_tab';
        //            // var screenid = trim(document.getElementById('TabContainer1_' + 'TabPanel' + index + '_ScreenId').innerText);
        //             var pos = index + 1;
        //            var tabHeader = 'TabContainer1_' + 'TabPanel' + pos + '_tab';
        //            ////

        //            var tabPageFrom = 'TabContainer1_' + 'TabPanel' + pos + '_tabFrom';
        //            var tabPageScreenId = document.getElementById(tabPageFrom).innerText;

        //            ////////
        //            var header = document.getElementById(tabHeader);
        //            alert('pageScreeID' + tabPageScreenId);
        //            alert('headr' + header);
        //            alert('screenid' + screenid);
        //            alert(tabHeader + callNo);
        //        }
        //Close Tab from the Close button inside Pages.
        function closeTab() {

            var index = $find('TabContainer1').get_activeTabIndex();
            var activeTabPanel = $find('TabContainer1').get_tabs()[index];
            var tabContainerID = "<%=TabContainer1.ClientID %>";
            var pos = index + 1
            $find('TabContainer1').set_activeTabIndex(0);
            var tabHeader = 'TabContainer1_' + 'TabPanel' + pos + '_tab';
            ////

            var tabPageFrom = 'TabContainer1_' + 'TabPanel' + pos + '_tabFrom';
            var tabPageScreenId = document.getElementById(tabPageFrom).innerText;

            ////////
            var header = document.getElementById(tabHeader);
            $get(tabContainerID + "_header").removeChild(header);

            var body = document.getElementById('TabContainer1_' + 'TabPanel' + pos)
            $get(tabContainerID + "_body").removeChild(body);

            activeTabPanel.dispose();
            //To set the active tab index i.e 0
            //$find('TabContainer1').set_activeTabIndex(0);
            setTabFocus(tabPageScreenId);
        }

        function closeTab1(screenId) {
            closeTab();
            setTabFocus(screenId);

        }

        //Function to get active tab index
        function getActiveTabDetails() {
            var index = $find('TabContainer1').get_activeTabIndex();
            index = index + 1;
            var TabScreenId = document.getElementById('TabContainer1_' + 'TabPanel' + index + '_ScreenId');
            if (TabScreenId != null) {
                var screenid = trim(document.getElementById('TabContainer1_' + 'TabPanel' + index + '_ScreenId').innerText);
            }
            return screenid;
        }
        function setTabFocus(screenId) {

            //To Count number of tabs
            var count = $find('TabContainer1').get_tabs().length;
            var isTabOpen = false;
            var varscreenid;
            //If Condition to start loop from second tab because first tab will remain open.
            if (count > 0) {
                //For Loop to iterate between Tabs
                for (var i = 2; i <= count; i++) {
                    var tabHeader = 'TabContainer1_' + 'TabPanel' + i + '_tab';
                    if (tabHeader != null) {
                        var Header = document.getElementById(tabHeader);
                        if (Header != null) {
                            varscreenid = trim(document.getElementById('TabContainer1_' + 'TabPanel' + i + '_ScreenId').innerText);
                            var HeaderText = Header.childNodes(0).innerText;
                            var newHeaderText = trim(HeaderText.substr(0, HeaderText.length - 2));
                            //If Condition to check the page is already open or not on basis of screenId
                            if (trim(screenId) == varscreenid) {
                                isTabOpen = true;
                                $find('TabContainer1').set_activeTabIndex(i - 1);
                                break;
                            }
                        }
                    }
                }
            }
        }
        
    </script>

    <script type="text/javascript">

        var myLayout; // a var is required because this page utilizes: myLayout.allowOverflow() method
        $(document).ready(function() {
            myLayout = $('body').layout({
                // enable showOverflow on west-pane so popups will overlap north pane
                west__showOverflowOnHover: true
                , west: { size: 180, spacing_closed: 20, initClosed: true, togglerLength_closed: 100, slideTrigger_open: "mouseover"
                , togglerContent_closed: "<font size=2 type=verdana>M<BR>E<BR>N<BR>U</font>", togglerTip_closed: "Open & Pin Menu", sliderTip: "Slide Open Menu"
                }
                , west__fxSettings: { easing: "easeOutBounce", duration: 1500 }
            });
            myLayout.addOpenBtn("#imgSideOk", "west");
            myLayout.addCloseBtn("#imgSideClose", "west");
            document.getElementById("imgSideClose").style.display = "none";
            document.getElementById("imgSideOk").style.display = "block";

        });
    </script>

    <script language="Javascript" type="text/javascript">

        var rand_no = Math.ceil(500 * Math.random())
        function ParentNode(NodeName) {
            CallAjax(NodeName, 0);
            return false;
        }

        function ChildNode(NodeName, NodeID) {
            CallAjax(NodeName, 1);
            return false;
        }



        ///***********************Call View AJAX**********************************////////
        var gtype;
        var xmlHttp;
        var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0;
        var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5") != -1) ? 1 : 0;
        var is_opera = ((navigator.userAgent.indexOf("Opera6") != -1) || (navigator.userAgent.indexOf("Opera/6") != -1)) ? 1 : 0;
        //netscape, safari, mozilla behave the same??? 
        var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0;

        function CallAjax(Name, ID) {
            var url = '../AJAX Server/AjaxInfo.aspx?Type=MenuSession&Name=' + Name + '&ID=' + ID + '&RKey=' + Math.random();
            xmlHttp = GetXmlHttpObject(stateChangeHandler);
            xmlHttp_Get(xmlHttp, url);
        }



        function stateChangeHandler() {
            if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete') {
                //document.getElementById('imgAjax').style.display='none';//src="../images/divider.gif";
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


                                    }
                                    break;
                                } //case 0
                        } //switch
                    } //for loop
                }
            }
            else {
                //	document.getElementById('imgAjax').src="../../images/ajax.gif";
                //document.getElementById('imgAjax').style.display='inline';
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


        function OpenW(c) {
            var Query;
            var UName;
            var RoleID;
            var NowDate;
            UName = document.getElementById('HIDUserName').value;
            RoleID = document.getElementById('HIDRoleID').value;
            NowDate = document.getElementById('HIDNowDate').value;
            Query = "select  OBM.OBM_VC8_FPath  ID ,OBM.OBM_VC50_Alias_Name ScreenName ,OBM.OBM_VC4_Status_Code_FK Status from T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA WHERE UM.UM_VC50_UserID ='" + UName + "' AND UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND RA.RA_DT8_Valid_UpTo >='" + NowDate + "' AND RA.RA_VC4_Status_Code = 'ENB' AND RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND ROM.ROM_DT8_End_Date >= '" + NowDate + "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND OBM.OBM_VC4_Object_Type_FK ='SCR' and ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" + RoleID + " AND OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and OBM.OBM_VC8_FPath<>'' AND ROD.ROD_CH1_View_Hide='V'  and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_vc4_object_type_fk='SCR')"
            wopen('Search/Common/PopSearch1.aspx?ID=' + Query + ' &tbname=' + c, 'Search' + rand_no, 500, 450);
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
					'status=no, toolbar=no, scrollbars=no, resizable=no');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }

        function addToParentList(Afilename, TbName) {

            if (Afilename != "" || Afilename != 'undefined') {
                //alert(Afilename);
                document.getElementById(TbName).value = Afilename;
                aa = Afilename;
                document.getElementById('txtFastPath').focus();
                //document.frmTransterRight.submit();
                document.form1.submit();
            }
            else {
                document.form1.txtAB_Type.value = aa;
            }
        }

        function Submit() {

            if (event.keyCode == 13) {
                //	alert();
                //	document.frmTransterRight.submit();	
            }
        }
    </script>

    <!-- Code to Make Popup as Modal Window-->

    <script type="text/javascript">
        function LoadModalDiv() {
            var bcgDiv = document.getElementById("divBackground");
            bcgDiv.style.display = "block";
            if (bcgDiv != null) {
                if (document.body.clientHeight > document.body.scrollHeight) {
                    bcgDiv.style.height = document.body.clientHeight + "px";
                }
                else {
                    bcgDiv.style.height = document.body.scrollHeight + "px";
                }
                bcgDiv.style.width = "100%";

            }
        }        
    </script>

    <script type="text/javascript">
        function HideModalDiv() {
            var bcgDiv = document.getElementById("divBackground");
            bcgDiv.style.display = "none";
        }
    </script>

    <script type="text/javascript">
        function chgImage() {

            var imgSideOk = document.getElementById("imgSideOk");
            var imgSideClose = document.getElementById("imgSideClose");
            if (imgSideOk.style.display == "block") {
                imgSideClose.style.display = "block";
                imgSideOk.style.display = "none";
                tblRole.style.display = "block";
            }
            else {
                imgSideClose.style.display = "none";
                imgSideOk.style.display = "block";
                tblRole.style.display = "none";
            }
        }
        
    </script>

    <div class="ui-layout-west" style="margin-left: 0px; margin-right: 0px; margin-top: 0px;
        width: 20%; height: 100%; border-color: #BED5F4; border-style: solid; border-width: 10px;
        border-top: 0px; border-right: 0px;">
        <div style="vertical-align: top; width: 100%; height: 20px; text-align: left">
            <table cellpadding="0" cellspacing="0" border="0" width="100%" style="vertical-align: top;">
                <tr style="vertical-align: top; height: 20px;">
                    <td style="vertical-align: top; width: 100%" background="images/top_nav_back.gif"
                        nowrap="nowrap">
                        <span class="style1" style="text-align: center; float: left; width: 80%; color: Black">
                            &nbsp;&nbsp;ION Softnet&nbsp;<asp:Label Style="text-decoration: none" ID="lblEnv"
                                runat="server" Font-Bold="True">WSS5.3</asp:Label>
                        </span><span style="float: right; vertical-align: middle; width: 8%;">
                            <img src="Images/pin-dn-on.gif" id="imgSideClose" alt="" height="14" width="14" onclick="chgImage();" /></span>
                        <span style="float: right; vertical-align: middle; width: 8%;">
                            <img src="Images/pin-up-on.gif" id="imgSideOk" height="13" width="13" alt="" title="Pin"
                                onclick="chgImage();" /></span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tdmenu" style="vertical-align: top; text-align: left; overflow: auto; width: 100%;
            height: 550px">
            <table cellspacing="0" cellpadding="0" width="100%" border="0" style="height: 100%;
                vertical-align: top; text-align: left">
                <tr style="width: 100%; height: 100%">
                    <td width="100%" valign="top" align="left">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr style="height: 100%; vertical-align: top">
                                <td width="100%" style="height: 100%">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlMenu" runat="server" Height="100%">
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--<tr>
                            <td width="100%" align="left">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td width="10">
                                            <img height="31" src="images/bottom_left.gif" width="10">
                                        </td>
                                        <td background="images/bottom_back.gif">
                                            <asp:Label ID="lblEnvWEW" runat="server" Text="&nbsp;Press F11 for best view" Font-Name="Verdana"
                                                Font-Size="10px" Font-Bold="True" Font-Italic="True" ForeColor="DarkBlue"></asp:Label>
                                             </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
            </table>
        </div>
        <div style="vertical-align: top; text-align: left; overflow: auto; width: 90%;">
            <table cellspacing="0" cellpadding="0" width="100%" border="0" style="height: 100%;
                vertical-align: top; text-align: left">
                <tr>
                    <td>
                        <asp:Label ID="lblUserID" runat="server" Font-Names="Verdana" Font-Size="XX-Small"></asp:Label>
                    </td>
                </tr>
                <tr style="width: 100%; vertical-align: bottom">
                    <td width="100%" align="left">
                        <table id="tblRole" cellspacing="0" cellpadding="2" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblrole" runat="server" Font-Names="Verdana" Font-Size="XX-Small"><b>Role</b></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRoleName" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                        AutoPostBack="True" Width="95px">
                                    </asp:DropDownList>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="display: none">
                                    <font face="Verdana" color="midnightblue" size="1"><strong>Fast Path</strong> </font>
                                </td>
                                <td style="display: none">
                                    <asp:TextBox ID="txtFastPath" runat="server" Font-Size="xx-small" MaxLength="4" BorderWidth="1px"
                                        BorderStyle="Solid" Width="40px" CssClass="txtFocus" Height="15px" Font-Name="verdana"></asp:TextBox>
                                    <img class="PlusImageCSS" onclick="OpenW('txtFastPath');" alt="Fast Path" src="images/plus.gif"
                                        border="0">
                                </td>
                            </tr>
                        </table>
                        <%--<script type="text/javascript" language="JavaScript">
                                    // Anytime Anywhere Web Page Clock Generator
                                    // Clock Script Generated at
                                    // http://www.rainbow.arch.scriptmania.com/tools/clock

                                    function tS() { x = new Date(); x.setTime(x.getTime()); return x; }
                                    function lZ(x) { return (x > 9) ? x : '0' + x; }
                                    function tH(x) { if (x == 0) { x = 12; } return (x > 12) ? x -= 12 : x; }
                                    function y2(x) { x = (x < 500) ? x + 1900 : x; return String(x).substring(2, 4) }
                                    //function dT(){ window.status=''+eval(oT)+''; document.title=''+eval(oT)+''; if(fr==0){ fr=1; document.write('<font size=1 face=verdana color=midnightblue><b><span id="tP">'+eval(oT)+'</span></b></font>'); } tP.innerText=eval(oT); setTimeout('dT()',1000); } 
                                    function dT() { window.status = '' + eval(oT) + ''; document.title = '' + eval(oT) + ''; if (fr == 0) { fr = 1; document.write('<font size=1 face=verdana color=midnightblue><b><span id="tP">' + eval(oT) + '</span></b></font>'); } tP.innerText = ''; setTimeout('dT()', 1000); }
                                    function aP(x) { return (x > 11) ? 'pm' : 'am'; }
                                    var dN = new Array('Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'), mN = new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'), fr = 0, oT = "dN[tS().getDay()]+' '+tS().getDate()+' '+mN[tS().getMonth()]+' '+y2(tS().getYear())+' '+' '+' '+tH(tS().getHours())+':'+lZ(tS().getMinutes())+':'+lZ(tS().getSeconds())+' '+aP(tS().getHours())";
                                </script>

                                <!--	 Clock Part 1 - Ends Here  -->
                                <!-- Clock Part 2 - This Starts/Displays Your Clock -->

                                <script type="text/javascript" language="JavaScript">
                                    dT();
                                </script>--%>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                        <input id="HIDUserName" type="hidden" name="txtHIDUserName" runat="server" width="2" />
                        <input id="HIDRoleID" type="hidden" name="txtHIDRoleID" runat="server" width="2" />
                        <input id="HIDNowDate" type="hidden" name="txtHIDNowDate" runat="server" width="2" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="ui-layout-center" style="margin-left: 0px; margin-right: 0px; margin-top: 0px;
        width: 80%; height: 100%;">
        <cc1:TabContainer runat="server" ID="TabContainer1" Height="580px" BackColor="#F5F5F5">
            <cc1:TabPanel runat="server" ID="TabPanel1" HeaderText="TabPanel1" Width="100%" BackColor="#F5F5F5">
                <HeaderTemplate>
                    Home Page
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="Iframe0" height="100%" src="Home.aspx" width="100%" scrolling="auto"
                        runat="server"></iframe>
                </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>
        <!--<div style="background-color:#D1E6F5;width:100%;height:15px;text-align:center;vertical-align:middle;font-size:x-small;font-family:Verdana">Copyright
    © 2009 ION Solutions All rights reserved</div>-->
    </div>
    <div id="divBackground" style="position: absolute; top: 0px; left: 0px; background-color: black;
        z-index: 100; opacity: 0.8; filter: alpha(opacity=60); -moz-opacity: 0.8; overflow: hidden;
        display: none; text-align: center;">
    </div>
    </form>
</body>
</html>
