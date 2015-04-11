var app_ClientVersion = 3;
var ServerPort = 7812;
var URLOutWeb = "http://124.74.9.202" + ":" + ServerPort;   //外网接口
var URLInWeb = "http://10.0.101.202" + ":" + ServerPort;    //内网接口

var MainURL = URLOutWeb;     //接口决定器
if (window.localStorage.getItem("MainURL") == URLInWeb)
{ MainURL = URLInWeb; }

function GetCallStateDetail()
{
    var CallStateDetailInfo = new Object();
    CallStateDetailInfo.WaitForRepairPart = 4;      //等待备件
    CallStateDetailInfo.WaitForArrangeDropIn = 5;   //等待安排上门
    CallStateDetailInfo.WaitForOtherDeal = 6;       //等待第三方响应
    CallStateDetailInfo.WaitForArrive = 7;          //等待工程师上门
    CallStateDetailInfo.UserGetDropInList=8;        //工程师接收服务单
    CallStateDetailInfo.UserDealing=9;              //上门支持
    CallStateDetailInfo.UserArrived = 12;             //到达门店处理
    return CallStateDetailInfo;
}
var CallStateDetail = GetCallStateDetail();

function GetCallStateMain()
{//未处理 = 1, 处理中 = 2, 已完成 = 3,已关闭=4
    var CallStateMainInfo = new Object();
    CallStateMainInfo.NotHandleYet = 1;
    CallStateMainInfo.Handling = 2;
    CallStateMainInfo.Finish = 3;
    CallStateMainInfo.Colsed = 4;
    return CallStateMainInfo;
}
var CallStateMain = GetCallStateMain();



//显示加载器  
function showLoader() {
    //显示加载器.for jQuery Mobile 1.1.0  
    $.mobile.loadingMessage = '加载中...';     //显示的文字  
    $.mobile.loadingMessageTextVisible = false; //是否显示文字  
    $.mobile.loadingMessageTheme = 'b';        //加载器主题样式a-e  
    $.mobile.showPageLoadingMsg();             //显示加载器  
}

//隐藏加载器.for jQuery Mobile 1.1.0  
function hideLoader() {
    //隐藏加载器   
    $.mobile.hidePageLoadingMsg();
}





