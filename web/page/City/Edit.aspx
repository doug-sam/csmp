<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_City_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <div style="float: left;">
                       <asp:Literal ID="LtlAction" runat="server" Text="编辑"></asp:Literal>城市
                 </div>
                <div style="float: right; cursor: pointer;">
                    <a href="#" onclick="self.parent.tb_remove();return false;" >关闭</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                名称
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbTitle" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                所属省份
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlProvince" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                是否启用
            </td>
            <td class="td1_3">
                <asp:CheckBox ID="CbEnable" runat="server" Text="启用" Checked="true" />
            </td>
        </tr>
    </table>
    <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " OnClick="BtnSubmit_Click" />
</asp:Content>
