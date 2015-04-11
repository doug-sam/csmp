<%@ Control Language="C#" AutoEventWireup="true" CodeFile="listTrace.ascx.cs" Inherits="page_Trace_listTrace" %>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="7" style="height: 30px" class="td1_1">
                店铺催促记录查看
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
       OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%# Container.DataItemIndex+1%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="系统单号">
                <ItemTemplate>
                    <%# Eval("NO")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修日期">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime( Eval("ErrorDate")).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺号">
                <ItemTemplate>
                    <a href="javascript:tb_show('查看店铺', '/page/Store/View.aspx?ID=<%#Eval("StoreID") %>&TB_iframe=true&height=450&width=730', false);">
                        <%#Eval("StoreName")+"<br/>"+CSMP.BLL.StoresBLL.GetName(Tool.Function.ConverToInt(Eval("StoreID"))) %>
                        </a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="催促日期(最近)">
                <ItemTemplate>
                    <asp:Label ID="LabTraceDate" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="催促人">
                <ItemTemplate>
                    <asp:Label ID="LabMajorUserName" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="内容">
                <ItemTemplate>
                    <asp:Label ID="LabDetails" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修详细" Visible="true">
                <ItemTemplate>
                    <a href='/page/call/sch.aspx?CallNo=<%# Eval("NO")%>'>
                        <img src="/images/view.gif" /></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>