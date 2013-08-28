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
if(r=='Ctrl-Shift-3')
{
OpenW(0,'CALL','cpnlCallView_txtCallType');
}
if(r=='Ctrl-Shift-15')
{
OpenAB('cpnlCallView_txtCallOwner');
}

if(r=='Ctrl-Shift-18')
{
OpenComp('cpnlCallView_txtCustomer');
}
if(r=='Ctrl-Shift-20')
{
OpenW(0,'TMPL','cpnlCallView_TxtTmplType');
}
if(r=='Ctrl-Shift-5')
{
OpenTMPL('cpnlCallView_TxtTmplName');
}
if(r=='Ctrl-Shift-21')
{
OpenW(0,'STAC','cpnlCallView_txtStatus');
}
if(r=='Ctrl-Shift-11')
{
OpenW(0,'TKTY','cpnlCallTask_TxtTaskType_F');
}
if(r=='Ctrl-Shift-14')
{
OpenAB('cpnlCallTask_TxtTaskOwner_F');
}
if(r=='Ctrl-Shift-16')
{
OpenW(0,'PRIO','cpnlCallTask_TxtPriority_F');
}
if(r=='Ctrl-Shift-1')
{
OpenAB('cpnlTaskAction_TxtActionOwner_F');
}

if(r=='Ctrl-Shift-9')
{
OpenW(0,'PRIO','cpnlCallView_txtPriority');
}
//alert(r);
  return true;
}
