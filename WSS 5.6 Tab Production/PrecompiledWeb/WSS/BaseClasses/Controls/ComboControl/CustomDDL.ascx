<%@ control language="vb" autoeventwireup="false" inherits="CustomDDL, App_Web_e44ssakp" %>

<LINK rel="stylesheet" type="text/css" href="../../../Images/Js/StyleSheet1.css">
<script language="javascript">
var gDDLID;
var gUDC;
var gPOST;
	function SelectDDL(ID, URL, name, width, height,UDC,POST)
	{
		gPOST=POST;
		gUDC=UDC;
		gDDLID=ID;
		var n;
		var Query;
		Query=document.getElementById(ID + '_txtHIDQuery').value;
		URL=URL + '?ID=' + Query;
		n=document.getElementById(ID +'_DDL').options.selectedIndex;
		if ( document.getElementById(ID+ '_DDL').options(n).text=='More...' )
		{
			//document.getElementById(ID+ '_DDL').options(0).text='';
			//document.getElementById(ID+ '_DDL').options(0).value='';
			//document.getElementById(ID+ '_DDL').options.selectedIndex=0;
								Afilename = document.getElementById(gDDLID + '_txtHID').value;
								strName = document.getElementById(gDDLID + '_txtHIDName').value;
			
								var bln;
								bln=0;
								var intI;
								intI=document.getElementById(gDDLID + '_DDL').options.length;
								for ( i=0; i<intI; i++)
								{
									if(document.getElementById(gDDLID + '_DDL').options(i).value==Afilename)
									{
										document.getElementById(gDDLID + '_DDL').options.selectedIndex=i;
										bln=1;	
									}
								}	
								if ( bln== 0)
								{
										document.getElementById(gDDLID + '_DDL').options(0).text=strName;
										document.getElementById(gDDLID + '_DDL').options(0).value=Afilename;
										document.getElementById(gDDLID + '_DDL').options.selectedIndex=0;
								}

								
			
			
			wopen(URL, name, 480, 450);
		}
		else
		{
		
			document.getElementById(ID + '_txtHID').value=document.getElementById(ID+ '_DDL').options(n).value;
			document.getElementById(ID + '_txtHIDName').value=document.getElementById(ID+ '_DDL').options(n).text;
			if(gPOST==1)
			{
				document.Form1.submit();
			}
			
		}
		
		
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
	function addToParentList(Afilename,TbName,strName)
				{	
				//alert(Afilename);
					//alert(strName);
					//alert(gUDC);
				if (gUDC==1)
				{	
					strName=Afilename;
				}
					if (Afilename != "" || Afilename != 'undefined')
						{
						//alert(Afilename);
								var bln;
								bln=0;
								var intI;
								intI=document.getElementById(gDDLID + '_DDL').options.length;
								for ( i=0; i<intI; i++)
								{
									if(document.getElementById(gDDLID + '_DDL').options(i).value==Afilename)
									{
										document.getElementById(gDDLID + '_DDL').options.selectedIndex=i;
										bln=1;	
									}
								}	
								if ( bln== 0)
								{
										document.getElementById(gDDLID + '_DDL').options(0).text=strName;
										document.getElementById(gDDLID + '_DDL').options(0).value=Afilename;
										document.getElementById(gDDLID + '_DDL').options.selectedIndex=0;
								}
								document.getElementById(gDDLID + '_txtHID').value=Afilename;
								document.getElementById(gDDLID + '_txtHIDName').value=strName;
				
		
								if(gPOST==1)
								{
									document.Form1.submit();
								}	
						}
			
				}
	
	
</script>
<asp:DropDownList id="DDL" runat="server" Width="88px" Height="18px" CssClass=txtNoFocus></asp:DropDownList>
<input type="hidden" id="txtHID" name="HIDDDL" runat="server" style="WIDTH: 0px">
<input type="hidden" id="txtHIDName" name="HIDDDLName" runat="server" style="WIDTH: 0px">
<input type="hidden" id="txtHIDQuery" name="HIDQuery" runat="server" style="WIDTH: 0px">
<input type="hidden" id="txtHIDUDC" name="HIDUDC" runat="server" style="WIDTH: 0px">
