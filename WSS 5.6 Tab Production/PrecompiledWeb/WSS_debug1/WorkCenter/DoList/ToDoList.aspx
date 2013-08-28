<%@ page language="VB" autoeventwireup="false" validaterequest="false" inherits="ToDoList, App_Web_hld7bpp5" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register Assembly="IONCalendar" Namespace="IONCalendar" TagPrefix="ION" %>
<%@ Register TagPrefix="SControls" TagName="DateSelector" Src="../../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>To Do List</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
     <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript"/>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>

    <script type="text/javascript" src="../../DateControl/ION.js"></script>

    <script type="text/javascript" src="../../Images/Js/JSValidation.js"></script>

    <script type="text/javascript" src="../../Images/Js/ToDoListShortCuts.js"></script>
  
    <script src="../../Images/Js/PageLoader.js" type="text/javascript"></script>
  
    <link href="../../SupportCenter/calendar/popcalendar.css" type="text/css" rel="stylesheet" />
    
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />
         

     <script type="text/javascript">
        
        //A Function to call on Page Load to set grid width according to screen size
    function onLoad() 
        {
            var divTaskView = document.getElementById('divTaskView');
            divTaskView.style.width = document.body.clientWidth - 30 + "px";
        }
        //A Function to improve design i.e delete the extra cell of table
   function onEnd() 
        {
        var x = document.getElementById('cpnlTaskView_collapsible').cells[0].colSpan = "1";
        var y = document.getElementById('cpnlCallTask_collapsible').cells[0].colSpan = "1";
       // var a = document.getElementById('cpnlCallTask_cpnlTaskList_collapsible').cells[0].colSpan = "1";
        var Z = document.getElementById('cpnlTaskAction_collapsible').cells[0].colSpan = "1";
       
        } 
         //A Function is Called when we resize window
        window.onresize = onLoad;    
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
//          prm.add_initializeRequest(InitializeRequest);
//          prm.add_endRequest(EndRequest);
          prm.add_pageLoaded(onLoad);     
    </script>
    
    <script type="text/javascript">
 
	var globleID;
	var rand_no = Math.ceil(500*Math.random())

	function CheckLength()
		    {
				var ADLength=document.getElementById('cpnlTaskAction_Txtdescription_F').value.length;
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
			
	function ShowDateTimePopup(sender, eventArgs)
          {
             var picker = $find("<%# dtCallStartDate.ClientID %>");
             var userChar = eventArgs.get_keyCharacter();
             if (userChar == '@')
             {
             picker.showPopup();
             eventArgs.set_cancel(true);
             }
             else if (userChar == '#')
             {
             picker.showTimePopup();
             eventArgs.set_cancel(true);
             }
          }
	
	function ShowActions()
			{
			wopen('../../MessageCenter/UploadFiles/UploadAttachment.aspx?ID=A', 'Attachments'+rand_no, 500, 450)
			return false;
			}					
					
	function ChangeHeight(txt,id)
				{
					
				var n=document.getElementById(id).value.length;
				if ( n<60 )
					{
					document.getElementById(id).runtimeStyle.height=18;
					document.Form1.txtHIDSize.value="18";
					}
				if ( n>60 && n<120 )
					{
					document.getElementById(id).runtimeStyle.height=30;
					document.Form1.txtHIDSize.value="30";
					}
				if ( n>120 && n<180)
					{
					document.getElementById(id).runtimeStyle.height=42;
					document.Form1.txtHIDSize.value="42";
					}
				if ( n>180 && n<240)
					{
					document.getElementById(id).runtimeStyle.height=55;
					document.Form1.txtHIDSize.value="55";
					}
				if ( n>240 && n<300)
					{
					document.getElementById(id).runtimeStyle.height=68;
					document.Form1.txtHIDSize.value="68";
					}
				if ( n>300 && n<360)
					{
					document.getElementById(id).runtimeStyle.height=81;
					document.Form1.txtHIDSize.value="81";
					}
				if ( n>360 && n<420)
					{
					document.getElementById(id).runtimeStyle.height=94;
					document.Form1.txtHIDSize.value="94";
					}
				if ( n>420 && n<480)
					{
					document.getElementById(id).runtimeStyle.height=107;
					document.Form1.txtHIDSize.value="107";
					}
				if ( n>540 && n<600)
					{
					document.getElementById(id).runtimeStyle.height=120;
					document.Form1.txtHIDSize.value="120";
					}
				if ( n>600)
					{
					document.getElementById(id).runtimeStyle.height=133;
					document.Form1.txtHIDSize.value="133";
					}
				}
				
				
	function KeyViewImage(taskno,rowid,cpnlname,wopenmod,comp,CallNo)
			{
			if (wopenmod==0 ) //if comment is clicked
			    {
			        wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=329&ActionNo=0&CompId=' + comp + '&CallNo=' + CallNo + '&TaskNo=' + taskno + '&tbname=T', 'Comment' + rand_no, 500, 450);
				}
			else if (wopenmod==2)//if Attachment is clicked
				{
		    	wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=T&TaskNo='+taskno+'&CompID='+comp+'&CallNo='+CallNo ,'Attachment'+rand_no,700,240);
	    		}
			else
				{
				wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno='+CallNo+'&tno='+taskno+'&CompID='+comp ,'AttachForms'+rand_no,500,450);							
				}
			}
				
				
	function KeyImage(a,b,c,d, CallNo, TaskNo, CompID)
			{
//		   var CallNo = document.Form1.txthiddenCallNo.value;
//		   var TaskNo = document.Form1.txtTask.value;
//		   var CompID = document.Form1.txtComp.value;
		   		  	      
		    if (d==0 ) //if comment is clicked
				{
				    wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=329&ActionNo=' + a + '&tbname=A&CallNo=' + CallNo + '&TaskNo=' + TaskNo + '&CompID=' + CompID, 'Comment' + rand_no, 500, 450);
			    }
			else//if Attachment is clicked
				{
				wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=A&CompID='+CompID+'&CallNo='+CallNo+'&TaskNo='+TaskNo+'&ACTIONNO='+ a,'Attachment'+rand_no,700,240);
				}
			}
			
	function KeyImageTask(CompID,CallNo,TaskNo,ActionNo,Name,d)
				{					
		        if (d==0 ) //if comment is clicked
			        {				     
			        if (CallNo > 0 && CompID > 0)
			            wopenComment('../../SupportCenter/Callview/comment.aspx?ScrID=329&ActionNo=' + ActionNo + '&CompId=' + CompID + '&CallNo=' + CallNo + '&TaskNo=' + TaskNo + '&tbname=' + Name, 'Comment' + rand_no, 500, 450);
			        }
		            else if (d==1) //if Attachment is clicked
		            {
		            wopen("../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=T&CompId=" + CompID + "&CallNo=" + CallNo + "&TaskNo=" + TaskNo + "&ACTIONNO=" + ActionNo,'Attachment'+rand_no,700,240)
		            }
		            else // if Attach form is clicked
		            {
		            wopen('../../ChangeManagement/ShowCallTaskForm.aspx?cno='+a+'&tno='+a ,'AttachForms'+rand_no,500,450);							
		            }
		            return false;
				}
		
    function CallAttach(P)
			    {
				if ( P > 0 )
				{
				 var CompID = document.Form1.txtComp.value;
                 var CallNo= document.Form1.txthiddenCallNo.value;
				wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=C&CompID=' + CompID + '&CallNo=' + CallNo,'Attachments'+rand_no,700,240);
				}
				else
				{
				alert('No attachment Uploaded with this calll');
				}
				return false;
			}
								
			
	function OpenW(a,b,c)
				{
				wopen('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search'+rand_no,500,450);
				}

	function addToParentList(Afilename,TbName,strName)
			{
				if (Afilename != "" || Afilename != 'undefined')
				{
				varName = TbName + 'Name'
				document.getElementById(TbName).value=Afilename;
				document.getElementById(varName).value=strName;
				aa=Afilename;
				}
				else
				{
				document.Form1.txtAB_Type.value=aa;
				}
			}
    
    function OpenAB(c)
			{
				var compType='<%=session("propCompanyType")%>';
				var compID='<%=session("propCompanyID")%>';
				var strQuery;
				if (compType=='SCM')
				{
				strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id ';
				}
				else
				{
				strQuery='SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id='+compID+'  or um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='+"'SCM'"+' ))';
				}
				wopen('../../Search/Common/PopSearch.aspx?ID='+strQuery+'&tbname=' + c ,'Search'+rand_no,500,450);
				return false;
			}
				
    function formReload()
			{
			self.location.href='../../Workcenter/dolist/ToDoList.aspx';
			}					
			
	function callrefresh()
				{
				document.Form1.txthiddenImage.value='';
				//Form1.submit();
				__doPostBack("upnlTaskView","");
				//formReload();
				return false;
				}
								
	function ConfirmDelete(varImgValue)
			{
				  if (document.Form1.txthiddentable.value == 'cpnlTaskView_GrdAddSerach')
				  {
				    if (document.Form1.txtTask.value==0)
					{
					alert("Please select the row");
					}
					else
					{
					alert("You cannot delete the Task");
					}
					return false;
				    }
				    else
				    {
					if (document.Form1.txtTask.value==0)
					{
					alert("Please select the row");
					}
					else
					{
					var strStatus;
					strStatus=document.Form1.txtTaskStatus.value;
				    if ( strStatus=='CLOSED' )
				    {
				    alert('Task is  closed so you cannot delete actions!');
				    }
				    else
			        {
					var confirmed
					confirmed=window.confirm("Are you sure you want to Delete the selected Action ?");
					if(confirmed==false)
					{
					return false;
					}
					else
					{
					document.Form1.txthiddenImage.value=varImgValue;
					Form1.submit(); 
					}
					}		
					return false;
					}
				    }
				    return false;
			 }
			
    function ViewAttachment(P)
			{
				if ( P == 0 )
				{
				alert('No attachment uploaded');
				}
				else if ( P == 1 )
				{
				SaveEdit('Attach');
				}
				else if ( P == -1 )
				{
				alert('Please select a Task');
				}
				return false;
			}
						
				
	function SaveEdit(varImgValue)
				{
			    if (varImgValue=='View')
				    {
				    document.Form1.txthiddenImage.value=varImgValue;
				    document.Form1.txthiddenCallNo.value=0;
				    __doPostBack("upnlTaskView","");
				    //Form1.submit(); 		
					}				
			  	if (varImgValue=='Edit')
					{
					var col=document.Form1.txthiddenCallNo.value;
					var	nn=document.Form1.txtTask.value;
					var	tableID=document.Form1.txthiddentable.value;
					var	Comp=document.Form1.txtComp.value;
					var TaskStatus =document.Form1.txtTaskStatus.value;
//					alert(TaskStatus);
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
					if (document.Form1.txthiddentable.value=="")
					{
					alert("Please select the row");
					}
					else
					{
					//var	Comp='<%=Session("CompName")%>';
					//alert('<%=Session("CompName")%>');
					if (tableID=='cpnlTaskView_GrdAddSerach')
					{
					wopen('../../SupportCenter/CallView/Task_edit.aspx?ScrID=334&ReadOnly=1&CompID='+Comp+'&CallNo='+col+'&TASKNO='+nn,'Search'+rand_no,440,470);
					//	Form1.submit(); 
					//OpenTask(nn,col);
					}
					else if(tableID=='cpnlTaskAction_grdAction')
					{
					var strStatus;
					strStatus=document.Form1.txtTaskStatus.value;
									
					if (strStatus=='CLOSED')
					{
					alert('Task is  closed so you cannot modify action!');
					}
					else
					{
					if ( nn=='' )
					alert('Please select a row');
					else
					wopen('../../SupportCenter/CallView/Action_edit.aspx?ScrID=294&ACTIONNO='+nn,'Search'+rand_no,350,400);
					}		
					}

					return false;
					}
					}	
				if (varImgValue=='Close')
					{
					window.close();	
					}
				if (varImgValue=='Add')
					{
					location.href="../../SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=-1&PageID=3";

	    			}	
				if (varImgValue=='ShowReleased')
		    		{

					document.Form1.txthiddenImage.value=varImgValue;

					__doPostBack("upnlTaskView","");
					return false;
					}													
					if (varImgValue=='Search')
					{
					document.Form1.txthiddenImage.value=varImgValue;

					__doPostBack("upnlTaskView","");
					return false;
					}	
				if (varImgValue=='Select')
					{
					document.Form1.txthiddenImage.value=varImgValue;
					Form1.submit(); 
					return false;
					}	
				if (varImgValue=='Save')
					{
					//Security Block
					var obj=document.getElementById("imgSave")
					if(obj==null)
					{
					alert("You don't have access rights to save record");
					return false;
					}
					if (obj.disabled==true) 
					{
					alert("You don't have access rights to save record");
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
				if (varImgValue=='Logout')
					{
					document.Form1.txthiddenImage.value=varImgValue;
					Form1.submit(); 
					return false;
					}	
				if (varImgValue=='CloseTask')
					{
					if (	document.Form1.txtrowvalues.value==0)
					{
					alert("Please select the row");
	    			}
		    		else
					{	
					var strStatus;
					strStatus=document.Form1.txtTaskStatus.value;
				    if ( strStatus=='CLOSED' )
				    {
				    alert('Task is already closed');
				    }
				    else
			        {
					var confirmed
					confirmed=window.confirm("Are you sure you want to Close the selected Task ?");
					if(confirmed==true)
					{
					document.Form1.txthiddenImage.value=varImgValue;
					document.Form1.txtrowvaluesCall.value =0;  
					Form1.submit(); 
					}
					}
					}
					return false;
				    }	
				if (varImgValue=='Attach')
					{
					//	alert();
					if (document.Form1.txtTask.value==0)
					{
					alert("Please select a task");
					return false;
					}
					else
					{
				   var CallNo = document.Form1.txthiddenCallNo.value;
		           var CompID = document.Form1.txtComp.value;
					wopen('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CompID=' + CompID + '&CallNo=' + CallNo,'Attachments'+rand_no,800,550);

					return false;
					}

					}	
				if (varImgValue=='CloseCall')
					{
					document.Form1.txthiddenImage.value=varImgValue;
					document.Form1.txtrowvaluesCall.value =0;  
					__doPostBack("upnlTaskView","");
					//Form1.submit(); 
					return false;
					}
				if (varImgValue=='Fwd')
					{
					if (	document.Form1.txtrowvalues.value==0)
					{
					alert("Please select the row");
					}
					else
					{
					var CallNumber=document.Form1.txthiddenCallNo.value;
					var	TaskNumber=document.Form1.txtTask.value;
					//var	tableID=document.Form1.txthiddentable.value;
					var	CompanyID=document.Form1.txtComp.value;
						
					var strStatus;
					strStatus=document.Form1.txtTaskStatus.value;
					if ( strStatus=='CLOSED' )
					{
					alert('You cannot forward a closed task!');
					}
					else
					{
					wopen('../../SupportCenter/CallView/Task_Fwd.aspx?ScrID=340&CompID='+CompanyID+'&CallNo='+CallNumber+'&TASKNO='+TaskNumber,'FWD'+rand_no,400,250);
					}
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
    			if ( varImgValue=='ActionView')
					{
					if (document.Form1.txtCallNo.value=='' ||document.Form1.txtCallTask.value=='')
					{
					    alert("Please select a task from Call Detail Panel");
		     		    return false;
					}
					else
					{
					    intCallNo=document.Form1.txtCallNo.value;
					    intTaskNo=document.Form1.txtCallTask.value;
					    strComp=document.getElementById('cpnlCallTask_txtCustomer').value;
					    wopen('../../SupportCenter/CallView/ActionViewOnly.aspx?ScrID=539&intCallNo=' + intCallNo + '&intTaskNo=' + intTaskNo + '&strComp=' + strComp, 'ActionView' + rand_no, 800, 500);
					}					
		    		}			
				    return false;
		    }

		    ///To open resizable comment window.
		    function wopenComment(url, name, w, h) {
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
				'status=no, toolbar=no, scrollbars=yes, resizable=yes');
		        // Just in case width and height are ignored
		        win.resizeTo(w, h);
		        // Just in case left and top are ignored
		        win.moveTo(wleft, wtop);
		        win.focus();
		    }
	function KeyCheckTask(rowvalues,tableID,TaskNo)
		     {
			  
			    var table;
			
			    document.Form1.txtCallTask.value=TaskNo;			
			    document.Form1.txtCallNo.value=document.getElementById('cpnlCallTask_txtCallNumber').value;
			    document.getElementById('imgActionView').title="Click to View actions of selected Task";
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
				
	function KeyCheckMyTask(TaskNo,CallNo,rowvalues,rowvaluescall,tableID,Comp,ActionNo)
				{
			   
				globleID = CallNo;
				document.Form1.txthiddenCallNo.value=CallNo;
				document.Form1.txtTask.value=TaskNo;
				document.Form1.txtActionNo.value=ActionNo;
				document.Form1.txthiddentable.value=tableID;
				document.Form1.txtrowvalues.value=rowvalues;
				document.Form1.txtrowvaluesCall.value=rowvaluescall;
				
			if  (tableID=='cpnlTaskView_GrdAddSerach')
				{
				document.Form1.txtComp.value=Comp;
				}

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
				if (tableID=='cpnlTaskView_GrdAddSerach')
				{
				document.Form1.txthiddenImage.value='Select';
				__doPostBack("upnlCallTask","");
				}	
			}	
	function KeyCheckAction(TaskNo,CallNo,rowvalues,rowvaluescall,tableID,Comp,ActionNo,TaskStatus)
				{
			   
				globleID = CallNo;
				document.Form1.txthiddenCallNo.value=CallNo;
				document.Form1.txtTask.value=TaskNo;
				document.Form1.txtActionNo.value=ActionNo;
				document.Form1.txthiddentable.value=tableID;
				document.Form1.txtrowvalues.value=rowvalues;
				document.Form1.txtrowvaluesCall.value=rowvaluescall;
				document.Form1.txtTaskStatus.value=TaskStatus;
				
			if  (tableID=='cpnlTaskView_GrdAddSerach')
				{
				document.Form1.txtComp.value=Comp;
				}

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
				if (tableID=='cpnlTaskView_GrdAddSerach')
				{
				document.Form1.txthiddenImage.value='Select';
				//__doPostBack("upnlCallTask","");
				}	
			}		
    function KeyCheck555(Adressbookno,suppowner)
				{
					var strscreen;
					strscreen='502'
					OpenUserInfo(Adressbookno,suppowner,strscreen);
					//Form1.submit(); 
					return false;
				}	
    
    function OpenUserInfo(Adressbookno,supponer,strscreen)
				{
				wopen('../../SupportCenter/CallView/UserInfo.aspx?ADDNO='+Adressbookno +'&CALLOWNER='+supponer+'&ScreenID='+strscreen,'Search'+rand_no,350,500);
				}
					
    
    function OpenUserInfo2(ADDNO)
			{
			var strscreen='334';
			wopen('../../SupportCenter/CallView/UserInfo.aspx?ScrID=334&ADDNO='+ADDNO,'Search'+rand_no,350,500);
			return false;
			}		
					
	function KeyCheck55(nn,col,rowvalues,tableID,Comp)
					{
				
						    //Security Block
						    var obj=document.getElementById("imgEdit");
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
							
							if (tableID=='cpnlTaskView_GrdAddSerach')
							{
							document.Form1.txthiddenCallNo.value=col;
							document.Form1.txtTask.value=nn;
							document.Form1.txthiddenImage.value='Edit';
							document.Form1.txthiddentable.value=tableID;
							document.Form1.txtComp.value=Comp;
							wopen('../../SupportCenter/CallView/Task_edit.aspx?ScrID=334&ReadOnly=1&CompID='+Comp+'&CallNo='+col+'&TASKNO='+nn,'Search'+rand_no,440,470);
							}
							else if(tableID=='cpnlTaskAction_grdAction')
							{
							var strStatus;
							strStatus=document.Form1.txtTaskStatus.value;
							if ( strStatus=='CLOSED' )
							{
							alert('Task is  closed so you cannot modify actions!');
							}
							else
							{
							var CallNo =document.Form1.txthiddenCallNo.value;
							var TaskNo=document.Form1.txtTask.value;
							var CompID =document.Form1.txtComp.value;
														
							wopen('../../SupportCenter/CallView/Action_edit.aspx?ScrID=294&ACTIONNO='+nn+'&CompID='+CompID+'&CallNo='+CallNo+'&TASKNO='+TaskNo,'Search'+rand_no,350,400);
							}
							}
							else
							{
							//Form1.submit(); 
							return false;
							}
							return false;
				    }	
					
					
	function OpenTask(TASKNO,CALLNO)
				{
				wopen('../../SupportCenter/CallView/Task_edit.aspx?ScrID=334&TASKNO='+TASKNO+'&CALLNO='+CALLNO,'Search'+rand_no,440,470);
				}
					
	function OpenVW(vartable)
				{
				wopen('../../AdministrationCenter/AddressBook/AB_ViewColumns.aspx?ScrId=502&TBLName='+vartable,'Search'+rand_no,500,450);
				return false;
				}
				
    function ShowUserInfo(ID)
	       	{
				var Owner='';
				if ( ID=='txtCallBy' )
				{
				Owner=document.getElementById('cpnlCallTask_txtCallBy').value;	
				}
				else if ( ID=='DDLCoordinator' )
				{
				Owner=document.getElementById('cpnlCallTask_txtCordinator').value;		
				}
				else 
				{
			    Owner=document.getElementById('cpnlCallTask_CDDLCallOwner').value;	
				}
				if ( Owner=='' )
				{
				alert('No User Selected');
				}
				else
				{
				wopen('../../SupportCenter/CallView/UserInfo.aspx?ScrID=334&ADDNO='+ Owner ,'Search'+rand_no,350,500);
				}
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
			
 //Function is used to close the Tab
    function tabClose() 
        {
            window.parent.closeTab();
        }	


    </script>

    <table id="table1" height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td valign="top">
                <table id="table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table id="table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr width="100%">
                                    <td background="../../Images/top_nav_back.gif" height="47">
                                        <table id="table7" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BorderWidth="0px"
                                                        BackColor="#8AAFE5" BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="0px" Height="0px"
                                                        BorderWidth="0px" AlternateText="." CommandName="submit" ImageUrl="~/Images/white.gif">
                                                    </asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabelToDoList" runat="server" CssClass="TitleLabel">TO DO LIST</asp:Label></div>
                                                </td>
                                                <td style="width: 55%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:UpdatePanel ID="upnltop" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="imgSave" AccessKey="S" runat="server" ImageUrl="../../Images/S2Save01.gif"
                                                                    ToolTip="Select a Task to Add Actions"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                                    ToolTip="Select a Task to Edit"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgSearch" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                                    ToolTip="Search"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgAttachments" runat="server" AlternateText="View Attachments"
                                                                    ImageUrl="../../Images/ScreenHunter_075.bmp" ToolTip="View Attachments"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgFWD" runat="server" ImageUrl="../../Images/Fwd.jpg" ToolTip="Select a Task to Forward">
                                                                </asp:ImageButton>
                                                                <asp:ImageButton ID="imgCloseCall" runat="server" ImageUrl="../../Images/CloseCall1.gif"
                                                                    ToolTip="View Closed Task"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgShowReleased" runat="server" ImageUrl="../../Images/MyCall.jpg"
                                                                    ToolTip="Show only released tasks"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgActionView" runat="server" ImageUrl="../../Images/torch2.gif"
                                                                    ToolTip="Select a task from Call Detail Panel to view actions"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgCloseTask" runat="server" ImageUrl="../../Images/TaskClose.gif"
                                                                    ToolTip="Select a Task to Close"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                                    ToolTip="Select a Task to Delete Action"></asp:ImageButton>
                                                                &nbsp;<img src="../../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;"
                                                                    onclick="javascript:location.reload(true);" />
                                                                &nbsp;<img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                    style="cursor: hand;" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </center>
                                                </td>
                                             <td style="width: 5%">
                                                    <asp:UpdateProgress ID="progress" runat="server">
                                                        <ProgressTemplate>
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="../../Images/ajax1.gif" Width="24"
                                                                Height="24" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                                <td style="width: 15%">
                                                    <font face="Verdana" size="1"><strong>View&nbsp;<asp:DropDownList ID="ddlstview"
                                                        runat="server" Width="80px" Font-Names="Verdana" Font-Size="XX-Small">
                                                    </asp:DropDownList>
                                                        <asp:ImageButton ID="imgPlusCSS" runat="server" ImageUrl="../../Images/plus.gif">
                                                        </asp:ImageButton></strong></font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                          <img class="PlusImageCSS" id="VideoHelp" title="VideoHelp" onclick="ShowHelp('2211','../../');"
                                            alt="Video Help" src="../../Images/video_help.jpg" border="0">&nbsp;
                                        <img class="PlusImageCSS" id="Help" title="Word Help" onclick="ShowHelp('8','../../');"
                                            alt="E" src="../../Images/s1question02.gif" border="0" name="tbrbtnEdit" />&nbsp;
                                        <img class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                            src="../../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="100%">
                                            <asp:UpdatePanel ID="upnlTaskView" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <cc1:CollapsiblePanel ID="cpnlTaskView" runat="server" Width="100%" Height="262px"
                                                        BorderWidth="0px" BorderStyle="Solid" BorderColor="Indigo" Visible="true" TitleCSS="test"
                                                        PanelCSS="panel" TitleForeColor="black" TitleClickable="true" TitleBackColor="transparent"
                                                        Text="Task View" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                        Draggable="False">
                                                        <div id="divTaskView" style="overflow: auto; width: 1056px; height: 185pt">
                                                            <asp:Panel ID="cpnlCallTaskview" DefaultButton="btnCallTaskView" runat="server">
                                                                <span style="display: none">
                                                                    <asp:Button ID="btnCallTaskView" runat="server" Width="0" Height="0" /></span>
                                                                <table id="table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                                    width="100%" align="left" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="pnl" runat="server">
                                                                                <table id="table14" cellspacing="0" cellpadding="0" border="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="CHKC" runat="server" Width="20px" BorderWidth="0" Font-Size="XX-Small"
                                                                                                ToolTip="Comment Search"></asp:CheckBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="CHKA" runat="server" Width="20px" BorderWidth="0" Font-Size="XX-Small"
                                                                                                ToolTip="Attachment Search"></asp:CheckBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="CHKF" runat="server" Width="20px" BorderStyle="None" BorderWidth="0"
                                                                                                Font-Size="XX-Small" ToolTip="Form Search"></asp:CheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                        <td nowrap="nowrap">
                                                                            <asp:Panel ID="Panel1" runat="server">
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" align="left" colspan="2">
                                                                            <!--  **********************************************************************-->
                                                                            <asp:DataGrid ID="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px"
                                                                                Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" CellPadding="0"
                                                                                GridLines="Horizontal" HorizontalAlign="Left" PageSize="20" CssClass="Grid" DataKeyField="TaskNo"
                                                                                PagerStyle-Visible="False" AllowPaging="true" AllowSorting="true">
                                                                                <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="GridItem" BackColor="White">
                                                                                </ItemStyle>
                                                                                <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                                                </HeaderStyle>
                                                                                <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                                                <Columns>
                                                                                    <asp:TemplateColumn HeaderText="C">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="imgComm" runat="server" ImageUrl="../../Images/comment_Blank.gif">
                                                                                            </asp:Image>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn HeaderText="A">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="imgAtt" runat="server"></asp:Image>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn HeaderText="F">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image ID="imgform" runat="server"></asp:Image>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                </Columns>
                                                                                <PagerStyle Visible="False" Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                                            </asp:DataGrid><!-- Panel for displaying Task Info -->
                                                                            <!-- Panel for displaying Action Info-->
                                                                            <!-- ***********************************************************************-->
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </div>
                                                        <div>
                                                            <table id="table15" height="25">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="pg" Width="40px" Font-Size="8pt" Font-Names="Verdana" Font-Bold="true"
                                                                            ForeColor="#0000C0" runat="server">Page</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="CurrentPg" runat="server" Height="12px" Width="10px" Font-Size="X-Small"
                                                                            Font-Bold="true" ForeColor="Crimson"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="ofF" Font-Size="8pt" Font-Names="Verdana" Font-Bold="true" ForeColor="#0000C0"
                                                                            runat="server">of</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="TotalPages" runat="server" Height="12px" Width="10px" Font-Size="X-Small"
                                                                            Font-Bold="true" ForeColor="Crimson"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Firstbutton" runat="server" ImageUrl="../../Images/next9.jpg"
                                                                            AlternateText="First" ToolTip="First"></asp:ImageButton>
                                                                    </td>
                                                                    <td width="14">
                                                                        <asp:ImageButton ID="Prevbutton" runat="server" ImageUrl="../../Images/next99.jpg"
                                                                            ToolTip="Previous"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Nextbutton" runat="server" ImageUrl="../../Images/next9999.jpg"
                                                                            ToolTip="Next"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="Lastbutton" runat="server" ImageUrl="../../Images/next999.jpg"
                                                                            ToolTip="Last"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPageSize" runat="server" Height="14px" Width="24px" Font-Size="7pt"
                                                                            MaxLength="3"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="Button3" runat="server" Height="16px" Width="16px" BorderStyle="None"
                                                                            Font-Size="7pt" Font-Bold="true" ForeColor="Navy" ToolTip="Change Paging Size"
                                                                            Text=">"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblrecords" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                            Font-Bold="true" ForeColor="MediumBlue">Total Records</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="TotalRecods" runat="server" Height="12pt" Font-Size="8pt" Font-Names="Verdana"
                                                                            Font-Bold="true" ForeColor="Crimson"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </cc1:CollapsiblePanel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlCallTask" runat="server" UpdateMode="always">
                                                <ContentTemplate>
                                                    <asp:Panel DefaultButton="BtnGrdSearch1" ID="ii" runat="server">
                                                        <asp:Button ID="BtnGrdSearch1" runat="server" Height="0px" Width="0px" BorderWidth="0px" />
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <cc1:CollapsiblePanel ID="cpnlCallTask" runat="server" Width="100%" Height="232px"
                                                                        BorderWidth="0px" BorderStyle="Solid" BorderColor="Indigo" Visible="true" TitleCSS="test"
                                                                        PanelCSS="panel" TitleForeColor="black" TitleClickable="true" TitleBackColor="transparent"
                                                                        Text="Call Detail" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                                        Draggable="False">
                                                                        <table id="table3" cellspacing="0" cellpadding="0" width="750" border="0">
                                                                            <tr>
                                                                                <td valign="top" align="left" width="400">
                                                                                    <table id="table17" style="width: 420px" bordercolor="#5c5a5b" bgcolor="#f5f5f5"
                                                                                        border="1">
                                                                                        <tr>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="148">
                                                                                                <asp:Label ID="lblMiddleName4" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Customer</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCustomer" Width="110px" Height="14px" CssClass="txtNoFocus" runat="server"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="144">
                                                                                                <asp:Label ID="lblMiddleName10" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">SubCategory</asp:Label><br>
                                                                                                <asp:TextBox ID="txtProject" Width="110px" Height="14px" CssClass="txtNoFocus" runat="server"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label8" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Agreement</asp:Label><br>
                                                                                                <asp:TextBox ID="txtAgreement" Width="110px" Height="14px" CssClass="txtNoFocus"
                                                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="height: 35px" bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                            <td style="height: 35px" bordercolor="#f5f5f5" width="148">
                                                                                                <asp:Label ID="lblMiddleName2" runat="server" Height="12px" Width="112px" CssClass="FieldLabel">Requested By</asp:Label><br>
                                                                                                <asp:TextBox ID="txtrequestedBy" Width="105px" Height="14px" CssClass="txtNoFocus"
                                                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                                                <asp:Image ID="imgCallRequestedBy" Style="cursor: hand" onclick="return ShowUserInfo('CDDLCallOwner');"
                                                                                                    Width="15px" ImageUrl="../../Images/user.gif" AlternateText="Click to see User Info"
                                                                                                    runat="server"></asp:Image>
                                                                                                <span style="visibility: hidden">
                                                                                                    <asp:TextBox ID="CDDLCallOwner" runat="server" Visible="true" BorderStyle="Solid"
                                                                                                        BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" Height="0" Width="0"
                                                                                                        MaxLength="8" ReadOnly="true"></asp:TextBox></span>
                                                                                            </td>
                                                                                            <td style="height: 35px" bordercolor="#f5f5f5" width="144">
                                                                                                <asp:Label ID="Label11" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Coordinator</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCoordinator" Width="105px" Height="14px" CssClass="txtNoFocus"
                                                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                                                <asp:Image ID="imgCoordinator" Style="cursor: hand" onclick="return ShowUserInfo('DDLCoordinator');"
                                                                                                    Width="15px" ImageUrl="../../Images/user.gif" AlternateText="Click to see User Info"
                                                                                                    runat="server"></asp:Image>
                                                                                                <span style="visibility: hidden">
                                                                                                    <asp:TextBox ID="txtCordinator" runat="server" Visible="true" BorderStyle="Solid"
                                                                                                        BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" Height="0" Width="0"
                                                                                                        MaxLength="8" ReadOnly="true"></asp:TextBox></span>
                                                                                            </td>
                                                                                            <td style="height: 35px" bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="lblMiddleName6" runat="server" Height="12px" Width="99px" CssClass="FieldLabel">Entered By</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCallEnteredBy" runat="server" Height="14px" Width="100px" CssClass="txtNoFocus"
                                                                                                    ReadOnly="true"></asp:TextBox><asp:Image ID="imgCallEnteredBy" Style="cursor: hand"
                                                                                                        onclick="return ShowUserInfo('txtCallBy');" Width="15px" ImageUrl="../../Images/user.gif"
                                                                                                        AlternateText="Click to see User Info" runat="server"></asp:Image>
                                                                                                <span style="visibility: hidden">
                                                                                                    <asp:TextBox ID="txtCallBy" runat="server" Visible="true" BorderStyle="Solid" BorderWidth="1px"
                                                                                                        Font-Size="XX-Small" Font-Names="Verdana" Height="0" Width="0" MaxLength="8"
                                                                                                        ReadOnly="true"></asp:TextBox></span>
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="148">
                                                                                                <asp:Label ID="lblMiddleName" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Call Type</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCallType" Width="110px" Height="14px" CssClass="txtNoFocus" runat="server"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="144">
                                                                                                <asp:Label ID="Label7" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Priority</asp:Label><br>
                                                                                                <asp:TextBox ID="txtPriority" Width="110px" Height="14px" CssClass="txtNoFocus" runat="server"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label10" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Related Call</asp:Label><br>
                                                                                                <asp:TextBox ID="txtrelatedCall" Width="110px" Height="14px" CssClass="txtNoFocus"
                                                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="148">
                                                                                                <asp:Label ID="Label13" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Category</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCategory" runat="server" Height="14px" Width="109px" CssClass="txtNoFocus"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="144">
                                                                                                <asp:Label ID="lblMiddleName20" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Cause Code</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCauseCode" runat="server" Height="14px" Width="109px" CssClass="txtNoFocus"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="lblMiddleName12" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Reference</asp:Label><br>
                                                                                                <asp:TextBox ID="txtreference" runat="server" Height="14px" Width="109px" CssClass="txtNoFocus"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="148">
                                                                                                <asp:Label ID="Label4" runat="server" Height="12px" Width="104px" CssClass="FieldLabel">Request Date</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCallDate" runat="server" Height="14px" Width="110px" CssClass="txtNoFocus"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="144">
                                                                                                <asp:Label ID="Label5" runat="server" Height="12px" Width="96px" CssClass="FieldLabel">Template Type</asp:Label><br>
                                                                                                <asp:TextBox ID="txtTemplateType" Width="110px" CssClass="txtNoFocus" Height="14px"
                                                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label6" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Template</asp:Label><br>
                                                                                                <asp:TextBox ID="txtTemplateName" runat="server" Height="14px" Width="109px" CssClass="txtNoFocus"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="148">
                                                                                                <asp:Label ID="lblMiddleName18" runat="server" Height="13px" Width="108px" CssClass="FieldLabel">Est Close Date</asp:Label><br>
                                                                                                <asp:TextBox ID="txtEstCloseDate" Width="110px" Height="14px" CssClass="txtNoFocus"
                                                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="144">
                                                                                                <asp:Label ID="lblMiddleName14" runat="server" Height="12px" Width="100px" CssClass="FieldLabel">Estimated Hours</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCallEstHours" runat="server" Height="14px" Width="109px" CssClass="txtNoFocus"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="lblMiddleName16" runat="server" Height="12px" Width="100px" CssClass="FieldLabel">Reported Hours</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCallReportedHours" runat="server" Height="14px" Width="109px"
                                                                                                    CssClass="txtNoFocus" ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="height: 36px" bordercolor="#f5f5f5" width="4">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr style="height:25px">
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                </td>
                                                                                <td valign="top" align="left" width="258" height="0">
                                                                                    <table id="table18" bordercolor="#5c5a5b" bgcolor="#f5f5f5" border="1">
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5" width="14">
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label1" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Status</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCallStatus" Width="90px" Height="14px" CssClass="txtNoFocus"
                                                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label14" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Call No.</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCallNumber" runat="server" Height="14px" Width="60px" BorderStyle="Solid"
                                                                                                    BorderWidth="1px" Font-Size="XX-Small" Font-Names="Verdana" CssClass="txtNoFocus"
                                                                                                    MaxLength="10" ReadOnly="true" BackColor="#e5e5e5"></asp:TextBox>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" width="153">
                                                                                                <asp:Label ID="Label9" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Comment</asp:Label><br>
                                                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                <asp:ImageButton ID="imgComment" ImageUrl="../../Images/comment_Blank.gif" AlternateText="Comment"
                                                                                                    runat="server"></asp:ImageButton>
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5">
                                                                                                <asp:Label ID="Label2" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Attachment</asp:Label><br>
                                                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                <asp:ImageButton ID="imgCallAttach" ImageUrl="../../Images/Attach15_9.gif" CssClass="PlusImageCSS"
                                                                                                    runat="server"></asp:ImageButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5" width="10">
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" colspan="4">
                                                                                                <asp:Label ID="Label12" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">StartDate</asp:Label><br>
                                                                                                <telerik:RadDateTimePicker ID="dtCallStartDate" Height="15px" Width="200px" runat="server"
                                                                                                    DateInput-DisplayDateFormat="yyyy-MMM-dd HH:mm tt" Enabled="false">
                                                                                                    <DateInput ID="DateInput1" runat="server">
                                                                                                        <ClientEvents OnKeyPress="ShowDateTimePopup" />
                                                                                                    </DateInput>
                                                                                                    <DatePopupButton Visible="false" />
                                                                                                    <Calendar DayNameFormat="FirstLetter" FirstDayOfWeek="Default" UseColumnHeadersAsSelectors="False"
                                                                                                        UseRowHeadersAsSelectors="False">
                                                                                                    </Calendar>
                                                                                                    <TimePopupButton Visible="false" />
                                                                                                </telerik:RadDateTimePicker>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr style="height: 3px">
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td bordercolor="#f5f5f5" width="10">
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" colspan="4">
                                                                                                <asp:Label ID="Label3" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Subject</asp:Label><br>
                                                                                                <asp:TextBox ID="txtSubject" runat="server" Height="14px" Width="315px" CssClass="txtNoFocus"
                                                                                                    ReadOnly="true"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr valign="top">
                                                                                            <td bordercolor="#f5f5f5" width="10">
                                                                                            </td>
                                                                                            <td bordercolor="#f5f5f5" colspan="4">
                                                                                                <asp:Label ID="lblMiddleName21" runat="server" Height="12px" Width="72px" CssClass="FieldLabel">Description</asp:Label><br>
                                                                                                <asp:TextBox ID="txtCallDescription" runat="server" Height="133px" Width="315px"
                                                                                                    CssClass="txtNoFocus" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>&nbsp;&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <cc1:CollapsiblePanel ID="cpnlTaskList" runat="server" Height="232px" Width="100%"
                                                                            BorderStyle="Solid" BorderWidth="0px" Draggable="False" CollapseImage="../../Images/ToggleUp.gif"
                                                                            ExpandImage="../../Images/ToggleDown.gif" Text="Task List" TitleBackColor="transparent"
                                                                            TitleClickable="true" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                                            Visible="true" BorderColor="Indigo">
                                                                            <table id="table19" style="border-collapse: collapse" align="left" border="0">
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:PlaceHolder ID="Placeholder3" runat="server"></asp:PlaceHolder>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </cc1:CollapsiblePanel>
                                                                    </cc1:CollapsiblePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <cc1:CollapsiblePanel ID="cpnlTaskAction" runat="server" Width="100%" Height="262px"
                                                                        BorderWidth="0px" BorderStyle="Solid" BorderColor="Indigo" Visible="true" TitleCSS="test"
                                                                        PanelCSS="panel" TitleForeColor="black" TitleClickable="true" TitleBackColor="transparent"
                                                                        Text="Task View" ExpandImage="../../Images/ToggleDown.gif" CollapseImage="../../Images/ToggleUp.gif"
                                                                        Draggable="False">
                                                                        <table id="table20" style="border-collapse: collapse" width="100%" border="0">
                                                                            <tr>
                                                                                <td colspan="10">
                                                                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:Panel ID="PnlAction" Width="1pt" runat="server">
                                                                            <table id="table21" bordercolor="#ff0066" cellspacing="1" cellpadding="1" border="0">
                                                                                <tr valign="top">
                                                                                    <td>
                                                                                        <asp:Image ID="ImgHid" Width="88px" Height="18px" ImageUrl="../../Images/divider.gif"
                                                                                            runat="server"></asp:Image>
                                                                                        <asp:TextBox ID="Textbox1" Height="18px" BorderStyle="Solid" BorderWidth="1px" Visible="False"
                                                                                            CssClass="txtNoFocusFE" runat="server" Enabled="False"></asp:TextBox>
                                                                                        <asp:TextBox ID="TxtActionNo_F" Height="18px" BorderStyle="Solid" BorderWidth="1px"
                                                                                            Visible="False" CssClass="txtNoFocusFE" runat="server" Enabled="False"></asp:TextBox><!--<asp:imagebutton id="imgAttachAction" runat="server" ImageUrl="../../Images/ScreenHunter_075.bmp"></asp:imagebutton>-->
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="Txtdescription_F" runat="server" Height="18px" Width="345px" BorderStyle="Solid"
                                                                                            BorderWidth="1px" CssClass="txtNoFocusFE" MaxLength="1950" TextMode="MultiLine"></asp:TextBox>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkMandatoryHr" Height="18px" Width="38px" Font-Size="XX-Small"
                                                                                            Font-Names="Verdana" ToolTip="Hours Mandatory" runat="server" Checked="true">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="TxtUsedHr_F" runat="server" Height="18px" Width="40px" BorderStyle="Solid"
                                                                                            BorderWidth="1px" CssClass="txtNoFocusFE" MaxLength="10"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <ION:Customcalendar ID="dtActionDate" runat="server" Width="135px" Height="19px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="ChkActType" runat="server" Height="18px" Width="44px" Font-Size="XX-Small"
                                                                                            Font-Names="Verdana" Checked="true"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </cc1:CollapsiblePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
        </tr>
    </table>
    <asp:UpdatePanel ID="upnlhidden" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server">
            </asp:Panel>
            <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
            </td>
            <input type="hidden" name="txthidden" />
            <input name="txthiddenImage" type="hidden" />
            <input type="hidden" value="<%=mstrCallNumber%>" name="txthiddenCallNo" />
            <input type="hidden" value="<%=strhiddentable%>" name="txthiddentable" />
            <input type="hidden" value="<%=mstrTaskNumber%>" name="txtTask" />
            <input type="hidden" value="<%=introwvalues%>" name="txtrowvalues" />
            <input type="hidden" value="<%=introwvalues%>" name="txtrowvaluesCall" />
            <input type="hidden" id="txtComp" name="txtComp" runat="server" />
            <input type="hidden" id="txtActionNo" name="txtActionNo" runat="server" />
            <input type="hidden" name="txtHIDSize" />
            <input type="hidden" name="txtCallTask" />
            <input type="hidden" name="txtCallNo" />
            <input type="hidden" id="txtTaskStatus" name="txtTaskStatus" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
