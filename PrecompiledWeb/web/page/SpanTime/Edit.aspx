<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_SpanTime_Edit, App_Web_4uby5vwi" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<%@ Register Src="list.ascx" TagName="list" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>


    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1" runat="server"
        id="Tb_Add">
        <tr>
            <td colspan="6" style="height: 30px" class="td1_1">
                <div style="float: left;">
                    SLA恢复
                </div>
                <div style="float: right; cursor: pointer;">
                    <a href="#" onclick="self.parent.tb_remove();return false;">关闭</a>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                开始时间
            </td>
            <td class="td1_3">
                <asp:Label ID="LabDateBegin" runat="server" Text=""></asp:Label>
            </td>
            <td class="td1_2">
                到
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateEnd" runat="server" CssClass="DateEnd" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
                预定到<asp:Label ID="LabDateEnd" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="td1_2" colspan="4">
                               <asp:Button ID="BtnSubmit" runat="server" Text=" 我要现在停止计时 " CssClass="BigButton" OnClick="BtnSubmit_Click" />

            </td>
        </tr>

        </tr>
    </table>
    <uc1:list ID="UClist1" runat="server" />
</asp:Content>
