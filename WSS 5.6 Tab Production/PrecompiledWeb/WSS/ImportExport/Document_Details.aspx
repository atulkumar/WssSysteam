<%@ page language="VB" autoeventwireup="false" inherits="ImportExport_Document_Details, App_Web_p89p0wkj" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../images/Js/JSValidation.js"></script>

    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../../DateControl/ION.js"></script>

    <script language="javascript" src="../images/Js/TaskViewShortCuts.js"></script>

    <script language="javascript" src="../DateControl/ION.js"></script>

    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script>

	 
		//********************************************************************
		
			function CheckLength()
		{
				var ADLength=document.getElementById('cpnlviewinfo_txtDescription1').value.length;
				if ( ADLength>0 )
				{
					if ( ADLength > 2000 )
					{
						alert(' Description cannot be more than 2000 characters \n(Current Length :'+ADLength+')');
						return false;
					}
				}		
				return true;
		}
			function CheckReqdFields()
			{
			if(document.getElementById('cpnlviewinfo_txtDescription1').value=='') 
			{
			alert('Please enter File Description');
			return false;
			}
			if(document.getElementById('cpnlviewinfo_FileUpload').value=='') 
			{
			alert('Please Browse File to Upload');
			return false;
			}
			
			}
		   function CheckFile()
			{
			if(document.getElementById('cpnlFileInfo_txtDocumentName').value=='') 
			{
			alert('Please Upload File Before Saving');
			return false;
			}
			}					
				
				
			function SaveEdit(varImgValue)
				{
			    
									if (varImgValue=='Delete')
									{
												var confirmed
												if ( document.Form1.txtRuleID.value=='')
												{
													alert('Please select a Rule to Delete');
												}
												else
												{
														confirmed=window.confirm("Are you sure you want to delete the selected Rule?");
														if(confirmed==true)
														{	
																	document.Form1.txthiddenImage.value=varImgValue;
																	Form1.submit()
														}		
												}
												return false;

									}	
											
				}				
				
			
				
			
		
				
				function KeyCheck(ID,Index)
					{
								document.Form1.txtRuleID.value=ID;
							
									var tableID='cpnlAccessInfo_GrdDocuments';
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
												    table.rows [ Index  ] . style . backgroundColor = "#d4d4d4";
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
				var win = window.open(url,
					name,
					'width=' + w + ', height=' + h + ', ' +
					'left=' + wleft + ', top=' + wtop + ', ' +
					'location=no, menubar=no, ' +
					'status=no, toolbar=no, scrollbars=yes, resizable=no');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}
			
				
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" onload="Hideshow();" rightmargin="0"
    ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <div align="right">
                                <img height="2" src="../Images/top_right_line.gif" width="96"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../Images/top_left_back.gif">
                                        &nbsp;
                                    </td>
                                    <td width="50">
                                        <img height="20" src="../Images/top_right.gif" width="50">
                                    </td>
                                    <td width="21">
                                        <a href="#">
                                            <img height="20" src="../Images/bt_min.gif" width="21" border="0"></a>
                                    </td>
                                    <td width="21">
                                        <a href="#">
                                            <img height="20" src="../Images/bt_max.gif" width="21" border="0"></a>
                                    </td>
                                    <td width="19">
                                        <a href="#">
                                            <img onclick="CloseWSS();" height="20" src="../Images/bt_clo.gif" width="19" border="0"></a>
                                    </td>
                                    <td width="6">
                                        <img height="20" src="../Images/bt_space.gif" width="6">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../images/top_nav_back.gif" height="67">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 356px">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:Button><asp:ImageButton
                                                        ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="white.GIF"
                                                        CommandName="submit" AlternateText="."></asp:ImageButton><img class="PlusImageCSS"
                                                            onclick="HideContents();" alt="Hide" src="../images/left005.gif" name="imgHide">
                                                    <img class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../images/Right005.gif"
                                                        name="ingShow">
                                                    <asp:Label ID="lblTitleLabelUserOV" runat="server" Width="184px" Height="12px" BorderStyle="None"
                                                        BorderWidth="2px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal">Document Detail</asp:Label>
                                                </td>
                                                <td>
                                                    <img title="Seperator" alt="R" src="../images/00Seperator.gif" border="0">
                                                    <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../images/S2Save01.gif"
                                                        AlternateText="Save"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../Images/s2delete01.gif"
                                                        ToolTip="Delete"></asp:ImageButton>&nbsp;<asp:ImageButton ID="imgClose" AccessKey="L"
                                                            runat="server" ImageUrl="../Images/s2close01.gif" ToolTip="Close"></asp:ImageButton>&nbsp;<img
                                                                title="Seperator" alt="R" src="../images/00Seperator.gif" border="0">&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td style="width: 14px">
                                                    &nbsp; </STRONG></FONT>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../images/top_nav_back01.gif" height="67">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('954','../');"
                                            alt="E" src="../images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <tr>
                                <td height="10">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line.gif" height="10">
                                                <img height="10" src="../Images/main_line.gif" width="6">
                                            </td>
                                            <td width="7" height="10">
                                                <img height="10" src="../Images/main_line01.gif" width="7">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="2">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line02.gif" height="2">
                                                <img height="2" src="../Images/main_line02.gif" width="2">
                                            </td>
                                            <td width="12" height="2">
                                                <img height="2" src="../Images/main_line03.gif" width="12">
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
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <cc1:CollapsiblePanel ID="cpnlErrorPanel" runat="server" Width="100%" Height="47px"
                                                                                    BorderStyle="Solid" BorderWidth="0px" BorderColor="Indigo" Visible="False" TitleCSS="test"
                                                                                    PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                                                                    Text="Error Message" ExpandImage="../images/ToggleDown.gif" CollapseImage="../images/ToggleUp.gif"
                                                                                    Draggable="False">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Image ID="Image1" runat="server" Height="16px" Width="16px" ImageUrl="../images/warning.gif">
                                                                                                </asp:Image>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ListBox ID="lstError" runat="server" Width="560px" ForeColor="Red" Font-Names="Verdana"
                                                                                                    Font-Size="XX-Small" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </cc1:CollapsiblePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <cc1:CollapsiblePanel ID="cpnlviewinfo" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                                                                    BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                                    TitleClickable="True" TitleBackColor="Transparent" Text="Upload New File" ExpandImage="../images/ToggleDown.gif"
                                                                                    CollapseImage="../images/ToggleUp.gif" Draggable="False">
                                                                                    <table id="Table3" cellspacing="0" cellpadding="0" border="0">
                                                                                        <tr align="left">
                                                                                            <td valign="top">
                                                                                                <table id="Table1" bordercolor="#5c5a5b" bgcolor="#f5f5f5" border="1">
                                                                                                    <tr>
                                                                                                        <td bordercolor="#f5f5f5">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                        <td bordercolor="#f5f5f5">
                                                                                                            <asp:Label ID="Label9" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                                Font-Size="XX-Small">Company</asp:Label><br>
                                                                                                            <asp:DropDownList ID="DDLFileComp" runat="server" Width="153px" AutoPostBack="True"
                                                                                                                CssClass="txtNoFocus">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td bordercolor="#f5f5f5">
                                                                                                            <asp:Label ID="lblViewType" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                                Font-Size="XX-Small">Group</asp:Label><br>
                                                                                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="153px" CssClass="txtNoFocus">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td bordercolor="#f5f5f5">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td bordercolor="#f5f5f5">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                        <td bordercolor="#f5f5f5" colspan="2">
                                                                                                            <asp:Label ID="Label15" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                                Font-Size="XX-Small">Description</asp:Label><br>
                                                                                                            <asp:TextBox ID="txtDescription1" runat="server" Height="80px" Width="312px" Font-Names="Verdana"
                                                                                                                Font-Size="XX-Small" BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus"
                                                                                                                TextMode="MultiLine" MaxLength="256"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td bordercolor="#f5f5f5">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td bordercolor="#f5f5f5">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                        <td bordercolor="#f5f5f5" colspan="2">
                                                                                                            <asp:Label ID="Label5" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                                Font-Size="XX-Small">Document URL</asp:Label><br>
                                                                                                            <input id="FileUpload" style="width: 248px; height: 22px" type="file" size="22" name="FileUpload"
                                                                                                                runat="server">
                                                                                                            <asp:Button ID="Button1" runat="server" Text="Upload"></asp:Button>
                                                                                                        </td>
                                                                                                        <td bordercolor="#f5f5f5">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </cc1:CollapsiblePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <cc1:CollapsiblePanel ID="cpnlFileInfo" runat="server" Height="103px" BorderStyle="Solid"
                                                                                    BorderWidth="0px" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                                                                                    TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="File Info"
                                                                                    ExpandImage="../images/ToggleDown.gif" CollapseImage="../images/ToggleUp.gif"
                                                                                    Draggable="False">
                                                                                    <table id="Tables6" bordercolor="#5c5a5b" width="580" bgcolor="#f5f5f5" border="1">
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label10" runat="server" Width="72px" ForeColor="DimGray" Font-Bold="True"
                                                                                                    Font-Names="Verdana" Font-Size="XX-Small">Company</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCompanyName" runat="server" Width="200px" CssClass="txtNoFocus"
                                                                                                    BackColor="WhiteSmoke" ReadOnly="True"></asp:TextBox>
                                                                                                <asp:TextBox ID="txtCompanyID" runat="server" Width="0px" CssClass="txtNoFocus" BackColor="WhiteSmoke"
                                                                                                    ReadOnly="True"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" rowspan="5">
                                                                                                <asp:Label ID="Label6" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                    Font-Size="XX-Small">Description</asp:Label><br>
                                                                                                <asp:TextBox ID="txtDocDescription" runat="server" Height="143px" Width="340px" Font-Names="Verdana"
                                                                                                    Font-Size="XX-Small" BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus"
                                                                                                    TextMode="MultiLine" MaxLength="295"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="height: 23px" bordercolor="#f5f5f5">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <tr>
                                                                                                <td style="height: 21px" bordercolor="#f5f5f5">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td style="height: 21px" bordercolor="#f5f5f5">
                                                                                                    <asp:Label ID="Label7" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                        Font-Size="XX-Small">File Name</asp:Label><br>
                                                                                                    <asp:TextBox ID="txtDocumentName" runat="server" Height="18px" Width="200px" Font-Names="Verdana"
                                                                                                        Font-Size="XX-Small" BorderWidth="1px" BorderStyle="Solid" CssClass="txtNoFocus"
                                                                                                        MaxLength="250" BackColor="WhiteSmoke" ReadOnly="True"></asp:TextBox>
                                                                                                </td>
                                                                                                <td style="height: 23px" bordercolor="#f5f5f5">
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td bordercolor="#f5f5f5">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td bordercolor="#f5f5f5">
                                                                                                    <asp:Label ID="lblupload" runat="server" Width="72px" ForeColor="DimGray" Font-Bold="True"
                                                                                                        Font-Names="Verdana" Font-Size="XX-Small">Group Name</asp:Label><br>
                                                                                                    <asp:DropDownList ID="DDLFileGroup" runat="server" Width="200px" CssClass="txtNoFocus">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtFilepath" runat="server" Width="0px"></asp:TextBox>
                                                                                                    <asp:TextBox ID="txtFID" runat="server" Width="0px"></asp:TextBox>
                                                                                                </td>
                                                                                                <td style="height: 23px" bordercolor="#f5f5f5">
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td bordercolor="#f5f5f5">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td bordercolor="#f5f5f5">
                                                                                                    <asp:Label ID="Label8" Text="Version" runat="server"></asp:Label><br>
                                                                                                    <asp:TextBox ID="txtVersion" runat="server" Height="18px" Width="200px" CssClass="txtNoFocus"
                                                                                                        ReadOnly="True"></asp:TextBox>
                                                                                                </td>
                                                                                                <td style="height: 23px" bordercolor="#f5f5f5">
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td bordercolor="#f5f5f5">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td bordercolor="#f5f5f5">
                                                                                                    <asp:Label ID="Label11" Text="File Size" runat="server"></asp:Label><br>
                                                                                                    <asp:TextBox ID="txtFileSize" runat="server" Height="18px" Width="200px" CssClass="txtNoFocus"
                                                                                                        ReadOnly="True"></asp:TextBox>
                                                                                                </td>
                                                                                                <td bordercolor="#f5f5f5">
                                                                                                    <asp:HyperLink ID="hypFile" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                                                        Font-Size="XX-Small" Target="_blank"></asp:HyperLink>
                                                                                                </td>
                                                                                                <td style="height: 23px" bordercolor="#f5f5f5">
                                                                                                </td>
                                                                                            </tr>
                                                                                    </table>
                                                                                </cc1:CollapsiblePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <cc1:CollapsiblePanel ID="cpnlFilePermissions" runat="server" Height="302px" BorderStyle="Solid"
                                                                                    BorderWidth="0px" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                                                                                    TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="File Permissions"
                                                                                    ExpandImage="../images/ToggleDown.gif" CollapseImage="../images/ToggleUp.gif"
                                                                                    Draggable="False">
                                                                                    <table id="Table6" bordercolor="#5c5a5b" width="580" bgcolor="#f5f5f5" border="1">
                                                                                        <tr>
                                                                                            <td style="height: 21px" bordercolor="#f5f5f5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td style="height: 21px" bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label4" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                    Font-Size="XX-Small">Company Name</asp:Label><br>
                                                                                                <asp:DropDownList ID="DDLCompany" runat="server" Width="160px" AutoPostBack="True"
                                                                                                    CssClass="txtNoFocus">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td style="height: 21px" bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label1" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                    Font-Size="XX-Small">Status</asp:Label><br>
                                                                                                <asp:DropDownList ID="ddlStatus" runat="server" Width="160px" AutoPostBack="false"
                                                                                                    CssClass="txtNoFocus">
                                                                                                    <asp:ListItem Value="ENB" Selected="True">Enable</asp:ListItem>
                                                                                                    <asp:ListItem Value="DSB">Disable</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td style="height: 23px" bordercolor="#f5f5f5">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label2" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                    Font-Size="XX-Small">Date From</asp:Label><br>
                                                                                                <SControls:DateSelector ID="dtValidFromDate" runat="server" Width="160px" Text="Start Date:">
                                                                                                </SControls:DateSelector>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label3" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                                                                                                    Font-Size="XX-Small">Date To</asp:Label><br>
                                                                                                <SControls:DateSelector ID="dtValidToDate" runat="server" Width="160px" Text="Start Date:">
                                                                                                </SControls:DateSelector>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="height: 21px" bordercolor="#f5f5f5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td style="height: 21px" bordercolor="#f5f5f5" colspan="2">
                                                                                                <asp:Label ID="lblRoleId" runat="server" Width="80px" ForeColor="DimGray" Font-Bold="True"
                                                                                                    Font-Names="Verdana" Font-Size="XX-Small">Access Level</asp:Label><br>
                                                                                                <asp:DropDownList ID="DLLObjectType" runat="server" Width="160px" AutoPostBack="True"
                                                                                                    CssClass="txtNoFocus">
                                                                                                    <asp:ListItem Value="ROLE" Selected="True">Role Level</asp:ListItem>
                                                                                                    <asp:ListItem Value="USER">User Level</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td style="height: 23px" bordercolor="#f5f5f5">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td valign="top" bordercolor="#f5f5f5" colspan="2">
                                                                                                <div style="border-right: 1px solid; border-top: 1px solid; overflow: auto; border-left: 1px solid;
                                                                                                    width: 160px; border-bottom: 1px solid; height: 100px; background-color: white">
                                                                                                    <table id="Table4">
                                                                                                        <tr>
                                                                                                            <td valign="top">
                                                                                                                <asp:CheckBoxList ID="cblObjects" runat="server" Height="10px" Width="135px" Font-Size="Smaller"
                                                                                                                    RepeatLayout="Table" RepeatColumns="1">
                                                                                                                </asp:CheckBoxList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </cc1:CollapsiblePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <cc1:CollapsiblePanel ID="cpnlAccessInfo" runat="server" Height="135px" BorderStyle="Solid"
                                                                                    BorderWidth="0px" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                                                                                    TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="File Access Info"
                                                                                    ExpandImage="../images/ToggleDown.gif" CollapseImage="../images/ToggleUp.gif"
                                                                                    Draggable="False">
                                                                                    <table id="Table2" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                                                        align="left" border="0">
                                                                                        <tr>
                                                                                            <td valign="top" align="left">
                                                                                                <asp:DataGrid ID="GrdDocuments" runat="server" ForeColor="MidnightBlue" Font-Names="Verdana"
                                                                                                    BorderWidth="1px" BorderStyle="None" BorderColor="Silver" CssClass="grid" DataKeyField="RuleID"
                                                                                                    HeaderStyle-Height="18px" AutoGenerateColumns="False" CellPadding="0" GridLines="Horizontal"
                                                                                                    HorizontalAlign="Left" PageSize="50">
                                                                                                    <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                                    <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                                                                    <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                                                                    </ItemStyle>
                                                                                                    <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                                                    </HeaderStyle>
                                                                                                    <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                                                    <Columns>
                                                                                                        <asp:BoundColumn DataField="ObjectType" HeaderText="Access&nbsp;Type" ItemStyle-Width="80px"
                                                                                                            ItemStyle-Wrap="True"></asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="Name" HeaderText="Name" ItemStyle-Width="150px" ItemStyle-Wrap="True">
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="ValidFrom" HeaderText="Valid From" ItemStyle-Width="120px"
                                                                                                            ItemStyle-Wrap="True"></asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="ValidTo" HeaderText="Valid UpTo" ItemStyle-Width="120px"
                                                                                                            ItemStyle-Wrap="True"></asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="Status" HeaderText="Status" ItemStyle-Width="100px" ItemStyle-Wrap="True">
                                                                                                        </asp:BoundColumn>
                                                                                                    </Columns>
                                                                                                </asp:DataGrid>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <input type="hidden" name="txthiddenImage">
                                                                                    <input type="hidden" name="txtRuleID">
                                                                                </cc1:CollapsiblePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td valign="top" width="12" background="../Images/main_line04.gif">
                                                    <img height="1" src="../Images/main_line04.gif" width="12">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td height="2">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/main_line06.gif" height="2">
                                                <img height="2" src="../Images/main_line06.gif" width="2">
                                            </td>
                                            <td width="12" height="2">
                                                <img height="2" src="../Images/main_line05.gif" width="12">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td background="../Images/bottom_back.gif">
                                                &nbsp;
                                            </td>
                                            <td width="66">
                                                <img height="31" src="../Images/bottom_right.gif" width="66">
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
