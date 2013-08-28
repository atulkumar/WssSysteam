<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MatchTables.aspx.vb" Inherits="MonitoringCenter_MatchTables" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>MatchTables</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="http://localhost/WSS4.3/Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">
		
			function Post(Action)
				{
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
			     		var tableID='cpnlEnvEntry_dgrMatchTable1';
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
			
			
		
				function select_deselectAll (chkVal, idVal)
					{ 
						var frm = document.forms[0];

						// Loop through all elements
						for (i=0; i<frm.length; i++) 
							{
								// Look for our Header Template's Checkbox
								if (idVal.indexOf ('CheckAll') != -1) 
								{
									// Check if main checkbox is checked, then select or deselect datagrid checkboxes 
									if(chkVal == true)
										{
											if(frm.elements[i].disabled==false)
												{
													frm.elements[i].checked = true;
												}
											else
												{
													frm.elements[i].checked = false;
												}
										}
									else
										{
											frm.elements[i].checked = false;
										}

										// Work here with the Item Template's multiple checkboxes
									} 
								else if (idVal.indexOf ('chkReq1') != -1) 
									{
										// Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
										if(frm.elements[i].checked == false) 
											{
												frm.elements[1].checked = false; //Uncheck main select all checkbox
											}
									}
							}
					}
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="HEIGHT: 81px" colSpan="8"><cc1:collapsiblepanel id="cpnlErrorPanel" runat="server" BorderColor="Indigo" Visible="False" TitleCSS="test"
							PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Error Message" ExpandImage="../../Images/ToggleDown.gif"
							CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderWidth="0px" BorderStyle="Solid" Width="100%">
							<TABLE id="Table2" style="WIDTH: 450px; HEIGHT: 20px" borderColor="lightgrey" cellSpacing="0"
								cellPadding="0" width="466" border="0">
								<TR>
									<TD vAlign="middle" colSpan="0" rowSpan="0">
										<asp:Image id="Image2" runat="server" Width="16px" ImageUrl="../../Images/warning.gif" Height="16px"></asp:Image></TD>
									<TD colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" Width="416px" Font-Size="XX-Small" Font-Names="Verdana"
											ForeColor="Red"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel>
						<cc1:collapsiblepanel id="cpnlEnvEntry" runat="server" BorderColor="#f5f5f5" Visible="True" TitleCSS="test"
							PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Env Entry" ExpandImage="../Images/ToggleDown.gif"
							CollapseImage="../Images/To&#9;ggleUp.gif" Draggable="False" BorderWidth="0px" BorderStyle="Solid" Width="100%">
							<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 480px">
								<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
									<TBODY>
										<TR>
											<TD style="WIDTH: 648px; HEIGHT: 25px" borderColor="#f5f5f5" align="left" colSpan="4">&nbsp;&nbsp;
												<asp:label id="lblCompare" runat="server" Width="32px" Height="16px" Font-Size="XX-Small" Font-Names="Verdana"
													Font-Bold="True">Compare</asp:label>&nbsp;
												<asp:dropdownlist id="ddlENV1" runat="server" Width="104px"></asp:dropdownlist>&nbsp;
												<asp:label id="Label1" runat="server" Width="16px" Height="16px" Font-Size="XX-Small" Font-Names="Verdana"
													Font-Bold="True">To</asp:label>&nbsp;
												<asp:dropdownlist id="ddlEnv2" runat="server" Width="104px"></asp:dropdownlist>&nbsp;
												<asp:label id="Label2" runat="server" Width="16px" Height="16px" Font-Size="XX-Small" Font-Names="Verdana"
													Font-Bold="True">Table</asp:label>&nbsp;<asp:textbox id="txtTableName" runat="server" Width="112px"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;
												<asp:button id="btnGST" runat="server" Text="Get Structure"></asp:button></TD>
										</TR>
										<tr>
											<TD style="WIDTH: 474px; HEIGHT: 69px"><asp:panel id="Panel1" runat="server">
													<asp:panel id="Panel2" runat="server">
														<TABLE height="5">
															<TR>
																<TD></TD>
																<TD></TD>
																<TD>&nbsp;
																	<asp:Label id="Label3" Width="40px" Font-Size="8pt" Font-Names="Verdana" ForeColor="#0000C0"
																		Font-Bold="True" Runat="server">Page</asp:Label></TD>
																<TD>
																	<asp:Label id="Label4" runat="server" Width="10px" Height="12px" Font-Size="X-Small" ForeColor="Crimson"
																		Font-Bold="True"></asp:Label></TD>
																<TD>
																	<asp:Label id="Label5" Font-Size="8pt" Font-Names="Verdana" ForeColor="#0000C0" Font-Bold="True"
																		Runat="server">of</asp:Label></TD>
																<TD>
																	<asp:label id="Label6" runat="server" Width="10px" Height="12px" Font-Size="X-Small" ForeColor="Crimson"
																		Font-Bold="True"></asp:label></TD>
																<TD>
																	<asp:imagebutton id="Imagebutton1" runat="server" ImageUrl="../Images/next9.jpg" ToolTip="First"
																		AlternateText="First"></asp:imagebutton></TD>
																<TD width="14">
																	<asp:imagebutton id="Imagebutton2" runat="server" ImageUrl="../Images/next99.jpg" ToolTip="Previous"></asp:imagebutton></TD>
																<TD>
																	<asp:imagebutton id="Imagebutton3" runat="server" ImageUrl="../Images/next9999.jpg" ToolTip="Next"></asp:imagebutton></TD>
																<TD>
																	<asp:imagebutton id="Imagebutton4" runat="server" ImageUrl="../Images/next999.jpg" ToolTip="Last"></asp:imagebutton></TD>
																<TD>
																	<asp:textbox id="Textbox1" runat="server" Width="24px" Height="14px" Font-Size="7pt" MaxLength="2"></asp:textbox></TD>
																<TD>
																	<asp:Button id="Button6" runat="server" Width="16px" BorderStyle="None" Text=">" Height="16px"
																		Font-Size="7pt" ForeColor="Navy" Font-Bold="True" ToolTip="Change Paging Size"></asp:Button></TD>
															</TR>
														</TABLE>
													</asp:panel>
												</asp:panel></TD>
											<td style="WIDTH: 43px; HEIGHT: 69px"></td>
											<td style="HEIGHT: 69px"></td>
											<td style="FONT-WEIGHT: bold; FONT-SIZE: 8pt; WIDTH: 174px; COLOR: black; FONT-FAMILY: Verdana; HEIGHT: 69px">
												<P>&nbsp;</P>
												<P>&nbsp;</P>
												<P>Column Name</P>
											</td>
										</tr>
										<TR>
											<TD style="WIDTH: 474px">
												<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 352px"><asp:datagrid id="dgrMatchTable1" BorderColor="#d4d4d4" BorderWidth="0" Runat="server" AllowPaging="True"
														PagerStyle-Visible="False" PageSize="20" CellPadding="0" AutoGenerateColumns="False" DataKeyField="rs_nu9_sqid_fk" CssClass="grid">
														<SelectedItemStyle BackColor="#FFFF80"></SelectedItemStyle>
														<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
														<ItemStyle CssClass="GridItem"></ItemStyle>
														<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
														<Columns>
															<asp:BoundColumn Visible="False" DataField="rs_nu9_sqid_fk"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="RS_VC150_CAT5"></asp:BoundColumn>
															<asp:TemplateColumn>
																<HeaderStyle Font-Bold="True"></HeaderStyle>
																<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
																<HeaderTemplate>
																	<asp:CheckBox ID="CheckAll" Runat="server" OnClick="javascript: return select_deselectAll (this.checked, this.id);"></asp:CheckBox>
																</HeaderTemplate>
																<ItemTemplate>
																	<asp:CheckBox ID="chkReq1" Runat="server"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<HeaderTemplate>
																	<asp:TextBox Width="100%" id="txtColName" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																	Name
																</HeaderTemplate>
																<ItemTemplate>
																	<%# container.dataitem("RS_VC150_CAT5")%>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<HeaderTemplate>
																	<asp:TextBox Width="100%" id="txtKey" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																	Key
																</HeaderTemplate>
																<ItemTemplate>
																	<%# container.dataitem("RS_VC150_CAT6")%>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<HeaderTemplate>
																	<asp:TextBox Width="100%" id="txtData" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																		Enabled="False"></asp:TextBox>
																	Data Type
																</HeaderTemplate>
																<ItemTemplate>
																	<%# container.dataitem("RS_VC150_CAT7")%>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<HeaderTemplate>
																	<asp:TextBox Width="100%" id="txtSize" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																		Enabled="False"></asp:TextBox>
																	Size
																</HeaderTemplate>
																<ItemTemplate>
																	<%# container.dataitem("RS_VC150_CAT8")%>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<HeaderTemplate>
																	<asp:TextBox Width="100%" id="txtDBNULL" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																	Null
																</HeaderTemplate>
																<ItemTemplate>
																	<%# container.dataitem("RS_VC150_CAT9")%>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
														<PagerStyle Visible="False"></PagerStyle>
													</asp:datagrid></DIV>
											<td></td>
											<TD>
												<P>&nbsp;
													<asp:button id="Button2" runat="server" Text=">" Width="25px"></asp:button>&nbsp;</P>
												<P>&nbsp;
													<asp:button id="Button4" runat="server" Text="<" Width="25px"></asp:button></P>
												<P>&nbsp;</P>
												<P>&nbsp;</P>
												<P>&nbsp;</P>
												<P>&nbsp;</P>
												<P>&nbsp;</P>
												<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</P>
											</TD>
											<TD style="WIDTH: 174px"><asp:textbox id="txtcolumanName" runat="server" Width="128px"></asp:textbox><asp:button id="Button5" runat="server" Text="+" Width="15Px" Font-Bold="True"></asp:button><asp:listbox id="lstColName" runat="server" Width="152px" Height="340px" Font-Size="X-Small"
													Font-Names="Tahoma" SelectionMode="Multiple"></asp:listbox></TD>
											<TD vAlign="middle" borderColor="#f5f5f5" align="left"><P>&nbsp;</P>
												<P>&nbsp;</P>
											</TD>
										</TR>
										<TR>
											<TD style="WIDTH: 474px" vAlign="top" borderColor="#f5f5f5" align="center"></TD>
											<TD style="WIDTH: 43px" vAlign="middle" borderColor="#f5f5f5" align="left"></TD>
											<TD style="WIDTH: 7px" vAlign="top" borderColor="#f5f5f5" align="left">&nbsp;</TD>
											<TD style="WIDTH: 174px" vAlign="middle" borderColor="#f5f5f5" align="left"></TD></TR></TBODY></TABLE></DIV></cc1:collapsiblepanel></TD>
				</TR>
			</table>
			<input type="hidden" name="txthiddenImage"> <input type="hidden" name="txtHiddenID">
			<input type="hidden" name="txtMachineInfo"> </form>
	</body>
</html>
