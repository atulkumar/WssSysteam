<%@ page language="VB" autoeventwireup="false" inherits="Reports_ReportsIndex, App_Web_fzfabjfx" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Reports</title>

    <script src="../images/js/core.js" type="text/javascript"></script>

    <script src="../images/js/events.js" type="text/javascript"></script>

    <script src="../images/js/css.js" type="text/javascript"></script>

    <script src="../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

     <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
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
    <form id="Form1" runat="server">
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
         <tr>
         <td>
           <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr width="100%">
                    <td background="../Images/top_nav_back.gif" height="47">
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BorderWidth="0px" BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None">
                                    </asp:Button><asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px"
                                        Width="1px" AlternateText="." CommandName="submit" ImageUrl="white.GIF"></asp:ImageButton>
                                    <asp:Label ID="lblTitle" runat="server" CssClass="TitleLabel" BorderStyle="None">WSS Report Center</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" width="152" background="../Images/top_nav_back.gif" height="47">
                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('48','../');"
                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
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
                            <tbody>
                                <tr>
                                    <td colspan="2">
                                        <cc1:CollapsiblePanel ID="cpnlReport" runat="server" Height="600px" Width="100%"
                                            BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../Images/white.gif"
                                            ExpandImage="../Images/white.gif" Text="WSS REPORT CENTER" TitleBackColor="Transparent"
                                            TitleClickable="false" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                            Visible="true" BorderColor="Indigo">
                                            <table height="100%" width="100%" border="0">
                                                <tr>
                                                    <td align="center" colspan="2" height="84">
                                                        <font face="Verdana" size="5"><strong>WSS Report Center</strong></font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="62" height="8">
                                                    </td>
                                                    <td height="8">
                                                        <br>
                                                        <font face="Verdana" size="2">Welcome to <strong>WSS Report Center</strong>.<br>
                                                            Here we present the 24󬱛65 Reporting of WSS Systems.Currently these Reports show
                                                            the&nbsp;details of calls , call summary , call status , call Delivery Status etc.<br>
                                                            <br>
                                                            Based on these reports One can analyze the productivity of the system.</font>
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="CallDetailDS.aspx?ip=cd"><u><font face="Verdana" size="2">Call Details</font></u></a>&nbsp;&nbsp;
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="Imagebutton5" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="CallDetails.aspx?ip=cdf"><u><font face="Verdana" size="2">Call Details
                                                                (Full)</font></u></a>&nbsp;&nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="Imagebutton3" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="CallDetails.aspx?ip=ctd"><u><font face="Verdana" size="2">Call/Task
                                                                Details</font></u></a>&nbsp;&nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="Imagebutton11" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="CallTaskAction.aspx?ip=cta"><u><font face="Verdana" size="2">Call/Task/Action
                                                                Details</font></u></a>&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="Imagebutton4" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="DailyAction.aspx?ip=da"><u><font face="Verdana" size="2">Daily&nbsp;Actions</font></u></a>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="Imagebutton17" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="DailyAction.aspx?ip=da2"><u><font face="Verdana" size="2">Task&nbsp;Summary</font></u></a>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="Imgbtn13" runat="server" ImageUrl="../images/doc04.gif"></asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="CallTaskAction.aspx?ip=Tr"><u><font face="Verdana" size="2">Task Wise
                                                                Productivity </font></u></a>&nbsp;&nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="Imagebutton14" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="CallTaskAction.aspx?ip=ed"><u><font face="Verdana" size="2">Emp Call
                                                                Detail</font></u></a>&nbsp;&nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="CReports.aspx?ip=cs&ScrID=499"><u><font face="Verdana" size="2">Call Summary</font></u></a>&nbsp;&nbsp;
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="29">
                                                            <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="29">
                                                            &nbsp; <a href="CReports.aspx?ip=cs2"><u><font face="Verdana" size="2">Call Summary
                                                                (Hrs)</font></u></a>&nbsp;&nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="29">
                                                            <asp:ImageButton ID="Imagebutton16" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="29">
                                                            &nbsp; <a href="prioritywiseCsummary.aspx?ip=PCS"><u><font face="Verdana" size="2">Call
                                                                Summary (Priority)</font></u></a>&nbsp;&nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="28">
                                                            <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="28">
                                                            &nbsp; <a href="CallStatus.aspx?ip=cst"><u><font face="Verdana" size="2">Call Status</font></u></a>&nbsp;&nbsp;
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="28">
                                                            <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="28">
                                                            &nbsp; <a href="Reports.aspx?ip=csd"><u><font face="Verdana" size="2">Call Status Summary</font></u></a>&nbsp;&nbsp;
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="28">
                                                            <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="28">
                                                            &nbsp; <a href="Reports.aspx?ip=cds"><u><font face="Verdana" size="2">Call Delivery
                                                                Status</font></u></a>&nbsp;&nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="28">
                                                            <asp:ImageButton ID="Imagebutton13" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="28">
                                                            &nbsp; <a href="Addressinfo.aspx?ip=AI"><u><font face="Verdana" size="2">Address Book</font></u></a>&nbsp;&nbsp;
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="28">
                                                            <asp:ImageButton ID="Imagebutton15" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="28">
                                                            &nbsp; <a href="tasksdelayed.aspx?ip=TDR"><u><font face="Verdana" size="2">Task Delay</font></u></a>&nbsp;&nbsp;
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="Imagebutton1" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="Invoices.aspx?ip=inl"><u><font face="Verdana" size="2">Invoice Summary</font></u></a>&nbsp;&nbsp;
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="62" height="27">
                                                            <asp:ImageButton ID="ImageButton12" runat="server" ImageUrl="../images/doc04.gif">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td height="27">
                                                            &nbsp; <a href="TimeRegistrationDS.aspx?ip=tms"><u><font face="Verdana" size="2">Time
                                                                Registration</font></u></a>&nbsp;&nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                            </table>
                                        </cc1:CollapsiblePanel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <input type="hidden" name="txthiddenImage">
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
