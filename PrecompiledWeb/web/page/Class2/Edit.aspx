<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Class2_Edit, App_Web_orzo0yei" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td colspan="2" style="height: 30px" class="td1_1">
                        <asp:Label ID="LabAction" runat="server" Text="新建"></asp:Label>中类
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        名称：
                    </td>
                    <td class="td1_3">
                        <asp:TextBox ID="TxbName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="td1_2">
                        所属客户：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" runat="server"
                         DataTextField="Name" DataValueField="ID" AutoPostBack="True" 
                            onselectedindexchanged="DdlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
               <tr>
                    <td class="td1_2">
                        所属大类：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlClass1" runat="server"
                         DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        是否使用：
                    </td>
                    <td class="td1_3">
                        <asp:RadioButtonList ID="RblIsClose" runat="server">
                            <asp:ListItem Selected="True" Text="使用" Value="1"></asp:ListItem>
                            <asp:ListItem Text="停用" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                         <asp:Button ID="BtnSubmit" runat="server" Text="提交" CssClass="BigButton"
                            OnClick="BtnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
