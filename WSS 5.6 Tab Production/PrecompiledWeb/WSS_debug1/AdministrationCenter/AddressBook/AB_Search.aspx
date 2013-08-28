<%@ page language="VB" autoeventwireup="false" inherits="AdministrationCenter_AddressBook_AB_Search, App_Web_owb2nwqw" maintainscrollpositiononpostback="true" enableEventValidation="false" theme="App_Themes" %>

<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../../images/js/StyleSheet1.css" type="text/css" rel="stylesheet" />

    <script src="../../images/js/core.js" type="text/javascript"></script>

    <script src="../../images/js/events.js" type="text/javascript"></script>

    <script src="../../images/js/css.js" type="text/javascript"></script>

    <script src="../../images/js/coordinates.js" type="text/javascript"></script>

    <script src="../../images/js/drag.js" type="text/javascript"></script>

    <script src="../../Images/Js/JSValidation.js" type="text/javascript"></script>

    <script type="text/javascript">

        var globleID;



        function callrefresh() {
            location.href = "../../AdministrationCenter/addressbook/AB_Search.aspx?ScrID=37";
            //Form1.submit();
        }

        function ConfirmDelete(varImgValue) {
            //alert(document.Form1.txthiddenTable.value);
            //alert(document.Form1.txtTask.value);

            if (globleID == null) {
                alert("Please select the row");
                return false;
            }
            else {
                var confirmed
                confirmed = window.confirm("Are you sure you want to Delete the selected record ?");
                if (confirmed == true) {
                    document.Form1.txthiddenImage.value = varImgValue;
                    Form1.submit();
                }
            }
            return false;
        }



        function SaveEdit(varImgValue) {
            if (varImgValue == 'ShowAll') {
                document.Form1.txthiddenImage.value = varImgValue;
                __doPostBack("upnlCallView", "");
                //								Form1.submit(); 
                return false;
            }

            if (varImgValue == 'View') {

                document.Form1.txthiddenImage.value = varImgValue;
                __doPostBack("upnlCallView", "");
                //								 	Form1.submit(); 		

            }

            if (varImgValue == 'Edit') {

                //Security Block
                var obj = document.getElementById("imgEdit")
                if (obj == null) {
                    alert("You don't have access rights to edit record");
                    return false;
                }

                if (obj.disabled == true) {
                    alert("You don't have access rights to edit record");
                    return false;
                }
                //End of Security Block
                if (globleID == null) {
                    alert("Please select the row");
                    return false;
                }
                else {
                    document.Form1.txthiddenImage.value = varImgValue;
                    var AddressNo= document.Form1.txthidden.value;
                    var screenid = window.parent.getActiveTabDetails();
                    window.parent.OpenTabOnDBClick('Address No#' + AddressNo,"AdministrationCenter/AddressBook/AB_Main.aspx?ScrID=194&AddressNo="+ AddressNo, 'Address No#' + AddressNo,screenid);
                    //__doPostBack("upnlCallView", "");
                    //Form1.submit(); 
                    return false;
                }

            }

            if (varImgValue == 'EditPassportInfo') {
                var obj1 = document.getElementById("imgPassportInfo")
                if (obj1 == null) {
                    alert("You don't have access rights to edit record");
                    return false;
                }

                if (obj1.disabled == true) {
                    alert("You don't have access rights to edit record");
                    return false;
                }
                //End of Security Block
                if (globleID == null) {
                    alert("Please select the row");
                    return false;
                }
                else {
                    document.Form1.txthiddenImage.value = varImgValue;
                    var AddressNumber = document.Form1.txthidden.value;
                    var Screenid = window.parent.getActiveTabDetails();
                    window.parent.OpenTabOnDBClick('EmpInfo Entry#' + AddressNumber, "AdministrationCenter/AddressBook/AB_Detail.aspx?ScrID=2021&AddressNo=" + AddressNumber, 'Address No#' + AddressNumber, Screenid);
                    return false;
                }
            }
            
            if (varImgValue == 'Close') {
                window.close();
            }


            if (varImgValue == 'Add') {
                document.Form1.txthiddenImage.value = varImgValue;
                window.parent.OpenTabOnAddClick('Address Book',"AdministrationCenter/AddressBook/AB_Main.aspx?ScrID=194", "194");
                //Form1.submit();
                return false;

            }
            if (varImgValue == 'Search') {
                document.Form1.txthiddenImage.value = varImgValue;
                __doPostBack("upnlCallView", "");
                //									Form1.submit(); 
                return false;
            }

            if (varImgValue == 'Logout') {
                document.Form1.txthiddenImage.value = varImgValue;
                Form1.submit();
                return false;
            }

            if (varImgValue == 'Reset') {
                var confirmed
                confirmed = window.confirm("Do You Want To reset The Page ?");
                if (confirmed == true) {
                    Form1.reset()
                    return false;
                }
                else {
                    return false;
                }

            }
        }


        function KeyCheck(nn, rowvalues) {
            //alert(rowvalues);
            globleID = nn;
            document.Form1.txthidden.value = nn;
            //Form1.submit();

            var tableID = 'cpnlCallView_GrdAddSerach'  //your datagrids id
            var table;

            if (document.all) table = document.all[tableID];
            if (document.getElementById) table = document.getElementById(tableID);
            if (table) {

                for (var i = 1; i < table.rows.length; i++) {
                    if (i % 2 == 0) {
                        table.rows[i].style.backgroundColor = "#f5f5f5";
                    }
                    else {

                        table.rows[i].style.backgroundColor = "#ffffff";
                    }
                }
                table.rows[rowvalues].style.backgroundColor = "#d4d4d4";
            }

        }

        function KeyCheck55(nn, rowvalues) {
            document.Form1.txthiddenImage.value = 'Edit';
            SaveEdit('Edit');
        }

        function OpenW(varTable) {

            //window.open('../../Search/Common/PopSearch.aspx?ID=select Name as ID,Description,Company from UDC where ProductCode='+ a + '  and UDCType='+"'"+b+"'"+' &tbname=' + c ,'Search','scrollBars=no,resizable=No,width=550,height=450,status=yes');
            wopen('AB_ViewColumns.aspx?ScrID=229&TBLName=' + varTable, 'Search', 480, 440);
            return false;
            //	wopen('AB_ViewColumns.aspx','UserView',500,450);
        }

        function wopen(url, name, w, h) {
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
				
    </script>

    <script type="text/javascript">
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";


        } 
        
 //Function is used to close the Tab
        function tabClose() {
            window.parent.closeTab();
        }	

    </script>

    <script type="text/javascript">
        //A Function to call on Page Load to set grid width according to screen size
        function onLoad() {
            var divCallView = document.getElementById('divCallView');
           if(document.body.clientWidth !=0)
           {
            divCallView.style.width = document.body.clientWidth - 30 + "px";
            }
        }
        //A Function to improve design i.e delete the extra cell of table
        function onEnd() {
            var x = document.getElementById('cpnlCallView_collapsible').cells[0].colSpan = "1";

        }
        //A Function is Called when we resize window
        window.onresize = onLoad;   
    </script>

</head>
<body style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
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
                                                    <asp:Button ID="BtnGrdSearch" runat="server" Width="0px" Height="0px" BackColor="#8AAFE5"
                                                        BorderColor="#8AAFE5" BorderStyle="None"></asp:Button>
                                                    <asp:ImageButton ID="imgbtnSearch" TabIndex="1" runat="server" Width="1px" Height="1px"
                                                        ImageUrl="white.GIF" CommandName="submit" AlternateText="."></asp:ImageButton>
                                                    <asp:Label ID="lblTitleLabel1" runat="server" CssClass="TitleLabel">ADDRESS BOOK</asp:Label>
                                                </td>
                                                <td style="width: 60%; text-align: center;" nowrap="nowrap">
                                                    <center>
                                                        <asp:UpdatePanel ID="upnltop" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="imgAdd" AccessKey="A" runat="server" ImageUrl="../../Images/s2Add01.gif"
                                                                    ToolTip="Add"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgEdit" AccessKey="E" runat="server" ImageUrl="../../Images/S2edit01.gif"
                                                                    ToolTip="Edit"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgReset" AccessKey="R" runat="server" ImageUrl="../../Images/reset_20.gif"
                                                                    ToolTip="Reset"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgSearch" AccessKey="H" runat="server" ImageUrl="../../Images/s1search02.gif"
                                                                    ToolTip="Search"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgShowAll" AccessKey="A" runat="server" ImageUrl="../../Images/Disabled.gif"
                                                                    ToolTip="Show All"></asp:ImageButton>
                                                                    <asp:ImageButton ID="imgPassportInfo" AccessKey="E" runat="server" ImageUrl="../../Images/editfiles.jpg"
                                                                    ToolTip="EditEmpInfo"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgDelete" AccessKey="D" runat="server" ImageUrl="../../Images/s2delete01.gif"
                                                                    ToolTip="Delete"></asp:ImageButton>
                                                                <img src="../../Images/s2close01.gif" title="Close" alt="" onclick="tabClose();"
                                                                    style="cursor: hand;" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </center>
                                                </td>
                                                <td style="width: 15%">
                                                    <font face="Verdana" size="1"><strong>&nbsp;&nbsp;<font face="Verdana" size="1"><strong>View&nbsp;
                                                        <asp:DropDownList ID="ddlstview" runat="server" Width="80px" Font-Size="XX-Small"
                                                            Font-Names="Verdana" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:ImageButton ID="imgPlus" AccessKey="P" runat="server" ImageUrl="../../Images/plus.gif">
                                                        </asp:ImageButton></strong></font>&nbsp;&nbsp;</strong></font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td nowrap="nowrap" style="width: 10%" background="../../Images/top_nav_back.gif"
                                        height="47">
                                        <img class="PlusImageCSS" id="Help" title="Help" onclick="ShowHelp('37','../../');"
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
                            <asp:UpdatePanel ID="upnlCallView" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <cc1:CollapsiblePanel ID="cpnlCallView" runat="server" Width="100%" BorderColor="Indigo"
                                        Visible="true" TitleCSS="test" PanelCSS="panel" TitleForeColor="black" TitleClickable="true"
                                        TitleBackColor="transparent" Text="Address Book" ExpandImage="../../Images/ToggleDown.gif"
                                        CollapseImage="../../Images/ToggleUp.gif" Draggable="False" BorderStyle="Solid"
                                        BorderWidth="0px">
                                        <div id="divCallView" style="overflow: auto; width: 1054px; height: 370pt">
                                            <table id="Table1261" bordercolor="activeborder" cellspacing="0" cellpadding="0"
                                                width="100%" align="left" border="0">
                                                <tr>
                                                    <td nowrap="nowrap">
                                                        <asp:Panel ID="Panel1" runat="server">
                                                            <asp:TextBox ID="TextBox1" runat="server" Width="15px" BorderStyle="None" Enabled="False"></asp:TextBox>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="left">
                                                        <!--  **********************************************************************-->
                                                        <asp:DataGrid ID="GrdAddSerach" runat="server" BorderStyle="None" BorderWidth="1px"
                                                            Font-Names="Verdana" ForeColor="MidnightBlue" BorderColor="Silver" CellPadding="0"
                                                            GridLines="Horizontal" HorizontalAlign="Left" PageSize="25" CssClass="Grid" DataKeyField="AddNo"
                                                            AllowPaging="True" PagerStyle-Visible="False" AllowSorting="True">
                                                            <SelectedItemStyle CssClass="GridSelectedItem"></SelectedItemStyle>
                                                            <AlternatingItemStyle CssClass="GridAlternateItem"></AlternatingItemStyle>
                                                            <ItemStyle Font-Size="8pt" ForeColor="#333333" CssClass="Griditem" BackColor="White">
                                                            </ItemStyle>
                                                            <HeaderStyle Font-Size="8pt" Font-Bold="True" ForeColor="Black" BackColor="#E0E0E0">
                                                            </HeaderStyle>
                                                            <FooterStyle ForeColor="DarkBlue" BackColor="White"></FooterStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="R">
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Width="13px"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="hyatt" Target="_blank" runat="server" ImageUrl="..\..\Images\Attach15_9.gif"></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <PagerStyle Visible="False" Position="TopAndBottom" Mode="NumericPages"></PagerStyle>
                                                        </asp:DataGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div>
                                            <asp:Panel ID="Panel7" runat="server">
                                                <table height="25">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="pg" Height="12pt" Width="40px" ForeColor="#0000C0" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="8pt" runat="server">Page</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="CurrentPg" runat="server" Height="12px" Width="10px" ForeColor="Crimson"
                                                                Font-Bold="True" Font-Size="X-Small"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="of" ForeColor="#0000C0" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt"
                                                                runat="server">of</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="TotalPages" runat="server" Height="12px" Width="10px" ForeColor="Crimson"
                                                                Font-Bold="True" Font-Size="X-Small"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="Firstbutton" runat="server" AlternateText="First" ImageUrl="../../Images/next9.jpg"
                                                                ToolTip="First"></asp:ImageButton>
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
                                                            <asp:Button ID="Button3" runat="server" Height="12pt" Width="16px" ForeColor="Navy"
                                                                Font-Bold="True" Font-Size="7pt" BorderStyle="None" ToolTip="Change Paging Size"
                                                                Text=">"></asp:Button>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblrecords" runat="server" Height="12pt" ForeColor="MediumBlue" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="8pt">Total Records</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="TotalRecods" runat="server" Height="12pt" ForeColor="Crimson" Font-Bold="True"
                                                                Font-Names="Verdana" Font-Size="8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>
                                    </cc1:CollapsiblePanel>
                                    <span style="display: none">
                                        <asp:Button ID="BtnGrdSearch1" runat="server" Height="0px" BorderWidth="0px" Width="0px" />
                                    </span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="PanelUpdate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMsg" runat="server">
                        </asp:Panel>
                        <asp:ListBox ID="lstError" runat="server" Width="752px" ForeColor="Red" Font-Names="Verdana"
                            Visible="false" Font-Size="XX-Small" BorderWidth="0" BorderStyle="Groove"></asp:ListBox>
                        <input type="hidden" name="txthidden" />
                        <input type="hidden" name="txthiddenImage" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
