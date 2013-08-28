<%@ page language="VB" autoeventwireup="false" enableeventvalidation="false" inherits="DocumentsMgt_FolderMgtMaster, App_Web_p5id54gx" maintainscrollpositiononpostback="true" theme="App_Themes" %>

<%@ Register TagPrefix="uc1" TagName="CustomDDL" Src="../BaseClasses/Controls/ComboControl/CustomDDL.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="uc1" TagName="DateSelector" Src="../SupportCenter/calendar/DateSelector.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Folder Mgt Master</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="JavaScript" src="../Images/js/core.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/events.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/css.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/coordinates.js" type="text/javascript"></script>

    <script language="JavaScript" src="../Images/js/drag.js" type="text/javascript"></script>

    <link href="../Images/js/StyleSheet1.css " type="text/css" rel="stylesheet">

    <script language="javascript" src="../Images/Js/JSValidation.js"></script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript">

        var globleID;
		var globleUser;
		var globleRole;
		var globleCompany;
	var rand_no = Math.ceil(500*Math.random())		
				
function callrefresh()
			{
				location.href="AB_Search.aspx";
			}
				
function ShowUserInfo(ID)
		{
	
		var Owner='';
			if ( ID=='txtCallBy' )
				{
					Owner=document.getElementById('cpnlCallView_txtCallBy').value;			
				}
				else if ( ID=='DDLCoordinator' )
				{
					Owner=document.getElementById('cpnlCallView_'+ ID).options(document.getElementById('cpnlCallView_'+ ID).selectedIndex).value;
				}
				else 
				{
					Owner=document.getElementById('cpnlGrdView_'+ ID+'_DDL').options(document.getElementById('cpnlGrdView_'+ ID +'_DDL').selectedIndex).value;
				}
				if ( Owner=='' )
				{
					alert('No User Selected');
				}
				else
				{
					wopen('../SupportCenter/CallView/UserInfo.aspx?ScrID=334&ADDNO='+ Owner ,'Search'+rand_no,350,500);
				}
		}			
								
function ConfirmDelete(varImgValue)
				{
						if (globleID==null)
								{
									alert("Please select the row");
									return false;
								}
								else
								{
									var confirmed
									confirmed=window.confirm("Delete,Are you sure you want to Delete the selected record ?");
									if(confirmed==false)
									{
										return false;
									}
									else
									{
										 document.Form1.txthiddenImage.value=varImgValue;
										 Form1.submit(); 
										 return false;
									}
								}
				}
				
	function SaveEdit(varImgValue)
			    {
			          if (varImgValue=='Permission')
				          {
								if ( '<%= Session("FolderID") %>'  =='')
									{
									alert('Please Select the Folder for Permission........');
									return false;
									}
									else
									{
									//alert(document.Form1.txthiddenAdno.value);
									wopen('FolderPermissions.aspx','FolderPermissions'+rand_no,400,480);
									}
							}	
												
			          if (varImgValue=='Close')
												{
												 document.Form1.txthiddenImage.value=varImgValue;
												 Form1.submit(); 
												 return false;
												}
								
			          if (varImgValue=='AddFolder')
										 {
									     wopen('FolderSettings.aspx?ButtonClick=ADD' ,'FolderSettings'+rand_no,600,450);
										 return false;
								         }	
												
			         if (varImgValue=='EditFolder')
							 {
									var IsComp;
									var IsFOlder;
									 	IsComp=document.getElementById('txtIsComp').value;	
									 	IsFOlder=document.getElementById('txtIsFolder').value;			
									 										
									if (IsComp==0)
									    {
											if (IsFOlder==0)
												{
												alert('Please Select Empty Folder to Edit........');
												return false;
												}
											else
												{
												var test=IsComp
													wopen('FolderSettings.aspx?FolderEmpty='+test+'&ButtonClick=Edit' ,'FolderSettings'+rand_no,600,450);
													return false;
																									
												}
									    }
									 else
										{
										alert('Please Select the Folder in place of company for Edit........');
										return false;
										}	  
										 
							  }	
			
			     if (varImgValue=='DeleteFolder')
							 {
									var IsComp;
									var IsFOlder;
									 	IsComp=document.getElementById('txtIsComp').value;	
									 	IsFOlder=document.getElementById('txtIsFolder').value;		
									 		
									if (IsComp==0)
									    {
											if (IsFOlder==0)
												{
												alert('Please Select Empty Folder for Delete........');
												return false;
												}
											else
												{
									                 var confirmed
									                   confirmed=window.confirm("Delete,Are you sure you want to Delete the selected Folder ?");
									                   if(confirmed==false)
									                             {
										                         return false;
									                              }
									                         else
									                         {
										                      document.Form1.txthiddenImage.value=varImgValue;
										                       __doPostBack("upnlFolderMgt","");
										                        //Form1.submit(); 
										                        return false;
									                         }
																					
												}
											  }
									   else
										{
										alert('Please Select the Folder in place of company for Delete........');
										return false;
										}	  
										 
							  }	
			
												
				 if (varImgValue=='AddFile')
							 {
								var IsComp;
								var IsFOlder;
								 	IsComp=document.getElementById('txtIsComp').value;	
								 	IsFOlder=document.getElementById('txtIsFolder').value;		
									//alert(IsFOlder);
							if (IsComp==0)
							     {
											if (IsFOlder=='')
												{
												alert('Please Select the Folder to add Files .....');
												return false;
												}
											else
												{
												var test=IsFOlder
													wopen('FileDetails.aspx?ButtonClick=ADD' ,'FileDetailsAdd'+rand_no,450,350);
													return false;
												}
									}
								 else
									{
									alert('Please Select the Folder to add File........');
									return false;
									}	 
							 }	
								    
					 if (varImgValue=='EditFile')
								  {
									 if (document.Form1.txtFileID.value=='')
										 {
										   alert('Please Select the File .....');
										   return false;
										  }
									 else
										 {
										   var FileID=document.Form1.txtFileID.value
										    wopen('FileDetails.aspx?FileID='+FileID+'&ButtonClick=EDIT' ,'FileDetailsEdit'+rand_no,450,350);
											return false;
									     }
							    }
							    												    								
					 if (varImgValue=='DeleteFile')
								  {
									 if (document.Form1.txtFileID.value=='')
										 {
										   alert('Please Select the File .....');
										   return false;
										  }
									 else
										 {
																		
										     var confirmed
									         confirmed=window.confirm("Delete,Are you sure you want to Delete the selected File ?");
									         if(confirmed==false)
									                 {
										              return false;
									                  }
									                else
									                   {
										                document.Form1.txthiddenImage.value=varImgValue;
										                  __doPostBack("upnlFolderMgt","");
										                //Form1.submit(); 
										                return false;
									                   }
										 }
							    }																
																	
			       if (varImgValue=='Search')
								{
								  //alert('ok');
								  document.Form1.txthiddenImage.value=varImgValue;
								  Form1.submit(); 
								  return false;
								}	
								
			       if (varImgValue=='Logout')
								    {
									document.Form1.txthiddenImage.value=varImgValue;
									  alert('ok');
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
											else
												    {
													return false;
													}

									        }			
			        }				
				
 					
function KeyCheck55(nn,rowvalues,FilePath)
					{
					    alert('ok');
					  document.Form1.txthiddenAdno.value=nn;
					  document.Form1.txthiddenImage.value='Edit';
					  document.Form1.txtFilePath.value=FilePath;
					  Form1.submit(); 
					 //  __doPostBack("upnlFolderMgt","");
					}	

function OpenW(varTable)
				{
				wopen('../AdministrationCenter/AddressBook/AB_ViewColumns.aspx? ID='+varTable,'Search'+rand_no,500,450);
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
				
 function SClick(FileID, RI)
		{
		 document.Form1.txtFileID.value=FileID;
			var tableID='cpnlGrdView_cpnlFilesList_grdFiles';
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
					table.rows [ RI  ] . style . backgroundColor = "#d4d4d4";
				}
		}


				
    </script>

    <script type="text/javascript">
        
        //A Function to call on Page Load to set grid width according to screen size
    function onLoad() 
        {
            var divGrdView = document.getElementById('divGrdView');
            divGrdView.style.width = document.body.clientWidth - 30 + "px";
        }
        
        //A Function to improve design i.e delete the extra cell of table
   function onEnd() 
        {
        var x = document.getElementById('cpnlGrdView_collapsible').cells[0].colSpan = "1";
          var y = document.getElementById('cpnlGrdView_cpnlFilesList_collapsible').cells[0].colSpan = "1";    
        } 
         //A Function is Called when we resize window
        window.onresize = onLoad; 
        
 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	
   
    </script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(onLoad);     
    </script>

 <%--  / <asp:UpdatePanel ID="upnlFolderMgt" runat="server">
        <ContentTemplate>--%>
            <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td valign="top">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr width="100%">
                                            <td background="../Images/top_nav_back.gif" height="47">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td style="width: 25%">
                                                            <asp:Button ID="BtnGrdSearch" runat="server" Height="0px" Width="0px" BorderWidth="0px"
                                                                BorderStyle="None" BackColor="#8AAFE5" BorderColor="#8AAFE5"></asp:Button>
                                                            <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Height="1px" Width="1px"
                                                                AlternateText="." CommandName="submit" ImageUrl="~/images/white.GIF"></asp:ImageButton>
                                                            <asp:Label ID="lblTitleLabelRoleSearch" runat="server" CssClass="TitleLabel" BorderStyle="None"> Folder Mgt Master</asp:Label>
                                                        </td>
                                                        <td style="width: 65%; text-align: center;" nowrap="nowrap">
                                                            <center>
                                                                <asp:ImageButton ID="imgAdd" AccessKey="S" runat="server" ImageUrl="../Images/Addfoleders.jpg"
                                                                    ToolTip="Add Folder"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgEditFolder" AccessKey="S" runat="server" ImageUrl="../Images/Editfolder.jpg"
                                                                    ToolTip="Edit Folder"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgDeleteFolder" AccessKey="L" runat="server" ImageUrl="../Images/s2delete01.gif"
                                                                    ToolTip="Delete Folder"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgPermission" AccessKey="E" runat="server" ImageUrl="../Images/q1.jpg"
                                                                    ToolTip="Set Permission"></asp:ImageButton>
                                                                <img title="Seperator" alt="R" src="../Images/00Seperator.gif" border="0">
                                                                <asp:ImageButton ID="imgAddFiles" AccessKey="E" runat="server" ImageUrl="../Images/Addfiles.jpg"
                                                                    ToolTip="Add File "></asp:ImageButton>
                                                                <asp:ImageButton ID="imgEditFile" AccessKey="E" runat="server" ImageUrl="../Images/editfiles.jpg"
                                                                    ToolTip="Edit File "></asp:ImageButton>
                                                                <asp:ImageButton ID="imgDeleteFile" AccessKey="L" runat="server" ImageUrl="../Images/Deletefiles.jpg"
                                                                    ToolTip="Delete File"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../Images/s1search02.gif"
                                                                    ToolTip="Search"></asp:ImageButton>
                                                                <img src="../Images/reset_20.gif" title="Refresh" alt="" style="cursor: hand;" onclick="javascript:location.reload(true);" />
                                                                <img src="../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();" style="cursor: hand;" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td nowrap="nowrap" style="width: 10%" background="../Images/top_nav_back.gif" height="47">
                                                <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('970','../');"
                                                    alt="E" src="../Images/s1question02.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;<img
                                                        class="PlusImageCSS" id="Logout" title="Logout" onclick="LogoutWSS();" alt="E"
                                                        src="../icons/logoff.gif" border="0" name="tbrbtnEdit">&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:CollapsiblePanel ID="cpnlGrdView" runat="server" Height="47px" Width="100%"
                                        BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                        ExpandImage="../Images/ToggleDown.gif" Text="Folders Info" TitleBackColor="Transparent"
                                        TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                        BorderColor="Indigo" Visible="true">
                                        <div id="divGrdView" style="overflow: auto; width: 100%; height: 540px">
                                            <table id="Table1261" bordercolor="#5c5a5b" cellspacing="0" cellpadding="0" width="100%"
                                                align="left" bgcolor="#f5f5f5" border="1">
                                                <tr>
                                                    <td width="15%">
                                                        <div style="overflow: auto; width: 180px; height: 488px">
                                                            <asp:Panel ID="Panel1" runat="server">
                                                            </asp:Panel>
                                                            <asp:Panel ID="cpnlMnu" runat="server">
                                                            </asp:Panel>
                                                            <asp:TreeView ID="mobjTreeMenu" runat="server">
                                                            </asp:TreeView>
                                                        </div>
                                                    </td>
                                                    <td valign="top" width="85%">
                                                        <cc1:CollapsiblePanel ID="cpnlFilesList" runat="server" Height="47px" Width="100%"
                                                            BorderWidth="0px" BorderStyle="Solid" Draggable="False" CollapseImage="../Images/ToggleUp.gif"
                                                            ExpandImage="../Images/ToggleDown.gif" Text="Files List" TitleBackColor="Transparent"
                                                            TitleClickable="True" TitleForeColor="black" PanelCSS="panel" TitleCSS="test"
                                                            BorderColor="Indigo" Visible="true">
                                                            <div style="overflow: auto; width: 100%; height: 480px">
                                                                <table width="100%" align="left">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DataGrid ID="grdFiles" runat="server" Height="0px" BorderStyle="None" BorderWidth="1px"
                                                                                CssClass="Grid" BorderColor="Silver" Font-Names="Verdana" CellPadding="0" AutoGenerateColumns="False">
                                                                                <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                                                <ItemStyle CssClass="GridItem"></ItemStyle>
                                                                                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                                                <Columns>
                                                                                    <asp:TemplateColumn>
                                                                                        <HeaderTemplate>
                                                                                            <asp:TextBox ID="txtFileID_H" runat="server" Width="50px" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                            FileID
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblFileID" runat="server" Width="50px" Text='<%#container.dataitem("FileID")%>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn>
                                                                                        <HeaderTemplate>
                                                                                            <asp:TextBox ID="txtCompany_H" runat="server" Width="120px" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                            Company
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <%#container.dataitem("company")%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn>
                                                                                        <HeaderTemplate>
                                                                                            <asp:TextBox ID="txtFileName_H" runat="server" Width="245px" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                            FileName
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="systemLink5" runat="server" CommandArgument='<%# DataBinder.Eval(Container," DataItem.FilePath") %>'
                                                                                                CommandName='<%# DataBinder.Eval(Container, "DataItem.FileName") %>'>
																																								<%# DataBinder.Eval(Container, "DataItem.FileName") %>
                                                                                            </asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn>
                                                                                        <HeaderTemplate>
                                                                                            <asp:TextBox ID="txtDescription_H" runat="server" Width="265px" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                            Description
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <%#container.dataitem("Description")%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn>
                                                                                        <HeaderTemplate>
                                                                                            <asp:TextBox ID="txtUploadedOn_H" runat="server" Width="80px" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                            UploadedOn
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <%#container.dataitem("UploadedOn")%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn>
                                                                                        <HeaderTemplate>
                                                                                            <asp:TextBox ID="txtUploadedBy_H" runat="server" Width="80px" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                            UploadedBy
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblResType" runat="server" Width="70px" Text='<%#container.dataitem("UploadedBy")%>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn>
                                                                                        <HeaderTemplate>
                                                                                            <asp:TextBox ID="txtVersion_H" runat="server" Width="60px" CssClass="SearchTxtBox"></asp:TextBox>
                                                                                            Version
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblVersion" runat="server" Width="60px" Text='<%#container.dataitem("Version")%>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                </Columns>
                                                                            </asp:DataGrid>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </cc1:CollapsiblePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </cc1:CollapsiblePanel>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Visible="false"></asp:ListBox>
                        <input type="hidden" name="txthiddenAdno" />
                        <input type="hidden" name="txthiddenImage" />
                        <input type="hidden" name="txtFilePath" />
                        <input type="hidden" name="txtFileID" />
                        <span style="display: none">
                            <input type="text" id="txtIsComp" runat="server" />
                            <input type="text" id="txtIsFolder" runat="server" />
                        </span>
                    </td>
                </tr>
            </table>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
