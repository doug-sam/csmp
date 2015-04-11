<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="list.aspx.cs" Inherits="page_Store_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="5" style="height: 30px" class="td1_1">
                        店铺管理
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属客户
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" DataTextField="Name" DataValueField="ID" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        所属品牌
                    </td>
                    <td class="td1_3">
                         <asp:DropDownList ID="DdlBrand" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2" rowspan="5">
                        <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
                      <div>
                            <asp:Button ID="BtnExport" runat="server" Text="导出搜索结果"  CssClass="BigButton" 
                            onclick="BtnExport_Click" />
                          </div>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属省份
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlProvince" DataTextField="Name" DataValueField="ID" runat="server"
                            AutoPostBack="True" 
                            onselectedindexchanged="DdlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        所属城市
                    </td>
                    <td class="td1_3">
                         <asp:DropDownList ID="DdlCity" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺名或编号
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbWd" runat="server"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        店铺电话
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbTel" runat="server"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺添加时间 从
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateBegin" runat="server" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        至
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEnd" runat="server" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        是否可用
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:DropDownList ID="DdlIsClosed" runat="server">
                            <asp:ListItem Text="不限" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="可用" Value="0"></asp:ListItem>
                            <asp:ListItem Text="不可用" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>                    
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSch" />
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%# Eval("ID")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺编号">
                <ItemTemplate>
                    <%# Eval("NO")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺名称">
                <ItemTemplate>
                    <%# Eval("Name")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属客户——品牌">
                <ItemTemplate>
                    <%# Eval("CustomerName")%>——<%# Eval("BrandName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="地区">
                <ItemTemplate>
                    <%# Eval("ProvinceName")%>——<%# Eval("CityName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="地址">
                <ItemTemplate>
                    <%# Eval("Address")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="电话">
                <ItemTemplate>
                    <%# Eval("Tel")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="添加日期">
                <ItemTemplate>
                    <%# Eval("AddDate")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="是否禁用">
                <ItemTemplate>
                    <%#Eval("IsClosed").ToString().ToLower()=="true"?"禁用":"启用"%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="编辑">
                <ItemTemplate>
                    <a class="link_4" href="javascript:tb_show('编辑店铺', '/page/Store/Edit.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
                        编辑</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px;
        font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
    <p style="padding: 10px;" id="P_Manage" runat="server">
        <asp:Literal ID="LtlAdd" runat="server">
            <a href="javascript:tb_show('添加', '/page/store/Edit.aspx?TB_iframe=true&height=450&width=730', false);" >添加</a>
        </asp:Literal>
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" OnClientClick="return confirm('确定删除数据？如果店铺被删除，\n\n那么它的所有报修记录也将被删除！！')" />
    </p>
</asp:Content>
