createApp({
  data() {
	  return {
	  	accountNo: '', // 擺檔賬號
	  	password: '' // 密碼
	  }
	},
	methods: {
		// 登入
		signIn: function() {
			console.log('signIn');

			if (!validator.form()) {
                return;
            }

            InstoreSdk.api.member.setUILanguage('C', function(data) {
                // if (data.Succeeded) {

                //     var href = window.location.href;
                //     if (href.indexOf("?") === -1) {
                //         if (href.indexOf("?lang=") !== -1 || href.indexOf("&lang=") !== -1) {
                //             href = setUrlParam(href, ["lang"], [e]);
                //         } else {
                //             href += "?lang=" + e;
                //         }

                //     } else {
                //         if (href.indexOf("?lang=") !== -1 || href.indexOf("&lang=") !== -1) {
                //             href = setUrlParam(href, ["lang"], [e]);
                //         } else {
                //             href += "&lang=" + e;
                //         }
                //     }
                //     window.location.href = href;
                // } else {
                //     console.log(data);
                // }
            });

            let param = {
              account: this.accountNo,
              password: this.password
            };

			InstoreSdk.api.member.login(param, function (data) {
				console.log(data,'signIn');
                // if (data.Succeeded) {
                //     if (localStorage.getItem("subscribeEmail")) {
                //         window.location.href = "/Account/subscribe?email=" + localStorage.getItem("subscribeEmail");
                //     }
                //     else { 
                //         if (app.targetUrl == "") {
                //             app.targetUrl = "/";
                //         }

                //         window.location.href = app.targetUrl;
                //     }

                // } else {
                //     _this.errorPassNum++; 
                //     $("#loginFailMsg").text(data.Message || 'Login Failed' );
                //     $("#loginFailMsg").show();
                //     $("input[name='passWord']").val("").focus();
                //     if (_this.errorPassNum == 5) {
                //         window.location.href = '/account/forgetpassword';
                //     }
                // }
            });
		}
	},
	created() {
	},
	mounted() {

	}
}).mount('#container')

var validator = $("#login-form").validate({
    debug: true,
    rules: {
        accountNo: {
            required: true,
            email: true
        },
        password: {
            required: true
        }
    },
    messages: {
        accountNo: {
            required: jsMessage.EnterEmail,
            email: jsMessage.EnterCorrectEmail
        },
        password: jsMessage.EnterPWD
    },
    errorElement: "p",
    errorPlacement: function (error, element) {
        error.appendTo(element.parents('.form-group'));
    },
    errorClass: "errorTip",
});