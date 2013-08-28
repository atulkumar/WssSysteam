<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_AlertEdit, App_Web_2azc-idb" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../SupportCenter/calendar/DateSelector.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Alert Add/Edit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../Images/Js/ION.js"></script>
		<LINK href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../SupportCenter/calendar/popcalendar.js"></script>
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<style type="text/css">.DataGridFixedHeader { POSITION: relative; ; TOP: expression(this.offsetParent.scrollTop); BACKGROUND-COLOR: #e0e0e0 }
		</style>
		<script language="Javascript">



			var globalid;
			var globaldbclick = 0;
						
			function OpenW(a,b,c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
				}
			
				
			function OpenAB(c)
				{
						wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'Search',500,450);
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
								
				

			
				
							
			function SaveEdit(varImgValue)
				{
											
					if (varImgValue=='Close')
						{
								window.close();
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
							self.opener.callrefresh();		
						}
							
					if (varImgValue=='Save')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							self.opener.callrefresh();		
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

		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="white" leftMargin="0" topMargin="0" 
		rightMargin="0">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				background="../images/top_nav_back.gif" border="0">
				<TR>
					<TD width="145">&nbsp;&nbsp;&nbsp;<asp:label id="lblTitleLabelAlertEdit" runat="server" Font-Size="X-Small" Font-Names="Verdana"
							Font-Bold="True" ForeColor="Teal" BorderStyle="None" BorderWidth="2px" Width="120px">&nbsp;Alert Add/Edit</asp:label></TD>
					<TD width="445">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp; 
						&nbsp; <IMG class="PlusImageCSS" id="Save" title="Save" onclick="SaveEdit('Save');" alt="" src="../Images/S2Save01.gif"
							border="0" name="tbrbtnSave">&nbsp; <IMG class="PlusImageCSS" id="Ok" title="Ok" onclick="SaveEdit('Ok');" alt="" src="../Images/s1ok02.gif"
							border="0" name="tbrbtnOk">&nbsp; <IMG class="PlusImageCSS" id="Reset" title="Reset" onclick="SaveEdit('Reset');" alt="R"
							src="../Images/reset_20.gif" border="0" name="tbrbtnReset">&nbsp; &nbsp; <IMG class="PlusImageCSS" id="Close" title="Close" onclick="SaveEdit('Close');" alt=""
							src="../Images/s2close01.gif" border="0" name="tbrbtnClose">&nbsp; &nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
					</TD>
					<td width="10" background="../images/top_nav_back01.gif" height="67" style="WIDTH: 10px">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Table126" borderColor="activeborder" height="100%" cellSpacing="0" cellPadding="0"
				width="100%" border="1">
				<TR>
					<TD vAlign="top" colSpan="1">
						<!--  **********************************************************************-->
						<TABLE id="Table2" borderColor="#d3d3d3" height="96%" cellSpacing="0" cellPadding="0"
							width="100%" border="2">
							<TR>
								<TD vAlign="top" align="center"><cc1:collapsiblepanel id="cpnlError" runat="server" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
										Visible="False" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Message"
										TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Width="100%">
										<TABLE id="Table3" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<TR>
												<TD colSpan="0" rowSpan="0">
													<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../images/warning.gif"></asp:Image></TD>
												<TD colSpan="0" rowSpan="0">
													<asp:ListBox id="lstError" runat="server" Width="400px" BorderWidth="0" BorderStyle="Groove"
														ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox></TD>
											</TR>
										</TABLE>
									</cc1:collapsiblepanel>
									<TABLE id="Table4" height="89" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="center" width="100%" height="89"><cc1:collapsiblepanel id="Collapsiblepanel1" runat="server" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
													Visible="true" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Alert Edit" TitleBackColor="Transparent"
													TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Width="100%"><BR>
													<BR>
													<TABLE style="WIDTH: 345px; HEIGHT: 233px" borderColor="#5c5a5b" align="center" bgColor="#f5f5f5"
														border="1" DESIGNTIMEDRAGDROP="42">
														<TR>
															<TD style="HEIGHT: 36px" borderColor="#f5f5f5" width="14">&nbsp;</TD>
															<TD style="HEIGHT: 36px" borderColor="#f5f5f5" width="179">
																<asp:label id="lblAlertType" runat="server" Width="72px" ForeColor="DimGray" Font-Bold="True"
																	Font-Names="Verdana" Font-Size="XX-Small" Height="12px">Alert Type</asp:label><BR>
																<asp:DropDownList id="ddlAlertType" runat="server" Width="160px" Font-Names="Verdana" Font-Size="XX-Small"
																	Height="18px"></asp:DropDownList></TD>
															<TD style="HEIGHT: 36px" borderColor="#f5f5f5"></TD>
														</TR>
														<TR>
															<TD borderColor="#f5f5f5"></TD>
															<TD style="HEIGHT: 36px" borderColor="#f5f5f5">
																<asp:label id="Label1" runat="server" Width="104px" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
																	Font-Size="XX-Small" Height="12px">Alert Code</asp:label><BR>
																<asp:textbox id="txtAlertCode" runat="server" Width="372" BorderWidth="1px" BorderStyle="Solid"
																	Font-Names="Verdana" Font-Size="XX-Small" Height="18px" MaxLength="150" CssClass="txtNoFocus"></asp:textbox></TD>
															<TD style="HEIGHT: 36px" borderColor="#f5f5f5"></TD>
														</TR>
														<TR>
															<TD borderColor="#f5f5f5"></TD>
															<TD style="HEIGHT: 36px" borderColor="#f5f5f5">
																<asp:label id="lblAlertSubject" runat="server" Width="104px" ForeColor="DimGray" Font-Bold="True"
																	Font-Names="Verdana" Font-Size="XX-Small" Height="12px">Alert Subject</asp:label><BR>
																<asp:textbox id="txtAlertSubject" runat="server" Width="372" BorderWidth="1px" BorderStyle="Solid"
																	Font-Names="Verdana" Font-Size="XX-Small" Height="18px" MaxLength="150" CssClass="txtNoFocus"></asp:textbox></TD>
															<TD style="HEIGHT: 36px" borderColor="#f5f5f5"></TD>
														</TR>
														<TR>
															<TD borderColor="#f5f5f5"></TD>
															<TD borderColor="#f5f5f5" width="14" colSpan="3">
																<asp:label id="lblMachineDescription" runat="server" Width="128px" ForeColor="DimGray" Font-Bold="True"
																	Font-Names="Verdana" Font-Size="XX-Small" Height="12px">Alert Description</asp:label>
																<asp:TextBox id="txtAlertDescription" runat="server" Width="372" BorderWidth="1px" BorderStyle="Solid"
																	Font-Names="Verdana" Font-Size="XX-Small" Height="119px" MaxLength="500" CssClass="txtNoFocus"
																	TextMode="MultiLine"></asp:TextBox></TD>
															<TD borderColor="#f5f5f5" width="14"></TD>
														</TR>
													</TABLE>
													<BR>
													<BR>
												</cc1:collapsiblepanel></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE> <!-- **********************************************************************--> 
			<INPUT type="hidden" name="txthidden"> <!--Address Nuimber --><INPUT type="hidden" name="txthiddenImage"><!-- Image Clicked-->
		</FORM>
	</body>
</html>
