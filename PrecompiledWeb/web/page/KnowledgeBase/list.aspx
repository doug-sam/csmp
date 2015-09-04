<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_KnowledgeBase_list, App_Web_is0o3ksp" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="5" style="height: 30px" class="td1_1">知识库管理 
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">所属客户
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">所属品牌
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td class="td1_2">关键词
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbLabs" runat="server"></asp:TextBox>
                    </td>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSch" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle" />
        <HeaderStyle CssClass="GV_HeaderStyle" />
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle" />
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%#Eval("ID") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="问题">
                <ItemTemplate>
                    <%#Eval("Title") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="回答描述">
                <ItemTemplate>
                    <%#Tool.Function.Cuter(Tool.Function.RemoveHTML(Eval("Content")),50) %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="附件">
                <ItemTemplate>
                    <asp:Image ID="ImgHaveAttachment" ImageUrl="/images/Up.gif" Visible="false" runat="server" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="浏览数">
                <ItemTemplate>
                    <%#Eval("ViewCount") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="查看">
                <ItemTemplate>
                    <a href="#" onclick='tb_show("", "View.aspx?ID=<%# Eval("ID")%>&TB_iframe=true&height=400&width=800&modal=false", false); return false;'>查看</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="添加日期">
                <ItemTemplate>
                    <%#Eval("AddDate") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="添加人">
                <ItemTemplate>
                    <%#Eval("AddByUserName") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="支持数">
                <ItemTemplate>
                    <%#Eval("GoodCount") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="详细资料">
                <ItemTemplate>
                    <a href="#" onclick='tb_show("", "Edit.aspx?ID=<%# Eval("ID")%>&TB_iframe=true&height=400&width=800&modal=false", false); return false;'>编辑</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px; font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
    <p style="padding: 10px;">
        <asp:Literal ID="LtlAdd" runat="server">    
            <a href="javascript:tb_show('添加', 'Edit.aspx?TB_iframe=true&height=450&width=730', false);" >添加</a>
        </asp:Literal>
        <asp:Panel ID="LtlDelete" runat="server">
            <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll', 'ckDel')" />全选&nbsp;&nbsp;
            <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="BtnDelete_Click" OnClientClick="return confirm('\n\n删除组后\n\n\n组内的员工帐号将不拥有任何权限\n\n\n\n你确定删除吗？');" />
        </asp:Panel>
    </p>
</asp:Content>
