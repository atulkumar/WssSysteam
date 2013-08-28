<%@ page language="VB" autoeventwireup="false" inherits="MobileBillsModule_TraceMobileNo, App_Web_b3coazgw" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="cc2" Namespace="ION.Web" Assembly="IONPOP" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HomePage</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../Images/Js/StyleSheet1.css" />

    <script type="text/javascript">

        function MIN() {
            //window.blur();		
            //self.parent.resizeTo(0,0);
            //window.moveTo(500,500);
        }
      

        function ShowEvent(strDate) {

            //alert(strDate);
            //alert();
            wopen('MessageCenter/HomeCalender/Events.aspx?strDate=' + strDate, 'IONPOP', 430, 400);
            return false;
        }
        function SaveEdit(varImgValue) {

            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }
            if (varImgValue == 'Help') {
                document.Form1.txthiddenImage.value = varImgValue;
                //Form1.submit(); 
                //wopen("Help/WSSHelp.aspx?ScreenID=9999","HomeHelp",500,500);
                var topPos;
                leftPos = (screen.width / 2) - 250;
                topPos = (screen.height / 2) - 250;
                window.open('Help/WSSHelp.aspx?ScreenID=9999', 'posA', 'overfilter="Alpha(opacity=75);";style=ScrollingSampStyle;toolbar=no, titlebar=no,width=500,height=555,top=' + topPos + ',left=' + leftPos);
            }


        }
         function LogoutWSS() {
            document.Form1.txthiddenImage.value = 'Logout';
            Form1.submit();
        }
function onlyNumbers(evt)
{
	var e = event || evt; // for trans-browser compatibility
	var charCode = e.which || e.keyCode;

	if (charCode > 31 && (charCode < 48 || charCode > 57))
		return false;

	return true;

}
        function callrefresh() {
            Form1.submit();
        }
        function wopen(url, name, w, h) {
            w += 32;
            h += 96;
            wleft = (screen.width - w) / 2;
            wtop = (screen.height - h) / 2;
            var win = window.open(url,
						name,
						'width=' + w + ', height=' + h + ', ' +
						'left=' + wleft + ', top=' + wtop + ', ' +
						'location=no, menubar=no, ' +
						'status=no, toolbar=no, scrollbars=no, resizable=yes');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }

        function OpenComment(Level, CompID, CN, TN, AN) {
            wopen('SupportCenter/Callview/comment.aspx?ScrID=464&From=Home&Level=' + Level + '&CID=' + CompID + '&CN=' + CN + '&TN=' + TN + '&AN=' + AN, 'Comment', 500, 450);
        }
		
    </script>

    <script type="text/javascript">
        //A function to open call detail in new tab after dblclick on Call Grid Row
        function openCallDetailsInTab(CallNo, CompId) {
            //Calling the Parent window function
          
            window.parent.OpenCallDetailInTab('Call# ' + CallNo, 'SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=5&CallNumber=' + CallNo + '&CompId=' + CompId, 'Call' + CallNo,'-1')

        }
        //A function to open To Do List  in new tab after dblclick on Task Grid Row
        function openToDoListInTab(CallNo, CompId) {
            //Calling the Parent window function
          
           window.parent.OpenCallDetailInTab('To Do List', 'WorkCenter/DoList/ToDoList.aspx?ScrID=8&CallNumber=' + CallNo + "&CompId=" + CompId, '8','-1')
        }
        //A function to open Comment View  in new tab after click on View Old Comments link
        function openCommentViewInTab() {
            //Calling the Parent window function
           
            window.parent.OpenCallDetailInTab('Comment View', 'SupportCenter/CallView/Comment_View.aspx', '968', '-1')
        }
    </script>

    <style type="text/css">
        .style1
        {
            height: 49px;
        }
        .style2
        {
            width: 10%;
            height: 49px;
        }
    </style>
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <div align="left" style="width: 100%">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../Images/top_nav_back.gif" valign="middle" class="style1">
                                            <table cellspacing="0" cellpadding="0" width="94%" border="0">
                                                <tr>
                                                    <td style="width: 20%">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label ID="lblTitleLabelWssHome" runat="server" CssClass="TitleLabel" BorderStyle="None">WSS HOME</asp:Label>
                                                    </td>
                                                    <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                        <center>
                                                            <asp:ImageButton ID="imgSearch" AccessKey="S" runat="server" ImageUrl="../Images/s1search02.gif"
                                                                ToolTip="Search"></asp:ImageButton>
                                                            <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />&nbsp;
                                                            <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;&nbsp;
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="border-collapse: collapse" class="panel">
                            </td>
                            <table class="panel" width="100%">
                                <tr>
                                    <td align="left">
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" Width="90px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True"
                                            ID="Label3" Text="Select Category"></asp:Label>
                                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" Width="148px"
                                            Visible="true">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Trace Ion Number</asp:ListItem>
                                            <asp:ListItem Value="2">Trace Ion no call to other No</asp:ListItem>
                                            <asp:ListItem Value="3">Trace other Number</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="Label1" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                            Width="80px" runat="server">Date Range
                                        </asp:Label>
                                        <ION:Customcalendar ID="dtFromDate" runat="server" Height="16px" Width="148px" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblToDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                            Width="30px" runat="server">To</asp:Label>
                                        <ION:Customcalendar ID="dtToDate" runat="server" Height="16px" Width="148px" />
                                    </td>
                                    <%-- <td style="width:500px">
                                    </td>--%>
                                    <asp:Panel ID="Panel4" runat="server" Visible="false">
                                        <td align="left">
                                            <asp:Label runat="server" Width="90px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True"
                                                ID="lblCalledNo" Text="Called Number"></asp:Label>
                                            <asp:TextBox ID="txtCalledNo" AutoCompleteType="Disabled" Onkeypress="return onlyNumbers(event)"
                                                runat="server" Width="148px"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:Label runat="server" Width="90px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True"
                                                ID="Label2" Text="Select Duration"></asp:Label>
                                            <asp:DropDownList ID="ddlTime" runat="server" Width="148px" Visible="true">
                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                <asp:ListItem Value="10">10min</asp:ListItem>
                                                <asp:ListItem Value="20">20min</asp:ListItem>
                                                <asp:ListItem Value="30">30min</asp:ListItem>
                                                <asp:ListItem Value="40">40min</asp:ListItem>
                                                <asp:ListItem Value="50">50min</asp:ListItem>
                                                <asp:ListItem Value="60">60min</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </asp:Panel>
                                </tr>
                                <tr style="border-collapse: collapse" class="panel">
                                    <td align="left">
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" Width="90px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True"
                                            ID="lblMobileNo" Text="Select Name"></asp:Label>
                                        <telerik:RadComboBox ID="CDDLmobileNo" Font-Names="Verdana" Font-Size="7pt" runat="server"
                                            Width="150px" DropDownWidth="180px" Height="150px" AppendDataBoundItems="true"
                                            DataTextField="Description" DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith"
                                            NoWrap="true" EmptyMessage="Select User" AllowCustomText="True" EnableTextSelection="true"
                                            EnableVirtualScrolling="true">
                                        </telerik:RadComboBox>
                                        <asp:TextBox ID="txtMobileNo" AutoCompleteType="Disabled" Visible="false" Onkeypress="return onlyNumbers(event)"
                                            runat="server" Width="148px"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <div style="overflow: auto; width: 100%;">
                        <table style="border-collapse: collapse" class="panel" width="100%" border="0">
                            <tr>
                                <td valign="top" width="51%">
                                    <!-- *****************************************-->
                                    <cc1:CollapsiblePanel ID="cpnlCallDetails" runat="server" Width="100%" BorderStyle="Solid"
                                        BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                        TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent" Text="Call Details"
                                        ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
                                        <div style="overflow: auto; width: 100%; height: 190pt">
                                            <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                align="left" border="0">
                                                <tr>
                                                    <td valign="top" align="left">
                                                        <!--  **********************************************************************-->
                                                        <telerik:RadGrid ID="rgCallDetail" Width="500px" runat="server" Skin="Office2007"
                                                            AutoGenerateColumns="false" AllowMultiRowSelection="true" AllowFilteringByColumn="false">
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true" />
                                                            </ClientSettings>
                                                            <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                                <Pdf FontType="Subset" PaperSize="Letter" />
                                                                <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                            </ExportSettings>
                                                            <MasterTableView AllowAutomaticInserts="True">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="called_num" HeaderStyle-Width="15%" HeaderText="Called Number"
                                                                        UniqueName="CalledNumber" HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="CalledPersonName" HeaderStyle-Width="55%" AllowFiltering="false"
                                                                        ShowFilterIcon="false" HeaderText="Called Person Name/Company" UniqueName="Number"
                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="ChargedAmount" HeaderStyle-Width="15%" HeaderText="Charges"
                                                                        UniqueName="Charges_Amt" HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="Official_Flag" HeaderStyle-Width="15%" HeaderText="Personal/Official"
                                                                        UniqueName="Official_Flag" HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                        <!-- ***********************************************************************-->
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </cc1:CollapsiblePanel>
                                    <!-- *****************************************-->
                                </td>
                                <td valign="top" width="48%">
                                    <!-- *****************************************-->
                                    <cc1:CollapsiblePanel ID="cpnlTop5No" runat="server" Width="100%" BorderStyle="Solid"
                                        BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                        TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent" Text="Top 5 Called Numbers"
                                        ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
                                        <div style="overflow: hidden; width: 100%; height: 190pt; bordercolor: Indigo; borderstyle: Solid;">
                                            <table id="Table11" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                align="left" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadGrid ID="rgTop5Number" Width="400px" runat="server" Skin="Office2007"
                                                            AutoGenerateColumns="false" AllowMultiRowSelection="true" AllowFilteringByColumn="false">
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true" />
                                                            </ClientSettings>
                                                            <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                                <Pdf FontType="Subset" PaperSize="Letter" />
                                                                <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                            </ExportSettings>
                                                            <MasterTableView AllowAutomaticInserts="True">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="called_num" HeaderText="Called Number" UniqueName="CalledNumber"
                                                                        HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="Charges_Amt" HeaderText="Charges" UniqueName="Charges_Amt"
                                                                        HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="Number" AllowFiltering="false" ShowFilterIcon="false"
                                                                        HeaderText="No of time Called" UniqueName="Number" HeaderStyle-HorizontalAlign="Center">
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="left">
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </cc1:CollapsiblePanel>
                                    <!-- *****************************************-->
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="48%">
                                    <!-- *****************************************-->
                                    <cc1:CollapsiblePanel ID="cpnlCategoryWise" runat="server" Width="100%" BorderStyle="Solid"
                                        BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                        TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent" Text="Category wise Call Deatil"
                                        ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
                                        <div style="overflow: auto; width: 100%; height: 190pt">
                                            <telerik:RadGrid ID="rgCategoryWise" Width="500px" runat="server" Skin="Office2007"
                                                AutoGenerateColumns="false" AllowMultiRowSelection="true" AllowFilteringByColumn="false">
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" />
                                                </ClientSettings>
                                                <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                    <Pdf FontType="Subset" PaperSize="Letter" />
                                                    <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                </ExportSettings>
                                                <MasterTableView AllowAutomaticInserts="True">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="called_num" HeaderText="Called Number" UniqueName="CalledNumber"
                                                            HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="CalledPersonName" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Called Person Name/Company" UniqueName="Number" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ChargedAmount" HeaderText="Charges" UniqueName="Charges_Amt"
                                                            HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Category" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Category" UniqueName="Category" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </cc1:CollapsiblePanel>
                                    <!-- *****************************************-->
                                </td>
                                <td valign="top" width="48%">
                                    <!-- ****************************************-->
                                    <cc1:CollapsiblePanel ID="cpnlAlerts" runat="server" Width="100%" BorderStyle="Solid"
                                        BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                        TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent" Text=""
                                        ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
                                        <div style="overflow: auto; width: 100%; height: 185pt">
                                            <asp:DataGrid ID="dgrCommentAlert" runat="server" CssClass="Grid" Font-Names="Verdana"
                                                BorderWidth="0px" BorderStyle="None" BorderColor="Silver" ForeColor="MidnightBlue"
                                                CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left" BackColor="White">
                                                <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                <SelectedItemStyle CssClass="GridSelectedItem" BackColor="Silver"></SelectedItemStyle>
                                                <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                </ItemStyle>
                                                <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                    BackColor="#E0E0E0"></HeaderStyle>
                                            </asp:DataGrid></div>
                                    </cc1:CollapsiblePanel>
                                    <!-- *****************************************-->
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" Visible="false">
                    <div>
                        <tr>
                            <td valign="top" width="100%">
                                <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadGrid ID="rgParticularNoDetail" OnSelectedIndexChanged="rgParticularNoDetail_SelectedIndexChanged"
                                                Width="800px" AllowPaging="true" PageSize="10" runat="server" Skin="Office2007"
                                                AutoGenerateColumns="false" AllowMultiRowSelection="false" AllowFilteringByColumn="false">
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" />
                                                </ClientSettings>
                                                <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                    <Pdf FontType="Subset" PaperSize="Letter" />
                                                    <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                </ExportSettings>
                                                <MasterTableView AllowAutomaticInserts="True">
                                                    <Columns>
                                                        <telerik:GridButtonColumn Text="Select" CommandName="Select" />
                                                        <telerik:GridBoundColumn DataField="called_num" HeaderText="Called Number" UniqueName="CalledNumber"
                                                            HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ChargedAmount" HeaderText="Charges" UniqueName="ChargedAmount"
                                                            HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="calledDate" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Called Date" UniqueName="calledDate" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Called_Time" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Called Time" UniqueName="Called_Time" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Duration_Vol" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Duration" UniqueName="Duration_Vol" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="CalledPersonName" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Called Person Name/Company" UniqueName="CalledPersonName" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Category" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Category" UniqueName="Category" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Official_Flag" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Personal/Official" UniqueName="Official_Flag" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <br />
                                            <br />
                                            <asp:Panel ID="Panel3" runat="server" Visible="false">
                                                <asp:Label ID="lbl" runat="server"></asp:Label>
                                                <telerik:RadGrid ID="RadGrid1" OnSelectedIndexChanged="rgParticularNoDetail_SelectedIndexChanged"
                                                    Width="800px" AllowPaging="true" PageSize="10" runat="server" Skin="Office2007"
                                                    AutoGenerateColumns="false" AllowMultiRowSelection="true" AllowFilteringByColumn="false">
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="true" />
                                                    </ClientSettings>
                                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                        <Pdf FontType="Subset" PaperSize="Letter" />
                                                        <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                    </ExportSettings>
                                                    <MasterTableView AllowAutomaticInserts="True">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="called_num" HeaderText="Called Number" UniqueName="CalledNumber"
                                                                HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ChargedAmount" HeaderText="Charges" UniqueName="ChargedAmount"
                                                                HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="calledDate" AllowFiltering="false" ShowFilterIcon="false"
                                                                HeaderText="Called Date" UniqueName="calledDate" HeaderStyle-HorizontalAlign="Center">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Called_Time" AllowFiltering="false" ShowFilterIcon="false"
                                                                HeaderText="Called Time" UniqueName="Called_Time" HeaderStyle-HorizontalAlign="Center">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Duration_Vol" AllowFiltering="false" ShowFilterIcon="false"
                                                                HeaderText="Duration" UniqueName="Duration_Vol" HeaderStyle-HorizontalAlign="Center">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="CalledPersonName" AllowFiltering="false" ShowFilterIcon="false"
                                                                HeaderText="Called Person Name/Company" UniqueName="CalledPersonName" HeaderStyle-HorizontalAlign="Center">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Category" AllowFiltering="false" ShowFilterIcon="false"
                                                                HeaderText="Category" UniqueName="Category" HeaderStyle-HorizontalAlign="Center">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Official_Flag" AllowFiltering="false" ShowFilterIcon="false"
                                                                HeaderText="Personal/Official" UniqueName="Official_Flag" HeaderStyle-HorizontalAlign="Center">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlCallingDetail" Visible="false" runat="server">
                    <div>
                        <tr>
                            <td valign="top" width="100%">
                                <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                                    <tr>
                                        <td align="center">
                                            <br />
                                            <telerik:RadGrid ID="rgcallingDetail" Width="800px" AllowPaging="true" PageSize="15"
                                                runat="server" Skin="Office2007" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                                                AllowFilteringByColumn="false">
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" />
                                                </ClientSettings>
                                                <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                    <Pdf FontType="Subset" PaperSize="Letter" />
                                                    <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                </ExportSettings>
                                                <MasterTableView AllowAutomaticInserts="True">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="User_Name" HeaderText="Caller" UniqueName="User_Name"
                                                            HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="owner_num" HeaderText="Calling Number" UniqueName="owner_num"
                                                            HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ChargedAmount" HeaderText="Charges" UniqueName="ChargedAmount"
                                                            HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="calledDate" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Called Date" UniqueName="calledDate" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Called_Time" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Called Time" UniqueName="Called_Time" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Duration_Vol" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Duration" UniqueName="Duration_Vol" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Category" AllowFiltering="false" ShowFilterIcon="false"
                                                            HeaderText="Category" UniqueName="Category" HeaderStyle-HorizontalAlign="Center">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </td> </tr> </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <asp:ListBox ID="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" Font-Size="X-Small"
                Font-Names="Verdana" ForeColor="Red" Width="100px" Height="32px" Visible="false">
            </asp:ListBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="hidden" name="txthiddenImage" />
    </form>
</body>
</html>
