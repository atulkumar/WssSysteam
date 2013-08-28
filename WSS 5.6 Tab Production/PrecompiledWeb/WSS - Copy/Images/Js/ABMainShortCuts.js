document.onkeypress = function (evt) 
{
  var r = '';
  if (document.all) {
    r += event.ctrlKey ? 'Ctrl-' : '';
    r += event.altKey ? 'Alt-' : '';
    r += event.shiftKey ? 'Shift-' : '';
    r += event.keyCode;
  }
  else if (document.getElementById) {
    r += evt.ctrlKey ? 'Ctrl-' : '';
    r += evt.altKey ? 'Alt-' : '';
    r += evt.shiftKey ? 'Shift-' : '';
    r += evt.charCode;
  }
  else if (document.layers) {
    r += evt.modifiers & Event.CONTROL_MASK ? 'Ctrl-' : '';
    r += evt.modifiers & Event.ALT_MASK ? 'Alt-' : '';
    r += evt.modifiers & Event.SHIFT_MASK ? 'Shift-' : '';
    r += evt.which;
  }
if(r=='Ctrl-Shift-1')
{
OpenW(0,'ABTY','cPnlContact_txtAB_Type');
}
if(r=='Ctrl-Shift-2')
{
//OpenBR('COM','cPnlContact_txtBr');
}
if(r=='Ctrl-Shift-20')
{
OpenW(0,'STA','cPnlContact_txtStatus');
}
if(r=='Ctrl-Shift-3')
{
OpenW(0,'CTY','cPnlContact_txtCity');
}
if(r=='Ctrl-Shift-16')
{
OpenW(0,'PROV','cPnlContact_txtProvince');
}
if(r=='Ctrl-Shift-18')
{
OpenW(0,'CNTY','cPnlContact_txtCountry');
}
if(r=='Ctrl-Shift-13')
{
OpenW(0,'EMLT','cPnlContact_txtEmailType1');
}
if(r=='Ctrl-Shift-9')
{
OpenW(0,'EMLT','cPnlContact_txtEmailType2');
}
if(r=='Ctrl-Shift-8')
{
OpenW(0,'PHTY','cPnlContact_txtPhoneType1');
}
if(r=='Ctrl-Shift-15')
{
OpenW(0,'ARCD','cPnlContact_txtAreaCode1');
}
if(r=='Ctrl-Shift-21')
{
OpenW(0,'CCD','cPnlContact_txtCountryCode1');
}
if(r=='Ctrl-Shift-25')
{
OpenW(0,'PHTY','cPnlContact_txtPhoneType2');
}
if(r=='Ctrl-Shift-5')
{
OpenW(0,'ARCD','cPnlContact_txtAreaCode2');
}
if(r=='Ctrl-Shift-14')
{
OpenW(0,'CCD','cPnlContact_txtCountryCode2');
}


//  alert(r);
  return true;
}
