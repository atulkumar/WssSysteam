<%@ page language="VB" autoeventwireup="false" inherits="Reports_crSPOCcalldetailReport, App_Web_fzfabjfx" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SPOC Call Detail Report</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../DateControl/ION.js"></script>

    <script type="text/javascript" src="../Images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../Images/Js/ABMainShortCuts.js"></script>

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
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
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
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">SPOC 
                                                    Call Details </asp:Label>
                                                </td>
                                                <td style="width: 55%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSearch" AccessKey="S" runat="server" ImageUrl="../Images/s1search02.gif"
                                                            ToolTip="Search" ></asp:ImageButton>
                                                             <asp:ImageButton ID="imgRefresh" AccessKey="R" runat="server"  ImageUrl="../Images/reset_20.gif" OnClientClick ="javascript:location.reload(true);"
                                                            ToolTip="Refresh" CausesValidation="False"  ></asp:ImageButton>
                                                        
                                                        <asp:ImageButton ID="imgClose" AccessKey="C" runat="server"  ImageUrl="../Images/s2close01.gif" OnClientClick ="return tabClose()"
                                                            ToolTip="Close" CausesValidation="False"  ></asp:ImageButton>
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" style="text-align:right" />
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
                                                                    ExpandImage="../Images/ToggleDown.gif" Text="SPOC Call Detail Report" TitleBackColor="Transparent"
                                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                    Height="0px">
                                                                    <table cellspacing="0px" cellpadding="0px" width="700px" border="0px" style="border-style: Solid;
                                                                        width: 100%; ">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label5" runat="server" CssClass="FieldLabel">Department</asp:Label><br>
                                                                                <asp:DropDownList ID="ddlDepartment" TabIndex="3" Width="113px" 
                                                                                    CssClass="txtNoFocus" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">SubCategories</asp:Label><br>
                                                                                <asp:DropDownList ID="ddlSubCategories" TabIndex="3" Width="113px" 
                                                                                    CssClass="txtNoFocus" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">Call Status</asp:Label><br>
                                                                                <asp:DropDownList ID="ddlCallStatus" TabIndex="3" Width="113px" 
                                                                                    CssClass="txtNoFocus" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">Start Date</asp:Label><br>
                                                                                <telerik:RadDatePicker ID="dtFrom" Width="148px" runat="server"
                                                                                    DateInput-AutoPostBack="False" AutoPostBack ="false" EnableTyping="False">
                                                                                    <DateInput Height="16px" ReadOnly="True">
                                                                                    </DateInput>
                                                                                    <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" 
                                                                                        ViewSelectorText="x">
                                                                                    </Calendar>
                                                                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                                </telerik:RadDatePicker>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">End Date</asp:Label><br>
                                                                                <telerik:RadDatePicker ID="dtTODate" Width="148px"
                                                                                    runat="server" AutoPostBack ="false" DateInput-AutoPostBack ="false" 
                                                                                    EnableTyping="False">
                                                                                    <DateInput Height="16px" ReadOnly="True">
                                                                                    </DateInput>
                                                                                    <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" 
                                                                                        ViewSelectorText="x">
                                                                                    </Calendar>
                                                                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                                </telerik:RadDatePicker>
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
                                                                    PanelCSS="panel" Text="Spoc Calls" TitleBackColor="Transparent" TitleClickable="True"
                                                                    TitleCSS="test" TitleForeColor="black" Visible="true" Width="100%">
                                                                    <table align="left" style=" width: 100%">
                                                                        <tr>
                                                                        <td> <center> <asp:Label runat="server" ID="lblMsg" ForeColor ="Red" Font-Size ="14" Font-Bold ="true"></asp:Label></center></td>
                                                                            <td valign="top" align="left" colspan="1" rowspan="1">
                                                                                <CR:CrystalReportViewer ID="crvReport" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="False"
                                                                                    EnableParameterPrompt="False" DisplayGroupTree="False" HasCrystalLogo="False"
                                                                                    HasRefreshButton="True" HasToggleGroupTreeButton="False" HasZoomFactorList="False"
                                                                                    PrintMode="ActiveX" ReuseParameterValuesOnRefresh="True" Height="600px" EnableTheming="true"
                                                                                    BestFitPage="False" Width="1019px" HasPageNavigationButtons="False" SeparatePages="False" />
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
