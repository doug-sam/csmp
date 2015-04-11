<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="MonthCount.aspx.cs" Inherits="page_Stat_MonthCount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<script type="text/javascript" src="/js/Highcharts-2.1.9/js/highcharts.js"></script>--%>
    <script src="/js/Highcharts-3.0.7/js/highcharts.js"></script>

    <script type="text/javascript">

		    var chart;
		    function Draw() {
		        $(function () {
		            $('#ViewReport').highcharts({
		                chart: {
		                    type: 'line'
		                },
		                title: {
		                    text: '月报修总量趋势'
		                },
		                subtitle: {
		                    text: ''
		                },
		                xAxis: {
		                    categories: [<asp:Literal runat="server" ID="LtlFootTxt"></asp:Literal>]
		                },
		                yAxis: {
		                    title: {
		                        text: ''
		                    }
		                },
		                tooltip: {
		                    enabled: false,
		                    formatter: function () {
		                        return '<b>' + this.series.name + '</b><br/>' +
                                    this.x + ': ' + this.y + '';
		                    }
		                },
		                plotOptions: {
		                    line: {
		                        dataLabels: {
		                            enabled: true
		                        },
		                        enableMouseTracking: false
		                    }
		                },
		                series: [{
		                    name: '报修量',
		                    data: [<asp:Literal runat="server" ID="LtlPrecent"></asp:Literal>]
		                }]
		            });
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
                        月报修总量趋势
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        报修月份 从
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxtDateBegin" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM'})"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        至
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEnd" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM'})"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属客户/品牌
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
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
       <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" >
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="月份">
                <ItemTemplate>
                    <%# Eval("s_Month")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修量">
                <ItemTemplate>
                    <%# Eval("s_Count")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
