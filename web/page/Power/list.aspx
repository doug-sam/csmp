<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="page_Power_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
            function CheckAll(form, checked) {
                for (var i = 0; i < form.elements.length; i++) {
                    var e = form.elements[i];
                    if (e.name == 'manages') e.checked = checked;
                }
            }
            function Check(id, checked) {
                var cbs = document.getElementById(id).getElementsByTagName("input");
                for (var i = 0, n = cbs.length; i < n; i++) { cbs[i].checked = checked; }
            }

    </script>
    <style type="text/css">
    .ulRule{ margin:10px; padding:0px 10px;}
    .ulRule li{ list-style:decimal;margin:10px; padding:0px;}
    
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="table1"  cellpadding="0" cellspacing="0" width="100%" border="0">
                <tr>
                    <td class="td1_2">
                        当前组名：
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabGroup" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        当前组角色：
                    </td>
                    <td class="td1_3">
                        <asp:Label ID="LabRule" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td class="td1_2">
                        权限
                    </td>
                    <td class="td1_3">
                        <asp:Repeater ID="rpManages"  runat="server" OnItemDataBound="rpManages_ItemDataBound">
                            <ItemTemplate>
                            <%@ Import Namespace="System.Collections.Generic" %>
                             
                                <table  class="table1"  cellpadding="0" cellspacing="0" width="100%" border="0"  id="tb_<%# ((KeyValuePair<string, int>)Container.DataItem).Value %>">
                                    <tr>
                                       <td class="td1_2">
                                            <label>
                                                <input type="checkbox" name="manages" value="<%# ((KeyValuePair<string, int>)Container.DataItem).Value %>" 
                                                onclick="Check('tb_<%# ((KeyValuePair<string, int>)Container.DataItem).Value %>',this.checked)" 
                                                <asp:Literal ID="ltlChecked" runat="server"></asp:Literal>/>
                                               <span style="color:#6BA6EF; font-weight:bold;"> <%# ((KeyValuePair<string, int>)Container.DataItem).Key %></span>
                                            </label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    </tr>                                        
                                    <tr>
                                        <td class="td1_3">
                                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Repeater ID="rpSecond" runat="server" OnItemDataBound="rpSecond_ItemDataBound">
                                                <ItemTemplate>
                                                    <label>
                                                        <input type="checkbox" name="manages" value="<%# ((KeyValuePair<string, int>)Container.DataItem).Value %>"  <asp:Literal ID="ltlChecked" runat="server"></asp:Literal>/>
                                                       <%# ((KeyValuePair<string, int>)Container.DataItem).Key %>
                                                    </label>
                                                </ItemTemplate>
                                             </asp:Repeater>
                                        </td>                                            
                                    </tr>
                                        
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td class="td1_2">
                        权限规则：
                    </td>
                    <td class="td1_3">
                        <ul class="ulRule">
                            <li>拥有子级权限(权限详细)，必需先勾先父级(权限分类名称)权限</li>
                            <li>给定的组色角，应认真编辑对应的权限，如一线角色，不应给予《处理报修》权限</li>
                            <li>编辑权限大于查看权限，拥有对数据编辑权限则自然拥有查看详细记录权限</li>
                            <li>实际页面中，页面权限大于页内权限，则必需有页面的进入权，页面的明细操作权限才有意义</li>
                            <li>制作好的权限规则建议不要常修改</li>
                        </ul>
                    </td>
                </tr>
            </table>    
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
        <asp:CheckBox ID="cbDel" runat="server" onclick="CheckAll(this.form,this.checked)"
            Text="全选" />
        <asp:Button ID="btnSub" runat="server" Text="保 存" CssClass="button" OnClick="btnSub_Click" />
    </p>


</asp:Content>

