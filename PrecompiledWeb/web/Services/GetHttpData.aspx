<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="Services_GetHttpData, App_Web_1_wqmjdc" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">获取数据
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">URL
                    </td>
                    <td class="td1_2">
                        <asp:TextBox ID="TxbURL" runat="server" Height="36px" Width="780px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">Encode
                    </td>
                    <td class="td1_2">
                        <asp:TextBox ID="TxbEncode" runat="server" Height="36px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnGetData" runat="server" Text="获取" Height="32px" Width="325px" OnClick="BtnGetData_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">Result
                    </td>
                    <td class="td1_2">
                        <asp:TextBox ID="TxbResult" runat="server" Height="306px" Width="780px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
</asp:Content>

