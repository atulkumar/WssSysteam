<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_AlertSetUp, App_Web_y4bhs5yb" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Alerts</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>

    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../Images/Js/JSValidation.js"></script>

    <script>

			var globleID;
			var globlestrTempName;
			
			var gAID;
			
			function callrefresh()
				{
				//alert();
					//location.href="alert.aspx";
					Form1.submit();
				}
								
			function ConfirmDelete(varImgValue,varMessage)
				{
					
					
							if (globleID==null)
								{
									alert("Please Select the Record");
								}
								else
								{
									var confirmed
									confirmed=window.confirm(varMessage);
									if(confirmed==true)
											{
											    document.Form1.txthiddenImage.value=varImgValue;
												Form1.submit(); 
											}
											else
											{
											}	
								}
				}
				
				
				
			function SaveEdit(varImgValue)
				{
			    				if (varImgValue=='Edit')
								{
											if (globleID==null)
											{
												alert("Please select the row");
											}
											else
											{
												document.Form1.txthiddenImage.value=varImgValue;
												Form1.submit(); 
											}
											
								}	
										
								if (varImgValue=='Close')
								{
											window.close();	
								}
				
						
								if (varImgValue=='Save')
												{
													document.Form1.txthiddenImage.value=varImgValue;
													Form1.submit();
													  
												}	
								if (varImgValue=='Add')
								{
										//document.Form1.txthiddenImage.value=varImgValue;
										//Form1.submit();
										wopen("AlertEdit.aspx?ID=-1","AlertEdit",450,400);			
								}	
								
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								if (varImgValue=='Close')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
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
				}				
				
				
				function KeyCheck(nn,rowvalues,strTempName)
					{
				
						globleID = nn;
						globlestrTempName = strTempName;
						document.Form1.txthidden.value=nn;
						document.getElementById("txthiddenAlertID").value=nn;
						document.getElementById("txthiddenSV").value=rowvalues;
						//document.Form1.txthiddenAlertID.value=nn;
						//Form1.submit();
						
										var tableID='cpnlViewJobs_GrdAddSerach'  //your datagrids id
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
								setTimeout('Form1.submit();',600);
					//document.Form1.submit();
					}	
					
					function KeyCheck55(ID,rowvalues)
					{
							//self.opener.templrefresh(globleID,globlestrTempName);
							//window.close(); 
						//	alert(rowvalues);
						
						wopen("AlertEdit.aspx?ID="+ID,"AlertEdit",450,400);							
						//'document.Form1.txthiddenImage.value='Edit';
						//Form1.submit(); 
						
					}	
					
		function KeyCheckAF(nn,rowvalues,strTempName)
					{
						//alert(rowvalues);
						globleID = nn;
						globlestrTempName = strTempName;
						document.Form1.txthidden.value=nn;
						//document.getElementById("txthiddenAlertID").value=nn;
						//document.getElementById("txthiddenSV").value=rowvalues;
						//document.Form1.txthiddenAlertID.value=nn;
						//Form1.submit();
						
										var tableID='cpnlAlerFlow_GrdAlertFlow'  //your datagrids id
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
				//	document.Form1.submit();
					}	
					
					function KeyCheck55AF(ID,rowvalues)
					{
							//self.opener.templrefresh(globleID,globlestrTempName);
							//window.close(); 
						gAID=document.getElementById('txthiddenAlertID').value;
						wopen("AlertFlowEdit.aspx?LineID="+ID + '&AID=' + gAID ,"AlertEdit",450,400);							
						//'document.Form1.txthiddenImage.value='Edit';
						//Form1.submit(); 
						
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

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" onload="Hideshow();" rightmargin="0">
    <form id="Form1" method="post" runat="server">
        <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td valign="top">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td>
                                <div align="right">
                                    <img height="2" src="../Images/top_right_line.gif" width="96"></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../Images/top_left_back.gif">
                                            &nbsp;</td>
                                        <td width="50">
                                            <img height="20" src="../Images/top_right.gif" width="50"></td>
                                        <td width="21">
                                            <a href="#">
                                                <img height="20" src="../Images/bt_min.gif" width="21" border="0"></a></td>
                                        <td width="21">
                                            <a href="#">
                                                <img height="20" src="../Images/bt_max.gif" width="21" border="0"></a></td>
                                        <td width="19">
                                            <a href="#">
                                                <img onclick="CloseWSS();" height="20" src="../Images/bt_clo.gif" width="19" border="0"></a></td>
                                        <td width="6">
                                            <img height="20" src="../Images/bt_space.gif" width="6"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr width="100%">
                                        <td background="../Images/top_nav_back.gif" height="67">
                                            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:Button><asp:ImageButton
                                                            ID="Imagebutton1" TabIndex="1" runat="server" Width="1px" Height="1px" AlternateText="."
                                                            CommandName="submit" ImageUrl="~/images/white.GIF"></asp:ImageButton><img class="PlusImageCSS"
                                                                onclick="HideContents();" alt="Hide" src="../Images/left005.gif" name="imgHide">
                                                        <img class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
                                                            name="ingShow">
                                                        <asp:Label ID="lblTitleLabelAlerts" runat="server" ForeColor="Teal" Font-Bold="True"
                                                            Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">ALERTS</asp:Label></td>
                                                    <td valign="bottom" align="center">
                                                        <img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;&nbsp;<img
                                                            class="PlusImageCSS" id="Save" title="Save" onclick="SaveEdit('Save');" alt=""
                                                            src="../Images/S2Save01.gif" border="0" name="tbrbtnSave">&nbsp;<img class="PlusImageCSS"
                                                                title="Add Alert" onclick="SaveEdit('Add');" alt="" src="../Images/S2Add01.gif"
                                                                border="0" name="tbrbtnClose">&nbsp;<img class="PlusImageCSS" title="Search" onclick="SaveEdit('Search');"
                                                                    alt="S" src="../Images/s1search02.gif" border="0" name="tbrbtnSearch">&nbsp;&nbsp;<img
                                                                        class="PlusImageCSS" title="Reset" onclick="SaveEdit('Reset');" alt="R" src="../Images/reset_20.gif"
                                                                        border="0" name="tbrbtnReset">&nbsp;<img class="PlusImageCSS" title="Close" onclick="SaveEdit('Close');"
                                                                            alt="" src="../Images/s2close01.gif" border="0" name="tbrbtnClose">&nbsp;&nbsp;<img
                                                                                title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right" width="152" background="../Images/top_nav_back01.gif" height="67">
                                            <img class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
                                                src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img class="PlusImageCSS"
                                                    id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
                                                    border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;</td>
                                    </tr>
                                </table>
                                <tr>
                                    <td height="10">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td background="../Images/main_line.gif" height="10">
                                                    <img height="10" src="../Images/main_line.gif" width="6"></td>
                                                <td width="7" height="10">
                                                    <img height="10" src="../Images/main_line01.gif" width="7"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                        <tr>
                            <td height="2">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../Images/main_line02.gif" height="2">
                                            <img height="2" src="../Images/main_line02.gif" width="2"></td>
                                        <td width="12" height="2">
                                            <img height="2" src="../Images/main_line03.gif" width="12"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="overflow: auto; width: 100%; height: 581px">
                                    <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                        border="0">
                                        <tbody>
                                            <tr>
                                                <td valign="top" colspan="1">
                                                    <!--  **********************************************************************-->
                                                    <div style="overflow: auto; width: 100%; height: 581px">
                                                        <table width="100%" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <cc1:CollapsiblePanel ID="cpnlError" runat="server" Width="100%" Height="47px" BorderWidth="0px"
                                                                            BorderStyle="Solid" BorderColor="Indigo" Visible="False" TitleCSS="test" PanelCSS="panel"
                                                                            TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Error Message"
                                                                            ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
                                                                            Draggable="False">
                                                                            <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" border="0">
                                                                                <tr>
                                                                                    <td valign="middle">
                                                                                        <asp:Image ID="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif">
                                                                                        </asp:Image></td>
                                                                                    <td>
                                                                                        <asp:ListBox ID="lstError" runat="server" Height="64px" Width="600" Font-Size="XX-Small">
                                                                                        </asp:ListBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </cc1:CollapsiblePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <cc1:CollapsiblePanel ID="cpnlViewJobs" runat="server" Width="100%" Height="47px"
                                                                            BorderWidth="0px" BorderStyle="Solid" BorderColor="Indigo" Visible="True" TitleCSS="test"
                                                                            PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                                                            Text="Alerts" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
                                                                            Draggable="False">
                                                                            <div style="overflow: auto; width: 100%; height: 190pt">
                                                                                <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                                    width="100%" align="left" border="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Panel ID="Panel1" runat="server">
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="top" align="left">
                                                                                            <!--  **********************************************************************-->
                                                                                            <asp:DataGrid ID="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px"
                                                                                                Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" DataKeyField="AlertID"
                                                                                                CssClass="grid" PageSize="50" HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0">
                                                                                                <SelectedItemStyle BackColor="#d4d4d4"></SelectedItemStyle>
                                                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                                <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                                                                </ItemStyle>
                                                                                                <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                                                </HeaderStyle>
                                                                                                <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                                <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                                            </asp:DataGrid><!-- Panel for displaying Task Info -->
                                                                                            <!-- Panel for displaying Action Info-->
                                                                                            <!-- ***********************************************************************-->
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </cc1:CollapsiblePanel>
                                                                        <cc1:CollapsiblePanel ID="cpnlAlerFlow" runat="server" Width="100%" Height="47px" BorderWidth="0px" BorderStyle="Solid" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Alert Flow" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif" Draggable="False">
                                                                            <div style="overflow: auto; width: 100%; height: 190pt">
                                                                                <asp:Panel ID="Panel3" runat="server" Width="300pt">
                                                                                    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                                                        align="left" border="0">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td colspan="8">
                                                                                                    <asp:Panel ID="Panel2" runat="server">
                                                                                                    </asp:Panel>
                                                                                                    <asp:DataGrid ID="GrdAlertFlow" runat="server" Font-Names="Verdana" BorderWidth="1px"
                                                                                                        BorderColor="Silver" CellPadding="0" CssClass="grid" DataKeyField="LineID" AutoGenerateColumns="False">
                                                                                                        <AlternatingItemStyle CssClass="AlternateItem"></AlternatingItemStyle>
                                                                                                        <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem"></ItemStyle>
                                                                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" CssClass="GridHeader"></HeaderStyle>
                                                                                                        <Columns>
                                                                                                            <asp:BoundColumn DataField="AF_NU9_LNID" HeaderText="LineID"></asp:BoundColumn>
                                                                                                            <asp:BoundColumn DataField="AF_VC8_Type" HeaderText="AlType"></asp:BoundColumn>
                                                                                                            <asp:BoundColumn DataField="AF_VC50_COM" HeaderText="Com Mode"></asp:BoundColumn>
                                                                                                            <asp:BoundColumn DataField="AF_VC12_Template_Type" HeaderText="Template Type"></asp:BoundColumn>
                                                                                                            <asp:BoundColumn DataField="AF_NU9_Template_ID_FK" HeaderText="Template"></asp:BoundColumn>
                                                                                                            <asp:BoundColumn DataField="AF_VC8_Status" HeaderText="Status"></asp:BoundColumn>
                                                                                                        </Columns>
                                                                                                    </asp:DataGrid></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Image ID="Image1" Width="54px" Height="18px" ImageUrl="../images/divider.gif"
                                                                                                        runat="server"></asp:Image></td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="DDLAlertActionType" runat="server" Width="69px" Height="18px"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small" CssClass="txtNoFocusFE" AutoPostBack="True">
                                                                                                        <asp:ListItem></asp:ListItem>
                                                                                                        <asp:ListItem Value="EML">EML</asp:ListItem>
                                                                                                        <asp:ListItem Value="WSS">WSS</asp:ListItem>
                                                                                                    </asp:DropDownList></td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtCom" Width="361" Height="18px" CssClass="txtNoFocusFE" runat="server"
                                                                                                        MaxLength="50"></asp:TextBox></td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="DDLTemplType" runat="server" Width="108px" Height="18px" Font-Names="Verdana"
                                                                                                        Font-Size="XX-Small" CssClass="txtNoFocusFE" AutoPostBack="True">
                                                                                                        <asp:ListItem></asp:ListItem>
                                                                                                        <asp:ListItem Value="CAO">CAO</asp:ListItem>
                                                                                                        <asp:ListItem Value="CNT">CNT</asp:ListItem>
                                                                                                    </asp:DropDownList></td>
                                                                                                <td>
                                              <uc1:customddl id="CDDLTempl" runat="server" width="161px"></uc1:customddl></td>
                                                                                                <td>
                                                                                                    <uc1:customddl id="CDDLStatus" runat="server" width="68px">
                                                                                                    </uc1:customddl></td>
                                                                                                            </tr>
     </tbody>
     </table></asp:Panel></div></cc1:CollapsiblePanel>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" width="12" background="../Images/main_line04.gif">
                    <img height="1" src="../Images/main_line04.gif" width="12"></td>
            </tr>
        </table>
        <tr>
            <td height="2">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td background="../Images/main_line06.gif" height="2">
                            <img height="2" src="../Images/main_line06.gif" width="2"></td>
                        <td width="12" height="2">
                            <img height="2" src="../Images/main_line05.gif" width="12"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td background="../Images/bottom_back.gif">
                            &nbsp;</td>
                        <td width="66">
                            <img height="31" src="../Images/bottom_right.gif" width="66"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <div>
        </div>
        <input type="hidden" name="txthidden">
        <input type="hidden" name="txthiddenImage">
        <input id="txthiddenAlertID" type="hidden" value="0" name="txthiddenAlertID" runat="server">
        <input id="txthiddenSV" type="hidden" value="-1" name="txthiddenSV" runat="server">
    </form>
</body>
</html>
