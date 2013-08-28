<%@ page language="VB" autoeventwireup="false" inherits="Reports_jdInfo, App_Web_wm48jtpa" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
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
		<LINK href="../SupportCenter/calend&#9;&#9;ar/popcalendar.css" type="text/css" rel="stylesheet">
		<LINK href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">
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
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
							<tr>
								<td><div align="right"><img src="../Images/top_right_line.gif" width="96" height="2"></div>
								</td>
							</tr>
							<tr>
								<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/top_left_back.gif">&nbsp;</td>
											<td width="50"><img src="../Images/top_right.gif" width="50" height="20"></td>
											<td width="21"><a href="#"><img src="../Images/bt_min.gif" width="21" height="20" border="0"></a></td>
											<td width="21"><a href="#"><img src="../Images/bt_max.gif" width="21" height="20" border="0"></a></td>
											<td width="19"><a href="#"><img onclick="CloseWSS()" src="../Images/bt_clo.gif" width="19" height="20" border="0"></a></td>
											<td width="6"><img src="../Images/bt_space.gif" width="6" height="20"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr width="100%">
											<td background="../Images/top_nav_back.gif" height="67">
												<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
													<TR>
														<TD style="WIDTH: 365px">
															<IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
																name="ingShow">
															<asp:label id="lblHead" runat="server" Width="320px" ForeColor="White" Font-Bold="True" Font-Names="Verdana"
																Font-Size="X-Small" BorderStyle="None" BorderWidth="2px"> DATABASE DETAILS </asp:label>
														</TD>
														<td>
															<IMG class="PlusImageCSS" id="Ok" title="Search" onclick="SaveEdit('Ok');" alt="Search"
																src="../Images/s1search02.gif" border="0" name="tbrbtnOk">
															<asp:ImageButton id="imgClose" runat="server" ImageUrl="../Images/s2close01.gif" ToolTip="Close"
																AlternateText="Close"></asp:ImageButton></td>
														<TD></TD>
														<td>
														</td>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../Images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(document.getElementById('HIDSCRID').value ,'../');" alt="E"
													src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="SaveEdit('Logout');" alt="E"
													src="../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/main_line.gif" height="10"><img src="../Images/main_line.gif" width="6" height="10"></td>
											<td width="7" height="10"><img src="../Images/main_line01.gif" width="7" height="10"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2"><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/main_line02.gif" height="2"><img src="../Images/main_line02.gif" width="2" height="2"></td>
											<td width="12" height="2"><img src="../Images/main_line03.gif" width="12" height="2"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
										<TABLE id="Table16" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<TR>
												<TD vAlign="top" colSpan="1">
													<!--  **********************************************************************-->
													<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
														<table width="100%" border="0">
															<TBODY>
																<tr>
																	<td colSpan="2">
																		<cc1:collapsiblepanel id="cpnlReport" runat="server" BorderWidth="0px" BorderStyle="Solid" Width="100%"
																			BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
																			Text="Call Summary" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
																			TitleCSS="test" Height="65px">
																	<%--		<CR:CRYSTALREPORTVIEWER id="crvReport" runat="server" Width="350px" Height="50px" hassearchbutton="false"
																				HasViewList="False" ReuseParameterValuesOnRefresh="True" ShowAllPageIds="True" PrintMode="ActiveX" DisplayGroupTree="False"
																				HasCrystalLogo="False" AutoDataBind="true" HasToggleGroupTreeButton="False" EnableParameterPrompt="False"
																				EnableDatabaseLogonPrompt="False" SeparatePages="False"></CR:CRYSTALREPORTVIEWER>--%>
					 
<cr:crystalreportviewer id="crvReport" runat="server" HasToggleGroupTreeButton="False" autodatabind="True" 
displaygrouptree="False" enabledatabaselogonprompt="False" enableparameterprompt="False" enabletheming="false" 
hascrystallogo="False" hasrefreshbutton="True" hassearchbutton="False" hasviewlist="False" haszoomfactorlist="False" 
ClientTarget="Uplevel" BestFitPage="true"></cr:crystalreportviewer>															
																		</cc1:collapsiblepanel></td>
																</tr>
															</TBODY>
														</table>
													</DIV>
												</TD>
												<td width="12" valign="top" background="../Images/main_line04.gif"><img src="../Images/main_line04.gif" width="12" height="1"></td>
											</TR>
										</TABLE>
									</DIV>
								</td>
							</tr>
							<tr>
								<td height="2"><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/main_line06.gif" height="2"><img src="../Images/main_line06.gif" width="2" height="2"></td>
											<td width="12" height="2"><img src="../Images/main_line05.gif" width="12" height="2"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/bottom_back.gif">&nbsp;</td>
											<td width="66"><img src="../Images/bottom_right.gif" width="66" height="31"></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<INPUT type="hidden" name="txthiddenImage">
			<input type="hidden" name="HIDSCRIDName" runat="server" id="HIDSCRID">
		</form>
	</body>
</html>
