<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="BrandSolvedBy.aspx.cs" Inherits="page_Report_BrandSolvedBy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                2. 客户/品牌服务方式统计表及占比
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                开始日期
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxtDateBegin" runat="server" class="input3" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td class="td1_2">
                结束日期
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateEnd" runat="server" class="input3" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                品牌
            </td>
            <td class="td1_3">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="td1_2" colspan="2">
                <asp:Button ID="BtnSearch" runat="server" Text="查询" CssClass="BigButton" OnClick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table2" GridLines="Vertical">
        <RowStyle CssClass="GV_RowStyle"/>
        
        
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
    </asp:GridView>
</asp:Content>
