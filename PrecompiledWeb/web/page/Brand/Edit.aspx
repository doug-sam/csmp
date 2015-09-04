<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Brand_Edit, App_Web_ojyef7yu" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>品牌
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        名称：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属客户：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" runat="server"
                         DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="td1_2">
                        项目经理：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlUser" runat="server"
                         DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        联系人：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbContact" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        联系电话：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbPhone" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        电子邮件：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbEmail" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        sla模型：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlSlaMode" runat="server"
                         DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        超sla超时第一次倒计：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbSlaTimer1" placeHolder="30" runat="server"></asp:TextBox>分钟
                        (若不想触发，请输入0)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        超sla超时第二次倒计：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbSlaTimer2" placeHolder="10" runat="server"></asp:TextBox>分钟
                        (若不想触发，请输入0)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        超时收件人：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbSlaTimerTo" placeHolder="xx@mvs.com;yy@mvs.com.cn" TextMode="MultiLine" runat="server" Height="103px" Width="362px"></asp:TextBox>
                        <div>
                            输入邮箱。多个邮箱之间用英文分号隔开
                            <br />
                            如果你不认真地填写邮箱地址进去。请不要填写上边那个sla倒计！！
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        是否使用：
                    </td>
                    <td class="td1_3">
                        <asp:RadioButtonList ID="RblIsClose" runat="server">
                            <asp:ListItem Selected="True" Text="使用" Value="1"></asp:ListItem>
                            <asp:ListItem Text="停用" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
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
