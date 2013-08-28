<%@ Page Language="VB" AutoEventWireup="false" CodeFile="JobDetail.aspx.vb" Inherits="MonitoringCenter_JobDetail" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>WebForm4</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script>

		
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
					Form1.submit();
				}
								
			function ConfirmDelete(varImgValue,varMessage)
				{

				if (document.Form1.txtSelectHID.value!='selected')
					{
						alert("Please select the row");
					}
				else
					{												
						var confirmed
						confirmed=window.confirm(varMessage);
						
						if(confirmed==true)
							{
								document.Form1.txthidden.value=varImgValue;
								Form1.submit(); 
							}
						else
							{
							}					
					}										
				}
				
			function SaveEdit(varImgValue)
				{
					if (varImgValue=='Logout')
						{
							document.Form1.txthidden.value=varImgValue;
							Form1.submit(); 
						}	
						
					if (varImgValue=='Print')
						{
							document.Form1.txthidden.value=varImgValue;
							Form1.submit(); 
						}	
							
					if (varImgValue=='Reset')
						{
							document.Form1.txthidden.value=varImgValue;
							Form1.submit(); 
						}	
					if (varImgValue=='Edit')
						{
							document.Form1.txthidden.value=varImgValue;
							Form1.submit(); 
						}	
					
					
			    	//document.Form1.txthidden.value=varImgValue;
					//Form1.submit(); 
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
						<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="250" bgColor="#e0e0e0" border="0">
							<TR>
								<TD align="left" bgColor="lightgrey"><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" CommandName="submit" AlternateText="."
										ImageUrl="white.GIF" Width="1px" Height="1px"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
										name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
										name="ingShow">
									<asp:label id="lblTitleLabelJobDetail" runat="server" Font-Size="X-Small" Font-Names="Verdana"
										Font-Bold="True" ForeColor="Teal" BorderStyle="None" BorderWidth="2px">Job Detail</asp:label></TD>
							</TR>
						</TABLE>
					</TD>
					<TD style="WIDTH: 386px" borderColor="#e0e0e0" borderColorLight="#e0e0e0" align="center"
						bgColor="lightgrey" borderColorDark="#e0e0e0"><TABLE id="Table6" cellSpacing="0" cellPadding="0" width="300" bgColor="#e0e0e0" border="0">
							<TR>
								<TD align="center" bgColor="lightgrey"><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
									<IMG class="PlusImageCSS" id="Ok" onmouseover="FP_swapImg(1,0,/*id*/'Ok',/*url*/'../Images/s1ok02_s.gif')"
										title="Ok" onclick="SaveEdit('Ok');" onmouseout="FP_swapImg(0,0,/*id*/'Ok',/*url*/'../Images/s1ok02.gif')"
										alt="" src="../Images/s1ok02.gif" border="0" name="tbrbtnOk">&nbsp;&nbsp; <IMG class="PlusImageCSS" id="Reset" onmouseover="FP_swapImg(1,0,/*id*/'Reset',/*url*/'../Images/reset_20_s.gif')"
										title="Reset" onclick="SaveEdit('Reset');" onmouseout="FP_swapImg(0,0,/*id*/'Reset',/*url*/'../Images/reset_20.gif')" height="28" alt="R" src="../Images/reset_20.gif"
										width="40" border="0" name="tbrbtnReset" style="WIDTH: 40px; HEIGHT: 28px"><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
									<IMG class="PlusImageCSS" id="Save" onmouseover="FP_swapImg(1,0,/*id*/'Save',/*url*/'../Images/S2Save01_s.gif')"
										title="Save" onclick="SaveEdit('Print');" onmouseout="FP_swapImg(0,0,/*id*/'Save',/*url*/'../Images/S2Save01.gif')"
										alt="" src="../Images/print2.jpg" border="0" name="tbrbtnSave">&nbsp;<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
									<IMG class="PlusImageCSS" id="Close" onmouseover="FP_swapImg(1,0,/*id*/'Close',/*url*/'../Images/s2close01_s.gif')"
										title="Close" onclick="SaveEdit('Close');" onmouseout="FP_swapImg(0,0,/*id*/'Close',/*url*/'../Images/s2close01.gif')"
										alt="" src="../Images/s2close01.gif" border="0" name="tbrbtnClose">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"></TD>
							</TR>
						</TABLE>
					</TD>
					<TD align="right" bgColor="lightgrey"><FONT face="Verdana" size="1">
							<TABLE id="Table2" style="HEIGHT: 24px" cellSpacing="0" cellPadding="0" width="250" bgColor="#e0e0e0"
								border="0">
								<TR>
									<TD align="right" bgColor="lightgrey"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
											src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp; <IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
											border="0" name="tbrbtnEdit">&nbsp;
									</TD>
								</TR>
							</TABLE>
						</FONT>
					</TD>
				</TR>
			</TABLE>
			<TABLE id="Table16" borderColor="activeborder" height="94%" cellSpacing="0" cellPadding="0"
				width="100%" border="2">
				<TR>
					<TD vAlign="top" colSpan="1">
						<!--  **********************************************************************-->
						<table style="BORDER-COLLAPSE: collapse" width="100%" border="0">
							<tr>
								<td colSpan="2"><cc1:collapsiblepanel id="cpnlErrorPanel" runat="server" Width="100%" Height="47px" BorderWidth="0px"
										BorderStyle="Solid" BorderColor="Indigo" Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
										TitleClickable="True" TitleBackColor="Transparent" Text="Error Message" ExpandImage="../../Images/ToggleDown.gif"
										CollapseImage="../../Images/ToggleUp.gif" Draggable="False">
										<TABLE id="Table3" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
											<TR>
												<TD colSpan="0" rowSpan="0">
													<asp:Image id="Image1" runat="server" Height="16px" Width="16px" ImageUrl="../../Images/warning.gif"></asp:Image></TD>
												<TD colSpan="0" rowSpan="0">
													<asp:ListBox id="lstError" runat="server" Width="520px" ForeColor="Red" Font-Names="Verdana"
														Font-Size="XX-Small"></asp:ListBox>&nbsp;&nbsp;&nbsp;</TD>
											</TR>
										</TABLE>
									</cc1:collapsiblepanel></td>
							</tr>
							<tr>
								<td vAlign="top" colSpan="2" height="1"><cc1:collapsiblepanel id="cpnlUDCType" runat="server" Width="100%" Height="52px" BorderWidth="0px" BorderStyle="Solid"
										BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
										Text="UBE Header" ExpandImage="../Images/ToggleDown1.gif" CollapseImage="../Images/ToggleUp1.gif" Draggable="False"><!--  **********************************************************************-->
										<TABLE id="Table126" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<TR>
												<TD colSpan="1">
													<TABLE id="Table7" cellSpacing="0" cellPadding="0" width="100%" border="0">
														<TR>
															<TD vAlign="top" align="left">
																<asp:panel id="pnlUDCTypeTxtbox" runat="server" Height="4px"></asp:panel>
																<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 200px">
																	<asp:datagrid id="grdUDCType" runat="server" BorderWidth="1px" BorderStyle="None" ForeColor="MidnightBlue"
																		Font-Names="Verdana" BorderColor="Silver" CellPadding="1" HorizontalAlign="left" BackColor="#E0E0E0"
																		PagerStyle-Visible="False" AutoGenerateColumns="False">
																		<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
																		<SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
																		<AlternatingItemStyle BackColor="WhiteSmoke"></AlternatingItemStyle>
																		<ItemStyle Font-Size="8pt" ForeColor="Black" BackColor="White"></ItemStyle>
																		<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White" CssClass="GridFixedHeader"
																			BackColor="#E0E0E0"></HeaderStyle>
																		<Columns>
																			<asp:ButtonColumn Visible="False" CommandName="select"></asp:ButtonColumn>
																			<asp:BoundColumn DataField="UH_NU12_UID" HeaderText="UID">
																				<HeaderStyle Width="60pt"></HeaderStyle>
																				<ItemStyle Width="60pt"></ItemStyle>
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="UH_NU12_URQID" HeaderText="Request ID">
																				<HeaderStyle Width="70pt"></HeaderStyle>
																				<ItemStyle Width="70pt"></ItemStyle>
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="UH_NU12_URSID" HeaderText="Response ID">
																				<HeaderStyle Width="154pt"></HeaderStyle>
																				<ItemStyle Width="154pt"></ItemStyle>
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="UH_NU12_UTRAN" HeaderText="Transaction">
																				<HeaderStyle Width="60pt"></HeaderStyle>
																				<ItemStyle Width="60pt"></ItemStyle>
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="UH_NU12_UTERR" HeaderText="Error">
																				<HeaderStyle Width="60pt"></HeaderStyle>
																				<ItemStyle Width="60pt"></ItemStyle>
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="UH_DT8_URDate" HeaderText="Request Date">
																				<HeaderStyle Width="90pt"></HeaderStyle>
																				<ItemStyle Width="90pt"></ItemStyle>
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="UH_NU6_URTime" HeaderText="Response Time">
																				<HeaderStyle Width="90pt"></HeaderStyle>
																				<ItemStyle Width="90pt"></ItemStyle>
																			</asp:BoundColumn>
																		</Columns>
																		<PagerStyle Visible="False" Font-Size="XX-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="DarkBlue"
																			Position="TopAndBottom" BackColor="#CDD7ED" Mode="NumericPages"></PagerStyle>
																	</asp:datagrid></DIV>
															</TD>
														</TR>
														<TR>
															<TD vAlign="top" align="left"></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
										<DIV></DIV>
									</cc1:collapsiblepanel></td>
							</tr>
							<tr>
								<td vAlign="top"><cc1:collapsiblepanel id="cpnlUDC" runat="server" Width="100%" Height="57px" BorderWidth="0px" BorderStyle="Solid"
										BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
										TitleBackColor="Transparent" Text="UDE Detail" ExpandImage="../Images/ToggleDown1.gif" CollapseImage="../Images/ToggleUp1.gif"
										Draggable="False">
										<TABLE width="100%" border="0">
											<TR>
												<TD>
													<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 277px">
														<asp:panel id="pnlUDC" runat="server" Height="4px" Visible="False"></asp:panel>
														<asp:datagrid id="grdUDC" runat="server" Width="200%" BorderWidth="1px" BorderStyle="None" ForeColor="MidnightBlue"
															Font-Names="Verdana" BorderColor="Silver" CellPadding="1" HorizontalAlign="left" BackColor="#E0E0E0"
															PagerStyle-Visible="False" AutoGenerateColumns="False">
															<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
															<SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
															<AlternatingItemStyle BackColor="WhiteSmoke"></AlternatingItemStyle>
															<ItemStyle Font-Size="8pt" ForeColor="Black" BackColor="White"></ItemStyle>
															<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White" CssClass="GridfixedHeader"
																BackColor="#E0E0E0"></HeaderStyle>
															<Columns>
																<asp:ButtonColumn Visible="False" CommandName="Select"></asp:ButtonColumn>
																<asp:BoundColumn DataField="UD_NU12_UDUID" HeaderText="UID">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_NU12_UDLNID" HeaderText="Line ID">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_VC50_UDName" HeaderText="Name">
																	<HeaderStyle Width="70pt"></HeaderStyle>
																	<ItemStyle Width="70pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_VC15_UDQUE" HeaderText="Que">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_VC10_UDENV" HeaderText="Env">
																	<HeaderStyle Width="80pt"></HeaderStyle>
																	<ItemStyle Width="80pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_VC10_UDUser" HeaderText="User">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_NU12_UDNum" HeaderText="UDNum">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_VC20_UDHost" HeaderText="Host">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_VC20_UDOrg" HeaderText="Organization">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_DT8_UDSDate" HeaderText="SDate">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_NU6_UDSTime" HeaderText="STime">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_DT8_UDADate" HeaderText="ADate">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_NU6_UDATime" HeaderText="ATime">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="UD_VC1_UDSTS" HeaderText="Status">
																	<HeaderStyle Width="60pt"></HeaderStyle>
																	<ItemStyle Width="60pt"></ItemStyle>
																</asp:BoundColumn>
															</Columns>
															<PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
														</asp:datagrid></DIV>
												</TD>
											</TR>
										</TABLE>
									</cc1:collapsiblepanel></td>
							</tr>
						</table>
					</TD>
				</TR>
			</TABLE>
			<INPUT type="hidden" name="txthidden"> <INPUT id="txtSelectHID" type="hidden" name="txthiddenSelect" runat="server">
		</form>
	</body>
</html>
