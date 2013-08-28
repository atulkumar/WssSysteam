<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_UDC_CallTypeAccess, App_Web_cj8ho3o2" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Call Type Access</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/CallViewShortCuts.js" type="text/javascript"></script>

    <script type="text/javascript">
                                  
			function SaveEdit(varImgValue)
			{
							
								
								if (varImgValue=='Logout')
								{
										document.Form1.txthiddenImage.value=varImgValue;
										Form1.submit(); 

								}
	
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
									return false;
								}	

								
								if (varImgValue=='Save')
								{
								

										document.Form1.txthiddenImage.value=varImgValue;
										Form1.submit(); 
								}	
							return false;								

				}				
				function KeyCheck(rowvalues)
					{
						var tableID='cpnlCallType_grdCallType';  //your datagrids id
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

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallType_collapsible').cells[0].colSpan = "1";


        } 
         //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
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
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0" Width="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                        AlternateText="." CommandName="submit" ImageUrl="~/images/white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel">Call Type Access</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                        </asp:ImageButton>
                                                        <img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;"
                                                            onclick="javascript:location.reload(true);" />
                                                        <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('670','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
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
                                                            <td width="100%">
                                                                <cc1:CollapsiblePanel ID="cpnlCallType" runat="server" Height="294px" Width="100%"
                                                                    BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                    TitleClickable="True" TitleBackColor="Transparent" Text="Call Type Access" ExpandImage="../../Images/ToggleDown.gif"
                                                                    CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderStyle="Solid"
                                                                    BorderWidth="0px">
                                                                    <div style="overflow: auto; width: 100%; height: 480px">
                                                                        <table cellspacing="0" cellpadding="0" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DataGrid ID="grdCallType" runat="server" CssClass="Grid" AutoGenerateColumns="False">
                                                                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                        <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                                        <HeaderStyle Height="18px" CssClass="GridHeader"></HeaderStyle>
                                                                                        <Columns>
                                                                                            <asp:BoundColumn Visible="False" DataField="CompID" HeaderText="CompID">
                                                                                                <HeaderStyle Width="100px"></HeaderStyle>
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="CallType" HeaderText="Call Type">
                                                                                                <HeaderStyle Width="100px"></HeaderStyle>
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="CallTypeDesc" HeaderText="Description">
                                                                                                <HeaderStyle Width="200px"></HeaderStyle>
                                                                                            </asp:BoundColumn>
                                                                                            <asp:TemplateColumn>
                                                                                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                <HeaderTemplate>
                                                                                                    <asp:Label ID="lblCEAccess_H" runat="server">Call Entry</asp:Label>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkCEAccess" runat="server" Checked='<%#Container.DataItem("CEFlag")%>'>
                                                                                                    </asp:CheckBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn>
                                                                                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                <HeaderTemplate>
                                                                                                    <asp:Label ID="lblCFEAccess_H" runat="server">Call Fast Entry</asp:Label>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkCFEAccess" runat="server" Checked='<%#Container.DataItem("CFEFlag")%>'>
                                                                                                    </asp:CheckBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="CompName" HeaderText="Company">
                                                                                                <HeaderStyle Width="250px"></HeaderStyle>
                                                                                            </asp:BoundColumn>
                                                                                        </Columns>
                                                                                    </asp:DataGrid>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </cc1:CollapsiblePanel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
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
                        <asp:ListBox ID="lstError" runat="server" Width="752px" BorderStyle="Groove" BorderWidth="0"
                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
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
