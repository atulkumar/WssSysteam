<%@ page language="VB" autoeventwireup="false" inherits="Reports_AddressInfo, App_Web_wzhf50l2" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
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
var gAID;
var gCID;
var gEID;
var gDDL;



function ShowImg()
{
document.getElementById('imgAjax').src="../images/ajax1.gif";
document.getElementById('imgAjax').style.display='inline';
}


function DDLChange(DDL,CMID,EID)
{

gEID=EID;//Employee
gDDL=DDL;
xmlHttp=null;
var ddlCompany=document.getElementById(CMID);
var CompID=ddlCompany.options(ddlCompany.selectedIndex).value;
//var ddlProject=document.getElementById(PID);
var ddlEmployee=document.getElementById(EID);
//var ddlAssignedBy=document.getElementById(CID);
//var ProjectID=ddlProject.options(ddlProject.selectedIndex).value;
if ( CompID==0)
{
//ddlProject.disabled=true;
ddlEmployee.disabled=true;
//ddlAssignedBy.disabled=true;
}
else
{
//ddlProject.disabled=false;
ddlEmployee.disabled=false;
//ddlAssignedBy.disabled=false;
}
var url= '../AJAX Server/AjaxInfo.aspx?DDL='+ DDL +'&Type=AddressInfo&CompID='+ CompID +'&Rnd='+Math.random();
xmlHttp = GetXmlHttpObject(stateChangeHandler);    
xmlHttp_Get(xmlHttp, url); 
}


function stateChangeHandler() 
{ 	

if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
{ 

document.getElementById('imgAjax').style.display='none'
document.getElementById(gEID).options.length=0;
var objNewOption;
objNewOption = document.createElement("OPTION");
document.getElementById(gEID).options.add(objNewOption);
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
case 0://Employee
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
document.Form1.txthiddenEmployee.value= RoleDataName + '~' + RoleDataID ;
break;
}//case 2
									
}//switch
} //for loop
}//if
}
//
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
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%"  border="0">
                                            <tr>
                                                <td style="width: 25%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblHead" runat="server" CssClass="TitleLabel"
                                                        BorderStyle="None">Address Book </asp:Label>
                                                </td>
                                                <td style="width: 60%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <img class="PlusImageCSS" id="Ok" title="Find" onclick="SaveEdit('Ok');" alt="Find"
                                                            src="../Images/s1search02.gif" width="0" border="0" name="tbrbtnOk" runat="server" />
                                                        <asp:ImageButton ID="imgOK" runat="server" ImageUrl="../Images/s1search02.gif" ToolTip="Search">
                                                        </asp:ImageButton>
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img width="24" height="24" id="imgAjax" title="ajax" src="../images/divider.gif" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(793 ,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="Logout"
                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <table width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <cc1:CollapsiblePanel ID="cpnlRS" runat="server" Height="0px" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                TitleBackColor="Transparent" Text="Call Summary" ExpandImage="../Images/ToggleDown.gif"
                                                CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                <table id="Table2" style="border-color: #d7d7d7" cellspacing="1" cellpadding="4"
                                                    border="0">
                                                    <tr>
                                                        <td style="width: 41px; height: 30px">
                                                            <asp:Label ID="lblCompany" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                runat="server"> Company</asp:Label>
                                                        </td>
                                                        <td style="width: 140px; height: 30px">
                                                            <asp:DropDownList ID="ddlCompany" Font-Size="XX-Small" Width="160px" runat="server"
                                                                CssClass="DDLField" Height="18px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 35px; height: 30px">
                                                            <asp:Label ID="Label6" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                runat="server">Employee</asp:Label>
                                                        </td>
                                                        <td style="width: 70px; height: 30px">
                                                            <asp:DropDownList ID="ddlEmployee" runat="server" Font-Size="XX-Small" Width="168px"
                                                                CssClass="DDLField" Height="18px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                            <cc1:CollapsiblePanel ID="cpnlReport" runat="server" Height="0px" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                TitleBackColor="Transparent" Text="Call Summary" ExpandImage="../Images/ToggleDown.gif"
                                                CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                              <%--  <div style="overflow: auto; width: 99.7%; height: 476px">--%>
                                                    <table align="left" style="height: 100%">
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
                                </tbody>
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
                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthiddenProject" />
                        <input type="hidden" name="txthiddenAssignBy" />
                        <input type="hidden" name="txthiddenOwner" />
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
