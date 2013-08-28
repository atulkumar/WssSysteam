<%@ page language="VB" autoeventwireup="false" inherits="CopyPasteDemo, App_Web_n44rks5n" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title>Invoice Detail</title>
		<LINK href="images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
			<LINK href="SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">
				<script language="javascript" src="SupportCenter/calendar/popcalendar.js"></script>
				<script language="javascript" src="images/Js/JSValidation.js"></script>
				<style type="text/css">.DataGridFixedHeader { POSITION: relative; ; TOP: expression(this.offsetParent.scrollTop); BACKGROUND-COLOR: #e0e0e0 }
	</style>
				<script language="javascript" src="DateControl/ION.js"></script>
				<script language="Javascript">
			var globalid;
			var globalSkil;
			var globalAddNo;
			var globalGrid;
			var globaldbclick = 0;
						
			
				function xxyy(r,c,ME)
			{
			//alert(r+'--'+c);
		

		document.Form1.txthidden1.value=r ;
			
document.Form1.txthidden2.value=c ;
			
			
								var tableID='cpnlShow_GrdAddSerach'  //your datagrids id
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
												    table.rows [ r +1 ] . style . backgroundColor = "#d4d4d4";
												}
													ME.style.backgroundColor='skyblue';
			}
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			function OpenW(a,b,c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
					return false;
				}
				
			function OpenAB(c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as CustomerName,CI_VC16_Alias as AliasName from T010011 where CI_VC8_Address_Book_Type='+"'COM'"+ '  &tbname=' + c ,'Search',500,450);
					return false;
				}
				
			function OpenABPer(c)
				{
				var cusComp=document.getElementById('cpnlAGH_txtCusNo').value;
				var sComp='<%=session("propCompanyID")%>';
				
					wopen('../../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as UserName,CI_IN4_Business_Relation as Company from T010011 where CI_IN4_Business_Relation in('+"'"+cusComp+"','"+sComp+"')" + '  &tbname=' + c ,'Search',500,450);
					return false;
				}
			function getVal(val)
			{
				//alert(val);
			}
			
			function PasteFromClipboard()

				{ 



				document.Form1.TextBox1.focus();

				PastedText = document.Form1.TextBox1.createTextRange();

				PastedText.execCommand("Paste");
				Form1.submit();
				return false;
				//window.setTimeout("return true;",2000);
				/*  document.Form1.txtArea.focus();

				PastedText = document.Form1.txtArea.createTextRange();

				PastedText.execCommand("Paste");*/

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
					window.open("Udc_Home_Search.aspx","ss","scrollBars=no,resizable=No,width=350,height=450,status=yes");
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
				
				
			function ContactKey(cc)
				{
					document.getElementById('ClpContact_Info_txtBr').value=cc;
				}					
								
			
					
			function callrefresh()
				{
					Form1.submit();
				}							

																							
			function KeyCheck(nn,rowNo,AgNo,CType,taskNo,taskType)
					{
												
						document.Form1.txthidden.value=nn;
						document.getElementById('txtRowNo').value=rowNo;
						document.getElementById('txtAgNo').value=AgNo;
						document.getElementById('txtCType').value=CType;
						document.getElementById('txtTaskNo').value=taskNo;
						document.getElementById('txtTaskType').value=taskType;
						
//alert(document.Form1.txthidden.value+' '+document.getElementById('txtRowNo').value+' '+document.getElementById('txtAgNo').value+' '+document.getElementById('txtCType').value+' '+document.getElementById('txtTaskNo').value+' '+document.getElementById('txtTaskType').value);		      											
										var tableID='cpnlICall_GrdAddSerach'  //your datagrids id
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
				
			function KeyCheck55(aa,bb,cc,dd)
				{	
					SaveEdit('Edit');
					return false;	
				}
						
			function SaveEdit(varImgValue)
				{
			    	if (varImgValue=='Edit')
						{
						
							//Security Block
							var obj=document.getElementById("imgEdit")
							if(obj==null)
							{
								alert("You don't have access rights to edit record");
								return false;
							}

							if (obj.disabled==true) 
							{
							
								alert("You don't have access rights to edit record");
								return false;
							}
							//End of Security Block
						
							if (document.Form1.txthidden.value==0)
								{
									alert("Please select the row");
								}
							else
								{
									if(document.getElementById('cpnlID_txtInvNo').value=='')
									{
									alert('Save Record');
									return false;
									}
									else 
									{
									var code=document.Form1.txthidden.value;
									var compID=document.Form1.cpnlID_cddlCustomer_txtHID.value;
									var rowNo=document.getElementById('txtRowNo').value;
									var InvNo=document.getElementById('cpnlID_txtInvNo').value;
									var AgNo=document.getElementById('txtAgNo').value;
									var CType=document.getElementById('txtCType').value;
									var TaskNo=document.getElementById('txtTaskNo').value;
									var TaskType=document.getElementById('txtTaskType').value;
									wopen('InvoiceEdit.aspx?CallNo='+code+'&CompID='+compID+'&rowNo='+rowNo+'&InvNo='+InvNo+'&AgNo='+AgNo+'&CType='+CType+'&TaskNo='+TaskNo+'&TaskType='+TaskType,'Search',600,500);
									return false;
									}
								}										
						}	
											
					if (varImgValue=='Close')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							return false;
						}								
							
					if (varImgValue=='Add')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							return false;
						}	
					
					if (varImgValue=='Logout')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
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
					
					if(document.getElementById('cpnlID_txtPC').disabled)
					{
						document.getElementById('txtDisType').value='0';
					}
					else
					{
						document.getElementById('txtDisType').value='1';		
					}
					
					document.getElementById('cpnlID_txtPC').disabled=false;
					
					if (document.getElementById('cpnlID_txtInvNo').value=='')
					{
						if(confirm('Make sure that From and To date for the Invoice are correct.'))
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 		
							return false;	
						}
						else
						{
							return false;
						}
					}
					else
					{
						document.Form1.txthiddenImage.value=varImgValue;
						Form1.submit(); 		
						return false;
					}
						
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
					
					if(document.getElementById('cpnlID_txtPC').disabled)
					{
						document.getElementById('txtDisType').value='0';
					}
					else
					{
						document.getElementById('txtDisType').value='1';		
					}
					
					document.getElementById('cpnlID_txtPC').disabled=false;
					
					if (document.getElementById('cpnlID_txtInvNo').value=='')
					{
						if(confirm('Make sure that From and To date for the Invoice are correct.'))
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 		
							return false;	
						}
						else
						{
							return false;
						}
					}
					else
					{
						document.Form1.txthiddenImage.value=varImgValue;
						Form1.submit(); 		
						return false;
					}
																			
							
				}		
						
					if (varImgValue=='Search')
								{
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
									Form1.reset()
								}		
						}			
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
					alert("Please Enter Numerics Only!");
				}
							
		}	
		
		function disPC()
		{
			var flag;
			flag=confirm('Any discounts given on the Call Types will be erased.\nAre u sure to proceed ?');
			if (flag)
			{
				document.getElementById('cpnlID_txtPC').disabled=false;	
			}
			else
			{
				document.getElementById('cpnlID_txtPC').disabled=true;
			}
		}
		
		function NumericDis(CtrlID)
		{	
		
			var ControlID=document.getElementById(CtrlID);
			var proceed;
			
			if(document.getElementById('cpnlID_txtPC').disabled==false)
			{
			proceed=confirm('This action will autocalculate the total discount.\nAre you sure to give discount on Call Types ?');
			}
			else
			{
			proceed=true;
			}
						
			if (proceed)
			{
			document.getElementById('cpnlID_txtPC').disabled=true;
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
					alert("Please Enter Numerics Only!");
				}
			}
			else
			{
				ControlID.value='';
			}						
		}						
			
	function calculateAmtCall(totAmt,objPC,objDisAmt)
	{
	
		var pc,amt;
		
		pc=document.getElementById(objPC).value;
		amt=totAmt;
		document.getElementById(objDisAmt).value= Number(amt)- (Number(amt)* Number(pc)/100);
		calculateTotalAmt();
	}	
					
function calculateAmt()
	{
	
		var pc,amt;
		
		pc=document.getElementById('cpnlID_txtPC').value;
		amt=document.getElementById('cpnlID_txtTotAmt').value;
		document.getElementById('cpnlID_txtDAmt').value= Number(amt)- (Number(amt)* Number(pc)/100);
		
	}		


function calculateTotalAmt()
	{
	
		var rowsCnt,i,limit,ctrlNo;
		var total='0';
		var pc;
		
		rowsCnt =document.getElementById('txtCntRep').value;
		document.getElementById('cpnlID_txtDAmt').value= '0';
		limit = parseInt(rowsCnt);

		for(i=1;i<=limit ;i++)
		{
		ctrlNo = i+1;
		
		if(document.getElementById('cpnlHrsDetails_grdReport__ctl'+ctrlNo+'_txtDisAmt'))
		{			
			if (!(document.getElementById('cpnlHrsDetails_grdReport__ctl'+ctrlNo+'_txtDisAmt').value ==''))
			{
				total = Number(total) + Number(document.getElementById('cpnlHrsDetails_grdReport__ctl'+ctrlNo+'_txtDisAmt').value);
			}
		}
		}	
	
		document.getElementById('cpnlID_txtDAmt').value = total;
		document.getElementById('cpnlID_txtPC').value=100-(Number(total)*100/ Number(document.getElementById('cpnlID_txtTotAmt').value));
		
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
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<table height="20%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td vAlign="top">
						<table height="20%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<div align="right"><IMG height="2" src="images/top_right_line.gif" width="96"></div>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="images/top_left_back.gif">&nbsp;</td>
											<td width="50"><IMG height="20" src="images/top_right.gif" width="50"></td>
											<td width="21"><A href="#"><IMG onclick="Minimize();" height="20" src="images/bt_min.gif" width="21" border="0"></A></td>
											<td width="21"><A href="#"><IMG onclick="Maximize();" height="20" src="images/bt_max.gif" width="21" border="0"></A></td>
											<td width="19"><A href="#"><IMG onclick="CloseWSS();" height="20" src="images/bt_clo.gif" width="19" border="0"></A></td>
											<td width="6"><IMG height="20" src="images/bt_space.gif" width="6"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="images/top_nav_back.gif" height="67">
												<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
													<TR>
														<TD align="left" width="271"><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="white.GIF"
																CommandName="submit" AlternateText="."></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelInvDetail" runat="server" BorderStyle="None" BorderWidth="2px"
																Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal">Invoice Detail</asp:label></TD>
														<td align="left"><IMG title="Seperator" alt="R" src="images/00Seperator.gif" border="0">&nbsp;
															<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="images/S2Save01.gif" ToolTip="Save"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgOk" accessKey="K" runat="server" ImageUrl="images/s1ok02.gif" ToolTip="Ok"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgEdit" accessKey="E" runat="server" ImageUrl="Images/S2edit01.gif" AlternateText="Edit"
																ToolTip="Edit"></asp:imagebutton>&nbsp;&nbsp;
															<asp:imagebutton id="imgReset" accessKey="R" runat="server" ImageUrl="images/reset_20.gif" ToolTip="Reset"></asp:imagebutton>
															<asp:imagebutton id="imgSearch" accessKey="H" runat="server" ImageUrl="images/s1search02.gif" ToolTip="Search"></asp:imagebutton>&nbsp;&nbsp;
															<asp:imagebutton id="imgClose" accessKey="L" runat="server" ImageUrl="images/s2close01.gif" ToolTip="Close"></asp:imagebutton>&nbsp;<IMG title="Seperator" style="WIDTH: 8px; HEIGHT: 20px" height="20" alt="R" src="images/00Seperator.gif"
																width="8" border="0">
														</td>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
													src="images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp; <IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;&nbsp;</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="images/main_line.gif" height="10"><IMG height="10" src="images/main_line.gif" width="6"></td>
											<td width="7" height="10"><IMG height="10" src="images/main_line01.gif" width="7"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="images/main_line02.gif" height="2"><IMG height="2" src="images/main_line02.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="images/main_line03.gif" width="12"></td>
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
														<table style="BORDER-COLLAPSE: collapse" width="100%" border="0">
															<tr>
																<td colSpan="2">
																	<table cellSpacing="0" cellPadding="0" width="100%" border="0">
																		<tr>
																			<td height="100"><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
																					BorderColor="Indigo" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
																					Text="Message" ExpandImage="images/ToggleDown.gif" CollapseImage="images/ToggleUp.gif" Draggable="False" visible="false">
																					<asp:Image id="ImgError" runat="server" ImageUrl="icons/warning.gif" Height="16px" Width="16px"></asp:Image>
																					<asp:ListBox id="lstError" runat="server" Width="635px" ForeColor="Red" Font-Names="Verdana"
																						Font-Size="XX-Small" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
																					<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
																						border="0">
																						<TR>
																							<TD colSpan="0" rowSpan="0"></TD>
																							<TD colSpan="0" rowSpan="0"></TD>
																						</TR>
																					</TABLE>
																				</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlShow" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
																					BorderColor="Indigo" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
																					Text="Message" ExpandImage="images/ToggleDown.gif" CollapseImage="images/ToggleUp.gif" Draggable="False">
																					<TABLE id="Table1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																						align="left" border="0">
																						<TR>
																							<TD vAlign="top" align="left" height="146">
																								<asp:panel id="Panel1" runat="server"></asp:panel>
																								<asp:datagrid id="GrdAddSerach" runat="server" Width="100%" ForeColor="MidnightBlue" Font-Names="Verdana"
																									BorderWidth="1px" BorderStyle="None" BorderColor="Silver" CellPadding="0" GridLines="Horizontal"
																									HorizontalAlign="Left" PageSize="50" CssClass="grid">
																									<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
																									<AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
																									<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
																									<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
																									<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
																									<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
																								</asp:datagrid>
																								<asp:TextBox id="txtTotal" runat="server" Height="0px" Width="0px" Font-Names="Verdana" Font-Size="XX-Small"
																									BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus" MaxLength="8"></asp:TextBox>
																								<asp:TextBox id="txtCnt" runat="server" Height="0px" Width="0px" Font-Names="Verdana" Font-Size="XX-Small"
																									BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus" MaxLength="8" name="txtAddNo"></asp:TextBox></TD>
																						</TR>
																						<TR>
																							<TD vAlign="top" align="left">
																								<asp:Button id="btnpastedata" accessKey="p" runat="server" Width="0px" Text="Paste data"></asp:Button></TD>
																						</TR>
																					</TABLE>
																				</cc1:collapsiblepanel></td>
																		</tr>
																		<TR>
																			<TD>
																				<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																					align="left" border="0">
																					<TR>
																						<TD vAlign="top" align="left" height="85"><asp:textbox id="TextBox1" runat="server" Width="0px" Height="80px" TextMode="MultiLine"></asp:textbox></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" align="left"></TD>
																					</TR>
																				</TABLE>
																			</TD>
																		</TR>
																	</table>
																</td>
															</tr>
														</table>
													</DIV>
												</TD>
												<td vAlign="top" width="12" background="images/main_line04.gif"><IMG height="1" src="images/main_line04.gif" width="12"></td>
											</TR>
										</TABLE>
									</DIV>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="images/main_line06.gif" height="2"><IMG height="2" src="images/main_line06.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="images/main_line05.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="images/bottom_back.gif">&nbsp;<asp:textbox id="txthidden" runat="server" Width="0px" TextMode="Password"></asp:textbox></td>
											<td width="66"><IMG height="31" src="images/bottom_right.gif" width="66"></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<asp:textbox id="txtTaskNo" runat="server" Width="0px"></asp:textbox><asp:textbox id="txtRowNo" runat="server" Width="0px"></asp:textbox><asp:textbox id="txtAgNo" runat="server" Width="0px"></asp:textbox><asp:textbox id="txtTaskType" runat="server" Width="0px" Height="0px" BorderStyle="Solid" BorderWidth="1px"
				Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="8"></asp:textbox><asp:textbox id="txtCType" runat="server" Width="0px"></asp:textbox><asp:textbox id="txthiddenImage" runat="server" Width="0px"></asp:textbox><asp:textbox id="txtPreInv" runat="server" Width="0px" Height="0px" BorderStyle="Solid" BorderWidth="1px"
				Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="50"></asp:textbox><asp:textbox id="txtCntRep" runat="server" Width="0px" Height="0px" BorderStyle="Solid" BorderWidth="1px"
				Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="50"></asp:textbox><asp:textbox id="txtDisType" runat="server" Width="0px" Height="0px" BorderStyle="Solid" BorderWidth="1px"
				Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus"></asp:textbox>
			<input type="hidden" name="txthidden1"> <input type="hidden" name="txthidden2">
		</form>
	</body>
</HTML>
