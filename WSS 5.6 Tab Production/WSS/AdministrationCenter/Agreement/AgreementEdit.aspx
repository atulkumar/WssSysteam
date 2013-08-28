<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AgreementEdit.aspx.vb" Inherits="AdministrationCenter_Agreement_AgreementEdit" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Agreement Edit</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script type="text/javascript">
			 
		function OpenW(a,b,c)
				{
				
				//wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				//window.showModalDialog
			wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,"",500,450);
				}
				
		function OpenComm(a,b)
				{
				
				wopen('comment.aspx?ScrID=329&ID='+ b + '&tbname=T' ,'Comments',500,450);
				
				}
				
		function OpenAtt()
				{
						
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=T','Additional_Address',400,450);
				
				}
		function OpenAB(c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'',500,450);										
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName, ab.CI_VC36_Name as Name, UL_VC8_Role_PK as Role from T010011 ab, T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'',500,450);					
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
					}
					else
					
					{
						//document.Form1.txtAB_Type.value=aa;
					}
				}
				

						
					
				
				
				
					function SaveEdit(varimgValue)
				{
			    		
												
						if (varimgValue=='Close')
						{
								window.close();
								return false; 
						}
								
								
															
						if (varimgValue=='Ok')
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
					
									document.Form1.txthiddenImage.value=varimgValue;
									Form1.submit();
									window.close(); 
									return false; 
									 
						}
								
						if (varimgValue=='Save')
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
					
									document.Form1.txthiddenImage.value=varimgValue;
									Form1.submit();
									return false; 
						}		
							
						if (varimgValue=='Reset')
						{
									var confirmed
									confirmed=window.confirm("Do You Want To reset The Page ?");
									if(confirmed==true)
											{	
													Form1.reset();
													return false;
											}		

						}			
				}			
					
					function callrefresh()
				{
					document.Form1.txthiddenImage.value='';
			    	Form1.submit();
				}
				
				
					function FP_swapimg() {//v1.0
							var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
							n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
							elm.$src=elm.src; elm.src=args[n+1]; } }
							}

							function FP_preloadimgs() {//v1.0
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

    <script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.HideModalDiv)
                opener.parent.HideModalDiv();
                self.opener.callrefresh();
            }
        }
        //Modified By Atul to execute script on Page Load
        function OnLoad() {
           if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.LoadModalDiv)
                opener.parent.LoadModalDiv();
            }
        }
        window.onload = OnLoad;
        window.onunload = OnClose;
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="table1" style="border-color: ActiveBorder" cellspacing="0" cellpadding="0"
        width="100%" background="../../images/top_nav_back.gif" border="0">
        <tr>
            <td>
                &nbsp;
                <asp:Label ID="lblTitleLabelAggEdit" runat="server" ForeColor="Teal" Font-Bold="true"
                    Font-Names="Verdana" Font-Size="X-Small" Width="150px" BorderWidth="2px" BorderStyle="None">Agreement Edit</asp:Label>
            </td>
            <td>
                <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0" />&nbsp;
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                    ToolTip="Save"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif"
                    ToolTip="Ok"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                    ToolTip="Reset"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                    ToolTip="Close"></asp:ImageButton>&nbsp;&nbsp;<img title="Seperator" alt="R" src="../../Images/00Seperator.gif"
                        border="0" />
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="table2" style="width: 272px; height: 19px" bordercolor="lightgrey" cellspacing="0"
        cellpadding="0" width="272" border="0">
        <tr>
            <td style="width: 32px" colspan="0" rowspan="0">
                <table id="table4" bordercolor="#f5f5f5" width="400" bgcolor="#f5f5f5" border="1">
                    <tr>
                        <td valign="top" bordercolor="#f5f5f5" align="left" width="422" rowspan="1">
                            <asp:Label ID="Label1" runat="server" ForeColor="Dimgray" Font-Bold="true" Font-Names="Verdana"
                                Font-Size="XX-Small" Width="80px"> Customer</asp:Label><br>
                            <asp:TextBox ID="txtCustomer" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                Width="129px" BorderWidth="1px" BorderStyle="Solid" Height="18px" MaxLength="8"
                                ReadOnly="true" CssClass="txtNoFocus"></asp:TextBox>
                            <input type="hidden" id="txtLineNo" runat="server" />
                        </td>
                        <td valign="top" bordercolor="#f5f5f5" align="left" width="422" height="24" rowspan="1">
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" bordercolor="#f5f5f5" align="left" width="422" rowspan="1">
                            <asp:Label ID="lblName3" runat="server" ForeColor="Dimgray" Font-Bold="true" Font-Names="Verdana"
                                Font-Size="XX-Small" Width="80px">Call Type</asp:Label><br>
                            <uc1:CustomDDL ID="cddlcall" runat="server"></uc1:CustomDDL>
                        </td>
                        <td valign="top" bordercolor="#f5f5f5" align="left" width="422" height="24" rowspan="1">
                            <asp:Label ID="Label2" runat="server" ForeColor="Dimgray" Font-Bold="true" Font-Names="Verdana"
                                Font-Size="XX-Small" Width="112px" Height="12px"> Agreemwnt No.</asp:Label>
                            <asp:TextBox ID="txtAGNo" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                Width="129px" BorderWidth="1px" BorderStyle="Solid" Height="18px" MaxLength="9"
                                ReadOnly="true" CssClass="txtNoFocus"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" bordercolor="#f5f5f5" align="left" width="422">
                            <asp:Label ID="Label3" runat="server" Width="80px" Font-Size="XX-Small" Font-Names="Verdana"
                                Font-Bold="true" ForeColor="Dimgray">Task Type</asp:Label><br>
                            <uc1:CustomDDL ID="cddlTask" runat="server"></uc1:CustomDDL>
                        </td>
                        <td valign="top" bordercolor="#f5f5f5" align="left" width="422">
                            <asp:Label ID="lblName6" runat="server" ForeColor="Dimgray" Font-Bold="true" Font-Names="Verdana"
                                Font-Size="XX-Small" Width="85px" Height="12px"> Price</asp:Label>
                            <asp:TextBox ID="txtPrice" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                Width="129px" BorderWidth="1px" BorderStyle="Solid" Height="18px" MaxLength="9"
                                CssClass="txtNoFocus"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" bordercolor="#f5f5f5" align="left" width="422">
                            <asp:Label ID="lblName9" runat="server" ForeColor="Dimgray" Font-Bold="true" Font-Names="Verdana"
                                Font-Size="XX-Small" Width="115px" Height="13px">Skill Level</asp:Label><br>
                            <uc1:CustomDDL ID="cddllevel" runat="server"></uc1:CustomDDL>
                            <br>
                        </td>
                        <td valign="top" bordercolor="#f5f5f5" align="left" width="422" height="21">
                            <asp:Label ID="lblName8" runat="server" ForeColor="Dimgray" Font-Bold="true" Font-Names="Verdana"
                                Font-Size="XX-Small" Width="85px" Height="12px">ChargeBasis</asp:Label>
                            <asp:RadioButtonList ID="rblHour" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                Width="149px" RepeatDirection="Horizontal">
                                <asp:ListItem Value="H" Selected="true">Hourly</asp:ListItem>
                                <asp:ListItem Value="F">Fixed</asp:ListItem>
                            </asp:RadioButtonList>
                            <br>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="352px"
                            Visible="false" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                        <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
                    </ContentTemplate>
                </asp:UpdatePanel>
    </form>
    </td> </tr> </table>
</body>
</html>
