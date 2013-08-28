<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Template_TemplateDetail, App_Web_u8bqjff1" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../supportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Template Detail</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../SupportCenter/calendar/popcalendar.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative; ;TOP:expression(this.offsetParent.scrollTop);BACKGROUND-COLOR:#e0e0e0}</style>

    <script type="text/javascript">



			var globalid; 
			var globalSkil;
			var globalAddNo;
			var globalGrid;
			var globaldbclick = 0;
		var rand_no = Math.ceil(500*Math.random())
		
		function RefreshAttachment()
		{
			document.Form1.submit();
		}
		
		function CheckLength()
		{
				var CDLength=document.getElementById('cpnlCallView_txtDescription').value.length;
				var TDLength=document.getElementById('cpnlTemplate_txtTmplDesc').value.length;
				
				if ( TDLength>0 )
				{
					if ( TDLength > 200)
					{
						alert('The Template Description length cannot be more than 200 characters \n(Current Length :'+TDLength+')');
						return false;
					}
				}
				
				if ( CDLength>0 )
				{
					if ( CDLength > 2000 )
					{
						alert('The Call Description cannot be more than 2000 characters\n (Current Length :'+CDLength+')');
						return false;
					}
				}
				
		
				return true;
		}
			
			function OpenW(a,b,c)
				{
					if (b=='TMPL')
					{
						document.getElementById('cpnlTemplate_txtTmplTaskType').value='';
						document.getElementById('cpnlTemplate_txtTmplCallType').value='';
					}
					if (b=='CALL')
					{
							if (document.getElementById('cpnlTemplate_txtTmplType').value=='TAO')
							{
							alert("No Call Type can be specified in case of TAO");
							return 0;
							}
					}
					if (b=='TKTY')
					{
							if (document.getElementById('cpnlTemplate_txtTmplType').value=='CAO')
							{
							alert("No Task Type can be specified in case of CAO");
							return 0;
							}
					}
					//wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
					
							if ('<%=session("PropCompanyType")%>' == 'SCM')        
								{
								wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
								}
								else
								{
									wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 ' +
											' where UDC.Company=CI_NU8_Address_Number and ProductCode='+ a + '   and UDCType='+"'"+b+"'"+
											' and UDC.Company=<%=session("PropCompanyID")%>' +
											' union ' +
											' select Name as ID,Description,'+"'"+"'"+' as Company from UDC' +
											' where   ProductCode='+ a + '  and UDCType='+"'"+b+"'"+
											' and UDC.Company=0'+
											' &tbname=' + c ,'Search'+rand_no,500,450);
								}
				}
				
			function OpenAB(c)
				{
					wopen('../../Search/Common/PopSearch1.aspx?ID=select UM_IN4_Address_No_FK as ID,UM_VC50_UserID as UserID,UM_VC4_Status_Code_FK as Status from T060011' + '  &tbname=' + c ,'Search'+rand_no,500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'Search',500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,ab.CI_VC36_Name as Name,UL_VC8_Role_PK as Role from T010011 ab,T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'Search',500,450);
				}
				
			function ForcedPostBack()
				{
					document.Form1.txthiddenImage.value="forced";
				}
				
			function 	OpenAttach()
				{
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=C&OPT=2','Additional_Address'+rand_no,400,450);
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
						'status=no, toolbar=no, scrollbars=yes, resizable=no');
					// Just in case width and height are ignored
					win.resizeTo(w, h);
					// Just in case left and top are ignored
					win.moveTo(wleft, wtop);
					win.focus();
				}
				///To open resizable comment window.
				function wopenComment(url, name, w, h) {
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
				'status=no, toolbar=no, scrollbars=yes, resizable=yes');
				    // Just in case width and height are ignored
				    win.resizeTo(w, h);
				    // Just in case left and top are ignored
				    win.moveTo(wleft, wtop);
				    win.focus();
				}
			function OpenWUdc_Search()
				{
					window.open("Udc_Home_Search.aspx","ss","scrollBars=no,resizable=No,width=400,height=450,status=no");
				}
			
				function addToParentList(Afilename,TbName,strName)
				{				
					if (Afilename != "" || Afilename != 'undefined')
						{
						varName = TbName + 'Name'
						//alert(strName);
							document.getElementById(TbName).value=Afilename;
							document.getElementById(varName).value=strName;
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
					return false;
				}							
		
				
			function OpenTask(varTable,addNo,compID,CallNo)
				{
				wopen('TemplateTask_edit.aspx?TASKNO='+varTable+'&TemplateID='+addNo+'&companyID='+compID+'&CallNo='+CallNo,'Search'+rand_no,430,400);
				}
				
			function KeyCheckTaskEdit(nn,rowvalues,tableID,AddressNo,compID,CallNo)
				{		
					globaldbclick = 1;
					document.Form1.txthiddenCallNo.value=nn;
					document.Form1.txthidden.value=nn;
					document.Form1.txthiddenImage.value='Edit';
					document.Form1.txthiddenTable.value=tableID;
			
					//alert(nn);
					if (tableID=='cpnlCallTask_dtgTask')
						{
							OpenTask(nn,AddressNo,compID,CallNo);
						}
					else if(tableID=='cpnlTaskAction_grdAction')
					{
						
						wopen('Action_edit.aspx?ScrID=294&ACTIONNO='+nn,'Search'+rand_no,430,300);
					}
					else
						{
							Form1.submit(); 
							return false;
						}													
				}								
				
																				
			function KeyCheck(aa,cc,dd)
				{
				
					globalid = cc;
					globalSkil  = aa;
					//globalAddNo = bb;
					globalGrid = dd;
				
					
					document.Form1.txthiddenSkil.value=aa;
					
					document.Form1.txthidden.value=aa;
					document.Form1.txtrowvalues.value=cc;
					document.Form1.txthiddenTable.value=dd;
						
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
								//alert(aa);
								
							}
							if(tableID=='cpnlCallTask_dtgTask')
							{
								document.Form1.txthiddenImage.value='Select';
								//setTimeout('Form1.submit();',700);
							}
						   	//Form1.submit(); 
					}
				
	
			function OpenUserInfo2(ADDNO)
			{
				var strscreen='294';
				wopen('../../SupportCenter/Callview/UserInfo.aspx?ScrID=294&ADDNO='+ADDNO,'Search'+rand_no,350,500);
				return false;
			}
			
			function KeyCheck55(aa,bb,cc)
				{		
				
					document.Form1.txthiddenImage.value='Edit';
					document.Form1.txthiddenSkil.value=aa;
					document.Form1.txthidden.value=bb;	
					//document.Form1.txthiddenGrid.value=dd;	
						SaveEdit('Edit'); 
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
									document.Form1.txthiddenImage.value=varImgValue;
									document.Form1.txthiddenSkil.value=globalSkil;
									document.Form1.txthidden.value=globalAddNo;	
									document.Form1.txthiddenGrid.value=globalGrid;	
									Form1.submit(); 
									return false;
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
							if ( CheckLength()==true )
							{
											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit(); 
							}	
							
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
							if ( CheckLength()==true )
							{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
							}		
							return false;
							
						}		
						
					if (varImgValue=='Attach')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							window.open ('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&OPT=2','Attachments'+rand_no,"scrollBars=yes,resizable=No,width=800,height=550,status=no  ");
							//wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&OPT=2','Attachments'+rand_no, 800, 500);
							//Form1.submit(); 
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
									return false;
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
											confirmed=window.confirm("Do You Want To Delete The Data ?");
											//confirmed=window.confirm(varMessage);
											if(confirmed==true)
													{
														document.Form1.txthiddenImage.value=varImgValue;
														Form1.submit(); 
														return false;
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
											confirmed=window.confirm("Do You Want To Delete The Data ?");
											//confirmed=window.confirm(varMessage);
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
				  return false;
			}
				
				
			function KeyImage(a,b,c,d)
				{							
					if (d==0 ) //if comment is clicked
						{
						    wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c + '&OPT=2', 'Comment' + rand_no, 500, 450);
						}
					else if (d==1)//if Attachment is clicked
						{
							wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+a+'&OPT=2' ,'Attachment'+rand_no,800,450);
						}
						else // if Attach form is clicked
						{
							wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno='+'<%=session("PropCallNumber")%>'+'&tno='+a +'&OPT=2'+'&TemplateID='+'<%=Session("SAddressNumber_Template")%>','AttachForms'+rand_no,500,450);							
						}
						return false;
				}
				
					
			function OpenVW(varTable)
				{
					wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ID='+varTable,'Search'+rand_no,500,450);
				}
				
				
				function OpenComp(c)
				{
				//	alert(c);
				
					wopen('../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type = '+ "'COM'" +' &tbname=' + c ,'Search'+rand_no,500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,ab.CI_VC36_Name as Name,UL_VC8_Role_PK as Role from T010011 ab,T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'Search',500,450);
				}	
				
		
				
					
				
    </script>

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlTemplate_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";
            var z = document.getElementById('cpnlCallTask_collapsible').cells[0].colSpan = "1";
        }
        
 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        } 
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    &nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                        ImageUrl="~/images/white.GIF" AlternateText="." CommandName="submit"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelTemplateDetail" runat="server" Width="120px" BorderWidth="2px"
                                                        BorderStyle="None" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small">Template Detail</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgOk" AccessKey="K" Visible="false" runat="server" ImageUrl="../../Images/s1ok02.gif"
                                                            ToolTip="Ok"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgAttachments" AccessKey="T" runat="server" ImageUrl="../../Images/ScreenHunter_075.bmp"
                                                            ToolTip="Attachment"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                            ToolTip="Delete"></asp:ImageButton>
                                                        <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('419','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 100%; height: 581px">
                                <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td valign="top" colspan="1">
                                            <!--  **********************************************************************-->
                                            <div style="overflow: auto; width: 100%; height: 581px">
                                                <table width="100%" border="0">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <cc1:CollapsiblePanel ID="cpnlTemplate" runat="server" Width="100%" BorderWidth="0px"
                                                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                                    TitleBackColor="Transparent" Text="Template Detail" ExpandImage="../../Images/ToggleDown.gif"
                                                                    CollapseImage="../../Images/ToggleUp.gif" Draggable="False" Visible="true" BorderColor="Indigo"
                                                                    Height="180px">
                                                                    <!-- **********************************************************************-->
                                                                    <table id="Table20" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="780"
                                                                        bgcolor="#f5f5f5" border="1">
                                                                        <tr>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                &nbsp;&nbsp;
                                                                            </td>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                <asp:Label ID="Label13" runat="server" Width="112px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Customer*</asp:Label><br>
                                                                                <asp:DropDownList ID="DDLCustomer" runat="server" Width="120px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td bordercolor="#f5f5f5">
                                                                                <asp:Label ID="lblMiddleName10" runat="server" Width="72px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">SubCategory*</asp:Label><br>
                                                                                <asp:DropDownList ID="DDLProject" runat="server" Width="120px" AutoPostBack="True"
                                                                                    CssClass="txtNoFocus">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td bordercolor="#f5f5f5">
                                                                                <asp:Label ID="Label7" runat="server" Width="112px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Template Type*</asp:Label><br>
                                                                                <uc1:CustomDDL ID="CDDLTemplateType" runat="server" Width="120px"></uc1:CustomDDL>
                                                                            </td>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                <asp:Label ID="Label15" runat="server" Width="132px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Copy Template</asp:Label><br>
                                                                                <asp:DropDownList ID="DDLCopyTemplate" runat="server" Width="230px" CssClass="txtNoFocus">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                &nbsp;&nbsp;
                                                                            </td>
                                                                            <td bordercolor="#f5f5f5">
                                                                                <asp:Label ID="Label5" runat="server" Width="99px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Template Name*</asp:Label><br>
                                                                                <asp:TextBox ID="txtTmplName" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                    MaxLength="25"></asp:TextBox>
                                                                            </td>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                <asp:Label ID="Label9" runat="server" Width="64px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Call Type*</asp:Label><br>
                                                                                <uc1:CustomDDL ID="CDDLCallType" runat="server" Width="120px"></uc1:CustomDDL>
                                                                            </td>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                <asp:Label ID="Label10" runat="server" Width="112px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Category1</asp:Label><br>
                                                                                <asp:TextBox ID="txtTmplCat1" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                    MaxLength="8"></asp:TextBox>
                                                                            </td>
                                                                            <td valign="top" bordercolor="#f5f5f5" rowspan="2">
                                                                                <asp:Label ID="Label8" runat="server" Width="112px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Description</asp:Label><br>
                                                                                <asp:TextBox ID="txtTmplDesc" runat="server" Width="350px" Height="53px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                    MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                &nbsp;&nbsp;
                                                                            </td>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                <asp:Label ID="Label12" runat="server" Width="70px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                    Font-Bold="True" ForeColor="DimGray">Category2</asp:Label><br>
                                                                                <asp:TextBox ID="txtTmplCat2" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                    MaxLength="8"></asp:TextBox>
                                                                            </td>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                <asp:Label ID="Label4" runat="server" Width="112px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Template Date</asp:Label><br>
                                                                                <asp:TextBox ID="txtTmplDate" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                    MaxLength="14" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                            <td valign="top" bordercolor="#f5f5f5">
                                                                                <asp:Label ID="lblMiddleName6" runat="server" Width="99px" Height="12px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Created By</asp:Label><br>
                                                                                <asp:TextBox ID="txtTmplByName" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                    MaxLength="8" ReadOnly="True"></asp:TextBox>
                                                                                <asp:TextBox ID="txtTmplBy" runat="server" Width="0px" Height="0" Font-Size="XX-Small"
                                                                                    Font-Names="Verdana" BorderWidth="0" MaxLength="8" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <!-- **********************************************************************-->
                                                                </cc1:CollapsiblePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 100%" valign="top" colspan="1">
                                                                <!-- **********************************************************************-->
                                                                <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" Height="274px" Width="100%"
                                                                    BorderWidth="0px" BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                    TitleClickable="True" TitleBackColor="Transparent" Text="Call Detail" ExpandImage="../../Images/ToggleDown.gif"
                                                                    CollapseImage="../../Images/ToggleUp.gif" Draggable="False" Visible="true" BorderColor="Indigo">
                                                                    <table id="Table3" height="89" cellspacing="0" cellpadding="0" width="780" border="0">
                                                                        <tr>
                                                                            <td valign="top" align="left">
                                                                                <table bordercolor="#5c5a5b" bgcolor="#f5f5f5" border="1">
                                                                                    <tr>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5" nowrap="nowrap">
                                                                                            <asp:Label ID="lblMiddleName4" runat="server" Width="72px" Height="12px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Customer</asp:Label><br>
                                                                                            <asp:TextBox ID="txtCustomerName" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                                MaxLength="36" ReadOnly="True"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtCustomer" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                                                                Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid" Visible="True"
                                                                                                CssClass="txtNoFocus" MaxLength="36"></asp:TextBox>
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 130px" valign="top" bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="lblMiddleName2" runat="server" Width="112px" Height="12px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Call Requested By*</asp:Label><br>
                                                                                            <uc1:CustomDDL ID="CDDLCallOwner" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                        </td>
                                                                                        <td style="width: 16px" valign="top" bordercolor="#f5f5f5">
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="Label14" runat="server" Width="112px" Height="12px" CssClass="FieldLabel">Coordinator</asp:Label><br>
                                                                                            <uc1:CustomDDL ID="CDDLCoordinator" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="lblMiddleName8" runat="server" Width="72px" Height="12px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Priority*</asp:Label><br>
                                                                                            <uc1:CustomDDL ID="CDDLPriority" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 130px" valign="top" bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="Label6" runat="server" Width="72px" CssClass="FieldLabel">Category</asp:Label><br>
                                                                                            <uc1:CustomDDL ID="CDDLCategory" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                        </td>
                                                                                        <td style="width: 16px" valign="top" bordercolor="#f5f5f5">
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="Label11" runat="server" Width="72px" CssClass="FieldLabel">Cause Code</asp:Label><br>
                                                                                            <uc1:CustomDDL ID="CDDLCauseCode" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td bordercolor="#f5f5f5">
                                                                                        </td>
                                                                                        <td bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="lblMiddleName12" runat="server" Width="72px" Height="12px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Reference</asp:Label><br>
                                                                                            <asp:TextBox ID="txtReference" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                                MaxLength="36"></asp:TextBox>
                                                                                        </td>
                                                                                        <td bordercolor="#f5f5f5">
                                                                                        </td>
                                                                                        <td style="width: 130px" bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="lblCateCode1" runat="server" Width="72px" CssClass="FieldLabel">CateCode1</asp:Label><br>
                                                                                            <asp:TextBox ID="txtCateCode1" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="width: 16px" bordercolor="#f5f5f5">
                                                                                        </td>
                                                                                        <td bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="lblCateCode2" runat="server" Width="72px" CssClass="FieldLabel">CateCode2</asp:Label><br>
                                                                                            <asp:TextBox ID="txtCateCode2" runat="server" Width="120px" Height="18px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                        <td valign="top" bordercolor="#f5f5f5">
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td bordercolor="#f5f5f5" colspan="7">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td valign="top" align="left">
                                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                            <td valign="top" align="left">
                                                                                <table bordercolor="#5c5a5b" height="142" width="100%" bgcolor="#f5f5f5" border="1">
                                                                                    <tr>
                                                                                        <td bordercolor="#f5f5f5" width="4">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td bordercolor="#f5f5f5" width="211">
                                                                                            <asp:Label ID="Label1" runat="server" Width="72px" Height="12px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Status</asp:Label><br>
                                                                                            <uc1:CustomDDL ID="CDDLStatus" runat="server" Width="110px" Enabled="False"></uc1:CustomDDL>
                                                                                        </td>
                                                                                        <td bordercolor="#f5f5f5">
                                                                                            <asp:Label ID="Label2" runat="server" Width="72px" Height="12px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Attachment</asp:Label><br>
                                                                                            &nbsp;<img class="PlusImageCSS" id="Attachment" onclick="OpenAttach();" alt="Attachment"
                                                                                                src="../../Images/Attach15_9.gif" border="0">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td bordercolor="#f5f5f5" width="4">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td bordercolor="#f5f5f5" colspan="2">
                                                                                            <asp:Label ID="Label3" runat="server" Width="72px" Height="12px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Subject*</asp:Label><br>
                                                                                            <asp:TextBox ID="txtSubject" runat="server" Width="284px" Height="18px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                                MaxLength="100"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td bordercolor="#f5f5f5" width="4">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td bordercolor="#f5f5f5" colspan="2">
                                                                                            <asp:Label ID="lblMiddleName21" runat="server" Width="72px" Height="12px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Description*</asp:Label><br>
                                                                                            <asp:TextBox ID="txtDescription" runat="server" Width="282px" Height="42px" Font-Size="XX-Small"
                                                                                                Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocus"
                                                                                                MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </cc1:CollapsiblePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Height="66px" Width="100%"
                                                                    BorderWidth="0px" BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                    TitleClickable="True" TitleBackColor="Transparent" Text="Task View" ExpandImage="../../Images/ToggleDown.gif"
                                                                    CollapseImage="../../Images/ToggleUp.gif" Draggable="False" Visible="true" BorderColor="Indigo">
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Panel ID="pnlTask" Width="1px" runat="server">
                                                                        <table style="border-collapse: collapse;" cellspacing="0" cellpadding="0" align="left"
                                                                            border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Image ID="ImgHidTask" Width="170px" ImageUrl="../../Images/divider.gif" Height="18px"
                                                                                        runat="server"></asp:Image>
                                                                                    <asp:TextBox ID="TxtStatus_F" runat="server" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                        BorderStyle="Solid" BorderWidth="1px" Visible="False" CssClass="txtNoFocusFE">ASSIGNED</asp:TextBox>
                                                                                    <asp:TextBox ID="TxtTaskNo_F" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                        BorderStyle="Solid" BorderWidth="1px" Visible="False" Enabled="False" runat="server"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TxtSubject_F" runat="server" Width="323px" Height="18px" Font-Size="XX-Small"
                                                                                        Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocusFE"
                                                                                        MaxLength="950" TextMode="MultiLine"></asp:TextBox>&nbsp;
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <uc1:CustomDDL ID="CDDLTaskType_F" runat="server" Width="65px"></uc1:CustomDDL>
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <uc1:CustomDDL ID="CDDLTaskOwner_F" runat="server" Width="74px"></uc1:CustomDDL>
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <asp:DropDownList ID="DDLDependancy_F" runat="server" Width="41px">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <asp:TextBox ID="TxtEstimatedHrs" runat="server" Width="36px" Height="18px" Font-Size="XX-Small"
                                                                                        Font-Names="Verdana" BorderStyle="Solid" BorderWidth="1px" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <asp:CheckBox ID="chkMandatory" runat="server" Width="26px" Height="18px" Font-Size="XX-Small"
                                                                                        Font-Names="Verdana" AutoPostBack="False" Checked="True"></asp:CheckBox>
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                                <%--<TD>
																							<uc1:CustomDDL id="CDDLPriority_F" runat="server" width="58px"></uc1:CustomDDL></TD>--%>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </cc1:CollapsiblePanel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <asp:ListBox ID="lstError" runat="server" Width="635px" Font-Size="XX-Small" Font-Names="Verdana"
                Visible="false" ForeColor="Red" BorderStyle="Groove" BorderWidth="0"></asp:ListBox>
            <input type="hidden" name="txthidden" />
            <!--Address Nuimber -->
            <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenSkil" />
            <!--Skill -->
            <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
            <input type="hidden" name="txthiddenGrid" /><!-- Image Clicked-->
            <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
            <input type="hidden" value="<%=strhiddenTable%>" name="txthiddenTable" />
            <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues" />
            <input type="hidden" value="<%=mstrCallNumber%>" name="txtTask" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
