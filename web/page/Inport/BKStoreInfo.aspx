<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BKStoreInfo.aspx.cs" MasterPageFile="~/Controls/Site1.master"
 Inherits="page_Inport_BKStoreInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                汉堡王店铺数据导入
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                选择文件：
            </td>
            <td class="td1_3">
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnCheck" runat="server" Text="检查数据" CssClass="BigButton" 
                    onclick="BtnCheck_Click"/>
                <asp:Button ID="BtnSubmit" runat="server" Text="开始导入" CssClass="BigButton" 
                    Visible="false" onclick="BtnSubmit_Click"/>&nbsp&nbsp
                <asp:Button ID="BtnPostToBK" runat="server" Text="同步数据到汉堡王转呈系统" 
                    CssClass="BigButton" onclick="BtnPostToBK_Click" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                <asp:CheckBox ID="CbUpdate" Text="将已存在的店铺更新为Excel中的数据" runat="server" />
            </td>
            <td class="td1_3" colspan="2">
                如果你勾选了更新数据，那么请确保你希望更新的数据中。EXCEL及系统中的店铺号高度一至（勾选了如果EXCEL中的店铺在数据库中已存在，那么导入时系统将用EXCEL中的店铺信息覆盖数据库中对应的店铺信息）
            </td>
        </tr>
        <tr>
            <td class="td1_2">
            导入注意事项：
            </td>
            <td class="td1_3" colspan="2">
                1.从MDS下载好Excel数据(数据一定要包含MVS维护的所有店铺信息)后，请将Excel另存为Excel997-2003工作薄;<br/>
                2.如果Excel第一行不是表头，请将第一行删除，保证第一行为表头信息;<br/>
                3.如果Excel的A、B、C列为隐藏列，请删除这三列，确保Excel A列表头名为Name;<br/>
                4.删除（注意不是筛选或是隐藏）不是MVS维护的店铺信息;<br/>
                5.请修改Excel中所有的店铺名称，只保留中文描述，另外如果只有一个英文名称，请确保不包含英文单引号;<br/>
                6.Excel中CITY列的值请将城市名的汉语拼音修改为CSMP系统中对应的城市名中文描述，且必须保证完全一致，例如Beijing，应修改为北京市;<br />
                7.导入Excel后，MVS系统中店铺的状态（是否禁用）是根据Excel中STATUS列的值来判断的(Excel中STATUS的值为Opened时MVS系统中的店铺状态为可用，Excel中STATUS的值为Closed时MVS系统中的店铺状态为禁用)，导入成功后请根据需要到MVS系统中修改店铺状态;<br/>
                
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
</asp:Content>