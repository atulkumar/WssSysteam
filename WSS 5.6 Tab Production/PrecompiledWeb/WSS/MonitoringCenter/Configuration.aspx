<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_Configuration, App_Web_zn3-f7gx" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Configuration Home</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">
	
	
		
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td vAlign="top">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<div align="right"><IMG height="2" src="../images/top_right_line.gif" width="96"></div>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../images/top_left_back.gif">&nbsp;</td>
											<td width="50"><IMG height="20" src="../images/top_right.gif" width="50"></td>
											<td width="21"><A href="#"><IMG onclick="Minimize();" height="20" src="../images/bt_min.gif" width="21" border="0"></A></td>
											<td width="21"><A href="#"><IMG onclick="Maximize();" height="20" src="../images/bt_max.gif" width="21" border="0"></A></td>
											<td width="19"><A href="#"><IMG onclick="CloseWSS();" height="20" src="../images/bt_clo.gif" width="19" border="0"></A></td>
											<td width="6"><IMG height="20" src="../images/bt_space.gif" width="6"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../images/top_nav_back.gif" height="67">
												<table cellSpacing="0" cellPadding="0" width="94%" align="center" border="0">
													<TR>
														<TD style="WIDTH: 271px" align="left"><span style="display:none"><asp:button id="BtnGrdSearch" runat="server" Height="0" Width="0px"></asp:button></span><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
																CommandName="submit" ImageUrl="~/images/white.GIF"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelAddHelp" runat="server" Height="12px" Width="160px" CssClass="HeaderTestMenu"
																Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">Configuration Home</asp:label></TD>
														<TD align="left">&nbsp;&nbsp;&nbsp;&nbsp;</TD>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../images/top_nav_back01.gif" height="67">&nbsp;
												<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../images/main_line.gif" height="10"><IMG height="10" src="../images/main_line.gif" width="6"></td>
											<td width="7" height="10"><IMG height="10" src="../images/main_line01.gif" width="7"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../images/main_line02.gif" height="2"><IMG height="2" src="../images/main_line02.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../images/main_line03.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td bgColor="#f5f5f5">
									<TABLE id="Table16" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
										border="0">
										<TR>
											<TD vAlign="top" bgColor="#f5f5f5" colSpan="1">
												<!--  **********************************************************************-->
												<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
													<table style="BORDER-COLLAPSE: collapse" width="100%" border="0">
														<tr>
															<td bgColor="#f5f5f5" colSpan="2">
																<!-- *****************************************--><cc1:collapsiblepanel id="cpnlError" runat="server" Height="54px" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																	BorderColor="Indigo" Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Error Message"
																	ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																	<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
																		<TR>
																			<TD colSpan="0" rowSpan="0">
																				<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
																			<TD colSpan="0" rowSpan="0">
																				<asp:ListBox id="lstError" runat="server" Width="752px" BorderStyle="Groove" BorderWidth="0"
																					Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox></TD>
																		</TR>
																	</TABLE>
																</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlConfig" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																	BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
																	Text="Configuration Home" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																	<TABLE height="100%" width="100%" border="0">
																		<TR>
																			<TD align="center" colSpan="2" height="84"><FONT face="Verdana" size="5"><STRONG>ION 
																						Monitoring Center</STRONG></FONT>
																			</TD>
																		</TR>
																		<TR>
																			<TD width="62" height="8"></TD>
																			<TD height="8"><BR>
																				<FONT face="Verdana" size="2">Welcome to <STRONG>ION Monitoring Center</STRONG>.<BR>
																					Here we present the 24󬱛65 monitoring of Client Network.
																					<BR>
																					Currently this monitoring deals with&nbsp;three main areas.
																					<UL type="square">
																						<LI>
																						Alert Center,
																						<LI>
																						Basic Monitoring&nbsp;and
																						<LI>
																							PeopleSoft.
																						</LI>
																					</UL>
																				</FONT>
																			</TD>
																		<TR>
																		</TR>
																	</TABLE>
																</cc1:collapsiblepanel>
																<TABLE height="100%" width="100%" bgColor="#f5f5f5" border="0">
																	<TR>
																		<td style="WIDTH: 63px"></td>
																		<TD height="8">&nbsp;
																			<asp:label id="Label1" runat="server" CssClass="FieldLabel">Company</asp:label>&nbsp;&nbsp;
																			<asp:dropdownlist id="DDLCompany" Width="120px" CssClass="txtNoFocus" Runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
																	</TR>
																	<TR>
																		<TD style="WIDTH: 63px"></TD>
																		<TD>
																			<TABLE height="100%" width="100%" border="0">
																				<TR>
																					<TD style="WIDTH: 400px"><cc1:collapsiblepanel id="cpnlalert" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																							BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent"
																							Text="Alert Center" ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
																							<TABLE height="100%" width="100%" border="0">
																								<TR>
																									<TD style="WIDTH: 35px">
																										<asp:image id="ImgQue" runat="server" Height="30px" ImageUrl="../Images/Alert.gif"></asp:image></TD>
																									<TD>
																										<asp:hyperlink id="HyAlert" runat="server" Font-Size="Smaller" Font-Names="Verdana" NavigateUrl="AlertSetUP.aspx">Set alert</asp:hyperlink></TD>
																								</TR>
																							</TABLE>
																						</cc1:collapsiblepanel></TD>
																					<TD style="WIDTH: 400px"><cc1:collapsiblepanel id="cpnlMonitoring" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																							BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent"
																							Text="Monitoring" ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
																							<TABLE height="100%" width="100%" bgColor="#f5f5f5" border="0">
																								<TR>
																									<TD style="WIDTH: 35px">
																										<asp:image id="Image1" runat="server" Height="30px" ImageUrl="../Images/options.gif"></asp:image></TD>
																									<TD>
																										<asp:hyperlink id="HyMonitoring" runat="server" Font-Size="Smaller" Font-Names="Verdana" NavigateUrl="PSFTHome.aspx">Monitoring</asp:hyperlink></TD>
																								</TR>
																							</TABLE>
																						</cc1:collapsiblepanel></TD>
																				</TR>
																				<TR>
																					<TD style="WIDTH: 400px"><cc1:collapsiblepanel id="cpnlReport" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																							BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent"
																							Text="Reports" ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
																							<TABLE height="100%" width="100%" border="0">
																								<TR>
																									<TD style="WIDTH: 35px">
																										<asp:ImageButton id="ImageButton2" runat="server" ImageUrl="../images/doc04.gif" DESIGNTIMEDRAGDROP="124"></asp:ImageButton></TD>
																									<TD>
																										<asp:hyperlink id="HyReport" runat="server" Font-Size="Smaller" Font-Names="Verdana" NavigateUrl="../Reports/MonitorIndex.aspx">Reports</asp:hyperlink></TD>
																								</TR>
																							</TABLE>
																						</cc1:collapsiblepanel></TD>
																					<TD style="WIDTH: 400px"><cc1:collapsiblepanel id="cpnlreports" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																							BorderColor="Indigo" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent"
																							Text="Reports" ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
																							<TABLE height="100%" width="100%" border="0">
																								<TR>
																									<TD style="WIDTH: 35px">
																										<asp:image id="ImgReport" runat="server" ImageUrl="../Images/options.gif"></asp:image></TD>
																									<TD>
																										<asp:HyperLink id="HyDomain" runat="server" Font-Size="Smaller" Font-Names="Verdana" NavigateUrl="DomainMachine.aspx">Domain & Machine</asp:HyperLink></TD>
																								</TR>
																							</TABLE>
																						</cc1:collapsiblepanel></TD>
																				</TR>
																			</TABLE>
																		</TD>
																	<TR>
																	</TR>
																</TABLE>
																<!-- *****************************************-->
																<DIV></DIV>
															</td>
														</tr>
													</table>
													<DIV></DIV>
												</DIV>
											</TD>
											<td vAlign="top" width="12" background="../images/main_line04.gif"><IMG height="1" src="../images/main_line04.gif" width="12"></td>
										</TR>
									</TABLE>
									<DIV></DIV>
								</td>
							</tr>
							<tr>
								<td bgColor="#f5f5f5" height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../images/main_line06.gif" height="2"><IMG height="2" src="../images/main_line06.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../images/main_line05.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../images/bottom_back.gif">&nbsp;</td>
											<td width="66"><IMG height="31" src="../images/bottom_right.gif" width="66"></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<INPUT type="hidden" name="txthiddenImage">
		</form>
	</body>
</html>
