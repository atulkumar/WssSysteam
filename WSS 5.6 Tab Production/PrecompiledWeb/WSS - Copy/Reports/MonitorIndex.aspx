<%@ page language="VB" autoeventwireup="false" inherits="Reports_MonitorIndex, App_Web_wm48jtpa" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Reports</title>

    <script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../Images/Js/JSValidation.js"></script>

    <script language="javascript" src="../calendar/popcalendar.js"></script>

    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet">
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script language="javascript">
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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" method="post" runat="server">
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr width="100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button><asp:ImageButton
                                                            ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
                                                            CommandName="submit" ImageUrl="~/images/white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="TitleLabel" BorderStyle="None">ION Monitoring Center</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" visible="false" onclick="SaveEdit('Help');" alt="E"
                                            src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img class="PlusImageCSS"
                                                id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
                                                border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="1">
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 581px">
                                <table width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                <cc1:CollapsiblePanel ID="cpnlReport" runat="server" Height="600px" Width="100%"
                                                    BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../Images/white.gif"
                                                    ExpandImage="../Images/white.gif" Text="ION MONITORING CENTER" TitleBackColor="Transparent"
                                                    TitleClickable="false" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                    Visible="true" BorderColor="Indigo">
                                                    <table height="100%" width="100%" border="0">
                                                        <tr>
                                                            <td align="center" colspan="2" height="84">
                                                                <font face="Verdana" size="5"><strong>ION Monitoring Center</strong></font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="62" height="8">
                                                            </td>
                                                            <td height="8">
                                                                <p>
                                                                    <br>
                                                                    <font face="Verdana" size="2">Welcome to <strong>ION Monitoring Center</strong>.<br>
                                                                        Here we present the 24󬱛65 monitoring of JDE Systems. </font>
                                                                </p>
                                                                <p>
                                                                    <font face="Verdana" size="2">Based on these reports ION analyze the performence and
                                                                        Maintenance of JDE systems .</font></p>
                                                            </td>
                                                            <tr>
                                                                <td width="62" height="8">
                                                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../images/doc04.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                                <td height="8">
                                                                    &nbsp;
                                                                    <asp:HyperLink ID="Hyperlink13" Width="96px" Font-Size="Smaller" Font-Underline="True"
                                                                        NavigateUrl="MonitorReports.aspx?ip=a" runat="server" Font-Names="Verdana">UBE Report</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="62" height="8">
                                                                    <asp:ImageButton ID="Imagebutton5" runat="server" ImageUrl="../images/doc04.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                                <td height="8">
                                                                    &nbsp;
                                                                    <asp:HyperLink ID="Hyperlink1" Width="190px" Font-Size="Smaller" Font-Underline="True"
                                                                        NavigateUrl="MonitorReports.aspx?ip=c" runat="server" Font-Names="Verdana">UBE Summary Report</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="62" height="22">
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../images/record_save.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                                <td height="22">
                                                                    &nbsp; <a href="MonitorReports.aspx?ip=b"><u><font face="Verdana" size="2">Database
                                                                        Details</font></u></a>&nbsp;&nbsp; &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="62" height="22">
                                                                    <asp:ImageButton ID="Imagebutton3" runat="server" ImageUrl="../images/record_save.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                                <td height="22">
                                                                    &nbsp; <a href="jdInfo.aspx?ip=minfo"><u><font face="Verdana" size="2">Machine Overview</font></u></a>&nbsp;&nbsp;
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <tr>
                                                                    <td width="62" height="22">
                                                                        <asp:ImageButton ID="Imagebutton6" runat="server" ImageUrl="../images/record_save.gif">
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                    <td height="22">
                                                                        &nbsp; <a href="jdInfo.aspx?ip=mosts"><u><font face="Verdana" size="2">Machine Online
                                                                            Status </font></u></a>&nbsp;&nbsp; &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="62" height="22">
                                                                        <asp:ImageButton ID="Imagebutton4" runat="server" ImageUrl="../images/record_save.gif">
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                    <td height="22">
                                                                        &nbsp; <a href="jdInfo.aspx?ip=sinfo"><u><font face="Verdana" size="2">System Overview</font></u></a>&nbsp;&nbsp;
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="62" height="8">
                                                                        <asp:ImageButton ID="Imagebutton7" runat="server" ImageUrl="../images/doc04.gif">
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                    <td height="8">
                                                                        &nbsp;
                                                                        <asp:HyperLink ID="Hyperlink2" Width="190px" Font-Size="Smaller" Font-Underline="True"
                                                                            NavigateUrl="MonitorReports.aspx?ip=d" runat="server" Font-Names="Verdana">Ques & Reports</asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="62" height="8">
                                                                        <asp:ImageButton ID="ImgbtnDskRpt" runat="server" ImageUrl="../images/doc04.gif">
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                    <td height="8">
                                                                        &nbsp;
                                                                        <asp:HyperLink ID="lnkDskRpt" Width="190px" Font-Size="Smaller" Font-Underline="True"
                                                                            NavigateUrl="MonitorReports.aspx?ip=g" runat="server" Font-Names="Verdana">Disk Reports</asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                    </table>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" name="txthiddenImage">
    </form>
</body>
</html>
