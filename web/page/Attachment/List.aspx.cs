using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_Attachment_List : _Sys_Attachment
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            P_Manage.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.文件删除);

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1 ";

            DateTime DateBegin = Function.GetRequestDateTime("DateBegin");
            DateTime DateEnd = Function.GetRequestDateTime("DateEnd");
            string UserName = Function.GetRequestSrtring("UserName");
            int ID = Function.GetRequestInt("ID");
            if (DateBegin != Tool.Function.ErrorDate)
            {
                strWhere += "AND DATEDIFF(DAY,f_Addtime,'"+DateBegin+"')<=0";
                url += "&DateBegin="+DateBegin;
                TxbDateBegin.Text = DateBegin.ToString("yyyy-MM-dd");
            }
            if (DateEnd != Tool.Function.ErrorDate)
            {
                strWhere += "AND DATEDIFF(DAY,f_Addtime,'"+DateEnd+"')>=0";
                url += "&DateEnd="+DateEnd;
                TxbDateEnd.Text = DateEnd.ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(UserName))
            {
                strWhere += " AND f_UserName LIKE '%" + Function.ClearText(UserName) + "%'";
                url += "&UserName=" + UserName;
                TxbUserName.Text = UserName;
            }
            if (ID>0)
            {
                strWhere += " and ID="+ID;
            }
            strWhere += " order by id desc ";

            GridView1.DataSource = AttachmentBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);
        }
    }


    protected void ImgBtnDownLoad_Click(object sender, ImageClickEventArgs e)
    {
        int ID = Function.ConverToInt(((ImageButton)sender).CommandArgument);
        Response.Redirect("/page/sys/DownLoadFile.ashx?ID="+ID);
    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?DateBegin="+TxbDateBegin.Text;
        Url += "&DateEnd="+TxbDateEnd.Text;
        Url += "&UserName="+TxbUserName.Text;
        Response.Redirect(Url);
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
                AttachmentBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();

    }
}
