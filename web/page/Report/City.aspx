<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="City.aspx.cs" Inherits="page_Report_City" %>

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
		                text: '城市报修比例',           //图标的标题
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
		                    text: '报修数量'
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
		                    name: '城市',
		                    data: [<asp:Literal runat="server" ID="LtlPrecent"></asp:Literal>],
                            dataLabels: {
                                    enabled: true,
                                    rotation: -0,
                                    color: '#FFFFFF',
                                    align: 'right',
                                    x: -10,
                                    y: 20
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
                城市的报修比例top10
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
                <asp:Button ID="BtnSearch" runat="server" Text="查询"  CssClass="BigButton" OnClick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="resulttable">
        <div id="ViewReport">
        </div>
    </div>

     <div class="title-search">
        报表详细<asp:Literal ID="LtlCount" runat="server"></asp:Literal></div>
       <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" >
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="城市">
                <ItemTemplate>
                    <%# Eval("CityName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修量">
                <ItemTemplate>
                    <%# Eval("Count")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="占当前比例">
                <ItemTemplate>
                    <%# Eval("PercentCurrent")%>%
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="占当前总报修比">
                <ItemTemplate>
                    <%# Eval("PercentTotal")%>%
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
