<%@ WebHandler Language="c#" Class="File_WebHandler" Debug="true" %>

using System;
using System.Web;
using System.IO;

public class File_WebHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        
        string Path = context.Request.QueryString["Path"];
        //context.Response.Write(Path);
        //return;
        if (!string.IsNullOrEmpty(Path))
        {
            if (Path.IndexOf(":")<=0)
            {
                Path = context.Server.MapPath(Path);
            }
        }
        else
        {
            return;
        }

            context.Response.ContentType = "image/jpeg";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(DateTime.Now.ToString("yyyyMMddHHmmss"), System.Text.Encoding.UTF8) + "\"");
            context.Response.WriteFile(Path);
            context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}