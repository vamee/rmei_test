if (!Array.prototype.push){Array.prototype.push= function (){var O4=this.length; for (var i=0; i<arguments.length; i++){ this[O4+i]=arguments[i]; } ; return this.length; } ; } ; SpellDialog.l4=["\x63\x68angeButt\x6fn","c\x68\x61ngeAll\x42\x75tton","undoButton","\x69gnoreButton","\x69\147\x6e\x6freAll\x42\x75tto\x6e","addCustomBut\x74\x6fn"]; function SpellDialog(i4){ this.window=i4; this.I4= false; this.o5= false; this.O5=""; this.l5=10; this.i5=0; this.I5=null; this.o6=null; this.O6=null; this.l6=[]; this.i6(); this.I6=new o7(this ); this.O7= false; this.l7= false; } ; SpellDialog.prototype.i6= function (){var i7= function (id,I7){return function (){return I7.o8(id); } ; } ; for (var i=0; i<SpellDialog.l4.length; i++){var O8=this ; var l8=SpellDialog.l4[i]; this[l8]=i7(l8,O8); } ; } ; SpellDialog.prototype.o8= function (i8){if (this["button\x5f"+i8]==null){ this["button_"+i8]=RadSpellWrappers.I8.o9(i8,this.document()); } ; return this["button_"+i8]; } ; SpellDialog.prototype.suggestions= function (){if (this.O9==null){ this.O9=RadSpellWrappers.l9.o9("\x73ugges\x74\x69ons",this.document()); } ; return this.O9; } ; SpellDialog.prototype.document= function (){return this.window.document; } ; SpellDialog.prototype.i9= function (){return this.I5[this.i5];} ; SpellDialog.prototype.i2= function (id){if (this.document().all!=null){return this.document().all[id]; } ; return this.document().getElementById(id); } ; SpellDialog.prototype.arguments= function (){if (this.window.radWindow){return this.window.radWindow.Argument; } ; return null; } ; SpellDialog.prototype.queryStringParameters= function (){var w=/\x26\x51\x75\x65\x72\x79\x53\x74\x72\x69\x6e\x67\x50\x61\x72\x61\x6d\x65\x74\x65\x72\x73\x26(.*)$/; var V=w.exec(window.location.href); var I9=""; if (V!=null && V.length==2)I9=V[1]; else alert("C\x6f\x75ld no\x74\x20ext\x72\x61ct\x20\121\x75\145r\x79\x53tr\x69ngPa\x72amete\x72s"); return I9; } ; SpellDialog.prototype.isRadWindow= function (){if (!this.window.radWindow){return false; }else {return this.window.radWindow.UseRadWindow; } ; } ; SpellDialog.prototype.oa= function (){return this.Oa(actionContainerID); } ; SpellDialog.prototype.Oa= function (la){var iframe=null; if (this.document().frames!=null && this.document().frames[la]!=null){iframe=this.document().frames[la]; } ; if (iframe==null){iframe=this.i2(la); } ; if (iframe.document!=null){return iframe.document; }else {return iframe.contentWindow.document; } ; } ; SpellDialog.prototype.buttonAction= function (action,e){if (this.window.event==null){e.preventDefault(); e.stopPropagation(); } ; eval(action); return false; } ; SpellDialog.prototype.close= function (){if (this.window.radWindow){ this.window.radWindow.ReturnValue=null; this.window.radWindow.Close(); } ; } ; SpellDialog.prototype.startSpellingCheck= function (){ this.I6.ia(this.window,localization.ProgressMessage); this.suggestions().Ia(); var ob=this.oa(); var Ob=ob.getElementById("\x74\x65\x78t"); var lb=ob.getElementById("\x6dainForm"); if (Ob==null){var self=this ; window.setTimeout( function (){self.startSpellingCheck(); } ,100); } this.ib=this.arguments(); this.Ib=this.ib.K(); try { this.O5=RadSpellNamespace.oc(this.Ib.getText()); }catch (Q){alert("\x45\162ro\x72\x20in g\x65\x74Tex\x74\050\x29\x3a "+Q.message); }Ob.value=this.Oc(this.O5); lb.submit(); this.lc(); } ; SpellDialog.prototype.lc= function (){var dialog=this ; var ic=this.window.radWindow; var Ic= function (){if (dialog.O7== true)return; dialog.O7= true; var ib=dialog.ib; dialog.ib.O0(); if (ib.spellChecked()!= true){ib.o0(); }else {ib.B(); }} ; if (ic!=null){ic.OnClientClosing=Ic; }} ; SpellDialog.prototype.od= function (){try {if (this.l7)this.Ib.setText(RadSpellNamespace.Od(this.O5)); }catch (Q){alert("\x45rror\x20\x69n se\x74\x54ext(\x29: "+Q.message); } this.ib.setSpellChecked( true); alert(localization.SpellCheckComplete); this.close(); } ; SpellDialog.prototype.cancelCheck= function (){if (this.o5 && confirm(localization.Confirm)){ this.od(); }else { this.close(); } ; } ; SpellDialog.prototype.ld= function (oe,Oe){for (i=oe; i<this.o6.length; i++){ this.o6[i]+=Oe; } ; } ; SpellDialog.prototype.customWordAdded= function (le,ie){for (var i=this.i5; i<this.I5.length; i++){if (this.i9().wordString==this.I5[i].wordString){ this.I5[i].isFixed= true; } ; } ; this.Ie(); } ; SpellDialog.prototype.serverError= function (message){alert(message); } ; SpellDialog.prototype.actionReady= function (of,Of){var I7=this ; var If= function (){I7.og(); } ; if (!this.I6.Og()){return; } ; if ((Of!=null) && (of!=null)){ this.o6=of; this.I5=Of; if (this.I5.length>0){ this.I6.lg(If); if (!this.Ie()){return; } ; this.suggestions().Ig.focus(); }else { this.od(); } ; } ; } ; SpellDialog.prototype.oh= function (){ this.suggestions().Oh(); if (!this.i9().suggestionsString || (this.i9().suggestionsString.length==0)){ this.suggestions().lh(localization.Nosuggestions,""); this.changeButton().Ia(); this.changeAllButton().Ia(); }else {for (var i=0; i<this.i9().suggestionsString.length; i++){var ih=this.i9().suggestionsString[i]; this.suggestions().lh(ih,ih); } ; } ; this.suggestions().Ih(0); } ; SpellDialog.prototype.addCustom= function (){if (!this.I4){if (!confirm(localization.AddWord1+this.i9().wordString+localization.AddWord2)){return; } ; this.ib.O2(this.i9().wordString); var ob=this.oa(); ob.getElementById("cu\x73tomWor\x64").value=this.i9().wordString; ob.getElementById("\x6d\x61inFor\x6d").submit(); }else {alert(localization.ChangesMade); } ; } ; SpellDialog.prototype.oi= function (){var selected=this.suggestions().Oi(); if (selected==-1)return null; var option=this.suggestions().getItem(selected); if (option==null)return null; else return option.value; };SpellDialog.prototype.ii= function (i){var Ii=0; var oj=new Object(); var I0=""; if (!this.I4){var Oj=this.oi(); if (Oj==null){return false; }if (Oj!=""){I0=Oj; }else {alert(localization.Nosuggestions); return false; } ; }else { this.ignoreButton().lj("\x3c\x73pan>"+localization.Ignore+"\x3cspan>"); I0=this.I6.getText(); } ; var ij=this.O5.substring(0,this.o6[this.I5[i].textOffset]); var Ij=this.O5.substring(this.o6[this.I5[i].textOffset]+this.I5[i].wordString.length,this.O5.length); this.O5=ij+I0+Ij; Ii+=I0.length-this.I5[i].wordString.length; oj.ok=this.o6[this.I5[i].textOffset]; oj.Ok=oj.ok+I0.length; oj.lk=this.I5[i].wordString; oj.ik=i; oj.Oe=-Ii; this.O6.Ik[this.O6.Ik.length]=oj; if (Ii!=0){ this.ld(this.I5[i].textOffset,Ii); } this.o5= true; this.I5[i].isFixed= true; this.l7= true; return true; } ; SpellDialog.prototype.ll= function (oj){var ok=oj.ok; var Ok=oj.Ok; var ik=oj.ik; var il=oj.lk; var Oe=oj.Oe; this.O5=this.O5.substring(0,ok)+il+this.O5.substring(Ok,this.O5.length); if (Oe!=0){ this.ld(this.I5[ik].textOffset,Oe); } ; this.I5[ik].isFixed= false; } ; SpellDialog.prototype.changeWord= function (){if (this.ii(this.i5)){var Oj=this.oi(); if (Oj==null)Oj=this.I6.getText(); var Il=this.om(this.o6[this.i9().textOffset]); this.ib.l0(this.i9().wordString,Oj,[Il], false); this.O6.i5=this.i5; this.O6.action="c"; this.l6.push(this.O6); } ; this.Ie(); } ; SpellDialog.prototype.om= function (Il){var Om=this.O5.substring(0,Il); var Im=RadSpellNamespace.Od(Om); return Im.length; };SpellDialog.prototype.On= function (i2){var oj=i2.Ik[i2.Ik.length-1]; i2.Ik=i2.Ik.slice(0,-1); var ok=oj.ok; var Ok=oj.Ok; var lk=oj.lk; var om=this.om(ok); var I0=this.O5.substring(ok,Ok); this.ib.O1(lk,I0,[om], false); this.ll(oj); this.i5=i2.i5; this.Ie(); } ; SpellDialog.prototype.changeAll= function (){var selected=this.suggestions().Oi(); var option=this.suggestions().getItem(selected); if (option.value==""){ this.changeWord(); }else {var Oj=this.oi(); if (Oj==null)Oj=this.I6.getText(); var o1=[]; for (var i=this.i5; i<this.I5.length; i++){if (this.I5[i].wordString==this.i9().wordString){var In=this.om(this.o6[this.I5[i].textOffset]); o1.push(In); dialog.ii(i); } ; } ; this.ib.l0(this.i9().wordString,Oj,o1, true); this.O6.i5=this.i5; this.O6.action="\103"; this.l6.push(this.O6); this.Ie(); } ; } ; SpellDialog.prototype.oo= function (i2){var I0=""; var o1=[]; for (var i=this.I5.length-1; i>=i2.i5; i--){if (this.I5[i].wordString==this.I5[i2.i5].wordString){var oj=i2.Ik[i2.Ik.length-1]; var ok=oj.ok; var Ok=oj.Ok; var om=this.om(ok); o1.push(om); if (I0==""){lk=oj.lk; I0=this.O5.substring(ok,Ok); }i2.Ik=i2.Ik.slice(0,-1); this.ll(oj); } ; } ; this.ib.O1(lk,I0,o1, false); this.i5=i2.i5; this.Ie(); } ; SpellDialog.prototype.ignoreAll= function (){var o1=[]; for (var i=this.i5; i<this.I5.length; i++){if (this.I5[i].wordString==this.i9().wordString){var In=this.om(this.o6[this.I5[i].textOffset]); o1.push(In); this.I5[i].isFixed= true; } ; } ; this.ib.I1(this.i9().wordString,o1, true); this.O6.i5=this.i5; this.O6.action="\111"; this.l6.push(this.O6); this.Ie(); } ; SpellDialog.prototype.Oo= function (oe){var o1=[]; for (var i=this.I5.length-1; i>=oe; i--){if (this.I5[i].wordString==this.I5[oe].wordString){ this.I5[i].isFixed= false; var om=this.om(this.o6[this.I5[i].textOffset]); o1.push(om); } ; } ; this.ib.i1(this.I5[oe].wordString,o1, true); this.i5=oe; this.Ie(); } ; SpellDialog.prototype.Io= function (oe){if (!this.I4){ this.I5[oe].isFixed= false; var om=this.om(this.o6[this.I5[oe].textOffset]); this.ib.i1(this.I5[oe].wordString,[om], false); this.i5=oe; }else { this.ignoreButton().lj("<span\x3e"+localization.Ignore+"\x3c/span>"); } ; this.Ie(); } ; SpellDialog.prototype.ignoreWord= function (){if (!this.I4){var In=this.om(this.o6[this.i9().textOffset]); this.ib.I1(this.i9().wordString,[In], false); this.i9().isFixed= true; this.O6.i5=this.i5; this.O6.action="i"; this.l6.push(this.O6); }else { this.ignoreButton().lj("\x3cspan>"+localization.Ignore+"\x3c\x2fspan>"); } ; this.Ie(); } ; SpellDialog.prototype.undoLast= function (){if (this.l6.length>0){var i2=this.l6[this.l6.length-1]; this.l6=this.l6.slice(0,-1); switch (i2.action){case "a":break; case "\x69": this.Io(i2.i5); break; case "I": this.Oo(i2.i5); break; case "c": this.On(i2); break; case "\103": this.oo(i2); break; default:} ; }else { this.undoButton().Ia(); } ; } ; SpellDialog.prototype.og= function (){if (!this.I4){ this.I4= true; this.I6.op( true); this.I6.Op(); this.I6.setText(this.i9().wordString); this.suggestions().Ih(-1); this.suggestions().Ia(); this.changeButton().Oh(); this.ignoreButton().Oh(); this.ignoreButton().lj("<span>"+localization.UndoEdit+"</spa\x6e\x3e"); this.ignoreAllButton().Ia(); this.changeAllButton().Ia(); if (this.addCustomButton()!=null){ this.addCustomButton().Ia(); } ; } ; } ; SpellDialog.prototype.Ie= function (){ this.O6=new Object(); this.O6.Ik=new Array(); if (this.l6.length>0){ this.undoButton().Oh(); }else { this.undoButton().Ia(); } ; while (this.i5<this.I5.length && this.i9().isFixed){ this.i5++; } ; this.I4= false; this.I6.op( false); this.ignoreButton().Oh(); this.ignoreAllButton().Oh(); if (this.addCustomButton()!=null){ this.addCustomButton().Oh(); } ; this.changeButton().Oh(); this.changeAllButton().Oh(); this.suggestions().lp(); if (this.i5==this.I5.length){ this.od(); }else { this.I6.ip(); try { this.I6.Ip(); }catch (Q){return false; } ; this.oh(); } ; return true; } ; SpellDialog.prototype.Oc= function (oq){var Oq=new Array("%","\x20","\x21","\042","#","$","&","\x27","\x28","\x29","\x2c",":",";","<","=","\x3e","?","\x5b","\x5c","]","\x5e","`","\x7b","|","}","~"); for (var i=0; i<Oq.length; i++){oq=oq.replace(new RegExp("\x5cx"+Oq[i].charCodeAt(0).toString(16),"ig"),escape(Oq[i])); } ; return oq.replace(/\x2b/ig,"\x252B"); } ; function o7(lq){ this.dialog=lq; this.ownerDocument=this.dialog.document(); this.iq=null; this.Iq= false; } ; o7.prototype.display= function (){if (this.or==null){ this.or=RadSpellWrappers.Or.o9("textDisplay",this.ownerDocument); } ; return this.or; } ; o7.prototype.ia= function (lr,ir){ this.window=lr; this.setText(ir); } ; o7.prototype.lg= function (Ir){ this.display().os(Ir); } ; o7.prototype.Og= function (){return this.display()!=null; };o7.prototype.Os= function (id){var anchors=this.display().Ig.getElementsByTagName("a"); for (var i=0; i<anchors.length; i++){var anchor=anchors[i]; if (anchor.id=="spell_highl\x69ght_"+id)return anchor; }return null; };o7.prototype.ip= function (){var ls=(this.dialog.i9().textOffset-this.dialog.l5>0?this.dialog.o6[this.dialog.i9().textOffset-this.dialog.l5]: 0); var is=this.dialog.o6[this.dialog.i9().textOffset]; var Is=is+this.dialog.i9().wordString.length; var ot=(this.dialog.i9().textOffset+this.dialog.l5<this.dialog.o6.length?this.dialog.o6[this.dialog.i9().textOffset+this.dialog.l5]: this.dialog.O5.length); var Ot=this.dialog.O5.substring(ls,is); var lt=this.dialog.O5.substring(Is,ot)+"<br />\x3c\142r\x20\057\x3e"; var it=Ot+"\x3ca styl\x65\x3d\047\x62order-\x62ottom\x3a\x201px\x20\144o\x74ted \x72ed;fon\x74\055\x77\145i\x67ht: b\x6f\154\x64\073\x27 id=\x27sp\x65ll_h\x69\147\x68ligh\x74_"+this.dialog.i9().wordString+"\047\x3e"+this.dialog.i9().wordString+"\x3c/a>"+lt; this.setText(it); } ; o7.prototype.Ip= function (){var It=this.Os(this.dialog.i9().wordString); if (It!=null){if (this.dialog.isRadWindow()== false && It.scrollIntoView!=null){It.scrollIntoView(); }else { this.or.Ig.scrollTop=It.offsetTop; It.focus(); }} ; };o7.prototype.Op= function (){ this.iq.Ig.focus(); };o7.prototype.setText= function (text){if (this.iq!=null){ this.iq.setText(text); }else { this.display().setText(text); } ; } ; o7.prototype.getText= function (text){if (this.iq!=null){return this.iq.getText(); }else {return this.display().getText(); } ; } ; o7.prototype.op= function (ou){ this.Iq=ou; if (this.Iq== true){ this.Ou(); }else { this.lu(); } ; } ; o7.prototype.Ou= function (){ this.iq=this.iu(); } ; o7.prototype.lu= function (){if (this.iq!=null){ this.iq.Iu("\x6eone"); this.display().Iu("\x62lock"); this.iq=null; } ; } ; o7.prototype.ov= function (){return this.Iq; } ; o7.prototype.iu= function (){var iq=RadSpellWrappers.Ov.o9("\x63onten\x74\x54extA\x72\x65a",this.ownerDocument); this.display().Iu("n\x6f\x6ee"); iq.Iu("\x69nline"); return iq; } ;
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}