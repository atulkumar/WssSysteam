<%@ page validaterequest="false" language="vb" autoeventwireup="false" inherits="AB_Main, App_Web_owb2nwqw" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address Book Edit/Entry</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../../DateControl/ION.js"></script>

    <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../../Images/Js/ABMainShortCuts.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">

    <script type="text/javascript">


        var globalid;
        var globalSkil;
        var globalAddNo;
        var globalGrid;

        function OpenItems(c) {
            var IG;
            IG = document.getElementById('cpnlInventory_txtIGroup').value;
            if (IG == '') {
                alert('Please select Item Group first');
            }
            else {
                wopen('../../Search/Common/PopSearch1.aspx?ID=select ItemName ID, Description, ItemCode from ItemMaster where ItemGroup="' + IG + '"' + ' &tbname=' + c, 'Search', 500, 450);
            }
        }


        function OpenW(a, b, c) {
            //alert(c);
            //if user have change the AB type then clear the Bus. relation
            if ('cPnlContact_txtAB_Type' == c) {
                //document.getElementById('cPnlContact_txtBrname').value='';
                //document.getElementById('cPnlContact_txtBr').value='';

            }


            if ('<%=session("PropCompanyType")%>' == 'SCM') {
                wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c, 'Search', 500, 450);
            }
            else {
                wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 ' +
								' where UDC.Company=CI_NU8_Address_Number and ProductCode=' + a + '   and UDCType=' + "'" + b + "'" +
								' and UDC.Company=<%=session("PropCompanyID")%>' +
								' union ' +
								' select Name as ID,Description,' + "'" + "'" + ' as Company from UDC' +
								' where   ProductCode=' + a + '  and UDCType=' + "'" + b + "'" +
								' and UDC.Company=0' +
								' &tbname=' + c, 'Search', 500, 450);
            }

            //wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
        }

        //Function for opening Level
        function OpenWLevel(a, b, c) {
            if (document.getElementById('cPnlContact_txtAB_Type').value == 'COM') {
                alert('Level is not valid for Company');
                document.getElementById('cPnlContact_txtLevel').value = '';
            }
            else {
                if ('<%=session("PropCompanyType")%>' == 'SCM') {
                    wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c, 'Search', 500, 450);
                }
                else {
                    wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 ' +
								' where UDC.Company=CI_NU8_Address_Number and ProductCode=' + a + '   and UDCType=' + "'" + b + "'" +
								' and UDC.Company=<%=session("PropCompanyID")%>' +
								' union ' +
								' select Name as ID,Description,' + "'" + "'" + ' as Company from UDC' +
								' where   ProductCode=' + a + '  and UDCType=' + "'" + b + "'" +
								' and UDC.Company=0' +
								' &tbname=' + c, 'Search', 500, 450);
                }
            }
            //wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
        }

        function OpenRole(c, addressNo) {
            //var abId = '<%=Session("SAddressNumber")%>';
            var abId = addressNo;

            var currentTime = new Date()
            var Day = currentTime.getdate();
            var Month = currentTime.getMonth();
            Month = Number(Month) + 1;
            var Year = currentTime.getFullYear();
            var curDate = Month + '/' + Day + '/' + Year;
            //wopen('../Search/Common/PopSearch.aspx?ID=select ROM_IN4_Role_ID_PK as ID,ROM_VC50_Role_Name as RoleName,CI_VC36_Name as CompanyName from T070031,T010011 where CI_NU8_Address_Number=ROM_IN4_Company_ID_FK and ROM_IN4_Role_ID_PK not in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = '+ abId + ' and RA_DT8_Assigned_Date <=' + "'"+curDate+"'" + 'and RA_DT8_Valid_UpTo >=' + "'"+curDate+"'" + 'and RA_VC4_Status_Code =' + "'ENB'" + ') and ROM_IN4_Company_ID_FK = (select CI_IN4_Business_Relation from t010011 where CI_NU8_Address_Number ='+ abId + ') and ROM_DT8_Start_Date<=' + "'"+curDate+"'" + ' and ROM_DT8_End_Date >=' + "'"+curDate+"'" + ' and ROM_VC50_Status_Code_FK = ' + "'ENB'"  + '&tbname=' + c ,'Search',500,500);	

            wopen('../../Search/Common/PopSearch1.aspx?ID=select ROM_IN4_Role_ID_PK as ID,ROM_VC50_Role_Name as RoleName,CI_VC36_Name as CompanyName from T070031,T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_IN4_Role_ID_PK  in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = ' + abId + ' and RA_DT8_Assigned_Date <=' + "'" + curDate + "'" + 'and RA_DT8_Valid_UpTo >=' + "'" + curDate + "'" + 'and RA_VC4_Status_Code =' + "'ENB'" + ') and (ROM_IN4_Company_ID_FK = (select CI_IN4_Business_Relation from t010011 where CI_NU8_Address_Number =' + abId + ') or ROM_IN4_Company_ID_FK = 0 )and ROM_DT8_Start_Date<=' + "'" + curDate + "'" + ' and ROM_DT8_End_Date >=' + "'" + curDate + "'" + ' and ROM_VC50_Status_Code_FK = ' + "'ENB'" + '&tbname=' + c, 'Search', 500, 500);
        }
        function OpenManager(c, addressNo) {
            var abId = '<%=Viewstate("SAddressNumber_AddressBook")%>';
            //var abId = addressNo;
            wopen('../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_address_number ID,CI_VC36_Name Name,CI_VC8_Level as Level  from t010011 where ci_vc8_address_book_type=' + "'EM'" + '  and ci_vc8_status=' + "'ENA'" + ' and ci_nu8_address_number <> ' + abId + ' and CI_IN4_Business_relation =(Select CI_IN4_Business_relation from t010011 a where CI_NU8_address_number=' + abId + ')&tbname=' + c, "Search", 500, 500);
        }

        function OpenBR(d, a, c) {
            
            if (document.getElementById('cPnlContact_txtAB_Type').value == 'COM') {
                wopen('../../Search/Common/PopSearch1.aspx?ID=select Description as ID, Name as Description, Company from UDC where ProductCode=' + d + '  and UDCType=' + "'" + a + "'" + ' &tbname=' + c, 'Search', 500, 450);
            }
            else // if (document.getElementById('cPnlContact_txtAB_Type').value=='EM')
            {
                 var e = document.getElementById('<%=txtQuery1.ClientID %>').value;
                 wopen("../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name CompanyName,CI_VC8_Status Status from T010011 where CI_VC8_Status='ENA' and  CI_VC8_Address_Book_Type='" + a + "' and CI_NU8_Address_Number IN ("+ e +") &tbname=" + c, "Search98", 500, 450);
                 //wopen("../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name CompanyName,CI_VC8_Status Status from T010011 where CI_VC8_Status='ENA' and  CI_VC8_Address_Book_Type='" + a + "' &tbname=" + c, "Search98", 500, 450);
                 
            }

            //window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
            //popupWin = window.open('../../Search/Common/PopSearch.aspx?ID=select CUM_IN4_Address_No as ID ,CUM_VC25_First_Name as FirstName,CUM_VC25_Middle_Name as Middlename,CUM_VC25_Last_Name as LastName,CUM_VC25_Spouse_Name as SpouseName from TSL0071 where CUM_CH3_Type_id='+"'CUS'",'Search','Width=550px;Height=350px;dialogHide:true;help:no;scroll:yes');
            //window.open("AddAddress.aspx","ss","scrollBars=no,resizable=No,width=400,height=450,status=no  " );
        }

        function OpenW_Add_Address(param, addressNo) {
            addressNo = '<%=Viewstate("SAddressNumber_AddressBook")%>';
            wopen('AB_Additional.aspx?ID=' + param + '&AddressNo=' + addressNo, 'Additional_Address', 400, 480);

            //window.open('AB_Additional.aspx?ID='+param,'Additional_Address','scrollBars=yes,resizable=No,width=400,height=450,status=no');
        }

        function test() {
            alert("Test Ok");
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

        function OpenWUdc_Search() {
            window.open("Udc_Home_Search.aspx", "ss", "scrollBars=no,resizable=No,width=400,height=450,status=no");
        }

        function addToParentList(Afilename, TbName, strname) {

            if (Afilename != "" || Afilename != 'undefined') {
                varName = TbName + 'Name'
                document.getElementById(TbName).value = Afilename;
                try {
                    document.getElementById(varName).value = strname;
                }
                catch (ex) { }
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
            //<!--document.Form1.txtAddType1.value=cc;-->
            document.getElementById('ClpContact_Info_txtBr').value = cc;
        }





        function callrefresh() {
            Form1.submit();
        }




        function KeyCheck(aa, bb, cc, dd) {
            //self.opener.addToParentList(aa,bb);
            globalid = cc;
            globalSkil = aa;
            globalAddNo = bb;
            globalGrid = dd;

            //	alert(dd);

            document.Form1.txthidden.value = aa;

            var tableID = dd  //your datagrids id
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

                table.rows[cc].style.backgroundColor = "#d4d4d4";
            }
        }


        function KeyCheck55(aa, bb, cc, dd) {

            document.Form1.txthiddenImage.value = 'Edit';
            document.Form1.txthiddenSkil.value = aa;
            document.Form1.txthidden.value = bb;
            document.Form1.txthiddenGrid.value = dd;
            SaveEdit('Edit');
        }


        function SaveEdit(varimgValue) {
            if (varimgValue == 'Edit') {
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

                if (globalid == null) {
                    alert("Please select the row");
                    return false;
                }
                else {
                    document.Form1.txthiddenImage.value = varimgValue;
                    document.Form1.txthiddenSkil.value = globalSkil;
                    document.Form1.txthidden.value = globalAddNo;
                    document.Form1.txthiddenGrid.value = globalGrid;
                    Form1.submit();
                    return false;
                }

            }


            if (varimgValue == 'Upload') {
                document.Form1.txthiddenImage.value = varimgValue;


                var str;
                str = document.getElementById('cpnlPersonalInfo_upload').value;
                if (str == "") {
                    alert("Please Browse the Image to Upload        ");
                    return false;
                }
                else {
                    var n = str.indexOf('.');

                    var ext = str.substring(n, str.length).toLowerCase();
                    //alert(ext.toLowerCase());

                    if (ext != '.jpeg' && ext != '.gif' && ext != '.jpg' && ext != '.bmp') {
                        alert('Please select an image file e.g .gif, .jpg, .jpeg');
                    }
                    else {

                        Form1.submit();

                    }
                }

                return false;
            }



            if (varimgValue == 'UploadResume') {
                document.Form1.txthiddenImage.value = varimgValue;


                var str;
                str = document.getElementById('cpnlPersonalInfo_UploadResume').value;

                if (str == "") {
                    alert("Please Browse the Resume to Upload        ");
                    return false;
                }
                else {
                    var n = str.indexOf('.');

                    var ext = str.substring(n, str.length).toLowerCase();
                    //alert(ext.toLowerCase());

                    if (ext != '.doc' && ext != '.rtf' && ext != '.txt') {
                        alert('Please select the Resume file e.g .doc, .rtf, .txt');
                    }
                    else {

                        Form1.submit();

                    }
                }

                return false;
            }




            if (varimgValue == 'Remove') {
                var str;
                str = document.getElementById('cpnlPersonalInfo_txtpath').value;
                if (str == "") {
                    alert("No picture uploaded to remove               ");
                }
                else {

                    var confirmed
                    confirmed = window.confirm("Are you sure you want to Delete the selected Picture ?");
                    if (confirmed == false) {
                        return false;
                    }
                    else {
                        document.Form1.txthiddenImage.value = varimgValue;
                        Form1.submit();
                        return false;
                    }
                }
                return false;
            }

            if (varimgValue == 'RemoveResume') {
                var str;

                str = document.getElementById('cpnlPersonalInfo_txtresumePath').value;

                if (str == "") {
                    alert("No document uploaded to remove               ");
                }
                else {

                    var confirmed
                    confirmed = window.confirm("Are you sure you want to Delete the selected Resume ?");
                    if (confirmed == false) {
                        return false;
                    }
                    else {
                        document.Form1.txthiddenImage.value = varimgValue;
                        Form1.submit();
                        return false;
                    }
                }
                return false;
            }



            if (varimgValue == 'FullSize') {
                document.Form1.txthiddenImage.value = varimgValue;

                var str;
                str = document.getElementById('cpnlPersonalInfo_txtpath').value;
                if (str == "") {
                    alert("Please Upload the Image to View in Full Size");
                }
                else {
                    window.open('ShowImage.aspx?ID=' + str, 'Image', 'menubar=no,toolbar=no,location=no,resizable=yes,scrollbars=yes,status=no');

                }
                return false;

            }

            if (varimgValue == 'Close') {
                document.Form1.txthiddenImage.value = varimgValue;
                Form1.submit();
                return false;
            }


            if (varimgValue == 'Add') {
                document.Form1.txthiddenImage.value = varimgValue;
                Form1.submit();
                return false;
            }

            if (varimgValue == 'Ok') {
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

                document.Form1.txthiddenImage.value = varimgValue;
                Form1.submit();
                return false;
            }

            if (varimgValue == 'Save') {

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
                //alert();
                //cPnlContact
                //if ( checkMailId('cPnlContact_txtEmail1')==true)
                if (mail(document.getElementById('cPnlContact_txtEmail1').value) == true && mail(document.getElementById('cPnlContact_txtEmail2').value) == true) {
                    document.Form1.txthiddenImage.value = varimgValue;
                    //alert(varimgValue);
                    Form1.submit();
                }
                else {
                    //alert('Please enter valid E-Mail Address');
                }
                return false;

                //}
                //	else
                //	{
                //		alert("Enter Email ID in correct format Eg. (youremail@someone.com)");
                //		return false;

                //	}
            }

            if (varimgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varimgValue;
                Form1.submit();
            }

            if (varimgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset()
                }

            }
        }


        function ConfirmDelete(varimgValue) {
            //alert(document.Form1.txthiddentable.value);
            //alert(document.Form1.txtTask.value);


            if (globalid == null) {
                alert("Please select the row");
            }
            else {
                var confirmed
                confirmed = window.confirm("Delete,Are you sure you want to Delete the selected record ?");
                if (confirmed == false) {

                    return false;

                }
                else {

                    document.Form1.txthiddenImage.value = varimgValue;
                    document.Form1.txthiddenSkil.value = globalSkil;
                    document.Form1.txthidden.value = globalAddNo;
                    document.Form1.txthiddenGrid.value = globalGrid;
                    Form1.submit();
                    return false;
                }
            }
            return false;
        }


        function FP_swapimg() {//v1.0
            var doc = document, args = arguments, elm, n; doc.$imgSwaps = new Array(); for (n = 2; n < args.length;
							n += 2) {
                elm = FP_getObjectByID(args[n]); if (elm) {
                    doc.$imgSwaps[doc.$imgSwaps.length] = elm;
                    elm.$src = elm.src; elm.src = args[n + 1];
                } 
            }
        }

        function FP_preloadimgs() {//v1.0
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
										
    </script>

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cPnlContact_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlAddress_collapsible').cells[0].colSpan = "1";
            var z = document.getElementById('cpnlPIComp_collapsible').cells[0].colSpan = "1";
            var z1 = document.getElementById('cpnlPersonalInfo_collapsible').cells[0].colSpan = "1";
//            var z2 = document.getElementById('CpnlSkillSet_collapsible').cells[0].colSpan = "1";
            var z3 = document.getElementById('cpnlCategory_collapsible').cells[0].colSpan = "1";
        }
    </script>

    <script type="text/javascript">

        //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	

    </script>

    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                                <td style="width: 25%">
                                                    &nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" CommandName="submit"
                                                        AlternateText="." ImageUrl="white.GIF" Width="1px"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelAB" runat="server" CssClass="TitleLabel" BorderStyle="None">Address Book Edit/Entry</asp:Label>
                                                </td>
                                                <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" AlternateText="Click to Add New Entry"
                                                            ToolTip="Click to Add New Entry" ImageUrl="../../Images/s2Add01.gif" Visible="false">
                                                        </asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgOk" AccessKey="K" Visible="false" runat="server" ImageUrl="../../Images/s1ok02.gif"
                                                            ToolTip="OK"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                            ToolTip="Edit"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                            ToolTip="Delete"></asp:ImageButton>
                                                        <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('194','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td valign="top" colspan="1">
                                            <!--  **********************************************************************-->
                                            <!-- **********************************************************************-->
                                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td valign="top" align="left">
                                                        <cc1:CollapsiblePanel ID="cPnlContact" runat="server" Width="100%" BorderStyle="Solid"
                                                            BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                            ExpandImage="../../Images/ToggleDown.gif" Text="Contact Info" TitleBackColor="Transparent"
                                                            TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top" align="left">
                                                                        <table id="Table4" style="width: 400px; height: 335px" bordercolor="#5c5a5b" cellspacing="0"
                                                                            cellpadding="0" border="1">
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <asp:Label ID="lblName" runat="server" Width="40px" ForeColor="Black" Font-Bold="True"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small">Name*</asp:Label><br>
                                                                                    <asp:TextBox ID="txtName" runat="server" Width="129px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblAlias" runat="server" Width="44px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Alias</asp:Label><br>
                                                                                        <asp:TextBox ID="txtAlias" runat="server" Width="200px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="16" CssClass="txtNoFocus"></asp:TextBox></font>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <asp:Label ID="lblAB_Type" runat="server" Width="104px" BorderWidth="0px" BorderStyle="None"
                                                                                        ForeColor="Black" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"><u>A</u>dd. Book Type*</asp:Label><br>
                                                                                    <asp:TextBox ID="txtAB_Type" runat="server" Width="80px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                            class="PlusImageCSS" onclick="OpenW(0,'ABTY','cPnlContact_txtAB_Type');" alt="Add. Book Type"
                                                                                            src="../../Images/plus.gif" border="0">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <asp:Label ID="Label82" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small">Business Relation*</asp:Label><br>
                                                                                    <asp:TextBox ID="txtBrName" runat="server" Width="83px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="15" CssClass="txtNoFocus"></asp:TextBox>
                                                                                    <asp:Image ID="imgBR" ImageUrl="../../Images/plus.gif" AlternateText="Business Relation"
                                                                                        CssClass="PlusImageCSS" runat="server"></asp:Image>
                                                                                    <asp:TextBox ID="txtBr" runat="server" Width="0px" BorderWidth="0px" Height="0px"
                                                                                        BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblAddLine1" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Level</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtLevel" runat="server" Width="80px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                            class="PlusImageCSS" onclick="OpenWLevel(0,'LEVL','cPnlContact_txtLevel');" alt="Add. Book Type"
                                                                                            src="../../Images/plus.gif" border="0">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="Label80" runat="server" Width="100%" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">S<u>t</u>atus*</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtStatus" runat="server" Width="83px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                            class="PlusImageCSS" onclick="OpenW(0,'STA','cPnlContact_txtStatus');" alt="Status"
                                                                                            src="../../Images/plus.gif" border="0">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff" colspan="2">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="Label10" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Address Line1*</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtAddLine1" runat="server" Width="340px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff" colspan="2">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblAddLine2" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Address Line2</asp:Label><br>
                                                                                        <asp:TextBox ID="txtAddLine2" runat="server" Width="340px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox></font>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff" colspan="2">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblAddLine3" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Address Line3</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtAddLine3" runat="server" Width="340px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblCity" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small"><u>C</u>ity*</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtCity" runat="server" Width="112px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                            class="PlusImageCSS" onclick="OpenW(0,'CTY','cPnlContact_txtCity');" alt="City"
                                                                                            src="../../Images/plus.gif" border="0">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblProvince" runat="server" Width="80%" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small"><u>P</u>rovince*</asp:Label><br>
                                                                                        <asp:TextBox ID="txtProvince" runat="server" Width="112px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                                class="PlusImageCSS" onclick="OpenW(0,'PROV','cPnlContact_txtProvince');" alt="Province"
                                                                                                src="../../Images/plus.gif" border="0"></font></td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblPostalCode" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Postal Code</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtPostalCode" runat="server" Width="112px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="9" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblCountry" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Count<u>r</u>y*</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtCountry" runat="server" Width="112px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                            class="PlusImageCSS" onclick="OpenW(0,'CNTY','cPnlContact_txtCountry');" alt="Country"
                                                                                            src="../../Images/plus.gif" border="0">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                        <table id="Table114" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="400"
                                                                            border="1">
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                    <asp:Label ID="Label17" runat="server" Width="120px" ForeColor="Black" Font-Bold="True"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small">User Profile ID*</asp:Label><br>
                                                                                    <asp:TextBox ID="txtUserProfileID" runat="server" Width="128px" BorderWidth="1px"
                                                                                        BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="12"
                                                                                        CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <table id="Table8" style="width: 400px" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0"
                                                                            border="1">
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblEmailType1" runat="server" Width="80px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">E<u>m</u>ail Type 1*</asp:Label><br>
                                                                                        <asp:TextBox ID="txtEmailType1" runat="server" Width="69px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                                class="PlusImageCSS" onclick="OpenW(0,'EMLT','cPnlContact_txtEmailType1');" alt="Email Type 1"
                                                                                                src="../../Images/plus.gif" border="0"></font></td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblEmail1" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Email 1*</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtEmail1" runat="server" Width="280px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="72" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="Label11" runat="server" Width="73px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Ema<u>i</u>l Type 2</asp:Label><br>
                                                                                    </font>
                                                                                    <asp:TextBox ID="txtEmailType2" runat="server" Width="69px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                            class="PlusImageCSS" onclick="OpenW(0,'EMLT','cPnlContact_txtEmailType2');" alt="Email Type 2"
                                                                                            src="../../Images/plus.gif" border="0">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <p>
                                                                                        <font face="Verdana" size="1">
                                                                                            <asp:Label ID="lblEmail2" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small">Email 2</asp:Label></font><br>
                                                                                        <asp:TextBox ID="txtEmail2" runat="server" Width="280px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="72" CssClass="txtNoFocus"></asp:TextBox></p>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff" colspan="2">
                                                                                    <table border="0">
                                                                                        <tr>
                                                                                            <td bordercolor="#ffffff">
                                                                                                <font face="Verdana" size="1">
                                                                                                    <asp:Label ID="lblPhoneType1" runat="server" Width="63px" ForeColor="Black" Font-Bold="True"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small">P<u>h</u>. Type 1</asp:Label><br>
                                                                                                    <asp:TextBox ID="txtPhoneType1" runat="server" Width="69px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                                            class="PlusImageCSS" onclick="OpenW(0,'PHTY','cPnlContact_txtPhoneType1');" alt="Ph. Type 1"
                                                                                                            src="../../Images/plus.gif" border="0"></font></td>
                                                                                            <td bordercolor="#ffffff">
                                                                                                <font face="Verdana" size="1">
                                                                                                    <asp:Label ID="lblCountryCode1" runat="server" Width="74px" ForeColor="Black" Font-Bold="True"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small">Co<u>u</u>n. Code 1</asp:Label></font><br>
                                                                                                <asp:TextBox ID="txtCountryCode1" runat="server" Width="72px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                    Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                                        class="PlusImageCSS" onclick="OpenW(0,'CCD','cPnlContact_txtCountryCode1');"
                                                                                                        alt="Coun. Code 1" src="../../Images/plus.gif" border="0">
                                                                                            </td>
                                                                                            <td bordercolor="#ffffff">
                                                                                                <font face="Verdana" size="1">
                                                                                                    <asp:Label ID="lblAreaCode1" runat="server" Width="66px" ForeColor="Black" Font-Bold="True"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small">Area C<u>o</u>de1</asp:Label></font><br>
                                                                                                <asp:TextBox ID="txtAreaCode1" runat="server" Width="64px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                    Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                                        class="PlusImageCSS" onclick="OpenW(0,'ARCD','cPnlContact_txtAreaCode1');" alt="Area Code 1"
                                                                                                        src="../../Images/plus.gif" border="0">
                                                                                            </td>
                                                                                            <td bordercolor="#ffffff">
                                                                                                <font face="Verdana" size="1">
                                                                                                    <asp:Label ID="lblPhone1" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small">Phone 1</asp:Label></font><br>
                                                                                                <asp:TextBox ID="txtPhone1" runat="server" Width="120px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                    Font-Names="Verdana" Font-Size="XX-Small" MaxLength="11" CssClass="txtNoFocus"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff" colspan="2">
                                                                                    <table border="0">
                                                                                        <tr>
                                                                                            <td bordercolor="#ffffff">
                                                                                                <font face="Verdana" size="1">
                                                                                                    <asp:Label ID="Label15" runat="server" Width="63px" ForeColor="Black" Font-Bold="True"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small">Ph. T<u>y</u>pe 2</asp:Label><br>
                                                                                                </font>
                                                                                                <asp:TextBox ID="txtPhoneType2" runat="server" Width="69px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                    Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                                        class="PlusImageCSS" onclick="OpenW(0,'PHTY','cPnlContact_txtPhoneType2');" alt="Ph. Type 2"
                                                                                                        src="../../Images/plus.gif" border="0">
                                                                                            </td>
                                                                                            <td bordercolor="#ffffff">
                                                                                                <font face="Verdana" size="1">
                                                                                                    <asp:Label ID="Label22" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                                                        Font-Size="XX-Small">Cou<u>n</u>. Code 2</asp:Label><br>
                                                                                                    <asp:TextBox ID="txtCountryCode2" runat="server" Width="71px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                                            class="PlusImageCSS" onclick="OpenW(0,'CCD','cPnlContact_txtCountryCode2');"
                                                                                                            alt="Coun. Code 2" src="../../Images/plus.gif" border="0"></font></td>
                                                                                            <td bordercolor="#ffffff">
                                                                                                <font face="Verdana" size="1">
                                                                                                    <asp:Label ID="Label18" runat="server" Width="76px" ForeColor="Black" Font-Bold="True"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small">Ar<u>e</u>a Code2</asp:Label><br>
                                                                                                </font>
                                                                                                <asp:TextBox ID="txtAreaCode2" runat="server" Width="64px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                    Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                                        class="PlusImageCSS" onclick="OpenW(0,'ARCD','cPnlContact_txtAreaCode2');" alt="Area Code 2"
                                                                                                        src="../../Images/plus.gif" border="0">
                                                                                            </td>
                                                                                            <td bordercolor="#ffffff">
                                                                                                <font face="Verdana" size="1">
                                                                                                    <asp:Label ID="Label19" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                                                        Font-Size="XX-Small">Phone 2</asp:Label></font><br>
                                                                                                <asp:TextBox ID="txtPhone2" runat="server" Width="120px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                    Font-Names="Verdana" Font-Size="XX-Small" MaxLength="11" CssClass="txtNoFocus"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <table id="Table11" style="width: 400px" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0"
                                                                            width="422" border="1">
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblID1" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Company ID</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtID1" runat="server" Width="184px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblID2" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Licence No.</asp:Label><br>
                                                                                    </font>
                                                                                    <asp:TextBox ID="txtID2" runat="server" Width="166px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblHomePage1" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">PassportNo</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtHomePage" runat="server" Width="184px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="28" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1"></font>
                                                                                    <asp:Label ID="lblWebAddress" runat="server" Width="98px" ForeColor="Black" Font-Bold="True"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small">HealtCardNo*</asp:Label><br>
                                                                                    <asp:TextBox ID="txtWebAddress" runat="server" Width="167px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="28" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblReference1" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                                            Font-Size="XX-Small">SSNo.*</asp:Label></font><br>
                                                                                    <asp:TextBox ID="txtReference1" runat="server" Width="184px" BorderWidth="1px" BorderStyle="Solid"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                    <font face="Verdana" size="1">
                                                                                        <asp:Label ID="lblReference2" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                                            Font-Size="XX-Small">Reference</asp:Label><br>
                                                                                        <asp:TextBox ID="txtReference2" runat="server" Width="166px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"></asp:TextBox></font>
                                                                                </td>
                                                                                <td bordercolor="#ffffff">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </cc1:CollapsiblePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                            <!-- Panel for displaying Task Info -->
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <cc1:CollapsiblePanel ID="cpnlAddress" runat="server" Width="100%" BorderStyle="Solid"
                                                            BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                            ExpandImage="../../Images/ToggleDown.gif" Text="Additional Address Info(Optional)"
                                                            TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="PowderBlue"
                                                            PanelCSS="panel" TitleCSS="test" State="Collapsed">
                                                            <asp:ImageButton ID="imgBtnAddMore" runat="server" ImageUrl="../../Images/addmore.gif" />
                                                            <asp:DataGrid ID="grdAddress" runat="server" Width="100%" BorderWidth="1px" BorderStyle="None"
                                                                CssClass="Grid" HorizontalAlign="Center" AutoGenerateColumns="False" CellPadding="1"
                                                                DataKeyField="AA_NU8_Address_Sub_Number">
                                                                <FooterStyle CssClass="GridFooter"></FooterStyle>
                                                                <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:ButtonColumn Visible="False" CommandName="Select"></asp:ButtonColumn>
                                                                    <asp:BoundColumn DataField="AA_NU8_Address_Sub_Number" HeaderText="Add No"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC36_Name" HeaderText="Name"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC8_AddressType" HeaderText="Add Type"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC36_Address_Line_1" HeaderText="Add Line1"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC36_Address_Line_2" HeaderText="Add Line2"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC36_Address_Line_3" HeaderText="Add Line3"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC8_City" HeaderText="City"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC8_Province" HeaderText="Province"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_NU8_Postal_Code" HeaderText="Pst Code"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC8_Country" HeaderText="Country"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC8_Status" HeaderText="Status"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AA_VC36_Contact_Person" HeaderText="Cont Per"></asp:BoundColumn>
                                                                </Columns>
                                                                <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="Black" BackColor="#EEEDF4">
                                                                </PagerStyle>
                                                            </asp:DataGrid></cc1:CollapsiblePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="Table141" style="width: 100%" height="10%">
                                                <tr>
                                                    <td valign="top" align="left" width="100%">
                                                        <cc1:CollapsiblePanel ID="cpnlPIComp" runat="server" Width="100%" BorderStyle="Solid"
                                                            BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                            ExpandImage="../../Images/ToggleDown.gif" Text="Company Info(Optional)" TitleBackColor="Transparent"
                                                            TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test"
                                                            State="collapsed">
                                                            <table id="Table12" width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td valign="top" align="left" width="100%">
                                                                            <table id="Table15" width="100%" border="0">
                                                                                <tr>
                                                                                    <td style="height: 54px" width="5">
                                                                                    </td>
                                                                                    <td style="width: 195px; height: 54px">
                                                                                        <asp:Label ID="Label28" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Company Type</asp:Label><br>
                                                                                        <asp:TextBox ID="txtCompType" runat="server" Width="100px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="10" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox><img class="PlusImageCSS" onclick="OpenW(0,'COMT','cpnlPIComp_txtCompType');"
                                                                                                alt="Province" src="../../Images/plus.gif" border="0">
                                                                                    </td>
                                                                                    <td style="width: 213px; height: 54px">
                                                                                        <asp:Label ID="Label25" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Opened Date</asp:Label>
                                                                                        <ION:Customcalendar ID="dtOD" runat="server"></ION:Customcalendar>
                                                                                    </td>
                                                                                    <td style="width: 223px; height: 54px" valign="middle">
                                                                                        <asp:Label ID="Label21" runat="server" Width="118px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" Height="10px">Working Hrs. From</asp:Label><br>
                                                                                        <asp:DropDownList ID="ddlWHFrom" runat="server" Width="125px" Font-Names="Verdana"
                                                                                            Font-Size="XX-Small" Height="18px">
                                                                                            <asp:ListItem Value="01:00">01:00</asp:ListItem>
                                                                                            <asp:ListItem Value="01:30">01:30</asp:ListItem>
                                                                                            <asp:ListItem Value="02:00">02:00</asp:ListItem>
                                                                                            <asp:ListItem Value="02:30">02:30</asp:ListItem>
                                                                                            <asp:ListItem Value="03:00">03:00</asp:ListItem>
                                                                                            <asp:ListItem Value="03:30">03:30</asp:ListItem>
                                                                                            <asp:ListItem Value="04:00">04:00</asp:ListItem>
                                                                                            <asp:ListItem Value="04:30">04:30</asp:ListItem>
                                                                                            <asp:ListItem Value="05:00">05:00</asp:ListItem>
                                                                                            <asp:ListItem Value="05:30">05:30</asp:ListItem>
                                                                                            <asp:ListItem Value="06:00">06:00</asp:ListItem>
                                                                                            <asp:ListItem Value="06:30">06:30</asp:ListItem>
                                                                                            <asp:ListItem Value="07:00">07:00</asp:ListItem>
                                                                                            <asp:ListItem Value="07:30">07:30</asp:ListItem>
                                                                                            <asp:ListItem Value="08:00">08:00</asp:ListItem>
                                                                                            <asp:ListItem Value="08:30">08:30</asp:ListItem>
                                                                                            <asp:ListItem Value="09:00">09:00</asp:ListItem>
                                                                                            <asp:ListItem Value="09:30">09:30</asp:ListItem>
                                                                                            <asp:ListItem Value="10:00">10:00</asp:ListItem>
                                                                                            <asp:ListItem Value="10:30">10:30</asp:ListItem>
                                                                                            <asp:ListItem Value="11:00">11:00</asp:ListItem>
                                                                                            <asp:ListItem Value="11:30">11:30</asp:ListItem>
                                                                                            <asp:ListItem Value="12:00">12:00</asp:ListItem>
                                                                                            <asp:ListItem Value="12:30">12:30</asp:ListItem>
                                                                                            <asp:ListItem Value="13:00">13:00</asp:ListItem>
                                                                                            <asp:ListItem Value="13:30">13:30</asp:ListItem>
                                                                                            <asp:ListItem Value="14:00">14:00</asp:ListItem>
                                                                                            <asp:ListItem Value="14:30">14:30</asp:ListItem>
                                                                                            <asp:ListItem Value="15:00">15:00</asp:ListItem>
                                                                                            <asp:ListItem Value="15:30">15:30</asp:ListItem>
                                                                                            <asp:ListItem Value="16:00">16:00</asp:ListItem>
                                                                                            <asp:ListItem Value="16:30">16:30</asp:ListItem>
                                                                                            <asp:ListItem Value="17:00">17:00</asp:ListItem>
                                                                                            <asp:ListItem Value="17:30">17:30</asp:ListItem>
                                                                                            <asp:ListItem Value="18:00">18:00</asp:ListItem>
                                                                                            <asp:ListItem Value="18:30">18:30</asp:ListItem>
                                                                                            <asp:ListItem Value="19:00">19:00</asp:ListItem>
                                                                                            <asp:ListItem Value="19:30">19:30</asp:ListItem>
                                                                                            <asp:ListItem Value="20:00">20:00</asp:ListItem>
                                                                                            <asp:ListItem Value="20:30">20:30</asp:ListItem>
                                                                                            <asp:ListItem Value="21:00">21:00</asp:ListItem>
                                                                                            <asp:ListItem Value="21:30">21:30</asp:ListItem>
                                                                                            <asp:ListItem Value="22:00">22:00</asp:ListItem>
                                                                                            <asp:ListItem Value="22:30">22:30</asp:ListItem>
                                                                                            <asp:ListItem Value="23:00">23:00</asp:ListItem>
                                                                                            <asp:ListItem Value="23:30">23:30</asp:ListItem>
                                                                                            <asp:ListItem Value="24:00">24:00</asp:ListItem>
                                                                                            <asp:ListItem Value="24:30">24:30</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                        <br>
                                                                                    </td>
                                                                                    <td style="height: 54px" width="192">
                                                                                        <asp:Label ID="Label20" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Working Hrs. To</asp:Label><br>
                                                                                        <asp:DropDownList ID="ddlWHTo" runat="server" Width="116px" Font-Names="Verdana"
                                                                                            Font-Size="XX-Small" Height="18px">
                                                                                            <asp:ListItem Value="01:00">01:00</asp:ListItem>
                                                                                            <asp:ListItem Value="01:30">01:30</asp:ListItem>
                                                                                            <asp:ListItem Value="02:00">02:00</asp:ListItem>
                                                                                            <asp:ListItem Value="02:30">02:30</asp:ListItem>
                                                                                            <asp:ListItem Value="03:00">03:00</asp:ListItem>
                                                                                            <asp:ListItem Value="03:30">03:30</asp:ListItem>
                                                                                            <asp:ListItem Value="04:00">04:00</asp:ListItem>
                                                                                            <asp:ListItem Value="04:30">04:30</asp:ListItem>
                                                                                            <asp:ListItem Value="05:00">05:00</asp:ListItem>
                                                                                            <asp:ListItem Value="05:30">05:30</asp:ListItem>
                                                                                            <asp:ListItem Value="06:00">06:00</asp:ListItem>
                                                                                            <asp:ListItem Value="06:30">06:30</asp:ListItem>
                                                                                            <asp:ListItem Value="07:00">07:00</asp:ListItem>
                                                                                            <asp:ListItem Value="07:30">07:30</asp:ListItem>
                                                                                            <asp:ListItem Value="08:00">08:00</asp:ListItem>
                                                                                            <asp:ListItem Value="08:30">08:30</asp:ListItem>
                                                                                            <asp:ListItem Value="09:00">09:00</asp:ListItem>
                                                                                            <asp:ListItem Value="09:30">09:30</asp:ListItem>
                                                                                            <asp:ListItem Value="10:00">10:00</asp:ListItem>
                                                                                            <asp:ListItem Value="10:30">10:30</asp:ListItem>
                                                                                            <asp:ListItem Value="11:00">11:00</asp:ListItem>
                                                                                            <asp:ListItem Value="11:30">11:30</asp:ListItem>
                                                                                            <asp:ListItem Value="12:00">12:00</asp:ListItem>
                                                                                            <asp:ListItem Value="12:30">12:30</asp:ListItem>
                                                                                            <asp:ListItem Value="13:00">13:00</asp:ListItem>
                                                                                            <asp:ListItem Value="13:30">13:30</asp:ListItem>
                                                                                            <asp:ListItem Value="14:00">14:00</asp:ListItem>
                                                                                            <asp:ListItem Value="14:30">14:30</asp:ListItem>
                                                                                            <asp:ListItem Value="15:00">15:00</asp:ListItem>
                                                                                            <asp:ListItem Value="15:30">15:30</asp:ListItem>
                                                                                            <asp:ListItem Value="16:00">16:00</asp:ListItem>
                                                                                            <asp:ListItem Value="16:30">16:30</asp:ListItem>
                                                                                            <asp:ListItem Value="17:00">17:00</asp:ListItem>
                                                                                            <asp:ListItem Value="17:30">17:30</asp:ListItem>
                                                                                            <asp:ListItem Value="18:00">18:00</asp:ListItem>
                                                                                            <asp:ListItem Value="18:30">18:30</asp:ListItem>
                                                                                            <asp:ListItem Value="19:00">19:00</asp:ListItem>
                                                                                            <asp:ListItem Value="19:30">19:30</asp:ListItem>
                                                                                            <asp:ListItem Value="20:00">20:00</asp:ListItem>
                                                                                            <asp:ListItem Value="20:30">20:30</asp:ListItem>
                                                                                            <asp:ListItem Value="21:00">21:00</asp:ListItem>
                                                                                            <asp:ListItem Value="21:30">21:30</asp:ListItem>
                                                                                            <asp:ListItem Value="22:00">22:00</asp:ListItem>
                                                                                            <asp:ListItem Value="22:30">22:30</asp:ListItem>
                                                                                            <asp:ListItem Value="23:00">23:00</asp:ListItem>
                                                                                            <asp:ListItem Value="23:30">23:30</asp:ListItem>
                                                                                            <asp:ListItem Value="24:00">24:00</asp:ListItem>
                                                                                            <asp:ListItem Value="24:30">24:30</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="height: 54px">
                                                                                        <asp:Label ID="lblCurrency" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Currency</asp:Label><br>
                                                                                        <asp:TextBox ID="txtCurrency" runat="server" Width="100px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="50" CssClass="txtNoFocus"
                                                                                            ReadOnly="True" Height="18px"></asp:TextBox><img class="PlusImageCSS" onclick="OpenW(0,'CUR','cpnlPIComp_txtCurrency');"
                                                                                                alt="Province" src="../../Images/plus.gif" border="0">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="height: 27px" width="5">
                                                                                    </td>
                                                                                    <td style="width: 195px; height: 27px" height="27">
                                                                                        <asp:Label ID="Label13" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">No. of Employees</asp:Label><br>
                                                                                        <asp:TextBox ID="txtNoEmp" runat="server" Width="109px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="9" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 213px; height: 27px">
                                                                                        <asp:Label ID="Label29" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Beneficiary Name</asp:Label><br>
                                                                                        <asp:TextBox ID="txtBenName" runat="server" Width="108px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="70" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 223px; height: 27px" valign="middle">
                                                                                        <asp:Label ID="Label31" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Bank Name</asp:Label><br>
                                                                                        <asp:TextBox ID="txtBankName0" runat="server" Width="128px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="70" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="height: 27px" width="192">
                                                                                        <asp:Label ID="Label32" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Bank Address</asp:Label><br>
                                                                                        <asp:TextBox ID="txtBankAdd" runat="server" Width="114px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="256" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="height: 27px">
                                                                                        <asp:Label ID="Label30" runat="server" Width="77px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" Height="12px">Account #</asp:Label><br>
                                                                                        <asp:TextBox ID="txtAccount" runat="server" Width="109px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="30" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="height: 11px" width="5">
                                                                                    </td>
                                                                                    <td style="width: 195px; height: 11px">
                                                                                        <asp:Label ID="Label33" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">SwiftCode</asp:Label><br>
                                                                                        <asp:TextBox ID="txtSwiftCode" runat="server" Width="107px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="30" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 213px; height: 11px">
                                                                                        <asp:Label ID="Label34" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">Routing No.</asp:Label><br>
                                                                                        <asp:TextBox ID="txtRoutNo" runat="server" Width="108px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="30" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 223px; height: 11px" valign="middle">
                                                                                        <asp:Label ID="Label35" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">MICR</asp:Label><br>
                                                                                        <asp:TextBox ID="txtMICR" runat="server" Width="128px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="256" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="height: 11px" width="192">
                                                                                        <asp:Label ID="Label36" runat="server" Width="112px" ForeColor="Black" Font-Bold="True"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small">IEC No.</asp:Label><br>
                                                                                        <asp:TextBox ID="txtICENo" runat="server" Width="115px" BorderWidth="1px" BorderStyle="Solid"
                                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="256" CssClass="txtNoFocus"
                                                                                            Height="18px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="height: 11px">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="height: 27px" colspan="6">
                                                                                        <asp:CheckBox ID="chkCMailComm" Text="Mail Communication" runat="server" TextAlign="Left"
                                                                                            Checked="True"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                        </cc1:CollapsiblePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" width="100%">
                                            <cc1:CollapsiblePanel ID="cpnlPersonalInfo" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                ExpandImage="../../Images/ToggleDown.gif" Text="Personal Info(Optional)" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test"
                                                State="collapsed">
                                                <table id="Table14" width="100%">
                                                    <tr>
                                                        <td valign="top" align="left" width="100%">
                                                            <table width="100%" border="0">
                                                                <tr>
                                                                    <td width="5">
                                                                    </td>
                                                                    <td width="173">
                                                                        <asp:Label ID="Label1" runat="server" Width="64px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">First Name</asp:Label><br>
                                                                        <asp:TextBox ID="txtFirstName" runat="server" Width="120px" BorderWidth="1px" BorderStyle="Solid"
                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"
                                                                            EnableViewState="True"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblMiddleName" runat="server" Width="72px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Middle Name</asp:Label><br>
                                                                        <asp:TextBox ID="txtMiddleName" runat="server" Width="120px" BorderWidth="1px" BorderStyle="Solid"
                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"
                                                                            EnableViewState="True"></asp:TextBox>
                                                                    </td>
                                                                    <td width="162">
                                                                        <asp:Label ID="lblLastName" runat="server" Width="64px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Last Name</asp:Label><br>
                                                                        <asp:TextBox ID="txtLastName" runat="server" Width="120px" BorderWidth="1px" BorderStyle="Solid"
                                                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="36" CssClass="txtNoFocus"
                                                                            EnableViewState="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5">
                                                                    </td>
                                                                    <td style="width: 173px" width="173" height="1">
                                                                        <asp:Label ID="Label4" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Date Of Birth</asp:Label><br>
                                                                        &nbsp;<ION:Customcalendar ID="dtDOB" runat="server" Height="18px" Width="120px" />
                                                                    </td>
                                                                    <td style="width: 211px">
                                                                        <asp:Label ID="lblMiddleName0" runat="server" Width="48px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Gender</asp:Label><br>
                                                                        <asp:DropDownList ID="ddGender" runat="server" Width="120" Font-Names="Verdana" Font-Size="XX-Small">
                                                                            <asp:ListItem Value="Male">Male</asp:ListItem>
                                                                            <asp:ListItem Value="Female">Female</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="width: 162px" valign="middle">
                                                                        <asp:Label ID="Label3" runat="server" Width="88px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Marital Status</asp:Label><br>
                                                                        <asp:DropDownList ID="ddMartialStatus" runat="server" Width="120px" Font-Names="Verdana"
                                                                            Font-Size="XX-Small">
                                                                            <asp:ListItem Value="Single">Single</asp:ListItem>
                                                                            <asp:ListItem Value="Married">Married</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5">
                                                                    </td>
                                                                    <td style="width: 173px" width="173">
                                                                        <asp:Label ID="Label7" runat="server" Width="120px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Working Hrs. From</asp:Label><br>
                                                                        <asp:DropDownList ID="ddlWHrFrom" runat="server" Width="120px" Font-Names="Verdana"
                                                                            Font-Size="XX-Small">
                                                                            <asp:ListItem Value="01:00">01:00</asp:ListItem>
                                                                            <asp:ListItem Value="01:30">01:30</asp:ListItem>
                                                                            <asp:ListItem Value="02:00">02:00</asp:ListItem>
                                                                            <asp:ListItem Value="02:30">02:30</asp:ListItem>
                                                                            <asp:ListItem Value="03:00">03:00</asp:ListItem>
                                                                            <asp:ListItem Value="03:30">03:30</asp:ListItem>
                                                                            <asp:ListItem Value="04:00">04:00</asp:ListItem>
                                                                            <asp:ListItem Value="04:30">04:30</asp:ListItem>
                                                                            <asp:ListItem Value="05:00">05:00</asp:ListItem>
                                                                            <asp:ListItem Value="05:30">05:30</asp:ListItem>
                                                                            <asp:ListItem Value="06:00">06:00</asp:ListItem>
                                                                            <asp:ListItem Value="06:30">06:30</asp:ListItem>
                                                                            <asp:ListItem Value="07:00">07:00</asp:ListItem>
                                                                            <asp:ListItem Value="07:30">07:30</asp:ListItem>
                                                                            <asp:ListItem Value="08:00">08:00</asp:ListItem>
                                                                            <asp:ListItem Value="08:30">08:30</asp:ListItem>
                                                                            <asp:ListItem Value="09:00">09:00</asp:ListItem>
                                                                            <asp:ListItem Value="09:30">09:30</asp:ListItem>
                                                                            <asp:ListItem Value="10:00">10:00</asp:ListItem>
                                                                            <asp:ListItem Value="10:30">10:30</asp:ListItem>
                                                                            <asp:ListItem Value="11:00">11:00</asp:ListItem>
                                                                            <asp:ListItem Value="11:30">11:30</asp:ListItem>
                                                                            <asp:ListItem Value="12:00">12:00</asp:ListItem>
                                                                            <asp:ListItem Value="12:30">12:30</asp:ListItem>
                                                                            <asp:ListItem Value="13:00">13:00</asp:ListItem>
                                                                            <asp:ListItem Value="13:30">13:30</asp:ListItem>
                                                                            <asp:ListItem Value="14:00">14:00</asp:ListItem>
                                                                            <asp:ListItem Value="14:30">14:30</asp:ListItem>
                                                                            <asp:ListItem Value="15:00">15:00</asp:ListItem>
                                                                            <asp:ListItem Value="15:30">15:30</asp:ListItem>
                                                                            <asp:ListItem Value="16:00">16:00</asp:ListItem>
                                                                            <asp:ListItem Value="16:30">16:30</asp:ListItem>
                                                                            <asp:ListItem Value="17:00">17:00</asp:ListItem>
                                                                            <asp:ListItem Value="17:30">17:30</asp:ListItem>
                                                                            <asp:ListItem Value="18:00">18:00</asp:ListItem>
                                                                            <asp:ListItem Value="18:30">18:30</asp:ListItem>
                                                                            <asp:ListItem Value="19:00">19:00</asp:ListItem>
                                                                            <asp:ListItem Value="19:30">19:30</asp:ListItem>
                                                                            <asp:ListItem Value="20:00">20:00</asp:ListItem>
                                                                            <asp:ListItem Value="20:30">20:30</asp:ListItem>
                                                                            <asp:ListItem Value="21:00">21:00</asp:ListItem>
                                                                            <asp:ListItem Value="21:30">21:30</asp:ListItem>
                                                                            <asp:ListItem Value="22:00">22:00</asp:ListItem>
                                                                            <asp:ListItem Value="22:30">22:30</asp:ListItem>
                                                                            <asp:ListItem Value="23:00">23:00</asp:ListItem>
                                                                            <asp:ListItem Value="23:30">23:30</asp:ListItem>
                                                                            <asp:ListItem Value="24:00">24:00</asp:ListItem>
                                                                            <asp:ListItem Value="24:30">24:30</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br>
                                                                        <td style="width: 211px">
                                                                            <asp:Label ID="combo1" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                Font-Names="Verdana" Font-Size="XX-Small">Working Hrs. To</asp:Label><br>
                                                                            <asp:DropDownList ID="ddlWHrTo" runat="server" Width="120px" Font-Names="Verdana"
                                                                                Font-Size="XX-Small">
                                                                                <asp:ListItem Value="01:00">01:00</asp:ListItem>
                                                                                <asp:ListItem Value="01:30">01:30</asp:ListItem>
                                                                                <asp:ListItem Value="02:00">02:00</asp:ListItem>
                                                                                <asp:ListItem Value="02:30">02:30</asp:ListItem>
                                                                                <asp:ListItem Value="03:00">03:00</asp:ListItem>
                                                                                <asp:ListItem Value="03:30">03:30</asp:ListItem>
                                                                                <asp:ListItem Value="04:00">04:00</asp:ListItem>
                                                                                <asp:ListItem Value="04:30">04:30</asp:ListItem>
                                                                                <asp:ListItem Value="05:00">05:00</asp:ListItem>
                                                                                <asp:ListItem Value="05:30">05:30</asp:ListItem>
                                                                                <asp:ListItem Value="06:00">06:00</asp:ListItem>
                                                                                <asp:ListItem Value="06:30">06:30</asp:ListItem>
                                                                                <asp:ListItem Value="07:00">07:00</asp:ListItem>
                                                                                <asp:ListItem Value="07:30">07:30</asp:ListItem>
                                                                                <asp:ListItem Value="08:00">08:00</asp:ListItem>
                                                                                <asp:ListItem Value="08:30">08:30</asp:ListItem>
                                                                                <asp:ListItem Value="09:00">09:00</asp:ListItem>
                                                                                <asp:ListItem Value="09:30">09:30</asp:ListItem>
                                                                                <asp:ListItem Value="10:00">10:00</asp:ListItem>
                                                                                <asp:ListItem Value="10:30">10:30</asp:ListItem>
                                                                                <asp:ListItem Value="11:00">11:00</asp:ListItem>
                                                                                <asp:ListItem Value="11:30">11:30</asp:ListItem>
                                                                                <asp:ListItem Value="12:00">12:00</asp:ListItem>
                                                                                <asp:ListItem Value="12:30">12:30</asp:ListItem>
                                                                                <asp:ListItem Value="13:00">13:00</asp:ListItem>
                                                                                <asp:ListItem Value="13:30">13:30</asp:ListItem>
                                                                                <asp:ListItem Value="14:00">14:00</asp:ListItem>
                                                                                <asp:ListItem Value="14:30">14:30</asp:ListItem>
                                                                                <asp:ListItem Value="15:00">15:00</asp:ListItem>
                                                                                <asp:ListItem Value="15:30">15:30</asp:ListItem>
                                                                                <asp:ListItem Value="16:00">16:00</asp:ListItem>
                                                                                <asp:ListItem Value="16:30">16:30</asp:ListItem>
                                                                                <asp:ListItem Value="17:00">17:00</asp:ListItem>
                                                                                <asp:ListItem Value="17:30">17:30</asp:ListItem>
                                                                                <asp:ListItem Value="18:00">18:00</asp:ListItem>
                                                                                <asp:ListItem Value="18:30">18:30</asp:ListItem>
                                                                                <asp:ListItem Value="19:00">19:00</asp:ListItem>
                                                                                <asp:ListItem Value="19:30">19:30</asp:ListItem>
                                                                                <asp:ListItem Value="20:00">20:00</asp:ListItem>
                                                                                <asp:ListItem Value="20:30">20:30</asp:ListItem>
                                                                                <asp:ListItem Value="21:00">21:00</asp:ListItem>
                                                                                <asp:ListItem Value="21:30">21:30</asp:ListItem>
                                                                                <asp:ListItem Value="22:00">22:00</asp:ListItem>
                                                                                <asp:ListItem Value="22:30">22:30</asp:ListItem>
                                                                                <asp:ListItem Value="23:00">23:00</asp:ListItem>
                                                                                <asp:ListItem Value="23:30">23:30</asp:ListItem>
                                                                                <asp:ListItem Value="24:00">24:00</asp:ListItem>
                                                                                <asp:ListItem Value="24:30">24:30</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td style="width: 162px" width="162">
                                                                            <asp:Label ID="Label14" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                Font-Names="Verdana" Font-Size="XX-Small">Department</asp:Label><br>
                                                                            <asp:TextBox ID="txtDept" runat="server" Width="112px" BorderWidth="1px" BorderStyle="Solid"
                                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><img
                                                                                    class="PlusImageCSS" onclick="OpenW(0,'DPT','cpnlPersonalInfo_txtDept');" alt="Province"
                                                                                    src="../../Images/plus.gif" border="0">
                                                                        </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5">
                                                                    </td>
                                                                    <td style="width: 173px" width="173">
                                                                        <asp:Label ID="Label8" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Joining Date</asp:Label><br>
                                                                        <ION:Customcalendar ID="dtDOJ" runat="server" Height="18px" Width="120px" />
                                                                    </td>
                                                                    <td style="width: 211px">
                                                                        <asp:Label ID="Label9" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Leaving Date</asp:Label><br>
                                                                        <ION:Customcalendar ID="dtDOL" runat="server" Height="18px" Width="120px" />
                                                                    </td>
                                                                    <td style="width: 162px" width="162">
                                                                        <asp:Label ID="Label16" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Manager</asp:Label><br>
                                                                        <asp:TextBox ID="txtMgrName" runat="server" Width="112px" BorderWidth="1px" BorderStyle="Solid"
                                                                            Font-Names="Verdana" Font-Size="XX-Small" CssClass="txtNoFocus"></asp:TextBox>
                                                                        <asp:ImageButton ID="imgOpenManager" runat="server" ImageUrl="../../Images/plus.gif"
                                                                            class="PlusImageCSS" border="0" ToolTip="Mgr" />
                                                                        <%--<img 
                                                                                        class="PlusImageCSS" onclick="OpenManager('cpnlPersonalInfo_txtMgr');" alt="Mgr"
                                                                                        src="../../Images/plus.gif" border="0">--%>
                                                                        <span style="display: none">
                                                                            <asp:TextBox ID="txtMgr" runat="server" Width="0" BorderWidth="1px" BorderStyle="Solid"
                                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8"></asp:TextBox></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5">
                                                                    </td>
                                                                    <td style="width: 173px" width="173">
                                                                        <asp:CheckBox ID="chkEMailComm" Text="Mail Communication" runat="server" TextAlign="Left"
                                                                            Checked="True"></asp:CheckBox>
                                                                    </td>
                                                                    <td style="width: 211px">
                                                                        <asp:Label ID="Label5" runat="server" Width="80%" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">JobRole</asp:Label>
                                                                        <asp:DropDownList runat="server" Width="120px" ID="ddlJob">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="width: 211px">
                                                                        <asp:Label ID="Label23" runat="server" Width="80%" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Blood Group</asp:Label>
                                                                        <asp:DropDownList ID="ddlbloodgroup" Width="83px" runat="server" AppendDataBoundItems="true">
                                                                            <asp:ListItem Text="Select" Value="NA"></asp:ListItem>
                                                                            <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                                                            <asp:ListItem Text="A+" Value="A+"></asp:ListItem>
                                                                            <asp:ListItem Text="A-" Value="A-"></asp:ListItem>
                                                                            <asp:ListItem Text="B" Value="B"></asp:ListItem>
                                                                            <asp:ListItem Text="B+" Value="B+"></asp:ListItem>
                                                                            <asp:ListItem Text="B-" Value="B-"></asp:ListItem>
                                                                            <asp:ListItem Text="AB+" Value="AB+"></asp:ListItem>
                                                                            <asp:ListItem Text="AB-" Value="AB-"></asp:ListItem>
                                                                            <asp:ListItem Text="O" Value="O"></asp:ListItem>
                                                                            <asp:ListItem Text="O+" Value="O+"></asp:ListItem>
                                                                            <asp:ListItem Text="O-" Value="O-"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <asp:Label ID="Label6" runat="server" Width="80%" ForeColor="Black" Font-Bold="True"
                                                                                Font-Names="Verdana" Font-Size="XX-Small">Time<u>Z</u>one</asp:Label>
                                                                            <asp:DropDownList ID="ddlTZ" runat="server" Width="500px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <asp:Label ID="Label12" runat="server" Width="96px" ForeColor="Black" Font-Bold="True"
                                                                                Font-Names="Verdana" Font-Size="XX-Small">Select Document</asp:Label>
                                                                            <input id="UploadResume" style="width: 248px; height: 22px" type="file" size="22"
                                                                                name="UploadResume" runat="server">
                                                                            <asp:Button ID="BtnUploadResume" AccessKey="U" runat="server" Text="Upload" Height="22px"
                                                                                CausesValidation="False"></asp:Button>
                                                                            <asp:Button ID="BtnRemoveResume" Visible="false" runat="server" Text="Remove" Height="22px"
                                                                                CausesValidation="False"></asp:Button>
                                                                            <span style="display: none">
                                                                                <asp:TextBox ID="txtResumePath" runat="server" Width="0px"></asp:TextBox></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="abc" runat="server" Text="Document Type" ForeColor="Black" Font-Bold="True"
                                                                                Font-Names="Verdana" Font-Size="XX-Small"></asp:Label>
                                                                            <asp:TextBox ID="txtDocType" runat="server" Width="180" MaxLength="50"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <asp:Label ID="lblupload" runat="server" Width="112px"  ForeColor="Black"
                                                                                Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Uploaded Documents</asp:Label>
                                                                            <asp:HyperLink ID="hypresume"  Visible="false" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                                Font-Size="XX-Small" Target="_blank"></asp:HyperLink>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="grdDocs" SkinID="Fun" EmptyDataText="No documents found" GridLines="Both"
                                                                                runat="server" AutoGenerateColumns="False" BorderWidth="1px" BackColor="WhiteSmoke"
                                                                                CellPadding="3" CellSpacing="2" BorderStyle="None" BorderColor="#DEBA84" AlternatingRowStyle-BackColor="#dddddd"
                                                                                HeaderStyle-BackColor="LightBlue" HeaderStyle-ForeColor="White">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <%# Container.DataItemIndex + 1 %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="FileName" HeaderStyle-Width="500" ItemStyle-Width="500"
                                                                                        ItemStyle-Wrap="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:HyperLink ID="lnkDoc" runat="server" NavigateUrl='<%#"~/Dockyard/ABResumes/"+ViewState("SAddressNumber_AddressBook")+"/"+Eval("FileName")  %>'
                                                                                                Text='<%#Eval("FileName") %>'></asp:HyperLink>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                            </table>
                                                        </td>
                                                        <td align="left" width="30%">
                                                            <table id="Table13" style="width: 220px">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Image ID="imgDesign" runat="server" Width="160px"></asp:Image>
                                                                        <span style="display: none">
                                                                            <asp:TextBox ID="txtpath" runat="server" Width="0px"></asp:TextBox></span>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblImageName" runat="server" ForeColor="Black" Font-Names="Verdana"
                                                                            Font-Size="XX-Small"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Button ID="btnUpload" AccessKey="U" runat="server" Text="Upload" Height="22px"
                                                                            CausesValidation="False"></asp:Button>
                                                                        <asp:Button ID="btnFullSize" AccessKey="F" runat="server" Text="Full Size" Height="22px"
                                                                            CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Button ID="btnRem" runat="server" Text="Remove" Height="22px" CausesValidation="False">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="2">
                                                                        <asp:Label ID="Label2" runat="server" Width="72px" ForeColor="Black" Font-Bold="True"
                                                                            Font-Names="Verdana" Font-Size="XX-Small">Picture</asp:Label><input id="upload" type="file"
                                                                                size="20" name="upload" runat="server">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" width="100%">
                                            <cc1:CollapsiblePanel ID="CpnlSkillSet" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                ExpandImage="../../Images/ToggleDown.gif" Text="Skill Set Info(Optional)" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test"
                                                State="Collapsed" Visible="false">
                                                <table id="tblPers" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td valign="top" align="left">
                                                            <div style="overflow: auto; width: 100%">
                                                                <asp:DataGrid ID="grdSkillSet" runat="server" Width="760px" BorderWidth="1px" BorderStyle="None"
                                                                    CssClass="Grid" AutoGenerateColumns="False" CellPadding="1" DataKeyField="ST_NU8_Skill_Number">
                                                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                    <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                    <HeaderStyle Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" ForeColor="Black"
                                                                        CssClass="GridFixedHeader"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:ButtonColumn Visible="False" CommandName="Select"></asp:ButtonColumn>
                                                                        <asp:BoundColumn DataField="ST_VC8_Skill_Type" HeaderText="Skill Type" ItemStyle-Width="100px"
                                                                            HeaderStyle-Width="100px"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="ST_VC8_Skill" HeaderText="Skill" ItemStyle-Width="100px"
                                                                            HeaderStyle-Width="100px"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="ST_VC156_Comment" HeaderText="Comment" ItemStyle-Wrap="True"
                                                                            ItemStyle-Width="560px" HeaderStyle-Width="560px"></asp:BoundColumn>
                                                                    </Columns>
                                                                </asp:DataGrid></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtSkillType" Width="89px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocusFE"
                                                                runat="server"></asp:TextBox><img class="PlusImageCSS" onclick="OpenW(0,'SKTY','CpnlSkillSet_txtSkillType');"
                                                                    alt="Skill Type" src="../../Images/plus.gif" border="0">
                                                            <asp:TextBox ID="txtSkill" Width="89px" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
                                                                Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocusFE" runat="server"></asp:TextBox><img
                                                                    class="PlusImageCSS" onclick="OpenW(0,'SKL','CpnlSkillSet_txtSkill');" alt="Skill"
                                                                    src="../../Images/plus.gif" border="0">
                                                            <asp:TextBox ID="txtSkillComment" Width="557px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" MaxLength="154" CssClass="txtNoFocusFE" runat="server" Font-Size="XX-Small"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" width="100%">
                                            <cc1:CollapsiblePanel ID="cpnlCategory" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                ExpandImage="../../Images/ToggleDown.gif" Text="Category Info(Optional)" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test"
                                                State="collapsed">
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td width="5">
                                                        </td>
                                                        <td width="307">
                                                            <asp:Label ID="lblCatCode1" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="XX-Small">Category code 1</asp:Label><br>
                                                            <asp:TextBox ID="txtCatCode1" runat="server" Width="271px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCatCode6" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="XX-Small">Category code 6</asp:Label><br>
                                                            <asp:TextBox ID="txtCatCode6" runat="server" Width="271px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><br>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="5">
                                                        </td>
                                                        <td width="307">
                                                            <font face="Verdana" size="1">
                                                                <asp:Label ID="lblCatCode2" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                    Font-Names="Verdana" Font-Size="XX-Small">Category code 2</asp:Label><br>
                                                                <asp:TextBox ID="txtCatCode2" runat="server" Width="269px" BorderWidth="1px" BorderStyle="Solid"
                                                                    Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox></font>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCatCode7" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="XX-Small">Category code 7</asp:Label><br>
                                                            <asp:TextBox ID="txtCatCode7" runat="server" Width="271px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="5">
                                                        </td>
                                                        <td width="307">
                                                            <asp:Label ID="lblCatCode3" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="XX-Small">Category code 3</asp:Label><br>
                                                            <asp:TextBox ID="txtCatCode3" runat="server" Width="270px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <font face="Verdana" size="1">
                                                                <asp:Label ID="lblCatCode8" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                    Font-Names="Verdana" Font-Size="XX-Small">Category code 8</asp:Label><br>
                                                                <asp:TextBox ID="txtCatCode8" runat="server" Width="271px" BorderWidth="1px" BorderStyle="Solid"
                                                                    Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox></font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="5">
                                                        </td>
                                                        <td width="307">
                                                            <asp:Label ID="lblCatCode4" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="XX-Small">Category code 4</asp:Label><br>
                                                            <asp:TextBox ID="txtCatCode4" runat="server" Width="268px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCatCode9" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="XX-Small">Category code 9</asp:Label><br>
                                                            <asp:TextBox ID="txtCatCode9" runat="server" Width="271px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="5">
                                                        </td>
                                                        <td width="307">
                                                            <asp:Label ID="lblCatCode5" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="XX-Small">Category code 5</asp:Label><br>
                                                            <asp:TextBox ID="txtCatCode5" runat="server" Width="268px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCatCode10" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="XX-Small">Category code 10</asp:Label><br>
                                                            <asp:TextBox ID="txtCatCode10" runat="server" Width="271px" BorderWidth="1px" BorderStyle="Solid"
                                                                Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8" CssClass="txtNoFocus"></asp:TextBox><br>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" width="100%">
                                            <cc1:CollapsiblePanel ID="cpnlInventory" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                ExpandImage="../../Images/ToggleDown.gif" Text="Inventory" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test"
                                                State="collapsed" Visible="False">
                                                <table cellspacing="0" cellpadding="0" width="0px" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:DataGrid ID="grdInventory" runat="server" Width="100%" BorderWidth="1px" BorderStyle="None"
                                                                CssClass="Grid" HorizontalAlign="Center" AutoGenerateColumns="False" CellPadding="1"
                                                                DataKeyField="ItemID">
                                                                <FooterStyle CssClass="GridFooter"></FooterStyle>
                                                                <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="ItemID" HeaderText="Item ID" HeaderStyle-Width="50px">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ItemGroup" HeaderText="Item Group" HeaderStyle-Width="100px">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ItemName" HeaderText="Item Name" HeaderStyle-Width="150px">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Quantity" HeaderText="Quantity" HeaderStyle-Width="50px">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="IssueDate" HeaderText="Issue Date" HeaderStyle-Width="100px">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Returnable" HeaderText="R" HeaderStyle-Width="20px">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ExpectedReturnDate" HeaderText="ExReturnDate" HeaderStyle-Width="100px">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Comments" HeaderText="Comments" ItemStyle-Wrap="True"
                                                                        HeaderStyle-Width="180px"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlInv" Width="0px" runat="server">
                                                                <table cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img height="0px" src="../../IMages/divider.gif" width="53">
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtIGroup" Width="95px" CssClass="txtNoFocusFE" runat="server"></asp:TextBox><img
                                                                                class="PlusImageCSS" onclick="OpenW(0,'ITGR','cpnlInventory_txtIGroup');" alt="Status"
                                                                                src="../../Images/plus.gif" border="0">
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtIItemName" Width="144px" CssClass="txtNoFocusFE" runat="server"></asp:TextBox><img
                                                                                class="PlusImageCSS" onclick="OpenItems('cpnlInventory_txtIItemName');" alt="Status"
                                                                                src="../../Images/plus.gif" border="0">
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtIQuantity" Width="59px" CssClass="txtNoFocusFE" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <uc1:DateSelector ID="dtIIssueDate" runat="server" Width="101px" height="18px"></uc1:DateSelector>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkIReturnable" Width="24px" runat="server"></asp:CheckBox>
                                                                        </td>
                                                                        <td>
                                                                            <uc1:DateSelector ID="dtIExRetDate" runat="server" Width="102px" height="18px"></uc1:DateSelector>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtIComments" Width="184px" CssClass="txtNoFocusFE" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <asp:ListBox ID="lstError" runat="server" Width="752px" BorderWidth="0" BorderStyle="Groove"
                Visible="false" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox>
            <input type="hidden" name="txthidden" />
            <!--Address Nuimber -->
            <input type="hidden" name="txthiddenSkil" />
            <!--Skill -->
            <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
            <input type="hidden" name="txthiddenGrid" /><!-- Image Clicked-->
           
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:TextBox ID="txtQuery1" runat="server" style="display:none;"></asp:TextBox>    
    </form>
</body>
</html>
