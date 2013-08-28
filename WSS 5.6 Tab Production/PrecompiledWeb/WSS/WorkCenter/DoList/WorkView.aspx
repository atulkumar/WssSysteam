<%@ page language="VB" autoeventwireup="false" inherits="WorkCenter_DoList_WorkView, App_Web_6cnpkvel" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../supportcenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Historic View</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js"></script>

    <script language="javascript" src="../../DateControl/ION.js"></script>

    <script language="javascript" src="../../Images/Js/TaskViewShortCuts.js"></script>

    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script type="text/javascript">
        
        //A Function to call on Page Load to set grid width according to screen size
    function onLoad() 
        {
            var divTaskView = document.getElementById('divTaskView');
            divTaskView.style.width = document.body.clientWidth - 30 + "px";
        }
        //A Function to improve design i.e delete the extra cell of table
   function onEnd() 
        {
        var x = document.getElementById('cpnlTaskView_collapsible').cells[0].colSpan = "1";
        var y = document.getElementById('cpnlTaskAction_collapsible').cells[0].colSpan = "1";
       
        } 
         //A Function is Called when we resize window
        window.onresize = onLoad;    
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(onLoad);   
    </script>

    <script type="text/javascript">


			var globleID;
			var rand_no = Math.ceil(500*Math.random())
				
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
			
		function ShowAttachment(P)		
		{
		
				if ( P == 1 )
				{
					wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments'+rand_no,800,550);
				}
				else if ( P==-1 )
				{
					alert('Please select a Task');
				}
				else 
				{
					alert('No attachment Uploaded');
				}
				return false;
		
		}
				
		function KeyViewImage(taskno,rowid,cpnlname,wopenmod,comp,CallNo)
			{
			
				if (wopenmod==0 ) //if comment is clicked
					 
				{		
				
				wopen('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID='+ taskno + '&tbname=TASK&CallNo='+CallNo+'&comp='+comp  ,'Comment'+rand_no,500,450);
					
				}
				else if (wopenmod==2)//if Attachment is clicked
				{
					//wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ACTIONNO='+ a + '&ID=A','Attachment',500,450);
         
            	wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=T&TaskNo='+taskno+'&CompID='+comp+'&CallNo='+CallNo ,'Attachment'+rand_no,700,240);
	
				}
				else
				{
				wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno='+CallNo+'&tno='+taskno+'&CompID='+comp ,'AttachForms'+rand_no,500,450);							
				}
	
			}
				
				
				
				
				
						
			function KeyImage(a,b,c,d)
			{
		
				if (d==0 ) //if comment is clicked
				{		
				
					wopen('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID='+ a + '&tbname=A'  ,'Comment'+rand_no,500,450);
				}
				else//if Attachment is clicked
				{
					wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ACTIONNO='+ a + '&ID=A','Attachment'+rand_no,700,240);
	
				}
		
			}
			
			
			function OpenW(a,b,c)
				{
				wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
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
					wopen('../../Search/Common/PopSearch.aspx?ID='+strQuery+'&tbname=' + c ,'Search'+rand_no,500,450);
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
											alert("You cannot delete the Task");
									/*	var confirmed
										confirmed=window.confirm("Delete,Are you sure you want to Delete the selected record ?");
										if(confirmed==false)
										{
											
											return false;
										
										}
										
										

											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit();
											return false;*/
										}
									return false;	
							  }
							  
							 else
								{
									  
									  if (document.Form1.txthiddenTable.value != '')
									  {
			
											if (document.Form1.txtTask.value==0)
											{
												alert("Please select the row");
											}
											else
											{
														var ts='<%=session("PropTaskStatus")%>';
														//alert(ts);
														if(ts=='CLOSED')
														{
															alert('You cannot delete action of a Closed Task');
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
																	document.Form1.txtrowvaluesCall.value =0;  
																	
																	Form1.submit(); 
																	return false;
														}
											}
										}	
										else
										{
											alert("Please select the row");
										}
								}	
									return false;
			  }
	
			function OpenUserInfo2(ADDNO)
			{
				var strscreen='329';
				wopen('../../supportCenter/callview/UserInfo.aspx?ScrID=329&ADDNO='+ADDNO,'Search'+rand_no,350,500);
				return false;
			}
				
		function SaveEdit(varImgValue)
			
				{
			      var TaskNo = document.Form1.txtTask.value;
                  var CallNo =document.Form1.txthiddenCallNo.value;
			     var CompID =document.Form1.txtComp.value;
			    	if (varImgValue=='View')
							{
						      
						      document.Form1.txthiddenImage.value=varImgValue;
						      document.Form1.txthiddenCallNo.value=0;
						      Form1.submit(); 		
								
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
							
								if (document.Form1.txthiddenCallNo.value==0)
												{
													alert("Please select the row");
												}
												else
												{
													//location.href="Call_Detail.aspx?ScrID=3&ID=0";
																document.Form1.txthiddenImage.value=varImgValue;
																var screenid = window.parent.getActiveTabDetails();
																window.parent.OpenCallDetailInTab('Call# ' + CallNo, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=6&CallNumber=" + CallNo + "&CompId=" + CompID, 'Call' + CallNo,screenid);
																
																//Form1.submit(); 
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
                                                    window.parent.OpenTabOnAddClick('Call Entry', "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=-1&PageID=6" , "3");
													//Form1.submit();
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
											    document.Form1.txtrowvaluesCall.value =0;  
													
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
														var ts='<%=session("PropTaskStatus")%>';
														if(ts=='CLOSED')
															alert('You cannot forward a Closed Task');
														else
															wopen('../../supportcenter/callview/Task_Fwd.aspx?ScrID=340','FWD'+rand_no,400,250);
													}	 
													return false;
									   
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

									}			
								return false;
				}				
				
			
			function KeyCheck555(Adressbookno,suppowner)
					{
							//alert(Adressbookno);
						var strscreen;
							strscreen='536'
								OpenUserInfo(Adressbookno,suppowner,strscreen);
												
							//Form1.submit(); 
							return false;
					}	
									
					function OpenUserInfo(Adressbookno,supponer,strscreen)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
				wopen('../../SupportCenter/CallView/UserInfo.aspx?ADDNO='+Adressbookno +'&CALLOWNER='+supponer+'&ScreenID='+strscreen,'Search'+rand_no,350,500);
				//wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo='+a+'&CompID='+comp+'&CallNo='+CallNo ,'Attachment',800,550);
				
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
				}		
		
					
						
				function KeyCheck(nn,col,rowvalues,rowvaluescall,tableID,Comp)
					{
						
						globleID = col;
						//alert(nn);
						document.Form1.txthiddenCallNo.value=col;
						document.Form1.txtTask.value=nn;
						document.Form1.txthiddenTable.value=tableID;
						document.Form1.txtrowvalues.value=rowvalues;
						document.Form1.txtrowvaluesCall.value =rowvaluescall;  
						document.Form1.txtComp.value=Comp;
						
															
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
								
									if (tableID=='cpnlTaskView_GrdAddSerach')
									{
									
										document.Form1.txthiddenImage.value='Select';
										__doPostBack("upnlTaskView", "")
										//setTimeout('Form1.submit();',700);
										//Form1.submit(); 
									}
					}	
					
					function KeyCheck55(nn,col,rowvalues,tableID,Comp)
					{
					    var screenid = window.parent.getActiveTabDetails();
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
							
																					
							document.Form1.txthiddenCallNo.value=col;
							document.Form1.txtTask.value=nn;
							document.Form1.txthiddenImage.value='Edit';
							document.Form1.txthiddenTable.value=tableID;
							document.Form1.txtComp.value=Comp;
						
							if (tableID=='cpnlTaskView_GrdAddSerach')
							{

							    window.parent.OpenCallDetailInTab('Call# ' + col, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=6&CallNumber=" + col + "&CompId=" + Comp, 'Call' + col, screenid);
								//Form1.submit(); 
								return false;
								//OpenTask(nn,col); 
							}
							else if(tableID=='cpnlTaskAction_grdAction')
							{
								//wopen('../../supportcenter/callview/Action_edit.aspx?ScrID=294&ACTIONNO='+nn,'Search001',500,450);
							}
							else
							{
							Form1.submit(); 
							return false;
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
				wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=536&TBLName='+varTable,'Search'+rand_no,500,450);
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
		
 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	
	
				
    </script>

    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BackColor="#8AAFE5"
                                                        BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px"
                                                        ImageUrl="white.GIF" CommandName="submit" AlternateText="."></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelHistoricView" runat="server" CssClass="TitleLabel" BorderStyle="None">HISTORIC VIEW</asp:Label>
                                                </td>
                                                <td style="width: 55%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:UpdatePanel ID="ee" runat="server">
                                                            <ContentTemplate>
                                                                <!--<asp:ImageButton id="imgSave" runat="server" ImageUrl="../../Images/S2Save01.gif" AccessKey="S"></asp:ImageButton>&nbsp;-->
                                                                <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../../Images/s2Add01.gif"
                                                                    ToolTip="New Call"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                                    ToolTip="Select a Task to Edit"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                                    ToolTip="Search"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgAttachments" AccessKey="T" runat="server" ImageUrl="../../Images/ScreenHunter_075.bmp"
                                                                    ToolTip="Select a Task to View Attachment"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgFWD" AccessKey="F" runat="server" ImageUrl="../../Images/Fwd.jpg"
                                                                    ToolTip="Select a Task to Forward"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server" ImageUrl="../../Images/CloseCall1.gif"
                                                                    ToolTip="View All Task"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                                    ToolTip="Select a Task to Delete Action"></asp:ImageButton>&nbsp;<img src="../../Images/reset_20.gif"
                                                                        title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                                <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                    style="cursor: hand;" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:UpdateProgress ID="progress" runat="server">
                                                        <ProgressTemplate>
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="../../Images/ajax1.gif" Width="24"
                                                                Height="24" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                                <td style="width: 15%">
                                                    <font face="Verdana" size="1"><strong>View&nbsp;
                                                        <asp:DropDownList ID="ddlstview" runat="server" Width="80px" AutoPostBack="false"
                                                            Font-Size="XX-Small" Font-Names="Verdana">
                                                        </asp:DropDownList>
                                                        &nbsp;<asp:ImageButton ID="imgBtnViewPopup" runat="server" ImageUrl="../../Images/plus.gif">
                                                        </asp:ImageButton></strong></font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('536','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">
                                        &nbsp;&nbsp;&nbsp;
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
                                        <td>
                                            <asp:UpdatePanel ID="upnlTaskView" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cc1:CollapsiblePanel ID="cpnlTaskView" runat="server" Width="100%" Height="294px"
                                                        BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test"
                                                        PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                                        Text="Task View" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                        Draggable="False">
                                                        <div id="divTaskView" style="overflow: auto; width: 1056px; height: 200pt">
                                                            <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                width="100%" align="left" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <table cellspacing="0" cellpadding="0" border="0">
                                                                            <tr>
                                                                                <td nowrap="nowrap">
                                                                                    <asp:Panel ID="pnl" runat="server" Width="0px">
                                                                                        <table cellspacing="0" cellpadding="0" border="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="CHKC" runat="server" Width="20px" BorderWidth="0" Font-Size="XX-Small"
                                                                                                        ToolTip="Comment Search"></asp:CheckBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="CHKA" runat="server" Width="20px" BorderWidth="0" Font-Size="XX-Small"
                                                                                                        ToolTip="Attachment Search"></asp:CheckBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="CHKF" runat="server" Width="20px" BorderStyle="None" BorderWidth="0"
                                                                                                        Font-Size="XX-Small" ToolTip="Form Search"></asp:CheckBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                                <td nowrap="nowrap">
                                                                                    <asp:Panel ID="Panel1" runat="server">
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="left">
                                                                        <!--  **********************************************************************-->
                                                                        <asp:DataGrid ID="GrdAddSerach" runat="server" BorderWidth="1px" BorderStyle="None"
                                                                            CssClass="Grid" Font-Names="Verdana" BorderColor="Silver" ForeColor="MidnightBlue"
                                                                            CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left" PageSize="25" DataKeyField="TaskNo"
                                                                            PagerStyle-Visible="False" AllowPaging="True" AllowSorting="True">
                                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                            <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="GridItem" BackColor="White">
                                                                            </ItemStyle>
                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                            </HeaderStyle>
                                                                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                            <Columns>
                                                                                <asp:TemplateColumn HeaderText="C">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgComm" runat="server" ImageUrl="../../Images/comment_Blank.gif">
                                                                                        </asp:Image>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="A">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgAtt" runat="server"></asp:Image>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="F">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgform" runat="server"></asp:Image>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <PagerStyle Visible="False" Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                        </asp:DataGrid><!-- Panel for displaying Task Info --><!-- Panel for displaying Action Info--><!-- ***********************************************************************-->
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <asp:Panel ID="Panel7" runat="server">
                                                            <table height="25">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="pg" Width="40px" Font-Names="Verdana" Font-Size="8pt" ForeColor="#0000C0"
                                                                            runat="server" Font-Bold="True">Page</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="CurrentPg" runat="server" Height="12px" Width="10px" Font-Size="X-Small"
                                                                            ForeColor="Crimson" Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="of" Font-Names="Verdana" Font-Size="8pt" ForeColor="#0000C0" runat="server"
                                                                            Font-Bold="True">of</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="TotalPages" runat="server" Height="12px" Width="10px" Font-Size="X-Small"
                                                                            ForeColor="Crimson" Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Firstbutton" runat="server" AlternateText="First" ImageUrl="../../Images/next9.jpg"
                                                                            ToolTip="First"></asp:ImageButton>
                                                                    </td>
                                                                    <td width="14">
                                                                        <asp:ImageButton ID="Prevbutton" runat="server" ImageUrl="../../Images/next99.jpg"
                                                                            ToolTip="Previous"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Nextbutton" runat="server" ImageUrl="../../Images/next9999.jpg"
                                                                            ToolTip="Next"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Lastbutton" runat="server" ImageUrl="../../Images/next999.jpg"
                                                                            ToolTip="Last"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPageSize" runat="server" Height="14px" Width="24px" Font-Size="7pt"
                                                                            MaxLength="3"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="Button3" runat="server" Height="16px" Width="16px" BorderStyle="None"
                                                                            ToolTip="Change Paging Size" Font-Size="7pt" Text=">" ForeColor="Navy" Font-Bold="True">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblrecords" runat="server" Height="12pt" Font-Names="Verdana" Font-Size="8pt"
                                                                            ForeColor="MediumBlue" Font-Bold="True">Total Records</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="TotalRecods" runat="server" Height="12pt" Font-Names="Verdana" Font-Size="8pt"
                                                                            ForeColor="Crimson" Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </cc1:CollapsiblePanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlCallTask" runat="server">
                                                <ContentTemplate>
                                                    <cc1:CollapsiblePanel ID="cpnlTaskAction" runat="server" Width="100%" Height="274px"
                                                        BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test"
                                                        PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                                        Text="Action View" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                        Draggable="False">
                                                        <div style="overflow: auto; width: 1056px">
                                                            <table style="border-collapse: collapse" width="100%" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
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
    <asp:UpdatePanel ID="upnlhidden" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
            <input type="hidden" name="txthidden" />
            <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues" />
            <input type="hidden" name="txthiddenImage" />
            <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
            <input type="hidden" value="<%=introwvalues%>" name="txtrowvaluesCall" />
            <input type="hidden" value="<%=strhiddenTable%>" name="txthiddenTable" />
            <input type="hidden" value="<%=mstrTaskNumber%>" name="txtTask" />
            <input type="hidden" value="<%=mstrComp%>" name="txtComp" id="txtComp" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
