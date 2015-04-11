<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="edit.aspx.cs" Inherits="page_WorkGroup_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Literal ID="LtlAction" runat="server">添加</asp:Literal>工作组
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        组名称：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbName" runat="server" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属类型：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlType" runat="server" DataTextField="value" DataValueField="key">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所在省份：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlProvince" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
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
