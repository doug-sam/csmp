<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="listRec.aspx.cs" Inherits="page_call_listRec" %>

<%@ Register src="/Controls/ListRec.ascx" tagname="ListRec" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ListRec ID="ListRec1" runat="server" />
</asp:Content>
