<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchODBObject.aspx.vb" Inherits="MonitoringCenter_SearchODBObject" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<title>Database Objects</title>
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
					location.href="AB_Search.aspx";
					//Form1.submit();
				}
								
			function ConfirmDelete(varImgValue,varMessage)
				{
					
					
							if (globleID==null)
								{
									alert("Please select the row");
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
								if (varImgValue=='Delete')
								{
											if (globleID==null)
											{
												alert("Please select the row");
											}
											else
											{
											var confirmed
											confirmed=window.confirm("Do You Want To Delete the selected record ?");
											if(confirmed==true)
													{	
															Form1.reset()
													}		
											}

								}			
												
												if (varImgValue=='Close')
												{
															window.close();	
												}
								
								
								if (varImgValue=='Add')
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
								if (varImgValue=='Print')
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
						//alert(strTempName);
						globleID = nn;
						globlestrTempName = strTempName;
						document.Form1.txthidden.value=nn;
						//Form1.submit();
						
										var tableID='cpnlDatabaseObjects_GrdAddSerach'  //your datagrids id
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
					
					function KeyCheck55(nn,rowvalues)
					{
							//self.opener.templrefresh(globleID,globlestrTempName);
							//window.close(); 
						document.Form1.txthiddenImage.value='Edit';
						Form1.submit(); 
						
					}	
					
			function OpenW(varTable)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('AB_ViewColumns.aspx?TBLName='+varTable,'Search',480,440);
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
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
		<script id="clientEventHandlersJS" language="javascript">
<!--

function DIV1_onclick() {

}

//-->
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
														<TD>
															<asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button>
															<asp:imagebutton id="Imagebutton1" tabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
																CommandName="submit" ImageUrl="~/images/white.GIF"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelDBObj" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
																Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">DATABASE OBJECTS</asp:label></TD>
														<TD vAlign="bottom" align="left">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
															<IMG class="PlusImageCSS" title="Add" onclick="SaveEdit('Add');" alt="A" src="../Images/s2Add01.gif"
																border="0" name="tbrbtnNew">&nbsp; <IMG class="PlusImageCSS" title="Edit" onclick="SaveEdit('Edit');" alt="E" src="../Images/S2edit01.gif"
																border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" title="Search" onclick="SaveEdit('Search');" alt="S" src="../Images/s1search02.gif"
																border="0" name="tbrbtnSearch">&nbsp; <IMG class="PlusImageCSS" title="Reset" onclick="SaveEdit('Reset');" alt="R" src="../Images/reset_20.gif"
																border="0" name="tbrbtnReset"> <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"><IMG class="PlusImageCSS" id="Print" onmouseover="FP_swapImg(1,0,/*id*/'Save',/*url*/'../Images/S2Save01_s.gif')"
																title="Print Database Object Report" onclick="SaveEdit('Print');" onmouseout="FP_swapImg(0,0,/*id*/'Save',/*url*/'../Images/S2Save01.gif')" alt="" src="../Images/print2.jpg" border="0" name="tbrbtnSave">&nbsp;&nbsp;<IMG class="PlusImageCSS" title="Close" onclick="SaveEdit('Close');" alt="" src="../Images/s2close01.gif"
																border="0" name="tbrbtnClose">&nbsp;&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
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
																	<td colSpan="2"><cc1:collapsiblepanel id="cpnlErrorPanel" runat="server" Width="100%" Height="47px" BorderStyle="Solid"
																			BorderWidth="0px" BorderColor="Indigo" Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
																			TitleClickable="True" TitleBackColor="Transparent" Text="Error Message" ExpandImage="../Images/ToggleDown.gif"
																			CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
																				<TR>
																					<TD vAlign="middle">
																						<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
																					<TD>
																						<asp:ListBox id="lstError" runat="server" Height="64px" Width="600"></asp:ListBox></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel></td>
																</tr>
																<TR>
																	<td>
																		<cc1:collapsiblepanel id="cpnlDatabaseObjects" runat="server" Width="100%" Height="47px" BorderStyle="Solid"
																			BorderWidth="0px" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
																			TitleClickable="True" TitleBackColor="Transparent" Text="Database Objects" ExpandImage="../Images/ToggleDown.gif"
																			CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																			<DIV language="javascript" id="DIV1" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 400pt"
																				onclick="return DIV1_onclick()">
																				<TABLE id="Table1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																					align="left" border="0">
																					<TR>
																						<TD>
																							<ASP:PANEL id="Panel1" runat="server"></ASP:PANEL></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" align="left"><!--  **********************************************************************-->
																							<ASP:DATAGRID id="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px" Font-Names="Verdana"
																								ForeColor="MidnightBlue" BorderColor="Silver" CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left"
																								PageSize="50" DataKeyField="ID" CssClass="grid">
																								<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
																								<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
																								<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																								<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
																								<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
																								<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
																							</ASP:DATAGRID><!-- Panel for displaying Task Info --> <!-- Panel for displaying Action Info-->  <!-- ***********************************************************************--></TD>
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
