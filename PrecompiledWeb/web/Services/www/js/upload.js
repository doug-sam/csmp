
/**FileTransfer*/
var ft;

/**
 * 清除上传进度，处理上传失败，上传中断，上传成功
 */
function clearProcess() {
    $('.upload_process_bar,#process_info').hide();
    ft.abort();
};

/**
 * 打开文件选择器,并让其支持所有文件的选择。
 */
function openFileSelector() {
    var source = navigator.camera.PictureSourceType.PHOTOLIBRARY;
    //描述类型，取文件路径
    var destinationType = navigator.camera.DestinationType.FILE_URI;
    //媒体类型，设置为ALLMEDIA即支持任意文件选择
    var mediaType = navigator.camera.MediaType.ALLMEDIA;
    var options = {
        quality: 50,
        destinationType: destinationType,
        sourceType: source,
        mediaType: mediaType
    };
    navigator.camera.getPicture(uploadFile, uploadBroken, options);
};

/**
 * 上传意外终止处理。
 * @param message
 */
function uploadBroken(message) {
    alert(message);
    clearProcess();
};

/**
 * 上传过程回调，用于处理上传进度，如显示进度条等。
 */
function uploadProcessing(progressEvent) {

    if (progressEvent.lengthComputable) {
        //已经上传
        var loaded = progressEvent.loaded;
        //文件总长度
        var total = progressEvent.total;
        //计算百分比，用于显示进度条
        var percent = parseInt((loaded / total) * 100);
        //换算成MB
        loaded = (loaded / 1024 / 1024).toFixed(2);
        total = (total / 1024 / 1024).toFixed(2);
        $('#process_info').html(loaded + 'M/' + total + 'M');
        $('.upload_current_process').css({ 'width': percent + '%' });
    }
};

/**
 * 选择文件后回调上传。
 */
function uploadFile(fileURI) {
    var options = new FileUploadOptions();
    options.fileKey = "file";
    options.fileName = fileURI.substr(fileURI.lastIndexOf('/') + 1);
    options.mimeType = "multipart/form-data";
    options.chunkedMode = false;
    ft = new FileTransfer();
    var uploadUrl = encodeURI(MainURL + "/services/doc/GetFile2.ashx?callid=" + window.localStorage.getItem("ViewItem"));
    ft.upload(fileURI, uploadUrl, uploadSuccess, uploadFailed, options);
    //获取上传进度
    ft.onprogress = uploadProcessing;
    //显示进度条
    $('.upload_process_bar,#process_info').show();
}

/**
 * 上传成功回调.
 * @param r
 */
function uploadSuccess(r) {
    if (r.response!="success") {
        alert("上传失败:" + r.response);
        return;
    }
    alert('文件上传成功');
    clearProcess();
    location.href = this.location.href;
}

/**
 * 上传失败回调.
 * @param error
 */
function uploadFailed(error) {
    alert('上传失败了。');
    clearProcess();
}

/**
 * 页面实例化回调.
 */
document.addEventListener("deviceready", function () {
    $(function () {
        $('#upload_file_link').click(openFileSelector);
    });
}, false);



