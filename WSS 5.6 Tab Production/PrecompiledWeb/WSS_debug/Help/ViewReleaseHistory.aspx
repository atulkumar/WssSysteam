<%@ page language="VB" autoeventwireup="false" inherits="Help_ViewReleaseHistory, App_Web_bnj2ix_q" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <title>ViewReleaseHistory</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="javascript" src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript" src="../DateControl/ION.js"></script>

    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

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
										//Form1.submit(); 
							}		
					
					if (varImgValue=='Logout')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									//Form1.submit(); 
						}
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
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                                <td style="width: 271px" align="left">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:Button><asp:ImageButton
                                                        ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px" ImageUrl="white.GIF"
                                                        CommandName="submit" AlternateText="."></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelAddHelp" runat="server" Width="140px" Height="12px" BorderStyle="None"
                                                        BorderWidth="2px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" CssClass="HeaderTestMenu">View New Updation</asp:Label>
                                                </td>
                                                <td align="center">
                                                    <img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;&nbsp;<img
                                                        src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                    &nbsp;<img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                        style="cursor: hand;" />
                                                    <img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../images/top_nav_back.gif" height="47">
                                        &nbsp;
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;&nbsp;
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
                                            <cc1:CollapsiblePanel ID="cpnlHelp" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
                                                Text="View Updation" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black"
                                                PanelCSS="panel" TitleCSS="test" Visible="True" BorderColor="Indigo">
                                                <table id="Table22" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                    border="1">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="Label1" runat="server">Label</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                            <!-- *****************************************-->
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="100px" Font-Names="Verdana" Font-Size="XX-Small"
                            BorderWidth="0" BorderStyle="Groove" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthiddenImage" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
