<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Feedback_Edit, App_Web_rasqts5x" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        回访记录查看
                    </td>
                </tr>
                <asp:Repeater ID="RpRadio" runat="server" OnItemDataBound="RpRadio_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="td1_3">
                              问  <%#Container.ItemIndex+1 %>、
                                <%#Eval("Name") %>
                                <div>                                    
                               答     <asp:Label ID="LabAnswer" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="RpEssay" runat="server" OnItemDataBound="RpEssay_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="td1_2">
                                问
                                <%#Container.ItemIndex+1 %>、
                                <%#Eval("Name") %>
                            </td>
                        </tr>
                        <tr>
                            <td class="td1_3">
                                答                                
                                <asp:Label ID="LabAnswer" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
