function fbLogin() {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            checkPermissions(response.authResponse.accessToken);
        } else {
            callFBLogin();
        }
    });
    //FB.login(function (response) {
    //    checkLoginState();
    //}, { scope: 'publish_stream' });
}

function checkPermissions(accessToken) {
    FB.api('/me/permissions',
        { 'access_token': accessToken },
        function (response) {
            // Logged in and authorized for this site.
            if (response.data && response.data.length) {
                doLOGINAPI();
                // Logged in to FB but not authorized for this site.
            } else {
                callFBLogin();
            }
        }
    );
}

function callFBLogin() {
    FB.login(function () {
        doLOGINAPI();
    }, { scope: 'public_profile,email,publish_stream,user_birthday,user_location' });
}

function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}
function statusChangeCallback(response) {
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    if (response.status === 'connected') {
        // Logged into your app and Facebook.
        doLOGINAPI();
    } else {
        // The person is not logged into your app or we are unable to tell.
        //document.getElementById('status').innerHTML = 'Please log ' +
        //    'into this app.';
    }
}
//window.fbAsyncInit = function () {
//    // init the FB JS SDK
//    FB.init({
//        appId: appid,
//        cookie: true,
//        xfbml: true,
//        version: 'v2.9'                      // Look for social plugins on the page
//    });
//    //FB.getLoginStatus(function(response) {
//    //  statusChangeCallback(response);
//    // });

//};

window.fbAsyncInit = function () {
    FB.init({
        appId: 1088530114983854,
        cookie: true,
        xfbml: true,
        version: 'v7.0'
    });
    //3204344786353663 prod
    //312987433135672 uat
    //1088530114983854 transin prod
    //FB.AppEvents.logPageView();
};

// Load the SDK asynchronously

// Here we run a very simple test of the Graph API after login is
// successful.  See statusChangeCallback() for when this call is made.
function doLOGINAPI() {
    console.log('Welcome!  Fetching your information.... ');
    //FB.api('/me?fields=id,name,email,first_name,last_name', function (response) {
    FB.api('/me', { fields: 'id,name,email,first_name,last_name' }, function (response) {
        //console.log('Successful login for: ' + response.name);
        var model = {};
        model.UserName = response.email;
        model.Email = response.email;
        model.ExternalAccId = response.id;
        model.ExternalType = "1";
        InstoreSdk.api.member.fbLogin(model, function (data) {
            if (data.Succeeded) {

                
                localStorage.setItem('logined', '1');
                var returnUrl = getQueryVariable("returnUrl");
                goPAGE();
                if (returnUrl === null) {
                    document.location.href = "/";
                } else {
                    window.location.href = returnUrl;
                }
            }
            else {
                var member = {};
                member.Email = response.email;
                member.FirstName = response.first_name;
                member.LastName = response.last_name;
                member.Mobile = "88888888";
                member.ThirdPartyUserId = response.id;
                member.ThirdPartyType = "1";
                member.SyncTranSin = true;
                //alert(3333);
                InstoreSdk.api.member.register(member, function (data, status) {
                    if (data.Succeeded) {
                        var modelRep = {};
                        modelRep.Email = response.email;
                        modelRep.UserName = response.email;
                        modelRep.ExternalAccId = response.id;
                        modelRep.ExternalType = "1";
                        //alert(4444);
                        InstoreSdk.api.member.fbLogin(modelRep, function (data) {
                            if (data.Succeeded) {
                                localStorage.setItem('logined', '1');

                                var returnUrl = getQueryVariable("returnUrl");
                                
                                goPAGE();
                                if (returnUrl === null) {
                                    window.location.href = "/";
                                } else {
                                    window.location.href = returnUrl;
                                }
                            }
                        });
                    }
                });
            }
        });
        fblogout();
    });
}
function fblogout() {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            FB.logout();
        }
    });
}

function goPAGE() {
    if ((navigator.userAgent.match(/(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone)/i))) {
        alert("登入成功");
    }
}

