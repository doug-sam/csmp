<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" CodeFile="StepSwitchAfterFinish.aspx.cs" Inherits="StepSwitchAfterFinish" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .td1_3 div input {
            width:190px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:updatepanel id="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2"  style="height: 30px" class="td1_1">
                        报修处理
                    </td>
                    </tr>
                <tr>
                    <td  class="td1_2" style="width:180px;">
                        <div style="line-height:23px;">报修服务已完成，接下来您想</div>
                    </td>
                    <td class="td1_3">
                        <div>
                            <input type="button" id="BtnReplacement" class="BigButton" value="记录备件流程(r)" accesskey="r" onclick="BtnReplacement_Click();" />
                        </div>
                        <div>
                            <input type="button" id="BtnFeedback" class="BigButton" value="回访(f)" accesskey="f" onclick="BtnFeedback_Click();" />
                        </div>
                        <div>
                            <input type="button" id="BtnClose" class="BigButton" value="关闭(c)" accesskey="c" onclick="BtnClose_Click();" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td1_3" colspan="2">
                                                <div style="text-align:right;line-height:30px;">
                          <span style="font-weight:bold;font-size:14px;">Ps:</span>  系统中很多地方的按钮里有个括号，里头有个字母
                            那是想告诉你，用IE、chrome浏览器时候，
                            按住alt+按钮里的字母则可以作为快捷键输入的
                        </div>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:updatepanel>
    <script type="text/javascript">
        function SetHaveFeedback() {
            $("#BtnFeedback").val("已回访");
        }
        function BtnReplacement_Click() {
            location.href = '/page/callStep/StepReplacement.aspx?ID=<%=GetInfo().ID %>';
                //tb_show('备件情况跟进记录', '/page/callStep/StepReplacement.aspx?ID=<%=GetInfo().ID %>&TB_iframe=true&height=450&width=730', false);
            }
        function BtnFeedback_Click() {
            location.href = '/page/callStep/Feedback.aspx?ID=<%=GetInfo().ID %>';
            }

            function BtnClose_Click() {

                location.href = '/page/call/Close.aspx?ID=<%=GetInfo().ID %>';
            //tb_show('关闭', '/page/call/Close.aspx?ID=<%=GetInfo().ID %>&TB_iframe=true&height=450&width=730', false);
        }
    </script>
</asp:Content>
