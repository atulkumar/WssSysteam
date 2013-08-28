<%@ page language="VB" autoeventwireup="false" inherits="Search_Logs_Logs, App_Web_jth5zadc" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script language="javascript" src="../../Images/Js/ABMainShortCuts.js" type="text/javascript"></script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
		
		var rand_no = Math.ceil(500*Math.random())
	             function SelectComp()
					{
						var value1;
						var txt;
						value1=document.getElementById('cpnlAdvSearch_ddlCompany').value; 
						txt=document.getElementById('cpnlAdvSearch_txtsearch').value; 
						
						if(value1=="")
								{
								alert('Please Select Company First...');
								document.getElementById('cpnlAdvSearch_ddlcompany').focus();
								document.getElementById('cpnlAdvSearch_txtsearch').value="";  
								return false;
						
								}
								if(txt=="")
								{
									alert('Please enter search string... ');
									document.getElementById('cpnlAdvSearch_txtsearch').focus();
									return false;					
									
								}
							}
				
			
				
				function KeyCheck(callno,grdrowid,cpnlname,Compid,taskno)
					{
						
						globleID = callno;
												
						document.Form1.txthidden.value=callno;
						document.Form1.txthiddenTable.value=cpnlname;
						document.Form1.txtrowvalues.value=grdrowid;
				 
						document.Form1.txtComp.value=Compid;
			         
							
							if (cpnlname =='cpnlcall_grdcall')
							{

							document.Form1.txthiddenImage.value='Select';
							
								setTimeout('Form1.submit();',100);
							return false;
							//Form1.submit(); 
							}
					}	
				
				function SaveEdit(varImgValue)
				{			
					
						if (varImgValue=='Logout')
								{
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
							return false;	
						}		
				
				
				}
				
				
				
					
					function KeyCheck55(callno,grdrowid,cpnlname,compid,taskno)
					{
							//alert("asdf");
							
							document.Form1.txthidden.value=callno;
							document.Form1.txthiddenImage.value='Edit';
							document.Form1.txthiddenTable.value=cpnlname;
							document.Form1.txtComp.value=compid;
				
						
							if (cpnlname=='cpnltask_grdtask' || cpnlname=='cpnlaction_grdaction')
							{
						
							document.Form1.txtTaskno.value=taskno; 
							Form1.submit(); 
							OpenCallwindow(taskno);
						
						
							}
							else
							{
							
							Form1.submit(); 
							OpenCallwindow(taskno);
							//return false;
														
							}
												
					}	
				
			function OpenCallwindow(taskno)
			{
				//alert(taskno);
			wopen('../../SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&SearchID=1&SEARTASKNO='+taskno,'CallDetailSearch'+rand_no,900,600);
			
			}
			
				
			function OpenTask(callno)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
						var CS='<%=session("PropCallStatus")%>';
						if ( CS=='CLOSED')
						{
								alert('Task cannot be edited for a Closed Call');
						}
						else
						{
								wopen('Task_edit.aspx?ScrID=334&TASKNO='+callno,'Search'+rand_no,430,400);
						}
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
					'status=no, toolbar=no, scrollbars=yes, resizable=no');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}			
			
				
		 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }				
		
					
    </script>

    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr style="width: 100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    &nbsp;&nbsp;
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" ImageUrl="white.GIF"
                                                        AlternateText="." CommandName="submit"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelSearch" runat="server" CssClass="TitleLabel" BorderWidth="2px"
                                                        BorderStyle="None">WSS Logs</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <!--<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif" ToolTip="Save"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgOk" accessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif" ToolTip="OK"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgEdit" accessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif" ToolTip="Edit"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgDelete" accessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"	ToolTip="Delete" Visible="False"></asp:imagebutton>&nbsp;&nbsp;-->
                                                        <asp:ImageButton ID="BtnSearch" AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                            AlternateText="Search" ToolTip="Search"></asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>&nbsp;
                                                        <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('854','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../../icons/logoff.gif" border="0" name="tbrbtnEdit" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div style="overflow: auto; width: 100%">
                                <table id="Table312" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <cc1:CollapsiblePanel ID="cpnlAdvSearch" runat="server" Width="100%" BorderWidth="0px"
                                                BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="PowderBlue"
                                                TitleClickable="True" TitleBackColor="Transparent" Text="WSS Logs" ExpandImage="../../Images/ToggleDown.gif"
                                                CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                <table id="Table3" bordercolor="#5c5a5b" cellspacing="6" cellpadding="0" width="100%"
                                                    border="1">
                                                    <tr>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left">
                                                            <!-- advance serach fields -->
                                                            <font face="Verdana" size="1">
                                                                <asp:Label ID="Label10" runat="server" CssClass="FieldLabel">Company</asp:Label></font>&nbsp;&nbsp;&nbsp;
                                                            <asp:DropDownList ID="ddlCompany" TabIndex="3" Width="129px" CssClass="txtnofocus"
                                                                runat="server">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <font face="Verdana" size="1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label5" runat="server" CssClass="FieldLabel">Call No.</asp:Label>&nbsp;
                                                                <asp:TextBox ID="TxtCallNo" runat="server" Width="72px" CssClass="txtnofocus" Font-Size="XX-Small"
                                                                    Font-Names="Verdana" MaxLength="8"></asp:TextBox></font>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">Task No.</asp:Label>&nbsp;<font
                                                                face="Verdana" size="1">&nbsp;&nbsp;
                                                                <asp:TextBox ID="TxtTaskNo" runat="server" Width="72px" CssClass="txtnofocus" Font-Size="XX-Small"
                                                                    Font-Names="Verdana" MaxLength="8"></asp:TextBox></font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td bordercolor="#f5f5f5">
                                                            &nbsp;&nbsp;<!-- ---------------------- -->
                                                            <font face="Verdana" size="1">&nbsp;&nbsp;
                                                                <asp:CheckBox ID="chkcallLevel" runat="server" Text="Call Level" Font-Size="8pt"
                                                                    Font-Names="Verdana" Checked="True"></asp:CheckBox>&nbsp;&nbsp;&nbsp;
                                                                <asp:CheckBox ID="chktskLevel" runat="server" Text="Task Level" Font-Size="8pt" Font-Names="Verdana"
                                                                    Checked="True"></asp:CheckBox>&nbsp;
                                                                <asp:CheckBox ID="ChkActLevel" runat="server" Text="Action Level" Font-Size="8pt"
                                                                    Font-Names="Verdana"></asp:CheckBox><!--<asp:Button id="BtnSearch1" runat="server" Text="Search"></asp:Button></FONT>--></font>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table423" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td valign="top" align="left">
                                                <cc1:CollapsiblePanel ID="cpnlcallsearch" runat="server" Width="100%" BorderWidth="0px"
                                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="PowderBlue"
                                                    TitleClickable="True" TitleBackColor="Transparent" Text="Call" ExpandImage="../../Images/ToggleDown.gif"
                                                    CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                    <div style="overflow: auto; width: 100%; height: 250px">
                                                        <table id="Table44" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td valign="top" align="left" width="100%">
                                                                    &nbsp;
                                                                    <asp:DataGrid ID="grdcall" runat="server" Width="0px" BorderWidth="1px" CssClass="Grid"
                                                                        BorderColor="#5C5A5B" Height="0px" PageSize="25" DataKeyField="CM_NU9_Call_No_PK"
                                                                        CellPadding="0" AllowPaging="True" AutoGenerateColumns="False">
                                                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                        <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                        <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                            BackColor="#E0E0E0"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundColumn DataField="CM_NU9_Call_No_PK" HeaderText="Call No">
                                                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="ModifyBy" HeaderText="Modify By">
                                                                                <HeaderStyle Width="70px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="CN_VC20_Call_Status" HeaderText="Status">
                                                                                <HeaderStyle Width="70px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="CM_DT8_Log_Date" HeaderText="Log Date">
                                                                                <HeaderStyle Width="150px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="CM_VC100_Subject" HeaderText="Subject">
                                                                                <HeaderStyle Width="200px"></HeaderStyle>
                                                                                <ItemStyle Width="200px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="CM_VC2000_Call_Desc" HeaderText="Call Desc">
                                                                                <HeaderStyle Width="400px"></HeaderStyle>
                                                                                <ItemStyle Width="400px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="UserID" HeaderText="Call Owner">
                                                                                <HeaderStyle Width="80px"></HeaderStyle>
                                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="CM_VC8_Call_Type" HeaderText="Call Type">
                                                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="CM_VC200_Work_Priority" HeaderText="Priority">
                                                                                <HeaderStyle Width="70px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="CM_DT8_Close_Date" HeaderText="Est Close Date">
                                                                                <HeaderStyle Width="120px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                        </Columns>
                                                                        <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                        </PagerStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <cc1:CollapsiblePanel ID="cpnltasksearch" runat="server" Width="100%" BorderWidth="0px"
                                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="PowderBlue"
                                                    TitleClickable="True" TitleBackColor="Transparent" Text="Task" ExpandImage="../../Images/ToggleDown.gif"
                                                    CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                    <div style="overflow: auto; width: 100%; height: 250px">
                                                        <table id="Table47" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td valign="top" align="left" width="100%">
                                                                    &nbsp;
                                                                    <asp:DataGrid ID="grdtask" runat="server" Width="0px" BorderWidth="1px" CssClass="Grid"
                                                                        BorderColor="#5C5A5B" Height="0px" PageSize="25" DataKeyField="TM_NU9_Task_no_PK"
                                                                        CellPadding="0" AllowPaging="True" AutoGenerateColumns="False">
                                                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                        <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                        <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                            BackColor="#E0E0E0"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundColumn DataField="TM_NU9_Call_No_FK" HeaderText="Call No">
                                                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="TM_NU9_Task_no_PK" HeaderText="Task No">
                                                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="TM_VC50_Deve_status" HeaderText="Status">
                                                                                <HeaderStyle Width="70px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="ModifyBy" HeaderText="Modify By">
                                                                                <HeaderStyle Width="70px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="TM_DT8_Log_Date" HeaderText="Log Date">
                                                                                <HeaderStyle Width="150px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="TM_VC8_task_type" HeaderText="Task Type">
                                                                                <HeaderStyle Width="80px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="TM_VC1000_Subtsk_Desc" HeaderText="Task Desc">
                                                                                <HeaderStyle Width="400px"></HeaderStyle>
                                                                                <ItemStyle Width="400px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="TM_VC8_Priority" HeaderText="Priority">
                                                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="UM_VC50_UserID" HeaderText="Task Owner">
                                                                                <HeaderStyle Width="80px"></HeaderStyle>
                                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="TM_DT8_Est_close_date" HeaderText="Est Close Date">
                                                                                <HeaderStyle Width="120px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                        </Columns>
                                                                        <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                        </PagerStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                                <td valign="top" align="left" width="76%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <cc1:CollapsiblePanel ID="cpnlactionsearch" runat="server" Width="100%" BorderWidth="0px"
                                                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="PowderBlue"
                                                    TitleClickable="True" TitleBackColor="Transparent" Text="Action" ExpandImage="../../Images/ToggleDown.gif"
                                                    CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderColor="Indigo">
                                                    <div style="overflow: auto; width: 100%; height: 250px">
                                                        <table id="Table34" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td valign="top" align="left" width="100%">
                                                                    &nbsp;
                                                                    <asp:DataGrid ID="grdaction" runat="server" Width="0px" BorderWidth="1px" CssClass="Grid"
                                                                        BorderColor="#5C5A5B" Height="0px" PageSize="25" CellPadding="0" AllowPaging="True"
                                                                        AutoGenerateColumns="False">
                                                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                        <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                        <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                            BackColor="#E0E0E0"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundColumn DataField="AM_NU9_Call_Number" HeaderText="Call No">
                                                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="AM_NU9_Task_Number" HeaderText="Task No">
                                                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="AM_NU9_Action_Number" HeaderText="Action No">
                                                                                <HeaderStyle Width="70px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="AM_VC8_Call_Status" HeaderText="Call Status">
                                                                                <HeaderStyle Width="80px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="modifyBy" HeaderText="Modify By"></asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="AM_DT8_Log_Date" HeaderText="Log Date">
                                                                                <HeaderStyle Width="150px"></HeaderStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="AM_VC_2000_Description" HeaderText="Action Desc">
                                                                                <HeaderStyle Width="400px"></HeaderStyle>
                                                                                <ItemStyle Width="400px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="UM_VC50_UserID" HeaderText="Act Owner">
                                                                                <HeaderStyle Width="80px"></HeaderStyle>
                                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                        </Columns>
                                                                        <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                        </PagerStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                                <td valign="top" align="left" width="76%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                            <!-- ***********************************************************************************    -->
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
                        <asp:ListBox ID="lstError" runat="server" Width="100px" BorderStyle="Groove" BorderWidth="0"
                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txtrowvalues" />
                        <input type="hidden" name="txthiddenCallNo" />
                        <input type="hidden" name="txthiddenTable" />
                        <input type="hidden" name="txtComp" />
                        <input type="hidden" name="txtByWhom" />
                        <input type="hidden" name="txtTaskno" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
