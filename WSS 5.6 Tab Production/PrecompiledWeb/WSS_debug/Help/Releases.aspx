<%@ page language="VB" autoeventwireup="false" inherits="Help_Releases, App_Web_bnj2ix_q" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Releases</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="javascript" src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript" src="../DateControl/ION.js"></script>

    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Images/Js/JSValidation.js">
    </script>

    <script type="text/javascript">
		function Reset()
		{
			document.Form1.reset();
			return false;
		}
			function SaveEdit(varImgValue)
				{
					
					if (varImgValue=='Save')
						
							{
							
										document.Form1.txthiddenImage.value=varImgValue;
										
							}		
					
					if (varImgValue=='Logout')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									//Form1.submit(); 
						}
						
							
					if (varImgValue=='Add')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									//Form1.submit(); 
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
				
					function KeyCheck(rowvalues,ID)
						{
               
									var tableID='cpnlUpdation_DgReleses';
									var table;
									document.Form1.txthiddenID.value=ID;		
								 
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
						
						
		
		
							function KeyCheck56(rowvalues,ID)
					{
					
					
					document.Form1.txtID.value=ID	
					document.Form1.txthiddenImage.value='Edit';
					Form1.submit ();
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
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="94%" align="center" border="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                        AlternateText="." CommandName="submit" ImageUrl="white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelAddHelp" runat="server" CssClass="TitleLabel" BorderStyle="None">Add Updations</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../Images/s2Add01.gif"
                                                            ToolTip="Add"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../Images/S2Save01.gif">
                                                        </asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>&nbsp; &nbsp;<img src="../Images/s2close01.gif"
                                                                title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 5%" background="../images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                            border="0">
                                            <tr>
                                                <td valign="top" colspan="1">
                                                    <!--  **********************************************************************-->
                                                    <div style="overflow: auto; width: 100%; height: 581px">
                                                        <table style="border-collapse: collapse" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <!-- *****************************************-->
                                                                    <cc1:CollapsiblePanel ID="cpnlHelp" runat="server" Width="100%" BorderWidth="0px"
                                                                        BorderStyle="Solid" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                                                                        TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Updations"
                                                                        ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
                                                                        Draggable="False">
                                                                        <table bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="100%" border="1">
                                                                            <tr>
                                                                                <td bordercolor="#f5f5f5">
                                                                                    <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                                        width="100%" align="left" border="0">
                                                                                        <tr>
                                                                                            <td width="28">
                                                                                            </td>
                                                                                            <td width="100">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td style="width: 812px" colspan="5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="28">
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblModule" runat="server" CssClass="FieldLabel">Module</asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 121px" width="121">
                                                                                                <asp:DropDownList ID="ddlModule" runat="server" Width="120px" Font-Size="XX-Small"
                                                                                                    Font-Names="Verdana">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblDate" runat="server" CssClass="FieldLabel">Date</asp:Label>&nbsp;
                                                                                            </td>
                                                                                            <td>
                                                                                                <%--<SControls:DateSelector ID="dtDate" runat="server" Text="Start Date:" Width="80pt">
                                                                                            </SControls:DateSelector>--%>
                                                                                                <ION:Customcalendar ID="dtDate" runat="server" Width="120pt" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">Type</asp:Label>&nbsp;
                                                                                            </td>
                                                                                            <td style="width: 403px">
                                                                                                <asp:DropDownList ID="ddlType" runat="server" Width="120px" Font-Size="XX-Small"
                                                                                                    Font-Names="Verdana">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="28">
                                                                                            </td>
                                                                                            <td width="100">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td style="width: 812px" colspan="5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="28">
                                                                                            </td>
                                                                                            <td width="100">
                                                                                                <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">Subject *</asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 812px" colspan="5">
                                                                                                <asp:TextBox ID="txtSubject" runat="server" Width="566px" CssClass="txtNoFocus" MaxLength="100"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="28">
                                                                                                <font size="1">&nbsp;</font>
                                                                                            </td>
                                                                                            <td width="100">
                                                                                                <font size="1">&nbsp;</font>
                                                                                            </td>
                                                                                            <td style="width: 812px" colspan="5">
                                                                                                <font size="1">&nbsp;</font>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left" width="28">
                                                                                                <!--  **********************************************************************-->
                                                                                            </td>
                                                                                            <td valign="top" align="left" width="100">
                                                                                                <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">Description *</asp:Label><!-- Panel for displaying Task Info -->
                                                                                                <!-- Panel for displaying Action Info-->
                                                                                                <!-- ***********************************************************************-->
                                                                                            </td>
                                                                                            <td style="width: 812px" valign="top" align="left" colspan="5">
                                                                                                <asp:TextBox ID="txtDesc" runat="server" Width="566px" Height="165px" CssClass="txtNoFocus"
                                                                                                    MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left" width="28">
                                                                                                <font size="1">&nbsp;</font>
                                                                                            </td>
                                                                                            <td valign="top" align="left" width="100">
                                                                                                <font size="1">&nbsp;</font>
                                                                                            </td>
                                                                                            <td style="width: 812px" valign="top" align="left" colspan="5">
                                                                                                <font size="1">&nbsp; </font>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </cc1:CollapsiblePanel>
                                                                    <cc1:CollapsiblePanel ID="cpnlUpdation" runat="server" Width="100%" BorderWidth="0px"
                                                                        BorderStyle="Solid" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                                        ExpandImage="../Images/ToggleDown.gif" Text="View Updation" TitleBackColor="Transparent"
                                                                        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                        BorderColor="Indigo" Visible="True">
                                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <div style="overflow: auto; width: 100%; height: 180px; background-color: #f5f5f5">
                                                                                        <asp:DataGrid ID="DgReleses" runat="server" BorderWidth="0" CssClass="Grid" BorderColor="#d4d4d4"
                                                                                            AutoGenerateColumns="False" CellPadding="0" AllowSorting="True" PageSize="20"
                                                                                            PagerStyle-Visible="False" AllowPaging="True">
                                                                                            <SelectedItemStyle CssClass="GridSelectedItem" BackColor="#D4D4D4"></SelectedItemStyle>
                                                                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                            <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                                            </HeaderStyle>
                                                                                            <Columns>
                                                                                                <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                                                                                <asp:TemplateColumn>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:TextBox Width="100%" ID="txtModule" runat="server" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                                        Module
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <%# container.dataitem("ModName")%>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:TextBox Width="100%" ID="txtRelDate" runat="server" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                                        Date
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <%# container.dataitem("RelDate")%>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:TextBox Width="100%" ID="txtRelType" runat="server" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                                        Updation Type
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <%# container.dataitem("RelType")%>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:TextBox Width="100%" ID="txtSub" runat="server" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                                        Subject
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <%# container.dataitem("Sub")%>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:TextBox Width="100%" ID="txtDesc1" runat="server" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                                        Description
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <%# container.dataitem("Description")%>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                            </Columns>
                                                                                        </asp:DataGrid></div>
                                                                                    <asp:Panel ID="Panel5" runat="server">
                                                                                        <asp:Panel ID="Panel6" runat="server">
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
                                                                                                        <asp:ImageButton ID="Firstbutton" runat="server" ImageUrl="../Images/next9.jpg" AlternateText="First"
                                                                                                            ToolTip="First"></asp:ImageButton>
                                                                                                    </td>
                                                                                                    <td width="14">
                                                                                                        <asp:ImageButton ID="Prevbutton" runat="server" ImageUrl="../Images/next99.jpg" ToolTip="Previous">
                                                                                                        </asp:ImageButton>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="Nextbutton" runat="server" ImageUrl="../Images/next9999.jpg"
                                                                                                            ToolTip="Next"></asp:ImageButton>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="Lastbutton" runat="server" ImageUrl="../Images/next999.jpg"
                                                                                                            ToolTip="Last"></asp:ImageButton>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtPageSize" runat="server" Width="24px" Height="14px" Font-Size="7pt"
                                                                                                            MaxLength="3"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Button ID="Button3" runat="server" Width="16px" Height="12pt" BorderStyle="None"
                                                                                                            Font-Size="7pt" Font-Bold="True" ToolTip="Change Paging Size" Text=">" ForeColor="Navy">
                                                                                                        </asp:Button>
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
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </cc1:CollapsiblePanel>
                                                                    <!-- *****************************************-->
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
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
                        <asp:ListBox ID="lstError" runat="server" Width="100px" BorderStyle="Groove" BorderWidth="0"
                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthiddenID" />
                        <input type="hidden" name="txtID" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
