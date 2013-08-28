<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_Design, App_Web_ixviuxgi" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <div>
        <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="1">
            <tr>
                <td valign="top" width="100%">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td>
                                <div align="right">
                                    <img height="2" src="../../Images/top_right_line.gif" width="96"></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../../Images/top_left_back.gif">
                                            &nbsp;
                                        </td>
                                        <td width="50">
                                            <img height="20" src="../../Images/top_right.gif" width="50">
                                        </td>
                                        <td width="21">
                                            <a href="#">
                                                <img height="20" src="../../Images/bt_min.gif" width="21" border="0"></a>
                                        </td>
                                        <td width="21">
                                            <a href="#">
                                                <img height="20" src="../../Images/bt_max.gif" width="21" border="0"></a>
                                        </td>
                                        <td width="19">
                                            <a href="#">
                                                <img onclick="CloseWSS();" height="20" src="../../Images/bt_clo.gif" width="19" border="0"></a>
                                        </td>
                                        <td width="6">
                                            <img height="20" src="../../Images/bt_space.gif" width="6">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr width="100%">
                                        <td background="../../Images/top_nav_back.gif" height="67">
                                            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                                <tr>
                                                    <td>
                                                        <div style="width: 130px">
                                                            <span style="display: none">
                                                                <asp:Button ID="BtnGrdSearch" runat="server" Height="0" Width="0px"></asp:Button></span><asp:ImageButton
                                                                    ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px" AlternateText="."
                                                                    CommandName="submit" ImageUrl="white.GIF"></asp:ImageButton><img class="PlusImageCSS"
                                                                        onclick="HideContents();" alt="Hide" src="../../Images/left005.gif" name="imgHide">
                                                            <img class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../../Images/Right005.gif"
                                                                name="ingShow">
                                                            <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel"> CALL/TASK</asp:Label></div>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <asp:UpdatePanel ID="ee" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" AlternateText="Add" ImageUrl="../../Images/s2Add01.gif">
                                                                    </asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif">
                                                                    </asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" AlternateText="Edit" ImageUrl="../../Images/S2edit01.gif">
                                                                    </asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" AlternateText="Search"
                                                                        ImageUrl="../../Images/s1search02.gif"></asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgAttachments" AccessKey="A" runat="server" AlternateText="View Attachments"
                                                                        ImageUrl="../../Images/ScreenHunter_075.bmp"></asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server" AlternateText="Close Call"
                                                                        ImageUrl="../../Images/CloseCall1.gif"></asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgMyCall" AccessKey="M" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                                        ToolTip="My Calls"></asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgMonitor" AccessKey="M" runat="server" AlternateText="Call Monitor"
                                                                        ImageUrl="../../Images/callmonitor.jpg"></asp:ImageButton>&nbsp;
                                                                    <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                                                                        ToolTip="Close"></asp:ImageButton>&nbsp;
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                    <td style="width: 40px">
                                                        <asp:UpdateProgress ID="progress" runat="server">
                                                            <ProgressTemplate>
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="../../Images/ajax1.gif" Width="24"
                                                                    Height="24" />
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </td>
                                                    <td align="center">
                                                        <div style="width: 130px">
                                                            <font face="Verdana" size="1"><strong>View&nbsp;
                                                                <asp:DropDownList ID="ddlstview" runat="server" Width="80px" Font-Names="Verdana"
                                                                    Font-Size="XX-Small" AutoPostBack="False">
                                                                </asp:DropDownList>
                                                                <asp:ImageButton ID="imgBtnViewPopup" runat="server" ImageUrl="../../Images/plus.gif">
                                                                </asp:ImageButton></strong></font></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right" width="152" background="../../Images/top_nav_back01.gif" height="67">
                                            <div style="width: 150px">
                                                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('4','../../');"
                                                    alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                        class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                        src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;</div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="10">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../../Images/main_line.gif" height="10">
                                            <img height="10" src="../../Images/main_line.gif" width="6">
                                        </td>
                                        <td width="7" height="10">
                                            <img height="10" src="../../Images/main_line01.gif" width="7">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--  <tr>
                            <td height="2">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../../Images/main_line02.gif" height="2">
                                            <img height="2" src="../../Images/main_line02.gif" width="2">
                                        </td>
                                        <td width="12" height="2">
                                            <img height="2" src="../../Images/main_line03.gif" width="12">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr>
                            <td width="100%">
                                <div style="overflow: auto; width: 100%; height: 581px">
                                    <table id="table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                        border="0">
                                        <tr>
                                            <td valign="top">
                                                <!--  **********************************************************************-->
                                                <div style="overflow: auto; width: 100%; height: 581px">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="1" style="border-color: Red">
                                                        <tbody>
                                                             <tr>
                                                                <td style="width: 1099px">
                                                                    <cc1:CollapsiblePanel ID="cpnlError" runat="server" Height="47px" Width="100%" BorderColor="Indigo"
                                                                        Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="true"
                                                                        TitleBackColor="transparent" Text="Error Message" ExpandImage="../../Images/ToggleDown.gif"
                                                                        CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderStyle="Solid"
                                                                        BorderWidth="0px">
                                                                        <table id="table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" width="100%"
                                                                            border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="../../Images/warning.gif">
                                                                                    </asp:Image>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                    <asp:Label ID="lblError" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                        ForeColor="Red"></asp:Label>
                                                                                    <asp:ListBox ID="lstError" runat="server" Width="702px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                        BorderWidth="0" BorderStyle="Groove" ForeColor="Red"></asp:ListBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </cc1:CollapsiblePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 1099px">
                                                                    <asp:UpdatePanel ID="upnlCallView" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table cellspacing="0" cellpadding="0" width="100%" border="2">
                                                                                <tr>
                                                                                    <td>
                                                                                        <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" Height="294px" Width="100%"
                                                                                            BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                                            TitleClickable="true" TitleBackColor="transparent" Text="Call View" ExpandImage="../../Images/ToggleDown.gif"
                                                                                            CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderStyle="Solid"
                                                                                            BorderWidth="1px">
                                                                                            <div style="overflow:auto; width: 100%; height: 250px">
                                                                                                <table id="table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                                                    width="380%" align="left" border="1">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:Panel ID="pnl" runat="server" Width="42px">
                                                                                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:CheckBox ID="CHKC" runat="server" Width="18px" ToolTip="Comment Search" Font-Size="XX-Small"
                                                                                                                                            BorderWidth="0"></asp:CheckBox>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:CheckBox ID="CHKA" runat="server" Width="18px" ToolTip="Attachment Search" Font-Size="XX-Small"
                                                                                                                                            BorderWidth="0"></asp:CheckBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </asp:Panel>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:Panel ID="Panel1" runat="server">
                                                                                                                        </asp:Panel>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td valign="top" align="left">
                                                                                                            <!--  **********************************************************************-->
                                                                                                            <asp:DataGrid ID="GrdAddSerach" runat="server" CssClass="Grid" Font-Names="Verdana"
                                                                                                                BorderWidth="1px" BorderStyle="None" BorderColor="Silver" ForeColor="MidnightBlue"
                                                                                                                AllowSorting="true" DataKeyField="CallNo" PageSize="20" HorizontalAlign="Left"
                                                                                                                GridLines="Horizontal" CellPadding="0" Width="100%" PagerStyle-Visible="False"
                                                                                                                AllowPaging="true">
                                                                                                                <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                                                <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                                                                                </ItemStyle>
                                                                                                                <HeaderStyle Font-Size="8pt" Font-Bold="true" ForeColor="Black" BorderColor="White"
                                                                                                                    BackColor="#E0E0E0"></HeaderStyle>
                                                                                                                <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                                                <Columns>
                                                                                                                    <asp:TemplateColumn HeaderText="C">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Image ID="imgComm" runat="server" ImageUrl="../../Images/comment_Blank.gif">
                                                                                                                            </asp:Image>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateColumn>
                                                                                                                    <asp:TemplateColumn HeaderText="A">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Image ID="imgAtt" runat="server"></asp:Image>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateColumn>
                                                                                                                </Columns>
                                                                                                                <PagerStyle Visible="False" Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                                                            </asp:DataGrid><!-- Panel for displaying Task Info -->
                                                                                                            <!-- Panel for displaying Action Info-->
                                                                                                            <!-- ***********************************************************************-->
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="left" style="background-color:Blue"><span style="vertical-align:top">
                                                                                                                  <span width="40px">
                                                                                                                        <asp:Label ID="pg" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                                                            ForeColor="#0000C0" runat="server" Font-Bold="true">Page</asp:Label>
                                                                                                                    </span>
                                                                                                                   <span  Width="10px">
                                                                                                                        <asp:Label ID="CurrentPg" runat="server" Height="12px" Font-Size="X-Small"
                                                                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                                                                    </span>
                                                                                                                     <span Width="5%">
                                                                                                                        <asp:Label ID="off" Font-Size="8pt" Font-Names="Verdana" ForeColor="#0000C0" runat="server"
                                                                                                                            Font-Bold="true">of</asp:Label>
                                                                                                                    </span>
                                                                                                                    <span Width="10px" >
                                                                                                                        <asp:Label ID="TotalPages" runat="server" Height="12px" Font-Size="X-Small"
                                                                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                                                                    </span>
                                                                                                                    <span  style="vertical-align:top;">
                                                                                                                        <asp:ImageButton ID="Firstbutton" runat="server" ImageUrl="../../Images/next9.jpg"
                                                                                                                            AlternateText="First" ToolTip="First"></asp:ImageButton>
                                                                                                                    </span>
                                                                                                                   <span>
                                                                                                                        <asp:ImageButton ID="Prevbutton" runat="server" ImageUrl="../../Images/next99.jpg"
                                                                                                                            ToolTip="Previous"></asp:ImageButton>
                                                                                                                    </span>
                                                                                                                    <span >
                                                                                                                        <asp:ImageButton ID="Nextbutton" runat="server" ImageUrl="../../Images/next9999.jpg"
                                                                                                                            ToolTip="Next"></asp:ImageButton>
                                                                                                                    </span>
                                                                                                                    <span>
                                                                                                                        <asp:ImageButton ID="Lastbutton" runat="server" ImageUrl="../../Images/next999.jpg"
                                                                                                                            ToolTip="Last"></asp:ImageButton>
                                                                                                                    </span>
                                                                                                                    <span  style="vertical-align:top;">
                                                                                                                        <asp:TextBox ID="txtPageSize" runat="server" Width="24px" Height="14px" Font-Size="7pt"
                                                                                                                            MaxLength="3"></asp:TextBox>
                                                                                                                    </span>
                                                                                                                    <span>
                                                                                                                        <asp:Button ID="Button3" runat="server" Width="16px" Height="12pt" ToolTip="Change Paging Size"
                                                                                                                            Font-Size="7pt" BorderStyle="None" Text=">" ForeColor="Navy" Font-Bold="true">
                                                                                                                        </asp:Button>
                                                                                                                    </span>
                                                                                                                    <span>
                                                                                                                        <asp:Label ID="lblrecords" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                                                            ForeColor="MediumBlue" Font-Bold="true">Total Records</asp:Label>
                                                                                                                    </span>
                                                                                                                    <span> 
                                                                                                                        <asp:Label ID="TotalRecods" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                                                                    </span>
                                                                                                              </span>
                                                                                                      <%--      <table cellpadding="0" cellspacing="0" height="25" border="1" style="background-color:Lime" width="250px" >
                                                                                                                <tr>
                                                                                                                    <td Width="5%">
                                                                                                                        <asp:Label ID="pg" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                                                            ForeColor="#0000C0" runat="server" Font-Bold="true">Page</asp:Label>
                                                                                                                    </td>
                                                                                                                   <td  Width="5%">
                                                                                                                        <asp:Label ID="CurrentPg" runat="server" Height="12px" Font-Size="X-Small"
                                                                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td  Width="5%">
                                                                                                                        <asp:Label ID="off" Font-Size="8pt" Font-Names="Verdana" ForeColor="#0000C0" runat="server"
                                                                                                                            Font-Bold="true">of</asp:Label>
                                                                                                                    </td>
                                                                                                                    <td Width="5%">
                                                                                                                        <asp:Label ID="TotalPages" runat="server" Height="12px" Font-Size="X-Small"
                                                                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td width="5%">
                                                                                                                        <asp:ImageButton ID="Firstbutton" runat="server" ImageUrl="../../Images/next9.jpg"
                                                                                                                            AlternateText="First" ToolTip="First"></asp:ImageButton>
                                                                                                                    </td>
                                                                                                                   <td width="5%">
                                                                                                                        <asp:ImageButton ID="Prevbutton" runat="server" ImageUrl="../../Images/next99.jpg"
                                                                                                                            ToolTip="Previous"></asp:ImageButton>
                                                                                                                    </td>
                                                                                                                    <td width="5%">
                                                                                                                        <asp:ImageButton ID="Nextbutton" runat="server" ImageUrl="../../Images/next9999.jpg"
                                                                                                                            ToolTip="Next"></asp:ImageButton>
                                                                                                                    </td>
                                                                                                                    <td width="5%">
                                                                                                                        <asp:ImageButton ID="Lastbutton" runat="server" ImageUrl="../../Images/next999.jpg"
                                                                                                                            ToolTip="Last"></asp:ImageButton>
                                                                                                                    </td>
                                                                                                                    <td width="5%">
                                                                                                                        <asp:TextBox ID="txtPageSize" runat="server" Width="24px" Height="14px" Font-Size="7pt"
                                                                                                                            MaxLength="3"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td width="5%">
                                                                                                                        <asp:Button ID="Button3" runat="server" Width="16px" Height="12pt" ToolTip="Change Paging Size"
                                                                                                                            Font-Size="7pt" BorderStyle="None" Text=">" ForeColor="Navy" Font-Bold="true">
                                                                                                                        </asp:Button>
                                                                                                                    </td>
                                                                                                                    <td width="5%">
                                                                                                                        <asp:Label ID="lblrecords" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                                                            ForeColor="MediumBlue" Font-Bold="true">Total Records</asp:Label>
                                                                                                                    </td>
                                                                                                                    <td width="5%"> 
                                                                                                                        <asp:Label ID="TotalRecods" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                                                                            ForeColor="Crimson" Font-Bold="true"></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>--%>
                                                                                                        </td>
                                                                                                       
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </cc1:CollapsiblePanel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <%--     <tr>
                                                                <td style="width: 1099px">
                                                                    <asp:UpdatePanel ID="upnlCallTask" runat="server">
                                                                        <ContentTemplate>
                                                                            <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Height="66px" Width="100%"
                                                                                BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
                                                                                TitleClickable="true" TitleBackColor="transparent" Text="Task View" ExpandImage="../../Images/ToggleDown.gif"
                                                                                CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderStyle="Solid"
                                                                                BorderWidth="0px">
                                                                                <table style="border-collapse: collapse" width="670" border="0">
                                                                                    <tr align="left">
                                                                                        <td>
                                                                                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <asp:Panel ID="pnlTask" Width="1px" runat="server">
                                                                                    <table style="border-collapse: collapse" cellspacing="0" cellpadding="0" align="left"
                                                                                        border="0">
                                                                                        <tr valign="top">
                                                                                            <td>
                                                                                                <asp:Image ID="imgHidTask" Width="180px" Height="18px" ImageUrl="../../Images/divider.gif"
                                                                                                    runat="server"></asp:Image>
                                                                                                <asp:TextBox ID="TxtStatus_F" runat="server" Height="18px" CssClass="txtNoFocusFE"
                                                                                                    Font-Size="XX-Small" Font-Names="Verdana" BorderWidth="1px" BorderStyle="Solid"
                                                                                                    Visible="False">ASSIGNED</asp:TextBox>
                                                                                                <asp:TextBox ID="TxtTaskNo_F" Height="18px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    BorderWidth="1px" BorderStyle="Solid" Visible="False" runat="server" Enabled="False"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="TxtSubject_F" runat="server" Width="187px" Height="18px" CssClass="txtNoFocusFE"
                                                                                                    Font-Size="XX-Small" Font-Names="Verdana" BorderWidth="1px" BorderStyle="Solid"
                                                                                                    MaxLength="950" TextMode="MultiLine"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <uc1:CustomDDL ID="CDDLTaskType_F" runat="server" Width="68px"></uc1:CustomDDL>
                                                                                            </td>
                                                                                            <td>
                                                                                                <uc1:CustomDDL ID="CDDLTaskOwner_F" runat="server" Width="77px"></uc1:CustomDDL>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="DDLDependency_F" class="DDLFieldFE" Width="42px" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <ION:Customcalendar ID="dtEstCloseDate" runat="server" Width="91px" Height="19px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="TxtEstimatedHrs" runat="server" Width="30px" Height="12px" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:CheckBox ID="chkMandatory" runat="server" Width="27px" Height="18px" ToolTip="Action Mandatory"
                                                                                                    AutoPostBack="False" Font-Size="XX-Small" Font-Names="Verdana" Checked="true">
                                                                                                </asp:CheckBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <uc1:CustomDDL ID="CDDLPriority_F" runat="server" Width="63px"></uc1:CustomDDL>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </cc1:CollapsiblePanel>
                                                                            <input type="hidden" runat="server" id="txtcallNo" />
                                                                            <span style="display: none">
                                                                                <asp:Button ID="BtnGrdSearch1" runat="server" Height="0" Width="0" /></span>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>--%>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                            <td valign="top" width="12" background="../../Images/main_line04.gif">
                                                <img height="1" src="../../Images/main_line04.gif" width="12">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="2">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../../images/main_line06.gif" height="2">
                                            <img height="2" src="../../images/main_line06.gif" width="2">
                                        </td>
                                        <td width="12" height="2">
                                            <img height="2" src="../../images/main_line05.gif" width="12">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../../images/bottom_back.gif">
                                            &nbsp;
                                        </td>
                                        <td width="66">
                                            <img height="31" src="../../images/bottom_right.gif" width="66">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlMsg" runat="server">
                    </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server">
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
