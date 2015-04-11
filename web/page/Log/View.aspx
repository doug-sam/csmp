<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="View.aspx.cs" Inherits="Log_View" %>
<%@ Import Namespace="CSMP.Model" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <div style="float: left;">
                    查看系统日志 
                </div>
                <div style="float: right; cursor: pointer;">
                    <a href="#" onclick="self.parent.tb_remove();return false;">关闭</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                分类
            </td>
            <td class="td1_3">
                <%= ((LogInfo)ViewState["INFO"]).Category %>
            </td>
            <td class="td1_2">
                引错用户
            </td>
            <td class="td1_3">
                 <%=((LogInfo)ViewState["INFO"]).UserName %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                添加日期
            </td>
            <td class="td1_3">
                <%=((LogInfo)ViewState["INFO"]).AddDate %>
            </td>
            <td class="td1_2">
                引错日期
            </td>
            <td class="td1_3">
                 <%=((LogInfo)ViewState["INFO"]).ErrorDate %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                严重程度
            </td>
            <td class="td1_3">
                <%=((LogInfo)ViewState["INFO"]).Serious %>
            </td>
            <td class="td1_2">
                邮件提醒
            </td>
            <td class="td1_3">
                <%=((LogInfo)ViewState["INFO"]).SendEmail %>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                详细
            </td>
            <td class="td1_3" colspan="3">
                <%=((LogInfo)ViewState["INFO"]).Content %>
            </td>
        </tr>
    </table>
    
</asp:Content>
