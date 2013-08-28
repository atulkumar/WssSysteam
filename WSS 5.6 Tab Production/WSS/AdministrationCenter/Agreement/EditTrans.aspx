<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditTrans.aspx.vb" Inherits="AdministrationCenter_Agreement_EditTrans" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script language="javascript" src="../../DateControl/ION.js"></script>

    <link href="../../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script type="text/javascript">
		
		
		function calculatebalance()
	{
var receivecamt=0;	
var exchrate=0;
var totrece=0;
var taxes=0;

receivecamt=	document.getElementById('txtamtreceivednow').value;
exchrate=document.getElementById('txtConvRate').value;
taxes=document.getElementById('txttaxes').value;

if (exchrate==0) 
{

document.getElementById('txtConvRate').value=1;
exchrate=1;
taxes=0;
document.getElementById('txttaxes').value=0;

}


totrece=(receivecamt * exchrate) - (taxes);

document.getElementById('txtActuRecev').value=totrece;
	
	
	}
	
		function NumericHour(ControlID)
		{	
			
			var Val;
			Val = ControlID.value;
			var temp;
			if ( Val.indexOf('.')>=0)
			{
			temp=Val.substr(Val.indexOf('.'));
			//alert(temp);
				if ( temp.length > 2 && (event.keyCode!=13) )
				{	
					event.returnValue = false;
					//alert(temp);
				}
			}
			if (Val.indexOf('.')>0 && event.keyCode==46 )
			{
				event.returnValue = false;
			}
			
			if((event.keyCode<13 || event.keyCode>13) && (event.keyCode<46 || event.keyCode>46) &&( event.keyCode<48 || event.keyCode>57))
				{
					event.returnValue = false;
					alert("Please Enter Numerics Only!");
				}
							
		}		
		
		function SaveEdit(varImgValue)
				{
			    													
												if (varImgValue=='Close')
												{							
													window.close();
																																			 
												}
														
				}		
		
	
	
    </script>

</head>
<body ottommargin="0" bgcolor="#f5f5f5" leftmargin="0" topmargin="0" rightmargin="0"
    ms_positioning="GridLayout" onunload="self.opener.Form1.submit();">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        background="../../images/top_nav_back.gif" border="0">
        <tbody>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblTitleLabelEditInvoice" runat="server" ForeColor="Teal" Font-Bold="True"
                        Font-Names="Verdana" Font-Size="X-Small" Width="144px" Height="12px" BorderWidth="2px"
                        BorderStyle="None"> EDIT INVOICE</asp:Label>
                </td>
                <td style="width: 1px" valign="middle">
                    <td>
                        <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">
                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                            ToolTip="Save"></asp:ImageButton>&nbsp;
                        <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif"
                            ToolTip="Ok"></asp:ImageButton>&nbsp;
                        <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                            ToolTip="Close"></asp:ImageButton>&nbsp;
                        <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
                    </td>
                </td>
                <td valign="top" width="42" background="../../images/top_nav_back01.gif" height="67">
                    &nbsp;
                    <img class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
                        src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">
                </td>
            </tr>
        </tbody>
    </table>
    <table width="450">
        <tr>
            <td>
                <cc1:CollapsiblePanel ID="cpnlErrorPanel" runat="server" Width="6.63%" Height="87px"
                    BorderWidth="0px" BorderStyle="Solid" BorderColor="Indigo" Visible="False" TitleCSS="test"
                    PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
                    Text="Error Message" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                    Draggable="False">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="Image1" runat="server" Height="16px" Width="16px" ImageUrl="../../Images/warning.gif">
                                </asp:Image>
                            </td>
                            <td>
                                <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="408px"
                                    Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
            </td>
        </tr>
    </table>
    <table id="Table1" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="450"
        bgcolor="#f5f5f5" border="1">
        <tr>
            <td bordercolor="#f5f5f5" style="height: 36px">
                &nbsp;&nbsp;
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label3" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Amt Received Date</asp:Label><br>
                <uc1:DateSelector ID="dtreceivedate" runat="server"></uc1:DateSelector>
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Amt Received Now</asp:Label><br>
                <asp:TextBox onkeypress="javascript:NumericHour(this);" ID="txtamtreceivednow" runat="server"
                    Font-Names="Verdana" Font-Size="XX-Small" Width="152px" BorderWidth="1px" CssClass="txtNoFocus"
                    MaxLength="9"></asp:TextBox>
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
            </td>
        </tr>
        <tr>
            <td style="height: 21px" bordercolor="#f5f5f5">
                &nbsp;&nbsp;
            </td>
            <td style="height: 21px" bordercolor="#f5f5f5">
                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Bank Name</asp:Label><br>
                <asp:TextBox ID="txtBankName" runat="server" Width="160px" Height="16px" BorderWidth="1px"
                    CssClass="txtNoFocus" MaxLength="50"></asp:TextBox>
            </td>
            <td style="height: 21px" bordercolor="#f5f5f5">
            </td>
            <td style="height: 21px" bordercolor="#f5f5f5">
                <asp:Label ID="lblError" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Exchange Rate</asp:Label><br>
                <asp:TextBox onkeypress="javascript:NumericHour(this);" ID="txtConvRate" runat="server"
                    Font-Names="Verdana" Font-Size="XX-Small" Width="152px" BorderWidth="1px" CssClass="txtNoFocus"
                    MaxLength="9"></asp:TextBox>
            </td>
            <td style="height: 21px" bordercolor="#f5f5f5">
            </td>
        </tr>
        <tr>
            <td style="height: 36px" bordercolor="#f5f5f5">
                &nbsp;&nbsp;
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small"> Draft No</asp:Label><br>
                <asp:TextBox ID="txtDraftNo" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                    Width="160px" BorderWidth="1px" CssClass="txtNoFocus" MaxLength="40"></asp:TextBox>
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Bank Charges</asp:Label><br>
                <asp:TextBox onkeypress="javascript:NumericHour(this);" ID="txttaxes" runat="server"
                    Font-Names="Verdana" Font-Size="XX-Small" Width="152px" BorderWidth="1px" CssClass="txtNoFocus"
                    MaxLength="9"></asp:TextBox>
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
            </td>
        </tr>
        <tr>
            <td bordercolor="#f5f5f5">
                &nbsp;&nbsp;
            </td>
            <td bordercolor="#f5f5f5">
                <br>
            </td>
            <td bordercolor="#f5f5f5">
            </td>
            <td bordercolor="#f5f5f5">
                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Actual Received</asp:Label><br>
                <asp:TextBox ID="txtActuRecev" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                    Width="152px" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="9"></asp:TextBox>
            </td>
            <td bordercolor="#f5f5f5">
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
