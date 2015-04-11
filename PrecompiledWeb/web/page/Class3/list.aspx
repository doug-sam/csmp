<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Class3_list, App_Web_pw8llrzh" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="6" style="height: 30px" class="td1_1">
                        小类管理
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属客户
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" DataTextField="Name" DataValueField="ID" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        所属大类
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlClass1" DataTextField="Name" DataValueField="ID" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlClass1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        所属中类
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlClass2" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        小类名
                    </td>
                    <td class="td1_3" colspan="4">
                        <asp:TextBox ID="TxbWd" runat="server" Width="251px"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        <asp:Button ID="BtnSch" CssClass="BigButton" runat="server" Text="搜索" OnClick="BtnSch_Click" />
                        <asp:Button ID="BtnExport" CssClass="BigButton" runat="server" Text="导出当前结果" OnClick="BtnExport_Click"  />
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
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%# Eval("ID")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属客户">
                <ItemTemplate>
                    <asp:Label ID="LabCustomer" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属大类">
                <ItemTemplate>
                    <asp:Label ID="LabClass1" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属中类">
                <ItemTemplate>
                    <%# Eval("Class2Name")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="小类名">
                <ItemTemplate>
                    <%# Eval("Name")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="SLA">
                <ItemTemplate>
                    <%# Eval("SLA")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="优先级">
                <ItemTemplate>
                    <%# Eval("PriorityName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="是否禁用">
                <ItemTemplate>
                    <%#Eval("IsClosed").ToString().ToLower()=="true"?"禁用":"启用"%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="编辑">
                <ItemTemplate>
                    <a class="link_4" href="javascript:tb_show('编辑小类故障', '/page/Class3/Edit.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
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
        <asp:Literal ID="LtlAdd" runat="server">
            <a href="javascript:tb_show('添加', '/page/class3/Edit.aspx?TB_iframe=true&height=450&width=730', false);" >添加</a>
        </asp:Literal>
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" OnClientClick="return confirm('确定删除数据？')" />
    </p>
</asp:Content>
