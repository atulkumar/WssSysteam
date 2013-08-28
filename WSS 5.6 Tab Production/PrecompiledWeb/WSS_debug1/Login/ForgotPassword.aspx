<%@ page language="VB" validaterequest="false" autoeventwireup="false" inherits="Login_ForgotPassword, App_Web_63ldieft" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Forgot Password</title>
    <meta content="False" name="vs_snapToGrid" />
    <meta content="False" name="vs_showGrid" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .style3
        {
            width: 75px;
        }
    </style>

    <script type="text/javascript" src="../images/js/ION.js"></script>

    <script type="text/javascript" src="../Images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../Images/Js/LoginShortCuts.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center;">
        <table>
            <tr style="height: 50px">
                <td style="height: 100px">
                </td>
            </tr>
        </table>
        <table style="width: 600px; height: 380px; background-color: #7196C2" align="center">
            <tr>
                <td>
                    <table align="center" style="width: 540px; height: 320px; background-color: White">
                        <tr>
                            <td valign="top" align="center" colspan="3" style="height: 20px">
                                <table align="center" width="540px" style="height: 80px">
                                    <!-- FORM -->
                                    <tr>
                                        <td colspan="2" style="font-family: Verdana; font-size: 11px">
                                            <h5>
                                                Forgotten password</h5>
                                            If you have forgotten your password, fill in your user name and we will email your
                                            password.
                                            <br />
                                            If you do not remember your user Name either, please contact us .
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 40px">
                            <td valign="top">
                                <table align="center" style="height: 40px; width: 372px;">
                                    <tr>
                                        <td class="style3">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="../Images/ForgotPassword.jpg" />
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblUserName" runat="server" Text="User Name:" Font-Names="verdana"
                                                Font-Size="10px"></asp:Label>
                                            <asp:TextBox ID="txtUserName" runat="server" BorderStyle="Solid" Font-Size="XX-Small"
                                                Font-Names="Verdana" Width="150px" name="txtUserName" CssClass="txtNoFocus" Height="14px"
                                                BorderWidth="1px" MaxLength="36"></asp:TextBox>
                                            <br />
                                            <br />
                                            &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnpwd" Text="Send Password" runat="server" Width="95px" Font-Names="Verdana"
                                                Font-Size="10px" />
                                            &nbsp;<asp:Button ID="btnBack" Text="Back" runat="server" Width="50px" Font-Names="Verdana"
                                                Font-Size="10px" />
                                            <br />
                                            <br />
                                            <asp:Label ID="lblError" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                Width="328px" ForeColor="Red" BackColor="transparent" Font-Bold="true" Height="18px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height: 80px">
                                        <td align="left" valign="bottom">
                                            <font face="Verdana" size="1">Press Esc To Exit</font>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
