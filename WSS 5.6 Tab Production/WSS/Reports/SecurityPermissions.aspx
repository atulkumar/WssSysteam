<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SecurityPermissions.aspx.vb"
    Inherits="Reports_SecurityPermissions" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SecurityPermissions</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../images/js/core.js" type="text/javascript"></script>

    <script src="../images/js/events.js" type="text/javascript"></script>

    <script src="../images/js/css.js" type="text/javascript"></script>

    <script src="../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <link href="../../SupportCenter/calend&#9;&#9;ar/popcalendar.css" type="text/css"
        rel="stylesheet" />
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
		function ShowImg()
        {
          document.getElementById('imgAjax').src="../images/ajax1.gif";
          document.getElementById('imgAjax').style.display='inline';
        }
    </script>

    <script type="text/javascript">

        function tabClose() {
            window.parent.closeTab();
        }
          //A Function to improve design i.e delete the extra cell of table

 function onEnd() {
           var x = document.getElementById('cpnlRS_collapsible').cells[0].colSpan = "1";
              var y = document.getElementById('cpnlReport_collapsible').cells[0].colSpan = "1";
             
        }	
        
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%"  border="0">
                                            <tr>
                                                <td style="width: 30%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblhead" runat="server" CssClass="TitleLabel" BorderStyle="None">Security Permissions </asp:Label>
                                                </td>
                                                <td style="width: 55%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgShowReport" runat="server" ImageUrl="../Images/s1search02.gif"
                                                            ToolTip="Search"></asp:ImageButton>&nbsp;<img src="../Images/reset_20.gif" title="Refresh"
                                                                alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../images/divider.gif" width="24" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(951 ,'../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%; height: 100%">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                Width="100%" Height="24px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                TitleClickable="true" TitleBackColor="transparent" Text="Security Permissions"
                                                ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
                                                Draggable="False" BorderColor="Indigo">
                                                <table id="table1" bordercolor="#d7d7d7" cellspacing="1" cellpadding="1" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCompany" Font-Bold="true" Font-Names="Verdana" Font-Size="XX-Small"
                                                                runat="server"> Company</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCompany" Width="167px" Font-Size="XX-Small" runat="server"
                                                                AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 1px">
                                                            <asp:Label ID="lblRoles" Width="32px" Font-Bold="true" Font-Names="Verdana" Font-Size="XX-Small"
                                                                runat="server">Roles</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRoles" Width="167px" Font-Size="XX-Small" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblSecurityPermission" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                Font-Size="XX-Small" Height="8px" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="0">Menu</asp:ListItem>
                                                                <asp:ListItem Value="1">Screens</asp:ListItem>
                                                                <asp:ListItem Value="2">Objects</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                            <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                Width="100%" Height="65px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                TitleClickable="true" TitleBackColor="transparent" Text="Report" ExpandImage="../Images/ToggleDown.gif"
                                                CollapseImage="../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                <table align="left" border="0px">
                                                    <tr>
                                                        <td>
                                                            <CR:CrystalReportViewer ID="crvSecurityPermissions" runat="server" AutoDataBind="true"
                                                                EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" DisplayGroupTree="False"
                                                                HasCrystalLogo="False" HasRefreshButton="True" HasToggleGroupTreeButton="False"
                                                                HasZoomFactorList="False" PrintMode="ActiveX" ReuseParameterValuesOnRefresh="True"
                                                                Height="490px" EnableTheming="true" BestFitPage="False" Width="820px" HasExportButton="False"
                                                                HasGotoPageButton="False" HasSearchButton="False" HasViewList="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="PanelUpdate" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMsg" runat="server">
                                    </asp:Panel>
                                    <asp:ListBox ID="lstError" runat="server" Width="697px" ForeColor="Red" Font-Names="Verdana"
                                        Font-Size="XX-Small" BorderStyle="Groove" BorderWidth="0" Height="32px" Visible="false">
                                    </asp:ListBox>
                                    <input type="hidden" name="txthiddenImage" />
                                    <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                                    <input type="hidden" name="txthiddenProject" />
                                    <input type="hidden" name="txthiddenOwner" />
                                    <input type="hidden" name="txthiddenAssignBy" />
                                    <input type="hidden" name="txthiddenCallNos" />
                                    <input type="hidden" name="txthiddenCallNos2" />
                                    <input type="hidden" name="txthiddenEmployee" /><input type="hidden" name="HIDSCRIDName"
                                        runat="server" id="HIDSCRID" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
