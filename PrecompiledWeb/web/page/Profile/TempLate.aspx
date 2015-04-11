<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Profile_TempLate, App_Web_mpyqhhy8" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" charset="utf-8" src="/js/kindeditor/kindeditor.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td style="height: 30px" class="td1_1">
                邮件模板设置
            </td>
        </tr>
        <tr>
            <td style="height: 30px" class="td1_2">
                请保留模板只的关键字。
                <div>(((((对方名称)))))</div>
                <div>(((((店铺名)))))</div>
                <div>(((((小类故障)))))</div>
                <div>(((((预约上门时间)))))</div>
                <div>(((((备注工作说明)))))</div>
                <div>(((((二线人员)))))</div>
                <div>(((((二线电话)))))</div>
                <div>(((((二线电话)))))</div>
                <div>(((((当前登录用户)))))</div>
                <div>(((((当前登录用户邮箱)))))</div>

            </td>
        </tr>
        <tr>
            <td class="td1_3">
                <asp:TextBox ID="TxbContact" runat="server"></asp:TextBox>

                <script>
                    KE.show({
                        id: '<%=TxbContact.ClientID %>',
                        imageUploadJson: '/js/kindeditor/upload_json.ashx',
                        fileManagerJson: '/js/kindeditor/file_manager_json.ashx',
                        width: '700px',
                        resizeMode: 1,
                        height: '320px'
                    });
                </script>

            </td>
        </tr>
        <tr>
            <td class="td1_2">
                <asp:Button ID="BtnCallRecCount" runat="server" Text="  提 交  " 
                    CssClass="BigButton" onclick="BtnCallRecCount_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
