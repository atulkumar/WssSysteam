<%@ page language="VB" autoeventwireup="false" enableeventvalidation="false" inherits="AdministrationCenter_UserOverwriteView_OverwriteView, App_Web_qankb1sf" maintainscrollpositiononpostback="true" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>User Overwrite View</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/TaskViewShortCuts.js" type="text/javascript"></script>

    <script type="text/javascript">
        var rand_no = Math.ceil(500*Math.random())
	
		//********************************************************************
		
			var gtype;
		var xmlHttp; 
		var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
		var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
		var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
		//netscape, safari, mozilla behave the same??? 
		var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 
	
		function CompanyChange()
		{
			xmlHttp=null;
			var ddlMember=document.getElementById('cpnlviewinfo_DDLCompany');
			var mem=ddlMember.options(ddlMember.selectedIndex).value;
			var url= '../../AJAX Server/AjaxInfo.aspx?Type=ROLEBYCOMP&CompID='+mem+'&Rnd='+Math.random();
			xmlHttp = GetXmlHttpObject(stateChangeHandler);    
			xmlHttp_Get(xmlHttp, url); 
			
		}
		 function FillHidden()
		 {
		 
			var ddlRole=document.getElementById('cpnlviewinfo_DDLRole');
			document.Form1.txthiddenRoleID.value=ddlRole.options(ddlRole.selectedIndex).value;
			
		 }
		function stateChangeHandler() 
		 { 	
				 document.getElementById("cpnlviewinfo_DDLRole").options.length=1;
			
				if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
				{ 
						var response = xmlHttp.responseXML; 
						var info = response.getElementsByTagName("INFO");
						
						if(info.length > 0)
						{
								var vTable = response.getElementsByTagName("TABLE");
								var intT;
								for ( intT=0; intT<vTable.length; intT++)
								{
									var item = vTable[intT].getElementsByTagName("ITEM");
									var objForm = document.Form1;
									switch(intT)
									{
										case 0:
										{								
											var RoleDataName='';
											var RoleDataID='';
											for (var inti=0; inti<item.length; inti++)
											{
												
													var objNewOption = document.createElement("OPTION");
													document.getElementById("cpnlviewinfo_DDLRole").options.add(objNewOption);
													objNewOption.value = item[inti].getAttribute("COL0");
													objNewOption.innerText = item[inti].getAttribute("COL1");
													RoleDataName=RoleDataName+item[inti].getAttribute("COL1") + '^';
													RoleDataID=RoleDataID+item[inti].getAttribute("COL0") + '^';
											}
											document.Form1.txthiddenRoleData.value=RoleDataName + '~' + RoleDataID ;
											break;
										}//case 0
							
									}//switch
								} //for loop
						
						}//if
						
				}//
				
		} //function
		
		
		function xmlHttp_Get(xmlhttp, url) 
		{ 
		        xmlhttp.open('GET', url, true); 
		        xmlhttp.send(null); 
		       
		} 
    
		function GetXmlHttpObject(handler) 
		{ 
				var objXmlHttp = null;    //Holds the local xmlHTTP object instance 
				if (is_ie)
		        { 
						var strObjName = (is_ie5) ? 'Microsoft.XMLHTTP' : 'Msxml2.XMLHTTP'; 
				        try
				        { 
								objXmlHttp = new ActiveXObject(strObjName); 
								objXmlHttp.onreadystatechange = handler; 
						} 
						catch(e)
						{ 
								alert('IE detected, but object could not be created. Verify that active scripting and activeX controls are enabled'); 
								return; 
			            } 
				} 
				else if (is_opera)
				{ 
						alert('Opera detected. The page may not behave as expected.'); 
						return; 
				} 
				else
				{ 
						objXmlHttp = new XMLHttpRequest(); 
						objXmlHttp.onload = handler; 
						objXmlHttp.onerror = handler; 
				} 
				return objXmlHttp; 
		} 
    
		
		
		
		//********************************************************************

		function OpenComp(c)
				{
					//var UId = document.getElementById('txtUserName').value
					wopen('../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type = '+ "'COM'" +' &tbname=' + c ,'Search11'+rand_no,500,450);
					
					//wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
//					wopen('../../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID, CI_VC36_Name as CompanyName, CI_VC8_Status as Status from T010011 where ' +' &tbname=' + c ,'Search',500,450);
				}
		
		
		function OpenRole(c)
				{
				
						var CompId;
						
						if((document.getElementById('cpnlviewinfo_txtCompId').value == '') || (document.getElementById('cpnlviewinfo_txtCompId').value == 'undefined'))
						{
						CompId=0;
					
							}
						else
						{
											
							CompId = document.getElementById('cpnlviewinfo_txtCompId').value;
						}
						
						
					//CompId = document.getElementById('txtCompNameName').value;
					var currentTime = new Date()
					var Day = currentTime.getDate();
					var Month = currentTime.getMonth();
					Month = Number(Month) + 1;
					var Year = currentTime.getFullYear();
					var curDate = Month + '/' + Day + '/' + Year;
					
					wopen('../../Search/Common/PopSearch1.aspx?ID=select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as RoleName, ROM_VC50_Status_Code_FK as Status from T070031 where  ROM_IN4_Company_ID_FK= '+ CompId + '&tbname=' + c ,'Search'+rand_no,500,500);	
					//wopen('../Search/Common/PopSearch.aspx?ID=select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as RoleName from T070031,T060011 where UM_VC50_UserID = ' + "'"+ UId  + "'"+'  and ROM_IN4_Role_ID_PK not in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = (select UM_IN4_Address_No_FK from t060011 where UM_VC50_UserID = ' + "'"+ UId  + "'"+') and RA_DT8_Assigned_Date <=' + "'"+curDate+"'" + 'and RA_DT8_Valid_UpTo >=' + "'"+curDate+"'" + 'and RA_VC4_Status_Code =' + "'ENB'" + ')and ROM_DT8_Start_Date<=' + "'"+curDate+"'" + ' and ROM_DT8_End_Date >=' + "'"+curDate+"'" + ' and UM_IN4_Company_AB_ID = '+ CompId +'  and ROM_VC50_Status_Code_FK = ' + "'ENB'"  + '&tbname=' + c ,'Search',500,500);	
				} 
		
		
		function OpenView(c)
		{
		 var ScreenId;
		 var CompId;
		 ScreenId=document.getElementById('ddlViewType').value 
		 CompId=document.getElementById('txtfromCompId').value 
		
		 
		 if (CompId > 0) 
		 {
		
		 wopen('../../Search/Common/PopSearch1.aspx?ID=select UV_IN4_View_ID as ID, UV_VC50_View_Name as ViewName, UV_IN4_Role_ID as RoleID from T030201 where  UV_NU9_Comp_ID= '+ CompId + ' and UV_VC50_tbl_Name=' +"'"+ ScreenId+"'" +'&tbname=' + c ,'Search'+rand_no,500,500);	
		}
		else
		
		{
		wopen('../../Search/Common/PopSearch1.aspx?ID=select UV_IN4_View_ID as ID, UV_VC50_View_Name as ViewName, UV_IN4_Role_ID as RoleID from T030201 where  UV_VC50_tbl_Name=' +"'"+ ScreenId+"'" +'&tbname=' + c ,'Search'+rand_no,500,500);	
		}
		
		}
		
		
			var globleID;
			function CallPrint(strid) 
			{ 
				var i=0;
				var j=0;
				var prtContent = document.getElementById(strid); 
				var WinPrint = window.open('','','left=-50,top=-50,width=0,height=0,toolbar=0,scrollbars=yes,status=0'); 
				WinPrint.document.write(prtContent.innerHTML); 
				WinPrint.document.close(); 
				WinPrint.focus(); 
				for(i=0;i<=4000;i++)
				{
					for (j=0;j<=500;j++);
				}
				 WinPrint.print(); 
				 WinPrint.close();
				 
				 
				//prtContent.innerHTML=strOldOne; 
			}

			
			
			
			function AdminEditView(viewid,viewtype,rowid)
			{
		
			wopen('AdminEditView.aspx?viewid='+ viewid +'&viewtype='+viewtype,'Search'+rand_no,450,500);
			
			}
			
			
			function KeyImage(a,b,c,d)
			{
		
				if (d==0 ) //if comment is clicked
				{		
				
					wopen('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID='+ a + '&tbname=A'  ,'Comment'+rand_no,500,450);
				}
				else//if Attachment is clicked
				{
					wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ACTIONNO='+ a + '&ID=A','Attachment'+rand_no,800,450);
	
				}
		
			}
			
			
			function OpenW(a,b,c)
				{
				wopen('../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				//popupWin = window.open('../../Search/Common/PopSearch.aspx?ID=select CUM_IN4_Address_No as ID ,CUM_VC25_First_Name as FirstName,CUM_VC25_Middle_Name as Middlename,CUM_VC25_Last_Name as LastName,CUM_VC25_Spouse_Name as SpouseName from TSL0071 where CUM_CH3_Type_id='+"'CUS'",'Search','Width=550px;Height=350px;dialogHide:true;help:no;scroll:yes');
				//window.open("AddAddress.aspx","ss","scrollBars=no,resizable=No,width=400,height=450,status=yes  " );
				}
				function addToParentList(Afilename,TbName,strName)
				{
				
					if (Afilename != "" || Afilename != 'undefined')
					{
						varName = TbName + 'Name'
					   //alert(Afilename);
						document.getElementById(TbName).value=Afilename;
						document.getElementById(varName).value=strName;
						aa=Afilename;
					}
					else
					
					{
						document.Form1.txtAB_Type.value=aa;
					}
				}
			function OpenAB(c)
				{
				    	wopen('../../Search/Common/PopSearch1.aspx?ID=select UM_IN4_Address_No_FK as ID,UM_VC50_UserID as UserID,UM_VC4_Status_Code_FK as Status from T060011' + '  &tbname=' + c ,'Search'+rand_no,500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'Search',500,450);					
				}
									
			function callrefresh()
				{
			//alert("hello");
					//location.href="../../SupportCenter/CallView/Task_View.aspx";
					document.Form1.txthiddenImage.value='';
					Form1.submit();
					//location.href="../../SupportCenter/CallView/Task_View.aspx";
				}
								
								
				function ConfirmDelete(varImgValue)
				
				
					{
								
							if (document.Form1.txthiddenTable.value == 'cpnlTaskView_GrdAddSerach')
							{
							
										if (document.Form1.txtTask.value==0)
										{
											alert("Please select the row");
										}
										else
										{
										
										var confirmed
										confirmed=window.confirm("Delete,Are you sure you want to Delete the selected record ?");
										if(confirmed==false)
							{
								
								return false;
							
							}
										
										

											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit();
											return false;
										}
							  }
							 else
								{
			
									if (document.Form1.txtTask.value==0)
									{
										alert("Please select the row");
									}
									else
										{
							
											var confirmed
										confirmed=window.confirm("Delete,Are you sure you want to Delete the selected record ?");
										if(confirmed==false)
							{
								
								return false;
							
							}
											
											
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 
								return false;
														
										}
								}	
			  }
				
			function SaveEdit(varImgValue)
				{
			    			if (varImgValue=='Edit')
												{
															if (document.Form1.txthiddenCallNo.value==0)
															{
																alert("Please select the row");
															}
															else
															{
																			//location.href="Call_Detail.aspx?ScrID=3&ID=0";
															document.Form1.txthiddenImage.value=varImgValue;
															Form1.submit(); 
															return false;
															}
															
												}	
												
												if (varImgValue=='Close')
												{
															window.close();	
												}
								
								
								if (varImgValue=='Add')
												{
												//location.href="Call_Detail.aspx?ScrID=3&ID=-1&PageID=3";
													document.Form1.txthiddenImage.value=varImgValue;
													Form1.submit();
													return false;
													  
												}	
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
									return false;
								}	
								
								if (varImgValue=='Select')
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
								alert("You don't have access rights to save record");
								return false;
							}

							if (obj.disabled==true) 
							{
								alert("You don't have access rights to save record");
								return false;
							}
						//End of Security Block
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
								
								if (varImgValue=='CloseCall')
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
								
								
								if (varImgValue=='Attach')
								
								{
								
													if (document.Form1.txtTask.value==0)
															{
																alert("Please select a task");
															}
															else
															{
																			//location.href="Call_Detail.aspx?ScrID=3&ID=0";
															document.Form1.txthiddenImage.value=varImgValue;
															Form1.submit(); 
															return false;
															}
									//document.Form1.txthiddenImage.value=varImgValue;
									//Form1.submit(); 
								}	
								
								if (varImgValue=='Reset')
									{
												var confirmed
												confirmed=window.confirm("Do You Want To reset The Page ?");
												if(confirmed==true)
														{	
																Form1.reset()
															
														}		
														return false;

									}			
									
									if (varImgValue=='Delete')
									{
									//alert(varImgValue);
												var confirmed
												if ( document.getElementById('cpnlviewinfo_txtViewID').value=='')
												{
													alert('Please select View');
												}
												else
												{
												confirmed=window.confirm("Do You Want To Delete The View ?");
												if(confirmed==true)
														{	
															document.Form1.txthiddenImage.value=varImgValue;
																Form1.submit()
															
														}		
													}
														return false;

									}			
				}				
				
			
				
			
		
				
				function KeyCheck(nn,VName,tableID,rowvalues,Comp)
					{
						
						globleID = VName;
						
							document.getElementById('cpnlviewinfo_txtViewID').value=nn;
							document.getElementById('cpnlviewinfo_txtViewN').value=VName;
							//document.Form1.txthiddenImage.value='Edit';
							
									var tableID='cpnlviewdetail_GrdAddSerach'  //your datagrids id
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
					
					
					
				function OpenTask(TASKNO,CALLNO)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('Task_edit.aspx?ScrID=334&TASKNO='+TASKNO+'&CALLNO='+CALLNO,'Search'+rand_no,500,450);
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
				}
					
			function OpenVW(varTable)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=464&TBLName='+varTable,'Search'+rand_no,500,450);
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
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
					'status=no, toolbar=no, scrollbars=yes, resizable=no');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}
			
				
    </script>

    <script type="text/javascript">
			    //A Function to improve design i.e delete the extra cell of table
			    function onEnd() {
			        var x = document.getElementById('cpnlviewinfo_collapsible').cells[0].colSpan = "1";
			        var y = document.getElementById('cpnlviewdetail_collapsible').cells[0].colSpan = "1";
			        
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
                                        <table cellspacing="0" cellpadding="0" width="100%"  border="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0" Width="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                        AlternateText="." CommandName="submit" ImageUrl="~/images/white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelUserOV" runat="server" Height="12px" Width="184px" ForeColor="Teal"
                                                        Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">USER OVERWRITE VIEW</asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                        </asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" AlternateText="Reset"
                                                            ImageUrl="../../Images/reset_20.gif"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                            ToolTip="Delete"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                            ToolTip="Search"></asp:ImageButton>
                                                        <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('533','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlviewinfo" runat="server" Height="47px" Width="100%"
                                                    BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                    ExpandImage="../../Images/ToggleDown.gif" Text="New View Info" TitleBackColor="Transparent"
                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                    Visible="True" BorderColor="Indigo">
                                                    <table id="Table122" style="height: 56px" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0"
                                                        width="100%" bgcolor="#f5f5f5" border="0">
                                                        <tr>
                                                            <td>
                                                                &nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lblViewName" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                    Font-Bold="True" ForeColor="DimGray">New View Name</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblViewType" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                    Font-Bold="True" ForeColor="DimGray">View Type</asp:Label>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtViewName" runat="server" Width="140px" MaxLength="25" CssClass="txtNoFocus"></asp:TextBox>&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlViewType" runat="server" Width="140px" CssClass="txtNoFocus"
                                                                    AutoPostBack="True">
                                                                    <asp:ListItem Value="463">Call/Task View</asp:ListItem>
                                                                    <asp:ListItem Value="464">Task View</asp:ListItem>
                                                                    <asp:ListItem Value="502">TDL View</asp:ListItem>
                                                                    <asp:ListItem Value="536">Historic View</asp:ListItem>
                                                                    <asp:ListItem Value="229">AB View</asp:ListItem>
                                                                    <asp:ListItem Value="40">SubCategory View</asp:ListItem>
                                                                    <asp:ListItem Value="799">Call View</asp:ListItem>
                                                                    <asp:ListItem Value="1024">Call Heirarchy</asp:ListItem>
                                                                    <asp:ListItem Value="2212">Task Heirarchy</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lblCompId" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                    Font-Bold="True" ForeColor="DimGray">Company Name</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRoleId" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                    Font-Bold="True" ForeColor="DimGray">Role Name</asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;&nbsp;
                                                                <asp:Label ID="lblcopyview" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                    Font-Bold="True" ForeColor="Blue">Copy View from</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="DDLCompany" runat="server" Width="140px" Font-Size="XX-Small"
                                                                    Font-Name="Verdana">
                                                                    <asp:ListItem></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DDLRole" runat="server" Width="140px" CssClass="txtNoFocus">
                                                                    <asp:ListItem Value=""></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtViewN" runat="server" Width="140px" Height="15px" BorderStyle="Solid"
                                                                    BorderWidth="1px" Font-Size="XX-Small" Font-Bold="True" BorderColor="Gray"></asp:TextBox>
                                                                <%--	<asp:textbox id="txtViewID" runat="server" Width="0px" CssClass="txtNoFocus"></asp:textbox>--%>
                                                                <input id="txtViewID" runat="server" type="hidden" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlviewdetail" runat="server" Height="47px" Width="100%"
                                                    BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                    ExpandImage="../../Images/ToggleDown.gif" Text="View Detail" TitleBackColor="Transparent"
                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                    Visible="True" BorderColor="Indigo">
                                                    <div style="overflow: auto; width: 100%; height: 420px">
                                                        <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                            width="100%" align="left" border="0">
                                                            <tr>
                                                                <td valign="top" align="left">
                                                                    <asp:Panel ID="Panel1" runat="server">
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="left">
                                                                    <!--  **********************************************************************-->
                                                                    <asp:DataGrid ID="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px"
                                                                        Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" CssClass="Grid"
                                                                        DataKeyField="ViewID" PageSize="50" HorizontalAlign="Left" GridLines="Horizontal"
                                                                        CellPadding="0">
                                                                        <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                        <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                                        <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                                        </ItemStyle>
                                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                        </HeaderStyle>
                                                                        <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                    </asp:DataGrid><!-- Panel for displaying Task Info -->
                                                                    <!-- Panel for displaying Action Info-->
                                                                    <!-- ***********************************************************************-->
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </cc1:CollapsiblePanel>
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
                        <asp:ListBox ID="lstError" runat="server" Width="752px" BorderStyle="Groove" BorderWidth="0"
                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input style="width: 32px; height: 22px" type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthiddenRoleID" />
                        <input type="hidden" name="txthiddenRoleData" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
