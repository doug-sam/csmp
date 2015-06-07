<%@ Page Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" 
CodeFile="TrunkNOList.aspx.cs" Inherits="page_TrunkNO_TrunkNOList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="7" style="height: 30px" class="td1_1">
                中继号码管理
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                物理号码
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbPNO" runat="server"></asp:TextBox>
            </td>
            <td class="td1_2">
                400号码
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbVNO" runat="server"></asp:TextBox>
            </td>
            <td class="td1_2">
                描述
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDesc" runat="server"></asp:TextBox>
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnSch" runat="server" Text="搜索" onclick="BtnSch_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" >
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
            <asp:TemplateField HeaderText="物理号码">
                <ItemTemplate>
                    <%# Eval("PhysicalNo")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="400号码">
                <ItemTemplate>
                    <%# Eval("VirtualNo")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="描述">
                <ItemTemplate>
                    <%# Eval("Description")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="编辑">
                <ItemTemplate>
                    <a class="link_4" href="javascript:tb_show('编辑中继号码', '/page/TrunkNO/TrunkNOEdit.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
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
            <a href="javascript:tb_show('添加', '/page/TrunkNO/TrunkNOEdit.aspx?TB_iframe=true&height=450&width=730', false);" >添加</a>
        </asp:Literal>
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button  ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" OnClientClick="return confirm('确定删除数据？')" />
    </p>
</asp:Content>
