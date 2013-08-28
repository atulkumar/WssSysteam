<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DomainMachine.aspx.vb" Inherits="MonitoringCenter_DomainMachine" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../SupportCenter/calendar/DateSelector.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Domain Machines</title>
		<LINK href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
			<LINK href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">
				<script language="javascript" src="../SupportCenter/calendar/popcalendar.js"></script>
				<script language="javascript" src="../images/Js/JSValidation.js"></script>
				<style type="text/css">.DataGridFixedHeader { POSITION: relative; ; TOP: expression(this.offsetParent.scrollTop); BACKGROUND-COLOR: #e0e0e0 }
	</style>
				<script language="javascript" src="../DateControl/ION.js"></script>
				<script language="Javascript">
			
			
			function wopen(url, name, w, h)
				{
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

			
			function addToParentList(Afilename,TbName,strname)
				{				
					if (Afilename != "" || Afilename != 'undefined')
						{
							varName = TbName + 'Name'
							if((document.getElementById(varName) == null))
							{								
							}
							else
							{
								document.getElementById(varName).value=strname;
							}
							document.getElementById(TbName).value=Afilename;
							aa=Afilename;
						}
					else					
						{
							document.Form1.txtAB_Type.value=aa;
						}
				}
				
			function addToParentCtrl(value)
				{
					document.getElementById('ContactInfo_txtBr').value=Value;
				}
						
			function callrefresh()
				{
					Form1.submit();
				}							

			

																							
			function KeyCheck(nn,rowvalues)
					{
				        
										var tableID='cpnlDom_GrdAddSerach'  //your datagrids id
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
					
		
			function KeyCheck55(aa,bb)
				{
				wopen('EditDomainMachine.aspx?MachName='+aa+'&DomID='+ bb,'Comments',800,350);
					return false;	
				}
						
			function FP_swapImg() 
				{//v1.0
						var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
						n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
						elm.$src=elm.src; elm.src=args[n+1]; } }
				}

			function FP_preloadImgs() 
				{//v1.0
						var d=document,a=arguments; if(!d.FP_imgs) d.FP_imgs=new Array();
						for(var i=0; i<a.length; i++) { d.FP_imgs[i]=new Image; d.FP_imgs[i].src=a[i]; }
				}

			function FP_getObjectByID(id,o) 
				{//v1.0
						var c,el,els,f,m,n; if(!o)o=document; if(o.getElementById) el=o.getElementById(id);
						else if(o.layers) c=o.layers; else if(o.all) el=o.all[id]; if(el) return el;
						if(o.id==id || o.name==id) return o; if(o.childNodes) c=o.childNodes; if(c)
						for(n=0; n<c.length; n++) { el=FP_getObjectByID(id,c[n]); if(el) return el; }
						f=o.forms; if(f) for(n=0; n<f.length; n++) { els=f[n].elements;
						for(m=0; m<els.length; m++){ el=FP_getObjectByID(id,els[n]); if(el) return el; } }
						return null;
				}
				
				</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<FORM id="Form1" method="post" runat="server">
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
														<TD><asp:button id="BtnGrdSearch" runat="server" Text="Button" Height="0px" Width="0px"></asp:button>&nbsp;<asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
																CommandName="submit" ImageUrl="white.GIF"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelDM" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
																Font-Size="X-Small" BorderWidth="2px" BorderStyle="None"> DOMAIN MACHINES</asp:label></TD>
														<td><IMG title="Seperator" alt="R" src="../images/00Seperator.gif" border="0">&nbsp;&nbsp;
															<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../images/S2Save01.gif"></asp:imagebutton>&nbsp;&nbsp;
															<asp:imagebutton id="imgReset" accessKey="S" runat="server" ImageUrl="../Images/reset_20.gif" ToolTip="Reset"></asp:imagebutton>&nbsp;<asp:imagebutton id="imgAdd" accessKey="A" runat="server" ImageUrl="../images/Machine.gif" ToolTip="Get Machines"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgSearch" accessKey="H" runat="server" ImageUrl="../images/Domain.gif" ToolTip="Get Domains"></asp:imagebutton>&nbsp;<asp:imagebutton id="ImgbtnClose" accessKey="C" runat="server" ImageUrl="../Images/s2close01.gif"
																ToolTip="Close"></asp:imagebutton>
															&nbsp;<IMG title="Seperator" alt="R" src="../images/00Seperator.gif" border="0">
														</td>
													</TR>
												</table>
											</td>
											<TD align="right" width="152" background="../images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
													src="../images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp; <IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;</TD>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../images/main_line.gif" height="10"><IMG height="10" src="../images/main_line.gif" width="6"></td>
											<td width="7" height="10"><IMG height="10" src="../images/main_line01.gif" width="7"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../images/main_line02.gif" height="2"><IMG height="2" src="../images/main_line02.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../images/main_line03.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<TABLE id="Table16" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
										border="0">
										<TR>
											<TD vAlign="top" colSpan="1">
												<!--  **********************************************************************-->
												<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
													<table style="BORDER-COLLAPSE: collapse" width="100%" border="0">
														<tr>
															<td colSpan="2">
																<table cellSpacing="0" cellPadding="0" width="100%" border="0">
																	<tr>
																		<td><cc1:collapsiblepanel id="cpnlError" runat="server" Text="Message" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																				Draggable="False" CollapseImage="../images/ToggleUp.gif" ExpandImage="../images/ToggleDown.gif" TitleBackColor="Transparent"
																				TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="False" BorderColor="Indigo">
																				<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
																					border="0">
																					<TR>
																						<TD colSpan="0" rowSpan="0">
																							<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../icons/warning.gif"></asp:Image></TD>
																						<TD colSpan="0" rowSpan="0">
																							<asp:ListBox id="lstError" runat="server" Width="635px" BorderStyle="Groove" BorderWidth="0"
																								Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox></TD>
																					</TR>
																				</TABLE>
																			</cc1:collapsiblepanel></td>
																	</tr>
																</table>
																<cc1:collapsiblepanel id="cpnlDom2" runat="server" Text="Domains" Height="47px" Width="100%" BorderWidth="0px"
																	BorderStyle="Solid" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
																	TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
																	Visible="True" BorderColor="Indigo">
																	<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																		align="left" border="0">
																		<TR>
																			<TD vAlign="top" align="left">
																				<asp:label id="Label2" runat="server" Width="104px" Height="12px" Font-Size="XX-Small" Font-Names="Verdana"
																					Font-Bold="True" ForeColor="DimGray">Select Domain</asp:label>
																				<uc1:customddl id="cddlDomain" runat="server"></uc1:customddl>&nbsp;
																				<asp:imagebutton id="imgShow" accessKey="L" runat="server" Height="22px" ImageUrl="../Images/s1search02.gif"
																					ToolTip="Search Request"></asp:imagebutton></TD>
																		</TR>
																		<TR>
																			<TD vAlign="top" align="left">
																				<asp:label id="lblMachineCode" runat="server" Width="104px" Height="12px" Font-Size="XX-Small"
																					Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray" Visible="False">Select Machine</asp:label>
																				<asp:DropDownList id="ddMach" runat="server" Width="120pt" Visible="False"></asp:DropDownList></TD>
																		</TR>
																		<TR>
																			<TD vAlign="top" align="left"><BR>
																				<asp:datagrid id="grdDom" runat="server" BorderStyle="None" BorderWidth="1px" Font-Names="Verdana"
																					ForeColor="MidnightBlue" BorderColor="Silver" AutoGenerateColumns="False" OnItemDataBound="myItems_ItemDataBound"
																					DataKeyField="DomID" CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left" PageSize="50"
																					CssClass="grid">
																					<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
																					<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
																					<AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
																					<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
																					<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
																					<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
																					<Columns>
																						<asp:ButtonColumn Visible="false" CommandName="select">
																							<itemstyle width="20pt"></itemstyle>
																						</asp:ButtonColumn>
																						<asp:TemplateColumn HeaderText="DomID">
																							<HeaderStyle Font-Bold="True"></HeaderStyle>
																							<ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="80"></ItemStyle>
																							<ItemTemplate>
																								<asp:label ID="lblDomID" Runat="server" Width="60px" text='<%# DataBinder.Eval(Container.DataItem, "DomID") %>'>
																								</asp:label>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn HeaderText="DomName">
																							<HeaderStyle Font-Bold="True"></HeaderStyle>
																							<ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="120"></ItemStyle>
																							<ItemTemplate>
																								<asp:label ID="lblDomName" Runat="server" Width="60px" text='<%# DataBinder.Eval(Container.DataItem, "DomName") %>'>
																								</asp:label>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn HeaderText="Status">
																							<HeaderStyle Font-Bold="True"></HeaderStyle>
																							<ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="80px"></ItemStyle>
																							<ItemTemplate>
																								<asp:label ID="lblStatus" Runat="server" Visible="true" width="80px" text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
																								</asp:label>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn HeaderText="ReqNo">
																							<HeaderStyle Font-Bold="True"></HeaderStyle>
																							<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
																							<ItemTemplate>
																								<asp:label ID="lblReqNo" Runat="server" width="60px" text='<%# DataBinder.Eval(Container.DataItem, "ReqNo") %>'>
																								</asp:label>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																					</Columns>
																				</asp:datagrid></TD>
																		</TR>
																		<TR>
																			<TD vAlign="top" align="left"></TD>
																		</TR>
																	</TABLE>
																</cc1:collapsiblepanel></td>
														</tr>
														<TR>
															<TD colSpan="2"><cc1:collapsiblepanel id="cpnlDom" runat="server" Text="Machines" Height="47px" Width="100%" BorderWidth="0px"
																	BorderStyle="Solid" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
																	TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="True"
																	BorderColor="Indigo">
																	<TABLE id="Table1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																		align="left" border="0">
																		<TR>
																			<TD vAlign="top" align="left"></TD>
																		</TR>
																		<TR>
																			<TD vAlign="top" align="left">
																				<asp:Label id="Label3" runat="server" Width="80px" Font-Size="XX-Small" Font-Names="Verdana"
																					Font-Bold="True">UserID</asp:Label>
																				<asp:TextBox id="txtUserID" runat="server" Width="150px" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
																					CssClass="txtNoFocus"></asp:TextBox></TD>
																		</TR>
																		<TR>
																			<TD vAlign="top" align="left">
																				<asp:Label id="Label4" runat="server" Width="80px" Font-Size="XX-Small" Font-Names="Verdana"
																					Font-Bold="True">Password</asp:Label>
																				<asp:TextBox id="txtPwd" runat="server" Width="150px" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
																					CssClass="txtNoFocus" TextMode="Password"></asp:TextBox></TD>
																		</TR>
																		<TR>
																			<TD vAlign="top" align="left"></TD>
																		</TR>
																		<TR>
																			<TD vAlign="top" align="left" height="146">
																				<asp:panel id="Panel1" runat="server">
																					<asp:Label id="Label1" runat="server" Width="26px"></asp:Label>
																				</asp:panel>
																				<asp:datagrid id="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px" Font-Names="Verdana"
																					ForeColor="MidnightBlue" BorderColor="Silver" AutoGenerateColumns="False" DataKeyField="MachineName"
																					CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left" PageSize="50" CssClass="grid">
																					<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
																					<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
																					<AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
																					<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
																					<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
																					<Columns>
																						<asp:TemplateColumn>
																							<HeaderStyle Font-Bold="True"></HeaderStyle>
																							<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
																							<ItemTemplate>
																								<asp:CheckBox ID="chkReq" Runat="server" Width="20"></asp:CheckBox>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																						<asp:TemplateColumn HeaderText="MachineName">
																							<HeaderStyle Font-Bold="True"></HeaderStyle>
																							<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
																							<ItemTemplate>
																								<asp:label ID="lblMachName" Runat="server" Width="80px" text='<%# DataBinder.Eval(Container.DataItem, "MachineName") %>'>
																								</asp:label>
																							</ItemTemplate>
																						</asp:TemplateColumn>
																					</Columns>
																					<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
																				</asp:datagrid></TD>
																		</TR>
																		<TR>
																			<TD vAlign="top" align="left"></TD>
																		</TR>
																	</TABLE>
																</cc1:collapsiblepanel></TD>
														</TR>
														<TR>
															<TD colSpan="2" height="23"><asp:textbox id="txtReqNo" runat="server" Width="0px"></asp:textbox><asp:textbox id="txtMach" runat="server" Width="0px"></asp:textbox><asp:textbox id="txtStatus" runat="server" Height="0px" Width="0px" Font-Names="Verdana" Font-Size="XX-Small"
																	CssClass="txtNoFocus"></asp:textbox></TD>
														</TR>
														<TR>
															<TD colSpan="2"></TD>
														</TR>
													</table>
												</DIV>
												<asp:textbox id="txtDomID" runat="server" Width="20px" Visible="False"></asp:textbox><asp:textbox id="txtDom" runat="server" Width="20px" Visible="False"></asp:textbox></TD>
											<td vAlign="top" width="12" background="../images/main_line04.gif"><IMG height="1" src="../images/main_line04.gif" width="12"></td>
										</TR>
									</TABLE>
									<DIV></DIV>
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
											<td width="66"><IMG height="31" src="../images/bottom_right.gif" width="66">
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<INPUT type="hidden" name="txthiddenImage"> <INPUT type="hidden" name="txthiddenID">
			<input type="hidden" name="txtMachineInfo">
		</FORM>
	</body>
</html>
