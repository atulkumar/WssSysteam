<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Agreement_TempInvoices, App_Web_nliv-wby" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Temporary Invoices</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

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
							
															if  (document.Form1.txthidden.value=="")
															{
																 alert("Please select the row");
																 return false;
															}
															else
															{
															 //alert(document.Form1.txthiddenAdno.value);
																 document.Form1.txthiddenImage.value=varImgValue;
																 var Dno=document.Form1.txthidden.value;
																 var CompId=document.Form1.txthiddenCompany.value;
																 var screenid = window.parent.getActiveTabDetails();
    												             window.parent.OpenTabOnDBClick('Draft No#' + Dno,"AdministrationCenter/Agreement/InvoiceDetails.aspx?ID=-1&SInvID="+ Dno +"&SCompany="+CompId, 'Draft No#' + Dno,screenid);
																 
																// Form1.submit(); 
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
										//Form1.submit();
                                        window.parent.OpenTabOnAddClick('Draft',"AdministrationCenter/Agreement/InvoiceDetails.aspx?ID=1", "717");
										return false;
									}	
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value='Search';
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
																
														}		

									}			 return false;
				}				
				
				function KeyCheck(nn,rowvalues,comp)
					{
						//alert(rowvalues);
						globleID = nn;
						document.Form1.txthidden.value=nn;
						document.Form1.txthiddenCompany.value=comp;
			
						//Form1.submit();
						
										var tableID='GrdAddSerach'  //your datagrids id
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
					
					function KeyCheck55(nn,rowvalues,comp)
					{
						document.Form1.txthidden.value=nn;
						document.Form1.txthiddenCompany.value=comp;
						SaveEdit('Edit');
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
			
			
 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	

				
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr width="100%">
                        <td background="../../Images/top_nav_back.gif" height="47">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 25%">
                                        <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BorderWidth="0px"
                                            BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                        <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                            AlternateText="." CommandName="submit" ImageUrl="~/images/white.GIF"></asp:ImageButton>
                                        <asp:Label ID="lblTitleLabelTempInvoices" runat="server" Height="12px" Width="168px"
                                            ForeColor="Teal" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px"
                                            BorderStyle="None">TEMPORARY INVOICES</asp:Label>
                                    </td>
                                    <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                        <center>
                                            <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../../images/s2Add01.gif"
                                                ToolTip="Add"></asp:ImageButton>
                                            <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../images/S2edit01.gif"
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
                            <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('582','../../');"
                                alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                    class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                    src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                <cc1:CollapsiblePanel ID="cpnlErrorPanel" runat="server" Height="47px" Width="100%"
                                                    BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../../images/ToggleUp.gif"
                                                    ExpandImage="../../images/ToggleDown.gif" Text="Error Message" TitleBackColor="Transparent"
                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                    Visible="False" BorderColor="Indigo">
                                                    <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td colspan="0" rowspan="0">
                                                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="../../images/warning.gif">
                                                                </asp:Image>
                                                                <asp:ListBox ID="lstError" runat="server" Width="722px" BorderStyle="Groove" BorderWidth="0"
                                                                    Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                                                            </td>
                                                            <td colspan="0" rowspan="0">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Label ID="lblError" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                        ForeColor="Red"></asp:Label>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="overflow: auto; width: 100%; height: 400pt">
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
                                                                <asp:DataGrid ID="GrdAddSerach" runat="server" ForeColor="MidnightBlue" Font-Names="Verdana"
                                                                    BorderWidth="1px" BorderStyle="None" BorderColor="Silver" CssClass="Grid" PageSize="50"
                                                                    HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0">
                                                                    <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                    <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                    <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="Griditem" BackColor="White">
                                                                    </ItemStyle>
                                                                    <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                    </HeaderStyle>
                                                                    <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                </asp:DataGrid>
                                                                <!-- Panel for displaying Task Info -->
                                                                <!-- Panel for displaying Action Info-->
                                                                <!-- ***********************************************************************-->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlMsg" runat="server">
                </asp:Panel>
                <input type="hidden" name="txthidden" />
                <input type="hidden" name="txthiddenImage" /><input type="hidden" name="txthiddenCompany" />
                </ContentTemplate> </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
