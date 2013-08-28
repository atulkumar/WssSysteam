<%@ page language="VB" autoeventwireup="false" inherits="Security_frm_Objectdata, App_Web_mewe43ky" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>
		<link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet"/>
		<script language="javascript" type="text/javascript" src="../DateControl/ION.js"></script>
		<script language="javascript" type="text/javascript"src="../images/Js/JSValidation.js"></script>
		<link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet"/>
		<script language="javascript" type="text/javascript" src="../SupportCenter/calendar/popcalendar.js"></script>
		<style type="text/css">.DataGridFixedHeader { POSITION: relative; ; TOP: expression(this.offsetParent.scrollTop); BACKGROUND-COLOR: #e0e0e0 }
		</style>
		<script language="javascript" type="text/javascript">
	
		function ConfirmDelete()
				{
					var confirmed
					confirmed=window.confirm("Delete,Are you sure you want to Delete the selected record ?");
					if(confirmed==false)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
				
		</script>

    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>


   <table width="100%" style="height:100%" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
							<tr>
								<td><div align="right"><img src="../Images/top_right_line.gif" width="96" height="2" /></div>
								</td>
							</tr>
							<tr>
								<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/top_left_back.gif">&nbsp;</td>
											<td width="50"><img src="../Images/top_right.gif" width="50" height="20"/></td>
											<td width="21"><a href="#"><img src="../Images/bt_min.gif" width="21" height="20" border="0"/></a></td>
											<td width="21"><a href="#"><img src="../Images/bt_max.gif" width="21" height="20" border="0"/></a></td>
											<td width="19"><a href="#"><img onclick="CloseWSS();" src="../Images/bt_clo.gif" width="19" height="20" border="0"/></a></td>
											<td width="6"><img src="../Images/bt_space.gif" width="6" height="20"/></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td style="height: 86px"><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr width="100%">
											<td background="../Images/top_nav_back.gif" height="67">
												<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
													<tr>
														<td>
															<asp:button id="BtnGrdSearch" runat="server" Height="0" Width="0px"></asp:button><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
																CommandName="submit" ImageUrl="white.GIF"></asp:imagebutton><img class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
																name="imgHide"/> <img class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
																name="ingShow"/>
															<asp:label id="lblTitleLabelObjData" runat="server" Height="12px" Width="111px" BorderWidth="2px"
																BorderStyle="None" CssClass="TitleLabel"> Object Data</asp:label>
															<img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"/>&nbsp;
															<asp:imagebutton id="imgAdd" accessKey="A" runat="server" ImageUrl="../Images/s2Add01.gif" CausesValidation="False"
																ToolTip="Add"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif" ToolTip="Save"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgReset" accessKey="R" runat="server" ImageUrl="../Images/reset_20.gif" CausesValidation="False"
																ToolTip="Reset"></asp:imagebutton>&nbsp;&nbsp;<asp:imagebutton id="imgDelete" accessKey="D" runat="server" ImageUrl="../Images/s2delete01.gif"
																CausesValidation="False" ToolTip="Delete"></asp:imagebutton>&nbsp; <img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"/>
															&nbsp;
														</td>
														<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
															</td>
														<td>&nbsp;
														</td>
													</tr>
												</table>
											</td>
											<td align="right" width="152" background="../Images/top_nav_back01.gif" height="67"><img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('63','../');" alt="E"
													src="../Images/s1question02.gif" border="0" name="tbrbtnEdit"/>&nbsp;&nbsp;<asp:ImageButton
                                                        ID="Logout" runat="server" ImageUrl="~/Images/logoff.gif" ToolTip="Logout" />&nbsp;&nbsp;</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/main_line.gif" height="10"><img src="../Images/main_line.gif" width="6" height="10"/></td>
											<td width="7" height="10"><img src="../Images/main_line01.gif" width="7" height="10"/></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2"><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/main_line02.gif" height="2"><img src="../Images/main_line02.gif" width="2" height="2"/></td>
											<td width="12" height="2"><img src="../Images/main_line03.gif" width="12" height="2"/></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
										<TABLE id="Table16" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<tr>
												<td vAlign="top" colSpan="1">
													<!--  **********************************************************************-->
													<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
														<table width="100%" border="0">
															<TBODY>
																<tr>
																	<td colSpan="2">
																		<table>
																			<tr>
																				<td>
																					<asp:panel id="pnltree" runat="server" Height="100%" Width="300" BorderStyle="None">
																						<iewc:treeView id="Newtree" runat="server" AutoPostBack="true"></iewc:treeView>
																					</asp:panel>
																				</td>
																				<td valign="top">
																					<table width="100%" border="1">
																						<TBODY>
																							<tr>
																								<td><asp:label id="Label2" runat="server" Width="96px" CssClass="FieldLabel">Parent Name</asp:label></td>
																								<td><asp:label id="Label4" runat="server" Width="104px" CssClass="FieldLabel">Object Type</asp:label></td>
																							</tr>
																							<tr>
																								<td><asp:dropdownlist id="cboPObjName" runat="server" Width="160px" CssClass="txtNoFocus"></asp:dropdownlist></td>
																								<td><asp:dropdownlist id="cboObjectType" tabIndex="1" runat="server" Width="160px" CssClass="txtNoFocus"></asp:dropdownlist></td>
																							</tr>
																							<tr>
																								<td style="HEIGHT: 10px"><asp:label id="Label12" runat="server" Width="86px" CssClass="FieldLabel">Object Name</asp:label><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" Font-Size="XX-Small" ErrorMessage="*"
																										ControlToValidate="txtObjName"></asp:requiredfieldvalidator></td>
																								<td style="HEIGHT: 10px"><asp:label id="Label13" runat="server" Width="70px" CssClass="FieldLabel">Alias Name</asp:label><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" Font-Size="XX-Small" ErrorMessage="*"
																										ControlToValidate="txtObjAName"></asp:requiredfieldvalidator></td>
																							</tr>
																							<tr>
																								<td><asp:textbox id="txtObjName" tabIndex="2" runat="server" Width="216px" CssClass="txtNoFocus"
																										MaxLength="25"></asp:textbox></td>
																								<td><asp:textbox id="txtObjAName" tabIndex="3" runat="server" Width="216px" CssClass="txtNoFocus"
																										MaxLength="15"></asp:textbox></td>
																							</tr>
																							<tr>
																								<td style="HEIGHT: 15px"><asp:label id="Label3" runat="server" Width="70px" CssClass="FieldLabel">Image URL</asp:label><asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" Font-Size="XX-Small" ErrorMessage="*"
																										ControlToValidate="txtimgURL"></asp:requiredfieldvalidator></td>
																								<td style="HEIGHT: 15px"><asp:label id="Label5" runat="server" Width="72px" CssClass="FieldLabel">Page URL</asp:label></td>
																							</tr>
																							<tr>
																								<td><asp:textbox id="txtimgURL" tabIndex="4" runat="server" Width="216px" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></td>
																								<td><asp:textbox id="txtPageURL" tabIndex="5" runat="server" Width="216px" CssClass="txtNoFocus"
																										MaxLength="200"></asp:textbox></td>
																							</tr>
																							<tr>
																								<td><asp:label id="Label14" runat="server" Width="144px" CssClass="FieldLabel">Fast Path</asp:label></td>
																								<td><asp:label id="Label8" runat="server" Width="144px" CssClass="FieldLabel">Grid Name</asp:label></td>
																							</tr>
																							<tr>
																								<td style="HEIGHT: 23px"><asp:textbox id="txtFPath" tabIndex="6" runat="server" Width="216px" CssClass="txtNoFocus" MaxLength="8"></asp:textbox></td>
																								<td style="HEIGHT: 23px"><asp:textbox id="txtGName" tabIndex="7" runat="server" Width="216px" CssClass="txtNoFocus" MaxLength="50"></asp:textbox></td>
																							</tr>
																							<tr>
																								<td><asp:label id="Label6" runat="server" Width="144px" CssClass="FieldLabel">View Column Name</asp:label></td>
																								<td><asp:label id="Label7" runat="server" Width="144px" CssClass="FieldLabel">View Column Width</asp:label></td>
																							</tr>
																							<tr>
																								<td style="HEIGHT: 23px"><asp:textbox id="txtVColName" tabIndex="8" runat="server" Width="216px" CssClass="txtNoFocus"
																										MaxLength="200"></asp:textbox></td>
																								<td style="HEIGHT: 23px"><asp:textbox id="txtVColWidth" tabIndex="9" runat="server" CssClass="txtNoFocus" MaxLength="5"></asp:textbox></td>
																							</tr>
																							<tr>
																								<td style="HEIGHT: 14px" colSpan="2"><asp:label id="Label9" runat="server" Width="72px" CssClass="FieldLabel">Order By</asp:label></td>
																							</tr>
																							<tr>
																								<td><asp:textbox id="txtOrderBy" tabIndex="10" runat="server" CssClass="txtNoFocus" MaxLength="2"></asp:textbox></td>
																								<td><asp:checkbox id="chkIsMandatory" runat="server" Text="If Mandatory" CssClass="FieldLabel"></asp:checkbox></td>
																							</tr>
																							<tr>
																								<td><asp:label id="Label10" runat="server" Width="64px" CssClass="FieldLabel">Status</asp:label></td>
																								<td><asp:label id="Label11" runat="server" Width="96px" CssClass="FieldLabel">Status Date</asp:label></td>
																							</tr>
																							<tr>
																								<td><asp:dropdownlist id="cboStatus" tabIndex="12" runat="server" Width="160px" CssClass="txtNoFocus"></asp:dropdownlist></td>
																								<td><SCONtrOLS:DATESELECTOR id="dtStatusDate" runat="server" Text=""></SCONtrOLS:DATESELECTOR></td>
																							</tr>
																						</TBODY>
																					</table>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>
															</TBODY>
														</table>
													</div>
												</td>
												<td width="12" valign="top" background="../Images/main_line04.gif"><img src="../Images/main_line04.gif" width="12" height="1"/></td>
											</tr>
										</TABLE>
									</div>
								</td>
							</tr>
							<tr>
								<td height="2"><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/main_line06.gif" height="2"><img src="../Images/main_line06.gif" width="2" height="2"/></td>
											<td width="12" height="2"><img src="../Images/main_line05.gif" width="12" height="2"/></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="../Images/bottom_back.gif">&nbsp;</td>
											<td width="66"><img src="../Images/bottom_right.gif" width="66" height="31"/></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			  <asp:UpdatePanel ID="PanelUpdate" runat="server">
                        <ContentTemplate>

			<asp:Panel id="pnlMsg" runat="server"></asp:Panel>
			</ContentTemplate>
  </asp:UpdatePanel>
			<div></div>
    </form>
</body>
</html>
