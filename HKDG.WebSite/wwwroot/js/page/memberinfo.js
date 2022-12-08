var testData = {"Succeeded":true,"Message":null,"ReturnValue":{"Password":null,"Gender":false,"Language":1,"Currency":{"Id":358,"Code":"HKD","Name":"港幣","ExchangeRate":1.0,"IsDefaultCurrency":true},"OptOut":false,"ComeFrom":0,"IsLogin":false,"SNO":null,"LastAccessTime":"0001-01-01 00:00:00","Linkuped":false,"HKPID":null,"PwdEnable":true,"ThirdPartyUserId":null,"ThirdPartyType":0,"Preference":[],"SyncTranSin":false,"IsTransin":false,"TransinLinkuped":false,"IsTempUser":false,"TransinAcctDetail":{"Email":null,"Name":null,"PhoneNum":null,"TPoint":0.0,"TMark":0.0,"BeforeFun":0.0,"RealFun":0.0,"CurrentDayTPoints":0.0,"CurrnetMonthTPoints":0.0,"CurrnetYearTPoints":0.0,"MaxLimitDayFun":0.0,"MaxLimitMonthFun":0.0,"MaxLimitYearFun":0.0,"ScaleValue":null},"Id":"4cf34da8-2386-49aa-b9ea-61c654382f1f","FirstName":"Vicky","LastName":"Zhu","Email":"vicky@ptx.hk","Phone":null,"Mobile":"13414932315"}};

createApp({
	components: {
		'nav-tab': NavTab,
		'bd-dialog': BdDialog
	},
	data() {
		return {
			memberInfo: {}, // 會員信息
            addressList: [], // 用戶郵寄地址列表
            editClAddr: {}, // 編輯郵寄地址信息
            countryList: [],    // 城市列表
            provinceList: []   // 省份列表
		}
	},
	methods: {
		// 获取登录后的会员数据
		getMemberInfo: function () {
			let _this = this;
			InstoreSdk.api.member.getMemberInfo(function (result) {
				if (result) {
					_this.memberInfo = result;
				} else {
					location.href = "/default/index";
				}
            });
		},
		// 更改密碼
		changePWD: function () {
			validator.resetForm();
			this.$refs.pwd.openDialog();
		},
        // 保存密碼
		savePwd: function () {
			let _this = this;
			if (!validator.form()) {
                return;
            }

            InstoreSdk.api.member.updatePassword(this.memberInfo.password, this.memberInfo.newPwd, function (data) {
                if(data.Succeeded){
                    showMessage(data.Message);
                    _this.$refs.pwd.quitDialog();
                }
                else{
                    showMessage(data.Message);
                }
            });
		},
		// 登出
		logout: function () {
			InstoreSdk.api.member.logout(function () {
               // window.location.reload(true);
                localStorage.removeItem("user");
                localStorage.removeItem("userId");
                localStorage.setItem("logined", "0");
                location.href = "/Default/Index";
            }); 
		},
        // 獲取用戶郵寄地址列表
        getAddress: function () {
            let _this = this;

            InstoreSdk.api.delivery.getAddress(function (result) {
                _this.addressList = result;
            });
        },
        // 新增/編輯地址
        editAddr: function (addr) {
            let _this = this;
            let cName;  // 處理收貨地址國家選中值（autocomplete）

            if (addr.Id) {
                this.editClAddr = JSON.parse(JSON.stringify(addr));

                cName = this.countryList.find(function(i) {
                    return i.Id === _this.editClAddr.CountryId;
                }).Name;
            } else {
                this.editClAddr.CountryId = this.countryList.length ? this.countryList[0].Id : '';
                cName = this.countryList.length ? this.countryList[0].Name : '';
            }

            $("#country").parent().find(".dest-combobox-input[name='country']").val(cName || "");

            this.getProvince(1);

            this.openDialog('.editAddr');
        },
        // 管理郵寄地址
        manageAddr: function () {
            this.openDialog('.manageAddr');
        },
        // 設置默認地址
        setDefaultAddr: function (addr) {
            this.editClAddr = JSON.parse(JSON.stringify(addr));
            this.editClAddr.Default = true;
            this.saveAddr();
        },
        // 保存郵寄地址
        saveAddr: function () {
            if (!addrValidator.form()) {
                return;
            }

            let _this = this;
            // let param = addr.Id ? addr : this.editClAddr;
            let param = {
                Id: this.editClAddr.Id || '',
                Default: this.editClAddr.Default,
                LastName: this.editClAddr.LastName,
                ContractAddress: this.editClAddr.Address,
                CountryId: this.editClAddr.CountryId,
                ProvinceId: this.editClAddr.ProvinceId,
                Mobile: this.editClAddr.Mobile
            };

            InstoreSdk.api.delivery.saveAddress(param, function (data) {
                if (data.Succeeded) {
                    addtocartS(data.Message, '/imgs/tick-icon.png');
                    _this.getAddress();        //成功后重新刷新地址列表
                    _this.quitDialog('.editAddr');
                } else {
                    addtocartS(data.Message, '/imgs/warn-icon.png');
                }

            });
        },
        // 刪除郵寄地址
        deleteAddr: function (addrId) {
            let _this = this;
            showConfirm(jsMessage.SureDeleteAddr, function() {
                InstoreSdk.api.delivery.removeAddress(addrId, function (result) {
                    if (result.Succeeded) {
                        addtocartS(result.Message, '/imgs/tick-icon.png');
                        _this.getAddress();
                    } else {
                        showInfo(result.Message);
                    }
                });
            });
        },
        // 獲取城市數據
        getCountry: function () {
            let _this = this;
            InstoreSdk.api.delivery.getCountry(function (result) {
                _this.countryList = result;
            })
        },
        // 獲取省份數據
        getProvince: function (flag) { // flag => 1, 打開彈窗獲取
            let _this = this;
            let param = this.editClAddr.CountryId;
            InstoreSdk.api.delivery.getProvince(param, function (result) {
                _this.provinceList = result;

                let pName;  // 處理收貨地址省份選中值（autocomplete）
                // 新增地址/更改國家 省份默認值處理
                if(!(_this.editClAddr.Id && flag === 1)) {
                    _this.editClAddr.ProvinceId = _this.provinceList.length ? _this.provinceList[0].Id : '';
                    pName = _this.provinceList.length ? _this.provinceList[0].Name : '';
                } else {
                    pName = _this.provinceList.find(function(i) {
                        return i.Id === _this.editClAddr.ProvinceId;
                    }).Name;
                }

                $("#country").parent().find(".dest-combobox-input[name='province']").val(pName || "");
            });
        },
        // 打開彈窗
        openDialog: function (domClass) {
            if (platform === 'M' && $(domClass).hasClass('inside')) {
                let topHeight = $('#header').height() + 1;
                let bottomHeight = $('.header-bootom').height() + 1;

                $(domClass).css({ 
                    'top': topHeight + 'px',
                    'bottom': bottomHeight + 'px',
                    'z-index': '10'
                });
            }

            $('body').css({ 'overflow': 'hidden' });

            $(domClass).fadeIn();
        },
        // 退出彈窗
        quitDialog: function (domClass) {
            $('body').css({ 'overflow': 'auto' });

            if (typeof domClass === 'string') {
                $(domClass).fadeOut();
            } else {
                $(event.target).closest('.mDialog').fadeOut();
            }

            // 重置地址表單信息
            this.editClAddr = {};
            addrValidator.resetForm();
        },
        // 初始化地址search drop list
        destComInit: function () {
            let _this = this;
            $("#country").combobox({
                select: function(event, ui) {
                    _this.editClAddr.CountryId = Number(this.value);
                    _this.getProvince();
                }
            });

            $("#province").combobox({
                select: function(event, ui) {
                    _this.editClAddr.ProvinceId = Number(this.value);
                }
            });
        }
	},
	created() {
		this.getMemberInfo();
        this.getAddress();
        this.getCountry();
	},
	mounted() {
        this.destComInit();
	}
}).mount('#container')

var validator = $("#password").validate({
    debug: true,
    rules: {
        password: {
            required: true,
        },
        newPwd: {
            required: true,
            minlength: 8,
            passcheck: true
        },
        confirmPwd: {
            equalTo: "#newPwd"
        }
    },
    messages: {
        password: {
            required: 'jsMessage.EnterPWD',
        },
        newPwd: {
            required: 'jsMessage.EnterPWD',
            minlength: 'jsMessage.PasswordRule',
            passcheck: 'jsMessage.EnterPWD'
        },
        confirmPwd: 'jsMessage.ComfirmPWD'
    },
    errorElement: "p",
    errorPlacement: function (error, element) {
        error.appendTo(element.parents('.form-group'));
    },
    errorClass: "errorTip",
});

jQuery.validator.addMethod("compareToNameLength", function (value, element, param) {
    var valLength = lengthInUtf8Bytes(value);
    if (valLength > param) {
        return false;
    }
    else {
        return true;
    }
}, jsMessage.NameCharacters);

jQuery.validator.addMethod("compareToAddressLength", function (value, element, param) {
    var valLength = lengthInUtf8Bytes(value);
    if (valLength > param) {
        return false;
    }
    else {
        return true;
    }
}, jsMessage.AddressCharacters);

var addrValidator = $("#addrForm").validate({
    debug: true,
    rules: {
        country: {
            required: true
        },
        province: {
            required: true
        },
        address: {
            required: true,
            compareToAddressLength: 300
        },
        name: {
            required: true,
            compareToNameLength: 17,
            namecheck: true
        },
        mobile: {
            required: true,
            numcheck: true
        }
    },
    groups: {
        locationGroup: "country province address"
    },
    messages: {
        country: {
            required: jsMessage.IsRequired
        },
        province: {
            required: jsMessage.IsRequired
        },
        address: {
            required: jsMessage.IsRequired
        },
        name: {
            required: jsMessage.IsRequired,
            namecheck: jsMessage.EnterCorrectName
        },
        mobile: {
            required: jsMessage.IsRequired,
            numcheck: jsMessage.EnterCorrectPhoneNumber
        }
    },
    errorElement: "p",
    errorPlacement: function (error, element) {
        error.appendTo(element.parents('.form_group'));
    },
    showErrors: function (errorMap, errorList) {
        if (errorList && errorList.length > 0) {
            errorList[0].element.focus();
        }
        this.defaultShowErrors();
    },
    errorClass: "errorTip"
});