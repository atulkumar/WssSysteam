<%@ page language="VB" autoeventwireup="false" inherits="Reports_TimeRegistrationDS, App_Web_sbqj3uth" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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

    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
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

    <script type="text/javascript">

        function tabClose() {
            window.parent.closeTab();
        }
        
         function onEnd() {
           var x = document.getElementById('cpnlRS_collapsible').cells[0].colSpan = "1";
              var y = document.getElementById('cpnlReport_collapsible').cells[0].colSpan = "1";
             
        }	
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">Time Registration </asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgOK" runat="server" ToolTip="Search" ImageUrl="../Images/s1search02.gif">
                                                        </asp:ImageButton>&nbsp;
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(548 ,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0">
                                <tr>
                                    <td>
                                        <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                            Width="100%" Height="24px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                            TitleClickable="True" TitleBackColor="Transparent" Text="Time Registration" ExpandImage="../Images/ToggleDown.gif"
                                            CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                            <table id="Table1" bordercolor="#d7d7d7" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCompany" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                            Width="72px" Height="8px" runat="server"> Company</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCompany" Font-Size="XX-Small" Width="156px" runat="server" Height="18px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEmployee" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                            Width="80px" runat="server">Task Owner</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEmployee" Font-Size="XX-Small" Width="156px" Height="18px"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFromDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                            Width="64px" runat="server"> From Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <ION:Customcalendar ID="dtFromDate" runat="server" Height="16px" Width="148px" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                            Width="80px" runat="server">To Date</asp:Label>
                                                    </td>
                                                    <td style="z-index: 2">
                                                        <ION:Customcalendar ID="dtToDate" runat="server" Height="16px" Width="148px" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </cc1:CollapsiblePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                            Width="100%" Height="65px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                            TitleClickable="True" TitleBackColor="Transparent" Text="Time Registration" ExpandImage="../Images/ToggleDown.gif"
                                            CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                            <table id="Table3" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%">
                                                <tr>
                                                    <td valign="top" align="left" colspan="1" rowspan="1">
                                                        <CR:CrystalReportViewer ID="crvReport" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="False"
                                                            EnableParameterPrompt="False" DisplayGroupTree="False" HasCrystalLogo="False"
                                                            HasRefreshButton="True" HasToggleGroupTreeButton="False" HasZoomFactorList="False"
                                                            PrintMode="ActiveX" ReuseParameterValuesOnRefresh="True" Height="530px" EnableTheming="true"
                                                            BestFitPage="False" Width="800px" HasPageNavigationButtons="False" SeparatePages="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </cc1:CollapsiblePanel>
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
                        <asp:ListBox ID="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" Font-Size="XX-Small"
                            Font-Names="Verdana" ForeColor="Red" Width="100px" Height="32px" Visible="false">
                        </asp:ListBox>
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                        <input type="hidden" name="txthiddenProject" />
                        <input type="hidden" name="txthiddenOwner" />
                        <input type="hidden" name="txthiddenAssignBy" />
                        <input type="hidden" name="txthiddenCallNos" />
                        <input type="hidden" name="txthiddenCallNos2" />
                        <input type="hidden" name="txthiddenEmployee"><input id="HIDSCRID" type="hidden"
                            name="HIDSCRIDName" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
