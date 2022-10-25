// rem
function sizeChange(){
	var s_width = $(window).width();
	var newFont = (s_width / 320) * 10;

	$('html').css('fontSize',newFont+'px');

	$(window).resize(function(){
		s_width = $(window).width();
	    newFont = (s_width / 320) * 10;
	    $('html').css('fontSize',newFont+'px');
	})
}

function GetHttpRequest()  
{  
    if ( window.XMLHttpRequest ) // Gecko  
        return new XMLHttpRequest() ;  
    else if ( window.ActiveXObject ) // IE  
        return new ActiveXObject("MsXml2.XmlHttp") ;  
}  

// js內同步引入外部js
function ajaxPage(sId, url){  
    var oXmlHttp = GetHttpRequest() ;  
    oXmlHttp.onreadystatechange = function()    
     {  
        if (oXmlHttp.readyState == 4)  
        {  
           includeJS( sId, url, oXmlHttp.responseText );  
        }  
    }  
    oXmlHttp.open('GET', url, false);//同步操作  
    oXmlHttp.send(null);  
}  
  
// 判斷js是否已引入
function includeJS(sId, fileUrl, source)  
{  
    if ( ( source != null ) && ( !document.getElementById( sId ) ) ){  
        var oHead = document.getElementsByTagName('HEAD').item(0);  
        var oScript = document.createElement( "script" );  
        oScript.type = "text/javascript";  
        oScript.id = sId;  
        oScript.text = source;  
        oHead.appendChild( oScript );  
    }  
}  