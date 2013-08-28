<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TaskViewTelerik.aspx.vb"
    Inherits="SupportCenter_CallView_TaskViewTelerik" %>

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
     var rand_no = Math.ceil(500 * Math.random())
        //Code to open ModelPopUpWindow for SimpleCallView related views
        function handleOnClick()
        
        {
            document.getElementById('btnGroupBy').click();
        
        }
        
        function ShowSimpleCallViewsForm() {
            window.radopen("../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=1024&TBLName=T040011", "Win_SimpleCallView");
            return false;
        }
        function OpenVW(varTable) 
        {
            wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrId=2212&TBLName=' + varTable, 'Search' + rand_no, 500, 450);
            return false;
        }
function wopen(url, name, w, h) {
            // Fudge factors for window decoration space.
            // In my tests these work well on all platforms & browsers.
            w += 32;
            h += 96;
            wleft = (screen.width - w) / 2;
            wtop = (screen.height - h) / 2;
            var win = window.open(url,
					name,
					'width=' + w + ', height=' + h + ', ' +
					'left=' + wleft + ', top=' + wtop + ', ' +
					'location=no, menubar=no, ' +
					'status=no, toolbar=no, scrollbars=yes, resizable=no');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }
        function confirm_Logout() {
            if (confirm("Are you sure you want to Logout WSS?") == true)
            {
                return true;
                }
            else
            {
                return false;}
        }

        function KeyCheck55(nn, rowvalues, tableID, Comp) {
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
                window.parent.OpenCallDetailInTab('Call# ' + nn, "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&CallNumber=" + nn + "&CompId=" + Comp , 'Call' + nn);
                //Form1.submit();
                return false;
            }


        }


        function RefreshGrid() {
            document.getElementById('hid').click();
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
        function OpenTab()
        {
           window.parent.OpenTabOnAddClick('Call Entry', "SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=-1" , "3");
        }
        
            function openWin()
        {
            var oWnd = radopen("../../Reports/TaskH.aspx", "RadWindow2");
              oWnd.setSize(900,500);
                    return false;

        }
        

    </script>

    <link rel="stylesheet" type="text/css" href="../../Images/Js/StyleSheet1.css" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; height: 500px">
    <form id="Form1" runat="server">
    <telerik:RadScriptManager ID="radscript" runat="server" AsyncPostBackTimeout="600">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="AjaxPanel" runat="server" EnableAJAX="true" LoadingPanelID="RadAjaxLoadingPanel1"
        ClientEvents-OnRequestStart="pnlRequestStarted">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td valign="top">
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
                                                        <asp:Label ID="lblTitleLabelInvDetail" runat="server" CssClass="TitleLabel">Task Hierarchy </asp:Label>
                                                    </td>
                                                    <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                        <center>
                                                            <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" OnClientClick="OpenTab()"
                                                                ImageUrl="../../Images/s2Add01.gif" Visible="false" AlternateText="New Call">
                                                            </asp:ImageButton>&nbsp;
                                                            <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" AlternateText="Search"
                                                                ImageUrl="../../Images/s1search02.gif"></asp:ImageButton>&nbsp;
                                                            <asp:ImageButton ID="imgCloseCall" AccessKey="C" runat="server" ImageUrl="../../Images/CloseCall1.gif"
                                                                Visible="false" AlternateText="Close Call"></asp:ImageButton>
                                                            <asp:ImageButton ID="imgMyCall" AccessKey="M" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                                Visible="false" ToolTip="My Calls"></asp:ImageButton>
                                                            <asp:ImageButton ID="imgExportToExcel" AccessKey="E" runat="server" ImageUrl="../../Images/Excel.jpg"
                                                                ToolTip="Export To Excel"></asp:ImageButton>
                                                            <asp:ImageButton ID="imgExportToWord" AccessKey="E" runat="server" ImageUrl="../../Images/save_dis999.gif"
                                                                ToolTip="Export To Word" Visible="false"></asp:ImageButton>
                                                            <img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;"
                                                                onclick="javascript:location.reload(true);" />
                                                            <img class="PlusImageCSS" id="Img1" title="ViewChart" onclick="openWin();" alt="E"
                                                                src="../../Images/graph_help.gif" border="0" name="tbrbtnEdit" />&nbsp;
                                                            <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                style="cursor: hand;" />
                                                        </center>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <font face="Verdana" size="1"><strong>View&nbsp;
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
                                <div style="overflow: auto; width: 100%; height: 500px">
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
                                                <telerik:RadGrid ID="GrdAddSerach" runat="server" AllowPaging="True" Font-Names="Verdana"
                                                    EnableEmbeddedSkins="true" BorderColor="Silver" BorderStyle="None" Skin="Office2007"
                                                    ShowHeader="true" BorderWidth="1px" GridLines="None" EnableTheming="true" Width="800px"
                                                    AllowSorting="True" PageSize="20" AllowFilteringByColumn="true" AutoGenerateColumns="false"
                                                    GroupPanel-Height="20px" GroupPanel-Font-Bold="false" ShowGroupPanel="True" GroupingEnabled="true">
                                                    <SelectedItemStyle BorderColor="Green" />
                                                    <GroupPanel runat="server" Width="1600px">
                                                        <PanelItemsStyle Width="1600px"/>
                                                        <PanelStyle Width="1600px" Height="0px" />
                                                    </GroupPanel>
                                                    <HeaderStyle Font-Bold="true" />
                                                    <SelectedItemStyle BackColor="#FFCC66" />
                                                    <ClientSettings AllowDragToGroup="true" AllowAutoScrollOnDragDrop="true" Selecting-AllowRowSelect="true">
                                                        <Selecting AllowRowSelect="true" />
                                                    </ClientSettings>
                                                    <PagerStyle Wrap="false" Mode="NextPrevAndNumeric" Font-Size="8pt" />
                                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                                                        <Pdf FontType="Subset" PaperSize="Letter" />
                                                        <Excel Format="ExcelML" />
                                                        <Csv ColumnDelimiter="Colon" RowDelimiter="NewLine" />
                                                    </ExportSettings>
                                                    <MasterTableView DataKeyNames="CallNo,CompID,TaskNo" HierarchyLoadMode="ServerOnDemand"
                                                        Height="150px" ShowGroupFooter="false" runat="server">
                                                        <DetailTables>
                                                            <telerik:GridTableView DataMember="TaskActions" DataKeyNames="AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_NU9_Action_Number"
                                                                NoDetailRecordsText="No Actions available for this call" ShowHeadersWhenNoRecords="false"
                                                                AllowFilteringByColumn="false" AutoGenerateColumns="false" HierarchyLoadMode="ServerOnDemand"
                                                                AllowPaging="true" PageSize="10" Width="30%" runat="server" >
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
                                                        <%-- <GroupByExpressions>
                                                            <telerik:GridGroupByExpression>
                                                                <SelectFields>
                                                                    <telerik:GridGroupByField FieldName="TaskOwner" />
                                                                    <telerik:GridGroupByField FieldName="Actual_Hours" Aggregate="Sum" HeaderText="Actual Hours  " />
                                                                    <telerik:GridGroupByField FieldName="EstHr" HeaderText="Est. Hrs " Aggregate="Sum" />
                                                                </SelectFields>
                                                                <GroupByFields>
                                                                    <telerik:GridGroupByField FieldName="TaskOwner" /> 
                                                                </GroupByFields>
                                                            </telerik:GridGroupByExpression>
                                                        </GroupByExpressions>--%>
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
    </telerik:RadAjaxPanel>
    <asp:Button ID="btnGroupBy" runat="server" Width="0px" Height="0px" BorderWidth="0px">
                                                </asp:Button>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
         <telerik:AjaxSetting AjaxControlID="btnGroupBy">
            <updatedcontrols>
                        <telerik:AjaxUpdatedControl ControlID="GrdAddSerach" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </updatedcontrols>
        </telerik:AjaxSetting>
        </AjaxSettings>
       
    </telerik:RadAjaxManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Sunset" />
    <telerik:RadWindowManager ID="RadWindowManager2" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="true" runat="server" Skin="Office2007">
        <Windows>
            <telerik:RadWindow ID="RadWindow2" Behavior="Close" runat="server" Animation="Resize"
                Width="800px" Height="500" Modal="true" Title="TaskHierarchy" NavigateUrl="../../Reports/TaskH.aspx">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="Win_SimpleCallView" runat="server" Title="Simple Call Views"
                Height="520px" Width="500px" Left="50px" ReloadOnShow="true" Modal="true" Skin="Vista"
                VisibleStatusbar="false" Behavior="Close,Maximize,Minimize,Reload,Move">
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
