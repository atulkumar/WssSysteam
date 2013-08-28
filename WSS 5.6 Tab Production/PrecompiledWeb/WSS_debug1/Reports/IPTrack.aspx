<%@ page language="VB" autoeventwireup="false" inherits="Reports_IPTrack, App_Web_wzhf50l2" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CReports</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../images/js/core.js" type="text/javascript"></script>

    <script src="../images/js/events.js" type="text/javascript"></script>

    <script src="../images/js/css.js" type="text/javascript"></script>

    <script src="../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../images/js/drag.js" type="text/javascript"></script>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="../../SupportCenter/calend&#9;&#9;ar/popcalendar.css" type="text/css"
        rel="stylesheet" />
    <link href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
        function back(flag) {
            if (flag == 'inl') {
                document.Form1.txthiddenImage.value = "inl";
                document.Form1.submit();
            }
            else {
                window.history.back(-1);
            }
            return false;
        }

        function SaveEdit(varImgValue) {

            if (varImgValue == 'Close') {
                window.close();
            }



            if (varImgValue == 'Ok') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                CloseWindow()
            }

            if (varImgValue == 'Save') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }

            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }


            if (varImgValue == 'Attach') {
                if (document.Form1.txthiddenCallNo.value == 0) {
                    alert("Please select a call number");
                }
                else {
                    //location.href="Call_Detail.aspx?ID=0";
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                //document.Form1.txthiddenImage.value=varImgValue;
                //Form1.submit(); 
            }
            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset()
                }

            }
        }	


    </script>

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
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr valign="top">
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr width="100%">
                                                <td background="../Images/top_nav_back.gif" height="47">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 35%">
                                                                &nbsp;&nbsp;<asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" CommandName="submit"
                                                                    AlternateText="." ImageUrl="~/Images/white.gif" Width="1px" Height="1px"></asp:ImageButton>
                                                                <asp:Label ID="lblTitleLabel" runat="server" CssClass="TitleLabel" BorderStyle="None"
                                                                    BorderWidth="2px" Width="264px">IP Track Report</asp:Label>
                                                            </td>
                                                            <td style="width: 50%; text-align: center;" nowrap="nowrap">
                                                                <center>
                                                                    <asp:ImageButton ID="imgOK" runat="server" AccessKey="H" ImageUrl="../Images/s1search02.gif"
                                                                        ToolTip="Search" />
                                                                    <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                                    <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                                </center>
                                                            </td>
                                                            <td style="width: 5%">
                                                                <img width="24" height="24" id="imgAjax" title="ajax" src="../images/divider.gif" />&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                                    <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('258','../');"
                                                        alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                                    <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutHRMS();" alt="E"
                                                        src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <!--  **********************************************************************-->
                                        <div style="overflow: auto; width: 100%">
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <!-- **********************************************************************-->
                                                        <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                            CollapseImage="../images/ToggleUp.gif" Draggable="False" ExpandImage="../images/ToggleDown.gif"
                                                            PanelCSS="panel" Text="Resume Bank" TitleBackColor="Transparent" TitleClickable="True"
                                                            TitleCSS="test" TitleForeColor="black" Visible="true" Width="100%">
                                                            <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                width="100%" align="left" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCompany" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                            runat="server"> Company</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlCompany" Width="150px" Font-Size="XX-Small" runat="server"
                                                                            AutoPostBack="True" Height="18px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblEmployee" Width="72px" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                            runat="server">Employee</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlEmployee" Width="150px" Font-Size="XX-Small" Height="18px"
                                                                            runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblIP" Width="32px" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                            runat="server">IP</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlIP" Width="125px" Font-Size="XX-Small" Height="18px" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="width: 460px">
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 20px">
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblFromDate" Width="64px" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                            runat="server"> From Date</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <ION:Customcalendar ID="dtFromDate" runat="server" Height="16px" Width="148px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblToDate" Width="48px" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                                            runat="server">To Date</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <ION:Customcalendar ID="dtToDate" runat="server" Height="16px" Width="148px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </cc1:CollapsiblePanel>
                                                        <!-- **********************************************************************-->
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <cc1:CollapsiblePanel ID="cpnlReport" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                            CollapseImage="../images/ToggleUp.gif" Draggable="False" ExpandImage="../images/ToggleDown.gif"
                                                            PanelCSS="panel" Text="Employee Detail Report" TitleBackColor="Transparent" TitleClickable="True"
                                                            TitleCSS="test" TitleForeColor="black" Visible="true" Width="100%">
                                                            <table id="Table1" cellspacing="0" cellpadding="0" width="99%" style="height: 100%;
                                                                border-color: activeborder" align="left" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <CR:CrystalReportViewer ID="crvIPTrack" runat="server" HasToggleGroupTreeButton="False"
                                                                            AutoDataBind="True" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False"
                                                                            EnableParameterPrompt="False" EnableTheming="false" HasCrystalLogo="False" HasRefreshButton="True"
                                                                            HasSearchButton="False" HasViewList="False" HasZoomFactorList="False" ClientTarget="Uplevel">
                                                                        </CR:CrystalReportViewer>
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
                            </table>
                            <asp:UpdatePanel ID="PanelUpdate" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMsg" runat="server">
                                    </asp:Panel>
                                    <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Font-Names="Verdana"
                                        Font-Size="XX-Small" ForeColor="Red" Width="635px" Visible="false"></asp:ListBox>
                                    <input id="TxtHidd1" name="TxtHidd1" type="hidden" />
                                    <input type="hidden" name="txthiddenImage" />
                                    <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
                                    <input type="hidden" name="txthiddenProject" />
                                    <input type="hidden" name="txthiddenOwner" />
                                    <input type="hidden" name="txthiddenAssignBy" />
                                    <input type="hidden" name="txthiddenCallNos" />
                                    <input type="hidden" name="txthiddenCallNos2" />
                                    <input type="hidden" name="txthiddenEmployee" />
                                    <input type="hidden" name="HIDSCRIDName" runat="server" id="HIDSCRID" />
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
