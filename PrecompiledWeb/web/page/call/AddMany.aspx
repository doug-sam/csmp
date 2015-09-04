<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Call_AddMany, App_Web_isgyizrg" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="5" style="height: 30px" class="td1_1">
                        批量开CALL
                        <span class="red2">建议一次不要选择超过20个店铺建call</span>
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
                        所属品牌
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlBrand" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2" rowspan="3">
                        <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" CssClass="BigButton" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        所属省份
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlProvince" DataTextField="Name" DataValueField="ID" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        所属城市
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCity" DataTextField="Name" DataValueField="ID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺名或编号
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbWd" runat="server"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        店铺电话
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbTel" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSch" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical">
        <RowStyle CssClass="GV_RowStyle"/>
        
        
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' onclick="CheckAll();"/>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%# Eval("ID")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺编号">
                <ItemTemplate>
                    <%# Eval("NO")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺名称">
                <ItemTemplate>
                    <%# Eval("Name")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属客户——品牌">
                <ItemTemplate>
                    <%# Eval("CustomerName")%>——<%# Eval("BrandName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="地区">
                <ItemTemplate>
                    <%# Eval("ProvinceName")%>——<%# Eval("CityName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="地址">
                <ItemTemplate>
                    <%# Eval("Address")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="电话">
                <ItemTemplate>
                    <%# Eval("Tel")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="padding: 10px;">
        <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel');CheckAll();" />全选&nbsp;&nbsp;
        共有｛<span id="Span_ALL"><asp:Literal ID="LtlAllCount" runat="server"></asp:Literal></span>｝条搜索结果，
        ｛<span id="Span_Checked">0</span>｝条已选中
        <script language="javascript" type="text/javascript">
                function CheckAll() {
                    var FlagChecked = 0;
                    $(":checkbox[name='ckDel']").each(
                        function() {
                            if (this.checked == true) {
                                FlagChecked++;
                            }                        
                        }
                    );
                        Set_Span_Checked(FlagChecked);
                }
                function Set_Span_Checked(val) {
                    $("#Span_Checked").html(val);
                }
                function Get_Span_Checked(val) {
                    return $("#Span_Checked").html();
                }
                function Get_Span_ALL()
                {
                    return $("#Span_ALL").html();
                }
        </script>
    </p>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="6" style="height: 30px" class="td1_1">
                <div style="margin: 0 10px 0 0; line-height: 24px;" align="right">
                    (报修信息)
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                报修来源：
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="ddlReportSource" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                来源单号：
            </td>
            <td class="td1_3">
                <asp:TextBox ID="txtSourceNo" runat="server"></asp:TextBox>
            </td>
            <td class="td1_2">
                报修时间：
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxtErrorDate" runat="server" CssClass="input2" Width="198px" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                大类故障：
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="ddlClass1" runat="server" DataTextField="Name" DataValueField="ID"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlClass1_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                中类故障：
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="ddlClass2" runat="server" Enabled="false" DataTextField="Name"
                    DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlClass2_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                小类故障：
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="ddlClass3" runat="server" Enabled="false" DataTextField="Name"
                    DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlClass3_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                优先级：
            </td>
            <td class="td1_3">
                <asp:Literal ID="LtlPriority" runat="server"></asp:Literal>
            </td>
            <td class="td1_2">
                SLA：
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbSLA" runat="server"></asp:TextBox>
            </td>
            <td class="td1_2">
                SLA扩展：
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbSLA2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2"  rowspan="2">
                故障描述：
            </td>
            <td colspan="3" rowspan="2" class="td1_3">
                <asp:TextBox ID="TxtDetails" runat="server" CssClass="input6" Width="400px" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td class="td1_2">
                分派二线<br />
                （技术中心）：
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="ddlL2Id" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
        </tr>
                <tr>
                    <td class="td1_2">
                        服务类型：
                    </td>
                    <td colspan="3" class="td1_3">
                        <asp:DropDownList ID="DdlCategory" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                    </td>
                </tr>
        <tr>
            <td colspan="6" style="height: 30px" class="td1_1">
                <asp:Button ID="BtnSubmit" runat="server" Text="保存,并交给技术中心处理" CssClass="BigButton"
                    OnClick="BtnSubmit_Click" /><span class="red2">如果call量多。保存时有点慢，奈心等哈</span>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
