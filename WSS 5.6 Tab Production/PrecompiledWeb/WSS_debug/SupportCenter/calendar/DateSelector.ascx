<%@ control language="vb" autoeventwireup="false" inherits="DateSelector, App_Web_crjyjl9r" %>
<LINK rel="stylesheet" type="text/css" href="../../Images/Js/StyleSheet1.css">
<asp:Panel Runat="server" ID="PnlCalender" BorderWidth=0 Wrap=False Height=18px>
	<asp:textbox id="txt_Date" ReadOnly="True" BorderStyle="Solid" Height="18px" CssClass="txtNoFocus"
		Font-Size="XX-Small" Font-Names="Verdana" MaxLength="10" Wrap="False"	
		runat="server"></asp:textbox><asp:image id="imgCalendar" CssClass="PlusImageCSS" Width="15px" runat="server" AlternateText="Select Date"
		ImageUrl="iconPicDate.gif"></asp:image>
</asp:Panel>