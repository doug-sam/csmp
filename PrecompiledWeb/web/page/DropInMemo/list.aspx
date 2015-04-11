<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_DropInMemo_list, App_Web_bfo9tvhi" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td style="height: 30px" class="td1_1">
                上门过程备注
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle" />
        <HeaderStyle CssClass="GV_HeaderStyle" />
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle" />
        <Columns>
            <asp:TemplateField HeaderText="记录时间">
                <ItemTemplate>
                    <%# Eval("AddDate")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="记录人">
                <ItemTemplate>
                    <%# Eval("UserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详细内容">
                <ItemTemplate>
                    <%# Eval("Details")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                 <asp:LinkButton ID="LbDel" runat="server"  CommandArgument='<%#Eval("ID") %>'
                    onclick="LbDel_Click" OnClientClick="return confirm('确定删除数据？');">删除</asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
        <input type="button" value="添加记录备注" class="BigButton" 
    onclick="javascript:tb_show('添加记录备注', '/page/DropInMemo/Edit.aspx?StepID=<%=Tool.Function.GetRequestInt("StepID") %>&TB_iframe=true&height=400&width=700', false);"/>
</asp:Content>
