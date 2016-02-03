<%@ WebHandler Language="C#" Class="WordToHTML" %>

using System;
using System.Web;
using System.IO;
using CSMP.Model;
using CSMP.BLL;

public class WordToHTML : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        int id = 0;
        string urlPath = "";
        if (context.Request.QueryString["id"] != null)
        {
            id = Convert.ToInt32(context.Request.QueryString["id"].ToString());
        }
        if (id <= 0)
        {
            return;
        }

        AttachmentInfo file = AttachmentBLL.Get(id);
        if (null != file)
        {
            string filePath = file.FilePath + file.Title + file.Ext;
            //string filePath = "~/file/download/普通镜头配置说明-Gazelle.docx";
            string htmlPath = "~/file/download/";
            string htmlName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            bool flag = Tool.WordToHTML.Word2Html(filePath, context.Server.MapPath(htmlPath), htmlName);
            Tool.Logger.GetLogger(this.GetType()).Info("Word转HTML" + flag.ToString(), null);
            if (flag)
            {
                urlPath = "/file/download/" + htmlName + ".html";
            }
            else
            {
                urlPath = "";
            }
        }
        //context.Response.Write(urlPath);
        context.Response.Redirect(urlPath);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}