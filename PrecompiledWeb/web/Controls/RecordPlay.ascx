<%@ control language="C#" autoeventwireup="true" inherits="Controls_RecordPlay, App_Web_etara6uy" %>
<asp:Panel ID="Panel1" runat="server">
    <audio src='<asp:Literal ID="LtlSrc1" runat="server"></asp:Literal>' controls="controls">
        <object classid="clsid:22d6f312-b0f6-11d0-94ab-0080c74c7e95" id="Object1" width="700"
            height="150">
                 <embed> 
                    <param name="SRC" value='<asp:Literal ID="LtlSrc2" runat="server"></asp:Literal>'/>
                    <param name="autoplay" value="false" />
                  </embed>
        </object>
    </audio>
    <div style=" margin:7px;">如果录音较长，请耐心等待。。</div>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Visible="false">
无
</asp:Panel>
