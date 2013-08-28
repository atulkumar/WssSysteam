<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Inventory_EmpInventoryInfo, App_Web_y4sr_wuo" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Employee Inventory Info</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../../Images/Js/StyleSheet1.css">
	</HEAD>
	<body bottommargin="0" topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" background="../../images/top_nav_back.gif"
				border="0">
				<TR>
					<TD><asp:label id="lblTitleLabelUserInfo" runat="server" Width="200px" CssClass="TitleLabel">&nbsp;Employee Inventory Info</asp:label></TD>
					<td style="WIDTH: 902px">
						&nbsp;&nbsp;<IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0"><asp:imagebutton id="imgClose" accessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
							ToolTip="Close"></asp:imagebutton>&nbsp;&nbsp;<IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
					</td>
					<td width="42" background="../../images/top_nav_back01.gif" height="67">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Ta2ble1" cellSpacing="0" cellPadding="0" width="100%" background="../../images/top_nav_back.gif"
				border="0">
				<TR>
					<TD vAlign="top" align="left" width="100%"><cc1:collapsiblepanel id="cpnlInventory" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
							TitleCSS="test" PanelCSS="panel" TitleForeColor="PowderBlue" TitleClickable="True" TitleBackColor="Transparent" Text="Employee Inventory Info"
							ExpandImage="../../Images/ToggleDown1.gif" CollapseImage="../../Images/ToggleUp1.gif" Draggable="False" BorderColor="Indigo" state="expanded">
							<TABLE cellSpacing="0" cellPadding="0" width="0px" border="0">
								<TR>
									<TD>
										<asp:DataGrid id="grdInventory" runat="server" CssClass="Grid" Width="100%" BorderStyle="None"
											BorderWidth="1px" DataKeyField="ItemID" CellPadding="1" AutoGenerateColumns="False" HorizontalAlign="Center">
											<FooterStyle CssClass="GridFooter"></FooterStyle>
											<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
											<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
											<ItemStyle CssClass="GridItem"></ItemStyle>
											<HeaderStyle CssClass="GridHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="ItemID" HeaderText="Item&nbsp;ID" HeaderStyle-Width="50px"></asp:BoundColumn>
												<asp:BoundColumn DataField="ItemGroup" HeaderText="Item&nbsp;Group" HeaderStyle-Width="100px"></asp:BoundColumn>
												<asp:BoundColumn DataField="ItemName" HeaderText="Item&nbsp;Name" HeaderStyle-Width="150px"></asp:BoundColumn>
												<asp:BoundColumn DataField="Quantity" HeaderText="Quantity" HeaderStyle-Width="50px"></asp:BoundColumn>
												<asp:BoundColumn DataField="IssueDate" HeaderText="Issue&nbsp;Date" HeaderStyle-Width="100px"></asp:BoundColumn>
												<asp:BoundColumn DataField="Returnable" HeaderText="R" HeaderStyle-Width="20px"></asp:BoundColumn>
												<asp:BoundColumn DataField="ExpectedReturnDate" HeaderText="ExReturnDate" HeaderStyle-Width="100px"></asp:BoundColumn>
												<asp:BoundColumn DataField="Comments" HeaderText="Comments" ItemStyle-Wrap="True" HeaderStyle-Width="180px"></asp:BoundColumn>
											</Columns>
										</asp:DataGrid></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</html>
