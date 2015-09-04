<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_slnDropIn, App_Web_isgyizrg" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<%@ Register Src="/Controls/CallState.ascx" TagName="CallState" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CallState ID="CallState1" runat="server" FocusItemIndex="2" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        上门解决(提交备件情况)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        备件情况
                    </td>
                    <td class="td1_3">
                        <asp:CheckBox ID="CbHardWare" runat="server" Text="不需备件或已准备好备件"  oncheckedchanged="CbHardWare_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        初步预约上门时间
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" runat="server" CssClass="VDateDate"></asp:TextBox>
                    </td>
                </tr>
                <tr id="TR_Hardware" runat="server">
                    <td class="td1_2">
                        维修建议及工作说明
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbHardWare" runat="server" TextMode="MultiLine" Height="60px" 
                            Width="429px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton"  OnClick="BtnSubmit_Click"
                        OnClientClick="$(this).hide();return true;" 
                            AccessKey="s" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
