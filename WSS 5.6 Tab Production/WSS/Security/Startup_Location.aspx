<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Startup_Location.aspx.vb"
    Inherits="Security_Startup_Location" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>StartUpLocation</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script language="javascript" src="../Images/Js/JSValidation.js" type="text/javascript">
    </script>

    <script language="javascript" type="text/javascript">

        function SelectChange(chka, chkB) {
            var ID = chkB;
            var count = document.getElementById("Collapsiblepanel1_dgControls").rows.length;
            var i;
            for (i = 2; i <= count; i++) {
                if (i <= 9) {
                    var id = "Collapsiblepanel1_dgControls_ctl0" + i + "_rdDisable";
                }
                else {
                    var id = "Collapsiblepanel1_dgControls_ctl" + i + "_rdDisable";
                }

                var elem = document.getElementById(id);
                elem.checked = false;
            }
            document.getElementById(ID).checked = true;
        }


        function SaveEdit(varImgValue) {

            if (varImgValue == 'Save') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }
            if (varImgValue == 'Ok') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }

            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }

           
        }		
				
	  //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('Collapsiblepanel1_collapsible').cells[0].colSpan = "1";
        }
        
         //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	

      				
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0px">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    &nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" CommandName="submit"
                                                        AlternateText="." ImageUrl="../Images/white.GIF" Width="0px" Height="0px" BorderWidth="0px">
                                                    </asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelRolepermission" runat="server" CssClass="TitleLabel">StartUp Location</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>
                                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('1008','../');"
                                            alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
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
                                <cc1:CollapsiblePanel ID="Collapsiblepanel1" runat="server" Width="100%" BorderWidth="0px"
                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="true"
                                    TitleBackColor="transparent" Text="Controls" ExpandImage="../images/ToggleDown.gif"
                                    CollapseImage="../images/ToggleUp.gif" Draggable="False" Visible="true" BorderColor="Indigo">
                                    <div style="overflow: auto; width: 100%; height: 400px">
                                        <table style="border-color: #5c5a5b; background-color: #f5f5f5" width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <asp:DataGrid ID="dgControls" runat="server" Width="100%" DataKeyField="ObjectID"
                                                        AutoGenerateColumns="False">
                                                        <SelectedItemStyle></SelectedItemStyle>
                                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="GridItem"></ItemStyle>
                                                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:ButtonColumn Visible="False" CommandName="select"></asp:ButtonColumn>
                                                            <asp:BoundColumn DataField="ObjectID" Visible="False"></asp:BoundColumn>
                                                            <asp:TemplateColumn>
                                                                <HeaderTemplate>
                                                                    <b>Object Type</b>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgObjType" runat="server" ImageUrl='<%# container.dataitem("ImageURL") %>'>
                                                                    </asp:Image>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <HeaderTemplate>
                                                                    <b>Object Name</b>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType" runat="server" Width="150px" ForeColor="Black" Height="18px"
                                                                        Font-Names="Verdana" Font-Size="XX-Small" Text='<%# container.dataitem("Name")%>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <HeaderTemplate>
                                                                    <b>HomePage</b>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:RadioButton ID="rdDisable" Visible="true" Height="18px" Width="50px" runat="server"
                                                                        onClick="javascript:SelectChange(this.checked, this.id);" Checked='<%#container.dataitem("Booked")%>'>
                                                                    </asp:RadioButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </cc1:CollapsiblePanel>
                                <!-- ***********************************************************************-->
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="100px" BorderStyle="Groove" BorderWidth="0"
                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthiddenImage" />
                        <input id="txtHIDRoleID" type="hidden" name="txthiddenRoleID" runat="server" />
                        <input type="hidden" name="txtroleData" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
