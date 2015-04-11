<%@ control language="C#" autoeventwireup="true" inherits="Controls_ListRec, App_Web_etara6uy" %>
<asp:UpdatePanel ID="UpdatePanelListRec" runat="server">
    <ContentTemplate>
        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
            <tr>
                <td style="height: 30px" class="td1_1">
                     <div >店铺报修记录查看</div>
                     <div style=" float:right;">
                         <asp:Label ID="LabMoreInfo" runat="server" Text=""></asp:Label></div>
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
            GridLines="Vertical" AllowPaging="true" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging">
            <RowStyle CssClass="GV_RowStyle"/>
            <HeaderStyle CssClass="GV_HeaderStyle"/>
            <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
            <Columns>
                <asp:TemplateField HeaderText="报修日期">
                    <ItemTemplate>
                        <%# Eval("ErrorDate")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="系统单号">
                    <ItemTemplate>
                        <%# Eval("NO")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="故障描述">
                    <ItemTemplate>
                        <%#Eval("Details")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="解决方案">
                    <ItemTemplate>
                        <%#Eval("SlnName")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="服务方式">
                    <ItemTemplate>
                        <%#Eval("SloveBy")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态">
                    <ItemTemplate>
                        <%#Enum.GetName(typeof(CSMP.Model.SysEnum.CallStateMain), Eval("StateMain"))%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="记录查看" Visible="true">
                    <ItemTemplate>
                        <a href="javascript:tb_show('处理记录查看', '/page/CallStep/listview.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
                            处理记录</a> <a href="javascript:tb_show('报修信息查看', '/page/call/view.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
                                报修信息</a>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
