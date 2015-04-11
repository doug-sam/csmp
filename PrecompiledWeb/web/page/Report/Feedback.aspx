<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Report_Feedback, App_Web_jwnq_nxg" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/js/Highcharts-2.1.9/js/highcharts.js"></script>

    <!-- 1a) Optional: add a theme file -->
    <!--
			<script type="text/javascript" src="../js/themes/gray.js"></script>
		-->
    <!-- 2. Add the JavaScript to initialize the chart on document ready -->

    <script type="text/javascript">

		    var chart;
		    function Draw() {
		        chart = new Highcharts.Chart({
		            chart: {
		                renderTo: 'ViewReport',         //放置图表的容器
		                defaultSeriesType: 'column',      //图表类型line, spline, area, areaspline, column, bar, pie , scatter
		                marginRight: 60,
		                marginBottom: 30
		            },
		            title: {
		                text: '回访问卷统计',           //图标的标题
		                x: -20 //center
		            },
		            subtitle: {
		                text: '',   //图标的副标题
		                x: -20
		            },
		            xAxis: {
		                categories: [<asp:Literal runat="server" ID="LtlFootTxt"></asp:Literal>]
		            },
		            yAxis: {
		                title: {
		                    text: '所占数量'
		                },
		                plotLines: [{
		                    value: 0,
		                    width: 1,
		                    color: '#808080'
}]
		                },
		                tooltip: {
		                    formatter: function() {
		                        return '<b>' + this.series.name + '</b><br/>' +
								this.x + ': ' + this.y + '个';
		                    }
		                },
		                legend: {
		                    layout: 'vertical',
		                    align: 'right',
		                    verticalAlign: 'top',
		                    x: -10,
		                    y: 100,
		                    borderWidth: 0
		                },
		                series: [
		                {
		                    name: '选项',
		                    data: [<asp:Literal runat="server" ID="LtlPrecent"></asp:Literal>]
		                }
                        ]
		            });
		        }	
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="5" style="height: 30px" class="td1_1">
                        回访问卷统计
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        开始日期
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxtDateBegin" runat="server" class="input3" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        结束日期
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEnd" runat="server" class="input3" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                    </td>
                    <td class="td1_2" rowspan="3">
                        <asp:Button ID="BtnSearch" runat="server" Text="查询" CssClass="BigButton" OnClick="BtnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        客户
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        品牌
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        问卷
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlPaper" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" onselectedindexchanged="DdlPaper_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        问题
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlQuestion" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSearch" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="title-search">
        报表详细<asp:Literal ID="LtlCount" runat="server"></asp:Literal></div>
    <div class="resulttable">
        <div id="ViewReport">
        </div>
    </div>
</asp:Content>
