﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" CodeFile="ReciveBill.aspx.cs" Inherits="page_CallStep_ReciveBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="/js/Public.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<script>
    try {
        if (IsIE()) {
            document.write("<h1 style=' text-align:center;'>5秒后将关系窗口</h1>");
            setTimeout("window.open('','_parent','');window.close();", 5000);
        }
    }
    catch (ex) { }
</script>
</asp:Content>

