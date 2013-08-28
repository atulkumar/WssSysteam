<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_StatusUDC_StatusUDCEdit, App_Web_o4bdz2ex" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Status UDC</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script language="javascript" src="../../DateControl/ION.js" type="text/javascript"></script>

    <script language="javascript" src="../../Images/Js/TaskViewShortCuts.js" type="text/javascript"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script type="text/javascript" language="javascript">


			var globleID;

								
				
			function SaveEdit(varImgValue)
				{
				
								if (varImgValue=='Ok')
								{
										document.Form1.txthiddenImage.value=varImgValue;
										Form1.submit(); 
										self.opener.Form1.submit(); 
										return false;
								}

							
								if (varImgValue=='Close')
								{
											window.close();	
								}
								
								if (varImgValue=='Save')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit();
									self.opener.Form1.submit(); 
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
					'status=no, toolbar=no, scrollbars=yes, resizable=no');
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
  
    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        background="../../images/top_nav_back.gif" border="0">
        <tr>
            <td>
                                 
                &nbsp;<asp:Label ID="lblTitleLabelStatusUDC" runat="server" BorderStyle="None" BorderWidth="2px"
                    Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal" Width="90px"
                    Height="12px">Status UDC</asp:Label>
            </td>
            <td align="center">
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../../Images/S2Save01.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ToolTip="OK" ImageUrl="../../Images/s1ok02.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ToolTip="Reset" ImageUrl="../../Images/reset_20.gif"
                    AlternateText="Reset"></asp:ImageButton>
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ToolTip="Close" ImageUrl="../../Images/s2close01.gif">
                </asp:ImageButton>
            </td>
            
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        border="0">
        <tr>
            <td valign="top">
                <cc1:CollapsiblePanel ID="cpnlError" runat="server" BorderStyle="Solid" BorderWidth="0px"
                    Width="100%" Height="47px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                    ExpandImage="../../Images/ToggleDown.gif" Text="Error Message" TitleBackColor="Transparent"
                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                    Visible="False" BorderColor="Indigo">
                    <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td colspan="0" rowspan="0">
                                <asp:Image ID="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../../Images/warning.gif">
                                </asp:Image>
                            </td>
                            <td colspan="0" rowspan="0">
                                <asp:ListBox ID="lstError" runat="server" Width="300px" ForeColor="Red" Font-Names="Verdana"
                                    Font-Size="XX-Small" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
            </td>
        </tr>
    </table>
    <table id="Table165" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        border="0">
        <tr>
            <td valign="top">
                <cc1:CollapsiblePanel ID="cpnlStatusUDC" runat="server" BorderStyle="Solid" BorderWidth="0px"
                    Width="100%" Height="500px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                    ExpandImage="../../Images/ToggleDown.gif" Text="Status UDC" TitleBackColor="Transparent"
                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                    Visible="True" BorderColor="Indigo">
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tr valign="top">
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblScreen" runat="server" Height="17px" Width="104px" ForeColor="DimGray"
                                    Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Screen Name</asp:Label><br>
                                <asp:TextBox ID="txtScreenName" runat="server" Height="18" Width="100" ReadOnly="True"
                                    CssClass="txtNoFocus"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label3" runat="server" Height="17px" Width="104px" ForeColor="DimGray"
                                    Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Company</asp:Label><br>
                                <asp:TextBox ID="txtCompany" runat="server" Height="18" Width="100" ReadOnly="True"
                                    CssClass="txtNoFocus"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label2" runat="server" Height="8px" Width="104px" ForeColor="DimGray"
                                    Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Status Name</asp:Label><br>
                                <asp:TextBox ID="txtStatusName" runat="server" Height="18" Width="100" CssClass="txtNoFocus"
                                    MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="top" rowspan="2">
                                <asp:Label ID="Label4" runat="server" Height="8px" Width="104px" ForeColor="DimGray"
                                    Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Description</asp:Label><br>
                                <asp:TextBox ID="txtDescription" runat="server" Height="80px" Width="272px" CssClass="txtNoFocus"
                                    MaxLength="10" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label5" runat="server" Height="8px" Width="104px" ForeColor="DimGray"
                                    Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small">Status Code</asp:Label><br>
                                <asp:TextBox ID="txtStatusCode" runat="server" Height="18" Width="100" CssClass="txtNoFocus"
                                    MaxLength="2"></asp:TextBox><br>
                                <br>
                                <br>
                                <br>
            
                            </td>
                            <td>
                               <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <input type="hidden" name="txthiddenImage">
        </ContentTemplate>
    </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
            
            </td>
        </tr>
    </table>
 
    </form>
</body>
</html>
