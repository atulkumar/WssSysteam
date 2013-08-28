<%@ page language="VB" autoeventwireup="false" inherits="CommunicationSetup_frm_CommunicationSetup, App_Web_fzr1r8ux" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Call Detail</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link rel="stylesheet" type="text/css" href="../Images/Js/StyleSheet1.css" />
    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <script src="../images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../SupportCenter/calendar/popcalendar.js" type="text/javascript"></script>
 <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>

    <script type="text/javascript">
		
			var globleID;
			var globleUser;
			var globleRole;
			var globleCompany;
			
	var rand_no = Math.ceil(500*Math.random())		
		
				
		function OpenW(a,b,c)
				{
				
				//wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				//window.showModalDialog
			wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Common'+rand_no,500,450);
				}
				
		function OpenComm(a,b)
				{
				
				wopen('comment.aspx?ScrID=329&ID='+ b + '&tbname=T' ,'Comments'+rand_no,500,450);
				
				}
				
		function OpenAtt()
				{
						
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=T','Additional_Address'+rand_no,400,450);
				
				}
		function OpenAB(c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'Common'+rand_no,500,450);										
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName, ab.CI_VC36_Name as Name, UL_VC8_Role_PK as Role from T010011 ab, T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'',500,450);					
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
					'status=no, toolbar=no, scrollbars=no, resizable=no');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}


			function addToParentList(Afilename,TbName)
				{
				
					if (Afilename != "" || Afilename != 'undefined')
					{
					//alert(Afilename);
						document.getElementById(TbName).value=Afilename;
						aa=Afilename;
					}
					else
					
					{
						//document.Form1.txtAB_Type.value=aa;
					}
				}
				

						
			function CloseWindow()
				{
					self.opener.callrefresh();
				}			
				function SaveEdit(varImgValue)
				{
			    	if (varImgValue=='Close')
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
					
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit();
							window.close(); 
							return false; 
						}
						if (varImgValue=='Search')
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
							
						if (varImgValue=='Reset')
						{
							var confirmed
							confirmed=window.confirm("Do You Want To reset The Page ?");
							//alert(confirmed);
							if(confirmed==true)
							{	
								//alert(confirmed);
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit();
								return false;
							}
							else
							{
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

						//alert();
							if (document.Form1.txthidden.value=='')
							{
								alert('Please select a row');
								return false;
							}
							else
							{
								wopen('edit_CommunicationSetup.aspx?InvId=' + globleID,'CommunicationSetup'+rand_no,550,450);		
							}
							
							return false;						
						}			
				}	
				function callrefresh()
				{
					document.Form1.txthiddenImage.value='';
			    	Form1.submit();
				}
				
				
					function FP_swapImg() {//v1.0
							var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
							n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
							elm.$src=elm.src; elm.src=args[n+1]; } }
							}

							function FP_preloadImgs() {//v1.0
							var d=document,a=arguments; if(!d.FP_imgs) d.FP_imgs=new Array();
							for(var i=0; i<a.length; i++) { d.FP_imgs[i]=new Image; d.FP_imgs[i].src=a[i]; }
							}

							function FP_getObjectByID(id,o) {//v1.0
							var c,el,els,f,m,n; if(!o)o=document; if(o.getElementById) el=o.getElementById(id);
							else if(o.layers) c=o.layers; else if(o.all) el=o.all[id]; if(el) return el;
							if(o.id==id || o.name==id) return o; if(o.childNodes) c=o.childNodes; if(c)
							for(n=0; n<c.length; n++) { el=FP_getObjectByID(id,c[n]); if(el) return el; }
							f=o.forms; if(f) for(n=0; n<f.length; n++) { els=f[n].elements;
							for(m=0; m<els.length; m++){ el=FP_getObjectByID(id,els[n]); if(el) return el; } }
							return null;
							}	
							
							
					function KeyCheck(nn,rowvalues)
					{
						//alert(nn);
						globleID = nn;
						document.Form1.txthidden.value=nn;
			/*			document.Form1.txthiddenUser.value=nn;
						document.Form1.txthiddenRole.value=nn;
						document.Form1.txthiddenCompany.value=nn;
			*/
						//Form1.submit();
						
										var tableID='cpnlAD_dgCommRules'  //your datagrids id
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
					
					function KeyCheck55(nn,rowvalues,com,flagkey1)
					{
						if(flagkey1==0)
						{
							if(com=='&nbsp;')
							{
								alert("Default rule cannot be modify by client company");
								return false;
							}
							else
							{
								document.Form1.txthiddenImage.value='Edit';
								SaveEdit('Edit');
								return false;
							}
						}
						else
						{
								document.Form1.txthiddenImage.value='Edit';
								SaveEdit('Edit');
								return false;
						}
					}	
					
			function OpenW(varTable)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company  from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c  ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('../AdministrationCenter/AddressBook/AB_ViewColumns.aspx? ID='+varTable,'Search'+rand_no,500,450);
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
					'status=no, toolbar=no, scrollbars=no, resizable=no');
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
        var x = document.getElementById('cpnlAD_collapsible').cells[0].colSpan = "1";
    } 
    
     //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	
    </script>

    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table width="100%" style="height: 100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0" BorderWidth="0px" BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None">
                                                    </asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px"
                                                        ImageUrl="~/images/white.GIF" AlternateText="." CommandName="submit"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelCS" runat="server" BorderWidth="2px" BorderStyle="None"
                                                        CssClass="TitleLabel">Communication Setup</asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../Images/S2edit01.gif"
                                                            ToolTip="Edit"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../Images/s1search02.gif"
                                                            AlternateText="Search"></asp:ImageButton>&nbsp;
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('544','../');"
                                            alt="E" src="../images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:CollapsiblePanel ID="cpnlAD" runat="server" Width="100%" Height="47px" BorderWidth="0px"
                                BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                TitleBackColor="Transparent" Text="Communication Setup Rules" ExpandImage="../Images/ToggleDown.gif"
                                CollapseImage="../Images/ToggleUp.gif" Draggable="False" Visible="True" BorderColor="Indigo">
                                <div style="overflow: auto; width: 100%; height: 530px">
                                    <table id="Table1261" style="border-color: activeborder" cellspacing="0" cellpadding="0"
                                        width="100%" align="left" border="0">
                                        <tr>
                                            <td nowrap="nowrap">
                                                <asp:Panel ID="Panel1" runat="server">
                                                </asp:Panel>
                                                <asp:DataGrid ID="dgCommRules" runat="server" BorderColor="Silver" BorderStyle="None"
                                                    BorderWidth="1px" CellPadding="0" CssClass="Grid" DataKeyField="RNo" Font-Names="Verdana"
                                                    ForeColor="MidnightBlue" GridLines="Horizontal" HorizontalAlign="Left" PageSize="50">
                                                    <SelectedItemStyle CssClass="GridSelectedItem" />
                                                    <AlternatingItemStyle BackColor="WhiteSmoke" CssClass="GridAlternateItem" />
                                                    <ItemStyle BackColor="White" CssClass="GridItem" Font-Size="8pt" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#E0E0E0" Font-Bold="True" Font-Size="8pt" ForeColor="Black" />
                                                    <FooterStyle BackColor="White" ForeColor="DarkBlue" />
                                                    <PagerStyle Mode="NumericPages" Position="TopAndBottom" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="1">
                                                <!--  *************************** -->
                                                <asp:Panel ID="pnlFE" Width="800px" runat="server">
                                                    <table id="Table121" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                        align="left" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="TextBox2" runat="server" Width="36px" BorderStyle="None" BorderWidth="1px"
                                                                    BackColor="WhiteSmoke" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsMail" runat="server" Width="23px" Text="i" Font-Size="Smaller"
                                                                    Font-Bold="True"></asp:CheckBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsSMS" runat="server" Width="22px" Text="i" Font-Size="Smaller"
                                                                    Font-Bold="True"></asp:CheckBox>
                                                            </td>
                                                            <td>
                                                                <ION:Customcalendar ID="dtStartDate" runat="server" Width="95px" Height="14px" />
                                                            </td>
                                                            <td>
                                                                <ION:Customcalendar ID="dtEndDate" runat="server" Width="100px" Height="14px" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddEventName" runat="server" Height="18px" Width="96px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddEventUser" runat="server" Height="18px" Width="100px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddEventFired" runat="server" Height="18px" Width="100px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddCompany" runat="server" Height="18px" Width="120px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddUserName" runat="server" Height="18px" Width="111px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddRoleName" runat="server" Height="18px" Width="110px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddPriority" runat="server" Height="18px" Width="57px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddCallType" runat="server" Height="18px" Width="60px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddCallStatus" runat="server" Height="18px" Width="65px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddTaskType" runat="server" Height="18px" Width="60px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddTaskStatus" runat="server" Height="18px" Width="60px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddProject" runat="server" Height="18px" Width="58px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                    <asp:ListItem Value="0" Selected="True">Opt</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddRecordStatus" runat="server" Height="18px" Width="60px" BackColor="#D7E3F3" CssClass="txtNoFocusFE"
                                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                                    <asp:ListItem Value="1">Enb</asp:ListItem>
                                                                    <asp:ListItem Value="2">Dsb</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:TextBox ID="txtCompanyID" TabIndex="2" runat="server" Height="0" Width="0px"
                                                    BorderStyle="Solid" BorderWidth="0px" Font-Size="XX-Small" Font-Names="Verdana"
                                                    ReadOnly="True" MaxLength="12"></asp:TextBox>
                                                <!--  *************************** -->
                                                <input id="Textbox1" type="hidden" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </cc1:CollapsiblePanel>
                            <asp:Label ID="Label14" runat="server" Width="136px" Height="12px" ForeColor="DimGray"
                                Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small" Visible="False">Staus Change(on/off)</asp:Label><input
                                    type="hidden" name="txthiddenImage" style="width: 128px; height: 22px" size="16">
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="100px" BorderStyle="Groove" BorderWidth="0"
                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
