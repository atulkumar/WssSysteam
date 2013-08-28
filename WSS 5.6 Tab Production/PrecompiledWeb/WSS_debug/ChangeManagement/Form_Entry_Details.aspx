<%@ page language="VB" autoeventwireup="false" inherits="ChangeManagement_Form_Entry_Details, App_Web_jikqy_wr" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript" src="../SupportCenter/calendar/popcalendar.js"></script>

    <script language="javascript" type="text/javascript" src="../images/Js/JSValidation.js"></script>

    <script language="javascript" type="text/javascript" src="../DateControl/ION.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative; ;TOP:expression(this.offsetParent.scrollTop);BACKGROUND-COLOR:#e0e0e0}</style>

    <script language="Javascript" type="text/javascript">
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
							
			function SaveEdit(varimgValue)
				{
			    	if (varimgValue=='Edit')
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
											
					if (varimgValue=='Close')
						{
							document.Form1.txthiddenImage.value=varimgValue;
							Form1.submit(); 
							return false;
						}								
							
					if (varimgValue=='Add')
						{
							document.Form1.txthiddenImage.value="";
							Form1.submit(); 
							//return false;
						}	
					
					if (varimgValue=='Logout')
						{
							document.Form1.txthiddenImage.value=varimgValue;
							Form1.submit(); 
						}
													
					if (varimgValue=='Ok')
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

								document.Form1.txthiddenImage.value=varimgValue;
								Form1.submit();
							//	self.opener.Form1.submit();
								return false; 			
						}
							
					if (varimgValue=='Save')
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

							document.Form1.txthiddenImage.value=varimgValue;
							Form1.submit(); 
							//self.opener.Form1.submit(); 
							return false;
						}		
						
					if (varimgValue=='Attach')
						{
							document.Form1.txthiddenImage.value=varimgValue;
							Form1.submit(); 
							return false;
						}	
					
					if (varimgValue=='Fwd')
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
						
				
							
					if (varimgValue=='Reset')
						{
							var confirmed
							confirmed=window.confirm("Do You Want To reset The Page ?");
							if(confirmed==true)
								{	
									Form1.reset()
								}		
						}			
					}			
					
				function ConfirmDelete(varimgValue,varMessage)
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
										
											    document.Form1.txthiddenImage.value=varimgValue;
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
				
			function FP_swapimg() 
				{//v1.0
						var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
						n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
						elm.$src=elm.src; elm.src=args[n+1]; } }
				}

			function FP_preloadimgs() 
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

    <script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.HideModalDiv)
                opener.parent.HideModalDiv();
                
            }
        }
        //Modified By Atul to execute script on Page Load
        function OnLoad() {
           if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.LoadModalDiv)
                opener.parent.LoadModalDiv();
            }
        }
        window.onload = OnLoad;
        window.onunload = OnClose;
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;" onunload="RefreshParent();">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" style="border-color: ActiveBorder" cellspacing="0" cellpadding="0"
        width="100%" background="../Images/top_nav_back.gif" border="0">
        <tr>
            <td width="365">
                &nbsp;
                <asp:Label ID="lblTitleLabelfrmEntry" runat="server" Width="144px" Height="12px"
                    BorderWidth="2px" BorderStyle="None" CssClass="TitleLabel">Form Entry Details</asp:Label>
            </td>
            <td>
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                    ToolTip="Save"></asp:ImageButton>&nbsp;&nbsp;
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../Images/s1ok02.gif"
                    ToolTip="Ok"></asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton ID="imgReset" AccessKey="R"
                        runat="server" ImageUrl="../Images/reset_20.gif" ToolTip="Reset"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../Images/s2close01.gif"
                    ToolTip="Close"></asp:ImageButton>&nbsp;
            </td>
            <td width="42" background="../Images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table126" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        border="1">
        <tr>
            <td valign="top" colspan="1">
                <!--  **********************************************************************-->
                <div style="overflow: auto; width: 930px; height: 540px">
                    <table id="Table7" cellspacing="0" cellpadding="2" width="100%" border="0">
                        <tr>
                            <td colspan="2">
                                <cc1:CollapsiblePanel ID="cpnlError" runat="server" Width="100%" BorderStyle="Solid"
                                    BorderWidth="0px" BorderColor="Indigo" Visible="False" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                    ExpandImage="../images/ToggleDown.gif" Text="Message" TitleBackColor="transparent"
                                    TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                    <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" width="100%"
                                        border="0">
                                        <tr>
                                            <td colspan="0" rowspan="0">
                                                <asp:Image ID="imgError" runat="server" Height="16px" Width="16px" ImageUrl="../icons/warning.gif">
                                                </asp:Image>
                                            </td>
                                            <td colspan="0" rowspan="0">
                                                <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="635px"
                                                    ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </cc1:CollapsiblePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="1">
                                <!-- **********************************************************************-->
                                <table id="Table3" cellspacing="0" cellpadding="0" width="40%" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Width="72px" Font-Size="XX-Small" Font-Names="Verdana"
                                                Font-Bold="true" ForeColor="Dimgray">Form Name</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFormName" runat="server" Width="129px" Font-Size="XX-Small" Font-Names="Verdana"
                                                BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Width="48px" Font-Size="XX-Small" Font-Names="Verdana"
                                                Font-Bold="true" ForeColor="Dimgray">Company</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCompany" runat="server" Width="129px" Font-Size="XX-Small" Font-Names="Verdana"
                                                BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Width="16px" Font-Size="XX-Small" Font-Names="Verdana"
                                                Font-Bold="true" ForeColor="Dimgray">User</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUser" runat="server" Width="129px" Font-Size="XX-Small" Font-Names="Verdana"
                                                BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <!-- **********************************************************************-->
                                <asp:TextBox ID="txtCallNo" runat="server" Width="0px" Height="0px" Font-Size="XX-Small"
                                    Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"></asp:TextBox><asp:TextBox
                                        ID="txtTaskNo" runat="server" Width="0px" Height="0px" Font-Size="XX-Small" Font-Names="Verdana"
                                        BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"></asp:TextBox><asp:TextBox
                                            ID="txtFormNo" runat="server" Width="0px" Height="0px" Font-Size="XX-Small" Font-Names="Verdana"
                                            BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"></asp:TextBox><asp:TextBox
                                                ID="txtrPA" runat="server" Width="0px" Height="0px" Font-Size="XX-Small" Font-Names="Verdana"
                                                BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:CollapsiblePanel ID="pnl1" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
                                    BorderColor="Indigo" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                    ExpandImage="../images/ToggleDown.gif" Text="Request Information" TitleBackColor="transparent"
                                    TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                    <table id="Table11" width="30%" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblReqBy" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Requested By</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtreqBy" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 32px">
                                                <asp:Label ID="lblReqDate" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Requested Date</asp:Label>
                                            </td>
                                            <td style="height: 32px">
                                                <ION:Customcalendar ID="dtreqDate" runat="server" Width="120px" Height="20px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPro" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">SubCategory/System</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPro" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPriority" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Priority</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPriority" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAuthBy" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Authorized By</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAuthBy" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF1" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field1</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrIF1" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF2" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field2</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrIF2" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF3" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field3</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrIF3" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF4" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field4</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrIF4" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF5" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field5</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrIF5" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF6" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field6</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrIF6" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Width="129px" Font-Names="Verdana" Font-Size="XX-Small" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF9" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field9</asp:Label>
                                            </td>
                                            <td>
                                                <ION:Customcalendar ID="dtrIF9" runat="server" Width="120px" Height="20px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF10" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field10</asp:Label>
                                            </td>
                                            <td>
                                                <ION:Customcalendar ID="dtrIF10" runat="server" Width="120px" Height="20px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF7" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field7</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrIF7" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Height="30" Width="150px" Font-Names="Verdana" Font-Size="XX-Small"
                                                    MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRIF8" runat="server" Width="136px" ForeColor="Dimgray" Font-Names="Verdana"
                                                    Font-Size="XX-Small" Font-Bold="true">Field8</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrIF8" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                                    BorderWidth="1px" Height="30" Width="150px" Font-Names="Verdana" Font-Size="XX-Small"
                                                    MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </cc1:CollapsiblePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:CollapsiblePanel ID="pnl2" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
                                    BorderColor="Indigo" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                    ExpandImage="../images/ToggleDown.gif" Text="SubCategory Details" TitleBackColor="transparent"
                                    TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                    <asp:DataGrid ID="grdTab2" runat="server" GridLines="Horizontal" OnItemDataBound="myBound"
                                        DataKeyField="TB_IN4_Tab4_ID" AutoGenerateColumns="False" BackColor="White">
                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                        <HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="true" CssClass="GridHeader">
                                        </HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Obj.">
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:Image ID="imgObj" ImageUrl="../icons/flag1.gif" runat="server" Visible="False">
                                                    </asp:Image>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF1" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field1") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF2" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field2") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF3" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field3") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <ION:Customcalendar ID="PDD1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'
                                                        Width="120px" Height="20px" />
                                                    <%--<SCONtrOLS:DATESELECTOR Visible=False id="PDD1" runat="server" Text="Start Date:" CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'>
															</SCONtrOLS:DATESELECTOR>--%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF4" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field4") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF5" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field5") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="90px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF6" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field6") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF7" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field7") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF8" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field8") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF9" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field9") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF13" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field13") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF14" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field14") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="PDF15" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field15") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="PDD2" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date2") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="PDD3" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date3") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                    <asp:LinkButton ID="AddRow" runat="server" Font-Names="Verdana" Font-Size="X-Small">
												<font color="Dimgray">Add Row</font></asp:LinkButton>
                                </cc1:CollapsiblePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:CollapsiblePanel ID="pnl3" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
                                    BorderColor="Indigo" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                    ExpandImage="../images/ToggleDown.gif" Text="Technical Worksheet" TitleBackColor="transparent"
                                    TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                    <asp:DataGrid ID="grdTab3" runat="server" GridLines="Horizontal" AutoGenerateColumns="False"
                                        BackColor="White">
                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                        <HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="true" CssClass="GridHeader">
                                        </HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF1" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field1") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF2" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field2") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF3" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field3") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF4" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field4") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF5" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field5") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="90px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF6" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field6") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF7" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field7") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF8" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field8") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF13" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field13") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="WF14" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field14") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="WD1" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="WD2" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date2") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="WD3" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date3") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                    <asp:LinkButton ID="linkbutton1" runat="server" Font-Names="Verdana" Font-Size="X-Small">
												<font color="Dimgray">Add Row</font></asp:LinkButton>
                                </cc1:CollapsiblePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:CollapsiblePanel ID="pnl4" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
                                    BorderColor="Indigo" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                    ExpandImage="../images/ToggleDown.gif" Text="Tab 4" TitleBackColor="transparent"
                                    TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                    <asp:DataGrid ID="grdTab4" runat="server" GridLines="Horizontal" AutoGenerateColumns="False"
                                        BackColor="White">
                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                        <HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="true" CssClass="GridHeader">
                                        </HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F1" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field1") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F2" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field2") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F3" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field3") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F4" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field4") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F5" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field5") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="90px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F6" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field6") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F7" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field7") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F8" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field8") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F9" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field9") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F10" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field10") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F11" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field11") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F12" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field12") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F13" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field13") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F14" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field14") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T4F15" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field15") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="T4D1" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="T4D2" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date2") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="T4D3" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date3") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                    <asp:LinkButton ID="linkbutton2" runat="server" Font-Names="Verdana" Font-Size="X-Small">
												<font color="Dimgray">Add Row</font></asp:LinkButton>
                                </cc1:CollapsiblePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:CollapsiblePanel ID="pnl5" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px"
                                    BorderColor="Indigo" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                    ExpandImage="../images/ToggleDown.gif" Text="Tab 5" TitleBackColor="transparent"
                                    TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                    <asp:DataGrid ID="grdTab5" runat="server" GridLines="Horizontal" AutoGenerateColumns="False"
                                        BackColor="White">
                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                        <HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="true" CssClass="GridHeader">
                                        </HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F1" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field1") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F2" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field2") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F3" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field3") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F4" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field4") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F5" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field5") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F6" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field6") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F7" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field7") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F8" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field8") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F9" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field9") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F10" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field10") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F11" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field11") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F12" Width="100px"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC200_Field12") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F13" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field13") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F14" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field14") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox MaxLength="200" CssClass="txtNoFocus" Visible="False" ID="T5F15" TextMode="MultiLine"
                                                        Width="150px" Height="30" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tb_VC2000_Field15") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="T5D1" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date1") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="T5D2" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date2") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                                <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                <ItemTemplate>
                                                    <SControls:DateSelector Visible="False" ID="T5D3" runat="server" Text="Start Date:"
                                                        CalendarDate='<%# DataBinder.Eval(Container.DataItem, "Tb_DT8_Date3") %>'></SControls:DateSelector>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                    <asp:LinkButton ID="linkbutton3" runat="server" Font-Names="Verdana" Font-Size="X-Small">
												<font color="Dimgray">Add Row</font></asp:LinkButton>
                                </cc1:CollapsiblePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <!-- ***********************************************************************-->
    <asp:TextBox ID="txthiddenImage" runat="server" Width="0px"></asp:TextBox>
    </form>
</body>
</html>
