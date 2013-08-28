<%@ page language="VB" autoeventwireup="false" inherits="Reports_MonitorReports, App_Web_wm48jtpa" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Monitoring Reports</title>
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
	
 
	
	function SaveEdit(varImgValue)
				{
			    		
				//alert(varImgValue);								
						if (varImgValue=='Ok')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
									 CloseWindow();
						}
						if (varImgValue=='Logout')
								{
										document.Form1.txthiddenImage.value=varImgValue;
										Form1.submit(); 
										return false;
								}
				
				}	
						function HideContents()
				{
				
					parent.document.all("SideMenu1").cols="0,*";
					document.Form1.imgHide.style.visibility = 'hidden'; 
					document.Form1.ingShow.style.visibility = 'visible'; 				
							}
					
			function ShowContents()
				{
				
			
					document.Form1.ingShow.style.visibility = 'hidden'; 
					document.Form1.imgHide.style.visibility = 'visible'; 
					parent.document.all("SideMenu1").cols="14%,*";					
				}
					
			function Hideshow()
				{
					if (parent.document.all("SideMenu1").cols =="0,*")
					{
							document.Form1.imgHide.style.visibility = 'hidden'; 
							document.Form1.ingShow.style.visibility = 'visible'; 
					}
					else
					{
							document.Form1.ingShow.style.visibility = 'hidden'; 
							document.Form1.imgHide.style.visibility = 'visible'; 
					}
				}								
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 399px">
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None"> DATABASE DETAILS </asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <img class="PlusImageCSS" id="Ok" title="Search" onclick="SaveEdit('Ok');" alt="Search"
                                                        src="../Images/s1search02.gif" border="0" name="tbrbtnOk">&nbsp;
                                                    <asp:ImageButton ID="imgClose" runat="server" AlternateText="Close" ToolTip="Close"
                                                        ImageUrl="../Images/s2close01.gif"></asp:ImageButton>
                                                </td>
                                                <td>
                                                    <strong><font face="Verdana" size="1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </font></strong>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
                                            src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img class="PlusImageCSS"
                                                id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
                                                border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <tr>
                                <td>
                                    <!--  **********************************************************************-->
                                    <div style="overflow: auto; width: 100%; height: 581px">
                                        <table width="100%" border="0">
                                            <tbody>
                                                <tr>
                                                    <td colspan="2">
                                                        <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                            Width="100%" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                            ExpandImage="../Images/ToggleDown.gif" Text="Call Summary" TitleBackColor="Transparent"
                                                            TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                            Height="40px">
                                                            <table id="Table2" style="height: 27px" cellspacing="1" cellpadding="1" width="100%"
                                                                border="0">
                                                                <tr>
                                                                    <td style="width: 59px">
                                                                        <asp:Label ID="lblFromDate" Width="88px" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                            runat="server"> From Date</asp:Label>
                                                                    </td>
                                                                    <td style="width: 131px" nowrap>
                                                                        <%--<SCONTROLS:DATESELECTOR id="dtFromDate" runat="server" Text="Start Date:"></SCONTROLS:DATESELECTOR>--%>
                                                                        <ION:Customcalendar ID="dtFromDate" runat="server" Height="19px" Width="148px" />
                                                                    </td>
                                                                    <td style="width: 45px">
                                                                        <asp:Label ID="lblToDate" Width="48px" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                            runat="server">To Date</asp:Label>
                                                                    </td>
                                                                    <td nowrap>
                                                                        <%--<SCONTROLS:DATESELECTOR id="dtToDate" runat="server" Text="Start Date:"></SCONTROLS:DATESELECTOR>--%>
                                                                        <ION:Customcalendar ID="dtToDate" runat="server" Height="19px" Width="148px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </cc1:CollapsiblePanel>
                                                        <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                            Width="100%" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                            ExpandImage="../Images/ToggleDown.gif" Text="Call Summary" TitleBackColor="Transparent"
                                                            TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                            Height="65px">
                                                            <%--<CR:CRYSTALREPORTVIEWER id="crvReport" runat="server" Width="350px" Height="50px" ReuseParameterValuesOnRefresh="True"
																				ShowAllPageIds="True" PrintMode="ActiveX" DisplayGroupTree="False" HasCrystalLogo="False" AutoDataBind="true"
																				HasToggleGroupTreeButton="False" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False" SeparatePages="False"></CR:CRYSTALREPORTVIEWER>--%>
                                                            <CR:CrystalReportViewer ID="crvReport" runat="server" HasToggleGroupTreeButton="False"
                                                                AutoDataBind="True" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False"
                                                                EnableParameterPrompt="False" EnableTheming="false" HasCrystalLogo="False" HasRefreshButton="True"
                                                                HasSearchButton="False" HasViewList="False" HasZoomFactorList="False" ClientTarget="Uplevel"
                                                                BestFitPage="true"></CR:CrystalReportViewer>
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
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <input type="hidden" name="txthiddenImage">
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
