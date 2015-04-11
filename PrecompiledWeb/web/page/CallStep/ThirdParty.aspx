<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_CallStep_ThirdParty, App_Web_mh7akdho" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

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
          </ContentTemplate>
    </asp:UpdatePanel>
          <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <%=CSMP.Model.SysEnum.StepType.第三方预约上门 %>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        第三方选择
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlThirdParty" runat="server" DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                        <asp:Label ID="LabThirdParty" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        预约上门时间
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" runat="server" CssClass="VDateDate"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        备注
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbDetails" TextMode="MultiLine" runat="server" CssClass="VDateDate" Height="77px" Width="237px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton" OnClick="BtnSubmit_Click"
                            AccessKey="s" OnClientClick="$(this).hide();return true;" />
                        
                        <%--<span class="red2" style=" display:none;">短信正在发送。请等上几秒。</span>--%>
                    </td>
                </tr>
            </table>
            
            
</asp:Content>
