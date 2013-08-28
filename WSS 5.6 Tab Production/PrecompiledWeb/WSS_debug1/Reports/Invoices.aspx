<%@ page language="VB" autoeventwireup="false" inherits="Reports_Invoices, App_Web_wzhf50l2" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>CReports</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../images/js/core.js" type="text/javascript"></script>

    <script src="../images/js/events.js" type="text/javascript"></script>

    <script src="../images/js/css.js" type="text/javascript"></script>

    <script src="../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
	/**********************AJAX for Project****************************************/

	var gtype;
	var xmlHttp; 
	var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
	var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
	var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
	//netscape, safari, mozilla behave the same??? 
	var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 


	var gPID;
	var gSID;
	var gDDL;

    function ShowImg()
    {
     document.getElementById('imgAjax').src="../images/ajax1.gif";
     document.getElementById('imgAjax').style.display='inline';
    }
	function DDLChange(DDL,CMID,SID )
	{
 
	//alert(PID);
	//document.getElementById('txtHIDAgreement')='';
	//gPID=PID;//Project
	gSID=SID;//Status
	gDDL=DDL;
	//gCNID=CNID;//Call Numbers
	//gCNID2=CNID2;//Call Numbers
	xmlHttp=null;
	var ddlCompany=document.getElementById(CMID);
	var CompID=ddlCompany.options(ddlCompany.selectedIndex).value;
	var ddlStatus=document.getElementById(SID);
	//var ddlCallFrom=document.getElementById(CNID);
	//var ProjectID=ddlProject.options(ddlProject.selectedIndex).value;

	if ( CompID==0)
	{
	ddlStatus.disabled=true;
	}
	else
	{
	ddlStatus.disabled=false;
	}
	var url= '../AJAX Server/AjaxInfo.aspx?DDL='+ DDL +'&Type=Invoices&CompID='+CompID+'&Rnd='+Math.random();

	xmlHttp = GetXmlHttpObject(stateChangeHandler); 

	xmlHttp_Get(xmlHttp, url); 
	}


	function stateChangeHandler() 
	{ 	

	if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
	{ 
	document.getElementById(gSID).options.length=0;
	//document.getElementById(gCNID).options.length=0;
	//document.getElementById(gCNID2).options.length=0;
	var objNewOption;

	objNewOption = document.createElement("OPTION");
	document.getElementById(gSID).options.add(objNewOption);
	objNewOption.value = '0';
	objNewOption.innerText ='--ALL--';

	//objNewOption = document.createElement("OPTION");
	//document.getElementById(gCNID).options.add(objNewOption);
	//objNewOption.value = '0';
	//objNewOption.innerText ='--ALL--';



	//objNewOption = document.createElement("OPTION");
	//document.getElementById(gCNID2).options.add(objNewOption);
	//objNewOption.value = '0';
	//objNewOption.innerText ='--ALL--';



	var response = xmlHttp.responseXML; 
	var info = response.getElementsByTagName("INFO");

	if(info.length > 0)
	{
	var vTable = response.getElementsByTagName("TABLE");
	var intT;

	for ( intT=0; intT<vTable.length; intT++)
	{
	var item = vTable[intT].getElementsByTagName("ITEM");
	var objForm = document.Form1;
	var RoleDataName='';
	var RoleDataID='';
	switch(intT)
	{


	case 0://Status
	{
	for (var inti=0; inti<item.length; inti++)
	{
	var objNewOption = document.createElement("OPTION");
	document.getElementById(gSID).options.add(objNewOption);
	objNewOption.value = item[inti].getAttribute("COL0");
	objNewOption.innerText = item[inti].getAttribute("COL1");
	RoleDataName=RoleDataName+item[inti].getAttribute("COL1") + '^';
	RoleDataID=RoleDataID+item[inti].getAttribute("COL0") + '^';													
	}
	document.Form1.txtHiddenStatus.value= RoleDataName + '~' + RoleDataID ;
	break;
	}//case 0
					
	}//switch
	} //for loop
	}//if
	}//
	else
	{
	//document.getElementById('imgAjax').src="../../images/ajax.gif";
	//document.getElementById('imgAjax').style.display='inline';

	}

	} //function


	function xmlHttp_Get(xmlhttp, url) 
	{ 
	xmlhttp.open('GET', url, true); 
	xmlhttp.send(null); 

	} 

	function GetXmlHttpObject(handler) 
	{ 
	var objXmlHttp = null;    //Holds the local xmlHTTP object instance 
	if (is_ie)
	{ 
	var strObjName = (is_ie5) ? 'Microsoft.XMLHTTP' : 'Msxml2.XMLHTTP'; 
	try
	{ 
	objXmlHttp = new ActiveXObject(strObjName); 
	objXmlHttp.onreadystatechange = handler; 
	} 
	catch(e)
	{ 
	alert('IE detected, but object could not be created. Verify that active scripting and activeX controls are enabled'); 
	return; 
	} 
	} 
	else if (is_opera)
	{ 
	alert('Opera detected. The page may not behave as expected.'); 
	return; 
	} 
	else
	{ 
	objXmlHttp = new XMLHttpRequest(); 
	objXmlHttp.onload = handler; 
	objXmlHttp.onerror = handler; 
	} 
	return objXmlHttp; 
	} 



	/***************************************************************************/

	function back(flag)
	{
	if ( flag =='inl')
	{
	document.Form1.txthiddenImage.value="inl";	         
	document.Form1.submit();
	}
	else
	{
	window.history.back(-1);
	}
	return false;
	}

	function SaveEdit(varImgValue)
	{


	if (varImgValue=='Close')
	{
	window.close(); 
	}



	if (varImgValue=='Ok')
	{
	document.Form1.txthiddenImage.value=varImgValue;
	Form1.submit(); 
	CloseWindow()
	}

	if (varImgValue=='Save')
	{
	document.Form1.txthiddenImage.value=varImgValue;
	Form1.submit(); 
	}		

	if (varImgValue=='Logout')
	{
	document.Form1.txthiddenImage.value=varImgValue;
	Form1.submit(); 
	return false;
	}



	if (varImgValue=='Reset')
	{
	var confirmed
	confirmed=window.confirm("Do You Want To reset The Page ?");
	if(confirmed==true)
	{	
	Form1.reset()
	}		

	}			
	}	




    </script>

    <script type="text/javascript">

        function tabClose() {
            window.parent.closeTab();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None"> Invoices </asp:Label>
                                                </td>
                                                <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <img class="PlusImageCSS" id="Ok" title="Search" onclick="SaveEdit('Ok');" alt="Search"
                                                            src="../Images/s1search02.gif" border="0" name="tbrbtnOk" runat="server" />
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                        <%--<asp:ImageButton ID="imgClose" runat="server" AlternateText="Close" ToolTip="Close"
                                                        ImageUrl="../Images/s2close01.gif"></asp:ImageButton>&nbsp;--%>
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(587 ,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%">
                                <table width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="left" colspan="1" rowspan="1">
                                            <cc1:CollapsiblePanel ID="cpnlError" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                Width="100%" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                ExpandImage="../Images/ToggleDown.gif" Text="Error Message" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                Height="47px" Visible="False">
                                                <table id="Table5" bordercolor="lightgrey" cellspacing="0" cellpadding="0" width="100%"
                                                    border="0">
                                                    <tr>
                                                        <td colspan="0" rowspan="0">
                                                            <asp:Image ID="Image1" runat="server" Width="16px" ImageUrl="../Images/warning.gif"
                                                                Height="16px"></asp:Image>
                                                        </td>
                                                        <td colspan="0" rowspan="0">
                                                            &nbsp;
                                                            <asp:ListBox ID="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" Font-Size="XX-Small"
                                                                Font-Names="Verdana" ForeColor="Red" Width="727px" Height="32px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                            <cc1:CollapsiblePanel ID="CpnlCallDetails" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" Height="40px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                TitleClickable="True" TitleBackColor="Transparent" Text="Call Summary" ExpandImage="../Images/ToggleDown.gif"
                                                CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                <table id="tblCallDetailsOptions" bordercolor="#d7d7d7" cellspacing="1" cellpadding="4"
                                                    border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCompany2" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="100px" Height="12px" runat="server"> Select Company</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCompany" Font-Size="XX-Small" Width="144px" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblInvoice" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                runat="server">Select Invoice Number</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlInvoiceNo" Font-Size="XX-Small" Width="128px" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table1" width="100%">
                                    <tr>
                                        <td valign="top" align="left" colspan="1" rowspan="1">
                                            <cc1:CollapsiblePanel ID="cpnlReport" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" Height="65px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                TitleClickable="True" TitleBackColor="Transparent" Text="CALL SUMMARY" ExpandImage="../Images/ToggleDown.gif"
                                                CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                <CR:CrystalReportViewer ID="crvReport" runat="server" HasToggleGroupTreeButton="False"
                                                    AutoDataBind="True" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False"
                                                    EnableParameterPrompt="False" EnableTheming="false" HasCrystalLogo="False" HasRefreshButton="True"
                                                    HasSearchButton="False" HasViewList="False" HasZoomFactorList="False" ClientTarget="Uplevel"
                                                    BestFitPage="true"></CR:CrystalReportViewer>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthiddenImage2" />
                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                        <input type="hidden" value="<%=mstrCompanyID%>" name="txtCompanyID" />
                        <input type="hidden" name="txtHiddenStatus" /><input type="hidden" name="HIDSCRIDName"
                            runat="server" id="HIDSCRID" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
