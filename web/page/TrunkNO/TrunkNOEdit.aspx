<%@ Page Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
 CodeFile="TrunkNOEdit.aspx.cs" Inherits="page_TrunkNO_TrunkNOEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>中继号码信息
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        物理号码：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbPNO" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        400号码：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbVNO" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        描述：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDesc" runat="server"></asp:TextBox>
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