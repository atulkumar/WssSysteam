<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DeleteRecords.aspx.vb" Inherits="MonitoringCenter_DeleteRecords" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>DeleteRecords</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../DateControl/ION.js"></script>
		<script language="javascript">

	
	
		
		function Post(Action)
		{
			//alert(Action);
			SaveEdit(Action)
		}
		function SaveEdit(varImgValue)
				{
			    			if (varImgValue=='Save')
							{
											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit(); 
							}	
						
							  	if (varImgValue=='Add')
					               {
					                  alert("Add button is not for this screen");
					                 }	
					                
					          				
												
							if (varImgValue=='Delete')
							{
                            var id=0;
                            var idVal='cpnlDeleteRecords_dgrDeleteRecord__ctl2_CheckAll'; 							
							
							var chkVal;
							chkVal=document.getElementById(idVal).checked;
							
							var frm = document.forms[0];

							// Loop through all elements
							for (i=0; i<frm.length; i++) 
							{

							// Look for our Header Template's Checkbox
								if (idVal.indexOf ('CheckAll') != -1) 
								{

									if(frm.elements[i].disabled==false)
									{
										if (frm.elements[i].checked == true)
										{
											id=1;
										}
									}
								}
							}
							if (id==0)
							{
								alert('Please check row');
							}
							else
							{
								if (window.confirm('Are you sure you want to delete the selected record')==true )
								{
									document.Form1.txthiddenImage.value=varImgValue;	
									Form1.submit();
								}
							}
							
						}
								
					return false;
				}
				
//*******************************************************************************************************				
	function KeyCheck(rowvalues,ID)
		{
		   var tableID='cpnlDeleteRecords_dgrDeleteRecord';
    	   var table;
		   document.Form1.txthiddenID.value=ID;		
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
		
		
	function KeyCheck56()
	 {
	   alert("No Updation for this Row");	//SaveEdit('Edit');
	 }
	
	function KeyCheck55(rowvalues,ID)
	  {
	   	wopen("BGDailyMonitorEdit.aspx?ID="+ID,"AlertEdit",450,400);							
	    //'document.Form1.txthiddenImage.value='Edit';
	    //Form1.submit(); 
	  }
	
	
	
	function wopen(url, name, w, h)
     {
    // Fudge factors for window decoration space.
    // In my tests these work well on all platforms & browsers.
    w += 26;
    h += 50;
    wleft = (screen.width - w) / 2;
    wtop = (screen.height - h) / 2;
    var win = window.open(url,
     name,
     'width=' + w + ', height=' + h + ', ' +
     'left=' + wleft + ', top=' + wtop + ', ' +
     'location=no, menubar=no, ' +
     'status=no, toolbar=no, scrollbars=No, resizable=no');
    // Just in case width and height are ignored
    win.resizeTo(w, h);
    // Just in case left and top are ignored
    win.moveTo(wleft, wtop);
    win.focus();
    }	
							
				//**************
  function select_deselectAll (chkVal, idVal)
   { 

    var frm = document.forms[0];

    // Loop through all elements
    for (i=0; i<frm.length; i++) {

   // Look for our Header Template's Checkbox
   if (idVal.indexOf ('CheckAll') != -1) {

   // Check if main checkbox is checked, then select or deselect datagrid checkboxes 
   if(chkVal == true)
   {
    if(frm.elements[i].disabled==false)
      {
       frm.elements[i].checked = true;
      }
       else
     {
      frm.elements[i].checked = false;

       }
       }
    else
     {
       frm.elements[i].checked = false;

      }

       // Work here with the Item Template's multiple checkboxes
       } else if (idVal.indexOf ('chkReq1') != -1) 
       {
        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
         if(frm.elements[i].checked == false) {
           frm.elements[1].checked = false; //Uncheck main select all checkbox
           }
           }
           }
           }
           
 //***************************************************************************************          			
					
		</script>
	</HEAD>
	<body bottomMargin="0" bgColor="#f5f5f5" leftMargin="0" topMargin="0" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<TR>
					<TD><asp:button id="BtnGrdSearch" runat="server" Height="0px" Width="0px"></asp:button></TD>
					<td colSpan="2">
						<!-- *****************************************--><cc1:collapsiblepanel id="cpnlError" runat="server" Height="54px" Width="100%" BorderWidth="0px" BorderStyle="Solid"
							Visible="False" Draggable="False" CollapseImage="../Images/ToggleUp.gif" ExpandImage="../Images/ToggleDown.gif" TitleBackColor="Transparent" TitleClickable="True"
							TitleForeColor="black" PanelCSS="panel" TitleCSS="test" BorderColor="Indigo" Text="Error Message">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD colSpan="0" rowSpan="0">
										<asp:Image id="ImgError" runat="server" Width="16px" Height="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
									<TD colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" Width="552px" BorderStyle="Groove" BorderWidth="0"
											ForeColor="Red" Font-Size="XX-Small" Font-Names="Verdana"></asp:ListBox>
										<asp:TextBox id="TxtrequestID" runat="server" Width="0px" Height="0px"></asp:TextBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD style="HEIGHT: 14px"><asp:panel id="Panel5" runat="server">
										<asp:panel id="Panel6" runat="server">
											<TABLE style="WIDTH: 248px; HEIGHT: 24px" height="24">
												<TR>
													<TD style="WIDTH: 200px">
														<asp:Label id="Label1" Font-Size="8pt" Font-Names="Verdana" Runat="server" Font-Bold="True">Select Process</asp:Label>
														<asp:DropDownList id="DdlProcess" Width="100px" Runat="server" CssClass="txtNoFocus" AutoPostBack="True">
															<asp:ListItem Value="0">Select</asp:ListItem>
															<asp:ListItem Value="10020012">Dsk</asp:ListItem>
															<asp:ListItem Value="10020020">Data Base</asp:ListItem>
															<asp:ListItem Value="10020016">Ping</asp:ListItem>
															<asp:ListItem Value="10020018">Ques &amp; Reports</asp:ListItem>
														</asp:DropDownList></TD>
												</TR>
											</TABLE>
										</asp:panel>
									</asp:panel></TD>
							</TR>
							<TR>
								<TD><cc1:collapsiblepanel id="cpnlDeleteRecords" runat="server" Width="100%" BorderWidth="0px" BorderStyle="Solid"
										Visible="True" Draggable="False" CollapseImage="../Images/To&#9;ggleUp.gif" ExpandImage="../Images/ToggleDown.gif"
										TitleBackColor="Transparent" TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
										BorderColor="#f5f5f5" Text="Disk Monitor">
										<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 480px">
											<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
												<TR>
													<TD style="HEIGHT: 14px">
														<asp:panel id="Panel1" runat="server">
															<asp:panel id="Panel7" runat="server">
																<TABLE height="25">
																	<TR>
																		<TD>
																			<asp:Label id="lblFreq" Font-Size="8pt" Font-Names="Verdana" Runat="server" Font-Bold="True">Select Frequency [Hrs.]</asp:Label>
																			<asp:DropDownList id="DDLFrequency" Width="40px" Height="22px" Runat="server" CssClass="txtNoFocus">
																				<asp:ListItem Value="0">00</asp:ListItem>
																				<asp:ListItem Value="01">01</asp:ListItem>
																				<asp:ListItem Value="02">02</asp:ListItem>
																				<asp:ListItem Value="03">03</asp:ListItem>
																				<asp:ListItem Value="04">04</asp:ListItem>
																				<asp:ListItem Value="05">05</asp:ListItem>
																				<asp:ListItem Value="06">06</asp:ListItem>
																				<asp:ListItem Value="07">07</asp:ListItem>
																				<asp:ListItem Value="08">08</asp:ListItem>
																				<asp:ListItem Value="09">09</asp:ListItem>
																				<asp:ListItem Value="10">10</asp:ListItem>
																				<asp:ListItem Value="11">11</asp:ListItem>
																				<asp:ListItem Value="12">12</asp:ListItem>
																				<asp:ListItem Value="13">13</asp:ListItem>
																				<asp:ListItem Value="14">14</asp:ListItem>
																				<asp:ListItem Value="15">15</asp:ListItem>
																				<asp:ListItem Value="16">16</asp:ListItem>
																				<asp:ListItem Value="17">17</asp:ListItem>
																				<asp:ListItem Value="18">18</asp:ListItem>
																				<asp:ListItem Value="19">19</asp:ListItem>
																				<asp:ListItem Value="20">20</asp:ListItem>
																				<asp:ListItem Value="21">21</asp:ListItem>
																				<asp:ListItem Value="22">22</asp:ListItem>
																				<asp:ListItem Value="23">23</asp:ListItem>
																				<asp:ListItem Value="00">24</asp:ListItem>
																			</asp:DropDownList>
																			<asp:DropDownList id="DdlMin_Dsk" runat="server" Width="40px" CssClass="txtNoFocus">
																				<asp:ListItem Value="0">00</asp:ListItem>
																				<asp:ListItem Value="1">01</asp:ListItem>
																				<asp:ListItem Value="2">02</asp:ListItem>
																				<asp:ListItem Value="3">03</asp:ListItem>
																				<asp:ListItem Value="4">04</asp:ListItem>
																				<asp:ListItem Value="5">05</asp:ListItem>
																				<asp:ListItem Value="6">06</asp:ListItem>
																				<asp:ListItem Value="7">07</asp:ListItem>
																				<asp:ListItem Value="8">08</asp:ListItem>
																				<asp:ListItem Value="9">09</asp:ListItem>
																				<asp:ListItem Value="10">10</asp:ListItem>
																				<asp:ListItem Value="11">11</asp:ListItem>
																				<asp:ListItem Value="12">12</asp:ListItem>
																				<asp:ListItem Value="13">13</asp:ListItem>
																				<asp:ListItem Value="14">14</asp:ListItem>
																				<asp:ListItem Value="15">15</asp:ListItem>
																				<asp:ListItem Value="16">16</asp:ListItem>
																				<asp:ListItem Value="17">17</asp:ListItem>
																				<asp:ListItem Value="18">18</asp:ListItem>
																				<asp:ListItem Value="19">19</asp:ListItem>
																				<asp:ListItem Value="20">20</asp:ListItem>
																				<asp:ListItem Value="21">21</asp:ListItem>
																				<asp:ListItem Value="22">22</asp:ListItem>
																				<asp:ListItem Value="23">23</asp:ListItem>
																				<asp:ListItem Value="24">24</asp:ListItem>
																				<asp:ListItem Value="25">25</asp:ListItem>
																				<asp:ListItem Value="26">26</asp:ListItem>
																				<asp:ListItem Value="27">27</asp:ListItem>
																				<asp:ListItem Value="28">28</asp:ListItem>
																				<asp:ListItem Value="29">29</asp:ListItem>
																				<asp:ListItem Value="30">30</asp:ListItem>
																				<asp:ListItem Value="31">31</asp:ListItem>
																				<asp:ListItem Value="32">32</asp:ListItem>
																				<asp:ListItem Value="33">33</asp:ListItem>
																				<asp:ListItem Value="34">34</asp:ListItem>
																				<asp:ListItem Value="35">35</asp:ListItem>
																				<asp:ListItem Value="36">36</asp:ListItem>
																				<asp:ListItem Value="37">37</asp:ListItem>
																				<asp:ListItem Value="38">38</asp:ListItem>
																				<asp:ListItem Value="39">39</asp:ListItem>
																				<asp:ListItem Value="40">40</asp:ListItem>
																				<asp:ListItem Value="41">41</asp:ListItem>
																				<asp:ListItem Value="42">42</asp:ListItem>
																				<asp:ListItem Value="43">43</asp:ListItem>
																				<asp:ListItem Value="44">44</asp:ListItem>
																				<asp:ListItem Value="45">45</asp:ListItem>
																				<asp:ListItem Value="46">46</asp:ListItem>
																				<asp:ListItem Value="47">47</asp:ListItem>
																				<asp:ListItem Value="48">48</asp:ListItem>
																				<asp:ListItem Value="49">49</asp:ListItem>
																				<asp:ListItem Value="50">50</asp:ListItem>
																				<asp:ListItem Value="51">51</asp:ListItem>
																				<asp:ListItem Value="52">52</asp:ListItem>
																				<asp:ListItem Value="53">53</asp:ListItem>
																				<asp:ListItem Value="54">54</asp:ListItem>
																				<asp:ListItem Value="55">55</asp:ListItem>
																				<asp:ListItem Value="56">56</asp:ListItem>
																				<asp:ListItem Value="57">57</asp:ListItem>
																				<asp:ListItem Value="58">58</asp:ListItem>
																				<asp:ListItem Value="59">59</asp:ListItem>
																			</asp:DropDownList></TD>
																		<TD>
																			<asp:Label id="pg" Width="40px" ForeColor="#0000C0" Font-Size="8pt" Font-Names="Verdana" Runat="server"
																				Font-Bold="True">Page</asp:Label></TD>
																		<TD>
																			<asp:Label id="CurrentPg" runat="server" Width="10px" Height="12px" ForeColor="Crimson" Font-Size="X-Small"
																				Font-Bold="True"></asp:Label></TD>
																		<TD>
																			<asp:Label id="of" ForeColor="#0000C0" Font-Size="8pt" Font-Names="Verdana" Runat="server"
																				Font-Bold="True">of</asp:Label></TD>
																		<TD>
																			<asp:label id="TotalPages" runat="server" Width="10px" Height="12px" ForeColor="Crimson" Font-Size="X-Small"
																				Font-Bold="True"></asp:label></TD>
																		<TD>
																			<asp:imagebutton id="Firstbutton" runat="server" ImageUrl="../Images/next9.jpg" AlternateText="First"
																				ToolTip="First"></asp:imagebutton></TD>
																		<TD width="14">
																			<asp:imagebutton id="Prevbutton" runat="server" ImageUrl="../Images/next99.jpg" ToolTip="Previous"></asp:imagebutton></TD>
																		<TD>
																			<asp:imagebutton id="Nextbutton" runat="server" ImageUrl="../Images/next9999.jpg" ToolTip="Next"></asp:imagebutton></TD>
																		<TD>
																			<asp:imagebutton id="Lastbutton" runat="server" ImageUrl="../Images/next999.jpg" ToolTip="Last"></asp:imagebutton></TD>
																		<TD>
																			<asp:textbox id="txtPageSize" runat="server" Width="24px" Height="14px" Font-Size="7pt" MaxLength="3"></asp:textbox></TD>
																		<TD>
																			<asp:Button id="Button3" runat="server" Width="16px" Height="16px" Text=">" BorderStyle="None"
																				ForeColor="Navy" Font-Size="7pt" Font-Bold="True" ToolTip="Change Paging Size"></asp:Button></TD>
																		<TD></TD>
																		<TD>
																			<asp:Label id="lblrecords" runat="server" Height="12pt" ForeColor="MediumBlue" Font-Size="8pt"
																				Font-Names="Verdana" Font-Bold="True">Total Records</asp:Label></TD>
																		<TD>
																			<asp:Label id="TotalRecods" runat="server" Height="12pt" ForeColor="Crimson" Font-Size="8pt"
																				Font-Names="Verdana" Font-Bold="True"></asp:Label></TD>
																		<TD></TD>
																	</TR>
																</TABLE>
															</asp:panel>
														</asp:panel></TD>
												</TR>
												<TR>
													<TD>
														<DIV style="OVERFLOW: auto; WIDTH: 250%">
															<asp:DataGrid id="dgrDeleteRecord" BorderColor="#d4d4d4" BorderWidth="0" Runat="server" CssClass="grid"
																DataKeyField="ID" PageSize="20" PagerStyle-Visible="False" AllowPaging="True"
																CellPadding="0">
																<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																<ItemStyle CssClass="GridItem"></ItemStyle>
																<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
																	<asp:TemplateColumn>
																		<HeaderStyle Font-Bold="True"></HeaderStyle>
																		<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
																		<HeaderTemplate>
																			<asp:CheckBox ID="CheckAll" OnClick="javascript: return select_deselectAll (this.checked, this.id);"
																				Runat="server"></asp:CheckBox>
																		</HeaderTemplate>
																		<ItemTemplate>
																			<asp:CheckBox ID="chkReq1" Runat="server"></asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle Visible="False"></PagerStyle>
															</asp:DataGrid></DIV>
													</TD>
												</TR>
											</TABLE>
										</DIV>
									</cc1:collapsiblepanel></TD>
							</TR>
						</TABLE>
						<!-- *****************************************--></td>
				</TR>
			</table>
			<INPUT type="hidden" name="txthiddenImage"> <INPUT type="hidden" name="txthiddenID">
			<input type="hidden" name="txtMachineInfo">
		</form>
	</body>
</html>
