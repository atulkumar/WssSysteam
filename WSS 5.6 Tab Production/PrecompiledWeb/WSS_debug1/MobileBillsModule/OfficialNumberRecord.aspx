<%@ page language="VB" autoeventwireup="false" inherits="MobileBillsModule_OfficialNumberRecord, App_Web_b3coazgw" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script type="text/javascript">
        function CloseAndRebind(args) {
            GetRadWindow().Close();
            GetRadWindow().BrowserWindow.refreshGrid(args);
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow; //Will work in Moz in all cases, including classic dialog
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow; //IE (and Moz as well)

            return oWindow;
        }
        function CancelEdit() {
            GetRadWindow().Close();
        }
    </script>

    <title>Update Details</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManagerPage" runat="server">
        </asp:ScriptManager>
        <div>
            <table width="100%">
                <tr>
                    <td style="font-size: x-small; font-family: Verdana; font-weight: bold;">
                        Official First Name*
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                    </td>
                    <td style="font-size: x-small; font-family: Verdana; font-weight: bold;">
                        Official Last Name*
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: x-small; font-family: Verdana; font-weight: bold;">
                        Official Company Name*
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                    </td>
                    <td style="font-size: x-small; font-family: Verdana; font-weight: bold;">
                        Official Type
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOfficialType" runat="server" Width="150px" AutoPostBack="false"
                            Font-Names="Verdana" Font-Bold="false" Font-Size="X-Small">
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txtCallType" runat="server"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td style="height: 50px">
                    </td>
                    <td style="text-align: right">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update data" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="PanelUpdate" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlMsg" runat="server">
                </asp:Panel>
                <asp:ListBox ID="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" Font-Size="X-Small"
                    Font-Names="Verdana" ForeColor="Red" Width="100px" Height="32px" Visible="false">
                </asp:ListBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
