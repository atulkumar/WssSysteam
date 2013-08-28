<%@ page language="VB" autoeventwireup="false" inherits="MobileBillsModule_mobileDashBoard, App_Web_m8wpfoiq" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc2" Namespace="ION.Web" Assembly="IONPOP" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HomePage</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../Images/Js/StyleSheet1.css" />

    <script type="text/javascript">

        function MIN() {
            //window.blur();		
            //self.parent.resizeTo(0,0);
            //window.moveTo(500,500);
        }
      

        function ShowEvent(strDate) {

            //alert(strDate);
            //alert();
            wopen('MessageCenter/HomeCalender/Events.aspx?strDate=' + strDate, 'IONPOP', 430, 400);
            return false;
        }
        function SaveEdit(varImgValue) {

            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }
            if (varImgValue == 'Help') {
                document.Form1.txthiddenImage.value = varImgValue;
                //Form1.submit(); 
                //wopen("Help/WSSHelp.aspx?ScreenID=9999","HomeHelp",500,500);
                var topPos;
                leftPos = (screen.width / 2) - 250;
                topPos = (screen.height / 2) - 250;
                window.open('Help/WSSHelp.aspx?ScreenID=9999', 'posA', 'overfilter="Alpha(opacity=75);";style=ScrollingSampStyle;toolbar=no, titlebar=no,width=500,height=555,top=' + topPos + ',left=' + leftPos);
            }


        }
        function LogoutWSS() {
            document.Form1.txthiddenImage.value = 'Logout';
            Form1.submit();
        }

        function callrefresh() {
            Form1.submit();
        }
        function wopen(url, name, w, h) {
            w += 32;
            h += 96;
            wleft = (screen.width - w) / 2;
            wtop = (screen.height - h) / 2;
            var win = window.open(url,
						name,
						'width=' + w + ', height=' + h + ', ' +
						'left=' + wleft + ', top=' + wtop + ', ' +
						'location=no, menubar=no, ' +
						'status=no, toolbar=no, scrollbars=no, resizable=yes');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }

        function OpenComment(Level, CompID, CN, TN, AN) {
            wopen('SupportCenter/Callview/comment.aspx?ScrID=464&From=Home&Level=' + Level + '&CID=' + CompID + '&CN=' + CN + '&TN=' + TN + '&AN=' + AN, 'Comment', 500, 450);
        }
		
    </script>

    <script type="text/javascript">
        //A function to open call detail in new tab after dblclick on Call Grid Row
        function openCallDetailsInTab(CallNo, CompId) {
            //Calling the Parent window function
          
            window.parent.OpenCallDetailInTab('Call# ' + CallNo, 'SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=5&CallNumber=' + CallNo + '&CompId=' + CompId, 'Call' + CallNo,'-1')

        }
        //A function to open To Do List  in new tab after dblclick on Task Grid Row
        function openToDoListInTab(CallNo, CompId) {
            //Calling the Parent window function
          
           window.parent.OpenCallDetailInTab('To Do List', 'WorkCenter/DoList/ToDoList.aspx?ScrID=8&CallNumber=' + CallNo + "&CompId=" + CompId, '8','-1')
        }
        //A function to open Comment View  in new tab after click on View Old Comments link
        function openCommentViewInTab() {
            //Calling the Parent window function
           
            window.parent.OpenCallDetailInTab('Comment View', 'SupportCenter/CallView/Comment_View.aspx', '968', '-1')
        }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../Images/top_nav_back.gif" height="40" valign="middle">
                                        <table cellspacing="0" cellpadding="0" width="94%" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px"
                                                        ImageUrl="../images/white.GIF" CommandName="submit" AlternateText="."></asp:ImageButton><%--<IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="Images/left005.gif"
																name="imgHide"> <img class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="Images/Right005.gif"
																name="ingShow">--%>
                                                    <asp:Label ID="lblTitleLabelWssHome" runat="server" CssClass="TitleLabel" BorderStyle="None">WSS HOME</asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../images/top_nav_back.gif" height="40"
                                        valign="bottom">
                                        <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />&nbsp;
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 100%;">
                                <table style="border-collapse: collapse" width="100%" border="0">
                                    <tr>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" Width="60px"  Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ID="lblMonth" Text="Month"></asp:Label>
                                            <asp:DropDownList ID="ddlMonth" Font-Size="X-Small" Width="156px" Height="18px" AutoPostBack="true"
                                                runat="server">
                                            </asp:DropDownList>
                                            <asp:Label Width="80px" ID="lblMobileNo" runat="server" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True">Mobile No</asp:Label>
                                            <asp:DropDownList ID="ddlMobileNo" Font-Size="X-Small" Width="156px" Height="18px" AutoPostBack="true"
                                                runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="48%">
                                            <!-- *****************************************-->
                                            <cc1:CollapsiblePanel ID="cpnlTop5CalledNo" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                                TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent" Text="Top 5 Called Numbers Ion Approved"
                                                ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
                                                <div style="overflow: auto; width: 100%; height: 190pt">
                                                    <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                        align="left" border="0">
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <!--  **********************************************************************-->
                                                                <telerik:RadGrid ID="rgTop5CalledNoIonApproved" Width="500px" runat="server" Skin="Office2007"
                                                                    AutoGenerateColumns="false" AllowMultiRowSelection="true" AllowFilteringByColumn="false">
                                                                    <ClientSettings>
                                                                        <Selecting AllowRowSelect="true" />
                                                                    </ClientSettings>
                                                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                                        <Pdf FontType="Subset" PaperSize="Letter" />
                                                                        <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                                    </ExportSettings>
                                                                    <MasterTableView AllowAutomaticInserts="True">
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn DataField="called_num" HeaderText="Called Number" UniqueName="CalledNumber"
                                                                                HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Charges_Amt" HeaderText="Charges" UniqueName="Charges_Amt"
                                                                                HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="Number" AllowFiltering="false" ShowFilterIcon="false"
                                                                                HeaderText="No of time Called" UniqueName="Number" HeaderStyle-HorizontalAlign="Center">
                                                                            </telerik:GridBoundColumn>
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                                
                                                                <!-- Panel for displaying Task Info -->
                                                                <!-- Panel for displaying Action Info-->
                                                                <!-- ***********************************************************************-->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </cc1:CollapsiblePanel>
                                            <!-- *****************************************-->
                                        </td>
                                        <td valign="top" width="48%">
                                            <!-- *****************************************-->
                                            <cc1:CollapsiblePanel ID="cpnlAnalysisChart" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                                TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent" Text="Bill Analysis"
                                                ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
                                                <div style="overflow: hidden ; width: 100%; height: 190pt; bordercolor: Indigo; borderstyle: Solid;">
                                                    <table id="Table11" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                        align="left" border="0">
                                                        <tr>
                                                            <td align="center">
                                                                <telerik:RadChart ID="rcAnalysis" ChartTitle-Appearance-Visible="false" runat="server" Height="250px" Width="500px"   
                                                                    AutoTextWrap="true" Skin="LightBlue">
                                                                    <Legend>
                                                                        <Appearance GroupNameFormat="#Total Amount">
                                                                        </Appearance>
                                                                    </Legend>
                                                                    <PlotArea>
                                                                        <XAxis DataLabelsColumn="Bill_date" >
                                                                        </XAxis>
                                                                          
                                                                    </PlotArea>
                                                                </telerik:RadChart>
                                                                 
                                                                   
                                                                <asp:Panel ID="Panel2" runat="server">
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <!--  **********************************************************************-->
                                                                <!-- Panel for displaying Task Info -->
                                                                <!-- Panel for displaying Action Info-->
                                                                <!-- ***********************************************************************-->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </cc1:CollapsiblePanel>
                                            <!-- *****************************************-->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="48%">
                                            <!-- *****************************************-->
                                            <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                                TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent" Text="Top 5 Called Numbers not Ion Approved"
                                                ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
                                                <div style="overflow: auto; width: 100%; height: 190pt">
                                                    <telerik:RadGrid ID="rgTop5NoNotIonapproved" Width="500px" runat="server" Skin="Office2007"
                                                        AutoGenerateColumns="false" AllowMultiRowSelection="true" AllowFilteringByColumn="false">
                                                        <ClientSettings>
                                                            <Selecting AllowRowSelect="true" />
                                                        </ClientSettings>
                                                        <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                            <Pdf FontType="Subset" PaperSize="Letter" />
                                                            <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                        </ExportSettings>
                                                        <MasterTableView AllowAutomaticInserts="True">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="called_num" HeaderText="Called Number" UniqueName="CalledNumber"
                                                                    HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Charges_Amt" HeaderText="Charges" UniqueName="Charges_Amt"
                                                                    HeaderStyle-HorizontalAlign="Center" ShowFilterIcon="true">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Number" AllowFiltering="false" ShowFilterIcon="false"
                                                                    HeaderText="No of time Called" UniqueName="Number" HeaderStyle-HorizontalAlign="Center">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </div>
                                             
                                            </cc1:CollapsiblePanel>
                                             
                                            <!-- *****************************************-->
                                        </td>
                                        <td valign="top" width="48%">
                                            <!-- ****************************************-->
                                            <cc1:CollapsiblePanel ID="cpnlAlerts" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel"
                                                TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent" Text=""
                                                ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False">
                                              
                                                <div style="overflow: auto; width: 100%; height: 185pt">
                                                    <asp:DataGrid ID="dgrCommentAlert" runat="server" CssClass="Grid" Font-Names="Verdana"
                                                        BorderWidth="0px" BorderStyle="None" BorderColor="Silver" ForeColor="MidnightBlue"
                                                        CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left" BackColor="White">
                                                        <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                        <SelectedItemStyle CssClass="GridSelectedItem" BackColor="Silver"></SelectedItemStyle>
                                                        <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                        <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                        </ItemStyle>
                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                            BackColor="#E0E0E0"></HeaderStyle>
                                                    </asp:DataGrid></div>
                                            </cc1:CollapsiblePanel>
                                            <!-- *****************************************-->
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <asp:ListBox ID="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" Font-Size="X-Small"
                Font-Names="Verdana" ForeColor="Red" Width="100px" Height="32px" Visible="false">
            </asp:ListBox>
        </ContentTemplate>
       
    </asp:UpdatePanel>
      <input type="hidden" name="txthiddenImage" />
    </form>
</body>
</html>
