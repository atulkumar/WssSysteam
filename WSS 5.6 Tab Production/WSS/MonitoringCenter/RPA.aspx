<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RPA.aspx.vb" Inherits="MonitoringCenter_RPA" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>RPA</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<style>.BTN { BORDER-TOP-STYLE: solid; BORDER-RIGHT-STYLE: solid; BORDER-LEFT-STYLE: solid; BORDER-BOTTOM-STYLE: solid }
		</style>
		<script language="javascript">
		function SaveEdit(varImgValue)
				{
			    			if (varImgValue=='Save')
							{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
							}	
								
							if (varImgValue=='Logout')
							{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
									
							}
							
							if (varImgValue=='Ok')
							{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit();
							}	
										
							if (varImgValue=='Close')
							{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit();
									//window.history.back(-1);
							}	
						return false;
				}				
				
						function KeyCheck(rowvalues)
						{
								
									
									var tableID='cpnlObject_dgrObject';
									var table;
												
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

						function KeyCheck55(ID)
						{
							SaveEdit('Edit');
						//alert(ID);
						}	
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" onload="Hideshow();"
		rightMargin="0">
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
															<asp:label id="lblTitleLabelRPA" runat="server" Width="128px" Height="12px" BorderStyle="None"
																BorderWidth="2px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" CssClass="HeaderTestMenu">RPA</asp:label></TD>
														<TD align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif" AlternateText="Save"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgOk" accessKey="K" runat="server" ImageUrl="../Images/s1ok02.gif" ToolTip="Ok"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgClose" accessKey="C" runat="server" ImageUrl="../Images/S2close01.gif" AlternateText="Close"></asp:imagebutton><asp:button id="Button1" runat="server" Text="Button" Visible="False"></asp:button></TD>
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
									<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px" DESIGNTIMEDRAGDROP="77">
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
																			Text="Error Message" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
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
																		</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlRPA" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" Text="RPA Environment Info"
																			Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" TitleBackColor="Transparent"
																			TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="True" BorderColor="Indigo">
																			<TABLE align="left">
																				<TR>
																					<TD>
																						<asp:label id="lblMiddleName4" runat="server" Height="12px" Width="72px" Font-Bold="True" Font-Names="Verdana"
																							Font-Size="XX-Small" ForeColor="DimGray">Company</asp:label><BR>
																						<asp:DropDownList id="DDLCompany" Width="150px" CssClass="txtNoFocus" Runat="server" AutoPostBack="True"></asp:DropDownList></TD>
																					<TD>&nbsp;</TD>
																					<TD vAlign="top" rowSpan="4">
																						<asp:label id="Label4" runat="server" Height="12px" Width="72px" Font-Bold="True" Font-Names="Verdana"
																							Font-Size="XX-Small" ForeColor="DimGray">&nbsp;Environment</asp:label>
																						<DIV style="OVERFLOW: auto; WIDTH: 130px; HEIGHT: 100px">
																							<asp:CheckBoxList id="cblEnv" runat="server" Height="50px" Width="110px" Font-Names="Verdana" Font-Size="XX-Small"
																								RepeatLayout="Flow"></asp:CheckBoxList></DIV>
																					</TD>
																					<TD>&nbsp;</TD>
																					<TD vAlign="top" rowSpan="4">
																						<asp:label id="Label3" runat="server" Height="12px" Width="72px" Font-Bold="True" Font-Names="Verdana"
																							Font-Size="XX-Small" ForeColor="DimGray">Notes</asp:label><BR>
																						<asp:TextBox id="txtNotes" runat="server" Height="105px" Width="300px" CssClass="txtNoFocus"
																							TextMode="MultiLine"></asp:TextBox></TD>
																					<TD rowSpan="4">
																						<asp:Button id="btnRaiseRPA" Width="100" CssClass=" " Font-Names="Verdana" Font-Size="XX-Small"
																							Text="Raise RPA" Runat="server"></asp:Button><BR>
																						<BR>
																						<asp:Button id="btnGetObjects" Width="100" Font-Names="Verdana" Font-Size="XX-Small" Text="Get Objects"
																							Runat="server"></asp:Button></TD>
																				</TR>
																				<TR>
																					<TD vAlign="top">
																						<asp:label id="Label5" runat="server" Height="12px" Width="72px" Font-Bold="True" Font-Names="Verdana"
																							Font-Size="XX-Small" ForeColor="DimGray">Template</asp:label><BR>
																						<asp:DropDownList id="DDLTemplate" Width="150px" CssClass="txtNoFocus" Runat="server"></asp:DropDownList></TD>
																					<TD>&nbsp;</TD>
																					<TD>&nbsp;</TD>
																				</TR>
																				<TR>
																					<TD style="HEIGHT: 30px" vAlign="top">
																						<asp:label id="Label2" runat="server" Height="12px" Width="96px" Font-Bold="True" Font-Names="Verdana"
																							Font-Size="XX-Small" ForeColor="DimGray">Project Name</asp:label><BR>
																						<asp:TextBox id="txtProject" Width="150px" CssClass="txtNoFocus" Runat="server" MaxLength="50"></asp:TextBox></TD>
																					<TD>&nbsp;</TD>
																					<TD>&nbsp;</TD>
																				</TR>
																				<TR>
																					<TD>
																						<asp:CheckBox id="cbWebGen" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Text="Web Generation"></asp:CheckBox><BR>
																					</TD>
																					<TD>&nbsp;</TD>
																					<TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlObject" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
																			Text="RPA Objects Info" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" TitleBackColor="Transparent"
																			TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="True" BorderColor="Indigo">
																			<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 300px">
																				<asp:DataGrid id="dgrObject" CssClass="Grid" BorderColor="#d4d4d4" Runat="server" AutoGenerateColumns="False"
																					CellPadding="0">
																					<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																					<ItemStyle CssClass="GridItem"></ItemStyle>
																					<HeaderStyle CssClass="GridHeader"></HeaderStyle>
																					<Columns>
																						<asp:TemplateColumn HeaderText="Select">
																							<ItemTemplate>
																								<p align="center">
																									<asp:CheckBox ID="chkSelect" Runat="server"></asp:CheckBox></p>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn HeaderText="Project">
																							<HeaderStyle Font-Bold="True" Width="150px"></HeaderStyle>
																							<ItemTemplate>
																								<asp:label ID="lblProject" Width="150px" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Project") %>'>
																								</asp:label>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn HeaderText="Object">
																							<HeaderStyle Font-Bold="True" Width="100px"></HeaderStyle>
																							<ItemTemplate>
																								<asp:label ID="lblObject" Width="100px" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Object") %>'>
																								</asp:label>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn HeaderText="ObjectType">
																							<HeaderStyle Font-Bold="True" Width="150px"></HeaderStyle>
																							<ItemTemplate>
																								<asp:label ID="lblObjectType" Width="150px" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ObjectType") %>'>
																								</asp:label>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn HeaderText="Status">
																							<ItemTemplate>
																								<p align="center">
																									<asp:Image ID="imgStatus" Runat="server"></asp:Image></p>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn ItemStyle-Width="250px" HeaderText="Comments">
																							<ItemTemplate>
																								<asp:TextBox ID="txtComment" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Comment") %>' Width="100%" MaxLength="255" CssClass="txtNoFocus">
																								</asp:TextBox>
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
			<INPUT type="hidden" name="txthiddenImage">
		</form>
	</body>
</html>
