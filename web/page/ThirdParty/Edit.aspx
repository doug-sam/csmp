﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_ThirdParty_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>第三方
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
                        联系信息：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbContact" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属工作组
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlWorkGroup" DataTextField="Name" DataValueField="ID" 
                            runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        是否使用：
                    </td>
                    <td class="td1_3">
                        <asp:CheckBox ID="CbEnable" Text="使用" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                         <asp:Button ID="BtnSubmit" runat="server" Text="提交" CssClass="BigButton"
                            OnClick="BtnSubmit_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <p style="color:Red">如果配置汉堡王对应的第三方，名称请按照此格式:二级支持名称&$&系统名称，示例：三星银众&$&SUMSUNG</p>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
