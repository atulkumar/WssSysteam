<%@ page language="VB" autoeventwireup="false" inherits="ComingSoon, App_Web_n44rks5n" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
  <HEAD>
		<title>ComingSoon</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
				<script language="javascript" src="Images/Js/JSValidation.js"></script>
	<LINK rel="stylesheet" type="text/css" href="Images/Js/StyleSheet1.css">
		<script>

			var globleID;
			
		
		
				function SaveEdit(varImgValue)
				{

						if (varImgValue=='Logout')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
						}		
						

				}			
		</script>
</HEAD>
		<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
							<tr>
								<td><div align="right"><img src="Images/top_right_line.gif" width="96" height="2"></div>
								</td>
							</tr>
							<tr>
								<td><table border="0" cellspacing="0" cellpadding="0" style="width:118%">
										<tr>
											<td background="Images/top_left_back.gif">&nbsp;</td>
											<td width="50"><img src="Images/top_right.gif" width="50" height="20"></td>
											<td width="21"><a href="#"><img src="Images/bt_min.gif" width="21" height="20" border="0"></a></td>
											<td width="21"><a href="#"><img src="Images/bt_max.gif" width="21" height="20" border="0"></a></td>
											<td width="19"><a href="#"><img onclick="CloseWSS();"  src="Images/bt_clo.gif" width="19" height="20" border="0"></a></td>
											<td width="6"><img src="Images/bt_space.gif" width="6" height="20"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr width="100%">
											<td background="Images/top_nav_back.gif" height="67">
												<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                   
													<TR>
														<TD>
														<span style="display:none">
									<asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button></span><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="white.GIF"
										CommandName="submit" AlternateText="."></asp:imagebutton><IMG onclick="HideContents();" alt="Hide" src="Images/left005.gif" name="imgHide" class="PlusImageCSS">
									<IMG onclick="ShowContents();" alt="Show" src="Images/Right005.gif" name="ingShow" class="PlusImageCSS">
									<asp:label id="Label2" runat="server" Width="169px" Height="12px" BorderStyle="None" BorderWidth="2px"
										Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal">WSS COMING FEATURE</asp:label>
								</TD>
								</TR>
						</table>
							</tr>
							</table>
							</td>
											<td align="right" width="152" background="Images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
													src="Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
													src="icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="Images/main_line.gif" height="10"><img src="Images/main_line.gif" width="6" height="10"></td>
											<td width="7" height="10"><img src="Images/main_line01.gif" width="7" height="10"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2"><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="Images/main_line02.gif" height="2"><img src="Images/main_line02.gif" width="2" height="2"></td>
											<td width="12" height="2"><img src="Images/main_line03.gif" width="12" height="2"></td>
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
																	<td colSpan="2">
			<asp:Image id="Image1" style="Z-INDEX: 101; LEFT: 190px; POSITION: absolute; TOP: 192px" runat="server"
				ImageUrl="Images/under-construction.gif"></asp:Image>
			<INPUT type="hidden" name="txthiddenImage">
			</td>
																</tr>
															</TBODY>
														</table>
													</DIV>
												</TD>
												<td width="12" valign="top" background="Images/main_line04.gif"><img src="Images/main_line04.gif" width="12" height="1"></td>
											</TR>
										</TABLE>
									</DIV>
								</td>
							</tr>
							<tr>
								<td height="2"><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="Images/main_line06.gif" height="2"><img src="Images/main_line06.gif" width="2" height="2"></td>
											<td width="12" height="2"><img src="Images/main_line05.gif" width="12" height="2"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td background="Images/bottom_back.gif">&nbsp;</td>
											<td width="66"><img src="Images/bottom_right.gif" width="66" height="31"></td>
										</tr>
									</table>
								</td>
							</tr>
							</td>
							</tr>
						</table>
		</form>
	</body>
</html>