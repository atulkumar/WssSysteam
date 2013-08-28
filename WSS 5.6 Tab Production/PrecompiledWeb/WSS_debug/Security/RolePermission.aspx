<%@ page language="VB" enableeventvalidation="false" autoeventwireup="false" inherits="Security_RolePermission, App_Web_kkyzf0kd" maintainscrollpositiononpostback="true" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Role Permission</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <link href="../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../SupportCenter/calendar/popcalendar.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>

    <script type="text/javascript" src="../Images/Js/JSValidation.js"></script>

    <script type="text/javascript">
			
		//********************* ***********************************************
			
			var rand_no = Math.ceil(500*Math.random())
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
			var ddlMember=document.getElementById('cpnlCallView_DDLCompany');
			var mem=ddlMember.options(ddlMember.selectedIndex).value;
			var url= '../AJAX Server/AjaxInfo.aspx?Type=ROLEBYCOMP&CompID='+mem+'&Rnd='+Math.random();
			xmlHttp = GetXmlHttpObject(stateChangeHandler);    
			xmlHttp_Get(xmlHttp, url); 
			
		}
		 function FillHidden()
		 {
			var ddlRole=document.getElementById('cpnlCallView_DDLRole');
			document.getElementById('txtHIDRoleID').value=ddlRole.options(ddlRole.selectedIndex).value;
		 }
		function stateChangeHandler() 
		 { 	
				 document.getElementById("cpnlCallView_DDLRole").options.length=1;
												
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
									var RoleDataName='';
									var RoleDataID='';
									switch(intT)
									{
										case 0:
										{								
											
											for (var inti=0; inti<item.length; inti++)
											{
												
													var objNewOption = document.createElement("OPTION");
													document.getElementById("cpnlCallView_DDLRole").options.add(objNewOption);
													objNewOption.value = item[inti].getAttribute("COL0");
													objNewOption.innerText = item[inti].getAttribute("COL1");
													RoleDataName=RoleDataName+item[inti].getAttribute("COL1") + '^';
													RoleDataID=RoleDataID+item[inti].getAttribute("COL0") + '^';																
											}
											document.Form1.txtRoleData.value= RoleDataName + '~' + RoleDataID ;
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
	var globalid;
			var globalSkil;
			var globalAddNo;
			var globalGrid;
			var globaldbclick = 0;
			
						function checkedChanged(val)
						{
						alert(val);
						var num=id.indexOf('ctl')+3;
						var lineNum = id.substring(num, num+1);
						var valText= 'pnlGrid:dgSRO:_ctl' + lineNum + ':txtCOF';
//Collapsiblepanel1:dgControls:_ctl3:2
						}
						
		function CopyText()
		{
			//alert("kaf");
			document.getElementById('cpnlCallView_txtAliasName').innerText = document.getElementById('cpnlCallView_txtRollName').value;
			
		}				
						function CheckAll(checkAllBox, rdvalue, PanelID)
						{
													
							var frm=document.Form1;
							var ChkState=checkAllBox.checked;
							document.getElementById('cpnlCallView_txtCheckStatus').value=PanelID;
							for (i=0; i< frm.length; i++)
							{
								e= frm.elements[i];
									if (e.type=='radio' && e.value==rdvalue)
									{
										//alert("1" + e.disabled);

										if(e.disabled==false) 
											{
										//		alert("2" + e.disabled);
												e.checked=ChkState;
											}
									}
							}
							document.Form1.submit();
							return false;
						}
						
						
			function OpenW(a,b,c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
					return false;
				}
				
			function OpenAB(c)
				{
					//alert("abc");
				
					wopen('../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as CompanyName,CI_VC16_Alias as AliasName  from T010011 WHERE CI_VC8_Address_Book_Type = '+ "'COM'" +' &tbname=' + c ,'Search11'+rand_no,500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,ab.CI_VC36_Name as Name,UL_VC8_Role_PK as Role from T010011 ab,T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'Search',500,450);
					document.getElementById('cpnlCallView_txtCRole').innerText="";
					document.getElementById('cpnlCallView_txtCRoleName').innerText="";
					return false;
				}
				
			function OpenRole(c)
				{
					var confirmed
					var abCompId =  document.getElementById('cpnlCallView_txtCompanyID').value;
					confirmed=window.confirm("Are you sure to copy already existing role ?");
					if(confirmed==false)
					{
						//alert("False");
						return false;
					}
					else
					{
						if ((abCompId == "") || (abCompId == 'undefined'))
						{
							abCompId = 0;
						}
						else
						{
						}
						wopen('../Search/Common/PopSearch.aspx?ID=select ROM_IN4_Role_ID_PK as ID,ROM_VC50_Role_Name as RoleName,ROM_IN4_Company_ID_FK as CompanyID from T070031 where ROM_IN4_Company_ID_FK = 0 or ROM_IN4_Company_ID_FK = ' + abCompId + ' &tbname=' + c ,'Search11'+rand_no,500,450);
						return false;
					}
				}

			function OpenBU(c)
				{
					//alert("abc");
				
					wopen('../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as BU_Name,CI_VC16_Alias as AliasName  from T010011 WHERE CI_VC8_Address_Book_Type = '+ "'BU'" +' &tbname=' + c ,'Search11'+rand_no,500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,ab.CI_VC36_Name as Name,UL_VC8_Role_PK as Role from T010011 ab,T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'Search',500,450);
					return false;
				}	
							
			function OpenW_Add_Address(param)
				{
					wopen('AB_Additional.aspx?ID='+param,'Additional_Address'+rand_no,400,450);
				}
			
			function 	OpenAttach()
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
					window.open("Udc_Home_Search.aspx","ss","scrollBars=no,resizable=No,width=400,height=450,status=yes");
				}
			
			function addToParentList(Afilename,TbName,strname)
				{				
					if (Afilename != "" || Afilename != 'undefined')
						{
							varName = TbName + 'Name';
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
				
																				
			function KeyCheck(aa,cc,dd)
				{
				
					globalid = cc;
					globalSkil  = aa;
					//globalAddNo = bb;
					globalGrid = dd;
					
					//alert(aa);
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
				
					document.Form1.txthiddenImage.value='Edit';
					document.Form1.txthiddenSkil.value=aa;
					document.Form1.txthidden.value=bb;	
					//document.Form1.txthiddenGrid.value=dd;	
						SaveEdit('Edit');  
				}
							
			function SaveEdit(varImgValue)
				{
				//alert('savedit');
			    	if (varImgValue=='Edit')
					{
												//Security Block
							var obj=document.getElementById("imgEdit");
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
							
						if(document.getElementById("cpnlCallView_txtRollName").value=="")
						{
							alert("Enter Role Name");
							}
						else
						{
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
								}										
						}
					}	
											
					if (varImgValue=='Close')
						{
//							document.Form1.txthiddenImage.value=varImgValue;
//							Form1.submit(); 
//							return false;
                            window.close();
						}								
							
					if (varImgValue=='Add')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
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
							var obj=document.getElementById("imgSave");
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
							
							if(document.getElementById("cpnlCallView_txtRollName").value=="")
							{
								alert("Enter Role Name");
								//alert(document.getElementById("cpnlCallView_dtEndDate").value);
								return false;
							}

							else
							{
								//alert(varImgValue);
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
									return false;
														
							}
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
					
							if(document.getElementById("cpnlCallView_txtRollName").value=="")
							{
							alert("Enter Role Name");
							return false;
							}
							else
							{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							return false;
							}
						}		
						
					if (varImgValue=='Attach')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
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
									Form1.reset();
								}		
							return false;
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
							wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+b ,'Attachment'+rand_no,500,450);
						}
				}
				
					
			function OpenVW(varTable)
				{
					wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ID='+varTable,'Search'+rand_no,500,450);
				}
				
		
		  //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";
              var y = document.getElementById('cpnlCallTask_collapsible').cells[0].colSpan = "1";
                var z = document.getElementById('cpnlTaskAction_collapsible').cells[0].colSpan = "1";
                  var m = document.getElementById('Collapsiblepanel1_collapsible').cells[0].colSpan = "1";
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
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="0px" BorderWidth="0px"
                                                        Width="0px" ImageUrl="white.GIF" AlternateText="." CommandName="submit"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelRolepermission" runat="server" BorderWidth="2px" BorderStyle="None"
                                                        CssClass="TitleLabel">Role Permission</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" Visible="false" ImageUrl="../Images/s1ok02.gif"
                                                            ToolTip="Ok"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                         <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                           style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('293','../');"
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
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td valign="top" colspan="1">
                                                <!-- **********************************************************************-->
                                                <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" Width="100%" BorderWidth="1px"
                                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                    TitleBackColor="Transparent" Text="Role" ExpandImage="../images/ToggleDown.gif"
                                                    CollapseImage="../images/ToggleUp.gif" Draggable="False" Visible="true">
                                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td valign="top" align="left" width="100%">
                                                                <table width="100%" bgcolor="#f5f5f5" border="0">
                                                                    <tr>
                                                                        <td style="width: 243px; height: 35px">
                                                                            <asp:Label ID="Label4" runat="server" Width="64px" CssClass="FieldLabel">Role Name</asp:Label><br>
                                                                            <asp:TextBox ID="txtRollName" runat="server" Width="150" Height="14px" CssClass="txtNoFocus"
                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                MaxLength="50"></asp:TextBox>
                                                                        </td>
                                                                        <td style="width: 241px; height: 35px">
                                                                            <asp:Label ID="lblMiddleName8" runat="server" Width="72px" Height="12px" CssClass="FieldLabel">Alias Name</asp:Label><br>
                                                                            <asp:TextBox ID="txtAliasName" runat="server" Width="150" Height="14px" CssClass="txtNoFocus"
                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                MaxLength="50"></asp:TextBox>
                                                                        </td>
                                                                        <td style="height: 35px">
                                                                            <asp:Label ID="lblMiddleName2" runat="server" CssClass="FieldLabel" Font-Bold="True">Company ID</asp:Label><br>
                                                                            <asp:DropDownList ID="DDLCompany" runat="server" Width="140px" Font-Size="XX-Small"
                                                                                Font-Name="Verdana" Height="20px">
                                                                                <asp:ListItem></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 243px">
                                                                            <asp:Label ID="Label1" runat="server" Height="12px" CssClass="FieldLabel">Start Date</asp:Label><br>
                                                                            <ION:Customcalendar ID="dtStartDate" runat="server" Width="148px" Height="16px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">End Date</asp:Label><br>
                                                                            <ION:Customcalendar ID="dtEndDate" runat="server" Width="148px" Height="16px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblMiddleName4" runat="server" CssClass="FieldLabel">Description</asp:Label><br>
                                                                            <asp:TextBox ID="txtDescription" runat="server" Width="300px" Height="14px" CssClass="txtNoFocus"
                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                MaxLength="50"></asp:TextBox>
                                                                            <asp:TextBox ID="txtBusinessUnit" TabIndex="2" runat="server" Width="0px" CssClass="txtBU"
                                                                                BorderStyle="Solid" BorderWidth="1px" Visible="False" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                MaxLength="8" ReadOnly="True"></asp:TextBox>
                                                                            <img class="PlusImageCSS" id="pb1" onclick="OpenBU('cpnlCallView_txtBusinessUnit');"
                                                                                alt="Call Type" width="0" border="0" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 243px">
                                                                            <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">Status Code</asp:Label><br />
                                                                            <asp:DropDownList ID="cboEnabeDisable" runat="server" Width="150" Height="20px" CssClass="txtNoFocus">
                                                                                <asp:ListItem Value="ENB">Enable</asp:ListItem>
                                                                                <asp:ListItem Value="DSB">Disable</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td style="width: 241px">
                                                                            <asp:Label ID="Label5" runat="server" CssClass="FieldLabel" Font-Bold="True">Status Date</asp:Label><br>
                                                                            <ION:Customcalendar ID="dtStatusDate" runat="server" Width="148px" Height="16px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label6" runat="server" CssClass="FieldLabel">Copy Role</asp:Label><br />
                                                                            <asp:DropDownList ID="DDLRole" runat="server" Width="140px" CssClass="txtNoFocus"
                                                                                Height="20px">
                                                                                <asp:ListItem Value=""></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <%--<asp:TextBox id="txtParentRole" Width="10" Height="0" Runat="server"></asp:TextBox>--%>
                                                                            <input type="hidden" id="txtParentRole" runat="server" />
                                                                            <input type="hidden" id="txtCRole" runat="server" />
                                                                            &nbsp &nbsp
                                                                            <asp:CheckBox ID="chkIsAdminRights" runat="server" Text="Is Admin Rights" CssClass="FieldLabel" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <%--<asp:TextBox id="txtCheckStatus" runat="server" Width="10px" Height="10px"></asp:TextBox>--%>
                                                                <input type="hidden" id="txtCheckStatus" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc1:CollapsiblePanel>
                                                <!-- **********************************************************************-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Width="100%" BorderWidth="0px"
                                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                    TitleBackColor="Transparent" Text="Menues" ExpandImage="../images/ToggleDown.gif"
                                                    CollapseImage="../images/ToggleUp.gif" Draggable="False" Visible="true" BorderColor="Indigo">
                                                    <div style="overflow: auto; width: 100%; height: 250px">
                                                        <table style="border-collapse: collapse" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="10">
                                                                    <asp:DataGrid ID="dgMenu" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        OnItemDataBound="myItems_ItemDataBound">
                                                                        <SelectedItemStyle BackColor="LightGray"></SelectedItemStyle>
                                                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:ButtonColumn Visible="False" CommandName="select"></asp:ButtonColumn>
                                                                            <asp:BoundColumn Visible="False" DataField="OBM_IN4_Object_ID_PK"></asp:BoundColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <b>Object Type</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="imgMenu" runat="server" ImageUrl='<%# container.dataitem("OBM_VC200_Image") %>'>
                                                                                    </asp:Image>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <b>Object Name</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemType" runat="server" Width="150px" ForeColor="Black" Height="18px"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" Text='<%# container.dataitem("OBM_VC50_Object_Name")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <b>Alias Name</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtAliasMenu" MaxLength="15" Height="18px" Width="250px" Font-Size="XX-Small"
                                                                                        runat="server" Text='<%# container.dataitem("OBM_VC50_Alias_Name")%>'>
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:RadioButton ID="chkAllView" Height="18px" GroupName='10' runat="server" onclick="CheckAll(this,'rdView','1');">
                                                                                    </asp:RadioButton><b>View </b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdView" Height="18px" Width="80px" runat="server" GroupName='1'
                                                                                        AutoPostBack="False"></asp:RadioButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:RadioButton ID="chkAllHide" Height="18px" runat="server" GroupName='10' onclick="CheckAll(this,'rdHide','1');">
                                                                                    </asp:RadioButton><b>Hide</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdHide" Height="18px" Width="80px" runat="server" GroupName='1'
                                                                                        AutoPostBack="False"></asp:RadioButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                        </Columns>
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlTaskAction" runat="server" Width="100%" BorderWidth="0px"
                                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                    TitleBackColor="Transparent" Text="Screens" ExpandImage="../images/ToggleDown.gif"
                                                    CollapseImage="../images/ToggleUp.gif" Draggable="False" Visible="true" BorderColor="Indigo">
                                                    <div style="overflow: auto; width: 100%; height: 250px">
                                                        <table style="border-collapse: collapse" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="10">
                                                                    <asp:DataGrid ID="dgScreen" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        OnItemDataBound="myItems_ItemDataBound">
                                                                        <SelectedItemStyle BackColor="lightgray"></SelectedItemStyle>
                                                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:ButtonColumn Visible="False" CommandName="select"></asp:ButtonColumn>
                                                                            <asp:BoundColumn DataField="OBM_IN4_Object_ID_PK" Visible="False"></asp:BoundColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <b>Object Type</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="imgSCR" runat="server" ImageUrl='<%# container.dataitem("OBM_VC200_Image") %>'>
                                                                                    </asp:Image>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <b>Object Name</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Label7" runat="server" Width="150px" Height="18px" ForeColor="Black"
                                                                                        Font-Names="Verdana" Font-Size="XX-Small" Text='<%# container.dataitem("OBM_VC50_Object_Name")%>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <b>Alias Name</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtAliasScreen" Width="250px" MaxLength="15" Height="18px" Font-Size="XX-Small"
                                                                                        runat="server" Text='<%# container.dataitem("OBM_VC50_Alias_Name")%>'>
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:RadioButton ID="chkAllView1" Height="18px" GroupName='15' runat="server" onclick="CheckAll(this,'rdView1','2');">
                                                                                    </asp:RadioButton><b>View </b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdView1" Height="18px" Width="80px" runat="server" GroupName='1'
                                                                                        AutoPostBack="False"></asp:RadioButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:RadioButton ID="chkAllHide1" Height="18px" runat="server" GroupName='15' onclick="CheckAll(this,'rdHide1','2');">
                                                                                    </asp:RadioButton><b>Hide</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdHide1" Height="18px" Width="80px" runat="server" GroupName='1'
                                                                                        AutoPostBack="False"></asp:RadioButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                        </Columns>
                                                                    </asp:DataGrid>
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
                                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                    TitleBackColor="Transparent" Text="Controls" ExpandImage="../images/ToggleDown.gif"
                                                    CollapseImage="../images/ToggleUp.gif" Draggable="False" Visible="true" BorderColor="Indigo">
                                                    <div style="overflow: auto; width: 100%; height: 250px">
                                                        <table style="border-collapse: collapse" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="10">
                                                                    <asp:DataGrid ID="dgControls" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        OnItemDataBound="myItems_ItemDataBound">
                                                                        <SelectedItemStyle></SelectedItemStyle>
                                                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:ButtonColumn Visible="False" CommandName="select"></asp:ButtonColumn>
                                                                            <asp:BoundColumn DataField="OBM_IN4_Object_ID_PK" Visible="False"></asp:BoundColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <b>Object Type</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="imgObjType" runat="server" ImageUrl='<%# container.dataitem("OBM_VC200_Image") %>'>
                                                                                    </asp:Image>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <b>Alias Name</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtAliasControl" MaxLength="15" Width="250px" Font-Size="XX-Small"
                                                                                        Height="18px" runat="server" Text='<%# container.dataitem("OBM_VC50_Alias_Name")%>'>
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:RadioButton ID="chkAllView2" Height="18px" GroupName='11' onclick="CheckAll(this,'rdView2','3');"
                                                                                        runat="server"></asp:RadioButton><b>View </b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdView2" Height="18px" Width="50px" runat="server" GroupName='1'
                                                                                        AutoPostBack="False"></asp:RadioButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:RadioButton ID="chkAllHide2" Height="18px" GroupName='11' onclick="CheckAll(this,'rdHide2','3');"
                                                                                        runat="server"></asp:RadioButton><b>Hide</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdHide2" Height="18px" Width="50px" runat="server" GroupName='1'
                                                                                        AutoPostBack="False"></asp:RadioButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:RadioButton ID="chkAllEnable" Height="18px" GroupName='12' runat="server" onclick="CheckAll(this,'rdEnable','3');">
                                                                                    </asp:RadioButton><b>Enable</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdEnable" Height="18px" Width="50px" runat="server" GroupName='2'
                                                                                        AutoPostBack="false"></asp:RadioButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:RadioButton ID="chkAllDisable" Height="18px" GroupName='12' runat="server" onclick="CheckAll(this,'rdDisable','3');">
                                                                                    </asp:RadioButton><b>Disable</b>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdDisable" Height="18px" Width="50px" runat="server" GroupName='2'
                                                                                        AutoPostBack="False"></asp:RadioButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                        </Columns>
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </cc1:CollapsiblePanel>
                                                <!-- Panel for displaying Task Info -->
                                                <!-- Panel for displaying Action Info-->
                                                <!-- ***********************************************************************-->
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
                        <asp:ListBox ID="lstError" runat="server" Width="635px" Height="40px" BorderStyle="Groove"
                            BorderWidth="0" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false">
                        </asp:ListBox>
                        <input type="hidden" name="txthiddenImage" />
                        <input id="txtHIDRoleID" type="hidden" name="txthiddenRoleID" runat="server" />
                        <input type="hidden" name="txtRoleData" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
