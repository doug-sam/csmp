<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_slnDropIn4, App_Web_rursbog1" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

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
                        评分
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        现场工程师技术评价
                    </td>
                    <td class="td1_3">
                        <asp:RadioButtonList ID="RblStar2" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="1分" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2分" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3分" Value="3" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="4分" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5分" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        现场工程师态度评价
                    </td>
                    <td class="td1_3">
                        <asp:RadioButtonList ID="RblStar3" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="1分" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2分" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3分" Value="3" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="4分" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5分" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>

            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        上门解决(离场跟进)
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        实际上门工程师
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabUser" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        离开现场时间
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateEnd" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                            Width="193px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        跟进备注
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDetails" TextMode="MultiLine" runat="server" Height="73px" Width="424px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        问题是否解决
                    </td>
                    <td class="td1_3">
                        <asp:RadioButtonList ID="RblSolved" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RblSolved_SelectedIndexChanged">
                            <asp:ListItem Text="已解决" Value="1"></asp:ListItem>
                            <asp:ListItem Text="未解决" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                        <div>
                            <asp:CheckBox ID="CbSendZara" Text="邮件通知Zara服务已解决" runat="server" Visible="false" />
                        </div>
                    </td>
                </tr>
                <tr id="Tr_Sln" runat="server" visible="false">
                    <td class="td1_2">
                        解决方案
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlSln" runat="server" AutoPostBack="True"  DataTextField="Name" DataValueField="ID" 
                        OnSelectedIndexChanged="DdlSln_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:TextBox ID="TxbSln" runat="server" Visible="false" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton" OnClick="BtnSubmit_Click"
                            AccessKey="s"  OnClientClick="$(this).hide();return true;" />


                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
