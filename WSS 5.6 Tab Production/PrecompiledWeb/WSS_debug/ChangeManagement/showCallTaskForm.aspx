<%@ page language="VB" autoeventwireup="false" inherits="ChangeManagement_showCallTaskForm, App_Web_jikqy_wr" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Call/Task Form</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../Images/js/core.js" type="text/javascript"></script>

    <script src="../Images/js/events.js" type="text/javascript"></script>

    <script src="../Images/js/css.js" type="text/javascript"></script>

    <script src="../Images/js/coordinates.js" type="text/javascript"></script>

    <script src="../Images/js/drag.js" type="text/javascript"></script>

    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px" onunload="RefreshParent();">
    <form id="Form1" runat="server">

    <script type="text/javascript">
			var globleID;
			var globleUser;
			var globleRole;
			var globleCompany;
			
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
																var cno=document.Form1.txtCallNo.value;
																var tno=document.Form1.txtTaskNo.value;
																var cnt=countInstances(formName,' ');
															
																for(i=0;i<cnt;i++)
																{
																	formName=formName.replace(' ','5z_q');
																}
															
																var opt;
																opt='<%=Request.QueryString("OPT")%>';
																var tempID;
																tempID='<%=Request.QueryString("TemplateID")%>';
																if ( opt==2 )
																{
																		wopen('../AdministrationCenter/Template/TemplateFormEntryDetails.aspx?TemplateID='+ tempID +'&ScrID=260&formName='+formName+'&cno='+cno+'&tno='+tno,formName,915,550);
																}
																else
																{
																		
																		wopen('Form_Entry_Details.aspx?ScrID=260&formName='+formName+'&cno='+cno+'&tno='+tno,formName,915,550);
																}
																
															}
															
												}	
												
												if (varImgValue=='Close')
												{
																 document.Form1.txthiddenImage.value=varImgValue;
																 Form1.submit(); 
																 return false;
												}
								
								
									
				}				
				
		function countInstances(string, word) 
		    {
			    var substrings = string.split(word);
			    return substrings.length - 1;
		    }

				
		function KeyCheck(nn,rowvalues)
			{
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
			    document.Form1.txthidden.value=nn;
			    SaveEdit('Edit')
	        }	
	    
		function callrefresh()					
		    {
			    Form1.submit();
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
					'status=no, toolbar=no, scrollbars=no, resizable=yes');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}
		function RefreshParent()
		    {
			    self.opener.Form1.submit();
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

    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        background="../Images/top_nav_back.gif" border="0">
        <tr>
            <td width="50%">
                &nbsp;
                <asp:Label ID="lblShowcallTaskForm" runat="server" Width="192px" Height="12px" BorderWidth="2px"
                    BorderStyle="None" CssClass="TitleLabel">Show Call/Task Forms</asp:Label>
            </td>
            <td>
                <img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="../Images/s2close01.gif"
                    AccessKey="O" ToolTip="Close"></asp:ImageButton>&nbsp;
                <img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">&nbsp;
            </td>
            <td width="42" background="../Images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table126" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        border="1">
        <tr>
            <td valign="top" colspan="1">
                <!--  **********************************************************************-->
                <table id="Table7" cellspacing="0" cellpadding="2" width="100%" border="0">
                    <tr>
                        <td colspan="2">
                            <cc1:CollapsiblePanel ID="cpnlErrorPanel" runat="server" Height="47px" Width="100%"
                                BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                ExpandImage="../Images/ToggleDown.gif" Text="Error Message" TitleBackColor="Transparent"
                                TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                Visible="False" BorderColor="Indigo">
                                <table id="Table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td colspan="0" rowspan="0">
                                            <asp:Image ID="Image1" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif">
                                            </asp:Image>
                                        </td>
                                        <td colspan="0" rowspan="0">
                                            &nbsp;
                                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </cc1:CollapsiblePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="GrdAddSerach" runat="server" DataKeyField="Form_Name" BackColor="White"
                                AutoGenerateColumns="False" Width="100%" Font-Names="Verdana" CssClass="grid">
                                <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                <ItemStyle CssClass="griditem"></ItemStyle>
                                <HeaderStyle CssClass="GridHeader" BackColor="#E0E0E0"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemStyle Width="20px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="imgObj" ImageUrl="../images/form1.jpg" runat="server" Width="10" Visible="False">
                                            </asp:Image>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Form Name">
                                        <HeaderStyle Font-Bold="True" Width="480px"></HeaderStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Form_Name") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <!-- ***********************************************************************-->
    </TD></TR></TABLE>
    <asp:TextBox ID="txtCallNo" runat="server" Height="0px" Width="0px"></asp:TextBox><asp:TextBox
        ID="txtTaskNo" runat="server" Height="0px" Width="0px"></asp:TextBox>
    <input type="hidden" name="txthidden">
    <input type="hidden" name="txthiddenImage">
    </form>
</body>
</html>
