<%@ page language="VB" autoeventwireup="false" inherits="MessageCenter_UploadFiles_UploadAttachment, App_Web_ojvgmzxq" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Attachment</title>
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
		    var	globleID;
			function FileUpload() 
				{ 
					var str; 
					str=document.getElementById('Upload').value; 
					
					if(str=="") 
						{ 
							alert("Please select a File"); 
							return false; 
						} 
					else 
						{ 
							return true; 
						} 
				}    
			
			
			function SaveEdit(varImgValue,CallNo)
				{			    			
						if (varImgValue=='Close')
						{
							window.close(); 
							self.opener.RefreshAttachment();
						}

						if (varImgValue=='Save')
						{
							var tableID='grdAttachments'  
							var table;
							if (document.all) table=document.all[tableID];
							var N=table.rows.length;		
							if(N<2)
							{
								alert("Please upload an attachment to Save");
							}
							else
							{							
								if (CallNo==0)
								{
									window.close();
								}
								else
								{
									document.Form1.txthiddenImage.value=varImgValue;
									document.Form1.submit(); 
								}
							}
						}		
						if (varImgValue=='Delete')
						{
							if(globleID==null)
							{
							alert("Please select a row");
							}
							else
							{
								var res;
								res=window.confirm("Do You Want To Delete the selected row ?");
								if(res==true)
								{
									document.Form1.txthiddenImage.value=varImgValue;
									document.Form1.txthiddenDelVal.value=globleID;
								}
								else
								{
										return false;			
								}
							 }
					      }																
					}			
																								
				function KeyCheck(nn,rr)
					{
				//alert(nn);
						globleID = nn;
					
												
						document.Form1.txthidden.value=nn;
						
						//Form1.submit();
						
										var tableID='grdAttachments'  //your datagrids id
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
												    table.rows [ rr  ] . style . backgroundColor = "#d4d4d4";
												}
				
					}	
    </script>

    <!--Added By Atul to make parent window Active after popup win close-->

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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table id="Table11" cellspacing="0" cellpadding="0" width="100%" border="0" background="../../images/top_nav_back.gif">
        <tr>
            <td valign="middle" nowrap align="left">
                <asp:Label ID="lblTitleLabelAttachment" runat="server" BorderStyle="None" BorderWidth="2px"
                    CssClass="TitleLabel">&nbsp;&nbsp;Attachment</asp:Label>
            </td>
            <td align="left">
             
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../../Images/s1ok02.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                    ToolTip="Delete"></asp:ImageButton>
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                    ToolTip="Close"></asp:ImageButton>
              
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table1" bordercolor="lightgrey" height="86%" cellspacing="0" cellpadding="0"
        width="100%" bgcolor="#f5f5f5" border="2">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblName0" runat="server" CssClass="FieldLabel">&nbsp;&nbsp;Call#</asp:Label>
                            <asp:TextBox ID="txtCallNo" runat="server" CssClass="txtNoFocus" BorderWidth="1px"
                                BorderStyle="Solid" Height="18px" Width="80px" Font-Size="XX-Small" Font-Names="Verdana"
                                MaxLength="18" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">&nbsp;&nbsp;Task#</asp:Label>
                            <asp:TextBox ID="txtTaskNo" runat="server" CssClass="txtNoFocus" BorderWidth="1px"
                                BorderStyle="Solid" Height="18px" Width="80px" Font-Size="XX-Small" Font-Names="Verdana"
                                MaxLength="18" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">&nbsp;&nbsp;Action#</asp:Label>
                            <asp:TextBox ID="txtActionNo" runat="server" CssClass="txtNoFocus" BorderWidth="1px"
                                BorderStyle="Solid" Height="18px" Width="80px" Font-Size="XX-Small" Font-Names="Verdana"
                                MaxLength="18" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" bordercolor="#f5f5f5" style="height: 6px">
                <input id="Upload" type="file" runat="server">
                <asp:Button ID="btnUpload" runat="server" Width="80px" Text="Upload" Height="22px">
                </asp:Button><br>
                <asp:Label ID="Label1" runat="server" Width="352px" Font-Size="10px" Font-Names="Verdana"
                    Font-Bold="True">Maximum size for attachment is 7MB</asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" bordercolor="#f5f5f5">
                <div style="overflow: auto; width: 100%; height: 230px">
                    <asp:DataGrid ID="grdAttachments" runat="server" BorderStyle="None" BorderWidth="1px"
                        Width="400px" Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver"
                        Height="16px" DataKeyField="AT_NU9_File_ID_PK" HorizontalAlign="left" CellPadding="1"
                        AutoGenerateColumns="False" PagerStyle-Visible="False" BackColor="#E0E0E0">
                        <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                        <SelectedItemStyle ForeColor="Black" BackColor="LightGray"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="WhiteSmoke"></AlternatingItemStyle>
                        <ItemStyle Font-Size="8pt" ForeColor="Black" BackColor="White"></ItemStyle>
                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                            CssClass="GridFixedHeader" BackColor="#E0E0E0"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="AT_NU9_File_ID_PK" HeaderText="ID" Visible="False">
                                <HeaderStyle Width="60pt"></HeaderStyle>
                                <ItemStyle Width="60pt"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="AT_VC255_File_Name" HeaderText="File Name">
                                <HeaderStyle Width="250pt"></HeaderStyle>
                                <ItemStyle Width="250pt"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="AT_IN4_Level" HeaderText="Attachment Level">
                                <HeaderStyle Width="150pt"></HeaderStyle>
                                <ItemStyle Width="150pt"></ItemStyle>
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid></div>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
            <input type="hidden" name="txthiddenImage"><!-- Image Clicked-->
            <asp:TextBox ID="txtPath" runat="server" Visible="False"></asp:TextBox><input type="hidden"
                name="txthidden">
            <input type="hidden" name="txthiddenDelVal">
            <asp:TextBox ID="txtid" Style="z-index: 101; left: 632px; position: absolute; top: 512px"
                runat="server" Visible="False"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
