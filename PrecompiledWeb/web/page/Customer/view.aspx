<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Customer_view, App_Web_bawm_hmy" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="2" style="height: 30px" class="td1_1">
                <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>客户
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                名称：
            </td>
            <td class="td1_3">
                <%=((CSMP.Model.CustomersInfo)ViewState["info"]).Name%>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                联系人：
            </td>
            <td class="td1_3">
                <%=((CSMP.Model.CustomersInfo)ViewState["info"]).Contact%>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                联系电话：
            </td>
            <td class="td1_3">
                <%=((CSMP.Model.CustomersInfo)ViewState["info"]).Phone%>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                电子邮件：
            </td>
            <td class="td1_3">
                <%=((CSMP.Model.CustomersInfo)ViewState["info"]).Email%>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                所属城市：
            </td>
            <td class="td1_3">
                <asp:Label ID="LabCity" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                是否使用：
            </td>
            <td class="td1_3">
                <%=((CSMP.Model.CustomersInfo)ViewState["info"]).IsClosed?"已停用":"使用中"%>
            </td>
        </tr>
    </table>
</asp:Content>
