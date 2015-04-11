<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Attachment_View, App_Web_d5yo-v8s" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                      附件列表
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%#Eval("ID") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="上传者">
                <ItemTemplate>
                    <%#Eval("UserName") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修信息" Visible="true">
                <ItemTemplate>
                    <a href="javascript:tb_show('报修信息查看', '/page/call/view.aspx?ID=<%#Eval("CallID") %>&TB_iframe=true&height=450&width=730', false);">
                        报修信息</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="文件名">
                <ItemTemplate>
                     <%#Eval("Title") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="扩展名">
                <ItemTemplate>
                     <%#Eval("Ext") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="文件类型">
                <ItemTemplate>
                     <%#Eval("ContentType") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="文件大小">
                <ItemTemplate>
                     <%#Eval("FileSize") %>B
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="添加时间">
                <ItemTemplate>
                     <%#Eval("Addtime") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="下载">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgBtnDownLoad" ImageUrl="/images/up.gif"
                         runat="server" CommandArgument='<%#Eval("ID") %>' OnClick="ImgBtnDownLoad_Click"/>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="查看">
                <ItemTemplate>
                    <a target="_blank" href='ViewImg.aspx?ID=<%#Eval("ID") %>'><img src="/images/view.gif" /></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
              
    <div class="NoData"  <%=GridView1.Rows.Count==0?"":"style='display:none;'" %>  >木有附件</div>
</asp:Content>
