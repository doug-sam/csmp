<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_CustomerRequest_Edit, App_Web_2yeec446" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        新建 报修请求
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺编号：
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabStoreMsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        详细内容：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDetail" TextMode="MultiLine" runat="server" Height="104px" Width="359px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        报修人：
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabErrorReportUser" runat="server" Text="LabErrorReportUser"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                         <asp:Button ID="BtnSubmit" runat="server" Text="提交(a)" CssClass="BigButton" AccessKey="a"
                            OnClick="BtnSubmit_Click" />
                         &nbsp;
                            <asp:Button ID="BtnDisable" runat="server" Text="取消记录" CssClass="BigButton" OnClick="BtnDisable_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSubmit" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
