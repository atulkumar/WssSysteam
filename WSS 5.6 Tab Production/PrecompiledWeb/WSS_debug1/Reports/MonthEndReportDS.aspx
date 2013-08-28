<%@ page language="VB" autoeventwireup="false" inherits="Reports_MonthEndReportDS, App_Web_wzhf50l2" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

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

    <script src="../images/js/core.js" type="text/javascript"></script>

    <script src="../images/js/events.js" type="text/javascript"></script>

    <script src="../images/js/css.js" type="text/javascript"></script>

    <script src="../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <link href="../SupportCenter/calend&#9;&#9;ar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">

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
function DDLChange(DDL,CMID,PID,EID,CNID,CNID2)
{
//alert(PID);
//document.getElementById('txtHIDAgreement')='';
gPID=PID;//Project
gEID=EID;//Call Owner
gDDL=DDL;
gCNID=CNID;//Call Numbers
gCNID2=CNID2;//Call Numbers
xmlHttp=null;
var ddlCompany=document.getElementById(CMID);
var CompID=ddlCompany.options(ddlCompany.selectedIndex).value;
var ddlProject=document.getElementById(PID);
var ddlEmployee=document.getElementById(EID);
var ddlCallFrom=document.getElementById(CNID);

var ProjectID=ddlProject.options(ddlProject.selectedIndex).value;

if ( CompID==0)
{
ddlProject.disabled=true;
ddlEmployee.disabled=true;
ddlCallFrom.disabled=true;
}
else
{
ddlProject.disabled=false;
ddlEmployee.disabled=false;
ddlCallFrom.disabled=false;
}
var url= '../AJAX Server/AjaxInfo.aspx?DDL='+ DDL +'&Type=CallDetails2&ProjectID='+ ProjectID +'&CompID='+CompID+'&Rnd='+Math.random();
xmlHttp = GetXmlHttpObject(stateChangeHandler);    
xmlHttp_Get(xmlHttp, url); 
}


function stateChangeHandler() 
{ 	

if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
{ 
document.getElementById('imgAjax').style.display='none'
document.getElementById(gEID).options.length=0;
document.getElementById(gCNID).options.length=0;
document.getElementById(gCNID2).options.length=0;
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

objNewOption = document.createElement("OPTION");
document.getElementById(gCNID).options.add(objNewOption);
objNewOption.value = '0';
objNewOption.innerText ='--Select--';



objNewOption = document.createElement("OPTION");
document.getElementById(gCNID2).options.add(objNewOption);
objNewOption.value = '0';
objNewOption.innerText ='--Select--';



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

/* 
case 1://Call Numbers 
{
for (var inti=0; inti<item.length; inti++)
{
var objNewOption = document.createElement("OPTION");
document.getElementById(gCNID).options.add(objNewOption);
objNewOption.value = item[inti].getAttribute("COL0");
objNewOption.innerText = item[inti].getAttribute("COL1");
RoleDataName=RoleDataName+item[inti].getAttribute("COL1") + '^';
RoleDataID=RoleDataID+item[inti].getAttribute("COL0") + '^';													
}
document.Form1.txthiddenCallNos.value= RoleDataName + '~' + RoleDataID ;
var RoleDataName='';
var RoleDataID='';
for (var inti=0; inti<item.length; inti++)
{
var objNewOption = document.createElement("OPTION");
document.getElementById(gCNID2).options.add(objNewOption);
objNewOption.value = item[inti].getAttribute("COL0");
objNewOption.innerText = item[inti].getAttribute("COL1");
RoleDataName=RoleDataName+item[inti].getAttribute("COL1") + '^';
RoleDataID=RoleDataID+item[inti].getAttribute("COL0") + '^';													
}
document.Form1.txthiddenCallNos2.value= RoleDataName + '~' + RoleDataID ;


break;
}//case 1
*/
case 2://Project
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







function KeyImage(a,b,c,d,comp,CallNo)
{							
if (d==0 ) //if comment is clicked

{		
wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+ "'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
}
else if (d==1) //if Attachment is clicked
{

wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+a ,'Attachment',800,450);
}
else if (d==5)//call comment
{
wopen('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID='+ a + '&tbname=C&CallNo='+CallNo+'&comp='+comp  ,'Comment',500,450);
}
else if (d==7)
{

wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo='+a+'&CompID='+comp+'&CallNo='+CallNo ,'Attachment',800,450);
}
else // if Attach form is clicked
{
wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno='+'<%=session("PropCallNumber")%>'+'&tno='+a ,'AttachForms',500,450);							

}

}




function back(count)
	{
	//var count= document.Form1.txtHiddenCount.value;
	window.history.back(-1);
	
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


if (varImgValue=='CloseCall')
{
document.Form1.txthiddenImage.value=varImgValue;
Form1.submit(); 
return false;
}
if (varImgValue=='Attach')
{
//alert(document.Form1.txtCompanyID.value);
if (document.Form1.txthiddenCallNo.value==0)
{
alert("Please select a call number");
}
else
{
window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CallNo='+document.Form1.txthiddenCallNo.value + '&CompID='+document.Form1.txtCompanyID.value  ,'Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');
}

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
return false;		
}	

function Attachment(varImgValue)
{



if (varImgValue=='AttachCall')
{
//alert(document.Form1.txtCompanyID.value);
if (document.Form1.txthiddenCallNo.value==0)
{
alert("Please select a call number");

}
else
{


window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CallNo='+document.Form1.txthiddenCallNo.value + '&CompID='+document.Form1.txtCompanyID.value  ,'Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');
}		
}		

if (varImgValue=='AttachTask')
{
//alert(document.Form1.txtCompanyID.value);
if (document.Form1.txthiddenCallNo.value==0)
{
alert("Please select a call number");

}
else
{


window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=T&CallNo='+document.Form1.txthiddenCallNo.value + '&CompID='+document.Form1.txtCompanyID.value + '&TaskNo='+document.Form1.txthiddenTaskNo.value ,'Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');
}		
}	
return false;

}	
function CheckStatus(chklstId,chkStatus)
{
alert();
alert(chklstId);
if(document.getElementById('chkStatus').checked==true)
{
chkBoxID.enabled=false;
}
else
{
chkBoxID.enabled=true;
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

    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr valign="top">
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr width="100%">
                                                <td background="../Images/top_nav_back.gif" height="47">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td style="width: 30%">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">Month End Report </asp:Label>
                                                            </td>
                                                            <td style="width: 55%; text-align: center;" nowrap="nowrap">
                                                                <center>
                                                                    <asp:ImageButton ID="imgOK" runat="server" ToolTip="Search" ImageUrl="../Images/s1search02.gif">
                                                                    </asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgAttach" AccessKey="A" runat="server" ToolTip="View Attachments"
                                                                        ImageUrl="../Images/ScreenHunter_075.bmp" AlternateText="View Attachments"></asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server" ToolTip="View/Hide Closed Calls"
                                                                        ImageUrl="../Images/CloseCall1.gif" AlternateText="View/Hide Closed Calls"></asp:ImageButton>
                                                                    <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                                    <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                                </center>
                                                            </td>
                                                            <td style="width: 5%">
                                                                <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24">&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                                    <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('258','../');"
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
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td colspan="1" valign="top">
                                                        <!-- **********************************************************************-->
                                                        <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                            Width="99.43%" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                            ExpandImage="../Images/ToggleDown.gif" Text="Month End Report" TitleBackColor="Transparent"
                                                            TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                            Height="168px">
                                                            <table id="Table3" style="width: 744px; height: 120px" bordercolor="#d7d7d7" cellspacing="1"
                                                                cellpadding="3" border="0">
                                                                <tr>
                                                                    <td style="height: 52px">
                                                                        <table id="Table4" style="width: 720px; height: 121px" cellspacing="1" cellpadding="3"
                                                                            width="720" border="0">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td style="width: 58px">
                                                                                        <asp:Label ID="Label7" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            runat="server"> Company</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 51px">
                                                                                    </td>
                                                                                    <td style="width: 131px">
                                                                                        <asp:DropDownList ID="ddlCompany" Font-Size="XX-Small" Width="128px" runat="server"
                                                                                            AutoPostBack="True" Height="18px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 14px">
                                                                                    </td>
                                                                                    <td style="width: 71px">
                                                                                        <asp:Label ID="Label1" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            Width="88px" runat="server">Sub Category</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 83px">
                                                                                        <asp:DropDownList ID="ddlProject" Font-Size="XX-Small" Width="128px" runat="server"
                                                                                            AutoPostBack="True" Height="18px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 84px">
                                                                                        <asp:Label ID="Label8" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            Width="88px" runat="server">Time Reported</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 120px">
                                                                                        <asp:DropDownList ID="ddlActionTime" Font-Size="XX-Small" Width="128px" runat="server"
                                                                                            Height="18px">
                                                                                            <asp:ListItem Value="--ALL--">--ALL--</asp:ListItem>
                                                                                            <asp:ListItem Value="Not Inculde Zero">Not Inculde Zero</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 58px">
                                                                                        <asp:Label ID="Label3" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            Width="80px" runat="server">Action Owner</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 51px">
                                                                                    </td>
                                                                                    <td style="width: 131px">
                                                                                        <asp:DropDownList ID="ddlActionOwner" Font-Size="XX-Small" Width="128px" runat="server"
                                                                                            Height="18px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 14px">
                                                                                    </td>
                                                                                    <td style="width: 71px">
                                                                                        <asp:Label ID="Label4" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            runat="server"> Category</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 83px">
                                                                                        <asp:DropDownList ID="ddlCategory" Font-Size="XX-Small" Width="128px" runat="server"
                                                                                            Height="18px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 71px">
                                                                                        <asp:Label ID="Label2" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            runat="server"> Support Company</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 83px">
                                                                                        <asp:DropDownList ID="ddlSuppComp" Font-Size="XX-Small" Width="128px" runat="server"
                                                                                            AutoPostBack="False" Height="18px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 120px">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 58px">
                                                                                        <asp:Label ID="Label5" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            Width="80px" Height="17px" runat="server">Action From</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 51px">
                                                                                    </td>
                                                                                    <td style="width: 131px">
                                                                                        <ION:Customcalendar ID="dtFromDate" runat="server" Height="16px" Width="130px" />
                                                                                    </td>
                                                                                    <td style="width: 14px">
                                                                                    </td>
                                                                                    <td style="width: 71px">
                                                                                        <asp:Label ID="Label6" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            runat="server">Action To</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 83px">
                                                                                        <ION:Customcalendar ID="dtToDate" runat="server" Height="16px" Width="130px" />
                                                                                    </td>
                                                                                    <td style="width: 71px">
                                                                                        <asp:Label ID="lblcalltype" Font-Size="XX-Small" Font-Names="verdana" Font-Bold="true"
                                                                                            runat="server">Call Type</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 84px">
                                                                                        <asp:DropDownList ID="ddlCallType" Font-Size="XX-Small" Width="128px" runat="server"
                                                                                            AutoPostBack="false" Height="18px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 120px">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="Label9" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                            Width="72px" runat="server"> Call Status</asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 51px">
                                                                                        <asp:CheckBox ID="chkStatus" runat="server" Text="ALL" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                            Font-Bold="True" Width="56px" Checked="True" AutoPostBack="True"></asp:CheckBox>
                                                                                    </td>
                                                                                    <td colspan="8">
                                                                                        <div style="overflow: auto; width: 600px; height: 50px">
                                                                                            <table>
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:CheckBoxList ID="chkLstStatus" runat="server" Height="16px" Font-Size="XX-Small"
                                                                                                                Font-Names="Verdana" Font-Bold="True" Enabled="False" RepeatColumns="7" RepeatDirection="Horizontal">
                                                                                                            </asp:CheckBoxList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </cc1:CollapsiblePanel>
                                                        <!-- **********************************************************************-->
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                            CollapseImage="../images/ToggleUp.gif" Draggable="False" ExpandImage="../images/ToggleDown.gif"
                                                            PanelCSS="panel" Text="Month End Report" TitleBackColor="Transparent" TitleClickable="True"
                                                            TitleCSS="test" TitleForeColor="black" Visible="true" Width="100%">
                                                            <%--<div style="overflow: auto; width: 98.7%; height: 450px">--%>
                                                            <table align="left">
                                                                <tr>
                                                                    <td valign="top" align="left" colspan="1" rowspan="1">
                                                                        <CR:CrystalReportViewer ID="crvReport" runat="server" HasToggleGroupTreeButton="False"
                                                                            AutoDataBind="True" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False"
                                                                            EnableParameterPrompt="False" EnableTheming="false" HasCrystalLogo="False" HasRefreshButton="True"
                                                                            HasSearchButton="False" HasViewList="False" HasZoomFactorList="False" ClientTarget="Uplevel"
                                                                            BestFitPage="false" Height="450px" Width="930px"></CR:CrystalReportViewer>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <%-- </div>--%>
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
                                    <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Font-Names="Verdana"
                                        Font-Size="XX-Small" ForeColor="Red" Width="100px" Visible="false"></asp:ListBox>
                                    <input type="hidden" name="txthiddenImage2">
                                    <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                                    <input type="hidden" value="<%=mstrTaskNumber%>" name="txthiddenTaskNo" />
                                    <input type="hidden" value="<%=mstrCompanyID%>" name="txtCompanyID" />
                                    <input type="hidden" name="txthiddenImage" />
                                    <input type="hidden" name=" txtHiddenCount" />
                                    <input type="hidden" name="txthiddenProject" />
                                    <input type="hidden" name="txthiddenOwner" />
                                    <input type="hidden" name="txthiddenAssignBy" />
                                    <input type="hidden" name="txthiddenCallNos" />
                                    <input type="hidden" name="txthiddenCallNos2" />
                                    <input type="hidden" name="txthiddenEmployee" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
