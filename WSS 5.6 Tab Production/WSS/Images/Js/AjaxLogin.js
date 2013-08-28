		///***********************Login AJAX**********************************////////
		var gtype;
		var xmlHttp; 
		var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
		var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
		var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
		//netscape, safari, mozilla behave the same??? 
		var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 

		function GetCompany()
		{
				document.getElementById('txtCompName').value='';
				document.getElementById('txtRole').value='';
				document.getElementById('txtCompNameName').value='';
				document.getElementById('txtRoleName').value='';
				var strUser=document.getElementById('txtUserName').value;
				var pwd=document.getElementById('txtPassword').value;
				document.getElementById("ddlCompany").options.length=1;
				document.getElementById("ddlRole").options.length=1;
		        if (strUser.length > 0 && pwd.length > 0 )
		        { 
						
						
					   var url= '../AJAX Server/GetLoginInfo.aspx?Type=COMP&Password='+ pwd +'&UserID=' + strUser+'&Rnd='+Math.random();
					    gtype='COMP';
						xmlHttp = GetXmlHttpObject(stateChangeHandler);    
//						alert(xmlHttp.readyState);
						xmlHttp_Get(xmlHttp, url); 
					//alert(xmlHttp.readyState);
					var i=0;
					//alert('hhh'+i);
					//var w=window.open(' ','text','width=100,height=100,fullscreen=0');
					
					for(i=0;i<=500;i++)
					{
					//alert(xmlHttp.readyState);
					if  (xmlHttp.readyState ==4)
					{
					//	alert('hhh'+w);
					//w.close();	
						
					return;
					}
						i+=1;
//						alert('hhh'+i);
					}     

				}
	        	else 
	        	{ 
						//Invalid user
				} 
		}
		function GetRole()
		{
				document.getElementById('txtCompName').value='';
				document.getElementById('txtRole').value='';
				document.getElementById('txtCompNameName').value='';
				document.getElementById('txtRoleName').value='';
				
				document.getElementById("ddlRole").options.length=1;
				var ddl=document.getElementById('ddlCompany');
				var strUser;
				strUser=ddl.options(ddl.options.selectedIndex).value;
		        if (strUser.length > 0)
		        { 
					   var url = '../AJAX Server/GetLoginInfo.aspx?Type=ROLE&UserID='+ document.getElementById('txtUserName').value +'&CompID='+ strUser;
				        gtype='ROLE';
						xmlHttp = GetXmlHttpObject(stateChangeHandler); 
						xmlHttp_Get(xmlHttp, url); 
						
				}
	        	else 
	        	{ 
						//Invalid user
				} 
		}
		 
		function FillHidden()
		{
			var ddlComp=document.getElementById('ddlCompany');
			var ddlRole=document.getElementById('ddlRole');
			document.getElementById('txtCompName').value=ddlComp.options(ddlComp.options.selectedIndex).value;
			document.getElementById('txtRole').value=ddlRole.options(ddlRole.options.selectedIndex).value;
			document.getElementById('txtCompNameName').value=ddlComp.options(ddlComp.options.selectedIndex).text;
			document.getElementById('txtRoleName').value=ddlRole.options(ddlRole.options.selectedIndex).text;
		}
		 
		function stateChangeHandler() 
		 { 	
				if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
				{ 
						var response = xmlHttp.responseXML; 
						//alert(xmlHttp.responseText);
						var info = response.getElementsByTagName("INFO");
						if ( gtype=='COMP' )
						document.getElementById("ddlCompany").options.length=1;
						else
						document.getElementById("ddlRole").options.length=1;
						var item = info[0].getElementsByTagName("ITEM");
						if(info.length > 0)
						{
								for (var i = 0; i < item.length; i++)
								{
										//var nodeText = document.createTextNode(user[i].getAttribute("CompID"));
										if ( item[i].getAttribute("Name").length > 0  && item[i].getAttribute("ID").length > 0  )
										{
												var objForm = document.Form1;
												var objNewOption = document.createElement("OPTION");
												if ( gtype=='COMP' )
												{document.getElementById("ddlCompany").options.add(objNewOption);}
												else
												{document.getElementById("ddlRole").options.add(objNewOption);}
												
												if ( item[i].getAttribute("Name").length > 0 )
												{objNewOption.innerText = item[i].getAttribute("Name");}
												if ( item[i].getAttribute("ID").length > 0 )
												{objNewOption.value = item[i].getAttribute("ID");}
										}
								}
						}
				} 
		} 
		
		
		function xmlHttp_Get(xmlhttp, url) 
		{ 
		        xmlhttp.open('GET', url, true); 
		        xmlhttp.send(null); 
		       
		} 
    
		function GetXmlHttpObject(handler) 
		{ 
				var objXmlHttp = null;    //Holds the local xmlHTTP object instance 
		        if (is_ie)
		        { 
						var strObjName = (is_ie5) ? 'Microsoft.XMLHTTP' : 'Msxml2.XMLHTTP'; 
				        try
				        { 
								objXmlHttp = new ActiveXObject(strObjName); 
								objXmlHttp.onreadystatechange = handler; 
						} 
						catch(e)
						{ 
								alert('IE detected, but object could not be created. Verify that active scripting and activeX controls are enabled'); 
								return; 
			            } 
				} 
				else if (is_opera)
				{ 
						alert('Opera detected. The page may not behave as expected.'); 
						return; 
				} 
				else
				{ 
						objXmlHttp = new XMLHttpRequest(); 
						objXmlHttp.onload = handler; 
						objXmlHttp.onerror = handler; 
				} 
				return objXmlHttp; 
		} 

		///**************************Login AJAX end*********************************///////