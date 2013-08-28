<%@ page language="VB" autoeventwireup="false" inherits="Reports_TasksDelayed, App_Web_wm48jtpa" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CReports</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>

    <script type="text/javascript" src="../Images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../DateControl/ION.js"></script>

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
 
function ShowImg()
{
document.getElementById('imgAjax').src="../images/ajax1.gif";
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
var url= '../AJAX Server/AjaxInfo.aspx?DDL='+ DDL +'&Type=DailyAction&ProjectID='+ ProjectID +'&CompID='+CompID+'&Rnd='+Math.random();

xmlHttp = GetXmlHttpObject(stateChangeHandler); 
  
xmlHttp_Get(xmlHttp, url); 
}


function stateChangeHandler() 
{ 	

if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
{ 
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
        
  //A Function to improve design i.e delete the extra cell of table

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
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0" id="Table3">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0" id="Table4">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0" id="Table7">
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0" id="Table8">
                                            <tr>
                                                <td style="width: 20%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblHead" runat="server" Width="192px" ForeColor="White" Font-Bold="True"
                                                        Font-Names="Verdana" Font-Size="X-Small" BorderStyle="None" BorderWidth="2px">Daily Actions </asp:Label>
                                                </td>
                                                <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgOK" runat="server" ImageUrl="../Images/s1search02.gif" ToolTip="Search">
                                                        </asp:ImageButton>
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" /></asp:ImageButton>
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(794 ,'../');"
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
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <cc1:CollapsiblePanel ID="cpnlRS" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                ExpandImage="../Images/ToggleDown.gif" Text="Call Summary" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                Height="24px">
                                                <table id="Table1" style="border-color: #d7d7d7" cellspacing="1" cellpadding="1"
                                                    border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCompany" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                runat="server"> Company</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCompany" Font-Size="XX-Small" Width="156px" runat="server"
                                                                Height="18px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label1" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                runat="server">GroupBy</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlgroup" Font-Size="XX-Small" Width="156px" runat="server"
                                                                Height="18px">
                                                                <asp:ListItem Value="0">USER</asp:ListItem>
                                                                <asp:ListItem Value="1">PRIORITY</asp:ListItem>
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
                                                                Width="48px" runat="server">To Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <ION:Customcalendar ID="dtToDate" runat="server" Height="16px" Width="148px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                            <cc1:CollapsiblePanel ID="cpnlReport" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                ExpandImage="../Images/ToggleDown.gif" Text="Call Summary" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                Height="65px">
                                                <%-- <div style="overflow: auto; width: 99.7%; height: 468px">--%>
                                                <table cellspacing="0" cellpadding="0" align="left">
                                                    <tr>
                                                        <td valign="top" align="left" colspan="1" rowspan="1">
                                                            <CR:CrystalReportViewer ID="crvReport" runat="server" HasToggleGroupTreeButton="False"
                                                                AutoDataBind="True" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False"
                                                                EnableParameterPrompt="False" EnableTheming="false" HasCrystalLogo="False" HasRefreshButton="True"
                                                                HasSearchButton="False" HasViewList="False" HasZoomFactorList="False" ClientTarget="Uplevel"
                                                                Height="500px" BestFitPage="False" Width="900px"></CR:CrystalReportViewer>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--</div>--%>
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
                        <input type="hidden" name="txthiddenEmployee" />
                        <input type="hidden" name="HIDSCRIDName" runat="server" id="HIDSCRID" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
