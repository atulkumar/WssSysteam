<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdvSearch.aspx.vb" Inherits="Search_AdvSearch_AdvSearch" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript" src="../../DateControl/ION.js"></script>

    <script language="javascript" type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

    <script language="javascript" type="text/javascript" src="../../Images/Js/ABMainShortCuts.js"></script>

    <style type="text/css">
        .DataGridFixedHeader
        {
            position: relative; ;TOP:expression(this.offsetParent.scrollTop);BACKGROUND-COLOR:#e0e0e0}</style>

    <script type="text/javascript">
		
			var rand_no = Math.ceil(500*Math.random())
	
		
	             function SelectComp()
					{
						var value1;
						var txt;
						value1=document.getElementById('cpnlAdvSearch_ddlCompany').value; 
						txt=document.getElementById('cpnlAdvSearch_txtsearch').value; 
						
						if(value1=="")
								{
								alert('Please Select Company First...');
								document.getElementById('cpnlAdvSearch_ddlcompany').focus();
								document.getElementById('cpnlAdvSearch_txtsearch').value="";  
								return false;
						
								}
								if(txt=="")
								{
									alert('Please enter search string... ');
									document.getElementById('cpnlAdvSearch_txtsearch').focus();
									return false;					
									
								}
							}
				
			
				
				function KeyCheck(callno,grdrowid,cpnlname,Compid,taskno)
					{
						
						globleID = callno;
												
						document.Form1.txthidden.value=callno;
						document.Form1.txthiddenTable.value=cpnlname;
						document.Form1.txtrowvalues.value=grdrowid;
				 
						document.Form1.txtComp.value=Compid;
			           // alert(Compid);
						
						//Form1.submit();
						//var tableID='cpnlCallView_GrdAddSerach'  //your datagrids id
						
					/*	var table;
							//	alert();	
						if (document.all) table=document.all[cpnlname];
							if (document.getElementById) table=document.getElementById(cpnlname);
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
								table.rows [ grdrowid  ] . style . backgroundColor = "#d4d4d4";
							}*/
							
							if (cpnlname =='cpnlcall_grdcall')
							{

							document.Form1.txthiddenImage.value='Select';
							
								setTimeout('Form1.submit();',100);
							return false;
							//Form1.submit(); 
							}
					}	
				
				function SaveEdit(varimgValue)
				{			
						
						if (varimgValue=='Logout')
								{
									document.Form1.txthiddenImage.value=varimgValue;
									Form1.submit(); 
								}	
								
				if (varimgValue=='Reset')
						{
							var confirmed
							confirmed=window.confirm("Do You Want To reset The Page ?");
							if(confirmed==true)
								{	
									Form1.reset()
								}	
							return false;	
						}		
								
															
				}
					
					function KeyCheck55(callno,grdrowid,cpnlname,compid,taskno)
					{
//							//alert(callno);
							
							document.Form1.txthidden.value=callno;
							document.Form1.txthiddenImage.value='Edit';
							document.Form1.txthiddenTable.value=cpnlname;
							document.Form1.txtComp.value=compid;
				
						
							if (cpnlname=='cpnltask_grdtask' || cpnlname=='cpnlaction_grdaction')
							{
						
							document.Form1.txtTaskno.value=taskno; 
							Form1.submit(); 
							OpenCallwindow(taskno,callno,compid);
						
						
							}
							else
							{
							
							Form1.submit(); 
							OpenCallwindow(taskno,callno,compid);
							//return false;
														
							}
												
					}	
				
			function OpenCallwindow(taskno,CallNo,CompanyID)
			{
//				//alert(CallNo);
			wopen('../../SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&SearchID=1&SEARTASKNO='+taskno+ '&CallNumber=' + CallNo + '&CompID=' + CompanyID,'CallDetailSearch'+rand_no,900,600);
			
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
				
			/*	if (varimgValue=='Logout')
					{
						document.Form1.txthiddenImage.value=varimgValue;
						Form1.submit(); 
					    return false;
					}*/
				
				
					
					
    </script>

    <script type="text/javascript">
        //A Function to call on Page Load to set grid width according to screen size
        function onLoad() {
            var divAdvSearch = document.getElementById('divAdvSearch');
            if (divAdvSearch != null) {
                if (document.body.clientWidth > 0) {
                    divAdvSearch.style.width = document.body.clientWidth - 30 + "px";
                }
            }
        }

        //A Function to improve design i.e delete the extra cell of table
        function onEnd() 
        {
                var x = document.getElementById('cpnlAdvSearch_collapsible').cells[0].colSpan = "1";
		        var y = document.getElementById('cpnlAdvSearch2_collapsible').cells[0].colSpan = "1";
		        var z = document.getElementById('cpnlSearch_collapsible').cells[0].colSpan = "1";
		        
		        if(document.getElementById('cpnlSearch_cpnlcallsearch_collapsible')!= null )
		        var a = document.getElementById('cpnlSearch_cpnlcallsearch_collapsible').cells[0].colSpan = "1";
		        
		        if(document.getElementById('cpnlSearch_cpnltasksearch_collapsible')!= null )
		        var b = document.getElementById('cpnlSearch_cpnltasksearch_collapsible').cells[0].colSpan = "1";
		        
		        if (document.getElementById('cpnlSearch_cpnlactionsearch_collapsible')!=null) 
		        var c = document.getElementById('cpnlSearch_cpnlactionsearch_collapsible').cells[0].colSpan = "1";
                
                if(document.getElementById('cpnlSearch_cpnlcomm_collapsible')!=null )
                var d = document.getElementById('cpnlSearch_cpnlcomm_collapsible').cells[0].colSpan = "1";
                
                if (document.getElementById('cpnlSearch_cpnlattachment_collapsible')!=null )
                var e = document.getElementById('cpnlSearch_cpnlattachment_collapsible').cells[0].colSpan = "1";
                
                if(document.getElementById('cpnlSearch_cpnldocument_collapsible')!= null )
                var g = document.getElementById('cpnlSearch_cpnldocument_collapsible').cells[0].colSpan = "1";
          
        }
        
        
        //A Function is Called when we resize window
        window.onresize = onLoad;    
         //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	    
         
    </script>

    <link href="../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(onLoad);     
    </script>

    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    &nbsp;&nbsp;<asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" CommandName="submit"
                                                        AlternateText="." ImageUrl="white.GIF" Width="1px"></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelSearch" runat="server" BorderStyle="None" BorderWidth="2px"
                                                        CssClass="TitleLabel">Search</asp:Label>
                                                </td>
                                                <td style="width: 75%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                            ToolTip="Reset"></asp:ImageButton>
                                                        <!--<asp:imagebutton id="imgSave" accessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif" ToolTip="Save"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgOk" accessKey="K" runat="server" ImageUrl="../../Images/s1ok02.gif" ToolTip="OK"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgEdit" accessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif" ToolTip="Edit"></asp:imagebutton>&nbsp;
															<asp:imagebutton id="imgDelete" accessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"	ToolTip="Delete" Visible="False"></asp:imagebutton>&nbsp;&nbsp;-->
                                                        <asp:ImageButton ID="BtnSearch" AccessKey="H" runat="server" AlternateText="Search"
                                                            ImageUrl="../../Images/s1search02.gif" ToolTip="Search"></asp:ImageButton>
                                                        <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                            style="cursor: hand;" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('45','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;<img
                                                class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--  **********************************************************************-->
                            <div id="divAdvSearch" style="overflow: auto; width: 100%; height: 100%">
                                <table id="Table312" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <cc1:CollapsiblePanel ID="cpnlAdvSearch" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                ExpandImage="../../Images/ToggleDown.gif" Text="Search Criteria" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                <table id="Table3" bordercolor="#5c5a5b" cellspacing="6" cellpadding="0" width="100%"
                                                    border="1">
                                                    <tr>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left">
                                                            <!-- advance serach fields -->
                                                            <font face="Verdana" size="1">
                                                                <asp:Label ID="Label10" runat="server" CssClass="FieldLabel">Company</asp:Label></font>&#160;&#160;&#160;
                                                            <asp:DropDownList ID="ddlCompany" TabIndex="3" Width="129px" CssClass="txtNoFocus"
                                                                runat="server" AutoPostBack="True">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <font face="Verdana" size="1">&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;
                                                                <asp:Label ID="Label9" runat="server" CssClass="FieldLabel">SubCategory</asp:Label>&#160;&#160;
                                                            </font>
                                                            <asp:DropDownList ID="ddlProject" TabIndex="3" Width="129px" CssClass="txtNoFocus"
                                                                runat="server">
                                                            </asp:DropDownList>
                                                            &#160;&#160;&#160;&#160;&#160;&#160;<font face="Verdana" size="1">&#160;&#160; </font>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:Label ID="Label11" runat="server" CssClass="FieldLabel">Enter Text</asp:Label>&#160;
                                                            <asp:TextBox ID="txtsearch" runat="server" Width="360px" CssClass="txtnofocus" ToolTip="Enter Text for Search and Press Enter"
                                                                MaxLength="35"></asp:TextBox><!-- ---------------------- --><font face="Verdana"
                                                                    size="1">&#160;&#160;
                                                                    <!--<asp:Button id="BtnSearch1" runat="server" Text="Search"></asp:Button></FONT>-->
                                                                </font>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                </table>
                                <!-- **********************************************************************-->
                                <table id="Table32" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <cc1:CollapsiblePanel ID="cpnlAdvSearch2" runat="server" Width="100%" BorderStyle="Solid"
                                                BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                ExpandImage="../../Images/ToggleDown.gif" Text="Advance Criteria" TitleBackColor="Transparent"
                                                TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                <table bordercolor="#5c5a5b" cellspacing="6" cellpadding="0" width="100%" border="1">
                                                    <tr>
                                                        <td bordercolor="#f5f5f5">
                                                            <font face="Verdana" size="1">&nbsp;
                                                                <asp:Label ID="Label5" runat="server" CssClass="FieldLabel">Call From</asp:Label>
                                                                <asp:TextBox ID="TxtCallFrm" runat="server" Width="72px" CssClass="txtnofocus" MaxLength="8"></asp:TextBox>
                                                                <asp:Label ID="Label3" runat="server" CssClass="FieldLabel">To</asp:Label>&nbsp;
                                                                <asp:TextBox ID="TxtCallTo" runat="server" Width="72px" CssClass="txtnofocus" MaxLength="8"></asp:TextBox></font>
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <asp:Label ID="Label2" runat="server" CssClass="FieldLabel">From Date</asp:Label>
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <ION:Customcalendar ID="dtFrom" runat="server" Width="120px" />
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            &nbsp;
                                                            <asp:Label ID="Label1" runat="server" CssClass="FieldLabel">To</asp:Label>
                                                        </td>
                                                        <td bordercolor="#f5f5f5">
                                                            <ION:Customcalendar ID="dtTo" runat="server" Width="120px" />
                                                        </td>
                                                        <%--<TD width="25%"></TD>--%>
                                                    </tr>
                                                </table>
                                                <br>
                                                <table id="Table442" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="100%"
                                                    border="1">
                                                    <tr>
                                                        <td style="height: 13px" valign="top" bordercolor="#f5f5f5" align="left" colspan="2">
                                                            <asp:Label ID="Label4" runat="server" CssClass="FieldLabel">Call Level Search</asp:Label>
                                                        </td>
                                                        <td style="height: 13px" valign="top" bordercolor="#f5f5f5" align="left" colspan="1">
                                                            <asp:Label ID="Label6" runat="server" CssClass="FieldLabel">Task Level Search</asp:Label>
                                                        </td>
                                                        <td style="height: 13px" valign="top" bordercolor="#f5f5f5" align="left" colspan="1">
                                                            <asp:Label ID="Label7" runat="server" CssClass="FieldLabel">Action Level Search</asp:Label>
                                                        </td>
                                                        <td style="height: 13px" valign="top" bordercolor="#f5f5f5" align="left" colspan="2">
                                                            <asp:Label ID="Label8" runat="server" CssClass="FieldLabel">Documents</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                            <asp:CheckBox ID="chkcallSubject" runat="server" Text="Subject " Font-Names="Verdana"
                                                                Font-Size="8pt" Checked="True"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="17%">
                                                            <asp:CheckBox ID="chkcalltype" runat="server" Text="Call Type" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="13%">
                                                            <asp:CheckBox ID="chktskdesc" runat="server" Text="Description" Font-Names="Verdana"
                                                                Font-Size="8pt" Checked="True"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="15%">
                                                            <asp:CheckBox ID="chkactdesc" runat="server" Text="Description" Font-Names="Verdana"
                                                                Font-Size="8pt" Checked="True"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="20%">
                                                            <asp:CheckBox ID="Chkfilename" runat="server" Text="File Name" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="19%">
                                                        &nbsp;
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 22px" valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                            <asp:CheckBox ID="chkcalldesc" runat="server" Text="Description" Font-Names="Verdana"
                                                                Font-Size="8pt" Checked="True"></asp:CheckBox>
                                                        </td>
                                                        <td style="height: 22px" valign="top" bordercolor="#f5f5f5" align="left" width="17%">
                                                            <asp:CheckBox ID="chkcallowner" runat="server" Text="Call Owner" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td style="height: 22px" valign="top" bordercolor="#f5f5f5" align="left" width="13%">
                                                            <asp:CheckBox ID="chktskowner" runat="server" Text="Task Owner" Font-Names="Verdana"
                                                                Font-Size="8pt" Checked="True"></asp:CheckBox>
                                                        </td>
                                                        <td style="height: 22px" valign="top" bordercolor="#f5f5f5" align="left" width="15%">
                                                            <asp:CheckBox ID="ChkActOwn" runat="server" Text="Action Owner" Font-Names="Verdana"
                                                                Font-Size="8pt" Checked="True"></asp:CheckBox>
                                                        </td>
                                                        <td style="height: 22px" valign="top" bordercolor="#f5f5f5" align="left" width="18%">
                                                            <asp:CheckBox ID="ChkDesc" runat="server" Text="Description" Font-Names="Verdana"
                                                                Font-Size="8pt" Checked="True"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                            <asp:CheckBox ID="chkcallcomm" runat="server" Text="Comment" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                            <asp:CheckBox ID="chkcallAtt" runat="server" Text="Attachment" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                            <asp:CheckBox ID="chktskcomm" runat="server" Text="Comment" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                            <asp:CheckBox ID="chkactcomm" runat="server" Text="Comment" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="19%">
                                                            <asp:CheckBox ID="chktskatt" runat="server" Text="Attachment" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="18%">
                                                            <asp:CheckBox ID="Chkactatt" runat="server" Text="Attachment" Font-Names="Verdana"
                                                                Font-Size="8pt"></asp:CheckBox>
                                                        </td>
                                                        <td valign="top" bordercolor="#f5f5f5" align="left" width="14%">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </cc1:CollapsiblePanel>
                                        </td>
                                    </tr>
                                </table>
                                <!-- **********************************************************************-->
                                <cc1:CollapsiblePanel ID="cpnlSearch" runat="server" Width="100%" BorderStyle="Solid"
                                    BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                    ExpandImage="../../Images/ToggleDown.gif" Text="Result" TitleBackColor="Transparent"
                                    TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                    <table id="Table423" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td valign="top" align="left" width="24%">
                                                    &nbsp;
                                                    <!--*********************************************************  -->
                                                    <cc1:CollapsiblePanel ID="cpnlcallsearch" runat="server" Width="100%" BorderStyle="Solid"
                                                        BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                        ExpandImage="../../Images/ToggleDown.gif" Text="Call" TitleBackColor="Transparent"
                                                        TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                        <div style="overflow: auto; width: 100%; height: 250px" designtimedragdrop="1529">
                                                            <table id="Table44" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top" align="left" width="24%">
                                                                        &nbsp;
                                                                        <asp:DataGrid ID="grdcall" runat="server" Width="750px" CssClass="Grid" BorderWidth="1px"
                                                                            BorderColor="#5c5a5b" AutoGenerateColumns="False" AllowPaging="True" CellPadding="0"
                                                                            DataKeyField="CM_NU9_Call_No_PK" PageSize="25">
                                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                            <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                            <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                                BackColor="#E0E0E0"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="CM_NU9_Call_No_PK" HeaderText="Call No">
                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CM_VC8_Call_Type" HeaderText="Call Type">
                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CM_VC100_Subject" HeaderText="Subject">
                                                                                    <HeaderStyle Width="100px"></HeaderStyle>
                                                                                    <ItemStyle Width="100px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CM_VC2000_Call_Desc" HeaderText="Call Desc">
                                                                                    <HeaderStyle Width="400px"></HeaderStyle>
                                                                                    <ItemStyle Width="400px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="UM_VC50_UserID" HeaderText="Call Owner">
                                                                                    <HeaderStyle Width="80px"></HeaderStyle>
                                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                            </PagerStyle>
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                    <td valign="top" align="left" width="76%">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </td>
                                                <!-- ***********************************************************************************    -->
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" width="24%">
                                                    &nbsp;
                                                    <!--*********************************************************  -->
                                                    <cc1:CollapsiblePanel ID="cpnltasksearch" runat="server" Width="100%" BorderStyle="Solid"
                                                        BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                        ExpandImage="../../Images/ToggleDown.gif" Text="Task" TitleBackColor="Transparent"
                                                        TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                        <div style="overflow: auto; width: 100%; height: 250px">
                                                            <table id="Table47" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top" align="left" width="24%">
                                                                        &nbsp;
                                                                        <asp:DataGrid ID="grdtask" runat="server" Width="750px" CssClass="Grid" BorderWidth="1px"
                                                                            BorderColor="#5c5a5b" AutoGenerateColumns="False" AllowPaging="True" CellPadding="0"
                                                                            DataKeyField="TM_NU9_Task_no_PK" PageSize="25">
                                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                            <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                            <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                                BackColor="#E0E0E0"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="TM_NU9_Call_No_FK" HeaderText="Call No">
                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TM_NU9_Task_no_PK" HeaderText="Task No">
                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TM_VC8_task_type" HeaderText="Task Type">
                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TM_VC1000_Subtsk_Desc" HeaderText="Task Desc">
                                                                                    <HeaderStyle Width="400px"></HeaderStyle>
                                                                                    <ItemStyle Width="400px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="UM_VC50_UserID" HeaderText="Task Owner">
                                                                                    <HeaderStyle Width="80px"></HeaderStyle>
                                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                            </PagerStyle>
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                    <td valign="top" align="left" width="76%">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </td>
                                                <!-- ***********************************************************************************    -->
                                            </tr>
                                            <!-- ***********************************************************************************    -->
                                            <tr>
                                                <td valign="top" align="left" width="24%">
                                                    &nbsp;
                                                    <!--*********************************************************  -->
                                                    <cc1:CollapsiblePanel ID="cpnlactionsearch" runat="server" Width="100%" BorderStyle="Solid"
                                                        BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                        ExpandImage="../../Images/ToggleDown.gif" Text="Action" TitleBackColor="Transparent"
                                                        TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                        <div style="overflow: auto; width: 100%; height: 250px">
                                                            <table id="Table34" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top" align="left" width="24%">
                                                                        &nbsp;
                                                                        <asp:DataGrid ID="grdaction" runat="server" Width="750px" CssClass="Grid" BorderWidth="1px"
                                                                            BorderColor="#5C5A5B" AutoGenerateColumns="False" AllowPaging="True" CellPadding="0"
                                                                            PageSize="25">
                                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                            <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                            <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                                BackColor="#E0E0E0"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="AM_NU9_Call_Number" HeaderText="Call No">
                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AM_NU9_Task_Number" HeaderText="Task No">
                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AM_NU9_Action_Number" HeaderText="Action No">
                                                                                    <HeaderStyle Width="60px"></HeaderStyle>
                                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AM_VC_2000_Description" HeaderText="Action Desc">
                                                                                    <HeaderStyle Width="400px"></HeaderStyle>
                                                                                    <ItemStyle Width="400px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="UM_VC50_UserID" HeaderText="Act Owner">
                                                                                    <HeaderStyle Width="80px"></HeaderStyle>
                                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                            </PagerStyle>
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                    <td valign="top" align="left" width="76%">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </td>
                                                <!-- ***********************************************************************************    -->
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" width="24%">
                                                    &nbsp;
                                                    <!--*********************************************************  -->
                                                    <cc1:CollapsiblePanel ID="cpnlcomm" runat="server" Width="100%" BorderStyle="Solid"
                                                        BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                        ExpandImage="../../Images/ToggleDown.gif" Text="Comment" TitleBackColor="Transparent"
                                                        TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                        <div style="overflow: auto; width: 100%; height: 250px">
                                                            <table id="Table46" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top" align="left" width="24%">
                                                                        &nbsp;
                                                                        <asp:DataGrid ID="grdcomm" runat="server" Width="750px" CssClass="Grid" BorderWidth="1px"
                                                                            BorderColor="#5C5A5B" AutoGenerateColumns="False" AllowPaging="True" CellPadding="0"
                                                                            DataKeyField="CM_NU9_Comment_Number_PK" PageSize="25">
                                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                            <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                            <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                                BackColor="#E0E0E0"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="CM_NU9_Call_Number" HeaderText="Call no">
                                                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CM_NU9_Task_Number" HeaderText="Task no">
                                                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CM_NU9_Action_Number" HeaderText="Action no">
                                                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CM_DT8_Date" HeaderText="Comm Date">
                                                                                    <HeaderStyle Width="80px"></HeaderStyle>
                                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CM_VC256_Comments" HeaderText="Comment">
                                                                                    <HeaderStyle Width="300px"></HeaderStyle>
                                                                                    <ItemStyle Width="300px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="UM_VC50_UserID" HeaderText="Comm User">
                                                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn HeaderText="Comm Level">
                                                                                    <HeaderStyle Width="70px"></HeaderStyle>
                                                                                    <ItemStyle Width="70px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                            </PagerStyle>
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                    <td valign="top" align="left" width="76%">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </td>
                                                <!-- ***********************************************************************************    -->
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" width="24%">
                                                    &nbsp;
                                                    <!--*********************************************************  -->
                                                    <cc1:CollapsiblePanel ID="cpnlattachment" runat="server" Width="100%" BorderStyle="Solid"
                                                        BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                        ExpandImage="../../Images/ToggleDown.gif" Text="Attachment" TitleBackColor="Transparent"
                                                        TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                        <div style="overflow: auto; width: 100%; height: 250px">
                                                            <table id="Table471" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="top" align="left" width="24%">
                                                                        <asp:DataGrid ID="grdatt" runat="server" Width="750px" CssClass="Grid" BorderWidth="1px"
                                                                            BorderColor="#5C5A5B" AutoGenerateColumns="False" AllowPaging="True" CellPadding="0"
                                                                            PageSize="25">
                                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                            <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                            <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                                BackColor="#E0E0E0"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="VH_NU9_Call_Number" HeaderText="Call no">
                                                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="VH_NU9_Task_Number" HeaderText="Task no">
                                                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="VH_NU9_Action_Number" HeaderText="Action no">
                                                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="VH_DT8_Date" HeaderText="Att Date">
                                                                                    <HeaderStyle Width="80px"></HeaderStyle>
                                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="VH_VC255_File_Name" HeaderText="Att Name">
                                                                                    <HeaderStyle Width="300px"></HeaderStyle>
                                                                                    <ItemStyle Width="300px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="UM_VC50_UserID" HeaderText="Att User">
                                                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn HeaderText="Att Level">
                                                                                    <HeaderStyle Width="70px"></HeaderStyle>
                                                                                    <ItemStyle Width="70px"></ItemStyle>
                                                                                </asp:BoundColumn>
                                                                            </Columns>
                                                                            <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                            </PagerStyle>
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                    <td valign="top" align="left" width="76%">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </td>
                                                <!-- ***********************************************************************************    -->
                                            </tr>
                                            <td valign="top" align="left" width="24%">
                                                &nbsp;
                                                <!--*********************************************************  -->
                                                <cc1:CollapsiblePanel ID="cpnldocument" runat="server" Width="100%" BorderStyle="Solid"
                                                    BorderWidth="0px" BorderColor="Indigo" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                    ExpandImage="../../Images/ToggleDown.gif" Text="Document" TitleBackColor="Transparent"
                                                    TitleClickable="True" TitleForeColor="PowderBlue" PanelCSS="panel" TitleCSS="test">
                                                    <div style="overflow: auto; width: 100%; height: 250px">
                                                        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td valign="top" align="left" width="24%">
                                                                    <asp:DataGrid ID="grddoc" runat="server" Width="750px" CssClass="Grid" BorderWidth="1px"
                                                                        BorderColor="#5C5A5B" AutoGenerateColumns="False" AllowPaging="True" CellPadding="0"
                                                                        PageSize="25">
                                                                        <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                        <AlternatingItemStyle ForeColor="Silver" CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                        <ItemStyle Font-Size="8pt" Font-Names="Verdana" CssClass="griditem"></ItemStyle>
                                                                        <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BorderColor="White"
                                                                            BackColor="#E0E0E0"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundColumn DataField="FI_VC255_File_Name" HeaderText="File Name">
                                                                                <HeaderStyle Width="100px"></HeaderStyle>
                                                                                <ItemStyle Width="100px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="FI_VC500_File_Description" HeaderText="Description">
                                                                                <HeaderStyle Width="300px"></HeaderStyle>
                                                                                <ItemStyle Width="300px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="FI_DT8_Upload_ON" HeaderText="Uploaded On">
                                                                                <HeaderStyle Width="50px"></HeaderStyle>
                                                                                <ItemStyle Width="50px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="CI_VC36_Name" HeaderText="Uploaded By">
                                                                                <HeaderStyle Width="50px"></HeaderStyle>
                                                                                <ItemStyle Width="50px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Fname" HeaderText="Folder Path">
                                                                                <HeaderStyle Width="50px"></HeaderStyle>
                                                                                <ItemStyle Width="50px"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                        </Columns>
                                                                        <PagerStyle Font-Size="12px" HorizontalAlign="Center" Position="TopAndBottom" Mode="NumericPages">
                                                                        </PagerStyle>
                                                                    </asp:DataGrid>
                                                                </td>
                                                                <td valign="top" align="left" width="76%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </cc1:CollapsiblePanel>
                                            </td>
                                        </tbody>
                                    </table>
                                </cc1:CollapsiblePanel>
                                <!-- **********************************************************************-->
                                <!-- **********************************************************************-->
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="100px" BorderWidth="0" BorderStyle="Groove"
                            ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txtrowvalues" />
                        <input type="hidden" name="txthiddenCallNo" />
                        <input type="hidden" name="txthiddenTable" />
                        <input type="hidden" name="txtComp" />
                        <input type="hidden" name="txtByWhom" />
                        <input type="hidden" name="txtTaskno" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
