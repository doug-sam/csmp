<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="Controls_View" %>
<%@ Import Namespace="CSMP.BLL" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
    <tr>
        <td colspan="8" style="height: 30px" class="td1_1">
            报修信息查看
        </td>
    </tr>
    <tr>
        <td class="td1_2" colspan="8">
            (店铺信息)
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            系统单号：
        </td>
        <td class="td1_3">
            <%=info.No %>
        </td>
        <td class="td1_2">
            报修人：
        </td>
        <td class="td1_3">
            <%=info.ReporterName %>
        </td>
        <td class="td1_2">
            客户：
        </td>
        <td class="td1_3">
            <%=info.CustomerName %>
        </td>
        <td class="td1_2">
            品牌：
        </td>
        <td class="td1_3">
            <%=info.BrandName %>
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            省份：
        </td>
        <td class="td1_3">
            <%=info.ProvinceName %>
        </td>
        <td class="td1_2">
            城市：
        </td>
        <td class="td1_3">
            <%=info.CityName %>
        </td>
        <td class="td1_2">
            店铺名称：
        </td>
        <td class="td1_3">
            <%=info.StoreNo %>
        </td>
        <td class="td1_2">
            店铺编号：
        </td>
        <td class="td1_3">
            <%=info.StoreName %>
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            店铺联系电话：
        </td>
        <td class="td1_3" colspan="3">
            <asp:Literal ID="LtlTel" runat="server"></asp:Literal>
        </td>
        <td class="td1_2">
            店铺类型：
        </td>
        <td class="td1_3" colspan="3">
            <asp:Literal ID="LtlStoreType" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            地址：
        </td>
        <td class="td1_3" colspan="7">
            <asp:Literal ID="LtlAddress" runat="server"></asp:Literal>
        </td>
        <%--<td class="td1_2">
            地址：
        </td>
        <td class="td1_3" colspan="3">
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </td>--%>
    </tr>
    <tr>
        <td class="td1_2" colspan="8">
            (报修信息)
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            报修来源：
        </td>
        <td class="td1_3">
            <%=info.ReportSourceName %>
        </td>
        <td class="td1_2">
            来源单号：
        </td>
        <td class="td1_3">
            <%=info.ReportSourceNo %>
        </td>
        <td class="td1_2">
            报修时间：
        </td>
        <td class="td1_3">
            <%=info.ErrorDate.ToString("yyyy-MM-dd HH:mm") %>
        </td>
        <td class="td1_2">
            SLA：
        </td>
        <td class="td1_3">
            <%=info.SLA %>
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            大类故障：
        </td>
        <td class="td1_3">
            <%=info.ClassName1 %>
        </td>
        <td class="td1_2">
            中类故障：
        </td>
        <td class="td1_3">
            <%=info.ClassName2 %>
        </td>
        <td class="td1_2">
            小类故障：
        </td>
        <td class="td1_3">
            <%=info.ClassName3 %>
        </td>
        <td class="td1_2">
            优先级：
        </td>
        <td class="td1_3">
            <%=info.PriorityName %>
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            故障描述：
        </td>
        <td class="td1_3" colspan="7">
            <%=info.Details %>
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            二线（技术中心）负责人：
        </td>
        <td class="td1_3">
            <%=info.MaintaimUserName %>
        </td>        
        <td class="td1_2">
            报修创建人：
        </td>
        <td class="td1_3">
            <%=info.CreatorName %>
        </td>
        <td class="td1_2">
            外部服务单号：
        </td>
        <td class="td1_3" colspan="3">
            <%=info.CallNo3 %>
        </td>
    </tr>
    <tr>
        <td class="td1_2">
            服务类型：
        </td>
        <td class="td1_3">
            <%=CallCategoryBLL.Get(info.Category).Name %>
        </td>
        <td class="td1_2">
            SLA扩展：
        </td>
        <td class="td1_3" colspan="5">
            <%=info.SLA2 %>
        </td>        
        
    </tr>
     <tr id="tr_Edit" runat="server" >
        <td class="td1_2" colspan="8">
                <a href="javascript:tb_show('编辑报修', '/page/call/Edit.aspx?ID=<%=info.ID %>&TB_iframe=true&height=380&width=720&modal=false', false);">
            编辑</a>
        </td>
    </tr>
    
</table>
