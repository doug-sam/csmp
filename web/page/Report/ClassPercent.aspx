<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="ClassPercent.aspx.cs" Inherits="page_Stat_ClassPercent" %>

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
		                text: '故障类分析',           //图标的标题
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
		                    text: '所占比例'
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
		                    name: '报修量',
		                    data: [<asp:Literal runat="server" ID="LtlPrecent"></asp:Literal>],
                            dataLabels: {
                                    enabled: true,
                                    rotation: -0,
                                    color: '#000',
                                    align: 'right',
                                    x: -10,
                                    y: -3
                                }
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
                    <td colspan="4" style="height: 30px" class="td1_1">
                      故障类分析
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        报修日期 从
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxtDateBegin" runat="server" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        至
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEnd" runat="server" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                </tr>
<tr>
                    <td class="td1_2">
                        所属省份
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlProvince" DataTextField="Name" DataValueField="ID" runat="server"
                            AutoPostBack="True" 
                            onselectedindexchanged="DdlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        所属城市
                    </td>
                    <td class="td1_3">
                         <asp:DropDownList ID="DdlCity" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>                <tr>
                    <td class="td1_2">
                        所属客户/品牌/大类故障/中类故障
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID" >
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlClass1" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="DdlClass1_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlClass2" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSch" runat="server" Text="搜&nbsp;&nbsp;&nbsp;&nbsp;索" OnClick="BtnSch_Click"
                            CssClass="BigButton" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSch" />
        </Triggers>
    </asp:UpdatePanel>
    
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
            <asp:TemplateField HeaderText="故障">
                <ItemTemplate>
                    <%# Eval("StatClass")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="数量">
                <ItemTemplate>
                    <%# Eval("StatCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="占比例">
                <ItemTemplate>
                    <%# Eval("StatPercent")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
