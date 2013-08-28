<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_AddressBook_AB_ViewColumns, App_Web_p6k4cy-b" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../../images/js/select_fix.js"></script>

    <script type="text/javascript">SelectFix.autoRepairFloatingElements(500);</script>

    
    <script language="javascript" type="text/javascript">
   
    
    </script>
   <%--<script type="text/javascript">
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
    </script>--%>

   <%-- <style type="text/css">
        .select-free
        {
            overflow: hidden; /*must have*/
        }
        .select-free .innerFixer
        {
            position: absolute; /*must have*/
            top: 0; /*must have*/
            left: 0; /*must have*/
            display: none; /*sorry for IE5*/
            display: /**/ block; /*sorry for IE5*/
            width: 100%; /* (old value 3000px) must have for any big value*/
            height: 100%; /* (old value 3000px) must have for any big value*/
        }
        .select-free iframe.innerFixer
        {
            border: 0;
            filter: mask(); /*must have*/
            z-index: -1; /*must have*/
        }
        .floating_block
        {
            position: absolute;
            z-index: 10;
        }
        select
        {
            display: block;
        }
    </style>--%>
    <%--<script language="javascript" type="text/javascript">
        window.close=alert(1);
    
    </script>--%>
</head>
<body bgcolor="#f5f5f5" onunload="refreshOnUnload();">
<script language="Javascript" type="text/javascript">
		
	
	function validateFAValue()
	{
	
			var lstCount=0;
			var ddlid;
			var id=1;
			var i;
	        var GridClientId;
	  lstCount=document.getElementById('txtCountHID').value;	
	
    // alert(lstCount);
	
	  for(i=1;i<=lstCount;i++)
					{
						id=id+1;
						//To check the Listbox items count i.e genration of Gridview client id on basis of no of items
						if(id<=9)
			                {
			                    GridClientId='GrdView_ctl0';
			                }
			            else
			                {
			                    GridClientId='GrdView_ctl';
			                }
					ddlid=document.getElementById(GridClientId+ id +'_ddlFA').value;
					//alert(ddlid);

					if (ddlid !='')
						{
						document.getElementById(GridClientId+ id +'_txtvalue').disabled=false;
					
						}
							else
							{
							document.getElementById(GridClientId+ id +'_txtvalue').value="";
							document.getElementById(GridClientId+ id +'_txtvalue').disabled=true;
							}

				}
	

	}
	
	
	
	
	
function ChkUniqueNumber()
{
//alert(document.getElementById('GrdView__ctl2_chk').checked);
	var lstCount=0;
	
	lstCount=document.getElementById('txtCountHID').value;	
	var j=0;
	var i=0;
	var uni=0;
	var c=0;
	var id=1;
	var ids=0;
	 var GridClientId;
	for(i=1;i<=lstCount;i++)
	{
				id=id+1;
				//To check the Listbox items count i.e genration of Gridview client id on basis of no of items
						if(id<=9)
			                {
			                    GridClientId='GrdView_ctl0';
			                }
			            else
			                {
			                    GridClientId='GrdView_ctl';
			                }
				uni=parseInt(document.getElementById(GridClientId+ id +'_txtbx').value);
										
						if (uni > lstCount)
						{
						
						
						alert("Value out of range");
							return false;
						}
						if(uni=="")
						{
							continue;
						}
				ids=id;
				for(j=i+1;j<=lstCount;j++)
				{
						ids=ids+1;
						if(ids<=9)
			                {
			                    GridClientId='GrdView_ctl0';
			                }
			            else
			                {
			                    GridClientId='GrdView_ctl';
			                }
						c=parseInt(document.getElementById(GridClientId+ ids +'_txtbx').value);
						
						
						
						if(c>lstCount||uni>lstCount)
						{
												
						alert("Value out of range");
							return false;
						}
						if(c=="")
						{
							continue;
						}
						if(c==uni)
						{
							alert("Value repeated");
							return false;
						}
						
				}
	}
//alert('check');	
if(ChkSeq()==true)
{
return true;
}
else
{
return false;
}
}

function ChkSeq()
{
	var lstCount=0;
	lstCount=parseInt(document.getElementById('txtCountHID').value);	
var k=0;
var max=0;
var idd=1;
var v=0;


var j=0;
var i=0;
var id;
var n=0;
var min=1;
var flag=0;
var m=1;
var flgBlank=0;
	 var GridClientId;
	for(k=1;k<=lstCount;k++)
	{
	idd=idd+1;
	
						if(idd<=9)
			                {
			                    GridClientId='GrdView_ctl0';
			                }
			            else
			                {
			                    GridClientId='GrdView_ctl';
			                }
	//alert('GrdView__ctl'+ idd +'_txtbx');
	if (document.getElementById(GridClientId + idd +'_txtbx').value=='')
	{
	v=-1;
	}
	else
	{
	//alert(document.getElementById('GrdView__ctl'+ idd +'_txtbx').value);
		v=parseInt(document.getElementById(GridClientId + idd +'_txtbx').value);
		if(v==0)
		{
		alert('Sequence number should be start from 1');
		return false;
		}
	}
			if(v>max)
		{
			max=v;
		}
	}
	//alert(v);
	if(v==0)
	{ 
		 return true; 
	}
	
	for(i=1;i<=lstCount;i++)
	{
			id=1;
			flag=0;
			for(j=1;j<=lstCount;j++)
			{
			m=0;
					id=id+1;
					if(id<=9)
			                {
			                    GridClientId='GrdView_ctl0';
			                }
			            else
			                {
			                    GridClientId='GrdView_ctl';
			                }
					if (document.getElementById(GridClientId+ id +'_txtbx').value=='')
					{
						n=0;
					}
					else
					{
						n=parseInt(document.getElementById(GridClientId+ id +'_txtbx').value);
						flgBlank=1;
					}
					if (n==min)
					{
						flag=1;
						m=3;
						continue;
					}
					
		}

				if((m==0 && flag==0) && (flgBlank==1))
				{	
					alert('Sequence Problem');
					document.getElementById(GridClientId+ id +'_txtbx').focus();
					document.getElementById(GridClientId+ id +'_txtbx').select();
					flag=0;  
					return false;
				}					
			min=min+1;  	
			if(min>max)
			{
				return  true;
				alert("true");
			}
		}

}
   
	function SaveEdit(varimgValue)
				{
			    			
									
//						if (varimgValue=='Close')
//						{
//						window.close(); 
//						CloseWindow(); 
//						}
//								
          if (varimgValue == 'Close') {
                //alert('<%=Session("CallPlus") %>');
                if ('<%=Request.QueryString("Page_name") %>' == 'Call_Heirarchy')
                 {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    oWindow.close();
                }
                else 
                {
                    self.opener.Form1.submit();
                    window.close();
                }
                return false;
            }

								
						if (varimgValue=='Add')
										{
											document.Form1.txthiddenImage.value=varimgValue;
											Form1.submit(); 
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
                         

									if (ChkUniqueNumber()==true)
									{
									    document.Form1.txthiddenImage.value=varimgValue;
									    Form1.submit();
									}
								  if ('<%=Request.QueryString("Page_name") %>' == 'Call_Heirarchy')
                                             {
                                                var oWindow = null;
                                                if (window.radWindow) oWindow = window.radWindow;
                                                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                                                oWindow.close();
                                            }
                                            else
                                            {
								                CloseWindow(); 
								                return false; 
                                         }       
                                   
									
									
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


									if (ChkUniqueNumber()==true)
									{
									    document.Form1.txthiddenImage.value=varimgValue;
									    Form1.submit();
									}
									return false; 
									//CloseWindow();
						}		
							
						if (varimgValue=='Reset')
						{
									var confirmed
									confirmed=window.confirm("Do You Want To reset The Page ?");
									if(confirmed==true)
											{	
													Form1.reset()
											}		
					   }		
					   
					  if (varimgValue=='Delete') 
							
							{
						
							var confirmed
							confirmed=window.confirm("Do You Want To Delete The View ?");
												
							if (confirmed==true)
										{
									   document.Form1.txthiddenImage.value=varimgValue;
									  	Form1.submit();
																	  
										}
										else
										{
										return false;
										}
							}
					   	
				
								
					   	 if (varimgValue=='Update') 
							
							{
							
						if (ChkUniqueNumber()==true)
						
						{
							var confirmed
							confirmed=window.confirm("Do You Want To Update The View ?");
												
							if (confirmed==true)
										{
						
								  	document.Form1.txthiddenImage.value=varimgValue;
										Form1.submit();
										return false;
										  
										}
										else
										{
										return false;
										}
							}
							}
				}			
						
					function callrefresh()
				    {
					    document.Form1.txthiddenImage.value='';
			    	    Form1.submit();
			    	    return false;
				    }
				function CloseWindow() 
						{ 
					
						self.opener.callrefresh(); 
						} 
				 function refreshOnUnload()
				{
				   if ('<%=Request.QueryString("Page_name") %>' != 'Call_Heirarchy')
                    {
				        window.opener.location.reload();
				    }
				}
    </script>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptMAnager1" runat="server">
    </asp:ScriptManager>
    <table id="table11" cellspacing="0" cellpadding="0" width="100%" bgcolor="#e0e0e0"
        border="0">
        <tr>
            <td align="center" bgcolor="lightgrey" background="../../Images/top_nav_back.gif" height="47">
                <asp:Label ID="lblTitleLabelCallView" runat="server" CssClass="TitleLabel">VIEW</asp:Label>
                <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0" />&nbsp;
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                    ToolTip="Save"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif"
                    ToolTip="OK"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                    ToolTip="Edit"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                    ToolTip="Reset"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                    ToolTip="Delete"></asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                    ToolTip="Close"></asp:ImageButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0" />
                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('921','../../');"
                    alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />
            </td>
        </tr>
    </table>
    <table id="table1" style="height: 400px" bordercolor="lightgrey" height="400" cellspacing="0"
        cellpadding="0" width="450" bgcolor="#f5f5f5" border="0">
        <tr>
            <td colspan="8">
                <cc1:CollapsiblePanel ID="cpnlErrorPanel" runat="server" Width="100%" BorderWidth="0px"
                    BorderStyle="Solid" Visible="False" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                    ExpandImage="../../Images/ToggleDown.gif" Text="Error Message" TitleBackColor="transparent"
                    TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                    BorderColor="Indigo">
                    <table id="table2" style="width: 450px; height: 20px" bordercolor="lightgrey" cellspacing="0"
                        cellpadding="0" width="466" border="0">
                        <tr>
                            <td valign="middle" colspan="0" rowspan="0">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../../Images/warning.gif" Width="16px"
                                    Height="16px"></asp:Image>
                            </td>
                            <td colspan="0" rowspan="0">
                                <asp:ListBox ID="lstError" runat="server" Width="416px" Height="48px" Font-Size="XX-Small"
                                    Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
            </td>
        </tr>
        <tr valign="top">
            <td style="border-color: #f5f5f5;" align="left" colspan="8" valign="top">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="50%">
                            <asp:Label ID="Label1" runat="server" Width="32px" Height="16px" Font-Size="XX-Small"
                                Font-Names="Verdana" Font-Bold="true">Name</asp:Label>&nbsp;&nbsp;<asp:TextBox ID="txtViewName"
                                    runat="server" CssClass="txtNoFocus" Width="128px" BorderWidth="1px" BorderStyle="Solid"
                                    Height="18px" Font-Size="XX-Small" Font-Names="Verdana" MaxLength="50"></asp:TextBox>
                        </td>
                        <td width="50%">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Width="72px" Height="16px" Font-Size="XX-Small"
                                            Font-Names="Verdana" Font-Bold="true">Modify View</asp:Label>&nbsp;
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlview" runat="server" Width="104px" Height="18px" Font-Size="10pt"
                                            Font-Names="verdana" AutoPostBack="true" Font-Name="verdana">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <table cellspacing="0" cellpadding="0" width="430" border="0">
                    <tr>
                        <td style="width: 193px">
                            <font face="Verdana" size="1"><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Default
                                Columns</strong></font>
                        </td>
                        <td>
                        </td>
                        <td>
                            <font face="Verdana" size="1"><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                Select your own columns</strong></font>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 193px" valign="top" bordercolor="#f5f5f5" align="right">
                            <asp:ListBox ID="lstdefaultcolumn" runat="server" Width="168px" Height="200px" Font-Size="X-Small"
                                Font-Names="Tahoma" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td valign="middle" bordercolor="#f5f5f5" align="left">
                            <p>
                                <asp:Button ID="btnaddall1" runat="server" Text=">>"></asp:Button></p>
                            <p>
                                <asp:Button ID="btnadditem1" runat="server" Width="25px" Text=">"></asp:Button></p>
                            <p>
                                <asp:Button ID="btnremoveitem1" runat="server" Width="24px" Text="<"></asp:Button></p>
                            <p>
                                <asp:Button ID="btnremoveall1" runat="server" Text="<<"></asp:Button></p>
                        </td>
                        <td valign="top" bordercolor="#f5f5f5" align="right">
                            <asp:ListBox ID="lstusercolumn" runat="server" Width="168px" Height="200px" Font-Size="X-Small"
                                Font-Names="Tahoma" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td valign="middle" bordercolor="#f5f5f5" align="left">
                            <p>
                                <asp:Button ID="btnColmoveup1" runat="server" Text="^"></asp:Button></p>
                            <p>
                                <asp:Button ID="btnColmovedwn1" runat="server" Text="v"></asp:Button></p>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 193px" valign="top" bordercolor="#f5f5f5" align="center">
                        </td>
                        <td valign="middle" bordercolor="#f5f5f5" align="left">
                        </td>
                        <td valign="top" bordercolor="#f5f5f5" align="left">
                            &nbsp;
                        </td>
                        <td valign="middle" bordercolor="#f5f5f5" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" bordercolor="#f5f5f5" colspan="4">
                            <div style="overflow: auto; width: 450px; height: 200px" align="right">
                                <asp:DataGrid ID="GrdView" runat="server" CssClass="Grid" Width="416px" BorderColor="Silver"
                                    Height="10px" AutoGenerateColumns="False" CellPadding="0">
                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                    <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <HeaderStyle CssClass="GridHeader" HorizontalAlign="Left"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="UV_VC50_COL_Name" HeaderText="Column Name"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="A/D">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlAD" runat="server" Font-Name="verdana" Font-Size="xx-small"
                                                    Height="18px" SelectedValue='<%# databinder.eval(container.dataitem,"UV_VC5_AD") %>'>
                                                    <asp:ListItem Value="UnSorted"></asp:ListItem>
                                                    <asp:ListItem Value="Asc"></asp:ListItem>
                                                    <asp:ListItem Value="Desc"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="SO">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtbx" Font-Name="verdana" Font-Size="xx-small" runat="server" Width="50pt"
                                                    Height="13px" MaxLength="2" Text='<%# databinder.eval(container.dataitem,"UV_NU9_SO") %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="FA">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlFA" runat="server" Font-Name="verdana" Font-Size="xx-small"
                                                    Height="18px" SelectedValue='<%# databinder.eval(container.dataitem,"UV_VC5_FA") %>'>
                                                    <asp:ListItem Value=""></asp:ListItem>
                                                    <asp:ListItem Value="<"></asp:ListItem>
                                                    <asp:ListItem Value=">"></asp:ListItem>
                                                    <asp:ListItem Value="<="></asp:ListItem>
                                                    <asp:ListItem Value=">"="></asp:ListItem>
                                                    <asp:ListItem Value="="></asp:ListItem>
                                                    <asp:ListItem Value="!="></asp:ListItem>
                                                    <asp:ListItem Value="IN"></asp:ListItem>
                                                    <asp:ListItem Value="NOT IN"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Value">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtvalue" runat="server" MaxLength="100" Width="100pt" Font-Name="verdana"
                                                    Font-Size="xx-small" Height="13px" Text='<%# databinder.eval(container.dataitem,"UV_VC20_Value") %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid></div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <input id="txtCountHID" type="hidden" name="txtCountHidden" runat="server">
                        <input type="hidden" name="txthiddenImage"><!-- Image Clicked-->
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
