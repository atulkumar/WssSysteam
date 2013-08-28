<%@ page language="VB" autoeventwireup="false" inherits="SupportCenter_CallView_Task_edit, App_Web_i-czgkd-" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Task Edit</title>
     <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../../DateControl/ION.js"></script>

    <script type="text/javascript">
		
	            var rand_no = Math.ceil(500*Math.random())
	
		        function RefreshAttachment()
		        {
			        //document.Form1.submit();
			        self.opener.Form1.submit();
		        }		
		 		
		        function CheckLength()
		        {
				        var TDLength=document.getElementById('txtSubject').value.length;
				        if ( TDLength>0 )
				        {
					        if ( TDLength > 1000 )
					        {
						        alert('The Task Subject cannot be more than 1000 characters\n (Current Length :'+TDLength+')');
						        return false;
					        }
				        }
				        return true;
		        }
		        
		        function OpenW(a,b,c)
				{							
			        wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Common'+rand_no,500,450);				
				}
				
			    function OpenComm(TaskNo,ActionNo,CompanyID,CallNo)
				{								
				    wopen('comment.aspx?ScrID=329&ID='+ TaskNo + '&tbname=T&CompID='+CompanyID+'&ActionNo='+ActionNo+'&CallNo='+CallNo+'&TaskNo='+ TaskNo,'Comments'+rand_no,500,450);
				    return false;
				}
				
			    function OpenAtt(CompanyID,Callno,TaskNo)
				{	
				    wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=T&VTaskNo=' + TaskNo + '&CompanyID=' + CompanyID + '&CallNo=' + Callno, 'Attachment' + rand_no, 400, 450);
				}
				
				function OpenForms()
				{
					wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno='+document.getElementById('txtCallNo').value+'&tno='+document.getElementById('txtTaskNo').value ,'AttachForms'+rand_no,500,450);							
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
									    if (CheckLength()==true)
									    {
										    document.Form1.txthiddenImage.value=varImgValue;
										    Form1.submit();
									    }
									    return false; 
									     CloseWindow();
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
									    if (CheckLength()==true)
									    {
											    document.Form1.txthiddenImage.value=varImgValue;
											    Form1.submit();
									    }
    								
									    return false; 
										    //CloseWindow();
						    }		
    							
						    if (varImgValue=='Forms')
    						
						    {
    				
								    var ct=document.getElementById('txtCallType').value;
								    var combo = $find('<%=CDDLTaskType.ClientID %>');      
                                    var ttvalue = combo.get_value(); 
    //								var tt=document.getElementById('CDDLTaskType$txtHID').value;
                                    var tt=ttvalue;
								    var cno=document.getElementById('txtCallNo').value;
								    var tno=document.getElementById('txtTaskNo').value;
								    var FormBit='<%=Session("FormBit")%>';
								    if (FormBit == 0)
								    {
									    alert('No form is attached to this Task');
								    }
								    else
								    {
									    wopen('../../ChangeManagement/showCallTaskForm.aspx?ScrID=262&ct='+ct+'&tt='+tt+'&cno='+cno+'&tno='+tno,'FormsJD'+rand_no,500,450);
									    window.close();
								    }
								    return false;
						    }	
    							
						    if (varImgValue=='Reset')
						    {
									    var confirmed
									    confirmed=window.confirm("Do You Want To reset The Page ?");
									    if(confirmed==true)
											    {	
													    Form1.reset();
													    return false;
											    }		

						    }			
				    }			
					
					function callrefresh()
				    {
					    document.Form1.txthiddenImage.value='';
			    	    Form1.submit();
			    	    return false;
				    }
				
				
					function FP_swapImg() 
					{//v1.0
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
                self.opener.Form1.submit();
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
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
        border="0">
        <tr>
            <td>
                <asp:Label ID="lblTitleLabelTaskEdit" runat="server" Width="96px" CssClass="TitleLabel">&nbsp;Task Edit</asp:Label>
            </td>
            <td style="width: 902px">
                &nbsp;<img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../../Images/S2Save01.gif">
                </asp:ImageButton>&nbsp;<asp:ImageButton ID="imgOk" AccessKey="K" runat="server"
                    ToolTip="OK" ImageUrl="../../Images/s1ok02.gif"></asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton
                        ID="imgReset" AccessKey="R" runat="server" ToolTip="Reset" ImageUrl="../../Images/reset_20.gif">
                    </asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgForm" AccessKey="M" runat="server" ToolTip="Form" ImageUrl="../../Images/update.gif">
                </asp:ImageButton>&nbsp;&nbsp;<asp:ImageButton ID="imgClose" AccessKey="L" runat="server"
                    ToolTip="Close" ImageUrl="../../Images/s2close01.gif"></asp:ImageButton>&nbsp;&nbsp;<img
                        title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
            </td>
            <td align="right" width="152" background="../../Images/top_nav_back.gif" height="47">
                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('334','../../');"
                    alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;
            </td>
        </tr>
    </table>
    <table id="Table4" bordercolor="#f5f5f5" cellspacing="5" cellpadding="0" bgcolor="#f5f5f5"
        border="1">
        <tr>
            <td bordercolor="#f5f5f5">
                &nbsp;
            </td>
            <td bordercolor="#f5f5f5">
                <asp:Label ID="lblComment" runat="server" Width="40px" CssClass="FieldLabel">Comment</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="imgComment" ImageUrl="../../Images/comment_Blank.gif" AlternateText="Comment"
                    runat="server"></asp:ImageButton>
            </td>
            <td bordercolor="#f5f5f5">
                <asp:Label ID="lblForm" runat="server" Width="40px" CssClass="FieldLabel">Forms</asp:Label>&nbsp;
                <asp:ImageButton ID="imgForms" ImageUrl="../../Images/Form1.jpg" AlternateText="Comment"
                    runat="server"></asp:ImageButton>
            </td>
            <td bordercolor="#f5f5f5" colspan="0">
                    <asp:Label ID="lblETC" runat="server" CssClass="FieldLabel">ETC</asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="middle" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td valign="middle" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="lblAttach" runat="server" Width="40px" CssClass="FieldLabel">Attachment</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <img class="PlusImageCSS" id="imgAttach" alt="Add Attachment" src="../../Images/Attach15_9.gif"
                    border="0" runat="server" />&nbsp;&nbsp;
            </td>
            <td valign="middle" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="Label1" runat="server" Width="104px" CssClass="FieldLabel">Action Mandatory</asp:Label><asp:CheckBox
                    ID="chkMandatory" runat="server" Checked="True"></asp:CheckBox>
            </td>
            <td valign="middle" bordercolor="#f5f5f5" align="left">
                <asp:TextBox ID="txtETC" runat="server" Width="129px" CssClass="txtNoFocus"
                    MaxLength="8"></asp:TextBox>
            </td>
            </tr>
        <tr>
                <td valign="top" bordercolor="#f5f5f5" align="left" rowspan="1">
                    &nbsp;
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" rowspan="1">
                    <asp:Label ID="lblName3" runat="server" Width="40px" CssClass="FieldLabel">Status</asp:Label><br>
                    <%--    <uc1:CustomDDL ID="CDDLStatus" runat="server" Width="129px"></uc1:CustomDDL>--%>
                    <telerik:RadComboBox ID="CDDLStatus" AllowCustomText="True" runat="server" Width="129px"
                        DropDownWidth="150px" Height="68px" Font-Names="Verdana" Font-Size="7pt" DataTextField="ID"
                        DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Status"
                        EnableTextSelection="true" EnableVirtualScrolling="true">
                    </telerik:RadComboBox>
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left">
                    <asp:Label ID="lblName4" runat="server" Width="85px" CssClass="FieldLabel">Task Type</asp:Label><br>
                    <%--<uc1:CustomDDL ID="CDDLTaskType" runat="server" Width="129px"></uc1:CustomDDL>--%>
                    <telerik:RadComboBox ID="CDDLTaskType" AllowCustomText="True" runat="server" Width="129px"
                        DropDownWidth="150px" Height="68px" Font-Names="Verdana" Font-Size="7pt" DataTextField="Name"
                        DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Task Type"
                        EnableTextSelection="true" EnableVirtualScrolling="true">
                    </telerik:RadComboBox>
                </td>
                <td valign="top" bordercolor="#f5f5f5" align="left" rowspan="1">
                    <asp:Label ID="lblName8" runat="server" CssClass="FieldLabel">Priority</asp:Label><br>
                    <%--<uc1:CustomDDL ID="CDDLPriority" runat="server" Width="129px"></uc1:CustomDDL>--%>
                    <telerik:RadComboBox ID="CDDLPriority" AllowCustomText="True" runat="server" Width="129px"
                        DropDownWidth="120px" Height="68px" Font-Names="Verdana" Font-Size="7pt" DataTextField="ID"
                        DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Priority"
                        EnableTextSelection="true" EnableVirtualScrolling="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
        <tr>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="lblName7" runat="server" Width="85px" CssClass="FieldLabel">Task Owner</asp:Label><br>
                <%-- <uc1:CustomDDL ID="CDDLTaskOwner" runat="server" Width="129px"></uc1:CustomDDL>--%>
                <telerik:RadComboBox ID="CDDLTaskOwner" AllowCustomText="True" runat="server" Width="129px"
                    DropDownWidth="150px" Height="68px" Font-Names="Verdana" Font-Size="7pt" DataTextField="ID"
                    DataValueField="ID" MarkFirstMatch="true" Filter="StartsWith" EmptyMessage="Task Owner"
                    EnableTextSelection="true" EnableVirtualScrolling="true">
                </telerik:RadComboBox>
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="Label6" runat="server" Width="85px" CssClass="FieldLabel">Assigned By</asp:Label><br>
                <asp:TextBox ID="txtAssignBy" runat="server" Width="129px" CssClass="txtNoFocus"
                    MaxLength="8" ReadOnly="True"></asp:TextBox>
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="Label3" runat="server" Width="85px" CssClass="FieldLabel">Agreement</asp:Label><br>
                <uc1:CustomDDL ID="CDDLAgreement" runat="server" Width="129px" Enabled="false"></uc1:CustomDDL>
            </td>
            
        </tr>
        <tr>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="Label5" runat="server" Width="135px" CssClass="FieldLabel">Task Start Date</asp:Label><br>
                   <ION:Customcalendar ID="dtStartDate" runat="server" Width="120px" Height="18px" />
                
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="lblName9" runat="server" Width="135px" CssClass="FieldLabel">Estimated Close Date</asp:Label><br>
                <%--<SCONTROLS:DATESELECTOR id="dtEstFinishDate" runat="server" Text="Start Date:"></SCONTROLS:DATESELECTOR>--%>
                <ION:Customcalendar ID="dtEstFinishDate" runat="server" Width="120px" Height="18px" />
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="Label4" runat="server" Width="85px" CssClass="FieldLabel">Task Order</asp:Label><br>
                <asp:TextBox ID="txttaskorder" runat="server" Width="129px" CssClass="txtTaskOder"
                    Height="18px" BorderWidth="1px" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana"
                    MaxLength="4"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="lblName6" runat="server" Width="85px" CssClass="FieldLabel">Est. Hrs.</asp:Label><br>
                <asp:TextBox ID="txtEstimatedHrs" runat="server" Width="129px" CssClass="txtNoFocus"
                    Height="18px" BorderWidth="1px" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana"
                    MaxLength="8"></asp:TextBox>
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="Label2" runat="server" Width="85px" CssClass="FieldLabel">Dependency</asp:Label><br>
                <asp:DropDownList ID="DDLDependency" runat="server" Width="129px">
                </asp:DropDownList>
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="Label7" runat="server" Width="85px" CssClass="FieldLabel">Task ID</asp:Label><br>
                <asp:TextBox ID="txttaskid" runat="server" Width="129px" CssClass="txtTaskOder" Height="18px"
                    BorderWidth="1px" BorderStyle="Solid" ReadOnly="true" Font-Size="XX-Small" Font-Names="Verdana"
                    MaxLength="4"></asp:TextBox>
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
            </td>
        </tr>
        <tr>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                &nbsp;
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left" colspan="3">
                <asp:Label ID="lblName5" runat="server" Width="40px" CssClass="FieldLabel">Subject</asp:Label><br>
                <asp:TextBox ID="txtSubject" runat="server" Width="430px" CssClass="txtNoFocus" Height="160px"
                    BorderWidth="1px" BorderStyle="Solid" Font-Size="XX-Small" Font-Names="Verdana"
                    MaxLength="950" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <input type="hidden" name="txthiddenImage"><!-- Image Clicked-->
                        <input type="hidden" id="txtTaskNo" runat="server" />
                        <input type="hidden" id="txtCallNo" runat="server" />
                        <input type="hidden" id="txtCallType" runat="server" />
                        </form>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form> 
</body>
</html>
