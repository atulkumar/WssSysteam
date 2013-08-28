<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@Register TagPrefix="SControls" TagName="DateSelector" src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Template_TemplateFormEntryDetails, App_Web_u8bqjff1" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Form Entry Details</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<LINK href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../../SupportCenter/calendar/popcalendar.js"></script>
		<script language="javascript" src="../../Images/Js/JSValidation.js"></script>
		<script language="javascript" src="../../DateControl/ION.js"></script>
		<style type="text/css">.DataGridFixedHeader { POSITION: relative; ; TOP: expression(this.offsetParent.scrollTop); BACKGROUND-COLOR: #e0e0e0 }
		</style>
		<script language="Javascript">
var rand_no = Math.ceil(500*Math.random())

			var globalid;
			var globalSkil;
			var globalAddNo;
			var globalGrid;
			var globaldbclick = 0;
				
				
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

			
				
			function OpenTask(varTable)
				{
					///wopen('Task_edit.aspx?ScrID=334&TASKNO='+varTable,'Search',430,300);
				}
				
			function KeyCheckTaskEdit(nn,rowvalues,tableID)
				{		
					globaldbclick = 1;
					document.Form1.txthiddenCallNo.value=nn;
					document.Form1.txthidden.value=nn;
					document.Form1.txthiddenImage.value='Edit';
					document.Form1.txthiddenTable.value=tableID;
			
					//alert(nn);
					if (tableID=='cpnlCallTask_dtgTask')
						{
							OpenTask(nn);
						}
					else if(tableID=='cpnlTaskAction_grdAction')
					{
						
						wopen('Action_edit.aspx?ScrID=294&ACTIONNO='+nn,'Search'+rand_no,430,300);
					}
					else
						{
							Form1.submit(); 
						}													
				}								
				
																				
			function KeyCheck(aa,cc,dd)
				{
					globalid = cc;
					globalSkil  = aa;
					//globalAddNo = bb;
					globalGrid = dd;
					
					//alert(aa);
					//alert(cc);
					//alert(dd);
					//document.Form1.txthiddenSkil.value=aa;
					//document.Form1.txthidden.value=aa;
					//document.Form1.txtrowvalues.value=cc;
					//document.Form1.txthiddenTable.value=dd;
					
						var tableID=dd  //your datagrids id
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
								table.rows [ cc  ] . style . backgroundColor = "#d4d4d4";
							}
							if(tableID=='cpnlCallTask_dtgTask')
							{
								document.Form1.txthiddenImage.value='Select';
								setTimeout('Form1.submit();',700);
							}
						   	//Form1.submit(); 
					}
				
			function KeyCheck55(aa,bb,cc)
				{		
				
					if (cc=='f')
					{
					document.Form1.txthiddenImage.value='';	
					}
					else
					{
					document.Form1.txthiddenImage.value='Refresh';	
					}
					wopen('Object_Assignment.aspx?ScrID=261&codeID='+aa+'&rowNo='+bb,'Search'+rand_no,500,400);
					
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
									wopen('Role_edit.aspx?ScrID=354','FWD'+rand_no,400,250);
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
							document.Form1.txthiddenImage.value="";
							Form1.submit(); 
							//return false;
						}	
					
					if (varImgValue=='Logout')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
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
							//self.opener.Form1.submit(); 
							return false;
						}		
						
					if (varImgValue=='Attach')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							return false;
						}	
					
					if (varImgValue=='Fwd')
						{
						   if (	document.Form1.txtrowvalues.value==0)
						   {
								alert("Please select the row");
							}
							else
							{	
								 wopen('Task_Fwd.aspx?ScrID=340','FWD'+rand_no,400,250);
							}	 
						   
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
							if (document.Form1.txthidden.value==0)
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
												document.Form1.txthiddenSkil.value=globalSkil;
												document.Form1.txthidden.value=globalAddNo;	
												document.Form1.txthiddenGrid.value=globalGrid;	
												Form1.submit(); 
											}
											else
											{
											}	
								}
				}
				
			function KeyImage(a,b,c,d)
				{							
					if (d==0 ) //if comment is clicked
						{		
							wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+ "'"+b+"'"+' &tbname=' + c ,'Comment'+rand_no,500,450);
						}
					else//if Attachment is clicked
						{
							wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+b ,'Attachment'+rand_no,800,450);
						}
				}
				
					
			function OpenVW(varTable)
				{
					wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ID='+varTable,'Search'+rand_no,500,450);
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
			function RefreshParent()
			{
				self.opener.Form1.submit();
			}				
		</script>

    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0"
		onunload="RefreshParent();">
		<FORM id="Form1" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
			<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				background="../../Images/top_nav_back.gif" border="0">
				<TR>
					<TD width="365">&nbsp;
						<asp:label id="lblTitleLabelFormEntryDetail" runat="server" ForeColor="Teal" Font-Bold="True"
							Font-Names="Verdana" Font-Size="X-Small" Width="144px" Height="12px" BorderWidth="2px"
							BorderStyle="None">Form Entry Details</asp:label>
					</TD>
					<td><IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
						<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif" ToolTip="Save"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgOk" accessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif" ToolTip="Ok"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgReset" accessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
							ToolTip="Reset"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgClose" accessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
							ToolTip="Close"></asp:imagebutton>&nbsp; <IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
					</td>
					<td width="42" background="../../Images/top_nav_back01.gif" height="67">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Table126" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				border="1">
				<TR>
					<TD vAlign="top" colSpan="1">
						<!--  **********************************************************************-->
						<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 530px">
							<TABLE id="Table7" cellSpacing="0" cellPadding="2" width="100%" border="0">
								<tr>
									<td colSpan="2"><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
											BorderColor="Indigo" Visible="False" Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
											Text="Message" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
											<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
												border="0">
												<TR>
													<TD colSpan="0" rowSpan="0">
														<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../../icons/warning.gif"></asp:Image></TD>
													<TD colSpan="0" rowSpan="0">
														<asp:ListBox id="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="635px"
															Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox></TD>
												</TR>
											</TABLE>
										</cc1:collapsiblepanel></td>
								</tr>
								<TR>
									<TD vAlign="top" colSpan="1">
										<!-- **********************************************************************-->
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="50%" border="0">
											<TR>
												<TD><asp:label id="Label1" runat="server" Width="72px" Font-Size="XX-Small" Font-Names="Verdana"
														Font-Bold="True" ForeColor="DimGray">Form Name</asp:label></TD>
												<TD><asp:textbox id="txtFormName" runat="server" Width="129px" Font-Size="XX-Small" Font-Names="Verdana"
														BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="True"></asp:textbox></TD>
												<TD><asp:label id="Label3" runat="server" Width="48px" Font-Size="XX-Small" Font-Names="Verdana"
														Font-Bold="True" ForeColor="DimGray">Company</asp:label></TD>
												<TD><asp:textbox id="txtCompany" runat="server" Width="129px" Font-Size="XX-Small" Font-Names="Verdana"
														BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="True"></asp:textbox></TD>
												<TD><asp:label id="Label4" runat="server" Width="16px" Font-Size="XX-Small" Font-Names="Verdana"
														Font-Bold="True" ForeColor="DimGray">User</asp:label></TD>
												<TD><asp:textbox id="txtUser" runat="server" Width="129px" Font-Size="XX-Small" Font-Names="Verdana"
														BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="True"></asp:textbox></TD>
											</TR>
										</TABLE>
										<!-- **********************************************************************--><asp:textbox id="txtTempID" runat="server" Width="0px" Height="0px" Font-Size="XX-Small" Font-Names="Verdana"
											BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"></asp:textbox><asp:textbox id="txtFormNo" runat="server" Width="0px" Height="0px" Font-Size="XX-Small" Font-Names="Verdana"
											BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"></asp:textbox><asp:textbox id="txtRPA" runat="server" Width="0px" Height="0px" Font-Size="XX-Small" Font-Names="Verdana"
											BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus" MaxLength="50"></asp:textbox></TD>
								</TR>
								<tr>
									<td><cc1:collapsiblepanel id="pnl1" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
											Visible="true" Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
											Text="Request Information" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black"
											PanelCSS="panel" TitleCSS="test">
											<TABLE id="Table11" width="50%" border="0">
												<TR>
													<TD>
														<asp:label id="lblReqBy" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Requested By</asp:label></TD>
													<TD>
														<asp:textbox id="txtReqBy" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></TD>
												</TR>
												<TR>
													<TD style="HEIGHT: 27px">
														<asp:label id="lblReqDate" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Requested Date</asp:label></TD>
													<TD style="HEIGHT: 27px">
                                                        <ION:Customcalendar ID="dtReqDate" runat="server" />
                                                    </TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblPro" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">SubCategory/System</asp:label></TD>
													<TD>
														<asp:textbox id="txtPro" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px" Font-Size="XX-Small"
															Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="50"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblPriority" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Priority</asp:label></TD>
													<TD>
														<asp:textbox id="txtPriority" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="50"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblAuthBy" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Authorized By</asp:label></TD>
													<TD>
														<asp:textbox id="txtAuthBy" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF1" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field1</asp:label></TD>
													<TD>
														<asp:textbox id="txtRIF1" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF2" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field2</asp:label></TD>
													<TD>
														<asp:textbox id="txtRIF2" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF3" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field3</asp:label></TD>
													<TD>
														<asp:textbox id="txtRIF3" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF4" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field4</asp:label></TD>
													<TD>
														<asp:textbox id="txtRIF4" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF5" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field5</asp:label></TD>
													<TD>
														<asp:textbox id="txtRIF5" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF6" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field6</asp:label></TD>
													<TD>
														<asp:textbox id="txtRIF6" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="129px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="200"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF9" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field9</asp:label></TD>
													<TD>
                                                        <ION:Customcalendar ID="dtRIF9" runat="server" />
                                                    </TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF10" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field10</asp:label></TD>
													<TD>
                                                        <ION:Customcalendar ID="dtRIF10" runat="server" />
                                                    </TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF7" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field7</asp:label></TD>
													<TD>
														<asp:textbox id="txtRIF7" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="30" Width="150px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="2000" TextMode="MultiLine"></asp:textbox></TD>
												</TR>
												<TR>
													<TD>
														<asp:label id="lblRIF8" runat="server" Width="136px" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" ForeColor="DimGray">Field8</asp:label></TD>
													<TD>
														<asp:textbox id="txtRIF8" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="30" Width="150px"
															Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="2000" TextMode="MultiLine"></asp:textbox></TD>
												</TR>
											</TABLE>
										</cc1:collapsiblepanel></td>
								</tr>
								<tr>
									<td><cc1:collapsiblepanel id="pnl2" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
											Visible="true" Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
											Text="SubCategory Details" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black"
											PanelCSS="panel" TitleCSS="test">
											<asp:datagrid id="grdTab2" runat="server" GridLines="Horizontal" OnItemDataBound="myBound" DataKeyField="TB_IN4_Tab4_ID"
												AutoGenerateColumns="False" BackColor="White">
												<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
												<HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" CssClass="GridHeader"></HeaderStyle>
												<Columns>
													<asp:TemplateColumn HeaderText="Obj.">
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemTemplate>
															<asp:Image ID="imgObj" ImageUrl="../icons/flag1.gif" Runat="server" Visible="False"></asp:Image>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF1" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field1") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF2" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field2") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF3" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field3") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="PDD1" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF4" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field4") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF5" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field5") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="90px"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF6" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field6") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF7" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field7") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF8" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field8") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF9" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field9") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF13" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field13") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF14" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field14") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="PDF15" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field15") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="PDD2" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date2") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="PDD3" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date3") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
												<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
											</asp:datagrid>
											<asp:linkbutton id="AddRow" runat="server" Font-Size="X-Small" Font-Names="Verdana">
												<font color="DimGray">Add Row</font></asp:linkbutton>
										</cc1:collapsiblepanel></td>
								</tr>
								<tr>
									<td><cc1:collapsiblepanel id="pnl3" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
											Visible="true" Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
											Text="Technical Worksheet" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black"
											PanelCSS="panel" TitleCSS="test">
											<asp:datagrid id="grdTab3" runat="server" GridLines="Horizontal" AutoGenerateColumns="False" BackColor="White">
												<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
												<HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" CssClass="GridHeader"></HeaderStyle>
												<Columns>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF1" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field1") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF2" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field2") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF3" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field3") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF4" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field4") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF5" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field5") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="90px"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF6" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field6") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF7" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field7") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF8" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field8") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF13" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field13") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="WF14" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field14") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="WD1" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="WD2" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date2") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="WD3" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date3") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
												<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
											</asp:datagrid>
											<asp:linkbutton id="Linkbutton1" runat="server" Font-Size="X-Small" Font-Names="Verdana">
												<font color="DimGray">Add Row</font></asp:linkbutton>
										</cc1:collapsiblepanel></td>
								</tr>
								<tr>
									<td>
										<cc1:collapsiblepanel id="pnl4" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
											Visible="true" Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
											Text="Tab 4" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
											TitleCSS="test">
											<asp:datagrid id="grdTab4" runat="server" GridLines="Horizontal" AutoGenerateColumns="False" BackColor="White">
												<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
												<HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" CssClass="GridHeader"></HeaderStyle>
												<Columns>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F1" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field1") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F2" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field2") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F3" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field3") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F4" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field4") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F5" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field5") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="90px"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F6" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field6") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F7" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field7") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F8" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field8") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F9" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field9") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F10" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field10") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F11" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field11") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F12" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field12") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F13" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field13") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F14" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field14") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T4F15" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field15") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="T4D1" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="T4D2" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date2") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="T4D3" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date3") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
												<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
											</asp:datagrid>
											<asp:linkbutton id="Linkbutton2" runat="server" Font-Size="X-Small" Font-Names="Verdana">
												<font color="DimGray">Add Row</font></asp:linkbutton>
										</cc1:collapsiblepanel>
									</td>
								</tr>
								<tr>
									<td>
										<cc1:collapsiblepanel id="pnl5" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo"
											Visible="true" Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
											Text="Tab 5" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
											TitleCSS="test">
											<asp:datagrid id="grdTab5" runat="server" GridLines="Horizontal" AutoGenerateColumns="False" BackColor="White">
												<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
												<HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" CssClass="GridHeader"></HeaderStyle>
												<Columns>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F1" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field1") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F2" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field2") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F3" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field3") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F4" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field4") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F5" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field5") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F6" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field6") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F7" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field7") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F8" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field8") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F9" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field9") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F10" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field10") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F11" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field11") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F12" Width="100px" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field12") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F13" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field13") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F14" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field14") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<asp:TextBox MaxLength=200 CssClass=txtNoFocus Visible=False ID="T5F15" TextMode=MultiLine Width="150px" Height=30 Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field15") %>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="T5D1" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="T5D2" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date2") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<HeaderStyle Font-Bold="True"></HeaderStyle>
														<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
														<ItemTemplate>
															<SCONTROLS:DATESELECTOR Visible=False id="T5D3" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date3") %>'>
															</SCONTROLS:DATESELECTOR>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
												<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
											</asp:datagrid>
											<asp:linkbutton id="Linkbutton3" runat="server" Font-Size="X-Small" Font-Names="Verdana">
												<font color="DimGray">Add Row</font></asp:linkbutton>
										</cc1:collapsiblepanel>
									</td>
								</tr>
							</TABLE>
						</DIV>
					</TD>
				</TR>
			</TABLE>
			
			<!-- ***********************************************************************--> 
			
			<asp:textbox id="txthiddenImage" runat="server" Width="0px"></asp:textbox>
		</FORM>
	</body>
</html>
