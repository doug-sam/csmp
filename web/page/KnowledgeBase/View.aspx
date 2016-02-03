<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="View.aspx.cs" Inherits="page_KnowledgeBase_ViewNew" %>

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
                <%=content %>
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
