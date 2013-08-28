<%@ page language="VB" autoeventwireup="false" inherits="Security_Role_Search, App_Web_kkyzf0kd" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Role_Search</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../Images/js/core.js" type="text/javascript"></script>

    <script src="../Images/js/events.js" type="text/javascript"></script>

    <script src="../Images/js/css.js" type="text/javascript"></script>

    <script src="../Images/js/coordinates.js" type="text/javascript"></script>

    <script src="../Images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript">

           var globleID;
			var globleUser;
			var globleRole;
			var globleCompany;
			var rand_no = Math.ceil(500*Math.random())
			
	
			
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
											    			 var RoleKey=document.Form1.txthiddenAdno.value;
											    			 var screenid = window.parent.getActiveTabDetails();
                                                             window.parent.OpenTabOnDBClick('Role Key#' + RoleKey,"Security/RolePermission.aspx?ID=-1&ScrID=293&RoleKey="+ RoleKey, 'Role Key#' + RoleKey,screenid);
    																 
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
													
                                                     window.parent.OpenTabOnAddClick('Roles',"Security/RolePermission.aspx?ID=1&ScrID=293", "293");
													 
													//Form1.submit();
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
				
				function KeyCheck(nn,rowvalues)
					{
						//alert(rowvalues);
						globleID = nn;
						document.Form1.txthiddenAdno.value=nn;
			/*			document.Form1.txthiddenUser.value=nn;
						document.Form1.txthiddenRole.value=nn;
						document.Form1.txthiddenCompany.value=nn;
			*/
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
					
					function KeyCheck55(nn,rowvalues)
					{
					
							document.Form1.txthiddenAdno.value=nn;
							document.Form1.txthiddenImage.value='Edit';
							 var RoleKey=document.Form1.txthiddenAdno.value;
							 var screenid = window.parent.getActiveTabDetails();
                            window.parent.OpenTabOnDBClick('Role Key#' + RoleKey,"Security/RolePermission.aspx?ID=-1&ScrID=293&RoleKey="+ RoleKey, 'Role Key#' + RoleKey,screenid);
							
							//Form1.submit(); 
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
    <table width="100%" style="height: 100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" ImageUrl="white.GIF"
                                                        Width="0px" Height="0px" BorderWidth="0px" CommandName="submit" AlternateText=".">
                                                    </asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelRoleSearch" runat="server" BorderStyle="None" CssClass="TitleLabel"> ROLE SEARCH</asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgAdd" runat="server" ImageUrl="../Images/s2Add01.gif" AccessKey="A"
                                                            ToolTip="Add"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="../Images/S2edit01.gif" AccessKey="E"
                                                            ToolTip="Edit"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" runat="server" ImageUrl="../Images/reset_20.gif" AccessKey="R"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgSearch" runat="server" ImageUrl="../Images/s1search02.gif"
                                                            AccessKey="H" ToolTip="Search"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="../Images/s2delete01.gif"
                                                            AccessKey="D" ToolTip="Delete"></asp:ImageButton>&nbsp;
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('64','../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="table1261" style="border-color: activeborder" cellspacing="0" cellpadding="0"
                                width="100%" align="left" border="0">
                                <tr>
                                    <td nowrap="nowrap">
                                        <asp:Panel ID="Panel1" runat="server">
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        <!--  **********************************************************************-->
                                        <asp:DataGrid ID="GrdAddSerach" runat="server" BorderWidth="1px" Font-Names="Verdana"
                                            ForeColor="MidnightBlue" CellPadding="0" BorderStyle="None" GridLines="Horizontal"
                                            BorderColor="Silver" HorizontalAlign="Left" PageSize="50" CssClass="Grid" DataKeyField="RoleID">
                                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                            <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                            </ItemStyle>
                                            <HeaderStyle Font-Size="8pt" Font-Bold="true" ForeColor="Black" BackColor="#E0E0E0">
                                            </HeaderStyle>
                                            <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
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
                        <input type="hidden" name="txthiddenAdno" />
                        <input type="hidden" name="txthiddenImage" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
