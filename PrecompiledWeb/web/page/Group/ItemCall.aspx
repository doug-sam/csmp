<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Group_ItemCall, App_Web_xqsjqxmb" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <div style="float: left;">
                      组显示设置
                 </div>
                <div style="float: right; cursor: pointer;">
                    <a href="#" onclick="self.parent.tb_remove();return false;" >关闭</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                组名
            </td>
            <td class="td1_3">
                <asp:Label ID="LabName" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                角色
            </td>
            <td class="td1_3">
                <asp:Label ID="LabRule" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
    <div>需要隐藏的字段</div>
    <ul id="divlist">
        
     <li><input type="checkbox" name="CbItem" value="系统单号"   />系统单号</li>
        <li><input type="checkbox" name="CbItem" value="报修日期" />报修日期</li>
        <li><input type="checkbox" name="CbItem" value="品牌" />品牌</li>
        <li><input type="checkbox" name="CbItem" value="店铺号" />店铺号</li>
        <li><input type="checkbox" name="CbItem" value="中类故障" />中类故障</li>
        <li><input type="checkbox" name="CbItem" value="小类故障" />小类故障</li>
        <li><input type="checkbox" name="CbItem" value="故障描述" />故障描述</li>
        <li><input type="checkbox" name="CbItem" value="一线处理人" />一线处理人</li>
        <li><input type="checkbox" name="CbItem" value="二线处理人" />二线处理人</li>
        <li><input type="checkbox" name="CbItem" value="预约时间/人员" />预约时间/人员</li>
        <li><input type="checkbox" name="CbItem" value="状态" />状态</li>
        <li><input type="checkbox" name="CbItem" value="SLA" />SLA</li>
        <li><input type="checkbox" name="CbItem" value="超时时间" />超时时间</li>
        <li><input type="checkbox" name="CbItem" value="服务类型" />服务类型</li>
    
    </ul>
    <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " OnClick="BtnSubmit_Click" />
<script>
    function CheckItem(ItemName) {
        $("li :input").each(
            function() {
                if (this.value == ItemName) {
                    this.checked = true;
                }
            }
        );
    }
    
</script>
</asp:Content>
