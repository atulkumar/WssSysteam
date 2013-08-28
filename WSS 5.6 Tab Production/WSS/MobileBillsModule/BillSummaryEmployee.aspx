<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BillSummaryEmployee.aspx.vb"
    Inherits="MobileBillsModule_BillSummaryEmployee" %>

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

    <style type="text/css">
        fieldset 
        {
            margin:0px;
            padding:0px;
        }
        .style1
        {
            width: 100%;
            margin:10px;
        }
        .style3
        {
            width: 205px;
        }
        .style4
        {
            width: 259px;
        }
        .style5
        {
            width: 259px;
            font-size: 14px;
            color: #FFFF99;
            background-color: #003300;
        }
        .style6
        {
            width: 259px;
            font-size: 14pt;
            color: #FF0000;
            background-color: #FFFF99;
        }
        .style7
        {
            width: 259px;
            height: 29px;
        }
        .style8
        {
            width: 205px;
            height: 29px;
        }
        .style9
        {
            width: 259px;
            height: 26px;
        }
        .style10
        {
            width: 205px;
            height: 26px;
        }
        .style11
        {
            width: 259px;
            height: 27px;
        }
        .style12
        {
            width: 205px;
            height: 27px;
        }
    </style>

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

        function ReloadOnClientClose(sender, eventArgs) {
            window.location.href = window.location.href;
        }

    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">

    <script language="javascript" type="text/javascript">
        function ShowEditForm(id, rowIndex, mynum) {
            var grid = $find("<%= rgBillDetails.ClientID %>");

            var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
            grid.get_masterTableView().selectItem(rowControl, true);

            window.radopen("OfficialNumberRecord.aspx?Called_num=" + id + "&myNum=" + mynum, "UserListDialog");
            return false;
        }

        function refreshGrid(arg) {
            if (!arg) {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
            }
            else {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("RebindAndNavigate");
            }
        }
    </script>

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
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">Mobile Bill Details </asp:Label>
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
                                        <img class="PlusImageCSS" id="Help" title="Help" visible="false" onclick="ShowHelp(2268 ,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                    Width="100%" Height="24px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                    TitleClickable="True" TitleBackColor="Transparent" Text="Bill Period" ExpandImage="../Images/ToggleDown.gif"
                    CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                    <table id="Table1" bordercolor="#d7d7d7" cellspacing="1" cellpadding="1" border="0">
                        <tr>
                            <td>
                                <table id="Table2" bordercolor="#d7d7d7" width="100%" cellspacing="1" cellpadding="1"
                                    border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBillMonth" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True"
                                                Width="100px" runat="server">Select Bill Month</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMonth" Font-Size="X-Small" Width="156px" Height="18px" AutoPostBack="true"
                                                runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        <asp:Label Width="100px" ID="lblMobileNo" runat="server" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True">Mobile No</asp:Label>
                                        </td>
                                        <td>
                                         <asp:DropDownList ID="ddlMobileNo" Font-Size="X-Small" Width="156px" Height="18px" AutoPostBack="true"
                                                runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 600px">
                                        </td>
                                        <td align="right" style="text-align: right">
                                            <asp:Button ID="btnShowAllDetails" runat="server" Text="View complete call details for the selected month" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table id="Table6" bordercolor="#d7d7d7" width="100%" cellspacing="1" cellpadding="1">
                        <tr>
                            <td style="vertical-align:top; width: 50%;">
                                <fieldset style="text-align: left; font-family: Verdana; font-size: 11px; width: 100%">
                                    <legend><b><span style="color: Red;">To be Billed Amount</span></b></legend>
                                    <table width="99%" style="padding: inherit; margin: inherit; font-family: Verdana;
                                        font-size: 11px; font-weight: bold; line-height: normal; border-style: solid;
                                        border-color: #000000; border-collapse: separate;" align="left" width="100%">
                                        <tr>
                                            <td class="style4">
                                                Employee Number
                                            </td>
                                            <td class="style3">
                                                <asp:Label ID="lblEmpNumber" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style11">
                                                On Bill Charges
                                            </td>
                                            <td class="style12">
                                                Rs.
                                                <asp:Label ID="lblOnBillCharges" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style9">
                                                Approved Official Calls (@ 10.3% deduction)
                                            </td>
                                            <td class="style10">
                                                Rs.
                                                <asp:Label ID="lblApprovedOfficialCalls" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style9">
                                                Balance Personal
                                            </td>
                                            <td class="style10">
                                                Rs.
                                                <asp:Label ID="lblBalancePersonal" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style7">
                                                Permissible Limit
                                            </td>
                                            <td class="style8">
                                                Rs.
                                                <asp:Label ID="lblPermissibleLimit" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                To Be PAID
                                            </td>
                                            <td class="style3" style="background-color: #C0C0C0">
                                                Rs.
                                                <asp:Label ID="lblToBePaid" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style11">
                                                Waiting Approval of Calls
                                            </td>
                                            <td class="style12">
                                                Rs.
                                                <asp:Label ID="lblWaitingApprovalCallsAmt" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style5">
                                                If not Approved, total amt. to be paid
                                            </td>
                                            <td class="style3" style="background-color: #C0C0C0">
                                                Rs.
                                                <asp:Label ID="lblTotalBillAmtToBePaid" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td width="50%" style="vertical-align:top;padding-left:15px;">
                                <table id="Table3" bordercolor="#d7d7d7" width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <fieldset style="text-align: left; font-family: Verdana; font-size: 11px; width:100%;">
                                                <legend><b><span style="color: Red;">Drilled Information on Billed Amount</span></b></legend>
                                                <table width="99%" style="font-family: Verdana; padding-right:10px;
                                                    font-size: 11px; font-weight: bold; line-height: normal; border-style: solid;
                                                    border-color: #000000; border-collapse: separate;" align="left" width="100%">
                                                    <tr>
                                                        <td class="style4">
                                                            Monthly Charges
                                                        </td>
                                                        <td class="style3">
                                                            Rs.
                                                            <asp:Label ID="lblMonthlyCharges" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style11">
                                                            Call Charges
                                                        </td>
                                                        <td class="style12">
                                                            Rs.
                                                            <asp:Label ID="lblCallCharges" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style9">
                                                            Value Added Services
                                                        </td>
                                                        <td class="style10">
                                                            Rs.
                                                            <asp:Label ID="lblValueAddedCharges" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style9">
                                                            Roaming Charges
                                                        </td>
                                                        <td class="style10">
                                                            Rs.
                                                            <asp:Label ID="lblRoamingCharges" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style7">
                                                            Discounts
                                                        </td>
                                                        <td class="style8">
                                                            Rs.
                                                            <asp:Label ID="lblDiscount" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style7">
                                                            Taxes
                                                        </td>
                                                        <td class="style8">
                                                            Rs.
                                                            <asp:Label ID="lblTax" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                          
                                                    <tr>
                                                        <td class="style11">
                                                            Total Charges
                                                        </td>
                                                        <td class="style12">
                                                            Rs.
                                                            <asp:Label ID="lblTotalCharges" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
            </td>
        </tr>
    </table>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderWidth="0px" BorderStyle="Solid"
                    Width="100%" Height="65px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                    TitleClickable="True" TitleBackColor="Transparent" Text="Your Call Details" ExpandImage="../Images/ToggleDown.gif"
                    CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                    <table bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%">
                        <tr style="height: 40px">
                            <td style="height: 30px; text-align: left" align="left" visible="false">
                                <asp:Label ID="lblLabelDetails" runat="server" Text="Mark Number Official" Font-Bold="true"
                                    Font-Names="Verdana" Font-Size="Large" ForeColor="Black"></asp:Label>
                                <asp:DropDownList ID="ddlCustomerGroup" runat="server" Visible="false">
                                    <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="CUG" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Official" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Personal" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlApprovalList" runat="server" Visible="false">
                                    <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Rejected" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" Width="100px" />
                            </td>
                            <td style="width: 650px;">
                            </td>
                            <%--<td>
                                            <asp:Button ID="EditLink1" runat="server" Text="View your bill summary"></asp:Button>
                                        </td>--%>
                        </tr>
                    </table>
                    <table bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td>
                                <td style="text-align: center;">
                                    <telerik:RadGrid ID="rgBillDetails" Width="800px" runat="server" Skin="Office2007"
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
                                                <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderStyle-Wrap="false"
                                                    AllowFiltering="false" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="EditLink" runat="server" Text="Edit"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="called_num" HeaderText="Called Number" UniqueName="CalledNumber"
                                                    HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="CalledPersonName" HeaderText="Called Person Name/Company" UniqueName="CalledPersonName"
                                                    HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ChargedAmount" AllowFiltering="false" ShowFilterIcon="false"
                                                    HeaderText="Charged Amount" UniqueName="ChargedAmount" HeaderStyle-HorizontalAlign="Center">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Official_Flag" DataType="System.String" HeaderText="Personal/Official"
                                                    HeaderStyle-HorizontalAlign="Center" UniqueName="Official_Flag" ShowFilterIcon="true">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                            <CommandItemTemplate>
                                                <a href="#" onclick="return ShowInsertForm();">Add Official Number Record</a>
                                            </CommandItemTemplate>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Vista">
                                        <Windows>
                                            <telerik:RadWindow Skin="Office2007" ID="UserListDialog" runat="server" Title="Add Official Number record"
                                                OnClientClose="ReloadOnClientClose" Height="400px" Width="800px" Left="150px"
                                                ReloadOnShow="true" Modal="true" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                    <telerik:RadWindowManager ID="RadWindowManager2" runat="server" Skin="Vista">
                                        <Windows>
                                            <telerik:RadWindow Skin="Office2007" ID="BillDetails" runat="server" Title="Employee Bill Details"
                                                Height="300px" Width="500px" Left="150px" ReloadOnShow="false" Modal="true" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <telerik:RadGrid ID="rgShowAllDetails" runat="server" Skin="Office2007" Width="700px"
                                    AutoGenerateColumns="false" AllowFiltering="true" Visible="true">
                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                        <Pdf FontType="Subset" PaperSize="Letter" />
                                        <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                    </ExportSettings>
                                    <MasterTableView>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="Called_num" ItemStyle-Width="100px" HeaderText="Called Number"
                                                ItemStyle-Height="30" UniqueName="Called_num" ShowFilterIcon="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Called_Date" ItemStyle-Width="100px" ItemStyle-Wrap="false"
                                                ItemStyle-Height="30" HeaderText="Called Date" UniqueName="Called_Date" ShowFilterIcon="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Called_Time" HeaderText="Called Time" ItemStyle-Wrap="false"
                                                HeaderStyle-Wrap="false" ItemStyle-Height="30" UniqueName="Called_Time" ShowFilterIcon="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Duration_vol" HeaderText="Call Duration" ItemStyle-Wrap="false"
                                                ItemStyle-Height="30" HeaderStyle-Wrap="false" UniqueName="Duration_vol" ShowFilterIcon="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Charges_Amt" HeaderText="Charges Amount" ItemStyle-Wrap="false"
                                                HeaderStyle-Wrap="false" ItemStyle-Height="30" UniqueName="Charges_Amt" ShowFilterIcon="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Official_Flag" HeaderText="Personal/Official"
                                                UniqueName="Official_Flag" ShowFilterIcon="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Category_Desc" HeaderText="Call Type" ItemStyle-Wrap="false"
                                                UniqueName="Category_Desc" ShowFilterIcon="true">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                        <RowIndicatorColumn>
                                            <HeaderStyle Width="20px" />
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn>
                                            <HeaderStyle Width="20px" />
                                        </ExpandCollapseColumn>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
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
