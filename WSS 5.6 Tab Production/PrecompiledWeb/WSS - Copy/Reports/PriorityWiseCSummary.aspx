<%@ page language="VB" autoeventwireup="false" inherits="Reports_PriorityWiseCSummary, App_Web_wm48jtpa" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
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
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../images/js/core.js" type="text/javascript"></script>

    <script src="../images/js/events.js" type="text/javascript"></script>

    <script src="../images/js/css.js" type="text/javascript"></script>

    <script src="../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

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
var ProjectID=ddlProject.options(ddlProject.selectedIndex).value;

//if ( CompID==0)
//{
//ddlProject.disabled=true;
//}
//else
//{
//ddlProject.disabled=false;
//}
var url= '../AJAX Server/AjaxInfo.aspx?DDL='+ DDL +'&Type=CallDetails&ProjectID='+ ProjectID +'&CompID='+CompID+'&Rnd='+Math.random();
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
objNewOption.innerText ='--ALL--';



objNewOption = document.createElement("OPTION");
document.getElementById(gCNID2).options.add(objNewOption);
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
                                                <td style="width: 20%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblHead" runat="server" CssClass="TitleLabel"
                                                        BorderStyle="None">Call Detail </asp:Label>
                                                </td>
                                                <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgOK" runat="server" ImageUrl="../Images/s1search02.gif" ToolTip="Search">
                                                        </asp:ImageButton>&nbsp;<asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server"
                                                            ImageUrl="../Images/CloseCall1.gif" ToolTip="View/Hide Closed Calls" AlternateText="View/Hide Closed Calls">
                                                        </asp:ImageButton>&nbsp;<asp:ImageButton ID="imgAttach" AccessKey="A" runat="server"
                                                            ImageUrl="../Images/ScreenHunter_075.bmp" ToolTip="View Attachments" AlternateText="View Attachments">
                                                        </asp:ImageButton>
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                        <img class="PlusImageCSS" id="Ok" onmouseover="FP_swapImg(1,0,/*id*/'Ok',/*url*/'../Images/s1ok02_s.gif')"
                                                            title="Find" onclick="SaveEdit('Ok');" onmouseout="FP_swapImg(0,0,/*id*/'Ok',/*url*/'../Images/s1ok02.gif')"
                                                            alt="Find" src="../Images/s1search02.gif" width="0" border="0" name="tbrbtnOk" />
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img width="24" height="24" id="imgAjax" title="ajax" src="../images/divider.gif">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(792 ,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td valign="top" align="left" colspan="1" rowspan="1">
                                                <cc1:CollapsiblePanel ID="CpnlCallDetails" runat="server" Width="100%" BorderStyle="Solid"
                                                    BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                    ExpandImage="../Images/ToggleDown.gif" Text="Call Summary" TitleBackColor="Transparent"
                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                    Height="40px" Visible="false">
                                                    <table id="tblCallDetailsOptions" style="border-color: #d7d7d7" border="1">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCompany2" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                    Width="100px" Height="12px" runat="server"> Select Company</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCompany2" Font-Size="XX-Small" Width="144px" runat="server"
                                                                    AutoPostBack="True" Height="18px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCallNo" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                    runat="server">Select Call Number</asp:Label>
                                                            </td>
                                                            <td valign="top" colspan="1" rowspan="1">
                                                                <asp:DropDownList ID="ddlCallNo" Font-Size="XX-Small" Width="128px" runat="server"
                                                                    AutoPostBack="false" Height="18px">
                                                                </asp:DropDownList>
                                                                <uc1:CustomDDL ID="CDDLCallNo" runat="server" Visible="False"></uc1:CustomDDL>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table id="Table1" width="100%">
                                    <tr>
                                        <td valign="top" align="left" colspan="1" rowspan="1">
                                            <cc1:CollapsiblePanel ID="cpnlRS" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                ExpandImage="../Images/ToggleDown.gif" Text="Call Summary" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                Height="96px">
                                                <table id="Table3" style="border-color: #d7d7d7" cellspacing="0" cellpadding="2"
                                                    width="100%" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label7" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="70px" runat="server"> Company</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCompany" Font-Size="XX-Small" Width="150px" runat="server"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label6" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="80px" runat="server">Sub Category</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlProject" Font-Size="XX-Small" Width="130px" runat="server"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label5" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="70px" runat="server">Call Req. By</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlEmployee" Font-Size="XX-Small" Width="150px" runat="server"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label4" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="76px" Height="16px" runat="server">From Call No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCallFrom" Width="150px" Font-Size="XX-Small" runat="server"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label3" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="76px" runat="server">To Call No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCallTo" Font-Size="XX-Small" Width="130px" Height="20px"
                                                                runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="80px">Call Priority</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCPriority" Font-Size="XX-Small" Width="150px" Height="20px"
                                                                runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label9" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="76px" runat="server"> From Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <ION:Customcalendar ID="dtFromDate" runat="server" Height="18px" Width="148px" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label8" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                Width="76px" runat="server">To Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <ION:Customcalendar ID="dtToDate" runat="server" Height="18px" Width="128px" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                            <cc1:CollapsiblePanel ID="cpnlReport" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                ExpandImage="../Images/ToggleDown.gif" Text="CALL SUMMARY" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                Height="65px">
                                                <%--<div style="overflow: auto; width: 99.7%; height: 350px">--%>
                                                <table id="Table2" style="height: 100%; border-color: activeborder" cellspacing="0"
                                                    cellpadding="0" width="100%" align="left" border="0">
                                                    <tr>
                                                        <td valign="top" align="left" colspan="1" rowspan="1">
                                                            <CR:CrystalReportViewer ID="crvReport" runat="server" Height="500px" BestFitPage="False"
                                                                Width="900px" EnableDrillDown="false" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False"
                                                                HasToggleGroupTreeButton="False" AutoDataBind="true" HasCrystalLogo="False" DisplayGroupTree="False"
                                                                SeparatePages="False" PrintMode="ActiveX" ShowAllPageIds="True" HasZoomFactorList="False" ReuseParameterValuesOnRefresh="True">
                                                            </CR:CrystalReportViewer>
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
                            Font-Names="Verdana" ForeColor="Red" Width="100px" Height="35px" Visible="false">
                        </asp:ListBox>
                        <input type="hidden" name="txthiddenImage2">
                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                        <input type="hidden" value="<%=mstrTaskNumber%>" name="txthiddenTaskNo" />
                        <input type="hidden" value="<%=mstrCompanyID%>" name="txtCompanyID">
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthiddenProject">
                        <input type="hidden" name="txthiddenAssignBy" />
                        <input type="hidden" name="txthiddenOwner" />
                        <input type="hidden" name="txthiddenCallNos" />
                        <input type="hidden" name="txthiddenCallNos2">
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
