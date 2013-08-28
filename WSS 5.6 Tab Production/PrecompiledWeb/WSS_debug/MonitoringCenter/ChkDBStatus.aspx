<%@ page language="VB" autoeventwireup="false" inherits="MonitoringCenter_ChkDBStatus, App_Web_2azc-idb" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>ChkDBStatus</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../Images/Js/JSValidation.js"></script>
		<LINK href="../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../DateControl/ION.js"></script>
		<script language="javascript">

		
			
		/**********************AJAX for SubCategory****************************************/

		var gtype;
		var xmlHttp; 
		var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
		var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
		var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
		//netscape, safari, mozilla behave the same??? 
		var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 

	
		var gDID;
		var gMID;
		
		function DomainChange(DID,MID)
		{
	//	alert(DID);
			//document.getElementById('txtHIDAgreement')='';
			gDID=DID;
			gMID=MID;
			
			xmlHttp=null;
			var ddlDomain=document.getElementById(gDID);
			var DomainID=ddlDomain.options(ddlDomain.selectedIndex).value;
			var url= '../AJAX Server/AjaxInfo.aspx?Type=DomainMachine&DomainID='+DomainID+'&Rnd='+Math.random();
			xmlHttp = GetXmlHttpObject(stateChangeHandler);    
			xmlHttp_Get(xmlHttp, url); 
		}
		 
		
		function stateChangeHandler() 
		 { 	
				 document.getElementById(gMID).options.length=0;
				objNewOption = document.createElement("OPTION");
				document.getElementById(gMID).options.add(objNewOption);
				objNewOption.value = '0';
				objNewOption.innerText ='Select';	
				
				 
				if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
				{ 
						var response = xmlHttp.responseXML; 
						var info = response.getElementsByTagName("INFO");
						
						if(info.length > 0)
						{
								var vTable = response.getElementsByTagName("TABLE");
								var intT;
								for ( intT=0; intT<vTable.length; intT++)
								{
									var item = vTable[intT].getElementsByTagName("ITEM");
									var objForm = document.Form1;
									var DataName='';
									var DataID='';
									switch(intT)
									{
										case 0:
										{	
										
											for (var inti=0; inti<item.length; inti++)
											{
													var objNewOption = document.createElement("OPTION");
													document.getElementById(gMID).options.add(objNewOption);
													objNewOption.value = item[inti].getAttribute("COL0");
													objNewOption.innerText = item[inti].getAttribute("COL1");
													DataName=DataName+item[inti].getAttribute("COL1") + '^';
													DataID=DataID+item[inti].getAttribute("COL0") + '^';			
											}
											document.Form1.txtMachineInfo.value= DataName + '~' + DataID ;
											break;
										}//case 0
						
									}//switch
								} //for loop
						
						}//if
						
				}//
				else
				{
						//wait				
				}
				
		} //function
		
		
		function xmlHttp_Get(xmlhttp, url) 
		{ 
		        xmlhttp.open('GET', url, true); 
		        xmlhttp.send(null); 
		       
		} 
    
		function GetXmlHttpObject(handler) 
		{ 
				var objXmlHttp = null;    //Holds the local xmlHTTP object instance 
				if (is_ie)
		        { 
						var strObjName = (is_ie5) ? 'Microsoft.XMLHTTP' : 'Msxml2.XMLHTTP'; 
				        try
				        { 
								objXmlHttp = new ActiveXObject(strObjName); 
								objXmlHttp.onreadystatechange = handler; 
						} 
						catch(e)
						{ 
								alert('IE detected, but object could not be created. Verify that active scripting and activeX controls are enabled'); 
								return; 
			            } 
				} 
				else if (is_opera)
				{ 
						alert('Opera detected. The page may not behave as expected.'); 
						return; 
				} 
				else
				{ 
						objXmlHttp = new XMLHttpRequest(); 
						objXmlHttp.onload = handler; 
						objXmlHttp.onerror = handler; 
				} 
				return objXmlHttp; 
		} 
    
		

/***************************************************************************/
		
		
		
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
					                
					       	if (varImgValue=='Close')
						{
						 	document.Form1.txthiddenImage.value=varImgValue;
						Form1.submit(); 
						}						
					          				
												
							if (varImgValue=='Delete')
							{
                            var id=0;
                            var idVal='cpnlDBMonitor_dgrDBMonitor__ctl2_CheckAll'; 							
							
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
		   var tableID='cpnlDBMonitor_dgrDBMonitor';
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
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" >
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td><asp:button id="BtnGrdSearch" runat="server" Width="0px" Height="0"></asp:button></td>
					<td><cc1:collapsiblepanel id="cpnlError" runat="server" Width="100%" Height="54px" BorderColor="#f5f5f5" Visible="False"
							TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
							Text="Error Message" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/ToggleUp.gif"
							Draggable="False" BorderWidth="0px" BorderStyle="Solid">
							<TABLE id="Table2" borderColor="lightgrey" cellSpacing="0" cellPadding="0" width="100%"
								border="0">
								<TR>
									<TD colSpan="0" rowSpan="0">
										<asp:Image id="ImgError" runat="server" Height="16px" Width="16px" ImageUrl="../Images/warning.gif"></asp:Image></TD>
									<TD colSpan="0" rowSpan="0">
										<asp:ListBox id="lstError" runat="server" Width="552px" BorderStyle="Groove" BorderWidth="0"
											ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"></asp:ListBox></TD>
								</TR>
							</TABLE>
						</cc1:collapsiblepanel>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD style="HEIGHT: 14px"><asp:panel id="Panel5" runat="server">
										<asp:panel id="Panel6" runat="server">
											<TABLE style="WIDTH: 328px; HEIGHT: 24px" height="24">
												<TR>
													<TD style="WIDTH: 200px">
														<asp:Label id="Label1" Font-Names="Verdana" Font-Size="8pt" Font-Bold="True" Runat="server">Select Domain </asp:Label>
														<asp:DropDownList id="Ddldomain" Width="100px" Runat="server" CssClass="txtNoFocus" AutoPostBack="True"></asp:DropDownList></TD>
												</TR>
											</TABLE>
										</asp:panel>
									</asp:panel></TD>
							</TR>
							<TR>
								<TD><cc1:collapsiblepanel id="cpnlDBMonitor" runat="server" Width="100%" Height="498px" BorderColor="#f5f5f5"
										Visible="True" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="True" TitleBackColor="Transparent"
										Text="DB Monitor" ExpandImage="../Images/ToggleDown.gif" CollapseImage="../Images/To&#9;ggleUp.gif"
										Draggable="False" BorderWidth="0px" BorderStyle="Solid">
										<DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 480px">
											<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
												<TR>
													<TD style="HEIGHT: 14px">
														<asp:panel id="Panel1" runat="server">
															<asp:panel id="Panel7" runat="server">
																<TABLE height="25">
																	<TR>
																		<TD>
																			<asp:Label id="lblFreq" Font-Names="Verdana" Font-Size="8pt" Font-Bold="True" Runat="server">Select Frequency [Hrs.]</asp:Label>
																			<asp:DropDownList id="DDLFrequency" Height="22px" Width="40px" Runat="server" CssClass="txtNoFocus">
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
																			<asp:DropDownList id="DdlMin_DB" runat="server" Width="40px" CssClass="txtNoFocus">
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
																			<asp:Label id="pg" Width="40px" ForeColor="#0000C0" Font-Names="Verdana" Font-Size="8pt" Font-Bold="True"
																				Runat="server">Page</asp:Label></TD>
																		<TD>
																			<asp:Label id="CurrentPg" runat="server" Height="12px" Width="10px" ForeColor="Crimson" Font-Size="X-Small"
																				Font-Bold="True"></asp:Label></TD>
																		<TD>
																			<asp:Label id="of" ForeColor="#0000C0" Font-Names="Verdana" Font-Size="8pt" Font-Bold="True"
																				Runat="server">of</asp:Label></TD>
																		<TD>
																			<asp:label id="TotalPages" runat="server" Height="12px" Width="10px" ForeColor="Crimson" Font-Size="X-Small"
																				Font-Bold="True"></asp:label></TD>
																		<TD>
																			<asp:imagebutton id="Firstbutton" runat="server" ImageUrl="../Images/next9.jpg" ToolTip="First" AlternateText="First"></asp:imagebutton></TD>
																		<TD width="14">
																			<asp:imagebutton id="Prevbutton" runat="server" ImageUrl="../Images/next99.jpg" ToolTip="Previous"></asp:imagebutton></TD>
																		<TD>
																			<asp:imagebutton id="Nextbutton" runat="server" ImageUrl="../Images/next9999.jpg" ToolTip="Next"></asp:imagebutton></TD>
																		<TD>
																			<asp:imagebutton id="Lastbutton" runat="server" ImageUrl="../Images/next999.jpg" ToolTip="Last"></asp:imagebutton></TD>
																		<TD>
																			<asp:textbox id="txtPageSize" runat="server" Height="14px" Width="24px" Font-Size="7pt" MaxLength="3"></asp:textbox></TD>
																		<TD>
																			<asp:Button id="Button3" runat="server" Height="16px" Width="16px" BorderStyle="None" Text=">"
																				ForeColor="Navy" Font-Size="7pt" Font-Bold="True" ToolTip="Change Paging Size"></asp:Button></TD>
																		<TD></TD>
																		<TD>
																			<asp:Label id="lblrecords" runat="server" Height="12pt" ForeColor="MediumBlue" Font-Names="Verdana"
																				Font-Size="8pt" Font-Bold="True">Total Records</asp:Label></TD>
																		<TD>
																			<asp:Label id="TotalRecods" runat="server" Height="12pt" ForeColor="Crimson" Font-Names="Verdana"
																				Font-Size="8pt" Font-Bold="True"></asp:Label></TD>
																		<TD></TD>
																	</TR>
																</TABLE>
															</asp:panel>
														</asp:panel></TD>
												</TR>
												<TR>
													<TD>
														<DIV style="OVERFLOW: auto; WIDTH: 250%">
															<asp:datagrid id="dgrDBMonitor" BorderWidth="0" BorderColor="#d4d4d4" Runat="server" CssClass="grid"
																AllowPaging="True" PagerStyle-Visible="False" PageSize="18" DataKeyField="RQ_NU9_SQID_PK"
																AutoGenerateColumns="False" CellPadding="0">
																<AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
																<ItemStyle CssClass="GridItem"></ItemStyle>
																<HeaderStyle CssClass="GridFixedHeader"></HeaderStyle>
																<Columns>
																	<asp:BoundColumn Visible="False" DataField="RQ_NU9_SQID_PK"></asp:BoundColumn>
																	<asp:TemplateColumn>
																		<HeaderStyle Font-Bold="True"></HeaderStyle>
																		<HeaderTemplate>
																			<asp:CheckBox ID="CheckAll" OnClick="javascript: return select_deselectAll (this.checked, this.id);"
																				Runat="server"></asp:CheckBox>
																		</HeaderTemplate>
																		<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
																		<ItemTemplate>
																			<asp:CheckBox ID="chkReq1" Runat="server"></asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtDBname_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			DB Name
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT2")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtDBType_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			DB Type
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT3")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtMachine_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Machine
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT12")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtstartDate_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Start Date
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT4")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtEnddate_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			End Date
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT6")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtTime_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Time
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT5")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="Txtuid_s" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																				Enabled="False"></asp:TextBox>
																			UID
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT7")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtPWD_s" Runat="server" CssClass="SearchTxtBox" BackColor="#D4D4D4"
																				Enabled="False"></asp:TextBox>
																			PWD
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT8")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtSource_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Source
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT9")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtAlert_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Alert
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("AM_VC20_Code")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtLimit_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Limit
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT10")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="TxtMB_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			MB/GB
																		</HeaderTemplate>
																		<ItemTemplate>
																			<%# container.dataitem("RQ_VC150_CAT11")%>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn>
																		<HeaderTemplate>
																			<asp:TextBox Width="100%" id="Txtstatus_s" Runat="server" CssClass="SearchTxtBox"></asp:TextBox>
																			Status
																		</HeaderTemplate>
																		<ItemTemplate>
																			<asp:Label ID=lblStatus Runat=server text='<%# container.dataitem("RQ_CH2_STATUS")%>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
																<PagerStyle Visible="False"></PagerStyle>
															</asp:datagrid></DIV>
													</TD>
												</TR>
												<TR>
													<TD>
														<asp:panel id="pnlef" Width="0" Runat="server">
															<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
																<TR>
																	<TD><IMG height="0" src="~/Images/divider.gif" width="21"></TD>
																	<TD>
																		<asp:TextBox id="TxtDB_Name" Width="81px" Runat="server" CssClass="txtNoFocusFE"></asp:TextBox></TD>
																	<TD>
																		<asp:DropDownList id="ddldb_Type" Width="74px" Runat="server" CssClass="txtNoFocusFE"></asp:DropDownList></TD>
																	<TD>
																		<asp:DropDownList id="DdlMachine" Width="111px" Runat="server" CssClass="txtNoFocusFE"></asp:DropDownList></TD>
																	<TD>
																		<UC1:DATESELECTOR id="dtStartdate_DB" runat="server" CssClass="txtNoFocusFE" width="81px"></UC1:DATESELECTOR></TD>
																	<TD>
																		<UC1:DATESELECTOR id="dtEnddate_DB" runat="server" CssClass="txtNoFocusFE" width="81px"></UC1:DATESELECTOR></TD>
																	<TD>
																		<asp:DropDownList id="ddlHours_F" runat="server" Width="41px" CssClass="txtNoFocusFE">
																			<asp:ListItem Value="00">00</asp:ListItem>
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
																		</asp:DropDownList></TD>
																	<TD style="WIDTH: 22px">
																		<asp:DropDownList id="DdlMins_F" runat="server" Width="40px" CssClass="txtNoFocusFE">
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
																		<asp:textbox id="txtUID" runat="server" Width="81px" CssClass="txtNoFocusFE" TextMode="Password"></asp:textbox></TD>
																	<TD>
																		<asp:textbox id="TxtPWD" runat="server" Width="83px" CssClass="txtNoFocusFE" TextMode="Password"></asp:textbox></TD>
																	<TD>
																		<asp:textbox id="TxtSource" runat="server" Width="83px" CssClass="txtNoFocusFE"></asp:textbox></TD>
																	<TD>
																		<asp:DropDownList id="DDLAlert_DB" runat="server" Width="96px" CssClass="txtNoFocusFE"></asp:DropDownList></TD>
																	<TD>
																		<asp:TextBox id="txtLimit" Width="79px" Runat="server" CssClass="txtNoFocusFE" MaxLength="3"></asp:TextBox></TD>
																	<TD>
																		<asp:DropDownList id="ddlmb" runat="server" Width="79px" CssClass="txtNoFocusFE">
																			<asp:ListItem Value="MB">MB</asp:ListItem>
																			<asp:ListItem Value="GB">GB</asp:ListItem>
																		</asp:DropDownList></TD>
																	<TD><IMG height="0" src="~/Images/divider.gif" width="51"></TD>
																</TR>
															</TABLE>
														</asp:panel></TD>
												</TR>
											</TABLE>
										</DIV>
									</cc1:collapsiblepanel></TD>
							</TR>
						</TABLE>
						<!-- *****************************************-->
					</td>
				</tr>
			</table>
			<INPUT type="hidden" name="txthiddenImage"> <INPUT type="hidden" name="txthiddenID">
			<input type="hidden" name="txtMachineInfo">
		</form>
	</body>
</html>
