<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_callStep_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="4" style="height: 30px" class="td1_1">
                        处理记录编辑
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        步骤类型
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabStepType" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="td1_2">
                        记录时间
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabAddDate" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        <asp:Label ID="LabDetail" runat="server" Text="JobCode"></asp:Label>
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:TextBox ID="TxbDetail" runat="server"
                            Width="526px" Height="72px" TextMode="MultiLine" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="4">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交" CssClass="BigButton" OnClick="BtnSubmit_Click"
                            AccessKey="s" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
