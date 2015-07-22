using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class Log_list : _Sys_Log
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            DdlCategory.DataSource = SysEnum.ToDictionary(typeof(SysEnum.LogType));
            DdlCategory.DataBind();
            DdlCategory.Items.Insert(0, new ListItem("不限", "0"));
            Sch();
        }
    }

    private void Sch()
    {
        string fromMenu = Request["fromMenu"];
        if (string.IsNullOrEmpty(fromMenu))
        {
            int PageSize = 50;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1 ";

            int sCateogry = Function.GetRequestInt("Cateogry");
            if (sCateogry > 0)
            {
                string ca = Enum.GetName(typeof(SysEnum.LogType), sCateogry);
                if (!string.IsNullOrEmpty(ca))
                {
                    strWhere += " and f_Category='" + ca + "' ";
                    url += "&Cateogry=" + sCateogry;
                    DdlCategory.SelectedValue = sCateogry.ToString();
                }
            }
            strWhere += " order by id desc ";
            GridView1.DataSource = LogBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);
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
                LogBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx?Cateogry="+DdlCategory.SelectedValue;
        Response.Redirect(Url);
    }
}
