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


public partial class page_Store_list : _BaseData_Store
{
    protected void Page_Load(object sender, EventArgs e)
    {
        P_Manage.Visible = GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.店铺管理);
        if (!IsPostBack)
        {
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));
            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));
            //StoreTypeBind();



            int PageSize = 40;  //每页记录数
            int PageIndex = Function.GetRequestInt("page");  //当前页码
            int page = 10;      //分页显示数
            int count = 0;     //记录总数
            string url = "";
            string strWhere = " 1=1 ";
            SchWhere(ref url, ref strWhere);
            strWhere += " order by ID desc ";
            GridView1.DataSource = StoresBLL.GetList(PageSize, PageIndex, strWhere, out count);
            GridView1.DataBind();
            this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);
            List<string> HideHead = new List<string>();
            if (!GroupBLL.PowerCheck((int)PowerInfo.P3_BaseData.店铺管理))
            {
                HideHead.Add("编辑");
                HideHead.Add("选择");
                GridViewHide(HideHead);
                BtnDelete.Visible = false;
            }
            
        }
    }

    private void SchWhere(ref string url, ref string strWhere)
    {
        SchWd(ref url, ref strWhere);

        SchCustomerBrand(ref url, ref strWhere);

        SchProvinceCity(ref url, ref strWhere);

        SchStoreType(ref url, ref strWhere);

        SchIsClosed(ref url, ref strWhere);
        SchDate(ref url, ref strWhere);
        if (!IsAdmin)
        {
            strWhere += " and f_BrandID in(select f_MID from sys_WorkGroupBrand where f_WorkGroupID=" + CurrentUser.WorkGroupID + ") ";
        }
    }


    private void SchProvinceCity(ref string url, ref string strWhere)
    {
        int ProvinceID = Function.GetRequestInt("ProvinceID");
        if (ProvinceID > 0)
        {
            DdlProvince.SelectedValue = ProvinceID.ToString();
            DdlProvince_SelectedIndexChanged(null, null);
            url += "&ProvinceID=" + ProvinceID;
            strWhere += string.Format(" and f_ProvinceID={0} ", ProvinceID);
            int CityID = Function.GetRequestInt("CityID");
            if (CityID > 0)
            {
                DdlCity.SelectedValue = CityID.ToString();
                strWhere += string.Format(" and f_CityID={0} ", CityID);
                url += "&CityID=" + CityID;
            }
        }
    }

    private void SchDate(ref string url, ref string strWhere)
    {
        DateTime DateBegin = Function.GetRequestDateTime("DateBegin");
        if (DateBegin != Function.ErrorDate)
        {
            strWhere += string.Format(" and DATEDIFF(DAY,f_AddDate,'{0}')<=0", DateBegin);
            url += "&DateBegin=" + DateBegin;
            TxbDateBegin.Text = DateBegin.ToString("yyyy-MM-dd");
        }
        DateTime DateEnd = Function.GetRequestDateTime("DateEnd");
        if (DateEnd != Function.ErrorDate)
        {
            strWhere += string.Format(" and DATEDIFF(DAY,f_AddDate,'{0}')>=0", DateEnd);
            url += "&DateEnd=" + DateEnd;
            TxbDateEnd.Text = DateEnd.ToString("yyyy-MM-dd");
        }
    }


    private void SchCustomerBrand(ref string url, ref string strWhere)
    {
        int CustomerID = Function.GetRequestInt("CustomerID");
        if (CustomerID > 0)
        {
            DdlCustomer.SelectedValue = CustomerID.ToString();
            DdlCustomer_SelectedIndexChanged(null, null);
            url += "&CustomerID=" + CustomerID;
            strWhere += string.Format(" and f_CustomerID={0} ", CustomerID);

            int BrandID = Function.GetRequestInt("BrandID");
            if (BrandID > 0)
            {
                DdlBrand.SelectedValue = BrandID.ToString();
                strWhere += string.Format(" and f_BrandID={0} ", BrandID);
                url += "&BrandID=" + BrandID;
            }
        }
    }

    private void SchWd(ref string url, ref string strWhere)
    {
        string wd = Function.GetRequestSrtring("wd");
        if (!string.IsNullOrEmpty(wd))
        {
            strWhere += " and (";
            strWhere += " f_Name like '%" + Function.ClearText(wd) + "%' ";
            strWhere += " or f_No like '%" + Function.ClearText(wd) + "%' ";
            strWhere += " ) ";
            url += "&wd=" + wd;
            TxbWd.Text = wd;
        }
        string Tel = Function.GetRequestSrtring("Tel");
        if (!string.IsNullOrEmpty(Tel))
        {
            strWhere += " and f_Tel like '%" + Function.ClearText(Tel) + "%' ";
            url += "&Tel=" + Tel;
            TxbTel.Text = Tel;
        }
    }

    private void SchIsClosed(ref string url, ref string strWhere)
    {
        int IsClosed = Function.GetRequestInt("IsClosed");
        if (IsClosed == 0 || IsClosed == 1)
        {
            strWhere += " and f_IsClosed=" + IsClosed;
            url += "&IsClosed=" + IsClosed;
            DdlIsClosed.SelectedValue = IsClosed.ToString();
        }
    }
    /// <summary>
    /// StoreType查询过滤
    /// </summary>
    /// <param name="url"></param>
    /// <param name="strWhere"></param>
    private void SchStoreType(ref string url, ref string strWhere)
    {
        string  storeType = Function.GetRequestSrtring("StoreType");
        if (!string.IsNullOrEmpty(storeType))
        {
            TxbStoreType.Text = storeType;
            url += "&StoreType=" + storeType;
            strWhere += " and f_StoreType like '%" + Function.ClearText(storeType) + "%' ";
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
                StoreInfo info = StoresBLL.Get(Function.ConverToInt(item));
                StoresBLL.Delete(Function.ConverToInt(item));
                //新增删除BKStore表中的信息
                
                if (info.No.StartsWith("BK"))
                {
                    info.No = info.No.Remove(0, 2);
                }
                BKStoreInfo BKStore = BKStoreInfoBLL.GetByStoreNo(info.No);
                if (BKStore != null)
                {
                    BKStoreInfoBLL.Delete(BKStore.ID);
                }
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
    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?wd=" + TxbWd.Text.Trim();
        Url += "&Tel=" + TxbTel.Text.Trim();
        Url += "&ProvinceID=" + DdlProvince.SelectedValue;
        Url += "&CityID=" + DdlCity.SelectedValue;
        Url += "&CustomerID=" + DdlCustomer.SelectedValue;
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Url += "&IsClosed=" + DdlIsClosed.SelectedValue;
        Url += "&DateBegin=" + TxbDateBegin.Text;
        Url += "&DateEnd=" + TxbDateEnd.Text;
        Url += "&StoreType=" + TxbStoreType.Text;
        Response.Redirect(Url);
    }
    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(DdlProvince.SelectedValue);
        DdlCity.Enabled = ID > 0;
        if (ID <= 0)
        {
            DdlCity.DataSource = null;
            DdlCity.DataBind();
        }
        else
        {
            DdlCity.DataSource = CityBLL.GetList(ID);
            DdlCity.DataBind();
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));
        }
    }


    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID <= 0)
        {
            DdlBrand.DataSource = null;
            DdlBrand.DataBind();
        }
        else
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
            DdlBrand.DataBind();
            DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
        }
    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        Response.Write(DateTime.Now + " 开始导出程序<br/>");
        Response.Flush();
        string url = "";
        string strWhere = "";
        SchWhere(ref url, ref strWhere);
        List<StoreInfo> list = StoresBLL.GetList(strWhere);
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

        string InnerPath = "~/file/download/" + DateTime.Now.ToString("yyyy-MM") + "/";
        string InnerName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        string FilePath = Server.MapPath(InnerPath);
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        string FileAll = FilePath + InnerName;

        Tool.DocHelper.ExportExcel(dt, FileAll);
        Response.Write(string.Format("{0} 恭喜你导出完成，<a href='{1}'>请点我下载</a>", DateTime.Now, InnerPath.TrimStart('~') + InnerName));
        Response.Flush();


    }


    private DataTable ListToTable(List<StoreInfo> list)
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("店铺号");
        dt.Columns.Add("店铺名称");
        dt.Columns.Add("对应品牌");
        dt.Columns.Add("对应客户");
        dt.Columns.Add("所属省份");
        dt.Columns.Add("所属城市");
        dt.Columns.Add("地址");
        dt.Columns.Add("电话");
        dt.Columns.Add("邮箱");
        dt.Columns.Add("是否可用");
        dt.Columns.Add("店铺类型");

        DataRow dr = dt.NewRow();

        foreach (StoreInfo item in list)
        {
            dr = dt.NewRow();
            dr["店铺号"] = item.No;
            dr["店铺名称"] = item.Name;
            dr["对应品牌"] = item.BrandName;
            dr["对应客户"] = item.CustomerName;
            dr["所属省份"] = item.ProvinceName;
            dr["所属城市"] = item.CityName;
            dr["地址"] = item.Address;
            dr["电话"] = item.Tel;
            dr["邮箱"] = item.Email;
            dr["是否可用"] = item.IsClosed ? "禁用" : "可用";
            dr["店铺类型"] = item.StoreType;
            dt.Rows.Add(dr);
        }

        return dt;
    }
    /// <summary>
    /// 绑定店铺类型下拉列表
    /// </summary>
    //protected void StoreTypeBind()
    //{
    //    var ss = Enum.GetNames(typeof(SysEnum.StoreType));
    //    foreach (var t in ss)
    //    {
    //        var j = (int)Enum.Parse(typeof(SysEnum.StoreType), t);
    //        DdlStoreType.Items.Add(new ListItem(t, j.ToString()));
    //    }
    //    DdlStoreType.Items.Insert(0, new ListItem("不确定", "0"));
    //}
    
}
