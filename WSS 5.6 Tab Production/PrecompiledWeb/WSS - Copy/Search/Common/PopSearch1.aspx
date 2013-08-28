<%@ page language="VB" autoeventwireup="false" inherits="Search_Common_PopSearch1, App_Web_c_vc503s" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Search</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../../Images/Js/PopSearchShortCuts.js" type="text/javascript"></script>

    <script type="text/javascript">

        //var globalid;
        var gID;
        var gName;
        var gRN;
        var gNameP;
        gRN = 0;
        var tableID;
        var MAX;
        MAX = -1;
        var arID;
        var arNameP;

        var globalid;
        var globalUDC;
        var globalTxtbox;
        var globalstrName;

        function closeMe() {
            var ret;
            ret = document.getElementById("txtValue").value;
            window.returnValue = ret;
            window.close();
        }

        function checkArrows(field, evt, ID, NameP, txtID) {
            //alert(NameP);
            globalTxtbox = txtID;
            var str;
            str = ID;
            arID = str.split('###');
            //MAX=arID.length-1;
            //alert(document.getElementById('txtHIDCount').value);
            MAX = parseInt(document.getElementById('txtHIDCount').value);

            arNameP = NameP.split('###');

            var keyCode =
		document.layers ? evt.which :
		document.all ? event.keyCode :
		document.getElementById ? evt.keyCode : 0;
            if (gRN > MAX) {
                gRN = gRN - 1;
            }
            if (MAX > 0) {
                //alert(keyCode);
                if (keyCode == 40) {
                    if (gRN == MAX) gRN = 0;
                    //alert(gRN);
                    DownKeyCheck(gRN + 1);
                }
                else if (keyCode == 38) {
                    gRN = parseInt(gRN);
                    if (gRN == 0) gRN = parseInt(MAX); +1;
                    UpKeyCheck(gRN - 1);
                }
                else if (keyCode == 37) {
                    gRN = parseInt(gRN);
                    if (gRN == 0) gRN = parseInt(MAX); +1;
                    UpKeyCheck(gRN - 1);
                }
                else if (keyCode == 39) {
                    if (gRN == MAX) gRN = 0;
                    //alert(gRN);
                    DownKeyCheck(gRN + 1);
                }
            }
            return true;
        }

        function Select(txtParent) {
            //alert(txtParent);
            globalUDC = gID;
            globalTxtbox = txtParent;
            if (gID != null) {
                if (event.keyCode == 13) {
                    self.opener.addToParentList(globalUDC, globalTxtbox, gName);
                    window.close();
                }
            }
        }

        function KeyCheck(kID, kName, IN, bb, ID, dd, NameP) {

//            
//            alert(kID);
//            alert(kName);
//            alert(IN);
//            alert(bb);
//            alert(ID);
//            alert(dd);
//            alert(NameP);

            gID = kID;
            gName = kName;
            globalid = gID;

            globalUDC = IN;
            globalTxtbox = bb;
            var str;
            str = ID;
            arID = str.split('###');
            arNameP = NameP.split('###');
            //MAX=arID.length-1;
            //alert(document.getElementById('txtHIDCount').value);
            MAX = parseInt(document.getElementById('txtHIDCount').value);
            gRN = IN - 1;
            gID = arID[gRN];
//            alert('gRN'+gRN);
            gName = arNameP[gRN];
//            alert('gname' + gName);
            gRN = parseInt(gRN);
            tableID = document.getElementById('grdsearch');
            if (tableID && IN != 0) {
                for (var i = 1; i < tableID.rows.length; i++) {
                    if (i % 2 == 0) {
                        tableID.rows[i].style.backgroundColor = "#f5f5f5";
                    }
                    else {
                        tableID.rows[i].style.backgroundColor = "#ffffff";
                    }
                }
                tableID.rows[IN].style.backgroundColor = "#d4d4d4";
                //document.getElementById('Textbox1').value=gID;
//                alert(gName);
                document.getElementById('txtFastSearch').value = gName;
            }

        }

        function DownKeyCheck(SR) {
            //alert(MAX);
            globalid = gID;
            tableID = document.getElementById('grdsearch');
            if (tableID && SR != 0) {
                for (var i = 1; i < tableID.rows.length; i++) {
                    if (i % 2 == 0) {
                        tableID.rows[i].style.backgroundColor = "#f5f5f5";
                    }
                    else {
                        tableID.rows[i].style.backgroundColor = "#ffffff";
                    }
                }
                tableID.rows[SR].style.backgroundColor = "#d4d4d4";
                gID = arID[gRN];
                gName = arNameP[gRN];
                //document.getElementById('TextBox1').value=gID;
                document.getElementById('txtFastSearch').value = gName;
            }
            gRN = gRN + 1;
            if (gRN >= MAX) {
                gRN = 0;
            }

        }
        function UpKeyCheck(SR) {
            //alert(SR);
            globalid = gID;
            tableID = document.getElementById('grdsearch');
            if (tableID && SR != 0) {
                for (var i = 1; i < tableID.rows.length; i++) {
                    if (i % 2 == 0) {
                        tableID.rows[i].style.backgroundColor = "#f5f5f5";
                    }
                    else {
                        tableID.rows[i].style.backgroundColor = "#ffffff";
                    }
                }
                tableID.rows[SR].style.backgroundColor = "#d4d4d4";
                gID = arID[gRN - 2];
                gName = arNameP[gRN - 2];
                //document.getElementById('TextBox1').value=gID;
                document.getElementById('txtFastSearch').value = gName;
            }
            gRN = gRN - 1;
            if (gRN <= 0) {
                gRN = MAX + 1;
            }

        }
        function KeyCheck55(aa, bb, cc, strName) {

//            alert(aa);
//            alert(bb);
//            alert(cc);
//            alert(strName);
            
            //self.opener.addToParentList(aa,bb);
            globalUDC = gID;
            //alert(gName);
            self.opener.addToParentList(globalUDC, globalTxtbox, gName);
            window.close();
        }

        function closeWindow(varImgValue) {
            globalUDC = gID;
            //alert(varImgValue);
            if (varImgValue == 'Ok') {
                if (globalid == null) {
                    alert("Please select the row");
                }
                else {

                    self.opener.addToParentList(globalUDC, globalTxtbox, gName);
                    window.close();
                    return false;
                }

            }

            if (varImgValue == 'Close') {
                window.close();
                return false;
            }
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

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" 
    rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" runat="server">
    <table id="Table1" bordercolor="activeborder" height="28" cellspacing="0" cellpadding="0"
        background="../../images/top_nav_back.gif" width="100%" border="0">
        <tr>
            <td align="left" width="202" height="16" nowrap="nowrap">
                <asp:Panel ID="ooo" runat="server" DefaultButton="btnSearch" TabIndex="0">
                    <asp:Button ID="btnSearch" runat="server" Width="0px" Height="0px" BorderStyle="None"
                        BorderWidth="0px" BackColor="#8AAFE5" BorderColor="#8AAFE5"></asp:Button>
                    <span><asp:Label ID="Label1" runat="server" Width="104px" Height="8px" ForeColor="Teal"
                        Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">&nbsp;UDC SEARCH</asp:Label>
                    <asp:TextBox ID="txtFastSearch" Width="82px" Height="18" CssClass="txtFocus" runat="server"
                        ReadOnly="true"></asp:TextBox>&nbsp;</span>
                </asp:Panel>
            </td>
            <td align="left" colspan="1" rowspan="1">
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif">
                </asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif">
                </asp:ImageButton>&nbsp;
            </td>
            <td align="right">
                &nbsp;&nbsp;
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table16" bordercolor="activeborder" height="400" cellspacing="0" cellpadding="0"
        width="100%" border="1">
        <tr>
            <td valign="top" colspan="1">
                <table width="100%" border="0">
                    <tr>
                        <td colspan="2">
                            <cc1:CollapsiblePanel ID="cpnlErrorPanel" runat="server" Text="Error Message" Width="100%"
                                Height="47px" BorderWidth="0px" BorderStyle="Solid" BorderColor="Indigo" Draggable="False"
                                CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
                                TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
                                TitleCSS="test" Visible="False">
                                <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td colspan="0" rowspan="0">
                                            <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="../../Images/warning.gif">
                                            </asp:Image>
                                        </td>
                                        <td colspan="0" rowspan="0">
                                            &nbsp;
                                            <asp:Label ID="lblError" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </cc1:CollapsiblePanel>
                        </td>
                    </tr>
                    <!-- **************************************************************************-->
                    <tr>
                        <td valign="top" colspan="2" height="1">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td nowrap="nowrap" id="pndgsrch" runat="server">
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="Button4">
                                <span style="display: none">
                                    <asp:Button ID="Button4" BorderWidth="0px" runat="server" Width="0px"></asp:Button></span>
                                <div style="overflow: auto; width: 100%; height: 400px">
                                    <asp:DataGrid ID="grdsearch" runat="server" ForeColor="MidnightBlue" Font-Names="Verdana"
                                        BorderWidth="1px" BorderStyle="None" BorderColor="Silver" Font-Size="8pt" PagerStyle-Visible="False"
                                        CellPadding="0" DataKeyField="ID">
                                        <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                        <SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="OddTableRow"></AlternatingItemStyle>
                                        <ItemStyle Font-Size="8pt" ForeColor="Black" BackColor="White"></ItemStyle>
                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" CssClass="GridFixedHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:ButtonColumn Visible="False" CommandName="select">
                                                <ItemStyle Width="12pt"></ItemStyle>
                                            </asp:ButtonColumn>
                                        </Columns>
                                        <PagerStyle Visible="False" HorizontalAlign="Left" ForeColor="#000066" BackColor="White"
                                            Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid></div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <!-- *****************************************************************************-->
                </table>
            </td>
        </tr>
    </table>
    <input style="left: 0px; position: absolute; top: 547px" type="hidden" name="txthidden">
    <input type="hidden" runat="server" id="txtHIDCount" name="txtCount">
    <asp:Button ID="btsearch" Style="left: 0px; position: absolute; top: 547px" runat="server"
        Width="0%" Text=""></asp:Button>
    </form>
</body>
</html>
