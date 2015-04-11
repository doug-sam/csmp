<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_call_Edit" %>

<%@ Register Src="/Controls/RecordPlay.ascx" TagName="RecordPlay" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="8" style="height: 30px" class="td1_1">
                        报修信息编辑
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="8">
                        (店铺信息)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        系统单号：
                    </td>
                    <td class="td1_3">
                        <%=info.No %>
                    </td>
                    <td class="td1_2">
                        报修人：
                    </td>
                    <td class="td1_3">
                        <%=info.ReporterName %>
                    </td>
                    <td class="td1_2">
                        客户：
                    </td>
                    <td class="td1_3">
                        <%=info.CustomerName %>
                    </td>
                    <td class="td1_2">
                        品牌：
                    </td>
                    <td class="td1_3">
                        <%=info.BrandName %>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        省份：
                    </td>
                    <td class="td1_3">
                        <%=info.ProvinceName %>
                    </td>
                    <td class="td1_2">
                        城市：
                    </td>
                    <td class="td1_3">
                        <%=info.CityName %>
                    </td>
                    <td class="td1_2">
                        店铺名称：
                    </td>
                    <td class="td1_3">
                        <asp:Literal ID="LtlStoreName" runat="server"></asp:Literal>
                    </td>
                    <td class="td1_2">
                        店铺编号：
                    </td>
                    <td class="td1_3">
                         <%=info.StoreName %>
                   </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺联系电话：
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:Literal ID="LtlTel" runat="server"></asp:Literal>
                    </td>
                    <td class="td1_2">
                        地址：
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:Literal ID="LtlAddress" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="8">
                        (报修信息)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        报修来源：
                    </td>
                    <td class="td1_3">
                        <%=info.ReportSourceName %>
                    </td>
                    <td class="td1_2">
                        来源单号：
                    </td>
                    <td class="td1_3">
                        <%=info.ReportSourceNo %>
                    </td>
                    <td class="td1_2">
                        报修时间：
                    </td>
                    <td class="td1_3">
                        <%=info.ErrorDate.ToString("yyyy-MM-dd HH:mm")%>
                    </td>
                    <td class="td1_2">
                        SLA：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbSLA" runat="server" Width="72px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        大类故障：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlClass1" class="input1" runat="server" DataTextField="Name"
                            DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlClass1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        中类故障：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlClass2" class="input1" runat="server" Enabled="false" DataTextField="Name"
                            DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlClass2_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        小类故障：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlClass3" class="input1" runat="server" Enabled="false" DataTextField="Name"
                            DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlClass3_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        优先级：
                    </td>
                    <td class="td1_3">
                        <asp:Literal ID="LtlPriority" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        故障描述：
                    </td>
                    <td class="td1_3" colspan="5">
                        <asp:TextBox ID="TxtDetails" runat="server"  Width="300px" TextMode="MultiLine" 
                            Height="46px"></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        二线负责人：
                        <br />
                        （技术中心）
                    </td>
                    <td class="td1_3">
                        <%=info.MaintaimUserName %>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        外部服务单号：
                    </td>
                    <td class="td1_3" colspan="5">
                        <asp:TextBox ID="TxbCallNo3" runat="server"  Width="300px" ></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        扩展SLA：
                    </td>
                    <td class="td1_3" >
                        <asp:TextBox ID="TxbSlaExt" runat="server" Width="72px"  ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        是否为重复报修：
                    </td>
                    <td class="td1_3" colspan="5">
                        <asp:CheckBox ID="CbIsSameCall" runat="server" Text="这是重复报修" />
                    </td>
                    <td class="td1_2">
                        服务类型
                    </td>
                    <td class="td1_3" >
                        <asp:DropDownList ID="DdlCategory" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton" AccessKey="s" 
        onclick="BtnSubmit_Click" />
</asp:Content>
