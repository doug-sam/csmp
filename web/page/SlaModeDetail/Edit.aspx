﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_SlaModeDetail_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>工作周期安排
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">工作周： </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlDayOfWeek" runat="server">
                        </asp:DropDownList>
                    </td>
                <tr>
                    <td class="td1_2">开始时间： </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateStart" runat="server" onclick="WdatePicker({dateFmt:'HH:mm'})"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">结束时间： </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEnd" runat="server" onclick="WdatePicker({dateFmt:'HH:mm'})"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" CssClass="BigButton" OnClick="BtnSubmit_Click" Text="提交" />
                    </td>
                </tr>
            </table>
</asp:Content>
