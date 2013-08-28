<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_UserEmail_serach, App_Web_mwk9lvv9" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>User Select</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script language="javascript" src="../../Images/Js/PopSearchShortCuts.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
		
		//var globalid;
	
				
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

function CheckBoxChange(ID)
{
    
}

 function CheckAll(ID)

{

var tableID='GrdAddSerach'; 

var table;

table=document.getElementById(tableID);

var n=table.rows.length-1;

if(document.getElementById(ID).checked==true)

{

for(var i=0; i<n; i++)

{

if ( i+2 <10 )

{

document.getElementById('GrdAddSerach_ctl0'+(i+2)+'_chkReq').checked=true;

}

else

{

document.getElementById('GrdAddSerach_ctl'+(i+2)+'_chkReq').checked=true; 

}


}

}

else

{

for(var i=0; i<n; i++)

{

if ( i+2 <10 )

{

document.getElementById('GrdAddSerach_ctl0'+(i+2)+'_chkReq').checked=false;

}

else

{

document.getElementById('GrdAddSerach_ctl'+(i+2)+'_chkReq').checked=false; 

}

}

}

}

                        
   
			
				
function CheckStatus()

{

var chkID='GrdAddSerach_ctl01_chkAll';

var chk=0;

var unchk=0;

var tableID='GrdAddSerach' ;

var table;

if (document.all) table=document.all[tableID];

if (document.getElementById) table=document.getElementById(tableID);

var n=table.rows.length-1;


for(var i=0; i<n; i++)

{

var Temp;

if ( i+2 <=9)

{


Temp='GrdAddSerach_ctl0'+(i+2)+'_chkReq'

}

else

{

Temp='GrdAddSerach_ctl'+(i+2)+'_chkReq'

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
					
					//GrdAddSerach__ctl2_chkReq
					
						if (rowvalues < 9)
						{
						if(document.getElementById('GrdAddSerach_ctl0'+(rowvalues+1)+'_chkReq').checked==true)
						{
							document.getElementById('GrdAddSerach_ctl0'+(rowvalues+1)+'_chkReq').checked=false;
						}
						else
						{
							document.getElementById('GrdAddSerach_ctl0'+(rowvalues+1)+'_chkReq').checked=true;
						}
						}
						else
						{
						if(document.getElementById('GrdAddSerach_ctl'+(rowvalues+1)+'_chkReq').checked==true)
						{
							document.getElementById('GrdAddSerach_ctl'+(rowvalues+1)+'_chkReq').checked=false;
						}
						else
						{
							document.getElementById('GrdAddSerach_ctl'+(rowvalues+1)+'_chkReq').checked=true;
						}
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
	
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px">
    <form id="Form1" method="post" runat="server">
    <table id="Table1" bordercolor="activeborder" height="28" cellspacing="0" cellpadding="0"
        width="100%" background="../../images/top_nav_back.gif" border="0">
        <tr>
            <td align="left" width="202" height="16">
                  <asp:Panel ID="ooo" runat="server" DefaultButton="btnSearch" TabIndex="0">
                <asp:Button ID="btnSearch" runat="server" Width="0px" Height="0px" Text="" BackColor="#8AAFE5" 
                    BorderColor="#8AAFE5" BorderStyle="None"></asp:Button><asp:Label ID="lblTitleLabelWssUsers"
                    runat="server" Width="104px" Height="8px" CssClass="TitleLabel">&nbsp;WSS Users</asp:Label>
                    </asp:Panel>
            </td>
            <td align="left" colspan="1" rowspan="1">
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif">
                </asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif">
                </asp:ImageButton>&nbsp;
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table16" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top" align="center">
             <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td nowrap="nowrap" id="pndgsrch" align="left" runat="server">
                                 
                                    </td>
                                </tr>
                            </table>
                <div style="overflow: auto; width: 100%; height: 490px">
                    <asp:DataGrid ID="GrdAddSerach" runat="server" AutoGenerateColumns="False" BorderColor="Silver"
                        BorderStyle="None" BorderWidth="1px" CellPadding="0" CssClass="Grid" DataKeyField="ID"
                        Font-Names="Verdana" ForeColor="MidnightBlue" GridLines="Horizontal" HorizontalAlign="Center">
                        <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                        <SelectedItemStyle CssClass="GridSelectedItem" BackColor="Silver" HorizontalAlign="Left">
                        </SelectedItemStyle>
                        <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke" HorizontalAlign="Left">
                        </AlternatingItemStyle>
                        <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White"
                            HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                            BackColor="#E0E0E0" HorizontalAlign="Left"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="ID" Visible="False"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" Width="30px" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkReq" runat="server" Width="30px" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Name">
                                <ItemStyle Width="178px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'
                                        Width="178px">
                                    </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="178px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="E-mail Address">
                                <ItemStyle Width="220px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblEmailId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EmailID") %>'
                                        Width="220px">
                                    </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="220px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Company">
                                <ItemStyle Width="120px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Company") %>'
                                        Width="120px">
                                    </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="120px" />
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid></div>
            </td>
            <%--<td visible="false">
                <input type="text" visible="false" name="txthidden"/>
            </td>--%>
        </tr>
    </table>
    <input style="left: 0px; position: absolute; top: 547px" type="hidden" name="txthidden">
    <input id="txtHIDCount" type="hidden" name="txtCount" runat="server">
    <asp:Button ID="btsearch" Style="left: 0px; position: absolute; top: 547px" runat="server"
        Text="" Width="0px" Height="0px" BorderWidth="0px"></asp:Button>
    </form>
</body>
</html>
