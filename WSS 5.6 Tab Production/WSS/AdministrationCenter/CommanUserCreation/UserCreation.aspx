<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserCreation.aspx.vb" Inherits="AdministrationCenter_CommanUserCreation_UserCreation" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

   <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>
 <link href="../../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">
    
</head>
<body>
    <form id="Form1" runat="server">
     <script type="text/javascript">
         //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
        function onEnd() {

            var x = document.getElementById('cpnlRS_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlReport_collapsible').cells[0].colSpan = "1";

        }	
    </script>
    
  
 
       <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <div>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%"  border="0">
                                            <tr>
                                                <td style="width: 20%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblHead" runat="server" CssClass="TitleLabel"
                                                        BorderStyle="None">Comman User Creation</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../../Images/s2Add01.gif"
                                                                    ToolTip="Add"></asp:ImageButton>
                                                                    <asp:ImageButton ID="imgExportToExcel" AccessKey="E" runat="server" ImageUrl="../../Images/Excel.jpg" Enabled="false"
                                                            ToolTip="Download New Excel File"></asp:ImageButton>
                                                       <%-- <img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />--%>
                                                      <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" /> 
                            &nbsp;
                                                    </center>
                                                </td>
                                                <td style="width: 5%">
                                                    <img id="imgAjax" title="ajax" height="24" src="../../images/divider.gif" width="24" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif" height="47">
                                     
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
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height:100%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <cc1:CollapsiblePanel ID="cpnlRS" runat="server" BorderWidth="0px" BorderStyle="Solid"
                                                Width="100%" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                ExpandImage="../../Images/ToggleDown.gif" Text="User Creation" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                Height="0px">
                                                <table id="Table2" cellspacing="4" cellpadding="1" border="0">
                                                <tr>
                                                
                                                <td>
                                                 <asp:FileUpload ID="fuCreateUser" runat="server" />
                                                </td>
                                                </tr>
                                           
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <cc1:CollapsiblePanel ID="cpnlReport" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                                                TitleBackColor="Transparent" Text="User Creation" ExpandImage="../../Images/ToggleDown.gif"
                                                CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                <%-- <div style="overflow: auto; width:1000px; height: 380px">--%>
                                                <table id="Table1" align="left" style="height: 100%">
                                                    <tr>
                                                        <td valign="top" align="left" colspan="1" rowspan="1">
                                                        <div>
      <h3>WSS User Creation Detail</h3>
      <b>User Created:</b>
     <br />
    
        <asp:Label ID="lblUserCreatedWSS"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
        <br />
      <b>
         Users having Invalid data in Excel:
      </b>
   <br />
      
       <asp:Label ID="lblInvalidDataWSS"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
     <br />
     <b>Exceptions:</b>
      
    <br />
        <asp:Label ID="lblException"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
      </div>
                                                        <div>
      <h3>SPOC User Creation Detail</h3>
      <b>User Created:</b>
      
     <br />
        <asp:Label ID="lblUserCreatedSpoc"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
      <br />
      <b>Users having Invalid data in Excel:</b>
      
      <br />
       <asp:Label ID="lblInvalidDateSPOC"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
     <br />
     <b>
        Exceptions:
     </b>
   <br />
        <asp:Label ID="lblExceptionSOpc"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
        <br />
      </div>
                                                        <div>
      <h3>Incidence User Creation Detail</h3>
  <b>User Created:</b>
       <br />
     
        <asp:Label ID="lblUserCreatedInc"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
        <br />
<b>
  Users having Invalid data in Excel:
</b>
   <br />
 
       <asp:Label ID="lblInvalidInc"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
    <br />
    <b>
      Exceptions:
    </b>
    
     <br />
        <asp:Label ID="lblExcetioninc"  runat="server" width="80%" Text="" Visible="false"></asp:Label>
        <br />
      </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%-- </div>--%>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
    </div>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="352px"
                            Visible="false" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                     <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txthiddenProject" />
                        <input type="hidden" name="txthiddenAssignBy" />
                        <input type="hidden" name="txthiddenOwner" />
                        <input type="hidden" name="txthiddenEmployee" />
                        <input type="hidden" name="HIDSCRIDName" runat="server" id="HIDSCRID" />
                    </ContentTemplate>
                </asp:UpdatePanel>
    </form>
</body>
</html>
