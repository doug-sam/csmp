<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="list.aspx.cs" Inherits="page_WorkGroup_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="5" style="height: 30px" class="td1_1">
                工作组管理
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                所在省份
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlProvince" DataTextField="Name" DataValueField="ID" runat="server">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                类别
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlType" DataTextField="value" DataValueField="key" runat="server">
                </asp:DropDownList>
            </td>
            <td class="td1_2" >
                        <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
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
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%# Eval("ID")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="组名">
                <ItemTemplate>
                    <%# Eval("Name")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所在省份">
                <ItemTemplate>
                    <%# Eval("ProvinceName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <a class="link_4" href="javascript:tb_show('编辑', '/page/WorkGroup/Edit.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
                        编辑</a>
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
        <a class="link_4" href="javascript:tb_show('添加工作组', '/page/WorkGroup/Edit.aspx?TB_iframe=true&height=450&width=730', false);" >
            添加工作组</a>
        </asp:Literal>
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" />
    </p>
</asp:Content>
