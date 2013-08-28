<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_RPA_Search, App_Web_vrtyhdgv" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>RPA SEARCH</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">
		function SaveEdit(varImgValue)
				{
			    			if (varImgValue=='Edit')
							{
									if ( document.Form1.txthiddenID.value =='')
									{
										alert('Please select a row');
									}
									else
									{
									
											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit(); 
									}
							}	
								
							if (varImgValue=='Logout')
							{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
									
							}
							
							if (varImgValue=='Add')
							{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit();
							}	
						return false;
				}				
				
						function KeyCheck(rowvalues,ID)
						{
								
									
									var tableID='cpnlRPA_dgrRPA';
									var table;
									document.Form1.txthiddenID.value=ID;		
								
									if (document.all) table=document.all[tableID];
										if (document.getElementById) table=document.getElementById(tableID);
										if (table)
										{
												for ( var i = 1 ;  i < table.rows.length ;  i++)
													{	
														if(i % 2 == 0)
															{
																table.rows [ i ] . style . backgroundColor = "#f5f5f5";
															}
															else
															{
															
																table.rows [ i ] . style . backgroundColor = "#ffffff";
															}
													}
											table.rows [ rowvalues  ] . style . backgroundColor = "#d4d4d4";
										}
						}

							function KeyCheck55()
							{
									SaveEdit('Edit');
							}
					
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
												<table cellSpacing="0" cellPadding="0" width="94%" align="left" border="0">
													<TR>
														<TD style="WIDTH: 271px" align="left"><asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="~/images/white.GIF"
																CommandName="submit" AlternateText="."></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelRPASearch" runat="server" Width="128px" Height="12px" BorderStyle="None"
																BorderWidth="2px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" CssClass="HeaderTestMenu">RPA SEARCH</asp:label></TD>
														<TD align="left">&nbsp;&nbsp;&nbsp;&nbsp;<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
															<asp:imagebutton id="imgAdd" accessKey="A" runat="server" ImageUrl="../Images/s2Add01.gif" AlternateText="Add"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgEdit" accessKey="E" runat="server" ImageUrl="../Images/S2edit01.gif" AlternateText="Edit"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgSearch" accessKey="H" runat="server" ImageUrl="../Images/s1search02.gif"
																AlternateText="Search"></asp:imagebutton>&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
														</TD>
														<TD></TD>
														<td>&nbsp;
														</td>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../Images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
													src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../Images/main_line.gif" height="10"><IMG height="10" src="../Images/main_line.gif" width="6"></td>
											<td width="7" height="10"><IMG height="10" src="../Images/main_line01.gif" width="7"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../Images/main_line02.gif" height="2"><IMG height="2" src="../Images/main_line02.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../Images/main_line03.gif" width="12"></td>
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
																	<td colSpan="2"><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="54px" BorderStyle="Solid" BorderWidth="0px"
																			Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Error Message"
																			TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="False"
																			BorderColor="Indigo">
																			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
																				<TR>
																					<TD colSpan="0" rowSpan="0">
																						<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../../Images/warning.gif"></asp:Image></TD>
																					<TD colSpan="0" rowSpan="0">
																						<asp:ListBox id="lstError" runat="server" Width="752px" Font-Names="Verdana" Font-Size="XX-Small"
																							BorderWidth="0" BorderStyle="Groove" ForeColor="Red"></asp:ListBox></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlRPA" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" Draggable="False"
																			CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="RPA" TitleBackColor="Transparent" TitleClickable="True"
																			TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="True" BorderColor="Indigo">
																			<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 545px">
																				<asp:DataGrid id="dgrRPA" CssClass="grid" BorderColor="#d4d4d4" DataKeyField="RQ_NU9_SQID_PK"
																					AutoGenerateColumns="False" Runat="server" CellPadding="0">
																					<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																					<ItemStyle CssClass="GridItem"></ItemStyle>
																					<HeaderStyle CssClass="GridHeader"></HeaderStyle>
																					<Columns>
																						<asp:BoundColumn DataField="RQ_NU9_SQID_PK" Visible="False"></asp:BoundColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="txtRequestID" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								RequestID
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("RQ_NU9_REQUEST_ID")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="txtProcessName" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								ProcessName
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("PM_VC20_PName")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="txtProject" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								Project
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("RQ_VC150_CAT1")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="txtENV" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								ENV
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("RQ_VC150_CAT5")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="TxtNotes" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								Notes
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("RQ_VC150_CAT8")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="txtDate" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								Date
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("RQ_VC150_CAT2")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="txtClient" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								Client
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("CI_VC36_Name")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="txtRequestType" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								RequestType
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("RQ_VC150_CAT3")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Wrap="True">
																							<HeaderTemplate>
																								<asp:TextBox Width="100%" id="txtStatus" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																								Status
																							</HeaderTemplate>
																							<ItemTemplate>
																								<%# container.dataitem("RQ_CH2_STATUS")%>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																					</Columns>
																				</asp:DataGrid></DIV>
																		</cc1:collapsiblepanel>
																		<!-- *****************************************--></td>
																</tr>
															</TBODY>
														</table>
													</DIV>
												</TD>
												<td vAlign="top" width="12" background="../images/main_line04.gif"><IMG height="1" src="../images/main_line04.gif" width="12"></td>
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
			<INPUT type="hidden" name="txthiddenImage"> <INPUT type="hidden" name="txthiddenID">
		</form>
	</body>
</html>
