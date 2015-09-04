<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_FeedbackChoose_list, App_Web_p3qykvrf" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/js/thickbox/thickbox.css" rel="stylesheet" type="text/css" />

    <script src="/js/thickbox/thickbox.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                       管理问题选项
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                问题
            </td>
            <td class="td1_3">
                <asp:Label ID="LabName" runat="server" Text=""></asp:Label>
            </td>
            <td class="td1_2">
                类型
            </td>
            <td class="td1_3">
                <asp:Label ID="LabType" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                选项
            </td>
            <td class="td1_3" colspan="3">
                    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        
        
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
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
            <asp:TemplateField HeaderText="是否启用">
                <ItemTemplate>
                    <%#Eval("Enable").ToString().ToLower()=="true"?"使用中":"禁用"%>
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
            </td>
        </tr>
    </table>

    <p style="padding: 10px;">
        <a href="javascript:tb_show('添加', '/page/FeedbackChoose/Add.aspx?QuestionID=<%=((CSMP.Model.FeedbackQuestionInfo)ViewState["INFO"]).ID %>&TB_iframe=true&height=450&width=730', false);">
            添加</a> &nbsp;&nbsp;
        </p>
</asp:Content>
