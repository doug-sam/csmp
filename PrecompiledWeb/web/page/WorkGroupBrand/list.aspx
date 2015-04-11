<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_WorkGroupBrand_list, App_Web_pjemc3fp" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="5" style="height: 30px" class="td1_1">
                组——品牌关系
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                所属品牌
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                工作组
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlWorkGroup" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnSearch" runat="server" Text="查询" class="button" OnClick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical">
        <RowStyle CssClass="GV_RowStyle"/>
        
        
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' /></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属品牌">
                <ItemTemplate>
                    <%# Eval("MName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="组名">
                <ItemTemplate>
                    <%# Eval("WorkGroupName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px;
        font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
    <p style="padding: 10px;">
        <asp:Literal ID="LtlAdd" runat="server">
            <a href="javascript:tb_show('添加关系', '/page/WorkGroupBrand/add.aspx?TB_iframe=true&height=450&width=730', false);" >设置</a>
        </asp:Literal>
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" />
    </p>
</asp:Content>
