<%@ page language="C#" autoeventwireup="true" inherits="page_call_MobileMsg, App_Web_isgyizrg" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/Site.css" rel="stylesheet" />
    <link href="/css/admin.css" rel="stylesheet" />
    <script type="text/javascript">
        function AlertAndExit()
        {
            alert("短信发送成功");
            self.parent.tb_remove();
            return false;
        }
    </script>
</head>
<body  style=" width:325px; overflow:hidden;">
    <form id="form1" runat="server">
        <div style=" width:325px; overflow:hidden;">
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="5" style="height: 30px" class="td1_1">短信发送
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        接收人
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbTel" runat="server" Width="267px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        内容
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbContent" runat="server" TextMode="MultiLine" Height="250px" Width="277px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-left:40px;">
                        <asp:Button ID="BtnSend" runat="server" Text="发送短信" Height="27px" Width="115px" OnClick="BtnSend_Click" />
                        <div>
                            <asp:Label ID="LabResult" runat="server" Text="" CssClass="red2"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
