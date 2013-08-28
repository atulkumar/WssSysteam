<%@ page language="VB" autoeventwireup="false" inherits="Reports_TimeRegistration, App_Web_fzfabjfx" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>CReports</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../Images/Js/JSValidation.js"></script>

    <script language="javascript" src="../DateControl/ION.js"></script>

    <link href="../SupportCenter/calend&#9;&#9;ar/popcalendar.css" type="text/css" rel="stylesheet">
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">

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
var gCNID;
var gCNID2;

function ShowImg()
{
document.getElementById('imgAjax').src="../images/ajax1.gif";
document.getElementById('imgAjax').style.display='inline';
}

function DDLChange(DDL,CMID,EID )
{
//alert(PID);
//document.getElementById('txtHIDAgreement')='';
//gPID=PID;//Project
gEID=EID;//Call Owner
gDDL=DDL;
//gCNID=CNID;//Call Numbers
//gCNID2=CNID2;//Call Numbers
xmlHttp=null;
var ddlCompany=document.getElementById(CMID);
var CompID=ddlCompany.options(ddlCompany.selectedIndex).value;
var ddlEmployee=document.getElementById(EID);
//var ddlCallFrom=document.getElementById(CNID);

//var ProjectID=ddlProject.options(ddlProject.selectedIndex).value;

if ( CompID==0)
{
ddlEmployee.disabled=true;
}
else
{
ddlEmployee.disabled=false;
}
var url= '../AJAX Server/AjaxInfo.aspx?DDL='+ DDL +'&Type=TimeRgs&CompID='+CompID+'&Rnd='+Math.random();
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
objNewOption.value = '0';
objNewOption.innerText ='--ALL--';



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

case 1://Call Numbers 
{
//for (var inti=0; inti<item.length; inti++)


break;
}//case 1

case 2://Project
{	

break;
}//case 3										
}//switch
} //for loop
}//if

}//

else
{
document.getElementById('imgAjax').src="../images/ajax1.gif";
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

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <div align="right">
                                <img height="2" src="../Images/top_right_line.gif" width="96"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../Images/top_left_back.gif">
                                        &nbsp;
                                    </td>
                                    <td width="50">
                                        <img height="20" src="../Images/top_right.gif" width="50">
                                    </td>
                                    <td width="21">
                                        <a href="#">
                                            <img height="20" src="../Images/bt_min.gif" width="21" border="0"></a>
                                    </td>
                                    <td width="21">
                                        <a href="#">
                                            <img height="20" src="../Images/bt_max.gif" width="21" border="0"></a>
                                    </td>
                                    <td width="19">
                                        <a href="#">
                                            <img onclick="CloseWSS()" height="20" src="../Images/bt_clo.gif" width="19" border="0"></a>
                                    </td>
                                    <td width="6">
                                        <img height="20" src="../Images/bt_space.gif" width="6">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../Images/top_nav_back.gif" height="67">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td valign="middle">
                                                    <img class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
                                                        name="imgHide">
                                                    <img class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
                                                        name="ingShow">
                                                    <asp:Label ID="lblHead" runat="server" BorderWidth="2px" BorderStyle="None" Font-Size="X-Small"
                                                        Font-Names="Verdana" Font-Bold="True" ForeColor="White" Width="192px">Time Registration </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgOK" runat="server" ToolTip="Search" ImageUrl="../Images/s1search02.gif">
                                                    </asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgClose" runat="server" ToolTip="Close" ImageUrl="../Images/s2close01.gif"
                                                        AlternateText="Close"></asp:ImageButton>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../Images/top_nav_back01.gif" height="67">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(548 ,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <tr>
                                <td height="10">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line.gif" height="10">
                                                <img height="10" src="../Images/main_line.gif" width="6">
                                            </td>
                                            <td width="7" height="10">
                                                <img height="10" src="../Images/main_line01.gif" width="7">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="2">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line02.gif" height="2">
                                                <img height="2" src="../Images/main_line02.gif" width="2">
                                            </td>
                                            <td width="12" height="2">
                                                <img height="2" src="../Images/main_line03.gif" width="12">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: auto; width: 100%; height: 581px">
                                        <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                            border="0">
                                            <tr>
                                                <td valign="top" colspan="1">
                                                    <!--  **********************************************************************-->
                                                    <div style="overflow: auto; width: 100%; height: 581px">
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <cc1:CollapsiblePanel ID="cpnlError" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                                        Width="100%" Visible="False" Height="47px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                        TitleClickable="True" TitleBackColor="Transparent" Text="Error Message" ExpandImage="../Images/ToggleDown.gif"
                                                                        CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
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
                                                                                        Font-Names="Verdana" ForeColor="Red" Width="697px" Height="32px"></asp:ListBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </cc1:CollapsiblePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                                        Width="100%" Height="24px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                        TitleClickable="True" TitleBackColor="Transparent" Text="Call Summary" ExpandImage="../Images/ToggleDown.gif"
                                                                        CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                                        <table id="Table1" bordercolor="#d7d7d7" cellspacing="1" cellpadding="1" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblCompany" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                        Width="72px" Height="8px" runat="server"> Company</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlCompany" Font-Size="XX-Small" Width="156px" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblEmployee" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                        Width="80px" runat="server">Task Owner</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlEmployee" Font-Size="XX-Small" Width="156px" Height="16px"
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblDateWeek" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                        Width="96px" runat="server">Week Start Date</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <SControls:DateSelector ID="dtWeekDate" runat="server" Text="Start Date:"></SControls:DateSelector>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblFromDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                        Width="64px" runat="server"> From Date</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <%--<SCONTROLS:DATESELECTOR id="dtFromDate" runat="server" Width="154px" Text="Start Date:"></SCONTROLS:DATESELECTOR>--%>
                                                                                    <ION:Customcalendar ID="dtFromDate" runat="server" Height="19px" Width="148px" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblToDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                        Width="80px" runat="server">To Date</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <%--<SCONTROLS:DATESELECTOR id="dtToDate" runat="server" Width="154px" Text="Start Date:"></SCONTROLS:DATESELECTOR>--%>
                                                                                    <ION:Customcalendar ID="dtToDate" runat="server" Height="19px" Width="148px" />
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </cc1:CollapsiblePanel>
                                                                    <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                                        Width="100%" Height="65px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                        TitleClickable="True" TitleBackColor="Transparent" Text="Call Summary" ExpandImage="../Images/ToggleDown.gif"
                                                                        CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                                        <div style="overflow: auto; width: 99.7%; height: 464px">
                                                                            <table align="left">
                                                                                <tr>
                                                                                    <td valign="top" align="left" colspan="1" rowspan="1">
                                                                                        <%--<CR:CRYSTALREPORTVIEWER id="crvReport" runat="server" Width="350px" Height="50px" ReuseParameterValuesOnRefresh="True" ShowAllPageIds="True" PrintMode="ActiveX" DisplayGroupTree="False" HasCrystalLogo="False" AutoDataBind="true" HasToggleGroupTreeButton="False" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False" SeparatePages="False"	has hassearchbutton="false"></CR:CRYSTALREPORTVIEWER>--%>
                                                                                        <CR:CrystalReportViewer ID="crvReport" runat="server" HasToggleGroupTreeButton="False"
                                                                                            AutoDataBind="True" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False"
                                                                                            EnableParameterPrompt="False" EnableTheming="false" HasCrystalLogo="False" HasRefreshButton="True"
                                                                                            HasSearchButton="False" HasViewList="False" HasZoomFactorList="False" ClientTarget="Uplevel"
                                                                                            BestFitPage="true"></CR:CrystalReportViewer>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </cc1:CollapsiblePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td valign="top" width="12" background="../Images/main_line04.gif">
                                                    <img height="1" src="../Images/main_line04.gif" width="12">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td height="2">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line06.gif" height="2">
                                                <img height="2" src="../Images/main_line06.gif" width="2">
                                            </td>
                                            <td width="12" height="2">
                                                <img height="2" src="../Images/main_line05.gif" width="12">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/bottom_back.gif">
                                                &nbsp;
                                            </td>
                                            <td width="66">
                                                <img height="31" src="../Images/bottom_right.gif" width="66">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                </table>
                <asp:Panel ID="pnlMsg" runat="server">
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <input type="hidden" name="txthiddenImage">
            <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo">
            <input type="hidden" name="txthiddenProject">
            <input type="hidden" name="txthiddenOwner">
            <input type="hidden" name="txthiddenAssignBy">
            <input type="hidden" name="txthiddenCallNos">
            <input type="hidden" name="txthiddenCallNos2">
            <input type="hidden" name="txthiddenEmployee"><input id="HIDSCRID" type="hidden"
                name="HIDSCRIDName" runat="server">
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
