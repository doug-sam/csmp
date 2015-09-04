<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_FeedbackQuestion_Edit, App_Web_2uux6kqk" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <div style="float: left;">
                    <asp:Literal ID="LtlAction" runat="server" Text="编辑"></asp:Literal>
                </div>
                <div style="float: right; cursor: pointer;">
                    <a href="#" onclick="self.parent.tb_remove();return false;">关闭</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                Question
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbName" runat="server" Width="404px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                Type
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlType" DataTextField="value" DataValueField="key" runat="server">
                </asp:DropDownList>
                (选择问类型后将不能修改)
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                Paper
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlPaper" DataTextField="Name" DataValueField="ID" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                OrderNumber
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbOrderNumber" runat="server" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                是否启用
            </td>
            <td class="td1_3">
                <asp:CheckBox ID="CbEnable" runat="server" Text="启用" Checked="true" />
            </td>
        </tr>
        <tr>
            <td class="td1_2" colspan="2">
                <asp:Panel ID="PanelChoose" runat="server">
                    <asp:HyperLink ID="HlChoose" Visible="true" runat="server" Target="main">管理问题选项</asp:HyperLink>
                    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
                        GridLines="Vertical" >
                        <RowStyle CssClass="GV_RowStyle"/>
                        
                        
                        
                        <HeaderStyle CssClass="GV_HeaderStyle"/>
                        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Eval("ID") %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="名称">
                                <ItemTemplate>
                                    <%#Eval("Name") %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="是否启用">
                                <ItemTemplate>
                                    <%#Eval("Enable").ToString().ToLower()=="true"?"使用中":"禁用"%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " OnClick="BtnSubmit_Click" />
</asp:Content>
