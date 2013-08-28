<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_AddressBook_AB_Skill, App_Web_p6k4cy-b" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add/Edit Skills</title>
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

    <script language="Javascript" type="text/javascript">
		
		
		function OpenW(a,b,c)
				{
				
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
					wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
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

		function addToParentList(Afilename,TbName)
				{
				
					if (Afilename != "" || Afilename != 'undefined')
					{
					//alert(Afilename);
						document.getElementById(TbName).value=Afilename;
						aa=Afilename;
					}
					else
					
					{
						//document.Form1.txtAB_Type.value=aa;
					}
				}
						
		function CloseWindow()
				{
					self.opener.callrefresh();
				}				
				
				function SaveEdit(varImgValue)
				{
			    			if (varImgValue=='Edit')
												{
															if (globalid==null)
															{
																alert("Please select the row");
															}
															else
															{
																			document.form1.txthiddenImage.value=varImgValue;
																			document.form1.txthiddenSkil.value=globalSkil;
																			document.form1.txthidden.value=globalAddNo;	
																			document.form1.txthiddenGrid.value=globalGrid;	
																			form1.submit(); 
																			return false;
															}
															
												}	
												
						if (varImgValue=='Close')
						{
									window.close(); 
						}
								
								
																
						if (varImgValue=='Ok')
						{
						//Security Block
							var obj=document.getElementById("imgOk")
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

									document.form1.txthiddenImage.value=varImgValue;
									form1.submit(); 
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

									document.form1.txthiddenImage.value=varImgValue;
									form1.submit(); 
									return false;
						}				
				}		
				
						
    </script>

</head>
<body onunload="CloseWindow();" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
        border="0">
        <tr valign="bottom">
            <td style="width: 122px" valign="middle" nowrap align="center">
                <asp:Label ID="lblTitleLabelSkillSet" runat="server" ForeColor="Teal" Font-Bold="true"
                    Font-Names="Verdana" Font-Size="X-Small" Width="160px" BorderWidth="2px" BorderStyle="None">Add/Edit Skill Set Info</asp:Label>
            </td>
            <td align="left" valign="middle">
                &nbsp;&nbsp;&nbsp;&nbsp;<img title="Seperator" alt="R" src="../../Images/00Seperator.gif"
                    border="0">&nbsp;<asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save"
                        ImageUrl="../../Images/S2Save01.gif" Visible="False"></asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton
                            ID="imgOk" AccessKey="K" runat="server" ToolTip="OK" ImageUrl="../../Images/s1ok02.gif">
                        </asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton ID="imgReset" AccessKey="R" runat="server"
                            ToolTip="Reset" ImageUrl="../../Images/reset_20.gif"></asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton
                                ID="imgClose" AccessKey="L" runat="server" ToolTip="Close" ImageUrl="../../Images/s2close01.gif">
                            </asp:ImageButton>&nbsp;&nbsp;<img title="Seperator" alt="R" src="../../Images/00Seperator.gif"
                                border="0">&nbsp;
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <cc1:CollapsiblePanel ID="cpnlError" runat="server" Width="100%" BorderWidth="0px"
        BorderStyle="Solid" Visible="False" TitleCSS="test" PanelCSS="panel" TitleForeColor="black"
        TitleClickable="true" TitleBackColor="transparent" Text="Error Message" ExpandImage="../../Images/ToggleDown.gif"
        CollapseImage="../../Images/ToggleUp.gif" Draggable="False" Height="81px" BorderColor="Indigo">
        <table id="table2" bordercolor="lightgrey" cellspacing="0" cellpadding="0" width="272"
            border="0">
            <tr>
                <td colspan="0" rowspan="0">
                    <asp:Image ID="Image1" runat="server" Width="16px" ImageUrl="..\..\icons\warning.gif"
                        Height="16px"></asp:Image>
                </td>
                <td colspan="0" rowspan="0">
                    &nbsp;
                    <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="352px"
                        Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                </td>
            </tr>
        </table>
    </cc1:CollapsiblePanel>
    <table id="table4" style="width: 392px; height: 96px" bordercolor="#f5f5f5" height="96"
        cellspacing="0" cellpadding="0" width="392" bgcolor="#f5f5f5" border="1">
        <tr>
            <td style="width: 121px" valign="middle" bordercolor="#ffffff" align="left" width="100%">
            </td>
            <tr>
                <td style="width: 341px; height: 39px" valign="top" bordercolor="#f5f5f5" align="left"
                    width="341" colspan="3" rowspan="1">
                    <font face="Verdana" size="1">&nbsp;
                        <asp:Label ID="lblAddLine1" runat="server" ForeColor="DimGray" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="XX-Small" Width="96px" Height="14px">Skill Type</asp:Label></font><br>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtSkillType" Width="160px" BorderWidth="1px" BorderStyle="Solid"
                        Height="18px" CssClass="txtNoFocus" runat="server" MaxLength="8"></asp:TextBox><img
                            class="PlusImageCSS" onclick="OpenW(0,'SKTY','txtSkillType');" alt="Skill Type"
                            src="../../Images/plus.gif" border="0">
                </td>
            </tr>
            <tr>
                <td style="width: 341px" valign="top" bordercolor="#ffffff" align="left" width="341"
                    colspan="3">
                    <font face="Verdana" size="1">&nbsp;
                        <asp:Label ID="lblAddLine2" runat="server" ForeColor="DimGray" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="XX-Small" Width="96px" Height="14px"> Skill</asp:Label><br>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtSkill" Width="160px" BorderWidth="1px" BorderStyle="Solid" Height="18px"
                            CssClass="txtNoFocus" runat="server" MaxLength="8"></asp:TextBox><img class="PlusImageCSS"
                                onclick="OpenW(0,'SKL','txtSkill');" alt="Skill" src="../../Images/plus.gif"
                                border="0"></font>
                </td>
            </tr>
            <tr>
                <td style="width: 341px" valign="top" bordercolor="#f5f5f5" align="left" width="341"
                    colspan="3">
                    <font face="Verdana" size="1">&nbsp;
                        <asp:Label ID="lblAddLine3" runat="server" ForeColor="DimGray" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="XX-Small" Width="96px" Height="14px"> Comments</asp:Label></font><br>
                    &nbsp;
                    <asp:TextBox ID="txtSkillComment" Width="368px" BorderWidth="1px" BorderStyle="Solid"
                        Height="184px" CssClass="txtNoFocus" runat="server" MaxLength="156"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 5px" valign="top" bordercolor="#f5f5f5" align="left" width="341"
                    colspan="3">
                </td>
            </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <input type="hidden" name="txthiddenImage"><!-- Image Clicked-->
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
