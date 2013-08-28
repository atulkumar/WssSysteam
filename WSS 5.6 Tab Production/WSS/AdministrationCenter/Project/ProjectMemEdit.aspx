<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProjectMemEdit.aspx.vb"
    Inherits="AdministrationCenter_Project_ProjectMemEdit" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SubCategory Member Edit</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script src="../calendar/popcalendar.js" type="text/javascript"></script>

    <script type="text/javascript">
		//********************************************************************
		
			var gtype;
			var rand_no = Math.ceil(500*Math.random())
		var xmlHttp; 
		var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
		var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
		var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
		//netscape, safari, mozilla behave the same??? 
		var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 

		function DoSubmit()
		{
			if (event.keyCode==13 )
			document.Form1.submit();

		}
		function SetDDL()
		{
			/*var ddlRole=document.getElementById('cpnlMember_DDLRole_F');
			document.Form1.txthiddenRole.value=ddlRole.options(ddlRole.selectedIndex).value;
			var ddlReport=document.getElementById('cpnlMember_DDLReportsTo_F');
			document.Form1.txthiddenReportsTo.value=ddlReport.options(ddlReport.selectedIndex).value;*/
		}
		
		function MemberChange(intcompID)
		{
						xmlHttp=null;
						var ddlMember=document.getElementById('cpnlMember_ddlMember');
						var mem=ddlMember.options(ddlMember.selectedIndex).value;
						var CompID;
						CompID=intcompID;
					   var url= '../../AJAX Server/AjaxInfo.aspx?Type=ROLE_AND_MEMBER&CompID='+ CompID +'&MemberID='+mem+'&Rnd='+Math.random();
						xmlHttp = GetXmlHttpObject(stateChangeHandler);    
						xmlHttp_Get(xmlHttp, url); 
		}
		 
		function stateChangeHandler() 
		 { 	
				 document.getElementById("cpnlMember_DDLRole_F").options.length=1;
				 document.getElementById("cpnlMember_DDLReportsTo_F").options.length=1;
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
													document.getElementById("cpnlMember_DDLRole_F").options.add(objNewOption);
													objNewOption.value = item[inti].getAttribute("COL0");
													objNewOption.innerText = item[inti].getAttribute("COL1");
													DataName=DataName+item[inti].getAttribute("COL1") + '^';
													DataID=DataID+item[inti].getAttribute("COL0") + '^';															
											}
											document.Form1.txthiddenRole.value= DataName + '~' + DataID ;
											break;
										}//case 0
										case 1:
										{
											for (var inti=0; inti<item.length; inti++)
											{
													var objNewOption = document.createElement("OPTION");
													document.getElementById("cpnlMember_DDLReportsTo_F").options.add(objNewOption);
													objNewOption.value = item[inti].getAttribute("COL0");
													objNewOption.innerText = item[inti].getAttribute("COL1");
													DataName=DataName+item[inti].getAttribute("COL1") + '^';
													DataID=DataID+item[inti].getAttribute("COL0") + '^';														
											}
											document.Form1.txthiddenReportsTo.value= DataName + '~' + DataID ;
											break;
										}//case 1
									}//switch
								} //for loop
						
						}//if
						
				}//
				
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
    
		
		
		
		//********************************************************************
			function OpenW(a,b,c)
				{				
					wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Common'+rand_no,500,450);
				}
				
			function OpenComm(a,b)
				{				
					wopen('comment.aspx?ScrID=329&ID='+ b + '&tbname=T' ,'Comments'+rand_no,500,450);				
				}
				
			function OpenAtt()
				{						
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=T','Additional_Address'+rand_no,400,450);				
				}
				
			function OpenAB(c)
				{
					var compType='<%=session("propCompanyType")%>';
				var compID='<%=session("propCompanyID")%>';
				var strQuery;
				
				strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='+"'SCM'"+' ))';
				
				/*
				if (compType=='SCM')
				{
					strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id ';
				}
				else
				{
					strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id='+compID+'  or um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='+"'SCM'"+' ))';
				}
				*/
					wopen('../../Search/Common/PopSearch.aspx?ID='+strQuery+'&tbname=' + c ,'Search'+rand_no,500,450);
				
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


			function addToParentList(Afilename,TbName,strName)
				{
				
					if (Afilename != "" || Afilename != 'undefined')
					{
						varName = TbName + 'Name'
					   //alert(Afilename);
						document.getElementById(TbName).value=Afilename;
						document.getElementById(varName).value=strName;
						aa=Afilename;
					}
					else
					
					{
						document.Form1.txtAB_Type.value=aa;
					}
				}
						
			function CloseWindow()
				{
					self.opener.callrefresh();
				}										
				
			function SaveEdit(varImgValue)
				{			    														
					if (varImgValue=='Close')
						{	
							window.close(); 
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
							// CloseWindow()
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
							return false;		
						}			
						
				}			
					
			function callrefresh()
				{
					document.Form1.txthiddenImage.value='';
			    	Form1.submit();
				}
						
			function FP_swapImg() {//v1.0
					var doc=document,args=arguments,elm,n; doc.$imgSwaps=new Array(); for(n=2; n<args.length;
							n+=2) { elm=FP_getObjectByID(args[n]); if(elm) { doc.$imgSwaps[doc.$imgSwaps.length]=elm;
							elm.$src=elm.src; elm.src=args[n+1]; } }
							}

			function FP_preloadImgs() {//v1.0
							var d=document,a=arguments; if(!d.FP_imgs) d.FP_imgs=new Array();
							for(var i=0; i<a.length; i++) { d.FP_imgs[i]=new Image; d.FP_imgs[i].src=a[i]; }
							}

			function FP_getObjectByID(id,o) {//v1.0
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
        var x = document.getElementById('cpnlMember_collapsible').cells[0].colSpan = "1";
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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
        border="0">
        <tr>
            
      <td style="width: 15%">
                &nbsp;<asp:Label ID="lblTitleLabelPrjMembEdit" runat="server" BorderStyle="None"
                    BorderWidth="2px" Width="192px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True"
                    ForeColor="Teal">SubCategory Member Edit</asp:Label>
            </td>
            <td valign="middle" nowrap="nowrap" bordercolor="#e0e0e0" bordercolorlight="#e0e0e0"
                align="left" bordercolordark="#e0e0e0" align="center" style="width: 85%">
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../../Images/S2Save01.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgOK" AccessKey="K" runat="server" ToolTip="OK" ImageUrl="../../Images/s1ok02.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgClose" AccessKey="O" runat="server" ToolTip="Close" ImageUrl="../../Images/s2close01.gif">
                </asp:ImageButton>
                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('756','../../');"
                    alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <cc1:CollapsiblePanel ID="cpnlMember" runat="server" Height="47px" Width="100%" BorderWidth="0px"
        BorderStyle="Solid" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
        ExpandImage="../../Images/ToggleDown.gif" Text="SubCategory Member" TitleBackColor="Transparent"
        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
        Visible="True" BorderColor="Indigo">
        <asp:Panel ID="PnlMain" Width="368px" Height="128px" HorizontalAlign="Center" runat="server">
            <table id="Table3" style="width: 360px; height: 118px">
                <tr align="left">
                    <td align="left">
                        <asp:Label ID="Label5" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                            Font-Size="XX-Small">Member Name</asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlMember" runat="server" Font-Size="XX-Small" Width="230px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="left">
                    <td align="left">
                        <asp:Label ID="Label1" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                            Font-Size="XX-Small">Role</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="DDLRole_F" runat="server" Width="230px" CssClass="txtNoFocus">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="left">
                    <td align="left">
                        <asp:Label ID="Label2" runat="server" ForeColor="DimGray" Font-Bold="True" Font-Names="Verdana"
                            Font-Size="XX-Small">Reports To</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="DDLReportsTo_F" runat="server" Width="230px" CssClass="txtNoFocus">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </cc1:CollapsiblePanel>
    <asp:UpdatePanel ID="PanelUpdate" runat="server">
        <ContentTemplate>
            <asp:ListBox ID="lstError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"
                Visible="false" Width="343px" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
            <%--<asp:textbox id="txtTaskOwner" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="0"
			BorderWidth="1px" BorderStyle="Solid" Height="0" MaxLength="8"></asp:textbox>--%>
            <input id="txtTaskOwner" runat="server" type="hidden" />
            <input type="hidden" name="txthiddenImage" /><!-- Image Clicked-->
            <input type="hidden" name="txthiddenRole" />
            <input type="hidden" name="txthiddenReportsTo" />
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
