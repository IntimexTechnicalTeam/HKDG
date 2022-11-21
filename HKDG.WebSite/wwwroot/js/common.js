var onBlock = false;

var language = {
    E: {
        confirm: 'Confirm',
        cancel: 'Cancel'  
    },
    C: {
        confirm: '確認',
        cancel: '取消'
    },
    S: {
        confirm: '确认',
        cancel: '取消'
    }
};

function getPMHost() {
    var pmserver = $.cookie("PMServer");
    if (pmserver) {
        return pmserver;
    } else {
        return "";
    }
}
function getCustUILanguage() {
    return $.cookie("uLanguage");
}

function getUILanguage() {
    //alert($("#pageLang").val());
    return $("#pageLang").val();
}

function WSAjaxStart() {
    $(document).ajaxStart(function () {
        showLoading("Loading...");
    });
}

function WSAjaxComplete() {
    $(document).ajaxComplete(function () {
        hideLoading();
    });
}

function WSGet(url, data, success, error) {
    //var accessToken = $.cookie("access_token");

    //if (!accessToken) {
    //    if (!redirectJob) {

    //        setTimeout(function () {
    //            window.location.href = window.location.href;
    //        }, 5000);
    //    }
    //    redirectJob = true;
    //    return;
    //}
    $.ajax({
        type: "get",
        url: url,
        data: data,
        contentType: 'application/json',
        beforeSend: function (request) {
            //  request.setRequestHeader("Authorization", "Bearer " + accessToken);
            var userLanguage = getUILanguage();
            request.setRequestHeader("UserLanguage", userLanguage);
        },
        statusCode: {
            404: function () {
                console.log("statusCode=404");
                //  alert("page not found");
            },
            403: function () {
                //$.cookie("access_token", null, { path: "/" });
                // setTimeout(function () { window.location.href = window.location.href; }, 5000);
                window.location.href = "/account/login?status=timeout&returnUrl=" + window.location.pathname;
            },
            500: function (xhr, status, text) {
                console.log("statusCode=500");
                alert(xhr.responseJSON.Message);
            }
        },
        success: function (data, status) {
            if (data && data.ReturnValue == 403) {
                // $.cookie("access_token", null, { path: "/" });
                if (localStorage.getItem("logined") == 1) {
                    localStorage.setItem("logined", 0);
                    window.location.href = "/account/login?status=timeout&returnUrl=" + window.location.pathname;
                } else {
                    // window.location.reload();
                }
                return;
            }
            success(data, status);
        },
        error: function (e) {

            if (error) {
                error(e);
            }
            console.log("獲取數據失敗，請稍後再試。");
        }
    });
}
var redirectJob = false;
function WSPost(url, data, success, error) {
    //var accessToken = $.cookie("access_token");
    //if (!accessToken) {
    //    if (!redirectJob) {
    //        setTimeout(function () { window.location.href = window.location.href; }, 5000);
    //    }
    //    redirectJob = true;
    //    return;
    //}
    $.ajax({
        type: "post",
        url: url,
        data: data,
        //contentType: 'application/json',
        beforeSend: function (request) {
            //  request.setRequestHeader("Authorization", "Bearer " + accessToken);
            var userLanguage = getUILanguage();
            request.setRequestHeader("UserLanguage", userLanguage);
        },
        statusCode: {
            404: function () {
                alert("page not found");
            },
            403: function () {
                // $.cookie("access_token", null, { path: "/" });
                // setTimeout(function () { window.location.href = window.location.href; }, 5000);
                window.location.href = "/account/login?status=timeout&returnUrl=" + window.location.pathname;
            }
        },
        success: function (data, status) {
            if (data && data.ReturnValue == 403) {
                // $.cookie("access_token", null, { path: "/" });
                if (localStorage.getItem("logined") == 1) {
                    localStorage.setItem("logined", 0);
                    window.location.href = "/account/login?returnUrl=" + window.location.href;
                } else {
                    // window.location.reload();
                }
                return;
            }
            success(data, status);

        },
        error: function (e) {
            if (error) {
                error(e);
            }
            console.log("獲取數據失敗，請稍後再試。");
        }
    });
}

//single parameter
function WSAjaxSP(p) {
    p.beforeSend = function (request) {
        //var accessToken = $.cookie("access_token"); 
        //  request.setRequestHeader("Authorization", "Bearer " + accessToken);

        var userLanguage = getUILanguage();
        request.setRequestHeader("UserLanguage", userLanguage);

    };
    if (!p.statusCode) {
        p.statusCode = {
            404: function () {
                alert("page not found");
            },
            403: function () {
                // $.cookie("access_token", null, { path: "/" });
                // setTimeout(function () { window.location.href = window.location.href; }, 5000);
            }
        };
    }

    $.ajax(p);
}

//type:post、get
//url:api路径
//data: var obj = new Object();
//      obj.id = id;
//successcallback:执行成功的回调函数
//errorcallback:执行失败的回调函数
function WSAjax(type, url, data, successcallback, errorcallback) {
    $.ajax({
        type: type,
        url: url,
        data: data,
        dataType: "json",
        success: function (response) {
            if (data && data.ReturnValue == 403) {
                //  $.cookie("access_token", null, { path: "/" });
                window.location.href = "/";
                return;
            }
            successcallback(response);
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }, 403: function () {
                //  $.cookie("access_token", null, { path: "/" });
                // setTimeout(function () { window.location.href = window.location.href; }, 5000);
            }
        },
        error: function () {
            errorcallback();
        },
        beforeSend: function (request) {
            // var accessToken = $.cookie("access_token");
            // request.setRequestHeader("Authorization", "Bearer " + accessToken);

            var userLanguage = getUILanguage();
            request.setRequestHeader("UserLanguage", userLanguage);
        }
    });
}

var inited_loadingUI = false;

function initLoading() {
    $("body").append("<!-- loading -->" +
        "<div class='modal fade' id='loading' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' data-backdrop='static'>" +
        "<div class='modal-dialog' role='document'>" +
        "<div class='modal-content'>" +
        "<div class='modal-header'>" +
        "<h4 class='modal-title' id='myModalLabel'>Tips</h4>" +
        "</div>" +
        "<div id='loadingText' class='modal-body'>" +
        "<span class='glyphicon glyphicon-refresh' aria-hidden='true'>1</span>" +
        "处理中，请稍候。。。" +
        "</div>" +
        "</div>" +
        "</div>" +
        "</div>"
    );
    inited_loadingUI = true;
}

function showLoading(text, closeDelay) {

    if (!inited_loadingUI) {
        initLoading();
    }

    if (closeDelay) {
        hideLoading(closeDelay);
    }

    if (!text) {
        text = "";
    }

    //$("#loadingText").html(text);
    //$("#loading").modal("show"); 
    // $.blockUI({ message: "<img width='100' src='/images/loading.gif' style='vertical-align:middle;'/>" + text });
    $.blockUI({ 
        message: "<div class='window loading'><img src='/images/loading.gif' />" + text + "</div>", 
        blockMsgClass: "popBlock" 
    });

}

function hideLoading(delay) {
    if (!delay) {
        delay = 500;
    }
    //setTimeout(function () {
    //    $("#loading").modal("hide");
    //}, delay);
    if ( !onBlock ) {
        setTimeout(function () {
            $.unblockUI();
        }, delay);
    }

}


function showInfo(message, sec, callback) {
    showInfoExtend({ icon: "/Images/warn-icon.png", message: message, sec: sec, callback: callback });
}
function showInfoSuccess(message, sec, callback) {
    showInfoExtend({ icon: "/Images/tick-icon.png", message: message, sec: sec, callback: callback });
}
// function showMessage(message) {
//     $.blockUI({
//         message: "<div style='padding:20px 0;'><p style='font-size:16px;'>" + message + "</p></div>",
//         css: {}
//     });
//     setTimeout($.unblockUI, 2000);
// }
function showInfoExtend(obj) {
    $.blockUI({
        message: "<div style='padding:20px 0;'><img width='150' height='150' src=" + obj.icon + " /><p style='font-size:16px;'>" + obj.message + "</p></div>",
        css: { top: '30%', left: '10%', width: '80%' }
    });
    var sec = obj.sec || 2000;

    setTimeout(function () { $.unblockUI(); if (obj.callback) { obj.callback(); } }, sec);
}
// function addtocartS(message, img) {
//     onBlock = true;
//     $.blockUI({
//         message: "<div style='padding:20px 0;'><img width='150' height='150' src=" + img + " /><p style='font-size:16px;'>" + message + "</p></div>",
//         css: { top: '30%', left: '10%', width: '80%' }
//     });
//     // setTimeout($.unblockUI, 2000);
//     setTimeout(function () {
//         onBlock = false;
//         $.unblockUI();
//     }, 2000)
// }

function addtopuptocartS(message, img) {
    $.blockUI({
        message: "<div style='padding:20px 0;'><img width='150' height='150' src=" + img + " /><p style='font-size:16px;'>" + message + "</p></div>",
        css: { top: '30%', left: '10%', width: '80%' }
    });
    setTimeout($.unblockUI, 5000);
}

function addtoList(message, img) {
    if(platFlag == 'M') {
        $.blockUI({
            message: "<div style='padding:40px 0;'><img style='max-width: 150px;max-height: 150px;' src=" + img + " /><p style='font-size:16px;color: #707070;margin-top: 20px;'>" + message + "</p></div>",
            css: { width: '80%', top: '30%', left: '10%' }
        });
    } else {
        $.blockUI({
            message: "<div style='padding:50px 0;'><img style='max-width: 150px;max-height: 150px;' src=" + img + " /><p style='font-size:16px;color: #707070;margin-top: 20px;'>" + message + "</p></div>",
            css: { width: '26%', top: '30%', left: '37%' }
        });
    }

    setTimeout($.unblockUI, 2000);
}

function showWarn(message) {

    $.blockUI({
        message: message,
        css: { top: '20%' }
    });

    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
}
function showError(message) {
    $.blockUI({
        message: message,
        css: { top: '20%' }
    });

    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI);
}
function showBtConfirm(message, callback) {
    BootstrapDialog.show({
        title: 'System Message',
        type: BootstrapDialog.TYPE_WARNING,
        message: message,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                callback();
                dialogItself.close();
            }
        },
        {
            label: 'No',
            cssClass: 'btn-default',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}
function showWinUI(obj) {
    $.blockUI({
        message: $(obj.id),
        css: { top: '30%', left: '10%', width: '80%' }
    });
    var sec = obj.sec || 2000;

    var runed = false;

    setTimeout(function () { if (!runed) { $.unblockUI(); if (obj.callback) { obj.callback(); } } }, sec);

    if (obj.canClose === undefined || obj.canClose) {
        $('.blockOverlay').attr('title', 'Click to unblock').click(function () {
            runed = true;            
            $.unblockUI();          
            if (obj.callback) { obj.callback(); }
        });
    }
}
function createMessage() {

    $("body").append("<div class='modal fade' id='alertMsgDiv' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' data-backdrop='static'>" +
        "<div class='modal-dialog' role='document'>" +
        "<div class='modal-content'>" +
        "<div class='modal-header'>" +
        "<h4 class='modal-title' id='myModalLabel'>Tips</h4>" +
        "</div>" +
        "<div id='alertMessageSpan' class='modal-body'>" +
        "<span class='glyphicon glyphicon-refresh' aria-hidden='true'>1</span>" +
        "处理中，请稍候。。。" +
        "</div>" +
        "</div>" +
        "</div>" +
        "</div>"
    );
}

function closeAlert() {
    $("#alertMsgDiv").modal("hide");
}


function setCookie(name, value, path, expiredays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + expiredays);
    document.cookie = name + "=" + escape(value) +
        ((expiredays === null) ? "" : ";expires=" + exdate.toGMTString()) + ";path=" + path;
}
function getCookie(name, path) {
    return $.cookie(name);
}
String.format = function () {
    if (arguments.length === 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
};


//解决浏览器缓存 或 刷新URL地址
function addTimeStamp(url) {
    var getTimestamp = new Date().getTime(); // 获取时间戳
    if (url.indexOf("?") > -1) { // 地址已含 “ ? ”符号
        url = url + "&tid=" + getTimestamp;
    } else { //地址还没有 “ ? ”符号
        url = url + "?tid=" + getTimestamp;
    }
    return url;
}
function setUrlParam(url, para_name, para_value) {
    var oUrl = url;
    for (var i = 0; i < para_name.length; i++) {
        var re = eval('/(' + para_name[i] + '=)([^&]*)/gi');
        oUrl = oUrl.replace(re, para_name[i] + '=' + para_value[i]);
    }
    return oUrl;
}

function lengthInUtf8Bytes(str) {
    var m = encodeURIComponent(str).match(/%[89ABab]/g);
    return str.length + (m ? m.length : 0);
}

// footer根据语言跳转外链
function goExternalLink(key) {
    switch (key) {
        case 'PrivacyPolicy':
            if(getUILanguage() == 'E'){
                window.open("https://www.hongkongpost.hk/en/privacy/index.html");
            } else if(getUILanguage() == 'C'){
                window.open("https://www.hongkongpost.hk/tc/privacy/index.html");
            } else if(getUILanguage() == 'S'){
                window.open("https://www.hongkongpost.hk/cn/privacy/index.html");
            }
            break;
        case 'AboutHongkongPost':
            if(getUILanguage() == 'E'){
                window.open("https://www.hongkongpost.hk/en/about_us/index.html");
            } else if(getUILanguage() == 'C'){
                window.open("https://www.hongkongpost.hk/tc/about_us/index.html");
            } else if(getUILanguage() == 'S'){
                window.open("https://www.hongkongpost.hk/sc/about_us/index.html");
            }
            break;
        case 'MailTracking':
            if(getUILanguage() == 'E'){
                window.open("https://www.hongkongpost.hk/en/mail_tracking/index.html");
            } else if(getUILanguage() == 'C'){
                window.open("https://www.hongkongpost.hk/tc/mail_tracking/index.html");
            } else if(getUILanguage() == 'S'){
                window.open("https://www.hongkongpost.hk/sc/mail_tracking/index.html");
            }
            break;
        case 'MailTracking':
            if(getUILanguage() == 'E'){
                window.open("https://www.hongkongpost.hk/en/mail_tracking/index.html");
            } else if(getUILanguage() == 'C'){
                window.open("https://www.hongkongpost.hk/tc/mail_tracking/index.html");
            } else if(getUILanguage() == 'S'){
                window.open("https://www.hongkongpost.hk/sc/mail_tracking/index.html");
            }
            break;
        case 'PostOfficeGuide':
            if(getUILanguage() == 'E'){
                window.open("https://www.hongkongpost.hk/en/about_us/corp_info/publications/poguide/index.html");
            } else if(getUILanguage() == 'C'){
                window.open("https://www.hongkongpost.hk/tc/about_us/corp_info/publications/poguide/index.html");
            } else if(getUILanguage() == 'S'){
                window.open("https://www.hongkongpost.hk/sc/about_us/corp_info/publications/poguide/index.html");
            }
            break;
    }
}

function getChiByteLength(content) {
    var patt1 = new RegExp("[\u4e00-\u9fa5]", "g");
    var arr = content.match(patt1);
    if (arr) {
        return arr.length*4;
    }
    else {
        return 0;
    }
}

function getCharByteLength(content) {
    var patt1 = new RegExp("[^\u4e00-\u9fa5]", "g");
    var arr = content.match(patt1);
    if (arr) {
        return arr.length;
    }
    else {
        return 0;
    }
}

function showConfirm (message, callback) {
    onBlock = true;
    var btn;

    switch (getUILanguage()) {
        case 'E':
            btn = language.E; 
            break;
        case 'C':
            btn = language.C; 
            break;
        case 'S':
            btn = language.S; 
            break;
    }

    if(plat_flag == 'D') {
        $.blockUI({ 
            message: "<div style='padding: 35px 20px;'><div style='font-size: 18px;margin-bottom: 30px;'>" + message + "</div><input type='button' id='confirm' class='green_btn' value='" + btn.confirm + "' style='font-size: 15px;padding: 10px 25px;outline: none;border: 0;cursor: pointer;' /><input type='button' id='cancel' class='green_btn' value='" + btn.cancel + "' style='font-size: 15px;padding: 10px 25px;outline: none;border: 0;cursor: pointer; margin-left: 20px;' /></div>", 
            css: { top: '30%', left: '30%', width: '40%' },
            blockMsgClass: "blockDialog" 
        });
    } else {
        $.blockUI({ 
            message: "<div style='padding: 40px 20px;'><div style='font-size: 18px;margin-bottom: 30px;'>" + message + "</div><input type='button' id='confirm' class='green_btn' value='" + btn.confirm + "' style='font-size: 15px;padding: 10px 25px;outline: none;border: 0;cursor: pointer;' /><input type='button' id='cancel' class='green_btn' value='" + btn.cancel + "' style='font-size: 15px;padding: 10px 25px;outline: none;border: 0;cursor: pointer; margin-left: 20px;' /></div>", 
            css: { top: '30%', left: '10%', width: '80%' } 
        });
    }

    $('#confirm').click(function() {
        onBlock = true;
        $.unblockUI();
        // return true;

        if(typeof callback == 'function') {
            callback();
        }
    });

    $('#cancel').click(function() {
        onBlock = true;
        $.unblockUI();
    });
}

function showAlert (message, callback) {
    onBlock = true;
    var btn;

    switch (getUILanguage()) {
        case 'E':
            btn = language.E; 
            break;
        case 'C':
            btn = language.C; 
            break;
        case 'S':
            btn = language.S; 
            break;
    }

    if(plat_flag == 'D') {
        $.blockUI({ 
            message: "<div style='padding: 35px 20px;'><div style='font-size: 18px;margin-bottom: 30px;'>" + message + "</div><input type='button' id='confirm' class='green_btn' value='" + btn.confirm + "' style='font-size: 15px;padding: 10px 25px;outline: none;border: 0;cursor: pointer;' /></div>", 
            css: { top: '30%', left: '30%', width: '40%' },
            blockMsgClass: "blockDialog" 
        });
    } else {
        $.blockUI({ 
            message: "<div style='padding: 40px 20px;'><div style='font-size: 18px;margin-bottom: 30px;'>" + message + "</div><input type='button' id='confirm' class='green_btn' value='" + btn.confirm + "' style='font-size: 15px;padding: 10px 25px;outline: none;border: 0;' /></div>", 
            css: { top: '30%', left: '10%', width: '80%' } 
        });
    }

    $('#confirm').click(function() {
        onBlock = true;
        $.unblockUI();
        // return true;

        if(typeof callback == 'function') {
            callback();
        }
    });
}

function addtocartS(message, img) {
    onBlock = true;
    if(platform == 'D') {
        $.blockUI({ 
            message: "<div class='window'><img src='" + img + "' /><p>" + message + "</p><span id='close-btn'></span></div>", 
            blockMsgClass: "popBlock" 
        });
    } else {
        $.blockUI({ 
            message: "<div class='window'><img src='" + img + "' /><p>" + message + "</p><span id='close-btn'></span></div>", 
            blockMsgClass: "popBlock" 
        });
    }
    // setTimeout($.unblockUI, 2000);
    setTimeout(function () {
        onBlock = false;
        $.unblockUI();
    }, 3000);

    $('#close-btn').click(function() {
        onBlock = false;
        $.unblockUI();
    });
}

function showMessage(message) {
    onBlock = true;
    if(platform == 'D') {
        $.blockUI({ 
            message: "<div class='window'><p>" + message + "</p><span id='close-btn'></span></div>", 
            blockMsgClass: "popBlock" 
        });
    } else {
        $.blockUI({ 
            message: "<div class='window'><p>" + message + "</p><span id='close-btn'></span></div>", 
            blockMsgClass: "popBlock" 
        });
    }

    setTimeout(function () {
        onBlock = false;
        $.unblockUI();
    }, 2000);

    // $.blockUI({
    //     message: "<div style='padding:20px 0;'><p style='font-size:16px;'>" + message + "</p></div>",
    //     css: {}
    // });
    // setTimeout($.unblockUI, 2000);
}

function showLoginDialog (returnUrl, callback) {
    onBlock = true;
    var btn;

    // let message = defaultMsg.Pleaselogin;

    if(platform == 'D') {
        $.blockUI({ 
            message: "<div class='window login'><div><img src='/Images/login-icon.png' /></div><input type='button' id='login-btn' value='" + defaultMsg.Pleaselogin + "' /><span id='close-btn'></span></div>", 
            // css: { top: '30%', left: '30%', width: '40%' },
            blockMsgClass: "popBlock" 
        });
    } else {
        $.blockUI({ 
            message: "<div class='window login'><div><img src='/Images/login-icon.png' /></div><input type='button' id='login-btn' value='" + defaultMsg.Pleaselogin + "' /><span id='close-btn'></span></div>", 
            // css: { top: '30%', left: '10%', width: '80%' } 
            blockMsgClass: "popBlock" 
        });
    }

    $('#close-btn').click(function() {
        onBlock = false;
        $.unblockUI();
    });

    $('#login-btn').click(function() {
        if (returnUrl) {
            window.location.href = "/Account/Login?returnUrl=" + returnUrl;
        } else {
            window.location.href = "/Account/Login";
        }
    });
}

/*兼容处理 低版本IE Array.find()*/
if(!Array.prototype.find) {
    Array.prototype.find = function (callback) {
        return callback && (this.filter(callback) || [])[0];
    }
}