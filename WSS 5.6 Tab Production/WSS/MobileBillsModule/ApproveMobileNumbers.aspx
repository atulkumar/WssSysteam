<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ApproveMobileNumbers.aspx.vb"
    Inherits="MobileBillsModule_ApproveMobileNumbers" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Images/Js/JSValidation.js"></script>

    <script type="text/javascript">
        //<![CDATA[

        //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlRS_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlReport_collapsible').cells[0].colSpan = "1";
        }
        function LogoutWSS() {
            document.Form1.txthiddenImage.value = 'Logout';
            Form1.submit();
        }

        

    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgBillDetails" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="padding-left: 20px;">
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">Approve Bills</asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgOK" runat="server" ToolTip="Search" ImageUrl="../Images/s1search02.gif">
                                                        </asp:ImageButton>&nbsp;
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                        <asp:ImageButton ID="imgExportToPDF" AccessKey="E" runat="server" ImageUrl="../Images/pdf888.gif"
                                                            ToolTip="Export To PDF"></asp:ImageButton>
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" visible="false" onclick="ShowHelp(2270 ,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table width="100%" border="0">
                                <tr>
                                    <td>
                                        <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                            Width="100%" Height="24px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                            TitleClickable="True" TitleBackColor="Transparent" Text="Bill Period" ExpandImage="../Images/ToggleDown.gif"
                                            CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                            <table id="Table1" bordercolor="#d7d7d7" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBillMonth" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True"
                                                            Width="100px" runat="server">Select Bill Month</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlMonth" Font-Size="X-Small" Width="156px" Height="18px" AutoPostBack="true"
                                                            runat="server">
                                                            <asp:ListItem Selected="True" Text="All Months" Value=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnApprove" runat="server" Text="Approve" Width="100px" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisApprove" runat="server" Text="Disapprove" Width="100px" />
                                                    </td>
                                                    <td style="width:400px">
                                                        
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnShowDisApproved" runat="server" Text="View Disapproved numbers" Width="200px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </cc1:CollapsiblePanel>
                                    </td>
                                </tr>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                Width="100%" Height="65px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                TitleClickable="True" TitleBackColor="Transparent" Text="Approve Numbers for Employee(s)"
                                ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
                                Draggable="False" BorderColor="Indigo">
                                <table bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%">
                                    <tr>
                                        <td>
                                            <td style="text-align: center;">
                                                <telerik:RadGrid ID="rgBillDetails" Width="900px" runat="server" Skin="Office2007"
                                                    AutoGenerateColumns="false" AllowFiltering="true" AllowMultiRowSelection="true">
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="true" />
                                                    </ClientSettings>
                                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                        <Pdf FontType="Subset" PaperSize="Letter" />
                                                        <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                    </ExportSettings>
                                                    <MasterTableView AllowAutomaticInserts="True">
                                                        <Columns>
                                                            <telerik:GridClientSelectColumn UniqueName="Approve" HeaderText="Approve" HeaderStyle-HorizontalAlign="Center">
                                                            </telerik:GridClientSelectColumn>
                                                            <telerik:GridBoundColumn DataField="U_UserID" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderText="User ID" UniqueName="U_UserID" Display="false" ShowFilterIcon="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="UserID" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderText="User ID" UniqueName="UserID" ShowFilterIcon="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="UserName" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderText="User Name" UniqueName="UserName" ShowFilterIcon="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="NumberCalled" HeaderText="Number Called" HeaderStyle-HorizontalAlign="Center"
                                                                UniqueName="NumberCalled" ShowFilterIcon="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="CalledPersonName" HeaderText="Called Person Name"
                                                                HeaderStyle-HorizontalAlign="Center" UniqueName="CalledPersonName" ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Company" HeaderText="Company" UniqueName="Company"
                                                                HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="CallType" HeaderText="Official Type" UniqueName="CallType"
                                                                HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                        </td>
                                    </tr>
                                </table>
                            </cc1:CollapsiblePanel>
                        </td>
                    </tr>
            </td>
        </tr>
    </table>
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
