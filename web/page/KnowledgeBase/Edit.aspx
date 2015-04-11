<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Site1.master" AutoEventWireup="true"
    CodeFile="Edit.aspx.cs" Inherits="page_KnowledgeBase_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="/js/kindeditor-4.1.7/themes/default/default.css" />
    <script charset="utf-8" src="/js/kindeditor-4.1.7/kindeditor-min.js"></script>
    <script charset="utf-8" src="/js/kindeditor-4.1.7/lang/zh_CN.js"></script>
    <script type="text/javascript">
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('#<%=TxbContent.ClientID %>', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                width: 660,
                height: 200,
                items: [
                        'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                        'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                        'insertunorderedlist', '|', 'emoticons', 'image', 'link']
            });
        });
    </script>
    <style type="text/css">
        .DivItem ul li
        {
            margin: 5px;
            padding: 5px;
        }
    </style>
    <style type="text/css">
        .ulClass
        {
            margin: 3px;
            padding: 3px 10px;
            list-style: none;
        }

            .ulClass li
            {
                margin: 3px;
                padding: 3px;
                border: 1px solid #CCC;
                float: left;
            }

        .DivItem li
        {
            list-style-type: decimal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="table1">
        <tr>
            <td colspan="4" style="height: 30px" class="td1_1">
                <asp:Literal ID="LtlAction" runat="server" Text="添加"></asp:Literal>知识库
            </td>
        </tr>
        <tr>
            <td class="td1_2">标题
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbTitle" runat="server" Width="375px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">标签
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbLab" runat="server" Width="375px"></asp:TextBox>(多个标签之前用“;”隔开)
            </td>
        </tr>
        <tr>
            <td class="td1_2">内容
            </td>
            <td class="td1_3">
                <asp:TextBox ID="TxbContent" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1_2">上传附件
            </td>
            <td class="td1_3">
                <asp:Literal ID="LtlUpload" runat="server">请保存好你新建的数据后再回来上传</asp:Literal>
                
                <div style="padding:10px;" id="DivAttachment">
                </div>
                <script type="text/javascript">
                    function SetAttachmentID(AttID) {
                        var vTexts = $("#DivAttachment").html();
                        vTexts += "<div style='margin:3px;'>";
                        vTexts += "<a target='_black' href='/page/Attachment/List.aspx?ID=" + AttID + "'>";
                        vTexts += "去附件管理里对这个附件进行管理";
                        vTexts += "</a>";
                        vTexts += "</div>";
                        $("#DivAttachment").html(vTexts);
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td class="td1_2">关联品牌
            </td>
            <td class="td1_3">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <ul class="ulClass">
                            <li>客户:<asp:DropDownList ID="DdlCustomer" DataTextField="name" DataValueField="id" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DdlCustomer_SelectedIndexChanged"></asp:DropDownList>
                            </li>
                            <li>品牌:<asp:DropDownList ID="DdlBrand" DataTextField="name" DataValueField="id"  runat="server"></asp:DropDownList>
                            </li>
                            <li>
                                <asp:Button ID="BtnAddClass3" runat="server" Text="添加关联" OnClick="BtnAddBrand_Click" />
                            </li>
                            <li style="clear: both;"></li>
                        </ul>
                        <div style="clear: both;"></div>
                        <div class="DivItem">
                            绑定品牌
                    <ul>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <li><%#Eval("Name") %>
                                    <asp:LinkButton ID="LbDelete" runat="server">删除</asp:LinkButton>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </td>
        </tr>
        <tr>
            <td class="td1_2" colspan="2">
                <asp:Label ID="LabUserInfo" runat="server" Text=""></asp:Label>
            </td>
        </tr>

    </table>

    <asp:Button ID="BtnSubmit" runat="server" Text=" 提交 " OnClick="BtnSubmit_Click" CssClass="BigButton" />
</asp:Content>
