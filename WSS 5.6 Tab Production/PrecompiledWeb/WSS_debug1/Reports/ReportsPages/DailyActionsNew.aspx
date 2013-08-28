<%@ page language="VB" autoeventwireup="false" inherits="Reports_ReportsPages_DailyActionsNew, App_Web_hojpnwda" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../../SupportCenter/calendar/DateSelector.ascx" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>DailyActionsNew</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../../images/Js/JSValidation.js"></script>
		<script language="javascript" src="../../DateControl/ION.js"></script>
		<LINK href="../../SupportCenter/calend&#9;&#9;ar/popcalendar.css" type="text/css" rel="stylesheet">
		<LINK href="../../images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">


/**********************AJAX for Project****************************************/

var gtype;
var xmlHttp; 
var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
//netscape, safari, mozilla behave the same??? 
var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 


var gPID;
var gEID;
var gDDL;
 
function ShowImg()
{
document.getElementById('imgAjax').src="../../images/ajax1.gif";
document.getElementById('imgAjax').style.display='inline';
}

function DDLChange(DDL,CMID,PID,EID )
{
//alert(PID);
//document.getElementById('txtHIDAgreement')='';
gPID=PID;//Project
gEID=EID;//Employee
gDDL=DDL;
//gCNID=CNID;//Call Numbers
//gCNID2=CNID2;//Call Numbers
xmlHttp=null;
var ddlCompany=document.getElementById(CMID);
var ddlProject=document.getElementById(PID);
var CompID=ddlCompany.options(ddlCompany.selectedIndex).value;
var ddlEmployee=document.getElementById(EID);
//var ddlCallFrom=document.getElementById(CNID);
	var ProjectID=ddlProject.options(ddlProject.selectedIndex).value;
 
if ( CompID==0)
{
ddlProject.disabled=true;
}
else
{
ddlProject.disabled=false;
}
var url= '../../AJAX Server/AjaxInfo.aspx?DDL='+ DDL +'&Type=DailyAction&ProjectID='+ ProjectID +'&CompID='+CompID+'&Rnd='+Math.random();

xmlHttp = GetXmlHttpObject(stateChangeHandler); 
  
xmlHttp_Get(xmlHttp, url); 
}


function stateChangeHandler() 
{ 	

if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
{ 
document.getElementById('imgAjax').style.display='none'
document.getElementById(gEID).options.length=0;
//document.getElementById(gCNID).options.length=0;
//document.getElementById(gCNID2).options.length=0;
var objNewOption;
if ( gDDL == 0 )
{
document.getElementById(gPID).options.length=0;
objNewOption = document.createElement("OPTION");
document.getElementById(gPID).options.add(objNewOption);
objNewOption.value = '0';
objNewOption.innerText ='--ALL--';	
}
objNewOption = document.createElement("OPTION");
document.getElementById(gEID).options.add(objNewOption);
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


case 0://Owner
{
for (var inti=0; inti<item.length; inti++)
{
var objNewOption = document.createElement("OPTION");
document.getElementById(gEID).options.add(objNewOption);
objNewOption.value = item[inti].getAttribute("COL0");
objNewOption.innerText = item[inti].getAttribute("COL1");
RoleDataName=RoleDataName+item[inti].getAttribute("COL1") + '^';
RoleDataID=RoleDataID+item[inti].getAttribute("COL0") + '^';													
}
document.Form1.txthiddenOwner.value= RoleDataName + '~' + RoleDataID ;
break;
}//case 0


case 1://Project
{	
for (var inti=0; inti<item.length; inti++)
{
var objNewOption = document.createElement("OPTION");
document.getElementById(gPID).options.add(objNewOption);
objNewOption.value = item[inti].getAttribute("COL0");
objNewOption.innerText = item[inti].getAttribute("COL1");
RoleDataName=RoleDataName+item[inti].getAttribute("COL1") + '^';
RoleDataID=RoleDataID+item[inti].getAttribute("COL0") + '^';
}
document.Form1.txthiddenProject.value= RoleDataName + '~' + RoleDataID ;
break;
}//case 3										
}//switch
} //for loop
}//if

}//
else
{
document.getElementById('imgAjax').src="../../images/ajax1.gif";
document.getElementById('imgAjax').style.display='inline';
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


if (varImgValue=='Attach')
{
if (document.Form1.txthiddenCallNo.value==0)
{
alert("Please select a call number");
}
else
{
//location.href="Call_Detail.aspx?ID=0";
document.Form1.txthiddenImage.value=varImgValue;
Form1.submit(); 
}
//document.Form1.txthiddenImage.value=varImgValue;
//Form1.submit(); 
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
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td vAlign="top">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<div align="right"><IMG height="2" src="~/images/top_right_line.gif" width="96"></div>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td >&nbsp;</td>
											<td width="50"><IMG height="20" src="../../images/top_right.gif" width="50"></td>
											<td width="21"><A href="#"><IMG height="20" src="../../images/bt_min.gif" width="21" border="0"></A></td>
											<td width="21"><A href="#"><IMG height="20" src="../../images/bt_max.gif" width="21" border="0"></A></td>
											<td width="19"><A href="#"><IMG onclick="CloseWSS()" height="20" src="../../images/bt_clo.gif" width="19" border="0"></A></td>
											<td width="6"><IMG height="20" src="../../images/bt_space.gif" width="6"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr width="100%">
											<td background="../../images/top_nav_back.gif" height="67">
												<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
													<TR>
														<TD vAlign="middle"><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../../images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../../images/Right005.gif"
																name="ingShow">
															<asp:label id="lblHead" runat="server" Width="192px" ForeColor="White" Font-Bold="True" Font-Names="Verdana"
																Font-Size="X-Small" BorderStyle="None" BorderWidth="2px">Daily Actions </asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:imagebutton id="imgOK" runat="server" ImageUrl="../../images/s1search02.gif" ToolTip="Search"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgClose" runat="server" ImageUrl="../../images/s2close01.gif" AlternateText="Close"
																ToolTip="Close"></asp:imagebutton></TD>
														<TD></TD>
														<td><IMG id="imgAjax" title="ajax" height="24" src="../../images/divider.gif" width="24">&nbsp;
														</td>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../../images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(737 ,'../../');" alt="E"
													src="../../images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../../icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;&nbsp;</td>
										</tr>
									</table>
									</td>
									        </tr>
							<tr>
								<td height="10">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../images/main_line.gif" height="10"><IMG height="10" src="../../images/main_line.gif" width="6"></td>
											<td width="7" height="10"><IMG height="10" src="../../images/main_line01.gif" width="7"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../images/main_line02.gif" height="2"><IMG height="2" src="../../images/main_line02.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../../images/main_line03.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px" DESIGNTIMEDRAGDROP="79">
										<TABLE id="Table16" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<TR>
												<TD vAlign="top" colSpan="1">
													<!--  **********************************************************************-->
													<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
														<table width="100%" border="0">
															<TBODY>
																<tr>
																	<td colSpan="2"><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
																			BorderColor="Indigo" Draggable="False" CollapseImage="../../images/ToggleUp.gif" ExpandImage="../../images/ToggleDown.gif"
																			Text="Error Message" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
																			TitleCSS="test" Height="47px" Visible="False">
																			<TABLE id="Table5" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
																				border="0">
																				<TR>
																					<TD colSpan="0" rowSpan="0">
																						<asp:Image id="Image1" runat="server" Width="16px" ImageUrl="../../images/warning.gif" Height="16px"></asp:Image></TD>
																					<TD colSpan="0" rowSpan="0">&nbsp;
																						<asp:ListBox id="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" Font-Size="XX-Small"
																							Font-Names="Verdana" ForeColor="Red" Width="697px" Height="32px"></asp:ListBox></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlRS" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
																			Draggable="False" CollapseImage="../../images/ToggleUp.gif" ExpandImage="../../images/ToggleDown.gif" Text="Call Summary"
																			TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Height="24px">
																			<TABLE id="Table1" borderColor="#d7d7d7" cellSpacing="1" cellPadding="1" width="479" border="1"
																				DESIGNTIMEDRAGDROP="156">
																				<TR>
																					<TD>
																						<TABLE id="Table2" style="HEIGHT: 27px" cellSpacing="1" cellPadding="1" width="100%" runat="server">
																							<TR>
																								<TD style="WIDTH: 58px">
																									<asp:label id="lblCompany" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" Runat="server"> Company</asp:label></TD>
																								<TD style="WIDTH: 81px">
																									<asp:dropdownlist id="ddlCompany" Font-Size="XX-Small" Width="144px" Runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
																								<TD style="WIDTH: 1px">
																									<asp:label id="Label6" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" Runat="server">SubCategory</asp:label></TD>
																								<TD style="WIDTH: 146px">
																									<asp:dropdownlist id="ddlProject" Font-Size="XX-Small" Width="137px" Runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
																								<TD id="tdweekdateTitle" style="WIDTH: 71px" noWrap runat="server">
																									<asp:label id="lblEmployee" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" Width="72px"
																										Runat="server">Task Owner</asp:label></TD>
																								<TD id="tdWeekDate" runat="server" stylew="WIDTH: 46px">
																									<asp:dropdownlist id="ddlEmployee" Font-Size="XX-Small" Width="152px" Height="16px" Runat="server"></asp:dropdownlist></TD>
																							</TR>
																						</TABLE>
																					</TD>
																				</TR>
																				<TR>
																					<TD>
																						<TABLE id="Table3" style="WIDTH: 416px; HEIGHT: 45px" cellSpacing="1" cellPadding="1" width="416">
																							<TR>
																								<TD style="WIDTH: 48px">
																									<asp:label id="lblFromDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" Width="64px"
																										Runat="server"> From Date</asp:label></TD>
																								<TD style="WIDTH: 137px">
																									<%--<SCONTROLS:DATESELECTOR id="dtFromDate" runat="server" Text="Start Date:"></SCONTROLS:DATESELECTOR>--%>
                            <ION:Customcalendar ID="dtFromDate" runat="server" />																						
																									</TD>
																								<TD style="WIDTH: 28px">
																									<asp:label id="lblToDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" Width="48px"
																										Runat="server">To Date</asp:label></TD>
																								<TD>
																									<%--<SCONTROLS:DATESELECTOR id="dtToDate" runat="server" Text="Start Date:"></SCONTROLS:DATESELECTOR>--%>
			   <ION:Customcalendar ID="dtToDate" runat="server" />																								
																									</TD>
																							</TR>
																						</TABLE>
																					</TD>
																				</TR>
																				<TR>
																					<TD id="tdLevels" runat="server"></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlReport" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
																			BorderColor="Indigo" Draggable="False" CollapseImage="../../images/ToggleUp.gif" ExpandImage="../../images/ToggleDown.gif"
																			Text="Call Summary" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
																			Height="65px">
																			<%--<CR:CRYSTALREPORTVIEWER id="crvReport" runat="server" Width="350px" Height="50px" ReuseParameterValuesOnRefresh="True"
																				ShowAllPageIds="True" PrintMode="ActiveX" DisplayGroupTree="False" HasCrystalLogo="False" AutoDataBind="true"
																				HasToggleGroupTreeButton="False" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False" has
																				hassearchbutton="false" HasPageNavigationButtons="False"></CR:CRYSTALREPORTVIEWER>--%>
					 
<cr:crystalreportviewer id="crvReport" runat="server" HasToggleGroupTreeButton="False" autodatabind="True" displaygrouptree="False" enabledatabaselogonprompt="False" enableparameterprompt="False" enabletheming="false" hascrystallogo="False" hasrefreshbutton="True" hassearchbutton="False" hasviewlist="False" haszoomfactorlist="False" ClientTarget="Uplevel" BestFitPage="true"></cr:crystalreportviewer>															
																		</cc1:collapsiblepanel></td>
																</tr>
															</TBODY>
														</table>
													</DIV>
												</TD>
												<td vAlign="top" width="12" background="../../images/main_line04.gif"><IMG height="1" src="../../images/main_line04.gif" width="12"></td>
											</TR>
										</TABLE>
									</DIV>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../images/main_line06.gif" height="2"><IMG height="2" src="../../images/main_line06.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../../images/main_line05.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../images/bottom_back.gif">&nbsp;</td>
											<td width="66"><IMG height="31" src="../../images/bottom_right.gif" width="66"></td>
										</tr>
									</table>
								</td>
							</tr>						
					</td>
				</tr>
			</table>
			<INPUT type="hidden" name="txthiddenImage"> <INPUT type=hidden 
value="<%=mstrCallNumber%>" name=txthiddenCallNo> <input type="hidden" name="txthiddenProject">
			<input type="hidden" name="txthiddenOwner"> <input type="hidden" name="txthiddenAssignBy">
			<input type="hidden" name="txthiddenCallNos"> <input type="hidden" name="txthiddenCallNos2">
			<input type="hidden" name="txthiddenEmployee"><input type="hidden" name="HIDSCRIDName" runat="server" id="HIDSCRID">
		</form>
	</body>
</html>
