<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DocumentView.aspx.vb" Inherits="ImportExport_DocumentView" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
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
		var globleUser;
		var globleRole;
		var globleCompany;
			
		
			
			function callrefresh()
				{
					location.href="AB_Search.aspx";
					//Form1.submit();
				}
								
		function ConfirmDelete(varImgValue)
				{
						if (globleID==null)
								{
									alert("Please select the row");
									return false;
								}
								else
								{
									var confirmed
									confirmed=window.confirm("Delete,Are you sure you want to Delete the selected record ?");
									if(confirmed==false)
									{
										return false;
									}
									else
									{
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
															if  (document.Form1.txthiddenAdno.value=="")
															{
																 alert("Please select the row");
																 return false;
															}
															else
															{
															 //alert(document.Form1.txthiddenAdno.value);
																 document.Form1.txthiddenImage.value=varImgValue;
																 Form1.submit(); 
																 return false;
															}
															
												}	
												
												if (varImgValue=='Close')
												{
																 document.Form1.txthiddenImage.value=varImgValue;
																 Form1.submit(); 
																 return false;
												}
								
								
								if (varImgValue=='Add')
												{
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
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								if (varImgValue=='Reset')
									{
												var confirmed
												confirmed=window.confirm("Do  You Want To reset The Page ?");
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
				
				function KeyCheck(nn,rowvalues,FilePath)
					{

						globleID = nn;
						document.Form1.txthiddenAdno.value=nn;
						document.Form1.txtFilePath.value=FilePath;
			
						
										var tableID='cpnlGrdView_GrdAddSerach';  //your datagrids id
										var table;
												      
											if (document.all) table=document.all[tableID];
												if (document.getElementById) table=document.getElementById(tableID);
												if (table)
												{
														
														for ( var i = 1 ;  i < table.rows.length ;  i++)
															{	
																if( i % 2 == 0)
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
					
					function KeyCheck55(nn,rowvalues,FilePath)
					{
					
							document.Form1.txthiddenAdno.value=nn;
							document.Form1.txthiddenImage.value='Edit';
							document.Form1.txtFilePath.value=FilePath;
							Form1.submit(); 
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

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" onload="Hideshow();" rightmargin="0"
    ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
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
                                        &nbsp;
                                    </td>
                                    <td width="50">
                                        <img height="20" src="../Images/top_right.gif" width="50">
                                    </td>
                                    <td width="21">
                                        <a href="#">
                                            <img height="20" src="../Images/bt_min.gif" width="21" border="0"></a>
                                    </td>
                                    <td width="21">
                                        <a href="#">
                                            <img height="20" src="../Images/bt_max.gif" width="21" border="0"></a>
                                    </td>
                                    <td width="19">
                                        <a href="#">
                                            <img onclick="CloseWSS();" height="20" src="../Images/bt_clo.gif" width="19" border="0"></a>
                                    </td>
                                    <td width="6">
                                        <img height="20" src="../Images/bt_space.gif" width="6">
                                    </td>
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
                                                <td valign="middle">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:Button><asp:ImageButton
                                                        ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="white.GIF"
                                                        CommandName="submit" AlternateText="."></asp:ImageButton><img class="PlusImageCSS"
                                                            onclick="HideContents();" alt="Hide" src="../Images/left005.gif" name="imgHide">
                                                    <img class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
                                                        name="ingShow">
                                                    <asp:Label ID="lblTitleLabelRoleSearch" runat="server" Width="136px" Height="12px"
                                                        BorderStyle="None" BorderWidth="2px" CssClass="TitleLabel"> DOCUMENT VIEW</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img
                                                            title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
                                                    <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../Images/s2Add01.gif"
                                                        ToolTip="Add"></asp:ImageButton>&nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../Images/S2edit01.gif"
                                                        ToolTip="Edit"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                                                        ToolTip="Reset"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../Images/s1search02.gif"
                                                        ToolTip="Search"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../Images/s2delete01.gif"
                                                        ToolTip="Delete"></asp:ImageButton>&nbsp;
                                                    <img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
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
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('953','../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <tr>
                                <td height="10">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line.gif" height="10">
                                                <img height="10" src="../Images/main_line.gif" width="6">
                                            </td>
                                            <td width="7" height="10">
                                                <img height="10" src="../Images/main_line01.gif" width="7">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="2">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line02.gif" height="2">
                                                <img height="2" src="../Images/main_line02.gif" width="2">
                                            </td>
                                            <td width="12" height="2">
                                                <img height="2" src="../Images/main_line03.gif" width="12">
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
                                                <td valign="top" colspan="1">
                                                    <!--  **********************************************************************-->
                                                    <div style="overflow: auto; width: 100%; height: 581px">
                                                        <table width="100%" border="0" align="left">
                                                            <tbody>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <cc1:CollapsiblePanel ID="cpnlErrorPanel" runat="server" Width="100%" Height="47px"
                                                                            BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="False" TitleCSS="test"
                                                                            PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                                                            Text="Error Message" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
                                                                            Draggable="False">
                                                                            <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" align="left"
                                                                                border="0">
                                                                                <tr>
                                                                                    <td colspan="0" rowspan="0">
                                                                                        <asp:Image ID="Image1" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif">
                                                                                        </asp:Image>
                                                                                    </td>
                                                                                    <td colspan="0" rowspan="0">
                                                                                        &nbsp;
                                                                                        <asp:ListBox ID="lstError" runat="server" Width="752px" BorderWidth="0" BorderStyle="Groove"
                                                                                            ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox>
                                                                                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </cc1:CollapsiblePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table id="Table12261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                            width="100%" align="left" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <cc1:CollapsiblePanel ID="cpnlGrdView" runat="server" Width="100%" Height="47px"
                                                                                        BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test"
                                                                                        PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                                                                        Text="Document View" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
                                                                                        Draggable="False">
                                                                                        <div style="overflow: auto; width: 100%; height: 540px">
                                                                                            <table id="Table1261" cellspacing="0" cellpadding="0" width="200%" align="left" border="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Panel ID="Panel1" runat="server">
                                                                                                        </asp:Panel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td valign="top">
                                                                                                        <!--  **********************************************************************-->
                                                                                                        <asp:DataGrid ID="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px"
                                                                                                            CssClass="Grid" BorderColor="Silver" ForeColor="MidnightBlue" Font-Names="Verdana"
                                                                                                            CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left" PageSize="50" DataKeyField="FileID">
                                                                                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                                            <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                                                            <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                                                            <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                                                        </asp:DataGrid>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        </cc1:collapsiblepanel>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td valign="top" width="12" background="../Images/main_line04.gif">
                                                    <img height="1" src="../Images/main_line04.gif" width="12">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td height="2">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line06.gif" height="2">
                                                <img height="2" src="../Images/main_line06.gif" width="2">
                                            </td>
                                            <td width="12" height="2">
                                                <img height="2" src="../Images/main_line05.gif" width="12">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/bottom_back.gif">
                                                &nbsp;
                                            </td>
                                            <td width="66">
                                                <img height="31" src="../Images/bottom_right.gif" width="66">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <input type="hidden" name="txthiddenAdno">
                        <input type="hidden" name="txthiddenImage"><input type="hidden" name="txtFilePath">
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
