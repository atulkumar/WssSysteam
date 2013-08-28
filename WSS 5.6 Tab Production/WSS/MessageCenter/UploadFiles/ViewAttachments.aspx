<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewAttachments.aspx.vb"
    Inherits="MessageCenter_UploadFiles_ViewAttachments" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>View Attachment</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
	       var rand_no = Math.ceil(500*Math.random())
	       
				function RefreshAttachment()
				{
					document.Form1.submit();
				}
				
				function OpenAttachCall()
				{
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?From=VIEW&ID=C' , 'UploadAttachment'+rand_no,460,450);
					return false;
				}			

				function OpenAttachTask(TaskNo)
				{
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?From=VIEW&ID=T&VTaskNo=' + TaskNo,'UploadAttachment'+rand_no,460,450);
					return false;
				}			

				function OpenAttachAction(TaskNo, ActionNo)
				{
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?From=VIEW&ID=A&VTaskNo='+ TaskNo +'&VActionNo='+ ActionNo ,'UploadAttachment'+rand_no,460,450);
					return false;
				}			
				
				function SaveEdit(varImgValue)
				{			    															
				             if ('<%=Request.QueryString("Page_name") %>' == 'Call_Heirarchy')
                             {
                                var oWindow = null;
                                if (window.radWindow) oWindow = window.radWindow;
                                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                                oWindow.close();
                            }
                            else 
                            {
                                window.close();
                            }
						if (varImgValue=='Delete')
						
						{
							var confirmed;
							confirmed=window.confirm("Do You Want To Delete The Record ?");
							if(confirmed==true)
							{	
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
							}		
							else
							{
						            return false;	    
							}
					    }	
										
				}			
					
	            function CloseWindow() 
					{ 					
					    self.opener.callrefresh(); 
					} 
						
					
	            function CheckBox(ID)
                    {

					    if(document.getElementById(ID).checked==true)
						    {
							    document.getElementById(ID).checked=false;
						    }
						    else
						    {
							    document.getElementById(ID).checked=true;
						    }
                   }
               
               
           	function KeyCheck(rowvalues)
					{
				
					//GrdAddSerach__ctl2_chkReq
				
					if(document.getElementById('GrdAddSerach__ctl'+(rowvalues+1)+'_chkReq').checked==true)
						{
							document.getElementById('GrdAddSerach__ctl'+(rowvalues+1)+'_chkReq').checked=false;
						}
				else
					{
						document.getElementById('GrdAddSerach_ctl'+(rowvalues+1)+'_chkReq').checked=true;
					}
				
						CheckStatus();
				
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
								
								if (tableID=='cpnlTaskView_GrdAddSerach')
								{
									document.Form1.txthiddenImage.value='Select';
									setTimeout('Form1.submit();',700);
									//Form1.submit(); 
								}	
					}	
					
						
		function CheckStatus()
				{
								var chkID='cPnlCallAtt_grdAttachments_ctl2_chkCall';
								var chk=0;
								var unchk=0;
								var tableID='cPnlCallAtt_grdAttachments' ;
								var table;
								if (document.all) table=document.all[tableID];
								if (document.getElementById) table=document.getElementById(tableID);
								var n=table.rows.length-1;
								
								for(var i=0; i<n; i++)
								{
										if(document.getElementById('cPnlCallAtt_grdAttachments_ctl'+(i+2)+'_chkReq').checked==true)
										{
												chk=1;
										}
										else
										{
												unchk=1;
										}
								}
							
								if(chk==1 && unchk==1)
								{
									document.getElementById(chkID).checked=false;
								}
								if(chk==1 && unchk==0)
								{
									document.getElementById(chkID).checked=true;
								}
								if(chk==0 && unchk==1)
								{
									document.getElementById(chkID).checked=false;
								}				
												
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
					// Just in case width and height are ignored
					win.resizeTo(w, h);
					// Just in case left and top are ignored
					win.moveTo(wleft, wtop);
					win.focus();
					return false;
				}			
														
    </script>
<script type="text/javascript">
    //A Function to improve design i.e delete the extra cell of table
    function onEnd() {
        if (document.getElementById('cPnlCallAtt_collapsible')!=null)
        {
            var x = document.getElementById('cPnlCallAtt_collapsible').cells[0].colSpan = "1";
        }
        if( document.getElementById('cPnlTaskAtt_collapsible') !=null)
        {
        var y = document.getElementById('cPnlTaskAtt_collapsible').cells[0].colSpan = "1";}
        if( document.getElementById('cPnlActionAtt_collapsible') !=null)
        {
        var z = document.getElementById('cPnlActionAtt_collapsible').cells[0].colSpan = "1";
        }
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

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table id="Table13" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        background="../../images/top_nav_back.gif" border="0">
        <tr>
            <td style="width: 229px">
                <asp:Label ID="lblTitleLabelViewAttachment" runat="server" CssClass="TitleLabel"
                    BorderStyle="None" BorderWidth="2px" Width="184px">&nbsp;View Attachment</asp:Label>
            </td>
            <td style="width: 644px">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img
                    title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ToolTip="Delete" ImageUrl="../../Images/s2delete01.gif">
                </asp:ImageButton>&nbsp;<asp:ImageButton ID="imgClose" AccessKey="L" runat="server"
                    ToolTip="Close" ImageUrl="../../Images/s2close01.gif"></asp:ImageButton>&nbsp;<img
                        title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">
            </td>
            <td>
                &nbsp;
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table1" bordercolor="lightgrey" cellspacing="0" cellpadding="0" width="100%"
        bgcolor="#f5f5f5" border="2">
        <tr>
            <td bordercolor="#f5f5f5">
                <cc1:CollapsiblePanel ID="cPnlCallAtt" runat="server" BorderStyle="Solid" BorderWidth="0px"
                    Width="99.65%" Height="0.53%" BorderColor="Indigo" TitleCSS="test" PanelCSS="panel"
                    TitleForeColor="PowderBlue" TitleClickable="True" TitleBackColor="Transparent"
                    Text="Call Attachment" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                    Draggable="False">
                    <div style="overflow: auto; width: 100%; height: 192px">
                        <asp:DataGrid ID="grdAttachments" runat="server" BorderWidth="1px" BorderStyle="None"
                            BorderColor="Silver" ForeColor="MidnightBlue" Font-Names="Verdana" HorizontalAlign="Left"
                            CellPadding="1" AutoGenerateColumns="False" PagerStyle-Visible="False" BackColor="#E0E0E0">
                            <SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
                            <AlternatingItemStyle BackColor="WhiteSmoke"></AlternatingItemStyle>
                            <ItemStyle Font-Size="8pt" ForeColor="Black" BackColor="White"></ItemStyle>
                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                BackColor="#E0E0E0"></HeaderStyle>
                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCall" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="U" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ToolTip="Upload More Attachments" ID="imgCallAttach" runat="server"
                                            ImageUrl="../../Images/Attach15_9.gif"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Attachment">
                                    <ItemStyle Width="210px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="systemLink" runat="server" CommandArgument='<%# DataBinder.Eval(Container," DataItem.VH_VC255_File_Path") %>'
                                            CommandName='<%# DataBinder.Eval(Container, "DataItem.VH_VC255_File_Name") %>'>
													<%# DataBinder.Eval(Container, "DataItem.VH_VC255_File_Name") %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="VH_VC255_File_Name" HeaderText="File Name">
                                    <HeaderStyle Width="130pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Call_Number" HeaderText="CallNo"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CI_VC36_Name" HeaderText="User">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_DT8_Date" HeaderText="Date">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Version" HeaderText="Version">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="VH_NU9_File_ID_PK" HeaderText="FileID">
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False"></PagerStyle>
                        </asp:DataGrid>
                    </div>
                </cc1:CollapsiblePanel>
                <cc1:CollapsiblePanel ID="cPnlTaskAtt" runat="server" BorderStyle="Solid" BorderWidth="0px"
                    Width="99.46%" Height="15.44%" BorderColor="Indigo" TitleCSS="test" PanelCSS="panel"
                    TitleForeColor="PowderBlue" TitleClickable="True" TitleBackColor="Transparent"
                    Text="Task Attachment" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                    Draggable="False">
                    <div style="overflow: auto; width: 100%; height: 192px">
                        <asp:DataGrid ID="grdTask" runat="server" BorderWidth="1px" BorderStyle="None" BorderColor="Silver"
                            ForeColor="MidnightBlue" Font-Names="Verdana" HorizontalAlign="Left" CellPadding="1"
                            AutoGenerateColumns="False" PagerStyle-Visible="False" BackColor="#E0E0E0">
                            <SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
                            <AlternatingItemStyle BackColor="WhiteSmoke"></AlternatingItemStyle>
                            <ItemStyle Font-Size="8pt" ForeColor="Black" BackColor="White"></ItemStyle>
                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                BackColor="#E0E0E0"></HeaderStyle>
                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkTask" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="U" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ToolTip="Upload More Attachments" ID="imgTaskAttach" runat="server"
                                            ImageUrl="../../Images/Attach15_9.gif"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Attachment">
                                    <ItemStyle Width="170px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Linkbutton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container,  "DataItem.VH_VC255_File_Path") %>'
                                            CommandName='<%# DataBinder.Eval(Container, "DataItem.VH_VC255_File_Name") %>'>
													<%# DataBinder.Eval(Container, "DataItem.VH_VC255_File_Name") %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="VH_VC255_File_Name" HeaderText="File Name">
                                    <HeaderStyle Width="120pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Call_Number" HeaderText="CallNo"></asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Task_Number" HeaderText="TaskNo"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CI_VC36_Name" HeaderText="User">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_DT8_Date" HeaderText="Date">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Version" HeaderText="Version">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="VH_NU9_File_ID_PK" HeaderText="FileID">
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False"></PagerStyle>
                        </asp:DataGrid>
                    </div>
                </cc1:CollapsiblePanel>
                <cc1:CollapsiblePanel ID="cPnlActionAtt" runat="server" BorderStyle="Solid" BorderWidth="0px"
                    Width="99.53%" Height="15.44%" BorderColor="Indigo" TitleCSS="test" PanelCSS="panel"
                    TitleForeColor="PowderBlue" TitleClickable="True" TitleBackColor="Transparent"
                    Text="Action Attachment" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                    Draggable="False">
                    <div style="overflow: auto; width: 100%; height: 192px">
                        <asp:DataGrid ID="grdAction" runat="server" BorderWidth="1px" BorderStyle="None"
                            BorderColor="Silver" ForeColor="MidnightBlue" Font-Names="Verdana" HorizontalAlign="Left"
                            CellPadding="1" AutoGenerateColumns="False" PagerStyle-Visible="False" BackColor="#E0E0E0">
                            <SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
                            <AlternatingItemStyle BackColor="WhiteSmoke"></AlternatingItemStyle>
                            <ItemStyle Font-Size="8pt" ForeColor="Black" BackColor="White"></ItemStyle>
                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                BackColor="#E0E0E0"></HeaderStyle>
                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAction" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="U" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ToolTip="Upload More Attachments" ID="imgActionAttach" runat="server"
                                            ImageUrl="../../Images/Attach15_9.gif"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Attachment">
                                    <ItemStyle Width="140px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Linkbutton2" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.VH_VC255_File_Path") %>'
                                            CommandName='<%# DataBinder.Eval(Container, "DataItem.VH_VC255_File_Name") %>'>
													<%# DataBinder.Eval(Container, "DataItem.VH_VC255_File_Name") %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="VH_VC255_File_Name" HeaderText="File Name">
                                    <HeaderStyle Width="100pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Call_Number" HeaderText="CallNo"></asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Task_Number" HeaderText="TaskNo"></asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Action_Number" HeaderText="ActionNo"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CI_VC36_Name" HeaderText="User">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_DT8_Date" HeaderText="Date">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="VH_NU9_Version" HeaderText="Version">
                                    <HeaderStyle Width="70pt"></HeaderStyle>
                                    <ItemStyle Width="70pt"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="VH_NU9_File_ID_PK" HeaderText="FileID">
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False"></PagerStyle>
                        </asp:DataGrid>
                    </div>
                </cc1:CollapsiblePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
            <input type="hidden" name="txthiddenImage"><!-- Image Clicked-->
        </ContentTemplate>
    </asp:UpdatePanel>
    </td>
    </form>
</body>
</html>
