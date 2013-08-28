<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UDCType_Edit.aspx.vb" Inherits="AdministrationCenter_UDC_UDCType_Edit" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>UDC Type Edit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../../Images/Js/JSValidation.js"></script>
		<LINK href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript">
var rand_no = Math.ceil(500*Math.random())
	var aa=2;
	var nn=1;

		function addSelectedItemsToParent()
			{
				self.opener.addToParentList(aa);
				window.close();
			}

 function OpenCompany(c)
				{
				    	wopen('../../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as Name,CI_IN4_Business_Relation as Type from T010011 where CI_VC8_Address_Book_Type='+"'COM'" + '  &tbname=' + c ,'Search'+rand_no
,500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'Search',500,450);					
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
			
			function addToParentList(Afilename,TbName,strName)
				{
				
					if (Afilename != "" || Afilename != 'undefined')
					{
						varName = TbName + 'Name'
					   //alert(Afilename);
					   //alert(strName);
						document.getElementById(TbName).value=Afilename;
						document.getElementById(varName).value=strName;
						aa=Afilename;
					}
					else
					
					{
						document.Form1.txtAB_Type.value=aa;
					}
				}
		function ChangeColorOver(obj,clrO,clrC)
			{
				if(obj.style.backgroundColor!=clrC)
					{
						obj.style.backgroundColor=clrO;
					}
			}

		function ChangeColorOut(obj,clrO,clrC)
			{
				if(obj.style.backgroundColor!=clrC)
					{
						obj.style.backgroundColor=clrO;          
					}
			}
		 
		function ChangeColorOut1(obj,clrO,clrC)
			{	
				document.getElementsByName("datagrid1").item(obj) .style.backgroundColor=clrC;  
			}

		function ChangeColorClick(obj, clrO, clrC)
			{
				obj.style.backgroundColor = clrC;
			}

		function HideContents()
			{
				parent.document.all("SideMenu1").cols="0,*";
			}
				
		function ShowContents()
			{
				parent.document.all("SideMenu1").cols="18%,*";
			}
			
		function Reload()
			{
				Form1.submit();
			}
				 
		function CloseWindow()
			{
				self.opener.callrefresh();
			}
			
				function SaveEdit(varImgValue)
				{
			    	if (varImgValue=='Edit')
						{
						
						//Security Block
							var obj=document.getElementById("imgEdit")
							if(obj==null)
							{
								alert("You don't have access rights to edit record");
								return false;
							}

							if (obj.disabled==true) 
							{
								alert("You don't have access rights to edit record");
								return false;
							}
					//End of Security Block

							if (document.Form1.txthidden.value==0)
								{
									alert("Please select the row");
								}
							else
								{
									document.Form1.txthiddenImage.value=varImgValue;
									document.Form1.txthiddenSkil.value=globalSkil;
									document.Form1.txthidden.value=globalAddNo;	
									document.Form1.txthiddenGrid.value=globalGrid;	
									Form1.submit(); 
										return false;
								}										
						}	
											
					if (varImgValue=='Close')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
								return false;
						}								
							
					if (varImgValue=='Add')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
								return false;
						}	
					
					if (varImgValue=='Logout')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
						}
													
					if (varImgValue=='Ok')
						{
						//Security Block
							var obj=document.getElementById("imgSave")
							if(obj==null)
							{
								alert("You don't have access rights to Save record");
								return false;
							}

							if (obj.disabled==true) 
							{
								alert("You don't have access rights to Save record");
								return false;
							}
					//End of Security Block


								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 
									return false;
													
						}
							
					if (varImgValue=='Save')
						{
						//Security Block
							var obj=document.getElementById("imgSave")
							if(obj==null)
							{
								alert("You don't have access rights to Save record");
								return false;
							}

							if (obj.disabled==true) 
							{
								alert("You don't have access rights to Save record");
								return false;
							}
					//End of Security Block

//alert(varImgValue);
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
								return false;
						}		
						
					if (varImgValue=='Attach')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
								return false;
						}	
					
					if (varImgValue=='Fwd')
						{
						   if (	document.Form1.txtrowvalues.value==0)
						   {
								alert("Please select the row");
							}
							else
							{	
								 wopen('Task_Fwd.aspx?ScrID=340','FWD'+rand_no,400,250);
							}	 
						   
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
		<%--<style type="text/css">TR:hover { BACKGROUND-COLOR: #ffccff }
	TR.over { BACKGROUND-COLOR: #ffccff }
	TR:hover { BACKGROUND-COLOR: #ffccff }
		</style>--%>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" onunload="CloseWindow();">
		<form id="Form1" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
			<TABLE id="Table1" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
				background="../../images/top_nav_back.gif" border="0">
				<TBODY>
					<TR>
						<TD>&nbsp;&nbsp;&nbsp;
							<asp:label id="Label1" runat="server" BorderStyle="None" BorderWidth="2px" Height="12px" Width="136px"
								Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal">UDC TYPE EDIT</asp:label></TD>
						<td>
							<IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
							<asp:ImageButton id="imgSave" runat="server" ImageUrl="../../Images/S2Save01.gif" accessKey="S" ToolTip="Save"></asp:ImageButton>
							<asp:imagebutton id="imgOk" accessKey="K" runat="server" ToolTip="OK" ImageUrl="../../Images/s1ok02.gif"></asp:imagebutton>
							<asp:ImageButton id="imgReset" runat="server" ImageUrl="../../Images/reset_20.gif" accessKey="R"
								ToolTip="Reset"></asp:ImageButton>
							<asp:ImageButton id="ImageButton1" accessKey="R" runat="server" ToolTip="Reset" ImageUrl="../../Images/S2close01.gif"></asp:ImageButton>
							<IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
						</td>
						<td>
							<!--<iewc:toolbar id="Toolbar2" runat="server" BorderStyle="None" Height="16px" Font-Size="XX-Small"
								Font-Names="Verdana" Font-Bold="True" ForeColor="Transparent" BackColor="LightGray">
								<iewc:ToolbarButton ImageUrl="../../Images/s1question02.gif" ToolTip="Help"></iewc:ToolbarButton>
							</iewc:toolbar>-->
						</td>
						<td width="42" background="../../images/top_nav_back.gif" height="47">&nbsp;</td>
					</TR>
				</TBODY>
			</TABLE>
			<TABLE id="Table126" borderColor="activeborder" height="100%" cellSpacing="0" cellPadding="0"
				width="100%" border="1">
				<TR>
					<TD vAlign="top" colSpan="1">
						<!--  **********************************************************************-->
						<TABLE id="Table7" cellSpacing="0" cellPadding="2" width="100%" border="0">
							<tr>
								<td colSpan="2"><cc1:collapsiblepanel id="cpnlErrorPanel" runat="server" BorderStyle="Solid" BorderWidth="0px" Height="47px"
										BorderColor="Indigo" Visible="False" Draggable="False" CollapseImage="../../Images/ToggleUp.gif" ExpandImage="../../Images/ToggleDown.gif"
										Text="Error Message" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel"
										TitleCSS="test">
										<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
											<TR>
												<TD colSpan="0" rowSpan="0">
													<asp:Image id="Image1" runat="server" Width="16px" Height="16px" ImageUrl="../../Images/warning.gif"></asp:Image></TD>
												<TD colSpan="0" rowSpan="0">&nbsp;
													<asp:Label id="lblError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:Label></TD>
											</TR>
										</TABLE>
									</cc1:collapsiblepanel></td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0" colSpan="2"><b><font face="Verdana" size="1">&nbsp;</font></b></td>
							</tr>
							<tr>
								<td style="HEIGHT: 24px" width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">Product 
											Code</font></b></td>
								<td style="HEIGHT: 24px" bgColor="#f0f0f0"><asp:textbox id="txtUDCTypePC" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="50"
										Font-Size="XX-Small" Font-Names="Verdana" Height="18px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="9"></asp:textbox></td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">UDC Type</font></b></td>
								<td bgColor="#f0f0f0"><asp:textbox id="txtUDCTypeP" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="50"
										Font-Size="XX-Small" Font-Names="Verdana" Height="18px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="4"></asp:textbox></td>
							</tr>
							<tr>
								<td style="HEIGHT: 20px" width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">UDC 
											Text</font></b></td>
								<td style="HEIGHT: 20px" bgColor="#f0f0f0"><asp:textbox id="txtUDCText" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="120"
										Font-Size="XX-Small" Font-Names="Verdana" MaxLength="30" Height="18px" CssClass="txtNoFocus"></asp:textbox></td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0" style="height: 67px"><b><font face="Verdana" size="1">Company</font></b></td>
								<td bgColor="#f0f0f0" style="height: 67px">
									<!--<asp:textbox id="txtCompanyName" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="120"
										Font-Size="XX-Small" Font-Names="Verdana" MaxLength="15" Height="18px" CssClass="txtNoFocus"></asp:textbox><img id="imgCompany" class="PlusImageCSS" onclick="OpenCompany('txtCompany');" src="../../Images/plus.gif"
										alt="Company" border="0">
									<asp:textbox id="txtCompany" runat="server" Width="0px" Height="0px" BorderWidth="0px" Font-Size="XX-Small"></asp:textbox>-->
									<uc1:CustomDDL id="CDDLCompany" runat="server" width="120"></uc1:CustomDDL>
								</td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">UDC Param</font></b></td>
								<td bgColor="#f0f0f0"><asp:checkbox id="chkUDCParam" runat="server"></asp:checkbox></td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			<!-- ***********************************************************************--> 
			 <INPUT type="hidden" name="txthiddenImage"><!-- Image Clicked-->
		</form>
	</body>
</html>
