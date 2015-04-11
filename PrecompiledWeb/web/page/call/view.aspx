<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_call_view, App_Web_rursbog1" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<%@ Register Src="/Controls/RecordPlay.ascx" TagName="RecordPlay" TagPrefix="uc1" %>
<%@ Register Src="/Controls/View.ascx" TagName="View" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ItemsDetail
        {margin:10px; padding:6px; border:1px solid #CCC;
        }
    </style>
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
