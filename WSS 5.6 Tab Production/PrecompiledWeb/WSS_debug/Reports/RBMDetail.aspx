<%@ page language="VB" autoeventwireup="false" inherits="Reports_RBMDetail, App_Web_sbqj3uth" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>CReports</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../Images/Js/JSValidation.js"></script>

    <script language="javascript" src="../DateControl/ION.js"></script>

    <link href="../../SupportCenter/calend&#9;&#9;ar/popcalendar.css" type="text/css"
        rel="stylesheet">
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">

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
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">Reimbursement Details </asp:Label>
                                                </td>
                                                <td style="width: 55%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgOK" runat="server" ToolTip="Search" ImageUrl="../Images/s1search02.gif">
                                                        </asp:ImageButton>
                                                        <asp:ImageButton ID="imgClose" runat="server" ToolTip="Close" ImageUrl="../Images/s2close01.gif"
                                                            AlternateText="Close"></asp:ImageButton>
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(2192,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 100%; height: 581px">
                                <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                    align="left" border="0">
                                    <tr>
                                        <td valign="top" colspan="1">
                                            <!--  **********************************************************************-->
                                            <div style="overflow: auto; width: 100%; height: 581px">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                                    Width="100%" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                                    ExpandImage="../Images/ToggleDown.gif" Text="Reimbursement Details" TitleBackColor="Transparent"
                                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                    Height="0px">
                                                                    <table id="Table2" cellspacing="4" cellpadding="1" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblEmpID" runat="server" Text="Employee Name" CssClass="FieldLabel"></asp:Label><br />
                                                                                <asp:DropDownList ID="ddlEmp" runat="server" CssClass="DDLField" Width="120px" AutoPostBack="True"
                                                                                    Height="18px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblRMB" runat="server" Text="Reimbursement Type" CssClass="FieldLabel"></asp:Label><br />
                                                                                <asp:DropDownList ID="ddlReimbursement" runat="server" CssClass="DDLField" Width="120px"
                                                                                    Height="18px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="FieldLabel"></asp:Label><br />
                                                                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="txtNoFocus" Width="120px"
                                                                                    Height="18px">
                                                                                    <asp:ListItem Value="0">All</asp:ListItem>
                                                                                    <asp:ListItem Value="1">January</asp:ListItem>
                                                                                    <asp:ListItem Value="2">February</asp:ListItem>
                                                                                    <asp:ListItem Value="3">March</asp:ListItem>
                                                                                    <asp:ListItem Value="4">April</asp:ListItem>
                                                                                    <asp:ListItem Value="5">May</asp:ListItem>
                                                                                    <asp:ListItem Value="6">June</asp:ListItem>
                                                                                    <asp:ListItem Value="7">July</asp:ListItem>
                                                                                    <asp:ListItem Value="8">August</asp:ListItem>
                                                                                    <asp:ListItem Value="9">September</asp:ListItem>
                                                                                    <asp:ListItem Value="10">October</asp:ListItem>
                                                                                    <asp:ListItem Value="11">November</asp:ListItem>
                                                                                    <asp:ListItem Value="12">December</asp:ListItem>
                                                                                </asp:DropDownList>
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
                                                                    PanelCSS="panel" Text="Reimbursement Details" TitleBackColor="Transparent" TitleClickable="True"
                                                                    TitleCSS="test" TitleForeColor="black" Visible="true" Width="100%">
                                                                    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="99%">
                                                                        <tr>
                                                                            <td valign="top" align="left" colspan="1" rowspan="1">
                                                                                <CR:CrystalReportViewer ID="crvReport" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="False"
                                                                                    EnableParameterPrompt="False" DisplayGroupTree="False" HasCrystalLogo="False"
                                                                                    HasRefreshButton="True" HasToggleGroupTreeButton="False" HasZoomFactorList="False"
                                                                                    PrintMode="ActiveX" ReuseParameterValuesOnRefresh="True" Height="430px" EnableTheming="true"
                                                                                    BestFitPage="False" Width="800px" HasPageNavigationButtons="False" SeparatePages="False" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </cc1:CollapsiblePanel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlMsg" runat="server">
                                </asp:Panel>
                                <asp:ListBox ID="lstError" runat="server" Width="465px" ForeColor="Red" Font-Names="Verdana"
                                    Visible="false" Font-Size="XX-Small" BorderStyle="Groove" BorderWidth="0" Height="32px">
                                </asp:ListBox>
                                 <input type="hidden" name="txthiddenImage" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
