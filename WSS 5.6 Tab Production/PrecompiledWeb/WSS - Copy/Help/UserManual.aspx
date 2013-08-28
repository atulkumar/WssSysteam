<%@ page language="VB" autoeventwireup="false" inherits="Help_UserManual, App_Web_pszgyzgn" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >--%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:w="urn:schemas-microsoft-com:office:word" xmlns:st1="urn:schemas-microsoft-com:office:smarttags"
xmlns="http://www.w3.org/TR/REC-html40">
<head>
    <title>WSSHelp</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
    <meta name="ProgId" content="Word.Document" />
    <meta name="Generator" content="Microsoft Word 10" />
    <meta name="Originator" content="Microsoft Word 10" />
    <link rel="File-List" href="HELP%20file%20for%20WSS_files/filelist.xml" />
    <link rel="Edit-Time-Data" href="HELP%20file%20for%20WSS_files/editdata.mso" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
        		
			 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	

						
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5"
    lang="EN-US" link="blue" vlink="purple">
    <form id="Form1" runat="server">
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td background="../images/top_nav_back.gif" height="47">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 25%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                        AlternateText="." CommandName="submit" ImageUrl="white.GIF"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelUM" runat="server" CssClass="TitleLabel" BorderStyle="None">WSS User Manual</asp:Label>
                                                </td>
                                                <td style="width:70%; text-align: center;" nowrap="nowrap">
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
                                            border="0" name="tbrbtnEdit" class="PlusImageCSS">&nbsp;&nbsp;&nbsp;
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
                                            <table style="border-collapse: collapse" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <table style="border-collapse: collapse" width="100%" border="0">
                                                            <tr>
                                                                <td bgcolor="#8AAFE5" colspan="3">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="2%" bgcolor="#8AAFE5">
                                                                    &nbsp;
                                                                </td>
                                                                <td width="90%" bgcolor="#ffffdd">
                                                                    <div style="overflow: auto; width: 1000px; height: 530px">
                                                                        <span id="spHellpManual" runat="server"></span>
                                                                    </div>
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
    <input type="hidden" name="txthiddenImage">
    </form>
</body>
</html>
