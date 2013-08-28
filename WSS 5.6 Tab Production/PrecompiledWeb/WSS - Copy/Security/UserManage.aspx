<%@ page language="VB" autoeventwireup="false" inherits="Security_UserManage, App_Web_mewe43ky" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="~/BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Management</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../SupportCenter/calendar/popcalendar.js"></script>

    <script type="text/javascript" src="../images/Js/JSValidation.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>

    <script type="text/javascript" src="../DateControl/ION.js"></script>

    <script type="text/javascript">

	var rand_no = Math.ceil(500*Math.random())

			var globalid;
			var globalSkil;
			var globalAddNo;
			var globalGrid;
			var globaldbclick = 0;
				var rand_no = Math.ceil(500*Math.random())
						
			function OpenW(a,b,c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
				}
				
			function OpenAB(c)
				{
					var Type='COM'
					wopen('../Search/Common/PopSearch.aspx?ID=select a.CI_NU8_Address_Number as ID,a.CI_VC36_Name as UserName,b.CI_VC36_Name as Company from T010011 a,T010011 b Where a.CI_IN4_Business_Relation=b.CI_NU8_Address_Number And a.CI_VC8_Address_Book_Type<>' +"'"+ Type +"'" + '  &tbname=' + c ,'Search'+rand_no,500,450);
					return false;
				}
			function OpenRole(c)
				{
			
					var abId = document.getElementById('cpnlCallView_CDDLUserID_DDL').value;
					var currentTime = new Date();
					var Day = currentTime.getdate();
					var Month = currentTime.getMonth();
					Month = Number(Month) + 1;
					var Year = currentTime.getFullYear();
					var curDate = Month + '/' + Day + '/' + Year;
					//wopen('../Search/Common/PopSearch.aspx?ID=select ROM_IN4_Role_ID_PK as ID,ROM_VC50_Role_Name as RoleName,CI_VC36_Name as CompanyName from T070031,T010011 where CI_NU8_Address_Number=ROM_IN4_Company_ID_FK and ROM_IN4_Role_ID_PK not in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = '+ abId + ' and RA_DT8_Assigned_Date <=' + "'"+curDate+"'" + 'and RA_DT8_Valid_UpTo >=' + "'"+curDate+"'" + 'and RA_VC4_Status_Code =' + "'ENB'" + ') and ROM_IN4_Company_ID_FK = (select CI_IN4_Business_Relation from t010011 where CI_NU8_Address_Number ='+ abId + ') and ROM_DT8_Start_Date<=' + "'"+curDate+"'" + ' and ROM_DT8_End_Date >=' + "'"+curDate+"'" + ' and ROM_VC50_Status_Code_FK = ' + "'ENB'"  + '&tbname=' + c ,'Search',500,500);	
					
					wopen('../Search/Common/PopSearch.aspx?ID=select ROM_IN4_Role_ID_PK as ID,ROM_VC50_Role_Name as RoleName,CI_VC36_Name as CompanyName from T070031,T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_IN4_Role_ID_PK not in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = '+ abId + ' and RA_DT8_Assigned_Date <=' + "'"+curDate+"'" + 'and RA_DT8_Valid_UpTo >=' + "'"+curDate+"'" + 'and RA_VC4_Status_Code =' + "'ENB'" + ') and (ROM_IN4_Company_ID_FK = (select CI_IN4_Business_Relation from t010011 where CI_NU8_Address_Number ='+ abId + ') or ROM_IN4_Company_ID_FK = 0 )and ROM_DT8_Start_Date<=' + "'"+curDate+"'" + ' and ROM_DT8_End_Date >=' + "'"+curDate+"'" + ' and ROM_VC50_Status_Code_FK = ' + "'ENB'"  + '&tbname=' + c ,'Search'+rand_no,500,500);	
				} 
			function OpenW_Add_Address(param)
				{
				
					wopen('AB_Additional.aspx?ID='+param,'Additional_Address'+rand_no,400,450);
				}
			
			function OpenAttach()
				{
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=C','Additional_Address'+rand_no,400,450);
				}
					
			//function test()
				//{
//					alert("Test Ok");
		//		}
			
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
					window.open("Udc_Home_Search.aspx","ss"+rand_no,"scrollBars=no,resizable=No,width=400,height=450,status=yes");
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
				
																				
			function KeyCheck(aa,cc,rowvalues)
				{
						var tableID='cpnlCallTask_dgMenu';  //your datagrids id
						var table;
					
						document.Form1.txtroleId.value=aa;
												
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
							      table.rows [ rowvalues ] . style . backgroundColor = "#d4d4d4";
								
								}
					}
				
				
			function KeyCheck55(aa)
				{		
					wopen('role_edit.aspx?ScrID=354&codeID='+aa,'Search'+rand_no,430,300);
				}
							
			function SaveEdit(varimgValue)
				{
			    	if (varimgValue=='Edit')
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
									wopen('Role_edit.aspx?ScrID=354&','FWD'+rand_no,400,250);
								}										
						}	
											
					if (varimgValue=='Close')
						{
//							document.Form1.txthiddenImage.value=varimgValue;
//							Form1.submit(); 
//							return false;
                            window.close();
						}								
							
					if (varimgValue=='Add')
						{
							document.Form1.txthiddenImage.value=varimgValue;
							Form1.submit(); 
							return false;
						}	
					
					if (varimgValue=='Logout')
						{
							document.Form1.txthiddenImage.value=varimgValue;
							Form1.submit(); 
							return false;
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
							return false;
						}			
					}			
					
				function ConfirmDelete(varimgValue)
					{											
							if (document.Form1.txtroleId.value==0)
								{
									alert("Please select the Assigned Role for delete");
									return false;
								}
								else
								{
									var confirmed
								//	confirmed=window.confirm(varMessage);
							    	confirmed=window.confirm("Are you sure you want to Delete the Assigned Role ?");
									if(confirmed==true)
											{
										    document.Form1.txthiddenImage.value=varimgValue;											
											Form1.submit(); 
											}
											else
											   {
											   return false;
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
							wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+b ,'Attachment'+rand_no,500,450);
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
				
				  //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";
             var y = document.getElementById('cpnlCallTask_collapsible').cells[0].colSpan = "1";
              var z = document.getElementById('cpnlCompany_collapsible').cells[0].colSpan = "1";
           
        }		
				
    </script>
  <script type="text/javascript">
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr valign="top">
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr style="width: 100%">
                                                <td background="../Images/top_nav_back.gif" height="47">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td style="width: 15%">
                                                            &nbsp;&nbsp;
                                                                <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" CommandName="submit"
                                                                    AlternateText="." ImageUrl="white.GIF" Width="0px" Height="0px" BorderWidth="0px">
                                                                </asp:ImageButton>
                                                                <asp:Label ID="lblTitleLabelUserMgmt" runat="server" CssClass="TitleLabel" BorderStyle="None">User Management</asp:Label>
                                                            </td>
                                                            <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                                <center>
                                                                    <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                                                                        ToolTip="Save"></asp:ImageButton>
                                                                    <asp:ImageButton ID="imgOk" AccessKey="K" Visible="false" runat="server" ImageUrl="../Images/s1ok02.gif"
                                                                        ToolTip="Ok"></asp:ImageButton>
                                                                    <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                                                                        ToolTip="Reset"></asp:ImageButton>
                                                                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="../Images/s2delete01.gif"
                                                                        ToolTip="Select a Task to Delete Action"></asp:ImageButton>
                                                                    <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                        style="cursor: hand;" />
                                                                </center>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                                    <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('258','../');"
                                                        alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                                    <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                        src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <!--  **********************************************************************-->
                                        <div style="overflow: auto; width: 100%">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td valign="top" colspan="1">
                                                            <!-- **********************************************************************-->
                                                            <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" Width="100%" BorderStyle="Solid"
                                                                BorderWidth="1px" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                                                ExpandImage="../images/ToggleDown.gif" Text="User" TitleBackColor="transparent"
                                                                TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                                                <table id="Table3" cellspacing="0" cellpadding="0" width="775" border="0">
                                                                    <tr>
                                                                        <td valign="top" align="left" width="100%">
                                                                            <table width="100%" bgcolor="#f5f5f5" border="0">
                                                                                <tr>
                                                                                    <td height="37">
                                                                                        <asp:Label ID="Label3" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Company*</asp:Label><br />
                                                                                        <asp:DropDownList ID="DDLCompany" runat="server" Width="200px" Font-Size="XX-Small"
                                                                                            AutoPostBack="true" Font-Name="Verdana" CssClass="txtNoFocus" Height="20px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td height="37">
                                                                                        <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">Address Book Name</asp:Label><br />
                                                                                        <uc1:CustomDDL ID="CDDLUserID" runat="server" Width="200px"></uc1:CustomDDL>
                                                                                    </td>
                                                                                    <td height="37">
                                                                                        <asp:Label ID="lblMiddleName2" runat="server" CssClass="FieldLabel">User ID</asp:Label><br />
                                                                                        <asp:TextBox ID="txtUserID" runat="server" Height="16px" Width="200px" BorderWidth="1px"
                                                                                            BorderStyle="Solid" CssClass="txtNoFocus" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                            MaxLength="12"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Font-Size="XX-Small"
                                                                                            ControlToValidate="txtUserID" ErrorMessage="Enter user ID">*</asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                    <td height="37">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="38">
                                                                                        <asp:Label ID="lblMiddleName6" runat="server" CssClass="FieldLabel">Password</asp:Label><br />
                                                                                        <asp:TextBox ID="txtPwd" runat="server" Height="16px" Width="200px" BorderWidth="1px"
                                                                                            BorderStyle="Solid" CssClass="txtNoFocus" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                            MaxLength="50" TextMode="Password"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Font-Size="XX-Small"
                                                                                            ControlToValidate="txtPwd" ErrorMessage="Enter Password" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Width="8px"
                                                                                            Font-Size="XX-Small" ControlToValidate="txtPwd" ErrorMessage="Password must be at least 6 characters long "
                                                                                            Display="Dynamic" ValidationExpression="^[\w]{6,}">*</asp:RegularExpressionValidator>
                                                                                    </td>
                                                                                    <td height="38">
                                                                                        <asp:Label ID="lblMiddleName" runat="server" CssClass="FieldLabel">Confirm Password</asp:Label><br />
                                                                                        <asp:TextBox ID="txtConPwd" runat="server" Height="16px" Width="200px" BorderWidth="1px"
                                                                                            BorderStyle="Solid" CssClass="txtNoFocus" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                            MaxLength="50" TextMode="Password"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Font-Names="Verdana"
                                                                                            Font-Size="XX-Small" ControlToValidate="txtConPwd" ErrorMessage="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                                        <asp:CompareValidator ID="CompareValidator1" runat="server" Font-Names="Verdana"
                                                                                            Font-Size="XX-Small" ControlToValidate="txtConPwd" ErrorMessage="Password does not match"
                                                                                            Display="Dynamic" ControlToCompare="txtPwd">*</asp:CompareValidator>
                                                                                    </td>
                                                                                    <td height="38">
                                                                                        <asp:Label ID="lblMiddleName14" runat="server" CssClass="FieldLabel">Status Code</asp:Label><br />
                                                                                        <asp:DropDownList ID="Ddl_Status" runat="server" Width="200px" CssClass="txtNoFocus"
                                                                                            Height="20px">
                                                                                            <asp:ListItem Value="ENB">Enable</asp:ListItem>
                                                                                            <asp:ListItem Value="DNB">Disable</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td height="38">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="38">
                                                                                        <asp:Label ID="lblMiddleName8" runat="server" Width="72px" CssClass="FieldLabel">From Date</asp:Label><br />
                                                                                        <ION:Customcalendar ID="dtStartdate" runat="server" Height="16px" Width="120px" />
                                                                                    </td>
                                                                                    <td height="38">
                                                                                        <asp:Label ID="lblMiddleName10" runat="server" Width="72px" CssClass="FieldLabel">To Date</asp:Label><br />
                                                                                        <ION:Customcalendar ID="dtEndDate" runat="server" Height="16px" Width="120px" />
                                                                                    </td>
                                                                                    <td height="38">
                                                                                        <asp:Label ID="lbl12" runat="server">Profile Expiry Mail</asp:Label><br />
                                                                                        <asp:DropDownList ID="DDLExpiryDays" Width="200px" runat="server" Height="20px">
                                                                                            <asp:ListItem Value="0">No Mail</asp:ListItem>
                                                                                            <asp:ListItem Value="1" Selected="true">1 Day Before Expiry</asp:ListItem>
                                                                                            <asp:ListItem Value="2">2 Day Before Expiry</asp:ListItem>
                                                                                            <asp:ListItem Value="3">3 Day Before Expiry</asp:ListItem>
                                                                                            <asp:ListItem Value="5">5 Day Before Expiry</asp:ListItem>
                                                                                            <asp:ListItem Value="10">10 Day Before Expiry</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td height="38">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkAdminRights" Text="Admin Reports Rights" runat="server" TextAlign="Left">
                                                                                        </asp:CheckBox>
                                                                                        <asp:TextBox ID="txtCreateDate" runat="server" Width="0px" Height="0px" BorderWidth="0px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkMailComm" Text="Mail Communication" runat="server" TextAlign="Left"
                                                                                            Checked="true"></asp:CheckBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <br>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </cc1:CollapsiblePanel>
                                                            <!-- **********************************************************************-->
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Width="100%" BorderStyle="Solid"
                                                                BorderWidth="0px" BorderColor="Indigo" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                                                ExpandImage="../images/ToggleDown.gif" Text="Roles" TitleBackColor="transparent"
                                                                TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="Panel1" Width="0" runat="server">
                                                                                <asp:DataGrid ID="dgMenu" runat="server" CssClass="Grid" AutoGenerateColumns="False"
                                                                                    DataKeyField="RA_IN4_User_Role_ID_PK" Width="720px">
                                                                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                    <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                                    <HeaderStyle Height="18px" CssClass="GridHeader"></HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundColumn Visible="False" DataField="RA_IN4_User_Role_ID_PK"></asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="ROM_VC50_Role_Name" HeaderText="Role Name">
                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="100px"></ItemStyle>
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="RA_DT8_Assigned_Date" HeaderText="Assigned Date">
                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="200px"></ItemStyle>
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="RA_DT8_Valid_UpTo" HeaderText="Valid Upto">
                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="200px"></ItemStyle>
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="RA_VC4_Status_Code" HeaderText="Status">
                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="100px"></ItemStyle>
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="CI_VC36_Name" HeaderText="Assigned By">
                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="106px"></ItemStyle>
                                                                                        </asp:BoundColumn>
                                                                                    </Columns>
                                                                                </asp:DataGrid>
                                                                                <table id="Table7" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td width="105">
                                                                                            <uc1:CustomDDL ID="CDDLRole" runat="server" Width="104px"></uc1:CustomDDL>
                                                                                        </td>
                                                                                        <td width="210">
                                                                                            <asp:TextBox ID="txtAssignDate" Width="198px" BorderWidth="1px" BorderStyle="Solid"
                                                                                                CssClass="txtNoFocusFE" Height="14px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                                runat="server" ReadOnly="true"></asp:TextBox>
                                                                                        </td>
                                                                                        <td width="210">
                                                                                            <ION:Customcalendar ID="dtrLValidUpto" runat="server" Width="195px" Height="16px" />
                                                                                        </td>
                                                                                        <td width="110">
                                                                                            <asp:DropDownList ID="ddlRLStatus" runat="server" Width="103px" CssClass="txtNoFocusFE">
                                                                                                <asp:ListItem Value="ENB">Enable</asp:ListItem>
                                                                                                <asp:ListItem Value="DNB">Disable</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td width="105">
                                                                                            <asp:TextBox ID="txtAssBy" Width="104px" CssClass="txtNoFocusFE" MaxLength="8" runat="server"
                                                                                                ReadOnly="true" Height="14px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </cc1:CollapsiblePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <cc1:CollapsiblePanel ID="cpnlCompany" runat="server" Width="100%" BorderStyle="Solid"
                                                                BorderWidth="0px" BorderColor="Indigo" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                                                ExpandImage="../images/ToggleDown.gif" Text="Company Access" TitleBackColor="transparent"
                                                                TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DataGrid ID="grdCompany" runat="server" CssClass="Grid" AutoGenerateColumns="False">
                                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                                <HeaderStyle Height="18px" CssClass="GridHeader"></HeaderStyle>
                                                                                <Columns>
                                                                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                        HeaderStyle-Width="100px">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblAccess_H" runat="server">Access</asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkAccess" runat="server" Checked='<%#Container.DataItem("Access")%>'>
                                                                                            </asp:CheckBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:BoundColumn HeaderStyle-Width="250px" DataField="CompName" HeaderText="Company Name">
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn HeaderStyle-Width="100px" DataField="CompID" HeaderText="CompID"
                                                                                        Visible="False"></asp:BoundColumn>
                                                                                    <asp:BoundColumn HeaderStyle-Width="150px" DataField="CompType" HeaderText="Company Type">
                                                                                    </asp:BoundColumn>
                                                                                </Columns>
                                                                            </asp:DataGrid>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </cc1:CollapsiblePanel>
                                                            <asp:TextBox ID="txthiddenImage" runat="server" Width="0px"></asp:TextBox><input
                                                                type="hidden" name="txtroleId" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="PanelUpdate" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMsg" runat="server">
                                    </asp:Panel>
                                    <asp:ListBox ID="lstError" runat="server" Width="635px" BorderWidth="0" BorderStyle="Groove"
                                        ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small" Visible="false"></asp:ListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
