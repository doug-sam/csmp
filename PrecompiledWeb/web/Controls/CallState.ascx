<%@ control language="C#" autoeventwireup="true" inherits="Controls_CallState, App_Web_zkjdnf4p" %>
<%@ Register src="View.ascx" tagname="View" tagprefix="uc1" %>
<style type="text/css">
    .StateAll
    {
        background-repeat: no-repeat;
        background: url(/images/state1.png);
        width: 220px;
        height: 33px;
        background-position: 100% 0;
        line-height: 33px;
        margin-right: 5px;
        text-align: center;
        font-size: 14px;
        font-weight: 700;
        float: left;
    }
<%--    .ViewCall
    {
        float: left;
        width: 80px;
        background: #f2f2f2;
        height: 33px;
        font-weight: 700;
        padding: 0px 5px;
        line-height: 33px;
        text-align: center;
    }--%>
</style>

<div style="height: 40px;">
    <div id="itemIndex1" class="StateAll">
        <a href="/Page/call/add.aspx" target="main">1.新建报Call信息</a></div>
    <div id="itemIndex2" class="StateAll" style="cursor: pointer;">
        2.处理记录跟进</div>
    <div id="itemIndex3" class="StateAll">
        3.完成</div>
    <asp:Label ID="LabListRec" runat="server" Text="" CssClass="StateAll" style=" width:80px; overflow:hidden;background-position:0;" Visible="false"></asp:Label>
</div>
<div class="clean">
</div>
<asp:Panel ID="Panel1" runat="server" Visible="false">
    <div id="step2Details" style="position: relative; top: -15px; z-index: -1;">
        <img src="/images/linetop.gif" />
        <div style="border-left: 1px solid #ccc; border-right: 1px solid #ccc; border-bottom: 1px solid #ccc;
            width: 745px; line-height: 22px; padding: 0px 10px;">
            <asp:Label ID="LabStep" runat="server" Text=""></asp:Label>
        </div>
    </div>
</asp:Panel>


<table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
    <tr id="tr_sln" runat="server"  Visible="false">
        <td class="td1_2" width="90px">
            建议解决方案
        </td>
        <td class="td1_3">
            <asp:Label ID="LabSln" runat="server" Text=""></asp:Label>
        </td>
    </tr>    
</table>
<asp:Panel ID="PanelIrameSrc" Visible="false" runat="server">
<%--<iframe src='<asp:Literal ID="LtlIframeSrc" runat="server"></asp:Literal>' 
onload="this.height=this.contentWindow.document.documentElement.scrollHeight"
 width="100%" frameBorder="0" noresize="noresize" scrolling="yes"> </iframe>--%>
    <uc1:View ID="View1" runat="server" />
</asp:Panel>
<script language="javascript" type="text/javascript">
    var PublicitemIndex;
    function FocusState(itemIndex) {
        PublicitemIndex = itemIndex;
        $("#itemIndex" + itemIndex).css({ 'background': 'url(/images/state2.png) 100% 0', 'color': '#fff' });
//        if (itemIndex != 2) {
//            $("#step2Details").hide();
 //       }
    }
    $("#itemIndex2").click(function() {
        if (PublicitemIndex == 2) {
            $("#step2Details").hide();
            tb_show('处理记录跟进', '/page/CallStep/listview.aspx?ID=<%=CallID %>&TB_iframe=true&height=450&width=730', false);

        }
    });

    
</script>

