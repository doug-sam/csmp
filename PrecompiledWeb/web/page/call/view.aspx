<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_view, App_Web_isgyizrg" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>
<%@ Register Src="/Controls/RecordPlay.ascx" TagName="RecordPlay" TagPrefix="uc1" %>
<%@ Register Src="/Controls/View.ascx" TagName="View" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ItemsDetail
        {margin:10px; padding:6px; border:1px solid #CCC;
        }
    </style>
    
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc2:View ID="View1" runat="server" />
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td class="td1_2">
                回放报修录音  
            </td>
            <td class="td1_3">
                <uc1:RecordPlay ID="RecordPlay1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                回放回访录音  
            </td>
            <td class="td1_3">
                <uc1:RecordPlay ID="RecordPlay2" runat="server" />
            </td>
        </tr>        
        <tr>
            <td class="td1_2" colspan="2" style="line-height:33px;">
                <asp:HyperLink CssClass="ItemsDetail" ID="HlHistory" runat="server" Target="main">查看店铺所有报修历史</asp:HyperLink>
                <asp:PlaceHolder ID="tr_Trace" runat="server">
                <a  class="ItemsDetail" href="javascript:tb_show('店铺催促', '/page/Trace/Trace.aspx?ID=<%=info.ID %>&TB_iframe=true&height=380&width=700&modal=false', false);">
                    店铺催促</a>
                </asp:PlaceHolder>
                <a class="ItemsDetail" href='/page/attachment/view.aspx?ID=<%=info.ID %>'>附件列表</a>
            </td>
        </tr>
    </table>
</asp:Content>
