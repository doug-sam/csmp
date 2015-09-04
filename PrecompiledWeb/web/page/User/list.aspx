<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_User_list, App_Web_3qpmjjwv" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="6" style="height: 30px" class="td1_1">
                系统帐户管理
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                所属权限组
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlGroup" DataTextField="Name" DataValueField="ID" runat="server">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                所属工作组
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlWorkGroup" DataTextField="Name" DataValueField="ID" runat="server">
                </asp:DropDownList>
            </td>
            <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
                        <asp:Button ID="BtnExport" runat="server" Text="导出搜索结果" CssClass="BigButton" OnClick="BtnExport_Click" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                所属角色
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlRule" DataTextField="Value" DataValueField="Key" runat="server">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                登录名
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbWd" runat="server"></asp:TextBox>
            </td>
            <td class="td1_2">
                是否可用
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlEnable" runat="server">
                    <asp:ListItem Text="不限" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="可用" Value="1"></asp:ListItem>
                    <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' /></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="登录ID">
                <ItemTemplate>
                    <%# Eval("Code")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示名">
                <ItemTemplate>
                    <%# Eval("Name")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="联系电话">
                <ItemTemplate>
                    <%#Eval("Tel")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="电子邮件">
                <ItemTemplate>
                    <%#Eval("Email")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="最后登录时间">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime(Eval("LastDate")).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建时间">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="角色">
                <ItemTemplate>
                   <%#string.Join(",", ((System.Collections.Generic.List<string>)Eval("Rule")).ToArray()) %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="工作组">
                <ItemTemplate>
                    <asp:Label ID="LabWorkGroup" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="是否启用">
                <ItemTemplate>
                    <%#Eval("Enable").ToString().ToLower()=="true"?"启用":"已禁用"%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="邮件联系人">
                <ItemTemplate>
                    <%#Eval("EmailContact").ToString().ToLower()=="true"?"是":"否"%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <a class="link_4" href="javascript:tb_show('编辑帐户', '/page/User/Edit.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
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
        <a class="link_4" href="javascript:tb_show('添加帐户', '/page/User/Edit.aspx?TB_iframe=true&height=450&width=730', false);" >
            添加帐户</a>
        </asp:Literal>
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" 
             Enabled="false" OnClientClick="return confirm('\n\n删除用户将同时删除与该用户相关的任何数据。\n\n建议禁用该用户\n\n你确定删除吗？');" />
    </p>
</asp:Content>
