<%@ page language="VB" autoeventwireup="false" inherits="Reimbursement_Default, App_Web_dipg-2cu" title="BillVerification" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>BillVerification</title>
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
     <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>
    <script language="javascript" type="text/javascript">
        var selecting = true;

        function RowSelecting(sender, args) {
            var id = args.get_id();
            var inputCheckBox = $get(id).getElementsByTagName("input")[0];
            if (!inputCheckBox || inputCheckBox.disabled) {
                //cancel selection for disabled rows  
                args.set_cancel(true);


            }

            // if no more unselected enabled rows left - check the header checkbox  

        }

        //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	
     
    </script>

    <style type="text/css">
        .style1
        {
            width: 171px;
        }
    </style>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <table id="Table1" style="height: 100%" cellspacing="0" cellpadding="0" width="100%"
        border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel">Bills Verification </asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" AlternateText="Save" ImageUrl="../../Images/S2Save01.gif"
                                                            ToolTip="Approved Bills"></asp:ImageButton>
                                                        <asp:ImageButton ID="ImgDisapproved" AccessKey="D" runat="server" AlternateText="Save"
                                                            ImageUrl="../../Images/s2delete01.gif" ToolTip="Disapproved Bills"></asp:ImageButton>
                                                        <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif"
                                                            ToolTip="Adjust Bills"></asp:ImageButton>
                                                        <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('4','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
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
                            <table>
                                <tr valign="top" align="left">
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblEmpName" runat="server" Text="Name Of Employee" CssClass="FieldLabel"></asp:Label><br />
                                                    <asp:DropDownList ID="ddlNameOfEmp" runat="server" CssClass="txtNoFocus" Width="120px"
                                                        AppendDataBoundItems="True" AutoPostBack="true">
                                                        <asp:ListItem Value="0">All</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                                                   
                                                <td class="style1">
                                                    <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="FieldLabel"></asp:Label><br />
                                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="txtNoFocus" Width="120px"
                                                        AutoPostBack="true">
                                                        <asp:ListItem Value="0">All</asp:ListItem>
                                                        <asp:ListItem Value="1">January</asp:ListItem>
                                                        <asp:ListItem Value="2">February</asp:ListItem>
                                                        <asp:ListItem Value="3">March</asp:ListItem>
                                                        <asp:ListItem Value="4">April</asp:ListItem>
                                                        <asp:ListItem Value="5">May</asp:ListItem>
                                                        <asp:ListItem Value="6">June</asp:ListItem>
                                                        <asp:ListItem Value="7">July</asp:ListItem>
                                                        <asp:ListItem Value="8">August</asp:ListItem>
                                                        <asp:ListItem Value="9">September</asp:ListItem>
                                                        <asp:ListItem Value="10">October</asp:ListItem>
                                                        <asp:ListItem Value="11">November</asp:ListItem>
                                                        <asp:ListItem Value="12">December</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="30px"></td>
                                            </tr>
                                            <tr>
                                                <td valign="bottom" colspan="3" >
                                                    <asp:RadioButton ID="rbtnApproved" CssClass="FieldLabel" runat="server" Text=" Show Approved"
                                                        GroupName="xx" AutoPostBack="true" />
                                                    <asp:RadioButton ID="rbtnNotApproved" CssClass="FieldLabel" runat="server" Text="Show Not Approved"
                                                        GroupName="xx" AutoPostBack="true" />
                                                    <asp:RadioButton ID="rbtDisapproved" CssClass="FieldLabel" runat="server" Text="Diapproved Bills"
                                                        GroupName="xx" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <table>
                                            <tr align="center">
                                                <td>
                                                    <telerik:RadGrid ID="rgvBillSubmitted" Skin="Office2007" Width="800px" Height="450px"
                                                        runat="server" AllowMultiRowSelection="True" AutoGenerateColumns="False" AllowPaging="True"
                                                        GridLines="None" ShowGroupPanel="True" PageSize="15" ShowFooter="true">
                                                        <MasterTableView DataKeyNames="ID" ShowGroupFooter="true" AllowAutomaticInserts="True">
                                                            <Columns>
                                                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn">
                                                                </telerik:GridClientSelectColumn>
                                                                <telerik:GridBoundColumn HeaderStyle-Width="200px" DataField="EmpName" HeaderText="Employee Name" UniqueName="EmpName">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ReimbursementType" HeaderText="ReimbursementType"
                                                                    UniqueName="ReimbursementType">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Month" HeaderText="Month" UniqueName="RBM_Month">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="BillSubmitted" Aggregate="Sum" HeaderText="BillSubmitted"
                                                                    UniqueName="BillSubmitted" FooterText="Total: ">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Verified" HeaderText="Verified" UniqueName="Verified"
                                                                    Visible="False">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="EMPID" HeaderText="EMPID" UniqueName="EMPID"
                                                                    Visible="False">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="RBMID" HeaderText="RBMID" UniqueName="RBMID"
                                                                    Visible="False">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="MonthID" HeaderText="MonthID" UniqueName="MonthID"
                                                                    Visible="False">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Year" HeaderText="Year" UniqueName="Year" Visible="False">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ID" HeaderText="ID" UniqueName="ID" Visible="False">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderText="BillAttachment">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkButton_ClearLine" runat="server" CommandName='<%# DataBinder.Eval(Container, "DataItem.BillFileName") %>'
                                                                            CommandArgument='<%#Eval("BillFilePath") %>' Text='<%# Eval("BillFileName") %>'>
                                               
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="BillFilePath" HeaderText="BillFilePath" UniqueName="BillFilePath"
                                                                    Visible="False">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn  HeaderText="Reason" UniqueName="Reason" DataField="Reason">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox runat="server" Text='<%#Eval("Reason")%>' ID="rntxtReason" MaxLength="50"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="BillStatusID" DataField="BillStatus" Visible="false" >
                                                                     <ItemTemplate>
                                                                         <asp:Label ID="BillStatus" runat="server" Visible="False" Text='<%# CType((iif(isdbnull(Eval("BillStatus")),0,Eval("BillStatus"))),Int32)%>'
                                                                            Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                                                                    </ItemTemplate>
                                                                   
                                                                </telerik:GridTemplateColumn>
                                                                  <telerik:GridTemplateColumn  HeaderText="Reason" UniqueName="ShowReason" DataField="Reason">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("Reason")%>' ID="rnlblReason" ></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                            <GroupByExpressions>
                                                                <telerik:GridGroupByExpression>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldName="EmpName" />
                                                                    </GroupByFields>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldName="EmpName" HeaderText="Employee Name" />
                                                                    </SelectFields>
                                                                </telerik:GridGroupByExpression>
                                                            </GroupByExpressions>
                                                        </MasterTableView>
                                                        <PagerStyle AlwaysVisible="true" />
                                                        <ClientSettings AllowDragToGroup="True" Scrolling-AllowScroll="true ">
                                                        </ClientSettings>
                                                        <ClientSettings EnableRowHoverStyle="True" AllowDragToGroup="True">
                                                            <Selecting AllowRowSelect="True" />
                                                            <ClientEvents OnRowSelecting="RowSelecting" />
                                                        </ClientSettings>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <AlternatingItemStyle HorizontalAlign="Left" />
                                                    </telerik:RadGrid>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <!--  **********************************************************************-->
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="100px" Font-Size="XX-Small" Font-Names="Verdana"
                            BorderWidth="0" BorderStyle="Groove" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
