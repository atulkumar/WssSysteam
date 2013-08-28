<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Security_Default" %>

<%@ Register Src="~/BaseClasses/Controls/ComboControl/CustomDDL.ascx" TagName="CustomDDL"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../../../Images/Js/StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <uc1:CustomDDL id="CustomDDL1" runat="server">
        </uc1:CustomDDL></div>
    </form>
</body>
</html>
