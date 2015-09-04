<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="page_Attachment_Upload, App_Web_4oewkeij" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="3" style="height: 30px" class="td1_1">
                      文件上传
            </td>
        </tr>
        <tr>
            <td class="td1_2">
                      请选择文件
            </td>
            <td class="td1_3">
                <%@ Register TagPrefix="telerik" Namespace="Telerik.QuickStart" Assembly="Telerik.QuickStart" %> 
                <%@ Register TagPrefix="radU" Namespace="Telerik.WebControls" Assembly="RadUpload.Net2" %>
                <radu:radprogressmanager id="Radprogressmanager1"  Width="100%" runat="server" Height="37px" /> 
                <radu:radprogressarea id="progressArea1" Width="100%"  runat="server"></radu:radprogressarea> 
        
                <asp:FileUpload ID="FileUpload1" runat="server" /> 
            </td>
            <td class="td1_2">
                <asp:button runat="server" text="上传" id ="BtnSave" CssClass="BigButton" OnClick="BtnSave_Click" />     
            </td>
        </tr>
    </table>
</asp:Content>
