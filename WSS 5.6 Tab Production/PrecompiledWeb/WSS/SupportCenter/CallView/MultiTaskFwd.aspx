<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_MultiTaskFwd, App_Web_ixviuxgi" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Multi Task forward</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../../DateControl/ION.js"></script>

    <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../../Images/Js/ABMainShortCuts.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative; ;TOP:expression(this.offsetParent.scrollTop);BACKGROUND-COLOR:#e0e0e0}</style>

    <script type="text/javascript">
		
		
						
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
          
   function CheckAll(ID)

{

var tableID='cpnlSearch_GrdTask'; 

var table;

table=document.getElementById(tableID);

var n=table.rows.length-1;

if(document.getElementById(ID).checked==true)

{

for(var i=0; i<n; i++)

{

if ( i+2 <10 )

{

document.getElementById('cpnlSearch_GrdTask_ctl0'+(i+2)+'_chkReq').checked=true;

}

else

{

document.getElementById('cpnlSearch_GrdTask_ctl'+(i+2)+'_chkReq').checked=true; 

}


}

}

else

{

for(var i=0; i<n; i++)

{

if ( i+2 <10 )

{

document.getElementById('cpnlSearch_GrdTask_ctl0'+(i+2)+'_chkReq').checked=false;

}

else

{

document.getElementById('cpnlSearch_GrdTask_ctl'+(i+2)+'_chkReq').checked=false; 

}

}

}

}

      
          

	
					
	function CheckStatus()

{

var chkID='cpnlSearch_GrdTask_ctl01_chkAll';

var chk=0;

var unchk=0;

var tableID='cpnlSearch_GrdTask' ;

var table;

if (document.all) table=document.all[tableID];

if (document.getElementById) table=document.getElementById(tableID);

var n=table.rows.length-1;


for(var i=0; i<n; i++)

{

var Temp;

if ( i+2 < 10)

{

Temp='cpnlSearch_GrdTask_ctl0'+(i+2)+'_chkReq'

}

else

{

Temp='cpnlSearch_GrdTask_ctl'+(i+2)+'_chkReq'

}

if(document.getElementById(Temp).checked==true)

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

				
				function closeWindow(varImgValue)
				{
																
										if (varImgValue=='Close')
										{
													window.close();
													return false;	
										}
										return false;
				}
				
				
function KeyCheck(rowvalues)
					{
					
				if (rowvalues < 10)
				{
			if(document.getElementById('cpnlSearch_GrdTask_ctl0'+(rowvalues+1)+'_chkReq').checked==true)
						{
							document.getElementById('cpnlSearch_GrdTask_ctl0'+(rowvalues+1)+'_chkReq').checked=false;
						}
						else
						{
							document.getElementById('cpnlSearch_GrdTask_ctl0'+(rowvalues+1)+'_chkReq').checked=true;
						}	
				}
				
				else
				{
				if(document.getElementById('cpnlSearch_GrdTask_ctl'+(rowvalues+1)+'_chkReq').checked==true)
						{
							document.getElementById('cpnlSearch_GrdTask_ctl'+(rowvalues+1)+'_chkReq').checked=false;
						}
						else
						{
							document.getElementById('cpnlSearch_GrdTask_ctl'+(rowvalues+1)+'_chkReq').checked=true;
						}
				}
									
						
						CheckStatus();
						var tableID='cpnlSearch_GrdTask'  //your datagrids id
										
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
								
								if (tableID=='cpnlSearch_GrdTask')
								{
									document.Form1.txthiddenImage.value='Select';
									//setTimeout('Form1.submit();',700);
									//Form1.submit(); 
								}	
					}	
						


						
				
				function SaveEdit(varImgValue)
				{			
					if (varImgValue=='Close')
						{
						     location.href="../../home.aspx"
								return false;
						}	
						if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
			
			if (varImgValue=='Close')
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
						
		if (varImgValue=='Save')
			    	{
			    	
		    		document.Form1.txthiddenImage.value=varImgValue;
//		    	__doPostBack("up2","");
//		    		return false;		
			    	}	
							
															
				}
					
				
		
		
					
    </script>

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlAdvSearch2_collapsible').cells[0].colSpan = "1";
            var y = document.getElementById('cpnlSearch_collapsible').cells[0].colSpan = "1";
        } 
         //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	

    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%"  border="0">
                                            <tr>
                                                <td style="width: 20%">
                                                   &nbsp;&nbsp;&nbsp;&nbsp; <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="0px" Height="0px"
                                                        BorderWidth="0px" AlternateText="." CommandName="submit" ImageUrl="white.GIF"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelSearch" runat="server" CssClass="TitleLabel" BorderWidth="2px"
                                                        BorderStyle="None">Multi Task forward</asp:Label>
                                                </td>
                                                <td style="width: 70%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                                                            ToolTip="Save"></asp:ImageButton>&nbsp;<img src="../../Images/reset_20.gif" title="Refresh"
                                                                alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                        &nbsp;<img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                          <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2209','../../');"
                                            alt="Video Help" src="../../Images/video_help.jpg" border="0">&nbsp;
                                        <img class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('967','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <tr>
                                <td style="height: 581px">
                                    <div style="overflow: auto; width: 100%; height: 581px">
                                        <table id="Table32" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td valign="top" align="left">
                                                    <asp:UpdatePanel ID="up1" runat="server">
                                                        <ContentTemplate>
                                                            <cc1:CollapsiblePanel ID="cpnlAdvSearch2" runat="server" Width="100%" BorderWidth="0px"
                                                                BorderStyle="Solid" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                                ExpandImage="../../Images/ToggleDown.gif" Text="Search Criteria" TitleBackColor="Transparent"
                                                                TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                                <table bordercolor="#5c5a5b" cellspacing="0px" cellpadding="0px" width="700px" border="0px">
                                                                    <tr>
                                                                        <td bordercolor="#f5f5f5">
                                                                            <asp:Label ID="Label5" runat="server" CssClass="FieldLabel">Company</asp:Label><br>
                                                                            <asp:DropDownList ID="ddlCompany" TabIndex="3" Width="113px" CssClass="txtNoFocus"
                                                                                AutoPostBack="True" runat="server">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td bordercolor="#f5f5f5">
                                                                            <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">SubCategory</asp:Label><br>
                                                                            <asp:DropDownList ID="ddlProject" TabIndex="3" Width="120px" CssClass="txtNoFocus"
                                                                                AutoPostBack="True" runat="server">
                                                                                <asp:ListItem></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td bordercolor="#f5f5f5">
                                                                            <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">Task Owner</asp:Label><br>
                                                                            <asp:DropDownList ID="ddlTaskOwner" TabIndex="3" Width="120px" CssClass="txtNoFocus"
                                                                                AutoPostBack="True" runat="server">
                                                                                <asp:ListItem></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td bordercolor="#f5f5f5">
                                                                            <asp:Label ID="Label3" runat="server" Width="65px" CssClass="FieldLabel">Assign To</asp:Label><br>
                                                                            <asp:DropDownList ID="ddlAssignTo" TabIndex="3" Width="120px" CssClass="txtNoFocus"
                                                                                runat="server">
                                                                                <asp:ListItem></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td valign="bottom" bordercolor="#f5f5f5">
                                                                            <asp:CheckBox ID="chkMailstatus" runat="server" CssClass="FieldLabel" Text="Send Mail"
                                                                                Checked="True"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </cc1:CollapsiblePanel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td valign="top" align="left">
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:UpdatePanel ID="up2" runat="server">
                                            <ContentTemplate>
                                                <cc1:CollapsiblePanel ID="cpnlSearch" runat="server" Width="100%" BorderWidth="0px"
                                                    BorderStyle="Solid" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                    ExpandImage="../../Images/ToggleDown.gif" Text="Task List" TitleBackColor="Transparent"
                                                    TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="100%">
                                                                <div style="overflow: auto; width: 100%; height: 380px">
                                                                    <asp:DataGrid ID="GrdTask" runat="server" BorderStyle="None" BorderWidth="1px" CssClass="Grid"
                                                                        BorderColor="Silver" ForeColor="MidnightBlue" Font-Names="Verdana" CellPadding="0"
                                                                        GridLines="Horizontal" HorizontalAlign="Left" PageSize="50" AutoGenerateColumns="False">
                                                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                        <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:TemplateColumn>
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="chkAll" runat="server" Width="20pt"></asp:CheckBox>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkReq" runat="server" Width="20pt"></asp:CheckBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="CallNo">
                                                                                <HeaderStyle Width="70px"></HeaderStyle>
                                                                                <ItemStyle Width="70px"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCallNo" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container.DataItem, "CallNo") %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="TaskNo">
                                                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                                                <ItemStyle Width="60px"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbTaskNo" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem, "TaskNo") %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="Company">
                                                                                <HeaderStyle Width="110px"></HeaderStyle>
                                                                                <ItemStyle Width="110px"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblcompany" runat="server" Width="110px" Text='<%# DataBinder.Eval(Container.DataItem, "Company") %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="Priority">
                                                                                <HeaderStyle Width="70px"></HeaderStyle>
                                                                                <ItemStyle Width="70px"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPriority" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container.DataItem, "Priority") %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="TaskDesc">
                                                                                <HeaderStyle Width="260px"></HeaderStyle>
                                                                                <ItemStyle Width="260px"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTaskDesc" runat="server" Width="260px" Text='<%# DataBinder.Eval(Container.DataItem, "TaskDesc") %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="AssignBy">
                                                                                <HeaderStyle Width="80px"></HeaderStyle>
                                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblAssignBy" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container.DataItem, "AssignBy") %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="EstCloseDate">
                                                                                <HeaderStyle Width="100px"></HeaderStyle>
                                                                                <ItemStyle Width="100px"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEstCloseDate" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "EstCloseDate") %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                        </Columns>
                                                                        <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                    </asp:DataGrid></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc1:CollapsiblePanel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden">
                        <input type="hidden" name="txthiddenImage">
                        <input type="hidden" name="txtrowvalues">
                        <input type="hidden" name="txthiddenCallNo">
                        <input type="hidden" name="txthiddenTable">
                        <input type="hidden" name="txtComp">
                        <input type="hidden" name="txtByWhom">
                        <input type="hidden" name="txtTaskno">
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
