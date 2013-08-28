<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_BasicMonitorEdit, App_Web_y4bhs5yb" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>basicMonitorEdit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<script language="Javascript">
		
			function SaveEdit(varImgValue)
				{
			
											
					if (varImgValue=='Close')
						{
							window.close();
						}								
					
					if (varImgValue=='Ok')
						{
						
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 
								self.opener.Form1.submit(); 
													
						}
							
					if (varImgValue=='Save')
						{	
							document.Form1.txthiddenImage.value=varImgValue;
							//alert(varImgValue);
							Form1.submit(); 
							self.opener.Form1.submit(); 
						}		
							
					if (varImgValue=='Reset')
						{
							var confirmed
							confirmed=window.confirm("Do You Want To reset The Page ?");
							if(confirmed==true)
								{	
									Form1.reset()
								}		
						}			
					}			
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				background="../images/top_nav_back.gif" border="0">
				<TR>
					<TD width="145"><asp:label id="lblTitleLabelAlertFlowEdit" runat="server" Font-Size="X-Small" Font-Names="Verdana"
							Font-Bold="True" ForeColor="Teal" BorderStyle="None" BorderWidth="2px" Width="160px">&nbsp;BasicMonitor Edit</asp:label></TD>
					<TD><IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
						<IMG class="PlusImageCSS" title="Save" onclick="SaveEdit('Save');" alt="" src="../Images/S2Save01.gif"
							border="0" name="tbrbtnSave">&nbsp; <IMG class="PlusImageCSS" id="Ok" title="Ok" onclick="SaveEdit('Ok');" alt="" src="../Images/s1ok02.gif"
							border="0" name="tbrbtnOk">&nbsp; <IMG class="PlusImageCSS" id="Reset" title="Reset" onclick="SaveEdit('Reset');" alt="R"
							src="../Images/reset_20.gif" border="0" name="tbrbtnReset">&nbsp; <IMG class="PlusImageCSS" id="Close" title="Close" onclick="SaveEdit('Close');" alt=""
							src="../Images/s2close01.gif" border="0" name="tbrbtnClose">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0"></TD>
					<td style="WIDTH: 10px" width="10" background="../images/top_nav_back01.gif" height="67">&nbsp;</td>
				</TR>
			</TABLE>
			<cc1:collapsiblepanel id="cpnlError" runat="server" BorderStyle="Solid" BorderWidth="0px" Width="100%"
				Height="81px" BorderColor="Indigo" Visible="False" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
				ExpandImage="../Images/ToggleDown.gif" Text="Message" TitleBackColor="Transparent" TitleClickable="True"
				TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
				<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="272"
					border="0">
					<TR>
						<TD colSpan="0" rowSpan="0">
							<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../../icons/warning.gif"></asp:Image></TD>
						<TD colSpan="0" rowSpan="0">
							<asp:ListBox id="lstError" runat="server" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"
								BorderStyle="Groove" BorderWidth="0" Width="420px" Height="40px"></asp:ListBox></TD>
					</TR>
				</TABLE>
			</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlReport" runat="server" BorderStyle="Solid" BorderWidth="0px" Width="100%"
				BorderColor="Indigo" Visible="true" Draggable="False" CollapseImage="../Images/white.gif" ExpandImage="../Images/white.gif"
				Text="Edit" TitleBackColor="Transparent" TitleClickable="false" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
				<TABLE id="Table4" borderColor="#f5f5f5" cellSpacing="5" cellPadding="0" bgColor="#f5f5f5"
					border="1">
					<TR>
						<TD vAlign="top" borderColor="#f5f5f5" align="left" rowSpan="1">&nbsp;</TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left" rowSpan="1">
							<asp:label id="lblDBType" runat="server" Width="120px" CssClass="FieldLabel">Type</asp:label><BR>
							<asp:TextBox id="TxtDiskType" runat="server" Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid"
								BorderWidth="1px" Width="129px" Height="18px" CssClass="txtNoFocus" MaxLength="8" ReadOnly="True"></asp:TextBox></TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left">
							<asp:label id="lblDBName" runat="server" Width="85px" CssClass="FieldLabel">Drive Name</asp:label><BR>
							<asp:TextBox id="TxtDriveName" runat="server" Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid"
								BorderWidth="1px" Width="129px" Height="18px" CssClass="txtNoFocus" MaxLength="8" ReadOnly="True"></asp:TextBox></TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left" rowSpan="1">
							<asp:label id="lblStart" runat="server" CssClass="FieldLabel">Start Date</asp:label><BR>
							<asp:textbox id="txtStartDate" runat="server" Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid"
								BorderWidth="1px" Width="129px" Height="18px" CssClass="txtNoFocus" MaxLength="8" ReadOnly="True"></asp:textbox></TD>
					</TR>
					<TR>
						<TD vAlign="top" borderColor="#f5f5f5" align="left">&nbsp;</TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left">
							<asp:label id="lblEnd" runat="server" Width="85px" CssClass="FieldLabel">End Date</asp:label><BR>
							<asp:TextBox id="TxtEndDate" runat="server" Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid"
								BorderWidth="1px" Width="129px" Height="18px" CssClass="txtNoFocus" MaxLength="8" ReadOnly="True"></asp:TextBox></TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left">
							<asp:label id="lblTime" runat="server" Width="85px" CssClass="FieldLabel">Time</asp:label><BR>
							<asp:TextBox id="TxtTime" runat="server" Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid"
								BorderWidth="1px" Width="129px" Height="18px" CssClass="txtNoFocus" MaxLength="8" ReadOnly="True"></asp:TextBox></TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left">
							<asp:label id="LblStatus" runat="server" Width="85px" CssClass="FieldLabel">Status</asp:label><BR>
							<asp:DropDownList id="DdlStatus" runat="server" Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid"
								BorderWidth="1px" Width="129px" Height="18px" CssClass="txtNoFocus">
								<asp:ListItem></asp:ListItem>
								<asp:ListItem Value="P">P</asp:ListItem>
								<asp:ListItem Value="H">H</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD vAlign="top" borderColor="#f5f5f5" align="left">&nbsp;</TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left">
							<asp:label id="lblReason" runat="server" Width="40px" CssClass="FieldLabel">Reason</asp:label></TD>
					</TR>
					<TR>
						<TD vAlign="top" borderColor="#f5f5f5" align="left">&nbsp;</TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left" colSpan="3">
							<asp:textbox id="txtReason" runat="server" Font-Size="XX-Small" Font-Names="Verdana" BorderStyle="Solid"
								BorderWidth="1px" Width="430px" Height="80px" CssClass="txtNoFocus" MaxLength="150" TextMode="MultiLine"></asp:textbox><BR>
						</TD>
						<TD vAlign="top" borderColor="#f5f5f5" align="left"></TD>
					</TR>
				</TABLE>
			</cc1:collapsiblepanel><INPUT type="hidden" name="txthidden"> <!--Address Nuimber --><INPUT type="hidden" name="txthiddenImage"><!-- Image Clicked-->
			<INPUT type="hidden" name="txtSave">
		</FORM>
	</body>
</html>
