<%@ page language="VB" autoeventwireup="false" inherits="Help_AddHelp, App_Web_pszgyzgn" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AddHelp</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="javascript" src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
		function Reset()
		{
			document.Form1.reset();
			return false;
		}
			function SaveEdit(varImgValue)
				{
								
						if (varImgValue=='Logout')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
						}
				}
				
				 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	

		
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" method="post" runat="server">
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BorderWidth="0px" BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None">
                                                    </asp:Button><asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px"
                                                        Width="1px" AlternateText="." CommandName="submit" ImageUrl="white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelAddHelp" runat="server" CssClass="TitleLabel" BorderStyle="None">Add WSS Help</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../Images/S2Save01.gif">
                                                        </asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>&nbsp;<img src="../Images/s2close01.gif" title="Close"
                                                                alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 5%" background="../images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table style="border-collapse: collapse" width="100%" border="0">
                                    <tr>
                                        <td colspan="2">
                                            <!-- *****************************************-->
                                            <cc1:CollapsiblePanel ID="cpnlHelp" runat="server" Width="100%" BorderWidth="0px"
                                                BorderStyle="Solid" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                                                TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Add Help File"
                                                ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
                                                Draggable="False">
                                                <table>
                                                    <tr>
                                                        <td style="width: 26px; height: 14px">
                                                            <asp:Label ID="lblScreenName" Width="96px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                Font-Bold="True" ForeColor="Teal" runat="server">Screen Name</asp:Label>
                                                        </td>
                                                        <td style="height: 14px">
                                                            <asp:DropDownList ID="ddlScrID" Width="152px" Height="18" Font-Size="xx-small" runat="server"
                                                                AutoPostBack="True" Font-Name="verdana">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label2" Width="59px" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                ForeColor="Teal" runat="server">Screen ID</asp:Label>
                                                            <asp:TextBox ID="txtScreenID" Width="60px" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                CssClass="txtNoFocus" runat="server" MaxLength="200" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblHelpFile" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                            ForeColor="Teal" runat="server">Help File</asp:Label><br>
                                                                        <asp:TextBox ID="txtHelpFile" Width="416px" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                            CssClass="txtNoFocus" runat="server" MaxLength="200"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblHelpTitle" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                            ForeColor="Teal" runat="server">Help Title</asp:Label><br>
                                                                        <asp:TextBox ID="txtHelpTitle" Width="416px" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                            CssClass="txtNoFocus" runat="server" MaxLength="1000"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br>
                                                            <br>
                                                            <br>
                                                            <br>
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
                <asp:Panel ID="pnlMsg" runat="server">
                </asp:Panel>
                <asp:ListBox ID="lstError" runat="server" Width="100px" BorderStyle="Groove" BorderWidth="0"
                    Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                <input type="hidden" name="txthiddenImage" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
