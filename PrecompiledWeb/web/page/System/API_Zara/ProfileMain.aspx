<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="system_API_Zara_ProfileMain, App_Web_gxnujlrj" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <asp:Panel ID="Panel1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
            <tr>
                <td colspan="4" style="height: 30px" class="td1_1">
                    Zara邮件设置
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    接口开关
                </td>
                <td class="td1_3">
                    <asp:CheckBox ID="CbEnable" Text="启用" runat="server" />
                </td>
                <td class="td1_2">
                    <asp:Button ID="BtnEnable" runat="server" Text="提交" onclick="BtnEnable_Click" />
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    Zara品牌在系统中的ID
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbID" runat="server"></asp:TextBox>
                    多个id用&quot;|&quot;隔开</td>
                <td class="td1_2">
                    <asp:Button ID="BtnID" runat="server" Text="提交" onclick="BtnID_Click"/>
                </td>
            </tr>
        </table>
        
    </asp:Panel>
</asp:Content>
