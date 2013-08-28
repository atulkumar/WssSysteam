<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_ReportDetail, App_Web_vrtyhdgv" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>ReportDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
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
					var tableID='cpnlDataBase_DgReportName';
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
						
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" onload="Hideshow();"
		rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="WIDTH: 653px" vAlign="top" colSpan="2" height="1"><cc1:collapsiblepanel id="cpnlError" runat="server" BorderStyle="Solid" BorderColor="Indigo" Width="100%"
							Text="Error Message" BorderWidth="0px" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" TitleBackColor="Transparent"
							TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Height="54px">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD>
										<asp:image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="Images/warning.gif"></asp:image></TD>
									<TD>
										<asp:ListBox id="lstError" runat="server" BorderWidth="0" Width="552px" BorderStyle="Groove"
											ForeColor="Red" Font-Size="XX-Small" Font-Names="Verdana"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel></TD>
				<TR>
					<TD style="WIDTH: 653px" vAlign="top" colSpan="2" height="1"><cc1:collapsiblepanel id="cpnlDataBase" runat="server" BorderStyle="Solid" BorderColor="Indigo" Width="100%"
							Text="Report Details" BorderWidth="0px" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" TitleBackColor="Transparent"
							TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="True">
							<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 480px">
								<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
									<TR>
										<TD>
											<DIV style="OVERFLOW: auto; WIDTH: 200%; HEIGHT: 320px">
												<asp:DataGrid id="DgReportName" runat="server" BorderWidth="0" BorderColor="#d4d4d4" AutoGenerateColumns="False"
													CellPadding="0" CssClass="Grid">
													<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
													<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
													<ItemStyle CssClass="GridItem"></ItemStyle>
													<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
													<Columns>
														<asp:ButtonColumn Visible="False" Text="select" CommandName="select"></asp:ButtonColumn>
														<asp:BoundColumn Visible="False" DataField="RP_NU9_SQID_PK"></asp:BoundColumn>
														<asp:BoundColumn DataField="RP_VC150_ReportName" HeaderText="Report Name">
															<HeaderStyle Width="100px"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DM_VC150_DomainName" HeaderText="Domain">
															<HeaderStyle Width="72px"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="MM_VC150_Machine_Name" HeaderText="Machine">
															<HeaderStyle Width="80px"></HeaderStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="RP_VC150_Release" HeaderText="Release">
															<HeaderStyle Width="80px"></HeaderStyle>
														</asp:BoundColumn>
													</Columns>
												</asp:DataGrid></DIV>
										</TD>
									</TR>
								</TABLE>
							</DIV>
						</cc1:collapsiblepanel><input type="hidden" name="txthiddenImage"> <input type="hidden" name="txtHiddenID">
					</TD>
				</TR>
			</table>
		</form>
	</body>
</html>
