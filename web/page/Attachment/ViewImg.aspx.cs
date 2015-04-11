using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Attachment_ViewImg : _Sys_Attachment
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int ID = Function.GetRequestInt("ID");
        AttachmentInfo info = AttachmentBLL.Get(ID);
        if (null==info)
        {
            Function.AlertMsg("数据有误");
            return;
        }

        if (info.ContentType.IndexOf("image")<0)
        {
            Response.Write("<div style='font-size:44px;font-weight:bold;text-align:center;margin-top:200px;'>");
            Response.Write("        只有图片能提供直接浏览器。其它格式文件请下载到本地");
            Response.Write("</div>");
            return;
        }

        try
        {
            ///创建文件流，以读取图像
            FileStream fs = new FileStream( info.FilePath + info.Title + info.Ext, FileMode.Open, FileAccess.Read);
            ///定义保存图像数据的二进制数组
            byte[] imageData = new byte[(int)fs.Length];
            ///读取文件的二进制数据
            fs.Read(imageData, 0, (int)fs.Length);
            ///输出图像的二进制数据
            Response.BinaryWrite(imageData);
            ///设置页面的输出格式，【注意】：在此只能输出jpg图片
            Response.ContentType = info.ContentType;
            Response.End();   ///中止页面的其他输出
        }
        catch (Exception ex)
        {
            if (IsAdmin)
            {
                throw ex;
            }
            else
            {
                Response.Write("文件无法显示，请下载到本地");
            }
        }



    }
}