<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="AssignWorkGroup.aspx.cs" Inherits="page_Assign_AssignWorkGroup" %>

<%@ Register Src="list.ascx" TagName="list" TagPrefix="uc1" %>
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
    <div class="tab">
                <a href="Assign.aspx?ID=<%=Tool.Function.GetRequestInt("ID") %>" >组内转派</a> 
                 <a href="AssignWorkGroup.aspx" class="on">转派到其它组</a>
        <div class="tabspace">
            &nbsp;</div>
    </div>
    <div class="tabbody">
        <div style="margin: 5px;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1" id="tb_Assign"
                        runat="server">
                        <tr>
                            <td colspan="2" style="height: 30px" class="td1_1">
                                <span class="red2">全国组</span>报修转派
                            </td>
                        </tr>
                        <tr>
                            <td class="td1_2">
                                转派给
                            </td>
                            <td class="td1_3">
                                <asp:DropDownList ID="DdlUser" runat="server" DataTextField="Name" DataValueField="ID">
                                </asp:DropDownList>
                                二线工程师
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
    <uc1:list ID="list1" runat="server" />
</asp:Content>
