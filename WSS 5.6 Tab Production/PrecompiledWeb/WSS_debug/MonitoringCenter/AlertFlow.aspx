<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_AlertFlow, App_Web_2azc-idb" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Alerts Flow</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<script>

			var globleID;
			var globlestrTempName;
			
	
			
			function callrefresh()
				{
				//alert();
					//location.href="alert.aspx";
					Form1.submit();
				}
								
			function ConfirmDelete(varImgValue,varMessage)
				{
					
					
							if (globleID==null)
								{
									alert("Please Select the Record");
								}
								else
								{
									var confirmed
									confirmed=window.confirm(varMessage);
									if(confirmed==true)
											{
											    document.Form1.txthiddenImage.value=varImgValue;
												Form1.submit(); 
											}
											else
											{
											}	
								}
				}
				
				
				
			function SaveEdit(varImgValue)
				{
			    				if (varImgValue=='Edit')
								{
											if (globleID==null)
											{
												alert("Please select the row");
											}
											else
											{
												document.Form1.txthiddenImage.value=varImgValue;
												Form1.submit(); 
											}
											
								}	
										
								if (varImgValue=='Close')
								{
											window.close();	
								}
				
						
								if (varImgValue=='Save')
												{
													document.Form1.txthiddenImage.value=varImgValue;
													Form1.submit();
													  
												}	
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								if (varImgValue=='Close')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								if (varImgValue=='Reset')
									{
												var confirmed
												confirmed=window.confirm("Do You Want To reset The Page ?");
												if(confirmed==true)
														{	
																Form1.reset()
														}		

									}			
				}				
				
				
				function KeyCheck(nn,rowvalues,strTempName)
					{
						//alert(nn);
						globleID = nn;
						globlestrTempName = strTempName;
						document.Form1.txthidden.value=nn;
						//Form1.submit();
						
										var tableID='cpnlViewJobs_GrdAddSerach'  //your datagrids id
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
					
					function KeyCheck55(ID,rowvalues)
					{
							//self.opener.templrefresh(globleID,globlestrTempName);
							//window.close(); 
						//	alert(rowvalues);
						location.href="AlertFlowEdit.aspx";
						//wopen("AlertEdit.aspx?ID="+ID,"AlertEdit",450,400);							
						//'document.Form1.txthiddenImage.value='Edit';
						//Form1.submit(); 
						
					}	
					
		
				
		function wopen(url, name, w, h)
			{
				// Fudge factors for window decoration space.
				// In my tests these work well on all platforms & browsers.
				w += 32;
				h += 96;
				wleft = (screen.width - w) / 2;
				wtop = (screen.height - h) / 2;
				var win = window.open(url,
					name,
					'width=' + w + ', height=' + h + ', ' +
					'left=' + wleft + ', top=' + wtop + ', ' +
					'location=no, menubar=no, ' +
					'status=no, toolbar=no, scrollbars=no, resizable=no');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}
				
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0">
		<FORM id="Form1" method="post" runat="server">
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
											<td width="19"><a href="#"><img onclick="CloseWSS();" src="../Images/bt_clo.gif" width="19" height="20" border="0"></a></td>
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
														<TD><asp:button id="BtnGrdSearch" runat="server" Height="0" Width="0px"></asp:button><asp:imagebutton id="Imagebutton1" tabIndex="1" runat="server" Height="1px" Width="1px" ImageUrl="~/images/white.GIF"
																CommandName="submit" AlternateText="."></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelAlertFlow" runat="server" BorderStyle="None" BorderWidth="2px"
																Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal">Alert Flow</asp:label></TD>
														<TD vAlign="bottom" align="center"><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Save" title="Save" onclick="SaveEdit('Save');" alt="" src="../Images/S2Save01.gif"
																border="0" name="tbrbtnSave">&nbsp;&nbsp;<IMG class="PlusImageCSS" title="Close" onclick="SaveEdit('Close');" alt="" src="../Images/s2close01.gif"
																border="0" name="tbrbtnClose">&nbsp;<IMG class="PlusImageCSS" title="Delete" onclick="ConfirmDelete('Delete','Are you sure you want to Delete the selected record ?');"
																alt="D" src="../Images/s2delete01.gif" border="0" name="tbrbtnDelete">&nbsp;&nbsp;<IMG class="PlusImageCSS" title="Reset" onclick="SaveEdit('Reset');" alt="R" src="../Images/reset_20.gif"
																border="0" name="tbrbtnReset">&nbsp;&nbsp;<IMG class="PlusImageCSS" title="Search" onclick="SaveEdit('Search');" alt="S" src="../Images/s1search02.gif"
																border="0" name="tbrbtnSearch">&nbsp;&nbsp;<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
														</TD>
														<TD></TD>
														<td>&nbsp;
														</td>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../Images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
													src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;&nbsp;</td>
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
																	<td colSpan="2"><cc1:collapsiblepanel id="cpnlError" runat="server" Height="47px" Width="100%" BorderStyle="Solid" BorderWidth="0px"
																			Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Error Message"
																			TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="False"
																			BorderColor="Indigo">
																			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
																				<TR>
																					<TD vAlign="middle">
																						<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
																					<TD>
																						<asp:ListBox id="lstError" runat="server" Width="600" Height="64px" Font-Size="XX-Small"></asp:ListBox></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel></td>
																</tr>
																<TR>
																	<td><cc1:collapsiblepanel id="cpnlViewJobs" runat="server" Height="47px" Width="100%" BorderStyle="Solid"
																			BorderWidth="0px" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
																			Text="Alert Flow" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
																			TitleCSS="test" Visible="True" BorderColor="Indigo">
																			<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 400pt">
																				<TABLE id="Table1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																					align="left" border="0">
																					<TR>
																						<TD>
																							<asp:panel id="Panel1" runat="server"></asp:panel></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" align="left"><!--  **********************************************************************-->
																							<asp:datagrid id="GrdAddSerach" runat="server" ForeColor="MidnightBlue" Font-Names="Verdana" BorderWidth="1px"
																								BorderStyle="None" BorderColor="Silver" CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left"
																								PageSize="50" CssClass="grid" DataKeyField="AlertID">
																								<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
																								<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																								<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
																								<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
																								<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
																								<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
																							</asp:datagrid><!-- Panel for displaying Task Info --> <!-- Panel for displaying Action Info-->  <!-- ***********************************************************************--></TD>
																					</TR>
																				</TABLE>
																			</DIV>
																		</cc1:collapsiblepanel></td>
												</TD>
											</TR>
										</TABLE>
									</DIV>
								</td>
								<td width="12" valign="top" background="../Images/main_line04.gif"><img src="../Images/main_line04.gif" width="12" height="1"></td>
							</tr>
						</table>
						
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
			
			<DIV></DIV>
			<INPUT type="hidden" name="txthidden"> <INPUT type="hidden" name="txthiddenImage">
		</FORM>
	</body>
</html>
