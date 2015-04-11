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


public partial class page_CustomerRequest_list : _CustomerRequest
{
    protected void Page_Load(object sender, EventArgs e)
    {
        P_Manage.Visible = GroupBLL.PowerCheck((int)PowerInfo.P7_CustomerRequest.删除请求);
        LtlAdd.Visible = GroupBLL.PowerCheck((int)PowerInfo.P7_CustomerRequest.添加请求);
        TrEnableDataAndCallID.Visible = GroupBLL.PowerCheck((int)PowerInfo.P7_CustomerRequest.无效数据及受理情况检索);
        if (!IsPostBack)
        {

            List<CustomersInfo> list = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataSource = list;
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));
            //if (list.Count==1)
            //{
            //    DdlCustomer.Visible = false;
            //    DdlBrand.DataSource = BrandBLL.GetListByWorkGroup(list[0].ID);
            //    DdlBrand.DataBind();
            //    DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
            //}



            int PageSize = 20;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1 ";
            SchWhere(ref url, ref strWhere);
            strWhere += " order by ID desc ";
            GridView1.DataSource = CustomerRequestBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);
            List<string> HideHead = new List<string>();
            if (!GroupBLL.PowerCheck((int)PowerInfo.P7_CustomerRequest.删除请求))
            {
                HideHead.Add("选择");
                GridViewHide(HideHead);
                BtnDelete.Visible = false;
            }

            if (!GroupBLL.PowerCheck((int)PowerInfo.P7_CustomerRequest.编辑请求))
            {
                HideHead.Add("编辑");
            }

            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.新建报修))
            {
                HideHead.Add("转换成报修");
            }

        }
    }

    private void SchWhere(ref string url, ref string strWhere)
    {
        if (!IsAdmin)
        {
            strWhere += " and f_BrandID in(select f_MID from sys_WorkGroupBrand where f_WorkGroupID=" + CurrentUser.WorkGroupID + ") ";
        }
        int BrandID = Function.GetRequestInt("BrandID");
        if (BrandID>0)
        {
            BrandInfo binfo = BrandBLL.Get(BrandID);
            if (null!=binfo)
            {
                strWhere += " and f_BrandID=" + BrandID;
                url += "&BrandID=" + BrandID;

                DdlCustomer.SelectedValue = binfo.CustomerID.ToString();
                DdlCustomer_SelectedIndexChanged(null, null);
                DdlBrand.SelectedValue = binfo.ID.ToString();
            }
        }
        string wd = Function.GetRequestSrtring("wd");
        if (!string.IsNullOrEmpty(wd))
        {
            strWhere += string.Format(" and (f_StoreNo like '%{0}%' or f_StoreName like '%{0}%')",Function.ClearText(wd), Function.ClearText(wd));
            url += "&wd="+wd;
            TxbWd.Text = wd;
        }

        DateTime dtBegin = Function.GetRequestDateTime("DateBegin");
        if (dtBegin != Function.ErrorDate)
        {
            strWhere += " AND DATEDIFF(DAY,f_ErrorReportDate,'" + dtBegin + "')<=0 ";
            url += "&DateBegin=" + dtBegin;
            TxbDateBegin.Text = dtBegin.ToString("yyyy-MM-dd");
        }
        DateTime dtEnd = Function.GetRequestDateTime("DateEnd");
        if (dtEnd != Function.ErrorDate)
        {
            strWhere += " AND DATEDIFF(DAY,f_ErrorReportDate,'" + dtEnd + "')>=0 ";
            url += "&DateEnd=" + dtEnd;
            TxbDateEnd.Text = dtEnd.ToString("yyyy-MM-dd");
        }
        int EnableData = Function.GetRequestInt("EnableData");
        if (EnableData >= 0)
        {
            strWhere += " AND f_Enable=" + (EnableData == 1 ? 1 : 0);
            url += "&EnableData=" + EnableData;
            DdlEnable.SelectedValue = EnableData == 1 ? "1" : "0";
        }
        int IsDeal = Function.GetRequestInt("IsDeal");
        if (IsDeal >= 0)
        {
            strWhere +=string.Format(" AND f_CallID{0}0",(IsDeal==1?">":"="));
            url += "&IsDeal=" + IsDeal;
            DdlDeal.SelectedValue = IsDeal == 1 ? "1" : "0";
        }
    }



    private void GridViewHide(List<string> header)
    {
        foreach (DataControlField column in GridView1.Columns)
        {
            if (header.Contains(column.HeaderText.Trim()))
            { column.Visible = false; }
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
                StoresBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        int CallID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "CallID").ToString());
        ImageButton ImgBtnToCall = (ImageButton)e.Row.FindControl("ImgBtnToCall");
        if (CallID==0)
        {
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.新建报修))
            {
                ImgBtnToCall.Visible = false;
            }
            else
            {
                ImgBtnToCall.ImageUrl = "/images/edit.gif";
            }
        }
        else
        {
            CallInfo cinfo = CallBLL.Get(CallID);
            Literal LtlStatus = (Literal)e.Row.FindControl("LtlStatus");
            if (null == cinfo)
            {
                LtlStatus.Text = "数据被删除";
            }
            else
            {
                LtlStatus.Text = Enum.GetName(typeof(SysEnum.CallStateDetails), cinfo.StateDetail);
            }
        }

    }
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?wd=" + TxbWd.Text.Trim();
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Url += "&DateBegin="+TxbDateBegin.Text;
        Url += "&DateEnd=" + TxbDateEnd.Text;
        Url += "&EnableData=" + DdlEnable.SelectedValue;
        Url += "&IsDeal=" + DdlDeal.SelectedValue;
        Response.Redirect(Url);
    }


    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID <= 0)
        {
            DdlBrand.DataSource = new object();
            DdlBrand.DataBind();
        }
        else
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
            DdlBrand.DataBind();
            DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
        }
    }



    protected void ImgBtnToCall_Click(object sender, ImageClickEventArgs e)
    {
        int CustomerRequestID=Function.ConverToInt(((ImageButton)sender).CommandArgument);
        CustomerRequestInfo info = CustomerRequestBLL.Get(CustomerRequestID);
        if (info.CallID>0)
        {
            if (!info.Enable)
            {
                Function.AlertMsg("请求已失败，无法转成报修"); return;
            }
            CallInfo cinfo = CallBLL.Get(info.CallID);
            if (null!=cinfo)
            {
                Response.Redirect("/page/call/Sch.aspx?CallNo="+cinfo.No);
                return;
            }
            Function.AlertMsg("找不到报修信息，可能已经被删除");
        }
        else
        {
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.新建报修))
            {
                Response.Redirect("edit.aspx?ID=" + CustomerRequestID);
               // Function.AlertMsg("当前请求没有进入报修流程，并且你没有权限新建报修");
                return;
            }
            Response.Redirect("/page/call/add.aspx?CustomerRequestID=" + CustomerRequestID);
        }
    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        string url = "";
        string strWhere = " 1=1 ";
        SchWhere(ref url, ref strWhere);
        List<CustomerRequestInfo> list = CustomerRequestBLL.GetList(strWhere);
        DataTable dt = ListToTable(list);

        string InnerPath = "~/file/download/" + DateTime.Now.ToString("yyyy-MM") + "/";
        string InnerName ="客户请求"+ DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        string FilePath = Server.MapPath(InnerPath);
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        string FileAll = FilePath + InnerName;

        Tool.DocHelper.ExportExcel(dt, FileAll);
        Response.Redirect(InnerPath + InnerName);

    }
    private DataTable ListToTable(List<CustomerRequestInfo> list)
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("系统单号");
        dt.Columns.Add("所属品牌");
        dt.Columns.Add("店铺编号");
        dt.Columns.Add("店铺名称");
        dt.Columns.Add("报修日期");
        dt.Columns.Add("详细");
        dt.Columns.Add("是否受理");
        dt.Columns.Add("处理状态");

        DataRow dr = dt.NewRow();
        CallInfo cinfo;
        StoreInfo sinfo;
        foreach (CustomerRequestInfo item in list)
        {
            cinfo = null;
            if (item.CallID>0)
            {
                cinfo = CallBLL.Get(item.CallID);
            }
            sinfo = StoresBLL.Get(item.StoreID);
            dr = dt.NewRow();
            if (null==cinfo)
            {

                dr["系统单号"] = string.Empty;
                dr["所属品牌"] = sinfo.BrandName;
                dr["店铺编号"] = item.StoreNo;
                dr["店铺名称"] = item.StoreName;
                dr["报修日期"] = item.ErrorReportDate.ToString("yyyy-MM-dd HH:mm");
                dr["详细"] = item.Details;
                dr["是否受理"] = "否";
                dr["处理状态"] = string.Empty;
            }
            else
            {
                dr["系统单号"] = cinfo.No;
                dr["所属品牌"] = sinfo.BrandName;
                dr["店铺编号"] = item.StoreNo;
                dr["店铺名称"] = item.StoreName;
                dr["报修日期"] = item.ErrorReportDate.ToString("yyyy-MM-dd HH:mm");
                dr["详细"] = item.Details;
                dr["是否受理"] = "是";
                dr["处理状态"] = cinfo.StateDetail;
            }
            dt.Rows.Add(dr);
        }

        return dt;
    }


}
