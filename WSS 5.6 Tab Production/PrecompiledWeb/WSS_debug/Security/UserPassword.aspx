<%@ page language="VB" autoeventwireup="false" inherits="Security_UserPassword, App_Web_kkyzf0kd" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Password</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../Images/js/core.js" type="text/javascript"></script>

    <script src="../Images/js/events.js" type="text/javascript"></script>

    <script src="../Images/js/css.js" type="text/javascript"></script>

    <script src="../Images/js/coordinates.js" type="text/javascript"></script>

    <script src="../Images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript">

				
			function SaveEdit(varImgValue)
				{
			    
								if (varImgValue=='Save')
								{
											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit(); 
								}	
								if (varImgValue=='Ok')
								{
												document.Form1.txthiddenImage.value=varImgValue;
												Form1.submit(); 
								}	
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								
								
								if (varImgValue=='Reset')
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
				
	
		  //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlUserPassword_collapsible').cells[0].colSpan = "1";
        }
        
         //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	
	
        	
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5"
    onload="document.getElementById('cpnlUserPassword_txtPassword').focus();">
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
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" BorderWidth="0px" Width="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:Label ID="lblTitleLabelUserSearch" runat="server" CssClass="TitleLabel" BorderStyle="None">CHANGE PASSWORD</asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgOK" AccessKey="O" runat="server" ImageUrl="../Images/s1ok02.gif"
                                                            ToolTip="OK"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('732','../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!--  **********************************************************************-->
                    <tr style="width: 100%">
                        <td>
                            <cc1:CollapsiblePanel ID="cpnlUserPassword" runat="server" Width="100%" BorderWidth="0px"
                                BorderStyle="Solid" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                                ExpandImage="../images/ToggleDown.gif" Text="Change Password" TitleBackColor="Transparent"
                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                Visible="true" BorderColor="Indigo">
                                <div style="overflow: auto; width: 100%">
                                    <table width="100%" border="0">
                                        <tr>
                                            <td valign="top" align="center">
                                                <table style="border-color: #5c5a5b; background-color: #f5f5f5" width="100%" border="1">
                                                    <tr>
                                                        <td bordercolor="#f5f5f5" width="50%">
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:Label ID="lblUserID" Text="User ID" runat="server"></asp:Label>
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <uc1:CustomDDL ID="CDDLUserID" runat="server" Width="120px"></uc1:CustomDDL>
                                                        </td>
                                                        <td bordercolor="#f5f5f5" width="50%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td bordercolor="#f5f5f5" width="50%">
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:Label ID="Label1" Text="Old Password" runat="server"></asp:Label>
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:TextBox ID="txtPassword" runat="server" Width="120px" CssClass="txtNoFocus"
                                                                TextMode="Password" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td bordercolor="#f5f5f5" width="50%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td bordercolor="#f5f5f5" width="50%">
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:Label ID="Label3" Width="160px" Text="New Password" runat="server"></asp:Label>
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:TextBox ID="txtNewPassword" runat="server" Width="120px" CssClass="txtNoFocus"
                                                                TextMode="Password" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td bordercolor="#f5f5f5" width="50%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td bordercolor="#f5f5f5" width="50%">
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:Label ID="Label2" Width="194px" Text="Confirm New Password" runat="server"></asp:Label>
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:TextBox ID="txtConNewPassword" runat="server" Width="120px" CssClass="txtNoFocus"
                                                                TextMode="Password" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td bordercolor="#f5f5f5" width="50%">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </cc1:CollapsiblePanel>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Visible="false" BorderStyle="Groove" BorderWidth="0"
                            ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox>
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
