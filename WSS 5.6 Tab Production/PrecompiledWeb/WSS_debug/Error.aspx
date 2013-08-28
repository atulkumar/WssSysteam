<%@ page language="VB" autoeventwireup="false" inherits="_Error, App_Web_pch9tqya" enableEventValidation="false" theme="App_Themes" maintainScrollPositionOnPostBack="true" %>
<%@ Register TagPrefix="cc1" Namespace="CustomControls.Web" Assembly="CustomControls.Web" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>Error</title>
<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
<meta content="Visual Basic .NET 7.1" name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
<script language=JavaScript src="core.js" type=text/javascript></script>

<script language=JavaScript src="events.js" type=text/javascript></script>

<script language=JavaScript src="css.js" type=text/javascript></script>
		<LINK href="Images/js/StyleSheet1.css" type="text/css" rel="stylesheet">
<script language=JavaScript src="coordinates.js" type=text/javascript></script>

<script language=javascript src="Images/Js/JSValidation.js"></script>

<script language=JavaScript src="drag.js" type=text/javascript></script>

<style type=text/css>TR:hover {
	BACKGROUND-COLOR: #ffccff
}
TR.over {
	BACKGROUND-COLOR: #ffccff
}
TR:hover {
	BACKGROUND-COLOR: #ffccff
}
</style>

<script language=javascript>
						function SaveEdit(varImgValue)
				{
	
						if (varImgValue=='Logout')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									Form1.submit(); 
						}		
						if (varImgValue=='Help')
						{
									document.Form1.txthiddenImage.value=varImgValue;
									//Form1.submit(); 
									//wopen("Help/WSSHelp.aspx?ScreenID=9999","HomeHelp",500,500);
							var topPos;
							leftPos=(screen.width/2)-250;
							topPos=(screen.height/2)-250;
							window.open('Help/WSSHelp.aspx?ScreenID=9999','posA','overfilter="Alpha(opacity=75);";style=ScrollingSampStyle;toolbar=no, titlebar=no,width=500,height=555,top='+ topPos +',left='+leftPos);
						}		
						
						
				}		
				</script>
</HEAD>
<body bottomMargin=0 leftMargin=0 topMargin=0 onload=Hideshow(); rightMargin=0>
<form id=Form1 method=post runat="server">
<table height="100%" cellSpacing=0 cellPadding=0 width="100%" border=0>
  <tr>
    <td vAlign=top>
      <table cellSpacing=0 cellPadding=0 width="100%" border=0 
      >
        <tr>
          <td>
            <div align=right><IMG height=2 src="images/top_right_line.gif" width=96 ></DIV></TD></TR>
        <tr>
          <td>
            <table cellSpacing=0 cellPadding=0 width="100%" border=0 
            >
              <tr>
                <td background=images/top_left_back.gif 
                >&nbsp;</TD>
                <td width=50><IMG height=20 src="images/top_right.gif" width=50 ></TD>
                <td width=21><A href="#" ><IMG height=20 src="images/bt_min.gif" width=21 border=0 ></A></TD>
                <td width=21><A href="#" ><IMG height=20 src="images/bt_max.gif" width=21 border=0 ></A></TD>
                <td width=19><A href="#" ><IMG onclick=CloseWSS(); height=20 src="images/bt_clo.gif" width=19 border=0 ></A></TD>
                <td width=6><IMG height=20 src="images/bt_space.gif" width=6 ></TD></TR></TABLE></TD></TR>
        <tr>
          <td>
            <table cellSpacing=0 cellPadding=0 width="100%" border=0 
            >
              <tr>
                <td background=images/top_nav_back.gif height=67 
                >
                  <table cellSpacing=0 cellPadding=0 width="94%" align=center 
                  border=0>
                    <TR>
                      <TD align=left><asp:button id=BtnGrdSearch runat="server" Height="0" Width="0px"></asp:button><asp:imagebutton id=imgbtnSearch tabIndex=1 runat="server" Height="1px" Width="1px" AlternateText="." CommandName="submit" ImageUrl="images/white.GIF"></asp:imagebutton><IMG class=PlusImageCSS onclick=HideContents(); alt=Hide src="Images/left005.gif" name=imgHide > 
                        <IMG class=PlusImageCSS onclick=ShowContents(); alt=Show src="Images/Right005.gif" name=ingShow > <asp:label id=lblTitleLabelWssHome runat="server" Height="12px" Width="128px" CssClass="HeaderTestMenu" >WSS</asp:label></TD>
                      <TD align=right 
                  >&nbsp;</TD></TR></TABLE></TD>
                <td align=right width=152 background=images/top_nav_back01.gif 
                height=67>&nbsp; <IMG class=PlusImageCSS id=Logout title=Logout onclick=LogoutWSS(); alt=E src="icons/logoff.gif" border=0 name=tbrbtnEdit >&nbsp;&nbsp;&nbsp;</TD></TR></TABLE>
        <tr>
          <td height=10>
            <table cellSpacing=0 cellPadding=0 width="100%" border=0 
            >
              <tr>
                <td background=images/main_line.gif height=10 
                ><IMG height=10 src="images/main_line.gif" width=6 ></TD>
                <td width=7 height=10><IMG height=10 src="images/main_line01.gif" width=7 ></TD></TR></TABLE></TD></TR>
        <tr>
          <td height=2>
            <table cellSpacing=0 cellPadding=0 width="100%" border=0 
            >
              <tr>
                <td background=images/main_line02.gif height=2 
                ><IMG height=2 src="images/main_line02.gif" width=2 ></TD>
                <td width=12 height=2><IMG height=2 src="images/main_line03.gif" width=12 ></TD></TR></TABLE></TD></TR>
        <tr>
          <td>
            <TABLE id=Table16 borderColor=activeborder cellSpacing=0 
            cellPadding=0 width="100%" border=0>
              <TR>
											<TD vAlign="top" colSpan="1">
												<!--  **********************************************************************-->
                  <DIV style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 581px" 
                  >
                  <table style="BORDER-COLLAPSE: collapse" width="100%" border=0 
                  >
                    <tr>
                      <td colSpan=2></TD></TR>
                    <TR>
                      <td vAlign=top align=center width="48%" 
                      >
																<!-- *****************************************--><br 
                        ><br><br 
                        ><br><br 
                        ><br><asp:label id=lblTitleLabelLabel1 runat="server">Server is unable to process your request. Sorry for Inconvenience.</asp:Label><BR 
                        ><BR><BR 
                        ><asp:image id=Image1 runat="server" ImageUrl="Images/error_page.gif"></asp:Image><BR 
                        >
                        <P></P>
                        <P><asp:label id=lblTitleLabelLabel2 runat="server">E-mail has been sent to administrator. Please try Later.</asp:Label>
																				<!-- *****************************************--></P></TD></TR></TABLE></DIV>
											</TD>
                <td vAlign=top width=12 background=images/main_line04.gif 
                ><IMG height=1 src="images/main_line04.gif" width=12 ></TD></TR></TABLE>
            <DIV></DIV></TD></TR>
        <tr>
          <td height=2>
            <table cellSpacing=0 cellPadding=0 width="100%" border=0 
            >
              <tr>
                <td background=images/main_line06.gif height=2 
                ><IMG height=2 src="images/main_line06.gif" width=2 ></TD>
                <td width=12 height=2><IMG height=2 src="images/main_line05.gif" width=12 ></TD></TR></TABLE></TD></TR>
        <tr>
          <td>
            <table cellSpacing=0 cellPadding=0 width="100%" border=0 
            >
              <tr>
                <td background=images/bottom_back.gif 
                >&nbsp;</TD>
											<td width="66"><IMG height="31" src="images/bottom_right.gif" width="66"></td></TR></TABLE></TD></TR></TABLE></TD></TR></TABLE>
			<INPUT type="hidden" name="txthidden"> <INPUT type="hidden" name="txthiddenImage"> </FORM>
	</body>
</html>
