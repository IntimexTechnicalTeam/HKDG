var Id = $("#orderId").val();

createApp({
	components: {
		'star-rating': StarRating
	},
  	data() {
  		return {
        comments: [
            {
                fontNum:0
            }
        ],
        fontNum: 0,
        OrderId:""
  		}
	},
	methods: {
    getComments: function () {
        var _this = this;
        InstoreSdk.api.prodcomment.getSubOrderComments(Id, function (data) {
            _this.comments = data;
            console.log(data,'返回的评论数据')
            _this.OrderId = data[0].OrderId;
            for(var i = 0;i < _this.comments.length;i++){
              _this.comments[i].Content = _this.comments[i].Content ? _this.comments[i].Content : '';
                _this.comments[i].fontNum = _this.comments[i].Content.length;
                if(_this.comments[i].Score < 1 ){
                    _this.comments[i].Score = 1;
                }

                console.log(_this.comments[i].CommentImages.length, _this.comments[i].ProductName + '的评论图片数量')
                _this.comments[i].ImgSum = _this.comments[i].CommentImages.length;
            }
            //vm.$nextTick(function () {
            //    vm.showImage();
            //});
        });
    },
    saveComment: function () {
        var _this =this;
        InstoreSdk.api.prodcomment.saveComments(this.comments, function (data) {
            if (data.Succeeded == true) {
                showInfoSuccess(InstoreSdk.message.CommentSucceeded);
      //          vm.getComments();
                setTimeout(function(){
                    window.location.href = "/order/detail/" + _this.OrderId;
                },1500);
                
            }
            else {
                showInfo(data.Message);
            }
        });
    },
    upload: function (comment, index) {
        var formData = new FormData();
        var files = document.getElementById("file" + index).files;
        var commentImages = [];
        var errMessage = checkUploadFormat(files);
        // if (comment.CommentImages.length + files.length > 5) {
        //     errMessage = InstoreSdk.message.FileNotMoreThan + '5';
        // }
        if (comment.ImgSum == 5) {
            errMessage = InstoreSdk.message.FileNotMoreThan + '5';
        }
        if (errMessage == "") {
            for (var i = 0; i < files.length; i++) {
                //console.log(files[i]);
                formData.append("file" + i, files[i]);
            }

            //comment
            //console.log(formData);
            var _this = this;
            InstoreSdk.api.uploadfile.uploadFiles(formData, function (data) {
                console.log(data,'pppp')
                // this.a();
                if (data.Succeeded == true) {
                    for (var i = 0; i < data.ReturnValue.length; i++) {
                        //$("#img" + i + index).attr("src", data.ReturnValue[i].Path);
                        //填充评论的晒图
                        //commentImages.push({
                        //    Id: null,
                        //    CommentId: null,
                        //    SmallImage: data.ReturnValue[i].Path,
                        //    BigImage: ''
                        //});
                        comment.CommentImages.push({
                            Id: null,
                            CommentId: null,
                            ImageName: data.ReturnValue[i].Name,
                            SmallImagePath: data.ReturnValue[i].Path,
                            ImageB: data.ReturnValue[i].Path,
                            BigImagePath: '',
                            IsDeleted:false

                        }); 
                    }
                    comment.ImgSum = 0;
                    for(var i=0; i < comment.CommentImages.length; i++){
                        if(comment.CommentImages[i].IsDeleted == false){
                            comment.ImgSum += 1;
                        }
                    }
                    console.log(comment.ImgSum, comment.ProductName + '的评论图片数量')
                }
                else {
                    showInfo(data.Message);
                }

            });
        }
        else {
            showInfo(errMessage);
        }
    },
    updateSorce: function (obj, sorce) {
        console.log(0);
        obj.Score = sorce;
    },
    selectFile: function (comment, index) {
        var files = document.getElementById("file" + index).files;
        files.value = '';
        //vm.cleanImage();
        //if (comment.CommentImages.length > 0) {
        //    comment.CommentImages.splice(0, comment.CommentImages.length);
        //}

        $("#file" + index).click();
    },
    showImage: function () {
        for (var i = 0; i < this.comments.length; i++) {
            for (var j = 0; j < this.comments[i].CommentImages.length; j++) {
                $("#img" + j + i).attr("src", this.comments[i].CommentImages[j].SmallImage);
            }
        }
    },
    cleanImage: function () {
        for (var i = 0; i < this.comments.length; i++) {
            for (var j = 0; j < this.comments[i].CommentImages.length; j++) {
                $("#img" + j + i).attr("src", "");
            }
        }
    },
    deleteImage:function(img,index,index2){
        img.IsDeleted = true;
        if(img.Id == null){
            this.comments[index].CommentImages.splice(index2,1);
        }
        this.comments[index].ImgSum = 0;
        for(var i=0; i < this.comments[index].CommentImages.length; i++){
            if(this.comments[index].CommentImages[i].IsDeleted == false){
                this.comments[index].ImgSum += 1;
            }
        }
        console.log(this.comments[index].ImgSum, this.comments[index].ProductName + '的评论图片数量')
        // this.comments[index].CommentImages.splice(index2,1);
    },
    calcNum:function(one){
        var vals = one.Content;
        one.fontNum = vals.length;
        if(one.fontNum > 1000){
            one.Content = one.Content.substr(0,1000);
            one.fontNum = 1000;
        }
    },
    getCalcNum:function(){
        var vals = $('.comment-area').val();
        this.fontNum = vals.length;
    },
    //判断全为空格
    allSpace: function (str) {
        var reg = /^[ ]*$/;
        if (str.match(reg)) {
            return true;
        }
        else {
            return false;
        }
    }
	},
	mounted() {
    this.getComments();
	}
}).mount('#container')

function checkUploadFormat(files) {
    var errMessage = "";
    if (files.length > 5) {
        errMessage = InstoreSdk.message.FileNotMoreThan + '5';
        //errMessage = '111111111111';
        return errMessage;
    }

    for (var i = 0; i < files.length; i++) {
        if (files[i].type.indexOf("image") < 0) {
            //errMessage = '@Resources.Message.OnlyUploadImage';
            errMessage = InstoreSdk.message.OnlyUploadImage;
            return errMessage;
        }
        var fileLength = (files[i].size / 1024) / 1024;
        if (fileLength > 2) {
            errMessage = InstoreSdk.message.FileSizeNotMoreThan + '2M';
            return errMessage;
        }
    }
    return errMessage;
}