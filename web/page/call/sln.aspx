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
                <tr>
                    <td>
                    请在被叫号码输入框内输入要拨打的号码。待拨打的号码必须全部是数字，不得包含其它非数字内容。<br />
                    如被叫号码框内的内容是多个号码，请将其调整成一个合法号码后再点击拨打。<br /><br />
                    分机号输入框是您当前登录的呼叫中心分机号。
                    </td>                
                </tr>                
                <tr><td>
                     <div style="border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-bottom: 1px solid #ccc; border-top: 1px solid #ccc;
                         line-height: 22px; padding: 10px 10px;">
                        <asp:Label ID="labStation" runat="server" Text="分机号"></asp:Label><asp:TextBox ID="txtStation" runat="server"></asp:TextBox>
                        <asp:Label ID="labCalledNO" runat="server" Text="被叫号码"></asp:Label><asp:TextBox ID="txtCalledNO" runat="server"></asp:TextBox>
                        <input type="button" onclick="OutboundCall();" value="拨打" class="BigButton"/><input type="text" name="callbackrecordid" id="callbackrecid" value="" style="display:none"/>
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
    <script type="text/javascript">
        function WriteToSysLog(msg){
            $.ajax({
                type: 'post',
                url: '/Services/Log/WriteToSysLogHandler.ashx',
                data: { "category": "开始处理外呼", "msg":msg },
                async: false,
                success: function (data) {}
            });        
        }            
        function OutboundCall() {
            var dnis = $("#<%=txtCalledNO.ClientID %>").val();
            if ( !dnis || dnis == "" )
                {alert("请输入被叫号码，否则无法执行外呼!");return;}
            var ani = $("#<%=txtStation.ClientID %>").val();
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
                            WriteToSysLog("开始处理外呼成功：" + data + "  url:" + originalUrlStr + callIDStr);
                            alert("外呼成功。");
                        }
                        else{
                            WriteToSysLog("开始处理外呼失败：" + data + "  url:" + originalUrlStr + callIDStr)
                            alert("外呼失败。" + data + "。");
                        }
                    }
                    else{
                        WriteToSysLog("开始处理外呼失败：" + data + "  url:" + originalUrlStr + callIDStr);
                        alert("外呼失败，请确认主被叫号码正确无误，或请稍后再试。");
                    }
                },
                error:function(xhr, errormsg, e) {
                        WriteToSysLog("开始处理外呼失败无响应。"+errormsg?"":errormsg + "  url:" + originalUrlStr + callIDStr);
                        alert("外呼请求无响应，请稍后再试。");
                }
            });
        }
    </script>    
</asp:Content>
