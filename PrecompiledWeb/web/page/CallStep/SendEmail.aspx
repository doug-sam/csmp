<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="CallStep_SendEmail, App_Web_tm5a1rj7" validaterequest="false" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script type="text/javascript" charset="utf-8" src="/js/kindeditor/kindeditor.js"></script>--%>
        <link rel="stylesheet" href="/js/kindeditor-4.1.7/themes/default/default.css" />
		<script charset="utf-8" src="/js/kindeditor-4.1.7/kindeditor-min.js"></script>
		<script charset="utf-8" src="/js/kindeditor-4.1.7/lang/zh_CN.js"></script>
        <script type="text/javascript">
            function InitData(v_Action) {
                var htmlobj = $.ajax({ url: "/page/WorkGroupEmail/initData.ashx?Action=" + v_Action, async: false });
                //alert(htmlobj.responseText);
                $(".Action" + v_Action).val(htmlobj.responseText);
                //TODO：下周一
            }
    </script>
    <script type="text/javascript">
        function SetAttachmentID(AttID)
        {
            var v_TextOld = $(".TxbAttachment").val();
            $(".TxbAttachment").val(v_TextOld + AttID + ",");
        }
    </script>
    <script>
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('#<%=TxbContent.ClientID %>', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                width: 750,
                height: 450
            });
        });


        function BtnSubmit_ClientClick() {
            document.getElementById('<%=BtnSubmit.ClientID%>').style.display = "none";
            var ConfirmResult = confirm("你确定发送邮件吗？你可以通过【回车】快捷键确定发送哦");
            if (!ConfirmResult) {
                document.getElementById('<%=BtnSubmit.ClientID%>').style.display = "inline";
            }
            return ConfirmResult;
        }
        function BtnSubmit_Clicked() {
            document.getElementById('<%=BtnSubmit.ClientID%>').style.display = "block";
        }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                发邮件
            </td>
        </tr>
        <tr>
            <td   class="td1_2" rowspan="5" style="width:80px;">
                 <asp:Button ID="BtnSubmit" runat="server" Text="发送(s)" OnClientClick="return BtnSubmit_ClientClick();" AccessKey="s" onclick="BtnSubmit_Click" CssClass="BigButton" style="padding:0px;" Height="104px" Width="76px" />
            </td>
            <td   class="td1_2">
                <a href="javascript:tb_show('从联系人中查找', '/page/WorkGroupEmail/View.aspx?Action=To&TB_iframe=true&height=450&width=730', false);">收件人</a>
            </td>
            <td   class="td1_3">
                <asp:TextBox ID="TxbToAddress" CssClass="ActionTo" runat="server" Width="560px" TextMode="MultiLine" Height="37px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td   class="td1_2">
                <a href="javascript:tb_show('从联系人中查找', '/page/WorkGroupEmail/View.aspx?Action=CC&TB_iframe=true&height=450&width=730', false);">抄送</a>
            </td>
            <td   class="td1_3">
                <asp:TextBox ID="TxbCC" CssClass="ActionCC" runat="server" Width="560px" TextMode="MultiLine" Height="37px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td   class="td1_2">
                主题
            </td>
            <td   class="td1_3">
                <asp:TextBox ID="TxbSubject" runat="server" Width="560px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td   class="td1_2"  rowspan="2">
                附件
            </td>
            <td   class="td1_3">
                <asp:RadioButtonList ID="RblTemplateID" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="合胜模板" Value="1"  Selected="True" ></asp:ListItem>
                    <asp:ListItem Text="IBM模板" Value="2"></asp:ListItem>
                    <asp:ListItem Text="不添加服务单附件" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td  class="td1_3">
              附件ID:  <asp:TextBox ID="TxbAttachment" runat="server" Width="210px" CssClass="TxbAttachment"></asp:TextBox>(多个ID请用英文逗号隔开)
                <a href="javascript:tb_show('', '/page/Attachment/Upload.aspx?returnID=true&CallID=<%=GetInfo().ID %>&UserFor=<%=(int)CSMP.Model.AttachmentInfo.EUserFor.Call %>&TB_iframe=true&height=450&width=730', false);">添加附件</a>
            </td>
        </tr>
        <tr>
            <td   class="td1_3" colspan="3">
                <asp:TextBox ID="TxbContent" TextMode="MultiLine" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  class="td1_1" colspan="3">
                <asp:Label ID="LabError" runat="server" Text="" CssClass="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
