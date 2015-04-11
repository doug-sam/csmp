<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_slnDropIn2, App_Web_rursbog1" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

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
                        工程师上门(工程师抵达)
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        实际上门工程师
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlUser" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        抵达现场时间
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDate" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                            Width="193px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        过程说明
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDetails" runat="server" TextMode="MultiLine" Text="工程师到达门店处理。"
                            Width="398px" Height="130px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                       <div style=" float:right; height:22px; line-height:22px; margin:10px;"><a href='slnDropInCancel.aspx?ID=<%=GetInfo().ID %>'>取消上门</a></div> 
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton"  OnClick="BtnSubmit_Click"
                            AccessKey="s"  OnClientClick="$(this).hide();return true;" />
                        <input type="button" value="记录备注" class="BigButton" 
                        onclick="javascript:tb_show('记录备注', '/page/DropInMemo/list.aspx?StepID=<%=GetLastStepID() %>&TB_iframe=true&height=450&width=730', false);"/>
                   
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
