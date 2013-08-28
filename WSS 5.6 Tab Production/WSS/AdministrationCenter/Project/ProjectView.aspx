<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProjectView.aspx.vb" Inherits="AdministrationCenter_Project_ProjectView"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SubCategory Search</title>
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

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript">

			var globleID;
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
					parent.document.all("SideMenu1").cols="14%,*";					
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
					location.href="../../AdministrationCenter/addressbook/ProjectView.aspx?ScrID=40";
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
																var ProjectID=document.Form1.txthiddenProjID.value;
																var CompID=document.Form1.txthiddenCompID.value;
																var screenid = window.parent.getActiveTabDetails();
												            	window.parent.OpenTabOnDBClick('SubCategory Detail#' + ProjectID,"AdministrationCenter/Project/ProjectMasterDetail.aspx?ScrID=670&ProjectID="+ ProjectID +"&CompanyID="+CompID, 'SubCategory Detail#' + ProjectID,screenid);
																//Form1.submit(); 
																return false;
															}
															
												}	
												
						if (varImgValue=='Close')
												{
															window.close();	
												}
								
								
								if (varImgValue=='Add')
												{
												
													document.Form1.txthiddenImage.value=varImgValue;
													var CompID=document.Form1.txthiddenCompID.value;
													window.parent.OpenTabOnAddClick('SubCategory Detail',"AdministrationCenter/Project/ProjectMasterDetail.aspx?ScrID=670&ProjectID=-1&CompanyID="+CompID, "670");
													//Form1.submit();
														return false;
													  
												}	
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									//Form1.submit(); 
									__doPostBack("up2","");
										return false;
								}	
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
										return false;
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
				
				
				function KeyCheck(nn,rowvalues,CompID)
					{
						//alert(rowvalues);
						globleID = nn;
						document.Form1.txthidden.value=nn;
			            document.Form1.txthiddenCompID.value=CompID;
			            document.Form1.txthiddenProjID.value =nn;  			
						//Form1.submit();
						
										var tableID='up1_GrdAddSerach'  //your datagrids id
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
					
					function KeyCheck55(nn,rowvalues,CompID)
					{
							document.Form1.txthiddenCompID.value=CompID;
							document.Form1.txthiddenProjID.value=nn;							
							
							document.Form1.txthiddenImage.value='Edit';
							SaveEdit('Edit');  
					}	
					
			function OpenW(varTable)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('../AddressBook/AB_ViewColumns.aspx?ScrID=40&TBLName=T210011','Search'+rand_no,480,440);
				return false;
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

    <script type="text/javascript" language="javascript">
		    //A Function to improve design i.e delete the extra cell of table
		    function onEnd() {
		        var x = document.getElementById('up1_collapsible').cells[0].colSpan = "1";
		    }
		    //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
            
        }	
 
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BackColor="#8AAFE5"
                                                        BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                        AlternateText="." CommandName="submit" ImageUrl="~/images/white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelProject" runat="server" Height="12px" Width="111px" ForeColor="Teal"
                                                        Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">SubCategory</asp:Label>
                                                </td>
                                                <td style="width: 60%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:UpdatePanel ID="upnltop" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../../Images/s2Add01.gif"
                                                                    ToolTip="Add"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                                    ToolTip="Edit"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                                    ToolTip="Reset"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                                    ToolTip="Search"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                                    ToolTip="Delete"></asp:ImageButton>
                                                                <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                    style="cursor: hand;" />&nbsp;
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </center>
                                                </td>
                                                <td style="width: 15%">
                                                    <font face="Verdana" size="1"><strong>&nbsp;&nbsp;<font face="Verdana" size="1"><strong>View&nbsp;
                                                        <asp:DropDownList ID="ddlstview" runat="server" Width="80px" Font-Names="Verdana"
                                                            Font-Size="XX-Small" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:ImageButton ID="imgPlus" AccessKey="P" runat="server" ImageUrl="../../Images/plus.gif">
                                                        </asp:ImageButton></strong></font>&nbsp;&nbsp;</strong></font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('40','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <tr>
                                <td>
                                    <!--  **********************************************************************-->
                                    <div style="overflow: auto; width: 100%; height: 100%">
                                        <table width="100%" border="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="up2" runat="server">
                                                            <ContentTemplate>
                                                                <cc1:CollapsiblePanel ID="up1" runat="server" Width="100%" Height="530px" BorderWidth="0px"
                                                                    BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                                    ExpandImage="../../Images/ToggleDown.gif" Text="SubCategory" TitleBackColor="transparent"
                                                                    TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                    Visible="true" BorderColor="Indigo">
                                                                    <div style="overflow: auto; width: 1000px; height: 350pt">
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
                                                                                        BorderWidth="1px" BorderStyle="None" BorderColor="Silver" CssClass="Grid" PageSize="25"
                                                                                        PagerStyle-Visible="False" HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0"
                                                                                        DataKeyField="SubCategory" AllowSorting="True" AllowPaging="True">
                                                                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                        <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="GridItem" BackColor="White">
                                                                                        </ItemStyle>
                                                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                                        </HeaderStyle>
                                                                                        <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                        <PagerStyle Visible="False" Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                                    </asp:DataGrid>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </cc1:CollapsiblePanel>
                                                                <asp:Panel ID="Panel6" runat="server">
                                                                    <asp:Panel ID="Panel7" runat="server">
                                                                        <table height="25">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="pg" Width="40px" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                        Font-Bold="True" ForeColor="#0000C0" runat="server">Page</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="CurrentPg" runat="server" Width="10px" Height="12px" Font-Size="X-Small"
                                                                                        Font-Bold="True" ForeColor="Crimson"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="of" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" ForeColor="#0000C0"
                                                                                        runat="server">of</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="TotalPages" runat="server" Width="10px" Height="12px" Font-Size="X-Small"
                                                                                        Font-Bold="True" ForeColor="Crimson"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="Firstbutton" runat="server" ImageUrl="../../Images/next9.jpg"
                                                                                        AlternateText="First" ToolTip="First"></asp:ImageButton>
                                                                                </td>
                                                                                <td width="14">
                                                                                    <asp:ImageButton ID="Prevbutton" runat="server" ImageUrl="../../Images/next99.jpg"
                                                                                        ToolTip="Previous"></asp:ImageButton>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="Nextbutton" runat="server" ImageUrl="../../Images/next9999.jpg"
                                                                                        ToolTip="Next"></asp:ImageButton>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="Lastbutton" runat="server" ImageUrl="../../Images/next999.jpg"
                                                                                        ToolTip="Last"></asp:ImageButton>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtPageSize" runat="server" Width="24px" Height="14px" Font-Size="7pt"
                                                                                        MaxLength="3"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="Button3" runat="server" Width="16px" Height="12pt" BorderStyle="None"
                                                                                        Font-Size="7pt" Font-Bold="True" ForeColor="Navy" ToolTip="Change Paging Size"
                                                                                        Text=">"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblrecords" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                        Font-Bold="True" ForeColor="MediumBlue">Total Records</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="TotalRecods" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                        Font-Bold="True" ForeColor="Crimson"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </asp:Panel>
                                                                <span style="display: none">
                                                                    <asp:Button ID="BtnGrdSearch1" runat="server" Height="0" Width="0" BorderWidth="0px" />
                                                                </span>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <br />
                                    </div>
                                </td>
                            </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="752px" BorderStyle="Groove" BorderWidth="0"
                            Font-Size="XX-Small" Visible="false" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthiddenCompID" />
                        <input type="hidden" name="txthiddenProjID" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
