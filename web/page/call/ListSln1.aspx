<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="ListSln1.aspx.cs" Inherits="page_call_ListSln1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
            <tr>
                <td colspan="7" style="height: 30px" class="td1_1">
                    等待安排上门
                    报修管理
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    一线负责人
                </td>
                <td class="td1_3" >
                    <asp:DropDownList ID="DdlL1" runat="server" DataTextField="Name" DataValueField="ID">
                    </asp:DropDownList>
                </td>

                <td class="td1_2">
                    二线负责人
                </td>
                <td class="td1_3">
                    <asp:DropDownList ID="DdlL2" runat="server" DataTextField="Name" DataValueField="ID">
                    </asp:DropDownList>
                </td>
                <td rowspan="4"  class="td1_2">
                     <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" 
                         Height="88px" Width="88px"  />   
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    报修日期 从
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxtDateBegin" runat="server" onclick="WdatePicker()"></asp:TextBox>
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
                    所属品牌
                </td>
                <td class="td1_3">
                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                                AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="td1_2">
                    店铺所在地区
                </td>
                <td class="td1_3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="DdlProvince" runat="server" DataTextField="Name" DataValueField="ID"
                                AutoPostBack="True" 
                                onselectedindexchanged="DdlProvince_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="DdlCity" runat="server" DataTextField="Name" DataValueField="ID">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="td1_2">
                    单号
                </td>
                <td class="td1_3" >
                    <asp:TextBox ID="TxbCallNo" runat="server"  ></asp:TextBox>
                </td>
                <td class="td1_2">
                    店铺号或店铺名(s)
                </td>
                <td class="td1_3">
                    <asp:TextBox ID="TxbStoreNo" AccessKey="s" runat="server"></asp:TextBox>
                </td>                
            </tr>
        </table>    
    
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" >
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' /></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="系统单号">
                <ItemTemplate>
                    <%# Eval("NO")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修日期">
                <ItemTemplate>
                    <%# Eval("ErrorDate")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建人">
                <ItemTemplate>
                    <%#Eval("CreatorName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="技术负责人">
                <ItemTemplate>
                    <%#Eval("MaintaimUserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="故障描述">
                <ItemTemplate>
                    <%#Eval("Details")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺号">
                <ItemTemplate>
                    <a href="javascript:tb_show('查看店铺', '/page/Store/View.aspx?ID=<%#Eval("StoreID") %>&TB_iframe=true&height=450&width=730', false);">
                        <%#Eval("StoreName") %></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%#Enum.GetName(typeof(CSMP.Model.SysEnum.CallStateDetails), Eval("StateDetail"))%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="安排上门">
                <ItemTemplate>
                    <asp:Panel ID="PanelnDropIn1" runat="server">
                        <a href="slnDropIn1.aspx?ID=<%#Eval("ID") %>">安排上门</a>
                    </asp:Panel>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="处理记录">
                <ItemTemplate>
                    <a href="javascript:tb_show('处理记录查看', '/page/CallStep/listview.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
                        处理记录</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修信息" Visible="true">
                <ItemTemplate>
                    <a href="javascript:tb_show('报修信息查看', '/page/call/view.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
                        报修信息</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px;
        font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
</asp:Content>
