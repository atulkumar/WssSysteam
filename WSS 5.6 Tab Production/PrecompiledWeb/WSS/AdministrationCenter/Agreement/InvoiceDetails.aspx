<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_Agreement_InvoiceDetails, App_Web_kirrnbfy" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice Detail</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <%--	<LINK href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">--%>

    <script language="javascript" src="../../images/Js/JSValidation.js" type="text/javascript"></script>
    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative; ;TOP:expression(this.offsetParent.scrollTop);BACKGROUND-COLOR:#e0e0e0}</style>

    <script language="javascript" src="../../DateControl/ION.js" type="text/javascript"></script>

    <script language="Javascript" type="text/javascript">
				
				
							
		function CheckLength()
		{
				var TDLength=document.getElementById('cpnlID_txtDesc').value.length;
				if ( TDLength>0 )
				{
					if ( TDLength > 500 )
					{
						alert('The Invoice Description cannot be more than 500 characters\n (Current Length :'+TDLength+')');
						return false;
					}
				}
				return true;
		}		
		
		
			var globalid;
			var globalSkil;
			var globalAddNo;
			var globalGrid;
			var globaldbclick = 0;
						
			function OpenW(a,b,c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search',500,450);
					return false;
				}
				
			function OpenAB(c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as CustomerName,CI_VC16_Alias as AliasName from T010011 where CI_VC8_Address_Book_Type='+"'COM'"+ '  &tbname=' + c ,'Search',500,450);
					return false;
				}
				
			function OpenABPer(c)
				{
				var cusComp=document.getElementById('cpnlAGH_txtCusNo').value;
				var sComp='<%=session("propCompanyID")%>';
				
					wopen('../../Search/Common/PopSearch.aspx?ID=select CI_NU8_Address_Number as ID,CI_VC36_Name as UserName,CI_IN4_Business_Relation as Company from T010011 where CI_IN4_Business_Relation in('+"'"+cusComp+"','"+sComp+"')" + '  &tbname=' + c ,'Search',500,450);
					return false;
				}
			function getVal(val)
			{
				//alert(val);
			}
			
			function wopen(url, name, w, h)
				{
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

			function OpenWUdc_Search()
				{
					window.open("Udc_Home_Search.aspx","ss","scrollBars=no,resizable=No,width=350,height=450,status=yes");
				}
			
			function addToParentList(Afilename,TbName,strname)
				{				
					if (Afilename != "" || Afilename != 'undefined')
						{
							varName = TbName + 'Name'
							if((document.getElementById(varName) == null))
							{								
							}
							else
							{
								document.getElementById(varName).value=strname;
							}
							document.getElementById(TbName).value=Afilename;
							aa=Afilename;
						}
					else					
						{
							document.Form1.txtAB_Type.value=aa;
						}
				}
				
			function addToParentCtrl(value)
				{
					document.getElementById('ContactInfo_txtBr').value=Value;
				}
				
				
			function ContactKey(cc)
				{
					document.getElementById('ClpContact_Info_txtBr').value=cc;
				}					
								
			
					
			function callrefresh()
				{
					Form1.submit();
				}							

																							
			function KeyCheck(nn,rowNo,AgNo,CType,taskNo,taskType)
					{
												
						document.Form1.txthidden.value=nn;
						document.getElementById('txtRowNo').value=rowNo;
						document.getElementById('txtAgNo').value=AgNo;
						document.getElementById('txtCType').value=CType;
						document.getElementById('txtTaskNo').value=taskNo;
						document.getElementById('txtTaskType').value=taskType;
						
//alert(document.Form1.txthidden.value+' '+document.getElementById('txtRowNo').value+' '+document.getElementById('txtAgNo').value+' '+document.getElementById('txtCType').value+' '+document.getElementById('txtTaskNo').value+' '+document.getElementById('txtTaskType').value);		      											
									/*	var tableID='cpnlICall_GrdAddSerach'  //your datagrids id
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
									*/
				}	
				
			function KeyCheck55(aa,bb,cc,dd)
				{	
					SaveEdit('Edit');
					return false;	
				}
						
			function SaveEdit(varImgValue)
				{
			    	if (varImgValue=='Edit')
						{
						
							//Security Block
							var obj=document.getElementById("imgEdit")
							if(obj==null)
							{
								alert("You don't have access rights to edit record");
								return false;
							}

							if (obj.disabled==true) 
							{
							
								alert("You don't have access rights to edit record");
								return false;
							}
							//End of Security Block
						
							if (document.Form1.txthidden.value==0)
								{
									alert("Please select the row");
								}
							else
								{
									if(document.getElementById('cpnlID_txtInvNo').value=='')
									{
									alert('Save Record');
									return false;
									}
									else 
									{
									var code=document.Form1.txthidden.value;
									var compID=document.Form1.cpnlID_cddlCustomer_txtHID.value;
									var rowNo=document.getElementById('txtRowNo').value;
									var InvNo=document.getElementById('cpnlID_txtInvNo').value;
									var AgNo=document.getElementById('txtAgNo').value;
									var CType=document.getElementById('txtCType').value;
									var TaskNo=document.getElementById('txtTaskNo').value;
									var TaskType=document.getElementById('txtTaskType').value;
									wopen('InvoiceEdit.aspx?CallNo='+code+'&CompID='+compID+'&rowNo='+rowNo+'&InvNo='+InvNo+'&AgNo='+AgNo+'&CType='+CType+'&TaskNo='+TaskNo+'&TaskType='+TaskType,'Search',600,500);
									return false;
									}
								}										
						}	
											
					if (varImgValue=='Close')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							return false;
						}								
							
					if (varImgValue=='Add')
						{
							document.Form1.txthiddenImage.value=varImgValue;
							Form1.submit(); 
							return false;
						}	
					
					if (varImgValue=='Logout')
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
					
					if(document.getElementById('cpnlID_txtPC').disabled)
					{
						document.getElementById('txtDisType').value='0';
					}
					else
					{
						document.getElementById('txtDisType').value='1';		
					}
					
					document.getElementById('cpnlID_txtPC').disabled=false;
					
					if (document.getElementById('cpnlID_txtInvNo').value=='')
					{
						if(confirm('Make sure that From and To date for the Invoice are correct.'))
						{
							if ( CheckLength()==true )
							{
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 		
							}
							return false;	
						}
						else
						{
							return false;
						}
					}
					else
					{
							if ( CheckLength()==true )
							{
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 		
							}
						return false;
					}
						
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
					
					if(document.getElementById('cpnlID_txtPC').disabled)
					{
						document.getElementById('txtDisType').value='0';
					}
					else
					{
						document.getElementById('txtDisType').value='1';		
					}
					
					document.getElementById('cpnlID_txtPC').disabled=false;
					
					if (document.getElementById('cpnlID_txtInvNo').value=='')
					{
						if(confirm('Make sure that From and To date for the Invoice are correct.'))
						{
							if ( CheckLength()==true )
							{
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 		
							}
							return false;	
						}
						else
						{
							return false;
						}
					}
					else
					{
							if ( CheckLength()==true )
							{
								document.Form1.txthiddenImage.value=varImgValue;
								Form1.submit(); 		
							} 
						return false;
					}
																			
							
				}		
						
					if (varImgValue=='Search')
								{
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


	function NumericHour(ControlID)
		{	
		
			var Val;
			Val = ControlID.value;
			//Val=document.getElementById('ControlID').value
		//alert(Val);
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
		
		function disPC()
		{
			var flag;
			flag=confirm('Any discounts given on the Call Types will be erased.\nAre u sure to proceed ?');
			if (flag)
			{
				document.getElementById('cpnlID_txtPC').disabled=false;	
			}
			else
			{
				document.getElementById('cpnlID_txtPC').disabled=true;
			}
		}
		
		function NumericDis(CtrlID)
		{	
		
			var ControlID=document.getElementById(CtrlID);
			var proceed;
			
			if(document.getElementById('cpnlID_txtPC').disabled==false)
			{
			proceed=confirm('This action will autocalculate the total discount.\nAre you sure to give discount on Call Types ?');
			}
			else
			{
			proceed=true;
			}
						
			if (proceed)
			{
			document.getElementById('cpnlID_txtPC').disabled=true;
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
			else
			{
				ControlID.value='';
			}						
		}						
			
	function calculateAmtCall(totAmt,objPC,objDisAmt)
	{
	
		var pc,amt;
		
		pc=document.getElementById(objPC).value;
		amt=totAmt;
		document.getElementById(objDisAmt).value= Number(amt)- (Number(amt)* Number(pc)/100);
		calculateTotalAmt();
	}	
					
function calculateAmt()
	{
	
		var pc,amt;
		
		pc=document.getElementById('cpnlID_txtPC').value;
		amt=document.getElementById('cpnlID_txtTotAmt').value;
		document.getElementById('cpnlID_txtDAmt').value= Number(amt)- (Number(amt)* Number(pc)/100);
		
	}		


function calculateTotalAmt()
	{
	
		var rowsCnt,i,limit,ctrlNo;
		var total='0';
		var pc;
		
		rowsCnt =document.getElementById('txtCntRep').value;
		document.getElementById('cpnlID_txtDAmt').value= '0';
		limit = parseInt(rowsCnt);

		for(i=1;i<=limit ;i++)
		{
		ctrlNo = i+1;
		
		if(document.getElementById('cpnlHrsDetails_grdReport__ctl'+ctrlNo+'_txtDisAmt'))
		{			
			if (!(document.getElementById('cpnlHrsDetails_grdReport__ctl'+ctrlNo+'_txtDisAmt').value ==''))
			{
				total = Number(total) + Number(document.getElementById('cpnlHrsDetails_grdReport__ctl'+ctrlNo+'_txtDisAmt').value);
			}
		}
		}	
	
		document.getElementById('cpnlID_txtDAmt').value = total;
		document.getElementById('cpnlID_txtPC').value=100-(Number(total)*100/ Number(document.getElementById('cpnlID_txtTotAmt').value));
		
	}		
	
				
			function FP_swapImg() 
				{//v1.0
						var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
						n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
						elm.$src=elm.src; elm.src=args[n+1]; } }
				}

			function FP_preloadImgs() 
				{//v1.0
						var d=document,a=arguments; if(!d.FP_imgs) d.FP_imgs=new Array();
						for(var i=0; i<a.length; i++) { d.FP_imgs[i]=new Image; d.FP_imgs[i].src=a[i]; }
				}

			function FP_getObjectByID(id,o) 
				{//v1.0
						var c,el,els,f,m,n; if(!o)o=document; if(o.getElementById) el=o.getElementById(id);
						else if(o.layers) c=o.layers; else if(o.all) el=o.all[id]; if(el) return el;
						if(o.id==id || o.name==id) return o; if(o.childNodes) c=o.childNodes; if(c)
						for(n=0; n<c.length; n++) { el=FP_getObjectByID(id,c[n]); if(el) return el; }
						f=o.forms; if(f) for(n=0; n<f.length; n++) { els=f[n].elements;
						for(m=0; m<els.length; m++){ el=FP_getObjectByID(id,els[n]); if(el) return el; } }
						return null;
				}
				
    </script>

    <script type="text/javascript">
    //A Function to improve design i.e delete the extra cell of table
    function onEnd() {
        var x = document.getElementById('cpnlID_collapsible').cells[0].colSpan = "1";
        var y = document.getElementById('cpnlICall_collapsible').cells[0].colSpan = "1";
        var z = document.getElementById('cpnlHrsDetails_collapsible').cells[0].colSpan = "1";
        
    } 
     //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	

    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table height="20%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table height="20%" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td background="../../images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td align="left" width="271">
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" AlternateText="."
                                                        CommandName="submit" ImageUrl="white.GIF" Height="1px" Width="1px"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelInvDetail" runat="server" ForeColor="Teal" Font-Bold="True"
                                                        Font-Names="Verdana" Font-Size="X-Small" BorderWidth="2px" BorderStyle="None">Invoice Detail</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <img title="Seperator" alt="R" src="../../images/00Seperator.gif" border="0">&nbsp;
                                                    <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../images/S2Save01.gif"
                                                        ToolTip="Save"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" Visible="false" ImageUrl="../../images/s1ok02.gif"
                                                        ToolTip="Ok"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" AlternateText="Edit" ImageUrl="../../Images/S2edit01.gif"
                                                        ToolTip="Edit"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../images/reset_20.gif"
                                                        ToolTip="Reset"></asp:ImageButton>&nbsp;
                                                    <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../images/s1search02.gif"
                                                        ToolTip="Search"></asp:ImageButton>&nbsp;
                                                    <img src="../../Images/S2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />&nbsp;<img
                                                        title="Seperator" alt="R" src="../../images/00Seperator.gif" border="0">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="152" background="../../images/top_nav_back.gif" height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('717','../../');"
                                            alt="E" src="../../images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <tr>
                                <td>
                                    <div style="overflow: auto; width: 100%; height: 581px">
                                        <table id="Table16" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                            border="0">
                                            <tr>
                                                <td valign="top" colspan="1">
                                                    <!--  **********************************************************************-->
                                                    <div style="overflow: auto; width: 100%; height: 581px">
                                                        <table style="border-collapse: collapse" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <!-- **********************************************************************-->
                                                                                <cc1:CollapsiblePanel ID="cpnlID" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
                                                                                    Draggable="False" CollapseImage="../../images/ToggleUp.gif" ExpandImage="../../images/ToggleDown.gif"
                                                                                    Text="Invoice Details" TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black"
                                                                                    PanelCSS="panel" TitleCSS="test" Visible="true" BorderColor="Indigo">
                                                                                    <table width="100%" bgcolor="#f5f5f5" border="0">
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <asp:Label ID="lblLastInv" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    Font-Bold="True" ForeColor="DimGray"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <asp:Label ID="Label4" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                                    ForeColor="DimGray">Customer</asp:Label><br>
                                                                                                <uc1:CustomDDL ID="cddlCustomer" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <asp:Label ID="Label3" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                                    ForeColor="DimGray">Our Reference</asp:Label><br>
                                                                                                <uc1:CustomDDL ID="cddlPerson" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="Label10" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    Font-Bold="True" ForeColor="DimGray">To Invoice</asp:Label><br>
                                                                                                <uc1:CustomDDL ID="CDDLTo" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <asp:Label ID="lblMiddleName6" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    Font-Bold="True" ForeColor="DimGray">Status</asp:Label><br>
                                                                                                <uc1:CustomDDL ID="cddlStatus" runat="server" Width="120px"></uc1:CustomDDL>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label2" runat="server" Width="120px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    Font-Bold="True" ForeColor="DimGray">Invoice From Date</asp:Label><br>
                                                                                                <ION:Customcalendar ID="dtFromDate" runat="server" Height="20px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="Label5" runat="server" Width="120px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    Font-Bold="True" ForeColor="DimGray">Invoice To Date</asp:Label><br>
                                                                                                <ION:Customcalendar ID="dtToDate" runat="server" Height="20px" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblMiddleName10" runat="server" Width="120px" Font-Size="XX-Small"
                                                                                                    Font-Names="Verdana" Font-Bold="True" ForeColor="DimGray">Invoice Due Date</asp:Label><br>
                                                                                                <ION:Customcalendar ID="dtDueDate" runat="server" Height="20px" />
                                                                                            </td>
                                                                                            <td valign="top" rowspan="3">
                                                                                                <asp:Label ID="Label6" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                                    ForeColor="DimGray">Description</asp:Label><br>
                                                                                                <asp:TextBox ID="txtDesc" runat="server" Width="350px" Height="95" BorderStyle="Solid"
                                                                                                    BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus"
                                                                                                    MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label7" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                                    ForeColor="DimGray">Total Amount</asp:Label><br>
                                                                                                <asp:TextBox ID="txtTotAmt" runat="server" Width="120px" BorderStyle="Solid" BorderWidth="1px"
                                                                                                    Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="9"
                                                                                                    ReadOnly="True"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="Label8" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                                    ForeColor="DimGray">Discount %age</asp:Label><br>
                                                                                                <asp:TextBox ID="txtPC" onblur="javascript:calculateAmt();" onclick="disPC();" runat="server"
                                                                                                    Width="120px" BorderStyle="Solid" BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    CssClass="txtNoFocus" MaxLength="9"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="Label9" runat="server" Width="144px" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    Font-Bold="True" ForeColor="DimGray">Net Amount</asp:Label><br>
                                                                                                <asp:TextBox ID="txtDAmt" runat="server" Width="120px" BorderStyle="Solid" BorderWidth="1px"
                                                                                                    Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="9"
                                                                                                    ReadOnly="True"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <asp:CheckBox ID="chkMail" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    Font-Bold="True" ForeColor="DimGray" Text="Send Mail"></asp:CheckBox>
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <asp:Label ID="lblMiddleName2" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                                                                                    Font-Bold="True" ForeColor="DimGray">E Mail</asp:Label><br>
                                                                                                <asp:TextBox ID="txtReference" runat="server" Width="120px" BorderStyle="Solid" BorderWidth="1px"
                                                                                                    Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus" MaxLength="50"></asp:TextBox>
                                                                                                <asp:TextBox ID="txtInvNo" runat="server" Width="0" Height="0px" BorderStyle="Solid"
                                                                                                    BorderWidth="0px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus"
                                                                                                    MaxLength="8" name="txtAddNo"></asp:TextBox>
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <asp:Label ID="Label1" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                                                                                                    ForeColor="DimGray">Draft/Create Invoice</asp:Label>
                                                                                                <asp:RadioButtonList ID="rblTemp" runat="server" Width="120px" Font-Size="XX-Small"
                                                                                                    Font-Names="Verdana" RepeatDirection="Horizontal">
                                                                                                    <asp:ListItem Value="Y" Selected="True">Draft</asp:ListItem>
                                                                                                    <asp:ListItem Value="N">Create</asp:ListItem>
                                                                                                </asp:RadioButtonList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </cc1:CollapsiblePanel>
                                                                                <!-- **********************************************************************-->
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <cc1:CollapsiblePanel ID="cpnlICall" runat="server" Height="47px" Width="100%" BorderWidth="0px"
                                                                                    BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                                                    ExpandImage="../../Images/ToggleDown.gif" Text="Invoice Calls" TitleBackColor="Transparent"
                                                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                                    Visible="True" BorderColor="Indigo">
                                                                                    <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                                        width="100%" align="left" border="0">
                                                                                        <tr>
                                                                                            <td valign="top" align="left" height="146">
                                                                                                <asp:Panel ID="Panel1" runat="server">
                                                                                                </asp:Panel>
                                                                                                <asp:DataGrid ID="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px"
                                                                                                    Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" CssClass="grid"
                                                                                                    DataKeyField="CallNo" PageSize="50" HorizontalAlign="Left" GridLines="Horizontal"
                                                                                                    CellPadding="0" AutoGenerateColumns="False">
                                                                                                    <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                                    <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                                                                    <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                                                                    </ItemStyle>
                                                                                                    <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                                                    </HeaderStyle>
                                                                                                    <Columns>
                                                                                                        <asp:TemplateColumn HeaderText="CallNo">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "CallNo") %>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="CallType">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CallType") %>'>
                                                                                                                </asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="TaskNo">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblTaskNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TaskNo") %>'>
                                                                                                                </asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="TaskType">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblTaskType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TaskType") %>'>
                                                                                                                </asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="CallDesc">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "CallDesc") %>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="Status">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "CallStatus") %>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="ActHrs">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="60"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblActHrs" runat="server" Width="60px"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="ActBillHrs">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="60px"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:TextBox ID="txtBillHrs" runat="server" ReadOnly="True" CssClass="txtNoFocus"
                                                                                                                    Width="60pt"></asp:TextBox>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn Visible="False">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="0"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblAgNo" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "AgNo") %>'>
                                                                                                                </asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="CallAmount">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCallAmt" runat="server" Width="80"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                    </Columns>
                                                                                                    <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                                                </asp:DataGrid>
                                                                                                <asp:TextBox ID="txtTotal" runat="server" Width="0px" Height="0px" BorderStyle="Solid"
                                                                                                    BorderWidth="0px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus"
                                                                                                    MaxLength="8"></asp:TextBox>
                                                                                                <asp:TextBox ID="txtCnt" runat="server" Width="0px" Height="0px" BorderStyle="Solid"
                                                                                                    BorderWidth="0px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus"
                                                                                                    MaxLength="8" name="txtAddNo"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </cc1:CollapsiblePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <cc1:CollapsiblePanel ID="cpnlHrsDetails" runat="server" Height="47px" Width="100%"
                                                                                    BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                                                    ExpandImage="../../Images/ToggleDown.gif" Text="Invoice Summary" TitleBackColor="Transparent"
                                                                                    TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                                    Visible="True" BorderColor="Indigo">
                                                                                    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
                                                                                        align="left" border="0">
                                                                                        <tr>
                                                                                            <td valign="top" align="left">
                                                                                                <asp:DataGrid ID="grdReport" runat="server" Width="70%" BorderStyle="None" BorderWidth="1px"
                                                                                                    Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" CssClass="grid"
                                                                                                    PageSize="50" HorizontalAlign="Left" GridLines="Horizontal" CellPadding="0" AutoGenerateColumns="False">
                                                                                                    <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                                    <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                                    <AlternatingItemStyle CssClass="GridAlternateItem" BackColor="WhiteSmoke"></AlternatingItemStyle>
                                                                                                    <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="griditem" BackColor="White">
                                                                                                    </ItemStyle>
                                                                                                    <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                                                    </HeaderStyle>
                                                                                                    <PagerStyle Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                                                    <Columns>
                                                                                                        <asp:TemplateColumn HeaderText="CallType">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="80"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCallType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CallType") %>'>
                                                                                                                </asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="TotalHours">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="80"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblTotalHours" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TotalHours") %>'>
                                                                                                                </asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="TotalAmount">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="90"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblTotAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TotalAmount") %>'>
                                                                                                                </asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="DiscountPC">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="60px"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:TextBox ID="txtDPC" runat="server" CssClass="txtNoFocus" Width="60pt" Text='<%# DataBinder.Eval(Container.DataItem, "DisPC") %>'>
                                                                                                                </asp:TextBox>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn HeaderText="NetAmt">
                                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                                            <ItemStyle Font-Size="XX-Small" Font-Names="Verdana" Width="60px"></ItemStyle>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:TextBox ID="txtDisAmt" runat="server" ReadOnly="True" CssClass="txtNoFocus"
                                                                                                                    Width="60pt" Text='<%# DataBinder.Eval(Container.DataItem, "DisAmount") %>'>
                                                                                                                </asp:TextBox>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                    </Columns>
                                                                                                </asp:DataGrid>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </cc1:CollapsiblePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="635px" BorderStyle="Groove" BorderWidth="0"
                            Visible="false" Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Red"></asp:ListBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:TextBox ID="txtTaskNo" runat="server" Width="0px" BorderWidth="0px"></asp:TextBox><asp:TextBox
        ID="txtRowNo" runat="server" Width="0px" BorderWidth="0px"></asp:TextBox><asp:TextBox
            ID="txtAgNo" runat="server" Width="0px" BorderWidth="0px"></asp:TextBox><asp:TextBox
                ID="txtTaskType" runat="server" Height="0px" Width="0px" Font-Names="Verdana"
                Font-Size="XX-Small" BorderWidth="0px" BorderStyle="Solid" CssClass="txtNoFocus"
                MaxLength="8"></asp:TextBox><asp:TextBox ID="txtCType" runat="server" Width="0px"
                    BorderWidth="0px"></asp:TextBox><asp:TextBox ID="txthiddenImage" runat="server" Width="0px"
                        BorderWidth="0px"></asp:TextBox><asp:TextBox ID="txthidden" runat="server" Width="0px"
                            BorderWidth="0px"></asp:TextBox><asp:TextBox ID="txtPreInv" runat="server" Height="0px"
                                Width="0px" Font-Names="Verdana" Font-Size="XX-Small" BorderWidth="0px" BorderStyle="Solid"
                                CssClass="txtNoFocus" MaxLength="50"></asp:TextBox><asp:TextBox ID="txtCntRep" runat="server"
                                    Height="0px" Width="0px" Font-Names="Verdana" Font-Size="XX-Small" BorderWidth="0px"
                                    BorderStyle="Solid" CssClass="txtNoFocus" MaxLength="50"></asp:TextBox><asp:TextBox
                                        ID="txtDisType" runat="server" Height="0px" Width="0px" Font-Names="Verdana"
                                        Font-Size="XX-Small" BorderWidth="0px" BorderStyle="Solid" CssClass="txtNoFocus"></asp:TextBox></form>
</body>
</html>
