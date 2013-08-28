<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_UserOverwriteView_AdminEditView, App_Web_wkfch9dl" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Address Set Columns</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../images/js/select_fix.js"></script>

    <script type="text/javascript">SelectFix.autoRepairFloatingElements(500);</script>

    <script language="Javascript" type="text/javascript">
	
	
	
	function validateFAValue()
	{
	
			var lstCount=0;
			var ddlid;
			var id=1;
			var i;
	
        lstCount=document.getElementById('txtCountHID').value;	
	
    // alert(lstCount);
	
	  for(i=1;i<=lstCount;i++)
{
						id=id+1;
					
    if (id<=9)
    {	
    //alert(id);
					ddlid=document.getElementById('GrdView_ctl0'+ id +'_ddlFA').value;
					//alert(ddlid);

					if (ddlid !='')
						{
						document.getElementById('GrdView_ctl0'+ id +'_txtvalue').disabled=false;
					
						}
							else
							{
							document.getElementById('GrdView_ctl0'+ id +'_txtvalue').value="";
							document.getElementById('GrdView_ctl0'+ id +'_txtvalue').disabled=true;
							}

	}
	else
	{
				    ddlid=document.getElementById('GrdView_ctl'+ id +'_ddlFA').value;
					//alert(ddlid);

					if (ddlid !='')
						{
						document.getElementById('GrdView_ctl'+ id +'_txtvalue').disabled=false;
					
						}
							else
							{
							document.getElementById('GrdView_ctl'+ id +'_txtvalue').value="";
							document.getElementById('GrdView_ctl'+ id +'_txtvalue').disabled=true;
							}
				
				
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
			    			
								
						if (varimgValue=='Close')
						{
									window.close(); 
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
									return false; 
									CloseWindow(); 
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
									return false; 
									CloseWindow();
									}
									
									
									
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
										
										self.opener.callrefresh(); 
										 							  
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
						
				
				function CloseWindow() 
						{ 
					
						self.opener.callrefresh(); 
						} 
						
    </script>

    <style type="text/css">
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
    </style>

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
<body bgcolor="#f5f5f5" style="margin-top: 0px; margin-left: 0px; margin-right: 0px;
    margin-bottom: 0px">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" bordercolor="activeborder" width="100%" background="../../images/top_nav_back.gif"
        border="0" height="47">
        <tr background="../../images/top_nav_back.gif" height="47">
            <td valign="middle">
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTitleLabelViews" runat="server" BorderStyle="None" BorderWidth="2px"
                    Height="12px" Width="88px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="true"
                    ForeColor="Teal">Views</asp:Label>
            </td>
            <td>
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../../Images/S2Save01.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ToolTip="Ok" ImageUrl="../../Images/s1ok02.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ToolTip="Reset" ImageUrl="../../Images/reset_20.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ToolTip="Delete" ImageUrl="../../Images/s2delete01.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ToolTip="Close" ImageUrl="../../Images/s2close01.gif">
                </asp:ImageButton>
            </td>
        </tr>
    </table>
    <table id="Table2" bordercolor="lightgrey" height="20%" bgcolor="#f5f5f5" border="0"
        style="width: 473px">
        <tr>
            <td style="height: 16px" bordercolor="#f5f5f5" align="left" colspan="4">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Height="16px"
                    Width="32px" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="true">Name</asp:Label>&nbsp;&nbsp;<asp:TextBox
                        ID="txtViewName" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="18px"
                        Width="128px" Font-Size="XX-Small" Font-Names="Verdana" ReadOnly="true" MaxLength="50"
                        CssClass="txtNoFocus"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
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
                                Select your own cloumns</strong></font>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 193px" valign="top" bordercolor="#f5f5f5" align="right">
                            <asp:ListBox ID="lstdefaultcolumn" runat="server" Height="200px" Width="168px" Font-Size="X-Small"
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
                            <asp:ListBox ID="lstusercolumn" runat="server" Height="200px" Width="168px" Font-Size="X-Small"
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
                            <div style="overflow: auto; width: 450px" align="right">
                                <asp:DataGrid ID="GrdView" runat="server" Height="10px" Width="400px" BorderColor="Silver"
                                    CssClass="Grid" CellPadding="0" AutoGenerateColumns="False">
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
                                                    Height="14px" MaxLength="2" Text='<%# databinder.eval(container.dataitem,"UV_NU9_SO") %>'>
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
                                                    Font-Size="xx-small" Height="14px" Text='<%# databinder.eval(container.dataitem,"UV_VC20_Value") %>'>
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
                        <asp:ListBox ID="lstError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"
                            Width="416px" Visible="false"></asp:ListBox>
                        <input id="txtCountHID" type="hidden" name="txtCountHidden" runat="server" />
                        <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="floating_block">
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
