<%@ page language="VB" autoeventwireup="false" inherits="Help_WSSSupport, App_Web_bnj2ix_q" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>WSS Support</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0"
		MS_POSITIONING="GridLayout">
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
																CommandName="submit" ImageUrl="white.GIF"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelAddHelp" runat="server" Height="12px" Width="128px" CssClass="HeaderTestMenu"
																Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">WSS Support</asp:label></TD>
														<TD align="left"><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;&nbsp;
															<asp:imagebutton id="imgClose" accessKey="L" runat="server" ImageUrl="../Images/s2close01.gif" ToolTip="Close"></asp:imagebutton>&nbsp;
															<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"></TD>
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
								<td>
									<TABLE id="Table16" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
										border="0">
										<TR>
											<TD vAlign="top" colSpan="1">
												<!--  **********************************************************************-->
												<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
													<table style="BORDER-COLLAPSE: collapse" width="100%" border="0">
														<tr>
															<td colSpan="2">
																<!-- *****************************************--><cc1:collapsiblepanel id="cpnlError" runat="server" Height="54px" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																	BorderColor="Indigo" Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Error Message"
																	ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																	<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
																		<TR>
																			<TD colSpan="0" rowSpan="0">
																				<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
																			<TD colSpan="0" rowSpan="0">
																				<asp:ListBox id="lstError" runat="server" Width="712px" BorderStyle="Groove" BorderWidth="0"
																					Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox></TD>
																		</TR>
																	</TABLE>
																</cc1:collapsiblepanel>
																<cc1:collapsiblepanel id="cpnlHelp" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																	BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
																	TitleBackColor="Transparent" Text="WSS Support" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
																	Draggable="False">
																	<div style="overflow:auto;width:100%">
																<TABLE align=left width="100%">
																	<TR>
																		<TD>
																			<asp:DataGrid Width="100%" id="grdSupport" runat="server" CssClass="Grid" AutoGenerateColumns="false">
																			<Columns>
<asp:TemplateColumn HeaderText="Support Document" HeaderStyle-VerticalAlign=Middle >
											<ItemTemplate>
											<asp:HyperLink Target="_blank" ID="HyperLink1" Text='<%# System.IO.Path.GetFileNameWithoutExtension( DataBinder.Eval(Container, "DataItem.FullName") )%>' runat="server" NavigateUrl='<%# "../Dockyard/WSSSupport/" & System.IO.Path.GetFileName( DataBinder.Eval(Container, "DataItem.FullName") )%>'></asp:HyperLink>
											</ItemTemplate>
										</asp:TemplateColumn>
																			
																			</Columns>
																			<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																			<HeaderStyle  Height="25px" CssClass="GridHeader"></HeaderStyle>
																			<ItemStyle Height="25px" CssClass="GridItem"></ItemStyle>
																			</asp:DataGrid>
																		</TD>
																	</TR>
																</TABLE>
																</div></cc1:collapsiblepanel>
																<!-- *****************************************--></td>
														</tr>
													</table>
												</DIV>
											</TD>
											<td vAlign="top" width="12" background="../images/main_line04.gif"><IMG height="1" src="../images/main_line04.gif" width="12"></td>
										</TR>
									</TABLE>
									<DIV></DIV>
								</td>
							</tr>
							<tr>
								<td height="2">
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
		</form>
	</body>
</html>
