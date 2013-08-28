<%@ page language="VB" autoeventwireup="false" maintainscrollpositiononpostback="true" inherits="NewWork_CallView_Simple, App_Web_w1fsapaf" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CallView Simple</title>
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script src="../../Images/Js/CallViewShortCuts.js" type="text/javascript"></script>

    <script type="text/javascript">
        //Code to open ModelPopUpWindow for SimpleCallView related views
        function ShowSimpleCallViewsForm() {
            window.radopen("../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=1024&TBLName=T040011&Page_name=Call_Heirarchy", "Win_SimpleCallView");
            return false;
        }
        function confirm_Logout() {
            if (confirm("Are you sure you want to Logout WSS?") == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function CheckChar(text, e) {
            var checkValue;
            if (e.srcElement.id.indexOf('_CallNo') > 0) {
                checkValue = document.getElementById(e.srcElement.id).value;
            }
            else {
                return false;
            }
            if (isNaN(checkValue)) {
                document.getElementById(e.srcElement.id).value = '';
                alert('Please Enter Numeric Values for Call No');
                return false;
            }
            else {
                return true;
            }
        }




        //    var checkOK = "0123456789";
        //    var checkStr = theForm.txtNumbers.value;
        //    var allValid = true;
        //    var allNum = "";
        //for (i = 0;  i < checkStr.length;  i++)
        //{
        //        ch = checkStr.charAt(i);
        //        for (j = 0;  j < checkOK.length;  j++)
        //            if (ch == checkOK.charAt(j))
        //            break;
        //            if (j == checkOK.length)
        //            {
        //                allValid = false;
        //                break;
        //            }
        //                if (ch != ",")
        //                allNum += ch;
        //}
        //if (!allValid)
        //{
        //alert("Please enter a numeric value.");
        //return (false);
        //} 
        //}}



        function KeyCheck55(nn, rowvalues, tableID, Comp) {
            var screenid = window.parent.getActiveTabDetails();
            //						    	//Security Block
            //								var obj=document.getElementById("imgEdit")
            //								
            //								if(obj==null)
            //								{
            //								alert("You don't have access rights to edit record");
            //								return false;
            //								}
            //         						if (obj.disabled==true) 
            //									{
            //										alert("You don't have access rights to edit record");
            //										return false;
            //									}
            //							//End of Security Block	


            //alert("asdf");

            document.Form1.txthidden.value = nn;
            document.Form1.txthiddenImage.value = 'Edit';
            document.Form1.txthiddentable.value = tableID;
            document.Form1.txtComp.value = Comp;

            if (tableID == 'cpnlCallTask_dtgTask') {
                document.Form1.txthiddenTaskNo.value = nn;
                OpenTask(nn);
            }
            else {
                document.Form1.txthiddenCallNo.value = nn;
                window.parent.OpenCallDetailInTab('Call# ' + nn, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&CallNumber=" + nn + "&CompId=" + Comp, 'Call' + nn, screenid);
                //Form1.submit();
                return false;
            }


        }


        function RefreshGrid() {
            //alert(2);
            //document.getElementById('hid').click();
            //this.GrdAddSerach.Rebind();
        }
        function RefreshPage() {
            // alert(1);
            document.getElementById('hid1').click();
            //this.GrdAddSerach.Rebind();
        }
        function pnlRequestStarted(ajaxPanel, eventArgs) {
            if (eventArgs.EventTarget == "imgExportToExcel") {
                eventArgs.EnableAjax = false;
            }
            if (eventArgs.EventTarget == "imgExportToWord") {
                eventArgs.EnableAjax = false;
            }
        }

        //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }
        function OpenTab() {
            window.parent.OpenTabOnAddClick('Call Entry', "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=-1", "3");
        }

    </script>

    <link rel="stylesheet" type="text/css" href="../../Images/Js/StyleSheet1.css" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <telerik:RadScriptManager ID="radscript" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="AjaxPanel" runat="server" EnableAJAX="true" LoadingPanelID="RadAjaxLoadingPanel1"
        ClientEvents-OnRequestStart="pnlRequestStarted">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td valign="top">
                    <asp:Button ID="hid" runat="server" Text="aaaa" Style="display: none" />
                    <asp:Button ID="hid1" runat="server" Text="aaaaaa" Style="display: none" />
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td background="../../Images/top_nav_back.gif" height="47">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td style="width: 15%">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server"
                                                            Width="1px" Height="1px" ImageUrl="white.GIF" CommandName="submit" AlternateText=".">
                                                        </asp:ImageButton>
                                                        <asp:Label ID="lblTitleLabelInvDetail" runat="server" CssClass="TitleLabel">Call Hierarchy </asp:Label>
                                                    </td>
                                                    <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                        <center>
                                                            <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" OnClientClick="OpenTab()"
                                                                ImageUrl="../../Images/s2Add01.gif" AlternateText="New Call"></asp:ImageButton>&nbsp;
                                                            <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" AlternateText="Search"
                                                                ImageUrl="../../Images/s1search02.gif"></asp:ImageButton>&nbsp;
                                                            <asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server" ImageUrl="../../Images/CloseCall1.gif"
                                                                AlternateText="Close Call"></asp:ImageButton>
                                                            <asp:ImageButton ID="imgMyCall" AccessKey="M" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                                ToolTip="My Calls"></asp:ImageButton>
                                                            <asp:ImageButton ID="imgExportToExcel" AccessKey="E" runat="server" ImageUrl="../../Images/Excel.jpg"
                                                                ToolTip="Export To Excel"></asp:ImageButton>
                                                            <asp:ImageButton ID="imgExportToWord" AccessKey="E" runat="server" ImageUrl="../../Images/save_dis999.gif"
                                                                ToolTip="Export To Word" Visible="false"></asp:ImageButton>
                                                            <img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;"
                                                                onclick="javascript:location.reload(true);" />
                                                            <asp:ImageButton ID="imgClose" runat="server" OnClientClick="tabClose();" ImageUrl="../../Images/s2close01.gif"
                                                                AlternateText="Close Window"></asp:ImageButton>&nbsp;
                                                            <%-- <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                style="cursor: hand;" />--%>
                                                        </center>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:DropDownList ID="ddlstview" runat="server" Width="80px" Font-Size="XX-Small"
                                                            Font-Names="Verdana" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:ImageButton ID="imgBtnViewPopup" runat="server" ImageUrl="../../Images/plus.gif">
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 5%;" nowrap="nowrap" background="../../Images/top_nav_back.gif"
                                            height="47">
                                            <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2205','../../');"
                                                alt="Video Help" src="../../Images/video_help.jpg" border="0">&nbsp;
                                            <img class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('2202','../../');"
                                                alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;
                                            <asp:ImageButton ID="Logout" runat="server" CssClass="PlusImageCSS" ImageUrl="../../icons/logoff.gif"
                                                OnClientClick="return confirm_Logout()" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table align="left">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFromDate" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                            runat="server">Action From Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <ION:Customcalendar ID="dtFromDate" runat="server" Height="16px" Width="148px" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbltoDate" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
                                                            runat="server">Action To Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <ION:Customcalendar ID="dtToDate" runat="server" Height="16px" Width="148px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 100%">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" OnAjaxUpdate="RadToolTipManager1_AjaxUpdate"
            RelativeTo="Mouse" Position="TopRight" Title="User Details" Skin="Web20" Width="280px"
            ManualClose="false" Height="220px" Animation="Fade" Sticky="true">
        </telerik:RadToolTipManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Width="35px"
            Transparency="30" Height="16px">
            <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
                style="margin-top: 100px" />
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadCodeBlock ID="CodeBlock1" runat="server">

            <script type="text/javascript">
                function ShowAttachmentForm(TaskId, CompId, CallNo, rowIndex) {
                    //var grid = $find("<%= GrdAddSerach.ClientID %>");
                    //var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element(); 
                    //grid.get_masterTableView().selectItem(rowControl, true);
                    window.radopen("../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&Page_name=Call_Heirarchy&TaskNo=" + TaskId + "&CompId=" + CompId + "&CallNo=" + CallNo, "AttachmentsDialog");
                    return false;

                }
                function ShowCommentsForm(CompId, CallNo, rowIndex) {
                    //var grid = $find("<%= GrdAddSerach.ClientID %>");
                    //var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element(); 
                    //grid.get_masterTableView().selectItem(rowControl, true);
                    window.radopen("../../SupportCenter/Callview/comment.aspx?ScrID=464&Page_name=Call_Heirarchy&tbname=C&ID=" + CallNo + "&CompId=" + CompId + "&CallNo=" + CallNo, "CommentsDialog");
                    return false;
                }
                function ShowCallReqByInfo(CompId, CallNo, CallOwner, rowIndex) {
                    window.radopen("../../SupportCenter/Callview/UserInfo.aspx?ScrID=334&CALLNO=" + CallNo + "&CALLOWNER=" + CallOwner + "&COMP=" + CompId + "&ScreenID=463", "ShowCallReqByInfoDialog");
                    return false;
                }
            </script>

        </telerik:RadCodeBlock>
        <input type="hidden" name="txthidden" />
        <input type="hidden" name="txthiddenImage" />
        <input type="hidden" name="txthiddentable" />
        <input type="hidden" name="txtComp" />
        <input type="hidden" name="txthiddenCallNo" />
        <input type="hidden" name="txthiddenTaskNo" />
        <asp:Panel ID="pnlMsg" runat="server">
        </asp:Panel>
        <asp:ListBox ID="lstError" runat="server" Width="722px" Font-Names="Verdana" Font-Size="XX-Small"
            BorderStyle="Groove" BorderWidth="0" Visible="false"></asp:ListBox>
        <div style="overflow: auto; width: 100%; height: 500px">
            <telerik:RadGrid ID="GrdAddSerach" runat="server" AllowPaging="True" Font-Names="Verdana"
                EnableEmbeddedSkins="true" BorderColor="Silver" BorderStyle="None" Skin="Office2007"
                BorderWidth="1px" GridLines="None" EnableTheming="true" Width="100%" Height="500px"
                AllowSorting="True" PageSize="20" AllowFilteringByColumn="true" AutoGenerateColumns="false"
                GroupPanel-Height="20px" GroupPanel-Font-Bold="false" ShowGroupPanel="True" GroupingEnabled="true">
                <SelectedItemStyle BorderColor="Green" />
                <HeaderStyle Font-Bold="true" />
                <SelectedItemStyle BackColor="#FFCC66" />
                <ClientSettings AllowDragToGroup="true" AllowAutoScrollOnDragDrop="true" Selecting-AllowRowSelect="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                </ClientSettings>
                <PagerStyle Wrap="false" Mode="NextPrevAndNumeric" Font-Size="8pt" />
                <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                    <Pdf FontType="Subset" PaperSize="Letter" />
                    <Excel Format="ExcelML" />
                    <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                </ExportSettings>
                <MasterTableView DataKeyNames="CALLNO,COMPID" HierarchyLoadMode="ServerOnDemand"
                    Height="150px">
                    <DetailTables>
                        <telerik:GridTableView DataMember="Tasks" DataKeyNames="TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK"
                            NoDetailRecordsText="No Tasks available for this call" ShowHeadersWhenNoRecords="false"
                            AllowFilteringByColumn="false" AutoGenerateColumns="false" HierarchyLoadMode="ServerOnDemand"
                            AllowPaging="true" PageSize="10" Width="30%">
                            <DetailTables>
                                <telerik:GridTableView DataMember="TaskActions" DataKeyNames="AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_NU9_Action_Number"
                                    AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowHeadersWhenNoRecords="false"
                                    NoDetailRecordsText="No Actions available for this Task" AllowPaging="true" PageSize="10">
                                    <RowIndicatorColumn>
                                        <HeaderStyle Width="40px"></HeaderStyle>
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="False" Resizable="False">
                                        <HeaderStyle Width="40px"></HeaderStyle>
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="AM_NU9_Action_Number" DataType="System.Int32"
                                            HeaderText="Action No" ReadOnly="True" SortExpression="AM_NU9_Action_Number"
                                            UniqueName="AM_NU9_Action_Number" FilterListOptions="VaryByDataType" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="AM_VC_2000_Description" DataType="System.String"
                                            HeaderText="Description" ReadOnly="True" SortExpression="AM_VC_2000_Description"
                                            UniqueName="AM_VC_2000_Description" AllowFiltering="true" CurrentFilterFunction="Contains">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="UM_VC50_UserID" DataType="System.Int32" HeaderText="ActionOwner"
                                            ReadOnly="True" SortExpression="UM_VC50_UserID" UniqueName="UM_VC50_UserID" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="AM_DT8_Action_Date" DataType="System.DateTime"
                                            HeaderText="Action Date" ReadOnly="True" SortExpression="AM_DT8_Action_Date"
                                            UniqueName="AM_DT8_Action_Date">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="AM_FL8_Used_Hr" DataType="System.DateTime" HeaderText="Action hrs"
                                            ReadOnly="True" SortExpression="AM_FL8_Used_Hr" UniqueName="AM_FL8_Used_Hr">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <EditFormSettings>
                                        <PopUpSettings ScrollBars="None"></PopUpSettings>
                                    </EditFormSettings>
                                </telerik:GridTableView>
                            </DetailTables>
                            <Columns>
                                <telerik:GridBoundColumn DataField="TM_NU9_Call_No_FK" DataType="System.Int32" HeaderText="Call No"
                                    Visible="false" ReadOnly="True" SortExpression="TM_NU9_Call_No_FK" UniqueName="TM_NU9_Call_No_FK"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" FilterListOptions="VaryByDataType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TM_NU9_Comp_ID_FK" DataType="System.Decimal"
                                    HeaderText="Comp Id" ReadOnly="True" SortExpression="TM_NU9_Comp_ID_FK" UniqueName="TM_NU9_Comp_ID_FK"
                                    Visible="false" FilterListOptions="VaryByDataType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TM_NU9_Task_no_PK" DataType="System.String" HeaderText="Task No"
                                    ReadOnly="True" SortExpression="TM_NU9_Task_no_PK" UniqueName="TM_NU9_Task_no_PK"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" FilterListOptions="VaryByDataType">
                                </telerik:GridBoundColumn>
                                <%-- <telerik:GridBoundColumn DataField="CI_VC36_Name" DataType="System.String" HeaderText="CompId"
                                                                        ReadOnly="True" SortExpression="CI_VC36_Name" UniqueName="CI_VC36_Name" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="EqualTo" FilterListOptions="VaryByDataType">
                                                                    </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="UM_VC50_UserID" DataType="System.Int32" HeaderText="TaskOwner"
                                    ReadOnly="True" SortExpression="UM_VC50_UserID" UniqueName="UM_VC50_UserID" AutoPostBackOnFilter="true"
                                    CurrentFilterFunction="EqualTo" FilterListOptions="VaryByDataType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TM_VC50_Deve_status" DataType="System.Int32"
                                    HeaderText="TaskStatus" ReadOnly="True" SortExpression="TM_VC50_Deve_status"
                                    UniqueName="TM_VC50_Deve_status" ItemStyle-Wrap="true" FilterListOptions="VaryByDataType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TM_VC1000_Subtsk_Desc" DataType="System.Int32"
                                    HeaderText="TaskDesc" ReadOnly="True" SortExpression="TM_VC1000_Subtsk_Desc"
                                    UniqueName="TM_VC1000_Subtsk_Desc" FilterListOptions="VaryByDataType" CurrentFilterFunction="Contains">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TM_VC8_task_type" DataType="System.Int32" HeaderText="TType"
                                    ReadOnly="True" SortExpression="CM_VC8_Call_Type" UniqueName="CM_VC8_Call_Type">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EST_HOURS" DataType="System.Int32" HeaderText="EstHours"
                                    ReadOnly="True" SortExpression="EST_HOURS" UniqueName="EST_HOURS">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ACTUAL_HOURS" DataType="System.Int32" HeaderText="ActHours"
                                    ReadOnly="True" SortExpression="ACTUAL_HOURS" UniqueName="ACTUAL_HOURS">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EST_CLOSE_DATE" DataType="System.String" HeaderText="EstCloseDate"
                                    ReadOnly="True" SortExpression="EST_CLOSE_DATE" UniqueName="EST_CLOSE_DATE">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ACT_CLOSE_DATE" DataType="System.String" HeaderText="ActCloseDate"
                                    ReadOnly="True" SortExpression="ACT_CLOSE_DATE" UniqueName="ACT_CLOSE_DATE">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CALCULATE_HOURS" DataType="System.Int32" HeaderText="%HrsTaken"
                                    ReadOnly="True" SortExpression="CALCULATE_HOURS" HeaderStyle-Wrap="false" UniqueName="CALCULATE_HOURS">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <RowIndicatorColumn Visible="False">
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="False" Resizable="False">
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </ExpandCollapseColumn>
                            <EditFormSettings>
                                <PopUpSettings ScrollBars="None"></PopUpSettings>
                            </EditFormSettings>
                        </telerik:GridTableView>
                    </DetailTables>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="C" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgComm" runat="server" ImageUrl="../../Images/comment_Blank.gif" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="A" AllowFiltering="false">
                            <ItemTemplate>
                                &nbsp;
                                <asp:ImageButton runat="server" ID="imgAtt" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="False" Resizable="False">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <EditFormSettings>
                        <PopUpSettings ScrollBars="None"></PopUpSettings>
                    </EditFormSettings>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="Win_SimpleCallView" runat="server" Title="Simple Call Views"
                Height="520px" OnClientClose="RefreshPage" Width="500px" Left="50px" ReloadOnShow="true"
                Modal="true" Skin="Vista" VisibleStatusbar="false" Behavior="Close,Maximize,Minimize,Reload,Move">
            </telerik:RadWindow>
            <telerik:RadWindow ID="AttachmentsDialog" runat="server" Title="Call Attachments"
                Height="350px" Width="700px" Left="50px" ReloadOnShow="true" Modal="true" Skin="Vista"
                VisibleStatusbar="false" Behavior="Close,Maximize,Minimize,Reload,Move">
            </telerik:RadWindow>
            <telerik:RadWindow ID="CommentsDialog" runat="server" Title="Call Comments" Width="525px"
                OnClientClose="RefreshGrid" Height="560px" ReloadOnShow="true" Modal="true" Skin="Vista"
                VisibleStatusbar="false" Behavior="Close,Maximize,Minimize,Reload,Move">
            </telerik:RadWindow>
            <telerik:RadWindow ID="ShowCallReqByInfoDialog" runat="server" Title="User Information"
                Height="650px" Width="300px" Left="50px" ReloadOnShow="true" Modal="false" Skin="Vista"
                VisibleStatusbar="false" Behavior="Close,Maximize,Minimize,Reload,Move">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    </form>
</body>
</html>
