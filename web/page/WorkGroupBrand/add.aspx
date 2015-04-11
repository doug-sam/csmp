<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="add.aspx.cs" Inherits="page_WorkGroupBrand_add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
                <tr>
                    <td class="td1_2">
                        所属品牌：
                    </td>
                    <td class="td1_3">
                        <asp:DropDownList ID="DdlCustomer" runat="server" DataTextField="Name" 
                            DataValueField="ID" AutoPostBack="True" 
                            onselectedindexchanged="DdlCustomer_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlBrand" runat="server" DataTextField="Name" 
                            DataValueField="ID" AutoPostBack="True" 
                            onselectedindexchanged="ddlBrand_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        组：
                    </td>
                    <td class="td1_3">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                <div>不属于该品牌的组</div>
                                    <asp:ListBox ID="LbUserLeft" runat="server" Width="150px" Height="300px" SelectionMode="Multiple"
                                        DataTextField="Name" DataValueField="ID"></asp:ListBox>
                                </td>
                                <td>
                                    <asp:Button ID="BtnAdd" runat="server" Text="&gt;&gt;" onclick="BtnAdd_Click" />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Button ID="BtnDel" runat="server" Text="&lt;&lt;" onclick="BtnDel_Click" />
                                </td>
                                <td>
                                <div>属于该品牌的组</div>
                                    <asp:ListBox ID="LbUserRight" runat="server" Width="150px" Height="300px" SelectionMode="Multiple"
                                        DataTextField="Name" DataValueField="ID"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2" colspan="2">
                        <asp:Button ID="BtnSubmit" runat="server" Text=" 保存 " 
                            onclick="BtnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <input id="btnCancel" type="button" value="取消"  onclick="parent.tb_remove();" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <input id="Button1" type="button" value="换个方式进行管理"  onclick="location.href='add2.aspx';" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
