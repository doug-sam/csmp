<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="list.aspx.cs" Inherits="page_Task_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="6" style="height: 30px" class="td1_1">
                        系统定时任务管理
                        <div><h1>非系统管理员不要在这里胡作非为</h1></div>
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
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%# Eval("ID")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="任务名">
                <ItemTemplate>
                    <%# Eval("Name")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="执行时间间隔">
                <ItemTemplate>
                    <%# Eval("IntervalTime")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="执行时间点">
                <ItemTemplate>
                    <%# Eval("ExcuteTime")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="最后执行时间">
                <ItemTemplate>
                    <%# Eval("ExcuteTimeLast")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="周期模式">
                <ItemTemplate>
                    <%# Eval("CycleMode")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="是否生效可用">
                <ItemTemplate>
                    <%# Eval("Enable")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="编辑">
                <ItemTemplate>
                    <a class="link_4" href="javascript:tb_show('编辑', 'Edit.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
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
            <a href="javascript:tb_show('添加', 'Edit.aspx?TB_iframe=true&height=450&width=730', false);" >添加</a>
        </asp:Literal>
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" OnClientClick="return confirm('确定删除数据？')" />
    </p>
</asp:Content>
