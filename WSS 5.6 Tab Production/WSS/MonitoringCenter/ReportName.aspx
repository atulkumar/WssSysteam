<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReportName.aspx.vb" Inherits="MonitoringCenter_ReportName" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>ReportName</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">
			
			function Post(Action)
		{
			//alert(Action);
			SaveEdit(Action);
		}
			function SaveEdit(imgValue)
			{
			
					if ( imgValue=='Delete' )
					{
					if(document.Form1.txtHiddenID.value=='')
							{
							alert("Please select row");
							}
							else
						{	
						document.Form1.txthiddenImage.value=imgValue;
						document.Form1.submit();
					    }
					    }
					 
					   	if (imgValue=='Add')
					       {
					        alert("Add button is not for this screen");
					       }
				
					if (imgValue=='Save')
					{
						document.Form1.txthiddenImage.value=imgValue;
						document.Form1.submit();
					}	
					if (imgValue=='Edit')
					{
						document.Form1.txthiddenImage.value=imgValue;
						document.Form1.submit();
					}	
					
					return false;
			}
			
			function GridDBLClick(ID)
			{
				document.Form1.txtHiddenID.value=ID;
				SaveEdit('Edit');
			}
				
		
			function GridClick(rowvalues,ID)
			{
			     	var tableID='cpnlReportDetail_DgReportDetail';
							var table;
										document.Form1.txtHiddenID.value=ID;
								
									if (document.all) table=document.all[tableID];
										if (document.getElementById) table=document.getElementById(tableID);
										if (table)
										{
												for ( var i = 1 ;  i < table.rows.length ;  i++)
													{	
														if(i % 2 == 0)
															{
																table.rows [ i ] . style . backgroundColor = "#f5f5f5";
															}
															else
															{
															
																table.rows [ i ] . style . backgroundColor = "#ffffff";
															}
													}
											table.rows [ rowvalues  ] . style . backgroundColor = "#d4d4d4";
										}
			}
			function ChkRdodisable()
		{
		
		
			if(document.getElementById('cpnlReport_RdbtnUID').checked==true)
			{
				document.getElementById('cpnlReport_TxtFile').disabled=true;
				document.getElementById('cpnlReport_TxtPUID').disabled=false;
				document.getElementById('cpnlReport_txtPPWD').disabled=false;
			}
			else
			{
				document.getElementById('cpnlReport_TxtFile').disabled=false;
				document.getElementById('cpnlReport_TxtPUID').disabled=true;
				document.getElementById('cpnlReport_txtPPWD').disabled=true;		
			}
			
			return true;
		}
						
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td><asp:button id="BtnGrdSearch" runat="server" Height="0" Width="0px"></asp:button></td>
					<TD><cc1:collapsiblepanel id="cpnlError" runat="server" Height="54px" Width="100%" BorderWidth="0px" BorderStyle="Solid"
							Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" TitleBackColor="Transparent"
							TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" BorderColor="#f5f5f5"
							Visible="False" Text="Error Message">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD>
										<asp:image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../Images/warning.gif"></asp:image></TD>
									<TD>
										<asp:ListBox id="lstError" runat="server" Width="552px" BorderStyle="Groove" BorderWidth="0"
											Font-Names="Verdana" Font-Size="XX-Small" ForeColor="Red"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlMachine" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
							Draggable="False" CollapseImage="../Images/white.gif" ExpandImage="../Images/white.gif" TitleBackColor="Transparent" TitleClickable="false"
							TitleForeColor="black" PanelCSS="panel" TitleCSS="test" BorderColor="Indigo" Visible="true" Text="Machine">
							<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD style="WIDTH: 69px; HEIGHT: 8px">
										<asp:label id="lblDomain" runat="server" CssClass="FieldLabel">Domain</asp:label></TD>
									<TD style="WIDTH: 118px; HEIGHT: 8px">
										<asp:dropdownlist id="DdlDomain" runat="server" Width="104px" CssClass="txtNoFocus" AutoPostBack="True"></asp:dropdownlist></TD>
									<TD style="HEIGHT: 8px">
										<asp:label id="lblMachine" runat="server" CssClass="FieldLabel">Machine</asp:label></TD>
									<TD style="HEIGHT: 8px">
										<asp:dropdownlist id="DdlMachine" runat="server" Width="104px" CssClass="txtNoFocus" AutoPostBack="True"></asp:dropdownlist></TD>
									<TD style="HEIGHT: 8px">
										<asp:label id="lblPeopleSoftRel" runat="server" Width="152px" CssClass="FieldLabel">People Soft Release</asp:label></TD>
									<TD style="HEIGHT: 8px">
										<asp:dropdownlist id="DdlPeopleSoftRel" runat="server" Width="104px" CssClass="txtNoFocus"></asp:dropdownlist></TD>
								</TR>
								<TR style="HEIGHT: 180px">
								</TR>
								<TR>
									<TD style="WIDTH: 69px">
										<asp:label id="LblUID" runat="server" CssClass="FieldLabel">UID</asp:label></TD>
									<TD style="WIDTH: 118px">
										<asp:textbox id="TxtUID" runat="server" Width="104px" Font-Names="Verdana" Font-Size="XX-Small"></asp:textbox></TD>
									<TD>
										<asp:label id="LblPWD" runat="server" CssClass="FieldLabel">PWD</asp:label></TD>
									<TD>
										<asp:textbox id="TxtPWD" runat="server" Width="104px" Font-Names="Verdana" Font-Size="XX-Small"
											TextMode="Password"></asp:textbox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel></TD>
				</tr>
				<TR>
					<TD vAlign="top" colSpan="2" height="1"><cc1:collapsiblepanel id="cpnlReport" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
							Draggable="False" CollapseImage="../Images/white.gif" ExpandImage="../Images/white.gif" TitleBackColor="Transparent" TitleClickable="false"
							TitleForeColor="black" PanelCSS="panel" TitleCSS="test" BorderColor="Indigo" Visible="true" Text="Report Entry">
							<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD>
										<asp:Label id="lblSubmitReport" runat="server" CssClass="FieldLabel">Submit Report</asp:Label></TD>
									<TD>
										<asp:DropDownList id="DdlSubReport" runat="server" Width="104px" CssClass="txtNoFocus" AutoPostBack="True">
											<asp:ListItem Value="Y">Y</asp:ListItem>
											<asp:ListItem Value="N">N</asp:ListItem>
										</asp:DropDownList></TD>
									<TD>&nbsp;
										<asp:Label id="LblJobQueue" runat="server" CssClass="FieldLabel">Job Queue</asp:Label></TD>
									<TD>
										<asp:TextBox id="TxtJobQueue" runat="server" Width="104px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="LblReportName" runat="server" CssClass="FieldLabel">Report Name</asp:Label></TD>
									<TD>
										<asp:TextBox id="txtReportName" runat="server" Width="104px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox></TD>
									<TD>
										<asp:RadioButton id="RdbtnUID" runat="server" CssClass="FieldLabel" GroupName="RdoGroup"></asp:RadioButton>
										<asp:Label id="LblPUID" runat="server" CssClass="FieldLabel">UID</asp:Label></TD>
									<TD>
										<asp:TextBox id="TxtPUID" runat="server" Width="104px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="LblVersion" runat="server" CssClass="FieldLabel">Version</asp:Label></TD>
									<TD>
										<asp:TextBox id="TxtVersion" runat="server" Width="104px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox></TD>
									<TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:Label id="LblPPWD" runat="server" CssClass="FieldLabel">PWD</asp:Label></TD>
									<TD>
										<asp:TextBox id="txtPPWD" runat="server" Width="104px" Font-Names="Verdana" Font-Size="XX-Small"
											TextMode="Password"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="LblRole" runat="server" CssClass="FieldLabel">Role</asp:Label></TD>
									<TD>
										<asp:DropDownList id="DDLRole" runat="server" Width="104px" CssClass="txtNoFocus"></asp:DropDownList></TD>
									<TD>
										<asp:RadioButton id="RdbtnFile" runat="server" CssClass="FieldLabel" GroupName="RdoGroup"></asp:RadioButton>
										<asp:Label id="LblFile" runat="server" CssClass="FieldLabel">File</asp:Label></TD>
									<TD>
										<asp:TextBox id="TxtFile" runat="server" Width="104px" Font-Names="Verdana" Font-Size="XX-Small"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="LblEnv" runat="server" CssClass="FieldLabel">Environment</asp:Label></TD>
									<TD>
										<asp:DropDownList id="DdlEnv" runat="server" Width="104px" CssClass="txtNoFocus"></asp:DropDownList></TD>
									<TD></TD>
									<TD></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel></TD>
				</TR>
				<tr>
					<TD vAlign="top" colSpan="2" height="1"><cc1:collapsiblepanel id="cpnlReportDetail" runat="server" Height="426px" Width="100.45%" BorderWidth="0px"
							BorderStyle="Solid" Draggable="False" CollapseImage="../Images/white.gif" ExpandImage="../Images/white.gif" TitleBackColor="Transparent"
							TitleClickable="false" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" BorderColor="Indigo" Visible="true" Text="Report Details">
							<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD>
										<asp:Panel id="Panel2" Width="0" Runat="server">
											<TABLE cellSpacing="0" cellPadding="0" border="0">
												<TR>
													<TD>
														<asp:TextBox id="TxtReportName_s" Width="101px" CssClass="SearchTxtBox" Runat="server"></asp:TextBox></TD>
													<TD>
														<asp:TextBox id="TxtUID_s" runat="server" Width="73px" CssClass="SearchTxtBox" Enabled="False"
															BackColor="#D4D4D4"></asp:TextBox></TD>
													<TD>
														<asp:TextBox id="TxtPWD_s" runat="server" Width="81px" CssClass="SearchTxtBox" Enabled="False"
															BackColor="#D4D4D4"></asp:TextBox></TD>
													<TD>
														<asp:TextBox id="TxtFile_s" runat="server" Width="81px" CssClass="SearchTxtBox"></asp:TextBox></TD>
													<TD>
														<asp:TextBox id="TxtEnv_s" runat="server" Width="81px" CssClass="SearchTxtBox"></asp:TextBox></TD>
													<TD>
														<asp:TextBox id="TxtVersion_s" Width="81px" CssClass="SearchTxtBox" Runat="server"></asp:TextBox></TD>
													<TD>
														<asp:TextBox id="TxtJobQueue_s" runat="server" Width="81px" CssClass="SearchTxtBox"></asp:TextBox></TD>
												</TR>
											</TABLE>
										</asp:Panel>
								<TR>
									<TD>
										<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 320px">
											<asp:DataGrid id="DgReportDetail" runat="server" BorderColor="#d4d4d4" BorderStyle="None" CssClass="Grid"
												CellPadding="0" AutoGenerateColumns="False">
												<SelectedItemStyle CssClass="GridSelectedItem" BackColor="#D4D4D4"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
												<ItemStyle CssClass="GridItem"></ItemStyle>
												<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
												<Columns>
													<asp:ButtonColumn Visible="False" Text="select" CommandName="select"></asp:ButtonColumn>
													<asp:BoundColumn Visible="False" DataField="RP_NU9_SQID_PK"></asp:BoundColumn>
													<asp:BoundColumn DataField="RP_VC150_ReportName" HeaderText="Report Name">
														<HeaderStyle Width="100px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="RP_VC150_PUID" HeaderText="UID">
														<HeaderStyle Width="72px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="RP_VC150_PPWD" HeaderText="PWD">
														<HeaderStyle Width="80px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="RP_VC150_FilePath" HeaderText="File">
														<HeaderStyle Width="80px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="RP_VC150_Env" HeaderText="ENV">
														<HeaderStyle Width="80px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="RP_VC150_Version" HeaderText="Version">
														<HeaderStyle Width="80px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="RP_VC150_JobQUE" HeaderText="Job Queue">
														<HeaderStyle Width="80px"></HeaderStyle>
													</asp:BoundColumn>
												</Columns>
											</asp:DataGrid></DIV>
									</TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel><input type="hidden" name="txthiddenImage"> <input type="hidden" name="txthiddenSQID">
						<input type="hidden" name="txtHiddenID">
					</TD>
				</tr>
			</table>
			</form>
	</body>
</html>
