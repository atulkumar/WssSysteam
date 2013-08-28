<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BillHistory.aspx.vb" Inherits="Finance_Reimbursment_BillHistory" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>BillHistory</title>
    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
       <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>
</head>
<body>

    <script language="javascript" type="text/javascript">
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog         
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow; //IE (and Moz as well)         
            return oWindow;
        }

        function Close() {
            GetRadWindow().Close();
        }
        function UpdateCommand() {
            document.getElementById('ValueEvent').value = 'Update';
            document.getElementById('ValueBillID').value = document.getElementById('valueID').value;
        }
        function NumericOnly() {
            //		alert(event.keyCode);
            if ((event.keyCode < 13 || event.keyCode > 13) && (event.keyCode < 48 || event.keyCode > 57)) {
                event.returnValue = false;
                alert("Please Enter Numerics Only");
            }
        }
    </script>

    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
        <table>
            <tr>
                <td>
                    <asp:RadioButton ID="rbtnApproved" CssClass="FieldLabel" runat="server" Text=" Show Approved"
                        GroupName="xx" AutoPostBack="true" />
                    <asp:RadioButton ID="rbtnNotApproved" CssClass="FieldLabel" runat="server" Checked="true"
                        Text="Show Not Approved" GroupName="xx" AutoPostBack="true" />
                    <asp:RadioButton ID="rbtnDisapproved" CssClass="FieldLabel" runat="server" Text="Disapproved Bills"
                        GroupName="xx" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="rgvBillSubmitted" Skin="Office2007" Width="600px" Height="480px"
                        runat="server" AutoGenerateColumns="False" AllowPaging="True" GridLines="None"
                        ShowGroupPanel="True" PageSize="15" ShowFooter="true" AutoGenerateEditColumn="true">
                        <MasterTableView EditMode="PopUp" ShowGroupFooter="true">
                            <Columns>
                                <telerik:GridBoundColumn DataField="Month" HeaderText="Month" UniqueName="RBM_Month">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ReimbursementType" HeaderText="ReimbursementType"
                                    UniqueName="ReimbursementType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BillSubmitted" Aggregate="Sum" HeaderText="BillSubmitted"
                                    UniqueName="BillSubmitted" FooterText="Total: ">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Verified" HeaderText="Verified" UniqueName="Verified"
                                    Visible="False">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="BillAttachment">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_ClearLine" runat="server" CommandName='<%# DataBinder.Eval(Container, "DataItem.BillFileName") %>'
                                            CommandArgument='<%#Eval("BillFilePath") %>' Text='<%# Eval("BillFileName") %>'>
                                               
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 <telerik:GridTemplateColumn HeaderText="Reason" UniqueName="ShowReason" DataField="Reason"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Reason")%>' ID="rnlblReason"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                   <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_Delete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("ID") %>'
                                            Text="Delete" OnClientClick="return confirm('Are You sure you want to delete this Bill?')">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ID" HeaderText="ID" UniqueName="ID" Visible="False">
                                </telerik:GridBoundColumn>
                               
                               
                            </Columns>
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="ReimbursementType" />
                                    </GroupByFields>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="ReimbursementType" HeaderText="ReimbursementType" />
                                    </SelectFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <EditFormSettings CaptionDataField="ID" EditFormType="Template" CaptionFormatString="<b>You are Editing ID : {0}</b>">
                                <FormTemplate>
                                    <table id="Table212" border="0" rules="none" style="border-collapse: collapse; background: white;">
                                        <tr>
                                            <td colspan="2" style="font-size: small; width: 40%">
                                                Bill Submitted
                                            </td>
                                            <td colspan="2" style="font-size: small; width: 40%">
                                            </td>
                                            <td colspan="2" style="font-size: small; width: 60%">
                                                <asp:TextBox ID="description" MaxLength="5"  runat="server" onkeypress="NumericOnly();" Width="200px"
                                                    Text='<%# Bind( "BillSubmitted" ) %>'></asp:TextBox>
                                                <input type="hidden" value='<%#Eval("ID") %>' id="valueID" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="font-size: small; width: 40%">
                                                File Name:
                                            </td>
                                            <td colspan="2" style="font-size: small; width: 40%">
                                            </td>
                                            <td colspan="2" style="font-size: small; width: 60%">
                                                <telerik:RadUpload ID="fupolicyfile1" ControlObjectsVisibility="None" EnableEmbeddedSkins="false"
                                                    EnableFileInputSkinning="false" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: right; font-size: small;">
                                                <asp:Button ID="btnUpdate" Text="Update" runat="server" OnClick="abc" OnClientClick="UpdateCommand()"
                                                    CommandName="Update" />
                                            </td>
                                            <td colspan="2" style="font-size: small">
                                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClientClick="Close()" CommandName="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </FormTemplate>
                            </EditFormSettings>
                        </MasterTableView>
                        <PagerStyle AlwaysVisible="true" Mode="NextPrev" />
                        <ClientSettings AllowDragToGroup="True" Scrolling-AllowScroll="true ">
                        </ClientSettings>
                        <ClientSettings EnableRowHoverStyle="True" AllowDragToGroup="True">
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                        <ItemStyle HorizontalAlign="Left" />
                        <AlternatingItemStyle HorizontalAlign="Left" />
                    </telerik:RadGrid>
                </td>
            </tr>
            <asp:UpdatePanel ID="PanelUpdate" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlMsg" runat="server">
                    </asp:Panel>
                    <asp:ListBox ID="lstError" runat="server" Width="100px" Font-Size="XX-Small" Font-Names="Verdana"
                        BorderWidth="0" BorderStyle="Groove" ForeColor="Red" Visible="false"></asp:ListBox>
                    <input type="hidden" id="ValueEvent" name="ValueEvent" />
                    <input type="hidden" id="ValueBillID" name="ValueBillID" />
                    <input type="hidden" id="valueFileUpload" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </table>
    </div>
    </form>
</body>
</html>
