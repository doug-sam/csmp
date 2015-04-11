<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lvLogin.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta name="ROBOTS" content="NOINDEX, NOFOLLOW" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/site.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery-1.4.4.min.js" type="text/javascript"></script>



         <style>
        body, div, dl, dt, dd, ul, ol, li, h1, h2, h3, h4, h5, h6, pre, form, fieldset, input, textarea, p, blockquote, th, td
        {
         padding: 0;
            margin: 0;
			font: 14px/1.5 Tahoma, Helvetica, Arial, sans-serif;
        }
        .login_main table
        {
            width: 100%;
            font-weight: bold;
        }
        .login_main table td
        {
            padding: 5px;
        }
    </style>
        
        <style type="text/css">
		.Default{ background: #001024 url(/images/Login_BG.jpg) ;color: #333; }
        .Default .Body{ text-align:center;}
        .Default .left{ position:absolute; bottom:2px; height:100px;width:300px; background:url(/images/Login_BG2.gif) no-repeat; left: 0px; }
        .Default #divBrandImg{display:none}
        .Default .Login_main{ }
        .Default .table{margin:200px auto;background: url(/images/login_main_bg1.jpg) center no-repeat;width: 440px; height: 250px; text-align:left; }
        .Default .div_TableBegin{}
        .Default .div_IDText{ float:left; margin:100px 30px 20px 55px;}
        .Default .div_ID{ float:left;margin:100px 30px 20px 0px;
                height: 21px;
                width: 124px;
            }
        .Default .div_PwdText{ float:left;margin:0px 30px 20px 55px; }
		.Default .div_Name{ clear:both;}
		.Default .div_Back .div_backContainer{ width:36px; height:26px; position:relative; top:-150px; left:20px; background:url(/images/back.gif) no-repeat; border:none; z-index:0; }
        .Default .div_Submit{ float:right; position:relative; left:-60px; top:-44px; }
        .Default .div_Submit input{  width:84px; height:65px; background:url(/images/login_btn.gif) no-repeat; border:none; cursor:pointer; z-index:0; }
		.Default .ErrorMsg{ margin-left:20px;}
		
		
		
		
        .Acqua{ text-align:center; background-color:#EFEFE7}
        .Acqua .Body{color: #333;}
        .Acqua .Login_main{ background: url(/images/Acqua_bg.png) no-repeat; width:900px; height:500px; margin:0 auto; text-align:left; }
        .Acqua .left{ height:50px; }        
        .Acqua #divBrandImg{display:none}
        .Acqua .table{ padding:421px 0px 0px 421px; }
        .Acqua table{}

        
         
        .Dior{ background-color: #e4e3e3; background:#e4e3e3 url(/images/dior/bg.png) no-repeat 50% 0px; text-align:center; }
        .Dior .Body{ text-align:center;}
        .Dior .left{  }
        .Dior #divBrandImg{display:none}
        .Dior .Login_main{ }
        .Dior .table{ width:250px; margin:0 auto; padding:333px 0px 0px 730px; text-align:left; }
        .Dior .div_TableBegin{ position:relative;}
        .Dior .div_IDText{ position:relative; font-weight:bold; color:#231105;}
        .Dior .div_ID{ width:242px; position:relative; top:-24px; left:44px; border:1px solid #CCC;}
        .Dior .div_ID .txtUserName,.Dior .div_Pwd .txtPassword{ width:239px; height:37px; line-height:37px; border:none;}
        .Dior .div_PwdText{ position:relative;font-weight:bold; color:#231105; }
        .Dior .div_Pwd{ width:242px;position:relative;top:-63px; left:44px;  }
        .Dior .div_Submit{ position:relative; top:31px;  left:44px; z-index:100; width:111px; }
        .Dior .div_Submit input{ width:107px; height:36px; background:url(/images/Dior/submit.gif) no-repeat; border:none; cursor:pointer;}
        .Dior .div_Back{ position:relative; top:-45px; left:160px; }
        .Dior .div_Back .div_backContainer{ width:107px; height:36px; background:url(/images/Dior/back.gif) no-repeat; border:none;}
        
        
        
        
        
      
        
        .Guerlain{ background-color: #e4e3e3; background:#e4e3e3 url(/images/Guerlain/bg.png) no-repeat 50% 0px; text-align:center; }
        .Guerlain .Body{ text-align:center;}
        .Guerlain .left{  }
        .Guerlain #divBrandImg{display:none}
        .Guerlain .Login_main{ }
        .Guerlain .table{ width:250px; margin:0 auto; padding:333px 0px 0px 730px; text-align:left; }
        .Guerlain .div_TableBegin{ position:relative;}
        .Guerlain .div_IDText{ position:relative; font-weight:bold; color:#231105;}
        .Guerlain .div_ID{ width:242px;position:relative; top:-24px; left:44px;}
        .Guerlain .div_ID .txtUserName,.Guerlain .div_Pwd .txtPassword{ width:239px; height:37px; line-height:37px; border:none; background:url(/images/Guerlain/input_text.gif) repeat 0 0;}
        .Guerlain .div_PwdText{ position:relative;font-weight:bold; color:#231105; }
        .Guerlain .div_Pwd{width:242px; position:relative;top:-63px; left:44px; }
        .Guerlain .div_Submit{ position:relative; top:31px;  left:44px; z-index:100; width:111px; }
        .Guerlain .div_Submit input{ width:107px; height:36px; background:url(/images/Dior/submit.gif) no-repeat; border:none; cursor:pointer;}
        .Guerlain .div_Back{ position:relative; top:-45px; left:160px; }
        .Guerlain .div_Back .div_backContainer{ width:107px; height:36px; background:url(/images/Dior/back.gif) no-repeat; border:none;}

        .Givenchy{ background-color: #e4e3e3; background:#e4e3e3 url(/images/Givenchy/bg.png) no-repeat 50% 0px; text-align:center; }
        .Givenchy .Body{ text-align:center;}
        .Givenchy .left{  }
        .Givenchy #divBrandImg{display:none}
        .Givenchy .Login_main{ }
        .Givenchy .table{ width:250px; margin:0 auto; padding:333px 0px 0px 250px; text-align:left; }
        .Givenchy .div_TableBegin{ position:relative;}
        .Givenchy .div_IDText{ position:relative; font-weight:bold; color:#231105;}
        .Givenchy .div_ID{ width:242px;position:relative; top:-24px; left:44px;}
        .Givenchy .div_ID .txtUserName,.Givenchy .div_Pwd .txtPassword{ width:239px; height:37px; line-height:37px; border:none; background:url(/images/Givenchy/input_text.gif) repeat 0 0;}
        .Givenchy .div_PwdText{ position:relative;font-weight:bold; color:#231105; }
        .Givenchy .div_Pwd{width:242px; position:relative;top:-63px; left:44px; }
        .Givenchy .div_Submit{ position:relative; top:31px;  left:44px; z-index:100; width:111px; }
        .Givenchy .div_Submit input{ width:107px; height:36px; background:url(/images/Dior/submit.gif) no-repeat; border:none; cursor:pointer;}
        .Givenchy .div_Back{ position:relative; top:-45px; left:160px; }
        .Givenchy .div_Back .div_backContainer{ width:107px; height:36px; background:url(/images/Dior/back.gif) no-repeat; border:none;}

        .Makeup{ background-color: #e4e3e3; background:#e4e3e3 url(/images/Makeup/bg.png) no-repeat 50% 0px; text-align:center; }
        .Makeup .Body{ text-align:center;}
        .Makeup .left{  }
        .Makeup #divBrandImg{display:none}
        .Makeup .Login_main{ }
        .Makeup .table{ width:250px; margin:0 auto; padding:333px 0px 0px 250px; text-align:left; }
        .Makeup .div_TableBegin{ position:relative;}
        .Makeup .div_IDText{ position:relative; font-weight:bold; color:#231105;}
        .Makeup .div_ID{ width:242px;position:relative; top:-24px; left:44px;}
        .Makeup .div_ID .txtUserName,.Makeup .div_Pwd .txtPassword{ width:239px; height:37px; line-height:37px; border:none; background:#FAFFBD;}
        .Makeup .div_PwdText{ position:relative;font-weight:bold; color:#231105; }
        .Makeup .div_Pwd{width:242px; position:relative;top:-63px; left:44px; }
        .Makeup .div_Submit{ position:relative; top:31px;  left:44px; z-index:100; width:111px; }
        .Makeup .div_Submit input{ width:107px; height:36px; background:url(/images/Dior/submit.gif) no-repeat; border:none; cursor:pointer;}
        .Makeup .div_Back{ position:relative; top:-45px; left:160px; }
        .Makeup .div_Back .div_backContainer{ width:107px; height:36px; background:url(/images/Dior/back.gif) no-repeat; border:none;}

        .benefit{ background-color: #e4e3e3; background:#e4e3e3 url(/images/benefit/bg.png) no-repeat 50% 0px; text-align:center; }
        .benefit .Body{ text-align:center;}
        .benefit .left{  }
        .benefit #divBrandImg{display:none}
        .benefit .Login_main{ }
        .benefit .table{ width:250px; margin:0 auto; padding:300px 0px 0px 240px; text-align:left;  }
        .benefit .div_TableBegin{ position:relative;}
        .benefit .div_IDText{ position:relative; font-weight:bold; color:#231105;}
        .benefit .div_ID{ width:242px;position:relative; top:-24px; left:44px;}
        .benefit .div_ID .txtUserName,.benefit .div_Pwd .txtPassword{ width:239px; height:37px; line-height:37px; border:none; background:#FAFFBD;}
        .benefit .div_PwdText{ position:relative;font-weight:bold; color:#231105; }
        .benefit .div_Pwd{width:242px; position:relative;top:-63px; left:44px; }
        .benefit .div_Submit{ position:relative; top:31px;  left:44px; z-index:100; width:111px; }
        .benefit .div_Submit input{ width:107px; height:36px; background:url(/images/Dior/submit.gif) no-repeat; border:none; cursor:pointer;}
        .benefit .div_Back{ position:relative; top:-45px; left:160px; }
        .benefit .div_Back .div_backContainer{ width:107px; height:36px; background:url(/images/Dior/back.gif) no-repeat; border:none;}
		
        .Acquea{ background-color: #e4e3e3; background:#e4e3e3 url(/images/Acquea/bg.png) no-repeat 50% 0px; text-align:center; }
        .Acquea .Body{ text-align:center;}
        .Acquea .left{  }
        .Acquea #divBrandImg{display:none}
        .Acquea .Login_main{ }
        .Acquea .table{ width:250px; margin:0 auto; padding:300px 0px 0px 290px; text-align:left;  }
        .Acquea .div_TableBegin{ position:relative;}
        .Acquea .div_IDText{ position:relative; font-weight:bold; color:#231105;}
        .Acquea .div_ID{ width:242px;position:relative; top:-24px; left:44px;}
        .Acquea .div_ID .txtUserName,.Acquea .div_Pwd .txtPassword{ width:239px; height:37px; line-height:37px; border:none; background:#FAFFBD;}
        .Acquea .div_PwdText{ position:relative;font-weight:bold; color:#231105; }
        .Acquea .div_Pwd{width:242px; position:relative;top:-63px; left:44px; }
        .Acquea .div_Submit{ position:relative; top:31px;  left:44px; z-index:100; width:111px; }
        .Acquea .div_Submit input{ width:107px; height:36px; background:url(/images/Dior/submit.gif) no-repeat; border:none; cursor:pointer;}
        .Acquea .div_Back{ position:relative; top:-45px; left:160px; }
        .Acquea .div_Back .div_backContainer{ width:107px; height:36px; background:url(/images/Dior/back.gif) no-repeat; border:none;}
		
        .fresh{ background-color: #e4e3e3; background:#e4e3e3 url(/images/fresh/bg.png) no-repeat 50% 0px; text-align:center; }
        .fresh .Body{ text-align:center;}
        .fresh .left{  }
        .fresh #divBrandImg{display:none}
        .fresh .Login_main{ }
        .fresh .table{ width:250px; margin:0 auto; padding:300px 0px 0px 320px; text-align:left;  }
        .fresh .div_TableBegin{ position:relative;}
        .fresh .div_IDText{ position:relative; font-weight:bold; color:#231105;}
        .fresh .div_ID{ width:242px;position:relative; top:-24px; left:44px;}
        .fresh .div_ID .txtUserName,.fresh .div_Pwd .txtPassword{ width:239px; height:37px; line-height:37px; border:none; background:#FAFFBD;}
        .fresh .div_PwdText{ position:relative;font-weight:bold; color:#231105; }
        .fresh .div_Pwd{width:242px; position:relative;top:-63px; left:44px; }
        .fresh .div_Submit{ position:relative; top:31px;  left:44px; z-index:100; width:111px; }
        .fresh .div_Submit input{ width:107px; height:36px; background:url(/images/Dior/submit.gif) no-repeat; border:none; cursor:pointer;}
        .fresh .div_Back{ position:relative; top:-45px; left:160px; }
        .fresh .div_Back .div_backContainer{ width:107px; height:36px; background:url(/images/Dior/back.gif) no-repeat; border:none;}
		
        </style>
        <link rel="shortcut icon" href="images/favicon.ico">

    <meta name="ROBOTS" content="NOINDEX, NOFOLLOW" />
          <script src="/js/Public.js" type="text/javascript"></script>

    	<script language="javascript" type="text/javascript">
    	    function ChangeImg(ImgEle) {
    	        ImgEle.src = "/page/sys/RandImg.aspx?id=" + Math.random();
    	    }
			function Fun_OutBrand()
			{
				setCookie('brand','', -9999999990);
				location.href='/lvdefault.html';
				}
    	    $(document).ready(function() { $("#<%=txtUserName.ClientID %>").focus(); });
        </script>

</head>
<body class="Default" id="ToChangeCss">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div style="background-position: center" class="Body">
            <div class="left">
            </div>
            <div class="Login_main" style="text-align:center;">
                        <div class="table" >
                        	<div class="div_TableBegin"></div>
                        	<div class="div_IDText">账号：</div>
                        	<div class="div_ID"><asp:TextBox ID="txtUserName" CssClass="txtUserName" runat="server" TabIndex="1"></asp:TextBox></div>
                            <div class="div_Name"></div>
                        	<div class="div_PwdText">密码：</div>
                        	<div class="div_Submit">
                                <asp:Button ID="BtnSubmit"  CssClass="txtUserName" runat="server" 
                                    Text="&nbsp;&nbsp;&nbsp;&nbsp;"  TabIndex="3" onclick="BtnSubmit_Click"/>
                            </div>
                        	<div class="div_Pwd"><asp:TextBox ID="txtPassword" CssClass="txtPassword" runat="server" TextMode="Password" TabIndex="2" ></asp:TextBox></div>
                        	<div class="div_Back">
                            	<a href="javascript: Fun_OutBrand();" title="后退" ><div class="div_backContainer"></div></a>
                            </div>
                            <div class="ErrorMsg" style="color:#F60;"><asp:Literal ID="LabError" runat="server"></asp:Literal></div>
                            <div style="clear:both"></div>
                        </div>

                        <script type="text/javascript">
                            var BrandName = Request("img");
                            var MainStyle = document.getElementById("ToChangeCss");
                            var BrandImg = document.getElementById("BrandImg");
                            if (BrandName == "") {
                                alert("error arms");
                                location.href = "/";
                            }
                            else if (BrandName == "dior")
                                MainStyle.className = "Dior";
                            else if (BrandName == "benefit")
                                MainStyle.className = "benefit";
                            else if (BrandName == "Acquea")
                                MainStyle.className = "Acquea";
                            else if (BrandName == "givenchy")
                                MainStyle.className = "Givenchy";
                            else if (BrandName == "fresh")
                                MainStyle.className = "fresh";
                            else if (BrandName == "makeup")
                                MainStyle.className = "Makeup";
                            else if (BrandName == "guerlain")
                                MainStyle.className = "Guerlain";
                            else {
                                BrandImg.style.display = "block";
                                BrandImg.src = "/images/" + BrandName + ".gif";
                            }
                            setCookie("brand", BrandName, 999999999);
                
                        </script>
            </div>
        </div>
        <div style="clear: both;">
        </div>
    </form>
</body>
</html>
