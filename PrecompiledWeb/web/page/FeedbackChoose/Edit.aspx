<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_FeedbackChoose_Edit, App_Web_p3qykvrf" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <div style="float: left;">
                       <asp:Literal ID="LtlAction" runat="server" Text="编辑"></asp:Literal> 
                 </div>
                <div style="float: right; cursor: pointer;">
                    <a href="#" onclick="self.parent.tb_remove();return false;" >关闭</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                选项
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                OrderNumber
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbOrderNumber" runat="server" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                是否启用
            </td>
            <td class="td1_3">
                <asp:CheckBox ID="CbEnable" runat="server" Text="启用" Checked="true" />
            </td>
        </tr>
    </table>
    <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " OnClick="BtnSubmit_Click" />

</asp:Content>
