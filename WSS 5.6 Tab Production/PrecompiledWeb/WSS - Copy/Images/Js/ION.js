

// This date is used throughout to determine today's date.

    var IONDateNow = new Date(Date.parse(new Date().toDateString()));


    //      var IONBaseYear = 2000;

    var IONBaseYear        = IONDateNow.getFullYear()-10;

    // How many years do want to be valid and to show in the drop-down list?

    var IONDropDownYears   = 20;

    // All language dependent changes can be made here...

    var IONToday               = 'Today:',
        IONInvalidDateMsg      = 'The entered date is invalid.\n',
        IONOutOfRangeMsg       = 'The entered date is out of range.',
        IONDoesNotExistMsg     = 'The entered date does not exist.',
        IONInvalidAlert        = ['Invalid date (',') ignored.'],
        IONDateDisablingError  = ['Error ',' is not a Date object.'],
        IONRangeDisablingError = ['Error ',' should consist of two elements.'],
        IONArrMonthNames       = ['01','02','03','04','05','06',
                                  '07','08','09','10','11','12'],
        IONArrWeekInits        = ['S','M','T','W','T','F','S'];

    // Note:  Always start the IONArrWeekInits array with your string for
    //        Sunday whatever IONWeekStart (below) is set to.

    // IONWeekStart determines the start of the week in the display
    // Set it to: 0 (Zero) for Sunday, 1 (One) for Monday etc..

    var IONWeekStart       =    0;

    // Set the allowed date delimiters here...
    // E.g. To set the rising slash, hyphen, full-stop (aka stop or point)
    //      and comma as delimiters use
    //              var IONArrDelimiters   = ['/','-','.',','];

    var IONArrDelimiters   = ['/','-','.',','];

    // IONZindex controls how the pop-up calendar interacts with the rest
    // of the page.  It is usually adequate to leave it as 1 (One) but I
    // have made it available here to help anyone who needs to alter the
    // level in order to ensure that the calendar displays correctly in
    // relation to all other elements on the page.

    var IONZindex          = 1;

    // Personally I like the fact that entering 31-Sep-2005 displays
    // 1-Oct-2005, however you may want that to be an error.  If so,
    // set IONBlnStrict = true.  That will cause an error message to
    // display and the selected month is displayed without a selected
    // day. Thanks to Brad Allan for his feedback prompting this feature.

    var IONBlnStrict       = false;

    // If you wish to disable any displayed day, e.g. Every Monday
    // you can do it by setting the following array.  The array elements
    // match the displayed cells.
    //
    // You could put something like the following in your calling page
    // to disable all weekend days;
    //
    //  for (var i=0;i<IONEnabledDay.length;i++)
    //      {if (i%7==0 || i%7==6)  // Disable all weekend days
    //          {IONEnabledDay[i] = false;}
    //      }

    var IONEnabledDay      = [true, true, true, true, true, true, true,
                              true, true, true, true, true, true, true,
                              true, true, true, true, true, true, true,
                              true, true, true, true, true, true, true,
                              true, true, true, true, true, true, true,
                              true, true, true, true, true, true, true];

    // You can disable any specific date by creating an element of the
    // array IONDisabledDates as a date object with the value you want
    // to disable.  Date ranges can be disabled by placing an array
    // of two values (Start and End) into an element of this array.

    var IONDisabledDates   = new Array();

    // e.g. To disable 10-Dec-2005:
    //          IONDisabledDates[0] = new Date(2005,11,10);
    //
    //      or a range from 2004-Dec-25 to 2005-Jan-01:
    //          IONDisabledDates[1] = [new Date(2004,11,25),new Date(2005,0,1)];
    //
    // Remember that Javascript months are Zero-based.

    // The disabling by date and date range does prevent the current day
    // from being selected.  Disabling days of the week does not so you can set
    // the IONActiveToday value to false to prevent selection.

    var IONActiveToday = true;

    // Closing the calendar by clicking on it (rather than elsewhere on the
    // main page) can be inconvenient.  The IONClickToHide boolean value
    // controls this feature.

    var IONClickToHide = false;

    // Blend the colours into your page here...

    var IONBackground           = 'Highlight';    // Calendar background
    var IONHeadText             = '#CCCCCC';    // Colour of week headings

    // If you want to "turn off" any of the highlighting then just
    // set the highlight colours to the same as the un-higlighted colours.

    // Today string
    var IONTodayText            = 'lavender',
        IONTodayHighlight       = 'yellow';

    // Active Cell
    var IONHighlightText        = 'red',
        IONHighlightBackground  = 'lavender';

    // Weekend Days
    var IONWeekendText          = '#CC6666',
        IONWeekendBackground    = 'White';

    // Days out of current month
    var IONExMonthText          = '#999999',
        IONExMonthBackground    = '#CCCCCC';

    // Current month's weekdays
    var IONCellText             = 'highlight',
        IONCellBackground       = 'white';

    // Input date
    var IONInDateText           = 'white',
        IONInDateBackground     = 'Highlight';

    // Disabled days
    var IONDisabledDayText       = '#993333',
        IONDisabledDayBackground = '#999999';


    // I have made every effort to isolate the pop-up script from any
    // CSS defined on the main page but if you have anything set that
    // affects the pop-up (or you may want to change the way it looks)
    // then you can address it here.
    //
    // The classes are;
    //      ION         Overall
    //      IONHead     The Selection buttons/drop-downs
    //      IONWeek     The Day Initials (Column Headings)
    //      IONCells    The Individual days
    //      IONFoot     The "Today" selector

    document.writeln("<style>");
    document.writeln(   '.ION       {padding:1px;vertical-align:middle;}');
    document.writeln(   'iframe.ION {position:absolute;z-index:' + IONZindex    +
                                    ';top:0px;left:0px;visibility:hidden;'      +
                                    'width:1px;height:1px;}');
    document.writeln(   'table.ION  {padding:0px;visibility:hidden;'            +
                                    'position:absolute;width:200px;'            +
                                    'top:0px;left:0px;z-index:' + (IONZindex+1) +
                                    ';text-align:center;'                       +
                                    'padding:1px;vertical-align:middle;'        +
                                    'background-color:' + IONBackground         +
                                    ';border:ridge 2px;font-size:10pt;'         +
                                    'font-family:Arial,Helvetica,Sans-Serif;'   +
                                    'font-weight:bold;}');
    document.writeln(   'td.IONHead     {padding:0px 0px;text-align:center;}');
    document.writeln(   'select.IONHead {margin:3px 1px;}');
    document.writeln(   'input.IONHead  {height:22px;width:22px;'               +
                                        'vertical-align:middle;'                +
                                        'text-align:center;margin:2px 1px;'     +
                                        'font-size:10pt;font-family:fixedSys;'  +
                                        'font-weight:bold;}');
    document.writeln(   'tr.IONWeek     {text-align:center;font-weight:bold;'   +
                                        'color:' + IONHeadText + ';}');
    document.writeln(   'td.IONWeek     {padding:0px;}');
    document.writeln(   'table.IONCells {text-align:right;font-size:8pt;'       +
                                        'width:96%;font-family:'                +
                                        'Arial,Helvetica,Sans-Serif;}');
    document.writeln(   'td.IONCells {padding:3px;vertical-align:middle;'       +
                                     'width:16px;height:16px;font-weight:bold;' +
                                     'color:' + IONCellText                     +
                                     ';background-color:' + IONCellBackground   +
                                     '}');
    document.writeln(   'td.IONFoot  {padding:0px;text-align:center;'           +
                                     'font-weight:normal;color:'                +
                                      IONTodayText + ';}');
    document.writeln("</style>");

    // You can modify the input, display and output date formats in the
    // following three functions;

    function IONInputFormat(IONArrInput,IONEleValue)
        {var IONArrSeed = new Array();

         IONBlnFullInputDate = false;

         switch (IONArrInput.length)
            {case 1:
                {// Year only entry
                 IONArrSeed[0] = parseInt(IONArrInput[0],10);   // Year
                 IONArrSeed[1] = '6';                           // Month
                 IONArrSeed[2] = 1;                             // Day
                 break;
                }
             case 2:
                {// Year and Month entry
                 IONArrSeed[0] = parseInt(IONArrInput[1],10);   // Year
                 IONArrSeed[1] = IONArrInput[0];                // Month
                 IONArrSeed[2] = 1;                             // Day
                 break;
                }
             case 3:
                {// Day Month and Year entry
                 IONArrSeed[0] = parseInt(IONArrInput[2],10);   // Year
                 IONArrSeed[1] = IONArrInput[1];                // Month
                 IONArrSeed[2] = parseInt(IONArrInput[0],10);   // Day

                 // for Month, Day and Year entry use...
                 //   IONArrSeed[0] = parseInt(IONArrInput[2],10);  // Year
                 //   IONArrSeed[1] = IONArrInput[0];               // Month
                 //   IONArrSeed[2] = parseInt(IONArrInput[1],10);  // Day

                 IONBlnFullInputDate = true;
                 break;
                }
             default:
                {// A stuff-up has led to more than three elements in the date.
                 IONArrSeed[0] = 0;     // Year
                 IONArrSeed[1] = 0;     // Month
                 IONArrSeed[2] = 0;     // Day
                }
            }

         // Apply validation and report failures

         if (IONExpValYear.exec(IONArrSeed[0])  == null ||
             IONExpValMonth.exec(IONArrSeed[1]) == null ||
             IONExpValDay.exec(IONArrSeed[2])   == null)
             {alert(IONInvalidDateMsg  +
                    IONInvalidAlert[0] + IONEleValue + IONInvalidAlert[1]);
              IONBlnFullInputDate = false;
              IONArrSeed[0] = IONBaseYear + Math.floor(IONDropDownYears/2); // Year
              IONArrSeed[1] = '6';     // Month
              IONArrSeed[2] = 1;       // Day
             }

         // Return the  Year    in IONArrSeed[0]
         //             Month   in IONArrSeed[1]
         //             Day     in IONArrSeed[2]

         return IONArrSeed;
        }

    function IONDisplayFormat(IONDisplayDate)
        {// The format of the display of today's date at the foot of the
         // calendar...
         // Day Month and Year display

         document.write(IONDisplayDate.getDate()                  + " / " +
                     IONArrMonthNames[IONDisplayDate.getMonth()]  + " / " +
                     IONDisplayDate.getFullYear());

         // for Month, Day and Year output use...
         //document.write(IONArrMonthNames[IONDisplayDate.getMonth()] + "-" +
         //            IONDisplayDate.getDate()                       + "-" +
         //            IONDisplayDate.getFullYear());
        }

    function IONSetOutput(IONOutputDate)
        {// Numeric months are held internally as 0 to 11 in this script so
         // the correct numeric month output should be in the form
         //                         (IONOutputDate.getMonth()+1)
         // e.g.
         //    IONTargetEle.value = ((IONOutputDate.getDate()<10)?'0':'')  +
         //                         IONOutputDate.getDate()             + '-' +
         //                         ((IONOutputDate.getMonth()<9)?'0':'')  +
         //                         (IONOutputDate.getMonth()+1)        + '-' +
         //                         IONOutputDate.getFullYear();

         // Day Month and Year output
         IONTargetEle.value =   ((IONOutputDate.getDate()<10)?'0':'')  +
                                  IONOutputDate.getDate()                + '/' +
                                  IONArrMonthNames[IONOutputDate.getMonth()] + '/' +
                                  IONOutputDate.getFullYear();

         // for Month, Day and Year output use...
         //IONTargetEle.value =   IONArrMonthNames[IONOutputDate.getMonth()] + '-' +
         //                     ((IONOutputDate.getDate()<10)?'0':'')  +
         //                       IONOutputDate.getDate()                + '-' +
         //                       IONOutputDate.getFullYear();

         IONHide();
        }

//------------------------------------------------------------------------------
// End of customisation section
//------------------------------------------------------------------------------

    // I try to avoid browser sniffing but the IE SELECT/z-index "feature" means
    // that I have had to place an IFRAME behind the pop-up.  This currently
    // renders incorrectly in Opera (the IFRAME renders in front of the
    // calendar) but as I write (2005-Nov-01) the rendering is fixed in Opera 9
    // which is in Beta.

    var IONIsOpera  = (navigator.userAgent.toLowerCase().indexOf("opera")!=-1);

    // Browsers handle positioning differently Mozilla (Firefox & Flock) needs
    // to exclude DIVs while IE/Opera must include them so unfortunately I have
    // to sniff this too.

    var IONIsFirefox= (navigator.userAgent.toLowerCase().indexOf("firefox")!=-1);

    var IONTargetEle,
        IONSaveText,
        IONSaveBackground,
        IONMonthSum         = 0,
        IONBlnFullInputDate = false,
        IONStartDate        = new Date(),
        IONSeedDate         = new Date(),
        IONWeekStart        = IONWeekStart%7;

    // "Escape" all the user defined date delimiters -
    // several delimiters will need it and it does no harm for the others.

    var IONExpDelimiters    = new RegExp('[\\'+IONArrDelimiters.join('\\')+']','g');

    // These regular expression validate the input date format to the
    // following rules;
    //
    // Format:              Day   1-31 (optional zero on single digits)
    //                      Month 1-12 (optional zero on single digits)
    //                            or case insensitive name
    //                      Year  Two or four digits

    // Months names and Delimiters are as defined above

    var IONExpValDay    = /^(0?[1-9]|[1-2]\d|3[0-1])$/,
        IONExpValMonth  = new RegExp("^(0?[1-9]|1[0-2]|"        +
                                     IONArrMonthNames.join("|") +
                                     ")$","i"),
        IONExpValYear   = /^(\d{2}|\d{4})$/;

    function showCal(IONEle,IONSourceEle)    {IONShow(IONEle,IONSourceEle);}
    function IONShow(IONEle,IONSourceEle)
        {//*********************************************************************
         //   If no value is preset then the seed date is
         //      Today (when today is in range) OR
         //      The middle of the date range.

         IONSeedDate = IONDateNow;

         // Strip space characters from start and end of date input
         IONEle.value = IONEle.value.replace(/^\s+/,'').replace(/\s+$/,'');

         if (IONEle.value.length==0)
            {// If no value is entered and today is within the range,
             // use today's date, otherwise use the middle of the valid range.

             IONBlnFullInputDate=false;

             if ((new Date(IONBaseYear+IONDropDownYears-1,11,31))<IONSeedDate ||
                 (new Date(IONBaseYear,0,1))                     >IONSeedDate
                )
                {IONSeedDate = new Date(IONBaseYear +
                                        Math.floor(IONDropDownYears / 2), 5, 1);
                }
            }
         else
            {// Parse the string into an array using the allowed delimiters
             IONArrSeedDate =
                 IONInputFormat(IONEle.value.split(IONExpDelimiters),
                                IONEle.value);

             // So now we have the Year, Month and Day in an array.

             //   If the year is two digits then the routine assumes a year
             //   belongs in the 21st Century unless it is less than 50 in which
             //   case it assumes the 20th Century is intended.

             if (IONArrSeedDate[0]<100)
                {IONArrSeedDate[0]= IONArrSeedDate[0] +
                                    parseInt((IONArrSeedDate[0]>50)?1900:2000,
                                             10);
                }

             // Check whether the month is in digits or an abbreviation

             if (IONArrSeedDate[1].search(/\d+/)!=0)
                {month = IONArrMonthNames.join('|').toUpperCase().
                            search(IONArrSeedDate[1].substr(0,3).toUpperCase());
                 IONArrSeedDate[1] = Math.floor(month/4)+1;
                }

             IONSeedDate = new Date(IONArrSeedDate[0],
                                    IONArrSeedDate[1]-1,
                                    IONArrSeedDate[2]);
            }

         // Test that we have arrived at a valid date

         if (isNaN(IONSeedDate))
            {alert( IONInvalidDateMsg +
                    IONInvalidAlert[0] + IONEle.value +
                    IONInvalidAlert[1]);
             IONSeedDate = new Date(IONBaseYear +
                    Math.floor(IONDropDownYears/2),5,1);
             IONBlnFullInputDate=false;
            }
         else
            {// Test that the date is within range,
             // if not then set date to a sensible date in range.

             if ((new Date(IONBaseYear,0,1)) > IONSeedDate)
                {if (IONBlnStrict) alert(IONOutOfRangeMsg);
                 IONSeedDate = new Date(IONBaseYear,0,1);
                 IONBlnFullInputDate=false;
                }
             else
                {if ((new Date(IONBaseYear+IONDropDownYears-1,11,31))<
                      IONSeedDate)
                    {if (IONBlnStrict) alert(IONOutOfRangeMsg);
                     IONSeedDate = new Date(IONBaseYear +
                                            Math.floor(IONDropDownYears)-1,
                                                       11,1);
                     IONBlnFullInputDate=false;
                    }
                 else
                    {if (IONBlnStrict && IONBlnFullInputDate &&
                          (IONSeedDate.getDate()      != IONArrSeedDate[2] ||
                           (IONSeedDate.getMonth()+1) != IONArrSeedDate[1] ||
                           IONSeedDate.getFullYear()  != IONArrSeedDate[0]
                          )
                        )
                        {alert(IONDoesNotExistMsg);
                         IONSeedDate = new Date(IONSeedDate.getFullYear(),
                                                IONSeedDate.getMonth()-1,1);
                         IONBlnFullInputDate=false;
                        }
                    }
                }
            }

         // Test the disabled dates for validity
         // Give error message if not valid.

         for (var i=0;i<IONDisabledDates.length;i++)
            {if (!((typeof IONDisabledDates[i]      == 'object') &&
                   (IONDisabledDates[i].constructor == Date)))
                {if ((typeof IONDisabledDates[i]      == 'object') &&
                     (IONDisabledDates[i].constructor == Array))
                    {var IONPass = true;

                     if (IONDisabledDates[i].length !=2)
                        {alert(IONRangeDisablingError[0] + IONDisabledDates[i] +
                               IONRangeDisablingError[1]);
                         IONPass = false;
                        }
                     else
                        {for (var j=0;j<IONDisabledDates[i].length;j++)
                            {if (!((typeof IONDisabledDates[i][j]      == 'object') &&
                                   (IONDisabledDates[i][j].constructor == Date)))
                                {alert(IONDateDisablingError[0] + IONDisabledDates[i][j] +
                                       IONDateDisablingError[1]);
                                 IONPass = false;
                                }
                            }
                        }

                     if (IONPass && (IONDisabledDates[i][0] > IONDisabledDates[i][1]))
                        {IONDisabledDates[i].reverse();}
                    }
                 else
                    {alert(IONDateDisablingError[0] + IONDisabledDates[i] +
                           IONDateDisablingError[1]);}
                }
            }

         // Calculate the number of months that the entered (or
         // defaulted) month is after the start of the allowed
         // date range.

         IONMonthSum =  12*(IONSeedDate.getFullYear()-IONBaseYear)+
                            IONSeedDate.getMonth();

         // Set the drop down boxes.

         document.getElementById('IONYears').options.selectedIndex =
            Math.floor(IONMonthSum/12);
         document.getElementById('IONMonths').options.selectedIndex=
            (IONMonthSum%12);

         // Position the calendar box

         var offsetTop =parseInt(IONEle.offsetTop,10)+
                        parseInt(IONEle.offsetHeight,10),
             offsetLeft=parseInt(IONEle.offsetLeft,10);

         IONTargetEle=IONEle;

         do {IONEle=IONEle.parentNode;
             if (IONEle.tagName!='FORM'  &&
                 IONEle.tagName!='TBODY' &&
                 IONEle.tagName!='TR'    &&
                 ((IONIsFirefox && IONEle.tagName!='DIV') || !IONIsFirefox) &&
                 IONEle.nodeType==1)
                {offsetTop +=parseInt(IONEle.offsetTop,10);
                 offsetLeft+=parseInt(IONEle.offsetLeft,10);
                }
            }
         while (IONEle.tagName!='BODY');

         document.getElementById('ION').style.top =offsetTop +'px';
         document.getElementById('ION').style.left=offsetLeft+'px';

         if (!IONIsOpera)
            {document.getElementById('IONIframe').style.top =offsetTop +'px';
             document.getElementById('IONIframe').style.left=offsetLeft+'px';
             document.getElementById('IONIframe').style.width  =
                (document.getElementById('ION').offsetWidth-2)+'px';
             document.getElementById('IONIframe').style.height =
                (document.getElementById('ION').offsetHeight-2)+'px';
             document.getElementById('IONIframe').style.visibility='visible';
            }

         // Display the month

         IONShowMonth(0);

         // Show it on the page

         document.getElementById('ION').style.visibility='visible';

         IONCancelPropagation(IONSourceEle);
        }

    function IONCellOutput(IONEvt)
        {var IONEle = eventTrigger(IONEvt),
             IONOutputDate = new Date(IONStartDate);

         IONOutputDate.setDate(IONStartDate.getDate() +
                                 parseInt(IONEle.id.substr(8),10));

         IONSetOutput(IONOutputDate);
        }

    function IONFootOutput()
        {IONSetOutput(IONDateNow);}

    function IONCancelPropagation(IONSourceEle)
        {if (typeof event=='undefined')         //Firefox
                {IONSourceEle.parentNode.
                    addEventListener("click",IONStopPropagation,false);
                }
         else   {event.cancelBubble = true;}    //IE, Opera
        }

    function IONStopPropagation(IONEvt)
        {if (typeof event=='undefined')
              IONEvt.stopPropagation();         //Firefox
         else IONEvt.cancelBubble = true;    //IE, Opera
        }

    function IONHighlight(e)
        {var IONEle = eventTrigger(e);

         IONSaveText        =IONEle.style.color;
         IONSaveBackground  =IONEle.style.backgroundColor;

         IONEle.style.color             =IONHighlightText;
         IONEle.style.backgroundColor   =IONHighlightBackground;

         return true;
        }

    function IONUnhighlight(e)
        {var IONEle = eventTrigger(e);

         IONEle.style.backgroundColor   =IONSaveBackground;
         IONEle.style.color             =IONSaveText;

         return true;
        }

    function eventTrigger(e)
        {if (!e) e = event;
         return e.target||e.srcElement;
        }

    function IONCancel(e)
        {if (IONClickToHide) IONHide();
         IONStopPropagation(e);}

    function IONHide()
        {document.getElementById('ION').style.visibility='hidden';
         if (!IONIsOpera)
            {document.getElementById('IONIframe').style.visibility='hidden';}
        }

    function IONFootOver()
        {document.getElementById('IONFoot').style.color=IONTodayHighlight;
         document.getElementById('IONFoot').style.fontWeight='bold';
        }

    function IONFootOut()
        {document.getElementById('IONFoot').style.color=IONTodayText;
         document.getElementById('IONFoot').style.fontWeight='normal';
        }

    function IONShowMonth(IONBias)
        {// Set the selectable Month and Year
         // May be called: from the left and right arrows
         //                  (shift month -1 and +1 respectively)
         //                from the month selection list
         //                from the year selection list
         //                from the showCal routine
         //                  (which initiates the display).

         var IONShowDate  = new Date(Date.parse(new Date().toDateString()));

         IONSelYears  = document.getElementById('IONYears');
         IONSelMonths = document.getElementById('IONMonths');

         if (IONSelYears.options.selectedIndex>-1)
            {IONMonthSum=12*(IONSelYears.options.selectedIndex)+IONBias;
			 if (IONSelMonths.options.selectedIndex>-1)
                {IONMonthSum+=IONSelMonths.options.selectedIndex;}
            }
         else
            {if (IONSelMonths.options.selectedIndex>-1)
                {IONMonthSum+=IONSelMonths.options.selectedIndex;}
            }

         IONShowDate.setFullYear(IONBaseYear + Math.floor(IONMonthSum/12),
                                 (IONMonthSum%12),
                                 1);

		 if ((12*parseInt((IONShowDate.getFullYear()-IONBaseYear),10)) +
             parseInt(IONShowDate.getMonth(),10) < (12*IONDropDownYears)    &&
             (12*parseInt((IONShowDate.getFullYear()-IONBaseYear),10)) +
             parseInt(IONShowDate.getMonth(),10) > -1)
            {IONSelYears.options.selectedIndex=Math.floor(IONMonthSum/12);
             IONSelMonths.options.selectedIndex=(IONMonthSum%12);

             IONCurMonth = IONShowDate.getMonth();

             IONShowDate.setDate(-(IONShowDate.getDay()-IONWeekStart)%7+1);

             IONStartDate = new Date(IONShowDate);

             var IONFoot = document.getElementById('IONFoot');

             if (IONDisabledDates.length==0)
                {if (IONActiveToday)
                    {IONFoot.onclick=IONFootOutput;
                     IONFoot.onmouseover=IONFootOver;
                     IONFoot.onmouseout =IONFootOut;
                    }
                 else
                    {if (document.addEventListener)
                            {IONFoot.addEventListener('click',IONStopPropagation, false);}
                     else   {IONFoot.attachEvent('onclick',IONStopPropagation);}
                     IONFoot.onmouseover=null;
                     IONFoot.onmouseout=null;
                    }
                }
             else
                {for (var k=0;k<IONDisabledDates.length;k++)
                    {if (!IONActiveToday ||
                         ((typeof IONDisabledDates[k]      == 'object')                 &&
                             (((IONDisabledDates[k].constructor == Date)                &&
                               IONDateNow.valueOf() == IONDisabledDates[k].valueOf()
                              ) ||
                              ((IONDisabledDates[k].constructor == Array)               &&
                               IONDateNow.valueOf() >= IONDisabledDates[k][0].valueOf() &&
                               IONDateNow.valueOf() <= IONDisabledDates[k][1].valueOf()
                              )
                             )
                         )
                        )
                        {if (document.addEventListener)
                                {IONFoot.addEventListener('click',IONStopPropagation, false);}
                         else   {IONFoot.attachEvent('onclick',IONStopPropagation);}
                         IONFoot.onmouseover=null;
                         IONFoot.onmouseout=null;
                         break;
                        }
                     else
                        {IONFoot.onclick=IONFootOutput;
                         IONFoot.onmouseover=IONFootOver;
                         IONFoot.onmouseout =IONFootOut;
                        }
                    }
                }

             // Treewalk to display the dates.
             // I tried to use getElementsByName but IE refused to cooperate
             // so I resorted to this method which works for all tested
             // browsers.

             var IONCells = document.getElementById('IONCells');

             for (i=0;i<IONCells.childNodes.length;i++)
                {var IONRows = IONCells.childNodes[i];
                 if (IONRows.nodeType==1 && IONRows.tagName=='TR')
                    {for (j=0;j<IONRows.childNodes.length;j++)
                        {var IONCols = IONRows.childNodes[j];
                         if (IONCols.nodeType==1 && IONCols.tagName=='TD')
                            {IONRows.childNodes[j].innerHTML=
                                    IONShowDate.getDate();

                             var IONCellStyle=IONRows.childNodes[j].style,
                                 IONDisabled = false;

                             for (var k=0;k<IONDisabledDates.length;k++)
                                {if ((typeof IONDisabledDates[k]      == 'object')  &&
                                     (IONDisabledDates[k].constructor == Date)      &&
                                     IONShowDate.valueOf() == IONDisabledDates[k].valueOf())
                                    {IONDisabled = true;}
                                 else
                                    {if ((typeof IONDisabledDates[k]      == 'object') &&
                                         (IONDisabledDates[k].constructor == Array)    &&
                                         IONShowDate.valueOf() >= IONDisabledDates[k][0].valueOf() &&
                                         IONShowDate.valueOf() <= IONDisabledDates[k][1].valueOf())
                                        {IONDisabled = true;}
                                    }
                                }

                             if (IONDisabled || !IONEnabledDay[j+(7*((i*IONCells.childNodes.length)/6))])
                                {IONRows.childNodes[j].onclick=null;
                                 IONRows.childNodes[j].onmouseover=null;
                                 IONRows.childNodes[j].onmouseout=null;
                                 IONCellStyle.color=IONDisabledDayText;
                                 IONCellStyle.backgroundColor=
                                     IONDisabledDayBackground;
                                }
                             else
                                {IONRows.childNodes[j].onclick      =IONCellOutput;
                                 IONRows.childNodes[j].onmouseover  =IONHighlight;
                                 IONRows.childNodes[j].onmouseout   =IONUnhighlight;

                                 if (IONShowDate.getMonth()!=IONCurMonth)
                                    {IONCellStyle.color=IONExMonthText;
                                     IONCellStyle.backgroundColor=
                                         IONExMonthBackground;
                                    }
                                 else if (IONBlnFullInputDate &&
                                          IONShowDate.toDateString()==
                                          IONSeedDate.toDateString())
                                    {IONCellStyle.color=IONInDateText;
                                     IONCellStyle.backgroundColor=
                                         IONInDateBackground;
                                    }
                                 else if (IONShowDate.getDay()%6==0)
                                    {IONCellStyle.color=IONWeekendText;
                                     IONCellStyle.backgroundColor=
                                         IONWeekendBackground;
                                    }
                                 else
                                    {IONCellStyle.color=IONCellText;
                                     IONCellStyle.backgroundColor=
                                         IONCellBackground;
                                    }
                                }

                             IONShowDate.setDate(IONShowDate.getDate()+1);
                            }
                        }
                    }
                }
            }
        }

    if (!IONIsOpera)
        {document.write("<iframe class='ION' " +
                                "id='IONIframe' name='IONIframe' "   +
                                "frameborder='0'>"                   +
                        "</iframe>");
        }


    document.write(
     "<table id='ION' class='ION'>" +
       "<tr class='ION'>" +
         "<td class='ION'>" +
           "<table class='IONHead' id='IONHead' " +
                    "cellspacing='0' cellpadding='0' width='100%'>" +
            "<tr class='IONHead'>" +
                "<td class='IONHead'>" +
                    "<input class='IONHead' type='button' value='<' " +
                            "onclick='IONShowMonth(-1);'  /></td>" +
                 "<td class='IONHead'>" +
                    "<select id='IONMonths' class='IONHead' " +
                            "onChange='IONShowMonth(0);'>");

    for (i=0;i<IONArrMonthNames.length;i++)
        document.write(   "<option>" + IONArrMonthNames[i] + "</option>");

    document.write("   </select>" +
                 "</td>" +
                 "<td class='IONHead'>" +
                    "<select id='IONYears' class='IONHead' " +
                            "onChange='IONShowMonth(0);'>");

    for (i=0;i<IONDropDownYears;i++)
        document.write(   "<option>" + (IONBaseYear+i) + "</option>");

    document.write(   "</select>" +
                 "</td>" +
                 "<td class='IONHead'>" +
                    "<input class='IONHead' type='button' value='>' " +
                            "onclick='IONShowMonth(1);' /></td>" +
                "</tr>" +
              "</table>" +
            "</td>" +
          "</tr>" +
          "<tr class='ION'>" +
            "<td class='ION'>" +
              "<table class='IONCells' align='center'>" +
                "<thead class='IONWeek'>" +
                  "<tr  class='IONWeek'>");

    for (i=0;i<IONArrWeekInits.length;i++)
        document.write( "<td class='IONWeek'>" +
                          IONArrWeekInits[(i+IONWeekStart)%IONArrWeekInits.length] +
                        "</td>");

    document.write("</tr>" +
                "</thead>" +
                "<tbody class='IONCells' id='IONCells'>");

    for (i=0;i<6;i++)
        {document.write(
                    "<tr class='IONCells'>");
         for (j=0;j<7;j++)
            {document.write(
                        "<td class='IONCells' id='IONCell_" + (j+(i*7)) +
                        "'></td>");
            }

         document.write(
                    "</tr>");
        }

    document.write(
                "</tbody>");

    if ((new Date(IONBaseYear + IONDropDownYears, 11, 32)) > IONDateNow &&
        (new Date(IONBaseYear, 0, 0))                      < IONDateNow)
        {document.write(
                  "<tfoot class='IONFoot'>" +
                    "<tr class='IONFoot'>" +
                      "<td class='IONFoot' id='IONFoot' colspan='7'>" + IONToday + " ");

         IONDisplayFormat(IONDateNow);

         document.write(
                        "</td>" +
                     "</tr>" +
                   "</tfoot>");
        }

    document.write(
              "</table>" +
            "</td>" +
          "</tr>" +
        "</table>");

    if (document.addEventListener)
        {document.addEventListener('click',IONHide, false);
         document.getElementById('ION').addEventListener('click',IONCancel,false);
         document.getElementById('IONHead').addEventListener('click',IONStopPropagation,false);
         document.getElementById('IONCells').addEventListener('click',IONStopPropagation,false);
        }
    else
        {document.attachEvent('onclick',IONHide);
         document.getElementById('ION').attachEvent('onclick',IONCancel);
         document.getElementById('IONHead').attachEvent('onclick',IONStopPropagation);
         document.getElementById('IONCells').attachEvent('onclick',IONStopPropagation);
        }

// End of Calendar

// Validations

// validation for correct field value using javascript
function validName(obj)
	{
	
		if(obj.value != "jaswinder")
		{
			if(obj.value== "")
			{
				alert(" name not entered");
			}
		else
			{
				alert("enter correct name")
				document.getElementById('txtNameCV').value="";
				document.getElementById('txtNameCV').focus();
			}
		}
		else 
		{
		alert("correct name")
		}
		
	}
	
	
	//validation for correct field value using custom validator asp.net
	
	function validText(sender,arguments)
	{
	
	
	
		if(arguments.Value != "validtext")
		{
		arguments.IsValid=false;
		}
		else
		{
		arguments.IsValid=true;
		}
		
	}
	
	
	//function for numeric field validation using custom validator asp.net
	
	function numericRequired(sender,arguments)
		{
		var checkOK = "0123456789"; 	
		var checkStr =arguments.Value; 
		var i,j;
		var flag = true;	
		var cntChk=checkStr.length;
		var cntOK=checkOK.length;
		
	
		for (i = 0; i < cntChk; i++)
			 {
				ch = checkStr.charAt(i);
				for (j = 0; j < cntOK ; j++)
				{
					if (ch == checkOK.charAt(j)) 
					break;
					if (j ==cntOK -1)
					 {
						flag = false;
						break; 
					} 
				}
			
			if(flag==true)
			{
			arguments.IsValid=true;
			}
			else
			{
			arguments.IsValid=false;
			}
		}
	}
	
	//function for numeric field validation using javascript 
	
	function numericReq(obj)
		{
		
			var checkOK = "0123456789"; 
			//var checkStr = objName; 
			var flag = true;

			for (i = 0; i < obj.value.length; i++)
			 {
				ch = obj.value.charAt(i);
				for (j = 0; j < checkOK.length; j++)
				{
					if (ch == checkOK.charAt(j)) 
					break;
					if (j == checkOK.length-1)
					 {
						flag = false;
						break; 
					} 
				}
			}
			
			if(flag==true)
			alert("data is numeric");
			else
			{
			alert("enter nuneric data");
			}
	}
	
function fun(val)
{
alert(val);
/*	if(document.getElementById('chkDis').Checked)
	{
	document.getElementById('textbox3').Visible=false;
	}
	else
	{
	document.getElementById('textbox3').Visible=true;
	}*/
}
