using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_KnowledgeBase_ViewNew : _KnowledgeBase_Library
{
    public string content = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            KnowledgeBaseInfo info = GetInfo();
            if (null == info)
            {
                Response.End();
                return;
            }
            info.ViewCount++;
            KnowledgeBaseBLL.Edit(info);

            List<AttachmentInfo> attList = AttachmentBLL.GetList(info.ID, AttachmentInfo.EUserFor.KnowledgeBase);
            if (attList != null && attList.Count > 0)
            {
                foreach (AttachmentInfo att in attList)
                {
                    if (att.ContentType.StartsWith("image"))
                    {
                        content += "<li><a href='/page/Attachment/ViewImg.aspx?ID=" + att.ID + "' target='_blank'>" + att.Memo + "</a></li>";
                        
                    }
                    else if (att.Ext == ".doc" || att.Ext == ".docx")
                    {

                        content += "<li><a href='/page/sys/WordToHTML.ashx?ID=" + att.ID + "' target='_blank'>" + att.Memo + "</a></li>";
                    }
                    else {
                        content += "<li><a href='/page/sys/DownLoadFile.ashx?ID=" + att.ID + "' target='_blank'>" + att.Memo + "</a></li>";
                    }
                }
            }
            else { 
            
            }

            //Repeater1.DataSource = AttachmentBLL.GetList(info.ID, AttachmentInfo.EUserFor.KnowledgeBase);
            //Repeater1.DataBind();
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

    protected void LbGood_Click(object sender, EventArgs e)
    {
        string cookiesName = "KnowledgebaseGood";
        string cookiesKey = "Good_" + GetInfo().ID;
        if (!string.IsNullOrEmpty(CookiesHelper.GetCookieValue(cookiesName, cookiesKey)))
        {
            Function.AlertMsg("你顶过啦");
            return;
        }
        KnowledgeBaseInfo info = GetInfo();
        info.GoodCount++;
        if (KnowledgeBaseBLL.Edit(info))
        {
            CookiesHelper.AddCookie(cookiesName, cookiesKey, cookiesKey, DateTime.Now.AddDays(1));

            Function.AlertRefresh("顶到了");
        }
    }
}
