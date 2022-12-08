createApp({
    data() {
        return {
            order: {
                Currency: {
                    Code: ''
                },
                Deliveries: [{
                    DeliveryItems: [{
                        Product: {
                            Imgs: [],
                            Currency: {
                                Code: ''
                            }
                        }
                    }]
                }]
            },
            merchantOrder: [{
                name: "",
                orderlist: []
            }],
            merchantOrder_t: [],
            merchantGroup: [],
            returnTypeList: [],
            selectReturnType: 1,
            returnText: '',
            orderSearchList: {
                Details: [{
                    MessageTime: "",
                    Message: ""
                }]
            },
            showReturnBtn: true,
            AttachImages: [],
            ImagesSrc: [],

            DiscountTotalPrice: 0,
            DiscountDeliveryCharge: 0
        }
    },
    methods: {
        getOrder: function() {
            var _this = this;
            InstoreSdk.api.order.getOrder(orderId, function(order) {
                _this.order = order.ReturnValue;
                var pageTitle = $(document).attr("title");
                //修改title值
                if (order.ReturnValue.OrderNO) {
                    $(document).attr("title", pageTitle + " - " + order.ReturnValue.OrderNO);
                } else {
                    $(document).attr("title", pageTitle + " - " + order.ReturnValue.InvoiceNO);
                }

                console.log(_this.order);
                //组装店铺数组
                var m_flag = true;
                for (var i = 0; i < _this.order.Deliveries.length; i++) {
                    m_flag = true;
                    for (var j = 0; j < _this.merchantGroup.length; j++) {
                        if (_this.merchantGroup[j] == _this.order.Deliveries[i].MerchantName) {
                            m_flag = false;
                            break;
                        }
                    }
                    if (m_flag) {
                        _this.merchantGroup.push(_this.order.Deliveries[i].MerchantName);
                    }
                }

                //组装订单列表
                for (var i = 0; i < _this.merchantGroup.length; i++) {
                    _this.merchantOrder_t.push({
                        name: _this.merchantGroup[i],
                        orderlist: []
                    });
                }



                for (var i = 0; i < _this.order.Deliveries.length; i++) {
                    for (var j = 0; j < _this.merchantOrder_t.length; j++) {
                        if (_this.order.Deliveries[i].MerchantName == _this.merchantOrder_t[j].name) {
                            _this.merchantOrder_t[j].orderlist.push(_this.order.Deliveries[i]);
                        }
                    }
                }

                console.log(_this.merchantOrder_t, '组装后数据');
                console.log(_this.merchantOrder_t[0].orderlist[0].Discounts, '商家优惠数据');

                for (var i = 0; i < _this.merchantOrder_t.length; i++) {
                    for (var j = 0; j < _this.merchantOrder_t[i].orderlist.length; j++) {
                        _this.DiscountTotalPrice += _this.merchantOrder_t[i].orderlist[j].DiscountTotalPrice;
                        _this.DiscountDeliveryCharge += _this.merchantOrder_t[i].orderlist[j].DiscountDeliveryCharge;
                    }
                }
                console.log(_this.DiscountTotalPrice, '平台合计', _this.DiscountDeliveryCharge, '平台邮费合计');

                _this.order.ItemsAmount = _this.order.ItemsAmount;
                _this.order.DeliveryCharge = _this.priceFormat(_this.order.DeliveryCharge.toString(), 2);
                _this.order.TotalAmount = _this.priceFormat(_this.order.TotalAmount.toString(), 2);

                for (var i = 0; i < _this.order.OrderItems.length; i++) {
                    _this.order.OrderItems[i].Product.SalePrice = _this.priceFormat(_this.order.OrderItems[i].Product.SalePrice.toString(), 2);
                }
                for (var i = 0; i < _this.order.Deliveries.length; i++) {
                    for (var z = 0; z < _this.order.Deliveries[i].DeliveryItems.length; z++) {
                        _this.order.Deliveries[i].DeliveryItems[z].Product.SalePrice = _this.priceFormat(_this.order.Deliveries[i].DeliveryItems[z].Product.SalePrice.toString(), 2);
                    }
                }
            });
        },
        getOldOrder: function() {
            var _this = this;
            InstoreSdk.api.order.getOldOrder(orderId, function(order) {
                _this.order = order.ReturnValue;
                var pageTitle = $(document).attr("title");
                $(document).attr("title", pageTitle + " - " + order.ReturnValue.OrderNO); //修改title值

                console.log(_this.order);
                //组装店铺数组
                var m_flag = true;
                for (var i = 0; i < _this.order.Deliveries.length; i++) {
                    m_flag = true;
                    for (var j = 0; j < _this.merchantGroup.length; j++) {
                        if (_this.merchantGroup[j] == _this.order.Deliveries[i].MerchantName) {
                            m_flag = false;
                            break;
                        }
                    }
                    if (m_flag) {
                        _this.merchantGroup.push(_this.order.Deliveries[i].MerchantName);
                    }
                }

                //组装订单列表
                for (var i = 0; i < _this.merchantGroup.length; i++) {
                    _this.merchantOrder_t.push({
                        name: _this.merchantGroup[i],
                        orderlist: []
                    });
                }



                for (var i = 0; i < _this.order.Deliveries.length; i++) {
                    for (var j = 0; j < _this.merchantOrder_t.length; j++) {
                        if (_this.order.Deliveries[i].MerchantName == _this.merchantOrder_t[j].name) {
                            _this.merchantOrder_t[j].orderlist.push(_this.order.Deliveries[i]);
                        }
                    }
                }

                console.log(_this.merchantOrder_t, '组装后数据');
                console.log(_this.merchantOrder_t[0].orderlist[0].Discounts, '商家优惠数据');

                for (var i = 0; i < _this.merchantOrder_t.length; i++) {
                    for (var j = 0; j < _this.merchantOrder_t[i].orderlist.length; j++) {
                        _this.DiscountTotalPrice += (_this.merchantOrder_t[i].orderlist[j].DiscountTotalPrice);
                        if (_this.merchantOrder_t[i].orderlist[j].Discounts) {
                            _this.DiscountTotalPrice -= _this.merchantOrder_t[i].orderlist[j].Discounts[0].DiscountPrice;
                        }
                        _this.DiscountDeliveryCharge += _this.merchantOrder_t[i].orderlist[j].DiscountDeliveryCharge;
                    }
                }
                console.log(_this.DiscountTotalPrice, '平台合计', _this.DiscountDeliveryCharge, '平台邮费合计');

                _this.order.ItemsAmount = _this.order.ItemsAmount;
                _this.order.DeliveryCharge = _this.priceFormat(_this.order.DeliveryCharge.toString(), 2);
                _this.order.TotalAmount = _this.priceFormat(_this.order.TotalAmount.toString(), 2);

                for (var i = 0; i < _this.order.OrderItems.length; i++) {
                    _this.order.OrderItems[i].Product.SalePrice = _this.priceFormat(_this.order.OrderItems[i].Product.SalePrice.toString(), 2);
                }
                for (var i = 0; i < _this.order.Deliveries.length; i++) {
                    for (var z = 0; z < _this.order.Deliveries[i].DeliveryItems.length; z++) {
                        _this.order.Deliveries[i].DeliveryItems[z].Product.SalePrice = _this.priceFormat(_this.order.Deliveries[i].DeliveryItems[z].Product.SalePrice.toString(), 2);
                    }
                }
            });
        },
        pay: function() {
            // window.open("/payment/" + this.order.PMCode + "/" + orderId + "/" + this.order.PMCode, "_self");

            window.location.href = "/TranView/GoTo?returnUrl=" + BuyDong + "/payment/" + this.order.PMCode + "/" + orderId + "/" + this.order.PMCode;
        },
        comment: function(order) {
            window.open("/product/Comment/" + order.Id, "_self");
        },
        refundListOpen: function(event) {
            this.showReturnBtn = false;
            var flag = $(event.target).attr('data-open');
            if (flag == 0) {
                $(event.target).parents('.favorite-one').find('.refundList-content1').slideDown();
                $(event.target).attr('data-open', 1);
            } else {
                $(event.target).parents('.favorite-one').find('.refundList-content1').slideUp();
                $(event.target).attr('data-open', 0);
            }
        },
        refundListOut: function(event) {
            $(event.target).parents('.refundList-content1').slideUp();
            $(event.target).parents('.favorite-one').find('.order-one-refundList').attr('data-open', 0);
            this.showReturnBtn = true;
        },
        close: function() {
            var browserName = navigator.appName;
            if (browserName == "Netscape") {
                window.open('', '_parent', '');
                window.close();
                //window.open('', '_self', '');
                //window.close();
                //window.location.href = "about:blank";
                //window.close();
            } else if (browserName == "Microsoft Internet Explorer") {
                window.opener = "whocares";
                window.close();
            } else {
                window.close();
            }
        },
        //退貨方式
        returnOrderTypeList: function() {
            var _this = this;
            InstoreSdk.api.order.getReturnOrderTypeList(function(result) {
                if (result.Succeeded) {
                    _this.returnTypeList = result.ReturnValue;

                }

            })
        },
        //确认退货
        returnSure: function(OrderId, Skus, DeliveryId) {
            DeliveryId = DeliveryId || "";
            var returnorder = {
                OrderId: OrderId,
                Items: [{
                    DeliveryId: DeliveryId,
                    SkuId: Skus,
                    MerchantId: this.order.OrderItems[0].Product.MerchantId,
                    AttachImages: this.AttachImages
                }],
                ApplyType: this.selectReturnType,
                Message: this.returnText
            };
            console.log(returnorder, '退货订单传参')
            InstoreSdk.api.order.createReturnOrder(returnorder, function(result) {
                if (result.Succeeded) {
                    // alert(jsMessage.ReturnRequestSubmit);
                    addtocartS(jsMessage.ReturnRequestSubmit, '/Images/tick-icon.png');
                    setTimeout(function() {
                        window.location.reload();
                    }, 1500);

                } else {
                    addtocartS(result.Message, '/Images/warn-icon.png');
                }
            })
        },
        // 跳转退货单详情页
        // toRefundDetail:function(){
        //     window.open("/order/RefundDetail/"+this.order.Deliveries[0].DeliveryItems[0].ReturnOrderId);
        // },
        //订单追踪
        orderCheckWindow: function(tNum) {
            var _this = this;
            $('.detail-login-box').fadeIn();
            $('.detail-login').fadeIn();
            InstoreSdk.api.order.getOrderTrackingInfo(tNum, function(result) {
                _this.orderSearchList = result.ReturnValue;
                console.log(_this.orderSearchList);

            })
        },
        //关闭窗口
        closeWindow: function() {
            $('.detail-login').fadeOut();
            $('.detail-login-box').fadeOut();

        },
        //格式化金额
        priceFormat: function(price, num) {
            var totalNum = price.length;
            if (price.indexOf('.') < 0) {
                price = price + '.00';
            } else {
                if (price.indexOf('.') + num + 1 > totalNum) {
                    price = price + '0';
                } else {
                    price = price.substr(0, price.indexOf('.') + num + 1);
                }
            }
            //添加千分位
            var new_price_arr = price.split('.');
            var price_rev_arr = [];
            var price_norev_arr = [];
            var price_arr = [];
            price_norev_arr = new_price_arr[0].split('');
            price_rev_arr = price_norev_arr.reverse();
            if (price_rev_arr.length > 3) {
                for (var i = 0; i < price_rev_arr.length; i++) {
                    price_arr.push(price_rev_arr[i]);
                    if ((i + 1) % 3 == 0 && (i + 1) != price_rev_arr.length) {
                        price_arr.push(',');
                    }
                }
                price = price_arr.reverse().join('') + '.' + new_price_arr[1];

            }

            return price;
        },

        //上传图片
        selectFile: function(comment, index) {
            var files = document.getElementById("file" + index).files;
            files.value = '';
            $("#file" + index).click();
            console.log(comment, index, '上传成功')
        },
        upload: function(comment, index) {
            var formData = new FormData();
            var files = document.getElementById("file" + index).files;

            if (this.ImagesSrc.length + files.length > 5) {
                addtocartS(jsMessage.ImgsExceed5, '/Images/warn-icon.png');
                return;
            }

            for (var i = 0; i < files.length; i++) {
                //console.log(files[i]);
                formData.append("file" + i, files[i]);
            }

            //comment
            //console.log(formData);
            _this = this;
            InstoreSdk.api.uploadfile.uploadFiles(formData, function(data) {
                console.log(data, 'shuju')
                if (data.Succeeded == true) {
                    for (var i = 0; i < data.ReturnValue.length; i++) {
                        _this.AttachImages.push({
                            ImageBName: data.ReturnValue[i].Name,
                            ImageSName: data.ReturnValue[i].Thumbnail
                        });
                        _this.ImagesSrc.push({
                            path: data.ReturnValue[i].Path,
                            IsDelete: false
                        })
                    }
                    console.log(_this.ImagesSrc.length, '长度')
                } else {
                    showInfo(data.Message);
                }

            });
        },
        // 删除图片
        deleteImage: function(img, index) {
            this.AttachImages.splice(index, 1);
            this.ImagesSrc.splice(index, 1);
        }
    },
    created() {
      if (isOld) {
          this.getOldOrder();
      } else {
          this.getOrder();
      }
    },  
    mounted() {
        this.returnOrderTypeList();
        if (typeof(ajustWindowLogin) == 'function') {
            ajustWindowLogin();
        }
        if (typeof(adjustVedioBox) == 'function') {
            adjustVedioBox();
        }
    }
}).mount('#container')