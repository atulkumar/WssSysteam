<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PSFTHome.aspx.vb" Inherits="MonitoringCenter_PSFTHome" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>PeopleSoft Home</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">

	
	function SubmitIFrame(msg)
	{
			//document.getElementById('IFrame').contentWindow.document.Form1.submit();
			//alert(msg);
			document.getElementById('IFrame').contentWindow.Post(msg);
			return false;
	}
	

	
		
		</script>
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
														<TD style="WIDTH: 271px" align="left"><asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="white.GIF"
																CommandName="submit" AlternateText="."></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelAddHelp" runat="server" Width="160px" Height="12px" BorderStyle="None"
																BorderWidth="2px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" CssClass="HeaderTestMenu">Monitoring Home</asp:label></TD>
														<TD align="left">&nbsp;<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
															<asp:imagebutton id="imgAdd" runat="server" ImageUrl="../Images/s2Add01.gif" ToolTip="Add"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif" ToolTip="Save"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgDelete" accessKey="D" runat="server" ImageUrl="../Images/s2delete01.gif"
																ToolTip="Delete"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgClose" accessKey="L" runat="server" ImageUrl="../Images/s2close01.gif" ToolTip="Close"></asp:imagebutton>
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
																<!-- *****************************************--><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="54px" BorderStyle="Solid" BorderWidth="0px"
																	Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Error Message" TitleBackColor="Transparent" TitleClickable="True"
																	TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="False" BorderColor="Indigo">
																	<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
																		border="0">
																		<TR>
																			<TD colSpan="0" rowSpan="0">
																				<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
																			<TD colSpan="0" rowSpan="0">
																				<asp:ListBox id="lstError" runat="server" Width="752px" Font-Names="Verdana" Font-Size="XX-Small"
																					BorderWidth="0" BorderStyle="Groove" ForeColor="Red"></asp:ListBox></TD>
																		</TR>
																	</TABLE>
																</cc1:collapsiblepanel>
																<TABLE cellSpacing="0" cellPadding="0" align="left" border="0" width="100%">
																	<TR>
																		<TD>
																			<DIV style="OVERFLOW: auto; WIDTH: 140px; HEIGHT: 560px">
																				<asp:panel id="pnlTree" runat="server" BorderStyle="None" Width="100%" Height="515px">
																					<iewc:TreeView id="PSFTTree" runat="server" AutoPostBack="false"></iewc:TreeView>
																				</asp:panel></DIV>
																		</TD>
																		<TD><IFRAME id="IFR" style="WIDTH: 685px; HEIGHT: 560px; BACKGROUND-COLOR: transparent" name="IFrame"
																				frameBorder="no" runat="server"> </IFRAME>
																		</TD>
																	</TR>
																</TABLE>
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
			<INPUT type="hidden" name="txthiddenImage">
		</form>
	</body>
</html>
