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

public partial class page_KnowledgeBase_EditForAtt : _KnowledgeBase_Library
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));


            KnowledgeBaseInfo info = GetInfo();
            if (null != info)
            {
                if (!GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.知识库编辑删除))
                {
                    Function.AlertBack("权限不足");
                    return;
                }
                LtlAction.Text = "编辑";
                labTitle.Text = info.Title;
                CbEnable.Checked = !info.Enable;
                LabUserInfo.Text = string.Format("添加人：{0}；添加时间：{1}；当前点击量：{2}，顶数：{3}", info.AddByUserName, info.AddDate, info.ViewCount, info.GoodCount);
                List<BrandInfo> list = BrandBLL.GetListByKnowledgeID(info.ID);
                BindClass3(list);
                FileUpload1.Visible = false;
                BtnSave.Visible = false;
                
            }
            else
            {
                if (!GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.知识库添加))
                {
                    Function.AlertBack("权限不足");
                    return;
                }
                
            }
        }
    }

    public KnowledgeBaseInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (KnowledgeBaseInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        KnowledgeBaseInfo info = KnowledgeBaseBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        
        KnowledgeBaseInfo info = GetInfo();
        if (info == null)
        {
            Function.AlertMsg("请先上传一个文件！");
            return;
        }


        List<BrandInfo> list = null;
        if (null != ViewState["listBrandInfo"])
        {
            list = (List<BrandInfo>)ViewState["listBrandInfo"];
        }


        bool result = false;
        if (ViewState["INFO"] == null)
        {
            info.ID = KnowledgeBaseBLL.Add(info);
            result = info.ID > 0;
        }
        else
        {
            info.Enable = CbEnable.Checked ? false : true;
            result = KnowledgeBaseBLL.Edit(info);
        }

        if (result)
        {
            UpdateKnowledgeClass(info, list);
            Function.AlertRedirect(LtlAction.Text + "成功", "EditForAtt.aspx?ID=" + info.ID);
        }
        else
        {
            Function.AlertMsg(LtlAction.Text + "失败");
        }
    }

    private static void UpdateKnowledgeClass(KnowledgeBaseInfo info, List<BrandInfo> list)
    {
        if (null != list)
        {
            KnowkedgeBaseBrandBLL.DeleteByKnowledgeID(info.ID);
            foreach (BrandInfo item in list)
            {
                KnowkedgeBaseBrandInfo kbcInfo = new KnowkedgeBaseBrandInfo();
                kbcInfo.BrandID = item.ID;
                kbcInfo.KnowledgeID = info.ID;
                KnowkedgeBaseBrandBLL.Add(kbcInfo);

            }
        }
    }
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue, 0);
        if (ID <= 0)
        {
            DdlBrand.DataSource = new List<BrandInfo>();
        }
        else
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
        }
        DdlBrand.DataBind();
        DdlBrand.Items.Insert(0, new ListItem("请选择", "0"));

    }
    protected void BtnAddBrand_Click(object sender, EventArgs e)
    {
        int BrandID = Function.ConverToInt(DdlBrand.SelectedValue, 0);
        if (ViewState["listBrandInfo"] == null)
        {
            ViewState["listBrandInfo"] = new List<BrandInfo>();
        }
        BrandInfo info = BrandBLL.Get(BrandID);
        if (null == info)
        {
            return;
        }
        List<BrandInfo> list = (List<BrandInfo>)ViewState["listBrandInfo"];
        bool HaveRecordFlag = false;
        foreach (BrandInfo item in list)
        {
            if (item.ID == info.ID)
            {
                HaveRecordFlag = true;
                break;
            }
        }
        if (!HaveRecordFlag)
        {
            list.Add(info);
        }
        BindClass3(list);
    }

    private void BindClass3(List<BrandInfo> list)
    {
        ViewState["listBrandInfo"] = list;
        Repeater1.DataSource = list;
        Repeater1.DataBind();
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        
        if (RadUploadContext.Current.UploadedFiles.Count > 1)
        {
            Function.AlertMsg("只允许上传一个文件！" );
            return;
        }
        if (RadUploadContext.Current.UploadedFiles.Count < 1)
        {
            Function.AlertMsg("请上传一个文件！");
            return;
        }
        UploadedFile file = RadUploadContext.Current.UploadedFiles[0];
        

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
        info.UseFor = AttachmentInfo.EUserFor.KnowledgeBase.ToString();
        file.SaveAs(info.FilePath + info.Title + info.Ext, true);


        KnowledgeBaseInfo knowinfo = new KnowledgeBaseInfo();
        knowinfo.AddByUserName = CurrentUser.Name;
        knowinfo.AddDate = DateTime.Now;
        knowinfo.GoodCount = 0;
        knowinfo.ViewCount = 0;
        knowinfo.Title = info.Memo.Replace(info.Ext, "");
        knowinfo.Content = knowinfo.Title;
        knowinfo.Labs ="";
        knowinfo.Enable = CbEnable.Checked?false:true;
        knowinfo.KnowledgeType = 1;
        knowinfo.ID = KnowledgeBaseBLL.Add(knowinfo);
        ViewState["INFO"] = knowinfo;

        info.CallID = knowinfo.ID;

        info.ID = AttachmentBLL.Add(info);
        if (info.ID > 0)
        {
            if (!string.IsNullOrEmpty(Function.GetRequestSrtring("returnID")))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "self.parent.SetAttachmentID(" + info.ID + ");self.parent.tb_remove();", true);
            }
            else
            {
                Function.AlertRedirect("上传成功，文件ID为:" + info.ID, "EditForAtt.aspx?ID=" + knowinfo.ID);
               
                return;
            }
        }
        else
        {
            File.Delete(info.FilePath + info.Title + info.Ext);
            Function.AlertMsg("上传失败");
        }
        
    }
}
