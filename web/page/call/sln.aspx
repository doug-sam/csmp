<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" CodeFile="sln.aspx.cs" Inherits="page_call_sln" %>

<%@ Register Src="../../Controls/CallState.ascx" TagName="CallState" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CallState ID="CallState1" runat="server" FocusItemIndex="2" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td  style="height: 30px" class="td1_1">
                        外呼
                    </td>
                </tr>
                <tr><td>
                     <div style="border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; border-top: 1px solid #ccc;
                         line-height: 22px; padding: 10px 10px;">
                        <asp:Label ID="labStation" runat="server" Text="分机号："></asp:Label><asp:TextBox ID="txtStation" runat="server"></asp:TextBox>
                        <asp:Label ID="labCalledNO" runat="server" Text="被叫号码："></asp:Label><asp:TextBox ID="txtCalledNO" runat="server"></asp:TextBox>
                        <asp:Button ID="btnDoCall" runat="server" Text="拨打" />
                    </div>
            </td></tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        报修处理
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" style=" padding:10px;">
                        请选择你接下来的处理办法
                    </td>
                    <td class="td1_3">
                        <div>
                            <asp:TextBox ID="TxbDetail" Enabled="false" Text="开始处理报修" TextMode="MultiLine" runat="server" Height="67px" Width="271px"></asp:TextBox>
                        </div>
                        <asp:Button ID="BtnDeal"  Visible="false" runat="server" CssClass="BigButton" Text=" 开始处理 " OnClick="BtnDeal_Click" OnClientClick="this.hide();return true;" />
                        <asp:DropDownList ID="DdlSln" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlSln_SelectedIndexChanged">
                            <asp:ListItem Text="请选择处理方法" Value="0"></asp:ListItem>
                            <asp:ListItem Text="远程支持" Value="1"></asp:ListItem>
                            <asp:ListItem Text="升级到客户" Value="2"></asp:ListItem>
                            <asp:ListItem Text="上门" Value="3"></asp:ListItem>
                            <asp:ListItem Text="第三方上门" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
