<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="page_Attachment_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                附件管理
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                上传时间 从
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateBegin" runat="server" onclick="WdatePicker()"></asp:TextBox>
            </td>
            <td class="td1_2">
                至
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateEnd" runat="server" onclick="WdatePicker()"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  class="td1_2">
                上传者
            </td>
            <td  class="td1_3">
                <asp:TextBox ID="TxbUserName" runat="server"></asp:TextBox>
            </td>
            <td  class="td1_2" colspan="2">
                 <asp:Button ID="BtnSch" runat="server" Text="搜索" onclick="BtnSch_Click" CssClass="BigButton" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' /></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
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
            <asp:TemplateField HeaderText="源数据信息" Visible="true">
                <ItemTemplate>
                    <a href="javascript:tb_show('报修信息查看', '/page/<%#Eval("UseFor")==CSMP.Model.AttachmentInfo.EUserFor.Call.ToString()?"call":"KnowledgeBase" %>/view.aspx?ID=<%#Eval("CallID") %>&TB_iframe=true&height=450&width=730', false);">
                        <img src="/images/view.gif" />
                        <%#Eval("UseFor")%>
                    </a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="文件名">
                <ItemTemplate>
                     <%#Eval("Memo") %>
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
                    <%--<a target="_blank" href='ViewImg.aspx?ID=<%#Eval("ID") %>'><img src="/images/view.gif" /></a>--%>
                    <%--<a target="_blank" href='<%#GetViewUrl(Eval("ID").ToString(),Eval("Title").ToString(),Eval("FilePath").ToString())%>'><img src="/images/view.gif" /></a>--%>
                    <%#GetViewUrl(Eval("ID").ToString(), Eval("Title").ToString(), Eval("FilePath").ToString(), Eval("CallID").ToString())%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px;
        font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
        <p style="padding: 10px;" id="P_Manage" runat="server">

            <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll', 'ckDel')" />全选&nbsp;&nbsp;
        <asp:Button  ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" OnClientClick="return confirm('确定删除数据？')" />
</p>
</asp:Content>
