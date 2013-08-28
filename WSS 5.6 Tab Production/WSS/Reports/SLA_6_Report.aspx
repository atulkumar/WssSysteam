<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SLA_6_Report.aspx.vb" Inherits="Reports_SLA_6_Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Images/Js/JSValidation.js"></script>

    <script type="text/javascript">

        //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
        function onEnd() {

            var x = document.getElementById('cpnlRS_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlReport_collapsible').cells[0].colSpan = "1";

        }	
    </script>

</head>
<body>
    <form id="Form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="radscript" runat="server">
        </telerik:RadScriptManager>
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td valign="top">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr style="width: 100%">
                                        <td background="../Images/top_nav_back.gif" height="47">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td style="width: 20%">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">SLA 6 Report </asp:Label>
                                                    </td>
                                                    <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                                        <center>
                                                            <asp:ImageButton ID="imgOK" runat="server" ImageUrl="../Images/s1search02.gif" ToolTip="Search">
                                                            </asp:ImageButton>
                                                            <img class="PlusImageCSS" id="Ok" title="Find" onclick="SaveEdit('Ok');" alt="Find"
                                                                src="../Images/s1search02.gif" width="0" border="0" name="tbrbtnOk" runat="server" />
                                                            <asp:ImageButton ID="imgExportToExcel" AccessKey="E" runat="server" ImageUrl="../Images/Excel.jpg"
                                                                ToolTip="Export To Excel"></asp:ImageButton>
                                                            <asp:ImageButton ID="imgExportToPDF" AccessKey="E" runat="server" ImageUrl="../Images/pdf888.gif"
                                                                ToolTip="Export To PDF"></asp:ImageButton>
                                                            <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                            <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                        </center>
                                                    </td>
                                                    <td style="width: 5%">
                                                        <img width="24" height="24" id="imgAjax" title="ajax" src="../images/divider.gif">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                            <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="Logout"
                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <!--  **********************************************************************-->
                                <div style="overflow: auto; width: 100%; height: 100%">
                                    <table align="left" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlRS" runat="server" Height="0px" Width="100%" BorderStyle="Solid"
                                                    BorderWidth="0px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                    TitleBackColor="Transparent" Text="SLA 6 Report" ExpandImage="../Images/ToggleDown.gif"
                                                    CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                    <table id="Table3" bordercolor="#d7d7d7" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblFromDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                    Width="72px" runat="server"> From Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <ION:Customcalendar ID="dtFromDate" runat="server" Height="16px" Width="148px" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblToDate" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                    Width="77px" runat="server">To Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <ION:Customcalendar ID="dtToDate" runat="server" Height="16px" Width="148px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc1:CollapsiblePanel>
                                                <cc1:CollapsiblePanel ID="cpnlReport" runat="server" Height="49px" Width="100%" BorderStyle="Solid"
                                                    BorderWidth="0px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                    TitleBackColor="Transparent" Text="SLA 6 Report" ExpandImage="../Images/ToggleDown.gif"
                                                    CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                    <%--  <div style="overflow: auto; width: 99.7%; height: 464px">--%>
                                                    <table align="left">
                                                        <tr>
                                                            <td valign="top" align="left" colspan="1" rowspan="1">
                                                                <telerik:RadGrid ID="grdCallSearch" Visible="false" Width="1160" Height="400" runat="server"
                                                                    AllowFilteringByColumn="true" AutoGenerateColumns="False" PageSize="20" AllowSorting="true"
                                                                    AllowPaging="true" Skin="Office2007">
                                                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                                        <Pdf FontType="Subset" PaperSize="Letter" />
                                                                        <Excel Format="ExcelML" />
                                                                        <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                                    </ExportSettings>
                                                                    <MasterTableView>
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn DataField="CallNo" DataType="System.String" HeaderText="CallNo"
                                                                                ReadOnly="True" SortExpression="CallNo" UniqueName="CallNo" AllowFiltering="true"
                                                                                CurrentFilterFunction="EqualTo">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="[Call Subject]" DataType="System.String" HeaderText="Call Subject"
                                                                                ReadOnly="True" SortExpression="[Call Subject]" UniqueName="Call Subject" AllowFiltering="true"
                                                                                CurrentFilterFunction="EqualTo" ItemStyle-Width="150px">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Priority" DataType="System.String" HeaderText="Priority"
                                                                                ReadOnly="True" SortExpression="Priority" UniqueName="Priority" AllowFiltering="true"
                                                                                CurrentFilterFunction="EqualTo">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Received Date" DataType="System.DateTime" HeaderText="Received Date"
                                                                                ReadOnly="True" SortExpression="Received Date" UniqueName="Received Date" AllowFiltering="true"
                                                                                CurrentFilterFunction="EqualTo">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Request Date" DataType="System.DateTime" HeaderText="Request Date"
                                                                                ReadOnly="True" SortExpression="Request Date" UniqueName="Request Date" AllowFiltering="true"
                                                                                CurrentFilterFunction="EqualTo">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Difference In minutes" DataType="System.Int"
                                                                                HeaderText="Difference In minutes" ReadOnly="True" SortExpression="Difference In minutes"
                                                                                UniqueName="Difference In minutes" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                              <telerik:GridBoundColumn DataField="Business Hours" DataType="System.String"
                                                                                HeaderText="Business Hours" ReadOnly="True" SortExpression="Business Hours"
                                                                                UniqueName="Business Hours" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                                            </telerik:GridBoundColumn>
                                                                                                                                                    </Columns>
                                                                    </MasterTableView>
                                                                    <SelectedItemStyle BackColor="#FFCC66"></SelectedItemStyle>
                                                                    <ItemStyle Font-Size="8pt"></ItemStyle>
                                                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="True" />
                                                                    <AlternatingItemStyle Font-Size="8pt" />
                                                                    <ClientSettings>
                                                                        <Selecting AllowRowSelect="True" />
                                                                        <Scrolling AllowScroll="true" ScrollHeight="100%" UseStaticHeaders="true" />
                                                                    </ClientSettings>
                                                                    <HeaderStyle Font-Size="8pt" Font-Bold="true" Height="6px" Wrap="false" ForeColor="Black"
                                                                        BorderColor="White" BackColor="#E0E0E0"></HeaderStyle>
                                                                    <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                </telerik:RadGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%-- </div>--%>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="PanelUpdate" runat="server">
                        <ContentTemplate>
                            <asp:ListBox ID="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" Font-Size="XX-Small"
                                Font-Names="Verdana" ForeColor="Red" Width="100px" Height="32px" Visible="false">
                            </asp:ListBox>
                            <asp:Panel ID="pnlMsg" runat="server">
                            </asp:Panel>
                            <input type="hidden" name="txthiddenImage" />
                            <input type="hidden" name="txthiddenProject" />
                            <input type="hidden" name="txthiddenAssignBy" />
                            <input type="hidden" name="txthiddenOwner" />
                            <input type="hidden" name="txthiddenEmployee" />
                            <input type="hidden" name="HIDSCRIDName" runat="server" id="HIDSCRID" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
