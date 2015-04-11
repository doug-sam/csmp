<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site2.master" AutoEventWireup="true" CodeFile="StatListDefault.aspx.cs" Inherits="page_Report_StatListDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        h2
        {
            margin:10px; 
        }
        .h2help
        {margin-left:40px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="text-align:center; margin:10px; padding:30px; border:1px solid #ccc;">
        <div style="text-align:left;">
            <h2><a href="http://124.74.9.202:7812/page/report/StatList3.aspx" target="_blank">延时数据导出</a></h2>
                <div class="h2help">
                    在这里我们将引你到另一地方进行导出，那里的数据均是一小时前的，
                    如果你无需导出实时的数据，
                    可以接受一小时的延误(例如你只需要昨天和更久远的数据)，
                    我们墙裂建议你点击这里导出，如果你是上海内网用户，
                    这个连接将打不开，
                    <a target="_blank" href="http://10.0.101.202:7812/page/report/StatList3.aspx"> 
                        请点击我并
                    </a>
                    进行登录后导出。
                    如果你发现访问不了的情况，就用下边的实时导出
                </div>
            <h2><a href="StatList3.aspx">实时数据导出</a></h2>
                <div class="h2help">
                    这时导出的数据是实时的，
                    但会稍微影响大家使用系统的速度，
                    如果你不是需要实时的数据(最近一小时的数据也得要的意思)，
                    请不要在这里导出。
                </div>
        </div>
    </div>
</asp:Content>

