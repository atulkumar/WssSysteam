<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Comment_View.aspx.vb" Inherits="NewCall_CommentView_Simple" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Comment View</title>

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/CallViewShortCuts.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../../Images/Js/StyleSheet1.css" />
    
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">

        var globleID;
        var globleUser;
        var globleRole;
        var globleCompany;
        var rand_no = Math.ceil(500 * Math.random())


        function callrefresh() {
            location.href = "AB_Search.aspx";
            //Form1.submit();
        }





        function SaveEdit(varImgValue) {


            if (varImgValue == 'Close') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }



            if (varImgValue == 'Search') {
                document.Form1.txthiddenImage.value = varImgValue;
                //Form1.submit(); 
                __doPostBack("up2", "");
                return false;
            }

            if (varImgValue == 'Logout') {

                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
            }

            if (varImgValue == 'CloseCall') {

                document.Form1.txthiddenImage.value = varImgValue;

                //document.Form1.txtrowvaluescall.value =0;  
                __doPostBack("AjaxPanel", "");
                //Form1.submit(); 

            }
            return false;
        }

        function OpenW(varTable) {

            //window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company  from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c  ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
            wopen('../AdministrationCenter/AddressBook/AB_ViewColumns.aspx? ID=' + varTable, 'Search' + rand_no, 500, 450);
            //	wopen('AB_ViewColumns.aspx','UserView',500,450);
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
						'status=no, toolbar=no, scrollbars=yes, resizable=yes');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }

        function OpenComment(Level, CompID, CN, TN, AN) {
            wopen('../../SupportCenter/Callview/comment.aspx?ScrID=464&From=Home&Level=' + Level + '&CID=' + CompID + '&CN=' + CN + '&TN=' + TN + '&AN=' + AN, 'Comment' + rand_no, 500, 450);
        }
				
    </script>

    <script type="text/javascript">
        //A Function to call on Page Load to set grid width according to screen size
        function onLoad() {
            var divCallView = document.getElementById('divCallView');
            if (divCallView != null) {
                if (document.body.clientWidth >0) {
                    divCallView.style.width = document.body.clientWidth - 30 + "px";
                }
            }
        }
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlGrdView_collapsible').cells[0].colSpan = "1";
        }
        //A Function is Called when we resize window
        window.onresize = onLoad;     
    </script>

    <script type="text/javascript">
    function tabClose() {
        window.parent.closeTab();
    }
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="Form1" runat="server">
    <telerik:RadScriptManager ID="radscript" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="AjaxPanel" runat="server" EnableAJAX="true" LoadingPanelID="RadAjaxLoadingPanel1">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td valign="top" style="width: 100%">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="width: 100%" background="../../Images/top_nav_back.gif" height="40" valign="middle"
                                nowrap="nowrap">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button><asp:ImageButton
                                                    ID="imgbtnSearch" TabIndex="1" runat="server" Width="0px" Height="0px" ImageUrl="../white.GIF"
                                                    CommandName="submit" AlternateText="."></asp:ImageButton>
                                            <asp:Label ID="lblTitleLabelRoleSearch" runat="server" CssClass="TitleLabel"> COMMENT VIEW</asp:Label>
                                        </td>
                                        <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                            <center>
                                                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                    ToolTip="Reset" Visible="False"></asp:ImageButton>&nbsp;
                                                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                    ToolTip="Search" Visible="false"></asp:ImageButton>&nbsp;
                                                <asp:ImageButton ID="imgCloseCall" AccessKey="M" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                    ToolTip="My Calls"></asp:ImageButton>&nbsp;
                                                <img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;"
                                                    onclick="javascript:location.reload(true);" />&nbsp;
                                                <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                    style="cursor: hand;" />
                                            </center>
                                        </td>
                                        <td style="width: 15%;" nowrap="nowrap">
                                            <img id="imgAjax" title="ajax" height="30" src="../../images/divider.gif" width="30" />
                                            <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2207','../../');"
                                            alt="Video Help" src="../../Images/video_help.jpg" border="0"/>&nbsp;<img
                                                class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('968','../../');"
                                                alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;<img
                                                    class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                    src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:CollapsiblePanel ID="cpnlGrdView" runat="server" Width="100%" Height="47px"
                                    BorderStyle="Solid" BorderWidth="0px" Visible="true" BorderColor="Indigo" TitleCSS="test"
                                    PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                                    Text="Comment View" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                    Draggable="False">
                                    <div id="divCallView" style="overflow: auto; height: 490px; width: 1055px">
                                        <telerik:RadGrid ID="GrdComments" runat="server" AllowPaging="True" EnableTheming="true"
                                            EnableEmbeddedSkins="true" Skin="Office2007" AllowFilteringByColumn="false" AllowSorting="True"
                                            PageSize="20" GridLines="None" AutoGenerateColumns="false" ShowGroupPanel="true"
                                            GroupingEnabled="true" Width="100%">
                                            <ClientSettings AllowDragToGroup="true" AllowColumnsReorder="true" AllowRowHide="true">
                                                <Selecting AllowRowSelect="true" />
                                            </ClientSettings>
                                            <SelectedItemStyle BackColor="#FFCC66" />
                                            <ItemStyle Font-Size="8pt" ForeColor="#FFFFFF" CssClass="GridItem"></ItemStyle>
                                            <FooterStyle BackColor="#E0E0E0" BorderStyle="None" />
                                            <AlternatingItemStyle Font-Size="8pt" ForeColor="#333333" BackColor="#F5F5F5" />
                                            <GroupHeaderItemStyle BackColor="#ECF3FC" BorderStyle="None" Width="100%" />
                                            <PagerStyle Wrap="false" Width="300px" Mode="NextPrevAndNumeric" Font-Size="8pt" />
                                            <ExportSettings ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                                                <Pdf FontType="Subset" PaperSize="Letter" />
                                                <Excel Format="ExcelML" />
                                                <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                            </ExportSettings>
                                            <FilterMenu Skin="Gray" EnableEmbeddedSkins="true" EnableTheming="true" DefaultGroupSettings-ExpandDirection="Down">
                                            </FilterMenu>
                                            <MasterTableView AllowFilteringByColumn="True" CssClass="Grid" GroupsDefaultExpanded="true">
                                                <GroupByExpressions>
                                                    <telerik:GridGroupByExpression>
                                                        <SelectFields>
                                                            <telerik:GridGroupByField FieldName="CommentLevel" />
                                                        </SelectFields>
                                                        <GroupByFields>
                                                            <telerik:GridGroupByField FieldName="CommentLevel" />
                                                        </GroupByFields>
                                                    </telerik:GridGroupByExpression>
                                                </GroupByExpressions>
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="C" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        AllowFiltering="false">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgComment" runat="server" ImageUrl="../../Images/comment_Unread.gif">
                                                            </asp:Image>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="CommentLevel" HeaderText="Level" DataType="System.String"
                                                        UniqueName="CommentLevel" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" SortExpression="CommentLevel">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn GroupByExpression="CommentBy CommentBy Group By CommentBy"
                                                        UniqueName="CommentBy" DataType="System.String" AutoPostBackOnFilter="true" DataField="CommentBy"
                                                        FilterListOptions="VaryByDataType" CurrentFilterFunction="Contains" HeaderText="CommentBy"
                                                        SortExpression="CommentBy">
                                                        <ItemTemplate>
                                                            <asp:LinkButton Text='<%# Eval("CommentBy") %>' ID="LnkRequestBy" runat="server"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <%-- <telerik:GridBoundColumn DataField="CommentBy" HeaderText="CommentBy" DataType="System.String"
                                                                                                                        UniqueName="CommentBy" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                                                                                        CurrentFilterFunction="Contains" SortExpression="CommentBy">
                                                                                                                        <HeaderStyle Width="85pt"></HeaderStyle>
                                                                                                                        <ItemStyle Width="85pt"></ItemStyle>
                                                                                                                    </telerik:GridBoundColumn>--%>
                                                    <telerik:GridBoundColumn DataField="CommentTo" HeaderText="CommentTo" DataType="System.String"
                                                        UniqueName="CommentTo" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" SortExpression="CommentTo">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridNumericColumn DataField="CallNo" HeaderText="CallNo"  
                                                        UniqueName="CallNo" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="EqualTo" SortExpression="CallNo">
                                                       <%-- <HeaderStyle Width="50pt" VerticalAlign="Top"></HeaderStyle>
                                                        <ItemStyle Width="50pt"></ItemStyle>--%>
                                                    </telerik:GridNumericColumn>
                                                    <telerik:GridNumericColumn DataField="TaskNo" HeaderText="TaskNo" 
                                                        UniqueName="TaskNo" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="EqualTo" SortExpression="TaskNo">
                                                    </telerik:GridNumericColumn>
                                                    <telerik:GridNumericColumn DataField="ActionNo" HeaderText="ActNo" 
                                                        UniqueName="ActionNo" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="EqualTo" SortExpression="ActionNo">
                                                      <%--  <HeaderStyle Width="50pt"></HeaderStyle>
                                                        <ItemStyle Width="50pt"></ItemStyle>--%>
                                                    </telerik:GridNumericColumn>
                                                    <telerik:GridBoundColumn DataField="CommentDesc" HeaderText="Description" DataType="System.String"
                                                        UniqueName="CommentDesc" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" SortExpression="CommentDesc">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridDateTimeColumn DataField="CommentDate" HeaderText="CommentDate" DataType="System.DateTime"
                                                        UniqueName="CommentDate" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="EqualTo" SortExpression="CommentDate">
                                                    </telerik:GridDateTimeColumn>
                                                    <telerik:GridBoundColumn DataField="Company" HeaderText="Company" DataType="System.String"
                                                        UniqueName="Company" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" SortExpression="Company">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MailList" HeaderText="Mail To" DataType="System.String"
                                                        UniqueName="MailList" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" SortExpression="MailList">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn Visible="False" DataField="CompID" HeaderText="CompID">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn Visible="False" DataField="FLag" HeaderText="Flag">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn Visible="False" DataField="ReadFlag" HeaderText="ReadFlag">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </cc1:CollapsiblePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox ID="lstError" runat="server" Width="752px" BorderWidth="0" BorderStyle="Groove"
                        ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small" Visible="false"></asp:ListBox>
                    <input type="hidden" name="txthiddenAdno" />
                    <input type="hidden" name="txthiddenImage" />
                    <input type="hidden" name="txtFilePath" />
                </td>
            </tr>
        </table>
        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" OnAjaxUpdate="RadToolTipManager1_AjaxUpdate"
            RelativeTo="Mouse" Position="TopRight" Title="User Details" Skin="Web20" Width="280px"
            ManualClose="false" Height="220px" Animation="Fade" Sticky="true" Visible="true">
        </telerik:RadToolTipManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Width="35px"
            Transparency="30" Height="16px">
            <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
                style="margin-top: 100px" />
        </telerik:RadAjaxLoadingPanel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="GrdCommants">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GrdComments" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    </form>
</body>
</html>
