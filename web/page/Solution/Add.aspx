﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Add.aspx.cs" Inherits="page_Solution_Add" %>

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
                        <asp:DropDownList ID="DdlCustomer" DataTextField="name" DataValueField="id"  runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属大类
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlClass1" DataTextField="name" DataValueField="id"  runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlClass1_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属中类
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlClass2" DataTextField="name" DataValueField="id"  runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlClass2_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属小类
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlClass3" DataTextField="name" DataValueField="id"  runat="server"></asp:DropDownList>
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
