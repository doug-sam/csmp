<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="ProfileStep1.aspx.cs" Inherits="system_API_YUM_ProfileStep1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" charset="utf-8" src="/js/kindeditor/kindeditor.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <asp:Panel ID="Panel1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
            <tr>
                <td colspan="4" style="height: 30px" class="td1_1">
                    YUM接口邮件1
                </td>
            </tr>
            <tr>
                <td style="height: 30px" class="td1_2">
                    请保留模板只的关键字。
                    <div>
                        (((((报修时间)))))</div>
                    <div>
                        (((((店铺名)))))</div>
                    <div>
                        (((((店铺号)))))</div>
                    <div>
                        (((((来源单号)))))</div>
                    <div>
                        (((((大类故障)))))</div>
                    <div>
                        (((((中类故障)))))</div>
                    <div>
                        (((((小类故障)))))</div>
                    <div>
                        (((((SLA)))))</div>
                    <div>
                        (((((扩展SLA)))))</div>
                    <div>
                        (((((故障描述)))))</div>
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
                    <asp:Button ID="BtnSubmit" runat="server" Text="  提 交  " CssClass="BigButton" OnClick="BtnSubmit_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
