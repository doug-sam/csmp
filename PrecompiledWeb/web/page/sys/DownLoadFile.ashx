<%@ WebHandler Language="c#" Class="File_WebHandler" Debug="true" %>

using System;
using System.Web;
using System.IO;
using CSMP.Model;
using CSMP.BLL;

public class File_WebHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        int id = 0;
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
            string FileFullName = file.FilePath + file.Title + file.Ext;
            if (!File.Exists(FileFullName))
            {
                context.Response.Write("文件不存在");
                context.Response.End();
                return;
            }
            context.Response.ContentType = file.ContentType;
            context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(file.Title, System.Text.Encoding.UTF8) + "\"");

            context.Response.WriteFile(FileFullName);
            context.Response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}