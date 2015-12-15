<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataCache.aspx.cs" Inherits="LeftMenu_DataCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br/>
        结果：<asp:TextBox ID="tboxResult" runat="server"></asp:TextBox>
        <br/>
        <br/>
        <br/>
        <asp:Button ID="BtnGetFromData" runat="server" Text="从数据库同步" 
            onclick="BtnGetFromData_Click" />&nbsp&nbsp&nbsp
        <asp:Button ID="BtnCacheAdd" runat="server" Text="内存数据加1" 
            onclick="BtnCacheAdd_Click" />&nbsp&nbsp&nbsp
        <asp:Button ID="BtnCacheMinus" runat="server" Text="内存数据减1" 
            onclick="BtnCacheMinus_Click" />&nbsp&nbsp&nbsp
        <asp:Button ID="BtnShowCache" runat="server" Text="显示内存数据" 
            onclick="BtnShowCache_Click" />&nbsp&nbsp&nbsp
        <asp:Button ID="postCachePage" runat="server" Text="访问写数据到内存的网页" 
            onclick="postCachePage_Click" /><br /><br />
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
