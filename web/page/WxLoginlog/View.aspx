<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="View.aspx.cs" Inherits="page_Store_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="4" style="height: 30px" class="td1_1">
                        查看店铺
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺编号：
                    </td>
                    <td class="td1_3">
                        <%=((CSMP.Model.StoreInfo)ViewState["info"]).No %>
                    </td>
                    <td class="td1_2">
                        店铺名称：
                    </td>
                    <td class="td1_3">
                        <%=((CSMP.Model.StoreInfo)ViewState["info"]).Name %>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属品牌：
                    </td>
                    <td class="td1_3">
                        <%=((CSMP.Model.StoreInfo)ViewState["info"]).BrandName %>
                    </td>
                    <td class="td1_2">
                        所属地区：
                    </td>
                    <td class="td1_3">
                        <%=((CSMP.Model.StoreInfo)ViewState["info"]).ProvinceName %>——
                        <%=((CSMP.Model.StoreInfo)ViewState["info"]).CityName %>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        地址：
                    </td>
                    <td class="td1_3">
                        <%=((CSMP.Model.StoreInfo)ViewState["info"]).Address %>
                    </td>
                    <td class="td1_2">
                        地图：
                    </td>
                    <td class="td1_3">
                        <img onclick="javascript:tb_show('','http://api.map.baidu.com/staticimage?center=<%=Server.UrlEncode(((CSMP.Model.StoreInfo)ViewState["info"]).Address.ToString()) %>&markers=<%=Server.UrlEncode(((CSMP.Model.StoreInfo)ViewState["info"]).Address.ToString()) %>&TB_iframe=true&height=300&width=400', false);"
                            src='http://api.map.baidu.com/staticimage?center=<%=Server.UrlEncode(((CSMP.Model.StoreInfo)ViewState["info"]).Address.ToString()) %>&markers=<%=Server.UrlEncode(((CSMP.Model.StoreInfo)ViewState["info"]).Address.ToString()) %>'
                            width="200" height="150" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        电话：
                    </td>
                    <td class="td1_3">
                        <%=((CSMP.Model.StoreInfo)ViewState["info"]).Tel %>
                    </td>
                    <td class="td1_2">
                        <asp:HyperLink ID="HlHistory" runat="server" Target="main">查看店铺所有报修历史</asp:HyperLink>
                    </td>
                    <td class="td1_2">
                        <asp:LinkButton ID="LbTop" runat="server" onclick="LbTop_Click">查看最近三次报修</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PanelHistory" runat="server">
                <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
                    GridLines="Vertical">
                    <RowStyle CssClass="GV_RowStyle"/>
                    
                    
                    
                    <HeaderStyle CssClass="GV_HeaderStyle"/>
                    <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
                    <Columns>
                        <asp:TemplateField HeaderText="系统单号">
                            <ItemTemplate>
                                <a href='/page/call/sch.aspx?CallNo=<%# Eval("NO")%>' target="main">
                                    <%# Eval("NO")%></a></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="报修日期">
                            <ItemTemplate>
                                <%#Tool.Function.ConverToDateTime( Eval("ErrorDate")).ToString("yyyy-MM-dd HH:mm")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="小类故障">
                            <ItemTemplate>
                                <%#Eval("ClassName3")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="故障描述">
                            <ItemTemplate>
                                <%#Eval("Details")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="解决方式">
                            <ItemTemplate>
                                <%#Eval("SloveBy")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="是否为重复报修">
                            <ItemTemplate>
                               <%#Eval("IsSameCall")=="true"?"是":"否" %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="LabHistoryNoRecord" runat="server" Text="没有记录" Visible="false" 
                style=" display:block; margin:3px; text-align:center; font-weight:700; color:#F60;"></asp:Label>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
