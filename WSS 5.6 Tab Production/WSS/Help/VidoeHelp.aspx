<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VidoeHelp.aspx.vb" Inherits="Help_VidoeHelp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:w="urn:schemas-microsoft-com:office:word" xmlns:st1="urn:schemas-microsoft-com:office:smarttags"
xmlns="http://www.w3.org/TR/REC-html40">
<head>
    <title>WSS VideoHelp</title>
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
<style type="text/css">
        a.active
        {
            .color:Blue;            
        }

</style>
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
                                                    <asp:Label ID="lblTitleLabelUM" runat="server" CssClass="TitleLabel" BorderStyle="None">WSS Video Help</asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
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
                        <td align="center" style="padding-top: 20px">
                            <span style="text-align: center; font-size: 15pt; font-family: Verdana; font: Bold;
                                color: Maroon">WSS Video Help</span>
                                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">1. </span><a id="A1" href="VideoHelp/Call_Entry.html" target="_blank"  runat="server"
                                    title="Call Entry Help Video"><span style="text-align: center; font-size: 10pt; font-family: Verdana;
                                        font: Bold; color: Black">Call Entry</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">2. </span><a id="A9" target="_blank"  href="VideoHelp/Call_Task_View.html" runat="server"
                                    title="Call Task View Help Video"><span style="text-align: center; font-size: 10pt;
                                        font-family: Verdana; font: Bold; color: Black">Call Task View</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">3. </span><a id="A3" href="VideoHelp/Call_Hierarchy.html" target="_blank"  runat="server"
                                    title="Call Heirarchy Help Video"><span style="text-align: center; font-size: 10pt;
                                        font-family: Verdana; font: Bold; color: Black">Call Heirarchy</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">4. </span><a id="A2" href="VideoHelp/Call_Fast_Entry.html" target="_blank"  runat="server"
                                    title="Call Fast Entry Help Video"><span style="text-align: center; font-size: 10pt;
                                        font-family: Verdana; font: Bold; color: Black">Call Fast Entry</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">5. </span><a id="A4" href="VideoHelp/Call_View.html" target="_blank"  runat="server"
                                    title="Comment View Help Video"><span style="text-align: center; font-size: 10pt;
                                        font-family: Verdana; font: Bold; color: Black">Comment View</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">6. </span><a id="A5" href="VideoHelp/Comment_View.html" target="_blank"  runat="server"
                                    title="Comment View Help Video"><span style="text-align: center; font-size: 10pt;
                                        font-family: Verdana; font: Bold; color: Black">Comment View</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">7. </span><a id="A7" href="VideoHelp/Task_View.html" target="_blank"  runat="server"
                                    title="Task View Help Video"><span style="text-align: center; font-size: 10pt; font-family: Verdana;
                                        font: Bold; color: Black">Task View</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">8. </span><a id="A6" href="VideoHelp/Multi_Task_Forward.html" target="_blank"  runat="server"
                                    title="Multi-Task Forward View Help Video"><span style="text-align: center; font-size: 10pt;
                                        font-family: Verdana; font: Bold; color: Black">Multi-Task Forward View</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="text-align: center; font-size: 10pt; font-family: Verdana; font: Bold;
                                color: Black">9. </span><a id="A8" href="VideoHelp/To_Do_List.html" target="_blank" runat="server"
                                    title="To-Do-List Help Video"><span style="text-align: center; font-size: 10pt; font-family: Verdana;
                                        font: Bold; color: Black">To-Do-List</span></a>
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
