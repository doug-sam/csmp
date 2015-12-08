<%@ Page Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" 
CodeFile="AppMessageList.aspx.cs" Inherits="APPService_AppMessageList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                APP消息提醒
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" >
        <RowStyle CssClass="GV_RowStyle"/>
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="单号">
                <ItemTemplate>
                    <%# Eval("No")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="二线负责人">
                <ItemTemplate>
                    <%# Eval("MaintainUserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="现场工程师">
                <ItemTemplate>
                    <%#Eval("MajorUserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="工作详情">
                <ItemTemplate>
                    <%#Eval("Content")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修详细" Visible="true">
                <ItemTemplate>
                    <a href='/page/call/sch.aspx?CallNo=<%# Eval("No")%>&isRead=1'>
                        <img src="/images/view.gif" /></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
