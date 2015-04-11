<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Add.aspx.cs" Inherits="page_SpanTime_Add" %>

<%@ Register Src="list.ascx" TagName="list" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>

    <script>
        $(document).ready(function() { $("#inputReason").hide(); });
        function CalcDateDiff() {
            var DateBegin = new Date($(".DateBegin").val());
            var DateEnd = new Date($(".DateEnd").val());
            $("#span_TimeSpan").text((DateEnd - DateBegin) / 3600000);
            if (DateEnd - DateBegin <= 0) {
                alert("日期有误，开始时间不能大于结束时间");
            }
        }
        function Select_Change(Sender) {
            if (Sender.value == '其它原因') {
                $("#inputReason").show();
            }
            else $("#inputReason").hide();
        }
    </script>

    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1" runat="server"
        id="Tb_Add">
        <tr>
            <td colspan="6" style="height: 30px" class="td1_1">
                <div style="float: left;">
                    暂停SLA计算
                </div>
                <div style="float: right; cursor: pointer;">
                    <a href="#" onclick="self.parent.tb_remove();return false;">关闭</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                暂停时间从
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateBegin"  Enabled="false" runat="server" CssClass="DateBegin" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
            <td class="td1_2">
                到
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateEnd" runat="server" CssClass="DateEnd" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                    onchange="CalcDateDiff();"></asp:TextBox>
                共<span id="span_TimeSpan">8</span>小时
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                暂停原因
            </td>
            <td class="td1_3" colspan="3">
                <select id="selectReason" name="inputReason" onchange="Select_Change(this);">
                    <option value="隔夜服务">隔夜服务</option>
                    <option value="等待客户备件">等待客户备件</option>
                    <option value="其它原因">其它原因</option>
                </select>
                <input id="inputReason" name="inputReason" type="text" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                备注
            </td>
            <td class="td1_3" colspan="3">
                <asp:TextBox ID="TxbMemo" TextMode="MultiLine" runat="server" Height="86px" Width="394px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_3" colspan="4">
                <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " CssClass="BigButton" OnClick="BtnSubmit_Click" />
                <div>
                    说明：<br />
                    1：如果你不确定什么时候恢复计时，你可以把恢复时间设置长久些，以后再手动停止它。<br />
                    2:
                </div>
            </td>
        </tr>
    </table>
    <uc1:list ID="UClist1" runat="server" />
</asp:Content>
