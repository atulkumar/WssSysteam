

		function FOCUS(txtID)
		{
				document.getElementById(txtID).className ="txtFocus";
		}
		function NOFOCUS(txtID)
		{
				document.getElementById(txtID).className ="txtNoFocus";
		}

		function FEFOCUS(txtID)
		{
				document.getElementById(txtID).className ="txtFocusFE";
		}
		function FENOFOCUS(txtID)
		{
				document.getElementById(txtID).className ="txtNoFocusFE";
		}

		

			function NumericOnly()
			{
	//		alert(event.keyCode);
				if((event.keyCode<13 || event.keyCode>13) && (event.keyCode<48 || event.keyCode>57))
				{
					event.returnValue = false;
					alert("Please Enter Numerics Only");
				}			
			}
		
		/////////////////a-z A-Z Validation////////
		
		function CharacterOnly()
			{
	//		alert(event.keyCode);
				//if(((event.keyCode<65 || event.keyCode>122))&&((event.keyCode<32)||(event.keyCode>32))||(event.keyCode==94)||(event.keyCode==96)||(event.keyCode==91)||(event.keyCode==92)||(event.keyCode==93))
				if(event.keyCode>32 && event.keyCode<65)
				{
					event.returnValue = false;
					alert("Please Enter Characters Only");
				}
			}			
///////////////////////////Phoone no validation/////////////
			function PhoneValidation()
			{
	//		alert(event.keyCode);
				if(event.keyCode<48 || event.keyCode>57)
				{
					event.returnValue = false;
					alert("Please Enter Numerics Only");
				}			
			}

//////////////////////////end phone validation///////////////
/////////////New Mail Validation Function///////////////
function mail(mailid,obj)
{
		mailid=mailid.replace(/^\s*([\S\s]*)\b\s*$/, '$1');
		if ( checkmail(mailid)==true )
		{
			return true;
		}
		else
		{
			alert('Please enter valid E-Mail Address');
			obj.focus();
		}

}

//////////////////////////////Email Validation/////////////////////////

function checkmail(mailid)
{
			if ( mailid.length>0)
			{
					if (mailid.indexOf(' ')==-1)
					{
							id=mailid;
							var at;
							at=id.indexOf('@');
							if ( at!=-1 )
							{
									if (id.substring(0,at).length > 0)
									{
														var aat;
														aat=id.substring(at+1,id.length);
														
														if ( aat.length >0 )
														{
																	var dot;
																	dot=aat.lastIndexOf('.');
																	if ( dot>0)
																	{
																					var adot;
																					adot=aat.substring(dot+1,aat.length);
																					if (adot.length>0)
																					{
																					
																									if (adot.length>5)
																									{
																												return false;
																									}
																									else
																									{
																												return true;
																									}
																					}
																					else
																					{
																								return false;
																					}
																	}
																	else
																	{
																				return false;
																	}
														
														}
														else
														{
																	return false;
														}
										}
										else
										{
													return false;
										}				
							}
							else
							{
										return false;
							}
					}
					else
					{
								return false;
					}			
					
			}
			else
			{
						return true;	
			}
}

function checkMailId(parm)
{
var p1;
p1=parm;
var arr = new Array('.com','.net','.org','.biz','.coop','.info','.museum','.name','.pro','.edu','.gov','.int','.mil','.ac','.ad','.ae','.af','.ag','.ai','.al','.am','.an','.ao','.aq','.ar','.as','.at','.au','.aw','.az','.ba','.bb','.bd','.be','.bf','.bg','.bh','.bi','.bj','.bm','.bn','.bo','.br','.bs','.bt','.bv','.bw','.by','.bz','.ca','.cc','.cd','.cf','.cg','.ch','.ci','.ck','.cl','.cm','.cn','.co','.cr','.cu','.cv','.cx','.cy','.cz','.de','.dj','.dk','.dm','.do','.dz','.ec','.ee','.eg','.eh','.er','.es','.et','.fi','.fj','.fk','.fm','.fo','.fr','.ga','.gd','.ge','.gf','.gg','.gh','.gi','.gl','.gm','.gn','.gp','.gq','.gr','.gs','.gt','.gu','.gv','.gy','.hk','.hm','.hn','.hr','.ht','.hu','.id','.ie','.il','.im','.in','.io','.iq','.ir','.is','.it','.je','.jm','.jo','.jp','.ke','.kg','.kh','.ki','.km','.kn','.kp','.kr','.kw','.ky','.kz','.la','.lb','.lc','.li','.lk','.lr','.ls','.lt','.lu','.lv','.ly','.ma','.mc','.md','.mg','.mh','.mk','.ml','.mm','.mn','.mo','.mp','.mq','.mr','.ms','.mt','.mu','.mv','.mw','.mx','.my','.mz','.na','.nc','.ne','.nf','.ng','.ni','.nl','.no','.np','.nr','.nu','.nz','.om','.pa','.pe','.pf','.pg','.ph','.pk','.pl','.pm','.pn','.pr','.ps','.pt','.pw','.py','.qa','.re','.ro','.rw','.ru','.sa','.sb','.sc','.sd','.se','.sg','.sh','.si','.sj','.sk','.sl','.sm','.sn','.so','.sr','.st','.sv','.sy','.sz','.tc','.td','.tf','.tg','.th','.tj','.tk','.tm','.tn','.to','.tp','.tr','.tt','.tv','.tw','.tz','.ua','.ug','.uk','.um','.us','.uy','.uz','.va','.vc','.ve','.vg','.vi','.vn','.vu','.ws','.wf','.ye','.yt','.yu','.za','.zm','.zw','.ik'); 
		var mai = document.getElementById(p1).value;
		
		var bln;
		bln=mail(mai);
		if( bln==true)
		{
			return true;
		}
		else
		{
			return false;
		}
}

/////////////////////////end  emailvalidation//////////////


/////////////////////////UsedHour//////////////

		function UsedHour(ControlID)
		{			
			var Val;
			Val = document.getElementById(ControlID).value;
			//alert(Val.indexOf('.'));
			
			if ( Val.indexOf('.') == -1 )
			{
					if ( Val.length > 2 )
					{
							if ( event.keyCode != 46 && (event.keyCode!=13) )
							{
									event.returnValue=false;
							}
					}
					else
					{
					
					
					
					}
			
			}
			else
			{
					if ( event.keyCode==46 )
					{
						event.returnValue=false;
					}
					else
					{
							if ( Val.substr(Val.indexOf('.')).length >2  && (event.keyCode!=13))
							{
									//alert(Val.substr(Val.indexOf('.')).length >2 );
									event.returnValue=false;
							}
					}		
		
			}
		
			if((event.keyCode<13 || event.keyCode>13) && (event.keyCode<46 || event.keyCode>46) &&( event.keyCode<48 || event.keyCode>57))
						{
							event.returnValue = false;
							alert("Please Enter Used Hours In Numerics Only!");
						}	
		
		
		
		
			
		
			}
			
			

		function UsedHour1(ControlID)
		{			
			var Val;
			Val = document.getElementById(ControlID).value;
			//alert(Val.indexOf('.'));
			
			
			if ( Val.length>2 && Val.indexOf('.')==-1 )
			{
					event.returnValue = false;
			}
			else
			{
			
					var temp;
					if ( Val.indexOf('.')>=0)
					{
					temp=Val.substr(Val.indexOf('.'));
					//alert(temp);
						if ( temp.length > 2 && (event.keyCode!=13) )
						{	
							event.returnValue = false;
							//alert(temp);
						}
					}
					if (Val.indexOf('.')>0 && event.keyCode==46 )
					{
						event.returnValue = false;
					}
					
					if((event.keyCode<13 || event.keyCode>13) && (event.keyCode<46 || event.keyCode>46) &&( event.keyCode<48 || event.keyCode>57))
						{
							event.returnValue = false;
							alert("Please Enter Used Hours In Numerics Only!");
						}			
				}
			}
			////////////////////End Used Hours/////////////////////////
			
			
		function FloatData(ControlID)
			{
			var Val;
			Val = document.getElementById(ControlID).value;
			if (Val.indexOf('.')>0 && event.keyCode==46 )
			{
				event.returnValue = false;
			}
			
				if((event.keyCode<13 || event.keyCode>13) && (event.keyCode<46 || event.keyCode>46) &&( event.keyCode<48 || event.keyCode>57))
				{
					event.returnValue = false;
					alert("Please Enter Numerics Only!");
				}			
			}
		
		
		
			function HideContents()
				{
					parent.document.all("SideMenu1").cols="0,*";
					document.Form1.imgHide.style.visibility = 'hidden'; 
					document.Form1.ingShow.style.visibility = 'visible'; 				
				}
					
			function ShowContents()
				{
					document.Form1.ingShow.style.visibility = 'hidden'; 
					document.Form1.imgHide.style.visibility = 'visible'; 
					parent.document.all("SideMenu1").cols="163,*";					
				}
					
			function Hideshow()
				{
				
//					if (parent.document.all("SideMenu1").cols =="0,*")
//					{
//							document.Form1.imgHide.style.visibility = 'hidden'; 
//							document.Form1.ingShow.style.visibility = 'visible'; 
//					}
//					else
//					{
//							document.Form1.ingShow.style.visibility = 'hidden'; 
//							document.Form1.imgHide.style.visibility = 'visible'; 
//					}
				}		
				
		function CloseWSS()
		{
				var con;
				con=window.confirm('Are You Sure You Want To Quit WSS');

				if ( con==true )
				{
				//				alert(con);
					window.parent.close();
				}
		}
	
		function Minimize() 
		{
			self.parent.Minimize() 
			//self.resizeTo(0,0);
		}

		function Maximize() 
		{
			self.parent.Maximize() 
		}

		function ShowHelp(ScreenID,Path)
		{
			openHelp(Path + 'Help/WSSHelp.aspx?ScreenID='+ScreenID,'HelpPopup' +ScreenID,900,600);
		}	

		function openHelp(url, name, w, h)
			{
				// Fudge factors for window decoration space.
				// In my tests these work well on all platforms & browsers.
				w += 32;
				h += 96;
				wleft = (screen.width - w) / 2;
				wtop = (screen.height - h) / 2;
				var win = window.open (url,
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

//////functino for limiting the number of chracters in a multiline textboox

function MaxLength(ID,N)
{
		if(document.getElementById(ID).value.length>N)
		{
			if( event.keyCode!=13)
			event.returnValue = false;
		}
		else
		{
			event.returnValue = true;
		}
}

//////////////////////////////////////////////////////////
	///////////////Logout///////////////////

		function LogoutWSS()
		{		
				var res;
				res=window.confirm('Are you sure you want to Logout WSS?');
				if ( res==true )
				{	
						document.Form1.txthiddenImage.value='Logout';
						document.Form1.submit(); 
				}
		}


//////////////////////////////////////

//*************************************************************


		function ShowToolTip(ID,Count)
		{
			ID.title='No. of characters : '+ ID.value.length + '\nNo. of Allowed characters:'+Count ;
		}
		
		
			
	
	function checkCapsLock(  ) {
	var e=event;
	var myKeyCode=0;
	var myShiftKey=false;
	var myMsg='Caps Lock is On.\n\nTo prevent entering your password incorrectly,\nyou should press Caps Lock to turn it off.';

	// Internet Explorer 4+
	if ( document.all ) {
		myKeyCode=e.keyCode;
		myShiftKey=e.shiftKey;

	// Netscape 4
	} else if ( document.layers ) {
		myKeyCode=e.which;
		myShiftKey=( myKeyCode == 16 ) ? true : false;

	// Netscape 6
	} else if ( document.getElementById ) {
		myKeyCode=e.which;
		myShiftKey=( myKeyCode == 16 ) ? true : false;

	}

	// Upper case letters are seen without depressing the Shift key, therefore Caps Lock is on
	if ( ( myKeyCode >= 65 && myKeyCode <= 90 ) && !myShiftKey ) {
		alert( myMsg );

	// Lower case letters are seen while depressing the Shift key, therefore Caps Lock is on
	} else if ( ( myKeyCode >= 97 && myKeyCode <= 122 ) && myShiftKey ) {
		alert( myMsg );

	}
}
		//RVS
		function CopyTOClipBoard(fieldname) 
		{ 
	    	//var selectedText = document.getElementById(fieldname).value; 
	    	var listObject = document.getElementById(fieldname);
	    	var strSelected="";
	    	for (var i=0;i<listObject.options.length;i++)
	    	{
	    	    if (i==0 )
	    	    {
	    	        strSelected = listObject.options[i].value;
	    	    }
	    	    else
	    	    {
	    	        strSelected = strSelected + '\n' +listObject.options[i].value;
	    	    }
	    	    
	    	}
		    window.clipboardData.setData("Text", strSelected);
		} 			

		