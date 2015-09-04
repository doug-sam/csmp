<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Task_Edit, App_Web_awp-hkwo" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="4" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>系统定时任务
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        任务名：
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:TextBox ID="TxbName" placeholder="标识，为了更容易记住它是做神马的" runat="server" replaceholder="A1" Width="549px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        循环周期：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCycleMode" DataTextField="value" DataValueField="key" runat="server"></asp:DropDownList>
                    </td>
                    <td class="td1_2">
                        每次执行间隔时间：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbIntervalTime" runat="server" placeholder="1"></asp:TextBox>依赖于循环周期，
                        <div>小时为周期则间隔多少小时</div>
                        <div>天为周期则间隔多少天</div>
                        <div>周为周期则间隔多少周</div>
                        <div>月为周期则间隔多少月</div>
                        <div>支持小数</div>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        具体执行时间点：
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:TextBox ID="TxbExcuteTime" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
                        依赖于循环周期
                        <div>小时周期跟循环周期脱离关系</div>
                        <div>天周期则关于在于具体的“时：分”</div>
                        <div>周周期则关于在于具体的“星期几 时：分”</div>
                        <div>月周期则关于在于具体的“日 时：分”，如果当前没有这一天（例如31号），则会跳过</div>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        上次执行时间：
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:TextBox ID="TxbExcuteTimeLast" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" placeholder="可以控制具体开始执行的时间点"></asp:TextBox>
                        <div>当你是添加新的任务时，这个时间定为系统当前时间即可</div>
                        <div>当你是编辑现有任务时，这个时间可以告诉当前任务最后一次执行是什么时候，但不建议你进行编辑修改</div>
                        <div>管理员为了更好地测试任务，不希望每每等待指定时间后再去执行，可以手动调整时间，</div>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        执行地址：
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:TextBox ID="TxbURL" runat="server" placeholder="这个地址需要开发者针对新的功能进行开发" Width="532px"></asp:TextBox>
                        
                    </td>
                </tr>
                 <tr>
                    <td class="td1_2">
                        是否可用：
                    </td>
                    <td class="td1_3" colspan="3">
                        <asp:CheckBox ID="CbEnable" runat="server" Text="可用"  />
                    </td>
                </tr>
               <tr>
                    <td class="td1_2" colspan="4">
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交" CssClass="BigButton" OnClick="BtnSubmit_Click" />
                    </td>
                </tr>
            </table>
</asp:Content>
