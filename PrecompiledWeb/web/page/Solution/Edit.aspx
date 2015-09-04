<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Solution_Edit, App_Web_dagnzctq" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="编辑"></asp:Label>解决方案
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属客户
                    </td>
                    <td class="td1_3">
                        <asp:Literal ID="LCustomer" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属大类
                    </td>
                    <td class="td1_3">
                        <asp:Literal ID="Lc1" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属中类
                    </td>
                    <td class="td1_3">
                        <asp:Literal ID="Lc2" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属小类
                    </td>
                    <td class="td1_3">
                        <asp:Literal ID="Lc3" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        方案名：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbName" runat="server" Width="359px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        解决次数：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbCount" runat="server"></asp:TextBox>(请输入整数)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                         <asp:Button ID="BtnSubmit" runat="server" Text="提交" CssClass="BigButton"
                            OnClick="BtnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
