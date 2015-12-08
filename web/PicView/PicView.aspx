<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PicView.aspx.cs" Inherits="PicView_PicView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
	
	<script type="text/javascript" src="js/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="js/jquery.lightbox-0.5.pack.js"></script>
    <link href="css/jquery.lightbox-0.5.css" rel="stylesheet" />
    <link href="css/style-jquery-lightbox.css" rel="stylesheet" />
</head>
<body>

    <div id="gallery" >
	    <ul id="pg" >
		    
	    </ul>
	    
	    <script type="text/javascript">
	        
	        window.onload = function() {
	            $.ajax({
	            url: "/page/Attachment/GetAPPPicList.ashx",
	                type: "POST",
	                dataType: "json",
	                data: { "PARAM": GetCallID() },
	                success: function(data) {
	                $("#pg").html(data.picList);
	                $('#gallery a').lightBox({ fixedNavigation: true });
	                }
	            });
	        }
	        
	        function GetCallID() {
	            var obj = {};
	            obj.callid = GetQueryString("CallID");
	            return JSON.stringify(obj);
	        }
	        function GetQueryString(name) {
	            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
	            var r = window.location.search.substr(1).match(reg);
	            if (r != null) return unescape(r[2]); return null;
	        }
    </script>
    </div>
    
</body>
</html>
