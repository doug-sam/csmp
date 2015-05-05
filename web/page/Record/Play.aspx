<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Play.aspx.cs" Inherits="page_Attachment_View" %>
<%@ Register Src="/Controls/RecordPlay.ascx" TagName="RecordPlay" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">
    function gethtml(idname, labelIdName) {
        var src = document.getElementById(idname).value;
        if (navigator.userAgent.indexOf("Chrome") > -1) {
            //如果是Chrome：
            document.getElementById(labelIdName).innerHTML = "<audio controls=\"controls\" src=\"" + src + "\" type=\"audio/mp3\" ></audio>";
        } else if (navigator.userAgent.indexOf("Firefox") != -1) {
            //如果是Firefox：
            document.getElementById(labelIdName).innerHTML = "<embed src=\"" + src + "\" type=\"audio/mp3\" hidden=\"no\" width=\"550\" height=\"50\" loop=\"false\" mastersound></embed>";
        } else if (navigator.appName.indexOf("Microsoft Internet Explorer") != -1 && document.all) {
            //如果是IE(6,7,8):
            document.getElementById(labelIdName).innerHTML = "<object width=\"550\" height=\"50\" classid=\"clsid:22D6F312-B0F6-11D0-94AB-0080C74C7E95\"><param name=\"AutoStart\" value=\"false\" /><param name=\"SRC\" value=\"" + src + "\" /></object>";
        } else if (navigator.appName.indexOf("Opera") != -1) {
            //如果是Oprea：
            document.getElementById(labelIdName).innerHTML = "<embed src=\"" + src + "\"  type=\"audio/mpeg\"   loop=\"false\"></embed>";
        } else {
            document.getElementById(labelIdName).innerHTML = "<embed src=\"" + src + "\"  type=\"audio/mp3\" hidden=\"no\" width=\"550\" height=\"50\" loop=\"false\" mastersound></embed>";
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                      回放录音
            </td>
        </tr>
        <tr>
            <td class="td1_3">
                <uc1:RecordPlay ID="RecordPlay1" runat="server" />
            </td>        
        </tr>
    </table>
</asp:Content>
