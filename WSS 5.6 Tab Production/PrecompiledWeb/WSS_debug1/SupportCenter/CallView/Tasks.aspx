<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_Tasks, App_Web_brwmhxgd" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tasks</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/CallViewShortCuts.js" type="text/javascript"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
                                  
		var globleID;			
        var rand_no = Math.ceil(500*Math.random())			
		
				function KeyImage(a,b,c,d,comp,CallNo)
				{		
					if (d==0 ) //if comment is clicked				
					{
						wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '&tbname=' + c + '&CallNo=' + CallNo +'&CompID=' + comp,'Comment'+rand_no,500,450);
					}
					else if (d==1) //if Attachment is clicked
					{
					
						wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+a +'&CompID=' + comp,'Attachment'+rand_no,700,240);
					}
					else if (d==5)//call comment
					{
						wopen('../../SupportCenter/Callview/comment.aspx?ScrID=464&ID='+ a + '&tbname=C&CallNo='+CallNo+'&comp='+ comp  +'&CompID=' + comp,'Comment'+rand_no,500,450);
					}
					else if (d==7)
					{						
						wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo='+a+'&CompID='+comp+'&CallNo='+CallNo +'&CompID=' + comp,'Attachment'+rand_no,700,240);
					}
					else // if Attach form is clicked
					{
						wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno='+ CallNo +'&tno='+a +'&CompID=' + comp,'AttachForms'+rand_no,500,450);												
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
						
						wopen('Action_edit.aspx?ScrID=294&ACTIONNO='+nn,'Search'+rand_no,430,300);
					}
					else
						{
							Form1.submit(); 
						}													
				}					
			function OpenW(a,b,c)
				{
				wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
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
					wopen('../../Search/Common/PopSearch.aspx?ID='+strQuery+'&tbname=' + c ,'Search'+rand_no,500,450);
				}
									
			function callrefresh()
				{
				//alert("hello");
					location.href="../../SupportCenter/CallView/Call_View.aspx?ScrID=4";
					//Form1.submit();
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

//															if (document.Form1.txthiddenCallNo.value==0 && document.Form1.txthiddenTaskNo.value=='')
//															{
//																alert("Please select the row");
//															}
//															else
//															{
//																	if ( document.Form1.txthiddenTable.value=='cpnlCallTask_dtgTask' )
//																	{
//																				var TASKNO=document.Form1.txthiddenTaskNo.value;
//																				if ( '<%=session("PropCallStatus")%>'=='CLOSED')
//																				{	alert('You cannot edit task for a Closed Call');}
//																				else
//																				{//wopen('Task_edit.aspx?ScrID=334&TASKNO='+TASKNO,'Search'+rand_no,440,470);
//																				}
//																	}
//																	else
//																	{
//																			//document.Form1.txthiddenImage.value=varImgValue;
//																			//Form1.submit(); 
//																	}
//															
//															}
															
															return false;
									}	
												
								if (varImgValue=='Close')
								{
											window.close();	
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
																Form1.reset();
																
														}		

									}			
				}				
				
			

				function KeyCheck(nn,rowvalues,rowvaluescall,tableID,Comp,SuppComp)
					{
				//	alert(SuppComp);
						globleID = nn;
						
							
						
						document.Form1.txthidden.value=nn;
						document.Form1.txthiddenTable.value=tableID;
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
						//	alert(SuppComp);
									document.Form1.txthiddensuppcomp.value=SuppComp;	
							document.Form1.txthiddenImage.value='Select';
							
							setTimeout('Form1.submit();',100);
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
								wopen('UserInfo.aspx?ScrID=334&ADDNO='+TaskOwnerID+'&CALLOWNER='+CallOwner,'Search'+rand_no,350,500);
							//	OpenUserInfo(TaskOwnerID);
							}
							else
							{
							//alert();
							wopen('UserInfo.aspx?ScrID=334&CALLNO='+CallNo+'&CALLOWNER='+CallOwner+'&COMP='+Comp+'&ScreenID='+strscreen,'Search'+rand_no,350,500);
						//	OpenUserInfo(CallNo,CallOwner,Comp,strscreen);
							//Form1.submit(); 
							return false;
							}
				
							
					}	
				
				
				function OpenUserInfo(varTable,CallOwner,Comp,strscreen)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
				wopen('UserInfo.aspx?ScrID=334&CALLNO='+varTable+'&CALLOWNER='+CallOwner+'&COMP='+Comp+'&ScreenID='+strscreen,'Search'+rand_no,350,500);
				//wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&TaskNo='+a+'&CompID='+comp+'&CallNo='+CallNo ,'Attachment',800,550);
				
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
				}
				
				
					
					function KeyCheck55(nn,rowvalues,tableID,Comp)
					{
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
						var CS='<%=session("PropCallStatus")%>';
						if ( CS=='CLOSED')
						{
								alert('Task cannot be edited for a Closed Call');
						}
						else
						{
								//wopen('Task_edit.aspx?ScrID=334&TASKNO='+varTable,'Search'+rand_no,460,500);
						}
				}
					
			function OpenVW(varTable)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
				wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=463&TBLName='+varTable,'Search'+rand_no,450,500);
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

    <script type="text/javascript">
        //A Function to call on Page Load to set grid width according to screen size
        function onLoad() {
            var divCallView = document.getElementById('divCallView');
            divCallView.style.width = document.body.clientWidth - 30 + "px";
        }
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlGrdView_collapsible').cells[0].colSpan = "1";
        }
        //A Function is Called when we resize window
        window.onresize = onLoad;     
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
                                                <td>
                                                    <div style="width: 130px">
                                                        <asp:Button ID="BtnGrdSearch" runat="server" Height="0" Width="0px" BackColor="#8AAFE5"
                                                            BorderColor="#8AAFE5" BorderStyle="None"></asp:Button><asp:ImageButton ID="imgbtnSearch"
                                                                TabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="." CommandName="submit"
                                                                ImageUrl="white.GIF"></asp:ImageButton>&nbsp;
                                                        <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel"> TASK VIEW</asp:Label></div>
                                                </td>
                                                <td>
                                                    <div style="width: 400px">
                                                        <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;<asp:ImageButton
                                                            ID="imgSearch" AccessKey="H" runat="server" AlternateText="Search" ImageUrl="../../Images/s1search02.gif">
                                                        </asp:ImageButton>
                                                        <asp:ImageButton ID="ImgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                                                            ToolTip="Close"></asp:ImageButton>&nbsp;<img title="Seperator" alt="R" src="../../Images/00Seperator.gif"
                                                                border="0">&nbsp;
                                                    </div>
                                                </td>
                                                <td align="center">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="overflow: auto; width: 100%; height: 481px">
                    <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                        border="0">
                        <tr>
                            <td valign="top" colspan="1">
                                <!--  **********************************************************************-->
                                <div style="overflow: auto; width: 100%; height: 481px">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td colspan="2">
                                                    <cc1:CollapsiblePanel ID="cpnlError" runat="server" Height="47px" Width="100%" BorderColor="Indigo"
                                                        Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                        TitleBackColor="Transparent" Text="Error Message" ExpandImage="../../Images/ToggleDown.gif"
                                                        CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderStyle="Solid"
                                                        BorderWidth="0px">
                                                        <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" width="100%"
                                                            border="0">
                                                            <tr>
                                                                <td colspan="0" rowspan="0">
                                                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="../../Images/warning.gif">
                                                                    </asp:Image>
                                                                </td>
                                                                <td colspan="0" rowspan="0">
                                                                    &nbsp;
                                                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Size="XX-Small" Font-Names="Verdana"></asp:Label>
                                                                    <asp:ListBox ID="lstError" runat="server" Width="722px" BorderWidth="0" BorderStyle="Groove"
                                                                        ForeColor="Red" Font-Size="XX-Small" Font-Names="Verdana"></asp:ListBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </cc1:CollapsiblePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="100%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Height="66px" Width="100%"
                                                        BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                        TitleClickable="True" TitleBackColor="Transparent" Text="Task View" ExpandImage="../../Images/ToggleDown.gif"
                                                        CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderStyle="Solid"
                                                        BorderWidth="0px">
                                                        <table style="border-collapse: collapse" width="825" border="0">
                                                            <tr align="left">
                                                                <td>
                                                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="pnlTask" Width="1px" runat="server">
                                                            <table style="border-collapse: collapse" cellspacing="0" cellpadding="0" align="left"
                                                                border="0">
                                                                <tr valign="top">
                                                                    <td>
                                                                        <asp:Image ID="ImgHidTask" Width="165px" Height="18px" ImageUrl="../../Images/divider.gif"
                                                                            runat="server"></asp:Image>
                                                                        <asp:TextBox ID="TxtTaskNo_F" Height="18px" BorderWidth="1px" BorderStyle="Solid"
                                                                            Visible="False" Font-Size="XX-Small" Font-Names="Verdana" runat="server" Enabled="False"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td align="center">
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <!--<TD>
																							<asp:TextBox id="TxtProject_F" runat="server" width="41px" Height="18px" BorderStyle="Solid"
																								BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocusFE"></asp:TextBox></TD>
																						<TD>
																							<uc1:CustomDDL id="CDDLAgmt_F" runat="server" width="50px" readonly="true"></uc1:CustomDDL></TD>-->
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
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues" />
                        <input type="hidden" value="<%=introwvalues%>" name="txtrowvaluescall" />
                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                        <input type="hidden" name="txthiddenTaskNo" />
                        <input type="hidden" value="<%=strhiddenTable%>" name="txthiddenTable" />
                        <input type="hidden" value="<%=mstrComp%>" name="txtComp" />
                        <input type="hidden" name="txtByWhom" />
                        <input type="hidden" name="txtHIDSize" />
                        <input type="hidden" value="<%=mstrsuppcomp%>" name="txthiddensuppcomp" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
