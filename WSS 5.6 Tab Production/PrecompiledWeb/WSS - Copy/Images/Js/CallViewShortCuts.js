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
if(r=='Ctrl-Shift-15')
{
OpenAB('cpnlCallTask_TxtTaskOwner');
}
if(r=='Ctrl-Shift-16')
{
OpenW(0,'PRIO','cpnlCallTask_TxtPriority');
}
if(r=='Ctrl-Shift-20')
{
OpenW(0,'TKTY','cpnlCallTask_TxtTaskType');
}


  //alert(r);
  return true;
}
