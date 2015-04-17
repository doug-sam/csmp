<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="edit.aspx.cs" Inherits="page_User_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Literal ID="LtlAction" runat="server">添加</asp:Literal>帐户
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        登录ID：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbCode" runat="server" CssClass="input1" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        显示名：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbName" runat="server" CssClass="input1" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        密码：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbPwd" runat="server" CssClass="input1" Width="150px"></asp:TextBox>(留空侧不作密码修改)
                    </td>
                </tr>
               <%-- <tr>
                    <td class="td1_2">
                        用户名称：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbName" runat="server" CssClass="input1" Width="150px"></asp:TextBox>
                    </td>
                </tr>--%>
                <tr>
                    <td class="td1_2">
                        联系电话：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbTel" runat="server" CssClass="input1" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        手机号码：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbPhone" runat="server" CssClass="input1" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        电子邮件：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbEmail" runat="server" CssClass="input1" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属工作组：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlWorkGroup" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>(负责不同地区的分组)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        权限组：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlPower" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>(负责它拥有的系统权限，以及担当起什么角色)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        用户性别：
                    </td>
                    <td class="td1_3">
                        <asp:RadioButtonList ID="RblSex" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="男" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="女" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        是否启用：
                    </td>
                    <td class="td1_3">
                        <asp:CheckBox ID="CbEnable" runat="server" Text="禁用" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text="  提交  " 
                            onclick="BtnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
