<%@ page language="VB" autoeventwireup="false" inherits="ChangeManagement_Form_Entry_Head, App_Web_8ch_aegk" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Head</title>
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative; ;TOP:expression(this.offsetParent.scrollTop);BACKGROUND-COLOR:#e0e0e0}</style>

    <script src="../images/js/ION.js" type="text/javascript"></script>

    <script type="text/javascript" src="../SupportCenter/calendar/popcalendar.js"></script>

    <script type="text/javascript" src="../images/Js/JSValidation.js"></script>

    <script type="text/javascript">

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
					wopen('Task_edit.aspx?ScrID=334&TASKNO='+varTable,'Search'+rand_no,430,300);
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
				
					//document.Form1.txthiddenImage.value='Edit';
					//document.Form1.txthiddenSkil.value=aa;
					//document.Form1.txthidden.value=bb;	
					//document.Form1.txthiddenGrid.value=dd;	
					//Form1.submit(); 
					wopen('role_edit.aspx?ScrID=354&codeID='+aa,'Search'+rand_no,430,300);
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
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit();
							return false;  
						}	
					if (varImgValue=='Delete')
						{
										var confirmed;
										confirmed=window.confirm("Delete,Are you sure you want to Delete the selected record ?");
										if(confirmed==false)
										{
												return false;
										}
										else
										{
											    document.Form1.txthiddenImage.value=varImgValue;
												Form1.submit(); 
												return false;
										}
						}	
					if (varImgValue=='Logout')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit();
							return false;  
						}
							
						if (varImgValue=='Delete')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit();
							return false;  
						}
													
					if (varImgValue=='Ok')
						{
						
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
    </script>

    <script type="text/javascript">

	                    function onEnd() 
				        {
                            var x = document.getElementById('cpnlCallTask_collapsible').cells[0].colSpan = "1";
                            var y = document.getElementById('Collapsiblepanel5_collapsible').cells[0].colSpan = "1";
                            var a = document.getElementById('Collapsiblepanel1_collapsible').cells[0].colSpan = "1";
                            var b = document.getElementById('Collapsiblepanel4_collapsible').cells[0].colSpan = "1";
                            var c = document.getElementById('Collapsiblepanel3_collapsible').cells[0].colSpan = "1";
                            
                        }
    
 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tbody>
            <tr>
                <td valign="top">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr width="100%">
                                            <td background="../Images/top_nav_back.gif" height="47">
                                                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                                    <tr>
                                                        <td style="width: 15%">
                                                            <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" BorderWidth="0px" Width="0px"
                                                                BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                            <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                                ImageUrl="white.GIF" AlternateText="." CommandName="submit"></asp:ImageButton>
                                                            <asp:Label ID="lblTitleLabelFrmEntryHead" runat="server" Height="12px" BorderWidth="2px"
                                                                BorderStyle="None" CssClass="TitleLabel">Form Detail</asp:Label>
                                                        </td>
                                                        <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                            <center>
                                                                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                                                                    Visible="False" ToolTip="Save"></asp:ImageButton>&nbsp;
                                                                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../Images/s1ok02.gif"
                                                                    ToolTip="Ok"></asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton ID="imgReset" AccessKey="R"
                                                                        runat="server" ImageUrl="../Images/reset_20.gif" ToolTip="Reset"></asp:ImageButton>&nbsp;<asp:ImageButton
                                                                            ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../Images/s2delete01.gif"
                                                                            ToolTip="Delete"></asp:ImageButton>&nbsp;
                                                                <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td  nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('259','../');"
                                                    alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                        class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                        src="../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                        border="0">
                                        <tbody>
                                            <tr>
                                                <td valign="top" colspan="1">
                                                    <!--  **********************************************************************-->
                                                    <div style="overflow: auto; width: 100%; height: 581px">
                                                        <table width="100%" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td valign="top" colspan="1">
                                                                        <!-- **********************************************************************-->
                                                                        <!-- **********************************************************************-->
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Width="100%" BorderWidth="0px"
                                                                            BorderStyle="Solid" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                            TitleClickable="True" TitleBackColor="Transparent" Text="Request Information"
                                                                            ExpandImage="../images/ToggleDown.gif" CollapseImage="../images/ToggleUp.gif"
                                                                            Draggable="False" BorderColor="Indigo">
                                                                            <div style="overflow: auto; width: 100%; height: 250px">
                                                                                <table id="Table11" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td colspan="1">
                                                                                            <asp:Label ID="lblName4" runat="server" Width="85px" Height="12px" CssClass="FieldLabel">Form Name</asp:Label>
                                                                                        </td>
                                                                                        <td colspan="5">
                                                                                            <asp:TextBox ID="txtFormName" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="100"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label1" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Request Information</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRI" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                        <td colspan="4">
                                                                                            <asp:RadioButtonList ID="rblRI" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label17" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Field Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label18" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Bold="True">Alias Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label21" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                                                                CssClass="FieldLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label20" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Mark</asp:Label>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label19" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Type</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblReqBy" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Requested By</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtReqBy" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtReqBySeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblReqBy" runat="server" Width="32px" CssClass="FieldLabel"
                                                                                                RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label22" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblReqDate" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Requested Date</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtReqDate" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtReqDateSeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblReqDate" runat="server" Width="32px" CssClass="FieldLabel"
                                                                                                RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label23" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Size="XX-Small">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPro" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">SubCategory/System</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPro" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtProSeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPro" runat="server" Width="32px" CssClass="FieldLabel"
                                                                                                RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label24" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td height="19">
                                                                                            <asp:Label ID="lblPriority" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Priority</asp:Label>
                                                                                        </td>
                                                                                        <td height="19">
                                                                                            <asp:TextBox ID="txtPriority" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPrioritySeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="19">
                                                                                            <asp:RadioButtonList ID="rblPriority" runat="server" Width="32px" CssClass="FieldLabel"
                                                                                                RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10" height="19">
                                                                                            <asp:Label ID="Label25" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblAuthBy" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Authorized By</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtAuthBy" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtAuthBySeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblAuthby" runat="server" Width="32px" CssClass="FieldLabel"
                                                                                                RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label26" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF1" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">Field1</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF1" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF1" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF1" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label27" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF2" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field2</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF2" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF2" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF2" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label28" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF3" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field3</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF3" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF3" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF3" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label29" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF4" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field4</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF4" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF4" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF4" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label30" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF5" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field5</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF5" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF5" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF5" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label31" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF6" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field6</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF6" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF6" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF6" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label32" runat="server" Width="136px" Height="12px" CssClass="FieldLabel"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF7" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field7</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF7" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF7" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF7" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label33" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF8" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field8</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF8" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF8" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF8" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label34" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF9" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field9</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF9" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF9" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF9" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label35" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRIF10" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field10</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRIF10" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtRISeqF10" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblRIF10" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label36" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </cc1:CollapsiblePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <cc1:CollapsiblePanel ID="Collapsiblepanel5" runat="server" Width="100%" BorderWidth="0px"
                                                                            BorderStyle="Solid" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                            TitleClickable="True" TitleBackColor="Transparent" Text="SubCategory Details"
                                                                            ExpandImage="../images/ToggleDown.gif" CollapseImage="../images/ToggleUp.gif"
                                                                            Draggable="False" BorderColor="Indigo">
                                                                            <div style="overflow: auto; width: 100%; height: 250px">
                                                                                <table id="Table10" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label72" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">SubCategory Details</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPD" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                        <td colspan="4">
                                                                                            <asp:RadioButtonList ID="rblPD" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label107" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label106" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Alias Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label105" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                                                                ForeColor="DimGray" Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label104" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Mark</asp:Label>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label103" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Type</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPName" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">SubCategory Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPname" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPnameSeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPname" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label101" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPDesc" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Description</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPDesc" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPdescSeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPdesc" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label99" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPAppBy" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Approved By</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPAppBy" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPAppBySeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPAppBy" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label97" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td height="19">
                                                                                            <asp:Label ID="lblPReqDate" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Requested Date</asp:Label>
                                                                                        </td>
                                                                                        <td height="19">
                                                                                            <asp:TextBox ID="txtPReqDate" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="19">
                                                                                            <asp:TextBox ID="txtPReqDateSeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="19">
                                                                                            <asp:RadioButtonList ID="rblPReqDate" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10" height="19">
                                                                                            <asp:Label ID="Label95" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPSplIns" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Special Inst.</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSpIns" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSpInsSeq" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPSpIns" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label93" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF1" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field1</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF1" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF1" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF1" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label91" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF2" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field2</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF2" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF2" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF2" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label89" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF3" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field3</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF3" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF3" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF3" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label87" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF4" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field4</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF4" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF4" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF4" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label85" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF5" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field5</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF5" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF5" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF5" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label83" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF6" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field6</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF6" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF6" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF6" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label81" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF7" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field7</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF7" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF7" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF7" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label79" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF8" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field8</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF8" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF8" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF8" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label77" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Multiline</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF9" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field9</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF9" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF9" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF9" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label75" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPF10" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field10</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPF10" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPSeqF10" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblPF10" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label73" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </cc1:CollapsiblePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <cc1:CollapsiblePanel ID="Collapsiblepanel1" runat="server" Width="100%" BorderWidth="0px"
                                                                            BorderStyle="Solid" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                            TitleClickable="True" TitleBackColor="Transparent" Text="Technical Worksheet"
                                                                            ExpandImage="../images/ToggleDown.gif" CollapseImage="../images/ToggleUp.gif"
                                                                            Draggable="False" BorderColor="Indigo">
                                                                            <div style="overflow: auto; width: 100%; height: 250px">
                                                                                <table id="Table3" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label108" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Worksheet Details</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWD" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                        <td colspan="5">
                                                                                            <asp:RadioButtonList ID="rblWD" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label71" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label70" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Alias Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label69" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                                                                ForeColor="DimGray" Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label68" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Mark</asp:Label>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label67" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Type</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF1" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field1</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF1" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF1" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF1" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label55" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF2" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field2</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF2" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF2" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF2" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label53" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF3" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field3</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF3" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF3" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF3" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label51" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF4" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field4</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF4" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF4" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF4" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label49" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF5" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field5</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF5" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF5" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF5" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label47" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF6" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field6</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF6" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF6" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF6" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label60" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF7" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field7</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF7" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF7" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF7" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label61" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF8" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field8</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF8" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF8" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF8" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label62" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF9" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field9</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF9" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF9" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF9" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label41" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF10" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field10</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF10" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF10" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF10" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label43" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF11" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field11</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF11" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF11" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF11" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label45" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td height="22">
                                                                                            <asp:Label ID="lblWF12" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field12</asp:Label>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:TextBox ID="txtWF12" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:TextBox ID="txtWSEqF12" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:RadioButtonList ID="rblWF12" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10" height="22">
                                                                                            <asp:Label ID="Label39" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblWF13" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field13</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWF13" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtWSEqF13" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblWF13" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label37" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </cc1:CollapsiblePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <cc1:CollapsiblePanel ID="Collapsiblepanel4" runat="server" Width="100%" BorderWidth="0px"
                                                                            BorderStyle="Solid" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                            TitleClickable="True" TitleBackColor="Transparent" Text="Tab 4" ExpandImage="../images/ToggleDown.gif"
                                                                            CollapseImage="../images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                                            <div style="overflow: auto; width: 100%; height: 250px">
                                                                                <table id="Table7" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label136" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Tab 4</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                        <td colspan="5">
                                                                                            <asp:RadioButtonList ID="rblT4" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label135" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label134" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Alias Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label133" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                                                                ForeColor="DimGray" Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label132" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Mark</asp:Label>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label131" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Type</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F1" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field1</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F1" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF1" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F1" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label129" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F2" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field2</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F2" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF2" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F2" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label127" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F3" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field3</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F3" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF3" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F3" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label125" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F4" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field4</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F4" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF4" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F4" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label123" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F5" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field5</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F5" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF5" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F5" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label121" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F6" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field6</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F6" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF6" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F6" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label119" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F7" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field7</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F7" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF7" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F7" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label117" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F8" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field8</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F8" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF8" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F8" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label115" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F9" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field9</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F9" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF9" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F9" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label138" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F10" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field10</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F10" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF10" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F10" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label139" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F11" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field11</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F11" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF11" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F11" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label143" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F12" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field12</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F12" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF12" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F12" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label144" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F13" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field13</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F13" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF13" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F13" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label137" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F14" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field14</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F14" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF14" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F14" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label113" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F15" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field15</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F15" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF15" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F15" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label111" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F16" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field16</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F16" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF16" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F16" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label109" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td height="22">
                                                                                            <asp:Label ID="lblT4F17" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field17</asp:Label>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:TextBox ID="txtT4F17" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:TextBox ID="txtT4SeqF17" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:RadioButtonList ID="rblT4F17" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10" height="22">
                                                                                            <asp:Label ID="Label65" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT4F18" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field18</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4F18" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT4SeqF18" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT4F18" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label63" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </cc1:CollapsiblePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <cc1:CollapsiblePanel ID="Collapsiblepanel3" runat="server" Width="100%" BorderWidth="0px"
                                                                            BorderStyle="Solid" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                            TitleClickable="True" TitleBackColor="Transparent" Text="Tab 5" ExpandImage="../images/ToggleDown.gif"
                                                                            CollapseImage="../images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                                            <div style="overflow: auto; width: 100%; height: 250px">
                                                                                <table id="Table8" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label188" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Tab 5</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                        <td colspan="5">
                                                                                            <asp:RadioButtonList ID="rblT5" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label187" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label186" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Alias Name</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label185" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                                                                ForeColor="DimGray" Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label184" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Mark</asp:Label>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label183" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Type</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F1" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field1</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F1" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF1" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F1" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label181" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F2" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field2</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F2" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF2" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F2" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label179" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F3" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field3</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F3" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF3" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F3" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label177" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F4" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field4</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F4" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF4" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F4" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label175" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F5" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field5</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F5" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF5" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F5" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label173" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F6" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field6</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F6" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF6" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F6" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label171" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F7" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field7</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F7" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF7" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F7" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label169" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F8" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field8</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F8" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF8" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F8" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label167" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F9" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field9</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F9" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF9" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F9" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label165" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F10" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field10</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F10" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF10" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F10" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label163" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F11" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field11</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F11" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF11" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F11" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label161" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F12" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field12</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F12" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF12" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F12" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label159" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">TextBox</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F13" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field13</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F13" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF13" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F13" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label157" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F14" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field14</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F14" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF14" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F14" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label155" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F15" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field15</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F15" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF15" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F15" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label153" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">MultiLine</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F16" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field16</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F16" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF16" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F16" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label151" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td height="22">
                                                                                            <asp:Label ID="lblT5F17" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field17</asp:Label>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:TextBox ID="txtT5F17" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:TextBox ID="txtT5SeqF17" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td height="22">
                                                                                            <asp:RadioButtonList ID="rblT5F17" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10" height="22">
                                                                                            <asp:Label ID="Label149" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblT5F18" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">Field18</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5F18" runat="server" Width="129px" Height="18px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtT5SeqF18" runat="server" Width="0px" Height="0px" CssClass="txtNoFocus"
                                                                                                BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                MaxLength="20"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rblT5F18" runat="server" Width="32px" Font-Names="Verdana"
                                                                                                Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td colspan="10">
                                                                                            <asp:Label ID="Label147" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                                                                                Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True">DateTime</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </cc1:CollapsiblePanel>
                                                                        <asp:TextBox ID="txthiddenImage" runat="server" Width="0px" Height="0px" BorderWidth="0px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:UpdatePanel ID="PanelUpdate" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMsg" runat="server">
                            </asp:Panel>
                            <asp:ListBox ID="lstError" runat="server" Width="635px" BorderStyle="Groove" BorderWidth="0"
                                ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small" Visible="false"></asp:ListBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
