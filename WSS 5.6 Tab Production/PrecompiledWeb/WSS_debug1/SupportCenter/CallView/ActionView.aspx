<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_ActionView, App_Web_mwk9lvv9" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Action View</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../../DateControl/ION.js"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../../Images/Js/ToDoListShortCuts.js"></script>

    <script>

			var globleID;
			
	var rand_no = Math.ceil(500*Math.random())
		
			function ShowActions()
			{
				wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=A', 'Attachments'+rand_no, 500, 450)
				return false;
			}					
		
			function KeyImage(a,b,c,d)
			{
		
				if (d==0 ) //if comment is clicked
				{		
					wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '&tbname=A'  ,'Comment'+rand_no,500,450);
				}
				else//if Attachment is clicked
				{
					wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ACTIONNO='+ a + '&ID=A','Attachment'+rand_no,690,450);
	
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
				return false;
				}
									
			function callrefresh()
				{
			//dalert("hello");
					//location.href="../../SupportCenter/CallView/Task_View.aspx";
					document.Form1.txthiddenImage.value='';
					Form1.submit();
					return false;
				}
								
			function ConfirmDelete(varImgValue)
				{
					
				
				
				
					//alert("Fun");
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
		
				
							
			function OpenUserInfo2(ADDNO)
			{
				var strscreen='329';
				wopen('UserInfo.aspx?ScrID=329&ADDNO='+ADDNO,'Search'+rand_no,350,500);
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
										
									location.href="../../SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=-1&PageID=3";
										//document.Form1.txthiddenImage.value=varImgValue;
										//Form1.submit();
											
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
						
						if (varImgValue=='Attach')
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
									wopen('../../SupportCenter/CallView/Task_Fwd.aspx','FWD'+rand_no,400,250);
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
				return false;
		}																		
				
				function KeyCheck(nn,col,rowvalues,tableID,Comp)
					{
						
						globleID = col;
						//alert(document.Form1.txtTask.value);
						document.Form1.txthiddenCallNo.value=col;
						document.Form1.txtTask.value=nn;
						document.Form1.txthiddenTable.value=tableID;
						document.Form1.txtrowvalues.value=rowvalues;
						document.Form1.txtComp.value=Comp;
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
								
								if (tableID=='cpnlTaskView_GrdAddSerach')
								{
									document.Form1.txthiddenImage.value='Select';
									setTimeout('Form1.submit();',700);
									//Form1.submit(); 
								}	
					}	
/*					
					function KeyCheck55(nn,col,rowvalues,tableID,Comp)
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
					
					
					
					
							document.Form1.txthiddenCallNo.value=col;
							document.Form1.txtTask.value=nn;
							document.Form1.txthiddenImage.value='Edit';
							document.Form1.txthiddenTable.value=tableID;
							document.Form1.txtComp.value=Comp;
							
								if (tableID=='cpnlTaskView_GrdAddSerach')
								{
									Form1.submit(); 
									//OpenTask(nn,col);
								}
								else if(tableID=='cpnlTaskAction_grdAction')
								{
									wopen('../../SupportCenter/CallView/Action_edit.aspx?ScrID=294&ACTIONNO='+nn,'Search',500,450);
								}
								else
								{
								Form1.submit(); 
								return false;
								}
						}	
	*/				
					
				function OpenTask(TASKNO,CALLNO)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('../../SupportCenter/CallView/Task_edit.aspx?ScrID=334&TASKNO='+TASKNO+'&CALLNO='+CALLNO,'Search'+rand_no,500,450);
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
				}
					
			function OpenVW(varTable)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrId=502&TBLName='+varTable,'Search'+rand_no,500,450);
				return false;
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

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
        border="0">
        <tr>
            <td style="width: 184px">
                <asp:Label ID="lblTitleLabelActionView" runat="server" Height="12px" Width="128px"
                    CssClass="TitleLabel">&nbsp;Action View</asp:Label>
            </td>
            <td style="width: 607px">
                <asp:Button ID="btnDefault" Height="18" Width="0" Text="Click" runat="server" BackColor="#8AAFE5"
                    BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img
                        title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ToolTip="Search" ImageUrl="../../Images/s1search02.gif"
                    AlternateText="Search"></asp:ImageButton>&nbsp;
                <asp:ImageButton Visible="true" ID="imgClose" AccessKey="L" TabIndex="21" runat="server"
                    ImageUrl="../../Images/s2close01.gif" ToolTip="Close"></asp:ImageButton>&nbsp;<img
                        title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">
            </td>
            <td>
                <font face="Verdana" size="1"><strong><font face="Verdana" size="1"><strong></strong>
                </font>&nbsp;</strong></font>&nbsp;
            </td>
            <td width="42" background="../../images/top_nav_back01.gif" height="67">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        border="0">
        <tr>
            <td valign="top" colspan="1">
                <!--  **********************************************************************-->
                <table style="border-collapse: collapse" width="100%" border="0">
                    <tr>
                        <td colspan="2">
                            <cc1:CollapsiblePanel ID="cpnlError" runat="server" Height="54px" Width="100%" BorderStyle="Solid"
                                BorderWidth="0px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                ExpandImage="../../Images/ToggleDown.gif" Text="Error Message" TitleBackColor="Transparent"
                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                Visible="False" BorderColor="Indigo">
                                <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td colspan="0" rowspan="0">
                                            <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="../../Images/warning.gif">
                                            </asp:Image>
                                        </td>
                                        <td colspan="0" rowspan="0">
                                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:Label>
                                            <asp:ListBox ID="lstError" runat="server" Width="752px" BorderWidth="0" BorderStyle="Groove"
                                                ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox>
                                        </td>
                                    </tr>
                                </table>
                            </cc1:CollapsiblePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:CollapsiblePanel ID="cpnlTaskAction" runat="server" Width="100%" BorderStyle="Solid"
                                BorderWidth="0px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                ExpandImage="../../Images/ToggleDown.gif" Text="Task View" TitleBackColor="Transparent"
                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                Visible="true" BorderColor="Indigo">
                                <div style="overflow: auto; width: 100%; height: 420px">
                                    <table style="border-collapse: collapse" width="100%" border="0">
                                        <tr>
                                            <td colspan="10">
                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </cc1:CollapsiblePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <input type="hidden" name="txthidden">
                        <input type="hidden" name="txthiddenImage">
                        <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo">
                        <input type="hidden" value="<%=strhiddenTable%>" name="txthiddenTable">
                        <input type="hidden" value="<%=mstrTaskNumber%>" name="txtTask">
                        <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues">
                        <input type="hidden" value="<%=mstrComp%>" name="txtComp">
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
