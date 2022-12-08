createApp({
  data() {
	  return {
	  	accountNo: '', // 擺檔賬號
	  	password: '', // 密碼
        targetUrl: targetUrl, // 登錄後跳轉Url
        errorPassNum: 0
	  }
	},
	methods: {
		// 登入
		signIn: function() {
			console.log('signIn');

			if (!validator.form()) {
                return;
            }

            let _this = this;
            let param = {
              account: this.accountNo,
              password: this.password
            };

			InstoreSdk.api.member.login(param, function (data) {
                if (data.Succeeded) {
                    localStorage.setItem("logined", "1");
                    if (localStorage.getItem("subscribeEmail")) {
                        window.location.href = "/Account/subscribe?email=" + localStorage.getItem("subscribeEmail");
                    }
                    else { 
                        if (!_this.targetUrl) {
                            _this.targetUrl = "/Account/Memberinfo";
                        }

                        window.location.href = _this.targetUrl;
                    }

                } else {
                    // _this.errorPassNum++; 
                    // $("#loginFailMsg").text(data.Message || 'Login Failed' );
                    // $("#loginFailMsg").show();
                    // $("input[name='password']").val("").focus();
                    // if (_this.errorPassNum == 5) {
                    //     window.location.href = '/account/forgetpassword';
                    // }

                    _this.errorPassNum++; 
                    if (_this.errorPassNum == 5) {
                        window.location.href = '/account/forgetpassword';
                    } else {
                        addtocartS(data.Message || 'Login Failed', '/imgs/warn-icon.png');
                    }
                }
            });
		},
        // 註冊
        register: function () {
            window.location.href = "/TranView/GoTo?returnUrl=" + BuyDong + "/account/register";
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