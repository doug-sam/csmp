<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Add.aspx.cs" Inherits="page_CustomerRequest_Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!--自动搜索完成控件                    begin-->

    <script type='text/javascript' src='/js/jquery-autocomplete/jquery.autocomplete.js'></script>

    <link rel="stylesheet" type="text/css" href="/js/jquery-autocomplete/jquery.autocomplete.css" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=TxtStoreNo.ClientID %>").autocomplete('/page/store/Sch.ashx', {
                dataType: "json",
                matchContains: true,
                formatItem: function (row, i, max) {
                    return row.No + '[' + row.ID + ']';
                },
                formatMatch: function (row, i, max) {
                    return row.No + row.ID;
                },
                parse: function (data) {
                    return $.map(data, function (row) {
                        return {
                            data: row,
                            value: row.No,
                            result: row.No
                        }
                    });
                }
            });

        });
    </script>

    <!--自动搜索完成控件                    end-->

    <script type="text/javascript">
        var IsTxbVisible = true;
        function BtnChangeSch_Click() {
            if (IsTxbVisible) {
                $("#tr_Txb").hide();
                $("#tr_Ddl").show();
                $("#BtnChangeSch").val("按店铺号搜索");
                IsTxbVisible = false;
            }
            else {
                $("#tr_Txb").show();
                $("#tr_Ddl").hide();
                $("#BtnChangeSch").val("按省份城市搜索");
                IsTxbVisible = true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="2" style="height: 30px" class="td1_1">新建 报修请求
            </td>
        </tr>
        <tr>
            <td colspan="2" class="td1_2">
                <input accesskey="c" type="button" value="找不到，我要按省份城市搜索(c)" class="BigButton" id="BtnChangeSch" onclick="BtnChangeSch_Click();" />
            </td>
        </tr>
        <tr id="tr_Txb">
            <td class="td1_2">店铺编号：
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxtStoreNo" runat="server" Width="229px"></asp:TextBox>
                <asp:Button ID="BtnStore" runat="server" Text="确定(s)" CssClass="BigButton" AccessKey="s" OnClick="BtnStore_Click" />
                <asp:Label ID="LabStoreMsg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr id="tr_Ddl" style="display: none; height: 45px; line-height: 45px;">
            <td class="td1_2">选择店铺：
            </td>
            <td class="td1_3">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        省份 
                        <asp:DropDownList ID="DdlProvince" runat="server" AutoPostBack="True" DataTextField="name" DataValueField="id" OnSelectedIndexChanged="DdlProvince_SelectedIndexChanged"></asp:DropDownList>
                        城市 
                        <asp:DropDownList ID="DdlCity" runat="server" AutoPostBack="True" DataTextField="name" DataValueField="id" OnSelectedIndexChanged="DdlCity_SelectedIndexChanged"></asp:DropDownList>
                        店铺 
                        <asp:DropDownList ID="DdlStore" runat="server" DataTextField="name" DataValueField="id"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="td1_2">详细内容：
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDetail" TextMode="MultiLine" runat="server" Height="104px" Width="359px"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td class="td1_2">联系电话：
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbTel" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">报修人：
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbErrorReportUser" Text="" onclick="this.select();" runat="server"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td class="td1_2" colspan="2">
                <asp:Button ID="BtnSubmit" runat="server" Text="提交(a)" CssClass="BigButton" AccessKey="a"
                    OnClick="BtnSubmit_Click" />
                <input type="button" value="重置(r)" class="BigButton" onclick="javascript: location.reload();" accesskey="r" />
            </td>
        </tr>
    </table>
</asp:Content>
