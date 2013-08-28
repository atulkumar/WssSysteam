<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SideMenu.aspx.vb" Inherits="Menu_SideMenu" %>
<%@ Register TagPrefix="mytab" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls,Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ import namespace="Microsoft.Web.UI.WebControls"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>DATA BASE TREE MENU WITH TREEVIEW</title><!--<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>-->
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<META http-equiv="Content-Style-Type" content="text/css">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
         <link href="../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
		<script language="javascript" type="text/javascript" src="../../../images/Js/JSValidation.js"></script>
		<script language="javascript"  type="text/javascript" src="../../../images/Js/FastPathShortCut.js"></script>
		<script language="Javascript" type="text/javascript">
		
		var rand_no = Math.ceil(500*Math.random())
			function ParentNode(NodeName)
			{
				CallAjax(NodeName,0);
				return false;
			}
			
			function ChildNode(NodeName, NodeID)
			{
				CallAjax(NodeName, 1);
				return false;
			}
				

                 
		///***********************Call View AJAX**********************************////////
		var gtype;
		var xmlHttp; 
		var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
		var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
		var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
		//netscape, safari, mozilla behave the same??? 
		var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 

		function CallAjax(Name, ID)
		{
					    var url= '../AJAX Server/AjaxInfo.aspx?Type=MenuSession&Name='+ Name +'&ID=' + ID +'&RKey='+ Math.random(); 
						xmlHttp = GetXmlHttpObject(stateChangeHandler);    
						xmlHttp_Get(xmlHttp, url); 
		}
		 
	
		 
		function stateChangeHandler() 
		 { 	
				if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
				{ 
						//document.getElementById('imgAjax').style.display='none';//src="../images/divider.gif";
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
									switch(intT)
									{
										case 0:
										{								
											
											for (var inti=0; inti<item.length; inti++)
											{
											
												
											}
											break;
										}//case 0
									}//switch
								} //for loop
						}
				} 
				else
				{
					//	document.getElementById('imgAjax').src="../../images/ajax.gif";
						//document.getElementById('imgAjax').style.display='inline';
				}
		} 
		
		
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
    
    
		///**************************Call View AJAX end*********************************///////

				
  			function OpenW(c)
			{
				var Query;
				var UName;
				var RoleID;
				var NowDate;
				UName=document.getElementById('HIDUserName').value;
				RoleID=document.getElementById('HIDRoleID').value;
				NowDate=document.getElementById('HIDNowDate').value;
				Query="select  OBM.OBM_VC8_FPath  ID ,OBM.OBM_VC50_Alias_Name ScreenName ,OBM.OBM_VC4_Status_Code_FK Status from T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA WHERE UM.UM_VC50_UserID ='" + UName + "' AND UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND RA.RA_DT8_Valid_UpTo >='"+ NowDate +"' AND RA.RA_VC4_Status_Code = 'ENB' AND RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND ROM.ROM_DT8_End_Date >= '"+ NowDate +"' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND OBM.OBM_VC4_Object_Type_FK ='SCR' and ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" + RoleID + " AND OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and OBM.OBM_VC8_FPath<>'' AND ROD.ROD_CH1_View_Hide='V'  and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_vc4_object_type_fk='SCR')"
				wopen('../Search/Common/PopSearch1.aspx?ID=' + Query +  ' &tbname=' + c ,'Search'+rand_no,500,450);				
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
			
			function addToParentList(Afilename,TbName)
				{
				
					if (Afilename != "" || Afilename != 'undefined')
					{
					//alert(Afilename);
						document.getElementById(TbName).value=Afilename;
						aa=Afilename;
						document.getElementById('txtFastPath').focus();
						document.frmTransterRight.submit(); 
					}
					else
					
					{
						document.Form1.txtAB_Type.value=aa;
					}
				}
				
				function Submit()
				{
			
					if ( event.keyCode==13 )
					{
					//	alert();
					//	document.frmTransterRight.submit();	
					}
				}
		</script>
		<style type="text/css">.style1 { FONT-WEIGHT: bold; FONT-SIZE: 12px; COLOR: #ffffff; FONT-FAMILY: Arial, Helvetica, sans-serif }
		</style>
	</HEAD>
	<body alink="blue" vlink="blue" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="frmTransterRight" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="163" border="0">
				<tr>
					<th vAlign="top" scope="row">
						<table height="481" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<th scope="row" height="2">
								</th>
							</tr>
							<tr>
								<th scope="row">
									<div align="left">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<th scope="row" width="18">
													<IMG height="20" src="../images/top_left.gif" width="18"></th>
												<td background="../images/top_left_back01.gif"><span class="style1">ION Softnet WSS</span>
												</td>
											</tr>
										</table>
									</div>
								</th>
							</tr>
							<tr>
								<th scope="row">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<th scope="row" width="22">
												<IMG height="67" src="../images/logo_left.gif" width="22"></th>
											<td align="center" width="127" background="../images/logo_middle.gif"><A  title="WSS Home" href="../Home.aspx" target="MainPage"><asp:Label  style="text-decoration: none"  ID="lblCompLogo" Runat="server" Font-Name="Verdana" Font-Size="10px" Font-Bold="True" ForeColor="darkblue" >
											ION Softnet
											</asp:Label>
											<br>
											<asp:Label style="text-decoration: none" ID="lblEnv" Runat="server" Font-Name="Verdana"
													Font-Size="10px" Font-Bold="True" ForeColor="DarkGreen" >WSS4.4</asp:Label></A>
											</td>
											<td width="16"><IMG height="67" src="../images/logo_right.gif" width="16"></td>
										</tr>
									</table>
								</th>
							</tr>
							<tr>
								<th scope="row">
									<IMG height="10" src="../images/top_line.gif" width="163"></th></tr>
							<tr>
								<th scope="row">
									<IMG height="3" src="../images/top_line01.gif" width="163"></th></tr>
							<tr>
								<th width="163" vAlign="top" scope="row" background="../images/top_line03.gif" height="100%">
									<div align="left">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="9%">&nbsp;&nbsp;&nbsp;
												</td>
												<td width="91%">
													<DIV style="background-color:#F5F5F5;OVERFLOW: auto; WIDTH: 140px; HEIGHT: 525px"><asp:panel id="pnlMenu" runat="server"></asp:panel></DIV>
												</td>
											</tr>
											<tr >
												<td>&nbsp;</td>
												<td align="left" >
													<table  cellSpacing="0" cellPadding="0" width="100%" border="0">
														<tr>
															<td style="HEIGHT: 9px" width="1%"></td>
															<td align="left" ><asp:label id="lblUserID" Runat="server" Font-Names="Verdana" Font-Size="XX-Small"></asp:label></td>
														</tr>
														<tr>
															<td style="HEIGHT: 3px" width="1%"></td>
															<td></td>
														</tr>
														<tr>
															<td width="1%"></td>
															<td width="99%" align="left">
																<asp:label id="lblrole" Runat="server" Font-Names="Verdana" Font-Size="XX-Small">Role</asp:label>
																<asp:dropdownlist id="ddlRoleName" runat="server" Font-Names="Verdana" Font-Size="XX-Small" AutoPostBack="True"
																	Width="85px"></asp:dropdownlist>
																<SCRIPT language="JavaScript">
																// Anytime Anywhere Web Page Clock Generator
																// Clock Script Generated at
																// http://www.rainbow.arch.scriptmania.com/tools/clock

																function tS(){ x=new Date(); x.setTime(x.getTime()); return x; } 
																function lZ(x){ return (x>9)?x:'0'+x; } 
																function tH(x){ if(x==0){ x=12; } return (x>12)?x-=12:x; } 
																function y2(x){ x=(x<500)?x+1900:x; return String(x).substring(2,4) } 
																//function dT(){ window.status=''+eval(oT)+''; document.title=''+eval(oT)+''; if(fr==0){ fr=1; document.write('<font size=1 face=verdana color=midnightblue><b><span id="tP">'+eval(oT)+'</span></b></font>'); } tP.innerText=eval(oT); setTimeout('dT()',1000); } 
																function dT(){ window.status=''+eval(oT)+''; document.title=''+eval(oT)+''; if(fr==0){ fr=1; document.write('<font size=1 face=verdana color=midnightblue><b><span id="tP">'+eval(oT)+'</span></b></font>'); } tP.innerText=''; setTimeout('dT()',1000); } 
																function aP(x){ return (x>11)?'pm':'am'; } 
																var dN=new Array('Sun','Mon','Tue','Wed','Thu','Fri','Sat'),mN=new Array('Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'),fr=0,oT="dN[tS().getDay()]+' '+tS().getDate()+' '+mN[tS().getMonth()]+' '+y2(tS().getYear())+' '+' '+' '+tH(tS().getHours())+':'+lZ(tS().getMinutes())+':'+lZ(tS().getSeconds())+' '+aP(tS().getHours())";
																</SCRIPT>
																<!--	 Clock Part 1 - Ends Here  --> <!-- Clock Part 2 - This Starts/Displays Your Clock -->
																<SCRIPT language="JavaScript">dT();</SCRIPT>
																<br>
																<FONT face="Verdana" color="midnightblue" size="1"><STRONG>Fast Path</STRONG> </FONT>
																<asp:textbox id="txtFastPath" runat="server" Font-Size="xx-small" MaxLength="4" BorderWidth="1px"
																	BorderStyle="Solid" Width="40px" CssClass="txtFocus" Height="15px" Font-Name="verdana"></asp:textbox><IMG class="PlusImageCSS" onclick="OpenW('txtFastPath');" alt="Fast Path" src="../images/plus.gif"
																	border="0">
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</div>
								</th>
							</tr>
							<tr>
								<th scope="row">
									<IMG height="3" src="../images/top_line04.gif" width="163"></th></tr>
							<tr>
								<th scope="row">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td width="10"><IMG height="31" src="../images/bottom_left.gif" width="10"></td>
											<td background="../images/bottom_back.gif"><asp:Label ID="lblEnvWEW" Runat="server" Text="&nbsp;Press F11 for best view" Font-Name="Verdana"
													Font-Size="10px" Font-Bold="True" Font-Italic="True" ForeColor="DarkBlue" ></asp:Label></td>
										</tr>
									</table>
								</th>
							</tr>
						</table>
					</th>
				</tr>
			</table>
			<input id="HIDUserName" type="hidden" name="txtHIDUserName" runat="server"> <input id="HIDRoleID" type="hidden" name="txtHIDRoleID" runat="server">
			<input id="HIDNowDate" type="hidden" name="txtHIDNowDate" runat="server">
			
		</form>
	</body>
</HTML>
