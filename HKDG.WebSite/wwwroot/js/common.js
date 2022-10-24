function getPMHost() {
    //var pmserver = $.cookie("PMServer");
    //if (pmserver) {
    //    return pmserver;
    //} else {
    //    return "";
    //}

    return "https://localhost:5003"
}

function getCookie(name, path) {
    return $.cookie(name);
}

function WSGet(url, data, success, error) {
    //var accessToken = $.cookie("B_ticket");

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
        beforeSend: function (request) {
             //request.setRequestHeader("Authorization", "Bearer " + accessToken);
            //var userLanguage = getUILanguage();
            //request.setRequestHeader("UserLanguage", userLanguage);
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
        beforeSend: function (request) {
            //  request.setRequestHeader("Authorization", "Bearer " + accessToken);
            //var userLanguage = getUILanguage();
            //request.setRequestHeader("UserLanguage", userLanguage);           
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