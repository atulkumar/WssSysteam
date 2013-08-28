<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TaskH.aspx.vb" Inherits="Reports_TaskH" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Task Hierarchy</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../images/js/core.js" type="text/javascript"></script>

    <script src="../images/js/events.js" type="text/javascript"></script>

    <script src="../images/js/css.js" type="text/javascript"></script>

    <script src="../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <script type="text/javascript">

        function tabClose() {
            window.parent.closeTab();
        }
          //A Function to improve design i.e delete the extra cell of table

 function onEnd() {
           var x = document.getElementById('cpnlProject_collapsible').cells[0].colSpan = "1";
              var y = document.getElementById('cpnlReport_collapsible').cells[0].colSpan = "1";
             
        }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" method="post" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <table  border="0" >
       <%-- <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%">
                                <table id="Table1" style="border-color: activeborder" cellspacing="0" cellpadding="0"
                                    width="99%">--%>
                                    <tr>
                                        <td valign="top" align="left" >
                                            <CR:CrystalReportViewer ID="crvReport" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="False"
                                                EnableParameterPrompt="False" DisplayGroupTree="False" HasCrystalLogo="False"
                                                HasRefreshButton="False" HasToggleGroupTreeButton="False" HasZoomFactorList="False"
                                                PrintMode="ActiveX" ReuseParameterValuesOnRefresh="False" Height="400px" EnableTheming="False"
                                                BestFitPage="False" Width="1200px" HasPageNavigationButtons="False" SeparatePages="False" />
                                        </td>
                                    </tr>
                             <%--   </table>
                            </div>
                        </td>
                    </tr>--%>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="100px" ForeColor="Red" Font-Names="Verdana"
                            Font-Size="XX-Small" BorderStyle="Groove" BorderWidth="0" Height="32px" Visible="false">
                        </asp:ListBox>
                        <input type="hidden" name="txthiddenImage" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
