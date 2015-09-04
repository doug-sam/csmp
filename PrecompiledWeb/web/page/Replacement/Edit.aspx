<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Replacement_Edit, App_Web_nt_lq0xu" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">备件跟踪记录
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">备件
                        <div>选择你要跟踪的备件</div>
                    </td>
                    <td class="td1_3">
                        <div style="float:left;margin: 20px 0px;">
                            <asp:DropDownList ID="DdlSerialNo" runat="server"></asp:DropDownList>
                            <asp:TextBox ID="TxbSerialNo" runat="server" Visible="false" Width="310px"></asp:TextBox>
                        </div>
                        <div style="float: right; margin: 5px 0px;">
                            <asp:Button ID="BtnReplacement" runat="server" Text="新备件记录&gt;&gt;" CssClass="BigButton" OnClick="BtnReplacement_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">事件时间
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDate" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                            Width="193px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">情况说明
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDetails" TextMode="MultiLine" runat="server" Height="73px" Width="424px"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="td1_2">备件负责人
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlUser" DataTextField="Name" DataValueField="ID" runat="server"></asp:DropDownList>
                    </td>
                </tr>
               <tr >
                    <td class="td1_2">是否完成跟进
                    </td>
                    <td class="td1_3">
                        <asp:CheckBox ID="CbFinish" runat="server" Text="备件跟进已完成" />
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton" OnClick="BtnSubmit_Click" AccessKey="s"
                            OnClientClick="$(this).hide();return true;" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSubmit" />
        </Triggers>
    </asp:UpdatePanel>

    
</asp:Content>
