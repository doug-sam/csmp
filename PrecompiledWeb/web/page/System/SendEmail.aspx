<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="system_SendEmail, App_Web_buvtbd_s" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" charset="utf-8" src="/js/kindeditor/kindeditor.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="2" style="height: 30px" class="td1_1">
                发邮件
            </td>
        </tr>
        <tr>
            <td   class="td1_2">
                收件人
            </td>
            <td   class="td1_3">
                <asp:TextBox ID="TxbToAddress" runat="server" Width="700px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td   class="td1_2">
                主题
            </td>
            <td   class="td1_3">
                <asp:TextBox ID="TxbSubject" runat="server" Width="700px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td   class="td1_2">
                内容
            </td>
            <td   class="td1_3">
                <asp:TextBox ID="TxbContent" runat="server"></asp:TextBox>
                <script>
                    KE.show({
                        id: '<%=TxbContent.ClientID %>',
                        imageUploadJson: '/js/kindeditor/upload_json.ashx',
                        fileManagerJson: '/js/kindeditor/file_manager_json.ashx',
                        width: '700px',
                        resizeMode: 1,
                        height: '300px'
                    });
                </script>
            </td>
        </tr>
        <tr>
            <td  class="td1_1" colspan="2">
                         
                <asp:Button ID="BtnSubmit" runat="server" Text="发送" onclick="BtnSubmit_Click" CssClass="BigButton" />
                <asp:Label ID="LabError" runat="server" Text="" CssClass="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
