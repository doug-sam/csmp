<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Trace_Trace, App_Web_6kfoqsrt" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        店铺催报修
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        催促时间
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDate" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                            Width="193px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        催促人
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbName" runat="server"
                            Width="193px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        备注
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDetail" runat="server"
                            Width="193px" Height="72px" TextMode="MultiLine" Text="客户催促"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交" Style="padding: 7px 14px;" OnClick="BtnSubmit_Click"
                            AccessKey="s" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
