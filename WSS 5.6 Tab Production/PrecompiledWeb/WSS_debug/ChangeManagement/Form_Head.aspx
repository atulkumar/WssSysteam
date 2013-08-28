<%@ page language="VB" autoeventwireup="false" inherits="ChangeManagement_Form_Head, App_Web_jikqy_wr" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
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

    <script src="../images/Js/JSValidation.js" type="text/javascript"></script>

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
																var FormID=document.Form1.txthidden.value;
																var screenid = window.parent.getActiveTabDetails();
												            	window.parent.OpenTabOnDBClick('Form Detail#' + FormID,"ChangeManagement/form_entry_head.aspx?ScrID=259&ID=0&SFormID"+FormID, 'Form Detail#' + FormID,screenid);
															
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
                                                    window.parent.OpenTabOnAddClick('Form Detail',"ChangeManagement/form_entry_head.aspx?ScrID=259&ID=0", "259");
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
														}		

									}			
				}				
				
				function KeyCheck(nn,rowvalues)
					{
						//alert(rowvalues);
						globleID = nn;
						document.Form1.txthidden.value=nn;
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
							document.Form1.txthiddenImage.value='Edit';
							//Form1.submit(); 
							var FormID=document.Form1.txthidden.value;
							var screenid = window.parent.getActiveTabDetails();
                            window.parent.OpenTabOnDBClick('Form Detail#' + FormID,'ChangeManagement/form_entry_head.aspx?ScrID=259&ID=0&SFormID='+FormID, 'Form Detail#' + FormID,screenid);
							
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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr width="100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0" Width="0px" BorderStyle="None"
                                                        BorderWidth="0px" BackColor="#8AAFE5" BorderColor="#8AAFE5"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                        AlternateText="." CommandName="submit" ImageUrl="../white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelFormHead" runat="server" CssClass="TitleLabel">FORM HEAD</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <!--<asp:ImageButton id="imgSave" runat="server" ImageUrl="../Images/S2Save01.gif" AccessKey="S"></asp:ImageButton>&nbsp;-->
                                                        <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" AlternateText="Add" ImageUrl="../Images/s2Add01.gif"
                                                            ToolTip="Add"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" AlternateText="Edit" ImageUrl="../Images/S2edit01.gif"
                                                            ToolTip="Edit"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" AlternateText="Search"
                                                            ImageUrl="../Images/s1search02.gif" ToolTip="Search"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" AlternateText="Delete"
                                                            ImageUrl="../Images/s2delete01.gif" ToolTip="Delete"></asp:ImageButton>
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" width="152" background="../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('14','../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
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
                                            <td>
                                                <div style="overflow: auto; height: 450px">
                                                    <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
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
                                                                <asp:DataGrid ID="GrdAddSerach" runat="server" ForeColor="MidnightBlue" Font-Names="Verdana"
                                                                    BorderWidth="1px" BorderStyle="None" BorderColor="Silver" DataKeyField="FormNo"
                                                                    CssClass="Grid" PageSize="50" HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0">
                                                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                    <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                    <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                    <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                        CssClass="GridHeader" BackColor="#E0E0E0"></HeaderStyle>
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
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="635px" BorderStyle="Groove" BorderWidth="0"
                            ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
