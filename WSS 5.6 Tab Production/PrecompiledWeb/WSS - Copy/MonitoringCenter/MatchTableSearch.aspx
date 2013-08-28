<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_MatchTableSearch, App_Web_zn3-f7gx" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>MatchTableSearch</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">
		
				function Post(Action)
		{
			//alert(Action);
			SaveEdit(Action);
		}
		
		function SaveEdit( imgValue)
			{
			
					if ( imgValue=='Delete' )
					{
					if(document.Form1.txtHiddenID.value=='')
							{
							alert("Please select row");
							}
							else
						{	
						document.Form1.txthiddenImage.value= imgValue;
						document.Form1.submit();
					    }
					    }
					 
					   	if ( imgValue=='Add')
					       {
					      
					        	document.Form1.txthiddenImage.value= imgValue;
								document.Form1.submit();
					       }
					       
					       	if (imgValue=='Edit')
							{
									if ( document.Form1.txthiddenID.value =='')
									{
										alert('Please select a row');
									}
									else
									{
										document.Form1.txthiddenImage.value=imgValue;
											document.Form1.submit();
									}
							}	
				
					if ( imgValue=='Save')
					{
						document.Form1.txthiddenImage.value= imgValue;
						document.Form1.submit();
					}	
								
					return false;
			}
			
				
						function KeyCheck(rowvalues,ID)
						{
								
									
									var tableID='cpnlRPA_dgrMatchTable';
									var table;
									document.Form1.txthiddenID.value=ID;		
								
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

							function KeyCheck55(rowvalues,ID)
							{
							    	document.Form1.txthiddenReqID.value=ID;		
									SaveEdit('Edit');
							}
					
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td>
						<cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="54px" BorderStyle="Solid" BorderWidth="0px"
							Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Error Message"
							TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
							Visible="False" BorderColor="Indigo">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD colSpan="0" rowSpan="0">
										<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../../Images/warning.gif"></asp:Image></TD>
									<TD colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" BorderWidth="0" BorderStyle="Groove" Width="752px"
											Font-Names="Verdana" Font-Size="XX-Small" ForeColor="Red"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlRPA" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="0px" Draggable="False"
							CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" Text="Match Table " TitleBackColor="Transparent"
							TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test" Visible="True" BorderColor="Indigo">
							<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 545px">
								<asp:DataGrid id="dgrMatchTable" BorderColor="#d4d4d4" CssClass="grid" DataKeyField="RQ_NU9_SQID_PK"
									AutoGenerateColumns="False" Runat="server" CellPadding="0">
									<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
									<ItemStyle CssClass="GridItem"></ItemStyle>
									<HeaderStyle CssClass="GridHeader"></HeaderStyle>
									<Columns>
										<asp:BoundColumn DataField="RQ_NU9_SQID_PK" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="RQ_NU9_REQUEST_ID" Visible="False"></asp:BoundColumn>
										<asp:TemplateColumn ItemStyle-Wrap="True">
											<HeaderTemplate>
												<asp:TextBox Width="100%" id="txtProcessName" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
												ProcessName
											</HeaderTemplate>
											<ItemTemplate>
												<%# container.dataitem("PM_VC20_PName")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn ItemStyle-Wrap="True">
											<HeaderTemplate>
												<asp:TextBox Width="100%" id="txtRequest" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
												Request Type
											</HeaderTemplate>
											<ItemTemplate>
												<%# container.dataitem("RQ_VC150_CAT2")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn ItemStyle-Wrap="True">
											<HeaderTemplate>
												<asp:TextBox Width="100%" id="txtENV" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
												ENV
											</HeaderTemplate>
											<ItemTemplate>
												<%# container.dataitem("RQ_VC150_CAT3")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn ItemStyle-Wrap="True">
											<HeaderTemplate>
												<asp:TextBox Width="100%" id="TxtTableName" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
												Table Name
											</HeaderTemplate>
											<ItemTemplate>
												<%# container.dataitem("RQ_VC150_CAT4")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn ItemStyle-Wrap="True">
											<HeaderTemplate>
												<asp:TextBox Width="100%" id="Txtclient" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
												Client
											</HeaderTemplate>
											<ItemTemplate>
												<%# container.dataitem("CI_VC36_Name")%>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn ItemStyle-Wrap="True">
											<HeaderTemplate>
												<asp:TextBox Width="100%" id="txtStatus" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
												Status
											</HeaderTemplate>
											<ItemTemplate>
												<%# container.dataitem("RQ_CH2_STATUS")%>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:DataGrid></DIV>
						</cc1:collapsiblepanel>
						<!-- *****************************************-->
						<INPUT type="hidden" name="txthiddenImage"> <INPUT type="hidden" name="txthiddenID">
						<INPUT type="hidden" name="txthiddenReqID">
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
