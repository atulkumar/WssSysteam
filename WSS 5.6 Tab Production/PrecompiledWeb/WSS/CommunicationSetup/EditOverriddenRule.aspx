<%@ page language="VB" autoeventwireup="false" inherits="CommunicationSetup_EditOverriddenRule, App_Web_p-jkrh6b" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Edit Communication Setup</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../DateControl/ION.js"></script>

    <script type="text/javascript" src="../images/Js/JSValidation.js"></script>

    <link href="../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../SupportCenter/calendar/popcalendar.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: #e0e0e0;
        }
    </style>

    <script type="text/javascript">
		

var rand_no = Math.ceil(500*Math.random())

var aa=2;
var nn=1;
	function addSelectedItemsToParent()
	 {
		 self.opener.addToParentList(aa);
		 window.close();
     }
     
     function OpenCompany(c)
				{
				    	wopen('../../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as Name,CI_IN4_Business_Relation as Type from T010011 where CI_VC8_Address_Book_Type='+"'COM'" + '  &tbname=' + c ,'Search'+rand_no,500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'Search',500,450);					
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
					'status=no, toolbar=no, scrollbars=no, resizable=no');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}
			
		function SelectDDL(UDC,ctrl,Type)
		{
				var n;
				n=document.getElementById(ctrl).options.selectedIndex;
				if ( document.getElementById(ctrl).options(n).text=='More' )
				{
					document.getElementById(ctrl).options(0).text=""
					document.getElementById(ctrl).options.selectedIndex=0;
					document.getElementById(ctrl.substr(0,ctrl.length-3)).value=""
					OpenCompany(ctrl);
				}
				else
				{
					document.getElementById(ctrl).options(0).text=""
					document.getElementById(ctrl.substr(0,ctrl.length-3)).value=document.getElementById(ctrl).options(n).value;			
				}
		}
				
		function addToParentList(Afilename,TbName,strName)
		{
					if (Afilename != "" || Afilename != 'undefined')
					{
					   if ( TbName.substr(TbName.length-3,TbName.length)=='DDL')
					   {
					   			document.getElementById(TbName).options(0).text=strName;
					   			document.getElementById(TbName).options(0).
								document.getElementById(TbName).options.selectedIndex=0;
								document.getElementById(TbName.substr(0,TbName.length-3)).value=Afilename;
					   }
					   else
					   {
						document.getElementById(TbName).value=Afilename;
						}
						aa=Afilename;
					}
					else
					{
						document.Form1.txtAB_Type.value=aa;
					}
		}	
					
	function ChangeColorOver(obj,clrO,clrC)
		{
			if(obj.style.backgroundColor!=clrC)
			{
				obj.style.backgroundColor=clrO;
			}
		}

	function ChangeColorOut(obj,clrO,clrC)
		{
			if(obj.style.backgroundColor!=clrC)
				{
					obj.style.backgroundColor=clrO;          
				}
		}
 
	function ChangeColorOut1(obj,clrO,clrC)
		{	
			document.getElementsByName("datagrid1").item(obj) .style.backgroundColor=clrC;  
		}

	function ChangeColorClick(obj, clrO, clrC)
		{
			obj.style.backgroundColor = clrC;
		}


	function HideContents()
		{
			parent.document.all("SideMenu1").cols="0,*";
		}
		
	function ShowContents()
		{
			parent.document.all("SideMenu1").cols="18%,*";
		}
		
	function Reload()
		{
			Form1.submit();
		}
	
	function CloseWindow()
		{
			self.opener.callrefresh();
		}
		
		function SaveEdit(varImgValue)
		{
			if (varImgValue=='Close')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
								return false;
						}								
							
			if (varImgValue=='Ok')
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
						
		if (varImgValue=='Reset')
						{
							var confirmed
							confirmed=window.confirm("Do You Want To reset The Page ?");
							if(confirmed==true)
								{	
									Form1.reset()
								}		
						}							
		}			
		
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5"
    onunload="CloseWindow();">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" style="border-color: activeborder" cellspacing="0" cellpadding="0"
        width="100%" background="../Images/top_nav_back.gif" border="0">
        <tr>
            <td style="width: 15%">
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTitleLabelEditOR" runat="server" Width="176px" Height="12px" BorderWidth="2px"
                    BorderStyle="None" CssClass="TitleLabel">EDIT OVERRIDDEN RULE</asp:Label>
            </td>
            <td style="width: 85%; text-align: center;" nowrap="nowrap">
                <center>
                    <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../Images/S2Save01.gif">
                    </asp:ImageButton>
                    <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ToolTip="Ok" ImageUrl="../Images/s1ok02.gif">
                    </asp:ImageButton>
                    <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ToolTip="Reset" ImageUrl="../Images/reset_20.gif">
                    </asp:ImageButton>
                    <asp:ImageButton ID="imgClose" AccessKey="O" runat="server" ToolTip="Close" ImageUrl="../Images/s2close01.gif">
                    </asp:ImageButton>
                </center>
            </td>
            <!--<td style="WIDTH: 1px">-->
            <td width="42" background="../Images/top_nav_back.gif" height="47">
            </td>
        </tr>
    </table>
    <table id="Table126" style="border-color: activeborder; height=10%" cellspacing="0"
        cellpadding="0" width="100%" border="1">
        <tr>
            <td valign="top" colspan="1">
                <!--  **********************************************************************-->
                <table id="Table7" cellspacing="0" cellpadding="2" width="100%" border="0">
                    <tr>
                        <td width="127" bgcolor="#f0f0f0" colspan="2">
                            <b><font face="Verdana" size="1">&nbsp;</font></b>
                        </td>
                    </tr>
                </table>
                <table id="Table32" cellspacing="0" cellpadding="0" width="62%" border="0">
                    <tr>
                        <td valign="top" align="left" width="63%">
                            <table style="width: 584px; height: 176px" width="584" bgcolor="#f5f5f5" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCallNo" runat="server" Width="100px" Height="12px" CssClass="FieldLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTaskNo" runat="server" Width="110px" Height="12px" CssClass="FieldLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 137px; height: 10px">
                                        <asp:Label ID="Label4" runat="server" Width="96px" CssClass="FieldLabel">Event Name</asp:Label><asp:DropDownList
                                            ID="ddEventName" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129px"
                                            Height="22px" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="height: 10px">
                                        <asp:Label ID="Label7" runat="server" Width="136px" Height="12px" CssClass="FieldLabel">On Event</asp:Label><asp:DropDownList
                                            ID="ddEventFired" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129px"
                                            Height="22px" AutoPostBack="True">
                                            <asp:ListItem Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1">ON</asp:ListItem>
                                            <asp:ListItem Value="2">OFF</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="height: 10px">
                                        <asp:Label ID="Label3" runat="server" Width="72px" Height="16px" CssClass="FieldLabel">User Name</asp:Label><br>
                                        <asp:DropDownList ID="ddUserName" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                            Width="129" Height="22">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="height: 10px">
                                        <asp:Label ID="Label2" runat="server" Width="72px" Height="12px" CssClass="FieldLabel">Role Name</asp:Label><asp:DropDownList
                                            ID="ddRoleName" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129"
                                            Height="22">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 137px">
                                        <asp:Label ID="lblMiddleName2" runat="server" Width="72px" CssClass="FieldLabel">Company</asp:Label><asp:DropDownList
                                            ID="ddCompany" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129px"
                                            Height="22px" Enabled="False" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Width="72px" Height="12px" CssClass="FieldLabel">Call Type</asp:Label><asp:DropDownList
                                            ID="ddCallType" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129px"
                                            Height="22px" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Width="72px" CssClass="FieldLabel">Call Status</asp:Label><asp:DropDownList
                                            ID="ddCallStatus" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129"
                                            Height="22" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Width="64px" CssClass="FieldLabel">Task type</asp:Label><asp:DropDownList
                                            ID="ddTaskType" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129px"
                                            Height="22px" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Width="68px" CssClass="FieldLabel">Task Status</asp:Label><asp:DropDownList
                                            ID="ddTaskStatus" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129"
                                            Height="22px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Width="72px" Height="12px" CssClass="FieldLabel">SubCategory</asp:Label><asp:DropDownList
                                            ID="ddProject" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129">
                                            <asp:ListItem Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsMail" runat="server" Width="80px" Text="Mail" CssClass="FieldLabel">
                                        </asp:CheckBox><asp:CheckBox ID="chkIsSMS" runat="server" Width="41px" Text="SMS"
                                            CssClass="FieldLabel"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Width="112px" Height="12px" CssClass="FieldLabel">Record Status</asp:Label><asp:DropDownList
                                            ID="ddRecordStatus" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                                            Width="129" Height="22">
                                            <asp:ListItem Value="1">Enable</asp:ListItem>
                                            <asp:ListItem Value="0">Disable</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Height="12px" CssClass="FieldLabel">Start Date</asp:Label>
                                        <ION:Customcalendar ID="dtStartDate" runat="server" Width="120px" Height="15px" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Height="12px" CssClass="FieldLabel">Stop Date</asp:Label>
                                        <ION:Customcalendar ID="dtEndDate" runat="server" Width="120px" Height="15px" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMiddleName8" runat="server" Width="72px" Height="12px" Visible="False"
                                            CssClass="FieldLabel">Default User</asp:Label><asp:DropDownList ID="ddEventUser"
                                                runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="129px" Height="22px"
                                                Visible="False">
                                            </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <!-- End Body***********************************************************************-->
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="100px"
                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
