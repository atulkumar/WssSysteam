<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_ProcessEnvironmentEntry, App_Web_vrtyhdgv" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Process Environment Edit</title>
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
					wopen('../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search1',500,450);
				}
			
				
			function OpenProcessCode(c)
				{
						wopen('../Search/Common/PopSearch.aspx?ID=select PM_IN4_PCode as ID, PM_VC20_PName as ProcessName, PM_VC10_PType as ProcessType from T130031' + '  &tbname=' + c ,'Search2',500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,ab.CI_VC36_Name as Name,UL_VC8_Role_PK as Role from T010011 ab,T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'Search',500,450);
				}
				
				function OpenSystemID(c)
				{
						wopen('../Search/Common/PopSearch.aspx?ID=select MM_VC20_MIP as ID, MM_VC20_MName as MachineName, MM_VC100_MDesc as Description from T130011' + '  &tbname=' + c ,'Search3',500,450);
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
						window.close();
						//self.opener.callrefresh();				
						}								
					
					if (varImgValue=='Logout')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
						}
													
					if (varImgValue=='Ok')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							window.close();
							Form1.submit();
							self.opener.Form1.submit(); 	
						}
							
					if (varImgValue=='Save')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							self.opener.Form1.submit(); 	
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
							wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+b ,'Attachment',500,450);
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
	<body bottomMargin="0" bgColor="white" leftMargin="0" topMargin="0" rightMargin="0" >
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" background="../images/top_nav_back.gif"
				border="0">
				<TR>
					<TD style="WIDTH: 128px" align="left"><asp:label id="lblTitleLabelProcessEnvEdit" runat="server" Font-Size="X-Small" Font-Names="Verdana"
							Font-Bold="True" ForeColor="Teal" BorderStyle="None" BorderWidth="2px">Process Env Edit</asp:label></TD>
					<TD align="center"><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
						<IMG class="PlusImageCSS" id="Save" title="Save" onclick="SaveEdit('Save');" src="../Images/S2Save01.gif"
							border="0" name="tbrbtnSave">&nbsp;&nbsp; <IMG class="PlusImageCSS" id="Ok" title="Ok" onclick="SaveEdit('Ok');" alt="" src="../Images/s1ok02.gif"
							border="0" name="tbrbtnOk">&nbsp; <IMG class="PlusImageCSS" id="Reset" title="Reset" onclick="SaveEdit('Reset');" alt="R"
							src="../Images/reset_20.gif" border="0" name="tbrbtnReset">&nbsp; <IMG class="PlusImageCSS" id="Close" title="Close" onclick="SaveEdit('Close');" alt=""
							src="../Images/s2close01.gif" border="0" name="tbrbtnClose">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"></TD>
					<td width="42" background="../images/top_nav_back01.gif" height="67">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Table126" borderColor="#d3d3d3" height="450" cellSpacing="0" cellPadding="0"
				width="450" border="2">
				<TR>
					<TD vAlign="top" align="center"><cc1:collapsiblepanel id="cpnlError" runat="server" BorderStyle="Solid" BorderWidth="0px" Width="100%"
							BorderColor="Indigo" Visible="False" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
							Text="Message" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD colSpan="0" rowSpan="0">
										<asp:Image id="ImgError" runat="server" Width="16px" ImageUrl="../images/warning.gif" Height="16px"></asp:Image></TD>
									<TD colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" ForeColor="Red"
											Font-Names="Verdana" Font-Size="XX-Small" Width="400px"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel>
						<TABLE id="Table3" height="89" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" align="center" width="100%" height="89"><cc1:collapsiblepanel id="ProcessEnvironmentEdit" runat="server" BorderStyle="Solid" BorderWidth="0px"
										Width="100%" BorderColor="Indigo" Visible="True" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Process Environment"
										TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"><BR>
										<BR>
										<TABLE borderColor="#5c5a5b" align="center" bgColor="#f5f5f5" border="1" DESIGNTIMEDRAGDROP="42">
											<TR>
												<TD borderColor="#f5f5f5">&nbsp;
												</TD>
												<TD vAlign="top" borderColor="#f5f5f5">
													<asp:label id="lblEnv" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
														Font-Size="XX-Small" Width="90px" Height="12px">Environment</asp:label><BR>
													<asp:DropDownList id="ddlEnv" runat="server" Width="164px"></asp:DropDownList></TD>
												<TD style="WIDTH: 178px" vAlign="top" borderColor="#f5f5f5">
													<asp:label id="lblDB" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
														Font-Size="XX-Small" Width="90px" Height="12px">Data Base</asp:label><BR>
													<asp:textbox id="txtDatabase" runat="server" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
														Font-Size="XX-Small" Width="164px" Height="18px" CssClass="txtNoFocus" MaxLength="15"></asp:textbox></TD>
												<TD borderColor="#f5f5f5">&nbsp;</TD>
											</TR>
											<TR>
												<TD borderColor="#f5f5f5">&nbsp;</TD>
												<TD vAlign="top" borderColor="#f5f5f5">
													<asp:label id="lblDatabase" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
														Font-Size="XX-Small" Width="104px" Height="12px">TB Owner</asp:label><BR>
													<asp:textbox id="txtOwner" runat="server" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
														Font-Size="XX-Small" Width="164px" Height="18px" CssClass="txtNoFocus" MaxLength="30"></asp:textbox></TD>
												<TD style="WIDTH: 178px" vAlign="top" borderColor="#f5f5f5">
													<asp:label id="lblMachine" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
														Font-Size="XX-Small" Width="104px" Height="12px">Machine</asp:label><BR>
													<asp:textbox id="txtMachine" runat="server" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
														Font-Size="XX-Small" Width="164px" Height="18px" CssClass="txtNoFocus" MaxLength="30"></asp:textbox></TD>
											</TR>
											<TR>
												<TD borderColor="#f5f5f5"></TD>
												<TD vAlign="top" borderColor="#f5f5f5">
													<asp:label id="lblJobTime" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
														Font-Size="XX-Small" Width="99px" Height="12px">UserID</asp:label><BR>
													<asp:TextBox id="txtUserID" runat="server" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
														Font-Size="XX-Small" Width="164px" Height="18px" CssClass="txtNoFocus" MaxLength="50"></asp:TextBox></TD>
												<TD style="WIDTH: 178px" vAlign="top" borderColor="#f5f5f5">
													<asp:label id="Label1" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
														Font-Size="XX-Small" Width="99px" Height="12px">Password</asp:label><BR>
													<asp:TextBox id="txtPassword" runat="server" BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana"
														Font-Size="XX-Small" Width="164px" Height="18px" CssClass="txtNoFocus" MaxLength="50" TextMode="Password"></asp:TextBox></TD>
												<TD borderColor="#f5f5f5"><BR>
													<BR>
													</TD>
											</TR>
										</TABLE>
										<BR>
										<BR>
									</cc1:collapsiblepanel></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE> <!-- **********************************************************************--> 
			<INPUT type="hidden" name="txthidden"> <!--Address Nuimber --><INPUT type="hidden" name="txthiddenImage">
			<INPUT type="hidden" name="txtSave"><!-- Image Clicked-->
		</FORM>
	</body>
</html>
