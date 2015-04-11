<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_list, App_Web_rursbog1" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>
<%@ Import Namespace="CSMP.BLL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="7" style="height: 30px" class="td1_1">
                <asp:Label ID="LabState" runat="server" Text="" Style="color: #F60; font-size:30px;"></asp:Label>
                报修管理
                <div style=" position:absolute; right:20px; top:5px; font-size:16px;">
                    <asp:Literal ID="LtlAssign" runat="server"></asp:Literal>

                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                一线负责人
            </td>
            <td class="td1_3" >
                <asp:DropDownList ID="DdlL1" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>

            <td class="td1_2">
                二线负责人
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlL2" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                上门工程师
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDropInUser" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td rowspan="5"  class="td1_2">
                 <asp:Button ID="BtnSch" runat="server" Text="搜索" OnClick="BtnSch_Click" 
                    CssClass="BigButton"  />   
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
            <td class="td1_2">
                单号
            </td>
            <td class="td1_3" >
                <asp:TextBox ID="TxbCallNo" runat="server"  ></asp:TextBox>
            </td>

        </tr>
        <tr>
            <td class="td1_2">
                所属品牌
            </td>
            <td class="td1_3">
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlBrand" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="td1_2">
                店铺所在地区
            </td>
            <td class="td1_3">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DdlProvince" runat="server" DataTextField="Name" DataValueField="ID"
                            AutoPostBack="True" 
                            onselectedindexchanged="DdlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlCity" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="td1_2">
                店铺号或店铺名(s)
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbStoreNo" runat="server"  AccessKey="s" ></asp:TextBox>
            </td>
        </tr>
        <tr >
            <td class="td1_2">
                状态
            </td>
            <td class="td1_3" >
                <asp:DropDownList ID="DdlState" runat="server" DataTextField="value" DataValueField="key" Enabled="false">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                解决方式
            </td>
            <td class="td1_3" >
                <asp:DropDownList ID="DdlSloveBy" runat="server" DataTextField="value" DataValueField="key" Enabled="false">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                备件情况
            </td>
            <td class="td1_3" >
                <asp:DropDownList ID="DdlReplacementStatus" runat="server" DataTextField="value" DataValueField="key" >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                来源单号
            </td>
            <td class="td1_3" >
                <asp:TextBox ID="TxbNo2" runat="server"></asp:TextBox>
            </td>
            <td class="td1_2">
                服务类型
            </td>
            <td class="td1_3" colspan="3">
                <asp:DropDownList ID="DdlCategory" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
       OnRowDataBound="GridView1_RowDataBound">
        <RowStyle CssClass="GV_RowStyle"/>
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="选择">
                <ItemTemplate>
                    <input type="checkbox" name="ckDel" value='<%# Eval("ID")%>' /></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="系统单号">
                <ItemTemplate>
                    <%# Eval("NO")%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="服务类型">
                <ItemTemplate>
                    <%#CallCategoryBLL.Get(Tool.Function.ConverToInt(Eval("Category"))).Name%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="报修日期">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime( Eval("ErrorDate")).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="品牌">
                <ItemTemplate>
                    <%# Eval("BrandName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店铺号">
                <ItemTemplate>
                    <a href="javascript:tb_show('查看店铺', '/page/Store/View.aspx?ID=<%#Eval("StoreID") %>&TB_iframe=true&height=450&width=730', false);">
                        <%#Eval("StoreName")+"<br/>"+CSMP.BLL.StoresBLL.GetName(Tool.Function.ConverToInt(Eval("StoreID"))) %>
                        </a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="中类故障">
                <ItemTemplate>
                    <%#Eval("ClassName2")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="小类故障">
                <ItemTemplate>
                    <asp:HyperLink ID="HlEditCall" NavigateUrl="#" runat="server">
                        <%#Eval("ClassName3")%>
                    </asp:HyperLink>                    
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="故障描述">
                <ItemTemplate>
                   <a href="#" class="tooltip">
                     <%#Tool.Function.Cuter(Eval("Details"),15) %>
                       <span>
                           <%#Eval("Details") %>
                       </span>
                    </a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="一线处理人">
                <ItemTemplate>
                    <%#Eval("CreatorName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="二线处理人">
                <ItemTemplate>
                    <%#Eval("MaintaimUserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="预约时间/人员">
                <ItemTemplate>
                    <asp:Label ID="LabDropIn1Date" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="LabDropInUser" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%#Enum.GetName(typeof(CSMP.Model.SysEnum.CallStateDetails), Eval("StateDetail"))%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="是否重复报修">
                <ItemTemplate>
                    <asp:Label ID="LabIsSameCall" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="SLA">
                <ItemTemplate>
                    <%#Eval("SLA")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="超时时间">
                <ItemTemplate>
                    <%#Tool.Function.ConverToDateTime(Eval("SLADateEnd"))==CSMP.Model.DicInfo.DateZone?"计算中":Eval("SLADateEnd").ToString()%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="解决方式">
                <ItemTemplate>
                    <%#Eval("SloveBy")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="处理">
                <ItemTemplate>
                    <asp:PlaceHolder ID="PhHandle" runat="server">
                        <a href="sln.aspx?id=<%#Eval("ID") %>">前往处理</a>
                    </asp:PlaceHolder>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
             <asp:TemplateField HeaderText="备件管理">
                <ItemTemplate>
                      <a href="javascript:tb_show('备件管理', '/page/Replacement/List.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=830', false);">
                        备件管理
                       </a>
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
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="转派" Visible="true">
                <ItemTemplate>
                    <a href="javascript:tb_show('转派', '/page/Assign/Assign.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
                        转派</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="后续处理" Visible="true">
                <ItemTemplate>
                    <a href="javascript:tb_show('后续处理', '/page/callStep/StepSwitchAfterFinish.aspx?ID=<%#Eval("ID") %>&TB_iframe=true&height=450&width=730', false);">
                        <img src="/images/up.gif" title="后续处理" alt="后续处理" /></a>
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
        <asp:PlaceHolder ID="PhSelectAll" runat="server">
         <input type='checkbox' name='ckAll' value='1' onclick="checkAll('ckAll','ckDel')" />全选&nbsp;&nbsp;
        </asp:PlaceHolder>
        <asp:Button ID="BtnDelete" runat="server" Text="删除选中" OnClick="Btn_Delete" OnClientClick="return confirm('确定删除数据？')" />
        &nbsp;
        <asp:Button ID="BtnClose" Visible="false" runat="server" Text="关闭选中" OnClick="BtnClose_Click" OnClientClick="return confirm('确定关闭选中吗？\n\n不单条关闭将默认回收单号为系统单号！')" />
    </p>
</asp:Content>
