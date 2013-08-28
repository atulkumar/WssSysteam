<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_ActionViewOnly, App_Web_ox4fgu7f" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Action View</title>
 <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
</head>

<script type="text/javascript"> 
    function RefreshGrid() 
    {
        document.getElementById('hid').click();
    } 
    function closeme()
    {
        window.close();
    }
</script>

<script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.HideModalDiv)
                opener.parent.HideModalDiv();
            }
        }
        //Modified By Atul to execute script on Page Load
        function OnLoad() {
           if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.LoadModalDiv)
                opener.parent.LoadModalDiv();
            }
        }
        window.onload = OnLoad;
        window.onunload = OnClose;
</script>

<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" OnAjaxUpdate="RadToolTipManager1_AjaxUpdate"
            RelativeTo="Mouse" Position="TopRight" Title="User Details" Skin="Web20" Width="280px"
            ManualClose="false" Height="220px" Animation="Fade" Sticky="true" Visible="true">
        </telerik:RadToolTipManager>
    <telerik:RadAjaxPanel ID="AjaxPanel" runat="server" EnableAJAX="true" LoadingPanelID="RadAjaxLoadingPanel1">
        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
            border="0">
            <tr>
                <td style="width: 184px">
                    <asp:Label ID="lblTitleLabelActionView" runat="server" Height="12px" Width="128px" CssClass="TitleLabel">&nbsp;Action View</asp:Label>
                </td>
                <td style="width: 607px" align="right">
                    <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" Visible="false" ToolTip="Search" ImageUrl="../../Images/s1search02.gif"
                        AlternateText="Search"></asp:ImageButton>&nbsp;
                    <asp:ImageButton Visible="true" ID="imgClose" AccessKey="L" TabIndex="21" runat="server"
                        ImageUrl="../../Images/s2close01.gif" ToolTip="Close" OnClientClick="closeme();"></asp:ImageButton>&nbsp;
                    <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0" />
                </td>
                <td>
                    <font face="Verdana" size="1"><strong><font face="Verdana" size="1"><strong></strong>
                    </font>&nbsp;</strong></font>&nbsp;
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                 
                    <telerik:RadGrid ID="radActionOnly" runat="server" AllowFilteringByColumn="True"
                        EnableEmbeddedSkins="true" BorderColor="Silver" BorderStyle="None" Skin="Office2007" CssClass="Grid"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="30"
                        ShowFooter="true" CellPadding="0" AllowSorting="True">
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle Font-Size="8pt" ForeColor="#FFFFFF" CssClass="GridItem"></ItemStyle>
                        <FooterStyle BackColor="#E0E0E0" BorderStyle="None" />
                        <AlternatingItemStyle Font-Size="8pt" ForeColor="#333333" BackColor="#F5F5F5" />
                        <GroupHeaderItemStyle BackColor="#ECF3FC" BorderStyle="None" Width="100%" />
                        <MasterTableView Width="100%" EditMode="InPlace" AllowAutomaticInserts="True" AllowFilteringByColumn="true">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="A" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgComm" runat="server" ImageUrl="../../Images/comment_Blank.gif" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="C" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="imgAtt" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="AM_NU9_Action_Number" AutoPostBackOnFilter="true" HeaderText="ActNo" ReadOnly="True"
                                    SortExpression="ActNo" UniqueName="ActNo" AllowFiltering="true" CurrentFilterFunction="EqualTo">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Description" AutoPostBackOnFilter="true" HeaderText="Description" ReadOnly="True"
                                    SortExpression="Description" UniqueName="Description" CurrentFilterFunction="Contains" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="ActionOwner" AutoPostBackOnFilter="true" HeaderText="ActionOwner" ReadOnly="True"
                                    SortExpression="ActionOwner" UniqueName="ActionOwner" AllowFiltering="true" CurrentFilterFunction="Contains">
                                                        <ItemTemplate>
                                                            <asp:LinkButton Text='<%# Eval("ActionOwner") %>' ID="LnkRequestBy" runat="server"></asp:LinkButton>
                                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ActionDate" AutoPostBackOnFilter="true" HeaderText="ActionDate" ReadOnly="True"
                                    SortExpression="ActionDate" UniqueName="ActionDate" CurrentFilterFunction="Contains" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserHours" AutoPostBackOnFilter="true" HeaderText="UserHours" ReadOnly="True"
                                    SortExpression="UserHours" UniqueName="AM_VC_2000_Description"
                                    CurrentFilterFunction="Contains" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Blank1"  HeaderText="Blank1" Visible="false" SortExpression="Blank1"
                                    UniqueName="Blank1" AllowFiltering="true" CurrentFilterFunction="Contains">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Blank2" HeaderText="Blank2" Visible="false" SortExpression="Blank2"
                                    UniqueName="Blank2" AllowFiltering="true" CurrentFilterFunction="Contains">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <FilterItemStyle Height="16px" />
                        <FilterMenu Skin="Vista">
                            <CollapseAnimation Duration="200" Type="OutQuint" />
                        </FilterMenu>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" HorizontalAlign="Left" AlwaysVisible="True"
                            Font-Names="Verdana" Font-Size="X-Small" Height="16px" />
                        <ClientSettings Selecting-AllowRowSelect="true">
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Width="35px"
            Transparency="30" Height="16px">
            <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
                style="margin-top: 100px" />
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadCodeBlock ID="CodeBlock1" runat="server">

            <script type="text/javascript">
                function ShowAttachmentForm(CompId, CallNo, TaskNo, ActionNo) 
                {              
                    window.radopen("../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AO&CompId=" + CompId + "&CallNo=" + CallNo + "&TaskNo=" + TaskNo + "&ACTIONNO=" + ActionNo, "AttachmentsDialog");
                    return false;
                }
                function ShowCommentsForm(CompId, CallNo, TaskNo, ActionNo) 
                {
                    window.radopen("comment.aspx?ScrID=464&tbname=AO&CompId=" + CompId + "&CallNo=" + CallNo + "&TaskNo=" + TaskNo + "&ActionNo=" + ActionNo, "CommentsDialog");
                    return false;
                }
            </script>

        </telerik:RadCodeBlock>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="AttachmentsDialog" runat="server" Title="Call Attachments"
                Height="350px" Width="700px" Left="50px" ReloadOnShow="true" Modal="true" Skin="Vista"
                VisibleStatusbar="false" Behavior="Close,Maximize,Minimize,Reload,Move">
            </telerik:RadWindow>
            <telerik:RadWindow ID="CommentsDialog" runat="server" Title="Call Comments" Width="525px"
                Height="510px" ReloadOnShow="true" Modal="true" Skin="Vista" OnClientClose="RefreshGrid"
                VisibleStatusbar="false" Behavior="Close,Maximize,Minimize,Reload,Move">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    </form>
</body>
</html>
