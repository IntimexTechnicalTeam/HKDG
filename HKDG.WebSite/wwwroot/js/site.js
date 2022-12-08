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

// 滾動加載 (dom => 監聽滾動元素, totalpage => 數據總頁數)
function loadPage (dom, totalpage, callback) {
    var page = 1;
    dom.dropload({
        scrollArea : window,
        autoLoad: false,
        domDown : {
            domClass   : 'dropload-down',
            domRefresh : '<div class="dropload-refresh"></div>',
            domLoad    : '<div class="dropload-load"><span class="loading"></span></div>',
            domNoData  : '<div class="dropload-noData">我是有底線的~</div>'
        },
        loadDownFn: function(me){
            page++;

            if (page > totalpage) {
                // 锁定
                me.lock();
                // 无数据
                me.noData();

                me.resetload();
            } else {
                if (typeof callback === 'function') {
                    callback(page, me);

                    // 此處需在callback方法數據請求結束後resetload
                }
            }

            // me.resetload();
        }
    });
};


//FB加入购物车
function fbpAddToCart() {
    fbq('track', 'AddToCart');
}

//FB购买
function fbpInitiateCheckout() {
    fbq('track', 'InitiateCheckout');
}

//FB内容显示
function fbpViewContent() {
    fbq('track', 'ViewContent');
}

//FB完成付款显示
function fbpPurchase() {
    fbq('track', 'Purchase', {value: 0.00, currency: 'USD'});
}

//FB加到願望清單  
function fbpAddToWishlist() {
    fbq('track', 'AddToWishlist');
}

//FB完成註冊  
function fbpCompleteRegistration() {
    fbq('track', 'CompleteRegistration');
}


//FB開始結帳      
function fbpInitiateCheckout() {
    fbq('track', 'InitiateCheckout');
}


//FB搜尋     
function fbpSearch() {
   fbq('track', 'Search');
}

//FB訂閱     
function fbpSubscribe() {
   fbq('track', 'Subscribe', {value:'0.00', currency:'USD', predicted_ltv:'0.00'});
}

// 获取url参数
function getQueryString(name) {
    var result = decodeURIComponent(window.location.search).match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
    if (result == null || result.length < 1) {
        return "";
    }
    return result[1];
}

// 修改url参数值
function replaceParamVal(paramName, replaceWith) {
    var oUrl = this.location.href.toString();
    var re = eval('/(' + paramName + '=)([^&]*)/gi');
    var nUrl = oUrl.replace(re, paramName + '=' + replaceWith);
    return nUrl;
}

// 价格格式化
function PriceFormat(value) {
    if (typeof (value) === 'number') {
        return (value).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'); //保留两位小数
        // return (value || 0).toString().replace(/(\d)(?=(?:\d{3})+$)/g, '$1,');
    }
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

// fixed 头部
function FixedHeader () {
    var headerHight = document.getElementById("header").offsetHeight;
    window.onscroll = function () {
      //如果滚动的高度等于头部的高,那么就设置固定定位
      let scrollTop = $(document).scrollTop();

      if (scrollTop >= headerHight) {
          $("#header").addClass("fixed");
          $("#container").css("padding-top", headerHight + 'px');
      } else {
          $("#header").removeClass("fixed");
          $("#container").css("padding-top", 0);
      }
    }
}