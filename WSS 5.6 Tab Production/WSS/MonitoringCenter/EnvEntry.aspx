<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EnvEntry.aspx.vb" Inherits="MonitoringCenter_EnvEntry" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>EnvEntry</title>
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
		<script language="javascript">
				function Post(Action)
		{
			//alert(Action);
			SaveEdit(Action);
		}
			function SaveEdit(imgValue)
			{
			
					if ( imgValue=='Delete' )
					{
					if(document.Form1.txtHiddenID.value=='')
							{
							alert("Please select row");
							}
							else
						{	
						document.Form1.txthiddenImage.value=imgValue;
						document.Form1.submit();
					    }
					    }
					 
					   	if (imgValue=='Add')
					       {
					        alert("Add button is not for this screen");
					       }
					       
					       	if (imgValue=='Close')
						{
						 	document.Form1.txthiddenImage.value=imgValue;
							document.Form1.submit();
						}						
				
					if (imgValue=='Save')
					{
						document.Form1.txthiddenImage.value=imgValue;
						document.Form1.submit();
					}	
								
					return false;
			}
			
		
			function GridClick(rowvalues,ID)
			{
			     	var tableID='cpnlEnvEntry_dgrEnvDetail';
			     		var table;
										document.Form1.txtHiddenID.value=ID;
								
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
			
			
		function KeyCheck55(rowvalues,ID)
					{
						
						//Popup window open
						wopen("ProcessEnvironmentEntry.aspx?ID="+ID,"AlertEdit",450,400);							
						//'document.Form1.txthiddenImage.value='Edit';
						//Form1.submit(); 
						
					}
					
	function wopen(url, name, w, h)
   {
    // Fudge factors for window decoration space.
    // In my tests these work well on all platforms & browsers.
    w += 26;
    h += 50;
    wleft = (screen.width - w) / 2;
    wtop = (screen.height - h) / 2;
    var win = window.open(url,
     name,
     'width=' + w + ', height=' + h + ', ' +
     'left=' + wleft + ', top=' + wtop + ', ' +
     'location=no, menubar=no, ' +
     'status=no, toolbar=no, scrollbars=No, resizable=no');
    // Just in case width and height are ignored
    win.resizeTo(w, h);
    // Just in case left and top are ignored
    win.moveTo(wleft, wtop);
    win.focus();
   }
	  
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td><asp:button id="BtnGrdSearch" runat="server" Height="0" Width="0px"></asp:button></td>
					<td><cc1:collapsiblepanel id="cpnlError" runat="server" Height="54px" Width="100%" BorderStyle="Solid" BorderWidth="0px"
							Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Error Message"
							TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
							Visible="False" BorderColor="#f5f5f5">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD colSpan="0" rowSpan="0">
										<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
									<TD colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" Width="552px" BorderWidth="0" BorderStyle="Groove"
											ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlEnvEntry" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
							Draggable="False" CollapseImage="../Images/To&#9;ggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Env Entry" TitleBackColor="Transparent"
							TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="True" BorderColor="#f5f5f5">
							<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 480px">
								<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
									<TR>
										<TD style="HEIGHT: 14px">
											<asp:panel id="Panel5" runat="server">
												<asp:panel id="Panel6" runat="server">
													<TABLE height="5">
														<TR>
															<TD></TD>
															<TD></TD>
															<TD>&nbsp;
																<asp:Label id="pg" Width="40px" ForeColor="#0000C0" Font-Names="Verdana" Font-Size="8pt" Font-Bold="True"
																	Runat="server">Page</asp:Label></TD>
															<TD>
																<asp:Label id="CurrentPg" runat="server" Width="10px" Height="12px" ForeColor="Crimson" Font-Size="X-Small"
																	Font-Bold="True"></asp:Label></TD>
															<TD>
																<asp:Label id="of" ForeColor="#0000C0" Font-Names="Verdana" Font-Size="8pt" Font-Bold="True"
																	Runat="server">of</asp:Label></TD>
															<TD>
																<asp:label id="TotalPages" runat="server" Width="10px" Height="12px" ForeColor="Crimson" Font-Size="X-Small"
																	Font-Bold="True"></asp:label></TD>
															<TD>
																<asp:imagebutton id="Firstbutton" runat="server" ImageUrl="../Images/next9.jpg" AlternateText="First"
																	ToolTip="First"></asp:imagebutton></TD>
															<TD width="14">
																<asp:imagebutton id="Prevbutton" runat="server" ImageUrl="../Images/next99.jpg" ToolTip="Previous"></asp:imagebutton></TD>
															<TD>
																<asp:imagebutton id="Nextbutton" runat="server" ImageUrl="../Images/next9999.jpg" ToolTip="Next"></asp:imagebutton></TD>
															<TD>
																<asp:imagebutton id="Lastbutton" runat="server" ImageUrl="../Images/next999.jpg" ToolTip="Last"></asp:imagebutton></TD>
															<TD>
																<asp:textbox id="txtPageSize" runat="server" Width="24px" Height="14px" Font-Size="7pt" MaxLength="2"></asp:textbox></TD>
															<TD>
																<asp:Button id="Button3" runat="server" Width="16px" Height="16px" Text=">" BorderStyle="None"
																	ForeColor="Navy" Font-Size="7pt" Font-Bold="True" ToolTip="Change Paging Size"></asp:Button></TD>
														</TR>
													</TABLE>
												</asp:panel>
											</asp:panel></TD>
									</TR>
									<TR>
										<TD>
											<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 352px">
												<asp:DataGrid id="dgrEnvDetail" BorderColor="#d4d4d4" BorderWidth="0" Runat="server" CellPadding="0"
													PageSize="20" CssClass="grid" DataKeyField="EV_NU9_ID_PK" AutoGenerateColumns="False" PagerStyle-Visible="False"
													AllowPaging="True">
													<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
													<ItemStyle CssClass="GridItem"></ItemStyle>
													<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="EV_NU9_ID_PK"></asp:BoundColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="txtEnvName" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																EnvName
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("EV_VC30_Environment_Name")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="txtDatabase" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																DataBase
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("EV_VC50_Database")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="txtOwner" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Table Owner
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("EV_VC30_Owner")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="txtUserID" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																	Enabled="False"></asp:TextBox>
																User ID
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("EV_VC50_UserID")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="txtPassword" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																	Enabled="False"></asp:TextBox>
																Password
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("EV_VC50_Password")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="txtMachine" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Machine
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("EV_VC100_SystemID")%>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle Visible="False"></PagerStyle>
												</asp:DataGrid></DIV>
											<asp:Panel id="pnlFE" Width="0" Runat="server">
												<TABLE cellSpacing="0" cellPadding="0" border="0">
													<TR>
														<TD><IMG height="0" src="../Images/divider.gif ToolTip"></TD>
														<TD>
															<asp:DropDownList id="DDLEnv_F" Width="81px" Runat="server" CssClass="txtNoFocusFE"></asp:DropDownList></TD>
														<TD>
															<asp:TextBox id="txtDatabase_F" Width="101px" Runat="server" CssClass="txtNoFocusFE"></asp:TextBox></TD>
														<TD>
															<asp:textbox id="txtOwner_F" runat="server" Width="101px" CssClass="txtNoFocusFE"></asp:textbox></TD>
														<TD>
															<asp:textbox id="txtUserID_F" runat="server" Width="101px" CssClass="txtNoFocusFE"></asp:textbox></TD>
														<TD>
															<asp:textbox id="txtPassword_F" runat="server" Width="101px" CssClass="txtNoFocusFE" TextMode="Password"></asp:textbox></TD>
														<TD>
															<asp:textbox id="txtMachine_F" runat="server" Width="101px" CssClass="txtNoFocusFE"></asp:textbox></TD>
														<TD></TD>
													</TR>
												</TABLE>
											</asp:Panel></TD>
									</TR>
								</TABLE>
							</DIV>
						</cc1:collapsiblepanel><input type="hidden" name="txthiddenImage"> <input type="hidden" name="txtHiddenID">
						<input type="hidden" name="txtMachineInfo">
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
