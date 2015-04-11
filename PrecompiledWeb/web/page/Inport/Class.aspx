<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Inport_Class, App_Web_ex7ph01r" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                故障类导入
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                选择文件：
            </td>
            <td class="td1_3">
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnCheck" runat="server" Text="检查数据" CssClass="BigButton" 
                    onclick="BtnCheck_Click"/>
                <asp:Button ID="BtnSubmit" runat="server" Text="开始导入" CssClass="BigButton" 
                    Visible="false" onclick="BtnSubmit_Click"/>
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
</asp:Content>
