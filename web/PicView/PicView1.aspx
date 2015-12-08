<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PicView1.aspx.cs" Inherits="PicView_PicView1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<link rel="stylesheet" type="text/css" href="Css/Commom.css" />
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var data = { "Images/01_small.jpg": ["Images/01.jpg", "图片1"], "Images/02_small.jpg": ["Images/02.jpg", "图片2"], "Images/03_small.jpg": ["Images/03.jpg", "图片3"] };  //Key:Value;
        $(function () {
            $.each(data, function (key, value) {
                //初始化最后一个div为隐蔽
                $("div").last().hide();
                //ket是小图的地址；
                var smallPath = $("<img src='" + key + "'/>").css("margin", "5px").css("padding", "2px").css("border", "1px solid #000");
                //设置大图地址和名称；
                bigImgPath = smallPath.attr("bigMapPath", value[0]);
                bigImgName = smallPath.attr("bigMapName", value[1]);

                //给小图添加事件
                smallPath.mouseover(function () {
                    //最后一个div淡入效果
                    $("div").last().fadeIn("slow");
                    //获取大图地址
                    $("#show").attr("src", $(this).attr("bigMapPath"));
                    //获取大图名称并设置样式
                    $("#imgTitle").text($(this).attr("bigMapName")).css("background", "#ebf1de").css("padding", "10px").css("margin-bottom", "10px");
                });
                smallPath.mouseout(function () {
                    //指定最后一个div；
                    $("div").last().fadeOut("slow");
                });
                //.first方法，指第第一个DIV添加小图；（过滤器）
                $("div").first().append(smallPath);
            });
        });
    </script>
</head>
<body>
    <div class="column">
    </div>
    <div class="column">
        <p id="imgTitle">
        </p>
        <img id="show" src="" alt="" />
    </div>
</body>
</ html>
