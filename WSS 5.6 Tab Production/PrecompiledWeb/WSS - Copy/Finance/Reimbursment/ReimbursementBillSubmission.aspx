<%@ page language="VB" autoeventwireup="false" inherits="Finance_Reimbursment_ReimbursementBillSubmission, App_Web_dipg-2cu" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

    <script type="text/javascript">
        //<![CDATA[
        function openWin()
        {
            var oWnd = radopen("BillHistory.aspx", "RadWindow2");
              oWnd.setSize(600,400);
                    return false;

        }
        
       //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	


    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgEmpMonthlyDetail">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgEmpMonthlyDetail" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rgEmpYearlyDetail">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgEmpYearlyDetail" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgReset">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rntxtAmount" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="ddlRBMType" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel"> Bill Submission</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                        </asp:ImageButton>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <img class="PlusImageCSS" id="Img1" title="BillHistory" onclick="openWin();" alt="E"
                                                            src="../../Images/ScreenHunter_075.bmp" border="0" name="tbrbtnEdit" />
                                                             <asp:ImageButton ID="imgClose" runat="server" OnClientClick="tabClose();"
                                                        ImageUrl="../../Images/s2close01.gif" AlternateText="Close Window">
                                                    </asp:ImageButton>&nbsp;
                                                       <%-- <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />--%>
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('2161','../../');"
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
                            <div style="overflow: auto; height: 581px">
                                <table cellspacing="0" cellpadding="0" width="700px" border="0">
                                    <tbody>
                                        <tr>
                                            <td style="width: 80%">
                                                <fieldset style="text-align: right; font-family: Verdana; font-size: 10px">
                                                    <legend><b>Bill Submission</b></legend>
                                                    <table align="left">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblRBMType" runat="server" Text="Reimbursement Type" CssClass="FieldLabel"></asp:Label><br />
                                                                <asp:DropDownList ID="ddlRBMType" runat="server" CssClass="txtNoFocus" Width="120px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 10px">
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBillSubmittedAmt" runat="server" Text="Bill Submitted Amount" CssClass="FieldLabel"></asp:Label><br />
                                                                <telerik:RadNumericTextBox ID="rntxtAmount" runat="server" CssClass="txtNoFocus"
                                                                    MaxLength="5" Width="125px" Height="12px">
                                                                </telerik:RadNumericTextBox>
                                                            </td>
                                                            <td style="width: 10px">
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="Label1" runat="server" Text="Attach Bill" CssClass="FieldLabel"></asp:Label><br />
                                                                <input id="File1" contenteditable="false" type="file" name="File1" runat="server"
                                                                    style="height: 18px; font-family: Verdana; font-size: 10px" />
                                                            </td>
                                                            <td valign="bottom">
                                                                <asp:Image ID="Imgadv" runat="server" ImageUrl="~/Images/RedImage.JPG" />
                                                                <asp:Label ID="lblBillDue" runat="server" Text="Bill Due"></asp:Label>
                                                            </td>
                                                            <td valign="bottom">
                                                                <asp:Image ID="ImgDue" runat="server" ImageUrl="~/Images/BlueImage.JPG" />
                                                                <asp:Label ID="lblBillAdv" runat="server" Text="Amount Over Paid"></asp:Label>
                                                            </td>
                                                            <td valign="bottom">
                                                                <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Sunset" />
                                                                <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
                                                                    ReloadOnShow="true" runat="server" Skin="Office2007">
                                                                    <Windows>
                                                                        <telerik:RadWindow ID="RadWindow2" Behavior="Close" runat="server" Animation="Resize"
                                                                            Width="700px" Height="445" Modal="true" Title="BillHistory" NavigateUrl="BillHistory.aspx">
                                                                        </telerik:RadWindow>
                                                                    </Windows>
                                                                </telerik:RadWindowManager>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table id="table1261" style="border-color: activeborder" cellspacing="0" cellpadding="0"
                                                    align="left" border="0">
                                                    <tr>
                                                        <td valign="top" align="left" style="float: left; width: 710px">
                                                            <!--  **********************************************************************-->
                                                            <fieldset style="text-align: right; font-family: Verdana; font-size: 10px">
                                                                <legend><b>Verified Yearly Bill Details</b></legend>
                                                                <telerik:RadGrid ID="rgEmpYearlyDetail" runat="server" Skin="Office2007" AutoGenerateColumns="false">
                                                                    <MasterTableView>
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn DataField="Reimbursement" HeaderText="Reimbursement" UniqueName="Reimbursement">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Yearly Entitilement" HeaderText="Yearly Entitilement"
                                                                                UniqueName="Yearly Entitilement">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Monthly Entitilement" HeaderText="Monthly Entitilement"
                                                                                UniqueName="Monthly Entitilement">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="BillSubmitted" HeaderText="BillSubmitted Till Date"
                                                                                UniqueName="BillSubmitted">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Reimbursement Paid to you till Date" HeaderText="Reimbursement to be Paid "
                                                                                UniqueName="Reimbursement Paid to you till Date">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Bill Pending for a full year" HeaderText="Balance"
                                                                                UniqueName="BillPending">
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
                                                            </fieldset>
                                                            <!-- ***********************************************************************-->
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="80%">
                                                <table style="border-collapse: collapse" cellspacing="0" cellpadding="0" align="left"
                                                    border="0" width="710px">
                                                    <tr>
                                                        <td>
                                                            <fieldset style="text-align: right; font-family: Verdana; font-size: 10px">
                                                                <legend><b>Verified Monthly Bill Details</b></legend>
                                                                <telerik:RadGrid ID="rgEmpMonthlyDetail" runat="server" Skin="Office2007" GridLines="None"
                                                                    ShowGroupPanel="True" AutoGenerateColumns="false">
                                                                    <MasterTableView ShowGroupFooter="true">
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn DataField="MName" HeaderText="Month" UniqueName="BillMonth">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Reimbursement" HeaderText="Reimbursement" UniqueName="Reimbursement">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="ReimbursementAllowed" Aggregate="Sum" HeaderText="Min. Bill Amount"
                                                                                UniqueName="ReimbursementAllowed">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="BillSubmitted" Aggregate="Sum" HeaderText="BillSubmitted"
                                                                                UniqueName="BillSubmitted">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="ReimbursementPaid" Aggregate="Sum" HeaderText="ReimbursementPaid"
                                                                                UniqueName="ReimbursementPaid">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Bill_InAdvance" HeaderText="Over Due Bills" UniqueName="Bill_InAdvance">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Bill_Due" HeaderText="Bill Due" UniqueName="Bill_Due">
                                                                            </telerik:GridBoundColumn>
                                                                        </Columns>
                                                                        <GroupByExpressions>
                                                                            <telerik:GridGroupByExpression>
                                                                                <GroupByFields>
                                                                                    <telerik:GridGroupByField FieldName="Reimbursement" />
                                                                                </GroupByFields>
                                                                                <SelectFields>
                                                                                    <telerik:GridGroupByField FieldName="Reimbursement" HeaderText="Reimbursement" />
                                                                                </SelectFields>
                                                                            </telerik:GridGroupByExpression>
                                                                        </GroupByExpressions>
                                                                    </MasterTableView>
                                                                    <ClientSettings AllowDragToGroup="True">
                                                                    </ClientSettings>
                                                                </telerik:RadGrid>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
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
                        <asp:Panel ID="Panel2" runat="server">
                        </asp:Panel>
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
