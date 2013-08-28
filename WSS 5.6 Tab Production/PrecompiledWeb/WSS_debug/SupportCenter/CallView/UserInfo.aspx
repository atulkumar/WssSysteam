<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_UserInfo, App_Web_i-czgkd-" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>UserInfo</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link rel="stylesheet" type="text/css" href="../../Images/Js/StyleSheet1.css" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="Form1" runat="server">

    <script type="text/javascript">
		var rand_no = Math.ceil(500*Math.random())			
		function OpenInv(ID)
		{
			wopen('../../AdministrationCenter/Inventory/EmpInventoryInfo.aspx?EmpID=' + ID ,'Inv'+rand_no,600,400);
			return false;
		}
		function wopen(url, name, w, h)
		{
			w += 32;
			h += 96;
			wleft = (screen.width - w) / 2;
			wtop = (screen.height - h) / 2;
			var win = window.open(url,
				name,
				'width=' + w + ', height=' + h + ', ' +
				'left=' + wleft + ', top=' + wtop + ', ' +
				'location=no, menubar=no, ' +
				'status=no, toolbar=no, scrollbars=no, resizable=no');
			// Just in case width and height are ignored
			win.resizeTo(w, h);
			// Just in case left and top are ignored
			win.moveTo(wleft, wtop);
			win.focus();
			return false;
		}
    </script>

    <script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.HideModalDiv)
                opener.parent.HideModalDiv();
            }
        }
        //Modified By Atul to execute script on Page Load
        function OnLoad() {
           if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.LoadModalDiv)
                opener.parent.LoadModalDiv();
            }
        }
        window.onload = OnLoad;
        window.onunload = OnClose;
    </script>

    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
        border="0">
        <tr>
            <td>
                <asp:Label ID="lblTitleLabelUserInfo" runat="server" Width="90px" CssClass="TitleLabel">&nbsp;User Info</asp:Label>
            </td>
            <td style="width: 902px" align="center">
                <a href="mailto:<%=mstrMailID%>?subject=<%=mstrMailSub%>">
                    <asp:Image ID="ImgEmail" runat="server" ImageUrl="../../Images/E-mail.gif" AlternateText="Send E-mail">
                    </asp:Image></a>&nbsp;
                <asp:Image ID="ImgSMS" runat="server" ImageUrl="../../Images/SMS.gif" AlternateText="Send SMS">
                </asp:Image>&nbsp;&nbsp;<asp:ImageButton ID="imgClose" AccessKey="L" runat="server"
                    ImageUrl="../../Images/s2close01.gif" ToolTip="Close"></asp:ImageButton>
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table style="width: 352px; border-collapse: collapse; height: 344px" width="352"
        border="0">
        <tr>
            <td style="width: 9px; height: 15px" width="9">
                &nbsp;
            </td>
            <td width="93" style="height: 15px">
                <asp:Label ID="lbUserid" runat="server" CssClass="FieldLabel">User ID:</asp:Label>
            </td>
            <td style="width: 354px; height: 15px" width="354">
                <b><font color="#006600" size="1">-</font></b>
                <asp:Label ID="lblUserID" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 15px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 9px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93">
                <asp:Label ID="lbFullName" runat="server" CssClass="FieldLabel">Full Name: </asp:Label>
            </td>
            <td style="width: 354px; height: 9px" width="354">
                <b><font color="#006600" size="1">- </font></b>
                <asp:Label ID="lblFullName" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 9px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 15px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93">
                <asp:Label ID="lbCompany" runat="server" CssClass="FieldLabel">Company:</asp:Label>
            </td>
            <td style="width: 354px; height: 15px" width="354">
                <b><font color="#006600" size="1">- </font></b>
                <asp:Label ID="lblCompany" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 15px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 16px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93">
                <asp:Label ID="lbJobrole" runat="server" CssClass="FieldLabel">Job Role:</asp:Label>
            </td>
            <td style="width: 354px; height: 16px" width="354">
                <b><font color="#006600" size="1">-</font> </b>
                <asp:Label ID="lblJobRole" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 16px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 14px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="height: 14px">
                <asp:Label ID="lbPhoneno1" runat="server" CssClass="FieldLabel">Phone no1:</asp:Label>
            </td>
            <td style="width: 354px; height: 14px" width="354">
                <b><font color="#006600" size="1">- </font></b>
                <asp:Label ID="lblMobNo" runat="server" CssClass="DataFieldLabel"></asp:Label>
                <asp:Label ID="lblPhone1Type" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 14px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="width: 93px; height: 9px">
                <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">Phone no2:</asp:Label>
            </td>
            <td style="width: 354px" width="354">
                <b><font color="#006600" size="1">-</font></b>
                <asp:Label ID="lblOffPhone" runat="server" CssClass="DataFieldLabel"></asp:Label>
                <asp:Label ID="lblPhone2Type" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td>
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 8px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="width: 93px; height: 9px">
                <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">Mail Address:</asp:Label>
            </td>
            <td style="width: 354px; height: 8px" width="354">
                <b><font color="#006600" size="1">-</font> </b>
                <asp:Label ID="lblMailAdd" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 8px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 16px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="width: 93px; height: 16px">
                <asp:Label ID="Label3" runat="server" CssClass="FieldLabel" Width="88px">Working hour:</asp:Label>
            </td>
            <td style="width: 354px; height: 16px" width="354">
                <b><font color="#006600" size="1">-</font> </b>
                <asp:Label ID="lblWorkingHr" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 16px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 10px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="width: 93px; height: 9px">
                <asp:Label ID="Label5" runat="server" CssClass="FieldLabel">Time Zone:</asp:Label>
            </td>
            <td style="width: 354px; height: 10px" width="354">
                <b><font color="#006600" size="1">-</font> </b>
                <asp:Label ID="lblTimeZone" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 10px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 10px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="width: 93px; height: 9px">
                <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">City:</asp:Label>
            </td>
            <td style="width: 354px; height: 10px" width="354">
                <b><font color="#006600" size="1">-</font> </b>
                <asp:Label ID="lblCity" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 10px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 10px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="width: 93px; height: 9px">
                <asp:Label ID="Label9" runat="server" CssClass="FieldLabel">Country:</asp:Label>
            </td>
            <td style="width: 354px; height: 10px" width="354">
                <b><font color="#006600" size="1">-</font> </b>
                <asp:Label ID="lblCountry" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 10px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 8px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="width: 93px; height: 9px">
                <asp:Label ID="Label6" runat="server" CssClass="FieldLabel">Manager:</asp:Label>
            </td>
            <td style="width: 354px; height: 8px" width="354">
                <b><font color="#006600" size="1">-</font> </b>
                <asp:Label ID="lblManager" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 8px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 14px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="93" style="width: 93px; height: 14px">
                <asp:Label ID="Label8" runat="server" CssClass="FieldLabel">Department:</asp:Label>
            </td>
            <td style="width: 354px; height: 14px" width="354">
                <b><font color="#006600" size="1">-</font> </b>
                <asp:Label ID="lblDepartment" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 14px">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 23px" width="9">
                <font color="#006600" size="1">&nbsp;</font>
            </td>
            <td width="585" style="width: 93px; height: 23px">
                <asp:Label ID="Label7" runat="server" CssClass="FieldLabel">Sex:</asp:Label>
            </td>
            <td style="width: 354px; height: 23px" width="354">
                <b><font color="#006600" size="1">-</font></b>
                <asp:Label ID="lblSex" runat="server" CssClass="DataFieldLabel"></asp:Label>
            </td>
            <td style="height: 23px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 9px" width="9">
                &nbsp;
            </td>
            <td style="width: 126px; height: 167px" valign="middle" align="center" width="126"
                rowspan="7" colspan="4">
                &nbsp;
                <asp:Image ID="Image1" runat="server" Width="160px"></asp:Image>
            </td>
        </tr>
        <tr>
            <td style="width: 9px; height: 23px" width="9">
            </td>
            <td style="width: 93px; height: 23px" width="585" colspan="2">
                <asp:LinkButton ID="lbInv" runat="server" Width="175px" Font-Bold="True" Font-Name="verdana"
                    Font-Size="10px" Visible="False">View Employee Inventory Info</asp:LinkButton>
            </td>
            <td style="height: 23px">
            </td>
        </tr>
        <tr>
            <td style="width: 9px" width="9" colspan="4">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 9px" width="9" colspan="4">
                &nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
