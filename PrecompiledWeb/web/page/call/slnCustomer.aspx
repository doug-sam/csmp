<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_slnCustomer, App_Web_rursbog1" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<%@ Register Src="/Controls/CallState.ascx" TagName="CallState" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CallState ID="CallState1" runat="server" FocusItemIndex="2" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="2" style="height: 30px" class="td1_1">
                升级客户记录
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                问题实际升级时间
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDate" runat="server" 
                    onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" Width="193px" ></asp:TextBox>
                    
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                过程备注
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDetails" TextMode="MultiLine" runat="server" Height="73px" 
                    Width="424px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                客户是否自行解决
            </td>
            <td class="td1_3">
                <asp:RadioButtonList ID="RblSolved" runat="server">
                    <asp:ListItem  Text="已解决" Value="1"></asp:ListItem>
                    <asp:ListItem  Text="未解决" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
         <tr>
            <td class="td1_2" colspan="2">
                <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton" OnClick="BtnSubmit_Click" AccessKey="s"
                 OnClientClick="$(this).hide();return true;" />
            </td>
        </tr>       
    </table>
</asp:Content>
