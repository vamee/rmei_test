if (typeof window.RadControlsNamespace=="\x75\x6edefine\x64"){window.RadControlsNamespace= {} ; }if (typeof(window.RadControlsNamespace.Screen)=="\x75\156d\x65\x66ined" || typeof(window.RadControlsNamespace.Screen.Version)==null || window.RadControlsNamespace.Screen.Version<.11e1){window.RadControlsNamespace.Screen= {Version: .11e1,GetViewPortSize:function (){var width=0; var height=0; var o5=document.body; if (RadControlsNamespace.Browser.StandardsMode && !RadControlsNamespace.Browser.IsSafari){o5=document.documentElement; }if (window.innerWidth){width=window.innerWidth; height=window.innerHeight; }else {width=o5.clientWidth; height=o5.clientHeight; }width+=o5.scrollLeft; height+=o5.scrollTop; return {width:width-6,height:height-6 } ; } ,GetElementPosition:function (el){var parent=null; var O5= {x: 0,y: 0 } ; var box; if (el.getBoundingClientRect){box=el.getBoundingClientRect(); var scrollTop=document.documentElement.scrollTop || document.body.scrollTop; var scrollLeft=document.documentElement.scrollLeft || document.body.scrollLeft; O5.x=box.left+scrollLeft-2; O5.y=box.top+scrollTop-2; return O5; }else if (document.getBoxObjectFor){box=document.getBoxObjectFor(el); O5.x=box.x-2; O5.y=box.y-2; }else {O5.x=el.offsetLeft; O5.y=el.offsetTop; parent=el.offsetParent; if (parent!=el){while (parent){O5.x+=parent.offsetLeft; O5.y+=parent.offsetTop; parent=parent.offsetParent; }}}if (window.opera){parent=el.offsetParent; while (parent && parent.tagName!="BODY" && parent.tagName!="HTML"){O5.x-=parent.scrollLeft; O5.y-=parent.scrollTop; parent=parent.offsetParent; }}else {parent=el.parentNode; while (parent && parent.tagName!="\102ODY" && parent.tagName!="\x48TML"){O5.x-=parent.scrollLeft; O5.y-=parent.scrollTop; parent=parent.parentNode; }}return O5; } ,ElementOverflowsTop:function (T){return this.GetElementPosition(T).y<0; } ,ElementOverflowsLeft:function (T){return this.GetElementPosition(T).x<0; } ,ElementOverflowsBottom:function (l5,T){var i5=this.GetElementPosition(T).y+RadControlsNamespace.Box.GetOuterHeight(T); return i5>l5.height; } ,ElementOverflowsRight:function (l5,T){var I5=this.GetElementPosition(T).x+RadControlsNamespace.Box.GetOuterWidth(T); return I5>l5.width; }};}
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}