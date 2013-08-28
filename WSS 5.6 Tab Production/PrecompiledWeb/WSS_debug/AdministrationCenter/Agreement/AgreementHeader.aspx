<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Agreement_AgreementHeader, App_Web_vd1tgp5h" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Agreement Header</title>
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script language="javascript" src="../../SupportCenter/calendar/popcalendar.js" type="text/javascript"></script>

    <script type="text/javascript" src="../../images/Js/JSValidation.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>

    <script type="text/javascript" src="../../DateControl/ION.js"></script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">

    <script type="text/javascript">
				
				
		function CheckLength()
		{
				var tdLength=document.getElementById('cpnlAGH_txtdesc').value.length;
				if ( tdLength>0 )
				{
					if ( tdLength > 500 )
					{
						alert('The Agreement Description cannot be more than 500 characters\n (Current Length :'+tdLength+')');
						return false;
					}
				}
				return true;
		}		
				
				
				
				
			var globalid;
			var globalSkil;
			var globalAddNo;
			var globalGrid;
			var globaldbclick = 0;
						
			function OpenW(a,b,c)
				{
					wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
					return false;
				}
				
			function OpenAB(c)
				{
						wopen('../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as CustomerName,CI_VC16_Alias as AliasName from T010011 where CI_VC8_Address_Book_Type='+"'COM'"+ '  &tbname=' + c ,'Search',500,450);
					return false;
				}
				
			function OpenABPer(c)
				{
				var cusComp=document.getElementById('cpnlAGH_txtCusNo').value;
				var sComp='<%=session("propCompanyID")%>';
				
					wopen('../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as UserName,CI_IN4_Business_Relation as Company from T010011 where CI_IN4_Business_Relation in('+"'"+cusComp+"','"+sComp+"')" + '  &tbname=' + c ,'Search',500,450);
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
						//alert(rowvalues);
						globleID = nn;
						document.Form1.txthidden.value=nn;
			
						
										var tableID='cpnlAD_GrdAddSerach'  //your datagrids id
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
				
			function KeyCheck55(aa,bb,cc)
				{	
				document.getElementById('txthidden').value=aa;
				SaveEdit('Edit');
				return false;	
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
									var code=document.Form1.txthidden.value;
									wopen('AgreementEdit.aspx?CodeID='+code,'Search',400,300);
									return false;
								}										
						}	
											
					if (varimgValue=='Close')
						{
							document.Form1.txthiddenImage.value=varimgValue;
							Form1.submit(); 
							return false;
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
							if (CheckLength()==true)
							{
								document.Form1.txthiddenImage.value=varimgValue;
								Form1.submit(); 		
							}
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
							if (CheckLength()==true)
							{					
							document.Form1.txthiddenImage.value=varimgValue;
							Form1.submit(); 
							}
							return false;
						}		
						
					if (varimgValue=='Search')
								{
									document.Form1.txthiddenImage.value=varimgValue;
									Form1.submit(); 
									return false;
								}					
				
							
					if (varimgValue=='Reset')
						{
							var confirmed
							confirmed=window.confirm("Do You Want To reset The Page ?");
							if(confirmed==true)
								{	
									Form1.reset()
								}		
						}			
					}			
					
				function ConfirmDelete(varimgValue,varMessage)
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
										
											    document.Form1.txthiddenImage.value=varimgValue;
												//document.Form1.txthiddenSkil.value=globalSkil;
												document.Form1.txthidden.value=globalAddNo;	
												//document.Form1.txthiddenGrid.value=globalGrid;	
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
							wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+ "'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
						}
					else//if Attachment is clicked
						{
							wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID='+c+'&ACTIONNO='+a +'&TaskNo='+b ,'Attachment',800,450);
						}
				}
				
					
			function OpenVW(varTable)
				{
					wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ID='+varTable,'Search',500,450);
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
				
    </script>

    <script type="text/javascript">
    //A Function to improve design i.e delete the extra cell of table
    function onEnd() {
        var x = document.getElementById('cpnlAGH_collapsible').cells[0].colSpan = "1";
        var y = document.getElementById('cpnlAD_collapsible').cells[0].colSpan = "1";
    } 
     //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

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
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px"
                                                        ImageUrl="white.GIF" CommandName="submit" AlternateText="."></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelAggHeader" runat="server" BorderStyle="None" BorderWidth="2px"
                                                        Font-Size="X-Small" Font-Names="Verdana" Font-Bold="true" ForeColor="Teal"> Agreement Detail</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgOk" AccessKey="K" Visible="false" runat="server" ImageUrl="../../images/s1ok02.gif"
                                                            ToolTip="Ok"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                            ToolTip="Edit"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../images/s1search02.gif"
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
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('41','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 581px">
                                <table width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td valign="top" colspan="1">
                                                <!-- **********************************************************************-->
                                                <cc1:CollapsiblePanel ID="cpnlAGH" runat="server" Width="100%" BorderStyle="Solid"
                                                    BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                                    TitleForeColor="black" TitleClickable="true" TitleBackColor="transparent" Text="Agreement Header"
                                                    ExpandImage="../../images/ToggleDown.gif" CollapseImage="../../images/ToggleUp.gif"
                                                    Draggable="False">
                                                    <table width="100%" bgcolor="#f5f5f5" border="0">
                                                        <tr>
                                                            <td valign="top" height="40">
                                                                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Bold="true" Font-Names="Verdana"
                                                                    Font-Size="XX-Small">Customer</asp:Label><br>
                                                                <asp:DropDownList ID="DDLCustomer" runat="server" Width="120px" Font-Size="XX-Small"
                                                                    Font-Name="Verdana" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td valign="top" height="40">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Font-Names="Verdana"
                                                                    Font-Size="XX-Small">SubCategory</asp:Label><br>
                                                                <uc1:CustomDDL ID="CDDLProject" runat="server" Width="120px"></uc1:CustomDDL>
                                                            </td>
                                                            <td valign="top" height="40">
                                                                <asp:Label ID="Label3" runat="server" ForeColor="Black" Font-Bold="true" Font-Names="Verdana"
                                                                    Font-Size="XX-Small">Contact Person</asp:Label><br>
                                                                <uc1:CustomDDL ID="cddlPerson" runat="server" Width="120px"></uc1:CustomDDL>
                                                            </td>
                                                            <td valign="top" height="40" rowspan="3">
                                                                <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Bold="true" Font-Names="Verdana"
                                                                    Font-Size="XX-Small">Description</asp:Label><br />
                                                                <asp:TextBox ID="txtdesc" runat="server" Height="102px" Width="400px" Font-Names="Verdana"
                                                                    Font-Size="XX-Small" BorderWidth="1px" BorderStyle="Solid" MaxLength="500" CssClass="txtNoFocus"
                                                                    TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Bold="true" Font-Names="Verdana"
                                                                    Font-Size="XX-Small">Currency</asp:Label><br />
                                                                <uc1:CustomDDL ID="CDDLCur" runat="server" Width="120px"></uc1:CustomDDL>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblMiddleName" runat="server" ForeColor="Black" Font-Bold="true" Font-Names="Verdana"
                                                                    Font-Size="XX-Small">Agreement Type</asp:Label><br />
                                                                <uc1:CustomDDL ID="cddlAGType" runat="server" Width="120px"></uc1:CustomDDL>
                                                            </td>
                                                            <td height="40" valign="top">
                                                                <asp:Label ID="lblMiddleName6" runat="server" ForeColor="Black" Font-Bold="true"
                                                                    Font-Names="Verdana" Font-Size="XX-Small">Status</asp:Label><br />
                                                                <uc1:CustomDDL ID="cddlStatus" runat="server" Width="120px"></uc1:CustomDDL>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" style="height: 64px">
                                                                <asp:Label ID="lblMiddleName2" runat="server" ForeColor="Black" Font-Bold="true"
                                                                    Font-Names="Verdana" Font-Size="XX-Small">Reference</asp:Label><br />
                                                                <asp:TextBox ID="txtreference" runat="server" Width="120px" Font-Names="Verdana"
                                                                    Font-Size="XX-Small" BorderWidth="1px" BorderStyle="Solid" MaxLength="50" CssClass="txtNoFocus"></asp:TextBox>
                                                            </td>
                                                            <td style="height: 64px" valign="top">
                                                                <asp:Label ID="lblMiddleName8" runat="server" Width="72px" ForeColor="Black" Font-Bold="true"
                                                                    Font-Names="Verdana" Font-Size="XX-Small">Valid From</asp:Label><br />
                                                                <ION:Customcalendar ID="dtValidFrom" runat="server" Width="120px" Height="20px" />
                                                            </td>
                                                            <td valign="top" style="height: 64px">
                                                                <asp:Label ID="lblMiddleName10" runat="server" Width="72px" ForeColor="Black" Font-Bold="true"
                                                                    Font-Names="Verdana" Font-Size="XX-Small">Valid Upto</asp:Label>
                                                                <br />
                                                                <ION:Customcalendar ID="dtValidUpto" runat="server" Width="120px" Height="20px" />
                                                                <asp:TextBox ID="txtAggNo" runat="server" Height="0px" Width="0px" BorderWidth="0px"
                                                                    Font-Names="Verdana" Font-Size="XX-Small" BorderStyle="Solid" MaxLength="8" CssClass="txtNoFocus"
                                                                    name="txtAddNo"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc1:CollapsiblePanel>
                                                <!-- **********************************************************************-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlAD" runat="server" Width="100%" Height="47px" BorderStyle="Solid"
                                                    BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                                    TitleForeColor="black" TitleClickable="true" TitleBackColor="transparent" Text="Agreement Lines"
                                                    ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                    Draggable="False">
                                                    <table id="Table1261" cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <asp:Panel ID="Panel1" runat="server">
                                                                </asp:Panel>
                                                                <asp:DataGrid ID="GrdAddSerach" runat="server" ForeColor="MidnightBlue" Font-Names="Verdana"
                                                                    BorderWidth="1px" BorderStyle="None" BorderColor="Silver" CssClass="Grid" CellPadding="0"
                                                                    GridLines="Horizontal" HorizontalAlign="Left" PageSize="50" DataKeyField="LinNo">
                                                                    <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                    <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                                    <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                                    </ItemStyle>
                                                                    <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                    </HeaderStyle>
                                                                    <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <asp:Panel ID="pnlAgrLine" Width="0" runat="server">
                                                                    <table id="Table3" cellspacing="0" cellpadding="0">
                                                                        <tr align="left">
                                                                            <td align="left">
                                                                                <uc1:CustomDDL ID="cddlcall" runat="server" Width="109px" CssClass="txtNoFocusFE">
                                                                                </uc1:CustomDDL>
                                                                            </td>
                                                                            <td align="left">
                                                                                <uc1:CustomDDL ID="cddlTask" runat="server" Width="108px" CssClass="txtNoFocusFE">
                                                                                </uc1:CustomDDL>
                                                                            </td>
                                                                            <td align="left">
                                                                                <uc1:CustomDDL ID="cddllevel" runat="server" Width="108px" CssClass="txtNoFocusFE">
                                                                                </uc1:CustomDDL>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:TextBox ID="txtPrice" runat="server" Height="18px" Width="110px" Font-Names="Verdana"
                                                                                    Font-Size="XX-Small" BorderWidth="1px" BorderStyle="Solid" MaxLength="9" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RadioButtonList ID="rblHour" runat="server" Height="18px" Width="108px" Font-Names="Verdana"
                                                                                    Font-Size="XX-Small" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="H" Selected="true">Hourly</asp:ListItem>
                                                                                    <asp:ListItem Value="F">Fixed</asp:ListItem>
                                                                                </asp:RadioButtonList>
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
                        <asp:ListBox ID="lstError" runat="server" Width="635px" ForeColor="Red" Font-Names="Verdana"
                            Visible="false" Font-Size="XX-Small" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <tr valign="top">
        <td height="25">
            <asp:TextBox ID="txthiddenImage" runat="server" Width="0px"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txthidden" runat="server" Width="0px"></asp:TextBox>
        </td>
    </tr>
    </form>
</body>
</html>
