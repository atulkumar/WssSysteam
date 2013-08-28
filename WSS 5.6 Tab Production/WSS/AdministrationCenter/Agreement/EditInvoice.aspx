<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditInvoice.aspx.vb" Inherits="AdministrationCenter_Agreement_EditInvoice" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enter Invoice</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script language="javascript" src="../../DateControl/ION.js" type="text/javascript"></script>

    <link href="../../Images/Js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
	
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
	
	
	function KeyCheck55(InvoiceID)
					
					{
							//alert("asdf");
							
							document.Form1.txthiddenInvoiceID.value=InvoiceID;
										
							SaveEdit('Edit');
					}	
					
		function OpenVW(varTable)
				{
				//alert(varTable);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=no');
				wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrID=463&TBLName='+varTable,'Search',450,500);
				//	wopen('AB_ViewColumns.aspx','UserView',500,450);
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
					'status=no, toolbar=no, scrollbars=yes, resizable=no');
				// Just in case width and height are ignored
				win.resizeTo(w, h);
				// Just in case left and top are ignored
				win.moveTo(wleft, wtop);
				win.focus();
			}
		
		
		function KeyCheck(nn,rowvalues,comp)
					{
						//alert(rowvalues);
						globleID = nn;
						document.Form1.txthidden.value=nn;
						document.Form1.txthiddenCompany.value=comp;
			
						//Form1.submit();
						
										var tableID='grdInvoiceDetail'  //your datagrids id
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

		
		
		
		function SaveEdit(varImgValue)
				{
			    			if (varImgValue=='Edit')
												{
											
													//Security Block
						
							//End of Security Block
							
															if  (document.Form1.txthiddenInvoiceID.value=="")
															{
																 alert("Please select the row");
															}
															else
															{
															 //alert(document.Form1.txthiddenAdno.value);
															 
															
															 
																 document.Form1.txthiddenImage.value=varImgValue;
																 var TranID=document.Form1.txthiddenInvoiceID.value;
															
													
													
														
																	wopen('EditTrans.aspx?TranID='+TranID,'EditTrans',450,300);
																return false;
																 
																 
															}
															
												}	
												
												if (varImgValue=='Close')
												{							
													window.close();
													
												// document.Form1.txthiddenImage.value=varImgValue;
												// Form1.submit(); 
												// return false;
																 
												}
								
								
								if (varImgValue=='Add')
									{
										document.Form1.txthiddenImage.value=varImgValue;
										Form1.submit();
										return false;
									}	
								if (varImgValue=='Search')
								{
									document.Form1.txthiddenImage.value='Search';
									Form1.submit(); 
									return false;
								}	
								
								if (varImgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								if (varImgValue=='Cancel')
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}	
								
								
								if (varImgValue=='Reset')
									{
												var confirmed
												confirmed=window.confirm("Do  You Want To reset The Page ?");
												if(confirmed==true)
														{	
																 Form1.reset()
																 return false;
														}		
														
                                            return false;
									}			
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
		
		
		
		
		
		
	
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        background="../../images/top_nav_back.gif" border="0">
        <tbody>
            <tr>
                <td style="width: 230px">
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblTitleLabelEnterInvoice" runat="server" ForeColor="Teal" Font-Bold="True"
                        Font-Names="Verdana" Font-Size="X-Small" Width="136px" Height="12px" BorderWidth="2px"
                        BorderStyle="None">Enter Invoice</asp:Label>
                </td>
                <td style="width: 1px" valign="middle">
                    <td width="334">
                        <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">
                        <!--<asp:imagebutton id="imgOk" accessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif"></asp:imagebutton>&nbsp;-->
                        <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                            ToolTip="Save"></asp:ImageButton>&nbsp;
                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                            ToolTip="Reset"></asp:ImageButton>&nbsp;
                        <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ImageUrl="../../Images/s2close01.gif"
                            ToolTip="Close"></asp:ImageButton>&nbsp;
                        <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
                    </td>
                </td>
                <td valign="top" width="42" background="../../images/top_nav_back.gif" height="47">
                    &nbsp;
                    <img class="PlusImageCSS" id="Help" title="Help" onclick="SaveEdit('Help');" alt="E"
                        src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">
                </td>
            </tr>
        </tbody>
    </table>
    <table width="742">
        <tr>
            <td>
                <cc1:CollapsiblePanel ID="cpnlErrorPanel" runat="server" Width="85.21%" Height="87px"
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
                                <asp:ListBox ID="lstError" runat="server" BorderStyle="Groove" BorderWidth="0" Width="702px"
                                    Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </cc1:CollapsiblePanel>
            </td>
        </tr>
    </table>
    <table id="Table1" style="width: 744px; height: 136px" bordercolor="#5c5a5b" cellspacing="0"
        cellpadding="0" width="744" bgcolor="#f5f5f5" border="1">
        <tr>
            <td style="height: 36px" bordercolor="#f5f5f5">
                &nbsp;&nbsp;
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label3" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Amt Received Date</asp:Label><br>
                <ION:Customcalendar ID="dtreceivedate" runat="server" Width="120px" />
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Amt Received Now</asp:Label><br>
                <asp:TextBox onkeypress="javascript:NumericHour(this);" ID="txtamtreceivednow" runat="server"
                    Font-Names="Verdana" Font-Size="XX-Small" Width="152px" BorderWidth="1px" CssClass="txtNoFocus"
                    MaxLength="9"></asp:TextBox>
            </td>
            <td style="width: 3px; height: 36px" bordercolor="#f5f5f5">
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label9" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Bank Name</asp:Label><br>
                <asp:TextBox ID="txtBankName" runat="server" Width="149px" BorderWidth="1px" CssClass="txtNoFocus"
                    MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 30px" bordercolor="#f5f5f5">
                &nbsp;&nbsp;
            </td>
            <td style="height: 30px" bordercolor="#f5f5f5">
                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Agreement Amount  in :</asp:Label>
                <asp:Label ID="lblCur" runat="server" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Black"
                    Font-Bold="True"></asp:Label><br>
                <asp:TextBox ID="txtAgreementAmt" runat="server" Width="160px" BorderWidth="1px"
                    CssClass="txtNoFocus" ReadOnly="True" MaxLength="9"></asp:TextBox>
            </td>
            <td style="height: 30px" bordercolor="#f5f5f5">
            </td>
            <td style="height: 30px" bordercolor="#f5f5f5">
                <asp:Label ID="lblError" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Exchange Rate</asp:Label><br>
                <asp:TextBox onkeypress="javascript:NumericHour(this);" ID="txtConvRate" runat="server"
                    Font-Names="Verdana" Font-Size="XX-Small" Width="152px" BorderWidth="1px" CssClass="txtNoFocus"
                    MaxLength="4"></asp:TextBox>
            </td>
            <td style="width: 3px; height: 30px" bordercolor="#f5f5f5">
            </td>
            <td style="height: 30px" bordercolor="#f5f5f5">
                <asp:Label ID="Label10" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Cheque/Draft No</asp:Label><br>
                <asp:TextBox ID="txtDraftNo" runat="server" Width="149px" CssClass="txtNoFocus" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 36px" bordercolor="#f5f5f5">
                &nbsp;&nbsp;
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Still Amt Received</asp:Label><br>
                <asp:TextBox ID="txtbalance" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                    Width="160px" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="9"></asp:TextBox>
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
            </td>
            <td style="height: 36px" bordercolor="#f5f5f5">
                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Bank Charges</asp:Label><br>
                <asp:TextBox onkeypress="javascript:NumericHour(this);" ID="txttaxes" runat="server"
                    Font-Names="Verdana" Font-Size="XX-Small" Width="152px" BorderWidth="1px" CssClass="txtNoFocus"
                    MaxLength="9"></asp:TextBox>
            </td>
            <td style="width: 3px; height: 36px" bordercolor="#f5f5f5">
            </td>
        </tr>
        <tr>
            <td bordercolor="#f5f5f5">
                &nbsp;&nbsp;
            </td>
            <td bordercolor="#f5f5f5">
                <asp:Label ID="Label8" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Balance Available</asp:Label><br>
                <asp:TextBox ID="txtBalanceAvailable" runat="server" Width="160px" BorderWidth="1px"
                    CssClass="txtNoFocus" ReadOnly="True" MaxLength="9"></asp:TextBox>
            </td>
            <td bordercolor="#f5f5f5">
            </td>
            <td bordercolor="#f5f5f5">
                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Names="Verdana" Font-Size="XX-Small">Actual Received</asp:Label><br>
                <asp:TextBox ID="txtActuRecev" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
                    Width="152px" BorderWidth="1px" CssClass="txtNoFocus" ReadOnly="True" MaxLength="9"></asp:TextBox>
            </td>
            <td style="width: 3px" bordercolor="#f5f5f5">
            </td>
        </tr>
    </table>
    <table style="width: 752px; height: 105px">
        <tr>
            <td>
                <asp:DataGrid ID="grdInvoiceDetail" Font-Names="Verdana" Font-Size="XX-Small" Width="744px"
                    CellPadding="0" runat="server" AutoGenerateColumns="False" DataKeyField="IP_NU9_Invoice_Pricing_ID_PK"
                    BorderColor="Silver" CssClass="Grid">
                    <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                    <ItemStyle CssClass="item"></ItemStyle>
                    <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                        CssClass="gridheader" BackColor="#E0E0E0"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="IP_DT8_Invoice_Amt_Rec_Date" HeaderText="Amt Receive Date">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IP_VC50_Bank_Name" HeaderText="Bank Name"></asp:BoundColumn>
                        <asp:BoundColumn DataField="IP_VC50_Draft_No" HeaderText="Chq/Draft No"></asp:BoundColumn>
                        <asp:BoundColumn DataField="IP_NU9_Invoice_Amt_Rec" HeaderText="Received Amt"></asp:BoundColumn>
                        <asp:BoundColumn DataField="IP_NU9_Invoice_Exchange_Rate" HeaderText="Transfer Rate">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IP_NU9_Invoice_Bank_charges" HeaderText="Bank Charges">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IP_NU_Invoice_Amt_rec_Actual" HeaderText="Total Received">
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <input id="txthiddenInvoiceID" type="hidden">
            <input id="txthiddenImage" type="hidden">
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
