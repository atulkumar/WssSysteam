<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_GetCallsPopup, App_Web_i-czgkd-" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>GetCallsPopup</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../../Images/Js/StyleSheet1.css">

    <script language="javascript" type="text/javascript">
		function OK()
		{
			var CallNo;
			CallNo=document.Form1.txtHIDCallNo.value;
			if ( document.Form1.txtHIDCallNo.value=='' )
			{
				alert('Please select Call No');
			}
			self.opener.AddCallNo(CallNo);
			if ( document.Form1.txtHIDCallNo.value!='' )
			{
			        window.close();
			}
			return false;
		}		
		function DClick(CallNo)
		{
			document.Form1.txtHIDCallNo.value=CallNo;
			self.opener.AddCallNo(CallNo);
			window.close();
		}
		function SClick(CallNo, RI)
		{
			document.Form1.txtHIDCallNo.value=CallNo;
			
			var tableID='cpnlCalls_grdCalls';
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
					table.rows [ RI  ] . style . backgroundColor = "#d4d4d4";
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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;" background="#8AAFE5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" bordercolor="activeborder" height="28" cellspacing="0" cellpadding="0"
        background="../../images/top_nav_back.gif" width="100%" border="0">
        <tr>
            <td style="width: 15%">
                <asp:Button ID="btnSearch" runat="server" Width="0px" BackColor="#8AAFE5" BorderColor="#8AAFE5"
                    BorderStyle="None"></asp:Button>
                <asp:Label ID="Label1" runat="server" CssClass="TitleLabel" BorderWidth="2px" BorderStyle="None">&nbsp;CALL SEARCH</asp:Label>
            </td>
            <td style="width: 85%; text-align: center;" nowrap="nowrap">
                <center>
                    <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif">
                    </asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif">
                    </asp:ImageButton>
                </center>
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <cc1:CollapsiblePanel ID="cpnlCalls" runat="server" Width="100%" BorderWidth="0px"
        BorderStyle="Solid" Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
        TitleClickable="True" TitleBackColor="Transparent" Text="Calls" ExpandImage="../../Images/ToggleDown.gif"
        CollapseImage="../../Images/ToggleUp.gif" Draggable="False" Height="30px" BorderColor="Indigo">
        <div style="overflow: auto; width: 100%; height: 435px">
            <table id="Table32" bordercolor="lightgrey" cellspacing="0" cellpadding="0" align="left"
                border="0">
                <tr>
                    <td>
                        <asp:DataGrid ID="grdCalls" runat="server" AutoGenerateColumns="False" CssClass="Grid">
                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                            <ItemStyle CssClass="GridItem"></ItemStyle>
                            <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <asp:TextBox CssClass="SearchTxtBox" ID="txtCallNo" runat="server" Width="60px"></asp:TextBox><br>
                                        Call No
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCallNo" Width="60px" runat="server" Text='<%#Container.DataItem("CallNo")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <asp:TextBox CssClass="SearchTxtBox" ID="txtCallSubject" runat="server" Width="240px"></asp:TextBox><br>
                                        Call Subject
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubject" Width="240px" runat="server" Text='<%#Container.DataItem("Subject")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <asp:TextBox CssClass="SearchTxtBox" ID="txtCallType" runat="server" Width="80px"></asp:TextBox><br>
                                        Call Type
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCallType" Width="80px" runat="server" Text='<%#Container.DataItem("CallType")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <asp:TextBox CssClass="SearchTxtBox" ID="txtCompany" runat="server" Width="100px"></asp:TextBox><br>
                                        Company
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompany" Width="100px" runat="server" Text='<%#Container.DataItem("Company")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <asp:TextBox CssClass="SearchTxtBox" ID="txtProject" runat="server" Width="100px"></asp:TextBox><br>
                                        SubCategory
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" Width="100px" runat="server" Text='<%#Container.DataItem("Project")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <asp:TextBox CssClass="SearchTxtBox" ID="txtRequestedBy" runat="server" Width="100px"></asp:TextBox><br>
                                        Requestedby
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" Width="100px" runat="server" Text='<%#Container.DataItem("RequestedBy")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </div>
    </cc1:CollapsiblePanel>
    <input type="hidden" name="txtHIDCallNo">
    <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
    </form>
</body>
</html>
