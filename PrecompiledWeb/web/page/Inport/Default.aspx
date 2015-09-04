<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_store_Default, App_Web_-z5vzseb" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .MyUl{ margin:50px; padding:20px;}
        .MyUl li{ margin:20px; font-size:18px; font-weight:700;}
    </style>
    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <ul class="MyUl">
            <li><a href="/page/Inport/Class2.aspx">中类数据导入</a></li>
            <li><a href="/page/Inport/Class.aspx">故障类整体导入</a></li>
            <li><a href="javascript:alert('在开发时遇到这样一个问题：不同的大类中下，中类名就可以重名，\n\n那么导入小类时，既要填写中类名，也要填写大类名，\n\n如果不匹配，系统也无法导入，\n\n这个在可行性上不够实际。');">小类数据导入</a></li>
            <li><a href="/page/Inport/Store.aspx">店铺信息数据导入</a></li>
        </ul>
<div style=" margin:50px; padding:10px; line-height:22px;">
    <h2>数据导入模板下载</h2>
<a href="Class2Template.xls">中类导入模板</a>
<br />
<a href="StoreTemplate.xls">店铺导入模板</a>
<br />
<a href="Class.xls">故障类整体导入模板</a>
</div>
</asp:Content>

