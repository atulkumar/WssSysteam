<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCurrencyInfo.aspx.vb"
    Inherits="IonCurrency_AddCurrencyInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr style="width: 100%">
            <td background="../Images/top_nav_back.gif" height="47">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td style="width: 20%">
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblHead" runat="server" CssClass="TitleLabel" BorderStyle="None">Call Detail </asp:Label>
                        </td>
                        <td style="width: 65%; text-align: center;" nowrap="nowrap">
                            <center>
                                <img class="PlusImageCSS" id="Ok" title="Find" onclick="SaveEdit('Ok');" alt="Find"
                                    src="../Images/s1search02.gif" width="0" border="0" name="tbrbtnOk" runat="server" />
                                <asp:ImageButton ID="imgOK" runat="server" ImageUrl="../Images/s1search02.gif" ToolTip="Search">
                                </asp:ImageButton>
                                <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                            </center>
                        </td>
                        <td style="width: 5%">
                            <img width="24" height="24" id="imgAjax" title="ajax" src="../images/divider.gif">&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp(763 ,'../');"
                    alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="Logout"
                    src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
        <td>
            <asp:Label ID="lblCurrenyCode" Text="CurrenyCode" runat="server"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtCurrencyCode" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
         <td>
            <asp:Label ID="lblCountry" Text="Country" runat="server"></asp:Label> </td>
            <td>
                <asp:DropDownList ID="drpCountry" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
         <td>
            <asp:Label ID="lblDescription" Text="Description" runat="server"></asp:Label> </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
         <td>
            <asp:Label ID="lblMinRate" Text="MinRate" runat="server"></asp:Label> </td>
            <td>
                <asp:TextBox ID="txtMinRate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
         <td>
            <asp:Label ID="lblMaxRate" Text="MaxRate" runat="server"></asp:Label> </td>
            <td>
                <asp:TextBox ID="txtMaxRate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <td></td>
        <td><asp:Button ID="btnSave" runat="server" />  </td>
        </tr>
    </table>
    </form>
</body>
</html>
