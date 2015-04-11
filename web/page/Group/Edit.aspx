<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_Group_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <div style="float: left;">
                       <asp:Literal ID="LtlAction" runat="server" Text="编辑"></asp:Literal>组
                 </div>
                <div style="float: right; cursor: pointer;">
                    <a href="#" onclick="self.parent.tb_remove();return false;" >关闭</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                组名
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbTitle" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                角色
            </td>
            <td class="td1_3">
                <asp:CheckBoxList ID="CblRule" runat="server" DataTextField="value" DataValueField="value">
                </asp:CheckBoxList>
                (角色不制定权限，它只作用于身份标识。权限请在《权限设置》中编辑)
            </td>
        </tr>
    </table>
    <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " OnClick="BtnSubmit_Click" />
</asp:Content>
