<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Replacement_List, App_Web_nt_lq0xu" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                备件跟踪记录
                <div>
                    <span style="width:30px;height:12px;color:green;"> 绿色</span>代表备件跟进结束
                </div>
            </td>
        </tr>
    </table>

    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false" AllowSorting="true"
         OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle" />
        <HeaderStyle CssClass="GV_HeaderStyle" />
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle" />
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%#Container.DataItemIndex+1 %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="备件" >
                <ItemTemplate>
                    <%#Eval("RpSerialNo") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期">
                <ItemTemplate>
                    <%#Eval("DateAction") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详细">
                <ItemTemplate>
                    <%#Eval("Detail") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="负责人">
                <ItemTemplate>
                    <%#Eval("MaintainUserName") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="记录人">
                <ItemTemplate>
                    <%#Eval("RecordUserName") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="系统记录时间">
                <ItemTemplate>
                    <%#Eval("DateAdd") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
        <asp:Panel ID="PanelNoData" Visible="false" style="text-align:center;font-size:22px;line-height:32px;font-weight:700;"
             runat="server">
            亲，目前还没有任何备件记录。
        </asp:Panel>

    <p style="padding: 10px;">
        <a href="javascript:tb_show('添加', 'Edit.aspx?ID=<%=GetInfo().ID %>&TB_iframe=true&height=400&width=730', false);">
            添加</a> 
    </p>
</asp:Content>
