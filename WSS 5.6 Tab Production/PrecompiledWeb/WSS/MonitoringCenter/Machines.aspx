<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_Machines, App_Web__eyibudh" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
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
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script>

			var globleID;
			var globlestrTempName;
			
		
			
			function callrefresh()
				{
					//location.href="Machines.aspx";
					alert();
					document.Form1.submit();
					alert();
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
																OpenMachine(document.Form1.txthidden.value )
																document.Form1.txthiddenImage.value=varImgValue;
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
								
								if (varImgValue=='Save')
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
						
			
				function KeyCheck(rowvalues,ID)
		{
	
		   var tableID='cpnlMachine_GrdAddSerach' ;
    	   var table;
		   document.Form1.txthiddenID.value=ID;		
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
			
					
					
				function KeyCheck55(rowvalues,aa,bb)
				{
			
				wopen('EditDomainMachine.aspx?MachName='+aa+'&DomID='+ bb,'Comments',800,350);
					return false;	
				}
	  
					function OpenMachine(varTable)
				{
					wopen('MachineEdit.aspx?ID='+varTable,'Search',430,400);
				}
		function KeyCheckMachines(nn,rowvalues,tableID,MachineType)
				{	
				//		alert(MachineType);
						if( MachineType!='S')
							{
									alert('This Machine Information cannot be Changed');							
							}
							else
							{
					globaldbclick = 1;
				//	document.Form1.txthiddenCallNo.value=nn;
					document.Form1.txthidden.value=nn;
					document.Form1.txthiddenImage.value='Edit';
				//	document.Form1.txthiddenTable.value=tableID;
			
					//alert(nn);
					if (tableID=='cpnlMachineSearch_GrdAddSerach')
						{
							OpenMachine(nn);
						}
					else
						{
							Form1.submit(); 
						}	
						}												
				}
				
				
					function addToParentList(Afilename,TbName,strName)
				{
				
					if (Afilename != "" || Afilename != 'undefined')
					{
						//varName = TbName + 'Name'
					   //alert(Afilename);
						document.getElementById(TbName).value=Afilename;
						//document.getElementById(varName).value=strName;
						aa=Afilename;
					}
					else
					
					{
						document.Form1.txtAB_Type.value=aa;
					}
				}					
			function OpenW(varTable)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('AB_ViewColumns.aspx?TBLName='+varTable,'Search',480,440);
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
				}
				
		function wopen(url, name, w, h)
			{
			alert(url);
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
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<TR>
					<TD><asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0px"></asp:button></TD>
					<td colSpan="2">
						<!-- *****************************************--><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="54px" Text="Error Message" BorderColor="Indigo"
							TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
							Draggable="False" Visible="False" BorderStyle="Solid" BorderWidth="0px">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD colSpan="0" rowSpan="0">
										<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
									<TD colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" Width="552px" BorderWidth="0" BorderStyle="Groove"
											Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
										<asp:TextBox id="TxtrequestID" runat="server" Height="0px" Width="0px"></asp:TextBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD><asp:panel id="Panel1" runat="server">
										<asp:panel id="Panel7" runat="server">
											<TABLE height="25">
												<TR>
													<TD>
														<asp:Label id="lblDomain" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Runat="server">Select Domain</asp:Label></TD>
													<TD>
														<asp:DropDownList id="Ddldomain" Width="100px" Runat="server" CssClass="txtNoFocus" AutoPostBack="True"></asp:DropDownList></TD>
													<TD></TD>
												</TR>
											</TABLE>
										</asp:panel>
									</asp:panel></TD>
							</TR>
							<TR>
								<TD><cc1:collapsiblepanel id="cpnlMachine" runat="server" Width="100%" Text="MachineDetail" BorderColor="#f5f5f5"
										TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
										ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/To&#9;ggleUp.gif" Draggable="False"
										Visible="True" BorderStyle="Solid" BorderWidth="0px">
										<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 480px">
											<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
												<TR>
													<TD style="HEIGHT: 14px">
														<asp:panel id="Panel11" runat="server">
															<asp:panel id="Panel17" runat="server">
																<TABLE height="25">
																	<TR>
																		<TD>
																			<asp:Label id="pg" Width="40px" Font-Size="8pt" Font-Names="Verdana" ForeColor="#0000C0" Font-Bold="True"
																				Runat="server">Page</asp:Label></TD>
																		<TD>
																			<asp:Label id="CurrentPg" runat="server" Height="12px" Width="10px" Font-Size="X-Small" ForeColor="Crimson"
																				Font-Bold="True"></asp:Label></TD>
																		<TD>
																			<asp:Label id="of" Font-Size="8pt" Font-Names="Verdana" ForeColor="#0000C0" Font-Bold="True"
																				Runat="server">of</asp:Label></TD>
																		<TD>
																			<asp:label id="TotalPages" runat="server" Height="12px" Width="10px" Font-Size="X-Small" ForeColor="Crimson"
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
																			<asp:textbox id="txtPageSize" runat="server" Height="14px" Width="24px" Font-Size="7pt" MaxLength="3"></asp:textbox></TD>
																		<TD>
																			<asp:Button id="Button3" runat="server" Height="16px" Width="16px" BorderStyle="None" Text=">"
																				Font-Size="7pt" ForeColor="Navy" Font-Bold="True" ToolTip="Change Paging Size"></asp:Button></TD>
																		<TD></TD>
																		<TD>
																			<asp:Label id="lblrecords" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
																				ForeColor="MediumBlue" Font-Bold="True">Total Records</asp:Label></TD>
																		<TD>
																			<asp:Label id="TotalRecods" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
																				ForeColor="Crimson" Font-Bold="True"></asp:Label></TD>
																		<TD></TD>
																	</TR>
																</TABLE>
															</asp:panel>
														</asp:panel></TD>
												</TR>
												<TR>
													<TD>
														<DIV style="OVERFLOW: auto; WIDTH: 250%">
															<asp:DataGrid id="GrdAddSerach" BorderWidth="0" BorderColor="#d4d4d4" Runat="server" CssClass="grid"
																DataKeyField="MM_VC150_Machine_Name" PageSize="18" CellPadding="0" PagerStyle-Visible="False"
																AllowPaging="True" AutoGenerateColumns="False">
																<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																<ItemStyle CssClass="GridItem"></ItemStyle>
																<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="MM_VC150_Machine_Name"></asp:BoundColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="txtMachineName" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Machine Name
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("MM_VC150_Machine_Name")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderStyle Wrap="False"></HeaderStyle>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtMachineIP" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Machine IP
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("MM_VC100_Machine_IP")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="txtMachineType" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Machine Type
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("MM_VC8_Machine_Type")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="txtStatus" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Status
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("MM_CH1_IsEnable")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderStyle Wrap="False"></HeaderStyle>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="txtCat1" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Cat1
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("MM_VC20_Cat1")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtCat2" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Cat2
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("MM_VC20_Cat2")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtCat3" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Cat3
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("MM_VC20_Cat3")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle Visible="False"></PagerStyle>
															</asp:DataGrid></DIV>
													</TD>
												</TR>
											</TABLE>
										</DIV>
									</cc1:collapsiblepanel></TD>
							</TR>
						</TABLE>
						<!-- *****************************************--></td>
				</TR>
			</table>
			<INPUT type="hidden" name="txthiddenImage"> <INPUT type="hidden" name="txthiddenID">
			<input type="hidden" name="txtMachineInfo">
		</form>
	</body>
</html>
