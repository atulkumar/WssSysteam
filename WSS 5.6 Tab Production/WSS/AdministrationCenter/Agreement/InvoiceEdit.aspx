<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InvoiceEdit.aspx.vb" Inherits="AdministrationCenter_Agreement_InvoiceEdit" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Invoice Edit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/ >
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>
		<link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet"/>
		<link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet"/>
		<script language="javascript" src="../../DateControl/ION.js" type="text/javascript"></script>
		<script language="Javascript" type="text/javascript">
		
		
							
function calculateHrs()
	{
	
		var rowsCnt,i,limit,ctrlNo;
		var total='0';
		
		rowsCnt = document.getElementById('txtCnt').value;
		document.getElementById('txtTotal').value= '0';
		limit = parseInt(rowsCnt);
		
		for(i=1;i<=limit ;i++)
		{
		ctrlNo = i+1;
		
		if(document.getElementById('GrdAddSerach__ctl'+ctrlNo+'_txtBillHrs'))
		{			
			if (!(document.getElementById('GrdAddSerach__ctl'+ctrlNo+'_txtBillHrs').value ==''))
			{
				total = Number(total) + Number(document.getElementById('GrdAddSerach__ctl'+ctrlNo+'_txtBillHrs').value);
			}
		}
		}	
	
		document.getElementById('txtTotal').value = total;
	}		
		
		
		function NumericHour(ControlID)
			{	
			
			var Val;
			Val = ControlID.value;
			var temp;
			if ( Val.indexOf('.')>=0)
			{
			temp=Val.substr(Val.indexOf('.'));
			//alert(temp);
				if ( temp.length > 2 && (event.keyCode!=13) )
				{	
					event.returnValue = false;
					//alert(temp);
				}
			}
			if (Val.indexOf('.')>0 && event.keyCode==46 )
			{
				event.returnValue = false;
			}
			
			if((event.keyCode<13 || event.keyCode>13) && (event.keyCode<46 || event.keyCode>46) &&( event.keyCode<48 || event.keyCode>57))
				{
					event.returnValue = false;
					alert("Please Enter Used Hours In Numerics Only!");
				}
			calculateHrs();						
			}
			
		 
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
				/*	var rowNo= parseInt(document.getElementById('txtRowNo').value)+1;
					var txtID='cpnlICall_GrdAddSerach__ctl'+rowNo+'_txtBillHrs';
					self.opener.document.getElementById(txtID).value=document.getElementById('txtTotal').value;
					//self.opener.calculateHrs();
				*/
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
									calculateHrs();
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit();
									window.close(); 
									
									
									var rowNo= parseInt(document.getElementById('txtRowNo').value)+1;
					var txtID='cpnlICall_GrdAddSerach__ctl'+rowNo+'_txtBillHrs';
					self.opener.document.getElementById(txtID).value=document.getElementById('txtTotal').value;
					
									
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
									calculateHrs();
									document.Form1.txthiddenImage.value=varImgValue;
									
									Form1.submit();
									
									
									var rowNo= parseInt(document.getElementById('txtRowNo').value)+1;
					var txtID='cpnlICall_GrdAddSerach__ctl'+rowNo+'_txtBillHrs';
					self.opener.document.getElementById(txtID).value=document.getElementById('txtTotal').value;
					
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
						
						if (varImgValue=='Up')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							return false;
						}	
						
						if (varImgValue=='Down')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							return false;
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
		onunload="CloseWindow();" MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
			<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				background="../../images/top_nav_back.gif" border="0">
				<TR>
					<TD style="WIDTH: 159pt">&nbsp;
						<asp:label id="lblTitleLabelInvEdit" runat="server" BorderStyle="None" BorderWidth="2px" Width="88px"
							Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal">Invoice Edit</asp:label></TD>
					<td><IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
						<asp:imagebutton id="imgSave" accessKey="S" runat="server" ToolTip="Save" ImageUrl="../../Images/S2Save01.gif"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgOk" accessKey="K" runat="server" ToolTip="Ok" ImageUrl="../../Images/s1ok02.gif"></asp:imagebutton>&nbsp;<asp:imagebutton id="imgReset" accessKey="R" runat="server" ToolTip="Reset" ImageUrl="../../Images/reset_20.gif"></asp:imagebutton>&nbsp;<asp:imagebutton id="imgClose" accessKey="L" runat="server" ToolTip="Close" ImageUrl="../../Images/s2close01.gif"></asp:imagebutton>&nbsp;<IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">
					</td>
					<td width="42" background="../../images/top_nav_back.gif" height="47">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
				border="0">
				<TR>
					<TD colSpan="0" rowSpan="0"><cc1:collapsiblepanel id="cpnlError" runat="server" BorderStyle="Solid" BorderWidth="0px" Width="100%"
							BorderColor="Indigo" Height="64px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
							Text="Message" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
							Visible="False">
							<TABLE id="Table3" style="WIDTH: 272px; HEIGHT: 19px" borderColor="lightgrey" cellSpacing="0"
								cellPadding="0" width="272" border="0">
								<TR>
									<TD style="WIDTH: 32px" colSpan="0" rowSpan="0">
										<asp:Image id="imgError" runat="server" Width="16px" ImageUrl="../../icons/warning.gif" Height="16px"></asp:Image></TD>
									<TD style="WIDTH: 246px" colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"
											Width="352px" BorderWidth="0" BorderStyle="Groove" Height="32px"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel></TD>
				</TR>
				<TR>
					<TD vAlign="top">External Actions</TD>
				</TR>
				<TR align="center">
					<TD vAlign="top">
						<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 200px">
							<asp:datagrid id="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px" Width="100%"
								Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" DataKeyField="SkillLevel"
								CssClass="grid" PageSize="50" HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0"
								AutoGenerateColumns="False">
								<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
								<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
								<AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
								<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
								<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="CallNo">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "CallNo") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="TaskNo">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "TaskNo") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActNo">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<asp:label ID="lblActNo" Runat="server" height="20" Width="35pt" Text='<%# DataBinder.Eval(Container.DataItem, "ActNo") %>'>
											</asp:label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActDesc">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "ActDesc") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActOwner">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "ActOwner") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="SkillLevel">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<asp:label ID="lblLevel" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SkillLevel") %>'>
											</asp:label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActHrs">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "ActHrs") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActBillHrs">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<asp:TextBox ID="txtBillHrs" Runat="server" CssClass="txtNoFocus" onblur="javascript:calculateHrs();" onkeypress="javascript:NumericHour(this);" Width="50pt" Text='<%# DataBinder.Eval(Container.DataItem, "BillHrs") %>'>
											</asp:TextBox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<asp:CheckBox ID="chkAct1" Runat="server"></asp:CheckBox>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></DIV>
					</TD>
				</TR>
				<tr>
					<td align="center">
						<asp:imagebutton id="imgUp" accessKey="D" runat="server" ImageUrl="../../Images/UpArrow.gif" ToolTip="Delete"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgDown" accessKey="F" runat="server" ImageUrl="../../Images/DownArrow.gif"
							ToolTip="Task Forward"></asp:imagebutton>
					</td>
				</tr>
				<TR>
					<TD>Internal Actions</TD>
				</TR>
				<tr>
					<td><br>
						<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 200px">
							<asp:datagrid id="GrdAddSerach2" runat="server" BorderStyle="None" BorderWidth="1px" Width="100%"
								Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" DataKeyField="SkillLevel"
								CssClass="grid" PageSize="50" HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0"
								AutoGenerateColumns="False">
								<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
								<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
								<AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
								<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
								<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="CallNo">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "CallNo") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="TaskNo">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "TaskNo") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActNo">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<asp:label ID="lblActNo2" Runat="server" height="20" Width="35pt" Text='<%# DataBinder.Eval(Container.DataItem, "ActNo") %>'>
											</asp:label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActDesc">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "ActDesc") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActOwner">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "ActOwner") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="SkillLevel">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "SkillLevel") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActHrs">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "ActHrs") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="ActBillHrs">
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<asp:TextBox ID="txtBillHrs2" Runat="server" ReadOnly=True CssClass="txtNoFocus" Width="50pt" Text='<%# DataBinder.Eval(Container.DataItem, "BillHrs") %>'>
											</asp:TextBox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<HeaderStyle Font-Bold="True"></HeaderStyle>
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
										<ItemTemplate>
											<asp:CheckBox ID="chkAct2" Runat="server"></asp:CheckBox>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
						</DIV>
					</td>
				</tr>
				<TR>
					<TD><asp:textbox id="txtCallNo" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px"
							Font-Size="XX-Small" Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" ReadOnly="True"
							MaxLength="8"></asp:textbox><asp:textbox id="txtTotal" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px" Font-Size="XX-Small"
							Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" MaxLength="8"></asp:textbox><asp:textbox id="txtCnt" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px" Font-Size="XX-Small"
							Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" MaxLength="8" name="txtAddNo"></asp:textbox><asp:textbox id="txtComp" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px" Font-Size="XX-Small"
							Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="8"></asp:textbox>
						<asp:textbox id="txtRowNo" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px" Font-Size="XX-Small"
							Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="8"></asp:textbox>&nbsp;
						<asp:textbox id="txtInvNo" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px" Font-Size="XX-Small"
							Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="8"></asp:textbox><asp:textbox id="txtAgNo" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px" Font-Size="XX-Small"
							Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="8"></asp:textbox>
						<asp:textbox id="txtTaskNo" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px"
							Font-Size="XX-Small" Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" ReadOnly="True"
							MaxLength="8"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:textbox id="txtCType" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px" Font-Size="XX-Small"
							Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="8"></asp:textbox><asp:textbox id="txtTaskType" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="0px"
							Font-Size="XX-Small" Font-Names="Verdana" Height="0px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="8"></asp:textbox></TD>
				</TR>
			</TABLE>
			
  <asp:UpdatePanel ID="PanelUpdate" runat="server">
                        <ContentTemplate>
			<asp:Panel id="pnlMsg" runat="server"></asp:Panel>
			<INPUT type="hidden" name="txthiddenImage"><!-- Image Clicked-->
			
  </ContentTemplate>
                    </asp:UpdatePanel>
			</FORM>
	</body>
</html>
