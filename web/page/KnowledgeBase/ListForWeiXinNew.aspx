<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListForWeiXinNew.aspx.cs" Inherits="page_KnowledgeBase_ListForWeiXinNew" %>

<!DOCTYPE html>

<html >
<head runat="server">
    <title></title>
    <link href="../../js/jquery-mobile/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jquery-mobile/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../../js/jquery-mobile/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //获取URL参数
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        var openId = GetQueryString('openid');
        function loadCustomer() {

            $.ajax({
            url: "/page/Customer/GetListByOpenID.ashx?openId=" + openId,
                type: "POST",
                dataType: "json",
                success: function(data) {

                    $("#ddlCustomer").html(data.resultInfo);
                    // $("#select-choice-1").listview("refresh");
                },
                error: function() {
                    $("#ddlCustomer").html("<option value='0'>请选择</option>");
                }
            });

        }

        function loadBrand(customer) {

            $.ajax({
            url: "/page/Brand/GetListByOpenIdAndCustomerName.ashx?openId=" + openId + "&customerName=" + encodeURIComponent(customer),
            type: "POST",
            dataType: "json",
            success: function(data) {
                $("#ddlBrand option").remove();
                $("#ddlBrand option[value=0]").attr("selected", true);
                $("#ddlBrand").append(data.resultInfo);
                $("#ddlBrand").selectmenu("refresh");
                
            },
            error: function() {
                $("#ddlBrand").listview("refresh");
                $("#ddlBrand").html("<option value='0'>请选择</option>");
            }
            });
        }


        function queryKnows() {

            $.ajax({
            url: "/page/KnowledgeBase/GetListForWeixin.ashx?customerId=" + $("#ddlCustomer  option:selected").text() + "&brandId=" + $("#ddlBrand  option:selected").text() + "&openId=" + openId,
            type: "POST",
            dataType: "json",
                success: function(data) {

                $("#listAll").html(data.resultInfo);
                    // $("#select-choice-1").listview("refresh");
                },
                error: function() {
                $("#listAll").html("<br/><h2>暂无数据</h2>");
                }
            });

        }

        $(document).ready(function() {
            loadCustomer();
            //queryKnows();

            $("#ddlCustomer").change(function() {
                loadBrand($(this).children("option:selected").text());
            });
            
        });

        
    </script>
</head>
<body>
    <div data-role="page" id="pageone">
      <div data-role="header">
        <h1>知识库查看</h1>
      </div>
      <div data-role="content">
        <div class="ui-grid-solo">
            <div class="ui-block-a">
		        <label for="text-basic">客户：</label> 
		        <select name="ddlCustomer" id="ddlCustomer">
		        </select>
		    </div>
	    </div>
	    <div class="ui-grid-solo">
	        <div class="ui-block-a">
		        <label for="text-basic">品牌：</label> 
		        <select name="ddlBrand" id="ddlBrand">
		        </select>
		    </div>
	    </div>
    	 	
        <div class="ui-grid-solo">
	        <div class="ui-block-a"><button type="button" data-theme="b" onclick="queryKnows()">查询知识库</button></div>
        </div>
        <br />
        <ul id="listAll" data-role="listview" data-autodividers="false" data-inset="true" data-filter="true">
        </ul>
      </div>
    </div> 
</body>
</html>
