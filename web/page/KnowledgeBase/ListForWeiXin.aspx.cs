using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Data;

public partial class page_KnowledgeBase_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string openid=Request["openid"];
            if (string.IsNullOrEmpty(openid))
            {
                HttpContext.Current.Response.Redirect("/Logout.aspx");
                return;
            }
            TxbLabs.Text = openid;
            hidOpenid.Value = openid;

            Sch();

        }
    }


    private void Sch()
    {
        int PageSize = 20;  //每页记录数
        int PageIndex = Function.GetRequestInt("page");  //当前页码
        int page = 10;      //分页显示数
        int count = 0;     //记录总数
        string url = "";
        string strWhere = " 1=1 ";

        
        strWhere += " and  id in(select f_KnowledgeID from sys_KnowkedgeBaseBrand where f_BrandID in";
        strWhere += " (SELECT f_MID from sys_WorkGroupBrand INNER JOIN  (SELECT f_WorkGroupID FROM sys_User LEFT JOIN wx_bindings ON sys_User.ID = f_userId WHERE f_openId ='" + hidOpenid.Value.Trim() + "' ) NewWorkGroupInfo ON sys_WorkGroupBrand.f_WorkGroupID=NewWorkGroupInfo.f_WorkGroupID)";
        strWhere +=  ")";
                
        string wd = Function.ClearText(Function.GetRequestSrtring("wd"));
        if (!string.IsNullOrEmpty(wd))
        {
            strWhere += string.Format(" and ( f_Labs like '%{0}%' or  f_Title like  '%{0}%')", wd);
            url += "&wd=" + wd;
            TxbLabs.Text = wd;
        }

        strWhere += " order by id desc ";
        GridView1.DataSource = KnowledgeBaseBLL.GetList(PageSize, PageIndex, strWhere, out count);
        GridView1.DataBind();
        this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        Image Control1 = (Image)e.Row.FindControl("ImgHaveAttachment");
        Control1.Visible = AttachmentBLL.GetList(ID, AttachmentInfo.EUserFor.KnowledgeBase).Count > 0;

    }



    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "ListForWeiXin.aspx";
        Url += "?openid=" + hidOpenid.Value.Trim();
        Url += "&wd=" + TxbLabs.Text.Trim();
        Response.Redirect(Url);
    }
}
