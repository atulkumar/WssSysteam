<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_SearchProcess, App_Web_zn3-f7gx" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Machines</title>
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
		<script>

			var globleID;
			var globlestrTempName;
			
			function HideContents()
				{
					parent.document.all("SideMenu1").cols="0,*";
					document.Form1.imgHide.style.visibility = 'hidden'; 
					document.Form1.ingShow.style.visibility = 'visible'; 				
				}
					
			function ShowContents()
				{
					document.Form1.ingShow.style.visibility = 'hidden'; 
					document.Form1.imgHide.style.visibility = 'visible'; 
					parent.document.all("SideMenu1").cols="18%,*";					
				}
					
			function Hideshow()
				{
					if (parent.document.all("SideMenu1").cols =="0,*")
					{
							document.Form1.imgHide.style.visibility = 'hidden'; 
							document.Form1.ingShow.style.visibility = 'visible'; 
					}
					else
					{
							document.Form1.ingShow.style.visibility = 'hidden'; 
							document.Form1.imgHide.style.visibility = 'visible'; 
					}
				}				
			
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
										
								if (varImgValue=='Close')
								{
											window.close();	
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
						
										var tableID='cpnlProcessSearch_GrdAddSerach'  //your datagrids id
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
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				bgColor="#e0e0e0" border="0">
				<TR>
					<TD align="left" bgColor="lightgrey">
						<TABLE id="Table11" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
							bgColor="#e0e0e0" border="0">
							<TR>
								<TD vAlign="middle" align="left" bgColor="lightgrey" style="WIDTH: 208px; HEIGHT: 23px">
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="200" bgColor="#e0e0e0" border="0">
										<TR>
											<TD align="left" bgColor="lightgrey">
												<asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button>
												<asp:imagebutton id="Imagebutton1" tabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
													CommandName="submit" ImageUrl="white.GIF"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
													name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
													name="ingShow">
												<asp:label id="lblTitleLabelProcesses" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
													Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">Processes</asp:label></TD>
										</TR>
									</TABLE>
								</TD>
								<td align="center" bgColor="lightgrey" style="HEIGHT: 23px">
									<TABLE id="Table3" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="200"
										bgColor="#e0e0e0" border="0">
										<TR>
											<TD vAlign="bottom" align="center" bgColor="lightgrey"><IMG class="PlusImageCSS" title="Close" onclick="SaveEdit('Close');" alt="" src="../Images/s2close01.gif"
													border="0" name="tbrbtnClose">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
												<IMG class="PlusImageCSS" title="Add" onclick="SaveEdit('Add');" alt="A" src="../Images/s2Add01.gif"
													border="0" name="tbrbtnNew">&nbsp; <IMG class="PlusImageCSS" title="Reset" onclick="SaveEdit('Reset');" height="20" alt="R"
													src="../Images/reset_20.gif" width="20" border="0" name="tbrbtnReset">&nbsp;
												<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"> <IMG class="PlusImageCSS" title="Edit" onclick="SaveEdit('Edit');" alt="E" src="../Images/S2edit01.gif"
													border="0" name="tbrbtnEdit">&nbsp; <IMG class="PlusImageCSS" title="Delete" onclick="ConfirmDelete('Delete','Are you sure you want to Delete the selected record ?');"
													alt="D" src="../Images/s2delete01.gif" border="0" name="tbrbtnDelete">&nbsp;
												<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;<IMG class="PlusImageCSS" title="Search" onclick="SaveEdit('Search');" alt="S" src="../Images/s1search02.gif"
													border="0" name="tbrbtnSearch">
											</TD>
										</TR>
									</TABLE>
								</td>
								<TD align="right" bgColor="lightgrey" style="HEIGHT: 23px">
									<TABLE id="Table4" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="200"
										bgColor="#e0e0e0" border="0">
										<TR>
											<TD vAlign="middle" align="right" bgColor="lightgrey"><FONT face="Verdana" size="1"><STRONG>&nbsp;&nbsp;</STRONG></FONT><IMG class="PlusImageCSS" id="Img1" title="Help" onclick="SaveEdit('Help');" alt="E"
													src="../Images/s1question02.gif" border="0" name="tbrbtnEdit"> <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
												<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			<TABLE id="Table16" borderColor="activeborder" height="676" cellSpacing="0" cellPadding="0"
				width="100%" border="2">
				<TR>
					<TD vAlign="top" colSpan="1">
						<!--  **********************************************************************-->
						<table style="BORDER-COLLAPSE: collapse" width="100%" border="0">
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
									<cc1:collapsiblepanel id="cpnlProcessSearch" runat="server" Width="100%" Height="47px" BorderStyle="Solid"
										BorderWidth="0px" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
										TitleClickable="True" TitleBackColor="Transparent" Text="Processes" ExpandImage="../Images/ToggleDown.gif"
										CollapseImage="../Images/ToggleUp.gif" Draggable="False">
										<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 400pt">
											<TABLE id="Table1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
												align="left" border="0">
												<TR>
													<TD>
														<asp:panel id="Panel1" runat="server"></asp:panel></TD>
												</TR>
												<TR>
													<TD vAlign="top" align="left"><!--  **********************************************************************-->
														<asp:datagrid id="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px" Font-Names="Verdana"
															ForeColor="MidnightBlue" BorderColor="Silver" CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left"
															PageSize="50" CssClass="grid" DataKeyField="ProcessCode">
															<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
															<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
															<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
															<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
															<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
															<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
														</asp:datagrid><!-- Panel for displaying Task Info --> <!-- Panel for displaying Action Info-->  <!-- ***********************************************************************--></TD>
												</TR>
											</TABLE>
										</DIV>
									</cc1:collapsiblepanel>
								</td>
							</TR>
						</table>
					</TD>
				</TR>
			</TABLE>
			<INPUT type="hidden" name="txthidden"> <INPUT type="hidden" name="txthiddenImage">
		</form>
	</body>
</html>
