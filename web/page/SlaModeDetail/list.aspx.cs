using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Data;
using System.IO;


public partial class page_SlaModeDetail_list : _BaseData_SLAModel
{
    protected SlaModeInfo GetInfo()
    {
        SlaModeInfo info;
        if (ViewState["info"] != null)
        {
            info = (SlaModeInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {

            info = SlaModeBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            SlaModeInfo info = GetInfo();
            if (null==info||info.ID==0)
            {
                Function.AlertBack("parms error");
                return;
            }
            
            string strWhere = string.Format(" 1=1 and f_SlaModeID={0}  order by ID desc ",info.ID);
            GridView1.DataSource = SlaModeDetailBLL.GetList(strWhere);
            GridView1.DataBind();

        }
    }

    protected void Btn_Delete(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertBack("没有选中数据");
            return;
        }
        foreach (string item in delList.Split(','))
        {
            if (item.Length > 0)
            {
                SlaModeDetailBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        //int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        //HyperLink HyperLink1 = (HyperLink)e.Row.FindControl("HyperLink1");
        //if (CallStepBLL.GetReview(ID) != null)
        //    HyperLink1.Text = "已回访";
        //HyperLink1.NavigateUrl = "javascript:tb_show('回访', '/page/call/Review.aspx?ID=" + ID + "&TB_iframe=true&height=450&width=730', false);";

    }

}
