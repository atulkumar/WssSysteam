<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WSSHelp.aspx.vb" Inherits="Help_WSSHelp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>WSSHelp</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../Images/Js/PopSearchShortCuts.js"></script>

</head>
<body ms_positioning="GridLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
        <table border="0" style="border-collapse: collapse" width="100%">
            <tr>
                <td>
                    <table border="0" style="border-collapse: collapse" width="100%">
                        <tr>
                            <td colspan="3" bgcolor="#D7E3F3" align="center" style="height: 20px">
                                &nbsp;
                                <asp:Label ID="lblHelpTitle" runat="server" Font-Bold="True" Font-Names="Verdana"
                                    Width="500px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td width="3%" bgcolor="#D7E3F3">
                                &nbsp;</td>
                            <td width="100%" bgcolor="#ffffdd">
                                <div style="overflow: auto; width: 100%; height: 100%">
                                    <iframe id="helpFrame" runat="server" style="height: 600px; width: 100%"></iframe>
                                </div>
                            </td>
                            <td width="0%" bgcolor="#D7E3F3">
                            </td>
                        </tr>
                        <tr>
                            <td width="99%" height="5%" colspan="3" bgcolor="#D7E3F3">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
