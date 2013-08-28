<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Role_edit.aspx.vb" Inherits="Security_Role_edit"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../supportcenter/calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Role_Edit</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../images/js/core.js" type="text/javascript"></script>

    <script src="../images/js/events.js" type="text/javascript"></script>

    <script src="../images/js/css.js" type="text/javascript"></script>

    <script src="../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../images/js/drag.js" type="text/javascript"></script>

    <script src="../images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <script type="text/javascript">
		
		var rand_no = Math.ceil(500*Math.random())
		
		function OpenW(a,b,c)
				{
				
				//wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				//window.showModalDialog
			wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Common'+rand_no,500,450);
				}
				
		function OpenComm(a,b)
				{
				
				wopen('comment.aspx?ScrID=329&ID='+ b + '&tbname=T' ,'Comments'+rand_no,500,450);
				
				}
				
		function OpenAtt()
				{
						
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=T','Additional_Address'+rand_no,400,450);
				
				}
		function OpenAB(c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'Common'+rand_no,500,450);										
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
				

						
			function CloseWindow()
				{
					self.opener.callrefresh();
				}			
				
				
				
					function SaveEdit(varImgValue)
				{
			    		
												
						if (varImgValue=='Close')
						{
								window.close();
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
								//	window.close(); 
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
							  //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('Collapsiblepanel1_collapsible').cells[0].colSpan = "1";
          
        }
        				
    </script>

    <script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.HideModalDiv)
                opener.parent.HideModalDiv();
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

    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5"
    onunload="CloseWindow();">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" style="border-color: activeborder" cellspacing="0" cellpadding="0"
        width="100%" background="../Images/top_nav_back.gif" border="0">
        <tr>
            <td style="width: 20%">
                &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:Label ID="lblTitleLabelRoleEdit" runat="server" BorderStyle="None" CssClass="TitleLabel">Role Edit</asp:Label>
            </td>
            <td style="width: 80%; text-align: center;" nowrap="nowrap">
                <center>
                    <asp:ImageButton ID="imgSave" runat="server" ImageUrl="../Images/S2Save01.gif" AccessKey="S"
                        ToolTip="Save"></asp:ImageButton>
                    <asp:ImageButton ID="imgOk" runat="server" ImageUrl="../Images/s1ok02.gif" AccessKey="K"
                        ToolTip="Ok"></asp:ImageButton>
                    <asp:ImageButton ID="imgReset" runat="server" ImageUrl="../Images/reset_20.gif" AccessKey="R"
                        ToolTip="Reset"></asp:ImageButton>
                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="../Images/s2close01.gif"
                        AccessKey="L" ToolTip="Close"></asp:ImageButton>
                </center>
            </td>
            <td width="42" background="../Images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table126" style="border-color: activeborder" cellspacing="0" cellpadding="0"
        width="100%" border="1">
        <tr>
            <td valign="top" colspan="1">
                <!--  **********************************************************************-->
                <cc1:CollapsiblePanel ID="Collapsiblepanel1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                    Width="100%" BorderColor="Indigo" Height="81px" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                    ExpandImage="../images/ToggleDown.gif" Text="Role" TitleBackColor="Transparent"
                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                    &nbsp;
                    <table id="Table4" cellspacing="0" cellpadding="0" bgcolor="#f5f5f5" border="0">
                        <tr>
                            <td valign="top" align="left" width="422" rowspan="1">
                                <asp:Label ID="lblName3" runat="server" CssClass="FieldLabel">Role ID</asp:Label><br>
                                <asp:TextBox ID="txtRole" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                    BorderWidth="1px" Height="14px" Width="129px" Font-Size="XX-Small" Font-Names="Verdana"
                                    ReadOnly="True" MaxLength="4"></asp:TextBox>
                            </td>
                            <td valign="top" align="left" width="422" height="24" rowspan="1">
                                <asp:Label ID="lblName6" runat="server" CssClass="FieldLabel" Height="12px" Width="85px">Assigned Date</asp:Label><br>
                                <ION:Customcalendar ID="dtAssignDate" runat="server" Width="120px" Height="16px" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="422">
                                <asp:Label ID="lblName4" runat="server" CssClass="FieldLabel" Height="12px" Width="85px">Valid Upto</asp:Label><br>
                                <ION:Customcalendar ID="dtValidUpto" runat="server" Width="120px" Height="16px" />
                            </td>
                            <td valign="top" align="left" width="422">
                                <asp:Label ID="lblName7" runat="server" CssClass="FieldLabel" Height="12px" Width="85px">Assigned By</asp:Label><br>
                                <asp:TextBox ID="txtAssByName" runat="server" CssClass="txtNoFocus" BorderStyle="Solid"
                                    BorderWidth="1px" Height="14px" Width="129px" Font-Size="XX-Small" Font-Names="Verdana"
                                    ReadOnly="True" MaxLength="8"></asp:TextBox>
                                <asp:TextBox ID="txtAssBy" runat="server" BorderWidth="0px" Height="0px" Width="0px"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="422">
                                <asp:Label ID="lblName9" runat="server" CssClass="FieldLabel" Height="13px" Width="115px"> Status</asp:Label><br>
                                <asp:DropDownList ID="Ddl_Status" runat="server" CssClass="txtNoFocus" Width="129px"
                                    Height="20px">
                                    <asp:ListItem Value="ENB">Enable</asp:ListItem>
                                    <asp:ListItem Value="DNB">Disable</asp:ListItem>
                                </asp:DropDownList>
                                <input type="hidden" name="txthiddenImage" />
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
            </td>
            <asp:UpdatePanel ID="PanelUpdate" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlMsg" runat="server">
                    </asp:Panel>
                    <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Height="32px"
                        Width="376px" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false">
                    </asp:ListBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </tr>
    </table>
    <!-- ***********************************************************************-->
    </form>
</body>
</html>
