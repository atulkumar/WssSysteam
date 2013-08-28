<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Comment.aspx.vb" Inherits="SupportCenter_CallView_Comment" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Comment</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>
      <script type="text/javascript">
          //A Function to call on Page Load to set grid width according to screen size
          function onLoad() {
              var divCallView = document.getElementById('divCallView');
              if (document.body.clientWidth != 0) {
                  divCallView.style.width = document.body.clientWidth - 30 + "px";
              }
          }
          //A Function to improve design i.e delete the extra cell of table
          function onEnd() {
              var x = document.getElementById('cpnlTaskView_collapsible').cells[0].colSpan = "1";

          }
          //A Function is Called when we resize window
          window.onresize = onLoad;   
    </script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptMAnager1" runat="server">
    </asp:ScriptManager>

    <script language="Javascript">

        var rand_no = Math.ceil(500 * Math.random())
    function CheckLength() {
            var TDLength = document.getElementById('txtComment').value.length;
            if (TDLength > 0) {
                if (TDLength > 1000) {
                    alert('The Comment cannot be more than 1000 characters\n (Current Length :' + TDLength + ')');
                    return false;
                }
            }
            return true;
        }

    function UpperClose() {
            CloseWindow()
            window.close();
        }
    function CheckCommentTo() {
            if (document.getElementById('<%=CDDLCommentTo.ClientID %>').value == '') {
                if (window.confirm('You have not selected Comment To User! \n Do you want to continue?')) {
                    return true;
                }
            }
            else {
                return true;
            }
            return false;
        }

    function OpenW(a, b, c) {

            //window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
            wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c, 'Search' + rand_no, 500, 450);
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
					'status=no, toolbar=no, scrollbars=no, resizable=no');
            // Just in case width and height are ignored
            win.resizeTo(w, h);
            // Just in case left and top are ignored
            win.moveTo(wleft, wtop);
            win.focus();
        }

    function addToParentList(Afilename, TbName) {

            if (Afilename != "" || Afilename != 'undefined') {
                //alert(Afilename);
                document.getElementById(TbName).value = Afilename;
                aa = Afilename;
            }
            else {
                //document.Form1.txtAB_Type.value=aa;
            }
        }

    function CloseWindow() {
            self.opener.callrefresh();
        }

    function EmailList(mailID, UserID) {
            document.getElementById('txtSendMail').value = mailID;
            document.getElementById('txtHiddenID').value = UserID;
        }

    function SaveEdit(varImgValue) {
            
            if (varImgValue == 'Save') {
                //Security Block
                var obj = document.getElementById("imgSave")
                if (obj == null) {
                    alert("You don't have access rights to Save record");
                    return false;
                }

                if (obj.disabled == true) {
                    alert("You don't have access rights to Save record");
                    return false;
                }
                //End of Security Block
                if (CheckLength() == true) {
                    if (CheckCommentTo() == true) {
                        document.Form1.txthidden.value = varImgValue;
                        Form1.submit();
                    }
                }
                return false;
            }

            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset();
                    return false;
                }

            }

            if (varImgValue == 'Ok') {
                //Security Block
                var obj = document.getElementById("imgSave")
                if (obj == null) {
                    alert("You don't have access rights to Save record");
                    return false;
                }

                if (obj.disabled == true) {
                    alert("You don't have access rights to Save record");
                    return false;
                }
                //End of Security Block
                if (CheckLength() == true) {
                    if (CheckCommentTo() == true) {
                        document.Form1.txthidden.value = varImgValue;
                        Form1.submit();
                    }
                }
                return false;
            }


            if (varImgValue == 'Close') {
                //alert('<%=Session("CallPlus") %>');
                if ('<%=Request.QueryString("Page_name") %>' == 'Call_Heirarchy')
                 {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    oWindow.close();
                }
                else 
                {
                    self.opener.Form1.submit();
                    window.close();
                }
                return false;
            }


            if (varImgValue == 'EmailList') {
                //alert();
                var at;
                var Id;
                Id = '<%=Session("PropUserID") %>';  // document.getElementById('txtHiddenID').value;
                //alert(Id);
                at = document.getElementById('ddlntExt').options(document.getElementById('ddlntExt').options.selectedIndex).value;
                var comtype;
                comtype = document.getElementById('ddlntExt').options(document.getElementById('ddlntExt').options.selectedIndex).value;
                //alert(comtype);
                wopen('UserEmail_serach.aspx?ActionType=' + at + '&userId=' + Id + '&comtype=' + comtype + '&ID=C', 'UserEmail' + rand_no, 600, 550);
                document.getElementById('txtHiddenID').value = '';
                return false;

            }

            if (varImgValue == 'Edit') {
                document.Form1.txthidden.value = varImgValue;
                Form1.submit();
                return false;
            }

        }
        
    function KeyCheck(rowvalues) {
            var tableID = 'grdComment'  //your datagrids id
            var table;

            if (document.all) table = document.all[tableID];
            if (document.getElementById) table = document.getElementById(tableID);
            if (table) {

                for (var i = 1; i < table.rows.length; i++) {
                    if (i % 2 == 0) {
                        table.rows[i].style.backgroundColor = "#f5f5f5";
                    }
                    else {

                        table.rows[i].style.backgroundColor = "#ffffff";
                    }
                }
                table.rows[rowvalues + 1].style.backgroundColor = "#d4d4d4";
            }
        }

  function SetHeight() {
            //window.resizeTo(510,600);
        }
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
        function setFocus() {
            try {
                document.getElementById('txtComment').focus();
            }
            catch (e) {
            }
        }
    </script>

    <!--Added By Atul to make parent window Active after popup win close-->

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
            document.getElementById('txtComment').focus();
            SetHeight();
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                 if(opener.parent.LoadModalDiv)
                 opener.parent.LoadModalDiv();
            }
        }
        window.onload = OnLoad;
        window.onunload = OnClose;
    </script>

    <table width="100%" id="Table1" cellspacing="0" cellpadding="0" border="0" background="../../images/top_nav_back.gif">
        <tr>
              <td style="width: 15%">
                &nbsp;<asp:Label ID="lblTitleLabelComment" runat="server" CssClass="TitleLabel">Comment</asp:Label>&nbsp;&nbsp;
            </td>
            <td style="width: 75%; text-align: center;" nowrap="nowrap">
                <center>
                    
                    <asp:ImageButton ID="imgSave" runat="server" ImageUrl="../../Images/S2Save01.gif"
                        AccessKey="S" ToolTip="Save"></asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="imgOk" runat="server" ImageUrl="../../Images/s1ok02.gif" AccessKey="K"
                        ToolTip="Ok"></asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="imgReset" runat="server" ImageUrl="../../Images/reset_20.gif"
                        AccessKey="R" ToolTip="Reset"></asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="../../Images/s2close01.gif"
                        AccessKey="L" ToolTip="Close"></asp:ImageButton>
                </center>
            </td>
            <td nowrap="nowrap" style="width: 10%" background="../../images/top_nav_back.gif" height="47" >
                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('329','../../');"
                    alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                &nbsp;&nbsp;
            </td>
            <td>
                <table bordercolor="#5c5a5b" border="1" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td bordercolor="#f5f5f5">
                            <table border="0" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblName0" runat="server" CssClass="FieldLabel"> Call#</asp:Label><br>
                                        <asp:TextBox ID="txtCallNo" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                            Width="80px" BorderWidth="1px" BorderStyle="Solid" Height="18px" MaxLength="18"
                                            ReadOnly="True" CssClass="txtNoFocus"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="Label5" runat="server" CssClass="FieldLabel">Comment To</asp:Label><br>
                                        <telerik:RadComboBox ID="CDDLCommentTo" AllowCustomText="True" runat="server" Width="230px"
                                            Height="150px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Name"
                                            DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Select Comment To"
                                            EnableTextSelection="true" EnableVirtualScrolling="true">
                                        </telerik:RadComboBox>
                                       <%-- <uc1:CustomDDL ID="CDDLCommentTo" runat="server" Width="230px"></uc1:CustomDDL>--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">Int/Ext</asp:Label><br>
                                        <asp:DropDownList ID="ddlntExt" Height="18px" Width="70px" Font-Size="xx-small" Font-Name="vardana"
                                            runat="server">
                                            <asp:ListItem Value="External" Selected="True">External</asp:ListItem>
                                            <asp:ListItem Value="Internal">Internal</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="bottom">
                                        <asp:ImageButton ID="imgEmailList" runat="server" ImageUrl="../../Images/SendEmail.gif">
                                        </asp:ImageButton>
                                    </td>
                                    <td valign="bottom">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">Task#</asp:Label><br>
                                        <asp:TextBox ID="txtTaskNo" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                            Width="80px" BorderWidth="1px" BorderStyle="Solid" Height="18px" MaxLength="18"
                                            ReadOnly="True" CssClass="txtNoFocus"></asp:TextBox>
                                    </td>
                                    <td valign="top">
                                    </td>
                                    <td rowspan="2" valign="top" colspan="3">
                                        <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">Mail List</asp:Label><br>
                                        <asp:TextBox ID="txtSendMail" runat="server" CssClass="txtNoFocus" Height="53px"
                                            BorderStyle="Solid" BorderWidth="1px" Width="390px" Font-Size="XX-Small" Font-Names="Verdana"
                                            ReadOnly="False" MaxLength="950" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td rowspan="2" valign="middle">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">Action#</asp:Label><br>
                                        <asp:TextBox ID="txtActionNo" runat="server" CssClass="txtNoFocus" Height="18px"
                                            BorderStyle="Solid" BorderWidth="1px" Width="80px" Font-Names="Verdana" Font-Size="XX-Small"
                                            ReadOnly="True" MaxLength="18"></asp:TextBox>
                                    </td>
                                    <td valign="top">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table id="Table4" bordercolor="#f5f5f5"  cellspacing="0" cellpadding="0" bgcolor="#f5f5f5"
        border="1" >
        <tr>
            <td valign="top" bordercolor="#f5f5f5" align="left" width="12">
                &nbsp;
            </td>
            <td valign="top" align="left"  colspan="2">
                <asp:Label ID="lblName5" runat="server" CssClass="FieldLabel">Comment</asp:Label><br>
                <asp:TextBox ID="txtComment" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                    Width="480px" BorderWidth="1px" BorderStyle="Solid" Height="65px" MaxLength="950"
                    TextMode="MultiLine" CssClass="txtNoFocus"></asp:TextBox>
                <div id="divCallView"  style="overflow: scroll;width:480px; height: 100%">
                    <table id="tblComment" runat="server" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td >
                                <asp:DataGrid BorderWidth="0" ID="grdComment"   runat="server" CssClass="Grid" DataKeyField="CM_NU9_Comment_Number_PK"
                                    AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <HeaderStyle Font-Bold="True" Font-Names="Verdana" Font-Size="11px" BackColor="#e0e0e0" />
                                    <Columns>
                                        <asp:BoundColumn DataField="CM_NU9_Comment_Number_PK" Visible="False"></asp:BoundColumn>
                                        <asp:TemplateColumn>
                                            <HeaderTemplate>
                                                Read
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox Enabled="False" ID="chkRead" runat="server" Checked='<%#container.dataitem("CommentRead")%>'>
                                                </asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="Comment" HeaderText="Comment">
                                            <ItemStyle Width="350px" Wrap="True" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="WrittenBy" HeaderText="Written By">
                                            <ItemStyle Width="70px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CommentTo" HeaderText="Comment&#160;To">
                                            <ItemStyle Width="80px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CommentDate" HeaderText="Date/Time">
                                            <ItemStyle Width="150px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="InternalExternal" HeaderText="Type">
                                            <ItemStyle Width="50px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="MailSent" HeaderText="Mail Sent [List of Mail IDs]">
                                            <ItemStyle Width="1255px" Wrap="True" />
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="overflow: auto; width: 0px; height: 0px">
                    <span class="Abc" id="xyz" runat="server"></span>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden">
                        <input id="txtHiddenID" runat="server" name="txtHiddenID" type="hidden">
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
