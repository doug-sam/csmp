﻿<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_CallStep_listview, App_Web_mh7akdho" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .EnableEdit{ cursor:pointer; color:Blue;}
    </style>
    <script type="text/javascript">
        $(document).ready(function() { });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                <asp:Literal ID="LtlStore" runat="server"></asp:Literal>
                处理记录
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical"  OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="步骤">
                <ItemTemplate>
                    步骤
                    <%#Container.DataItemIndex+1%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所进行操作">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server">
                        <%#Eval("StepName")%>
                    </asp:HyperLink>
                   </span>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="负责人">
                <ItemTemplate>
                    <%#Eval("MajorUserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="处理时间">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime(Eval("DateBegin")).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作备注">
                <ItemTemplate>
                    <%#Eval("Details")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="记录人/记录时间">
                <ItemTemplate>
                    <%#Eval("UserName") %><br />
                    <%#Tool.Function.ConverToDateTime(Eval("AddDate")).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="是否解决">
                <ItemTemplate>
                    <%#IsSloved(Eval("IsSolved").ToString(),Eval("StepType").ToString()) %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
   <div style="text-align:center; font-weight:700;font-size:14px; color:#F60;"> <asp:Literal ID="LtlSolution" runat="server"></asp:Literal></div>
                        <asp:Button ID="BtnRockBack" runat="server" Text="回滚(r)" OnClick="BtnRockBack_Click" Visible="false" CssClass="BigButton" AccessKey="r" />




</asp:Content>