var testImgs = ["https://img1.baidu.com/it/u=4076402559,3294297196&fm=253&fmt=auto&app=138&f=PNG?w=500&h=500","https://img1.baidu.com/it/u=3133082447,1921267555&fm=253&fmt=auto&app=138&f=JPEG?w=504&h=500","https://img2.baidu.com/it/u=2286725608,1362072907&fm=253&fmt=auto&app=138&f=JPEG?w=518&h=500","https://img2.baidu.com/it/u=3363028075,1655932363&fm=253&fmt=auto&app=120&f=JPEG?w=800&h=800","https://img0.baidu.com/it/u=2323958649,17793350&fm=253&fmt=auto&app=138&f=JPEG?w=500&h=485","https://img2.baidu.com/it/u=646701517,2138740410&fm=253&fmt=auto&app=120&f=JPEG?w=806&h=800"];

const app = createApp({
    components: {
        'star-rating': StarRating,
        'pmt-swiper': PmtSwiper,
        'pro-swiper': ProSwiper,
        'bd-dialog': BdDialog
    },
    data() {
  		return {
            areaCode: '', // 國家區域
            proData: ProudctDetail, // 产品數據
            merchantData: MerchantDetail, // 商家數據
            pmtRelateProd: null, // 相關產品推廣數據
            relateProd: RelateProd, // 相關產品數據
            banner: [{
              src: 'https://img0.baidu.com/it/u=2564065540,1877968420&fm=253&fmt=auto&app=138&f=JPEG?w=500&h=313'
            }, {
              src: 'https://img1.baidu.com/it/u=1740954870,1509955191&fm=253&fmt=auto&app=138&f=JPEG?w=800&h=500'
            }, {
              src: 'https://img2.baidu.com/it/u=72117843,339688063&fm=253&fmt=auto&app=138&f=JPEG?w=751&h=500'
            }],
            videoUrl: '', // 產品視頻地址
            buyQty: 1,
            proAttr: {}, // 選擇的產品屬性數據
            attrPrice: 0, // 選擇的產品屬性附加價格
            attrPrice2: 0, // 非默認貨幣擇的產品屬性附加價格
            termsDtls: '', // 當前查閱條款內容
            isOnSale: true,
            isSaleOut: false,
            isNotified: false,
            isSelling: false,
            btnStatus: 0, // 購物車按鈕狀態 (返貨電郵通知 -> 1, 取消返貨電郵通知 -> 2, 已售罄 -> 3, 更新中 -> 4, 即將發售 -> 5, 可購買 -> 6)
            tabType: 0, // 產品Tab類型 (商品詳情 -> 0, 顧客評價 -> 1)
            comments: [], // 產品評論
            selloutEmail: '', // 返貨電郵通知
            pager: { // 分頁數據
              page: 1,
              pageSize: 10
            },
            logined: localStorage.getItem("logined") === '1' ? true : false,
            attrKey: '', // 匹配選中的產品屬性圖片的key值
            prodAttrImg: {}, // 匹配選中的產品屬性圖片數據
            isViewAttrImg: false, // 預覽圖片是否為產品屬性圖片
            preSwiper: null, // 產品圖片預覽輪播
            isPlayVideo: false, // 是否正在播放視頻
            viewer: null, // 產品附加圖片查看器
            curPicIndex: 0
  		}
	},
	methods: {
        // 初始化數據的相關處理
        initHandle: function () {
          let _this = this;
          this.areaCode = localStorage.getItem('AreaCode') || '';
          this.videoUrl = this.areaCode === 'CN' ? this.proData.YoukuUrl : this.proData.YoutubeUrl;
          // this.videoUrl = 'https://player.youku.com/embed/XNTg3NzczODQ5Mg==';
          this.buyQty = this.proData.MinPurQty;
          if (platform == 'M') {
            this.$nextTick(this.formvideo);
          }

          if (this.proData.AttrList.length) {
            this.btnStatus = 6;
          } else {
            this.$nextTick(this.checkSaleOut);
          }
        },
        // 初始化產品圖片預覽
        initPreview: function () {
          if (IsMobile) {
            this.preSwiper = new Swiper(".proPreview", {
              navigation: {
                nextEl: '.swiper-button-next',
                prevEl: '.swiper-button-prev',
              },
              pagination: {
                el: '.swiper-pagination',
                type: 'fraction'
              }
            });
          } else {
            var swiper = new Swiper(".proThumbs", {
              spaceBetween: 10,
              slidesPerView: 5,
              freeMode: true,
              watchSlidesProgress: true,
            });
            var swiper2 = new Swiper(".proPreview", {
              spaceBetween: 10,
              navigation: {
                nextEl: ".swiper-button-next",
                prevEl: ".swiper-button-prev",
              },
              thumbs: {
                swiper: swiper,
              },
            });
          }

        },
        // 選擇產品屬性
        selectAttr: function (attr, val) {
          attr.selected = val.Id;
          this.proAttr['attr' + attr.Seq] = val;

          this.matchAttrImg(); // 匹配對應屬性產品圖片
          this.calAttrPrice(); // 計算屬性附加價格

          console.log(this.proAttr,'選擇的產品屬性');

          this.checkSaleOut();
        },
        // 計算屬性附加價格
        calAttrPrice: function () {
          let _this = this;
          this.attrPrice = 0;
          Object.values(this.proAttr).forEach((item) => {
            _this.attrPrice += item.AddPrice;
            _this.attrPrice2 += item.AddPrice2;
          });
        },
        // 修改/更新產品数量  type 類型 (輸入編輯 -> 0, 增加 -> 1, 減少 -> 2)
        updateQty: function (type) {
          if (typeof type !== 'number') type = 0;
          if (!type) {
            if (this.buyQty < 1) {
              this.buyQty = 1;
            }
          } else if (type === 1) {
            this.buyQty++;
          } else if (type === 2) {
            this.buyQty--;
          }

          // 起購處理
          if (this.proData.MinPurQty && this.buyQty < this.proData.MinPurQty) {
            this.buyQty = this.proData.MinPurQty;
          }
          // 限購處理
          if (this.proData.SaleQuota && this.buyQty > this.proData.SaleQuota) {
            // addtocartS(jsMessage.GreaterThanSalesQuota, '/Images/warn-icon.png');
            this.buyQty = this.proData.SaleQuota;
            return;
          }
        },
        // 添加/取消收藏
        addToFavorite: function(p, flag) { // flag: 1 => 收藏商家其他產品 ，2 => 收藏喜歡推薦產品
          // Auth驗證
          if (!this.logined) {
            location.href = "/Account/Login?returnUrl=" + location.pathname + "?fav=1";

            return;
          }


          var _this = this;
          var proId = flag ? p.ProductId : p.Id;
          if (p.IsFavorite) {
              InstoreSdk.api.member.removeFavorite(proId,
                  function(result) {
                      p.IsFavorite = false;
                      // showMessage(result.Message);
                      addtocartS(result.Message, '/imgs/icons/heart.png');
                      if (flag) {
                          _this.syncStatus(proId, flag, false);
                      }
                  },
                  function(result) {

                  });
          } else {
              InstoreSdk.api.member.addFavorite(proId,
                  function(result) {
                      p.IsFavorite = true;
                      // showMessage(result.Message);
                      addtocartS(result.Message, '/imgs/icons/heart2.png');
                      if (typeof(fbpAddToWishlist) === "function") {
                          _this.$nextTick(fbpAddToWishlist);
                      }
                      if (flag) {
                          _this.syncStatus(proId, flag, true);
                      }
                  },
                  function(data) {
                      if (data.Code == 1000) {
                          _this.addFavorite = 1;

                          if (data.Message === "LINKUP") {
                              app.linkup = true;
                              showWinUI({
                                  id: "#linkupDiv",
                                  sec: 120000,
                                  callback: function() {}
                              });
                          } else {
                              app.linkup = false;
                              var content = _this.selected.Attr1 + '/' + _this.selected.Attr2 + '/' + _this.selected.Attr3 + '/' + _this.selected.Qty + '/' + _this.colorSelected + '/' + _this.addFavorite;
                              window.location.href = "/Account/Login?returnUrl=" + window.location.href + '&sharp=' + content + '#' + content;
                          }

                      }
                  });
          }
        },
        // 加入購物車
        addToCart: function (flag) { // flag (1 => 立即購買)
          let _this = this;
          if (this.proData.AttrList.length !== Object.keys(this.proAttr).length) {
            addtocartS(jsMessage.PlsSelectAttr, '/imgs/warn-icon.png');
            return;
          }

          if (typeof(fbpAddToCart) === "function") {
            _this.$nextTick(fbpAddToCart);
          }

          // Auth驗證
          if (!this.logined) {
            let query = {};
            let keys = Object.keys(this.proAttr);
            let last = keys[keys.length-1];

            for(var key in this.proAttr) {
              // query += key + '=' + this.proAttr[key].Id + (key === last ? '' : '&');
              query[key] = this.proAttr[key].Id;
            }


            location.href = "/Account/Login?returnUrl=" + location.pathname + "?attr=" + JSON.stringify(query) + "&qty=" + this.buyQty + (flag === 1 ? "&buy=1" : "");

            return;
          }


          let param = {
            ProductId: this.proData.Id,
            ProdCode: this.proData.Code,
            Attr1: this.proAttr.attr1 ? this.proAttr.attr1.Id : '00000000-0000-0000-0000-000000000000',
            Attr2: this.proAttr.attr2 ? this.proAttr.attr2.Id : '00000000-0000-0000-0000-000000000000',
            Attr3: this.proAttr.attr3 ? this.proAttr.attr3.Id : '00000000-0000-0000-0000-000000000000',
            qty: this.buyQty,
            KolId: getQueryString('KolId') || ''
          };

          console.log(param, 'param');
          // console.log(this.proData.AttrList, 'proData.AttrList');
          // console.log(JSON.stringify(this.proAttr), 'JSON.stringify');
          // return;

          InstoreSdk.api.shoppingCart.addItem(param, function(data) {
            if (data.Succeeded) {
                console.log(data,'加入購物車');
                if (flag === 1) {
                  transitBD('/account/ShoppingCart');
                } else {
                  addtocartS(data.Message, '/imgs/icons/add-cart.png');
                  loadItems();
                }
            } else {
              // showLoginDialog(window.location.href);
              addtocartS(data.Message, '/imgs/warn-icon.png');
            }
          });
        },
        // 判断加入购物车按钮状态显示
        checkBtnStatus: function () {
            console.log(this.btnStatus, '判断前状态值');
            if (this.isOnSale && this.isSaleOut == 'unlimit' && !this.isNotified && this.isSelling) {
                this.btnStatus = 1;
            } else if (this.isOnSale && this.isSaleOut == 'unlimit' && this.isNotified && this.isSelling) {
                this.btnStatus = 2;
            } else if (this.isOnSale && this.isSaleOut == 'limit' && this.isSelling) {
                this.btnStatus = 3;
            } else if (!this.isOnSale) {
                this.btnStatus = 4;
            } else if (this.isOnSale && !this.isSelling) {
                this.btnStatus = 5;
            } else if (this.isOnSale && !this.isSaleOut && this.isSelling) {
                this.btnStatus = 6;
            }
            console.log(this.btnStatus, '判断后状态值');
        },
        //检查是否有库存
        checkSaleOut: function() {
            var _this = this;
            var isSearch = getQueryString("isSearch");
            if (!isSearch) {
                isSearch = 0;
            }
            var productCode = _this.proData.Code;
            var saleTime = _this.proData.SaleTime;
            // var code = getCookie(productCode, ''); //每个产品每个浏览器一天只能记录一次
            // if (!code) {
            //   InstoreSdk.api.product.countClick(productCode, isSearch, function(data) {
            //       var nowdate = new Date().getTime();
            //       setCookie(productCode, nowdate, '', 1);
            //   });
            // }

            var d = "";
            if (saleTime) {
                var date = new Date(parseInt(saleTime.replace(/\/Date\((-?\d+)\)\//, '$1')));
                d = date.getFullYear() + "-" + parseInt(date.getMonth() + 1) + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
            } else {
                d = null;
            }

            let attr1 = this.proAttr.attr1 ? this.proAttr.attr1.Id : '00000000-0000-0000-0000-000000000000';
            let attr2 = this.proAttr.attr2 ? this.proAttr.attr2.Id : '00000000-0000-0000-0000-000000000000';
            let attr3 = this.proAttr.attr3 ? this.proAttr.attr3.Id : '00000000-0000-0000-0000-000000000000';

            InstoreSdk.api.product.check(productCode, attr1, attr2, attr3, d, function(data) {
                if (data.Succeeded == true) {
                    if (!data.ReturnValue.IsOnSale) {
                        _this.isOnSale = false;
                        _this.isNone = false;
                    }

                    if (data.ReturnValue.IsOnSale) {
                        if (data.ReturnValue.IsSelling) {
                            _this.isSelling = true;
                        } else {
                            _this.isSelling = false;
                        }

                        if (data.ReturnValue.IsSaleOut) {
                            if (_this.proData.IsLimit) {
                                _this.isSaleOut = 'limit';
                            } else {
                                _this.isSaleOut = 'unlimit';
                            }
                        } else {
                            _this.isSaleOut = false;
                        }

                    }

                } else {
                    _this.isOnSale = false;
                    _this.isNone = false;
                }


                if (_this.logined) {
                  _this.checkExsitNotifyMe();
                } else {
                  _this.checkBtnStatus();
                }  
            });
        },
        // 獲取該產品評論數據
        getComments: function () {
            let _this = this;
            let param = {
                ProductId: this.proData.Id,
                Page: this.pager.page,
                PageSize: this.pager.pageSize
            };

            InstoreSdk.api.prodcomment.getProductComments(param, function (result) {
                _this.comments = _this.comments.concat(result);
            });
        },
        // 更多評論
        loadComments: function () {
            this.pager.page++;
            this.getComments();
        },
        // 点击图片放大浏览
        ImgViewer: function (index) {
            var ImgId = 'galley' + index;
            var galley = document.getElementById(ImgId);
            let toolbar;
            if (platform == 'D') {
                toolbar = {
                    // oneToOne: true,
                    prev: function () {
                        viewer.prev(true);
                    },
                    // play: true,
                    next: function () {
                        viewer.next(true);
                    },
                    rotateLeft: 4,
                    rotateRight: 4,
                };
            } else {
                toolbar = false;
            }
            var viewer = new Viewer(galley, {
                url: 'data-original',
                title: false,
                transition: false,
                toolbar: toolbar,
                loop: false
                // movable: false
            });
        },
        // 点击图片放大浏览
        ImgViewer2: function (index) {
          if (this.isViewAttrImg) {
            this.curPicIndex = 0;
          } else {
            this.curPicIndex = index;
          } 

          if (this.viewer) this.viewer.destroy();

          let _this = this;
          setTimeout(() => {
              _this.initViewer();
          }, 500);
        },
        // 初始化放大鏡圖片查看器
        initViewer: function () { // 0 => 附加圖片查看器, 1 => 屬性圖片查看器
            var galley = document.querySelector('.glass-viewer');
            this.viewer = new Viewer(galley, {
                url: 'relImg',
                title: false,
                transition: true,
                toolbar: true,
                loop: false
                // movable: false
            });

            this.viewer.view(this.curPicIndex);
        },
        // 閱讀相關條款 type 類型 (退貨及退款條款 -> 1, 運送及運費條款 -> 2)
        readTerms: function (type) {
          this.termsDtls = type === 1 ? this.proData.MerchantInfo.MerchantTerms : this.proData.MerchantInfo.ReturnTerms;
          this.$refs.terms.openDialog();
        },
        // 關注商店
        addFavMerchant: function () {
            let _this = this;
            if (this.merchantData.IsFavorite) {
                InstoreSdk.api.member.removeFavMerchant(this.merchantData.Code, function (result) {
                    _this.merchantData.IsFavorite = false;
                    addtocartS(result.Message, '/imgs/icons/heart.png')
                });
            } else {
                InstoreSdk.api.member.addFavMerchant(this.merchantData.Code,
                    function (result) {
                        _this.merchantData.IsFavorite = true;
                        addtocartS(result.Message, '/imgs/icons/heart2.png')
                    },
                    function () {
                       showLoginDialog(window.location.href);
                    });
            }
        },
        // 進入商店
        enterStore: function () {
          location.href="/Merchant/Detail/" + this.merchantData.MerchantId;
        },
        //檢查是否已存在返貨郵件通知記錄
        checkExsitNotifyMe: function () {
            var _this = this;

            var notify_obj = {
                ProductCode: this.proData.Code,
                ProductId: this.proData.Id,
                AttrVal1: this.proAttr.attr1 ? this.proAttr.attr1.Id : '00000000-0000-0000-0000-000000000000',
                AttrVal2: this.proAttr.attr2 ? this.proAttr.attr2.Id : '00000000-0000-0000-0000-000000000000',
                AttrVal3: this.proAttr.attr3 ? this.proAttr.attr3.Id : '00000000-0000-0000-0000-000000000000'
            };
            InstoreSdk.api.message.checkExsitArrivalNotify(notify_obj, function (result) {
                if (result) {
                    _this.isNotified = result.Succeeded;
                    if (!result.Succeeded) {
                        if (result.Message) {
                            showMessage(result.Message);
                        }
                    }
                } else {
                    showMessage("Please try again later.");
                }
                _this.checkBtnStatus();
            });
        },
        //新增返貨郵件通知記錄
        addNotifyMe: function () {
            var reg = /^([a-zA-Z0-9_-]|\.)+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
            if (!this.selloutEmail.match(reg)) {
                addtocartS(jsMessage.EnterEmailForSubscribe, '/imgs/warn-icon.png');
                return;
            }

            var notify_obj = {
                ProductCode: this.proData.Code,
                ProductId: this.proData.Id,
                AttrVal1: this.proAttr.attr1 ? this.proAttr.attr1.Id : '00000000-0000-0000-0000-000000000000',
                AttrVal2: this.proAttr.attr2 ? this.proAttr.attr2.Id : '00000000-0000-0000-0000-000000000000',
                AttrVal3: this.proAttr.attr3 ? this.proAttr.attr3.Id : '00000000-0000-0000-0000-000000000000',
                Email: this.selloutEmail
            };
            InstoreSdk.api.message.addArrivalNotify(notify_obj, function (result) {
                if (result) {
                    if (!result.Succeeded) {
                        showMessage(result.Message);
                    } else {
                        // 判断是否登录，没登录按鈕不轉做取消返貨電郵通知
                        if (localStorage.getItem("logined") == 0) {
                            _this.isNotified = false;
                        } else {
                            _this.isNotified = true;
                        }
                        showMessage(result.Message);
                        $('.sellout-window').fadeOut();
                        $('.detail-login-box').hide();
                        $('body').css({ 'position': 'static', 'width': 'auto', 'overflowY': 'auto' });
                        app.checkBtnStatus();
                    }
                } else {
                    showMessage("Please try again later.");
                }
            })
        },
        //取消返貨郵件通知記錄
        cancelNotifyMe: function () {
            var _this = this;

            var notify_obj = {
                ProductCode: this.proData.Code,
                ProductId: this.proData.Id,
                AttrVal1: this.proAttr.attr1 ? this.proAttr.attr1.Id : '00000000-0000-0000-0000-000000000000',
                AttrVal2: this.proAttr.attr2 ? this.proAttr.attr2.Id : '00000000-0000-0000-0000-000000000000',
                AttrVal3: this.proAttr.attr3 ? this.proAttr.attr3.Id : '00000000-0000-0000-0000-000000000000'
            };
            InstoreSdk.api.message.cancelArrivalNotify(notify_obj, function (result) {
                if (result) {
                    if (!result.Succeeded) {
                        if (result.Message) {
                            showMessage(result.Message);
                        }
                    } else {
                        _this.isNotified = false;
                        showMessage(result.Message);
                        $('.sellout-window').fadeOut();
                        $('.detail-login-box').hide();
                        $('body').css({ 'position': 'static', 'width': 'auto', 'overflowY': 'auto' });
                        app.checkBtnStatus();
                    }
                } else {
                    showMessage("Please try again later.");
                }
            })
        },
        // 匹配對應屬性產品圖片
        matchAttrImg: function () {
            this.matchAttrKey();
            let _this = this;
            if (this.AttrKey) {
                this.proData.ProdAttrImgs.forEach((item) => {
                    if (item.AttrKey === _this.AttrKey) {
                        _this.prodAttrImg = item;
                        _this.isViewAttrImg = true;
                    }
                });
            }
        },
        // 構建md5加密後的attrKey
        matchAttrKey: function () {
            let _this = this;
            let key = '';

            this.proData.AttrList.forEach((item) => {
                let flag = false;
                item.Values.forEach((citem) => {
                  if (_this.proAttr["attr" + item.Seq] && citem.Id === _this.proAttr["attr" + item.Seq].Id) {
                    key +=  citem.Id.toLowerCase();
                    flag = true;
                  }
                });

                if (!flag) key +=  "00000000-0000-0000-0000-000000000000";
            });

            if (key) {
                this.AttrKey = key.MD5();
            }
        },
        formvideo: function() {
          var _this = this;

          if (!this.videoUrl) {
              $('.vAndi').hide();
              $('.ban_video').hide();
          }

          //视频切换按钮
          $(".videoBtn").on("click", function() {
              $(".videoBtn").addClass("SWactive");
              $(".imgBtn").removeClass("SWactive");
              _this.preSwiper.slideTo(0, 1000, false);
              return false;
          });
          //图片切换按钮
          $(".imgBtn").on("click", function() {
              $(".imgBtn").addClass("SWactive");
              $(".videoBtn").removeClass("SWactive");
              _this.preSwiper.slideTo(1, 1000, false);
              $(".video_btn").show();
              return false;
          });
        },
        videostop: function() {
            this.isPlayVideo = false;
        },
        videoplay: function() {
            this.isPlayVideo = true;
        },
        // 匹配url傳參自動操作
        matchQuery: function () {
          let attr = getQueryString('attr') ? JSON.parse(getQueryString('attr')) : '';
          let fav = getQueryString('fav');
          let buy = getQueryString('buy') === '1' ? 1 : 0;
          let _this = this;
          if (attr) {
            this.proData.AttrList.forEach((item)=> {
              let value = attr['attr' + item.Seq];
              if (value) {
                item.selected = value;

                _this.proAttr['attr' + item.Seq] = item.Values.find(function(i) { return i.Id === value; });
              }
            });

            this.buyQty = Number(getQueryString('qty'));

            this.addToCart(buy);

            history.replaceState(null, document.title, location.pathname);
          } else if (fav) {
            this.addToFavorite(this.proData);
          }
        },
        // 价格格式化
        PriceFormat: function (value) {
          return PriceFormat(value);
        }
	},
	created() {
    this.pmtRelateProd = {
      Name: '為你推介',
      PrmtProductList: this.relateProd || []
    };

    this.matchQuery();
	},
	mounted() {
    console.log(this.proData,'產品數據');
    console.log(this.merchantData,'商家數據');
    console.log(this.relateProd, '相關產品數據');
    this.initPreview();
    this.initHandle();
    this.getComments();
	}
}).mount('#container')