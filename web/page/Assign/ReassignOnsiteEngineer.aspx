<%@ Page Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" 
CodeFile="ReassignOnsiteEngineer.aspx.cs" Inherits="page_Assign_ReassignOnsiteEngineer" %>

<%--<%@ Register src="list.ascx" tagname="list" tagprefix="uc1" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
.tab {border-left:#E4E4E4 1px solid; padding:5px;}
.tab a{text-decoration:none;border-top:1px #E4E4E4 solid;border-right:1px #E4E4E4 solid;
border-bottom:1px solid #E4E4E4;
background:#F6F5EE;
padding:5px;
color:#369;
float:left;}
.tab .on{border-bottom:1px solid #fff;background:#fff;color:#666}
.tabspace{border-bottom:1px solid #E4E4E4;padding-top:11px;padding-top:10px}
.tabbody{border:#E4E4E4 1px solid;border-top:0;padding:5px;padding-top:1px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="tabbody">
        <div style=" margin:5px;">
            <asp:HiddenField ID="CallStepID" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1" id="tb_Assign"
                        runat="server">
                        <tr>
                            <td colspan="2" style="height: 30px" class="td1_1">
                                更换现场工程师
                            </td>
                        </tr>
                        <tr>
                            <td class="td1_2">
                                原现场工程师
                            </td>
                            <td class="td1_3">
                                <asp:TextBox ID="oldOnside" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td1_2">
                                转派给
                            </td>
                            <td class="td1_3">
                                <asp:DropDownList ID="DdlMajorUserID" runat="server" DataTextField="Name" DataValueField="ID"
                                     AutoPostBack="True" OnSelectedIndexChanged="DdlMajorUserID_SelectedIndexChanged" CssClass="DropInUserName">
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
                            <td class="td1_2">
                                预约上门时间
                            </td>
                            <td class="td1_3">
                                <asp:TextBox ID="TxbDate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" runat="server" CssClass="VDateDate"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td1_2" colspan="2">
                                <asp:Button ID="BtnSubmit" runat="server" Text="确定(s)" CssClass="BigButton" AccessKey="s"
                                    OnClick="BtnSubmit_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--<uc1:list ID="list1" runat="server"  />--%>
</asp:Content>
