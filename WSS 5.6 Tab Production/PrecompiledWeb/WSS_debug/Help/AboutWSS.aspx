<%@ page language="VB" autoeventwireup="false" inherits="Help_AboutWSS, App_Web_bnj2ix_q" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WSSHelp</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript">

			
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
    <form id="Form1" runat="server">
    <table width="100%" style="height: 100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td background="../images/top_nav_back.gif" height="47">
                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px" BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None">
                                                    </asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px"
                                                        ImageUrl="~/images/white.GIF" CommandName="submit" AlternateText="."></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelAddHelp" runat="server" BorderStyle="None" CssClass="TitleLabel">About WSS</asp:Label>
                                                </td>
                                                <td style="width: 80%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 5%" background="../images/top_nav_back.gif" height="47">
                                        <img id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
                                            border="0" name="tbrbtnEdit" class="PlusImageCSS" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <!-- *****************************************-->
                                <table border="0" style="border-collapse: collapse" width="100%">
                                    <tr>
                                        <td>
                                            <table border="0" style="border-collapse: collapse" width="100%">
                                                <tr>
                                                    <td colspan="3" bgcolor="#8AAFE5">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="2%" bgcolor="#8AAFE5">
                                                        &nbsp;
                                                    </td>
                                                    <td width="100%" bgcolor="#ffffdd">
                                                        <%--  <div style="overflow: auto; width: 100%; height: 470px">--%>
                                                        <span id="spHellpAbout" runat="server"></span>
                                                        <%--  </div>--%>
                                                    </td>
                                                    <td width="0%" bgcolor="#8AAFE5">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="99%" height="5%" colspan="3" bgcolor="#8AAFE5">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <!-- *****************************************-->
                            </div>
                        </td>
                    </tr>
                </table>
                <input type="hidden" name="txthiddenImage" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
