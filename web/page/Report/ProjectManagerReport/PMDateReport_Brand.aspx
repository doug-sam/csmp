﻿<%@ Page Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" 
CodeFile="PMDateReport_Brand.aspx.cs" Inherits="page_Report_ProjectManagerReport_PMDateReport_Brand" %>
<%@ Register assembly="AspNetPager" namespace="Wuqi.Webdiyer" tagprefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../../css/page.css" rel="stylesheet" type="text/css" />
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                项目经理视觉故障报修日统计报表(品牌)
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                开始日期
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxtDateBegin" runat="server" class="input3" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td class="td1_2">
                结束日期
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateEnd" runat="server" class="input3" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                项目经理
            </td>
            <td class="td1_3">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DdlPM" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlPM_SelectedIndexChanged">
                        </asp:DropDownList>
                        客户<asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                        品牌<asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="td1_2" colspan="2">
                <asp:Button ID="BtnSearch" runat="server" Text="查询" CssClass="BigButton" OnClick="BtnSearch_Click" />
            </td>
        </tr>
    </table>

    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
    GridLines="Vertical" >
    <RowStyle CssClass="GV_RowStyle"/>
    <HeaderStyle CssClass="GV_HeaderStyle"/>
    <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="客户名称">
                <ItemTemplate>
                   <%# Eval("f_CustomerName")%>
                 </ItemTemplate> 
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期">
                <ItemTemplate>
                    <%# Eval("时间")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="品牌">
                <ItemTemplate>
                    <%# Eval("f_BrandName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="开单总量">
                <ItemTemplate>
                    <%# Eval("开单总量")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="完成总量">
                <ItemTemplate>
                    <%# Eval("完成总量")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="处理中量">
                <ItemTemplate>
                   <%# Eval("处理中量")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="处理中超期量">
                <ItemTemplate>
                    <%# Eval("处理中超期量")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:GridView ID="hdGridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
    GridLines="Vertical" Visible="false">
    <RowStyle CssClass="GV_RowStyle"/>
    <HeaderStyle CssClass="GV_HeaderStyle"/>
    <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="客户名称">
                <ItemTemplate>
                   <%# Eval("f_CustomerName")%>
                 </ItemTemplate> 
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期">
                <ItemTemplate>
                    <%# Eval("时间")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="开单总量">
                <ItemTemplate>
                    <%# Eval("开单总量")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="完成总量">
                <ItemTemplate>
                    <%# Eval("完成总量")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="处理中量">
                <ItemTemplate>
                   <%# Eval("处理中量")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="处理中超期量">
                <ItemTemplate>
                    <%# Eval("处理中超期量")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <div class ="text-align:center">
        <webdiyer:AspNetPager ID="AspNetPager1" runat="server"   PageSize="20" CurrentPageIndex ="1"
            onpagechanged="AspNetPager1_PageChanged" CssClass="anpager" 
            CurrentPageButtonClass="cpb" FirstPageText="首页" LastPageText="尾页" 
            NextPageText="后页" PrevPageText="前页">
        </webdiyer:AspNetPager>
    
    </div>
    
    
</asp:Content>