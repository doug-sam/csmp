   //图片按比例缩放   
   var flag = false;   
function DrawImage(ImgD, iwidth, iheight) {   
	  //参数(图片,允许的宽度,允许的高度)   
	  var image = new Image();   
	  image.src = ImgD.src;   
	  if (image.width > 0 && image.height > 0) {   
		  flag = true;   
		if (image.width / image.height >= iwidth / iheight) {   
			if (image.width > iwidth) {   
				ImgD.width = iwidth;   
				ImgD.height = (image.height * iwidth) / image.width;   
			} else {   
				ImgD.width = image.width;   
				ImgD.height = image.height;   
			}   
			ImgD.alt = image.width + "×" + image.height;   
		}   
		else {   
			if (image.height > iheight) {   
				ImgD.height = iheight;   
				ImgD.width = (image.width * iheight) / image.height;   
			} else {   
				ImgD.width = image.width;   
				ImgD.height = image.height;   
			}   
			ImgD.alt = image.width + "×" + image.height;   
		}   
	}   
}    
//<img onload="javascript:DrawImage(this,89,63)" src="http://www.baidu.com/img/baidu_logo.gif" width="89" height="63" border="0" />
