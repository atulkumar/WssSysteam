<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Agreement_PendingInvoice, App_Web_vd1tgp5h" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Temporary Invoices</title>
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

    <script type="text/javascript">

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
									alert("Please select the Pending Invoice to delete");
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
														if  (document.Form1.txthidden.value=="")
															{
																 alert("Please select the Invoice");
																 return false;
															}
															else
															{
															 //alert(document.Form1.txthiddenAdno.value);
																 document.Form1.txthiddenImage.value=varImgValue;
																 var code=document.Form1.txthidden.value;
																 var comid=document.Form1.txthiddenCompany.value;
																	wopen('EditInvoice.aspx?CodeID='+code+'&comp='+comid,'Search',770,450);
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
									document.Form1.txthiddenImage.value='Search';
									Form1.submit(); 
									return false;
								}	
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								if (varImgValue=='Cancel')
								{
								
									if (document.Form1.txthidden.value)
									{
											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit(); 
									}
									else
									{
										alert('Please select the Invoice');
										return false;
									}
								
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
									return false;		
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
				
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr width="100%">
                        <td background="../../Images/top_nav_back.gif" height="47">
                            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="BtnGrdSearch" runat="server" Height="0" Width="0px" BackColor="#8AAFE5"
                                            BorderColor="#8AAFE5" BorderStyle="None"></asp:Button><asp:ImageButton ID="imgbtnSearch"
                                                TabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="." CommandName="submit"
                                                ImageUrl="~/images/white.GIF"></asp:ImageButton>
                                        <asp:Label ID="lblTitleLabelPendingInvoices" runat="server" Height="12px" Width="168px"
                                            ForeColor="Teal" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px"
                                            BorderStyle="None">PENDING INVOICES</asp:Label>
                                    </td>
                                    <td valign="middle" align="left" background="../../Images/top_nav_back.gif" bgcolor="lightgrey"
                                        height="47">
                                        <img title="Seperator" alt="R" src="../../images/00Seperator.gif" border="0">&nbsp;
                                        <asp:ImageButton AlternateText="Reset" ID="imgReset" AccessKey="R" runat="server"
                                            ImageUrl="../../images/reset_20.gif"></asp:ImageButton>&nbsp;<asp:ImageButton AlternateText="Search"
                                                ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../images/s1search02.gif">
                                            </asp:ImageButton>
                                        <asp:ImageButton ID="ImgCancel" AccessKey="H" runat="server" AlternateText="Cancel Invoice"
                                            ImageUrl="../../Images/Cancel.gif"></asp:ImageButton>&nbsp;
                                        <asp:ImageButton ID="ImgClearInvoice" AccessKey="C" runat="server" AlternateText="Show Clear and Temp Invoice"
                                            ImageUrl="../../Images/Invoice.gif"></asp:ImageButton>&nbsp;<asp:ImageButton ID="imgEdit"
                                                AccessKey="E" runat="server" ImageUrl="../../images/AmtRec.jpg" ToolTip="Amount Received">
                                            </asp:ImageButton>
                                        <asp:ImageButton ID="ImgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                                            ToolTip="Close"></asp:ImageButton>&nbsp;&nbsp;<img title="Seperator" alt="R" src="../../images/00Seperator.gif"
                                                border="0">
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" width="152" background="../../Images/top_nav_back.gif" height="47">
                            <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('586','../../');"
                                alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                    class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                    src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <tr>
                    <td>
                        <div style="overflow: auto; width: 100%; height: 581px" id="asd">
                            <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td valign="top" colspan="1">
                                        <!--  **********************************************************************-->
                                        <div style="overflow: auto; width: 100%; height: 581px" id="div22">
                                            <table width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <div style="overflow: auto; width: 100%; height: 400pt" id="div1">
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
                                                                                BorderWidth="1px" BorderStyle="None" BorderColor="Silver" CssClass="grid" PageSize="50"
                                                                                HorizontalAlign="Left" DataKeyField="InvNo" GridLines="Horizontal" CellPadding="0">
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
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="722px" BorderStyle="Groove" BorderWidth="0"
                            Visible="false" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthiddenCompany" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
