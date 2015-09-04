<%@ page title="" language="C#" masterpagefile="~/Controls/Site2.master" autoeventwireup="true" inherits="page_System_StoreCopy, App_Web_buvtbd_s" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="4" style="height: 30px" class="td1_1">
                        店铺转移
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
                        移动品牌
                    </td>
                    <td class="td1_3">
                        <asp:CheckBoxList ID="CbListBrand" DataTextField="Name" DataValueField="ID" runat="server"></asp:CheckBoxList>
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
                </tr>
                <tr>
                    <td class="td1_2" colspan="4">
                        <asp:Button ID="BtnSubmit" runat="server" Text="确定复制（复制后系统没有删除复制品功能！！）"  Width="354px" Height="61px" OnClick="BtnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSubmit" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

