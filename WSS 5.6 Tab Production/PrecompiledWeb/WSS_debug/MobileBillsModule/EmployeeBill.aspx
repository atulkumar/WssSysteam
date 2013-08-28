<%@ page language="VB" autoeventwireup="false" inherits="MobileBillsModule_EmployeeBill, App_Web_m8wpfoiq" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 46%;
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
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <table class="style1" style="padding: inherit; margin: inherit; font-family: Verdana;
                font-size: 11px; font-weight: bold; line-height: normal; border-style: solid;
                border-color: #000000; border-collapse: separate;" align="center" width="100%">
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
                    Approved Official Calls
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
                    If Approved, total amt. to be paid
                </td>
                <td class="style3">
                    Rs.
                    <asp:Label ID="lblTotalBillAmtToBePaid" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                </asp:Panel>
                <asp:ListBox ID="ListBox1" runat="server" BorderWidth="0" BorderStyle="Groove" Font-Size="X-Small"
                    Font-Names="Verdana" ForeColor="Red" Width="100px" Height="32px" Visible="false">
                </asp:ListBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
 
</html>
