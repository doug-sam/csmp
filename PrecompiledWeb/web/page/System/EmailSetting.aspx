﻿<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="system_EmailSetting, App_Web_u88r9e9r" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <asp:Panel ID="Panel1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
            <tr>
                <td colspan="4" style="height: 30px" class="td1_1">
                    系统邮箱设置
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    host
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbHost" runat="server"></asp:TextBox>
                </td>
                <td class="td1_2">
                    Port
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbPort" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    Email Address
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbAddress" runat="server"></asp:TextBox>
                </td>
                <td class="td1_2">
                    Email Pwd
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbPwd" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    Disply Name
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbDisplayName" runat="server"></asp:TextBox>
                </td>
                <td class="td1_2">
                    Reply Address
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbReplayAddress" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    Reply Name
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbReplayName" runat="server"></asp:TextBox>
                </td>
                <td class="td1_2">
                   
                </td>
                <td class="td1_3">
                    
                </td>
            </tr>
            <tr>
                <td class="td1_1" colspan="4">
                    <asp:Button ID="BtnSubmit" runat="server" Text="提交" OnClick="BtnSubmit_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
