<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="page_Group_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/js/thickbox/thickbox.css" rel="stylesheet" type="text/css" />

    <script src="/js/thickbox/thickbox.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                组管理
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
            <td class="td1_2" >
                        <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical"  OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        
        
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' /></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                   <%#Eval("ID") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="组名">
                <ItemTemplate>
                   <%#Eval("Name") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="角色">
                <ItemTemplate>
                   <%#string.Join(",", ((System.Collections.Generic.List<string>)Eval("Rule")).ToArray()) %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详细资料">
                <ItemTemplate>
                     <a href="#"  onclick='tb_show("", "Edit.aspx?ID=<%# Eval("ID")%>&TB_iframe=true&height=400&width=800&modal=false", false); return false;'>
                        <img src="/images/edit.gif" /></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="权限设置">
                <ItemTemplate>
                     <a href="/page/Power/List.aspx?ID=<%# Eval("ID")%>">                    
                        <img src="/images/up.gif" /></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示设置">
                <ItemTemplate>
                     <a href="/page/Group/ItemDefault.aspx?ID=<%# Eval("ID")%>">                    
                        <img src="/images/play.gif" /></a>
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
<a href="javascript:tb_show('添加组', '/page/group/Edit.aspx?&TB_iframe=true&height=450&width=730', false);">
添加</a> &nbsp;&nbsp;
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="BtnDelete_Click"
         OnClientClick="return confirm('\n\n删除组后\n\n\n组内的员工帐号将不拥有任何权限\n\n\n\n你确定删除吗？');" Visible="false"  />
    </p>

</asp:Content>
