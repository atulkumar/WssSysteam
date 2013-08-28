<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Inventory_Inventory_View, App_Web_uw2qx94f" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Inventory View</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<meta http-equiv="Expires" content="0">
		<meta http-equiv="Cache-Control" content="no-cache">
		<meta http-equiv="Pragma" content="no-cache">
		<script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../../Images/Js/JSValidation.js"></script>
		<LINK href="../calendar/popcalendar.css" type="text/css" rel="stylesheet">
			<script language="javascript" src="../../DateControl/ION.js"></script>
			<script language="javascript" src="../../Images/Js/CallViewShortCuts.js"></script>
			<LINK href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
				<script language="javascript" type="text/javascript">
                    	                      
		var gCallStatus;
		
                      
		///***********************Call View AJAX**********************************////////
		var gtype;
		var xmlHttp; 
		var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
		var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
		var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
		//netscape, safari, mozilla behave the same??? 
		var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 

		function CallAjax()
		{
	
						var intCallNo=document.Form1.txthiddenCallNo.value;
						var intTaskNo=0;//document.Form1.txtTask.value;
						var strComp=document.Form1.txtComp.value;
					    var url= '../../AJAX Server/AjaxInfo.aspx?Type=FillCallViewSession&CallNo='+ intCallNo +'&Comp=' + strComp+'&RKey='+ Math.random(); 
						xmlHttp = GetXmlHttpObject(stateChangeHandler);    
						xmlHttp_Get(xmlHttp, url); 
					
		}
		 
	
		 
		function stateChangeHandler() 
		 { 	
				if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
				{ 
						document.getElementById('imgAjax').style.display='none';//src="../images/divider.gif";
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
											
											for (var inti=0; inti<item.length; inti++)
											{
											
													gCallStatus = item[inti].getAttribute("COL0");
													gblnAttachment= item[inti].getAttribute("COL1");
													if ( gblnAttachment==0 )
													{
															document.getElementById('imgAttachments').title="No Attachment Uploaded";
													}
													else if ( gblnAttachment==1 )
													{
															document.getElementById('imgAttachments').title="View Attachment";
													}
													else
													{
															document.getElementById('imgAttachments').title="Select a Call to View Attachment";
													}
													document.getElementById('imgEdit').title="Edit Call";
													document.getElementById('imgMonitor').title="Set Call Monitor";			
													document.getElementById('imgTask').title="View Task";
											}
											break;
										}//case 0
									}//switch
								} //for loop
						}
				} 
				else
				{
						document.getElementById('imgAjax').src="../../images/ajax.gif";
						document.getElementById('imgAjax').style.display='inline';
				}
		} 
		
		
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
    
    
		///**************************Call View AJAX end*********************************///////
                      
                                  
			var globleID;
			
			
		function CheckLength()
		{
				var TDLength=document.getElementById('cpnlCallTask_TxtSubject_F').value.length;
				if ( TDLength>0 )
				{
					if ( TDLength > 1000 )
					{
						alert('The Task Subject cannot be more than 1000 characters\n (Current Length :'+TDLength+')');
						return false;
					}
				}
				return true;
		}
		
			
			
			function ChangeHeight(txt,id)
				{
				//alert(event.keyCode);
				//alert(id);
					if (event.keyCode==13 )
					{
					document.Form1.submit();
					event.returnValue=false;
					}
					
					var n=document.getElementById(id).value.length;
					if ( n<30 )
					{
					document.getElementById(id).runtimeStyle.height=18;
					document.Form1.txtHIDSize.value="18";
					}
					if ( n>30 && n<60 )
					{
					document.getElementById(id).runtimeStyle.height=30;
					document.Form1.txtHIDSize.value="30";
					}
					if ( n>60 && n<90)
					{
					document.getElementById(id).runtimeStyle.height=42;
					document.Form1.txtHIDSize.value="42";
					}
					if ( n>90 && n<120)
					{
					document.getElementById(id).runtimeStyle.height=55;
					document.Form1.txtHIDSize.value="55";
					}
					if ( n>120 && n<150)
					{
					document.getElementById(id).runtimeStyle.height=68;
					document.Form1.txtHIDSize.value="68";
					}
					if ( n>150 && n<180)
					{
					document.getElementById(id).runtimeStyle.height=81;
					document.Form1.txtHIDSize.value="81";
					}
					if ( n>180 && n<210)
					{
					document.getElementById(id).runtimeStyle.height=94;
					document.Form1.txtHIDSize.value="94";
					}
					if ( n>210 && n<240)
					{
					document.getElementById(id).runtimeStyle.height=107;
					document.Form1.txtHIDSize.value="107";
					}
					if ( n>240 && n<270)
					{
					document.getElementById(id).runtimeStyle.height=120;
					document.Form1.txtHIDSize.value="120";
					}
					if ( n>270)
					{
					document.getElementById(id).runtimeStyle.height=133;
					document.Form1.txtHIDSize.value="133";
					}
				}
			
		
				function KeyImage(a,b,c,d,comp,CallNo)
				{							
					if (d==0 ) //if comment is clicked
				
						{		
							wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+ "'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
						}
					else if (d==1) //if Attachment is clicked
					{
					
						wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+a ,'Attachment',700,240);
					}
					else if (d==5)//call comment
					{
						wopen('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID='+ a + '&tbname=C&CallNo='+CallNo+'&comp='+comp  ,'Comment',500,450);
					}
					else if (d==7)
					{
						
						wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo='+a+'&CompID='+comp+'&CallNo='+CallNo ,'Attachment',700,240);
					}
					else // if Attach form is clicked
						{
							wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno='+'<%=session("PropCallNumber")%>'+'&tno='+a ,'AttachForms',500,450);							
						
						}
				}

				
			function KeyCheckTaskEdit(nn,rowvalues,tableID)
				{		
					globaldbclick = 1;
					document.Form1.txthiddenTaskNo.value=nn;
					document.Form1.txthidden.value=nn;
					document.Form1.txthiddenImage.value='Edit';
					document.Form1.txthiddenTable.value=tableID;
			
				//	alert(nn);
					if (tableID=='cpnlCallTask_dtgTask')
						{
							OpenTask(nn);
						}
					else if(tableID=='cpnlTaskAction_grdAction')
					{
						
						wopen('Action_edit.aspx?ScrID=294&ACTIONNO='+nn,'Search',430,300);
					}
					else
						{
							Form1.submit(); 
						}													
				}					
			function OpenW(a,b,c)
				{
				wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
				//popupWin = window.open('../../Search/Common/PopSearch.aspx?ID=select CUM_IN4_Address_No as ID ,CUM_VC25_First_Name as FirstName,CUM_VC25_Middle_Name as Middlename,CUM_VC25_Last_Name as LastName,CUM_VC25_Spouse_Name as SpouseName from TSL0071 where CUM_CH3_Type_id='+"'CUS'",'Search','Width=550px;Height=350px;dialogHide:true;help:no;scroll:yes');
				//window.open("AddAddress.aspx","ss","scrollBars=no,resizable=No,width=400,height=450,status=no  " );
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
								var compType='<%=session("propCompanyType")%>';
				var compID='<%=session("propCompanyID")%>';
				var strQuery;
				if (compType=='SCM')
				{
					strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id ';
				}
				else
				{
					strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id='+compID+'  or um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='+"'SCM'"+' ))';
				}
					wopen('../../Search/Common/PopSearch.aspx?ID='+strQuery+'&tbname=' + c ,'Search',500,450);
				}
									
			function callrefresh()
				{
				//alert("hello");
					location.href="../../SupportCenter/CallView/Call_View.aspx?ScrID=4";
					//Form1.submit();
				}
								

				
				
			function SaveEdit(varImgValue)
			
				{
			    					if ( varImgValue=='Tasks')
				      	{
										//	alert(document.Form1.txthiddenCallNo.value);
						if (document.Form1.txthiddenCallNo.value==0 ||document.Form1.txtComp.value=="")
						    {
							alert("Please select a Call");
				     		return false;
						     }
						else
						     {
							intCallNo=document.Form1.txthiddenCallNo.value;
							strComp=document.Form1.txtComp.value;
							wopen('Tasks.aspx?ScrID=832&intCallNo='+intCallNo + '&strComp=' +strComp , 'ActionView', 870,500);
							return false;
					          }					
					  }	
					  
			    		
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

															if (document.Form1.txthiddenCallNo.value==0 && document.Form1.txthiddenTaskNo.value=='')
															{
																alert("Please select the row");
															}
															else
															{
																	if ( document.Form1.txthiddenTable.value=='cpnlCallTask_dtgTask' )
																	{
																				var TASKNO=document.Form1.txthiddenTaskNo.value;
																				if ( gCallStatus=='CLOSED')
																				{	alert('You cannot edit task for a Closed Call');}
																				else
																				{wopen('Task_edit.aspx?ScrID=334&TASKNO='+TASKNO,'Search',440,470);}
																	}
																	else
																	{
																			document.Form1.txthiddenImage.value=varImgValue;
																			Form1.submit(); 
																	}
															
															}
															
															return false;
									}	
												
								if (varImgValue=='Close')
								{
											window.close();	
								}
								
								if (varImgValue=='Logout')
								{
										document.Form1.txthiddenImage.value=varImgValue;
										Form1.submit(); 
										return false;
								}
								
								if (varImgValue=='Add')
									{
												//location.href="Call_Detail.aspx?ScrID=3&ID=-1";
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
								if (varImgValue=='Monitor')
								{
									var CallStatus=gCallStatus;

									if (CallStatus=='CLOSED')
									{
										alert('You cannot set Monitor on CLOSED call');
									}
									else
									{
									 //CallNo=globleID;
										CallNo=document.Form1.txthidden.value;
									if( CallNo==0) 
										{
											CallNo = '<%=Session("PropCallNumber")%>';
										}
							//alert(CallNo);
										if((CallNo == '') || (CallNo == '0'))
										{
											alert('Please select the call first.');	
											return false;
										}
										else
										{
										TaskNo = document.Form1.txthidden.value;
										wopen('../../CommunicationSetup/CommunicationSetupOnCall.aspx?CallNo='+ CallNo +'&Comp='+document.Form1.txtComp.value ,'Attachment',800,490);
										return false;
										}									
									}						
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
							var obj=document.getElementById("imgtask")
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
									if (CheckLength()==true)
									{
										document.Form1.txthiddenImage.value=varImgValue;
										Form1.submit(); 
									}
									return false;
								}	
								
								if (varImgValue=='CloseCall')
								{	
									document.Form1.txthiddenImage.value=varImgValue;
									
									document.Form1.txtrowvaluescall.value =0;  
										
									Form1.submit(); 
									return false;
								}
								if (varImgValue=='MyCall')
								{	
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
									return false;
								}
																
								if (varImgValue=='Attach')
								{
							//	alert(gblnAttachment);
								//CallAjax();

								//alert(gblnAttachment);
								
															if (document.Form1.txthiddenCallNo.value==0 && document.Form1.txthiddenTaskNo.value=='')
															{
																alert("Please select the row");
																	return false;
															}
															else
															{
															//location.href="Call_Detail.aspx?ScrID=3&ID=0";
															//document.Form1.txthiddenImage.value=varImgValue;
															//Form1.submit(); 
															if (gblnAttachment==1)
															{
																	wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments',800,550);
															}
															else
															{
																	alert('No Attachment Uploaded');
															}
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
																Form1.reset();
																
														}		

									}			
				}				
				
			
				function callrefresh()
					{
					Form1.submit();
					document.Form1.txtComp.value=document.Form1.txtComp.value;
					}
				
				function KeyCheck(nn,rowvalues,rowvaluescall,tableID,Comp,SuppComp)
					{
				//	alert(SuppComp);
				
			
					globleID = nn;
						
					
						
						document.Form1.txthidden.value=nn;
						 
						document.Form1.txthiddenTable.value=tableID;
						  alert();
						document.Form1.txtrowvalues.value=rowvalues;
						document.Form1.txtrowvaluescall.value=rowvaluescall;
						//alert(rowvaluescall);
						if (tableID =='cpnlCallView_GrdAddSerach')
						{
						
							document.Form1.txtComp.value=Comp;
							document.Form1.txthiddenCallNo.value=nn;
						}
						else if (tableID =='cpnlCallTask_dtgTask')
						{
							document.Form1.txthiddenTaskNo.value=nn;
						}
						//Form1.submit();
						
										//var tableID='cpnlCallView_GrdAddSerach'  //your datagrids id
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
							
							
							if (tableID =='cpnlCallView_GrdAddSerach')
							{
						
									document.Form1.txthiddensuppcomp.value=SuppComp;	
							document.Form1.txthiddenImage.value='Select';
							CallAjax();
							//setTimeout('Form1.submit();',100);
							return false;
							//Form1.submit(); 
							}
							
					}	
				
				function KeyCheck555(CallNo,rowvalues,tableID,Comp,CallOwner,TaskOwnerID)
					{
							
							
													
							
							//alert("asdf");
							var strscreen;
							strscreen='463'
							 
							document.Form1.txthiddenCallNo.value=CallNo;
							document.Form1.txthidden.value=CallNo;
							document.Form1.txthiddenImage.value='';
							document.Form1.txthiddenTable.value=tableID;
							document.Form1.txtComp.value=Comp;
							document.Form1.txtByWhom.value=CallOwner;
							
							if (tableID=='cpnlCallTask_dtgTask')
							{
						//	alert();
								//OpenUserInfo(CallNo,CallOwner,Comp,strscreen);
								wopen('UserInfo.aspx?ScrID=334&ADDNO='+TaskOwnerID+'&CALLOWNER='+CallOwner,'Search',350,500);
							//	OpenUserInfo(TaskOwnerID);
							}
							else
							{
							//alert();
							wopen('UserInfo.aspx?ScrID=334&CALLNO='+CallNo+'&CALLOWNER='+CallOwner+'&COMP='+Comp+'&ScreenID='+strscreen,'Search',350,500);
						//	OpenUserInfo(CallNo,CallOwner,Comp,strscreen);
							//Form1.submit(); 
							return false;
							}
				
							
					}	
				
				
				function OpenUserInfo(varTable,CallOwner,Comp,strscreen)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
				wopen('UserInfo.aspx?ScrID=334&CALLNO='+varTable+'&CALLOWNER='+CallOwner+'&COMP='+Comp+'&ScreenID='+strscreen,'Search',350,500);
				//wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo='+a+'&CompID='+comp+'&CallNo='+CallNo ,'Attachment',800,550);
				
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
				}
				
				
					
					function KeyCheck55(nn,rowvalues,tableID,Comp)
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
							
							
							//alert("asdf");
							
							document.Form1.txthidden.value=nn;
							document.Form1.txthiddenImage.value='Edit';
							document.Form1.txthiddenTable.value=tableID;
							document.Form1.txtComp.value=Comp;
							
							if (tableID=='cpnlCallTask_dtgTask')
							{
								document.Form1.txthiddenTaskNo.value=nn;
								OpenTask(nn);
							}
							else
							{
								document.Form1.txthiddenCallNo.value=nn;
								Form1.submit(); 
								return false;
							}
						
							
					}	
					
					
				function OpenTask(varTable)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
						var CS=gCallStatus;
						if ( CS=='CLOSED')
						{
								alert('Task cannot be edited for a Closed Call');
						}
						else
						{
								wopen('Task_edit.aspx?ScrID=334&TASKNO='+varTable,'Search',440,470);
						}
				}
					
			function OpenVW(varTable)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
				wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=799&TBLName='+varTable,'Search',450,500);
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
				return false;
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
		
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0">
		<FORM id="Form1" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td vAlign="top">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<div align="right"><IMG height="2" src="../../Images/top_right_line.gif" width="96"></div>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../Images/top_left_back.gif">&nbsp;</td>
											<td width="50"><IMG height="20" src="../../Images/top_right.gif" width="50"></td>
											<td width="21"><A href="#"><IMG height="20" src="../../Images/bt_min.gif" width="21" border="0"></A></td>
											<td width="21"><A href="#"><IMG height="20" src="../../Images/bt_max.gif" width="21" border="0"></A></td>
											<td width="19"><A href="#"><IMG onclick="CloseWSS();" height="20" src="../../Images/bt_clo.gif" width="19" border="0"></A></td>
											<td width="6"><IMG height="20" src="../../Images/bt_space.gif" width="6"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr width="100%">
											<td background="../../Images/top_nav_back.gif" height="67">
												<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
													<TR>
														<TD width="256">
															<div style="WIDTH: 130px"><asp:button id="BtnGrdSearch" runat="server" Width="0px" 
                                                                    Height="0" BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:button><asp:imagebutton id="imgbtnSearch" tabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="~/images/white.GIF"
																	CommandName="submit" AlternateText="."></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../../Images/left005.gif"
																	name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../../Images/Right005.gif"
																	name="ingShow">
																<asp:label id="lblTitleLabelCallView" runat="server" Width="128px" CssClass="TitleLabel">INVENTORY VIEW</asp:label></div>
														</TD>
														<td>
															<div><IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
																<asp:imagebutton id="imgAdd" accessKey="A" runat="server" ImageUrl="../../Images/s2Add01.gif" AlternateText="New Call"></asp:imagebutton>&nbsp;<asp:imagebutton id="imgEdit" accessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif" AlternateText="Select a Call to Edit"></asp:imagebutton>&nbsp;<asp:imagebutton id="imgSearch" accessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
																	AlternateText="Search"></asp:imagebutton>&nbsp;<asp:imagebutton id="ImgClose" accessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
																	ToolTip="Close"></asp:imagebutton>&nbsp;<IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
															</div>
														</td>
														<TD align="center"></TD>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../../Images/top_nav_back01.gif" height="67">
												<div style="WIDTH: 150px"><IMG id="imgAjax" title="ajax" height="30" src="../../images/divider.gif" width="30"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('4','../../');" alt="E"
														src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../../icons/logoff.gif"
														border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;</div>
											</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../Images/main_line.gif" height="10"><IMG height="10" src="../../Images/main_line.gif" width="6"></td>
											<td width="7" height="10"><IMG height="10" src="../../Images/main_line01.gif" width="7"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../../Images/main_line02.gif" height="2"><IMG height="2" src="../../Images/main_line02.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../../Images/main_line03.gif" width="12"></td>
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
														<table cellSpacing="0" cellPadding="0" width="100%" border="0">
															<TBODY>
																<tr>
																	<td colSpan="2"><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="47px" BorderWidth="0px" BorderStyle="Solid"
																			Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif" Text="Error Message"
																			TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="False"
																			BorderColor="Indigo">
																			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
																				border="0">
																				<TR>
																					<TD colSpan="0" rowSpan="0">
																						<asp:Image id="Image1" runat="server" Height="16px" Width="16px" ImageUrl="../../Images/warning.gif"></asp:Image></TD>
																					<TD colSpan="0" rowSpan="0">&nbsp;
																						<asp:Label id="lblError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:Label>
																						<asp:ListBox id="lstError" runat="server" Width="722px" BorderStyle="Groove" BorderWidth="0"
																							ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel></td>
																</tr>
																<TR>
																	<td width="100%"><cc1:collapsiblepanel id="cpnlCallView" runat="server" Width="100%" Height="530px" BorderWidth="0px" BorderStyle="Solid"
																			Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif" Text="Inventory View"
																			TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="true"
																			BorderColor="Indigo">
																			<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 515px">
																				<TABLE id="Table1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="370%"
																					align="left" border="0">
																					<TR>
																						<TD>
																							<TABLE cellSpacing="0" cellPadding="0" border="0">
																								<TR>
																									<TD>
																										<asp:panel id="pnl" runat="server" Width="0px">
																											<TABLE cellSpacing="0" cellPadding="0" border="0">
																												<TR>
																												</TR>
																											</TABLE>
																										</asp:panel></TD>
																									<TD>
																										<asp:panel id="Panel1" runat="server"></asp:panel></TD>
																								</TR>
																							</TABLE>
																						</TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" align="left"><!--  **********************************************************************-->
																							<asp:datagrid id="GrdAddSerach" runat="server" CssClass="grid" BorderColor="Silver" BorderStyle="None"
																								BorderWidth="1px" ForeColor="MidnightBlue" Font-Names="Verdana" AllowPaging="True" PagerStyle-Visible="False"
																								CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left" PageSize="20" DataKeyField="ID"
																								AllowSorting="True">
																								<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
																								<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																								<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
																								<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White" BackColor="#E0E0E0"></HeaderStyle>
																								<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
																								<PagerStyle Visible="False" Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
																							</asp:datagrid><!-- Panel for displaying Task Info --> <!-- Panel for displaying Action Info-->  <!-- ***********************************************************************--></TD>
																					</TR>
																				</TABLE>
																			</DIV>
																			<asp:panel id="Panel5" runat="server">
																				<asp:panel id="Panel6" runat="server"></asp:panel>
																			</asp:panel>
																		</cc1:collapsiblepanel></td>
																</TR>
																<tr>
																	<td></td>
												</TD>
											</TR>
										</TABLE>
									</DIV>
								</td>
								<td vAlign="top" width="12" background="../../Images/main_line04.gif"><IMG height="1" src="../../Images/main_line04.gif" width="12"></td>
							</tr>
						
						</td>
				</tr>
				<tr>
					<td height="2">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td background="../../Images/main_line06.gif" height="2"><IMG height="2" src="../../Images/main_line06.gif" width="2"></td>
								<td width="12" height="2"><IMG height="2" src="../../Images/main_line05.gif" width="12"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td background="../../Images/bottom_back.gif">&nbsp;</td>
								<td width="66"><IMG height="31" src="../../Images/bottom_right.gif" width="66"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			 <INPUT type="hidden" name="txthidden">
			<INPUT type="hidden" name="txthiddenImage">
		</FORM>
	</body>
</html>
