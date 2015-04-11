<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Send.aspx.cs" Inherits="system_API_Message_Send" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        系统短信发送
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        对方手机号(多个号码请用英文分号";"隔开)
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbTel" runat="server" Width="582px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        短信内容
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbContent" TextMode="MultiLine" runat="server" Height="88px" Width="582px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="4">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交" OnClick="BtnSubmit_Click" CssClass="BigButton" />
                        <asp:Label ID="LabResult" runat="server" Text="" CssClass="red2"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
