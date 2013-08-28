<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_ObjectEntry, App_Web_zn3-f7gx" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Database Object</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>
		<script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<script language="javascript" src="../DateControl/ION.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script>
var globleID;
var globlestrTempName;
var globalStatus;

function OpenW(c)
{
//alert();
wopen('../Search/Common/PopSearch.aspx?ID=select AM_IN4_AID_FK as ID,AM_VC20_Code as AlertCode,AM_VC6_TYPE as AlertType from T180011' + '  &tbname=' + c ,'Search',500,450);
}

function OpenW12(a,b,c)
{
wopen('../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
}


function OpenProcessCode(c)
{

wopen('../Search/Common/PopSearch.aspx?ID=select PM_IN4_PCode as ID, PM_VC20_PName as ProcessName, PM_VC10_PType as ProcessType from T130031' + '  &tbname=' + c ,'Search',500,450);
}

function OpenMachineCode(c)
{
var domid;

var domid=document.getElementById('cpnlObject_txtDomain').value;
var type=document.getElementById('cpnlObject_txtObjectType').value;

if (type) 
{
	if (domid)
		{
			if (document.getElementById('cpnlObject_txtObjectType').value!='DB')
				{
						wopen('../Search/Common/PopSearch.aspx?ID=select MM_VC100_MID as ID, MM_VC150_Machine_Name as MachineName, MM_VC8_Machine_Type as Type from T170012 where MM_IN4_DID_FK='+domid + '  &tbname=' + c ,'Search',500,450);
				}
			else
				{
						wopen('../Search/Common/PopSearch.aspx?ID=select MM_VC100_MID as ID, MM_VC150_Machine_Name as MachineName, MM_VC8_Machine_Type as Type from T170012 where MM_VC8_Machine_Type='+"'S'"+' and  MM_IN4_DID_FK='+domid + '  &tbname=' + c ,'Search',500,450);
				}
		}
	else
		{
			alert('select domain first');
		}
	
}
else
	{
		alert('select object first');
	}

 }                                                           


function OpenDomainName(c)
{

wopen('../Search/Common/PopSearch.aspx?ID=select DM_IN4_DID as ID, DM_VC150_DomainName as DomainName, DM_VC150_UserID as UserId from T170011' + '  &tbname=' + c ,'Search',500,450);
}           


function OpenMachineIP(c)
{
wopen('../Search/Common/PopSearch.aspx?ID=select MM_VC20_MIP as ID, MM_IN4_MCode as MachineCode, MM_VC20_MName as MachineName from T130011' + '  &tbname=' + c ,'Search',500,450);
}                       
	function addToParentList(Afilename,TbName,strName)
				{			
				
					if (Afilename != "" || Afilename != 'undefined')
						{
						
						varName = TbName + 'Name'
							document.getElementById(TbName).value=Afilename;
							document.getElementById(varName).value=strName;
							aa=Afilename;
						}
					else					
						{
							document.Form1.txtAB_Type.value=aa;
						}
				}


                                

function callrefresh()
{
location.href="AB_Search.aspx";
//Form1.submit();
}

function ConfirmDelete(varImgValue,varMessage)
{


if (globleID==null)
{
alert("Please select the row");
}
else
{
var confirmed
confirmed=window.confirm(varMessage);
if(confirmed==true)
{
document.Form1.txthiddenImage.value=varImgValue;
Form1.submit(); 
}
else
{
}           
}
}



function SaveEdit(varImgValue)
{

					if (varImgValue=='Edit')

					{

					if (globleID==null)

					{
							alert("Please select the row");
					}
					else
					{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
					}
					}           

					if (varImgValue=='Delete')
					{

								if (globleID==null)
								{
								alert("Please select the row");
								}
								else
								{
//									alert(globalStatus);
									if(globalStatus=='E')
									{
									alert('You cannot delete this Executed Job!');
									}
									else
									{
											var confirmed
											confirmed=window.confirm("Do You Want To Delete the selected record ?");
											if(confirmed==true)
											{           
													document.Form1.txthiddenImage.value=varImgValue;
													Form1.submit();
											}                       
									}
							}                                   
						if (varImgValue=='Close')
							{
							window.close();  
							}
						if (varImgValue=='Add')
							{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit();
							}
					}           

					if (varImgValue=='Search')
					{
					document.Form1.txthiddenImage.value=varImgValue;

					Form1.submit(); 

					}           

				if (varImgValue=='Close')

				{

				document.Form1.txthiddenImage.value=varImgValue;

				Form1.submit(); 

				}           

					if (varImgValue=='Ok')

					{

					var txtHID;

					txtHID=document.getElementById('txthiddenOType').value;

					if (txtHID =='DB')

					{

					var temp;

					temp=ComparePassword('cpnlDBUserInfo_txtPassword' , 'cpnlDBUserInfo_txtConfirmPassword') ;

					//alert(temp);

					if (temp == true)

					{ 

					document.Form1.txthiddenImage.value=varImgValue;

					Form1.submit(); 

					}

					else

					{

					alert('Database Password mismatch !');

					}

					}

					else

					{

					document.Form1.txthiddenImage.value=varImgValue;

					Form1.submit();                                                                                                             

					}                                                                                               }                                                                                                           

					if (varImgValue=='Logout')

					{

					document.Form1.txthiddenImage.value=varImgValue;

					Form1.submit(); 

					}           

					if (varImgValue=='Save')
					{
						
							
							var txtHID;
							var Mail;
							Mail=document.getElementById('cpnlObjectDeatail_txtMail_F').value;
//alert(Mail);alert(chk);

					
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
						
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


function KeyCheck(nn,rowvalues,strTempName,status)
{
//alert(status);
globalStatus=status;
globleID = nn;

globlestrTempName = strTempName;

document.Form1.txthidden.value=nn;

//Form1.submit();



var tableID='cpnlObjectDeatail_GrdAddSerach'  //your datagrids id

var table;



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



function KeyCheck55(nn,rowvalues)
{
document.Form1.txthiddenImage.value='Edit';
Form1.submit(); 
}           

function wopen(url, name, w, h)
{
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
win.resizeTo(w, h);
win.moveTo(wleft, wtop);
win.focus();
}

function ComparePassword(ctrl1,ctrl2)
{
var p1;
var p2;
p1 = document.getElementById(ctrl1).value;
p2 = document.getElementById(ctrl2).value;
if ( p1==p2 )
{
return true;
}
else
{
return false;
}
}                       


		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" onload="Hideshow();" rightMargin="0">
		<FORM id="Form1" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td vAlign="top">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<div align="right"><IMG height="2" src="../Images/top_right_line.gif" width="96"></div>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../Images/top_left_back.gif">&nbsp;</td>
											<td width="50"><IMG height="20" src="../Images/top_right.gif" width="50"></td>
											<td width="21"><A href="#"><IMG height="20" src="../Images/bt_min.gif" width="21" border="0"></A></td>
											<td width="21"><A href="#"><IMG height="20" src="../Images/bt_max.gif" width="21" border="0"></A></td>
											<td width="19"><A href="#"><IMG onclick="CloseWSS();" height="20" src="../Images/bt_clo.gif" width="19" border="0"></A></td>
											<td width="6"><IMG height="20" src="../Images/bt_space.gif" width="6"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr width="100%">
											<td background="../Images/top_nav_back.gif" height="67">
												<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
													<TR>
														<TD><asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button><asp:imagebutton id="Imagebutton1" tabIndex="1" runat="server" Width="1px" Height="1px" AlternateText="."
																CommandName="submit" ImageUrl="~/images/white.GIF"></asp:imagebutton><IMG class="PlusImageCSS" onclick="HideContents();" alt="Hide" src="../Images/left005.gif"
																name="imgHide"> <IMG class="PlusImageCSS" onclick="ShowContents();" alt="Show" src="../Images/Right005.gif"
																name="ingShow">
															<asp:label id="lblTitleLabelObjInfo" runat="server" ForeColor="Teal" Font-Bold="True" Font-Names="Verdana"
																Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">Object Info</asp:label></TD>
														<TD vAlign="middle" align="left">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
															<IMG class="PlusImageCSS" id="Save" title="Save" onclick="SaveEdit('Save');" alt="S"
																src="../Images/S2Save01.gif" border="0" name="tbrbtnSave">&nbsp; <IMG class="PlusImageCSS" id="Ok" title="Ok" onclick="SaveEdit('Ok');" alt="" src="../Images/s1ok02.gif"
																border="0" name="tbrbtnOk">&nbsp; <IMG class="PlusImageCSS" title="Reset" onclick="SaveEdit('Reset');" alt="R" src="../Images/reset_20.gif"
																border="0" name="tbrbtnReset"> <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
															<IMG class="PlusImageCSS" title="Delete" onclick="SaveEdit('Delete');" alt="D" src="../Images/s2delete01.gif"
																border="0" name="tbrbtnDelete">&nbsp;<IMG class="PlusImageCSS" title="Close" onclick="SaveEdit('Close');" alt="" src="../Images/s2close01.gif"
																border="0" name="tbrbtnClose">&nbsp; <IMG title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
														</TD>
														<TD></TD>
														<td>&nbsp;
														</td>
													</TR>
												</table>
											</td>
											<td align="right" width="152" background="../Images/top_nav_back01.gif" height="67"><IMG class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
													src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<IMG class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E" src="../icons/logoff.gif"
													border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;</td>
										</tr>
									</table>
							<tr>
								<td height="10">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../Images/main_line.gif" height="10"><IMG height="10" src="../Images/main_line.gif" width="6"></td>
											<td width="7" height="10"><IMG height="10" src="../Images/main_line01.gif" width="7"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td background="../Images/main_line02.gif" height="2"><IMG height="2" src="../Images/main_line02.gif" width="2"></td>
											<td width="12" height="2"><IMG height="2" src="../Images/main_line03.gif" width="12"></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
										<TABLE id="Table16" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
											border="0">
											<TR>
												<TD vAlign="top" colSpan="1">
													<!--  **********************************************************************-->
													<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px">
														<table width="100%" border="0">
															<TBODY>
																<tr>
																	<td colSpan="2"><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="47px" BorderWidth="0px" BorderStyle="Solid"
																			BorderColor="Indigo" Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
																			TitleBackColor="Transparent" Text="Error Message" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
																			Draggable="False">
																			<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" border="0">
																				<TR>
																					<TD vAlign="middle">
																						<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
																					<TD>
																						<asp:ListBox id="lstError" runat="server" Height="64px" Width="600" Font-Size="XX-Small"></asp:ListBox></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel></td>
																</tr>
																<tr>
																	<TD vAlign="top" align="center" width="100%"><cc1:collapsiblepanel id="cpnlObject" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
																			BorderColor="Indigo" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
																			Text="Object" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																			<TABLE borderColor="#5c5a5b" align="center" bgColor="#f5f5f5" border="0">
																				<TR>
																					<TD style="HEIGHT: 34px" borderColor="#f5f5f5">&nbsp;&nbsp;&nbsp;&nbsp;</TD>
																					<TD style="HEIGHT: 34px" vAlign="top" borderColor="#f5f5f5">
																						<asp:label id="lblObjectID" runat="server" Height="12px" Width="120px" Font-Size="XX-Small"
																							Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Object ID</asp:label>
																						<asp:textbox id="txtObjectID" runat="server" Height="18px" Width="150px" BorderStyle="Solid"
																							BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="18"
																							ReadOnly="True"></asp:textbox></TD>
																					<TD style="HEIGHT: 34px" vAlign="top" borderColor="#f5f5f5">
																						<asp:label id="lblObjectType" runat="server" Height="12px" Width="104px" Font-Size="XX-Small"
																							Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Object Type</asp:label>
																						<asp:textbox id="txtObjectTypeName" runat="server" Height="18px" Width="150px" BorderStyle="Solid"
																							BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="4"
																							ReadOnly="True"></asp:textbox><IMG class="PlusImageCSS" id="pb1" onclick="OpenW12(0,'OBTY','cpnlObject_txtObjectType');"
																							alt="Process Type" src="../Images/Plus.gif" border="0">
																						<asp:textbox id="txtObjectType" runat="server" Height="0px" Width="0px" BorderStyle="Solid" BorderWidth="1px"
																							Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="4" ReadOnly="True"></asp:textbox></TD>
				<TD style="HEIGHT: 34px" vAlign="top" borderColor="#f5f5f5">
		<asp:label id="Label16" runat="server" Height="12px" Width="204px" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Date</asp:label><br />
		<%--<UC1:DATESELECTOR id="dtObjectDetail" runat="server" width="135pt"></UC1:DATESELECTOR>--%>
            <ION:Customcalendar ID="dtObjectDetail" width="135pt" runat="server" />
                                  </TD>
          	<TD style="HEIGHT: 34px" vAlign="top" borderColor="#f5f5f5"></TD>
								</TR>
								<TR>
								<TD borderColor="#f5f5f5"></TD>
								<TD vAlign="top" borderColor="#f5f5f5">
																						<asp:label id="lblObjectName" runat="server" Height="12px" Width="104px" Font-Size="XX-Small"
																							Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Object Name</asp:label>
																						<asp:textbox id="txtObjectName" runat="server" Height="18px" Width="150px" BorderStyle="Solid"
																							BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="250"
																							DESIGNTIMEDRAGDROP="124"></asp:textbox></TD>
																					<TD vAlign="top" borderColor="#f5f5f5">
																						<asp:label id="lblObjectPath" runat="server" Height="12px" Width="99px" Font-Size="XX-Small"
																							Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Object Path</asp:label>
																						<asp:TextBox id="txtObjectPath" runat="server" Height="18px" Width="150px" BorderStyle="Solid"
																							BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="250"></asp:TextBox></TD>
																					<TD style="HEIGHT: 56px" vAlign="top" borderColor="#f5f5f5" rowSpan="2">
																						<asp:label id="lblObjectDescription" runat="server" Height="12px" Width="128px" Font-Size="XX-Small"
																							Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Object Description</asp:label>
																						<asp:TextBox id="txtObjectDescription" runat="server" Height="60px" Width="340px" BorderStyle="Solid"
																							BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="100"
																							TextMode="MultiLine"></asp:TextBox></TD>
																				</TR>
																				<TR>
																					<TD style="HEIGHT: 20px" borderColor="#f5f5f5"></TD>
																					<TD style="HEIGHT: 20px" vAlign="top" borderColor="#f5f5f5">&nbsp;
																						<asp:label id="Label8" runat="server" Height="12px" Width="144px" Font-Size="XX-Small" Font-Names="Verdana"
																							Font-Bold="True" ForeColor="DimGray">Domain Name</asp:label>
																						<asp:TextBox id="txtDomainName" runat="server" Height="18px" Width="144px" CssClass="txtNoFocus"></asp:TextBox><IMG class="PlusImageCSS" onclick="OpenDomainName('cpnlObject_txtDomain');" alt="Call Owner"
																							src="../Images/Plus.gif" border="0">
																						<asp:TextBox id="txtDomain" runat="server" Height="18px" Width="0px" CssClass="txtNoFocus"></asp:TextBox></TD>
																					<TD style="HEIGHT: 20px" vAlign="top" borderColor="#f5f5f5">&nbsp;
																						<asp:label id="lblObjectMachineCode" runat="server" Height="12px" Width="136px" Font-Size="XX-Small"
																							Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Object Machine</asp:label>
																						<asp:textbox id="txtObjectMachineCodeName" runat="server" Height="18px" Width="150px" BorderStyle="Solid"
																							BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="10"
																							ReadOnly="True"></asp:textbox><IMG class="PlusImageCSS" onclick="OpenMachineCode('cpnlObject_txtObjectMachineCode');"
																							alt="Call Owner" src="../Images/Plus.gif" border="0">
																						<asp:textbox id="txtObjectMachineCode" runat="server" Height="0" Width="0px" BorderStyle="Solid"
																							BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="10"
																							ReadOnly="True"></asp:textbox></TD>
																				</TR>
																				<TR>
																					<TD style="HEIGHT: 33px" borderColor="#f5f5f5"></TD>
																					<TD style="HEIGHT: 33px" vAlign="top" borderColor="#f5f5f5">
																						<P>&nbsp;&nbsp;
																						</P>
																					</TD>
																					<TD style="HEIGHT: 33px" vAlign="top" borderColor="#f5f5f5">
																						<P>
																							<asp:TextBox id="txtObjectProcessCodeName" runat="server" Height="18px" Width="150px" BorderStyle="Solid"
																								BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" Visible="False" CssClass="txtNoFocus"
																								MaxLength="50" ReadOnly="True"></asp:TextBox>
																							<asp:TextBox id="txtObjectProcessCode" runat="server" Height="0px" Width="8px" BorderStyle="Solid"
																								BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" Visible="False" CssClass="txtNoFocus"
																								MaxLength="50" ReadOnly="True"></asp:TextBox><IMG class="PlusImageCSS" onclick="OpenProcessCode('cpnlObject_txtObjectProcessCode');"
																								alt="Call Owner" width="0" border="0">
																							<asp:label id="lblObjectProcessCode" runat="server" Height="12px" Width="144px" Font-Size="XX-Small"
																								Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray" Visible="False">Object Process</asp:label></P>
																					</TD>
																				</TR>
																				<TR>
																					<TD borderColor="#f5f5f5"></TD>
																					<TD vAlign="top" borderColor="#f5f5f5"></TD>
																					<TD vAlign="top" borderColor="#f5f5f5"></TD>
																				</TR>
																			</TABLE>
																		</cc1:collapsiblepanel></TD>
																</tr>
																<TR>
																	<td><cc1:collapsiblepanel id="cpnlObjectDeatail" runat="server" Width="100%" Height="47px" BorderWidth="0px"
																			BorderStyle="Solid" BorderColor="Indigo" Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
																			TitleClickable="True" TitleBackColor="Transparent" Text="Object Detail" ExpandImage="../Images/ToggleDown.gif"
																			CollapseImage="../Images/ToggleUp.gif" Draggable="False">
																			<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 260pt">
																				<TABLE id="Table1261" borderColor="activeborder" cellSpacing="0" cellPadding="0" width="100%"
																					align="left" border="0">
																					<TR>
																						<TD>
																							<asp:panel id="Panel1" runat="server"></asp:panel></TD>
																					</TR>
																					<TR>
																						<TD vAlign="top" align="left"><!--  **********************************************************************-->
																							<asp:datagrid id="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px" Font-Names="Verdana"
																								ForeColor="MidnightBlue" BorderColor="Silver" CssClass="grid" CellPadding="0" GridLines="Horizontal"
																								HorizontalAlign="Left" PageSize="50" DataKeyField="ID">
																								<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
																								<SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
																								<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																								<ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"></ItemStyle>
																								<HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0"></HeaderStyle>
																								<PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
																							</asp:datagrid><!-- Panel for displaying Task Info --> <!-- Panel for displaying Action Info-->  <!-- ***********************************************************************--></TD>
																					</TR>
																					<TR>
																						<TD>
																							<TABLE cellSpacing="0" cellPadding="0" border="0">
																								<TR>
																									<TD>
																										<asp:TextBox id="txtOIDPK_F" tabIndex="1" width="41pt" Height="18" CssClass="txtNoFocusFE" Runat="server"></asp:TextBox></TD> <TD>
	<%--<UC1:DATESELECTOR id="dtObjectDetail_F" runat="server" width="106pt"></UC1:DATESELECTOR>--%>                      <ION:Customcalendar ID="dtObjectDetail_F" width="106pt" runat="server" />
	                                        </TD>
											<TD>
	<asp:TextBox id="txtLimit_F" tabIndex="3" Height="18" Width="52pt" CssClass="txtNoFocusFE" MaxLength="18"
				Runat="server"></asp:TextBox></TD>
										<TD></TD>
											<TD>
	<asp:TextBox id="txtAlertType_F" tabIndex="5" Height="" Width="0" CssClass="txtNoFocusFE" ReadOnly="True"
				Runat="server"></asp:TextBox>
	<asp:TextBox id="txtAlertType_FName" tabIndex="5" Height="18" Width="51pt" CssClass="txtNoFocusFE"
																											Runat="server"></asp:TextBox><IMG class="PlusImageCSS" id="Img2" onclick="OpenW('cpnlObjectDeatail_txtAlertType_F');"
																											alt="Process Type" src="../Images/Plus.gif" border="0">
																									</TD>
																									<TD>
																										<asp:TextBox id="txtMail_F" tabIndex="6" Height="18" Width="241pt" CssClass="txtNoFocusFE" MaxLength="75"
																											Runat="server"></asp:TextBox></TD>
																									<TD>
																										<asp:RadioButtonList id="rblType" runat="server" Width="80pt" RepeatDirection="Horizontal">
																											<asp:ListItem Value="F" Selected="True">Fix</asp:ListItem>
																											<asp:ListItem Value="P">%age</asp:ListItem>
																										</asp:RadioButtonList></TD>
																								</TR>
																							</TABLE>
																						</TD>
																					</TR>
																				</TABLE>
																			</DIV>
																		</cc1:collapsiblepanel></td>
												</TD>
											</TR>
										</TABLE>
									</DIV>
								</td>
								<td vAlign="top" width="12" background="../Images/main_line04.gif"><IMG height="1" src="../Images/main_line04.gif" width="12"></td>
							</tr>
						</table>
										</tr>
				<tr>
					<td height="2">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td background="../Images/main_line06.gif" height="2"><IMG height="2" src="../Images/main_line06.gif" width="2"></td>
								<td width="12" height="2"><IMG height="2" src="../Images/main_line05.gif" width="12"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td background="../Images/bottom_back.gif">&nbsp;</td>
								<td width="66"><IMG height="31" src="../Images/bottom_right.gif" width="66"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			
			<DIV></DIV>
			<INPUT type="hidden" name="txthidden"> <INPUT type="hidden" name="txthiddenImage">
		</FORM>
	</body>
</html>
