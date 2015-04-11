<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="副本 view.aspx.cs" Inherits="page_WorkGroupEmail_view" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <asp:Button ID="BtnSubmit" CssClass="BigButton" runat="server" Text=" 提交 " OnClick="BtnSubmit_Click" />
            </td>
        </tr>
        <tr>
            <td class="td1_2" colspan="2">
                <input id="cTxbUser" type="text" onkeydown="cTxbUser_Change()" style="width:100%;" />
            </td>
        </tr>
           <tr>
            <td class="td1_2" valign="top">

                <h3>收件人                <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll', 'ckDel')" />全选&nbsp;&nbsp;
</h3>
                <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
                    GridLines="Vertical">
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
                        <asp:TemplateField HeaderText="收件人名">
                            <ItemTemplate>
                                <div class="DivName"><%#Eval("Name") %></div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="邮箱">
                            <ItemTemplate>
                                <%#Eval("Email") %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td class="td1_2" valign="top">
                <h3>收件人员组                <input type='checkbox' name='ckAll2' value='1' onclick="checkAll('ckAll2', 'ckEmailGroup')" />全选&nbsp;&nbsp;
</h3>
                <asp:GridView ID="GridView2" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
                    GridLines="Vertical">
                    <RowStyle CssClass="GV_RowStyle" />
                    <HeaderStyle CssClass="GV_HeaderStyle" />
                    <AlternatingRowStyle CssClass="GV_AlternatingRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="选择">
                            <ItemTemplate>
                                <input type="checkbox" name="ckEmailGroup" value='<%# Eval("ID")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="组名">
                            <ItemTemplate>
                             <div class="DivName"><%#Eval("Name") %></div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function cTxbUser_Change()
        {
            var TxbVal = $("#cTxbUser").val();
            $(".DivName").each(function () {
                if ($(this).text().indexOf(TxbVal) >= 0) {
                    $(this).css("color", "red");
                    $(this).css("font-weight", "700");
                }
                else {
                    $(this).css("color", "#717171");
                    $(this).css("font-weight", "normal");
                }
            });
        }
    </script>
</asp:Content>
