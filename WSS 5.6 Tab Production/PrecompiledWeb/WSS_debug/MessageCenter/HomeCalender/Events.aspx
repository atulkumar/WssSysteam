<%@ page language="VB" autoeventwireup="false" inherits="MessageCenter_HomeCalender_Events, App_Web_ermsaczv" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Messages</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

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
					'status=no, toolbar=no, scrollbars=no, resizable=no');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }

        function ShowAlert() {
            alert('You cannot edit this event  ');
            return false;
        }

        function ShowEdit(ID, strDate) {

            wopen("MsgCalender.aspx?strDate=" + strDate + "&Task=Edit&ID=" + ID, "AddEvent" + rand_no, 430, 400);
            return false;
        }

        function SaveEdit(varImgValue) {


            if (varImgValue == 'Close') {
                window.close();
                return false;
            }

            if (varImgValue == 'Add') {
                wopen("MsgCalender.aspx?strDate=" + document.getElementById('txthiddenDate').value + "&Task=Add", "AddEvent" + rand_no, 430, 400);
                return false;
            }

            if (varImgValue == 'Save') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }


        }


        function KeyCheck(cc) {
            //alert(cc);

            var tableID = 'cpnlEvent_dlstEvent'  //your datagrids id
            var table;

            if (document.all) table = document.all[tableID];
            if (document.getElementById) table = document.getElementById(tableID);
            if (table) {
                for (var i = 1; i < table.rows.length; i++) {
                    if (i % 2 == 0) {
                        table.rows[i].style.backgroundColor = "#f5f5f5";
                    }
                    else {
                        table.rows[i].style.backgroundColor = "#ffffff";
                    }
                }
                table.rows[cc].style.backgroundColor = "#d4d4d4";
            }

        }

        function ParentRefresh() {
            self.opener.Form1.submit();
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

    <!--Added By Atul to make parent window Active after popup win close-->

    <script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if (window.opener.parent.HideModalDiv) {
                    opener.parent.HideModalDiv();
                }
            }
        }
        //Modified By Atul to execute script on Page Load
        function OnLoad() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if (window.opener.parent.LoadModalDiv) {
                    opener.parent.LoadModalDiv();
                }
            }
       
        window.onload = OnLoad;
        window.onunload = OnClose;
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
                <asp:Label ID="lblTitleLabelEvents" runat="server" CssClass="TitleLabel" BorderStyle="None"
                    BorderWidth="2px" Width="128px">&nbsp;Messages</asp:Label>
            </td>
            <td style="width: 902px">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img title="Seperator" alt="R" src="../../Images/00Seperator.gif"
                    border="0">
                <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ToolTip="Add" ImageUrl="../../Images/s2Add01.gif">
                </asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="..\..\Images\S2Save01.gif"
                    ToolTip="Save"></asp:ImageButton>&nbsp;<asp:ImageButton ID="imgClose" AccessKey="L"
                        runat="server" ToolTip="Close" ImageUrl="../../Images/s2close01.gif"></asp:ImageButton>&nbsp;<img
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
                    <cc1:CollapsiblePanel ID="cpnlDate" runat="server" BorderStyle="Solid" BorderWidth="0px"
                        Width="100%" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                        TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="Date"
                        ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                        Draggable="False">
                        <table bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td bordercolor="#f5f5f5" align="center">
                                    <asp:Label ID="TitleLabelDate" runat="server" Font-Size="Medium" ForeColor="Black" Font-Names="Verdana"
                                        Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </cc1:CollapsiblePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <cc1:CollapsiblePanel ID="cpnlEvent" runat="server" BorderStyle="Solid" BorderWidth="0px"
                        Width="100%" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel"
                        TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent" Text="User Message"
                        ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                        Draggable="False">
                        <div style="overflow: auto; width: 100%; height: 310px">
                            <table id="Table3" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" border="1">
                                <tbody>
                                    <tr>
                                        <td valign="top" bordercolor="#f5f5f5" align="left">
                                            <asp:DataList ID="dlstEvent" runat="server">
                                                <ItemTemplate>
                                                    <table bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" border="0">
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="2" bordercolor="#f5f5f5" valign="top">
                                                                    <asp:CheckBox ID="chkR" runat="server" Text="Remember This Message" ForeColor="Teal"
                                                                        Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small" Checked='<%# container.dataitem("CL_VC4_Status")  %>'>
                                                                    </asp:CheckBox>
                                                                    <input type="hidden" id="txtID" runat="server" value='<%# container.dataitem("CL_NU9_ID_PK")  %>' />
                                                                    <%--<asp:TextBox Width="0px" ID="txtID" Runat="server" Text='<%# container.dataitem("CL_NU9_ID_PK")  %>' >
																		</asp:TextBox>--%>
                                                                </td>
                                                                <td valign="middle" rowspan="5" bordercolor="#f5f5f5">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td bordercolor="#f5f5f5" valign="top">
                                                                    <asp:Label ID="Label1" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small">Message&nbsp;Type &nbsp;&nbsp;&nbsp;:</asp:Label>
                                                                </td>
                                                                <td bordercolor="#f5f5f5" valign="top">
                                                                    <asp:Label ID="lblEventType" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" Width="80px">
																			<%# container.dataitem("CL_VC8_Event")  %>
                                                                    </asp:Label>
                                                                </td>
                                                                <td valign="middle" rowspan="5" bordercolor="#f5f5f5">
                                                                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                                        AlternateText="Edit" ToolTip="Edit"></asp:ImageButton>&nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label6" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small">Message&nbsp;From&nbsp;&nbsp;&nbsp;:</asp:Label>
                                                                </td>
                                                                <td valign="top" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" Width="180px">
																			<%# container.dataitem("CL_NU9_UserID_FK")  %>
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label4" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small">Message&nbsp;To&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</asp:Label>
                                                                </td>
                                                                <td valign="top" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" Width="180px">
																			<%# container.dataitem("CL_NU9_Event_Owner_FK")  %>
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label2" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small">Subject&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;&nbsp;</asp:Label>
                                                                </td>
                                                                <td valign="top" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="lblSubject" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" Width="180px">
																			<%# container.dataitem("CL_VC50_Subject")  %>
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="Label3" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small">Message&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;</asp:Label>
                                                                </td>
                                                                <td valign="top" bordercolor="#f5f5f5">
                                                                    <asp:Label ID="lblMessgae" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="XX-Small" Width="250px">
																			<%# container.dataitem("CL_VC500_Message")  %>
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </ItemTemplate>
                                                <SeparatorTemplate>
                                                    <hr>
                                                </SeparatorTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                            </table>
                        </div>
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
            <input id="txthiddenDate" type="hidden" name="txthiddenDate" runat="server">
            <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
