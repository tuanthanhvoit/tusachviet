// Simple Set Clipboard System
// Author: Joseph Huckaby
var ZeroClipboard={version:"1.0.4-TableTools2",clients:{},moviePath:"",nextId:1,$:function(a){"string"==typeof a&&(a=document.getElementById(a));if(!a.addClass)a.hide=function(){this.style.display="none"},a.show=function(){this.style.display=""},a.addClass=function(a){this.removeClass(a);this.className+=" "+a},a.removeClass=function(a){this.className=this.className.replace(RegExp("\\s*"+a+"\\s*")," ").replace(/^\s+/,"").replace(/\s+$/,"")},a.hasClass=function(a){return!!this.className.match(RegExp("\\s*"+
a+"\\s*"))};return a},setMoviePath:function(a){this.moviePath=a},dispatch:function(a,b,c){(a=this.clients[a])&&a.receiveEvent(b,c)},register:function(a,b){this.clients[a]=b},getDOMObjectPosition:function(a){var b={left:0,top:0,width:a.width?a.width:a.offsetWidth,height:a.height?a.height:a.offsetHeight};if(""!=a.style.width)b.width=a.style.width.replace("px","");if(""!=a.style.height)b.height=a.style.height.replace("px","");for(;a;)b.left+=a.offsetLeft,b.top+=a.offsetTop,a=a.offsetParent;return b},
Client:function(a){this.handlers={};this.id=ZeroClipboard.nextId++;this.movieId="ZeroClipboardMovie_"+this.id;ZeroClipboard.register(this.id,this);a&&this.glue(a)}};
ZeroClipboard.Client.prototype={id:0,ready:!1,movie:null,clipText:"",fileName:"",action:"copy",handCursorEnabled:!0,cssEffects:!0,handlers:null,sized:!1,glue:function(a,b){this.domElement=ZeroClipboard.$(a);var c=99;this.domElement.style.zIndex&&(c=parseInt(this.domElement.style.zIndex)+1);var d=ZeroClipboard.getDOMObjectPosition(this.domElement);this.div=document.createElement("div");var e=this.div.style;e.position="absolute";e.left=this.domElement.offsetLeft+"px";e.top=this.domElement.offsetTop+
"px";e.width=d.width+"px";e.height=d.height+"px";e.zIndex=c;if("undefined"!=typeof b&&""!=b)this.div.title=b;if(0!=d.width&&0!=d.height)this.sized=!0;this.domElement.parentNode.appendChild(this.div);this.div.innerHTML=this.getHTML(d.width,d.height)},positionElement:function(){var a=ZeroClipboard.getDOMObjectPosition(this.domElement),b=this.div.style;b.position="absolute";b.left=this.domElement.offsetLeft+"px";b.top=this.domElement.offsetTop+"px";b.width=a.width+"px";b.height=a.height+"px";if(0!=a.width&&
0!=a.height)this.sized=!0,b=this.div.childNodes[0],b.width=a.width,b.height=a.height},getHTML:function(a,b){var c="",d="id="+this.id+"&width="+a+"&height="+b;if(navigator.userAgent.match(/MSIE/))var e=location.href.match(/^https/i)?"https://":"http://",c=c+('<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="'+e+'download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=10,0,0,0" width="'+a+'" height="'+b+'" id="'+this.movieId+'" align="middle"><param name="allowScriptAccess" value="always" /><param name="allowFullScreen" value="false" /><param name="movie" value="'+
ZeroClipboard.moviePath+'" /><param name="loop" value="false" /><param name="menu" value="false" /><param name="quality" value="best" /><param name="bgcolor" value="#ffffff" /><param name="flashvars" value="'+d+'"/><param name="wmode" value="transparent"/></object>');else c+='<embed id="'+this.movieId+'" src="'+ZeroClipboard.moviePath+'" loop="false" menu="false" quality="best" bgcolor="#ffffff" width="'+a+'" height="'+b+'" name="'+this.movieId+'" align="middle" allowScriptAccess="always" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="'+
d+'" wmode="transparent" />';return c},hide:function(){if(this.div)this.div.style.left="-2000px"},show:function(){this.reposition()},destroy:function(){if(this.domElement&&this.div){this.hide();this.div.innerHTML="";var a=document.getElementsByTagName("body")[0];try{a.removeChild(this.div)}catch(b){}this.div=this.domElement=null}},reposition:function(a){if(a)(this.domElement=ZeroClipboard.$(a))||this.hide();if(this.domElement&&this.div){var a=ZeroClipboard.getDOMObjectPosition(this.domElement),b=
this.div.style;b.left=""+a.left+"px";b.top=""+a.top+"px"}},clearText:function(){this.clipText="";this.ready&&this.movie.clearText()},appendText:function(a){this.clipText+=a;this.ready&&this.movie.appendText(a)},setText:function(a){this.clipText=a;this.ready&&this.movie.setText(a)},setCharSet:function(a){this.charSet=a;this.ready&&this.movie.setCharSet(a)},setBomInc:function(a){this.incBom=a;this.ready&&this.movie.setBomInc(a)},setFileName:function(a){this.fileName=a;this.ready&&this.movie.setFileName(a)},
setAction:function(a){this.action=a;this.ready&&this.movie.setAction(a)},addEventListener:function(a,b){a=a.toString().toLowerCase().replace(/^on/,"");this.handlers[a]||(this.handlers[a]=[]);this.handlers[a].push(b)},setHandCursor:function(a){this.handCursorEnabled=a;this.ready&&this.movie.setHandCursor(a)},setCSSEffects:function(a){this.cssEffects=!!a},receiveEvent:function(a,b){a=a.toString().toLowerCase().replace(/^on/,"");switch(a){case "load":this.movie=document.getElementById(this.movieId);
if(!this.movie){var c=this;setTimeout(function(){c.receiveEvent("load",null)},1);return}if(!this.ready&&navigator.userAgent.match(/Firefox/)&&navigator.userAgent.match(/Windows/)){c=this;setTimeout(function(){c.receiveEvent("load",null)},100);this.ready=!0;return}this.ready=!0;this.movie.clearText();this.movie.appendText(this.clipText);this.movie.setFileName(this.fileName);this.movie.setAction(this.action);this.movie.setCharSet(this.charSet);this.movie.setBomInc(this.incBom);this.movie.setHandCursor(this.handCursorEnabled);
break;case "mouseover":this.domElement&&this.cssEffects&&this.recoverActive&&this.domElement.addClass("active");break;case "mouseout":if(this.domElement&&this.cssEffects&&(this.recoverActive=!1,this.domElement.hasClass("active")))this.domElement.removeClass("active"),this.recoverActive=!0;break;case "mousedown":this.domElement&&this.cssEffects&&this.domElement.addClass("active");break;case "mouseup":if(this.domElement&&this.cssEffects)this.domElement.removeClass("active"),this.recoverActive=!1}if(this.handlers[a])for(var d=
0,e=this.handlers[a].length;d<e;d++){var f=this.handlers[a][d];if("function"==typeof f)f(this,b);else if("object"==typeof f&&2==f.length)f[0][f[1]](this,b);else if("string"==typeof f)window[f](this,b)}}};


/*
 * File:        TableTools.min.js
 * Version:     2.0.2
 * Author:      Allan Jardine (www.sprymedia.co.uk)
 * 
 * Copyright 2009-2011 Allan Jardine, all rights reserved.
 *
 * This source file is free software, under either the GPL v2 license or a
 * BSD (3 point) style license, as supplied with this software.
 * BSD (3 point) style license, as supplied with this software.
 * 
 * This source file is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
 * or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
 */
var TableTools;
(function(e,n,h){TableTools=function(a,b){(!this.CLASS||"TableTools"!=this.CLASS)&&alert("Warning: TableTools must be initialised with the keyword 'new'");this.s={that:this,dt:null,print:{saveStart:-1,saveLength:-1,saveScroll:-1,funcEnd:function(){}},buttonCounter:0,select:{type:"",selected:[],preRowSelect:null,postSelected:null,postDeselected:null,all:!1,selectedClass:""},custom:{},swfPath:"",buttonSet:[],master:!1};this.dom={container:null,table:null,print:{hidden:[],message:null},collection:{collection:null,
background:null}};this.fnSettings=function(){return this.s};"undefined"==typeof b&&(b={});this.s.dt=a.fnSettings();this._fnConstruct(b);return this};TableTools.prototype={fnGetSelected:function(){return this._fnGetMasterSettings().select.selected},fnGetSelectedData:function(){for(var a=this._fnGetMasterSettings().select.selected,b=[],c=0,d=a.length;c<d;c++)b.push(this.s.dt.oInstance.fnGetData(a[c]));return b},fnIsSelected:function(a){for(var b=this.fnGetSelected(),c=0,d=b.length;c<d;c++)if(a==b[c])return!0;
return!1},fnSelectAll:function(){this._fnGetMasterSettings().that._fnRowSelectAll()},fnSelectNone:function(){this._fnGetMasterSettings().that._fnRowDeselectAll()},fnSelect:function(a){this.fnIsSelected(a)||("single"==this.s.select.type?this._fnRowSelectSingle(a):"multi"==this.s.select.type&&this._fnRowSelectMulti(a))},fnDeselect:function(a){this.fnIsSelected(a)&&("single"==this.s.select.type?this._fnRowSelectSingle(a):"multi"==this.s.select.type&&this._fnRowSelectMulti(a))},fnGetTitle:function(a){var b=
"";if("undefined"!=typeof a.sTitle&&""!==a.sTitle)b=a.sTitle;else if(a=h.getElementsByTagName("title"),0<a.length)b=a[0].innerHTML;return 4>"\u00a1".toString().length?b.replace(/[^a-zA-Z0-9_\u00A1-\uFFFF\.,\-_ !\(\)]/g,""):b.replace(/[^a-zA-Z0-9_\.,\-_ !\(\)]/g,"")},fnCalcColRatios:function(a){var b=this.s.dt.aoColumns,a=this._fnColumnTargets(a.mColumns),c=[],d=0,e=0,f,g;for(f=0,g=a.length;f<g;f++)if(a[f])d=b[f].nTh.offsetWidth,e+=d,c.push(d);for(f=0,g=c.length;f<g;f++)c[f]/=e;return c.join("\t")},
fnGetTableData:function(a){if(this.s.dt)return this._fnGetDataTablesData(a)},fnSetText:function(a,b){this._fnFlashSetText(a,b)},fnResizeButtons:function(){for(var a in ZeroClipboard.clients)if(a){var b=ZeroClipboard.clients[a];"undefined"!=typeof b.domElement&&b.domElement.parentNode==this.dom.container&&b.positionElement()}},fnResizeRequired:function(){for(var a in ZeroClipboard.clients)if(a){var b=ZeroClipboard.clients[a];if("undefined"!=typeof b.domElement&&b.domElement.parentNode==this.dom.container&&
!1===b.sized)return!0}return!1},_fnConstruct:function(a){var b=this;this._fnCustomiseSettings(a);this.dom.container=h.createElement("div");this.dom.container.className=!this.s.dt.bJUI?"DTTT_container":"DTTT_container ui-buttonset ui-buttonset-multi";"none"!=this.s.select.type&&this._fnRowSelectConfig();this._fnButtonDefinations(this.s.buttonSet,this.dom.container);this.s.dt.aoDestroyCallback.push({sName:"TableTools",fn:function(){b.dom.container.innerHTML=""}})},_fnCustomiseSettings:function(a){if("undefined"==
typeof this.s.dt._TableToolsInit)this.s.master=!0,this.s.dt._TableToolsInit=!0;this.dom.table=this.s.dt.nTable;this.s.custom=e.extend({},TableTools.DEFAULTS,a);this.s.swfPath=this.s.custom.sSwfPath;if("undefined"!=typeof ZeroClipboard)ZeroClipboard.moviePath=this.s.swfPath;this.s.select.type=this.s.custom.sRowSelect;this.s.select.preRowSelect=this.s.custom.fnPreRowSelect;this.s.select.postSelected=this.s.custom.fnRowSelected;this.s.select.postDeselected=this.s.custom.fnRowDeselected;this.s.select.selectedClass=
this.s.custom.sSelectedClass;this.s.buttonSet=this.s.custom.aButtons},_fnButtonDefinations:function(a,b){for(var c,d=0,j=a.length;d<j;d++){if("string"==typeof a[d]){if("undefined"==typeof TableTools.BUTTONS[a[d]]){alert("TableTools: Warning - unknown button type: "+a[d]);continue}c=e.extend({},TableTools.BUTTONS[a[d]],!0)}else{if("undefined"==typeof TableTools.BUTTONS[a[d].sExtends]){alert("TableTools: Warning - unknown button type: "+a[d].sExtends);continue}c=e.extend({},TableTools.BUTTONS[a[d].sExtends],
!0);c=e.extend(c,a[d],!0)}this.s.dt.bJUI&&(c.sButtonClass+=" ui-button ui-state-default",c.sButtonClassHover+=" ui-state-hover");b.appendChild(this._fnCreateButton(c))}},_fnCreateButton:function(a){var b="div"==a.sAction?this._fnDivBase(a):this._fnButtonBase(a);"print"==a.sAction?this._fnPrintConfig(b,a):a.sAction.match(/flash/)?this._fnFlashConfig(b,a):"text"==a.sAction?this._fnTextConfig(b,a):"div"==a.sAction?this._fnTextConfig(b,a):"collection"==a.sAction&&(this._fnTextConfig(b,a),this._fnCollectionConfig(b,
a));return b},_fnButtonBase:function(a){var b=h.createElement("button"),c=h.createElement("span"),d=this._fnGetMasterSettings();b.className="DTTT_button "+a.sButtonClass;b.setAttribute("id","ToolTables_"+this.s.dt.sInstance+"_"+d.buttonCounter);b.appendChild(c);c.innerHTML=a.sButtonText;d.buttonCounter++;return b},_fnDivBase:function(a){var b=h.createElement("div"),c=this._fnGetMasterSettings();b.className=a.sButtonClass;b.setAttribute("id","ToolTables_"+this.s.dt.sInstance+"_"+c.buttonCounter);b.innerHTML=
a.sButtonText;null!==a.nContent&&b.appendChild(a.nContent);c.buttonCounter++;return b},_fnGetMasterSettings:function(){if(this.s.master)return this.s;for(var a=TableTools._aInstances,b=0,c=a.length;b<c;b++)if(this.dom.table==a[b].s.dt.nTable)return a[b].s},_fnCollectionConfig:function(a,b){var c=h.createElement("div");c.style.display="none";c.className=!this.s.dt.bJUI?"DTTT_collection":"DTTT_collection ui-buttonset ui-buttonset-multi";b._collection=c;this._fnButtonDefinations(b.aButtons,c)},_fnCollectionShow:function(a,
b){var c=this,d=e(a).offset(),j=b._collection,f=d.left,d=d.top+e(a).outerHeight(),g=e(n).height(),l=e(h).height(),m=e(n).width(),o=e(h).width();j.style.position="absolute";j.style.left=f+"px";j.style.top=d+"px";j.style.display="block";e(j).css("opacity",0);var k=h.createElement("div");k.style.position="absolute";k.style.left="0px";k.style.top="0px";k.style.height=(g>l?g:l)+"px";k.style.width=(m>o?m:o)+"px";k.className="DTTT_collection_background";e(k).css("opacity",0);h.body.appendChild(k);h.body.appendChild(j);
g=e(j).outerWidth();m=e(j).outerHeight();if(f+g>o)j.style.left=o-g+"px";if(d+m>l)j.style.top=d-m-e(a).outerHeight()+"px";this.dom.collection.collection=j;this.dom.collection.background=k;setTimeout(function(){e(j).animate({opacity:1},500);e(k).animate({opacity:0.25},500)},10);e(k).click(function(){c._fnCollectionHide.call(c,null,null)})},_fnCollectionHide:function(a,b){if(!(null!==b&&"collection"==b.sExtends)&&null!==this.dom.collection.collection)e(this.dom.collection.collection).animate({opacity:0},
500,function(){this.style.display="none"}),e(this.dom.collection.background).animate({opacity:0},500,function(){this.parentNode.removeChild(this)}),this.dom.collection.collection=null,this.dom.collection.background=null},_fnRowSelectConfig:function(){if(this.s.master){var a=this;e(a.s.dt.nTable).addClass("DTTT_selectable");e("tr",a.s.dt.nTBody).live("click",function(b){if(this.parentNode==a.s.dt.nTBody){var c=a.s.dt.oInstance.fnGetNodes();-1===e.inArray(this,c)||null!==a.s.select.preRowSelect&&!a.s.select.preRowSelect.call(a,
b)||("single"==a.s.select.type?a._fnRowSelectSingle.call(a,this):a._fnRowSelectMulti.call(a,this))}});a.s.dt.aoDrawCallback.push({fn:function(){a.s.select.all&&a.s.dt.oFeatures.bServerSide&&a.fnSelectAll()},sName:"TableTools_select"})}},_fnRowSelectSingle:function(a){this.s.master&&!e("td",a).hasClass(this.s.dt.oClasses.sRowEmpty)&&(e(a).hasClass(this.s.select.selectedClass)?this._fnRowDeselect(a):(0!==this.s.select.selected.length&&this._fnRowDeselectAll(),this.s.select.selected.push(a),e(a).addClass(this.s.select.selectedClass),
null!==this.s.select.postSelected&&this.s.select.postSelected.call(this,a)),TableTools._fnEventDispatch(this,"select",a))},_fnRowSelectMulti:function(a){this.s.master&&!e("td",a).hasClass(this.s.dt.oClasses.sRowEmpty)&&(e(a).hasClass(this.s.select.selectedClass)?this._fnRowDeselect(a):(this.s.select.selected.push(a),e(a).addClass(this.s.select.selectedClass),null!==this.s.select.postSelected&&this.s.select.postSelected.call(this,a)),TableTools._fnEventDispatch(this,"select",a))},_fnRowSelectAll:function(){if(this.s.master){for(var a,
b=0,c=this.s.dt.aiDisplayMaster.length;b<c;b++)a=this.s.dt.aoData[this.s.dt.aiDisplayMaster[b]].nTr,e(a).hasClass(this.s.select.selectedClass)||(this.s.select.selected.push(a),e(a).addClass(this.s.select.selectedClass));null!==this.s.select.postSelected&&this.s.select.postSelected.call(this,null);this.s.select.all=!0;TableTools._fnEventDispatch(this,"select",null)}},_fnRowDeselectAll:function(){if(this.s.master){for(var a=this.s.select.selected.length-1;0<=a;a--)this._fnRowDeselect(a,!1);null!==this.s.select.postDeselected&&
this.s.select.postDeselected.call(this,null);this.s.select.all=!1;TableTools._fnEventDispatch(this,"select",null)}},_fnRowDeselect:function(a,b){"undefined"!=typeof a.nodeName&&(a=e.inArray(a,this.s.select.selected));var c=this.s.select.selected[a];e(c).removeClass(this.s.select.selectedClass);this.s.select.selected.splice(a,1);("undefined"==typeof b||b)&&null!==this.s.select.postDeselected&&this.s.select.postDeselected.call(this,c);this.s.select.all=!1},_fnTextConfig:function(a,b){var c=this;null!==
b.fnInit&&b.fnInit.call(this,a,b);if(""!==b.sToolTip)a.title=b.sToolTip;e(a).hover(function(){e(a).addClass(b.sButtonClassHover);null!==b.fnMouseover&&b.fnMouseover.call(this,a,b,null)},function(){e(a).removeClass(b.sButtonClassHover);null!==b.fnMouseout&&b.fnMouseout.call(this,a,b,null)});null!==b.fnSelect&&TableTools._fnEventListen(this,"select",function(d){b.fnSelect.call(c,a,b,d)});e(a).click(function(d){d.preventDefault();null!==b.fnClick&&b.fnClick.call(c,a,b,null);null!==b.fnComplete&&b.fnComplete.call(c,
a,b,null,null);c._fnCollectionHide(a,b)})},_fnFlashConfig:function(a,b){var c=this,d=new ZeroClipboard.Client;null!==b.fnInit&&b.fnInit.call(this,a,b);d.setHandCursor(!0);"flash_save"==b.sAction?(d.setAction("save"),d.setCharSet("utf16le"==b.sCharSet?"UTF16LE":"UTF8"),d.setBomInc(b.bBomInc),d.setFileName(b.sFileName.replace("*",this.fnGetTitle(b)))):"flash_pdf"==b.sAction?(d.setAction("pdf"),d.setFileName(b.sFileName.replace("*",this.fnGetTitle(b)))):d.setAction("copy");d.addEventListener("mouseOver",
function(){e(a).addClass(b.sButtonClassHover);null!==b.fnMouseover&&b.fnMouseover.call(c,a,b,d)});d.addEventListener("mouseOut",function(){e(a).removeClass(b.sButtonClassHover);null!==b.fnMouseout&&b.fnMouseout.call(c,a,b,d)});d.addEventListener("mouseDown",function(){null!==b.fnClick&&b.fnClick.call(c,a,b,d)});d.addEventListener("complete",function(e,f){null!==b.fnComplete&&b.fnComplete.call(c,a,b,d,f);c._fnCollectionHide(a,b)});this._fnFlashGlue(d,a,b.sToolTip)},_fnFlashGlue:function(a,b,c){var d=
this,e=b.getAttribute("id");if(h.getElementById(e)){if(a.glue(b,c),a.domElement.parentNode!=a.div.parentNode&&"undefined"==typeof d.__bZCWarning)d.s.dt.oApi._fnLog(this.s.dt,0,"It looks like you are using the version of ZeroClipboard which came with TableTools 1. Please update to use the version that came with TableTools 2."),d.__bZCWarning=!0}else setTimeout(function(){d._fnFlashGlue(a,b,c)},100)},_fnFlashSetText:function(a,b){var c=this._fnChunkData(b,8192);a.clearText();for(var d=0,e=c.length;d<
e;d++)a.appendText(c[d])},_fnColumnTargets:function(a){var b=[],c=this.s.dt;if("object"==typeof a){for(i=0,iLen=c.aoColumns.length;i<iLen;i++)b.push(!1);for(i=0,iLen=a.length;i<iLen;i++)b[a[i]]=!0}else if("visible"==a)for(i=0,iLen=c.aoColumns.length;i<iLen;i++)b.push(c.aoColumns[i].bVisible?!0:!1);else if("hidden"==a)for(i=0,iLen=c.aoColumns.length;i<iLen;i++)b.push(c.aoColumns[i].bVisible?!1:!0);else if("sortable"==a)for(i=0,iLen=c.aoColumns.length;i<iLen;i++)b.push(c.aoColumns[i].bSortable?!0:!1);
else for(i=0,iLen=c.aoColumns.length;i<iLen;i++)b.push(!0);return b},_fnNewline:function(a){return"auto"==a.sNewLine?navigator.userAgent.match(/Windows/)?"\r\n":"\n":a.sNewLine},_fnGetDataTablesData:function(a){var b,c,d,j,f="",g="",h=this.s.dt,m=RegExp(a.sFieldBoundary,"g"),o=this._fnColumnTargets(a.mColumns),k=this._fnNewline(a),n="undefined"!=typeof a.bSelectedOnly?a.bSelectedOnly:!1;if(a.bHeader){for(b=0,c=h.aoColumns.length;b<c;b++)o[b]&&(g=h.aoColumns[b].sTitle.replace(/\n/g," ").replace(/<.*?>/g,
"").replace(/^\s+|\s+$/g,""),g=this._fnHtmlDecode(g),f+=this._fnBoundData(g,a.sFieldBoundary,m)+a.sFieldSeperator);f=f.slice(0,-1*a.sFieldSeperator.length);f+=k}for(d=0,j=h.aiDisplay.length;d<j;d++)if("none"==this.s.select.type||n&&e(h.aoData[h.aiDisplay[d]].nTr).hasClass(this.s.select.selectedClass)||n&&0==this.s.select.selected.length){for(b=0,c=h.aoColumns.length;b<c;b++)o[b]&&(g=h.oApi._fnGetCellData(h,h.aiDisplay[d],b,"display"),a.fnCellRender?g=a.fnCellRender(g,b)+"":"string"==typeof g?(g=g.replace(/\n/g,
" "),g=g.replace(/<img.*?\s+alt\s*=\s*(?:"([^"]+)"|'([^']+)'|([^\s>]+)).*?>/gi,"$1$2$3"),g=g.replace(/<.*?>/g,"")):g+="",g=g.replace(/^\s+/,"").replace(/\s+$/,""),g=this._fnHtmlDecode(g),f+=this._fnBoundData(g,a.sFieldBoundary,m)+a.sFieldSeperator);f=f.slice(0,-1*a.sFieldSeperator.length);f+=k}f.slice(0,-1);if(a.bFooter){for(b=0,c=h.aoColumns.length;b<c;b++)o[b]&&null!==h.aoColumns[b].nTf&&(g=h.aoColumns[b].nTf.innerHTML.replace(/\n/g," ").replace(/<.*?>/g,""),g=this._fnHtmlDecode(g),f+=this._fnBoundData(g,
a.sFieldBoundary,m)+a.sFieldSeperator);f=f.slice(0,-1*a.sFieldSeperator.length)}return _sLastData=f},_fnBoundData:function(a,b,c){return""===b?a:b+a.replace(c,b+b)+b},_fnChunkData:function(a,b){for(var c=[],d=a.length,e=0;e<d;e+=b)e+b<d?c.push(a.substring(e,e+b)):c.push(a.substring(e,d));return c},_fnHtmlDecode:function(a){if(-1==a.indexOf("&"))return a;var a=this._fnChunkData(a,2048),b=h.createElement("div"),c,d,e,f="";for(c=0,d=a.length;c<d;c++)e=a[c].lastIndexOf("&"),-1!=e&&8<=a[c].length&&e>a[c].length-
8&&(a[c].substr(e),a[c]=a[c].substr(0,e)),b.innerHTML=a[c],f+=b.childNodes[0].nodeValue;return f},_fnPrintConfig:function(a,b){var c=this;null!==b.fnInit&&b.fnInit.call(this,a,b);if(""!==b.sToolTip)a.title=b.sToolTip;e(a).hover(function(){e(a).addClass(b.sButtonClassHover)},function(){e(a).removeClass(b.sButtonClassHover)});null!==b.fnSelect&&TableTools._fnEventListen(this,"select",function(d){b.fnSelect.call(c,a,b,d)});e(a).click(function(d){d.preventDefault();c._fnPrintStart.call(c,d,b);null!==
b.fnClick&&b.fnClick.call(c,a,b,null);null!==b.fnComplete&&b.fnComplete.call(c,a,b,null,null);c._fnCollectionHide(a,b)})},_fnPrintStart:function(a,b){var c=this,d=this.s.dt;this._fnPrintHideNodes(d.nTable);this.s.print.saveStart=d._iDisplayStart;this.s.print.saveLength=d._iDisplayLength;if(b.bShowAll)d._iDisplayStart=0,d._iDisplayLength=-1,d.oApi._fnCalculateEnd(d),d.oApi._fnDraw(d);(""!==d.oScroll.sX||""!==d.oScroll.sY)&&this._fnPrintScrollStart(d);var d=d.aanFeatures,j;for(j in d)if("i"!=j&&"t"!=
j&&1==j.length)for(var f=0,g=d[j].length;f<g;f++)this.dom.print.hidden.push({node:d[j][f],display:"block"}),d[j][f].style.display="none";e(h.body).addClass("DTTT_Print");if(""!==b.sInfo){var l=h.createElement("div");l.className="DTTT_print_info";l.innerHTML=b.sInfo;h.body.appendChild(l);setTimeout(function(){e(l).fadeOut("normal",function(){h.body.removeChild(l)})},2E3)}if(""!==b.sMessage)this.dom.print.message=h.createElement("div"),this.dom.print.message.className="DTTT_PrintMessage",this.dom.print.message.innerHTML=
b.sMessage,h.body.insertBefore(this.dom.print.message,h.body.childNodes[0]);this.s.print.saveScroll=e(n).scrollTop();n.scrollTo(0,0);this.s.print.funcEnd=function(a){c._fnPrintEnd.call(c,a)};e(h).bind("keydown",null,this.s.print.funcEnd)},_fnPrintEnd:function(a){if(27==a.keyCode){a.preventDefault();var a=this.s.dt,b=this.s.print,c=this.dom.print;this._fnPrintShowNodes();(""!==a.oScroll.sX||""!==a.oScroll.sY)&&this._fnPrintScrollEnd();n.scrollTo(0,b.saveScroll);if(null!==c.message)h.body.removeChild(c.message),
c.message=null;e(h.body).removeClass("DTTT_Print");a._iDisplayStart=b.saveStart;a._iDisplayLength=b.saveLength;a.oApi._fnCalculateEnd(a);a.oApi._fnDraw(a);e(h).unbind("keydown",this.s.print.funcEnd);this.s.print.funcEnd=null}},_fnPrintScrollStart:function(){var a=this.s.dt;a.nScrollHead.getElementsByTagName("div")[0].getElementsByTagName("table");var b=a.nTable.parentNode,c=a.nTable.getElementsByTagName("thead");0<c.length&&a.nTable.removeChild(c[0]);null!==a.nTFoot&&(c=a.nTable.getElementsByTagName("tfoot"),
0<c.length&&a.nTable.removeChild(c[0]));c=a.nTHead.cloneNode(!0);a.nTable.insertBefore(c,a.nTable.childNodes[0]);null!==a.nTFoot&&(c=a.nTFoot.cloneNode(!0),a.nTable.insertBefore(c,a.nTable.childNodes[1]));if(""!==a.oScroll.sX)a.nTable.style.width=e(a.nTable).outerWidth()+"px",b.style.width=e(a.nTable).outerWidth()+"px",b.style.overflow="visible";if(""!==a.oScroll.sY)b.style.height=e(a.nTable).outerHeight()+"px",b.style.overflow="visible"},_fnPrintScrollEnd:function(){var a=this.s.dt,b=a.nTable.parentNode;
if(""!==a.oScroll.sX)b.style.width=a.oApi._fnStringToCss(a.oScroll.sX),b.style.overflow="auto";if(""!==a.oScroll.sY)b.style.height=a.oApi._fnStringToCss(a.oScroll.sY),b.style.overflow="auto"},_fnPrintShowNodes:function(){for(var a=this.dom.print.hidden,b=0,c=a.length;b<c;b++)a[b].node.style.display=a[b].display;a.splice(0,a.length)},_fnPrintHideNodes:function(a){for(var b=this.dom.print.hidden,c=a.parentNode,d=c.childNodes,j=0,f=d.length;j<f;j++)if(d[j]!=a&&1==d[j].nodeType){var g=e(d[j]).css("display");
if("none"!=g)b.push({node:d[j],display:g}),d[j].style.display="none"}"BODY"!=c.nodeName&&this._fnPrintHideNodes(c)}};TableTools._aInstances=[];TableTools._aListeners=[];TableTools.fnGetMasters=function(){for(var a=[],b=0,c=TableTools._aInstances.length;b<c;b++)TableTools._aInstances[b].s.master&&a.push(TableTools._aInstances[b]);return a};TableTools.fnGetInstance=function(a){"object"!=typeof a&&(a=h.getElementById(a));for(var b=0,c=TableTools._aInstances.length;b<c;b++)if(TableTools._aInstances[b].s.master&&
TableTools._aInstances[b].dom.table==a)return TableTools._aInstances[b];return null};TableTools._fnEventListen=function(a,b,c){TableTools._aListeners.push({that:a,type:b,fn:c})};TableTools._fnEventDispatch=function(a,b,c){for(var d=TableTools._aListeners,e=0,f=d.length;e<f;e++)a.dom.table==d[e].that.dom.table&&d[e].type==b&&d[e].fn(c)};TableTools.BUTTONS={csv:{sAction:"flash_save",sCharSet:"utf8",bBomInc:!1,sFileName:"*.csv",sFieldBoundary:'"',sFieldSeperator:",",sNewLine:"auto",sTitle:"",sToolTip:"",
sButtonClass:"DTTT_button_csv",sButtonClassHover:"DTTT_button_csv_hover",sButtonText:"CSV",mColumns:"all",bHeader:!0,bFooter:!0,bSelectedOnly:!1,fnMouseover:null,fnMouseout:null,fnClick:function(a,b,c){this.fnSetText(c,this.fnGetTableData(b))},fnSelect:null,fnComplete:null,fnInit:null,fnCellRender:null},xls:{sAction:"flash_save",sCharSet:"utf16le",bBomInc:!0,sFileName:"*.xls",sFieldBoundary:"",sFieldSeperator:"\t",sNewLine:"auto",sTitle:"",sToolTip:"",sButtonClass:"DTTT_button_xls",sButtonClassHover:"DTTT_button_xls_hover",
sButtonText:"Excel",mColumns:"all",bHeader:!0,bFooter:!0,bSelectedOnly:!1,fnMouseover:null,fnMouseout:null,fnClick:function(a,b,c){this.fnSetText(c,this.fnGetTableData(b))},fnSelect:null,fnComplete:null,fnInit:null,fnCellRender:null},copy:{sAction:"flash_copy",sFieldBoundary:"",sFieldSeperator:"\t",sNewLine:"auto",sToolTip:"",sButtonClass:"DTTT_button_copy",sButtonClassHover:"DTTT_button_copy_hover",sButtonText:"Copy",mColumns:"all",bHeader:!0,bFooter:!0,bSelectedOnly:!1,fnMouseover:null,fnMouseout:null,
fnClick:function(a,b,c){this.fnSetText(c,this.fnGetTableData(b))},fnSelect:null,fnComplete:function(a,b,c,d){a=d.split("\n").length;a=null===this.s.dt.nTFoot?a-1:a-2;alert("Copied "+a+" row"+(1==a?"":"s")+" to the clipboard")},fnInit:null,fnCellRender:null},pdf:{sAction:"flash_pdf",sFieldBoundary:"",sFieldSeperator:"\t",sNewLine:"\n",sFileName:"*.pdf",sToolTip:"",sTitle:"",sButtonClass:"DTTT_button_pdf",sButtonClassHover:"DTTT_button_pdf_hover",sButtonText:"PDF",mColumns:"all",bHeader:!0,bFooter:!1,
bSelectedOnly:!1,fnMouseover:null,fnMouseout:null,sPdfOrientation:"portrait",sPdfSize:"A4",sPdfMessage:"",fnClick:function(a,b,c){this.fnSetText(c,"title:"+this.fnGetTitle(b)+"\nmessage:"+b.sPdfMessage+"\ncolWidth:"+this.fnCalcColRatios(b)+"\norientation:"+b.sPdfOrientation+"\nsize:"+b.sPdfSize+"\n--/TableToolsOpts--\n"+this.fnGetTableData(b))},fnSelect:null,fnComplete:null,fnInit:null,fnCellRender:null},print:{sAction:"print",sInfo:"<h6>Print view</h6><p>Please use your browser's print function to print this table. Press escape when finished.",
sMessage:"",bShowAll:!0,sToolTip:"View print view",sButtonClass:"DTTT_button_print",sButtonClassHover:"DTTT_button_print_hover",sButtonText:"Print",fnMouseover:null,fnMouseout:null,fnClick:null,fnSelect:null,fnComplete:null,fnInit:null,fnCellRender:null},text:{sAction:"text",sToolTip:"",sButtonClass:"DTTT_button_text",sButtonClassHover:"DTTT_button_text_hover",sButtonText:"Text button",mColumns:"all",bHeader:!0,bFooter:!0,bSelectedOnly:!1,fnMouseover:null,fnMouseout:null,fnClick:null,fnSelect:null,
fnComplete:null,fnInit:null,fnCellRender:null},select:{sAction:"text",sToolTip:"",sButtonClass:"DTTT_button_text",sButtonClassHover:"DTTT_button_text_hover",sButtonText:"Select button",mColumns:"all",bHeader:!0,bFooter:!0,fnMouseover:null,fnMouseout:null,fnClick:null,fnSelect:function(a){0!==this.fnGetSelected().length?e(a).removeClass("DTTT_disabled"):e(a).addClass("DTTT_disabled")},fnComplete:null,fnInit:function(a){e(a).addClass("DTTT_disabled")},fnCellRender:null},select_single:{sAction:"text",
sToolTip:"",sButtonClass:"DTTT_button_text",sButtonClassHover:"DTTT_button_text_hover",sButtonText:"Select button",mColumns:"all",bHeader:!0,bFooter:!0,fnMouseover:null,fnMouseout:null,fnClick:null,fnSelect:function(a){1==this.fnGetSelected().length?e(a).removeClass("DTTT_disabled"):e(a).addClass("DTTT_disabled")},fnComplete:null,fnInit:function(a){e(a).addClass("DTTT_disabled")},fnCellRender:null},select_all:{sAction:"text",sToolTip:"",sButtonClass:"DTTT_button_text",sButtonClassHover:"DTTT_button_text_hover",
sButtonText:"Select all",mColumns:"all",bHeader:!0,bFooter:!0,fnMouseover:null,fnMouseout:null,fnClick:function(){this.fnSelectAll()},fnSelect:function(a){this.fnGetSelected().length==this.s.dt.fnRecordsDisplay()?e(a).addClass("DTTT_disabled"):e(a).removeClass("DTTT_disabled")},fnComplete:null,fnInit:null,fnCellRender:null},select_none:{sAction:"text",sToolTip:"",sButtonClass:"DTTT_button_text",sButtonClassHover:"DTTT_button_text_hover",sButtonText:"Deselect all",mColumns:"all",bHeader:!0,bFooter:!0,
fnMouseover:null,fnMouseout:null,fnClick:function(){this.fnSelectNone()},fnSelect:function(a){0!==this.fnGetSelected().length?e(a).removeClass("DTTT_disabled"):e(a).addClass("DTTT_disabled")},fnComplete:null,fnInit:function(a){e(a).addClass("DTTT_disabled")},fnCellRender:null},ajax:{sAction:"text",sFieldBoundary:"",sFieldSeperator:"\t",sNewLine:"\n",sAjaxUrl:"/xhr.php",sToolTip:"",sButtonClass:"DTTT_button_text",sButtonClassHover:"DTTT_button_text_hover",sButtonText:"Ajax button",mColumns:"all",bHeader:!0,
bFooter:!0,bSelectedOnly:!1,fnMouseover:null,fnMouseout:null,fnClick:function(a,b){var c=this.fnGetTableData(b);e.ajax({url:b.sAjaxUrl,data:[{name:"tableData",value:c}],success:b.fnAjaxComplete,dataType:"json",type:"POST",cache:!1,error:function(){alert("Error detected when sending table data to server")}})},fnSelect:null,fnComplete:null,fnInit:null,fnAjaxComplete:function(){alert("Ajax complete")},fnCellRender:null},div:{sAction:"div",sToolTip:"",sButtonClass:"DTTT_nonbutton",sButtonClassHover:"",
sButtonText:"Text button",fnMouseover:null,fnMouseout:null,fnClick:null,fnSelect:null,fnComplete:null,fnInit:null,nContent:null,fnCellRender:null},collection:{sAction:"collection",sToolTip:"",sButtonClass:"DTTT_button_collection",sButtonClassHover:"DTTT_button_collection_hover",sButtonText:"Collection",fnMouseover:null,fnMouseout:null,fnClick:function(a,b){this._fnCollectionShow(a,b)},fnSelect:null,fnComplete:null,fnInit:null,fnCellRender:null}};TableTools.DEFAULTS={sSwfPath:"media/swf/copy_cvs_xls_pdf.swf",
sRowSelect:"none",sSelectedClass:"DTTT_selected",fnPreRowSelect:null,fnRowSelected:null,fnRowDeselected:null,aButtons:["copy","xls","print"]};TableTools.prototype.CLASS="TableTools";TableTools.VERSION="2.0.2";TableTools.prototype.VERSION=TableTools.VERSION;"function"==typeof e.fn.dataTable&&"function"==typeof e.fn.dataTableExt.fnVersionCheck&&e.fn.dataTableExt.fnVersionCheck("1.8.2")?e.fn.dataTableExt.aoFeatures.push({fnInit:function(a){a=new TableTools(a.oInstance,"undefined"!=typeof a.oInit.oTableTools?
a.oInit.oTableTools:{});TableTools._aInstances.push(a);return a.dom.container},cFeature:"T",sFeature:"TableTools"}):alert("Warning: TableTools 2 requires DataTables 1.8.2 or newer - www.datatables.net/download")})(jQuery,window,document);
