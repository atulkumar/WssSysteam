<%@ page language="VB" autoeventwireup="false" inherits="DocumentsMgt_FolderPermissions, App_Web_ci55pbfh" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Folder Permissions</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../Images/Js/JSValidation.js"></script>

    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../calendar/popcalendar.js"></script>

    <script language="Javascript">
		
	var rand_no = Math.ceil(500*Math.random())	
	

	function checkAllow(AID,DID)
			{
			    if ( document.getElementById(AID).checked==true )
				{
					document.getElementById(DID).checked=false;
				}
				if ( document.getElementById(AID).checked==false )
				{
					document.getElementById(DID).checked=true;
				}
			}
	function checkDeny(AID,DID)
			{
				if ( document.getElementById(DID).checked==true )
				{
					document.getElementById(AID).checked=false;
				}
				if ( document.getElementById(DID).checked==false )
				{
					document.getElementById(AID).checked=true;
				}
			}
					
			function SaveEdit(varImgValue)
				{			    														
					if (varImgValue=='Close')
						{
							window.close(); 
							return false;
						}																
															
					if (varImgValue=='Edit')
				          {
				          	document.Form1.txthiddenImage.value=varImgValue;
							wopen('PermissionPopUp.aspx','PermissionPopUp'+rand_no,400,500);	
							 return false;
						  }											
					if (varImgValue=='Ok')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
				            //window.close(); 
						    return false;
						}
								
					if (varImgValue=='Save')
						{
						   document.Form1.txthiddenImage.value=varImgValue;
							document.Form1.submit(); 
							return false;
						}		
							
					if (varImgValue=='Reset')
						{
							var confirmed
							confirmed=window.confirm("Do You Want To reset The Page ?");
							if(confirmed==true)
								{	
									Form1.reset()
								}	
							return false;		
						}		
							
						
				}			
					
			function KeyCheck(ID)
			{
					document.Form1.HIDPermID.value=ID;
					//document.Form1.HIDIONCode.value=IONCode;
			}			
		
		
			function callrefresh()
				{
					document.Form1.txthiddenImage.value='';
			    	Form1.submit();
				}
				
			function wopen(url, name, w, h)
				{
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

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../images/top_nav_back.gif"
        border="0">
        <tr>
            <td style="width: 35%">
                <asp:Label ID="lblTitleLabelTaskFwd" runat="server" CssClass="TitleLabel">Folder Permissions</asp:Label>
            </td>
            <td style="width: 60%; text-align: center;" nowrap="nowrap" bordercolor="#e0e0e0"
                bordercolorlight="#e0e0e0" bordercolordark="#e0e0e0">
                <center>
                    <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../Images/S2Save01.gif">
                    </asp:ImageButton>
                    <asp:ImageButton ID="imgOK" AccessKey="K" runat="server" ToolTip="OK" ImageUrl="../Images/s1ok02.gif">
                    </asp:ImageButton>
                    <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ToolTip="Reset" ImageUrl="../Images/reset_20.gif">
                    </asp:ImageButton>
                    <asp:ImageButton ID="imgClose" AccessKey="O" runat="server" ToolTip="Close" ImageUrl="../Images/s2close01.gif">
                    </asp:ImageButton>
                </center>
            </td>
            <td nowrap="nowrap" style="width: 5%" background="../images/top_nav_back.gif" height="47">
                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('1005','../');"
                    alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">
            </td>
        </tr>
    </table>
    <table id="Table4" style="width: 600px; height: 232px" bordercolor="#f5f5f5" cellspacing="0"
        cellpadding="0" width="600" bgcolor="#f5f5f5" border="1">
        <tr>
            <td style="width: 18px; height: 21px">
                &nbsp;
            </td>
            <td style="width: 184px; height: 21px" bordercolor="#f5f5f5">
                <asp:Label ID="lblCompany" runat="server" CssClass="FieldLabel" Height="19px" Width="58px">Folder</asp:Label>
            </td>
            <td style="width: 46px; height: 21px" bordercolor="#f5f5f5" colspan="0">
                <td style="width: 135px; height: 21px" bordercolor="#f5f5f5" colspan="0">
                </td>
        </tr>
        <tr>
            <td style="width: 18px; height: 9px" valign="middle" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td style="width: 184px; height: 9px" valign="middle" bordercolor="#f5f5f5" align="left">
                <asp:TextBox ID="txtFolderName" runat="server" CssClass="txtComoany" Height="18px"
                    BorderStyle="Solid" BorderWidth="1px" Width="145px" Font-Size="XX-Small" Font-Names="Verdana"
                    ReadOnly="True" MaxLength="9"></asp:TextBox>
            </td>
            <td style="height: 9px" valign="middle" bordercolor="#f5f5f5" align="left" width="46">
            </td>
            <td style="width: 135px; height: 9px" bordercolor="#f5f5f5" colspan="0">
            </td>
        </tr>
        <tr>
            <td style="width: 18px" valign="middle" bordercolor="#f5f5f5" align="left" height="46">
                &nbsp;
            </td>
            <td valign="middle" bordercolor="#f5f5f5" align="left" colspan="3" height="46">
                <asp:Label ID="Label2" runat="server" CssClass="FieldLabel" Height="19px" Width="136px">Company and Role</asp:Label><br>
                <div id="grid" style="overflow: auto; width: 340px; border-top-style: ridge; border-right-style: ridge;
                    border-left-style: ridge; height: 150px; border-bottom-style: ridge">
                    <asp:DataGrid ID="gridPermissions" runat="server" Width="140px" ShowHeader="False"
                        GridLines="None" CellPadding="0" CellSpacing="0" AutoGenerateColumns="False"
                        DataKeyField="PermID">
                        <ItemStyle Font-Name="Verdana" Font-Size="8"></ItemStyle>
                        <Columns>
                            <asp:ButtonColumn Visible="False" Text="SSSS" CommandName="Select"></asp:ButtonColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:Image ID="imgComm" runat="server" ImageUrl='<%#container.dataitem("Image")%>'>
                                    </asp:Image>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Width="296px" Text='<%#container.dataitem("AssignTO")%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid></div>
            </td>
        </tr>
        <tr>
            <td style="width: 18px" valign="middle" bordercolor="#f5f5f5" align="left">
            </td>
            <td style="width: 369px" bordercolor="#f5f5f5" colspan="3" valign="bottom">
                <asp:Label ID="lblCompany0" runat="server" CssClass="FieldLabel" Height="19px" Width="189px">Permission on Folder</asp:Label>
                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="72px"></asp:Button>
                <asp:Button ID="btnRemove" runat="server" Text="Remove" Width="72px"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="width: 18px" valign="middle" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td style="width: 369px" bordercolor="#f5f5f5" colspan="3">
                <asp:DataGrid ID="gridPermType" runat="server" BorderStyle="Ridge" GridLines="None"
                    CellPadding="0" CellSpacing="0" AutoGenerateColumns="False" DataKeyField="PermTypeID">
                    <HeaderStyle Font-Bold="True" ForeColor="Black" Font-Name="Verdana" Font-Size="8"
                        BorderStyle="Solid"></HeaderStyle>
                    <ItemStyle Font-Name="Verdana" Font-Size="8"></ItemStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderStyle-Width="250px" HeaderText="Permission Type">
                            <ItemTemplate>
                                <asp:Label ID="lblIONCode" runat="server" Width="250px" Text='<%#container.dataitem("PermissionType")%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-Width="40px" HeaderText="Allow">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAllow" runat="server" Checked='<%#container.dataitem("Allow")%>'>
                                </asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-Width="40px" HeaderText="Deny">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDeny" runat="server" Checked='<%#container.dataitem("PDeny")%>'>
                                </asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthiddenImage"><input type="hidden" name="HIDPermID"
                            runat="server" id="HIDPermID">
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
