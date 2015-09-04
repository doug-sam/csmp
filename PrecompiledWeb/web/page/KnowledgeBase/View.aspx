<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_KnowledgeBase_ViewNew, App_Web_is0o3ksp" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td class="td1_2">问
            </td>
            <td class="td1_3" colspan="3">
                <%=GetInfo().Title %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">答
            </td>
            <td class="td1_3" colspan="3">
                <%=GetInfo().Content %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">附件列表
            </td>
            <td class="td1_3">
                <ul class="ulAttachment">
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <li>
                            <a href='/page/sys/DownLoadFile.ashx?ID=<%#Eval("ID") %>' target="_blank"><%#Eval("Memo") %></a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            </td>
        </tr>
        <tr>
            <td class="td1_2">发布于
            </td>
            <td class="td1_3">
                <%=GetInfo().AddDate.ToString("yyyy年MM月dd日 HH:mm") %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">发布人
            </td>
            <td class="td1_3">
                <%=GetInfo().AddByUserName %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">查看
            </td>
            <td class="td1_3">
                <%=GetInfo().ViewCount %>
            </td>
        </tr>

    </table>

</asp:Content>
