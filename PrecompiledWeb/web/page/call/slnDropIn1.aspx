<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_slnDropIn1, App_Web_rursbog1" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<%@ Register Src="/Controls/CallState.ascx" TagName="CallState" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--自动搜索完成控件                    begin-->

    <script type='text/javascript' src='/js/jquery-autocomplete/jquery.autocomplete.js'></script>

    <link rel="stylesheet" type="text/css" href="/js/jquery-autocomplete/jquery.autocomplete.css" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=TxbJobCode.ClientID %>").autocomplete('/page/Jobcode/Sch.ashx', {
                dataType: "json",
                matchContains: true,
                formatItem: function (row, i, max) {
                    return row.CodeNo;
                },
                formatMatch: function (row, i, max) {
                    return row.CodeNo + row.ID;
                },
                parse: function (data) {
                    return $.map(data, function (row) {
                        return {
                            data: row,
                            value: row.CodeNo,
                            result: row.CodeNo
                        }
                    });
                }
            }).result(function (event, row, formatted) { $("#DivJobCode").html("实际费用：" + row.Money + "元；响应要求为：" + row.TimeAction + "；到场工作时间为：" + row.TimeArrive + "小时；"); });

        });
    </script>

    <!--自动搜索完成控件                    end-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CallState ID="CallState1" runat="server" FocusItemIndex="2" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        上门解决(安排上门)
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        备件情况
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabHardWare" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        备件详细及工作说明
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabDetails" runat="server" Text="" CssClass="Details"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        JobCoad
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbJobCode" runat="server" CssClass="CssJobcode"></asp:TextBox>
                        <div id="DivJobCode"></div>
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
                        上门工程师
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlActualize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlActualize_SelectedIndexChanged">
                            <asp:ListItem Text="请选择" Value="0"></asp:ListItem>
                            <asp:ListItem Text="当前工程师上门" Value="1"></asp:ListItem>
                            <asp:ListItem Text="转派第三方" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlProvince" runat="server" AutoPostBack="True" DataTextField="Name"
                            DataValueField="ID" Visible="false" OnSelectedIndexChanged="DdlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlWorkGroup" runat="server" AutoPostBack="True" Visible="false"
                            DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="DdlWorkGroup_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="DdlMajorUserID" runat="server" DataTextField="Name" DataValueField="ID"
                            Visible="false" AutoPostBack="True" OnSelectedIndexChanged="DdlMajorUserID_SelectedIndexChanged" CssClass="DropInUserName">
                        </asp:DropDownList>
                        <asp:Label ID="LabTel" runat="server" Text="" CssClass="Tel"></asp:Label>
                        <asp:Panel ID="PanelSendMsg" runat="server" Visible="false">
                            <input type="button" name="发送短信给对方" value="发送短信给对方" class="SendMsg" onclick="ShowMobileMsg();" />
                        </asp:Panel>
                        <script type="text/javascript">

                            function ShowMobileMsg()
                            {
                                var v_DateDate = $(".VDateDate").val();
                                var v_Tel = $(".Tel").html();
                                var v_Details = $(".Details").html();
                                var v_DropInUserName = $(".DropInUserName").find("option:selected").text();
                                var v_CallID = Request("ID");

                                var v_URL = "/page/call/MobileMsg.aspx?";
                                v_URL += "DateDate=" + v_DateDate;
                                v_URL += "&Tel=" + v_Tel;
                                v_URL += "&Details=" + v_Details;
                                v_URL += "&DropInUserName=" + v_DropInUserName;
                                v_URL += "&CallID=" + v_CallID;
                                v_URL += "&TB_iframe=true&height=400&width=300";
                                v_URL = encodeURI(v_URL);
                                //alert(v_URL);
                                tb_show('短信发送', v_URL, false);
                            }
                            function SendEmail()
                            {
                                var v_DateDate = $(".VDateDate").val();
                                var v_Tel = $(".Tel").html();
                                var v_Details = $(".Details").html();
                                var v_DropInUserID = $(".DropInUserName").find("option:selected").val();
                                var v_CallID = Request("ID");
                                var v_JobCode = $(".CssJobcode").val();

                                var v_URL = "/page/callStep/SendEmail.aspx?";
                                v_URL += "DateDate=" + v_DateDate;
                                v_URL += "&DropInUserID=" + v_DropInUserID;
                                v_URL += "&CallID=" + v_CallID;
                                v_URL += "&JobCode=" + v_JobCode;
                                v_URL += "&Details=" + encodeURIComponent(v_Details);
                                window.open(v_URL, "_black");
                                //v_URL += "&TB_iframe=true&height=450&width=730";
                                //v_URL = encodeURI(v_URL);
                                //tb_show('邮件发送', v_URL, false);
                            }

                        </script>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        服务单发送选择
                    </td>
                    <td class="td1_3">
                        <input type="button" name="发送邮件" value="发送邮件" class="SendMsg" onclick="SendEmail();" />
                        
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                       <div style=" float:right; height:22px; line-height:22px; margin:10px;"><a href='slnDropInCancel.aspx?ID=<%=GetInfo().ID %>'>取消上门</a></div> 
                        <asp:Button ID="BtnSubmit" runat="server" Text="提交(s)" CssClass="BigButton" OnClick="BtnSubmit_Click"
                            AccessKey="s" OnClientClick="$(this).hide();return true;" />
                        
                        <%--<span class="red2" style=" display:none;">短信正在发送。请等上几秒。</span>--%>
                    </td>
                </tr>
            </table>
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
