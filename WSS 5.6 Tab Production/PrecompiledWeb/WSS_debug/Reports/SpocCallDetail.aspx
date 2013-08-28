<%@ page language="VB" autoeventwireup="false" inherits="Reports_SpocCallDetail, App_Web_sbqj3uth" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Spoc Call Detail</title>
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlRS_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlReport_collapsible').cells[0].colSpan = "1";
        }
        //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <telerik:RadScriptManager ID="radscript" runat="server">
    </telerik:RadScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 30%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">Spoc Issue </asp:Label>
                                                </td>
                                                <td style="width: 60%; text-align: center;" nowrap="nowrap">
                                                    <center>
							<asp:ImageButton ID="imgOK" runat="server" ToolTip="Search" ImageUrl="../Images/s1search02.gif">
                                                        </asp:ImageButton>
							<asp:ImageButton ID="imgExportToPDF" AccessKey="E" runat="server" ImageUrl="../Images/CloseCall1.gif"
                                                        ToolTip="View Closed Calls"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgExportToExcel" AccessKey="E" runat="server" ImageUrl="../Images/Excel.jpg"
                                                            ToolTip="Export To Excel"></asp:ImageButton>
                                                       
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 5%" background="../Images/top_nav_back.gif" height="47">
                                        <%--<img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(2192,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />--%>
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="1">
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                    Width="100%" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                    ExpandImage="../Images/ToggleDown.gif" Text="Spoc Weekly Report" TitleBackColor="Transparent"
                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                    Height="0px">
                                                    <table id="Table2" cellspacing="4" cellpadding="1" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblFromDate" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                    runat="server"> From Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <ION:Customcalendar ID="dtFromDate" runat="server" Height="16px" Width="148px" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblToDate" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                    runat="server">To Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <ION:Customcalendar ID="dtToDate" runat="server" Height="16px" Width="148px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                    CollapseImage="../images/ToggleUp.gif" Draggable="False" ExpandImage="../images/ToggleDown.gif"
                                                    PanelCSS="panel" Text="Spoc Weekly Report" TitleBackColor="Transparent" TitleClickable="True"
                                                    TitleCSS="test" TitleForeColor="black" Visible="true" Width="100%">
                                                    <table align="left">
                                                        <tr>
                                                            <td valign="top" align="left" colspan="1" rowspan="1">
                                                                <telerik:RadGrid ID="grdSearch" Visible="false" Width="1260" Height="400" runat="server"
                                                                    AllowFilteringByColumn="true" AutoGenerateColumns="false" PageSize="20" AllowSorting="true"
                                                                    AllowPaging="true" Skin="Office2007">
                                                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                                        <Pdf PageHeight="250mm" PageWidth="440mm" PageTitle="Orders Details" PaperSize="A4"
                                                                            PageLeftMargin="10mm" PageRightMargin="10mm" />
                                                                        <Excel Format="ExcelML" />
                                                                        <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                                    </ExportSettings>
                                                                    <MasterTableView>
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn DataField="CM_NU9_Call_No_PK" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="CallNo" ReadOnly="True" SortExpression="CM_NU9_Call_No_PK" UniqueName="CallNo"
                                                                                AllowFiltering="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CM_DT8_Request_Date" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="RequestDate" ReadOnly="True" SortExpression="CM_DT8_Request_Date"
                                                                                UniqueName="RequestDate" AllowFiltering="true" CurrentFilterFunction="Contains"
                                                                                AutoPostBackOnFilter="true">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="TaskOwner" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="TaskOwner" ReadOnly="True" SortExpression="TaskOwner" UniqueName="TaskOwner"
                                                                                AllowFiltering="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CM_VC8_Call_Type" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="CallType" ReadOnly="True" SortExpression="CM_VC8_Call_Type" UniqueName="CallType"
                                                                                AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CM_VC100_Subject" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="Issue Description" ReadOnly="True" SortExpression="CM_VC100_Subject"
                                                                                UniqueName="CM_VC100_Subject" FilterControlWidth="200px" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="ActionDesc" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="Solution Provided"  FilterControlWidth="200px" ReadOnly="True" SortExpression="ActionDesc" UniqueName="ActionDesc"
                                                                                AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CM_VC2000_Call_Desc" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="Call Description" ReadOnly="True" SortExpression="CM_VC2000_Call_Desc"
                                                                                UniqueName="CallDesc" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CM_DT8_Close_Date" ShowFilterIcon="false" HeaderText="Est Close Date"
                                                                                ReadOnly="True" SortExpression="CM_DT8_Close_Date" UniqueName="CM_DT8_Close_Date"
                                                                                AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CM_DT8_Call_Close_Date" ShowFilterIcon="false"
                                                                                HeaderText="ActualCloseDate" ReadOnly="True" SortExpression="CM_DT8_Call_Close_Date"
                                                                                UniqueName="CM_DT8_Call_Close_Date" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CM_VC200_Work_Priority" ShowFilterIcon="false"
                                                                                DataType="System.String" HeaderText="Priority" ReadOnly="True" SortExpression="CM_VC200_Work_Priority"
                                                                                UniqueName="Priority" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="ProjectName" DataType="System.String" HeaderText="ProjectName"
                                                                                ReadOnly="True" SortExpression="ProjectName" UniqueName="ProjectName" AllowFiltering="true"
                                                                                CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CallOwner" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="CallOwner" ReadOnly="True" SortExpression="CallOwner" UniqueName="CallOwner"
                                                                                AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CallOwnerDepartment" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="Department" ReadOnly="True" SortExpression="CallOwnerDepartment"
                                                                                UniqueName="CallOwnerDepartment" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="CN_VC20_Call_Status" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="Call Status" ReadOnly="True" SortExpression="CN_VC20_Call_Status"
                                                                                UniqueName="CN_VC20_Call_Status" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="UsedHrs" ShowFilterIcon="false" DataType="System.String"
                                                                                HeaderText="UsedHrs" ReadOnly="True" SortExpression="UsedHrs" UniqueName="UsedHrs"
                                                                                AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <SelectedItemStyle BackColor="#FFCC66"></SelectedItemStyle>
                                                                    <ItemStyle Font-Size="8pt"></ItemStyle>
                                                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="True" />
                                                                    <AlternatingItemStyle Font-Size="8pt" BackColor="#F5F5F5" />
                                                                    <ClientSettings>
                                                                        <Selecting AllowRowSelect="True" />
                                                                        <Scrolling AllowScroll="true" ScrollHeight="100%" UseStaticHeaders="true" />
                                                                    </ClientSettings>
                                                                    <HeaderStyle Font-Size="8pt" Font-Bold="true" ForeColor="Black" BorderColor="White"
                                                                        BackColor="#E0E0E0"></HeaderStyle>
                                                                    <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                </telerik:RadGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%--<table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="99%">
                                                        <tr>
                                                            <td valign="top" align="left" colspan="1" rowspan="1">
                                                                <CR:CrystalReportViewer ID="crvReport" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="False"
                                                                    EnableParameterPrompt="False" DisplayGroupTree="False" HasCrystalLogo="False"
                                                                    HasRefreshButton="True" HasToggleGroupTreeButton="False" HasZoomFactorList="False"
                                                                    PrintMode="ActiveX" ReuseParameterValuesOnRefresh="True" Height="480px" EnableTheming="true"
                                                                    BestFitPage="False" Width="1000px" HasPageNavigationButtons="False" SeparatePages="False" />
                                                            </td>
                                                        </tr>
                                                    </table>--%>
                                                </cc1:CollapsiblePanel>                                                
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <asp:Panel ID="pnlMsg" runat="server">
                    </asp:Panel>
                    <asp:ListBox ID="lstError" runat="server" Width="465px" ForeColor="Red" Font-Names="Verdana"
                        Visible="false" Font-Size="XX-Small" BorderStyle="Groove" BorderWidth="0" Height="32px">
                    </asp:ListBox>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
