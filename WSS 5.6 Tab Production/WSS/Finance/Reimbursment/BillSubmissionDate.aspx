<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BillSubmissionDate.aspx.vb"
    Inherits="Reimbursement_BillSubmissionDate" Title="Bill Date" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>BillSubmissionDate</title>

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
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 30%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel">Set Bill Submission Date</asp:Label>
                                                </td>
                                                <td style="width: 60%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                        </asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
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
                            <div style="overflow: auto; width: 100">
                                <table style="width: 408px; height: 60px; border-color: #5c5a5b; background-color: #f5f5f5"
                                    border="1">
                                    <tr>
                                        <td bordercolor="#f5f5f5" width="4">
                                            &nbsp;
                                        </td>
                                        <td bordercolor="#f5f5f5" style="width: 120px">
                                            <asp:Label ID="lblStartdate" runat="server" Text="Start Date" CssClass="FieldLabel"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddlStartdate" runat="server" Width="120px" CssClass="txtNoFocus">
                                            </asp:DropDownList>
                                        </td>
                                        <td bordercolor="#f5f5f5" style="width: 131px">
                                            <asp:Label ID="lblEndDate" runat="server" Text="End Date" CssClass="FieldLabel"></asp:Label>
                                            <asp:DropDownList ID="ddlEndDate" runat="server" Width="120px" CssClass="txtNoFocus">
                                            </asp:DropDownList>
                                        </td>
                                        <td bordercolor="#f5f5f5">
                                            <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="FieldLabel"></asp:Label>
                                            <asp:TextBox ID="txtFinancialYear" runat="server" CssClass="txtFocus" Width="120px"
                                                ReadOnly="True" Height="14px"></asp:TextBox>
                                        </td>
                                        <tr>
                                        </tr>
                                    </tr>
                                    <tr>
                                        <td bordercolor="#f5f5f5" width="4">
                                            &nbsp;
                                        </td>
                                        <td colspan="3" style="width: 400px">
                                            <telerik:RadGrid ID="rgvBillSubmittedDate" Skin="Office2007" runat="server" AutoGenerateColumns="False"
                                                GridLines="None">
                                                <MasterTableView>
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="Startdate" HeaderText="Startdate" UniqueName="Startdate">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="EndDate" HeaderText="EndDate" UniqueName="EndDate">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="FinacialYear" HeaderText="FinacialYear" UniqueName="FinacialYear">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <AlternatingItemStyle HorizontalAlign="Left" />
                                            </telerik:RadGrid>
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
                        <asp:ListBox ID="lstError" runat="server" Width="100px" Font-Size="XX-Small" Font-Names="Verdana"
                            BorderWidth="0" BorderStyle="Groove" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
