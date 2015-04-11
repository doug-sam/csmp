<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="ThirdPartyLeave.aspx.cs" Inherits="page_CallStep_ThirdParty" %>

<%@ Register Src="/Controls/CallState.ascx" TagName="CallState" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .VDateDate
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CallState ID="CallState1" runat="server" FocusItemIndex="2" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <%=CSMP.Model.SysEnum.StepType.第三方处理离场 %>
                    </td>
                </tr>
                <%--<tr>
                    <td class="td1_2">
                        到场时间
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateArrive" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" runat="server" CssClass="VDateDate"></asp:TextBox>
                    </td>
                </tr>--%>
                <tr>
                    <td class="td1_2">
                        离场时间
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDateLeave" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" runat="server" CssClass="VDateDate"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        备注
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDetail" TextMode="MultiLine" runat="server" CssClass="VDateDate" Height="77px" Width="237px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        是否解决
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
                       <div style=" float:right; height:22px; line-height:22px; margin:10px;"><a href='../call/slnDropInCancel.aspx?ID=<%=GetInfo().ID %>'>取消上门</a></div> 
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton" OnClick="BtnSubmit_Click"
                            AccessKey="s" OnClientClick="$(this).hide();return true;" />
                        
                        <input type="button" value="记录备注" class="BigButton" 
                        onclick="javascript:tb_show('记录备注', '/page/CallStep/ThirdPartyMemo.aspx?ID=<%=GetInfo().ID %>&TB_iframe=true&height=450&width=730', false);"/>
                    </td>
                </tr>
            </table>
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
