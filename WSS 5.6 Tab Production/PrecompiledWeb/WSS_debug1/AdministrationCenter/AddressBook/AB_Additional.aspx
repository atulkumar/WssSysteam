<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_AddressBook_AB_Additional, App_Web_owb2nwqw" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add/Edit Address</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript">
		
			function Characterswp(field)
			{
			field=document.getElementById('txtName').value;
		
				var valid="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
				
				var ok='yes';
				var temp;
				for(var i=0;i<field.value.length;i++)
				{alert(field);
					temp=""+field.value.substring(i,i+1);
					if(valid.indexOf(temp) == "-1") ok="no";	
					
				}
				if(ok=="no")
				{
					alert("Invalid entry! only Characters without space");
					field.focus();
					field.select();
				}
			}

			
		function OpenW(a,b,c)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
					wopen('../../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
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
						
					
				
				
				
					function SaveEdit(varImgValue)
				{
			    		
												
						if (varImgValue=='Close')
						{
								window.close(); 
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
							for(m=0; m<els.length; m++){ el=FP_getObjectByID(id,els[n]); if(el) return el; }                             }
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
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
        border="0">
        <tr>
            <td style="width: 88px">
                <asp:Label ID="lblTitleLabelAB_addi" runat="server" BorderStyle="None" BorderWidth="2px"
                    Width="140px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="true" ForeColor="Teal">Add/Edit Address</asp:Label>
            </td>
            <td width="250" height="47">
                <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0" />&nbsp;
                <asp:ImageButton ID="imgSave" runat="server" ImageUrl="../../Images/S2Save01.gif"
                    AccessKey="S" ToolTip="Save"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgOk" runat="server" ImageUrl="../../Images/s1ok02.gif" AccessKey="K"
                    ToolTip="OK"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgReset" runat="server" ImageUrl="../../Images/reset_20.gif"
                    AccessKey="R" ToolTip="Reset"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="ImgClose" runat="server" ImageUrl="../../Images/s2close01.gif"
                    AccessKey="L" ToolTip="Close"></asp:ImageButton>
                <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0" />
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="table4" style="width: 440px; height: 355px" bordercolor="#f5f5f5" height="355px"
        cellspacing="0" cellpadding="0" width="440" bgcolor="#f5f5f5" border="1">
        <tr>
            <td style="width: 341px; height: 51px" bordercolor="#f5f5f5" width="341px" height="51">
                <table id="table5" height="8" width="330" border="0">
                    <tr>
                        <td width="5">
                            &nbsp;
                        </td>
                        <td style="height: 29px" width="162">
                            <asp:Label ID="lblName" runat="server" Width="40px" Font-Size="XX-Small" Font-Names="Verdana"
                                Font-Bold="true" ForeColor="DimGray">Name*</asp:Label><asp:TextBox ID="txtName" runat="server"
                                    BorderStyle="Solid" BorderWidth="1px" Width="129px" Height="18px" MaxLength="36"
                                    Font-Names="Verdana" Font-Size="XX-Small" CssClass="txtNoFocus"></asp:TextBox>
                        </td>
                        <td style="height: 29px">
                            <font face="Verdana" size="1">
                                <asp:Label ID="lblStatus" runat="server" Width="100%" Font-Size="XX-Small" Font-Names="Verdana"
                                    Font-Bold="true" ForeColor="DimGray" Height="14px">Status*</asp:Label></font><asp:TextBox
                                        ID="txtStatus" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="83px"
                                        Height="18px" MaxLength="8" Font-Names="Verdana" Font-Size="XX-Small" CssClass="txtNoFocus"></asp:TextBox><img
                                            onclick="OpenW(0,'STA','txtStatus');" alt="Status" src="..\..\Images/Plus.gif"
                                            border="0" class="PlusImageCSS">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 121px" valign="middle" bordercolor="#ffffff" align="left" width="100%">
                <table id="table51" style="width: 400px; height: 45px" height="45" width="400" border="0">
                    <tbody>
                        <tr>
                            <td width="5" style="width: 2px">
                            </td>
                            <td width="162" style="width: 78px">
                                <asp:Label ID="lblAB_Type" runat="server" BorderStyle="None" BorderWidth="0px" Width="64px"
                                    Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="true" ForeColor="DimGray"
                                    Height="12px">Add. Type*</asp:Label><br />
                                <asp:TextBox ID="txtAD_Type" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Width="56px" Height="18px" MaxLength="8" Font-Names="Verdana" Font-Size="XX-Small"
                                    CssClass="txtNoFocus"></asp:TextBox><img onclick="OpenW(0,'ADTY','txtAD_Type');"
                                        alt="Add. Type" src="..\..\Images/Plus.gif" border="0" class="PlusImageCSS"/>
                            </td>
                            <td bordercolor="#ffffff" align="left" width="72" colspan="1" rowspan="1" style="width: 72px">
                                <asp:Label ID="lblBr" runat="server" Width="104px" Font-Size="XX-Small" Font-Names="Verdana"
                                    Font-Bold="true" ForeColor="DimGray" Height="12px"> ContactPerson</asp:Label><br>
                                <asp:TextBox ID="txtContactPerson" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Width="104px" Height="18px" MaxLength="36" Font-Names="Verdana" Font-Size="XX-Small"
                                    CssClass="txtNoFocus"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <tr>
                <td style="width: 341px; height: 37px" valign="top" bordercolor="#f5f5f5" align="left"
                    width="341" colspan="3" rowspan="1">
                    <font face="Verdana" size="1">&nbsp;&nbsp;
                        <asp:Label ID="lblAddLine1" runat="server" Width="96px" Font-Size="XX-Small" Font-Names="Verdana"
                            Font-Bold="true" ForeColor="DimGray" Height="14px">Address Line1*</asp:Label></font><br>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtAddLine1" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        Width="320px" Height="18px" MaxLength="36" Font-Names="Verdana" Font-Size="XX-Small"
                        CssClass="txtNoFocus"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 341px" valign="top" bordercolor="#ffffff" align="left" width="341"
                    colspan="3">
                    <font face="Verdana" size="1">&nbsp;&nbsp;
                        <asp:Label ID="lblAddLine2" runat="server" Width="96px" Font-Size="XX-Small" Font-Names="Verdana"
                            Font-Bold="true" ForeColor="DimGray" Height="14px">Address Line2</asp:Label><br>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtAddLine2" runat="server" BorderStyle="Solid" BorderWidth="1px"
                            Width="320px" Height="18px" MaxLength="36" Font-Names="Verdana" Font-Size="XX-Small"
                            CssClass="txtNoFocus"></asp:TextBox></font>
                </td>
            </tr>
            <tr>
                <td style="width: 341px" valign="top" bordercolor="#f5f5f5" align="left" width="341"
                    colspan="3">
                    <font face="Verdana" size="1">&nbsp;&nbsp;
                        <asp:Label ID="lblAddLine3" runat="server" Width="96px" Font-Size="XX-Small" Font-Names="Verdana"
                            Font-Bold="true" ForeColor="DimGray" Height="14px">Address Line3</asp:Label></font><br>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtAddLine3" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        Width="319px" Height="18px" MaxLength="36" Font-Names="Verdana" Font-Size="XX-Small"
                        CssClass="txtNoFocus"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 5px" valign="top" bordercolor="#f5f5f5" align="left" width="341"
                    colspan="3">
                    <table id="table6" width="328" border="0">
                        <tr>
                            <td style="width: 166px" width="166">
                                <font face="Verdana" size="1">&nbsp;
                                    <asp:Label ID="lblCity" runat="server" Width="96px" Font-Size="XX-Small" Font-Names="Verdana"
                                        Font-Bold="true" ForeColor="DimGray">City*</asp:Label></font><br>
                                &nbsp;
                                <asp:TextBox ID="txtCity" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="112px"
                                    Height="18px" MaxLength="8" Font-Names="Verdana" Font-Size="XX-Small" CssClass="txtNoFocus"></asp:TextBox><img
                                        onclick="OpenW(0,'CTY','txtCity');" alt="City" src="..\..\Images/Plus.gif" border="0"
                                        class="PlusImageCSS">
                            </td>
                            <td style="width: 50%">
                                <font face="Verdana" size="1">
                                    <asp:Label ID="lblProvince" runat="server" Width="80%" Font-Size="XX-Small" Font-Names="Verdana"
                                        Font-Bold="true" ForeColor="DimGray" Height="14px">Province*</asp:Label><br>
                                    <asp:TextBox ID="txtProvince" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                        Width="112px" Height="18px" MaxLength="8" Font-Names="Verdana" Font-Size="XX-Small"
                                        CssClass="txtNoFocus"></asp:TextBox><img onclick="OpenW(0,'PROV','txtProvince');"
                                            alt="Province" src="..\..\Images/Plus.gif" border="0" class="PlusImageCSS"></font>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 341px" valign="top" bordercolor="#ffffff" align="left" width="341"
                    colspan="3">
                    <table id="table7" width="328" border="0">
                        <tr>
                            <td style="width: 50%" width="183">
                                <font face="Verdana" size="1">&nbsp;
                                    <asp:Label ID="lblPostalCode" runat="server" Width="96px" Font-Size="XX-Small" Font-Names="Verdana"
                                        Font-Bold="true" ForeColor="DimGray" Height="14px">Postal Code</asp:Label></font><br>
                                &nbsp;
                                <asp:TextBox ID="txtPostalCode" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Width="112px" Height="18px" MaxLength="9" Font-Names="Verdana" Font-Size="XX-Small"
                                    CssClass="txtNoFocus"></asp:TextBox>
                            </td>
                            <td>
                                <font face="Verdana" size="1">
                                    <asp:Label ID="lblCountry" runat="server" Width="96px" Font-Size="XX-Small" Font-Names="Verdana"
                                        Font-Bold="true" ForeColor="DimGray" Height="14px">Country*</asp:Label><br>
                                </font>
                                <asp:TextBox ID="txtCountry" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Width="113px" Height="18px" MaxLength="8" Font-Names="Verdana" Font-Size="XX-Small"
                                    CssClass="txtNoFocus"></asp:TextBox><img onclick="OpenW(0,'CNTY','txtCountry');"
                                        alt="Country" src="..\..\Images/Plus.gif" border="0" class="PlusImageCSS">
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="PanelUpdate" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMsg" runat="server">
                            </asp:Panel>
                            <asp:ListBox ID="lstError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"
                                Visible="false" Width="352px" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
    </table>
    <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
    </form>
</body>
</html>
