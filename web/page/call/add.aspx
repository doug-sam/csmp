<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="add.aspx.cs" Inherits="page_call_add" %>

<%@ Register Src="/Controls/CallState.ascx" TagName="CallState" TagPrefix="uc1" %>
<%@ Register src="/Controls/ListRec.ascx" tagname="ListRec" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .divSln
        {
            margin: 0px;
            padding: 0px;
        }
        .divSln a
        {
            margin: 10px;
            background: #BBFEB1;
            padding: 5px;
            display:block; 
            float:left;
        }
        .divSln .Focus
        {
            margin: 10px;
            background: #F8F769;
            padding: 5px;
        }
    </style>

    <script type="text/javascript">
        function FucusSln(toolTip) {
            $(".divSln a").each(function() {
                var CurrentTitle = $(this)[0].title;

                if (($(this)[0].title.toString() != toolTip.toString()))
                    $(this)[0].className = "";
            });
            
        }
    </script>

    <!--自动搜索完成控件                    begin-->

    <script type='text/javascript' src='/js/jquery-autocomplete/jquery.autocomplete.js'></script>

    <link rel="stylesheet" type="text/css" href="/js/jquery-autocomplete/jquery.autocomplete.css" />

    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=TxtStoreNo.ClientID %>").autocomplete('/page/store/Sch.ashx', {
                dataType: "json",
                matchContains: true,
                formatItem: function(row, i, max) {
                    return row.No + '[' + row.ID + ']';
                },
                formatMatch: function(row, i, max) {
                    return row.No + row.ID;
                },
                parse: function(data) {
                    return $.map(data, function(row) {
                        return {
                            data: row,
                            value: row.No,
                            result: row.No + " <ID:" + row.ID + ">"
                        }
                    });
                }
            });

        });    
    </script>

    <!--自动搜索完成控件                    end-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CallState ID="CallState1" runat="server" FocusItemIndex="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="8" style="height: 30px" class="td1_1">
                        <div class="querytable_title_name">
                            新建报修&nbsp&nbsp&nbsp&nbsp<asp:Label ID="labCalled" runat="server" Text=""></asp:Label>
                        </div>
                        <div style="margin: 0 10px 0 0; line-height: 24px;" align="right">
                            (店铺信息)
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        客户：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlCustomer"  runat="server" AutoPostBack="True"
                            DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        品牌：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlBrand"  runat="server" Enabled="false" AutoPostBack="True"
                            DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Literal ID="LtlBrandKnowledgeBase" runat="server"></asp:Literal>
                    </td>
                    <td class="td1_2">
                        省份：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlProvince"  runat="server" Enabled="false"
                            AutoPostBack="True" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        城市：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlCity"  runat="server" Enabled="false" AutoPostBack="True"
                            DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺名称：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlStore"  runat="server" Enabled="false" AutoPostBack="True"
                            DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        店铺编号：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxtStoreNo" runat="server" ></asp:TextBox>
                        <asp:Button ID="BtnStore" runat="server" Text=" 确定(a) " AccessKey="a" OnClick="BtnStore_Click" />
                    </td>
                    <td class="td1_2">
                        报修人：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="txtReporter" runat="server" ></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        系统单号：
                    </td>
                    <td class="td1_3">
                        <%=ViewState["No"]%>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        店铺类型：
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LtlStoreType" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="td1_2">
                        店铺电话：
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LtlTel" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="td1_2">
                        店铺地址：
                    </td>
                    <td colspan="3" class="td1_3">
                        <div style="width: 333px;">
                            <asp:Label ID="LtlAddress" runat="server" Text=""></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <div>
                &nbsp;</div>
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
                        <asp:DropDownList ID="ddlReportSource" runat="server" DataTextField="Name" DataValueField="ID"
                            >
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        来源单号：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="txtSourceNo" runat="server" ></asp:TextBox>
                    </td>
                    <td class="td1_2">
                        报修时间：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxtErrorDate" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        大类故障：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlClass1"  runat="server" DataTextField="Name"
                            DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlClass1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        中类故障：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlClass2"  runat="server" Enabled="false" DataTextField="Name"
                            DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlClass2_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        小类故障：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="ddlClass3"  runat="server" Enabled="false" DataTextField="Name"
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
                        <asp:TextBox ID="TxbSLA2" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" rowspan="2" >
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
                        <asp:TextBox ID="TxbL2Name" runat="server" AutoPostBack="True" OnTextChanged="TxbL2Name_TextChanged" Width="45px"></asp:TextBox>
                        <asp:DropDownList ID="ddlL2Id" runat="server" DataTextField="Name"
                            DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" >
                        服务类型：
                    </td>
                    <td colspan="3"  class="td1_3">
                        <asp:DropDownList ID="DdlCategory" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        建议解决方案
                    </td>
                    <td colspan="5" class="td1_3">
                        <div class="divSln">
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server"  OnClick="LinkButton1_Click" ToolTip="<%#Container.ItemIndex %>" CommandArgument='<%#Eval("ID") %>' >
                                    <%#Eval("Name") %>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div style=" clear:both;"></div>
                        </div>
                    </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="height: 30px" class="td1_1">
                            <asp:Button ID="BtnSubmit" runat="server" Text="保存,并交给技术中心处理(s)" CssClass="BigButton" AccessKey="s"
                                OnClick="BtnSubmit_Click" OnClientClick="this.style.display='none';return true;" />
                            <input type="button" value="重置(r)" class="BigButton" onclick="javascript:location.reload();" accesskey="r" />
                        </td>
                    </tr>
            </table>
                <uc2:ListRec ID="ListRec1" runat="server" UpdateMode="Conditional" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
