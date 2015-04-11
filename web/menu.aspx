<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="menu.aspx.cs" Inherits="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/admin.css" type="text/css" rel="stylesheet">

    <script language="javascript">
        function OpenWin(Address) {
            window.open(Address, "main");
        }
        function expand(el) {
            var TheCurrentEleToHideOrToShow = $("#" + el + " h6:first").css("display") == "none";
            try {
                for (var i = 0; i < 8; i++) {
                    var Ele = $("#div" + (i + 1) + " ul:first");
                    if (Ele != undefined)
                        $("#div" + (i + 1) + " h6:first").hide(100);
                    var a = 1;
                }
                if (TheCurrentEleToHideOrToShow) {
                    $("#" + el + " h6:first").show(500);
                }
            }
            catch (ex) { }
        }
    </script>

    <style>
        html, table
        {
            height: 100%;
        }
        body
        {
            height: 100%;
            background-image: url("/images/menu_bg.jpg");
            background-attachment: scroll;
            background-repeat: repeat-y;
            background-position-x: 0%;
            background-position-y: 0%;
            background-size: auto;
            background-origin: padding-box;
            background-clip: border-box;
        }
        h6
        {
            margin: 0px;
            padding: 0px;
            font-size: 12px;
            display: none;
            font-weight: normal;
        }
        #TextContent1
        {
        }
        #TextContent1 li
        {
            line-height: 20px;
            list-style-type: decimal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 175px; overflow: hidden;">
        <asp:Panel ID="PCall" runat="server">
            <div class="MeunContainer" id="div1">
                <div class="MeunFirst" onclick="expand(this.parentElement.id)">
                    报修管理
                </div>
                <h6 class="ItemContainer">
                    <ul class="LeftMeunUl">
                        <asp:Literal ID="LCallAdd" runat="server">
                            <li><a href="/page/call/add.aspx" target="main">新建报修</a></li>
                        </asp:Literal>
                        <asp:PlaceHolder ID="LCallSln1" runat="server">
                            <li><a href="/page/call/ListSln1.aspx"  target="main" >
                                <div class="CountSpan" >
                                    等待安排上门<span id="CountSpan0"></span></div>
                            </a></li>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="LCallList" runat="server">
                            <li><a target="main" href="/page/call/list.aspx?state=<%=(int)CSMP.Model.SysEnum.CallStateMain.未处理%>">
                                <div class="CountSpan" >
                                    未处理<span id="CountSpan1"></span></div>
                            </a></li>
                            <li><a target="main" href="/page/call/list.aspx?state=<%=(int)CSMP.Model.SysEnum.CallStateMain.处理中%>">
                                <div class="CountSpan">
                                    处理中<span id="CountSpan2"></span></div>
                            </a></li>
                            <li><a target="main" href="/page/call/list.aspx?state=<%=(int)CSMP.Model.SysEnum.CallStateMain.已完成%>">
                                <div class="CountSpan" >
                                    已完成<span id="CountSpan3"></span></div>
                            </a></li>
                            <li><a target="main" href="/page/call/list.aspx?state=<%=(int)CSMP.Model.SysEnum.CallStateMain.已关闭%>">
                                <div class="CountSpan" >
                                    已关闭<span id="CountSpan4"></span></div>
                            </a></li>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="LCallSch" runat="server">
                            <li><a href="/page/call/Sch.aspx" target="main">报修查询</a></li>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="LAddMany" runat="server">
                            <li><a href="/page/call/AddMany.aspx" target="main">批量报修</a></li>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="LCustomerRequest" runat="server">
                            <li><a href="/page/CustomerRequest/list.aspx?EnableData=1" target="main">
                                <div class="CountSpan" >
                                    报修请求<span id="CountSpan6"></span></div>
                                </a></li>
                        </asp:PlaceHolder>
                    </ul>
                </h6>
            </div>
        </asp:Panel>
        <asp:Panel ID="PSystem" runat="server">
            <div class="MeunContainer" id="div2">
                <div class="MeunFirst" onclick="expand(this.parentElement.id)">
                    系统管理
                </div>
                <h6 class="ItemContainer">
                    <ul class="LeftMeunUl">
                        <asp:Literal ID="LLog" runat="server">
                            <li><a href="/page/log/list.aspx" target="main">系统日志</a></li>
                            <li><a href="/page/OperationRec/list.aspx" target="main">报修回滚日志</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LInport" runat="server">
                            <li><a href="/page/inport/" target="main">数据导入</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LProfile" runat="server">
                            <li><a href="/page/system/EmailSetting.aspx" target="main">邮箱设置</a></li>
                            <li><a href="/page/system/SendEmail.aspx" target="main">发邮件</a></li>
                            <li><a href="/page/system/API_Message/Setting.aspx" target="main">短信设置</a></li>
                            <li><a href="/page/system/API_Message/Send.aspx" target="main">发短信</a></li>
                            <li><a href="/page/Profile/TempLate.aspx" target="main">邮件模板</a></li>
                            <li><a href="/page/System/API_YUM/" target="main">邮件接口</a></li>
                            <li><a href="/page/Profile/Edit.aspx" target="main">参数设置</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LServerMsg" runat="server">
                            <li><a href="/page/system/ServerMsg.aspx" target="main">系统信息</a></li>
                            <li><a href="/page/Task/List.aspx" target="main">定时任务</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LAttachment" runat="server">
                                <li><a href="/page/Attachment/List.aspx" target="main">文件管理</a> </li>
                        </asp:Literal>
                    </ul>
                </h6>
            </div>
        </asp:Panel>
        <asp:Panel ID="PBasedata" runat="server">
            <div class="MeunContainer" id="div3">
                <div class="MeunFirst" onclick="expand(this.parentElement.id)">
                    基础信息管理
                </div>
                <h6 class="ItemContainer">
                    <ul class="LeftMeunUl">
                    <asp:Literal ID="LCustomerBrand" runat="server">
                        <li><a href="/page/Customer/List.aspx" target="main">客户管理</a></li>
                        <li><a href="/page/Brand/List.aspx" target="main">品牌管理</a> </li>
                    </asp:Literal>
                    <asp:Literal ID="LStore" runat="server">
                        <li><a href="/page/Store/List.aspx" target="main">店铺管理</a></li>
                    </asp:Literal>
                    <asp:Literal ID="LProvinceCity" runat="server">
                        <li><a href="/page/Province/List.aspx" target="main">省份管理</a></li>
                        <li><a href="/page/City/List.aspx" target="main">城市管理</a> </li>
                    </asp:Literal>
                    <asp:Literal ID="LClass" runat="server">
                        <li><a href="/page/Class1/List.aspx" target="main">大类故障管理</a></li>
                        <li><a href="/page/Class2/List.aspx" target="main">中类故障管理</a></li>
                        <li><a href="/page/Class3/List.aspx" target="main">小类故障管理</a></li>
                        <li><a href="/page/system/InitClass.aspx" target="main">初始化所有项目故障类</a></li>
                    </asp:Literal>
                    <asp:Literal ID="LJobcode" runat="server">
                        <li><a href="/page/Jobcode/List.aspx" target="main">Jobcode管理</a> </li>
                    </asp:Literal>
                    <asp:Literal ID="LThirdParty" runat="server">
                        <li><a href="/page/ThirdParty/List.aspx" target="main">第三方管理</a> </li>
                    </asp:Literal>
                    <asp:Literal ID="LWorkGroupEmail" runat="server">
                            <li><a href="/page/WorkGroupEmail/List.aspx" target="main">常用收件人</a> </li>
                            <li><a href="/page/EmailGroup/List.aspx" target="main">收件人组</a> </li>
                    </asp:Literal>
                    <asp:Literal ID="LSLAModel" runat="server">
                            <li><a href="/page/SlaMode/List.aspx" target="main">SLA工作时间管理</a> </li>
                            <%--<li><a href="/page/SlaModeDetail/List.aspx" target="main">SLA工作时间详细</a> </li>--%>
                    </asp:Literal>
                    <asp:Literal ID="LCallCategory" runat="server">
                            <li><a href="/page/CallCategory/List.aspx" target="main">服务类型管理</a> </li>
                            <%--<li><a href="/page/SlaModeDetail/List.aspx" target="main">SLA工作时间详细</a> </li>--%>
                    </asp:Literal>
                    </ul>
                </h6>
            </div>
        </asp:Panel>
        <asp:Panel ID="PUser" runat="server">
            <div class="MeunContainer" id="div4">
                <div class="MeunFirst" onclick="expand(this.parentElement.id)">
                    操作人员管理
                </div>
                <h6 class="ItemContainer">
                    <ul class="LeftMeunUl">
                        <asp:Literal ID="LUser" runat="server">
                    <li><a href="/page/User/List.aspx" target="main">用户管理</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LPower" runat="server">
                    <li> <a href="/page/Group/List.aspx" target="main">权限组管理</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LWorkGroup" runat="server">
                    <li><a href="/page/workGroup/List.aspx" target="main">工作组管理</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LGroupCustomer" runat="server">
                    <li><a href="/page/WorkGroupBrand/List.aspx" target="main">组_品牌关系管理</a></li>
                        </asp:Literal>
                    </ul>
                </h6>
            </div>
        </asp:Panel>
        <asp:Panel ID="PReport" runat="server">
            <div class="MeunContainer" id="div5">
                <div class="MeunFirst" onclick="expand(this.parentElement.id)">
                    统计报表
                </div>
                <h6 class="ItemContainer">
                    <ul class="LeftMeunUl">
                        <asp:Literal ID="LReportA" runat="server">                    
                    <li><a href="/page/Report/City.aspx" target="main">A、TOP10城市报修统计</a></li>
                        </asp:Literal>
                     <asp:Literal ID="LReportK" runat="server">
                        <li><a href="/page/Report/StoreTop.aspx" target="main">B、TOP10店铺报修统计</a></li>
                    </asp:Literal>
                       <asp:Literal ID="LReportB" runat="server">
                     <li><a href="/page/Report/ClassPercent.aspx" target="main">C、故障类分析</a> </li>
                        </asp:Literal>
                     <asp:Literal ID="LReportO" runat="server" Visible="false">
                        <li><a href="/page/Report/StatO.aspx" target="main">D、城市报修占比</a></li>
                    </asp:Literal>
                        <asp:Literal ID="LReportJ" runat="server">
                    <li><a href="/page/Report/MonthCount.aspx" target="main">E、月报修总量趋势</a></li>
                        </asp:Literal>
                       <asp:Literal ID="LReportC" runat="server">
                    <li><a href="/page/Report/Hour.aspx" target="main">F、时段报修趋势</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LReportN" runat="server">
                    <li><a href="/page/Report/StatN.aspx" target="main">G、品牌pending分析</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LReportF" runat="server">
                    <li><a href="/page/Report/StatListDefault.aspx" target="main">H、明细数据</a></li>
                        </asp:Literal>
                    <asp:Literal ID="LEmailRecord" runat="server">
                        <li><a href="/page/EmailRecord/List.aspx" target="main">I、派工邮件统计</a></li>
                    </asp:Literal>
                      <asp:Literal ID="LPunchIn" runat="server">
                        <li><a href="/page/PunchIn/List.aspx" target="main">J、考勤管理</a></li>
                    </asp:Literal>
                        <asp:Literal ID="LReportH" runat="server">
                    <li><a href="/page/Report/BrandSolvedBy.aspx" target="main">K、服务方式分析</a></li>
                        </asp:Literal>
                        
                        <asp:Literal ID="LiteralL" runat="server">
                    <li><a href="/page/Report/CustomDateReport_Brand.aspx" target="main">L、日报修统计(品牌)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralM" runat="server">
                    <li><a href="/page/Report/CustomDateReport.aspx" target="main">M、日报修统计(故障)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralP" runat="server">
                    <li><a href="/page/Report/CustomMonthReport_Brand.aspx" target="main">P、月报修统计(品牌)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralQ" runat="server">
                    <li><a href="/page/Report/CustomMonthReport.aspx" target="main">Q、月报修统计(故障)</a></li>
                        </asp:Literal>
                        
                        <asp:Literal ID="LiteralR" runat="server">
                    <li><a href="/page/Report/CustomDateToDate_Brand.aspx" target="main">R、日期段统计(品牌)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralS" runat="server">
                    <li><a href="/page/Report/CustomDateToDate.aspx" target="main">S、日期段统计(故障)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralPMDB" runat="server">
                    <li><a href="/page/Report/ProjectManagerReport/PMDateReport_Brand.aspx" target="main">PM、日报修统计(品牌)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralPMDC" runat="server">
                    <li><a href="/page/Report/ProjectManagerReport/PMDateReport.aspx" target="main">PM、日报修统计(故障)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralPMMB" runat="server">
                    <li><a href="/page/Report/ProjectManagerReport/PMMonthReport_Brand.aspx" target="main">PM、月报修统计(品牌)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralPMMC" runat="server">
                    <li><a href="/page/Report/ProjectManagerReport/PMMonthReport.aspx" target="main">PM、月报修统计(故障)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralPMDTDB" runat="server">
                    <li><a href="/page/Report/ProjectManagerReport/PMDateToDateReport.aspx" target="main">PM、日期段统计(品牌)</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LiteralPMDTDC" runat="server">
                    <li><a href="/page/Report/ProjectManagerReport/PMDateToDateReport_Brand.aspx" target="main">PM、日期段统计(故障)</a></li>
                        </asp:Literal>
                        
                        
                        
                        
                    </ul>
                </h6>
            </div>
        </asp:Panel>
        <asp:Panel ID="PKnowledgeBase" runat="server">
            <div class="MeunContainer" id="div6">
                <div class="MeunFirst" onclick="expand(this.parentElement.id)">
                    知识库
                </div>
                <h6 class="ItemContainer">
                    <ul class="LeftMeunUl">
                        <asp:Literal ID="LSolution" runat="server">
                            <li><a href="/page/Solution/List.aspx" target="main">解决方案管理</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LFeedback" runat="server" Visible="false">
                            <li><a href="/page/Feedback/list.aspx" target="main">回访管理</a></li>
                            <li><a href="/page/Report/Feedback.aspx" target="main">回访分析</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LComment" runat="server">
                            <li><a href="/page/comment/List.aspx" target="main">互评管理</a></li>
                        </asp:Literal>
                        <asp:Literal ID="LKnowLedgeBaseLibrary" runat="server">
                            <li><a href="/page/KnowledgeBase/List.aspx" target="main">知识库管理</a></li>
                        </asp:Literal>
                    </ul>
                </h6>
            </div>
        </asp:Panel>

    </div>
    <div style="clear: both; position: absolute; bottom: 0px; line-height: 26px; height: 26px;
        width: 155px; text-align: center; margin-left: 3px; background: url(/images/b.gif);">
       <a style=" margin-right:20px;" id="RefreshTimer" href="javascript:location.reload();" title="这是刷新倒计时，点我也可以手动刷新" ></a>
        <a href="/page/trace/list.aspx" target="main">店铺催促<span id="CountSpan5"></span></a>
    </div>

    <script language="javascript">
        
        function UpdateCount() {
            var arr = new Array();
            $.get("/page/call/JS/GetCallCount.ashx", function(result) {
                arr = result.split("|");
                for (var i = 0; i < arr.length; i++) {
                   
                    $("#CountSpan" + i).html("(" + arr[i] + ")");
                }
            });
        }
        UpdateCount();
        
        
        var TimeSpan = 30;
        function Timer_Trick() {
            if (TimeSpan == 0) {
                UpdateCount();
                TimeSpan = 30;
                return;
            }
            TimeSpan--;
            $("#RefreshTimer").html(TimeSpan);
        }

        setInterval("Timer_Trick()", 1000);
    </script>

</asp:Content>
