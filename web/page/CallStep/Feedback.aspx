<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Feedback.aspx.cs" Inherits="page_callStep_Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1" id="TableCall" runat="server">
        <tr>
            <td colspan="5" class="td1_2">
            请在被叫号码输入框内输入要拨打的号码。待拨打的号码必须全部是数字，不得包含其它非数字内容。<br />
            如被叫号码框内的内容是多个号码，请将其调整成一个合法号码后再点击拨打。<br /><br />
            分机号输入框是您当前登录的呼叫中心分机号。
            </td>                
        </tr>    
        <tr>
            <td class="td1_2">
                被叫号码
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbTel" CssClass="myTel" runat="server" 
                    Width="200px"></asp:TextBox>
            </td>
            <td class="td1_2">
                分机号
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbExt" CssClass="myExtensionID" runat="server" 
                    Width="120px"></asp:TextBox>
            </td>            
            <td class="td1_2">
                <input type="button" onclick="OutboundCall();" value=" 拨打电话回访 " class="BigButton" /><input type="text" name="callbackrecordid" id="callbackrecid" value="" style="display:none"/>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="2" style="height: 30px" class="td1_1">
                店铺回访
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                是否需要重新报修
            </td>
            <td class="td1_3">
                <asp:CheckBox ID="CbNewCall" runat="server" Text="需要重新报修" />
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td style="height: 30px" class="td1_1">
                问卷调查
            </td>
        </tr>
        <tr>
            <td  class="td1_2">
                <asp:Label ID="LabPaperMeno" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td  class="td1_2">
                回访人姓名或工号：<asp:TextBox ID="TxbFeedbackUserName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <asp:Repeater ID="RpRadio" runat="server" OnItemDataBound="RpRadio_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="td1_3">
                     <%#Container.ItemIndex+1 %>、   <%#Eval("Name") %>
                     <div>
                         <asp:RadioButtonList ID="RadioButtonList1" DataTextField="name" DataValueField="ID" 
                         runat="server" RepeatDirection="Horizontal"  >
                         </asp:RadioButtonList>
                        <asp:Label ID="LabQuestionID" runat="server" Text='<%#Eval("ID") %>' style=" display:none;"></asp:Label>
                     </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="RpEssay" runat="server"  OnItemDataBound="RpEssay_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="td1_2">
                   问  <%#Container.ItemIndex+1 %>、   <%#Eval("Name") %>
                    </td>
                </tr>
                <tr>
                    <td class="td1_3">
                     答   <asp:TextBox ID="TxbAnswer" runat="server" TextMode="MultiLine" Height="36px" Width="100%"></asp:TextBox>
                     <asp:Label ID="LabQuestionID" runat="server" Text='<%#Eval("ID") %>' style=" display:none;"></asp:Label>
                    </td>
                </tr>
                <tr></tr>
            </ItemTemplate>
        </asp:Repeater>
        
    </table>
    <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" AccessKey="s" CssClass="BigButton" OnClick="BtnSubmit_Click" />

    <script type="text/javascript">
        function BtnCall_Click() {
            var Tel = $(".myTel").val();
            window.clipboardData.setData('Text', "'AAAA$$$$BBBB$$$$CCCC::" + Tel + "'");
        }
        function WriteToSysLog(msg){
            $.ajax({
                type: 'post',
                url: '/Services/Log/WriteToSysLogHandler.ashx',
                data: { "category": "回访外呼", "msg":msg },
                async: false
            });
        }
        function OutboundCall() {
            var dnis = $(".myTel").val();
            if ( !dnis || dnis == "" )
                {alert("请输入被叫号码，否则无法执行外呼!");return;}
            var ani = $(".myExtensionID").val();
            if ( !ani || ani == "" )
                {alert("请输入您登录的分机号，否则无法执行外呼!");return;}
            var callIDStr = "  CallID:<%= CallID %>";
            var originalUrlStr = "http://<%= CTIWSIP %>:<%= CTIWSPort %>/?<%= CTIWSObDnisName %>=" + dnis + "&<%= CTIWSObAniName %>=" + ani;
            var urlStr = encodeURIComponent(originalUrlStr);
            $.ajax({
                url: "../../Services/GetHttpDataNoPage.aspx?param=" + Math.random() + "&url=" + urlStr,
                success: function(data) {
                    var receivedstr = data;
                    var pos = receivedstr.indexOf("error#");
                    if ( pos != 0 ){
                        pos = receivedstr.indexOf("#");
                        if ( pos != -1 )  {
                            $("#callbackrecid").val(data);
                            WriteToSysLog("回访外呼成功：" + data + "  url:" + originalUrlStr + callIDStr);
                            alert("回访外呼成功。");
                        }
                        else{
                            WriteToSysLog("回访外呼失败：" + data + "  url:" + originalUrlStr + callIDStr)
                            alert("回访外呼失败。" + data + "。");
                        }
                    }
                    else{
                        WriteToSysLog("回访外呼失败：" + data + "  url:" + originalUrlStr + callIDStr);
                        alert("外呼失败，请确认主被叫号码正确无误，或请稍后再试。");
                    }
                },
                error:function(xhr, errormsg, e) {
                        WriteToSysLog("回访外呼失败无响应。"+errormsg?"":errormsg + "  url:" + originalUrlStr + callIDStr);
                        alert("外呼请求无响应，请稍后再试。");
                }
            });
        }
    </script>

</asp:Content>
