<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_Store_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>店铺
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺编号：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbNO" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺名称：
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
                         DataTextField="Name" DataValueField="ID" AutoPostBack="True" 
                            onselectedindexchanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属品牌：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlBrand" runat="server"
                         DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属地区：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlProvince" runat="server"
                         DataTextField="Name" DataValueField="ID" AutoPostBack="True" 
                            onselectedindexchanged="DdlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlCity" runat="server"
                         DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        地址：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbAddress" runat="server" Width="359px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        电话：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbTel" runat="server"></asp:TextBox>(多个电话请用/分隔，区号与号码间不要用任何符号分隔)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        邮箱：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbEmail" runat="server"></asp:TextBox>
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
