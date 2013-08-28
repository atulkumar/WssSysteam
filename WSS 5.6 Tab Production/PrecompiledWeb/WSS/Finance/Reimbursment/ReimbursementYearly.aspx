<%@ page language="VB" autoeventwireup="false" inherits="Reimbursement_ReimbursementYearly, App_Web_dipg-2cu" title="Yearly Reimburstment Amount" enableeventvalidation="false" maintainscrollpositiononpostback="true" theme="App_Themes" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ReimbursementYearly</title>

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript">

       //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
    </script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgvRBMYearlyRecord">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgvRBMYearlyRecord" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <table id="Table1" style="height: 100%" cellspacing="0" cellpadding="0" width="100%"
        border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td>
                                                    <td style="width: 25%">
                                                        <asp:Button ID="BtnGrdSearch" runat="server" BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None" Height="0" Width="0px" BorderWidth="0">
                                                        </asp:Button>
                                                        <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                            AlternateText="." CommandName="submit" ImageUrl="../../Images/white.gif"></asp:ImageButton>
                                                        <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel"> ReimbursementYearly</asp:Label>
                                                    </td>
                                                    <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                                        <center>
                                                            <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                            </asp:ImageButton>
                                                            <img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;"
                                                                onclick="javascript:location.reload(true);" />
                                                            <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                style="cursor: hand;" />
                                                        </center>
                                                    </td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('4','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="rgvRBMYearlyRecord" runat="server" Width="750px" AutoGenerateColumns="False"
                                                AllowMultiRowSelection="true" AllowFilteringByColumn="True" Skin="Office2007"
                                                AllowPaging="True" PageSize="20" ShowFooter="true" CellPadding="0" AllowSorting="True">
                                                <MasterTableView AllowAutomaticInserts="True" TableLayout="Fixed">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="CI_VC36_name" HeaderText="Employee Name" UniqueName="CI_VC36_name"
                                                            AutoPostBackOnFilter="True" CurrentFilterFunction="EqualTo">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="CI_NU8_Address_Number" HeaderText="Address Number"
                                                            UniqueName="CI_NU8_Address_Number" Visible="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Medical" UniqueName="Medical" DataField="Medical">
                                                            <ItemTemplate>
                                                                <telerik:RadNumericTextBox ID="rntxtMedical" MaxLength="5" runat="server" Value='<%# CType((iif(isdbnull(Eval("Medical")),0,Eval("Medical"))),Int32)%>'>
                                                                </telerik:RadNumericTextBox>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Telephone" UniqueName="Telephone" DataField="Telephone">
                                                            <ItemTemplate>
                                                                <telerik:RadNumericTextBox ID="rntxtTelephone" MaxLength="5" Value='<%# CType((iif(isdbnull(Eval("Telephone")),0,Eval("Telephone"))),Int32)%>'
                                                                    runat="server">
                                                                </telerik:RadNumericTextBox>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="LTA" UniqueName="LTA" DataField="LTA">
                                                            <ItemTemplate>
                                                                <telerik:RadNumericTextBox ID="rntxtLTA" MaxLength="5" Value='<%# CType((iif(isdbnull(Eval("LTA")),0,Eval("LTA"))),Int32)%>'
                                                                    runat="server">
                                                                </telerik:RadNumericTextBox>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Joining Month" UniqueName="JoinMonth" DataField="JoinMonth">
                                                            <ItemTemplate>
                                                                <telerik:RadNumericTextBox ID="rntxtJoinMonth" MaxLength="2" runat="server" Value='<%# CType((iif(isdbnull(Eval("JoinMonth")),0,Eval("JoinMonth"))),Int32)%>'>
                                                                </telerik:RadNumericTextBox>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="True" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--  **********************************************************************-->
                            <asp:UpdatePanel ID="PanelUpdate" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMsg" runat="server">
                                    </asp:Panel>
                                    <asp:ListBox ID="lstError" runat="server" Width="100px" Font-Names="Verdana" Font-Size="XX-Small"
                                        BorderStyle="Groove" BorderWidth="0" ForeColor="Red" Visible="false"></asp:ListBox>
                                    <input type="hidden" name="txthidden" />
                                    <input type="hidden" name="txthiddenImage" />
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
