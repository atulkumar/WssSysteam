<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StatusUDC.aspx.vb" Inherits="AdministrationCenter_StatusUDC_StatusUDC" MaintainScrollPositionOnPostback="true" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>STATUS UDC</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script language="javascript" src="../../DateControl/ION.js"></script>

    <script language="javascript" src="../../Images/Js/TaskViewShortCuts.js"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script type="text/javascript" language="javascript">


			var globleID;
var rand_no = Math.ceil(500*Math.random())

			function DLLChangeSCR(scrID,comID)
			{
				var com;
				com=document.getElementById(comID).options(document.getElementById(comID).selectedIndex).text;
				//if( com != "" )
			//	{
				document.Form1.submit();
				
				//}
			}
			
	
			function DLLChangeCOM(scrID,comID)
			{
				var scr;
				scr=document.getElementById(scrID).options(document.getElementById(scrID).selectedIndex).text;
			//	if( scr != "" )
			//	{
				document.Form1.submit();
			//	}
			}

	
									
			function callrefresh()
				{
			//alert("hello");
					//location.href="../../SupportCenter/CallView/Task_View.aspx";
					document.Form1.txthiddenImage.value='';
					Form1.submit();
					//location.href="../../SupportCenter/CallView/Task_View.aspx";
				}
								
								
				
			function SaveEdit(varImgValue)
				{
					
			    			if (varImgValue=='Edit')
												{
												
															if (document.Form1.txthidden.value=='0')
															{
																alert("Please select the Row");
																return false;
															}
															else
															{
															var ID;
															var compID;
															var scrID;
															var compName;
															var scrName;
															ID=document.Form1.txthidden.value;
															compID=document.Form1.txthiddenCompID.value;
															scrID=document.Form1.txthiddenScrID.value;
															compName=document.Form1.txthiddenCompName.value;
															scrName=document.Form1.txthiddenScrName.value;
															//alert(ID);
																if ( ID>4)
																{
																	wopen('StatusUDCEdit.aspx?ID='+ID+'&compID='+compID+'&scrID='+scrID+'&compName='+compName +'&scrName='+scrName, 'StatusUDCEdit'+rand_no, 450,300)	
																}
																else
																{
																	alert("You cannot Edit this Record!");
																}
															return false;
															}
															
												}	
												
												if (varImgValue=='Close')
												{
															window.close();	
												}
								
						
								if (varImgValue=='Delete')
								{

												if ( document.Form1.txthidden.value!='0' )
												{
												
														if ( document.Form1.txthidden.value > 4)
														{
															var res;
																res=confirm('Are you sure you want to delete the selected UDC?')
																if(res==true)
																{
																		document.Form1.txthiddenImage.value=varImgValue;
																		Form1.submit(); 
																}
														}
														else
														{
															alert("You cannot Delete this Record!");
														}
												}
												
												else
												{
													alert("Please select a row");
												}
												
												
									return false;
									
								}	
								
								if (varImgValue=='Save')
								{
									//alert(varImgValue);
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
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
														}		
												return false;
									}			
				}				
				
			
				
				function KeyCheck(rowvalues,statuscode,ID,compID,scrID,compName,scrName,statusName)
				{
						//alert(ID);
						//globleID = col;
						//alert(ID);
						//alert(col);
						
						document.getElementById('txthiddenStatusName').value=statusName;
					
						document.Form1.txthiddenCompID.value=compID;
						document.Form1.txthiddenScrID.value=scrID;
						document.Form1.txthiddenCompName.value=compName;
						document.Form1.txthiddenScrName.value=scrName;
						
/*						document.Form1.txtTask.value=nn;
						document.Form1.txthiddenTable.value=tableID;
						document.Form1.txtrowvalues.value=rowvalues;
						document.Form1.txtComp.value=Comp;
						
						//Form1.submit();
	*/					
						document.Form1.txthidden.value=ID;
						var tableID='cpnlStatusUDCDetail_grdStatusUDC'  //your datagrids id
										
						var table;
										
							if (document.all) table=document.all[tableID];
								if (document.getElementById) table=document.getElementById(tableID);
								if (table)
								{
										if(rowvalues>4)
										{
														for ( var i = 1 ;  i < table.rows.length ;  i++)
														{	
																if (i>4)
																{
																document.Form1.txthidden.value=ID;
																			if(i % 2 == 0)
																			{
																				table.rows [ i ] . style . backgroundColor = "#f5f5f5";
																			}
																			else
																			{
																			
																				table.rows [ i ] . style . backgroundColor = "#ffffff";
																			}
																	}
														}
										}
										else
										{
													table.rows [ rowvalues ] . style . backgroundColor = "#D7F1D7";
										}
										
										if(rowvalues>4)
										{
												table.rows [ rowvalues  ] . style . backgroundColor = "#d4d4d4";
										}
								}
								
					}	
					
					function KeyCheck55(ID,compID,scrID,compName,scrName)
					{
							//	alert(ID);
								if ( ID>4)
								{
									wopen('StatusUDCEdit.aspx?ID='+ID+'&compID='+compID+'&scrID='+scrID+'&compName='+compName +'&scrName='+scrName, 'StatusUDCEdit'+rand_no, 450,300)	
								}
								else
								{
									alert("You cannot Edit this Record!");
								}
					}	
					
					
		function wopen(url, name, w, h)
			{
				// Fudge factors for window decoration space.
				// In my tests these work well on all platforms & browsers.
				w += 32;
				h += 96;
				wleft = (screen.width - w) / 2;
				wtop = (screen.height - h) / 2;
				var win = window.open (url,
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
			 function onEnd() {
        var x = document.getElementById('cpnlStatusUDC_collapsible').cells[0].colSpan = "1";
        var y = document.getElementById('cpnlStatusUDCDetail_collapsible').cells[0].colSpan = "1";
    } 
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr width="100%">
                        <td background="../../Images/top_nav_back.gif" height="47">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 15%">
                                        <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px"
                                            ImageUrl="~/images/white.GIF" CommandName="submit" AlternateText="."></asp:ImageButton>
                                        <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                            BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                        <asp:Label ID="lblTitleLabelStatusUdc" runat="server" BorderStyle="None" CssClass="TitleLabel">STATUS UDC</asp:Label>
                                    </td>
                                    <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                        <center>
                                            <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                                                ToolTip="Save"></asp:ImageButton>
                                            <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                AlternateText="Edit" ToolTip="Edit"></asp:ImageButton>
                                            <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                AlternateText="Reset" ToolTip="Reset"></asp:ImageButton>
                                            <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                AlternateText="Delete" ToolTip="Delete"></asp:ImageButton>
                                            <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                style="cursor: hand;" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                            height="47">
                            <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('541','../../');"
                                alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                    class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                    src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                    border="0">
                                    <tr>
                                        <td valign="top" colspan="1">
                                            <!--  **********************************************************************-->
                                            <table width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td colspan="2">
                                                            <cc1:CollapsiblePanel ID="cpnlError" runat="server" Width="100%" Height="47px" BorderStyle="Solid"
                                                                BorderWidth="0px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                                ExpandImage="../../Images/ToggleDown.gif" Text="Error Message" TitleBackColor="Transparent"
                                                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                Visible="False" BorderColor="Indigo">
                                                                <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td colspan="0" rowspan="0">
                                                                            <asp:Image ID="ImgError" runat="server" ImageUrl="../../Images/warning.gif" Height="16px"
                                                                                Width="16px"></asp:Image>
                                                                        </td>
                                                                        <td colspan="0" rowspan="0">
                                                                            <asp:ListBox ID="lstError" runat="server" Width="752px" ForeColor="Red" Font-Names="Verdana"
                                                                                Font-Size="XX-Small" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </cc1:CollapsiblePanel>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <table id="Table165" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td valign="top">
                                                            <cc1:CollapsiblePanel ID="cpnlStatusUDC" runat="server" Width="100%" Height="47px"
                                                                BorderStyle="Solid" BorderWidth="0px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                                ExpandImage="../../Images/ToggleDown.gif" Text="Status UDC" TitleBackColor="Transparent"
                                                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                Visible="True" BorderColor="Indigo">
                                                                <table cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblScreen" runat="server" Height="10px" Width="81px" ForeColor="DimGray"
                                                                                Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Select Screen</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlScreen" AutoPostBack="true" Height="18px" Width="120px" Font-Size="xx-small"
                                                                                runat="server">
                                                                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Value="3">Call Detail</asp:ListItem>
                                                                                <asp:ListItem Value="464">Task Edit</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;&nbsp;
                                                                            <asp:Label ID="Label3" runat="server" Height="8px" Width="104px" ForeColor="DimGray"
                                                                                Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Select Company</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlCompany" AutoPostBack="true" Height="18px" Width="120px" Font-Size="xx-small"
                                                                                Font-Name="vardana" runat="server">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </cc1:CollapsiblePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <cc1:CollapsiblePanel ID="cpnlStatusUDCDetail" runat="server" Height="47px" Width="100%"
                                                                BorderWidth="0px" BorderStyle="Solid" BorderColor="Indigo" Visible="True" TitleCSS="test"
                                                                PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                                                Text="Status UDC Detail" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                                Draggable="False">
                                                                <table cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DataGrid ID="grdStatusUDC" Font-Names="Verdana" Font-Size="X-Small" BorderColor="silver"
                                                                                runat="server" DataKeyField="SU_NU9_ID_PK" AutoGenerateColumns="False" ShowFooter="True">
                                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                                <Columns>
                                                                                    <asp:TemplateColumn HeaderText="S.No.">
                                                                                        <ItemTemplate>
                                                                                            <%# container.dataitem("SU_NU9_ID_PK")  %>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ReadOnly="True" ID="txtSNo" runat="server" Height="18px" Width="0px"
                                                                                                ></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn HeaderText="Status Name">
                                                                                        <ItemTemplate>
                                                                                            <%# container.dataitem("SU_VC50_Status_Name")  %>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox MaxLength="8" ID="txtStatusName" runat="server" Height="18px" Width="200px"
                                                                                                BackColor="#D7E3F3" ></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn HeaderText="Description">
                                                                                        <ItemTemplate>
                                                                                            <%# container.dataitem("SU_VC500_Status_Description")  %>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox MaxLength="500" ID="txtDescription" runat="server" Height="18px" Width="480"
                                                                                              BackColor="#D7E3F3"  ></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn HeaderText="Status Code">
                                                                                        <ItemTemplate>
                                                                                            <%# container.dataitem("SU_NU9_Status_Code")  %>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox MaxLength="2" EnableViewState="True" ID="txtStatusCode" runat="server" BackColor="#D7E3F3"
                                                                                                Height="18px" Width="100px" ></asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                </Columns>
                                                                            </asp:DataGrid>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </cc1:CollapsiblePanel>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
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
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthidden" value="0" />
                        <input type="hidden" name="txthiddenCompID" />
                        <input type="hidden" name="txthiddenScrID" />
                        <input type="hidden" name="txthiddenCompName" />
                        <input type="hidden" name="txthiddenScrName" />
                        <input type="hidden" name="txthiddenStatusName" id="txthiddenStatusName" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
