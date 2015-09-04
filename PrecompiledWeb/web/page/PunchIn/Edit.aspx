<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_PunchIn_Edit, App_Web_llajh7jc" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <asp:Literal ID="LtlAction" runat="server" Text="编辑"></asp:Literal>考勤
            </td>
        </tr>
        <tr>
            <td class="td1_2">员工
            </td>
            <td class="td1_3">
                <%=GetInfo().UserName %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">操作
            </td>
            <td class="td1_3">
                <%=GetInfo().IsStartWork==1?"上班":"下班" %>打卡
            </td>
        </tr>
        <tr>
            <td class="td1_2">打卡时间
            </td>
            <td class="td1_3">
                <%=GetInfo().DateAdd %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">时间修正
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateRegisterAbs" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">使用设备
            </td>
            <td class="td1_3">
                <%=GetInfo().Device %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">备注
            </td>
            <td class="td1_3">
                <%=GetInfo().Memo %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">所在位置
            </td>
            <td class="td1_3">
                <div>
                    地址： <%=GetInfo().PositionAddress %>
                </div>
                <div>
                    经度： <%=GetInfo().PositionLog %>
                </div>
                <div>
                    纬度： <%=GetInfo().PositionLat %>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">备注详细<br />
                (你可以核对其常用设备。是否为非法登录)
            </td>
            <td class="td1_3">
                <%=GetInfo().DeviceDetail %>
            </td>
        </tr>
    </table>
    <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " OnClick="BtnSubmit_Click" />
</asp:Content>
