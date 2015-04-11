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


public partial class page_Feedback_list : _Feedback
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbDateBegin.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");


            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));

            DdlCreatorID.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.一线.ToString());
            DdlCreatorID.DataBind();
            DdlCreatorID.Items.Insert(0, new ListItem("不限", "0"));

            DdlPaper.DataSource = FeedbackPaperBLL.GetList();
            DdlPaper.DataBind();

            int PageSize = 60;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url;
            string strWhere;
            GetSQL(out url, out strWhere);
            GridView1.DataSource = FeedbackAnswerBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);

        }
    }

    private void GetSQL(out string url, out string strWhere)
    {
        url = "";
        strWhere = " 1=1 ";
        string strCall = " AND f_CallID IN( select ID from sys_Calls WHERE 1=1 ";
        int CustomerID = Function.GetRequestInt("CustomerID");
        if (CustomerID > 0)
        {
            url += "&CustomerID=" + CustomerID;
            strCall += " AND f_CustomerID=" + CustomerID;
            DdlCustomer.SelectedValue = CustomerID.ToString();
            DdlCustomer_SelectedIndexChanged(null, null);

            int BrandID = Function.GetRequestInt("BrandID");
            if (BrandID > 0)
            {
                DdlBrand.SelectedValue = BrandID.ToString();
                url += "&BrandID=" + BrandID;
                strCall += " AND f_BrandID=" + BrandID;
            }
        }

        int Class1ID = Function.GetRequestInt("Class1ID");
        if (Class1ID > 0)
        {
            DdlClass1.SelectedValue = Class1ID.ToString();
            url += "&Class1ID=" + Class1ID;
            strCall += " AND Class1ID=" + Class1ID;
        }


        string Store = Function.GetRequestSrtring("Store");
        if (!string.IsNullOrEmpty(Store))
        {
            TxbStore.Text = Store;
            url += "&Store=" + Class1ID;
            strCall += string.Format(" AND  (f_StoreName like '%{0}%' or f_StoreNo like '%{0}%') ", Store);
        }
        string No = Function.GetRequestSrtring("No");
        if (!string.IsNullOrEmpty(No))
        {
            TxbNo.Text = No;
            url += "&No=" + No;
            strCall += string.Format(" AND f_No LIKE '%{0}%' ", No);
        }

        strCall += " ) ";
        int CreatorID = Function.GetRequestInt("CreatorID");
        if (CreatorID > 0)
        {
            DdlCreatorID.SelectedValue = CreatorID.ToString();
            url += "&CreatorID=" + CreatorID;
            strWhere += " AND f_RecorderID=" + CreatorID;
        }

        DateTime DateBegin = Function.GetRequestDateTime("DateBegin");
        if (DateBegin != Function.ErrorDate)
        {
            TxbDateBegin.Text = DateBegin.ToString("yyyy-MM-dd");
            url += "&DateBegin=" + TxbDateBegin.Text;
            strWhere += string.Format("  AND DATEDIFF(DAY,f_AddDate,'{0}')<=0 ", DateBegin);
        }
        DateTime DateEnd = Function.GetRequestDateTime("DateEnd");
        if (DateEnd != Function.ErrorDate)
        {
            TxbDateEnd.Text = DateEnd.ToString("yyyy-MM-dd");
            url += "&DateEnd=" + TxbDateEnd.Text;
            strWhere += string.Format(" AND  DATEDIFF(DAY,f_AddDate,'{0}')>=0 ", DateEnd);
        }

        strWhere += strCall;
        strWhere += " order by f_AddDate desc";
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.DataItem == null) return;
        if (null == Session["CurrentColorIsDark"])
        {
            Session["CurrentColorIsDark"] = true;
        }
        int CallIDPre = 0;
        if (null != Session["CallIDPre"])
        {
            CallIDPre = Function.ConverToInt(Session["CallIDPre"], 0);
        }
        int CallID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "CallID").ToString());
        if (CallIDPre != CallID)
        {
            Session["CurrentColorIsDark"] = !Convert.ToBoolean(Session["CurrentColorIsDark"]);
            Session["CallIDPre"] = CallID;
        }
        if (Session["CurrentColorIsDark"] != null && Session["CurrentColorIsDark"].ToString().ToLower() == "true")
        {
            e.Row.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
        }
        else
        {
            e.Row.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
        }
        CallInfo info = CallBLL.Get(CallID);
        if (null != info)
        {
            ((Literal)e.Row.FindControl("LtlBrand")).Text = info.BrandName;
            ((Literal)e.Row.FindControl("LtlStore")).Text = info.StoreName+"<br/>"+info.StoreNo;
        }

    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?CustomerID=" + DdlCustomer.SelectedValue;
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Url += "&Class1ID=" + DdlClass1.SelectedValue;
        Url += "&Store=" + Server.UrlEncode(TxbStore.Text.Trim());
        Url += "&No=" + Server.UrlEncode(TxbNo.Text.Trim());
        Url += "&CreatorID=" + DdlCreatorID.SelectedValue;
        Url += "&DateBegin=" + TxbDateBegin.Text.Trim();
        Url += "&DateEnd=" + TxbDateEnd.Text.Trim();
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
                FeedbackAnswerBLL.Delete(Function.ConverToInt(item, 0));
            }
        }
        Function.Refresh();
    }
    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID <= 0)
        {
            DdlClass1.DataSource = null;
            DdlClass1.DataBind();

            DdlBrand.DataSource = null;
            DdlBrand.DataBind();
        }
        else
        {
            DdlClass1.DataSource = Class1BLL.GetList(ID);
            DdlClass1.DataBind();
            DdlClass1.Items.Insert(0, new ListItem("不限", "0"));
            DdlBrand.DataSource = BrandBLL.GetList(ID, CurrentUser.WorkGroupID);
            DdlBrand.DataBind();
            DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
        }
    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {



        string url;
        string strWhere;
        GetSQL(out url, out strWhere);

        List<FeedbackAnswerInfo> listAnswer = FeedbackAnswerBLL.GetList(strWhere);


        string InnerPath = "~/file/download/" + DateTime.Now.ToString("yyyy-MM") + "/";
        string InnerName = "Feedback" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        string FilePath = Server.MapPath(InnerPath);
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        string FileAll = FilePath + InnerName;
        FeedbackAnswerBLL.ExportExcel(listAnswer, Function.ConverToInt(DdlPaper.Items[0].Value), FileAll);

        Response.Redirect(InnerPath + InnerName);
    }
}
