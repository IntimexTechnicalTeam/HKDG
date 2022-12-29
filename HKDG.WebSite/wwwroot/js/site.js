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
            domNoData  : '<div class="dropload-noData">暫無更多</div>'
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

// 引入外部js或第三方js
async function LoadScript (src, async, id, site) { // site -> 'head', js在<head>標籤內引入； 'body'，js在<body>標籤內引入
    return new Promise((resolve, reject) => {
        if (!isInclude(src)) {
          const oScript = document.createElement('script');
          oScript.type = 'text/javascript';
          oScript.src = src;

          if (async) {
            oScript.async = true;
          }

          if (id) {
            oScript.id = id;
          }

          switch (site) {
            case 'head':
              document.getElementsByTagName('head')[0].appendChild(oScript);
              break;
            default:
              document.body.appendChild(oScript);
          }

          resolve(true);
        }
    });
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

// 轉跳擺檔
function transitBD (url) {
    location.href = "/TranView/GoTo?returnUrl=" + BuyDong + url + "?IsBD=false";
}

// 導航跳轉邏輯處理
function toUrl (n) {
    if (n.Type < 0) return;

    let link;
    let flag; // 是否最後執行跳轉
    switch (n.Type) {
        case 0: // 鏈接 => 0
            if (!n.IsNewWin && n.Url) {
                window.location.href = n.Url;
            } else if (n.IsNewWin && n.Url) {
                window.open(n.Url);
            }
            break;
        case 1: // cms目錄
            link = '/product/CatProduct?catId=' + n.Value.Id;
            break;
        case 2: // cms內容
            // link = '/CMS/content/' + n.Value.Id;
            transitBD('/CMS/content/' + n.Value.Id);
            flag = true;
            break;
        case 4: // 產品目錄
            if (platform === 'M') {
                link = '/Product/Category';
            } else if (!(n.Level === 1 && guidEmpty(n.Value.Id))) {
                link = '/product/Category?catId=' + n.Value.Id;
            }
            // link = '/product/Category?catId=' + n.Value.Id;
            break;
    }

    if (link && !flag) {
        window.location.href = link;
    }
}

// 判斷guid值是否有效
function guidEmpty(value) {
  return value === '00000000-0000-0000-0000-000000000000';
}