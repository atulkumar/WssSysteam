<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_UDC_UDC_Edit, App_Web_cj8ho3o2" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>UDC Edit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="core.js" type="text/javascript"></script>
		<script language="JavaScript" src="events.js" type="text/javascript"></script>
		<script language="JavaScript" src="css.js" type="text/javascript"></script>
		<script language="JavaScript" src="coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="drag.js" type="text/javascript"></script>
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
			
		function SelectDDL(UDC,ctrl,Type)
		{
				var n;
				n=document.getElementById(ctrl).options.selectedIndex;
				if ( document.getElementById(ctrl).options(n).text=='More' )
				{
					document.getElementById(ctrl).options(0).text=""
					document.getElementById(ctrl).options.selectedIndex=0;
					document.getElementById(ctrl.substr(0,ctrl.length-3)).value=""
					OpenCompany(ctrl);
				}
				else
				{
					document.getElementById(ctrl).options(0).text=""
					document.getElementById(ctrl.substr(0,ctrl.length-3)).value=document.getElementById(ctrl).options(n).value;			
				}
		}
				
		function addToParentList(Afilename,TbName,strName)
		{
					if (Afilename != "" || Afilename != 'undefined')
					{
					   if ( TbName.substr(TbName.length-3,TbName.length)=='DDL')
					   {
					   			document.getElementById(TbName).options(0).text=strName;
					   			document.getElementById(TbName).options(0).
								document.getElementById(TbName).options.selectedIndex=0;
								document.getElementById(TbName.substr(0,TbName.length-3)).value=Afilename;
					   }
					   else
					   {
						document.getElementById(TbName).value=Afilename;
						}
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
			if (varImgValue=='Close')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
								return false;
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


							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
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
				<TR>
					<TD>&nbsp;&nbsp;&nbsp;
						<asp:label id="Label1" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
							Font-Size="X-Small" Width="88px" Height="12px" BorderWidth="2px" BorderStyle="None">UDC Edit</asp:label>
					</TD>
					<td><IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
						<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif" ToolTip="Save"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgOk" runat="server" ImageUrl="../../Images/s1ok02.gif" AccessKey="K" ToolTip="OK"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgReset" accessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
							ToolTip="Reset"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgClose" runat="server" ImageUrl="../../Images/s2close01.gif" accessKey="O"
							ToolTip="Close"></asp:imagebutton>&nbsp; <IMG title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
					</td>
					<td>
						<!--<iewc:toolbar id="Toolbar2" runat="server" ForeColor="Transparent" Font-Bold="True" Font-Names="Verdana"
							Font-Size="XX-Small" Height="16px" BorderStyle="None" BackColor="LightGray">
							<iewc:ToolbarButton ImageUrl="../../Images/s1question02.gif" ToolTip="Help"></iewc:ToolbarButton>
						</iewc:toolbar>-->
					</td>
					<td width="42" background="../../images/top_nav_back.gif" height="47">&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE id="Table126" borderColor="activeborder" height="100%" cellSpacing="0" cellPadding="0"
				width="100%" border="1">
				<TR>
					<TD vAlign="top" colSpan="1">
						<!--  **********************************************************************-->
						<TABLE id="Table7" cellSpacing="0" cellPadding="2" width="100%" border="0">
							<tr>
								<td colSpan="2"><cc1:collapsiblepanel id="cpnlErrorPanel" runat="server" Height="47px" BorderWidth="0px" BorderStyle="Solid"
										BorderColor="Indigo" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
										Text="Error Message" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif" Draggable="False"
										Visible="False">
										<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
											<TR>
												<TD colSpan="0" rowSpan="0">
													<asp:Image id="Image1" runat="server" Height="16px" Width="16px" ImageUrl="../../Images/warning.gif"></asp:Image></TD>
												<TD colSpan="0" rowSpan="0">&nbsp;
													<asp:Label id="lblError" runat="server" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:Label></TD>
											</TR>
										</TABLE>
									</cc1:collapsiblepanel></td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0" colSpan="2"><b><font face="Verdana" size="1">&nbsp;</font></b></td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">Product Code</font></b></td>
								<td bgColor="#f0f0f0"><asp:textbox id="txtProductCode" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="50"
										Height="18px" BorderWidth="1px" BorderStyle="Solid" ReadOnly="True" CssClass="txtNoFocus" MaxLength="9"></asp:textbox></td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">UDC Type</font></b></td>
								<td bgColor="#f0f0f0"><asp:textbox id="txtUCDTypeF" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="50"
										Height="18px" BorderWidth="1px" BorderStyle="Solid" ReadOnly="True" CssClass="txtNoFocus" MaxLength="4"></asp:textbox></td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">Name</font></b></td>
								<td bgColor="#f0f0f0"><asp:textbox id="txtName" runat="server" Width="120" Font-Size="XX-Small" Font-Names="Verdana"
										MaxLength="8" BorderWidth="1px" BorderStyle="Solid" Height="18px" CssClass="txtNoFocus" ReadOnly="True"></asp:textbox></td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">Company</font></b></td>
								<td bgColor="#f0f0f0">
									<uc1:CustomDDL id="CDDLCustomer" runat="server" width="110px"></uc1:CustomDDL>
									<!--		<asp:DropDownList Width="120px" Height="18px" CssClass="txtNoFocus" ID="txtCompanyDDL" Runat="server"></asp:DropDownList>
									<asp:textbox id="txtCompanyName" runat="server" Width="0px" Height="18px" BorderWidth="0px" Font-Size="XX-Small"></asp:textbox>
									<asp:textbox id="txtCompany" runat="server" Width="0px" Height="18px" BorderWidth="0px" Font-Size="XX-Small"></asp:textbox>-->
								</td>
							</tr>
							<tr>
								<td width="127" bgColor="#f0f0f0"><b><font face="Verdana" size="1">Description</font></b></td>
								<td bgColor="#f0f0f0"><asp:textbox id="txtDescription" runat="server" Width="216px" Font-Size="XX-Small" Font-Names="Verdana"
										MaxLength="30" BorderWidth="1px" BorderStyle="Solid" Height="18px" CssClass="txtNoFocus"></asp:textbox></td>
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
