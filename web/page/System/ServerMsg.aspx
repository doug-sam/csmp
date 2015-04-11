<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="ServerMsg.aspx.cs" Inherits="system_ServerMsg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="2" style="height: 30px" class="td1_1">
                服务器信息
            </td>
        </tr>  
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">服务器名称</td>
             <td class="td1_3">
                 <asp:Label ID="Label1" runat="server" Text="Label">
                <%=Server.MachineName%></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">服务器IP</td>
             <td class="td1_3">
                 <asp:Label ID="Label2" runat="server" Text="Label">
                <%=Request.ServerVariables["LOCAL_ADDR"] %></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">服务器域名</td>
             <td class="td1_3">
                <%=Request.ServerVariables["SERVER_NAME"] %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">".NET解释引擎版本</td>
             <td class="td1_3">
                <%=".NET CLR" + Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision%>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">服务器操作系统版本</td>
             <td class="td1_3">
                <%=Environment.OSVersion%>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">服务器IIS版本</td>
             <td class="td1_3">
                <%=Request.ServerVariables["SERVER_SOFTWARE"] %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">HTTP访问端口</td>
             <td class="td1_3">
                <%= Request.ServerVariables["SERVER_PORT"]%>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">虚拟目录的绝对路径</td>
             <td class="td1_3">
                <%=Request.ServerVariables["APPL_RHYSICAL_PATH"]  %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">执行文件的绝对路径</td>
             <td class="td1_3">
                <%= Request.ServerVariables["PATH_TRANSLATED"]  %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">虚拟目录Session总数</td>
             <td class="td1_3">
                <%= Session.Contents.Count  %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">虚拟目录Application总数</td>
             <td class="td1_3">
                <%=Application.Contents.Count  %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">域名主机</td>
             <td class="td1_3">
                <%= Request.ServerVariables["HTTP_HOST"]  %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">服务器区域语言</td>
             <td class="td1_3">
                <%=Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"]  %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">用户信息</td>
             <td class="td1_3">
                <%= Request.ServerVariables["HTTP_USER_AGENT"]   %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">CPU个数</td>
             <td class="td1_3">
                <%=Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS")  %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">CPU类型</td>
             <td class="td1_3">
                <%=Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")  %>
            </td>
        </tr>
        <tr>
            <td class="td1_2" width="115" style=" width:115px;">系统版本</td>
             <td class="td1_3">
               V3.0
            </td>
        </tr>
    </table>
</asp:Content>
