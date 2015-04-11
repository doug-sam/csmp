<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Pwd.aspx.cs" Inherits="page_User_Pwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="2" style="height: 30px" class="td1_1">
                修改密码
            </td>
        </tr>
                <tr>
                    <td class="td1_2">
                        旧密码：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbOld" runat="server"  Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        新密码：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbNew1" runat="server"  Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        重复新密码：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbNew2" runat="server"  Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " 
                            onclick="BtnSubmit_Click" />
                    </td>
                </tr>
    </table>
</asp:Content>
