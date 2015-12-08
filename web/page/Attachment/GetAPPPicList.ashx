<%@ WebHandler Language="C#" Class="GetAPPPicList" %>

using System;
using System.Web;
using Newtonsoft.Json.Linq;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public class GetAPPPicList : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string param = context.Request.Form["PARAM"];
        JObject obj = JObject.Parse(param);
        string callid = obj["callid"].ToString();
        callid = callid.Trim('"');
        string result = string.Empty;
        if (string.IsNullOrEmpty(callid))
        {
            result = "{\"picList\":\"<h2>没有对应的图片</h2>";
        }
        else {
            System.Collections.Generic.List<AttachmentInfo> aList = AttachmentBLL.GetList(" f_CallID=" + callid + " AND f_Title='APP上传的图片列表'");
            if (aList.Count > 0)
            {
                string appPicCommonUrl = "http://hesheng.zen110.com/picture/";
                int i = 1;
                result += "{\"picList\":\"";
                foreach (AttachmentInfo item in aList)
                {
                    result += "<li>";
                    result += "<a href='" + appPicCommonUrl + item.FilePath + "' title='APP上传的图片image" + i + "'>";
                    result += "<img src='" + appPicCommonUrl + item.FilePath + "' width='72' height='72' alt=''>";
                    result += "</a></li>";
                    i++;

                }
                result += "\"}";
            }
            else
            {
                result = "{\"picList\":\"<h2>没有对应的图片</h2>";
            }
        
        }
        
        context.Response.Write(result);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}