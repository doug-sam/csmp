<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Top.aspx.cs" Inherits="Top" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="/js/Public.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SetSkinCookies(SkinName) {
            setCookie("Skin", SkinName, 999999);
            var SkinName = getCookie("Skin");
            top.main.location.reload();
        }
        
    </script>


    <style>
        li
        {
            height: 22px;
            font-size: 12px;
        }
        li a
        {
            color: #FFF;
            text-decoration: none;
        }
        .Bar
        {
            color: #F00;
            line-height: 22px;
            font-weight: 700;
            margin-top: 3px;
        }
        .UlLogin{ margin:0; padding:0; list-style:none;}
        .UlLogin li{ margin:0; padding:0; float:right; }
    </style>
    <style type="text/css">
   .SkinMenu{ }
   .SkinMenu a{ color:#152E6D;}
   .SkinMenu p{ line-height:18px; text-align:center;}
   .SkinMenu a:hover {
    background:#ffffff; /*要兼容IE6的话必须添加背景色*/
    text-decoration:none;
    }
   .SkinMenu a.tooltip span {
    display:none;
    padding:2px 3px;
    margin-left:-8px;
    margin-top:-20px;
    width:40px;
    }
   .SkinMenu a.tooltip:hover span{
    display:inline;
    position:absolute;
    background:#ffffff;
    border:1px solid #cccccc;
    color:#6c6c6c;
    }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="header">
        <div id="title">
            <div id="logo">
            </div>
            <!-- end of logo-->
            <div id="header_right">
                <div id="tail_menu" align="right" style="font-size: 14px; line-height: 22px; height: 62px;
                    margin: 10px 30px 0px 0px; color: #152e6d; font-weight: 700;">
                    欢迎你！<%=CurrentUserName %>(来自
                    <%=CSMP.BLL.WorkGroupBLL.Get(CurrentUser.WorkGroupID).Name %>
                    的
                    <%=string.Join(",",CurrentUser.Rule.ToArray()) %>)
                    <ul class="UlLogin">
                        <li>
                        | <a href="LogOut.aspx" class="link_2">注销</a>
                        </li>
                        <li>
                        | <a href="/page/User/Pwd.aspx" target="main" class="link_2">修改密码</a>
                        </li>
                        <li>
                          |<a href="/help/help.html"  class="link_2" target="_blank">查看说明书</a>
                        </li>
                        <li>
                          |<a href="/Note.html"  class="link_2" target="main">查看更新说明</a>
                        </li>
                        <li class="SkinMenu">
                           | 
                            <a class="tooltip" href="#">皮肤设置<span>
                            <p onclick="SetSkinCookies('Skin_Default')">默认</p>
                            <p onclick="SetSkinCookies('Skin_Second')">皮肤1</p>
                            <p onclick="SetSkinCookies('Skin_Third')">皮肤2</p> 
                            <p onclick="SetSkinCookies('Skin_Fourth')">皮肤3</p> 
                            </span>
                            </a>
                        </li>
                        <li>
                          |<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=378862252&site=qq&menu=yes">
            <img border="0" src="http://wpa.qq.com/pa?p=2:378862252:41" alt="问题反馈.系统问题反馈，上海-段卓雅" title="问题反馈.系统问题反馈，上海-段卓雅"></a>
                        </li>
                    </ul>
                </div>
                <!-- end of tail_menu-->
            </div>
        </div>
        <%--<span style="  margin-top:4px; color:#FFF; float:left;">系统版本：Beat V12.27</span>--%>
        
    </div>


</asp:Content>
