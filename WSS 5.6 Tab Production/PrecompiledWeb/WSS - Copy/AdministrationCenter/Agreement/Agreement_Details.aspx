<%@ page language="VB" autoeventwireup="false" enableeventvalidation="false" inherits="AdministrationCenter_Agreement_Agreement_Details, App_Web_kirrnbfy" maintainscrollpositiononpostback="true" theme="App_Themes" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

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
								
			function ConfirmDelete(varimgValue)
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
											    document.Form1.txthiddenImage.value=varimgValue;
												Form1.submit(); 
												return false;
										}
										
								}
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
							
															if  (document.Form1.txthidden.value=="")
															{
																 alert("Please select the row");
															}
															else
															{
															 //alert(document.Form1.txthiddenAdno.value);
																 document.Form1.txthiddenImage.value=varimgValue;
																 var AggNo =document.Form1.txthidden.value;
																 var CompID=document.Form1.txthiddenCompany.value;
																 //alert(AggNo);
                                                                 //alert(CompID);
                                                                 var screenid = window.parent.getActiveTabDetails();
    												             window.parent.OpenTabOnDBClick('Agreement No#' + AggNo,"AdministrationCenter/Agreement/AgreementHeader.aspx?ID=-1&ScrID=555&SAggID="+ AggNo +"&SCompany="+CompID, 'Aggreement No#' + AggNo,screenid);
																// Form1.submit(); 
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
                                        window.parent.OpenTabOnAddClick('Agreement',"AdministrationCenter/Agreement/AgreementHeader.aspx?ID=1", "41");
										//Form1.submit();
										return false;
									}	
								if (varimgValue=='Search')
								{
									document.Form1.txthiddenImage.value='Search';
									Form1.submit(); 
									return false;
								}	
								
								if (varimgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varimgValue;
									Form1.submit(); 
								}	
								
								if (varimgValue=='Reset')
									{
												var confirmed
												confirmed=window.confirm("Do  You Want To reset The Page ?");
												if(confirmed==true)
														{	
																 Form1.reset()
																 return false;
														}		

									}	
									 return false;		
				}				
				
				function KeyCheck(nn,rowvalues,comp)
					{
//						alert(nn);
//						alert(rowvalues);
//						alert(comp);
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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0" Width="0px" BackColor="#8AAFE5"
                                                        BorderColor="#8AAFE5" BorderStyle="None"></asp:Button><asp:ImageButton ID="imgbtnSearch"
                                                            TabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="." CommandName="submit"
                                                            ImageUrl="white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelAggrement" runat="server" Height="12px" Width="111px"
                                                        ForeColor="Teal" Font-Bold="true" Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px"
                                                        BorderStyle="None"> AGREEMENTS</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
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
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('555','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 1000px; height: 400pt">
                                <table id="table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
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
                                                BorderWidth="1px" BorderStyle="None" BorderColor="Silver" CssClass="Grid" PageSize="50"
                                                HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0">
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
                                        <!-- Panel for displaying Task Info -->
                                        <!-- Panel for displaying Action Info-->
                                        <!-- ***********************************************************************-->
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="722px" BorderStyle="Groove" BorderWidth="0"
                            Visible="false" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" /><input type="hidden" name="txthiddenCompany" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
