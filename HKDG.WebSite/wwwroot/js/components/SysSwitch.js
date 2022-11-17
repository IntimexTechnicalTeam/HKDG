var tempStr = '<div class="lang-switch" @click="openDialog">\
    <img src="/imgs/icons/language.png">\
    <span>繁體中文</span>\
</div>\
<div class="bd-dialog lang-currency">\
    <div class="dlg-mask"></div>\
    <div class="dlg-content-box">\
        <div class="dlg-body">\
            <div class="set-one">\
                <p>語言 / Language</p>\
                <ul>\
                    <li v-for="(item,index) in langList" :class="{\'active\': !index}" @click="changeLang(item.value)">{{item.lang}}</li>\
                </ul>\
            </div>\
            <div class="set-one">\
                <p>貨幣 / Currency</p>\
                <ul>\
                    <li v-for="(item,index) in currencyList" :class="{\'active\': !index}">{{item.Code}}</li>\
                </ul>\
            </div>\
            <div class="currency-tips">貨幣匯率會因應市場情況變動而不定期更新</div>\
        </div>\
        <div class="dlg-footer">\
            <button @click="confirm">確認</button>\
        </div>\
    </div>\
</div>';

// 定义一个系統 語言/貨幣 切換组件
var SysSwitch = {
    props: ['data'],
    data: function () {
        return {
            // lang: getQueryString("lang") || getUILanguage(), // 當前語言值
            lang: '',
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
            currencyList: [   // 貨幣列表
                {Code: 'HKD', id: 1},          
                {Code: 'USD', id: 2},
                {Code: 'RMB', id: 3}     
            ]
        }
    },
    template: tempStr,
    methods: {
        // 確認系統語言/貨幣設置
        confirm: function () {
            this.quitDialog();
        },
        openDialog: function () {
            $('body').css({ 'overflow': 'hidden' });
            $('.bd-dialog').fadeIn();
        },
        quitDialog: function () {
            $('body').css({ 'overflow': 'auto' });
            $('.bd-dialog').fadeOut();
        },
        // 語言切換
        changeLang: function(e) {
            InstoreSdk.api.member.setUILanguage(e, function(data) {
                if (data.Succeeded) {

                    var href = window.location.href;
                    if (href.indexOf("?") === -1) {
                        if (href.indexOf("?lang=") !== -1 || href.indexOf("&lang=") !== -1) {
                            href = setUrlParam(href, ["lang"], [e]);
                        } else {
                            href += "?lang=" + e;
                        }

                    } else {
                        if (href.indexOf("?lang=") !== -1 || href.indexOf("&lang=") !== -1) {
                            href = setUrlParam(href, ["lang"], [e]);
                        } else {
                            href += "&lang=" + e;
                        }
                    }
                    window.location.href = href;
                } else {
                    console.log(data);
                }
            });
        },
    }
}