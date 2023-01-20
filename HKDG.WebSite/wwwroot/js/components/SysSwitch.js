ajaxPage( "BdDialog", "/js/components/BdDialog.js" );

var tempStr = '<div class="lang-switch" @click="openDialog">\
    <img src="/imgs/icons/language.png">\
    <span>{{langStr}}</span>\
</div>\
<bd-dialog class="lang-currency" type="sumbit" :confirmtext="comStr.Confirm" :showcancelbtn="false" ref="SysSwitch" @confirm="changeSetting">\
    <div class="set-one">\
        <p>{{comStr.LanguageS}}</p>\
        <ul>\
            <li v-for="(item,index) in langList" :class="{\'active\': item.value === lang}" @click="lang = item.value;">{{item.lang}}</li>\
        </ul>\
    </div>\
    <div class="set-one">\
        <p>{{comStr.CurrencyS}}</p>\
        <ul>\
            <li v-for="(item,index) in currencyList" :class="{\'active\': item.Code === currency}" @click="currency = item.Code;">{{item.Code}}</li>\
        </ul>\
    </div>\
    <div class="currency-tips">{{comStr.CurrencyTips}}</div>\
</bd-dialog>';

// 定义一个系統 語言/貨幣 切換组件
var SysSwitch = {
    components: {
        'bd-dialog': BdDialog
    },
    props: ['data'],
    data: function () {
        return {
            lang: getQueryString("lang") || lang, // 當前語言值
            langStr: '', // 當前語言文本值
            langList: [{    // 系統語言列表
                lang: '繁體',
                value: 'C'
            }, {
                lang: '简体',
                value: 'S'
            }, {
                lang: 'English',
                value: 'E'
            }],
            currency: getQueryString("cur") || currency, // 當前貨幣值
            currencyList: [   // 貨幣列表
                {Code: 'HKD', id: 1},          
                {Code: 'USD', id: 2},
                {Code: 'RMB', id: 3}     
            ],
            comStr: comStr
        }
    },
    template: tempStr,
    methods: {
        // 確認系統語言/貨幣設置
        confirm: async function () {
            Promise.all([this.changeLang(),this.changeCurrency()]).then(result => {
                this.$refs.SysSwitch.quitDialog();
                location.reload();
            }).catch(error => {
                console.log(error);
            });
        },
        openDialog: function () {
            this.$refs.SysSwitch.openDialog();
        },
        // 語言切換
        changeLang: async function() {
            await new Promise((resolve, reject) => {
                InstoreSdk.api.member.setUILanguage(this.lang, function(data) {
                    if (data.Succeeded) {
                        console.log('changeLang');
                        resolve();
                    } else {
                        reject();
                    }
                });
            });
        },
        // 貨幣切換
        changeCurrency: async function () {
            await new Promise((resolve, reject) => {
                InstoreSdk.api.member.setCurrency(this.currency, function (data) {
                    if (data.Succeeded) {
                        console.log('changeCurrency');
                        resolve();
                    } else {
                        reject(data.Message);
                    }
                });
            });
        },
        // 語言/貨幣切換
        changeSetting: function () {
            let _this = this;
            let param = {
                Lang: this.lang,
                CurrencyCode: this.currency
            };
            InstoreSdk.api.member.changeSetting(param, function (data) {
                if (data.Succeeded) {
                    _this.$refs.SysSwitch.quitDialog();
                    location.reload();
                } else {
                    addtocartS(data.Message, '/imgs/warn-icon.png');
                }
            });
        }
    },
    created() {
        let _this = this;
        this.langStr = this.langList.find(function(i) {
            return i.value === _this.lang;
        }).lang;
    }
}