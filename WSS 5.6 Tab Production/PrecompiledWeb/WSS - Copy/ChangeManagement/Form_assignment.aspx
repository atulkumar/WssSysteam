<%@ page language="VB" autoeventwireup="false" inherits="ChangeManagement_Form_assignment, App_Web_8ch_aegk" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Assignment</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../Images/js/core.js" type="text/javascript"></script>

    <script src="../images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../Images/js/events.js" type="text/javascript"></script>

    <script src="../Images/js/css.js" type="text/javascript"></script>

    <script src="../Images/js/coordinates.js" type="text/javascript"></script>

    <script src="../Images/js/drag.js" type="text/javascript"></script>

    <script type="text/javascript">
			var globleID;
			var globleUser;
			var globleRole;
			var globleCompany;
			
		var rand_no = Math.ceil(500*Math.random())	
		
			function OpenComp(c)
				{
					wopen('../Search/Common/PopSearch1.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as CompanyName, CI_VC16_Alias AS AliasName  from T010011 WHERE CI_VC8_Address_Book_Type = '+ "'COM'" +' &tbname=' + c ,'Search'+rand_no,500,450);
				}
			function OpenCall(a, b, c)
				{
					var comp = document.getElementById('txtCompName').value;
					//wopen('../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + ' and company='+"'"+comp+"'"+' and UDCType='+"'"+b+"'"+' &tbname=txtCallType','Search',500,450);
					wopen('../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + ' and UDCType='+"'"+b+"'"+' &tbname=txtCallType','Search'+rand_no,500,450);
				}
				  
			function OpenTask(a, b, c)
				{   
					var comp = document.getElementById('txtCompName').value;
					//wopen('../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+ ' and Company = '+"'"+ comp +"'"+' &tbname=' + c ,'Search',500,450);
					wopen('../Search/Common/PopSearch1.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+ ' &tbname=' + c ,'Search'+rand_no,500,450);
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
											
											if  (document.Form1.txthidden.value=="")
												{
														alert("Please select the row");
												}
												else
												{
													var formName=document.Form1.txthidden.value;
													formName=formName.replace(' ','5z_q');
												
													wopen('Form_Entry_Details.aspx?ScrID=260&formName='+formName+'&cno=-1&tno=-1','formName'+rand_no,915,550);
														//alert(formName)
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
								if (varImgValue=='Search')
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
								
								if (varImgValue=='Reset')
									{
												var confirmed
												confirmed=window.confirm("Do  You Want To reset The Page ?");
												if(confirmed==true)
														{	
																 Form1.reset()
																 return false;
														}		

									}			
				}				
				
		function KeyCheck(nn,rowvalues)
			{
				//alert(rowvalues);
				globleID = nn;
				document.Form1.txthidden.value=nn;
							
				var tableID='GrdAddSerach'  //your datagrids id
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
			}	
					
		function OpenW(varTable)
			{
				wopen('../AdministrationCenter/AddressBook/AB_ViewColumns.aspx? ID='+varTable,'Search'+rand_no,500,450);
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
					'status=no, toolbar=no, scrollbars=yes, resizable=yes');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}
			
					
			function addToParentList(Afilename,TbName,strname)
				{				
					if (Afilename != "" || Afilename != 'undefined')
						{
							
							varName = TbName + 'Name'
							if((document.getElementById(varName) == null))
							{								
							}
							else
							{
								document.getElementById(varName).value=strname;
							}
												
							document.getElementById(TbName).value=Afilename;
							aa=Afilename;
						}
					else					
						{
							document.Form1.txtAB_Type.value=aa;
						}
				}
				
		 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }			
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr width="100%">
                        <td background="../Images/top_nav_back.gif" height="47">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 25%">
                                        <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" BorderWidth="0px" Width="0px"
                                            BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                        <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                            AlternateText="." CommandName="submit" ImageUrl="white.GIF"></asp:ImageButton>
                                        <asp:Label ID="lblTitleLabelFrmAssign" runat="server" CssClass="TitleLabel">FORM ASSIGNMENT</asp:Label>
                                    </td>
                                    <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                        <center>
                                            <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                                                ToolTip="Save"></asp:ImageButton>
                                            <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../Images/s1search02.gif"
                                                ToolTip="Search"></asp:ImageButton>
                                            <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                            <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                            <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('107','../');"
                                alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;<img
                                    class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                    src="../icons/logoff.gif" border="0" name="tbrbtnEdit" />&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <!--  **********************************************************************-->
                <div style="overflow: auto; width: 100%; height: 581px">
                    <table width="100%" border="0">
                        <tbody>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="Label4" runat="server" ForeColor="DimGray" Font-Bold="True"
                                                    Font-Names="Verdana" Font-Size="XX-Small">Company Name: </asp:Label>&nbsp;
                                                <uc1:CustomDDL ID="cddlComp" runat="server"></uc1:CustomDDL>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" ForeColor="DimGray" Font-Bold="True"
                                                    Font-Names="Verdana" Font-Size="XX-Small">Call Type: </asp:Label>&nbsp;
                                                <uc1:CustomDDL ID="cddlCall" runat="server"></uc1:CustomDDL>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="Label5" runat="server" ForeColor="DimGray" Font-Bold="True"
                                                    Font-Names="Verdana" Font-Size="XX-Small">Task Type: </asp:Label>&nbsp;
                                                <uc1:CustomDDL ID="cddlTask" runat="server"></uc1:CustomDDL>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:CollapsiblePanel ID="Collapsiblepanel1" runat="server" Height="24px" Width="100%"
                                        BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                        ExpandImage="../Images/ToggleDown.gif" Text="Form Assignment" TitleBackColor="Transparent"
                                        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                        Visible="true" BorderColor="Indigo">
                                        &nbsp;
                                        <asp:DataGrid ID="dgFormAssign" runat="server" Width="100%" DataKeyField="FN_VC100_Form_name"
                                            BackColor="White" AutoGenerateColumns="False">
                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                            <HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" CssClass="GridHeader">
                                            </HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn Visible="False">
                                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                    <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "FN_IN4_form_no") %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Form Name">
                                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                    <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="75%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "FN_VC100_Form_name") %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                    <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="0px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtIsExists" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IsExisted") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                    <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:RadioButtonList ID="rdlIsInserted" runat="server" Width="32px" Font-Size="XX-Small"
                                                            Font-Names="Verdana" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                            <asp:ListItem Value="No">No</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid></cc1:CollapsiblePanel>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="635px" BorderStyle="Groove" BorderWidth="0"
                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
