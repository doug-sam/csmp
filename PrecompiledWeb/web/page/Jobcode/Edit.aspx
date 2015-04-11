<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_JobCode_Edit, App_Web_ih92qwbu" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>Jobcode
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属工作组：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlWorkGroup" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        Jobcode值：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbCodeNo" runat="server" replaceholder="A1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        实际金额：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbMoney" runat="server" replaceholder="250"></asp:TextBox>元
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        响应时间：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbTimeAction" runat="server" replaceholder="工作时间2小时"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        到场工作时间：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbTimeArrive" runat="server" replaceholder="2"></asp:TextBox>小时
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交" CssClass="BigButton" OnClick="BtnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
