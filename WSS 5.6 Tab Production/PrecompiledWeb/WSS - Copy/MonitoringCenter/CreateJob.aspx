<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_CreateJob, App_Web_zn3-f7gx" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Create Job</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<LINK href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../DateControl/ION.js"></script>
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<style type="text/css">.DataGridFixedHeader { POSITION: relative; ; TOP: expression(this.offsetParent.scrollTop); BACKGROUND-COLOR: #e0e0e0 }
		</style>
		<script language="Javascript">

 

			var globalid;
			var globaldbclick = 0;
						
			function OpenW(a,b,c)
				{
					wopen('../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
				}
			
				
			function OpenProcessCode(c)
				{
						wopen('../Search/Common/PopSearch.aspx?ID=select PM_IN4_PCode as ID, PM_VC20_PName as ProcessName, PM_VC10_PType as ProcessType from T130031' + '  &tbname=' + c ,'Search',500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,ab.CI_VC36_Name as Name,UL_VC8_Role_PK as Role from T010011 ab,T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'Search',500,450);
				}
				
				function OpenMachineCode(c)
				{
						wopen('../Search/Common/PopSearch.aspx?ID=select MM_IN4_MCode as ID, MM_VC20_MName as MachineName, MM_VC20_MIP as MachineIP from T130011' + '  &tbname=' + c ,'Search',500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,ab.CI_VC36_Name as Name,UL_VC8_Role_PK as Role from T010011 ab,T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'Search',500,450);
				}					
			function OpenW_Add_Address(param)
				{
					wopen('AB_Additional.aspx?ID='+param,'Additional_Address',400,450);
				}
		
		function templrefresh(a,b)
		{
		//alert(a);
		
				//document.getElementById('cpnlCallView_TxtTmplId').value=a;
				document.getElementById('cpnlCallView_TxtTmplName').value=b;
		}
			
			function wopen1(url, name, w, h)
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
						'status=no, toolbar=no, scrollbars=yes, resizable=no');
					// Just in case width and height are ignored
					win.resizeTo(w, h);
					// Just in case left and top are ignored
					win.moveTo(wleft, wtop);
					win.focus();
				}
			
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

			function OpenWUdc_Search()
				{
					window.open("Udc_Home_Search.aspx","ss","scrollBars=no,resizable=No,width=400,height=450,status=yes");
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
				
			function addToParentCtrl(value)
				{
					document.getElementById('ContactInfo_txtBr').value=Value;
				}
				
				
			function ContactKey(cc)
				{
					document.getElementById('ClpContact_Info_txtBr').value=cc;
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

			
				
							
			function SaveEdit(varImgValue)
				{
				
			    	if (varImgValue=='Edit')
						{
							if (document.Form1.txthidden.value==0)
								{
									alert("Please select the row");
								}
							else
								{
									document.Form1.txthiddenImage.value=varImgValue;
									document.Form1.txthiddenSkil.value=globalSkil;
									document.Form1.txthidden.value=globalAddNo;	
									document.Form1.txthiddenGrid.value=globalGrid;	
									Form1.submit(); 
								}										
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
													
					if (varImgValue=='Ok')
						{
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 
													
						}
							
					if (varImgValue=='Save')
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
								
				
				function ConfirmDelete(varImgValue,varMessage)
				{
					//alert(document.Form1.txthiddenTable.value);
					//alert(document.Form1.txthiddenSkil.value);
					
					
				  if (document.Form1.txthiddenTable.value == 'cpnlCallTask_dtgTask')
				  {
				
							if (document.Form1.txthiddenSkil.value==0)
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
				  else
				  {
			
						if (document.Form1.txthiddenSkil.value==0)
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
														//document.Form1.txthiddenSkil.value=globalSkil;
														//document.Form1.txthidden.value=globalAddNo;	
														//document.Form1.txthiddenGrid.value=globalGrid;	
														Form1.submit(); 
													}
													else
													{
													}	
							}
				  }
			}
				
				
			function KeyImage(a,b,c,d)
				{							
					if (d==0 ) //if comment is clicked
						{		
							wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+ "'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
						}
					else//if Attachment is clicked
						{
							wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+b ,'Attachment',800,450);
						}
				}
				
					
			function OpenVW(varTable)
				{
					wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ID='+varTable,'Search',500,450);
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
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
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
															<asp:Button ID="btnClick" Runat="server" Width="0" Height="0"></asp:Button><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" CommandName="submit" AlternateText="."
																ImageUrl="white.GIF" Width="1px" Height="1px"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelPsJobme" runat="server" Font-Size="X-Small" Font-Names="Verdana"
																Font-Bold="True" ForeColor="Teal" BorderStyle="None" BorderWidth="2px">Create PSJOBME Job</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
															<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;&nbsp;
															<IMG class="PlusImageCSS" id="Save" title="Save" onclick="SaveEdit('Save');" alt="" src="../Images/S2Save01.gif"
																border="0" name="tbrbtnSave">&nbsp; <IMG class="PlusImageCSS" id="Ok" title="Ok" onclick="SaveEdit('Ok');" alt="" src="../Images/s1ok02.gif"
																border="0" name="tbrbtnOk">&nbsp; <IMG class="PlusImageCSS" id="Reset" title="Reset" onclick="SaveEdit('Reset');" alt="R"
																src="../Images/reset_20.gif" border="0" name="tbrbtnReset">&nbsp; <IMG class="PlusImageCSS" id="Close" title="Close" onclick="SaveEdit('Close');" alt=""
																src="../Images/s2close01.gif" border="0" name="tbrbtnClose">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
														</TD>
														<TD></TD>
														<td>&nbsp;
														</td>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../Images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
													src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;&nbsp;</td>
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
																	<td colSpan="2">
																		<cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
																			BorderColor="Indigo" Visible="False" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
																			Text="Message" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
																			TitleCSS="test">
																			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
																				border="0">
																				<TR>
																					<TD colSpan="0" rowSpan="0">
																						<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../images/warning.gif"></asp:Image></TD>
																					<TD colSpan="0" rowSpan="0">
																						<asp:ListBox id="lstError" runat="server" Height="64px" Width="600" Font-Size="XX-Small"></asp:ListBox></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel>
																		<cc1:collapsiblepanel id="cpnlCreateJob" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
																			BorderColor="Indigo" Visible="True" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
																			Text="Create PSJOBME Job" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black"
																			PanelCSS="panel" TitleCSS="test">
																			<BR>
																			<BR>
																			<TABLE borderColor="#5c5a5b" align="center" bgColor="#f5f5f5" border="1" DESIGNTIMEDRAGDROP="42">
																				<TR>
																					<TD borderColor="#f5f5f5">&nbsp;<BR>
																						<BR>
																					</TD>
																					<TD borderColor="#f5f5f5">
																						<asp:label id="lblProcessCode" runat="server" Height="12px" Width="90px" ForeColor="DimGray"
																							Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Process Code</asp:label><BR>
																						<asp:textbox id="txtProcessCode" runat="server" Height="18px" Width="140px" BorderWidth="1px"
																							BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small" ReadOnly="True" CssClass="txtNoFocus"
																							MaxLength="15">10020011</asp:textbox></TD>
																					<TD borderColor="#f5f5f5">
																						<asp:label id="lblJobDate" runat="server" Height="12px" Width="104px" ForeColor="DimGray" Font-Bold="True"
																							Font-Names="Verdana" Font-Size="XX-Small">Job Date</asp:label><BR>
																						<scontrols:DateSelector id="dtJobDate" runat="server" width="96pt"></scontrols:DateSelector></TD>
																					<TD borderColor="#f5f5f5">&nbsp;</TD>
																				</TR>
																				<TR>
																					<TD borderColor="#f5f5f5">&nbsp;</TD>
																					<TD borderColor="#f5f5f5">
																						<asp:label id="lblMachineCode" runat="server" Height="12px" Width="104px" ForeColor="DimGray"
																							Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Machine Code</asp:label><BR>
																						<asp:textbox id="txtMachineCode" runat="server" Height="18px" Width="140px" BorderWidth="1px"
																							BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small" CssClass="txtNoFocus" MaxLength="17"></asp:textbox><IMG class="PlusImageCSS" onclick="OpenMachineCode('cpnlCreateJob_txtMachineCode');"
																							alt="Call Owner" src="../Images/Plus.gif" border="0"></TD>
																					<TD borderColor="#f5f5f5">
																						<asp:label id="lblJobTime" runat="server" Height="12px" Width="99px" ForeColor="DimGray" Font-Bold="True"
																							Font-Names="Verdana" Font-Size="XX-Small">Job Time</asp:label><BR>
																						<asp:TextBox id="txtJobTime" runat="server" Height="18px" Width="150px" BorderWidth="1px" BorderStyle="Solid"
																							Font-Names="Verdana" Font-Size="XX-Small" CssClass="txtNoFocus" MaxLength="50"></asp:TextBox></TD>
																					<TD borderColor="#f5f5f5">&nbsp;<BR>
																						<BR>
																						<BR>
																					</TD>
																				</TR>
																			</TABLE>
																			<BR>
																			<BR>
																		</cc1:collapsiblepanel>
																	</td>
																</tr>
															</TBODY>
														</table>
													</DIV>
												</TD>
												<td width="12" valign="top" background="../Images/main_line04.gif"><img src="../Images/main_line04.gif" width="12" height="1"></td>
											</TR>
										</TABLE>
									</DIV>
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
					</td>
				</tr>
			</table>
			<INPUT type="hidden" name="txthidden"> <!--Address Nuimber -->
			<INPUT type="hidden" name="txthiddenImage"><!-- Image Clicked-->
		</form>
	</body>
</html>
