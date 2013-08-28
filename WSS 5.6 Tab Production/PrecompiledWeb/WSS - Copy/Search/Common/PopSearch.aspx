<%@ page language="VB" autoeventwireup="false" inherits="Search_Common_PopSearch, App_Web_c_vc503s" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../../Images/Js/PopSearchShortCuts.js"></script>
		<script language="javascript">
		
		//var globalid;
			var gID;
			var gName;
		var gRN;
		var gNameP;
		gRN=0;
		var tableID;
		var MAX;
		MAX=-1;
		var arID;
		var arNameP;
				
		var globalid;
		var globalUDC;
		var globalTxtbox;
		var globalstrName;
		
		function closeMe()
			{
			var ret;
			ret=document.getElementById("txtValue").value;
			window.returnValue=ret;
			window.close();	
			}
			
	function checkArrows (field, evt, ID, NameP,txtID) 
	{
	globalTxtbox=txtID;
	var str;
	str=ID;
	arID=str.split('###');
	MAX=arID.length-1;
	arNameP=NameP.split('###');

		var keyCode = 
		document.layers ? evt.which :
		document.all ? event.keyCode :
		document.getElementById ? evt.keyCode : 0;
		if ( gRN > MAX )
		{
			gRN=gRN-1;
		}
		if ( MAX > 0 )
		{
				if (keyCode == 40)
				{
					if ( gRN==MAX ) gRN=0;
					//alert(gRN);
					DownKeyCheck(gRN+1);
				}
				else if (keyCode == 38)
				{
					if ( gRN==0 ) gRN=MAX+1;
					UpKeyCheck(gRN-1);
				}
		}
		return true;
	}
	
	function Select(txtParent)
	{
	globalUDC=gID;
	globalTxtbox=txtParent;
		if ( gID != null)
			{
				if (event.keyCode==13)
				{
							self.opener.addToParentList(globalUDC,globalTxtbox, gName);
							window.close();				
				}
			}
	}			

		function KeyCheck(kID,kName,IN,bb,ID,dd,NameP)
		{
			gID=kID;
			gName=kName;
			globalid=gID;
		//alert(kID);
			globalUDC=IN;
			globalTxtbox = bb;
			var str;
			str=ID;
			arID=str.split('###');
			arNameP=NameP.split('###');
			MAX=arID.length-1;
			gRN=IN-1;
			gID=arID[gRN];
			gName=arNameP[gRN];
			gRN=parseInt(gRN);
			tableID=document.getElementById('DataGrid1');
			if (tableID && IN!=0)
			{
				for(var i=1;i< tableID.rows.length;i++)
				{
					if(i % 2 == 0)
					{
					tableID.rows[i].style.backgroundColor="#f5f5f5";
					}
					else
					{
					tableID.rows[i].style.backgroundColor="#ffffff";
					}
				}
				tableID.rows[IN].style.backgroundColor="#d4d4d4";
				//document.getElementById('Textbox1').value=gID;
				document.getElementById('txtFastSearch').value=gName;
			}
				
		}



		function DownKeyCheck(SR)
		{
		globalid=gID;
			tableID=document.getElementById('DataGrid1');
			if (tableID && SR!=0)
			{
				for(var i=1;i< tableID.rows.length;i++)
				{
					if(i % 2 == 0)
					{
					tableID.rows[i].style.backgroundColor="#f5f5f5";
					}
					else
					{
					tableID.rows[i].style.backgroundColor="#ffffff";
					}
				}
				tableID.rows[SR].style.backgroundColor="#d4d4d4";
				gID=arID[gRN];
				gName=arNameP[gRN];
				//document.getElementById('TextBox1').value=gID;
				document.getElementById('txtFastSearch').value=gName;
			}
			gRN=gRN+1;
			if ( gRN >= MAX )
			{
				gRN=0;
			}
			
		}	
		function UpKeyCheck(SR)
		{
		globalid=gID;
			tableID=document.getElementById('DataGrid1');
			if (tableID && SR!=0)
			{
				for(var i=1;i< tableID.rows.length;i++)
				{
					if(i % 2 == 0)
					{
					tableID.rows[i].style.backgroundColor="#f5f5f5";
					}
					else
					{
					tableID.rows[i].style.backgroundColor="#ffffff";
					}
				}
				tableID.rows[SR].style.backgroundColor="#d4d4d4";
				gID=arID[gRN-2];
				gName=arNameP[gRN-2];
				//document.getElementById('TextBox1').value=gID;
				document.getElementById('txtFastSearch').value=gName;
			}
			gRN=gRN-1;
			if ( gRN <= 0 )
			{
				gRN=MAX+1;
			}
			
		}			
				function KeyCheck55(aa,bb,cc,strName)
				{
					//self.opener.addToParentList(aa,bb);
					globalUDC=gID;
							self.opener.addToParentList(globalUDC,globalTxtbox,gName);
							window.close();				
				}

						function closeWindow(varImgValue)
							{
								globalUDC=gID;
							//alert(gName);
											if (varImgValue=='Ok')
												{
															if (globalid==null)
															{
																alert("Please select the row");
															}
															else
															{
															self.opener.addToParentList(globalUDC,globalTxtbox,gName);
															window.close();	
															return false;
															}
															
												}	
												
												if (varImgValue=='Close')
												{
															window.close();
															return false;	
												}
						}

						
							function FP_swapImg() {//v1.0
							var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
							n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
							elm.$src=elm.src; elm.src=args[n+1]; } }
							}

							function FP_preloadImgs() {//v1.0
							var d=document,a=arguments; if(!d.FP_imgs) d.FP_imgs=new Array();
							for(var i=0; i<a.length; i++) { d.FP_imgs[i]=new Image; d.FP_imgs[i].src=a[i]; }
							}

							function FP_getObjectByID(id,o) {//v1.0
							var c,el,els,f,m,n; if(!o)o=document; if(o.getElementById) el=o.getElementById(id);
							else if(o.layers) c=o.layers; else if(o.all) el=o.all[id]; if(el) return el;
							if(o.id==id || o.name==id) return o; if(o.childNodes) c=o.childNodes; if(c)
							for(n=0; n<c.length; n++) { el=FP_getObjectByID(id,c[n]); if(el) return el; }
							f=o.forms; if(f) for(n=0; n<f.length; n++) { els=f[n].elements;
							for(m=0; m<els.length; m++){ el=FP_getObjectByID(id,els[n]); if(el) return el; } }
							return null;
							}
	
		</script>
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" onload="document.getElementById('DataGrid1').focus();"
		rightMargin="0">
    <form id="Form1" runat="server">
   <TABLE id="Table1" style="LEFT: 0px; WIDTH: 528px; POSITION: absolute; TOP: 0px; HEIGHT: 40px"
				borderColor="activeborder" height="40" cellSpacing="0" cellPadding="0" width="528"
				bgColor="#0000e0" border="0">
				<TR>
					<TD style="WIDTH: 277px" vAlign="top" align="left" width="277" bgColor="lightgrey" height="16">&nbsp;
						<asp:button id="Button1" runat="server" Height="0" Width="0" Text="" 
                            BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:button><asp:label id="Label1" runat="server" Height="8px" Width="104px" BorderStyle="None" BorderWidth="2px"
							Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal">UDC SEARCH</asp:label>
						<asp:TextBox ID="txtFastSearch" Runat="server" CssClass="txtFocus" Width="150" Height="18"></asp:TextBox>
					</TD>
					<td vAlign="top" align="left" bgColor="lightgrey" colSpan="1" rowSpan="1" style="WIDTH: 166px"><asp:imagebutton id="imgOk" accessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="imgClose" accessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"></asp:imagebutton>&nbsp;</td>
					<!-- 
						onload="FP_preloadImgs(/*url*/'images/buttonE.jpg', /*url*/'images/buttonF.jpg', /*url*/'images/button11.jpg', /*url*/'images/button12.jpg', /*url*/'images/button14.jpg', /*url*/'images/button15.jpg', /*url*/'images/button17.jpg', /*url*/'images/button18.jpg', /*url*/'images/button1A.jpg', /*url*/'images/button1B.jpg', /*url*/'button52.jpg', /*url*/'button53.jpg')"
						onmouseover="FP_swapImg(1,0,/*id*/'img1',/*url*/'../../Images/s2close01.gif')" onmouseout="FP_swapImg(0,0,/*id*/'img1',/*url*/'../../Images/s1ok02.gif')" onmousedown="FP_swapImg(1,0,/*id*/'img1',/*url*/'images/buttonF.jpg')" onmouseup="FP_swapImg(0,0,/*id*/'img1',/*url*/'images/buttonE.jpg')"
						<IMG title="Search" onclick="Reopenw();"  alt="S" src="../../Images/s1search02.gif" border="0" name="tbrbtnSearch">--> 
					</TD>
					<td align="right" bgColor="lightgrey" vAlign="top"><IMG title="Help" alt="S" src="../../Images/s1question02.gif" border="0" name="tbrbtnHelp">&nbsp;&nbsp;
					</td>
				</TR>
			</TABLE>
			<TABLE id="Table16" style="LEFT: 0px; WIDTH: 528px; POSITION: absolute; TOP: 36px; HEIGHT: 569px"
				borderColor="activeborder" height="569" cellSpacing="0" cellPadding="0" width="528"
				border="1">
				<TR>
					<TD vAlign="top" colSpan="1">
						<table style="WIDTH: 520px; HEIGHT: 152px" width="520" border="0">
							<tr>
								<td colSpan="2"><cc1:collapsiblepanel id="cpnlErrorPanel" runat="server" Height="47px" Width="100%" Text="Error Message"
										BorderStyle="Solid" BorderWidth="0px" Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
										TitleClickable="True" TitleBackColor="Transparent" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
										Draggable="False" BorderColor="Indigo">
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
							<!-- **************************************************************************-->
							<tr>
								<td vAlign="top" colSpan="2" height="1"><asp:panel id="pndgsrch" runat="server" Width="96.66%">Panel</asp:panel>
									<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 443px"><asp:datagrid id="DataGrid1" runat="server" Width="472px" BorderStyle="None" BorderWidth="1px"
											Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" datakeyfield="ID" cellpadding="0" PagerStyle-Visible="False" font-size="8pt">
											<FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
											<SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
											<AlternatingItemStyle CssClass="OddTableRow"></AlternatingItemStyle>
											<ItemStyle Font-Size="8pt" ForeColor="Black" BackColor="White"></ItemStyle>
											<HeaderStyle Font-Size="8pt" Font-Bold="True" CssClass="GridFixedHeader"></HeaderStyle>
											<Columns>
												<asp:ButtonColumn Visible="False" CommandName="select">
													<ItemStyle Width="12pt"></ItemStyle>
												</asp:ButtonColumn>
											</Columns>
											<PagerStyle Visible="False" HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
										</asp:datagrid></div>
								</td>
							</tr>
							<!-- *****************************************************************************--></table>
					</TD>
				</TR>
			</TABLE>
			<INPUT style="LEFT: 0px; POSITION: absolute; TOP: 547px" type="hidden" name="txthidden">
			<asp:button id="btsearch" style="LEFT: 0px; POSITION: absolute; TOP: 547px" runat="server" text=""
				width="0%"></asp:button>
    </form>
</body>
</html>
