<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Comment_list, App_Web_xcjvkb_t" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="4" style="height: 30px" class="td1_1">评价管理
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">现场工程师：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlWorkGroup1" DataTextField="name" DataValueField="id" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlWorkGroup1_SelectedIndexChanged"></asp:DropDownList>
                        组的
                        <asp:DropDownList ID="DdlDropInUser" DataTextField="name" DataValueField="id" runat="server"></asp:DropDownList>
                        现场工程师
                    </td>
                    <td class="td1_2">二线：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlWorkGroup2" DataTextField="name" DataValueField="id" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlWorkGroup2_SelectedIndexChanged"></asp:DropDownList>
                        组的
                        <asp:DropDownList ID="DdlSupportUser" DataTextField="name" DataValueField="id" runat="server"></asp:DropDownList>
                        二线工程师
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">评论记录时间从：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxtDateBegin" runat="server" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                    <td class="td1_2">至
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEnd" runat="server" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">评论方
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCommandBy" runat="server">
                            <asp:ListItem Text="不限" Value="0"></asp:ListItem>
                            <asp:ListItem Text="现场工程师" Value="1"></asp:ListItem>
                            <asp:ListItem Text="二线" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSch" runat="server" Text="查看" CssClass="BigButton" OnClick="BtnSch_Click" />
                        <asp:Button ID="BtnExport" runat="server" Text="导出当前结果" CssClass="BigButton" OnClick="BtnExport_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <table class="table1">
        <tr>
            <td class="td1_2">技术评价指标</td>
            <td class="td1_2">态度评价指标</td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                    <tr>
                        <td class="td1_2">总评数：
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="HlAll" runat="server" OnClick="Hl_Click" CommandArgument="0"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1_2">1分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl1" runat="server" OnClick="Hl_Click" CommandArgument="1"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1_2">2分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl2" runat="server" OnClick="Hl_Click" CommandArgument="2"></asp:LinkButton>
                        </td>
                    </tr>

                    <tr>
                        <td class="td1_2">3分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl3" runat="server" OnClick="Hl_Click" CommandArgument="3"></asp:LinkButton>
                        </td>
                    </tr>

                    <tr>
                        <td class="td1_2">4分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl4" runat="server" OnClick="Hl_Click" CommandArgument="4"></asp:LinkButton>
                        </td>
                    </tr>

                    <tr>
                        <td class="td1_2">5分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl5" runat="server" OnClick="Hl_Click" CommandArgument="5"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1_2">平均分
                        </td>
                        <td class="td1_3">
                            <asp:HyperLink ID="HlAvg" runat="server"></asp:HyperLink>
                        </td>
                    </tr>

                </table>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                    <tr>
                        <td class="td1_2">总评数：
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="HlAlla" runat="server" OnClick="Hla_Click" CommandArgument="0"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1_2">1分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl1a" runat="server" OnClick="Hla_Click" CommandArgument="1"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1_2">2分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl2a" runat="server" OnClick="Hla_Click" CommandArgument="2"></asp:LinkButton>
                        </td>
                    </tr>

                    <tr>
                        <td class="td1_2">3分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl3a" runat="server" OnClick="Hla_Click" CommandArgument="3"></asp:LinkButton>
                        </td>
                    </tr>

                    <tr>
                        <td class="td1_2">4分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl4a" runat="server" OnClick="Hla_Click" CommandArgument="4"></asp:LinkButton>
                        </td>
                    </tr>

                    <tr>
                        <td class="td1_2">5分条数
                        </td>
                        <td class="td1_3">
                            <asp:LinkButton ID="Hl5a" runat="server" OnClick="Hla_Click" CommandArgument="5"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1_2">平均分
                        </td>
                        <td class="td1_3">
                            <asp:HyperLink ID="HlAvga" runat="server"></asp:HyperLink>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>


    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle" />
        <HeaderStyle CssClass="GV_HeaderStyle" />
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle" />
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%# Eval("ID")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="评论方">
                <ItemTemplate>
                    <%# Eval("IsDropInUserDoIt").ToString().ToLower()=="true"?"现场工程师":"二线"%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="现场工程师">
                <ItemTemplate>
                    <asp:Literal ID="LtlDropInUser" runat="server"></asp:Literal>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="二线">
                <ItemTemplate>
                    <asp:Literal ID="LtlSupportUser" runat="server"></asp:Literal>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="技术评分">
                <ItemTemplate>
                    <%#Eval("Score2")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="态度评分">
                <ItemTemplate>
                    <%#Eval("Score3")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="总评分">
                <ItemTemplate>
                    <%#Eval("Score")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期">
                <ItemTemplate>
                    <%#Eval("addDate")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="方式">
                <ItemTemplate>
                    <%#Eval("ByMachine")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修信息">
                <ItemTemplate>
                    <a href="javascript:tb_show('报修信息查看', '/page/call/view.aspx?ID=<%#Eval("CallID") %>&TB_iframe=true&height=450&width=730', false);">
                        <img src="/images/view.gif" /></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px; font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
    <p style="padding: 10px;">
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll', 'ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" OnClientClick="return confirm('确定删除数据？')" />
    </p>
</asp:Content>
