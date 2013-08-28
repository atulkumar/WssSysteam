<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TemplateTask_Edit.aspx.vb"
    Inherits="AdministrationCenter_Template_TemplateTask_Edit" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Template Task Edit</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../../images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../../images/js/drag.js" type="text/javascript"></script>

    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../../DateControl/ION.js"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet">

    <script language="Javascript">
		
	var rand_no = Math.ceil(500*Math.random())			
		function RefreshAttachment()
		{
			//document.Form1.submit();
			self.opener.Form1.submit();
		}		
		 		
		function CheckLength()
		{
				var TSLength=document.getElementById('txtSubject').value.length;
				
				if ( TSLength>0 )
				{
					if ( TSLength >1000 )
					{
						alert('TheTask Subject cannot be more than 1000 characters\n (Current Length :'+TSLength+')');
						return false;
					}
				}
				
				return true;
		}
		
		function OpenW(a,b,c)
				{
				
				//wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Comment',500,450);
				//window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
				//window.showModalDialog
			wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Common'+rand_no,500,450);
				}
				
				function OpenComm(a,b,c)
				{
				
				wopen('../../SupportCenter/Callview/comment.aspx?ScrID=329&ID='+ b + '&tbname=T&OPT=2&Comp='+c ,'Comments'+rand_no,500,450);
				return false;
				}
								
				function OpenAtt()
				{
						
					wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=T&OPT=2','Additional_Address'+rand_no,400,450);
				
				}
		function OpenAB(c)
				{
					wopen('../../Search/Common/PopSearch.aspx?ID=select UM_IN4_Address_No_FK as ID,UM_VC50_UserID as UserID,UM_VC4_Status_Code_FK as Status from T060011' + '  &tbname=' + c ,'Common'+rand_no,500,450);
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName,UL_VC8_Role_PK as Role from T010061' + '  &tbname=' + c ,'',500,450);										
					//wopen('../../Search/Common/PopSearch.aspx?ID=select UL_NU8_Address_No_FK as ID,UL_VC36_User_Name_PK as UserName, ab.CI_VC36_Name as Name, UL_VC8_Role_PK as Role from T010011 ab, T010061 ul where ab.CI_NU8_Address_Number=ul.UL_NU8_Address_No_FK' + '  &tbname=' + c ,'',500,450);					
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
				

						
			
				
				
					function SaveEdit(varImgValue,TaskNo)
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
					
					if ( CheckLength()==true )
									{
											document.Form1.txthiddenImage.value=varImgValue;
											Form1.submit(); 
									        CloseWindow()
									}		
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
									if ( CheckLength()==true )
									{
									document.Form1.txthiddenImage.value=varImgValue;
							        Form1.submit();
									}		
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
						
						if (varImgValue=='Forms')
						{
							var tempID;
							tempID='<%=Request.QueryString("TemplateID")%>';
							//var tno='<%=Request.QueryString("PropTaskNumber") %>';
							var tno=TaskNo;
						    wopen('../../ChangeManagement/showCallTaskForm.aspx?tno='+ tno +'&TemplateID='+ tempID +'&ScrID=262&OPT=2','FormsJD'+rand_no,500,450);
							//window.close();
							return false;
						}		
				return false;
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
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                var opener = window.opener;
                if(opener.parent.HideModalDiv)
                opener.parent.HideModalDiv();
                self.opener.callrefresh();
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
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px; background-color: #F5F5F5">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerPage" runat="server">
    </asp:ScriptManager>
    <table id="Table1" bordercolor="activeborder" cellspacing="0" cellpadding="0" width="100%"
        background="../../images/top_nav_back.gif" border="0">
        <tr>
            <td style="width: 151px">
                &nbsp;
                <asp:Label ID="lblTitleLabelTempTaskEdit" runat="server" BorderStyle="None" BorderWidth="2px"
                    Width="144px" Font-Size="X-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Teal">Template Task Edit</asp:Label>
            </td>
            <td align="center">
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../../Images/S2Save01.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ToolTip="Ok" ImageUrl="../../Images/s1ok02.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ToolTip="Reset" ImageUrl="../../Images/reset_20.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgForm" AccessKey="M" runat="server" ToolTip="Form" ImageUrl="../../Images/update.gif">
                </asp:ImageButton>
                <asp:ImageButton ID="imgClose" AccessKey="S" runat="server" ToolTip="Close" ImageUrl="../../Images/s2close01.gif">
                </asp:ImageButton>
            </td>
            <td width="42" background="../../images/top_nav_back.gif" height="47">
                &nbsp;
            </td>
        </tr>
    </table>
    <cc1:CollapsiblePanel ID="cpnlError" runat="server" BorderStyle="Solid" BorderWidth="0px"
        Width="100%" BorderColor="Indigo" Height="51px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
        ExpandImage="../../Images/ToggleDown.gif" Text="Message" TitleBackColor="Transparent"
        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
        Visible="False">
        <table id="Table2" style="width: 272px; height: 19px" bordercolor="lightgrey" cellspacing="0"
            cellpadding="0" width="272" border="0">
            <tr>
                <td style="width: 32px" colspan="0" rowspan="0">
                    <asp:Image ID="Image1" runat="server" Width="16px" ImageUrl="../../icons/warning.gif"
                        Height="16px"></asp:Image>
                </td>
                <td style="width: 246px" colspan="0" rowspan="0">
                    <asp:ListBox ID="lstError" runat="server" ForeColor="Red" Font-Names="Verdana" Font-Size="XX-Small"
                        Width="352px" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
                </td>
            </tr>
        </table>
    </cc1:CollapsiblePanel>
    <table id="Table4" bordercolor="#f5f5f5" cellspacing="0" cellpadding="0" width="400"
        bgcolor="#f5f5f5" border="1">
        <tr>
            <td style="width: 340px; height: 21px" bordercolor="#f5f5f5">
                &nbsp;
            </td>
            <td style="width: 422px; height: 21px" bordercolor="#f5f5f5">
                <asp:Label ID="lblName0" runat="server" Width="40px" Font-Size="XX-Small" Font-Names="Verdana"
                    Font-Bold="True" ForeColor="DimGray">Comment</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="imgComment" ImageUrl="../../Images/comment_Blank.gif" AlternateText="Comment"
                    runat="server"></asp:ImageButton>
            </td>
            <td style="width: 422px; height: 21px" bordercolor="#f5f5f5" colspan="0">
            </td>
        </tr>
        <tr>
            <td style="width: 340px" valign="middle" bordercolor="#f5f5f5" align="left" width="340">
                &nbsp;
            </td>
            <td valign="middle" bordercolor="#f5f5f5" align="left" width="422">
                <asp:Label ID="lblName2" runat="server" Width="40px" Font-Size="XX-Small" Font-Names="Verdana"
                    Font-Bold="True" ForeColor="DimGray">Attachment</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <img class="PlusImageCSS" onclick="OpenAtt();" alt="Attachment" src="../../Images/Attach15_9.gif"
                    border="0">&nbsp;&nbsp;
            </td>
            <tr>
                <td style="width: 340px" valign="top" bordercolor="#f5f5f5" align="left" width="340"
                    height="24" rowspan="1">
                    &nbsp;
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" width="422" rowspan="1">
                    <asp:Label ID="lblName3" runat="server" Width="40px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray">Status</asp:Label><br>
                    <uc1:CustomDDL ID="CDDLStatus" runat="server" Width="129px"></uc1:CustomDDL>
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" width="422">
                    <asp:Label ID="Label2" runat="server" Width="85px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray" Height="12px">Dependency</asp:Label><br>
                    <asp:DropDownList ID="DDLDependancy" runat="server" Width="129px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 340px" valign="top" bordercolor="#f5f5f5" align="left" width="340"
                    height="21">
                    &nbsp;
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" width="422">
                    <asp:Label ID="lblName4" runat="server" Width="85px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray" Height="12px">Task Type</asp:Label><br>
                    <uc1:CustomDDL ID="CDDLTaskType" runat="server" Width="129px"></uc1:CustomDDL>
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" width="422">
                    <asp:Label ID="lblName7" runat="server" Width="85px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray" Height="12px">Task Owner</asp:Label><br>
                    <uc1:CustomDDL ID="CDDLTaskOwner" runat="server" Width="129px"></uc1:CustomDDL>
                </td>
            </tr>
            <tr>
                <td style="width: 340px" valign="top" bordercolor="#f5f5f5" align="left" width="340"
                    height="21">
                    &nbsp;
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" width="422" height="24" rowspan="1">
                    <asp:Label ID="lblName6" runat="server" Width="85px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray" Height="12px">Est. Hrs.</asp:Label><br>
                    <asp:TextBox ID="txtEstimatedHrs" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        Width="129px" Font-Size="XX-Small" Font-Names="Verdana" Height="18px" MaxLength="8"
                        CssClass="txtNoFocus"></asp:TextBox>
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" width="422">
                    <asp:Label ID="Label3" runat="server" Width="85px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray" Height="12px">Agreement</asp:Label><br>
                    <uc1:CustomDDL ID="CDDLAgreement" runat="server" Width="129px"></uc1:CustomDDL>
                </td>
            </tr>
            <tr>
                <td style="width: 340px" valign="top" bordercolor="#f5f5f5" align="left" width="340"
                    height="21">
                    &nbsp;
                </td>
                <td valign="middle" bordercolor="#f5f5f5" align="left" width="422">
                    <asp:Label ID="Label5" runat="server" Width="108px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray" Height="12px">Task ID</asp:Label><br />
                    <asp:TextBox ID="txttaskid" runat="server" Width="129px" CssClass="txtTaskOder" Height="18px"
                        BorderWidth="1px" BorderStyle="Solid" ReadOnly="true" Font-Size="XX-Small" Font-Names="Verdana"
                        MaxLength="4"></asp:TextBox>
                </td>
                <td valign="middle" bordercolor="#f5f5f5" align="left" width="422">
                    <asp:Label ID="Label1" runat="server" Width="108px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray" Height="12px">Action Mandatory</asp:Label><asp:CheckBox
                            ID="chkMandatory" runat="server" Checked="True"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td style="width: 340px" valign="top" bordercolor="#f5f5f5" align="left" width="340"
                    height="21">
                    &nbsp;
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" width="422" height="24" rowspan="1">
                    <asp:Label ID="Label4" runat="server" Width="85px" Font-Size="XX-Small" Font-Names="Verdana"
                        Font-Bold="True" ForeColor="DimGray" Height="12px">Task Order</asp:Label><br>
                    <asp:TextBox ID="txttaskorder" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        Width="129px" Font-Size="XX-Small" Font-Names="Verdana" Height="18px" MaxLength="8"
                        CssClass="txtNoFocus"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 340px" valign="top" bordercolor="#f5f5f5" align="left" width="340"
                    height="21">
                    &nbsp;
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" width="844" colspan="2">
                    <p>
                        <asp:Label ID="lblName5" runat="server" Width="40px" Font-Size="XX-Small" Font-Names="Verdana"
                            Font-Bold="True" ForeColor="DimGray">Subject</asp:Label><br>
                        <asp:TextBox ID="txtSubject" runat="server" BorderStyle="Solid" BorderWidth="1px"
                            Width="383px" Font-Size="XX-Small" Font-Names="Verdana" Height="72px" MaxLength="100"
                            CssClass="txtNoFocus" TextMode="MultiLine"></asp:TextBox></p>
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
