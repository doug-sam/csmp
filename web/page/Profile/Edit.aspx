<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="page_Profile_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                系统参数设置
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                APP版本
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbAppVersion" runat="server"></asp:TextBox>(作用是app在检测是否有更新时，将客户端版本与这里的值进行对比，如果低于这个值，则提示下来服务器上提供的app)(这里输入整数，版本比客户端使用版本高则提示升级，另外app包请放在系统目录/services/csmp.apk，注意改名为csmp.apk)
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnAppVersion" runat="server" Text="  提 交  " OnClick="BtnAppVersion_Click" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                全国工作组ID
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbGrobalGroup" runat="server"></asp:TextBox>(输入数字。作用请参见说明书用“全国组”的概念，这里是标记哪个工作组为全国组)
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnGrobalGroup" runat="server" Text="  提 交  " OnClick="BtnGrobalGroup_Click" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                电话录音文件根目录
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbRecordUrl" runat="server" Width="378px"></asp:TextBox>(请保留"recording_file_path、recording_file_name"，分别代表录音路径及文件名)
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnRecordUrl" runat="server" Text="  提 交  " 
                    onclick="BtnRecordUrl_Click" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                系统地址
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbSysAddress" runat="server" Width="380px"></asp:TextBox>(应以http://开头。最后不要加上"/")
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnSysAddress" runat="server" Text="  提 交  " 
                    onclick="BtnSysAddress_Click" />
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                系统外网地址
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbSysAddressWWW" runat="server" Width="380px"></asp:TextBox>(应以http://开头。最后不要加上"/")
            </td>
            <td class="td1_2">
                <asp:Button ID="BtnSysAddressWWW" runat="server" Text="  提 交  " onclick="BtnSysAddressWWW_Click" 
                     />
            </td>
        </tr>
    </table>

</asp:Content>
