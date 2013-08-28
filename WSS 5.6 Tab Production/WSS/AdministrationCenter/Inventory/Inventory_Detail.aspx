<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Inventory_Detail.aspx.vb" Inherits="AdministrationCenter_Inventory_Inventory_Detail" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Inventory Detail</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<meta http-equiv="Expires" content="0">
		<meta http-equiv="Cache-Control" content="no-cache">
		<meta http-equiv="Pragma" content="no-cache">
		<script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../../Images/Js/JSValidation.js"></script>
		<LINK href="../calendar/popcalendar.css" type="text/css" rel="stylesheet">
			<script language="javascript" src="../../DateControl/ION.js"></script>
			<script language="javascript" src="../../Images/Js/CallViewShortCuts.js"></script>
			<LINK href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
				<script>

				
				
				</script>
	</HEAD>
	<body bgcolor="#f5f5f5" bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();"
		rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td vAlign="top">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<div align="right"><IMG height="2" src="../../Images/top_right_line.gif" width="96"></div>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../Images/top_left_back.gif">&nbsp;</td>
											<td width="50"><IMG height="20" src="../../Images/top_right.gif" width="50"></td>
											<td width="21"><A href="#"><IMG height="20" src="../../Images/bt_min.gif" width="21" border="0"></A></td>
											<td width="21"><A href="#"><IMG height="20" src="../../Images/bt_max.gif" width="21" border="0"></A></td>
											<td width="19"><A href="#"><IMG onclick="CloseWSS();" height="20" src="../../Images/bt_clo.gif" width="19" border="0"></A></td>
											<td width="6"><IMG height="20" src="../../Images/bt_space.gif" width="6"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr width="100%">
											<td background="../../Images/top_nav_back.gif" height="67">
												<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
													<TR>
														<TD width="332">
															<div><asp:button id="BtnGrdSearch" runat="server" Height="0" Width="0px" 
                                                                    BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:button><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
																	CommandName="submit" ImageUrl="white.GIF"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../../Images/left005.gif"
																	name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../../Images/Right005.gif"
																	name="ingShow">
																<asp:label id="lblTitleLabelTaskView" runat="server" Height="12px" Width="128px" CssClass="TitleLabel">CALL FAST ENTRY</asp:label></div>
														</TD>
														<TD align="left">
															<div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
																<asp:imagebutton id="imgSave" accessKey="A" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif"></asp:imagebutton>&nbsp;
																<asp:imagebutton id="ImgClose" accessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
																	ToolTip="Close"></asp:imagebutton>&nbsp;<IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
															</div>
														</TD>
														<TD align="center"></TD>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../../Images/top_nav_back01.gif" height="67">
												<div style="WIDTH: 150px"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('833','../../');"
														alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../../icons/logoff.gif"
														border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;</div>
											</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../Images/main_line.gif" height="10"><IMG height="10" src="../../Images/main_line.gif" width="6"></td>
											<td width="7" height="10"><IMG height="10" src="../../Images/main_line01.gif" width="7"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../Images/main_line02.gif" height="2"><IMG height="2" src="../../Images/main_line02.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../../Images/main_line03.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px" DESIGNTIMEDRAGDROP="99">
										<TABLE id="Table16" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<TR>
												<TD vAlign="top" colSpan="1">
													<!--  **********************************************************************-->
													<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
														<table width="100%" border="0">
															<TBODY>
																<tr>
																	<td colSpan="2"><cc1:collapsiblepanel id="cpnlError" runat="server" BorderWidth="0px" BorderStyle="Solid" Draggable="False"
																			CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif" Text="Error Message" TitleBackColor="Transparent"
																			TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="False" BorderColor="Indigo"
																			Height="47px" Width="100%">
																			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
																				border="0">
																				<TR>
																					<TD colSpan="0" rowSpan="0">
																						<asp:Image id="Image1" runat="server" Width="16px" Height="16px" ImageUrl="../../Images/warning.gif"></asp:Image></TD>
																					<TD colSpan="0" rowSpan="0">
																						<asp:ListBox id="lstError" runat="server" Width="722px" BorderStyle="Groove" BorderWidth="0"
																							Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
																						<asp:Panel id="Panel1" runat="server">
																							<asp:Panel id="Panel2" runat="server">
																								<asp:Label id="lblMsg" runat="server" Font-Size="10pt" Font-Names="Verdana" ForeColor="Black"
																									text="Your call Number is " Font-Bold="True"></asp:Label>
																								<asp:Label id="lblError" runat="server" Font-Size="12pt" Font-Names="Verdana" ForeColor="red"
																									Font-Bold="True"></asp:Label>
																							</asp:Panel>
																						</asp:Panel></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel></td>
																</tr>
																<TR>
																	<TD width="100%"><cc1:collapsiblepanel id="cpnlCallView" runat="server" BorderWidth="0px" BorderStyle="Solid" Draggable="False"
																			CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif" Text="Call Fast Entry" TitleBackColor="Transparent"
																			TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="true" BorderColor="Indigo"
																			Height="530px" Width="100%">
																			<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 450px">
																				<TABLE style="BORDER-COLLAPSE: collapse" height="31" width="100%" border="1">
																					<TR>
																						<TD width="6" height="31">&nbsp;</TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label7" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Item Name</asp:Label><BR>
																							<asp:TextBox id="txtCompany" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">Laptop</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label1" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Company</asp:Label><BR>
																							<asp:TextBox id="Textbox1" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100">ION</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label2" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Item Code</asp:Label><BR>
																							<asp:TextBox id="Textbox2" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100">Lap1002</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label3" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Description</asp:Label><BR>
																							<asp:TextBox id="Textbox3" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100">IBM laptop </asp:TextBox></TD>
																					</TR>
																					<TR>
																						<TD width="6" height="31">&nbsp;</TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label4" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Group</asp:Label><BR>
																							<asp:TextBox id="Textbox4" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100">DEV</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label5" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Type</asp:Label><BR>
																							<asp:TextBox id="Textbox5" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100">TFT</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label6" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Unit</asp:Label><BR>
																							<asp:TextBox id="Textbox6" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100">102</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label8" runat="server" Width="137px" Height="19px" CssClass="FieldLabel">Opening balance</asp:Label><BR>
																							<asp:TextBox id="Textbox7" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100">10</asp:TextBox></TD>
																					</TR>
																					<TR>
																						<TD width="6" height="31">&nbsp;</TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label9" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Reorder Level </asp:Label><BR>
																							<asp:TextBox id="Textbox8" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100"></asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label10" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">BalanceQuantity</asp:Label><BR>
																							<asp:TextBox id="Textbox9" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
																								MaxLength="100"></asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label11" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">Location</asp:Label><BR>
																							<asp:TextBox id="Textbox10" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">Mohali</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label12" runat="server" Width="137px" Height="19px" CssClass="FieldLabel">Date of Purchase </asp:Label><BR>
																							<asp:TextBox id="Textbox11" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																					</TR>
																					<TR>
																						<TD width="6" height="31">&nbsp;</TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label13" runat="server" Width="137px" Height="19px" CssClass="FieldLabel">Manufacturing Date </asp:Label><BR>
																							<asp:TextBox id="Textbox12" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label14" runat="server" Width="127px" Height="19px" CssClass="FieldLabel">Warranty Period</asp:Label><BR>
																							<asp:TextBox id="Textbox13" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">2 years</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label15" runat="server" Width="97px" Height="19px" CssClass="FieldLabel">WarrantyIn</asp:Label><BR>
																							<asp:TextBox id="Textbox14" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">Yes</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label16" runat="server" Width="127px" Height="19px" CssClass="FieldLabel">AMC Expires on</asp:Label><BR>
																							<asp:TextBox id="Textbox15" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																					</TR>
																					<TR>
																						<TD width="6" height="31">&nbsp;</TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label17" runat="server" Width="137px" Height="19px" CssClass="FieldLabel">AMC Vendor Name </asp:Label><BR>
																							<asp:TextBox id="Textbox16" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label18" runat="server" Width="147px" Height="19px" CssClass="FieldLabel">AMC Vendor Address</asp:Label><BR>
																							<asp:TextBox id="Textbox17" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">Sector 35 -c</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label19" runat="server" Width="147px" Height="19px" CssClass="FieldLabel">AMC Vendor Contact</asp:Label><BR>
																							<asp:TextBox id="Textbox18" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">09855706138</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label20" runat="server" Width="127px" Height="19px" CssClass="FieldLabel">AMC Comments</asp:Label><BR>
																							<asp:TextBox id="Textbox19" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																					</TR>
																					<TR>
																						<TD width="6" height="31">&nbsp;</TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label21" runat="server" Width="197px" Height="19px" CssClass="FieldLabel">AMC Alerts Daysin Advance</asp:Label><BR>
																							<asp:TextBox id="Textbox20" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label22" runat="server" Width="147px" Height="19px" CssClass="FieldLabel">Status</asp:Label><BR>
																							<asp:TextBox id="Textbox21" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">Pending</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label23" runat="server" Width="147px" Height="19px" CssClass="FieldLabel">Price</asp:Label><BR>
																							<asp:TextBox id="Textbox22" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">35000</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label24" runat="server" Width="127px" Height="19px" CssClass="FieldLabel">Price in Currency</asp:Label><BR>
																							<asp:TextBox id="Textbox23" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">Rupees</asp:TextBox></TD>
																					</TR>
																					<TR>
																						<TD width="6" height="31">&nbsp;</TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label25" runat="server" Width="137px" Height="19px" CssClass="FieldLabel">Dep. Percentage</asp:Label><BR>
																							<asp:TextBox id="Textbox24" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100">2</asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label26" runat="server" Width="147px" Height="19px" CssClass="FieldLabel">Category Code1</asp:Label><BR>
																							<asp:TextBox id="Textbox25" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label27" runat="server" Width="147px" Height="19px" CssClass="FieldLabel">Category Code2</asp:Label><BR>
																							<asp:TextBox id="Textbox26" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																						<TD width="222" height="31">
																							<asp:Label id="Label28" runat="server" Width="127px" Height="19px" CssClass="FieldLabel">Category Code3</asp:Label><BR>
																							<asp:TextBox id="Textbox27" runat="server" Width="139px" Height="18px" CssClass="txtNoFocus"
																								ReadOnly="True" MaxLength="100"></asp:TextBox></TD>
																					</TR>
																				</TABLE>
																			</DIV>
																			<asp:panel id="Panel5" runat="server">
																				<asp:panel id="Panel6" runat="server"></asp:panel>
																			</asp:panel>
																		</cc1:collapsiblepanel> <!-- *****************************************--></TD>
																</TR>
															</TBODY>
														</table>
													</DIV>
												</TD>
												<td vAlign="top" width="12" background="../../images/main_line04.gif"><IMG height="1" src="../../images/main_line04.gif" width="12"></td>
											</TR>
										</TABLE>
										<DIV></DIV>
									</DIV>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../images/main_line06.gif" height="2"><IMG height="2" src="../../images/main_line06.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../../images/main_line05.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../images/bottom_back.gif">&nbsp;</td>
											<td width="66"><IMG height="31" src="../../images/bottom_right.gif" width="66"></td>
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
