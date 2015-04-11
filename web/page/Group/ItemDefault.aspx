<%@ Page Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="ItemDefault.aspx.cs" Inherits="page_Group_ItemDefault" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .myladygaga{ padding:20px; margin:5px;}
        .myladygaga li{ padding:10px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ul class="myladygaga">
        <li>
            <asp:HyperLink ID="HlItem1" runat="server" Target="main">状态页显示设置</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="HlItem2" runat="server"  Target="main">搜索页显示设置</asp:HyperLink>
        </li>
    </ul>
</asp:Content>
