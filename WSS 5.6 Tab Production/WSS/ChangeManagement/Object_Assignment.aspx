<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Object_Assignment.aspx.vb" Inherits="ChangeManagement_Object_Assignment" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>
		<linkhref="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script type="text/javascript">
			var globleID;
			var globleUser;
			var globleRole;
			var globleCompany;
			var rand_no = Math.ceil(500*Math.random())
			function OpenComp(c)
				{
					wopen('../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name  from T010011 WHERE CI_VC8_Address_Book_Type = '+ "'COM'" +' &tbname=' + c ,'Search'+rand_no,500,450);
				}
			function OpenCall(a, b, c)
				{
					var comp = document.getElementById('txtCompName').value;
					wopen('../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+ ' and Company = '+"'"+ comp +"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
				}
				  
			function OpenTask(a, b, c)
				{   
					var comp = document.getElementById('txtCompName').value;
					wopen('../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+ ' and Company = '+"'"+ comp +"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
				}
				  
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
					//if (parent.document.all("SideMenu1").cols =="0,*")
					//{
					//		document.Form1.imgHide.style.visibility = 'hidden'; 
					//		document.Form1.ingShow.style.visibility = 'visible'; 
					//}
					//else
					//{
					//		document.Form1.ingShow.style.visibility = 'hidden'; 
					//		document.Form1.imgHide.style.visibility = 'visible'; 
					//}
				}				
			
			
			function CloseWindow()
				{
					self.opener.Form1.submit();
				}	
				
			function callRefresh()
				{
					Form1.submit();
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
												if  (document.Form1.txthidden.value=="")
												{
														alert("Please select the row");
												}
												else
												{
													//alert(document.Form1.txthiddenAdno.value);
														document.Form1.txthiddenImage.value=varImgValue;
														Form1.submit(); 
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
								if (varImgValue=='Save')
								{
								
								//Security Block
							var obj=document.getElementById("imgSave")
							if(obj==null)
							{
								alert("You don't have access rights to Save record");
								return false;
							}

							if (obj.disabled==true) 
							{
								alert("You don't have access rights to Save record");
								return false;
							}
					//End of Security Block


											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit();
											return false;
										
								}	
								
								if (varImgValue=='Close')
								{
									window.close();
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
														}		

									}			
				}				
				
				function KeyCheck(nn,rowvalues)
					{
						//alert(rowvalues);
						globleID = nn;
						document.Form1.txthidden.value=nn;
			/*			document.Form1.txthiddenUser.value=nn;
						document.Form1.txthiddenRole.value=nn;
						document.Form1.txthiddenCompany.value=nn;
			*/
						//Form1.submit();
						
										var tableID='GrdAddSerach'  //your datagrids id
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
							document.Form1.txthiddenImage.value='Edit';
							Form1.submit(); 
					}	
					
			function OpenW(varTable)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company  from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c  ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('../AdministrationCenter/AddressBook/AB_ViewColumns.aspx? ID='+varTable,'Search'+rand_no,500,450);
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
			
			function addToParentList(Afilename,TbName)
				{				
					if (Afilename != "" || Afilename != 'undefined')
						{
							document.getElementById(TbName).value=Afilename;
							aa=Afilename;
						}
					else					
						{
							document.Form1.txtAB_Type.value=aa;
						}
				}
				
		</script>
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="GridLayout"
		onunload="CloseWindow();">
    <form id="form1" runat="server">
  

    <TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				background="../Images/top_nav_back.gif" border="0">
				<TR>
					<TD width="217" style="WIDTH: 217px">&nbsp;
						<asp:label id="lblTitleLabelobjassign" runat="server" Width="192px" Height="12px" BorderWidth="2px"
							BorderStyle="None" CssClass="TitleLabel">Object Assignment</asp:label>
					</TD>
					<td><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
						<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif" ToolTip="Save"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgClose" runat="server" ImageUrl="../Images/s2close01.gif" accessKey="O" ToolTip="Close"></asp:imagebutton>&nbsp;
						<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
					</td>
					<td width="42" background="../Images/top_nav_back01.gif" height="67">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Table126" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				border="1">
				<TR>
					<TD vAlign="top" colSpan="1">
						<!--  **********************************************************************-->
						<TABLE id="Table7" cellSpacing="0" cellPadding="2" width="100%" border="0">
							<tr>
								<td colSpan="2"><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="24px" BorderStyle="Solid" BorderWidth="0px"
										BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
										TitleBackColor="Transparent" Text="Error Message" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
										Draggable="False">
										<TABLE id="Table3" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<TR>
												<TD width="20" colSpan="0" rowSpan="0">
													<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../icons/warning.gif"></asp:Image></TD>
												<TD width="100%" colSpan="0" rowSpan="0">
													<asp:ListBox id="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Height="56px"
														Width="100%" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox></TD>
											</TR>
										</TABLE>
									</cc1:collapsiblepanel></td>
							</tr>
							<TR>
								<TD></TD>
							</TR>
							<tr>
								<td><cc1:collapsiblepanel id="Collapsiblepanel1" runat="server" Width="100%" Height="24px" BorderStyle="Solid"
										BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
										TitleClickable="True" TitleBackColor="Transparent" Text="Object Details" ExpandImage="../Images/ToggleDown.gif"
										CollapseImage="../Images/ToggleUp.gif" Draggable="False">&nbsp; 
<asp:datagrid id="dgObjectDetails" runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White">
											<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
											<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
											<HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" CssClass="GridHeader"></HeaderStyle>
											<Columns>
												<asp:TemplateColumn HeaderText="Object Name">
													<HeaderStyle Font-Bold="True"></HeaderStyle>
													<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
													<ItemTemplate>
														<asp:TextBox CssClass=txtNoFocus ID="txtObjName" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OD_VC60_Object_name") %>'>
														</asp:TextBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Object Description">
													<HeaderStyle Font-Bold="True"></HeaderStyle>
													<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
													<ItemTemplate>
														<asp:TextBox CssClass=txtNoFocus ID="txtObjDesc" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OD_VC2000_Object_desc") %>'>
														</asp:TextBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Special Instruction">
													<HeaderStyle Font-Bold="True"></HeaderStyle>
													<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
													<ItemTemplate>
														<asp:TextBox CssClass=txtNoFocus ID="txtSpecInst" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OD_VC2000_Special_inst") %>'>
														</asp:TextBox>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
											<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
										</asp:datagrid></cc1:collapsiblepanel></td>
							</tr>
							<tr>
								<td align="left" colSpan="1"><asp:linkbutton id="AddRow" runat="server" Font-Names="Verdana" Font-Size="X-Small"><font family="verdana" color="DimGray">Add 
											Row</font></asp:linkbutton>
								</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			<!-- ***********************************************************************--> 
			</TD></TR></TABLE> <INPUT type="hidden" name="txthidden"> <INPUT type="hidden" name="txthiddenImage">
    </form>
</body>
</html>
