<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Report_StatN, App_Web_hhnouuyt" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/Highcharts-3.0.7/js/highcharts.js"></script>
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
		                text: '品牌pending分析',           //图标的标题
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
		                    text: '数量'
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
		                    name: '故障类',
		                    data: [<asp:Literal runat="server" ID="LtlPrecent"></asp:Literal>],
                            dataLabels: {
                                    enabled: true,
                                    rotation: -0,
                                    color: '#000',
                                    align: 'right',
                                    x: -10,
                                    y: -10
                            }
		                }
                        ]
		            });
		        }	
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                品牌pending分析
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
        </tr>
        <tr>
            <td class="td1_2">
                品牌
            </td>
            <td class="td1_3" >
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="td1_2" colspan="2">
                <asp:checkbox ID="CbIsOverLSA" runat="server" Text="只查超SLA"></asp:checkbox>
                <asp:Button ID="BtnSearch" runat="server" Text="查询"  CssClass="BigButton" OnClick="BtnSearch_Click" />
            </td>
        </tr>
    </table>

    <div class="resulttable">
        <div id="ViewReport">
        </div>
    </div>

       <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="故障类">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server"><%# Eval("StatClass")%></asp:HyperLink>
                    
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="pending数量">
                <ItemTemplate>
                    <%# Eval("StatCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
