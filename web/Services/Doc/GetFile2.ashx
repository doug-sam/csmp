<%@ WebHandler Language="C#" Class="GetFile2" %>

using System;
using System.Web;
using System.IO;
using System.Text;
using Tool;
using System.Drawing;
using CSMP.Model;
using CSMP.BLL;

public class GetFile2 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        CallStepInfo sinfo=null;

        try
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            int CallID = Function.Request.GetRequestInt(context, "CallID");
            CallInfo cinfo = CallBLL.Get(CallID);
            if (null==cinfo)
            {
                context.Response.Write("parmeError");
                return;
            }
            
            sinfo = CallStepBLL.GetLast(cinfo.ID);
            if (null == sinfo )
            {
                context.Response.Write("parmeError");
                return;
            }

            AttachmentInfo info = new AttachmentInfo();
            info.Addtime = DateTime.Now;
            info.CallID = cinfo.ID;
            info.CallStepID = sinfo.ID;
            info.ContentType = "image/pjpeg";
            info.DirID = 0;
            info.Ext = ".jpg";
            info.UserID = sinfo.MajorUserID;
            info.UserName = sinfo.MajorUserName;

            info.FilePath = AttachmentBLL.GetOrCreateFilePath();

            System.IO.Stream stream = context.Request.InputStream;//这是你获得的流 
            //byte[] buffer = new byte[stream.Length];
            //stream.Read(buffer, 0, buffer.Length);     //将流的内容读到缓冲区 
            info.FileSize = (int)stream.Length;
            info.Memo = string.Empty;
            info.Title = cinfo.No + "_" + sinfo.ID + "_" + sinfo.StepIndex + "_" + DateTime.Now.ToString("MMddHHmmssfff");
  
            HttpPostedFile oFile = HttpContext.Current.Request.Files[HttpContext.Current.Request.Files.AllKeys[0]];
            //oFile.ContentType
            oFile.SaveAs(info.FilePath + info.Title + info.Ext);//上传图片
            // System.IO.FileStream fs = new System.IO.FileStream(info.FilePath + info.Title + info.Ext, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            //fs.Write(buffer, 0, buffer.Length);

            
            
            //fs.Flush();
            //fs.Close();
            if (AttachmentBLL.Add(info) <= 0)
            {
                File.Delete(info.FilePath + info.Title + info.Ext);
                context.Response.Write("fail");
                return;
            }
            context.Response.Write("success");


        }
        catch (Exception ex)
        {
            context.Response.Write("fail with exception:" + ex.Message);
            LogInfo info = new LogInfo();
            info.AddDate = DateTime.Now;
            info.Category = SysEnum.LogType.系统出错.ToString();
            info.Content ="上传图片出错：" +ex.Message;
            info.ErrorDate=DateTime.Now;
            info.SendEmail=false;
            info.Serious=1;
            info.UserName = null == sinfo ? "未知" : sinfo.MajorUserName;
            LogBLL.Add(info);
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