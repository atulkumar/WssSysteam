<%@ page language="VB" autoeventwireup="false" inherits="ImportExport_Group_View, App_Web_p89p0wkj" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script>

		function GroupClick()
		{
	
			setTimeout('return false;',6000);
		
		}
					
		function OpenUserInfo(ID)
		{
			wopen('../SupportCenter/CallView/UserInfo.aspx?ADDNO=' + ID ,ID,350,500);
			return false;
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
				function SaveEdit(varImgValue)
				{
			    			if (varImgValue=='Edit')
												{
															if  (document.Form1.txthiddenAdno.value=="")
															{
																 alert("Please select the row");
																 return false;
															}
															else
															{
															 //alert(document.Form1.txthiddenAdno.value);
																 document.Form1.txthiddenImage.value=varImgValue;
																 Form1.submit(); 
																 return false;
															}
															
												}	
												
												if (varImgValue=='Close')
												{
																 document.Form1.txthiddenImage.value=varImgValue;
																 Form1.submit(); 
																 return false;
												}
								
								
								if (varImgValue=='Add')
												{
													 document.Form1.txthiddenImage.value=varImgValue;
													Form1.submit();
													return false;
													  
												}	
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
									return false;
								}	
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								if (varImgValue=='Reset')
									{
												var confirmed
												confirmed=window.confirm("Do  You Want To reset The Page ?");
												if(confirmed==true)
														{	
																 Form1.reset()
																 return false;
														}		
														else
														{
														return false;
														}

									}			
				}				
		</script>
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0"
		MS_POSITIONING="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>

    <table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td vAlign="top">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<div align="right"><IMG height="2" src="../Images/top_right_line.gif" width="96"></div>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../Images/top_left_back.gif">&nbsp;</td>
											<td width="50"><IMG height="20" src="../Images/top_right.gif" width="50"></td>
											<td width="21"><A href="#"><IMG height="20" src="../Images/bt_min.gif" width="21" border="0"></A></td>
											<td width="21"><A href="#"><IMG height="20" src="../Images/bt_max.gif" width="21" border="0"></A></td>
											<td width="19"><A href="#"><IMG onclick="CloseWSS();" height="20" src="../Images/bt_clo.gif" width="19" border="0"></A></td>
											<td width="6"><IMG height="20" src="../Images/bt_space.gif" width="6"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr width="100%">
											<td background="../Images/top_nav_back.gif" height="67">
												<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
													<TR>
														<TD vAlign="middle"><asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="white.GIF"
																CommandName="submit" AlternateText="."></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelRoleSearch" runat="server" Width="136px" Height="12px" BorderStyle="None"
																BorderWidth="2px" CssClass="TitleLabel"> LIST VIEW</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
															<asp:imagebutton id="imgSearch" accessKey="H" runat="server" ImageUrl="../Images/s1search02.gif"
																ToolTip="Search"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgClose" accessKey="L" runat="server" ImageUrl="../Images/s2close01.gif" ToolTip="Close"></asp:imagebutton>&nbsp;
															<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"></TD>
														<TD></TD>
														<td>&nbsp;
														</td>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../Images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('962','../');" alt="E"
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
																	<td colSpan="2"><cc1:collapsiblepanel id="cpnlErrorPanel" runat="server" Width="100%" Height="47px" BorderStyle="Solid"
																			BorderWidth="0px" BorderColor="Indigo" Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
																			TitleClickable="True" TitleBackColor="Transparent" Text="Error Message" ExpandImage="../Images/ToggleDown.gif"
																			CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
																				<TR>
																					<TD colSpan="0" rowSpan="0">
																						<asp:Image id="Image1" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
																					<TD colSpan="0" rowSpan="0">&nbsp;
																						<asp:Label id="lblError" runat="server" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:Label>
																						<asp:ListBox id="lstError" runat="server" Width="752px" BorderWidth="0" BorderStyle="Groove"
																							Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel></td>
																</tr>
																<TR>
																	<td>
																		<TABLE id="Table1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																			align="left" border="0">
																			<tr>
																				<td><asp:panel id="Panel12" runat="server"></asp:panel></td>
																			</tr>
																			<TR>
																				<TD vAlign="top" align="left">
																					<!--  **********************************************************************--><cc1:collapsiblepanel id="cpnlGroup" runat="server" Width="100%" Height="47px" BorderStyle="Solid" BorderWidth="0px"
																						BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Group View" ExpandImage="../Images/ToggleDown.gif"
																						CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																						<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 250px">
																							<TABLE id="Tables1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																								align="left" border="0">
																								<TR>
																									<TD>
																										<asp:DataList id="lstGroups" DataKeyField="GroupID" Runat="server" RepeatLayout="Table" RepeatColumns="7"
																											RepeatDirection="Horizontal">
																											<ItemStyle Width="100px" Height="100px" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
																											<ItemTemplate>
																												<TABLE id="Table1" borderColor="#5c5a5b" bgColor="#f5f5f5" border="1" width="100px" height="100px">
																													<TR>
																														<TD borderColor="#f5f5f5" valign="top" align="center">
																															<asp:ImageButton CommandName="GroupName" CommandArgument='<%#Container.Dataitem("GroupName") %>' ID="imgGroup" Runat="server" ImageUrl="..\Images\Folders.jpg" Width="60px">
																															</asp:ImageButton><br>
																															<asp:Label l CssClass="FieldLabel" ID="lblGroupName" Runat="server" Width="100px">
																																<%#Container.Dataitem("GroupName") %>
																															</asp:Label>
																														</TD>
																													</TR>
																												</TABLE>
																											</ItemTemplate>
																										</asp:DataList></TD>
																								</TR>
																							</TABLE>
																						</DIV>
																					</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlFiles" runat="server" Width="100%" Height="47px" BorderStyle="Solid" BorderWidth="0px"
																						BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
																						Text="File View" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																						<DIV id="ss" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 240px">
																							<TABLE id="1262" cellSpacing="0" cellPadding="0" width="200%" align="left" border="0">
																								<TR>
																									<TD>
																										<asp:panel id="Panel1" runat="server"></asp:panel></TD>
																								</TR>
																								<TR>
																									<TD vAlign="top"><!--  **********************************************************************-->
																										<asp:datagrid id="GrdAddSerach" runat="server" CssClass="Grid" BorderWidth="1px" BorderStyle="None"
																											BorderColor="Silver" Font-Names="Verdana" ForeColor="MidnightBlue" CellPadding="0" GridLines="Horizontal"
																											HorizontalAlign="Left" PageSize="50" AutoGenerateColumns="False">
																											<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																											<ItemStyle CssClass="GridItem"></ItemStyle>
																											<HeaderStyle CssClass="GridHeader"></HeaderStyle>
																											<Columns>
																												<asp:TemplateColumn HeaderText="Attachment">
																													<HeaderStyle Width="80pt"></HeaderStyle>
																													<ItemStyle Width="80pt"></ItemStyle>
																													<ItemTemplate>
																														<asp:HyperLink Target="_blank" ID="HylDownload" Text="Download" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.FilePath") %>'>
																														</asp:HyperLink>
																													</ItemTemplate>
																												</asp:TemplateColumn>
																												<asp:BoundColumn DataField="FileName" HeaderText="FileName">
																													<HeaderStyle Width="200pt"></HeaderStyle>
																													<ItemStyle Width="200pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn DataField="CompanyName" HeaderText="CompanyName">
																													<HeaderStyle Width="120pt"></HeaderStyle>
																													<ItemStyle Width="120pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn DataField="FileGroup" HeaderText="FileGroup">
																													<HeaderStyle Width="100pt"></HeaderStyle>
																													<ItemStyle Width="100pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn DataField="Description" HeaderText="Description">
																													<HeaderStyle Width="300pt"></HeaderStyle>
																													<ItemStyle Width="300pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn DataField="FileSize" HeaderText="FileSize">
																													<HeaderStyle Width="50pt"></HeaderStyle>
																													<ItemStyle Width="50pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn DataField="FileVersion" HeaderText="FileVersion">
																													<HeaderStyle Width="65pt"></HeaderStyle>
																													<ItemStyle Width="65pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn DataField="UploadDate" HeaderText="UploadDate">
																													<HeaderStyle Width="100pt"></HeaderStyle>
																													<ItemStyle Width="100pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn DataField="UploadedBy" HeaderText="UploadedBy">
																													<HeaderStyle Width="80pt"></HeaderStyle>
																													<ItemStyle Width="80pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn Visible="False" DataField="FilePath" HeaderText="FilePath">
																													<HeaderStyle Width="200pt"></HeaderStyle>
																													<ItemStyle Width="200pt"></ItemStyle>
																												</asp:BoundColumn>
																												<asp:BoundColumn Visible="False" DataField="FM_NU9_File_ID_PK" HeaderText="FileID"></asp:BoundColumn>
																											</Columns>
																											<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
																										</asp:datagrid></TD>
																								</TR>
																							</TABLE>
																						</DIV>
																					</cc1:collapsiblepanel>
																					<!-- Panel for displaying Task Info --> <!-- Panel for displaying Action Info-->  <!-- ***********************************************************************--></TD>
																			</TR>
																		</TABLE>
																	</td>
												</TD>
											</TR>
										</TABLE>
									</DIV>
								</td>
								<td vAlign="top" width="12" background="../Images/main_line04.gif"><IMG height="1" src="../Images/main_line04.gif" width="12"></td>
							</tr>
						</table>
						</DIV></td>
				</tr>
				<tr>
					<td height="2">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td background="../Images/main_line06.gif" height="2"><IMG height="2" src="../Images/main_line06.gif" width="2"></td>
								<td width="12" height="2"><IMG height="2" src="../Images/main_line05.gif" width="12"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td background="../Images/bottom_back.gif">&nbsp;</td>
								<td width="66"><IMG height="31" src="../Images/bottom_right.gif" width="66"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			</TD></TR></TBODY></TABLE></TD></TR></TABLE>
			<DIV></DIV>
			<INPUT type="hidden" name="txthidden"> <INPUT type="hidden" name="txthiddenImage">
    </form>
</body>
</html>
