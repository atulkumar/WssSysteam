<%@ page language="VB" autoeventwireup="false" inherits="Reports_WeeklyTaskReport, App_Web_d6f9x4fa" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Multi Task forward</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../DateControl/ION.js"></script>

    <script type="text/javascript" src="../Images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../Images/Js/ABMainShortCuts.js"></script>

    <%-- <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative; TOP:expression(this.offsetParent.scrollTop);BACKGROUND-COLOR:#e0e0e0
        }</style>
--%>

    <script type="text/javascript">
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
        function CheckBox(ID) {
            if (document.getElementById(ID).checked == true) {
                document.getElementById(ID).checked = false;
            }
            else {
                document.getElementById(ID).checked = true;
            }
        }
        function CheckAll(ID) {
            var tableID = 'cpnlSearch_GrdTask';
            var table;
            table = document.getElementById(tableID);
            var n = table.rows.length - 1;
            if (document.getElementById(ID).checked == true) {
                for (var i = 0; i < n; i++) {
                    if (i + 2 < 10) {
                        document.getElementById('cpnlSearch_GrdTask_ctl0' + (i + 2) + '_chkReq').checked = true;
                    }
                    else {
                        document.getElementById('cpnlSearch_GrdTask_ctl' + (i + 2) + '_chkReq').checked = true;
                    }
                }
            }
            else {
                for (var i = 0; i < n; i++) {
                    if (i + 2 < 10) {
                        document.getElementById('cpnlSearch_GrdTask_ctl0' + (i + 2) + '_chkReq').checked = false;
                    }
                    else {
                        document.getElementById('cpnlSearch_GrdTask_ctl' + (i + 2) + '_chkReq').checked = false;
                    }
                }
            }
        }
        function CheckStatus() {
            var chkID = 'cpnlSearch_GrdTask_ctl01_chkAll';
            var chk = 0;
            var unchk = 0;
            var tableID = 'cpnlSearch_GrdTask';
            var table;
            if (document.all) table = document.all[tableID];
            if (document.getElementById) table = document.getElementById(tableID);
            var n = table.rows.length - 1;
            for (var i = 0; i < n; i++) {
                var Temp;
                if (i + 2 < 10) {
                    Temp = 'cpnlSearch_GrdTask_ctl0' + (i + 2) + '_chkReq'
                }
                else {
                    Temp = 'cpnlSearch_GrdTask_ctl' + (i + 2) + '_chkReq'
                }
                if (document.getElementById(Temp).checked == true) {
                    chk = 1;
                }
                else {
                    unchk = 1;
                }
            }
            if (chk == 1 && unchk == 1) {
                document.getElementById(chkID).checked = false;
            }
            if (chk == 1 && unchk == 0) {
                document.getElementById(chkID).checked = true;
            }
            if (chk == 0 && unchk == 1) {
                document.getElementById(chkID).checked = false;
            }
        }
        function closeWindow(varImgValue) {
            if (varImgValue == 'Close') {
                window.close();
                return false;
            }
            return false;
        }
        function KeyCheck(rowvalues) {
            if (rowvalues < 10) {
                if (document.getElementById('cpnlSearch_GrdTask_ctl0' + (rowvalues + 1) + '_chkReq').checked == true) {
                    document.getElementById('cpnlSearch_GrdTask_ctl0' + (rowvalues + 1) + '_chkReq').checked = false;
                }
                else {
                    document.getElementById('cpnlSearch_GrdTask_ctl0' + (rowvalues + 1) + '_chkReq').checked = true;
                }
            }
            else {
                if (document.getElementById('cpnlSearch_GrdTask_ctl' + (rowvalues + 1) + '_chkReq').checked == true) {
                    document.getElementById('cpnlSearch_GrdTask_ctl' + (rowvalues + 1) + '_chkReq').checked = false;
                }
                else {
                    document.getElementById('cpnlSearch_GrdTask_ctl' + (rowvalues + 1) + '_chkReq').checked = true;
                }
            }
            CheckStatus();
            var tableID = 'cpnlSearch_GrdTask'  //your datagrids id
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
                table.rows[rowvalues].style.backgroundColor = "#d4d4d4";
            }
            if (tableID == 'cpnlSearch_GrdTask') {
                document.Form1.txthiddenImage.value = 'Select';
                //setTimeout('Form1.submit();',700);
                //Form1.submit(); 
            }
        }
        function SaveEdit(varImgValue) {
            if (varImgValue == 'Close') {
                location.href = "../../home.aspx"
                return false;
            }
            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }
            if (varImgValue == 'Close') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }
            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset()
                }
                return false;
            }
            if (varImgValue == 'Save') {
                document.Form1.txthiddenImage.value = varImgValue;
                //		    	__doPostBack("up2","");
                //		    		return false;		
            }
        }				
    </script>

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
//            var x = document.getElementById('cpnlAdvSearch2_collapsible').cells[0].colSpan = "1";
//            var y = document.getElementById('cpnlSearch_collapsible').cells[0].colSpan = "1";
        }
        //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
        function ClearAnchors() {
            var calendarTable = document.getElementById("<%# dtDateFrom.Calendar.ClientID %>");
            var anchors = calendarTable.getElementsByTagName("a");
            for (var i = 0; i < anchors.length; i++) {
                var anchor = anchors[i];
                var newHref = anchor.href.replace("javascript:void(0);", "#");
                anchor.href = newHref;
            }
        }
        function OnCalendarViewChanged(step) {
            ClearAnchors();
        }  
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr width="100%">
                        <td background="../Images/top_nav_back.gif" height="47">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td style="width: 20%">
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblTitleLabelSearch" runat="server" CssClass="TitleLabel" BorderWidth="2px"
                                            BorderStyle="None">Weekly Task Report</asp:Label>
                                    </td>
                                    <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                        <center>
                                            <asp:ImageButton ID="imgSearch" AccessKey="S" runat="server" ImageUrl="../Images/s1search02.gif"
                                                ToolTip="Search"></asp:ImageButton>
                                            <asp:ImageButton ID="imgExportToExcel" AccessKey="E" runat="server" ImageUrl="../Images/Excel.jpg"
                                                ToolTip="Export To Excel"></asp:ImageButton>
                                            <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                            &nbsp;<img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                style="cursor: hand;" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif"
                            height="47">
                            <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="width: 100%;">
                    <table id="Table32" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td valign="top" align="left">
                                <asp:UpdatePanel ID="up1" runat="server">
                                    <ContentTemplate>
                                        <cc1:CollapsiblePanel ID="cpnlAdvSearch2" runat="server" Width="100%" BorderWidth="0px"
                                            BorderStyle="Solid" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                            ExpandImage="../../Images/ToggleDown.gif" Text="Search Criteria" TitleBackColor="Transparent"
                                            TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                            <table cellspacing="0px" cellpadding="0px" width="700px" border="0px" style="width: 100%">
                                                <tr>
                                                    <td style="height:30px;">
                                                        <asp:Label ID="Label5" runat="server" CssClass="FieldLabel">Team</asp:Label><br>
                                                        <asp:DropDownList ID="ddlTeam" TabIndex="3" Width="113px" CssClass="txtNoFocus" AutoPostBack="True"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">Team Lead</asp:Label><br>
                                                        <asp:DropDownList ID="ddlTL" TabIndex="3" Width="113px" CssClass="txtNoFocus" AutoPostBack="True"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">Task Owner</asp:Label><br>
                                                        <span style="width: 100%; height: 100%">
                                                            <asp:DropDownList ID="ddlTaskOwner" runat="server" AutoPostBack="true" CssClass="txtNoFocus"
                                                                TabIndex="3" Width="150px">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height:45px;">
                                                        <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">Week Start Date</asp:Label><br>
                                                        <telerik:RadDatePicker ID="dtDateFrom" OnSelectedDateChanged="dtDateFrom_SelectedDateChanged"
                                                            Height="16px" Width="148px" runat="server" AutoPostBack="True" DateInput-AutoPostBack="True">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">Week End Date</asp:Label><br>
                                                        <telerik:RadDatePicker ID="dtDateTO" OnSelectedDateChanged="dtDateTo_SelectedDateChanged"
                                                            Height="16px" AutoPostBack="true" Width="148px" runat="server">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                       <asp:RadioButton ID="rdbCurrentWeeK" GroupName="a" runat="server" Text="Current Week"
                                                            CssClass="FieldLabel" Checked="true" />
                                                        <asp:RadioButton ID="rdbPreviousWeek" runat="server" GroupName="a" Text="Previous Week"
                                                            CssClass="FieldLabel" />
                                                    </td>
                                                    <td>
                                                      <asp:Label ID="Label6" runat="server" CssClass="FieldLabel">Week No.</asp:Label>
                                                        <right>                                                                                                                        
                                                       <asp:Label  ID="lblweekno" runat="server" Text=""></asp:Label></right>
                                                       
                                                    </td>
                                                </tr>
                                               
                                        </cc1:CollapsiblePanel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>

                        <cc1:CollapsiblePanel ID="cpnlSearch" runat="server" Width="100%" BorderWidth="0px"
                            Height="100%" BorderStyle="Solid" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                            ExpandImage="../Images/ToggleDown.gif" Text="Task List" TitleBackColor="Transparent"
                            TitleForeColor="PowderBlue" TitleClickable="true" State="Collapsed" PanelCSS="panel"
                            TitleCSS="test">
                            
                                        <div style="width: 100%">
                                        <table style="table-layout:fixed; position:fixed; margin:0px auto;">
                                        <tr>
                                        <td>
                                        <telerik:RadAjaxPanel ID="AjaxPanel" runat="server" EnableAJAX="true" LoadingPanelID="RadAjaxLoadingPanel1">
                                            <telerik:RadGrid ID="grdTask" OnNeedDataSource="grdTask_NeedDataSource" Visible="True"
                                                Width="100%" Height="440px" runat="server" AllowFilteringByColumn="true" AutoGenerateColumns="false"
                                                PageSize="10" AllowSorting="true" AllowPaging="true" Skin="Office2007" OnItemDataBound="grdTask_ItemDataBound">
                                                <ExportSettings ExportOnlyData="true" OpenInNewWindow="true" IgnorePaging="true">
                                                    <Pdf PageHeight="200mm" PageWidth="440mm" PageTitle="Orders Details" PaperSize="A4"
                                                        PageLeftMargin="10mm" PageRightMargin="10mm" />
                                                    <Excel Format="ExcelML" />
                                                    <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                </ExportSettings>
                                                <MasterTableView>
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="empname" HeaderStyle-Width="10%" ShowFilterIcon="false"
                                                            DataType="System.String" FilterControlWidth="100%"   HeaderText="Employee Name" ReadOnly="True" SortExpression="Empname"
                                                            UniqueName="Employee Name" AllowFiltering="true" CurrentFilterFunction="Contains"
                                                            AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn HeaderStyle-Width="5%" DataField="callNo" HeaderStyle-Height="15px"
                                                            ShowFilterIcon="false" FilterControlWidth="100%"  DataType="System.String" HeaderText="Call No" ReadOnly="True"
                                                            SortExpression="callNo" UniqueName="callNo" AllowFiltering="true" CurrentFilterFunction="Contains"
                                                            AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn HeaderStyle-Width="5%" DataField="taskNo" ShowFilterIcon="false"
                                                            DataType="System.String" HeaderText="Task No" FilterControlWidth="100%"  ReadOnly="True" SortExpression="taskNo"
                                                            UniqueName="taskNo" AllowFiltering="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="taskDescription" HeaderStyle-Width="40%" ShowFilterIcon="false"
                                                            DataType="System.String" HeaderText="Week Task" FilterControlWidth="100%" ReadOnly="True" SortExpression="WeekTask"
                                                            UniqueName="Week Task" AllowFiltering="true" CurrentFilterFunction="Contains"
                                                            AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn DataField="estimatedHours" HeaderStyle-Width="5%" ShowFilterIcon="false"
                                                            DataType="System.String" HeaderText="Estimated Hours" FilterControlWidth="100%"  ReadOnly="True" SortExpression="Estimated Hours"
                                                            UniqueName="estimatedHours" AllowFiltering="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="hours" HeaderStyle-Width="5%" ShowFilterIcon="false"
                                                            DataType="System.String" HeaderText="Hours Used" FilterControlWidth="100%"  ReadOnly="True" SortExpression="HoursUsed"
                                                            UniqueName="Hours Used" AllowFiltering="true" CurrentFilterFunction="Contains"
                                                            AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="types" HeaderStyle-Width="10%" ShowFilterIcon="false"
                                                            DataType="System.String" HeaderText="Planned\Unplanned" FilterControlWidth="100%"  ReadOnly="True" SortExpression="Types"
                                                            UniqueName="Type" AllowFiltering="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="status" HeaderStyle-Width="5%" ShowFilterIcon="false"
                                                            DataType="System.String" HeaderText="Status" ReadOnly="True" FilterControlWidth="100%"  SortExpression="Status"
                                                            UniqueName="status" AllowFiltering="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                           
                                                             <telerik:GridBoundColumn DataField="project" HeaderStyle-Width="20%" ShowFilterIcon="false"
                                                            DataType="System.String" HeaderText="Project" FilterControlWidth="100%"  ReadOnly="True" SortExpression="Project"
                                                            UniqueName="project" AllowFiltering="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                                <SelectedItemStyle BackColor="#FFCC66"></SelectedItemStyle>
                                                <ItemStyle Font-Size="8pt"></ItemStyle>
                                                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="True" />
                                                <AlternatingItemStyle Font-Size="8pt" BackColor="#F5F5F5" />
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="True" />
                                                    <Scrolling AllowScroll="true" ScrollHeight="100%" UseStaticHeaders="false" />
                                                </ClientSettings>
                                                <HeaderStyle Font-Size="8pt" Font-Bold="true" ForeColor="Black" BorderColor="White"
                                                    BackColor="#E0E0E0"></HeaderStyle>
                                                <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                            </telerik:RadGrid>
                                            </telerik:RadAjaxPanel>
                                         </td>
                                        </tr>
                                        </table>
                                                                                 </div>
                                 
  </cc1:CollapsiblePanel>

            </td>
        </tr>
    </table>
    </form>
</body>
</html>
