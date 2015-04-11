<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Feedback_list, App_Web_rasqts5x" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="6" style="height: 30px" class="td1_1">
                        回访答案查看
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属客户
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" DataTextField="Name" DataValueField="ID" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        品牌
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlBrand" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        店铺
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbStore" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        单号
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbNo" runat="server" ></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        所属大类
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlClass1" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        回访人
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCreatorID" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        开始日期
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateBegin" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        结束日期
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEnd" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                    </td>
                    <td class="td1_2" colspan="2"  >
                        <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
                        <asp:Button ID="BtnExport" runat="server" Text="导出数据"  CssClass="BigButton" 
                            onclick="BtnExport_Click" />
                        <asp:DropDownList ID="DdlPaper" runat="server" DataTextField="Name" DataValueField="ID" Visible="false">
                        </asp:DropDownList>
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
        GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        
        
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%# Eval("ID")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="添加时间">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime(Eval("AddDate")).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="问卷标识ID">
                <ItemTemplate>
                    <%#Eval("PaperID") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="回访人">
                <ItemTemplate>
                    <%#Eval("RecorderName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="品牌">
                <ItemTemplate>
                    <asp:Literal ID="LtlBrand" runat="server"></asp:Literal>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺">
                <ItemTemplate>
                    <asp:Literal ID="LtlStore" runat="server"></asp:Literal>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="被访人">
                <ItemTemplate>
                    <%#Eval("FeedbackUserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="问题">
                <ItemTemplate>
                    <%# Eval("QuestionName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="答案">
                <ItemTemplate>
                    <%# Eval("Answer")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修信息">
                <ItemTemplate>
                    <a class="link_4" href="javascript:tb_show('查看报修信息', '/page/call/View.aspx?ID=<%#Eval("CallID") %>&TB_iframe=true&height=450&width=730', false);">
                        查看报修信息</a>
                    
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="查看">
                <ItemTemplate>
                    <a class="link_4" href="javascript:tb_show('查看回答', '/page/Feedback/Edit.aspx?ID=<%#Eval("CallID") %>&TB_iframe=true&height=450&width=730', false);">
                        查看</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="background-color: #E3EFFB; text-align: center; height: 25px; line-height: 25px;
        font-weight: bold;">
        <asp:Literal ID="List_Page" runat="server"></asp:Literal>
    </p>
    <p style="padding: 10px;">
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" OnClientClick="return confirm('确定删除数据？')" />
    </p>
</asp:Content>
