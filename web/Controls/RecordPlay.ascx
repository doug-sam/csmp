<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordPlay.ascx.cs" Inherits="Controls_RecordPlay" %>

<asp:Panel ID="Panel1" runat="server">
    <asp:Label id="playlb" runat="server" />
    
    <asp:HiddenField ID="hdsrc" runat="server" />
    
    <div style=" margin:7px;">如果录音较长，请耐心等待。。</div>
    <script type="text/javascript">
        gethtml("<%= hdsrc.ClientID %>", "<%= playlb.ClientID %>");
   </script>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Visible="false">
无
</asp:Panel>
