<%@ page language="VB" validaterequest="false" enableeventvalidation="false" autoeventwireup="false" inherits="AdministrationCenter_UDC_UDC, App_Web_qswi9nss" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>UDC</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript">
        var rand_no = Math.ceil(500*Math.random())
		function OpenCompany(c)
				{
				    	wopen('../../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as Name,CI_IN4_Business_Relation as Type from T010011 where CI_VC8_Address_Book_Type='+"'COM'" + '  &tbname=' + c ,'Search'+rand_no,500,450);
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
				
			function HideContents()
				{
					parent.document.all("SideMenu1").cols="0,*";
					document.Form1.imgHide.style.visibility = 'hidden'; 
					document.Form1.ingShow.style.visibility = 'visible'; 				
				}
					
			function ShowContents()
				{
					document.Form1.ingShow.style.visibility = 'hidden'; 
					document.Form1.imgHide.style.visibility = 'visible'; 
					parent.document.all("SideMenu1").cols="14%,*";					
				}
					
			function Hideshow()
				{
					if (parent.document.all("SideMenu1").cols =="0,*")
					{
							document.Form1.imgHide.style.visibility = 'hidden'; 
							document.Form1.ingShow.style.visibility = 'visible'; 
					}
					else
					{
							document.Form1.ingShow.style.visibility = 'hidden'; 
							document.Form1.imgHide.style.visibility = 'visible'; 
					}
				}				
			
			function callrefresh()
				{
					Form1.submit();
				}
								
			function ConfirmDelete(varImgValue)
				{
					//alert(document.Form1.txthiddenTable.value);
					//alert(document.Form1.txtTask.value);
				if (document.Form1.txtSelectHID.value!='selected')
						{
							alert("Please select the row");
						}
					else
						{		
							var confirmed
								confirmed=window.confirm("Are you sure you want to Delete the selected record ?");
								if(confirmed==false)
								{
									
									return false;
								
								}
								else
								{										
										document.Form1.txthidden.value=varImgValue;
										Form1.submit(); 
										return false;
								}		
						}	
							return false;
				}
				
			function SaveEdit(varImgValue)
				{
				//alert(varImgValue);
					if (varImgValue=='Logout')
						{
							document.Form1.txthidden.value=varImgValue;
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
					
							document.Form1.txthidden.value=varImgValue;
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
																return false;
														}		
						}	
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
								var which='<%=ViewState("WhichGrid") %>';
								if ( which =='UDC' || which =='UDCType' )
								{
									document.Form1.txthidden.value=varImgValue;
									Form1.submit(); 
								}
								else
								{
									alert('Please select a row');
								}
							return false;
						}	
					
					
			    	//document.Form1.txthidden.value=varImgValue;
					//Form1.submit(); 
				}				
				
	
						
    </script>

    <script type="text/javascript">
    //A Function to improve design i.e delete the extra cell of table
    function onEnd() {
        var x = document.getElementById('cpnlUDCType_collapsible').cells[0].colSpan = "1";
        var y = document.getElementById('cpnlUDC_collapsible').cells[0].colSpan = "1";
        
    } 
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td valign="top">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td background="../../images/top_nav_back.gif" height="47">
                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 15%">
                                                        <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" AlternateText="."
                                                            CommandName="submit" Height="1px" Width="1px" ImageUrl="~/images/white.GIF">
                                                        </asp:ImageButton>
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="lblTitleLabelUDC" runat="server"  CssClass="TitleLabel" >UDC</asp:Label>
                                                    </td>
                                                    <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                        <center>
                                                            <asp:ImageButton ID="imgSave" runat="server" ImageUrl="../../Images/S2Save01.gif"
                                                                AccessKey="S" ToolTip="Save"></asp:ImageButton>&nbsp;
                                                            <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                                AccessKey="E" ToolTip="Edit"></asp:ImageButton>&nbsp;
                                                            <asp:ImageButton ID="imgReset" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                                AccessKey="R" ToolTip="Reset"></asp:ImageButton>&nbsp;
                                                            <asp:ImageButton ID="imgSearch" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                                AccessKey="H" ToolTip="Search"></asp:ImageButton>&nbsp;
                                                            <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                                AccessKey="D" ToolTip="Delete"></asp:ImageButton>
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                            height="47">
                                            <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('472','../../');"
                                                alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                    class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                    src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <!--  **********************************************************************-->
                                <div style="overflow: auto; width: 100%; height: 100%">
                                    <table width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="width: 100%" valign="top" colspan="2" height="1">
                                                    <cc1:CollapsiblePanel ID="cpnlUDCType" runat="server" Height="52px" Width="100%"
                                                        BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                        ExpandImage="../../Images/ToggleDown.gif" Text="UDC Type" TitleBackColor="Transparent"
                                                        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                        Visible="true" BorderColor="Indigo">
                                                        <!--  **********************************************************************-->
                                                        <table id="Table126" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                            border="0">
                                                            <tr>
                                                                <td colspan="1">
                                                                    <table id="Table7" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="top" align="left">
                                                                                <asp:Panel ID="pnlUDCTypeTxtbox" runat="server" Height="24px">
                                                                                </asp:Panel>
                                                                                <div style="overflow: auto; width: 100%; height: 230px">
                                                                                    <asp:DataGrid ID="grdUDCType" runat="server" BorderStyle="None" BorderWidth="1px"
                                                                                        Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" BackColor="#E0E0E0"
                                                                                        PagerStyle-Visible="False" AutoGenerateColumns="False" CellPadding="1" HorizontalAlign="left"
                                                                                        CssClass="Grid">
                                                                                        <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                        <SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
                                                                                        <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                                                        <ItemStyle Font-Size="8pt" ForeColor="Black" CssClass="GridItem" BackColor="White">
                                                                                        </ItemStyle>
                                                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                                            CssClass="GridFixedHeader" BackColor="#E0E0E0"></HeaderStyle>
                                                                                        <Columns>
                                                                                            <asp:ButtonColumn Visible="False" CommandName="select"></asp:ButtonColumn>
                                                                                            <asp:BoundColumn DataField="ProductCode" HeaderText="Product Code">
                                                                                                <HeaderStyle Width="60pt" Wrap="false"></HeaderStyle>
                                                                                                <ItemStyle Width="60pt"></ItemStyle>
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="UDCType" HeaderText="UDC Type">
                                                                                                <HeaderStyle Width="70pt" Wrap="false"></HeaderStyle>
                                                                                                <ItemStyle Width="70pt"></ItemStyle>
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="UDCTypeText" HeaderText="Text">
                                                                                                <HeaderStyle Width="154pt"></HeaderStyle>
                                                                                                <ItemStyle Width="154pt"></ItemStyle>
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="Company" HeaderText="Company">
                                                                                                <HeaderStyle Width="60pt"></HeaderStyle>
                                                                                                <ItemStyle Width="60pt"></ItemStyle>
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="UDCParam" HeaderText="Flag">
                                                                                                <HeaderStyle Width="60pt"></HeaderStyle>
                                                                                                <ItemStyle Width="60pt"></ItemStyle>
                                                                                            </asp:BoundColumn>
                                                                                        </Columns>
                                                                                        <PagerStyle Visible="False" Font-Size="XX-Small" Font-Bold="True" HorizontalAlign="Center"
                                                                                            ForeColor="DarkBlue" Position="TopAndBottom" BackColor="#CDD7ED" Mode="NumericPages">
                                                                                        </PagerStyle>
                                                                                    </asp:DataGrid></div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" align="left">
                                                                                <asp:TextBox ID="txtProductCode" runat="server" Width="82px" Height="18px" BorderStyle="Solid"
                                                                                    BorderWidth="1px" Font-Size="XX-Small" MaxLength="9" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                                <asp:TextBox ID="txtUDCTypeP" runat="server" Width="93" Height="18px" BorderStyle="Solid"
                                                                                    BorderWidth="1px" Font-Size="XX-Small" MaxLength="4" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                                <asp:TextBox ID="txtUDCTypeText" runat="server" Width="205px" Height="18px" BorderStyle="Solid"
                                                                                    BorderWidth="1px" Font-Size="XX-Small" MaxLength="30" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                                <uc1:CustomDDL ID="CDDLUDCTypeCompany" runat="server" Height="18px" Width="71px">
                                                                                </uc1:CustomDDL>
                                                                                <asp:CheckBox ID="chkUDCParam" runat="server" Width="80px"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </cc1:CollapsiblePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <cc1:CollapsiblePanel ID="cpnlUDC" runat="server" Height="57px" Width="100%" BorderWidth="0px"
                                                        BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                        ExpandImage="../../Images/ToggleDown.gif" Text="UDC" TitleBackColor="Transparent"
                                                        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                        Visible="true" BorderColor="Indigo">
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td width="50%" colspan="0">
                                                                    <asp:Panel ID="pnlUDC" runat="server" Height="24px">
                                                                    </asp:Panel>
                                                                    <div style="overflow: auto; width: 100%; height: 173px">
                                                                        <asp:DataGrid ID="grdUDC" runat="server" BorderStyle="None" BorderWidth="1px" Font-Names="Verdana"
                                                                            ForeColor="MidnightBlue" BorderColor="Silver" BackColor="#E0E0E0" PagerStyle-Visible="False"
                                                                            AutoGenerateColumns="False" CellPadding="1" HorizontalAlign="left" CssClass="Grid">
                                                                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                            <SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
                                                                            <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                                            <ItemStyle Font-Size="8pt" ForeColor="Black" CssClass="GridItem" BackColor="White">
                                                                            </ItemStyle>
                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                                CssClass="GridfixedHeader" BackColor="#E0E0E0"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:ButtonColumn Visible="False" CommandName="Select"></asp:ButtonColumn>
                                                                                <asp:BoundColumn DataField="ProductCode" HeaderText="Product Code">
                                                                                    <HeaderStyle Width="60pt"></HeaderStyle>
                                                                                    <ItemStyle Width="60pt"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="UDCType" HeaderText="UDC Type">
                                                                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                                                                    <ItemStyle Width="70pt"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Name" HeaderText="Name">
                                                                                    <HeaderStyle Width="60pt"></HeaderStyle>
                                                                                    <ItemStyle Width="60pt"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Description" HeaderText="Description">
                                                                                    <HeaderStyle Width="150pt"></HeaderStyle>
                                                                                    <ItemStyle Width="150pt"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Company" HeaderText="Company">
                                                                                    <HeaderStyle Width="60pt"></HeaderStyle>
                                                                                    <ItemStyle Width="60pt"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="Black" BackColor="#999999"
                                                                                Mode="NumericPages"></PagerStyle>
                                                                        </asp:DataGrid></div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="30%">
                                                                    <asp:TextBox ID="txtProductCodeUDC" runat="server" Width="80px" Height="18" BorderStyle="Solid"
                                                                        BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocusFE"
                                                                        ReadOnly="True"></asp:TextBox>
                                                                    <asp:TextBox ID="txtUDCTypeF" runat="server" Width="93px" Height="18" BorderStyle="Solid"
                                                                        BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocusFE"
                                                                        ReadOnly="True"></asp:TextBox><!--<IMG onclick="OpenW();" alt="" src="Images/plus.gif">-->
                                                                    <asp:TextBox ID="txtName" runat="server" Width="78px" Height="18" BorderStyle="Solid"
                                                                        BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" MaxLength="8" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                    <asp:TextBox ID="txtDescription" runat="server" Width="200px" Height="18" BorderStyle="Solid"
                                                                        BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" MaxLength="30" CssClass="txtNoFocusFE"></asp:TextBox>
                                                                    <uc1:CustomDDL ID="CDDLUDCCompany" runat="server" Height="18px" Width="76px"></uc1:CustomDDL>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </cc1:CollapsiblePanel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="PanelUpdate" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMsg" runat="server">
                            </asp:Panel>
                            <asp:ListBox ID="lstError" runat="server" Width="520px" Font-Size="XX-Small" Font-Names="Verdana"
                                Visible="false" ForeColor="Red"></asp:ListBox>
                            <input type="hidden" name="txthidden" />
                            <input type="hidden" name="txthiddenSelect" id="txtSelectHID" runat="server" />
                            <input type="hidden" name="txthiddenImage" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
