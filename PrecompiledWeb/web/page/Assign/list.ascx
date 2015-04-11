<%@ control language="C#" autoeventwireup="true" inherits="page_Assign_list, App_Web_0c8bslcr" %>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1" style="margin-top: 15px;">
                <tr>
                    <td style="height: 30px" class="td1_1">
                        转派记录
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
                GridLines="Vertical">
                <RowStyle CssClass="GV_RowStyle" />
                <HeaderStyle CssClass="GV_HeaderStyle" />
                <AlternatingRowStyle CssClass="GV_AlternatingRowStyle" />
                <Columns>
                    <asp:TemplateField HeaderText="转派日期">
                        <ItemTemplate>
                            <%#Eval("AddDate")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="原负责人">
                        <ItemTemplate>
                            <%#Eval("OldName")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="新负责人">
                        <ItemTemplate>
                            <%#Eval("UserName")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="所属工作组">
                        <ItemTemplate>
                            <%#CSMP.BLL.WorkGroupBLL.GetWorkGroupName(Tool.Function.ConverToInt(Eval("WorkGroupID")))%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="转派操作人">
                        <ItemTemplate>
                            <%#Eval("CreatorName")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
