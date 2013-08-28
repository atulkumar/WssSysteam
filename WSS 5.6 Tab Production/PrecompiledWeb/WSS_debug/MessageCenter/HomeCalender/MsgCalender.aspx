<%@ page language="VB" autoeventwireup="false" inherits="MessageCenter_HomeCalender_MsgCalender, App_Web_ermsaczv" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Add/Edit Message</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

    <script type="text/javascript">

        function CheckLength() {
            try {

                var CDLength = document.getElementById('cpnlEvent_txtMessage').value.length;

                if (CDLength > 0) {
                    if (CDLength > 500) {
                        alert('The Event Message cannot be more than 500 characters\n (Current Length :' + CDLength + ')');
                        return false;
                    }
                }

            }
            catch (e) {
                return true;
            }
            return true;
        }

        function wopen(url, name, w, h) {
            // Fudge factors for window decoration space.
            // In my tests these work well on all platforms & browsers.
            w += 32;
            h += 96;
            wleft = (screen.width - w) / 2;
            wtop = (screen.height - h) / 2;
            var win = window.open(url,
					name,
					'width=' + w + ', height=' + h + ', ' +
					'left=' + wleft + ', top=' + wtop + ', ' +
					'location=no, menubar=no, ' +
					'status=no, toolbar=no, scrollbars=yes, resizable=no');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }


        function addToParentList(Afilename, TbName, strName) {

            if (Afilename != "" || Afilename != 'undefined') {
                varName = TbName + 'Name'
                //alert(Afilename);
                document.getElementById(TbName).value = Afilename;
                document.getElementById(varName).value = strName;
                aa = Afilename;
            }
            else {
                document.Form1.txtAB_Type.value = aa;
            }
        }

        function SaveEdit(varImgValue) {


            if (varImgValue == 'Close') {
                window.close();
                return false;
            }



            if (varImgValue == 'Ok') {

                if (CheckLength() == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;
                // CloseWindow();
            }

            if (varImgValue == 'Save') {

                if (CheckLength() == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;
            }



            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset();

                }
                return false;

            }
        }			
		
		
		//A Function to call on Page Load to set grid width according to screen size
        function onLoad() {
            var divAdvSearch = document.getElementById('divAdvSearch');
            if (divAdvSearch != null) {
                if (document.body.clientWidth > 0) {
                    divAdvSearch.style.width = document.body.clientWidth - 30 + "px";
                }
            }
        }
		
		 //A Function to improve design i.e delete the extra cell of table
        function onEnd() 
        {
                var x = document.getElementById('cpnlDate_collapsible').cells[0].colSpan = "1";
		        var y = document.getElementById('cpnlEvent_collapsible').cells[0].colSpan = "1";
		}
        
       //A Function is Called when we resize window
        window.onresize = onLoad;  			
		
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; margin-bottom: 0px">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(onLoad);     
    </script>

    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
        border="0">
        <tr>
            <td>
                <asp:Label ID="lblTitleLabelAddEvent" runat="server" BorderStyle="None" ForeColor="Black" BorderWidth="2px"
                    Width="144px" CssClass="TitleLabel">&nbsp;Add/Edit Message</asp:Label>
            </td>
            <td style="width: 902px">
                <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                    ToolTip="Save"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif"
                    ToolTip="OK"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                    ToolTip="Reset"></asp:ImageButton>&nbsp;<asp:ImageButton ID="imgClose" AccessKey="L"
                        runat="server" ImageUrl="../../Images/s2close01.gif" ToolTip="Close"></asp:ImageButton>&nbsp;<img
                            title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <div id="divAdvSearch" style="overflow: auto; width: 100%; height: 100%">
        <table id="Table11" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <cc1:CollapsiblePanel ID="cpnlDate" runat="server" Width="100%" BorderWidth="0px"
                        BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                        ExpandImage="../../Images/ToggleDown.gif" Text="Date" TitleBackColor="Transparent"
                        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                        Visible="True" BorderColor="Indigo">
                        <table bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td bordercolor="#f5f5f5" align="center">
                                    <asp:Label ID="TitleLabelDate" runat="server" Font-Names="Verdana" Font-Size="Medium"
                                        Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </cc1:CollapsiblePanel>
                </td>
                <tr>
                    <td>
                        <cc1:CollapsiblePanel ID="cpnlEvent" runat="server" Width="100%" BorderStyle="Solid"
                            BorderWidth="0px" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                            TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="User Message"
                            ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                            Draggable="False">
                            <table id="Table3" bordercolor="lightgrey" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td colspan="3">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">Message Type</asp:Label>
                                    </td>
                                    <td>
                                        <uc1:CustomDDL ID="CDDLEventType" runat="server" Width="120px"></uc1:CustomDDL>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 17px" valign="top" align="left">
                                    </td>
                                    <td style="height: 17px">
                                        <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">Message To</asp:Label>
                                    </td>
                                    <td style="height: 17px">
                                        <uc1:CustomDDL ID="CDDLUser" runat="server" Width="120px"></uc1:CustomDDL>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">Subject</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="txtNoFocus" Width="330" Height="18px"
                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        &nbsp; &nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">Message</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMessage" runat="server" CssClass="txtNoFocus" Width="330px" Height="128px"
                                            Font-Names="Verdana" Font-Size="XX-Small" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        &nbsp; &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        &nbsp;
                                    </td>
                                    <td valign="top" align="left">
                                    </td>
                                    <td align="right">
                                        <asp:CheckBox ID="cbEnable" runat="server" CssClass="FieldLabel" Text="Remember This Message"
                                            TextAlign="Left" Checked="True"></asp:CheckBox>
                                    </td>
                                </tr>
                            </table>
                        </cc1:CollapsiblePanel>
                    </td>
                </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <input type="hidden" name="txthiddenImage">
            <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
