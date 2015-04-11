<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_City_list, App_Web_pqpbxiaz" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/js/thickbox/thickbox.css" rel="stylesheet" type="text/css" />

    <script src="/js/thickbox/thickbox.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="5" style="height: 30px" class="td1_1">
                城市管理 
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                所属省份
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlProvince" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                城市名
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbName" runat="server"></asp:TextBox>
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
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
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%#Eval("ID") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <%#Eval("Name") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属省份">
                <ItemTemplate>
                    <asp:Literal ID="LtlProvince" runat="server"></asp:Literal>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="是否启用">
                <ItemTemplate>
                    <%#Eval("IsClosed").ToString().ToLower()=="false"?"使用中":"禁用"%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详细资料">
                <ItemTemplate>
                    <a href="#" onclick='tb_show("", "Edit.aspx?ID=<%# Eval("ID")%>&TB_iframe=true&height=400&width=800&modal=false", false); return false;'>
                        <img src="/images/edit.gif" /></a>
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
        <a href="javascript:tb_show('添加城市', '/page/City/Edit.aspx?&TB_iframe=true&height=450&width=730', false);">
            添加城市</a> &nbsp;&nbsp;
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="BtnDelete_Click" OnClientClick="return confirm('\n\n删除组后\n\n\n组内的员工帐号将不拥有任何权限\n\n\n\n你确定删除吗？');"
          />
    </p>
</asp:Content>
