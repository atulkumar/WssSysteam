<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AlertFlowEdit.aspx.vb" Inherits="MonitoringCenter_AlertFlowEdit" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Alert Flow Edit</title>
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
								
								
					
			function callrefresh()
				{
					Form1.submit();
				}							

			
				
							
			function SaveEdit(varImgValue)
				{
			
											
					if (varImgValue=='Close')
						{
							window.close();
						}								
					
					if (varImgValue=='Ok')
						{
						
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 
								self.opener.Form1.submit(); 
													
						}
							
					if (varImgValue=='Save')
						{	
							document.Form1.txthiddenImage.value=varImgValue;
							//alert(varImgValue);
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
							wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+b ,'Attachment',800,450);
						}
				}
				
	
				
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" rightMargin="0"
		>
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				background="../images/top_nav_back.gif" border="0">
				<TR>
					<TD width="145"><asp:label id="lblTitleLabelAlertFlowEdit" runat="server" Font-Size="X-Small" Font-Names="Verdana"
							Font-Bold="True" ForeColor="Teal" BorderStyle="None" BorderWidth="2px">&nbsp;Alert Flow Edit</asp:label></TD>
					<TD><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
						<IMG class="PlusImageCSS" title="Save" onclick="SaveEdit('Save');" alt="" src="../Images/S2Save01.gif"
							border="0" name="tbrbtnSave">&nbsp; <IMG class="PlusImageCSS" id="Ok" title="Ok" onclick="SaveEdit('Ok');" alt="" src="../Images/s1ok02.gif"
							border="0" name="tbrbtnOk">&nbsp; <IMG class="PlusImageCSS" id="Reset" title="Reset" onclick="SaveEdit('Reset');" alt="R"
							src="../Images/reset_20.gif" border="0" name="tbrbtnReset">&nbsp; <IMG class="PlusImageCSS" id="Close" title="Close" onclick="SaveEdit('Close');" alt=""
							src="../Images/s2close01.gif" border="0" name="tbrbtnClose">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"></TD>
					<td style="WIDTH: 10px" width="10" background="../images/top_nav_back01.gif" height="67">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Table127" borderColor="activeborder" height="50%" cellSpacing="0" cellPadding="0"
				width="100%" border="0">
				<TR>
					<TD vAlign="top" colSpan="1">
						<!--  **********************************************************************-->
						<TABLE id="Table126" borderColor="#d3d3d3" height="52%" cellSpacing="0" cellPadding="0"
							width="100%" border="0">
							<TR>
								<TD vAlign="top" align="center">
									<cc1:collapsiblepanel id="cpnlError" runat="server" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
										Visible="False" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
										Text="Message" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
										TitleCSS="test" Width="100%">
										<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<TR>
												<TD colSpan="0" rowSpan="0">
													<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../images/warning.gif"></asp:Image></TD>
												<TD colSpan="0" rowSpan="0">
													<asp:ListBox id="lstError" runat="server" Font-Size="XX-Small" Width="300" Height="64px"></asp:ListBox></TD>
											</TR>
										</TABLE>
									</cc1:collapsiblepanel>
									<cc1:collapsiblepanel id="cpnlReport" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
										Draggable="False" CollapseImage="../Images/white.gif" ExpandImage="../Images/white.gif" TitleBackColor="Transparent"
										TitleClickable="false" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" BorderColor="Indigo"
										Visible="true" Text="Alert Flow Edit">
										<TABLE borderColor="#5c5a5b" align="center" bgColor="#f5f5f5" border="0">
											<TR>
												<TD style="WIDTH: 10px; HEIGHT: 25px" borderColor="#f5f5f5">&nbsp;</TD>
												<TD style="HEIGHT: 25px" borderColor="#f5f5f5">
													<asp:label id="lblJobDate" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
														ForeColor="DimGray" Width="104px" Height="6px">Alert Action Type</asp:label><BR>
													<asp:DropDownList id="DdlAlertType" runat="server" Width="150pt" AutoPostBack="True">
														<asp:ListItem></asp:ListItem>
														<asp:ListItem Value="EML">EML</asp:ListItem>
														<asp:ListItem Value="WSS">WSS</asp:ListItem>
													</asp:DropDownList></TD>
												<TD style="HEIGHT: 25px" borderColor="#f5f5f5">
													<asp:label id="Label1" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
														ForeColor="DimGray" Width="90px" Height="12px">Status</asp:label><BR>
													<uc1:CustomDDL id="CDDLStatus" runat="server"></uc1:CustomDDL></TD>
												<TD style="HEIGHT: 25px" borderColor="#f5f5f5">&nbsp;</TD>
											</TR>
											<TR>
												<TD style="WIDTH: 10px; HEIGHT: 68px" borderColor="#f5f5f5">&nbsp;<BR>
													<BR>
												</TD>
												<TD style="HEIGHT: 68px" borderColor="#f5f5f5">
													<asp:label id="lblMachineCode" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
														ForeColor="DimGray" Width="121px" Height="12px">Communication Mode</asp:label><BR>
													<asp:textbox id="txtCommMode" runat="server" Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid"
														BorderWidth="1px" Width="150pt" Height="18px" MaxLength="50" CssClass="txtNoFocus"></asp:textbox></TD>
												<TD style="HEIGHT: 68px" borderColor="#f5f5f5">
													<asp:label id="lblJobTime" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
														ForeColor="DimGray" Width="99px" Height="12px">User</asp:label><BR>
													<uc1:CustomDDL id="CDDLUser" runat="server"></uc1:CustomDDL>
												<TD style="HEIGHT: 68px" borderColor="#f5f5f5">&nbsp;<BR>
													<BR>
													<BR>
												</TD>
											</TR>
											<TR>
												<TD style="WIDTH: 10px; HEIGHT: 25px" borderColor="#f5f5f5">&nbsp;<BR>
													<BR>
												</TD>
												<TD style="HEIGHT: 25px" borderColor="#f5f5f5">
													<asp:label id="LbltempType" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
														ForeColor="DimGray" Width="99px" Height="12px">Template Type</asp:label><BR>
													<asp:DropDownList id="ddlTemplateType" runat="server" Width="150pt" AutoPostBack="True">
														<asp:ListItem></asp:ListItem>
														<asp:ListItem Value="CNT">CNT</asp:ListItem>
														<asp:ListItem Value="CAO">CAO</asp:ListItem>
													</asp:DropDownList></TD>
												<TD style="HEIGHT: 25px" borderColor="#f5f5f5">
													<asp:label id="LblTemplate" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
														ForeColor="DimGray" Width="121px" Height="12px">Template</asp:label><BR>
													<asp:DropDownList id="Ddltemp" runat="server" Width="150pt"></asp:DropDownList>
												<TD style="HEIGHT: 25px" borderColor="#f5f5f5">&nbsp;<BR>
													<BR>
													<BR>
												</TD>
											</TR>
										</TABLE>
									</cc1:collapsiblepanel></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			<!-- **********************************************************************-->  <!-- Image Clicked--><INPUT type="hidden" name="txthidden"> <!--Address Nuimber --><INPUT type="hidden" name="txthiddenImage"><!-- Image Clicked-->
			<INPUT type="hidden" name="txtSave">
		</FORM>
	</body>
</html>
