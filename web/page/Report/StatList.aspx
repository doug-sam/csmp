<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="StatList.aspx.cs" Inherits="page_call_StatList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="4" style="height: 30px" class="td1_1">
                        明细数据查询(导出方案1.旧版)
                    </td>
                </tr>
                <tr id="TR_State" runat="server">
                    <td class="td1_2">
                        状态
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlState" runat="server" DataTextField="value" DataValueField="key"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlState_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        详细状态
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlStateDetail" runat="server" DataTextField="value" DataValueField="key"
                            Visible="false">
                        </asp:DropDownList>
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
                        创建日期 从
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateBeginCreate" runat="server" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        至
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEndCreate" runat="server" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺所在地区
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlProvince" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlCity" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        店铺号或店铺名
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbStoreNo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        服务方式
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlSolvedBy" runat="server" DataTextField="value" DataValueField="key">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        单号
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbCallNo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr  >
                    <td class="td1_2">
                        所属品牌
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        SLA要求
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlSla" runat="server">
                            <asp:ListItem Text="不限" Value="0"></asp:ListItem>
                            <asp:ListItem Text="满足sla" Value="1"></asp:ListItem>
                            <asp:ListItem Text="超sla" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        明细选择
                    </td>
                    <td class="td1_3">
                        <asp:RadioButtonList ID="RblStep" runat="server">
                            <asp:ListItem Selected="True" Text="导出时连同操作步骤一起导出" Value="1"></asp:ListItem>
                            <asp:ListItem Text="导出时只需报修数据" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSch" runat="server" Text="搜&nbsp;&nbsp;&nbsp;&nbsp;索" OnClick="BtnSch_Click"
                          CssClass="BigButton" />
                        <asp:Button ID="BtnExport" runat="server" Text="导出搜索结果" 
                          CssClass="BigButton" onclick="BtnExport_Click" Visible="false" />
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
        GridLines="Vertical">
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#114caa" Height="30" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
        <AlternatingRowStyle BackColor="#DCDCDC" />
        <Columns>
            <asp:TemplateField HeaderText="系统单号">
                <ItemTemplate>
                    <%# Eval("NO")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修日期">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime(Eval("ErrorDate")).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="品牌">
                <ItemTemplate>
                    <%# Eval("BrandName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺名">
                <ItemTemplate>
                    <a href="javascript:tb_show('查看店铺', '/page/Store/View.aspx?ID=<%#Eval("StoreID") %>&TB_iframe=true&height=450&width=730', false);">
                        <%# CSMP.BLL.StoresBLL.GetName(Tool.Function.ConverToInt(Eval("StoreID")))%></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="大类故障">
                <ItemTemplate>
                    <%#Eval("ClassName1")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="中类故障">
                <ItemTemplate>
                    <%#Eval("ClassName2")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="故障描述">
                <ItemTemplate>
                    <%#Eval("Details")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="二线处理人" Visible="false">
                <ItemTemplate>
                    <%#Eval("MaintaimUserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%#Enum.GetName(typeof(CSMP.Model.SysEnum.CallStateMain), Eval("StateMain"))%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态详细">
                <ItemTemplate>
                    <%#Enum.GetName(typeof(CSMP.Model.SysEnum.CallStateDetails), Eval("StateDetail"))%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="SLA" Visible="false">
                <ItemTemplate>
                    <%#Eval("SLA")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="解决方式">
                <ItemTemplate>
                    <%#Eval("SloveBy")%>
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
                    <%--<asp:Label ID="LabRecord" runat="server" >
                        <a href="/page/record/play.aspx?CallID=<%#Eval("ID") %>" target="_blank"><img src="#" alt="喇叭"/></a>
                    </asp:Label>--%>
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
