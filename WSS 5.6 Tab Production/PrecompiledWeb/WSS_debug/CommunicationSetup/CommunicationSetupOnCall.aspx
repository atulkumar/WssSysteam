<%@ page language="VB" autoeventwireup="false" inherits="CommunicationSetup_CommunicationSetupOnCall, App_Web_tblewnny" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Communication Setup on Call</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../DateControl/ION.js"></script>

    <script type="text/javascript" src="../images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../SupportCenter/calendar/popcalendar.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>

    <script type="text/javascript">
		
			var globleID;
			var globleUser;
			var globleRole;
			var globleCompany;
	var rand_no = Math.ceil(500*Math.random())		
	
	function formReset()		
	{
		var confirmed
		confirmed=window.confirm("Do You Want To reset The Page ?");
		if(confirmed==true)
			{	
				Form1.reset()
			}	
		return false;	
	}			
				
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
						self.opener.callrefresh();
						//CloseWindow();
						window.close();
					}
					if (varImgValue=='Logout')
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
							//window.close(); 
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
							//alert(confirmed);
							if(confirmed==true)
							{	
								//alert(confirmed);
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit();
								
							}
							return false;		
						}			
						if (varImgValue=='Edit')
						
						{
							//Security Block
						var obj=document.getElementById("imgEdit")
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
					
							var InvID=document.getElementById("txthidden").value;
					
							if (InvID != '')
							{
								wopen('EditOverriddenRule.aspx?InvID='+InvID.toString(),'ComSetUP'+rand_no,580,450);
							}
							else
							{
							   	if ('<%=Session("PropTempFirstGridRows")%>'!=0)
									alert('Please select a row');
							}
							return false;				
						}		
						
						if (varImgValue=='Update')
						{
							//Security Block
						var obj=document.getElementById("imgUpdate")
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
							
							
					function KeyCheck(nn,rowvalues)
					{
						globleID = nn;
						document.Form1.txthidden.value=nn;
			/*			document.Form1.txthiddenUser.value=nn;
						document.Form1.txthiddenRole.value=nn;
						document.Form1.txthiddenCompany.value=nn;
			*/
						//Form1.submit();
						
										var tableID='cpnlAD_dgCommRules'  //your datagrids id
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
							SaveEdit('Edit');
							return false;
					}	
					
			function OpenW(varTable)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company  from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c  ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				wopen('../AdministrationCenter/AddressBook/AB_ViewColumns.aspx? ID='+varTable,'Search'+rand_no,500,450);
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
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
		 function onEnd() {
            var x = document.getElementById('cpnlFilds_collapsible').cells[0].colSpan = "1";
              var y = document.getElementById('cpnlAD_collapsible').cells[0].colSpan = "1";
             
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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" style="border-color: activeborder" cellspacing="0" cellpadding="0"
        width="100%" background="../images/top_nav_back.gif" border="0">
        <tr>
            <td style="width:25%">
                <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                    BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                <asp:Label ID="lblTitleLabelCSoncall" runat="server" CssClass="TitleLabel" >Communication Setup</asp:Label>
            </td>
            <td style="width: 75%; text-align: center;" nowrap="nowrap">
                <center>
                    <asp:ImageButton ID="imgUpdate" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                        ToolTip="Update" Visible="False"></asp:ImageButton>
                    <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                        ToolTip="Save"></asp:ImageButton>
                    <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../Images/s1ok02.gif"
                        ToolTip="Ok"></asp:ImageButton>
                    <asp:ImageButton ID="imgEdit" AccessKey="R" runat="server" ImageUrl="../Images/S2edit01.gif"
                        ToolTip="Edit"></asp:ImageButton>
                    <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                        ToolTip="Reset"></asp:ImageButton>
                    <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../Images/s2close01.gif"
                        ToolTip="Close"></asp:ImageButton>
                </center>
            </td>
            <td width="42" background="../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <!--  **********************************************************************-->
    <table style="border-color: activeborder" cellspacing="0" cellpadding="0" width="100%"
        border="0">
        <tbody>
            <tr>
                <td valign="top">
                    <!-- **********************************************************************-->
                    <cc1:CollapsiblePanel ID="cpnlFilds" runat="server" Width="100%" BorderStyle="Solid"
                        BorderWidth="1px" Visible="true" Draggable="False" CollapseImage="../images/ToggleUp.gif"
                        ExpandImage="../images/ToggleDown.gif" Text="Choose Criteria" TitleBackColor="Transparent"
                        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                        <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td valign="top" align="left" width="100%">
                                    <table width="100%" bgcolor="#f5f5f5" border="0">
                                        <tr>
                                            <td style="height: 26px">
                                                <asp:Label ID="Label4" runat="server" Width="96px" CssClass="FieldLabel" EnableViewState="False">Event Name</asp:Label><br>
                                                <asp:DropDownList ID="ddEventName" runat="server" Height="22px" Width="129px" Font-Size="XX-Small"
                                                    Font-Names="Verdana">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 26px">
                                                <asp:Label ID="Label7" runat="server" Height="12px" Width="136px" CssClass="FieldLabel"
                                                    EnableViewState="False">On Event</asp:Label><br>
                                                <asp:DropDownList ID="ddEventFired" runat="server" Height="22px" Width="129px" Font-Size="XX-Small"
                                                    Font-Names="Verdana">
                                                    <asp:ListItem Value="0" Selected="True">Optional</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 26px">
                                                <asp:Label ID="Label1" runat="server" Height="12px" Width="72px" CssClass="FieldLabel"
                                                    EnableViewState="False">User Name</asp:Label><br>
                                                <asp:DropDownList ID="ddUserName" runat="server" Height="22" Width="129" Font-Size="XX-Small"
                                                    Font-Names="Verdana" Font-Name="verdana">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 26px">
                                                <asp:Label ID="Label2" runat="server" Height="12px" Width="72px" CssClass="FieldLabel"
                                                    EnableViewState="False">Role Name</asp:Label><br>
                                                <asp:DropDownList ID="ddRoleName" runat="server" Height="22" Width="129" Font-Size="XX-Small"
                                                    Font-Names="Verdana" Font-Name="verdana">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 26px">
                                                <asp:Label ID="Label3" runat="server" Width="72px" CssClass="FieldLabel" Font-Size="XX-Small"
                                                    Font-Names="Verdana" EnableViewState="False" Font-Bold="True">Call Type</asp:Label><br>
                                                <asp:DropDownList ID="ddCallType" runat="server" Height="22" Width="129" Font-Size="XX-Small"
                                                    Font-Names="Verdana" Font-Name="verdana">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 32px">
                                                <asp:Label ID="Label5" runat="server" Width="72px" CssClass="FieldLabel" EnableViewState="False">Task Type</asp:Label><br>
                                                <asp:DropDownList ID="ddTaskType" runat="server" Height="22" Width="129" Font-Size="XX-Small"
                                                    Font-Names="Verdana">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 32px">
                                                <asp:Label ID="Label9" runat="server" Width="72px" CssClass="FieldLabel" EnableViewState="False">Call Status</asp:Label><br>
                                                <asp:DropDownList ID="ddCallStatus" runat="server" Height="22" Width="129" Font-Size="XX-Small"
                                                    Font-Names="Verdana">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 32px">
                                                <asp:Label ID="Label10" runat="server" Height="12px" Width="72px" CssClass="FieldLabel"
                                                    EnableViewState="False">Task Status</asp:Label><br>
                                                <asp:DropDownList ID="ddTaskStatus" runat="server" Height="22" Width="129" Font-Size="XX-Small"
                                                    Font-Names="Verdana">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 32px">
                                                <asp:Label ID="lblMiddleName2" runat="server" Width="72px" CssClass="FieldLabel"
                                                    EnableViewState="False">Company</asp:Label><br>
                                                <asp:DropDownList ID="ddCompany" runat="server" Height="22px" Width="129px" Font-Size="XX-Small"
                                                    Font-Names="Verdana" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 32px">
                                                <asp:Label ID="Label8" runat="server" Height="12px" Width="72px" CssClass="FieldLabel"
                                                    Font-Size="XX-Small" Font-Names="Verdana" EnableViewState="False" Font-Bold="True">SubCategory</asp:Label><br>
                                                <asp:DropDownList ID="ddProject" runat="server" Height="22" Width="129" Font-Size="XX-Small"
                                                    Font-Names="Verdana">
                                                    <asp:ListItem Value="0" Selected="True">Optional</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Height="12px" CssClass="FieldLabel">Start Date</asp:Label><br>
                                                <ION:Customcalendar ID="dtStartDate" runat="server" Height="16px" Width="120px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Height="12px" CssClass="FieldLabel">Stop Date</asp:Label><br>
                                                <ION:Customcalendar ID="dtEndDate" runat="server" Height="15px" Width="120px" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsMail" runat="server" Width="80px" CssClass="FieldLabel" Text="Mail">
                                                </asp:CheckBox>
                                                <asp:CheckBox ID="chkIsSMS" runat="server" Width="80px" CssClass="FieldLabel" Text="SMS">
                                                </asp:CheckBox>
                                            </td>
                                            <td rowspan="1">
                                                <asp:Label ID="Label13" runat="server" Height="12px" Width="112px" CssClass="FieldLabel"
                                                    EnableViewState="False">Record Status</asp:Label><br>
                                                <asp:DropDownList ID="ddRecordStatus" runat="server" Height="22" Width="129" Font-Size="XX-Small"
                                                    Font-Names="Verdana">
                                                    <asp:ListItem Value="1">Enable</asp:ListItem>
                                                    <asp:ListItem Value="0">Disable</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMiddleName8" runat="server" Height="2px" Width="81px" CssClass="FieldLabel"
                                                    Visible="False" Font-Size="XX-Small" Font-Names="Verdana" EnableViewState="False"
                                                    Font-Bold="True">Default User</asp:Label><br>
                                                <asp:DropDownList ID="ddEventUser" runat="server" Height="22px" Width="128px" Visible="False"
                                                    Font-Size="XX-Small" Font-Names="Verdana">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <input type="hidden" name="txtCompanyID" runat="server" id="txtCompanyID" />
                        <input type="hidden" name="Textbox1" runat="server" id="Textbox1" />
                    </cc1:CollapsiblePanel>
                </td>
            </tr>
        </tbody>
    </table>
    <table style="border-color: activeborder" cellspacing="0" cellpadding="0" width="800px"
        border="0px">
        <tbody>
            <tr>
                <td style="width: 800px">
                    <cc1:CollapsiblePanel ID="cpnlAD" runat="server" Width="800px" Height="47px" BorderStyle="Solid"
                        BorderWidth="0px" Visible="True" BorderColor="Indigo" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                        ExpandImage="../Images/ToggleDown.gif" Text="Communication Setup Rules" TitleBackColor="Transparent"
                        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test">
                        <div style="overflow: auto; width: 100%; height: 225px">
                            <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                width="100%" align="left" border="0">
                                <tr>
                                    <td nowrap="nowrap">
                                        <asp:Panel ID="Panel1" runat="server">
                                        </asp:Panel>
                                        <asp:DataGrid ID="dgCommRules" runat="server" BorderWidth="1px" BorderStyle="None"
                                            CssClass="Grid" BorderColor="Silver" Font-Names="Verdana" ForeColor="MidnightBlue"
                                            CellPadding="0" GridLines="Horizontal" HorizontalAlign="Left" PageSize="50" DataKeyField="Table_ID"
                                            Width="100%">
                                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                            <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem " BackColor="White">
                                            </ItemStyle>
                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                            </HeaderStyle>
                                            <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </cc1:CollapsiblePanel>
                </td>
            </tr>
            <!-- *****************************************-->
        </tbody>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <!-- ***********************************************************************-->
            <input type="hidden" name="txthiddenImage" />
            <input type="hidden" id="txthidden" name="txthidden" />
            <asp:ListBox ID="lstError" runat="server" Height="32px" Width="100px" BorderWidth="0"
                BorderStyle="Groove" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"
                Visible="false"></asp:ListBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
