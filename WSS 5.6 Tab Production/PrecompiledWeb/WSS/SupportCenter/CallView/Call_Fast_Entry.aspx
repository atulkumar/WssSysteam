<%@ page language="VB" autoeventwireup="false" validaterequest="false" inherits="SupportCenter_CallView_Call_Fast_Entry, App_Web_ixviuxgi" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Call Fast Entry</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/CallViewShortCuts.js" type="text/javascript"></script>

    <script type="text/javascript">

        var rand_no = Math.ceil(500 * Math.random())

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
        ///To open resizable comment window.
        function wopenComment(url, name, w, h) {
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
				'status=no, toolbar=no, scrollbars=yes, resizable=yes');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }

    function RefreshAttachment()
		{
			document.Form1.submit();
		}
		
        function CheckLength() {
            var TDLength = document.getElementById('cpnlCallView_txtDesc').value.length;
            if (TDLength > 0) {
                if (TDLength > 2000) {
                    alert('The Call Description cannot be more than 2000 characters\n (Current Length :' + TDLength + ')');
                    return false;
                }
            }
            return true;
        }

        function KeyImage(a, b, c, d) {
            wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=329&CallNo=' + a + ' &CompId=' + b + '&tbname=' + c, 'Comment' + rand_no, 500, 450);
            return false;
        }

        function OpenAttach(CompanyID) {
            wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=C&CompanyID=' + CompanyID, 'Additional_Address' + rand_no, 460, 450);
            return false;
        }

        function SaveEdit(varImgValue) {
            if (varImgValue == 'Edit') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }
            if (varImgValue == 'Close') {
                window.close();
            }
            if (varImgValue == 'Save') {
                if (CheckLength() == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;
            }
            if (varImgValue == 'OK') {
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
            }
        }        			
    </script>

    <script type="text/javascript">
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";
        }
    </script>

   </head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td width="382">
                                                    <div>
                                                        <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" 
                                                            BorderWidth="0px" BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None">
                                                        </asp:Button><asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="0px"
                                                            BorderWidth="0px" Width="0px" AlternateText="." CommandName="submit" ImageUrl="white.GIF">
                                                        </asp:ImageButton>
                                                        <asp:Label ID="lblTitleLabelTaskView" runat="server"
                                                            CssClass="TitleLabel">CALL FAST ENTRY</asp:Label></div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="imgSave" AccessKey="A" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                        </asp:ImageButton>&nbsp;
                                                         <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" Visible="true" ImageUrl="../../Images/s1ok02.gif"
                                                            ToolTip="Ok"></asp:ImageButton>&nbsp;
                                                        <img src="../../Images/reset_20.gif" title="Refresh" alt=""
                                                            style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                             <asp:ImageButton ID="imgClose" runat="server" OnClientClick="tabClose();"
                                                        ImageUrl="../../Images/s2close01.gif" AlternateText="Close Window">
                                                    </asp:ImageButton>&nbsp;
                                                       <%-- <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />--%>
                                                       
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../../Images/top_nav_back.gif" height="47">
                                        <div style="width: 150px">
                                        <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2204','../../');"
                                            alt="Video Help" src="../../Images/video_help.jpg" border="0">&nbsp;
                                            <img class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('833','../../');"
                                                alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;<img
                                                    class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                    src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;&nbsp;</div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
                                Text="Call Fast Entry" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black"
                                PanelCSS="panel" TitleCSS="test" Visible="true" BorderColor="Indigo">
                                <div style="overflow: auto; width: 100%;">
                                    <table bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="100%" border="1">
                                        <tr>
                                            <td bordercolor="#f5f5f5">
                                                <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                    width="100%" align="left" border="0">
                                                    <tr>
                                                        <td colspan="3">
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                        </td>
                                                        <td width="100">
                                                            <asp:Label ID="Label7" runat="server" CssClass="FieldLabel">Company</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCompany" runat="server" Width="200px" CssClass="txtNoFocus" ReadOnly="True"
                                                                MaxLength="100"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28" height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td width="100" height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td width="100">
                                                            <asp:Label ID="Label6" runat="server" CssClass="FieldLabel">Comment</asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgComment" ImageUrl="../../Images/comment_Blank.gif" AlternateText="Comment"
                                                                runat="server"></asp:ImageButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28" height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td width="100" height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                            &nbsp;
                                                        </td>
                                                        <td width="100">
                                                            <asp:Label ID="Label5" runat="server" CssClass="FieldLabel">Attachment</asp:Label>
                                                        </td>
                                                        <td>
                                                            <img class="PlusImageCSS" id="imgAttachment" alt="Add Attachment" src="../../Images/Attach15_9.gif"
                                                                border="0" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28" height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td width="100" height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td height="10">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                        </td>
                                                        <td width="100">
                                                            <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">Subject *</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSubject" runat="server" Width="566px" CssClass="txtNoFocus" MaxLength="100"></asp:TextBox>
                                                        </td>
                                                        <td rowspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td width="100">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td>
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left" width="28">
                                                            <!--  **********************************************************************-->
                                                        </td>
                                                        <td valign="top" align="left" width="100">
                                                            <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">Description *</asp:Label><!-- Panel for displaying Task Info -->
                                                            <!-- Panel for displaying Action Info-->
                                                            <!-- ***********************************************************************-->
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtDesc" runat="server" Width="566px" Height="165px" CssClass="txtNoFocus"
                                                                MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left" width="28">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td valign="top" align="left" width="100">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                        </td>
                                                        <td width="100">
                                                            <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">Call Type *</asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="CDDLCallType" AllowCustomText="true" runat="server" Width="140px"
                                                                DropDownWidth="140px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Select Call Type"
                                                                EnableTextSelection="true" EnableVirtualScrolling="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td width="100">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td>
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                        </td>
                                                        <td width="100">
                                                            <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">Priority *</asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="CDDLPriority" AllowCustomText="true" runat="server" Width="140px"
                                                                DropDownWidth="140px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Description"
                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Priority"
                                                                EnableTextSelection="true" EnableVirtualScrolling="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td width="100">
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                        <td>
                                                            <font size="1">&nbsp;</font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                        </td>
                                                        <td width="100">
                                                            <asp:Label ID="Label8" runat="server" CssClass="FieldLabel">Sub Category *</asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="CDDLProject" AllowCustomText="true" runat="server" Width="140px"
                                                                DropDownWidth="140px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Name"
                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Sub Category"
                                                                EnableTextSelection="true" EnableVirtualScrolling="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28" height="9">
                                                        </td>
                                                        <td width="100" height="9">
                                                        </td>
                                                        <td height="9">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="28">
                                                        </td>
                                                        <td width="100">
                                                            <asp:Label ID="lblMiddleName2" runat="server" Width="96px" Height="12px" CssClass="FieldLabel">Requested By*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="CDDLCallOwner" Font-Names="Verdana" Font-Size="7pt" AllowCustomText="true"
                                                                runat="server" DropDownWidth="200px" Width="140px" Height="150px" DataTextField="Description"
                                                                DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" NoWrap="true" EmptyMessage="Select User"
                                                                EnableTextSelection="true" EnableVirtualScrolling="true">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <br />
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
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap">
                <input type="hidden" name="txthiddenCallNo" />
                <input type="hidden" name="txtComp" />
                <input type="hidden" name="txthiddenImage" /><input type="hidden" name="txtCallNo" />
                <asp:ListBox ID="lstError" runat="server" Width="722px" BorderStyle="Groove" BorderWidth="0"
                    Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlMsg" runat="server" CssClass="pnlmsg">
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
