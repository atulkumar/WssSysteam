<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frm_NoAccess.aspx.vb" Inherits="frm_NoAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>NoAccess</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="javascript" src="Images/Js/JSValidation.js"></script>

    <link href="images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script language="Javascript">

			
				function SaveEdit(varImgValue)
				{
								
						if (varImgValue=='Logout')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
						}
				}
						
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 271px" align="left">
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                        AlternateText="." CommandName="submit" ImageUrl="white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelNoAccess" runat="server" Font-Size="X-Small" Font-Bold="True"
                                                        Height="12px" Width="128px" CssClass="HeaderTestMenu" Font-Names="Verdana" BorderWidth="2px"
                                                        BorderStyle="None">No Access</asp:Label>
                                                </td>
                                                <td align="left">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="images/top_nav_back.gif" height="47">
                                        &nbsp;
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
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
                                                            <table height="400" width="100%" align="center">
                                                                <tr>
                                                                    <td valign="bottom" align="center">
                                                                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Size="Medium" Font-Bold="True">You 
                                                                        don’t have access privilege on this screen</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 28px" valign="middle" align="center">
                                                                        <asp:Label ID="lblRole" runat="server" ForeColor="#0000C0" Font-Size="Small" Font-Bold="True">You 
                                                                        have logged in</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="center">
                                                                        <asp:Label ID="Label2" runat="server" Font-Size="Medium" Font-Italic="True">(Contact 
                                                                        your Administrator)</asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                </div>
                            </td>
                        </tr>
                    <tr>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" name="txthiddenImage">
    </form>
</body>
</html>
