<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FolderSettings.aspx.vb"
    Inherits="DocumentsMgt_FolderSettings" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Folder Settings</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <%--<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">--%>

    <script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../Images/Js/JSValidation.js"></script>

    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script language="Javascript" type="text/javascript">

        var rand_no = Math.ceil(500 * Math.random())
        function OpenW(a, b, c) {
            wopen('../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode=' + a + '  and UDCType=' + "'" + b + "'" + ' &tbname=' + c, 'Common' + rand_no, 500, 450);
        }

        function OpenComm(a, b) {
            wopen('comment.aspx?ScrID=329&ID=' + b + '&tbname=T', 'Comments' + rand_no, 500, 450);
        }

        function OpenAtt() {
            wopen('../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=T', 'Additional_Address' + rand_no, 400, 450);
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


        function addToParentList(Afilename, TbName, strName) {

            if (Afilename != "" || Afilename != 'undefined') {
                varName = TbName + 'Name'
                //alert(Afilename);
                document.getElementById(TbName).value = Afilename;
                document.getElementById(varName).value = strName;
                aa = Afilename;
            }
            else {
                document.Form1.txtAB_Type.value = aa;
            }
        }

        function CloseWindow() {
            self.opener.location = 'FolderMgtMaster.aspx';
            //self.opener.callrefresh();
        }

        function SaveEdit(varImgValue) {
            if (varImgValue == 'Close') {
                //self.opener.Form1.reload();
                //window.close(); 
                document.Form1.txthiddenImage.value = varImgValue;
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
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;

            }

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

                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
                return false;
            }

            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset()
                }
                return false;
            }

        }

        function callrefresh() {
            document.Form1.txthiddenImage.value = '';
            Form1.submit();
        }

        function FP_swapImg() {//v1.0
            var doc = document, args = arguments, elm, n; doc.$imgSwaps = new Array(); for (n = 2; n < args.length;
							n += 2) {
                elm = FP_getObjectByID(args[n]); if (elm) {
                    doc.$imgSwaps[doc.$imgSwaps.length] = elm;
                    elm.$src = elm.src; elm.src = args[n + 1];
                } 
            }
        }

        function FP_preloadImgs() {//v1.0
            var d = document, a = arguments; if (!d.FP_imgs) d.FP_imgs = new Array();
            for (var i = 0; i < a.length; i++) { d.FP_imgs[i] = new Image; d.FP_imgs[i].src = a[i]; }
        }

        function FP_getObjectByID(id, o) {//v1.0
            var c, el, els, f, m, n; if (!o) o = document; if (o.getElementById) el = o.getElementById(id);
            else if (o.layers) c = o.layers; else if (o.all) el = o.all[id]; if (el) return el;
            if (o.id == id || o.name == id) return o; if (o.childNodes) c = o.childNodes; if (c)
                for (n = 0; n < c.length; n++) { el = FP_getObjectByID(id, c[n]); if (el) return el; }
            f = o.forms; if (f) for (n = 0; n < f.length; n++) {
                els = f[n].elements;
                for (m = 0; m < els.length; m++) { el = FP_getObjectByID(id, els[n]); if (el) return el; } 
            }
            return null;
        }



        function CheckLength() {
            try {

                var CDLength = document.getElementById('txtdescription').value.length;


                if (CDLength > 0) {
                    if (CDLength > 5000) {
                        alert('The Description cannot be more than 5000 characters\n (Current Length :' + CDLength + ')');
                        return false;
                    }
                }


            }
            catch (e) {
                return true;
            }
            return true;
        }		
    </script>

    <script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if (opener.parent.HideModalDiv)
                    opener.parent.HideModalDiv();
            }
        }
        //Modified By Atul to execute script on Page Load
        function OnLoad() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if (opener.parent.LoadModalDiv)
                    opener.parent.LoadModalDiv();
            }
        }
        window.onload = OnLoad;
        window.onunload = OnClose;
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">

    <script language="javascript" type="text/javascript">
        function OpenAB(c) {
            var compType = '<%=session("propCompanyType")%>';
            var compID = '<%=session("propCompanyID")%>';
            var strQuery;

            strQuery = 'SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where um_in4_address_no_fk<>' + '<%=Session("PropTaskOwnerID")%>' + ' and ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation=' + "'SCM'" + ' ))';

            /*
            if (compType=='SCM')
            {
            strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id ';
            }
            else
            {
            strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id='+compID+'  or um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='+"'SCM'"+' ))';
            }
            */
            wopen('../Search/Common/PopSearch.aspx?ID=' + strQuery + '&tbname=' + c, 'Search' + rand_no, 500, 450);

        }
    </script>

    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../images/top_nav_back.gif"
        border="0">
        <tr>
            <td style="width: 35%">
                <asp:Label ID="lblTitleLabelTaskFwd" runat="server" CssClass="TitleLabel">Folder Properties</asp:Label>
            </td>
            <td style="width: 60%; text-align: center;" nowrap="nowrap" bordercolor="#e0e0e0"
                bordercolorlight="#e0e0e0" bordercolordark="#e0e0e0">
                <center>
                    <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../Images/S2Save01.gif">
                    </asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="imgOK" AccessKey="K" runat="server" ToolTip="OK" ImageUrl="../Images/s1ok02.gif">
                    </asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="imgClose" AccessKey="O" runat="server" ToolTip="Close" ImageUrl="../Images/s2close01.gif">
                    </asp:ImageButton>
                </center>
            </td>
            <td nowrap="nowrap" style="width: 5%" background="../images/top_nav_back.gif" height="47">
                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('995','../');"
                    alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">
            </td>
        </tr>
    </table>
    <table id="Table4" style="width: 500px; height: 232px" bordercolor="#f5f5f5" cellspacing="0"
        cellpadding="0" bgcolor="#f5f5f5" border="1">
        <tr>
            <td style="width: 18px; height: 21px">
                &nbsp;
            </td>
            <td style="width: 184px; height: 21px" bordercolor="#f5f5f5">
                <asp:Label ID="lblCompany" runat="server" CssClass="FieldLabel">Company</asp:Label>
            </td>
            <td style="width: 193px; height: 21px" bordercolor="#f5f5f5" colspan="0">
                <asp:Label ID="Label1" runat="server" CssClass="FieldLabel" Height="19px" Width="102px">Parent Folder</asp:Label>
                <td style="width: 181px; height: 21px" bordercolor="#f5f5f5" colspan="0">
                    <asp:Label ID="Label2" runat="server" CssClass="FieldLabel" Height="19px" Width="102px">Folder Name</asp:Label>
                </td>
        </tr>
        <tr>
            <td style="width: 18px; height: 9px" valign="middle" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td style="width: 184px; height: 9px" valign="middle" bordercolor="#f5f5f5" align="left">
                <asp:TextBox ID="txtCompany" runat="server" CssClass="txtNoFocus" Height="18px" BorderStyle="Solid"
                    BorderWidth="1px" Width="145px" Font-Names="Verdana" Font-Size="XX-Small" ReadOnly="True"></asp:TextBox>
            </td>
            <td style="height: 9px" valign="middle" bordercolor="#f5f5f5" align="left" width="193">
                <asp:TextBox ID="txtParentFolder" runat="server" CssClass="txtNoFocus" Height="18px"
                    BorderStyle="Solid" BorderWidth="1px" Width="157px" Font-Names="Verdana" Font-Size="XX-Small"
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td style="width: 181px; height: 9px" bordercolor="#f5f5f5" colspan="0">
                <asp:TextBox ID="txtFolderName" runat="server" CssClass="txtNoFocus" Height="18px"
                    BorderStyle="Solid" BorderWidth="1px" Width="160px" Font-Names="Verdana" Font-Size="XX-Small"
                    ReadOnly="True" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 18px" valign="middle" bordercolor="#f5f5f5" align="left" height="46">
                &nbsp;
            </td>
            <td valign="middle" bordercolor="#f5f5f5" align="left" colspan="3" height="46">
                <asp:Label ID="lblName7" runat="server" CssClass="FieldLabel" Height="18px" Width="103px">Folder Path</asp:Label><br>
                <asp:TextBox ID="txtFolderPath" runat="server" CssClass="txtNoFocus" Height="18px"
                    BorderStyle="Solid" BorderWidth="1px" Width="548px" Font-Names="Verdana" Font-Size="XX-Small"
                    ReadOnly="True" MaxLength="9"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 18px" valign="middle" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td style="width: 246px" bordercolor="#f5f5f5" colspan="3">
                <asp:Label ID="Label3" runat="server" CssClass="FieldLabel" Height="19px" Width="100px">Description</asp:Label><br>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="txtNoFocus" Height="80px"
                    BorderStyle="Solid" BorderWidth="1px" Width="547px" Font-Names="Verdana" Font-Size="XX-Small"
                    MaxLength="5000" TextMode="MultiLine"></asp:TextBox>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <!-- Image Clicked-->
    <input type="hidden" name="txthiddenImage">
    </form>
</body>
</html>
