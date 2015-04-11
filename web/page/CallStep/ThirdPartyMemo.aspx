<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="ThirdPartyMemo.aspx.cs" Inherits="page_ThirdPartyMemo_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        添加操作记录
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        (事件)时间：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbMemoDate" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        详细信息：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDetails" runat="server" TextMode="MultiLine" Height="147px" 
                            Width="500px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                         <asp:Button ID="BtnSubmit" runat="server" Text="提交" CssClass="BigButton"
                            OnClick="BtnSubmit_Click" />
                    </td>
                </tr>
            </table>
</asp:Content>
