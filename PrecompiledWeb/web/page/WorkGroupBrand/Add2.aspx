<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_WorkGroupBrand_Add2, App_Web_lds8_1je" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td class="td1_2">
                        所属工作组：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlProvince" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" 
                            onselectedindexchanged="DdlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlWorkGroup" runat="server" DataTextField="Name" 
                            DataValueField="ID" AutoPostBack="True" 
                            onselectedindexchanged="DdlWorkGroup_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        客户：
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_3">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <div>
                                        不属于该品牌的组</div>
                                    <asp:ListBox ID="LbBrandLeft" runat="server" Width="150px" Height="300px" SelectionMode="Multiple"
                                        DataTextField="Name" DataValueField="ID"></asp:ListBox>
                                </td>
                                <td>
                                    <asp:Button ID="BtnAdd" runat="server" Text="&gt;&gt;" OnClick="BtnAdd_Click" />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Button ID="BtnDel" runat="server" Text="&lt;&lt;" OnClick="BtnDel_Click" />
                                </td>
                                <td>
                                    <div>
                                        属于该品牌的组</div>
                                    <asp:ListBox ID="LbBrandRight" runat="server" Width="150px" Height="300px" SelectionMode="Multiple"
                                        DataTextField="Name" DataValueField="ID"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text=" 保存 " OnClick="BtnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <input id="btnCancel" type="button" value="取消" class="button" onclick="parent.tb_remove();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
