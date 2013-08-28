<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReportEntry.aspx.vb" Inherits="MonitoringCenter_ReportEntry" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>ReportEntry</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">
function UPPER(obj)
{
	obj.value=obj.value.upper();
}		

		
		/**********************AJAX for Project****************************************/

		var gtype;
		var xmlHttp; 
		var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
		var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
		var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
		//netscape, safari, mozilla behave the same??? 
		var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 

	
		var gDID;
		var gMID;
		
		function DomainChange(DID,MID)
		{
	//	alert(DID);
			//document.getElementById('txtHIDAgreement')='';
			gDID=DID;
			gMID=MID;
			
			xmlHttp=null;
			var ddlDomain=document.getElementById(gDID);
			var DomainID=ddlDomain.options(ddlDomain.selectedIndex).value;
			var url= '../AJAX Server/AjaxInfo.aspx?Type=DomainMachine&DomainID='+DomainID+'&Rnd='+Math.random();
			xmlHttp = GetXmlHttpObject(stateChangeHandler);    
			xmlHttp_Get(xmlHttp, url); 
		}
		 
		
		function stateChangeHandler() 
		 { 	
				 document.getElementById(gMID).options.length=0;
				objNewOption = document.createElement("OPTION");
				document.getElementById(gMID).options.add(objNewOption);
				objNewOption.value = '0';
				objNewOption.innerText ='Select';	
				
				 
				if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
				{ 
						var response = xmlHttp.responseXML; 
						var info = response.getElementsByTagName("INFO");
						
						if(info.length > 0)
						{
								var vTable = response.getElementsByTagName("TABLE");
								var intT;
								for ( intT=0; intT<vTable.length; intT++)
								{
									var item = vTable[intT].getElementsByTagName("ITEM");
									var objForm = document.Form1;
									var DataName='';
									var DataID='';
									switch(intT)
									{
										case 0:
										{	
										
											for (var inti=0; inti<item.length; inti++)
											{
													var objNewOption = document.createElement("OPTION");
													document.getElementById(gMID).options.add(objNewOption);
													objNewOption.value = item[inti].getAttribute("COL0");
													objNewOption.innerText = item[inti].getAttribute("COL1");
													DataName=DataName+item[inti].getAttribute("COL1") + '^';
													DataID=DataID+item[inti].getAttribute("COL0") + '^';			
											}
											document.Form1.txtMachineInfo.value= DataName + '~' + DataID ;
											break;
										}//case 0
						
									}//switch
								} //for loop
						
						}//if
						
				}//
				else
				{
						//wait				
				}
				
		} //function
		
		
		function xmlHttp_Get(xmlhttp, url) 
		{ 
		        xmlhttp.open('GET', url, true); 
		        xmlhttp.send(null); 
		       
		} 
    
		function GetXmlHttpObject(handler) 
		{ 
				var objXmlHttp = null;    //Holds the local xmlHTTP object instance 
				if (is_ie)
		        { 
						var strObjName = (is_ie5) ? 'Microsoft.XMLHTTP' : 'Msxml2.XMLHTTP'; 
				        try
				        { 
								objXmlHttp = new ActiveXObject(strObjName); 
								objXmlHttp.onreadystatechange = handler; 
						} 
						catch(e)
						{ 
								alert('IE detected, but object could not be created. Verify that active scripting and activeX controls are enabled'); 
								return; 
			            } 
				} 
				else if (is_opera)
				{ 
						alert('Opera detected. The page may not behave as expected.'); 
						return; 
				} 
				else
				{ 
						objXmlHttp = new XMLHttpRequest(); 
						objXmlHttp.onload = handler; 
						objXmlHttp.onerror = handler; 
				} 
				return objXmlHttp; 
		} 
    
		

/***************************************************************************/
		
		
		
		
		
		
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
								
					return false;
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
		
		try
		{
		
			if(document.getElementById('cpnlReportDetail_RdbtnUID').checked==true)
			{
				document.getElementById('cpnlReportDetail_TxtFile').disabled=true;
				document.getElementById('cpnlReportDetail_TxtPUID').disabled=false;
				document.getElementById('cpnlReportDetail_txtPPWD').disabled=false;
			}
			else
			{
				document.getElementById('cpnlReportDetail_TxtFile').disabled=false;
				document.getElementById('cpnlReportDetail_TxtPUID').disabled=true;
				document.getElementById('cpnlReportDetail_txtPPWD').disabled=true;		
			}
		}
		catch(e)
		{}	
			return true;
		}
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<td><asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button></td>
					<TD><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="54px" Text="Error Message" Visible="False"
							BorderColor="#f5f5f5" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
							TitleBackColor="Transparent" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
							Draggable="False" BorderStyle="Solid" BorderWidth="0px">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD>
										<asp:image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif"></asp:image></TD>
									<TD>
										<asp:ListBox id="lstError" runat="server" Width="552px" BorderWidth="0" BorderStyle="Groove"
											Font-Names="Verdana" Font-Size="XX-Small" ForeColor="Red"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel><cc1:collapsiblepanel id="cpnlReportDetail" runat="server" Width="100%" Text="Report Details" Visible="true"
							BorderColor="Indigo" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="false" TitleBackColor="Transparent"
							ExpandImage="../Images/white.gif" CollapseImage="../Images/white.gif" Draggable="False" BorderStyle="Solid" BorderWidth="0px">
							<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 480px">
								<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
									<TR>
										<TD>
											<DIV style="OVERFLOW: auto; WIDTH: 200%; HEIGHT: 320px">
												<asp:DataGrid id="DgReportDetail" runat="server" BorderStyle="None" BorderColor="#d4d4d4" CssClass="Grid"
													CellPadding="0" AutoGenerateColumns="False">
													<SelectedItemStyle CssClass="GridSelectedItem" BackColor="#D4D4D4"></SelectedItemStyle>
													<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
													<ItemStyle CssClass="GridItem"></ItemStyle>
													<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="RP_NU9_SQID_PK"></asp:BoundColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtDomain_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Domain<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("DM_VC150_DomainName")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtMachine_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Machine<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("MM_VC150_Machine_Name")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtRelease_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Release<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_Release")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtMacUID_s" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																	Enabled="False"></asp:TextBox>
																MacUID<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("MM_VC500_UID")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtMacPWD_s" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																	Enabled="False"></asp:TextBox>
																MacPWD<br>
																&nbsp;
															</HeaderTemplate>
															<ItemStyle VerticalAlign="Top"></ItemStyle>
															<ItemTemplate>
																<%# container.dataitem("MM_VC500_PWD")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderStyle Wrap="False"></HeaderStyle>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtSubmitReport_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Submit<BR>
																Report
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_CH2_SubmitReport")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<ItemStyle VerticalAlign="Top"></ItemStyle>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtReportName_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Report Name<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_ReportName")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtAliasName_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Alias Name<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC50_AliasName")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtVersion_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Version<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_Version")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtUID_s" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																	Enabled="False"></asp:TextBox>
																UID<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_PUID")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtPWD_s" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																	Enabled="False"></asp:TextBox>
																PWD<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_PPWD")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtFile_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																File<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_FilePath")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtRole_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Role<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_Role")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtEnv_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Env<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_Env")%>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn>
															<HeaderTemplate>
																<asp:TextBox Width="100%" id="TxtJobQueue_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																Job Queue<br>
																&nbsp;
															</HeaderTemplate>
															<ItemTemplate>
																<%# container.dataitem("RP_VC150_JobQUE")%>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
												</asp:DataGrid></DIV>
										</TD>
									</TR>
									<TR>
										<TD>
											<asp:Panel id="Panel1" Height="30px" Width="0" Runat="server">
												<TABLE cellSpacing="0" cellPadding="0" border="0">
													<TR>
														<TD>
															<asp:dropdownlist id="DdlDomain" runat="server" Width="73px" CssClass="txtNoFocusFE"></asp:dropdownlist></TD>
														<TD>
															<asp:dropdownlist id="DdlMachine" runat="server" Width="83px" CssClass="txtNoFocusFE" AutoPostBack="True"></asp:dropdownlist></TD>
														<TD>
															<asp:dropdownlist id="DdlPeopleSoftRel" runat="server" Width="61px" CssClass="txtNoFocusFE"></asp:dropdownlist></TD>
														<TD>
															<asp:textbox id="TxtUID" runat="server" Width="81px" CssClass="txtNoFocusFE"></asp:textbox></TD>
														<TD>
															<asp:textbox id="TxtPWD" runat="server" Width="81px" CssClass="txtNoFocusFE" TextMode="Password"></asp:textbox></TD>
														<TD>
															<asp:DropDownList id="DdlSubReport" runat="server" Width="61px" CssClass="txtNoFocusFE" AutoPostBack="True"
																Font-Bold="True">
																<asp:ListItem Value="Y">Y</asp:ListItem>
																<asp:ListItem Value="N">N</asp:ListItem>
															</asp:DropDownList></TD>
														<TD>
															<asp:TextBox id="txtReportName" runat="server" Width="102px" CssClass="txtNoFocusFE" MaxLength="150"></asp:TextBox></TD>
														<TD>
															<asp:TextBox id="TxtAname_F" runat="server" Width="102px" CssClass="txtNoFocusFE" MaxLength="50"></asp:TextBox></TD>
														<TD>
															<asp:TextBox id="TxtVer" runat="server" Width="81px" CssClass="txtNoFocusFE" MaxLength="150"></asp:TextBox></TD>
														<TD>
															<asp:RadioButton id="RdbtnUID" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"
																GroupName="RdoGroup"></asp:RadioButton></TD>
														<TD>
															<asp:TextBox id="TxtPUID" runat="server" Width="73px" CssClass="txtNoFocusFE"></asp:TextBox></TD>
														<TD>
															<asp:TextBox id="txtPPWD" runat="server" Width="81px" CssClass="txtNoFocusFE" TextMode="Password"></asp:TextBox></TD>
														<TD>
															<asp:RadioButton id="RdbtnFile" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="True"
																GroupName="RdoGroup"></asp:RadioButton></TD>
														<TD>
															<asp:TextBox id="TxtFile" runat="server" Width="99px" CssClass="txtNoFocusFE"></asp:TextBox></TD>
														<TD>
															<asp:DropDownList id="DDLRole" runat="server" Width="60px" CssClass="txtNoFocusFE" AutoPostBack="True"
																Font-Bold="True"></asp:DropDownList></TD>
														<TD>
															<asp:DropDownList id="DdlEnv" runat="server" Width="61px" CssClass="txtNoFocusFE" AutoPostBack="True"
																Font-Bold="True"></asp:DropDownList></TD>
														<TD>
															<asp:TextBox id="TxtJobQueue" runat="server" Width="80px" CssClass="txtNoFocusFE"></asp:TextBox></TD>
													</TR>
												</TABLE>
											</asp:Panel></TD>
									</TR>
								</TABLE>
							</DIV>
						</cc1:collapsiblepanel><input type="hidden" name="txthiddenImage"> <input type="hidden" name="txtHiddenID">
						<input type="hidden" name="txtMachineInfo">
					</TD>
				</TR>
			</table>
		</form>
	</body>
</html>
