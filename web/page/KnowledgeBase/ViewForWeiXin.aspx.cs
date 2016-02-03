using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_KnowledgeBase_ViewForWeiXin : System.Web.UI.Page
{
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

            Repeater1.DataSource = AttachmentBLL.GetList(info.ID, AttachmentInfo.EUserFor.KnowledgeBase);
            Repeater1.DataBind();
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
