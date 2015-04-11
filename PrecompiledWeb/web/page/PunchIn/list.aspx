<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_PunchIn_list, App_Web_zp1o4vas" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/js/thickbox/thickbox.css" rel="stylesheet" type="text/css" />

    <script src="/js/thickbox/thickbox.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:updatepanel id="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:updatepanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="5" style="height: 30px" class="td1_1">考勤管理 
            </td>
        </tr>
        <tr>
            <td class="td1_2">日期 从
            </td>
            <td class="td1_3">
                <asp:textbox id="TxbDateBegin" runat="server" onclick="WdatePicker()"></asp:textbox>
            </td>
            <td class="td1_2">至
            </td>
            <td class="td1_3">
                <asp:textbox id="TxbDateEnd" runat="server" onclick="WdatePicker()"></asp:textbox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">员工
            </td>
            <td class="td1_3">
                <asp:dropdownlist id="DdlUser" runat="server" datatextfield="Name" datavaluefield="ID">
                </asp:dropdownlist>
            </td>
            <td class="td1_2" colspan="2">
                <asp:button id="BtnSch" runat="server" text="搜索" onclick="BtnSch_Click" cssclass="BigButton" />
            </td>
        </tr>
    </table>
    <asp:gridview id="GridView1" width="100%" cssclass="table2" runat="server" autogeneratecolumns="false"
        gridlines="Vertical" onrowdatabound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="员工">
                <ItemTemplate>
                    <%=DdlUser.SelectedItem.Text %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime(Eval("DateAdd")).ToString("yyy-MM-dd") %> 
                    &nbsp;
                    <%#Tool.Function.ConverToDateTime(Eval("DateAdd")).DayOfWeek %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="打卡记录">
                <ItemTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                        <asp:Repeater ID="Rp1" runat="server">
                            <ItemTemplate>
                                 <tr>
                                    <td  class="td1_2">
                                        <%#Eval("Detail") %>
                                    </td>
                                    <td  class="td1_2">
                                        <%#Eval("PositionAddress") %>
                                    </td>
                                    <td  class="td1_2">
                                        <%#Eval("Memo") %>
                                    </td>
                                    <td  class="td1_2">
                                        <a href="#" onclick='tb_show("", "Edit.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=400&width=800&modal=false", false); return false;'>
                                            <img src="/images/edit.gif" /></a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
        </Columns>
    </asp:gridview>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px;
        font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
</asp:Content>
