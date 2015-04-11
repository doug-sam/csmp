<%@ page title="" language="C#" masterpagefile="~/Controls/Site1.master" autoeventwireup="true" inherits="Login, App_Web_kwecoy6s" enableviewstatemac="false" enableEventValidation="false" viewStateEncryptionMode="Never" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="images/favicon.ico">
    <meta name="ROBOTS" content="NOINDEX, NOFOLLOW" />
    
    	<script language="javascript" type="text/javascript">
	    function ChangeImg(ImgEle) {
	        ImgEle.src = "/page/sys/RandImg.aspx?id=" + Math.random();
	    }
	    $(document).ready(function() { $("#<%=txtUserName.ClientID %>").focus(); });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="bg_light">
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="bg_word">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin: 150px 0 0 0">
                                <tr>
                                    <td align="center">
                                        <table width="714" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td height="430" align="right" valign="top" background="images/BG-login.png">
                                                    <table width="714" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="41" rowspan="3">
                                                            </td>
                                                            <td width="632" height="200">
                                                                <asp:Label ID="LabError" runat="server" Text="" Style="color: #F60; font-size: 14px;
                                                                    font-weight: 700;"></asp:Label>
                                                            </td>
                                                            <td width="41" rowspan="3">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="48" align="left" valign="middle">
                                                                <table width="632" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="20">
                                                                            
                                                                        </td>
                                                                        <td>
                                                                            用户名：
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="input" MaxLength="16"></asp:TextBox>
                                                                        </td>
                                                                        <td >
                                                                            密码：
                                                                        </td>
                                                                        <td >
                                                                            <asp:TextBox runat="server" ID="txtPassword" MaxLength="16" TextMode="Password" CssClass="input"></asp:TextBox>
                                                                        </td>
                                                                        <td width="44">
                                                                            <asp:TextBox runat="server" AutoCompleteType="None" ID="TxbImg" MaxLength="4"  CssClass="input" Width="40"  ></asp:TextBox>
                                                                            
                                                                        </td>
                                                                        <td width="56">
                                                                            <img width="54" height="18" style=" cursor:pointer;" align="absmiddle" alt="点击更新新图" src="/page/sys/RandImg.aspx"  onclick="ChangeImg(this);" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="btnLogin" ImageUrl="images/button.png" runat="server" Width="63"
                                                                                Height="22" border="0" OnClick="btnLogin_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="182">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        if (window.top != window.self) { window.top.location = window.self.location; }
    </script>

</asp:Content>
