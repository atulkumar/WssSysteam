<%@ page language="VB" autoeventwireup="false" inherits="Reports_EVADetail, App_Web_fzfabjfx" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>EVA Report</title>
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
      <asp:ScriptManager ID="ScriptManager" runat="server">
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
                                                    <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">Earned Value Analysis </asp:Label>
                                                </td>
                                                <td style="width: 60%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgOK" runat="server" ToolTip="Search" ImageUrl="../Images/s1search02.gif">
                                                        </asp:ImageButton>
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
                                                    ExpandImage="../Images/ToggleDown.gif" Text=" EVA " TitleBackColor="Transparent"
                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                    Height="0px">
                                                    <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                   
                                                        <table id="Table2" cellspacing="4" cellpadding="1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblcompany" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                        Width="80px" runat="server"> Company</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlCompany" AutoPostBack="true" runat="server"  Font-Size="XX-Small" Font-Names="Verdana">
                                                                    </asp:DropDownList> 
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSubcat" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                        runat="server">SubCategory</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlProject" AutoPostBack="true" Font-Size="XX-Small" Width="140px" runat="server"
                                                                        Height="18px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCallNo" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                        runat="server">Call Number</asp:Label>
                                                                    <asp:DropDownList ID="ddlCallTo" AutoPostBack="true"  Font-Size="XX-Small" Width="60px" runat="server"
                                                                        Height="18px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lablTaskNo" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                        runat="server">Task Number</asp:Label>
                                                                    <asp:DropDownList ID="ddlTaskNo" Font-Size="XX-Small" Width="60px" runat="server"
                                                                        Height="18px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                         </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                    CollapseImage="../images/ToggleUp.gif" Draggable="False" ExpandImage="../images/ToggleDown.gif"
                                                    PanelCSS="panel" Text="EVA" TitleBackColor="Transparent" TitleClickable="True"
                                                    TitleCSS="test" TitleForeColor="black" Visible="true" Width="100%">
                                                    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="99%">
                                                        <tr>
                                                            <td valign="top" align="left" colspan="1" rowspan="1">
                                                                <CR:CrystalReportViewer ID="crvReport" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="False"
                                                                    EnableParameterPrompt="False" DisplayGroupTree="False" HasCrystalLogo="False"
                                                                    HasRefreshButton="True" HasToggleGroupTreeButton="False" HasZoomFactorList="False"
                                                                    PrintMode="ActiveX" ReuseParameterValuesOnRefresh="True" Height="480px" EnableTheming="true"
                                                                    BestFitPage="False" Width="1000px" HasPageNavigationButtons="False" SeparatePages="False" />
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
