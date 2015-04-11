<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="system_API_Message_Setting, App_Web_n1imhwjb" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css">
    .ulMainKey{ margin:5px; padding:5px;}
    .ulMainKey li{ line-height:22px; margin:4px; padding:3px; border:1px solid #CCC; float:left;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <asp:Panel ID="Panel1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
            <tr>
                <td colspan="4" style="height: 30px" class="td1_1">
                    系统短信设置
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    host
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbHost" runat="server"></asp:TextBox>
                </td>
                <td class="td1_2">
                    Port
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbPort" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    accountId
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbAccountId" runat="server"></asp:TextBox>
                </td>
                <td class="td1_2">
                    password
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbPwd" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    serviceId
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbServiceId" runat="server"></asp:TextBox>
                </td>
                <td class="td1_2">
                    总开关
                </td>
                <td class="td1_3">
                    <asp:CheckBox ID="CbEnable" runat="server" Text="启用" />
                </td>
            </tr>
            <tr>
                <td class="td1_1" colspan="4">
                    <asp:Button ID="BtnSubmit" runat="server" Text="提交" OnClick="BtnSubmit_Click" />
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
            <tr>
                <td  style="height: 30px" class="td1_1">
                    系统短信模板
                </td>
            </tr>
            <tr>
                <td class="td1_3">
                   需要的关键字请保留：
                   <ul class="ulMainKey">
                       <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <li>(((((<%#GetDataItem() %>)))))</li>
                        </ItemTemplate>
                       </asp:Repeater>
                   </ul>
                </td>
            </tr>
            <tr>
                <td class="td1_3">
                    <asp:TextBox ID="TxbTempLate1" runat="server" Height="77px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td1_1">
                    <asp:Button ID="BtnTempLate1" runat="server" Text="提交" 
                        onclick="BtnTempLate1_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
