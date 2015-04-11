<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_EmailRecord_list, App_Web_lds1kg_9" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/js/thickbox/thickbox.css" rel="stylesheet" type="text/css" />

    <script src="/js/thickbox/thickbox.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="7" style="height: 30px" class="td1_1">
                邮件记录管理 
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                发送人
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlUser" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                日期 从
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
            <td class="td1_2">
                <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
                <asp:Button ID="BtnDeleteCurrent" runat="server" Text="删除当前搜索结果" CssClass="BigButton" OnClick="BtnDeleteCurrent_Click" />
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
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' /></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%#Eval("ID") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="发件人">
                <ItemTemplate>
                    <%#Eval("UserName") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="收件人">
                <ItemTemplate>
                    <%#Eval("ToUser") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="发送时间">
                <ItemTemplate>
                    <%#Eval("DateAdd") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修信息">
                <ItemTemplate>
                   <a href="/page/call/sch.aspx?CallNo=<%#Eval("CallNo") %>">
                       <img src="/images/view.gif" alt="查看报修" />
                   </a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px;
        font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
    <p style="padding: 10px;">
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="BtnDelete_Click" OnClientClick="return confirm('你确定删除吗？');"
          />
    </p>
</asp:Content>
