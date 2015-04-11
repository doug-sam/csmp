using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Comment_list : _KnowledgeBase_Comment
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            DdlWorkGroup1.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup2.DataSource = WorkGroupBLL.GetList();
            DdlWorkGroup1.DataBind();
            DdlWorkGroup2.DataBind();
            DdlWorkGroup1.Items.Insert(0, new ListItem("请选择", "0"));
            DdlWorkGroup2.Items.Insert(0, new ListItem("请选择", "0"));

            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url;
            StringBuilder SQL;
            GetSQL(out url, out SQL);

            InitTableStat(SQL);

            int Score2 = Function.GetRequestInt("Score2");
            if (Score2 > 0)
            {
                SQL.Append(" and f_Score2=").Append(Score2);
                url += "&Score2=" + Score2;
            }
            int Score3 = Function.GetRequestInt("Score3");
            if (Score3 > 0)
            {
                SQL.Append(" and f_Score3=").Append(Score3);
                url += "&Score3=" + Score3;
            }
            SQL.Append(" order by id desc ");
            GridView1.DataSource = CommentBLL.GetList(PageSize, PageIndex, SQL.ToString(), out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }

    private void InitTableStat(StringBuilder SQL)
    {
        #region 技术评价
        int CountAll = CommentBLL.GetCountScore(SQL.ToString(), 0, CommentInfo.ScoreType.Score2);
        HlAll.Text = CommentBLL.GetCountScore(SQL.ToString(), 0, CommentInfo.ScoreType.Score2).ToString();
        Hl1.Text = CommentBLL.GetCountScore(SQL.ToString(), 1, CommentInfo.ScoreType.Score2).ToString();
        Hl2.Text = CommentBLL.GetCountScore(SQL.ToString(), 2, CommentInfo.ScoreType.Score2).ToString();
        Hl3.Text = CommentBLL.GetCountScore(SQL.ToString(), 3, CommentInfo.ScoreType.Score2).ToString();
        Hl4.Text = CommentBLL.GetCountScore(SQL.ToString(), 4, CommentInfo.ScoreType.Score2).ToString();
        Hl5.Text = CommentBLL.GetCountScore(SQL.ToString(), 5, CommentInfo.ScoreType.Score2).ToString();
        if (CountAll != 0)
        {
            decimal AVG = CommentBLL.GetSum(SQL.ToString(), 0, CommentInfo.ScoreType.Score2);
            //Response.Write("总分是:"+AVG+"<br/>");
            //Response.Write("总条数是:" + CountAll + "<br/>");
            AVG = Math.Round(AVG / (decimal)CountAll, 2);
            //Response.Write("平均分是:" + AVG + "<br/>");
            HlAvg.Text = AVG.ToString();
        }
        
        #endregion

         CountAll = CommentBLL.GetCountScore(SQL.ToString(), 0, CommentInfo.ScoreType.Score3);
        HlAlla.Text = CommentBLL.GetCountScore(SQL.ToString(), 0, CommentInfo.ScoreType.Score3).ToString();
        Hl1a.Text = CommentBLL.GetCountScore(SQL.ToString(), 1, CommentInfo.ScoreType.Score3).ToString();
        Hl2a.Text = CommentBLL.GetCountScore(SQL.ToString(), 2, CommentInfo.ScoreType.Score3).ToString();
        Hl3a.Text = CommentBLL.GetCountScore(SQL.ToString(), 3, CommentInfo.ScoreType.Score3).ToString();
        Hl4a.Text = CommentBLL.GetCountScore(SQL.ToString(), 4, CommentInfo.ScoreType.Score3).ToString();
        Hl5a.Text = CommentBLL.GetCountScore(SQL.ToString(), 5, CommentInfo.ScoreType.Score3).ToString();
        if (CountAll != 0)
        {
            decimal AVG = CommentBLL.GetSum(SQL.ToString(), 0, CommentInfo.ScoreType.Score3);
            //Response.Write("总分是:"+AVG+"<br/>");
            //Response.Write("总条数是:" + CountAll + "<br/>");
            AVG = Math.Round(AVG / (decimal)CountAll, 2);
            //Response.Write("平均分是:" + AVG + "<br/>");
            HlAvga.Text = AVG.ToString();
        }
    }

    private void GetSQL(out string url, out StringBuilder SQL)
    {
        url = "";
        int DropInUserID = Function.GetRequestInt("DropInUserID");
        int SupportUserID = Function.GetRequestInt("SupportUserID");
        DateTime DtBegin = Function.GetRequestDateTime("DtBegin");
        DateTime DtEnd = Function.GetRequestDateTime("DtEnd");
        int CommandBy = Function.GetRequestInt("CommandBy");

        SQL = new StringBuilder(" 1=1 ");
        if (!IsAdmin)
        {
            SQL.Append(" and f_WorkGroupID=").Append(CurrentUser.WorkGroupID);
        }
        if (DropInUserID > 0)
        {
            SQL.Append(" and f_DropInUserID=").Append(DropInUserID);
            url += "&DropInUserID=" + DropInUserID;
            DdlDropInUser.SelectedValue = DropInUserID.ToString();
        }
        if (SupportUserID > 0)
        {
            SQL.Append(" and f_SupportUserID=").Append(SupportUserID);
            url += "&SupportUserID=" + SupportUserID;
            DdlSupportUser.SelectedValue = SupportUserID.ToString();
        }
        if (DtBegin != Function.ErrorDate)
        {
            SQL.Append(" and DATEDIFF(day,f_AddDate,'").Append(DtBegin).Append("')<=0");
            url += "&DtBegin=" + DtBegin.ToString("yyyy-MM-dd");
            TxtDateBegin.Text = DtBegin.ToString("yyyy-MM-dd");
        }
        if (DtEnd != Function.ErrorDate)
        {
            SQL.Append(" and DATEDIFF(day,f_AddDate,'").Append(DtEnd).Append("')>=0");
            url += "&DtEnd=" + DtEnd.ToString("yyyy-MM-dd");
            TxbDateEnd.Text = DtEnd.ToString("yyyy-MM-dd");
        }
        if (CommandBy > 0)
        {
            SQL.Append(" and f_IsDropInUserDoIt=").Append((CommandBy == 1) ? 1 : 0);
            url += "&CommandBy=" + CommandBy;
            DdlCommandBy.SelectedValue = CommandBy.ToString();
        }

    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        SchRedirect(0,0);
    }

    private void SchRedirect(int Score2,int Score3)
    {
        string URL = "list.aspx?";
        URL += "DropInUserID=" + DdlDropInUser.SelectedValue;
        URL += "&SupportUserID=" + DdlSupportUser.SelectedValue;
        URL += "&DtBegin=" + TxtDateBegin.Text;
        URL += "&DtEnd=" + TxbDateEnd.Text;
        URL += "&CommandBy=" + DdlCommandBy.SelectedValue;
        URL += "&Score2=" + Score2;
        URL += "&Score3=" + Score3;
        Response.Redirect(URL);
    }

    //private StringBuilder GetSQL()
    //{
    //    StringBuilder SQL = new StringBuilder();
    //    SQL.Append(" 1=1 ");
    //    if (!IsAdmin)
    //    {
    //        SQL.Append(" and f_WorkGroupID=").Append(CurrentUser.WorkGroupID);
    //    }

    //    if (DdlDropInUser.SelectedValue != "0")
    //    {
    //        SQL.Append(" and f_DropInUserID=").Append(DdlDropInUser.SelectedValue);
    //    }
    //    if (DdlSupportUser.SelectedValue != "0")
    //    {
    //        SQL.Append(" and f_SupportUserID=").Append(DdlSupportUser.SelectedValue);
    //    }
    //    DateTime DtBegin = Function.ConverToDateTime(TxtDateBegin.Text);
    //    if (DtBegin != Function.ErrorDate)
    //    {
    //        SQL.Append(" and DATEDIFF(day,f_AddDate,").Append(DtBegin).Append(")<=0");
    //    }
    //    DateTime DtEnd = Function.ConverToDateTime(TxbDateEnd.Text);
    //    if (DtEnd != Function.ErrorDate)
    //    {
    //        SQL.Append(" and DATEDIFF(day,f_AddDate,").Append(DtEnd).Append(")>=0");
    //    }
    //    if (DdlCommandBy.SelectedValue != "0")
    //    {
    //        SQL.Append(" and f_IsDropInUserDoIt=").Append((DdlCommandBy.SelectedValue == "1") ? 1 : 0);
    //    }
    //    return SQL;
    //}

    protected void Hl_Click(object sender, EventArgs e)
    {
        //StringBuilder SQL = GetSQL();
        int Score = Function.ConverToInt(((LinkButton)sender).CommandArgument);
        //GridView1.DataSource = CommentBLL.GetList(SQL.ToString(), Score);
        //GridView1.DataBind();
        SchRedirect(Score,0);

    }
    protected void Hla_Click(object sender, EventArgs e)
    {
        //StringBuilder SQL = GetSQL();
        int Score = Function.ConverToInt(((LinkButton)sender).CommandArgument);
        //GridView1.DataSource = CommentBLL.GetList(SQL.ToString(), Score);
        //GridView1.DataBind();
        SchRedirect(0,Score);

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        int DropInUserID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "DropInUserID").ToString());
        int SupportUserID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "SupportUserID").ToString());
        //HyperLink HyperLink1 = (HyperLink)e.Row.FindControl("HyperLink1");
        //if (CallStepBLL.GetReview(ID) != null)
        //    HyperLink1.Text = "已回访";
        //HyperLink1.NavigateUrl = "javascript:tb_show('回访', '/page/call/Review.aspx?ID=" + ID + "&TB_iframe=true&height=450&width=730', false);";
        Literal LtlDropInUser = (Literal)e.Row.FindControl("LtlDropInUser");
        Literal LtlSupportUser = (Literal)e.Row.FindControl("LtlSupportUser");

        UserInfo uinfo= UserBLL.Get(DropInUserID);
        LtlDropInUser.Text =uinfo==null?string.Empty: uinfo.Name;
        uinfo = UserBLL.Get(SupportUserID);
        LtlSupportUser.Text = uinfo == null ? string.Empty : uinfo.Name;
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
                CommentBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();

    }
    protected void DdlWorkGroup1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int WorkGroupID = Function.ConverToInt(DdlWorkGroup1.SelectedValue, 0);
        if (WorkGroupID>=0)
        {
            DdlDropInUser.DataSource = UserBLL.GetList(WorkGroupID, SysEnum.Rule.现场工程师.ToString());
        }
        else
        {
            DdlDropInUser.DataSource = new List<UserInfo>();
        }
        DdlDropInUser.DataBind();
        DdlDropInUser.Items.Insert(0, new ListItem("请选择", "0"));

    }
    protected void DdlWorkGroup2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int WorkGroupID = Function.ConverToInt(DdlWorkGroup2.SelectedValue, 0);
        if (WorkGroupID >= 0)
        {
            DdlSupportUser.DataSource = UserBLL.GetList(WorkGroupID, SysEnum.Rule.二线.ToString());
        }
        else
        {
            DdlSupportUser.DataSource = new List<UserInfo>();
        }
        DdlSupportUser.DataBind();
        DdlSupportUser.Items.Insert(0, new ListItem("请选择", "0"));

    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        Response.Write(DateTime.Now + " 开始导出程序<br/>");
        Response.Flush();

        string url = "";
        StringBuilder strWhere = new StringBuilder();
        GetSQL(out url, out strWhere);
        List<CommentInfo> list = CommentBLL.GetList(strWhere.ToString());
        if (list.Count > 10000)
        {
            Response.Write(DateTime.Now + " 数据多于10000条，系统很累，不能帮你导出，请分次吧<br/>");
            Response.Flush();
            return;
        }
        Response.Write(DateTime.Now + " 原始数据获取完成<br/>");
        Response.Flush();
        DataTable dt = ListToTable(list);
        Response.Write(DateTime.Now + " 对原始整理完成，现在开始漫长的导出过程，请耐心等待<br/>");
        Response.Flush();

        string InnerPath = "~/file/download/" + DateTime.Now.ToString("yyyy-MM") + "/Comment";
        string InnerName = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + ".xlsx";
        string FilePath = Server.MapPath(InnerPath);
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        string FileAll = FilePath + InnerName;

        Tool.DocHelper.ExportExcel(dt, FileAll);
        Response.Write(string.Format("{0} 恭喜你导出完成，<a href='{1}'>请点我下载</a>", DateTime.Now, InnerPath.TrimStart('~') + InnerName));
        Response.Flush();
        Response.End();
    }


    private DataTable ListToTable(List<CommentInfo> list)
    {
        							
        DataTable dt = new DataTable();
        dt.Columns.Add("评论方");
        dt.Columns.Add("现场工程师");
        dt.Columns.Add("现场工程师工作组");
        dt.Columns.Add("二线");
        dt.Columns.Add("二线所在工作组");
        dt.Columns.Add("技术评分");
        dt.Columns.Add("态度评分");
        dt.Columns.Add("总评分");
        dt.Columns.Add("日期");
        dt.Columns.Add("方式");

        DataRow dr = dt.NewRow();

        foreach (CommentInfo item in list)
        {
            dr = dt.NewRow();
            dr["评论方"] = item.IsDropInUserDoIt?"现场工程师":"二线";

            UserInfo uinfo = UserBLL.Get(item.DropInUserID);
            
            dr["现场工程师"] = uinfo == null ? "数据丢失" : uinfo.Name;
            dr["现场工程师工作组"] = WorkGroupBLL.GetWorkGroupName(uinfo.WorkGroupID);

            uinfo = UserBLL.Get(item.SupportUserID);
            dr["二线"] = uinfo == null ? "数据丢失" : uinfo.Name;
            dr["二线所在工作组"] = WorkGroupBLL.GetWorkGroupName(uinfo.WorkGroupID);
            dr["技术评分"] = "'" + item.Score2;
            dr["态度评分"] = "'" + item.Score3;
            dr["总评分"] = "'" + item.Score;
            dr["日期"] = "'"+item.AddDate.ToString("yyyy年MM月dd日 HH时mm分");
            dr["方式"] = item.ByMachine;
            dt.Rows.Add(dr);
        }

        return dt;
    }


}
