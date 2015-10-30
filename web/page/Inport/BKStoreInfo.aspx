<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BKStoreInfo.aspx.cs" MasterPageFile="~/Controls/Site1.master"
 Inherits="page_Inport_BKStoreInfo" %>

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
                汉堡王店铺数据导入
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
                    Visible="false" onclick="BtnSubmit_Click"/>&nbsp&nbsp
                <asp:Button ID="BtnPostToBK" runat="server" Text="同步数据到汉堡王转呈系统" 
                    CssClass="BigButton" onclick="BtnPostToBK_Click" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                <asp:CheckBox ID="CbUpdate" Text="将已存在的店铺更新为Excel中的数据" runat="server" />
            </td>
            <td class="td1_3" colspan="2">
                如果你勾选了更新数据，那么请确保你希望更新的数据中。EXCEL及系统中的店铺号高度一至
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
</asp:Content>