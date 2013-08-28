<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Action_edit.aspx.vb" Inherits="SupportCenter_CallView_Action_edit" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Action Edit</title>
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

    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
    <link href="../calendar/popcalendar.css" type="text/css" rel="stylesheet" />

    <script src="../../DateControl/ION.js" type="text/javascript"></script>

    <script language="javascript" src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <link href="../../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
		
		var rand_no = Math.ceil(500*Math.random())
		
    function RefreshAttachment()
		{
			self.opener.Form1.submit();
		}		
		 				
	function CheckLength()
		{
				var ADLength=document.getElementById('txtdescription').value.length;
				if ( ADLength>0 )
				{
					if ( ADLength > 2000 )
					{
						alert('The Action Description cannot be more than 2000 characters \n(Current Length :'+ADLength+')');
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
//					alert(TaskNo);//TaskNo
//					alert(ActionNO);//ActionNo
//					alert(CompanyID);//CompanyID
//					alert(CallNo);//CallNo
					
				wopen('comment.aspx?ScrID=329&ID='+ ActionNo + '&tbname=A&CompID='+CompanyID+'&TaskNo='+TaskNo+'&CallNo='+CallNo+'&ActionNo='+ActionNo,'Comments'+rand_no,500,450);
				return false;
			}
				
				
   function OpenAtt(CompanyID,Callno,TaskNo,ActionNo)
		{				
			wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=A&VTaskNo=' + TaskNo + '&CompanyID=' + CompanyID + '&CallNo=' + Callno +'&VActionNo=' + ActionNo,'Attachments'+rand_no,400,450);		
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


	function callrefresh()
				{
					document.Form1.txthiddenImage.value='';
			    	Form1.submit();
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
								if ( CheckLength()==true )
								{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
								}
									return false;
									 CloseWindow()
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
													Form1.reset();
													return false;
											}		

						}			
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
    <!--Added By Atul to make parent window Active after popup win close-->
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

    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" background="../../images/top_nav_back.gif"
        border="0">
        <tr>
            <td>
                &nbsp;<asp:Label ID="lblTitleLabelActionEdit" runat="server" CssClass="TitleLabel">Action Edit</asp:Label>
            </td>
            <td>
                <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">&nbsp;
                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ToolTip="Save" ImageUrl="../../Images/S2Save01.gif">
                </asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgOk" AccessKey="K" runat="server" ToolTip="OK" ImageUrl="../../Images/s1ok02.gif">
                </asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ToolTip="Reset" ImageUrl="../../Images/reset_20.gif">
                </asp:ImageButton>&nbsp;
                <asp:ImageButton ID="imgClose" AccessKey="L" runat="server" ToolTip="Close" ImageUrl="../../Images/s2close01.gif">
                </asp:ImageButton>&nbsp;
                <img title="Seperator" alt="R" src="../../Images/00Seperator.gif" border="0">
            </td>
            <td align="right" width="52" background="../../Images/top_nav_back.gif" height="47">
                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('294','../../');"
                    alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;
            </td>
        </tr>
    </table>
    <table id="Table4" bordercolor="#f5f5f5" cellspacing="0" cellpadding="0" bgcolor="#f5f5f5"
        border="1" width="100%">
        <tr>
            <td bordercolor="#f5f5f5">
                &nbsp;
            </td>
            <td style="width: 422px; height: 21px" bordercolor="#f5f5f5">
                <asp:Label ID="lblName0" runat="server" CssClass="FieldLabel">Comment</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="imgComment" ImageUrl="../../Images/comment_Blank.gif" runat="server"
                    AlternateText="Comment"></asp:ImageButton>
            </td>
            <td style="width: 422px; height: 21px" bordercolor="#f5f5f5" colspan="0">
                <asp:Label ID="lblName2" runat="server" CssClass="FieldLabel">Attachment</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                <img id="imgAttach" class="PlusImageCSS" runat="server" alt="Add Attachment" src="../../Images/Attach15_9.gif"
                    border="0" />
            </td>
        </tr>
        <tr>
            <td bordercolor="#f5f5f5">
                &nbsp;
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="lblName3" runat="server" CssClass="FieldLabel">Action Type</asp:Label><br>
                <uc1:CustomDDL ID="CDDLActionType" runat="server" Width="110px"></uc1:CustomDDL>
            </td>
            <td style="height: 35px" valign="top" bordercolor="#f5f5f5" align="left" width="422"
                height="35" rowspan="1">
                <asp:Label ID="lblName9" runat="server" CssClass="FieldLabel"> ActionDate</asp:Label><br>
                &nbsp;<ION:Customcalendar ID="dtEstFinishDate" runat="server" Width="125px" />
            </td>
        </tr>
        <tr>
            <td bordercolor="#f5f5f5">
                &nbsp;
            </td>
            <td style="height: 40px" valign="top" bordercolor="#f5f5f5" align="left" width="422">
                <asp:Label ID="lblName4" runat="server" CssClass="FieldLabel">Action Owner</asp:Label><br>
                <uc1:CustomDDL ID="CDDLActionOwner" runat="server" Width="110px"></uc1:CustomDDL>
            </td>
            <td style="height: 40px" valign="top" bordercolor="#f5f5f5" align="left">
                <asp:Label ID="lblName6" runat="server" CssClass="FieldLabel" Width="85px">Used Hr.</asp:Label><br>
                <asp:TextBox ID="txtUsedHR" runat="server" CssClass="txtNoFocus" Height="18px" BorderStyle="Solid"
                    BorderWidth="1px" Width="130px" Font-Size="XX-Small" Font-Names="Verdana" MaxLength="8"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td bordercolor="#f5f5f5">
                &nbsp;
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left" width="422">
                <asp:Label ID="Label2" runat="server" CssClass="FieldLabel" Width="85px">Internal/External</asp:Label><br>
                <asp:CheckBox ID="ChkActType" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                    Checked="True"></asp:CheckBox>
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left" width="380" height="21">
                <asp:Label ID="Label1" runat="server" CssClass="FieldLabel" Width="120px">Used Hr. Madatory</asp:Label><br>
                <asp:CheckBox ID="ChkMandatoryHr" runat="server" Height="18px" Font-Size="XX-Small"
                    Font-Names="Verdana" Checked="True"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td bordercolor="#f5f5f5">
                &nbsp;
            </td>
            <td valign="top" bordercolor="#f5f5f5" align="left" width="844" colspan="2">
                <asp:Label ID="lblName5" runat="server" CssClass="FieldLabel">Description</asp:Label><br>
                <asp:TextBox ID="txtdescription" runat="server" CssClass="txtNoFocus" Height="160px"
                    BorderStyle="Solid" BorderWidth="1px" Width="360px" Font-Size="XX-Small" Font-Names="Verdana"
                    MaxLength="500" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthiddenImage">
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
