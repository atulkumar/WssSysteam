<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Edit_CommunicationSetup.aspx.vb"
    Inherits="CommunicationSetup_Edit_CommunicationSetup" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Edit Communication Setup</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <%-- <script  src="../Images/Js/core.js" type="text/javascript"></script>

    <script  src="../Images/Js/events.js" type="text/javascript"></script>

    <script  src="../Images/Js/css.js" type="text/javascript"></script>

    <script  src="../Images/Js/coordinates.js" type="text/javascript"></script>

    <script  src="../Images/Js/drag.js" type="text/javascript"></script>--%>

    <script src="../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../DateControl/ION.js" type="text/javascript"></script>

    <script type="text/javascript">
		
     var rand_no = Math.ceil(500*Math.random())
function formReset()		
	{
		var confirmed
		confirmed=window.confirm("Do You Want To reset The Page ?");
		if(confirmed==true)
			{	
				Form1.reset()
			}	
		return false;	
	}			


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
		
		 //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCommSetupEdit_collapsible').cells[0].colSpan = "1";
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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5"
    onunload="CloseWindow();">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" style="border-color: activeborder" cellspacing="0" cellpadding="0"
        width="100%" background="../Images/top_nav_back.gif" border="0">
        <tr>
            <td style="width: 247px">
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTitleLabelEditCS" runat="server" BorderStyle="None" BorderWidth="2px"
                    Height="12px" Width="200px" CssClass="TitleLabel">Edit Communication Setup</asp:Label>
            </td>
            <td>
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../Images/S2Save01.gif"
                    ToolTip="Save"></asp:ImageButton>
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../Images/s1ok02.gif"
                    ToolTip="Ok"></asp:ImageButton>
                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../Images/reset_20.gif"
                    ToolTip="Reset"></asp:ImageButton>
                <asp:ImageButton ID="imgClose" AccessKey="O" runat="server" ImageUrl="../Images/s2close01.gif"
                    ToolTip="Close"></asp:ImageButton>
            </td>
            <td width="42" background="../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="Table126" style="border-color: activeborder; height: 100%" cellspacing="0"
        cellpadding="0" width="100%" border="1">
        <tr>
            <td valign="top" colspan="1">
                <!--  **********************************************************************-->
                <cc1:CollapsiblePanel ID="cpnlCommSetupEdit" runat="server" Width="100%" BorderWidth="0px"
                    BorderStyle="Solid" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True"
                    TitleBackColor="Transparent" Text="Comm Setup Edit" ExpandImage="../Images/ToggleDown.gif"
                    CollapseImage="../Images/ToggleUp.gif" Draggable="False" Visible="true" BorderColor="Indigo">
                    <table id="Table32" cellspacing="0" cellpadding="0" align="center" border="0">
                        <tr>
                            <td>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblRuleNo" runat="server" CssClass="TitleLabel" Width="200px" Height="12px"
                                    BorderWidth="2px" BorderStyle="None">Rule No.1</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="63%">
                                <table width="520" bgcolor="#f5f5f5" border="0">
                                    <tr valign="middle">
                                        <td valign="middle">
                                            <asp:CheckBox ID="chkIsMail" runat="server" CssClass="FieldLabel" Width="40px" Text="Mail">
                                            </asp:CheckBox>
                                            <asp:CheckBox ID="chkIsSMS" runat="server" CssClass="FieldLabel" Width="41px" Text="SMS">
                                            </asp:CheckBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" CssClass="FieldLabel" Height="12px">Start Date</asp:Label>
                                            <ION:Customcalendar ID="dtStartDate" runat="server" EnableViewState="False" Height="16px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" CssClass="FieldLabel" Height="12px">Stop Date</asp:Label>
                                            <ION:Customcalendar ID="dtEndDate" runat="server" Height="16px" />
                                        </td>
                                        <td>
                                            &nbsp;
                                            <asp:Label ID="Label4" runat="server" CssClass="FieldLabel" Width="96px">Event Name</asp:Label>
                                            <asp:DropDownList ID="ddEventName" runat="server" Width="129px" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:Label ID="lblMiddleName8" runat="server" CssClass="FieldLabel" Width="72px"
                                                Height="12px">Default User</asp:Label>
                                            <asp:DropDownList ID="ddEventUser" runat="server" Width="129px" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" CssClass="FieldLabel" Width="136px" Height="12px">On Event</asp:Label>
                                            <asp:DropDownList ID="ddEventFired" runat="server" Width="129px" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                                <asp:ListItem Value="0" Selected="True">Optional</asp:ListItem>
                                                <asp:ListItem Value="1">ON</asp:ListItem>
                                                <asp:ListItem Value="2">OFF</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMiddleName2" runat="server" CssClass="FieldLabel" Width="72px">Company</asp:Label>
                                            <asp:DropDownList ID="ddCompany" runat="server" Width="129px" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" CssClass="FieldLabel" Width="72px" Height="16px">User Name</asp:Label><br>
                                            <asp:DropDownList ID="ddUserName" runat="server" Width="129" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td style="height: 8px">
                                            <asp:Label ID="Label2" runat="server" CssClass="FieldLabel" Width="72px" Height="12px">Role Name</asp:Label>
                                            <asp:DropDownList ID="ddRoleName" runat="server" Width="129" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 8px">
                                            <asp:Label ID="Label9" runat="server" CssClass="FieldLabel" Width="42px" Height="12px">Priority</asp:Label>
                                            <asp:DropDownList ID="ddPriority" runat="server" Width="129" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 8px">
                                            <asp:Label ID="Label6" runat="server" CssClass="FieldLabel" Width="72px" Height="12px">Call Type</asp:Label>
                                            <asp:DropDownList ID="ddCallType" runat="server" Width="129px" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 8px">
                                            <asp:Label ID="Label10" runat="server" CssClass="FieldLabel" Width="72px">Call Status</asp:Label>
                                            <asp:DropDownList ID="ddCallStatus" runat="server" Width="129" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" CssClass="FieldLabel" Width="96px">Task type</asp:Label>
                                            <asp:DropDownList ID="ddTaskType" runat="server" Width="129px" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" CssClass="FieldLabel" Width="68px" Height="14px">Task Status</asp:Label>
                                            <asp:DropDownList ID="ddTaskStatus" runat="server" Width="129" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" CssClass="FieldLabel" Width="72px" Height="12px">SubCategory</asp:Label>
                                            <asp:DropDownList ID="ddProject" runat="server" Width="129" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                                <asp:ListItem Value="0" Selected="True">Optional</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" CssClass="FieldLabel" Width="112px" Height="12px">Record Status</asp:Label>
                                            <asp:DropDownList ID="ddRecordStatus" runat="server" Width="129" Height="20px" Font-Size="XX-Small"
                                                Font-Names="Verdana">
                                                <asp:ListItem Value="1">Enable</asp:ListItem>
                                                <asp:ListItem Value="0">Disable</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlMsg" runat="server">
                                        </asp:Panel>
                                        <asp:ListBox ID="lstError" runat="server" Width="100px" BorderWidth="0" BorderStyle="Groove"
                                            Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red" Visible="false"></asp:ListBox>
                                        <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
                <!-- End Body***********************************************************************-->
            </td>
        </tr>
    </table>
    <!-- ***********************************************************************-->
    </form>
</body>
</html>
