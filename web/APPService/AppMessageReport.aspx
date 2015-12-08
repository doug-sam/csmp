<%@ Page Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
 CodeFile="AppMessageReport.aspx.cs" Inherits="APPService_AppMessageReport" %>
 <%@ Register assembly="AspNetPager" namespace="Wuqi.Webdiyer" tagprefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                APP操作时间段报表
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                开始日期
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxtDateBegin" runat="server" class="input3" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td class="td1_2">
                结束日期
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbDateEnd" runat="server" class="input3" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
               省份
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlProvince" runat="server" AutoPostBack="True" DataTextField="Name"
                    DataValueField="ID"  OnSelectedIndexChanged="DdlProvince_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="td1_2">
               工作组
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdlWorkGroup" runat="server" AutoPostBack="True" 
                    DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="DdlWorkGroup_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                上门工程师
            </td>
            <td class="td1_3">
                <asp:DropDownList ID="DdMajorUser" runat="server" DataTextField="Name" DataValueField="ID"
                     AutoPostBack="True" >
                </asp:DropDownList>
            </td>
            <td class="td1_2">
                具体工程师姓名
            </td>
            <td class="td1_3">
                <asp:TextBox ID="txtMajorUserName" runat="server"></asp:TextBox>
                <asp:Button ID="BtnSearch" runat="server" Text="查询" CssClass="BigButton" OnClick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
        
     <asp:GridView ID="GridView1" Width="100%" CssClass="table2" runat="server" AutoGenerateColumns="false"
        GridLines="Vertical" >
        <RowStyle CssClass="GV_RowStyle"/>
        
        <HeaderStyle CssClass="GV_HeaderStyle"/>
        <AlternatingRowStyle CssClass="GV_AlternatingRowStyle"/>
        <Columns>
            <asp:TemplateField HeaderText="现场工程师">
                <ItemTemplate>
                    <%# Eval("MajorUserName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="APP到场签到总数">
                <ItemTemplate>
                    <%# Eval("APPOnsiteCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="APP提交工作记录总数">
                <ItemTemplate>
                    <%#Eval("APPWorkRecordCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="APP操作离场总数">
                <ItemTemplate>
                    <%#Eval("APPLeaveCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="APP操作更换现场工程师总数">
                <ItemTemplate>
                    <%#Eval("APPChangeEngineerCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="APP完整操作数">
                <ItemTemplate>
                    <%# Eval("WholeStepCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>  
    
    <div class ="text-align:center">
        <webdiyer:AspNetPager ID="AspNetPager1" runat="server"   PageSize="20" CurrentPageIndex ="1"
            onpagechanged="AspNetPager1_PageChanged" CssClass="anpager" 
            CurrentPageButtonClass="cpb" FirstPageText="首页" LastPageText="尾页" 
            NextPageText="后页" PrevPageText="前页">
        </webdiyer:AspNetPager>
    
    </div> 
</asp:Content>