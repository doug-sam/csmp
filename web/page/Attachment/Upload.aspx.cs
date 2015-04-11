using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;
using Telerik.WebControls;
using System.IO;

public partial class page_Attachment_Upload : _Sys_Attachment_Upload
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int UserFor = Function.GetRequestInt("UserFor");
            if (UserFor <= 0)
            {
                UserFor = 1;
            }
            AttachmentInfo.EUserFor eUserFor = AttachmentInfo.EUserFor.Call;
            try
            {
                eUserFor = (AttachmentInfo.EUserFor)Enum.Parse(typeof(AttachmentInfo.EUserFor), UserFor.ToString());
            }
            catch (Exception)
            {
                eUserFor = AttachmentInfo.EUserFor.Call;
                
            }
            ViewState["eUserFor"] = eUserFor;
            switch (eUserFor)
            {
                case AttachmentInfo.EUserFor.Call:
                    int CallID = Function.GetRequestInt("CallID");
                    CallInfo cinfo = CallBLL.Get(CallID);
                    if (null == cinfo)
                    {
                        Function.AlertBack("参数有误");
                        return;
                    }
                    ViewState["info"] = cinfo;
                    break;
                case AttachmentInfo.EUserFor.KnowledgeBase:
                    int KnowledgeBaseID = Function.GetRequestInt("KnowledgeBaseID");
                    KnowledgeBaseInfo kinfo = KnowledgeBaseBLL.Get(KnowledgeBaseID);
                    if (null == kinfo)
                    {
                        Function.AlertBack("参数有误");
                        return;
                    }
                    ViewState["info"] = kinfo;
                    break;
                default:
                    break;
            }



        }
    }




    protected void BtnSave_Click(object sender, EventArgs e)
    {
       AttachmentInfo.EUserFor eUserFor= (AttachmentInfo.EUserFor)ViewState["eUserFor"];
        foreach (UploadedFile file in RadUploadContext.Current.UploadedFiles)
        {

            AttachmentInfo info = new AttachmentInfo();
            info.Addtime = DateTime.Now;
            

            info.CallStepID = 0;
            info.ContentType = file.ContentType;
            info.DirID = 0;
            info.Ext = file.GetExtension();
            info.UserID = CurrentUser.ID;
            info.UserName = CurrentUser.Name;

            info.FilePath = AttachmentBLL.GetOrCreateFilePath();
            info.FileSize = file.ContentLength;
            info.Memo = file.FileName.IndexOf("\\") > 0 ? file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1) : file.FileName;
            info.Title = DateTime.Now.ToString("MMddHHmmssfff") + Function.GetRand();
            info.UseFor = eUserFor.ToString();
            file.SaveAs(info.FilePath + info.Title + info.Ext, true);
            switch (eUserFor)
            {
                case AttachmentInfo.EUserFor.Call:
                    info.CallID = ((CallInfo)ViewState["info"]).ID;
                    break;
                case AttachmentInfo.EUserFor.KnowledgeBase:
                     info.CallID = ((KnowledgeBaseInfo)ViewState["info"]).ID;
                   break;
                default:
                    break;
            }


            info.ID = AttachmentBLL.Add(info);
            if (info.ID > 0)
            {
                if (!string.IsNullOrEmpty(Function.GetRequestSrtring("returnID")))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "self.parent.SetAttachmentID(" + info.ID + ");self.parent.tb_remove();", true);
                }
                else
                {
                    Function.AlertRefresh("上传成功，文件ID为:" + info.ID);
                    return;
                }
            }
            else
            {
                File.Delete(info.FilePath + info.Title + info.Ext);
                Function.AlertMsg("上传失败");
                break;
            }
        }
    }
}
