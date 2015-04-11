<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_Customer_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>客户
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        名称：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbName" runat="server"></asp:TextBox>
                        （不能带有- / \ ' "" _ 等特殊符号，建议使用26字母、数字、常见汉字）
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
                        所属城市：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlProvince" runat="server" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged"
                            DataTextField="Name" DataValueField="ID" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlCity" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
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
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:HyperLink ID="HlInitClass" Visible="false" runat="server">初始化该客户大中小类的项目故障类</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
