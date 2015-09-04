<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="Log_list, App_Web_dkrc1fbm" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                系统日志查看
            </td>
        </tr>
        <tr>
            <td  class="td1_2">
                类型
            </td>
            <td  class="td1_3">
                <asp:DropDownList ID="DdlCategory" runat="server" DataTextField="value" DataValueField="key">
                </asp:DropDownList>
            </td>
            <td  class="td1_2">
                <asp:Button ID="BtnSch" runat="server" Text=" 查找 " onclick="BtnSch_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical"  >
        <RowStyle CssClass="GV_RowStyle"/>
        
        
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' /></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="编号">
                <ItemTemplate>
                   <%#Eval("ID")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属分类">
                <ItemTemplate>
                   <%--<%#Enum.GetName(typeof(Rx.Model.SysEnum.LogType),Eval("Category"))%>--%>
                   <%#Eval("Category")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="标题">
                <ItemTemplate>
                   <%#Tool.Function.Cuter(Tool.Function.RemoveHTML(Eval("Content")),50)%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="引错用户">
                <ItemTemplate>
                   <%#Eval("UserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="添加日期">
                <ItemTemplate>
                   <%#Eval("AddDate")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="系统出错日期">
                <ItemTemplate>
                   <%#Eval("ErrorDate")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="查看">
                <ItemTemplate>
                     <a href="#" onclick='tb_show("", "View.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=400&width=800&modal=false", false); return false;'>                  
                        <img src="/images/view.gif" /></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px;
        font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
  <p style="padding: 10px;" runat="server" id="P_Add">
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" />
    </p>
</asp:Content>
