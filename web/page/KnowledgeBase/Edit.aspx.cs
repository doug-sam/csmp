using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_KnowledgeBase_Edit : _KnowledgeBase_Library
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("请选择", "0"));


            KnowledgeBaseInfo info = GetInfo();
            if (null!=info)
            {
                if (!GroupBLL.PowerCheck((int)PowerInfo.P6_KnowledgeBase.知识库编辑删除))
                {
                    Function.AlertBack("权限不足");
                    return;
                }
                LtlAction.Text = "编辑";
                TxbTitle.Text = info.Title;
                TxbContent.Text = info.Content;
                TxbLab.Text = info.Labs;
                LabUserInfo.Text = string.Format("添加人：{0}；添加时间：{1}；当前点击量：{2}，顶数：{3}", info.AddByUserName, info.AddDate,info.ViewCount,info.GoodCount);
                List<BrandInfo> list = BrandBLL.GetListByKnowledgeID(info.ID);
                BindClass3(list); 

                string UploadText="<a href='javascript:tb_show(\"\", \"/page/Attachment/Upload.aspx?UserFor={0}&KnowledgeBaseID={1}&returnID=true&TB_iframe=true&height=300&width=500&modal=false\", false); return false;'>点击上传</a>";
                LtlUpload.Text = string.Format(UploadText, (int)AttachmentInfo.EUserFor.KnowledgeBase, info.ID);
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
        if (!DataValidator.IsLen(TxbTitle.Text.Trim(), UpdatePanel1, this.GetType(), "标题", 2, 50))
        {
            return;
        }
        KnowledgeBaseInfo info = GetInfo();
        if (info == null)
        {
            info = new KnowledgeBaseInfo();
            info.AddByUserName = CurrentUser.Name;
            info.AddDate = DateTime.Now;
            info.GoodCount = 0;
            info.ViewCount = 0;
        }
        info.Title = TxbTitle.Text.Trim();
        info.Content = TxbContent.Text; ;
        info.Labs = TxbLab.Text;
        info.Enable = true;

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
            result = KnowledgeBaseBLL.Edit(info);
        }

        if (result)
        {
            UpdateKnowledgeClass(info, list);
            Function.AlertRedirect(LtlAction.Text + "成功","Edit.aspx?ID="+info.ID);
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
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue,0);
        if (ID<=0)
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
        if (null==info)
        {
            return;
        }
        List<BrandInfo> list = (List<BrandInfo>)ViewState["listBrandInfo"];
        bool HaveRecordFlag = false;
        foreach (BrandInfo item in list)
        {
            if (item.ID==info.ID)
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
}
