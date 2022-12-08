var popW = 600;
var popH = 450;
var left = (screen.width / 2) - (popW / 2);
var TOP = (screen.height / 2) - (popH / 2);

// share to facebook
function shareToFaceBook() {
    window.open('http://www.facebook.com/share.php?u=' + window.location.href + '&t=' + document.title, '', 'width=' + popW + ', height=' + popH + ', top=' + TOP + ', left=' + left);
}

// share to Twitter
function shareToTwitter() {
    window.open('https://twitter.com/share?url=' + window.location.href, '', 'width=' + popW + ', height=' + popH + ', top=' + TOP + ', left=' + left);
}

// share to weibo
function shareToWeibo() {
    window.open('http://v.t.sina.com.cn/share/share.php?title=' + document.title + '&url=' + document.URL, '', 'width=' + popW + ', height=' + popH + ', top=' + TOP + ', left=' + left);
}

// share to googleplus
function shareToGooglePlus() {
    window.open('https://plusone.google.com/share?url=' + document.URL, '', 'width=' + popW + ', height=' + popH + ', top=' + TOP + ', left=' + left);
}

// share to whatsapp
function shareTowhatsapp(CustomUrl) {
    console.log(CustomUrl,'1111')
     if (CustomUrl) {
    window.open('https://api.whatsapp.com/send?text=' + document.title + '    ' + CustomUrl , '', 'width=' + popW + ', height=' + popH + ', top=' + TOP + ', left=' + left);
     } else {
    window.open('https://api.whatsapp.com/send?text=' + document.title + '    ' + window.location.href, '', 'width=' + popW + ', height=' + popH + ', top=' + TOP + ', left=' + left);

     }
}

function sharewhatpdoduct() {
    window.open('https://api.whatsapp.com/send?text=' + document.title + '    ' + window.location.href, '', 'width=' + popW + ', height=' + popH + ', top=' + TOP + ', left=' + left);
}

// share to linkedin
function shareToLinkedln() {
    window.open('https://www.linkedin.com/shareArticle?mini=true&url=' + document.URL + '&title=' + document.title, '', 'width=' + popW + ', height=' + popH + ', top=' + TOP + ', left=' + left);
}

// share to email
function shareEmail(){
	var subject = "%E7%B6%B2%E9%A0%81%E5%88%86%E4%BA%AB%20-%20%E9%A6%99%E6%B8%AF%E9%83%B5%E6%94%BF%20";
	var href = "mailto:?subject=" + subject + "&body=" + window.location.href + '&t=' + document.title;
	return href;
}

// share to copyUrl
function copyUrl(msg) {
    var url = window.location.href;
    $("body").after("<input id='copyVal'></input>");
    var input = document.getElementById("copyVal");
    input.value = url;
    input.select();
    input.setSelectionRange(0, input.value.length);   
    document.execCommand("copy");
    $("#copyVal").remove();
    showMessage(msg || 'Copied Success');
}
