<%@ page language="VB" enableeventvalidation="false" autoeventwireup="false" inherits="AdministrationCenter_Project_ProjectMasterDetail, App_Web_q7pwq1cm" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SubCategory Detail</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <%--	<LINK href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">--%>

    <script language="javascript" src="../../images/Js/JSValidation.js" type="text/javascript"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative; ;TOP:expression(this.offsetParent.scrollTop);BACKGROUND-COLOR:#e0e0e0}</style>

    <script language="Javascript" type="text/javascript"> 
	var rand_no = Math.ceil(500*Math.random())	
					
		function CheckLength()
		{
				var TDLength=document.getElementById('cpnlPRJ_txtComment').value.length;
				if ( TDLength>0 )
				{
					if ( TDLength > 200 )
					{
						alert('The SubCategory Description cannot be more than 200 characters\n (Current Length :'+TDLength+')');
						return false;
					}
				}
				return true;
		}		
		
		//********************************************************************
		
			var gtype;
		var xmlHttp; 
		var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
		var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
		var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
		//netscape, safari, mozilla behave the same??? 
		var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 

		function DoSubmit()
		{
			if (event.keyCode==13 )
			document.Form1.submit();

		}
		function OpenUserInfo2(ADDNO)
		{
			var strscreen='0';
			wopen('../../SupportCenter/Callview/UserInfo.aspx?ScrID=0&ADDNO='+ADDNO,'Search'+rand_no,350,500);
			return false;
		}
			
		function SetDDL()
		{
		/*	var ddlRole=document.getElementById('cpnlMember_DDLRole_F');
			document.Form1.txthiddenRole.value=ddlRole.options(ddlRole.selectedIndex).value;
			var ddlReport=document.getElementById('cpnlMember_DDLReportsTo_F');
			document.Form1.txthiddenReportsTo.value=ddlReport.options(ddlReport.selectedIndex).value;*/
		}
		
		function MemberChange()
		{
			xmlHttp=null;
			var ddlMember=document.getElementById('cpnlMember_ddlMember_F');
			var mem=ddlMember.options(ddlMember.selectedIndex).value;
			
			var ddlCompany=document.getElementById('cpnlPRJ_ddlCustomer');
			var CompID=ddlCompany.options(ddlCompany.selectedIndex).value;
			
			var url= '../../AJAX Server/AjaxInfo.aspx?Type=ROLE_AND_MEMBER&CompID='+ CompID +'&MemberID='+mem+'&Rnd='+Math.random();
			xmlHttp = GetXmlHttpObject(stateChangeHandler);    
			xmlHttp_Get(xmlHttp, url); 
		}
		 
		function stateChangeHandler() 
		 { 	
				 document.getElementById("cpnlMember_DDLRole_F").options.length=1;
				 document.getElementById("cpnlMember_DDLReportsTo_F").options.length=1;
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
									var DataName='';
									var DataID='';
									switch(intT)
									{
										case 0:
										{								
											
											for (var inti=0; inti<item.length; inti++)
											{
													var objNewOption = document.createElement("OPTION");
													document.getElementById("cpnlMember_DDLRole_F").options.add(objNewOption);
													objNewOption.value = item[inti].getAttribute("COL0");
													objNewOption.innerText = item[inti].getAttribute("COL1");
													DataName=DataName+item[inti].getAttribute("COL1") + '^';
													DataID=DataID+item[inti].getAttribute("COL0") + '^';															
											}
											document.Form1.txthiddenRole.value= DataName + '~' + DataID ;
											break;
										}//case 0
										case 1:
										{
											for (var inti=0; inti<item.length; inti++)
											{
													var objNewOption = document.createElement("OPTION");
													document.getElementById("cpnlMember_DDLReportsTo_F").options.add(objNewOption);
													objNewOption.value = item[inti].getAttribute("COL0");
													objNewOption.innerText = item[inti].getAttribute("COL1");
													DataName=DataName+item[inti].getAttribute("COL1") + '^';
													DataID=DataID+item[inti].getAttribute("COL0") + '^';														
											}
											document.Form1.txthiddenReportsTo.value= DataName + '~' + DataID ;
											break;
										}//case 1
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
						
			function OpenW(a,b,c)
				{
					wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
					return false;
				}
				
			function OpenAB(c)
				{
						wopen('../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as CustomerName,CI_VC16_Alias as AliasName from T010011 where CI_VC8_Address_Book_Type='+"'COM'"+ '  &tbname=' + c ,'Search'+rand_no,500,450);
					return false;
				}
				
			function OpenABPer(c)
				{
				var cusComp=document.getElementById('cpnlAGH_txtCusNo').value;
				var sComp='<%=session("propCompanyID")%>';
				
					wopen('../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as UserName,CI_IN4_Business_Relation as Company from T010011 where CI_IN4_Business_Relation in('+"'"+cusComp+"','"+sComp+"')" + '  &tbname=' + c ,'Search'+rand_no,500,450);
					return false;
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
						'status=no, toolbar=no, scrollbars=no, resizable=no');
					// Just in case width and height are ignored
					win.resizeTo(w, h);
					// Just in case left and top are ignored
					win.moveTo(wleft, wtop);
					win.focus();
				}

			function OpenWUdc_Search()
				{
					window.open("Udc_Home_Search.aspx","ss","scrollBars=no,resizable=No,width=350,height=450,status=yes");
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

																							
			function KeyCheck(nn,rowvalues)
					{
						//alert(nn);
						//alert(rowvalues);
						globleID = nn;
						
						document.Form1.txthidden.value=nn;
			
						
										var tableID='cpnlMember_dtgMember'  //your datagrids id
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
					
			function saveclicked()
			{
				if ( CheckLength()==true )
				{
					document.getElementById('txthiddensaveclicked').value='save';
				}
				else
				{
					return false;
				}
			}
				
			function KeyCheck55(aa,bb,cc)
				{	
				//alert(aa);
				globleID = aa;
				wopen('ProjectMemEdit.aspx?MemberSNO='+aa+'&ProjectID='+ bb +'&CompanyID='+cc,'Search'+rand_no,400,300);
				document.getElementById('txthidden').value=aa;
				return false;	
				}
							
			function SaveEdit(varImgValue,projID,compID)
				{
				//alert(varImgValue);
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
									return false;
								}
							else
								{
									wopen('ProjectMemEdit.aspx?MemberSNO='+globleID+'&ProjectID='+ projID +'&CompanyID='+compID,'Search'+rand_no,400,300);
									//document.getElementById('txthidden').value=aa;
									return false;	
								}										
						}	
											
					if (varImgValue=='Close')
						{
							//href="ProjectMemEdit.aspx"
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
						{alert();
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
									Form1.reset()
									return false;
								}		
								else
								{
								return false;
								}
						}			
					}			
					
				function ConfirmDelete(varImgValue)
					{		
									
							if (document.getElementById('txthidden').value==0)
								{
									alert("Please select the row");
									return false;
								}
								else
								{
									var confirmed
									confirmed=window.confirm('Are you Sure you want to Delete the Selected Member');
									if(confirmed==true)
											{
										
											  //  document.Form1.txthiddenImage.value=globalID;
											}
											else
											{
												return false;
											}	
								}
				}
				
			function ForcedPostBack()
				{
					document.Form1.txthiddenImage.value="forced";
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
    //A Function to improve design i.e delete the extra cell of table
    function onEnd() {
        var x = document.getElementById('cpnlPRJ_collapsible').cells[0].colSpan = "1";
        var y = document.getElementById('cpnlMember_collapsible').cells[0].colSpan = "1";
        var z = document.getElementById('cpnlOverview_collapsible').cells[0].colSpan = "1";
        
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
    <table height="50%" cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="0px" Height="0px"
                                                        ImageUrl="white.GIF" CommandName="submit" AlternateText="."></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelPrjDetail" runat="server" BorderStyle="None" BorderWidth="2px"
                                                        Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="#FFFFFF">SubCategory Detail</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" Visible="false" ImageUrl="../../images/s1ok02.gif"
                                                            ToolTip="Ok"></asp:ImageButton><asp:ImageButton ID="imgEdit" AccessKey="E"
                                                                runat="server" ImageUrl="../../Images/S2edit01.gif" ToolTip="Edit"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../images/s1search02.gif"
                                                            ToolTip="Search"></asp:ImageButton>
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
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('670','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td valign="top" colspan="1">
                                            <!--  **********************************************************************-->
                                            <div style="overflow: auto; width: 100%; height: 581px">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="top" colspan="1">
                                                            <cc1:CollapsiblePanel ID="cpnlPRJ" runat="server" Width="100%" BorderStyle="Solid"
                                                                BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                                                TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="SubCategory Header"
                                                                ExpandImage="../../images/ToggleDown.gif" CollapseImage="../../images/ToggleUp.gif"
                                                                Draggable="False">
                                                                <table cellspacing="0" cellpadding="0" width="780" bgcolor="#f5f5f5" border="0">
                                                                    <tr>
                                                                        <td style="width: 218px" valign="top">
                                                                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Customer*</asp:Label><br>
                                                                            <asp:DropDownList ID="ddlCustomer" runat="server" Width="120px" Font-Size="XX-Small"
                                                                                AutoPostBack="True">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td style="width: 203px" valign="top">
                                                                            <asp:Label ID="lblMiddleName2" runat="server" Width="127px" Font-Bold="True" Font-Names="Verdana"
                                                                                Font-Size="XX-Small">SubCategory Name*</asp:Label><br>
                                                                            <asp:TextBox ID="txtProjectName" runat="server" Width="120px" Font-Names="Verdana"
                                                                                Font-Size="XX-Small" BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus"
                                                                                MaxLength="20"></asp:TextBox>
                                                                        </td>
                                                                        <td style="width: 212px" valign="top">
                                                                            <asp:Label ID="lblMiddleName" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                                Font-Size="XX-Small">Parent SubCategory</asp:Label><br>
                                                                            <uc1:CustomDDL ID="cddlParentProject" runat="server" Width="120px"></uc1:CustomDDL>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:Label ID="Label3" runat="server" Width="272px" Font-Bold="True" Font-Names="Verdana"
                                                                                Font-Size="XX-Small">SubCategory Owner*</asp:Label><br>
                                                                            <uc1:CustomDDL ID="cddlProjectOwner" runat="server" Width="254px"></uc1:CustomDDL>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 218px" valign="top">
                                                                            <asp:Label ID="lblMiddleName10" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                                Font-Size="XX-Small">Status*</asp:Label><br>
                                                                            <uc1:CustomDDL ID="cddlStatus" runat="server" Width="120px"></uc1:CustomDDL>
                                                                        </td>
                                                                        <td style="width: 203px" valign="top">
                                                                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Start Date*</asp:Label><br>
                                                                            <ION:Customcalendar ID="dtProjectStartDate" runat="server" Width="120px" Height="18px">
                                                                            </ION:Customcalendar>
                                                                        </td>
                                                                        <td style="width: 212px" valign="top">
                                                                            <asp:Label ID="lblMiddleName8" runat="server" Width="72px" Font-Bold="True" Font-Names="Verdana"
                                                                                Font-Size="XX-Small">Close Date*</asp:Label><br>
                                                                            <ION:Customcalendar ID="dtClosedDate" runat="server" Width="120px" Height="18px"
                                                                                Font-Size="XX-Small"></ION:Customcalendar>
                                                                        </td>
                                                                        <td valign="top" rowspan="3">
                                                                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Description</asp:Label><br>
                                                                            <asp:TextBox ID="txtComment" runat="server" Height="50px" Width="311px" Font-Names="Verdana"
                                                                                Font-Size="XX-Small" BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus"
                                                                                MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 218px" valign="top">
                                                                            <asp:Label ID="lblMiddleName6" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                                Font-Size="XX-Small">SubCategory Type</asp:Label><br>
                                                                            <uc1:CustomDDL ID="cddlProjectType" runat="server" Width="120px"></uc1:CustomDDL>
                                                                        </td>
                                                                        <td style="width: 203px" valign="top">
                                                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Category Code 1</asp:Label><br>
                                                                            <asp:TextBox ID="txtCat1" runat="server" Width="120px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus" MaxLength="50"></asp:TextBox>
                                                                        </td>
                                                                        <td style="width: 212px" valign="top">
                                                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Category Code 2</asp:Label><br>
                                                                            <asp:TextBox ID="txtCat2" runat="server" Width="120px" Font-Names="Verdana" Font-Size="XX-Small"
                                                                                BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus" MaxLength="50"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </cc1:CollapsiblePanel>
                                                            <!-- **********************************************************************-->
                                                        </td>
                                                    </tr>
                                                    <!-- **********************************************************************-->
                                                    <tr>
                                                        <td>
                                                            <cc1:CollapsiblePanel ID="cpnlMember" runat="server" Width="100%" Height="47px" BorderStyle="Solid"
                                                                BorderWidth="0px" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                                                                TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="SubCategory Member"
                                                                ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                                Draggable="False">
                                                                <div style="overflow: auto; width: 100%">
                                                                    <table bordercolor="#ff3333" cellspacing="0" cellpadding="0" width="100%" align="left"
                                                                        border="0">
                                                                        <tr valign="top">
                                                                            <td valign="top">
                                                                                <asp:PlaceHolder ID="Placeholder1" runat="server"></asp:PlaceHolder>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Panel ID="PnlAction" Width="1pt" runat="server">
                                                                    <table id="Table3" cellspacing="0" cellpadding="0" border="0">
                                                                        <tr align="left">
                                                                            <td align="left">
                                                                                <asp:Label ID="label" runat="server" Width="53px" Font-Size="XX-Small"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlMember_F" runat="server" Width="254px" BackColor="#D7E3F3" Font-Size="XX-Small"
                                                                                    CssClass="txtNoFocusFE">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="DDLRole_F" runat="server" Width="223px" BackColor="#D7E3F3" CssClass="txtNoFocusFE">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="DDLReportsTo_F" runat="server" Width="253px" BackColor="#D7E3F3" CssClass="txtNoFocusFE">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </cc1:CollapsiblePanel>
                                                        </td>
                                                    </tr>
                                                    <!-- **********************************************************************-->
                                                    <tr>
                                                        <td>
                                                            <cc1:CollapsiblePanel ID="cpnlOverview" runat="server" Width="100%" Height="47px"
                                                                BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="True" TitleCSS="test"
                                                                PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                                                Text="SubCategory Info" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                                Draggable="False">
                                                                <asp:Panel ID="pln" Width="0" runat="server">
                                                                    <table cellspacing="0" cellpadding="0" width="780" bgcolor="#f5f5f5" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Description</asp:Label><br>
                                                                                <asp:TextBox ID="txtDescription" runat="server" Height="16px" Width="260px" Font-Names="Verdana"
                                                                                    Font-Size="XX-Small" BackColor="#D7E3F3" BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus"
                                                                                    MaxLength="200" name="txtAddNo"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Key Deliverance</asp:Label><br>
                                                                                <asp:TextBox ID="txtKeyDeliverance" runat="server" Height="16px" Width="260px" Font-Names="Verdana"
                                                                                    Font-Size="XX-Small" BackColor="#D7E3F3" BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus"
                                                                                    MaxLength="200" name="txtAddNo"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Budget</asp:Label><br>
                                                                                <asp:TextBox ID="txtBudget" runat="server" Height="16px" Width="260px" Font-Names="Verdana"
                                                                                    Font-Size="XX-Small" BackColor="#D7E3F3" BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus"
                                                                                    MaxLength="8" name="txtAddNo"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </cc1:CollapsiblePanel>
                                                        </td>
                                                    </tr>
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
        <tr>
            <td>
                <asp:UpdatePanel ID="PanelUpdate" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="605px" ForeColor="Red" Font-Names="Verdana"
                            Font-Size="XX-Small" BorderWidth="0" BorderStyle="Groove" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthiddenRole" />
                        <input type="hidden" name="txthiddenReportsTo" />
                        <input type="hidden" name="txthiddensaveclicked" id="txthiddensaveclicked" runat="server" />
                        <asp:TextBox ID="txthiddenImage" runat="server" Width="0px" Height="0px" BorderWidth="0px"></asp:TextBox>
                        <asp:TextBox ID="txthidden" runat="server" Width="0px" Height="0px" BorderWidth="0px"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
