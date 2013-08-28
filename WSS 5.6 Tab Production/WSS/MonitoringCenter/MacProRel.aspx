<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MacProRel.aspx.vb" Inherits="MonitoringCenter_MacProRel" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>WebForm2</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
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
														<TD style="WIDTH: 271px" align="left"><asp:button id="BtnGrdSearch" runat="server" Height="0" Width="0px"></asp:button><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
																CommandName="submit" ImageUrl="white.GIF"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelAddHelp" runat="server" Height="12px" Width="128px" Font-Names="Verdana"
																Font-Size="X-Small" CssClass="HeaderTestMenu" BorderWidth="2px" BorderStyle="None" Font-Bold="True">Machine Process</asp:label></TD>
														<TD align="left"><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;&nbsp;&nbsp;&nbsp;<asp:imagebutton id="ImgbtnSAVE" runat="server" ImageUrl="../images/S2Save01.gif"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="Imgbtndelete" runat="server" Height="30px" ImageUrl="../images/S2delete01.gif"></asp:imagebutton>&nbsp;
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
																<!-- *****************************************-->
																<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
																	<TR>
																		<TD><cc1:collapsiblepanel id="cpnlError" runat="server" Height="54px" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																				Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Error Message"
																				TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
																				BorderColor="Indigo">
																				<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
																					border="0">
																					<TR>
																						<TD colSpan="0" rowSpan="0">
																							<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
																						<TD colSpan="0" rowSpan="0">
																							<asp:ListBox id="lstError" runat="server" Width="552px" Font-Names="Verdana" Font-Size="XX-Small"
																								BorderWidth="0" BorderStyle="Groove" ForeColor="Red"></asp:ListBox></TD>
																					</TR>
																				</TABLE>
																			</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlMachineProcess" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																				Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Machine Process" TitleBackColor="Transparent"
																				TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" BorderColor="Indigo" Visible="True">
																				<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
																					<TR>
																						<TD>
																							<asp:Panel id="Panel1" Width="0" Runat="server">
																								<TABLE cellSpacing="0" cellPadding="0" border="0">
																									<TR>
																										<TD>
																											<asp:TextBox id="TxtCompanyId" Width="150px" CssClass="SearchTxtBox" Runat="server"></asp:TextBox></TD>
																										<TD>
																											<asp:TextBox id="TxtDomain" runat="server" Width="118px" CssClass="SearchTxtBox"></asp:TextBox></TD>
																										<TD>
																											<asp:TextBox id="TxtProcessId" runat="server" Width="129px" CssClass="SearchTxtBox"></asp:TextBox></TD>
																										<TD>
																											<asp:TextBox id="TxtMachine" runat="server" Width="155px" CssClass="SearchTxtBox"></asp:TextBox></TD>
																									</TR>
																								</TABLE>
																							</asp:Panel>
																							<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 180px; BACKGROUND-COLOR: #f5f5f5">
																								<asp:datagrid id="DgDataEntry" runat="server" CssClass="Grid" BorderWidth="0" BorderColor="#d4d4d4"
																									AutoGenerateColumns="False" CellPadding="0">
																									<SelectedItemStyle CssClass="GridSelectedItem" BackColor="#D4D4D4"></SelectedItemStyle>
																									<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																									<ItemStyle CssClass="GridItem"></ItemStyle>
																									<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
																									<Columns>
																										<asp:ButtonColumn Visible="False" Text="select" CommandName="select"></asp:ButtonColumn>
																										<asp:BoundColumn Visible="False" DataField="MPID"></asp:BoundColumn>
																										<asp:BoundColumn DataField="CI_VC36_Name" HeaderText="Company ID">
																											<HeaderStyle Width="149px"></HeaderStyle>
																										</asp:BoundColumn>
																										<asp:BoundColumn DataField="DM_VC150_DomainName" HeaderText="	Domain">
																											<HeaderStyle Width="117px"></HeaderStyle>
																										</asp:BoundColumn>
																										<asp:BoundColumn DataField="PM_VC20_PName" HeaderText="Process ID">
																											<HeaderStyle Width="128px"></HeaderStyle>
																										</asp:BoundColumn>
																										<asp:BoundColumn DataField="MM_VC150_Machine_Name" HeaderText="Machine">
																											<HeaderStyle Width="154px"></HeaderStyle>
																										</asp:BoundColumn>
																									</Columns>
																								</asp:datagrid></DIV>
																							<asp:Panel id="pnlFE" Width="0" Runat="server">
																								<TABLE cellSpacing="0" cellPadding="0" border="0">
																									<TR>
																										<TD>
																											<asp:DropDownList id="DdlCompany" Width="150px" CssClass="txtNoFocusFE" Runat="server" AutoPostBack="True"></asp:DropDownList></TD>
																										<TD>
																											<asp:DropDownList id="DdlDomain" Width="118px" CssClass="txtNoFocusFE" Runat="server" AutoPostBack="True"></asp:DropDownList></TD>
																										<TD>
																											<asp:DropDownList id="DdlProcessId" Width="129px" CssClass="txtNoFocusFE" Runat="server"></asp:DropDownList></TD>
																										<TD>
																											<asp:DropDownList id="DdlMachineName" runat="server" Width="155px" CssClass="txtNoFocusFE"></asp:DropDownList></TD>
																									</TR>
																								</TABLE>
																							</asp:Panel></TD>
																					</TR>
																				</TABLE>
																			</cc1:collapsiblepanel></TD>
																	</TR>
																</TABLE>
															</td>
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
	</body></html>
