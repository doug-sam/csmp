<%@ page title="" language="C#" masterpagefile="~/Controls/Site2.master" autoeventwireup="true" inherits="page_System_ClassCopy, App_Web_u88r9e9r" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="4" style="height: 30px" class="td1_1">
                        故障类复制
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属客户
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" DataTextField="Name" DataValueField="ID" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        所属大类
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlClass1" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        转移到目标客户
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlTargetCustomer" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        状态
                    </td>
                    <td class="td1_3">
                        <asp:CheckBox ID="CbNewClassEnable" runat="server" Text="复制后的故障类处理停用状态" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="4">
                        <asp:Button ID="BtnSubmit" runat="server" Text="确定复制（复制后系统没有删除复制品功能！！）"  Width="354px" Height="61px" OnClick="BtnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

