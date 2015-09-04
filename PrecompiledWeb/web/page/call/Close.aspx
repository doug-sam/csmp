<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_Close, App_Web_isgyizrg" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="8" style="height: 30px" class="td1_1">
                服务单回收、报修关闭
            </td>
        </tr>
        <tr>
            <td class="td1_2" colspan="8">
                (店铺信息)
            </td>
        </tr>
        <tr>
            <td class="td1_2" >
               系统单号：
            </td>
            <td class="td1_3">
                <%=info.No %>
            </td>
            <td class="td1_2" >
                报修人：
            </td>
            <td class="td1_3">
                 <%=info.ReporterName %>
            </td>
            <td class="td1_2" >
               客户：
            </td>
            <td class="td1_3">
                 <%=info.CustomerName %>
            </td>
            <td class="td1_2" >
                品牌：
            </td>
            <td class="td1_3">
                <%=info.BrandName %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" >
               省份：
            </td>
            <td class="td1_3">
                <%=info.PriorityName %>
            </td>
            <td class="td1_2" >
                城市：
            </td>
            <td class="td1_3">
                <%=info.CityName %>
            </td>
            <td class="td1_2" >
               店铺名称：
            </td>
            <td class="td1_3">
                <%=info.StoreName %>
            </td>
            <td class="td1_2" >
                店铺编号：
            </td>
            <td class="td1_3">
                <asp:Literal ID="LtlStoreNo" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="td1_2" >
               店铺联系电话：
            </td>
            <td class="td1_3" colspan="3">
                <asp:Literal ID="LtlTel" runat="server"></asp:Literal>
            </td>
            <td class="td1_2" >
                地址：
            </td>
            <td class="td1_3" colspan="3">
                <asp:Literal ID="LtlAddress" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="td1_2" colspan="8">
                (报修信息)
            </td>
        </tr>
        <tr>
            <td class="td1_2" >
               报修来源：
            </td>
            <td class="td1_3">
                <%=info.ReportSourceName %>
            </td>
            <td class="td1_2" >
                来源单号：
            </td>
            <td class="td1_3">
                <%=info.ReportSourceNo %>
            </td>
            <td class="td1_2" >
               报修时间：
            </td>
            <td class="td1_3" >
                <%=info.ErrorDate %>
            </td>
            <td class="td1_2" >
               SLA：
            </td>
            <td class="td1_3" >
                <asp:Label ID="LabSLA" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="td1_2" >
               大类故障：
            </td>
            <td class="td1_3">
                <%=info.ClassName1 %>
            </td>
            <td class="td1_2" >
                中类故障：
            </td>
            <td class="td1_3">
                <%=info.ClassName2 %>
            </td>
            <td class="td1_2" >
               小类故障：
            </td>
            <td class="td1_3">
                <%=info.ClassName3 %>
            </td>
            <td class="td1_2" >
                优先级：
            </td>
            <td class="td1_3">
                <%=info.PriorityName %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" >
               故障描述：
            </td>
            <td class="td1_3" colspan="5">
                <%=info.Details %>
            </td>
            <td class="td1_2" >
                二线（技术中心）负责人：
            </td>
            <td class="td1_3">
                <%=info.MaintaimUserName %> 
            </td>
        </tr>
        <tr>
            <td class="td1_2"  >
                服务单号
            </td>
            <td class="td1_3" colspan="7" >
                <asp:TextBox ID="TxbNo2" CssClass="TxbNo2" runat="server" Width="349px"  onclick="this.select();" Height="23px"></asp:TextBox>
                <asp:CheckBox runat="server" ID="CbClose" CssClass="CbClose" Checked="true" Text="同时关闭报修" onclick="CbClose_Click()"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2" colspan="8" >
                <asp:Button ID="BtnSubmit" runat="server" Text="关闭报修(s)" CssClass="BigButton"  AccessKey="s"
                    onclick="BtnSubmit_Click"  OnClientClick="$(this).hide();return true;" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function CbClose_Click()
        {
            if ($(".CbClose").attr("checked") == true || $(".CbClose").attr("checked") == "true") {
                $(".BigButton").val("回收服务单（先不关单）");
            }
            else {
                $(".BigButton").val("关闭报修");
            }
        }
    </script>
</asp:Content>
