<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Template_Template, App_Web_uvcjeiy3" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>WebForm4</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../../Images/js/core.js" type="text/javascript"></script>

    <script src="../../Images/js/events.js" type="text/javascript"></script>

    <script src="../../Images/js/css.js" type="text/javascript"></script>

    <script src="../../Images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../Images/js/drag.js" type="text/javascript"></script>

    <link href="../../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script language="javascript">

			var globleID;
		var rand_no = Math.ceil(500*Math.random())	
	
			
			function callrefresh()
				{
					location.href="AB_Search.aspx";
					//Form1.submit();
				}
								
			function ConfirmDelete(varImgValue,varMessage)
				{
										
							if (globleID==null)
								{
									alert('Please select the row');
									return false;
								}
								else
								{
									var confirmed
									
									confirmed=window.confirm("Do You Want To Delete The Template ?");
									if(confirmed==true)
											{
											    document.Form1.txthiddenImage.value=varImgValue;
												Form1.submit(); 
											}
											else
											{
											return false;
											}	
								}
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
															if (globleID==null)
															{
																alert("Please select the row");
																return false;
															}
															else
															{
																document.Form1.txthiddenImage.value=varImgValue;
																var AddressNo= document.Form1.txthidden.value;
																//alert(document.Form1.txthidden.value);
																var screenid = window.parent.getActiveTabDetails();
            													window.parent.OpenTabOnDBClick('Template Detail# ' +AddressNo,"AdministrationCenter/Template/TemplateDetail.aspx?ScrID=419&AddressNo="+AddressNo, 'Template Detail' +AddressNo,screenid);
																//Form1.submit(); 
															}
															
												}	
												
												if (varImgValue=='Close')
												{
															window.close();	
												}
								
								
								if (varImgValue=='Add')
												{
													document.Form1.txthiddenImage.value=varImgValue;
                                                    window.parent.OpenTabOnAddClick('Template Detail',"AdministrationCenter/Template/TemplateDetail.aspx?ScrID=419&AddressNo=-1", "419");
													//Form1.submit();
													return false;
													  
												}	
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								if (varImgValue=='Ok')
								{
									self.opener.templrefresh(globleID);
									window.close(); 
								  //  alert(globleID);
									//document.Form1.txthiddenImage.value=varImgValue;
									//Form1.submit(); 
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
				
				
				function KeyCheck(nn,rowvalues)
					{
						//alert(rowvalues);
						globleID = nn;
						document.Form1.txthidden.value=nn;
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
							SaveEdit('Edit'); 
					}	
					
			function OpenW(varTable)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('AB_ViewColumns.aspx?TBLName='+varTable,'Search'+rand_no,480,440);
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
				function tabClose() {
            window.parent.closeTab();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td valign="top">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr width="100%">
                                            <td background="../../Images/top_nav_back.gif" height="47">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 15%">
                                                            <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BackColor="#8AAFE5"
                                                                BorderColor="#8AAFE5" BorderStyle="None" BorderWidth="0px"></asp:Button>
                                                            <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" ImageUrl="~/images/white.GIF"
                                                                Width="1px" Height="1px" CommandName="submit" AlternateText="."></asp:ImageButton>
                                                            <asp:Label ID="lblTitleLabelTemplate" runat="server" BorderStyle="None" CssClass="TitleLabel"> TEMPLATE</asp:Label>
                                                        </td>
                                                        <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                            <center>
                                                                <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../../Images/s2Add01.gif"
                                                                    ToolTip="Add"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                                    ToolTip="Edit"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                                    ToolTip="Search"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                                    ToolTip="Delete"></asp:ImageButton>
                                                                <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                                height="47">
                                                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('43','../../');"
                                                    alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                                <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                    src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />
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
                                                        <table width="100%" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <div style="overflow: auto; width: 100%; height: 428pt">
                                                                            <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                                width="110%" align="left" border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Panel ID="Panel1" runat="server">
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td valign="top" align="left">
                                                                                        <!--  **********************************************************************-->
                                                                                        <asp:DataGrid ID="GrdAddSerach" runat="server" BorderWidth="1px" Font-Names="Verdana"
                                                                                            ForeColor="MidnightBlue" CellPadding="0" BorderStyle="None" GridLines="Horizontal"
                                                                                            BorderColor="Silver" HorizontalAlign="Left" PageSize="50" CssClass="grid" DataKeyField="TmpID">
                                                                                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                            <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
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
                                                <input type="hidden" name="txthidden" />
                                                <input type="hidden" name="txthiddenImage" />
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:UpdatePanel ID="PanelUpdate" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMsg" runat="server">
                            </asp:Panel>
                            <asp:ListBox ID="lstError" runat="server" Width="472px" ForeColor="Red" Font-Names="Verdana"
                                Visible="false" Font-Size="XX-Small" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
    </form>
</body>
</html>
