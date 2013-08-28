<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Template_TemplateSearch, App_Web_uvcjeiy3" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Templates</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../../Images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../Images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../Images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../Images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../Images/js/drag.js" type="text/javascript"></script>

    <link href="../../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script>

			var globleID;
			var globlestrTempName;
			var gTemplateType;
			var rand_no = Math.ceil(500*Math.random())
			
			function HideContents()
				{
					parent.document.all("SideMenu1").cols="0,*";
					document.Form1.imgHide.style.visibility = 'hidden'; 
					document.Form1.ingShow.style.visibility = 'visible'; 				
				}
					
			function ShowContents()
				{
					document.Form1.ingShow.style.visibility = 'hidden'; 
					document.Form1.imgHide.style.visibility = 'visible'; 
					parent.document.all("SideMenu1").cols="18%,*";					
				}
					
			function Hideshow()
				{
					if (parent.document.all("SideMenu1").cols =="0,*")
					{
							document.Form1.imgHide.style.visibility = 'hidden'; 
							document.Form1.ingShow.style.visibility = 'visible'; 
					}
					else
					{
							document.Form1.ingShow.style.visibility = 'hidden'; 
							document.Form1.imgHide.style.visibility = 'visible'; 
					}
				}				
			
			function callrefresh()
				{
					location.href="AB_Search.aspx";
					//Form1.submit();
				}
								
			function ConfirmDelete(varImgValue,varMessage)
				{
					
					
							if (globleID==null)
								{
									alert("Please select the row");
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
								
								
								if (varImgValue=='Add')
												{
													document.Form1.txthiddenImage.value=varImgValue;
													Form1.submit();
													  
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
									if( globleID==null)
									   {
									      alert("Please select the row");
									       return false;
									
									    } 
									else
								    {
																	
									self.opener.templrefresh(globleID,globlestrTempName,gTemplateType);
									//self.opener.templrefresh(globleID,globlestrTempName);
									window.close(); 
								  //  alert(globleID);
									//document.Form1.txthiddenImage.value=varImgValue;
									//Form1.submit(); 
									}
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
				
				
				function KeyCheck(nn,rowvalues,strTempName,TempType)
					{
					gTemplateType=TempType;
						//alert(strTempName);
						globleID = nn;
						globlestrTempName = strTempName;
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
					
					function KeyCheck55(nn,rowvalues,temp,TempType)
					{
							//	alert(TempType);
							self.opener.templrefresh(globleID,globlestrTempName,TempType);
							window.close(); 
						//	document.Form1.txthiddenImage.value='Edit';
						//	Form1.submit(); 
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
				
    </script>

    <script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.HideModalDiv)
                opener.parent.HideModalDiv();
            }
        }
        //Modified By Atul to execute script on Page Load
        function OnLoad() {
           if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.LoadModalDiv)
                opener.parent.LoadModalDiv();
            }
        }
        window.onload = OnLoad;
        window.onunload = OnClose;
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        background="../../images/top_nav_back.gif" border="0">
        <tr>
            <td style="width: 15%">
                <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0" BackColor="#8AAFE5"
                    BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" ImageUrl="white.GIF"
                    Width="1px" Height="1px" CommandName="submit" AlternateText="."></asp:ImageButton>
                <asp:Label ID="lblTitleLabelTemplate" runat="server" CssClass="TitleLabel"> TEMPLATE</asp:Label>
            </td>
            <td style="width: 85%; text-align: center;" nowrap="nowrap">
                <center>
                    <asp:ImageButton ID="imgOk" runat="server" ImageUrl="../../Images/s1ok02.gif" AccessKey="K"
                        ToolTip="Ok"></asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="imgClose" AccessKey="S" runat="server" ImageUrl="../../Images/s2close01.gif"
                        ToolTip="Close"></asp:ImageButton>
                </center>
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        border="1">
        <tr>
            <td valign="top" colspan="1">
                <!--  **********************************************************************-->
                <table style="border-collapse: collapse" width="100%" border="0">
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 100%; height: 420px">
                                <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                    align="left" border="0">
                                    <tr>
                                        <td nowrap="nowrap" >
                                            <asp:Panel ID="Panel1" runat="server">
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left">
                                            <!--  **********************************************************************-->
                                            <asp:DataGrid ID="GrdAddSerach" runat="server" ForeColor="MidnightBlue" Font-Names="Verdana"
                                                BorderWidth="1px" BorderStyle="None" BorderColor="Silver" DataKeyField="TmpID"
                                                CssClass="Grid" PageSize="50" HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0">
                                                <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="GridItem" BackColor="White">
                                                </ItemStyle>
                                                <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                </HeaderStyle>
                                                <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid><!-- Panel for displaying Task Info -->
                                            <!-- Panel for displaying Action Info-->
                                            <!-- ***********************************************************************-->
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="PanelUpdate" runat="server">
                                <ContentTemplate>
                                    <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                                    <asp:Panel ID="pnlMsg" runat="server">
                                    </asp:Panel>
                                    <input type="hidden" name="txthidden">
                                    <input type="hidden" name="txthiddenImage">
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
