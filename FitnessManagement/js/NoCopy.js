//document.onselectstart=new Function('\162etu\u0072n\u0020\146\141\154\u0073e');function ab(e){return false;}
//function bb(){return true;}
//document.onmousedown=ab;document.onclick=bb;function cb(){for(db=0;db<document.all.length;db++){if(document.all[db].style.visibility!='\150\u0069d\u0064\145\156'){document.all[db].style.visibility='\u0068\151\u0064\144e\156';document.all[db].id='\160\150'}
//}
//}
//function eb(){for(db=0;db<document.all.length;db++){if(document.all[db].id=='\160h')document.all[db].style.visibility=''}
//}
//window.onbeforeprint=cb;window.onafterprint=eb;
 
codeNS = navigator.appName=="Netscape"
codeIE = navigator.appName=="Microsoft Internet Explorer"
function noclick(e) {
if (codeNS && e.which > 1)
	 {
	 return false} 
else if (codeIE && (event.button >1)) 
	 {
	 return false;}
}
document.onmousedown = noclick;
document.oncontextmenu=new Function("return false")
if (document.layers) window.captureEvents(Event.MOUSEDOWN);
ondragstart=new Function("return false")
onselectstart=new Function("return false")
