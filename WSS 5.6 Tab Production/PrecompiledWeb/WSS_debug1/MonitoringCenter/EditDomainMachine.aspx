<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_EditDomainMachine, App_Web_y4bhs5yb" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Domain Machine Edit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<LINK href="../calendar/popcalendar.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../DateControl/ION.js"></script>
		<script language="Javascript">
			
		 
		function OpenW(a,b,c)
				{
				
				//wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				//window.showModalDialog
			wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,"",500,450);
				}
				
		function OpenComm(a,b)
				{
				
				wopen('comment.aspx?ScrID=329&ID='+ b + '&tbname=T' ,'Comments',500,450);
				
				}
				
		function OpenAtt()
				{
						
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=T','Additional_Address',400,450);
				
				}
		function OpenAB(c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'',500,450);										
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName, ab.CI_VC36_Name as Name, UL_VC8_Role_PK as Role from T010011 ab, T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'',500,450);					
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
					//alert(Afilename);
						document.getElementById(TbName).value=Afilename;
						aa=Afilename;
					}
					else
					
					{
						//document.Form1.txtAB_Type.value=aa;
					}
				}
				

						
			function CloseWindow()
				{
					self.opener.callrefresh();
				}			
				
				
				
					function SaveEdit(varImgValue)
				{
			    		
												
						if (varImgValue=='Close')
						{
								window.close();
								return false; 
						}
								
								
															
						if (varImgValue=='Ok')
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
									window.close(); 
									return false; 
									 
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
							
						if (varImgValue=='Reset')
						{
									var confirmed
									confirmed=window.confirm("Do You Want To reset The Page ?");
									if(confirmed==true)
											{	
													Form1.reset();
													return false;
											}		

						}			
				}			
					
					function callrefresh()
				{
					document.Form1.txthiddenImage.value='';
			    	Form1.submit();
				}
				
				
					function FP_swapImg() {//v1.0
							var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
							n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
							elm.$src=elm.src; elm.src=args[n+1]; } }
							}

							function FP_preloadImgs() {//v1.0
							var d=document,a=arguments; if(!d.FP_imgs) d.FP_imgs=new Array();
							for(var i=0; i<a.length; i++) { d.FP_imgs[i]=new Image; d.FP_imgs[i].src=a[i]; }
							}

							function FP_getObjectByID(id,o) {//v1.0
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
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				background="../images/top_nav_back.gif" border="0">
				<TR>
					<TD width="25%">&nbsp;
						<asp:label id="Label4" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
							Font-Size="X-Small" Width="176px" BorderWidth="2px" BorderStyle="None">DOMAIN MACHINE EDIT</asp:label></TD>
					<td><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
						<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgOk" accessKey="K" runat="server" ImageUrl="../Images/s1ok02.gif"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgReset" accessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgClose" accessKey="L" runat="server" ImageUrl="../Images/s2close01.gif"></asp:imagebutton>&nbsp;&nbsp;<IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
					</td>
					<td width="42" background="../images/top_nav_back01.gif" height="67">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Table12" style="HEIGHT: 19px" borderColor="lightgrey" cellSpacing="0" cellPadding="0"
				width="100%" border="0">
				<TR>
					<TD Width="100%" colSpan="0" rowSpan="0">
						<cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
							Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
							Text="Message" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif" Draggable="False"
							Height="81px" BorderColor="Indigo">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD style="WIDTH: 32px" colSpan="0" rowSpan="0">
										<asp:Image id="imgError" runat="server" Width="16px" ImageUrl="../icons/warning.gif" Height="16px"></asp:Image></TD>
									<TD style="WIDTH: 246px" colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="352px"
											Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel>
						<TABLE id="Table4" borderColor="#f5f5f5" Width="100%" bgColor="#f5f5f5">
							<TR>
								<TD vAlign="top" borderColor="#f5f5f5" align="left" width="422" colSpan="2" height="24">
									<asp:label id="Label3" runat="server" Width="72px" Font-Size="XX-Small" Font-Names="Verdana"
										Font-Bold="True" ForeColor="DimGray">Machine ID</asp:label>
									<asp:textbox id="txtUser" runat="server" BorderStyle="Solid" BorderWidth="0px" Width="56px" Font-Size="XX-Small"
										Font-Names="Verdana" ForeColor="Black" Height="18px" CssClass="txtNoFocus" MaxLength="150"
										BackColor="WhiteSmoke"></asp:textbox></TD>
							</TR>
							<TR>
								<TD vAlign="top" borderColor="#f5f5f5" align="left" width="422">
									<asp:label id="Label6" runat="server" Width="115px" Font-Size="XX-Small" Font-Names="Verdana"
										Font-Bold="True" ForeColor="DimGray" Height="13px">Machine Name</asp:label><br>
									<asp:textbox id="txtMachName" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
										Font-Size="XX-Small" Font-Names="Verdana" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
										MaxLength="150"></asp:textbox></TD>
								<TD vAlign="top" borderColor="#f5f5f5" align="left" width="422" height="24">
									<asp:label id="lblName9" runat="server" Width="115px" Font-Size="XX-Small" Font-Names="Verdana"
										Font-Bold="True" ForeColor="DimGray" Height="13px">Machine Type</asp:label><br>
									<uc1:CustomDDL id="cddlType" runat="server"></uc1:CustomDDL></TD>
							</TR>
							<TR>
								<TD vAlign="top" borderColor="#f5f5f5" align="left" width="422" rowSpan="1"><asp:label id="lblName6" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
										Font-Size="XX-Small" Width="85px" Height="12px"> Cat 1</asp:label><br>
									<asp:textbox id="txtCat1" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129px"
										BorderWidth="1px" BorderStyle="Solid" Height="18px" MaxLength="20" CssClass="txtNoFocus"></asp:textbox></TD>
								<TD vAlign="top" borderColor="#f5f5f5" align="left" width="422" height="24" rowSpan="1">
									<asp:label id="Label1" runat="server" Width="80px" Font-Size="XX-Small" Font-Names="Verdana"
										Font-Bold="True" ForeColor="DimGray">Cat 2</asp:label><br>
									<asp:textbox id="txtCat2" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
										Font-Size="XX-Small" Font-Names="Verdana" Height="18px" CssClass="txtNoFocus" MaxLength="20"></asp:textbox></TD>
							</TR>
							<TR>
								<TD vAlign="top" borderColor="#f5f5f5" align="left" width="422" rowSpan="1">
									<asp:label id="Label2" runat="server" Width="112px" Font-Size="XX-Small" Font-Names="Verdana"
										Font-Bold="True" ForeColor="DimGray" Height="12px">Cat 3</asp:label><br>
									<asp:textbox id="txtCat3" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
										Font-Size="XX-Small" Font-Names="Verdana" Height="18px" CssClass="txtNoFocus" MaxLength="20"></asp:textbox></TD>
								<TD vAlign="top" borderColor="#f5f5f5" align="left" width="422" height="24" rowSpan="1">
									<asp:label id="lblName8" runat="server" Width="85px" Font-Size="XX-Small" Font-Names="Verdana"
										Font-Bold="True" ForeColor="DimGray" Height="12px">Status</asp:label><br>
									<asp:radiobuttonlist id="rblSts" runat="server" Width="149px" Font-Size="XX-Small" Font-Names="Verdana"
										RepeatDirection="Horizontal">
										<asp:ListItem Value="E" Selected="True">Enable</asp:ListItem>
										<asp:ListItem Value="D">Disable</asp:ListItem>
									</asp:radiobuttonlist></TD>
							</TR>
							<TR>
								<TD vAlign="top" borderColor="#f5f5f5" align="left" Width="100%" colSpan="2">
									<asp:textbox id="txtDomID" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px" Font-Size="XX-Small"
										Font-Names="Verdana" Height="18px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="8"></asp:textbox>
									<asp:textbox id="txthiddenImage" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px"
										Font-Size="XX-Small" Font-Names="Verdana" Height="18px" CssClass="txtNoFocus" ReadOnly="True"
										MaxLength="8"></asp:textbox>
									<asp:datagrid id="grdDom" runat="server" BorderStyle="None" BorderWidth="1px" Font-Names="Verdana"
										ForeColor="MidnightBlue" BorderColor="Silver" CssClass="grid" CellPadding="0" GridLines="Horizontal"
										HorizontalAlign="Left" PageSize="50">
										<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
										<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
										<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
										<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
										<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE> <!-- Image Clicked--></FORM>
	</body>
</html>
