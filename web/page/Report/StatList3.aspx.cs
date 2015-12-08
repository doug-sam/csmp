using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.IO;
using System.Data;
using System.Text;

public partial class page_call_StatList3 : _Report_ReportF
{
   private const string DataGetFinish = "{0} 获取成功完成，总数量量{1}条，现在开始整理数据，整理数据过程有点儿长，请耐心等待。。。<br/>";
   private const string ExportProcess = "{0}: 当前第  {1} 条/共   {2}  条<br/>";
   private const string DownLoadLink = "<a href='{0}' style='font-size:30px;font-weight:bold; color:#F60;text-align:center;margin:20px;display:block;'>请点击我下载</a>";
   private const string ReadyDataProcess = "{0} 当前整理了 {1} 条,共有{2}条数据需要处理<br/>";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TR_State.Visible = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.查询不同状态报修);

            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));
            //DdlBrand.DataSource = BrandBLL.GetListByWorkGroup(CurrentUser.WorkGroupID);
            //DdlBrand.DataBind();
            //DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));

            DdlState.DataSource = SysEnum.ToDictionary(typeof(SysEnum.CallStateMain));
            DdlState.DataBind();
            DdlStateDetail.DataSource = SysEnum.ToDictionary(typeof(SysEnum.CallStateDetails));
            DdlStateDetail.DataBind();
            DdlState.Items.Insert(0, new ListItem("不限", "0"));
            DdlStateDetail.Items.Insert(0, new ListItem("不限", "0"));
            DdlSolvedBy.DataSource = SysEnum.ToDictionary(typeof(SysEnum.SolvedBy));
            DdlSolvedBy.DataBind();
            DdlSolvedBy.Items.Insert(0, new ListItem("不限", "0"));

            DdlCategory.DataSource = CallCategoryBLL.GetListEnable();
            DdlCategory.DataBind();
            DdlCategory.Items.Insert(0, new ListItem("不限", "0"));

            for (int i = DdlState.Items.Count - 1; i >= 0; i--)
            {
                if (DdlState.Items[i].Text == SysEnum.CallStateDetails.处理完成.ToString())
                {
                    DdlState.Items.Remove(DdlState.Items[i]); continue;
                }
                if (DdlState.Items[i].Text == SysEnum.CallStateDetails.已回访.ToString())
                {
                    DdlState.Items.Remove(DdlState.Items[i]); continue;
                }
                if (DdlState.Items[i].Text == SysEnum.CallStateDetails.系统接单_未处理.ToString())
                {
                    DdlState.Items.Remove(DdlState.Items[i]); continue;
                }
            }

            BindData();
            List<string> HideHead = new List<string>();
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.查询不同状态报修))
            {
                HideHead.Add("状态");
            }
            BtnExport.Visible = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.数据导出);

            GridViewHide(HideHead);


        }
    }


    private void BindData()
    {
        int PageSize = 20;  //每页记录数
        int PageIndex = Function.GetRequestInt("page");  //当前页码
        int page = 10;      //分页显示数
        int count = 0;     //记录总数
        string url;
        string strWhere;
        GetSQL(out url, out strWhere);
        GridView1.DataSource = CallBLL.GetList(PageSize, PageIndex, strWhere, out count);
        GridView1.DataBind();
        this.List_Page.Text = Function.Paging3(PageSize, count, page, PageIndex, url);
        if (count > 10000)
        {
            BtnExport.Text = "数据量超过一万条，不可导出";
            BtnExport.Enabled = false;
        }
        else
        {
            BtnExport.Text = "导出搜索结果";
            BtnExport.Enabled = true;
        }
    }

    private void GetSQL(out string url, out string strWhere)
    {
        url = "";
        strWhere = " 1=1 ";
        #region 搜索条件

        SchState(ref url, ref strWhere);

        SchDate(ref url, ref strWhere);

        SchDateDropIn(ref url, ref strWhere);

        SchDateClose(ref url, ref  strWhere);

        SchDateLastRecord(ref url, ref strWhere);

        SchStoreBrand(ref url, ref strWhere);

        SchPrivinceCity(ref url, ref strWhere);

        SchStoreNo_No_SolvedBy(ref url, ref strWhere);

        Sch_SLA(ref url, ref strWhere);

        Sch_Class(ref url, ref strWhere);

        SchCategory(ref url, ref strWhere); 

        #endregion
        if (!IsAdmin)
        {
            strWhere += " AND f_BrandID in(SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=" + CurrentUser.WorkGroupID + ") ";
            //if (GroupBLL.PowerCheck((int)PowerInfo.P1_Call.查看组内所有报修))
            //{
            //    strWhere += " AND f_BrandID in(SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=" + CurrentUser.WorkGroupID + ") ";
            //}
            //else
            //{
            //    strWhere += " AND (f_CreatorID=" + CurrentUserID + " OR f_MaintainUserID=" + CurrentUserID + ")";
            //}
        }


        strWhere += " order by id desc ";
    }



    #region 搜索条件
    private void SchPrivinceCity(ref string url, ref string strWhere)
    {
        int PrivinceID = Function.GetRequestInt("PrivinceID");
        int CityID = Function.GetRequestInt("CityID");
        if (PrivinceID > 0)
        {
            DdlProvince.SelectedValue = PrivinceID.ToString();
            strWhere += string.Format(" and f_ProvinceID ={0} ", PrivinceID);
            url += "&PrivinceID=" + PrivinceID;
            DdlCity.DataSource = CityBLL.GetList(PrivinceID);
            DdlCity.DataBind();
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));

            if (CityID > 0)
            {
                DdlCity.SelectedValue = CityID.ToString();
                strWhere += string.Format(" and f_CityID ={0} ", CityID);
                url += "&CityID=" + CityID;
            }
        }
    }
    private void SchStoreBrand(ref string url, ref string strWhere)
    {
        int CustomerID = Function.GetRequestInt("CustomerID");
        if (CustomerID > 0)
        {
            DdlCustomer.SelectedValue = CustomerID.ToString();
            DdlCustomer_SelectedIndexChanged(null, null);
            strWhere += " AND f_CustomerID=" + CustomerID;
            url += "&CustomerID=" + CustomerID;

            int BrandID = Function.GetRequestInt("BrandID");
            if (BrandID > 0)
            {
                DdlBrand.SelectedValue = BrandID.ToString();
                strWhere += string.Format(" and f_BrandID ={0} ", BrandID);
                url += "&BrandID=" + BrandID;
            }
        }
    }

    private void SchState(ref string url, ref string strWhere)
    {
        if (TR_State.Visible)
        {
            int StateMain = Function.GetRequestInt("State");
            int StateDetail = Function.GetRequestInt("StateDetail");
            if (StateMain == -12)
            {
                strWhere += string.Format(" and f_StateMain in({0},{1}) ", (int)SysEnum.CallStateMain.未处理, (int)SysEnum.CallStateMain.处理中);
                url += "&State=" + StateMain;
            }
            else if (StateMain > 0)
            {
                DdlState.SelectedValue = StateMain.ToString();
                strWhere += string.Format(" and f_StateMain={0} ", StateMain);
                url += "&State=" + StateMain;
            }
            if (StateMain == (int)SysEnum.CallStateMain.处理中)
            {
                if (StateDetail > 0)
                {
                    DdlStateDetail.SelectedValue = StateDetail.ToString();
                    strWhere += " and f_StateDetail=" + StateDetail;
                    url += "&StateDetail=" + StateDetail;
                }
            }
        }
        else
        {
            strWhere += string.Format(" and f_StateMain in({0},{1}) ", (int)SysEnum.CallStateMain.已完成, (int)SysEnum.CallStateMain.已关闭);
            url += "";

        }
    }

    /// <summary>
    /// 时间范围
    /// </summary>
    /// <param name="url"></param>
    /// <param name="strWhere"></param>
    private void SchDate(ref string url, ref string strWhere)
    {
        DateTime DtBegin = Function.GetRequestDateTime("DtBegin");
        DateTime DtEnd = Function.GetRequestDateTime("DtEnd");
        DateTime DtBeginCreate = Function.GetRequestDateTime("DtBeginCreate");
        DateTime DtEndCreate = Function.GetRequestDateTime("DtEndCreate");
        if (DtBegin != Function.ErrorDate)
        {
            TxtDateBegin.Text = DtBegin.ToString("yyyy-MM-dd");
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_ErrorDate)>=0 ", DtBegin);
            url += "&DtBegin=" + TxtDateBegin.Text;
        }
        if (DtEnd != Function.ErrorDate)
        {
            TxbDateEnd.Text = DtEnd.ToString("yyyy-MM-dd");
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_ErrorDate)<=0 ", DtEnd);
            url += "&DtEnd=" + TxbDateEnd.Text;
        }

        if (DtBeginCreate != Function.ErrorDate)
        {
            TxbDateBeginCreate.Text = DtBeginCreate.ToString("yyyy-MM-dd");
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_CreateDate)>=0 ", DtBeginCreate);
            url += "&DtBeginCreate=" + TxbDateBeginCreate.Text;
        }
        if (DtEndCreate != Function.ErrorDate)
        {
            TxbDateEndCreate.Text = DtEndCreate.ToString("yyyy-MM-dd");
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_CreateDate)<=0 ", DtEndCreate);
            url += "&DtEndCreate=" + TxbDateEndCreate.Text;
        }
    }

    /// <summary>
    /// 时间范围
    /// </summary>
    /// <param name="url"></param>
    /// <param name="strWhere"></param>
    private void SchDateDropIn(ref string url, ref string strWhere)
    {
        DateTime DtBegin = Function.GetRequestDateTime("DtDropInBegin");
        DateTime DtEnd = Function.GetRequestDateTime("DtDropInEnd");
        if (DtBegin > DtEnd)
        {
            Function.AlertBack("预约上门的开始日期必需小于结束日期");
        }
        if (DtBegin != Function.ErrorDate || DtEnd != Function.ErrorDate)
        {
            strWhere += " AND id in( ";
            strWhere += string.Format("select f_CallID FROM sys_CallStep WHERE f_StepType in({0},{1},{2})",
                (int)SysEnum.CallStateDetails.等待备件,
                (int)SysEnum.CallStateDetails.等待工程师上门,
                (int)SysEnum.CallStateDetails.等待第三方响应
                );
            if (DtBegin != Function.ErrorDate)
            {
                TxbDropInDateBegin.Text = DtBegin.ToString("yyyy-MM-dd");
                strWhere += string.Format(" and DATEDIFF(day,'{0}',f_DateBegin)>=0 ", DtBegin);
                url += "&DtDropInBegin=" + TxtDateBegin.Text;
            }
            if (DtEnd != Function.ErrorDate)
            {
                TxbDropInDateEnd.Text = DtEnd.ToString("yyyy-MM-dd");
                strWhere += string.Format(" and DATEDIFF(day,'{0}',f_DateBegin)<=0 ", DtEnd);
                url += "&DtDropInEnd=" + TxbDateEnd.Text;
            }
            strWhere += "           ) ";
        }

    }

    /// <summary>
    /// 时间范围
    /// </summary>
    /// <param name="url"></param>
    /// <param name="strWhere"></param>
    private void SchDateClose(ref string url, ref string strWhere)
    {
        DateTime DtBegin = Function.GetRequestDateTime("DtCloseBegin");
        DateTime DtEnd = Function.GetRequestDateTime("DtCloseEnd");
        if (DtBegin != Function.ErrorDate)
        {
            TxbDateCloseBegin.Text = DtBegin.ToString("yyyy-MM-dd");
            strWhere += "  and  ID in(  ";
            strWhere += " select f_CallID from sys_CallStep where ";
            strWhere += " f_StepType=" + (int)SysEnum.StepType.关单;
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_DateBegin)>=0 ", DtBegin);
            url += "&DtCloseBegin=" + TxbDateCloseBegin.Text;

            if (DtEnd != Function.ErrorDate)
            {
                TxbDateCloseEnd.Text = DtEnd.ToString("yyyy-MM-dd");
                strWhere += string.Format(" and DATEDIFF(day,'{0}',f_DateBegin)<=0 ", DtEnd);
                url += "&DtCloseEnd=" + TxbDateCloseEnd.Text;
            }
            strWhere += " )";
        }

    }

    /// <summary>
    /// 二线最终记录 时间范围
    /// </summary>
    /// <param name="url"></param>
    /// <param name="strWhere"></param>
    private void SchDateLastRecord(ref string url, ref string strWhere)
    {
        DateTime DtBegin = Function.GetRequestDateTime("DtLastRecordBegin");
        DateTime DtEnd = Function.GetRequestDateTime("DtLastRecordEnd");
        if (DtBegin != Function.ErrorDate)
        {
            TxbLastRecordDateBegin.Text = DtBegin.ToString("yyyy-MM-dd");
            strWhere += "  and  ID in(  ";
            strWhere += " select f_CallID from sys_CallStep where ";
            strWhere += " f_IsSolved=1 ";
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_AddDate)>=0 ", DtBegin);
            url += "&DtLastRecordBegin=" + TxbLastRecordDateBegin.Text;

            if (DtEnd != Function.ErrorDate)
            {
                TxbLastRecordDateEnd.Text = DtEnd.ToString("yyyy-MM-dd");
                strWhere += string.Format(" and DATEDIFF(day,'{0}',f_AddDate)<=0 ", DtEnd);
                url += "&DtLastRecordEnd=" + TxbLastRecordDateEnd.Text;
            }
            strWhere += " )";
        }

    }

    /// <summary>
    /// 时间范围
    /// </summary>
    /// <param name="url"></param>
    /// <param name="strWhere"></param>
    private void SchDateReciveBill(ref string url, ref string strWhere)
    {
        DateTime DtBegin = Function.GetRequestDateTime("DtReciveBillDateBegin");
        DateTime DtEnd = Function.GetRequestDateTime("DtReciveBillDateEnd");
        if (DtBegin != Function.ErrorDate)
        {
            TxbDateCloseBegin.Text = DtBegin.ToString("yyyy-MM-dd");
            strWhere += "  and  ID in(  ";
            strWhere += " select f_CallID from sys_CallStep where ";
            strWhere += " f_StepType=" + (int)SysEnum.StepType.回收服务单;
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_DateBegin)>=0 ", DtBegin);
            url += "&DtReciveBillDateBegin=" + TxbDateCloseBegin.Text;

            if (DtEnd != Function.ErrorDate)
            {
                TxbDateCloseEnd.Text = DtEnd.ToString("yyyy-MM-dd");
                strWhere += string.Format(" and DATEDIFF(day,'{0}',f_DateBegin)<=0 ", DtEnd);
                url += "&DtReciveBillDateEnd=" + TxbDateCloseEnd.Text;
            }
            strWhere += " )";
        }

    }



    private void SchStoreNo_No_SolvedBy(ref string url, ref string strWhere)
    {
        string StoreNo = Function.ClearText(Function.GetRequestSrtring("StoreNo"));
        if (!string.IsNullOrEmpty(StoreNo))
        {
            strWhere += string.Format(" and f_StoreID in( select ID from sys_Stores where sys_Stores.f_Name like '%{0}%' or sys_Stores.f_No like '%{0}%') ", StoreNo);
            url += "&StoreNo=" + StoreNo;
            TxbStoreNo.Text = StoreNo;
        }
        string CallNo = Function.ClearText(Function.GetRequestSrtring("CallNo"));
        if (!string.IsNullOrEmpty(CallNo))
        {
            strWhere += string.Format(" AND F_No LIKE '%{0}%' ", CallNo);
            url += "&CallNo=" + CallNo;
            TxbCallNo.Text = CallNo;
        }
        int SolvedBy = Function.GetRequestInt("SolvedBy");
        if (!string.IsNullOrEmpty(Enum.GetName(typeof(SysEnum.SolvedBy), SolvedBy)))
        {
            strWhere += string.Format(" and f_SloveBy='{0}' ", Enum.GetName(typeof(SysEnum.SolvedBy), SolvedBy));
            url += "&SolvedBy=" + SolvedBy;
            DdlSolvedBy.SelectedValue = SolvedBy.ToString();
        }
    }

    private void Sch_SLA(ref string url, ref string strWhere)
    {
        int SLA = Function.GetRequestInt("SLA");
        if (SLA == 1 || SLA == 2)
        {
            strWhere += string.Format(" AND DATEDIFF(HOUR,f_ErrorDate,Getdate()){0}f_SLA ", SLA == 1 ? "<" : ">");
            DdlSla.SelectedValue = SLA.ToString();
            url += "&SLA=" + SLA;
        }

    }

    private void Sch_Class(ref string url, ref string strWhere)
    {
        int Class1ID = Function.GetRequestInt("Class1ID");
        if (Class1ID > 0)
        {
            strWhere += string.Format(" AND f_Class1={0} ", Class1ID);
            url += "&Class1ID=" + Class1ID;
        }

    }

    private void SchCategory(ref string url, ref string strWhere)
    {
        int Category = Function.GetRequestInt("Category");
        if (Category <= 0)
        {
            return;
        }
        strWhere += " and f_Category=" + Category;
        url += "&Category=" + Category;

    }


    #endregion

    private void GridViewHide(List<string> header)
    {
        foreach (DataControlField column in GridView1.Columns)
        {
            if (header.Contains(column.HeaderText.Trim()))
            { column.Visible = false; }
        }
    }



    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "StatList3.aspx";
        Url += "?State=" + DdlState.SelectedValue;
        Url += "&StateDetail=" + DdlStateDetail.SelectedValue;
        Url += "&DtBegin=" + TxtDateBegin.Text.Trim();
        Url += "&DtEnd=" + TxbDateEnd.Text.Trim();
        Url += "&PrivinceID=" + DdlProvince.SelectedValue;
        Url += "&CityID=" + DdlCity.SelectedValue;
        Url += "&StoreNo=" + TxbStoreNo.Text.Trim();
        Url += "&CustomerID=" + DdlCustomer.SelectedValue;
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Url += "&CallNo=" + TxbCallNo.Text.Trim();
        Url += "&SolvedBy=" + DdlSolvedBy.SelectedValue;
        Url += "&SLA=" + DdlSla.SelectedValue;
        Url += "&DtBeginCreate=" + TxbDateBeginCreate.Text;
        Url += "&DtEndCreate=" + TxbDateEndCreate.Text;
        Url += "&DtDropInBegin=" + TxbDropInDateBegin.Text;
        Url += "&DtDropInEnd=" + TxbDropInDateEnd.Text;
        Url += "&DtCloseBegin=" + TxbDateCloseBegin.Text;
        Url += "&DtCloseEnd=" + TxbDateCloseEnd.Text;
        Url += "&DtReciveBillDateBegin=" + TxbReciveBillDateBegin.Text;
        Url += "&DtReciveBillDateEnd=" + TxbReciveBillDateEnd.Text;
        Url += "&DtLastRecordBegin=" + TxbLastRecordDateBegin.Text;
        Url += "&DtLastRecordEnd=" + TxbLastRecordDateEnd.Text;
        Url += "&Category=" + DdlCategory.SelectedValue;
        Response.Redirect(Url);
    }


    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlProvince.SelectedValue);
        DdlCity.DataSource = (ID > 0) ? CityBLL.GetList(ID) : null;
        DdlCity.DataBind();
        DdlCity.Items.Insert(0, new ListItem("不限", "0"));

    }
    protected void DdlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlState.SelectedValue);
        DdlStateDetail.Visible = (ID == (int)SysEnum.CallStateMain.处理中);
    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        Response.Write("<div style=' border:1px solid #CCC;padding:3px;'>");
        Response.Write("建议不要用IE(IE 10除外)来导出数据，太卡了。会让整个浏览器假死。建议使用浏览器chrome/firefox/opear/ie 10。国产浏览器 qq 360 遨游等等同用于你电脑的IE！！<br/>");
        Response.Write("扫盲：<a href='ReadMe.docx' target='_blank'>怎样将导出的文件转换成Excel</a></div>");

        Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在获取数据，请等待。。。<br/>");
        Response.Flush();
        string url;
        string strWhere;
        GetSQL(out url, out strWhere);
        List<CallInfo> list = CallBLL.GetList(strWhere);
        if (list.Count == 0)
        {
            Function.AlertMsg("没有符合条件的数据"); return;
        }
        Response.Write(string.Format(DataGetFinish,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),list.Count));
        Response.Flush();

        DataTable dt = ListToTable(list);
        Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 整理数据成功完成，现在开始灌输在Excel或Access中。。。<br/>");
        Response.Flush();

        string InnerPath = "~/file/download/" + DateTime.Now.ToString("yyyy-MM") + "/";
        string InnerName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".accdb";
        string FilePath = Server.MapPath(InnerPath);
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        string FileAll = FilePath + InnerName;
        Response.Clear();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (!StatBLL.ExportAccess(dt, i, FilePath + InnerName))
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", "alert(‘出错了，请联系管理员’);", true);
                break;
            }
            if (i % 20 == 0)
            {
                Response.Write(string.Format(ExportProcess,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),i,list.Count));
                Response.Flush();
            }
        }
        Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + list.Count + "条数据全部灌输完成，全部进度已结果。<br/>");
        string DownloadURL = InnerPath.Substring(1) + InnerName;
        Response.Write(string.Format(DownLoadLink, DownloadURL));

        Response.Flush();
        Response.End();
    }

    private DataTable ListToTable(List<CallInfo> list)
    {
        DateTime NowTime = DateTime.Now;

        bool NeedStep = RblStep.SelectedValue == "1";

        DataTable dt = GetNewDataTable(NeedStep);
        DataRow dr = dt.NewRow();
        StoreInfo info;
        List<AssignInfo> listAssign;
        List<CallStepInfo> listStep;

        int forFlag = 0;
        foreach (CallInfo item in list)
        {
            forFlag++;
            if (forFlag % 20 == 0)
            {
                Response.Write(string.Format(ReadyDataProcess, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), forFlag,list.Count));
                Response.Flush();
            }
            if (null == item)
            {
                throw new Exception(item.No);
            }

            dr = dt.NewRow();
            info = StoresBLL.Get(item.StoreID);
            #region 列赋值
            dr["系统单号"] = item.No;
            dr["服务类型"] =CallCategoryBLL.Get(item.Category).Name;
            dr["下单一线"] = item.CreatorName;
            dr["创建时间"] = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            dr["报修人"] = item.ErrorReportUser;
            dr["客户"] = item.CustomerName;
            dr["品牌"] = item.BrandName;
            dr["省份"] = item.ProvinceName;
            dr["城市"] = item.CityName;
            dr["店铺号"] = null == info ? "" : info.No.Trim();
            dr["店铺名"] = null == info ? "" : info.Name.Trim();

            dr["报修源"] = item.ReportSourceName;
            dr["源单号"] = item.ReportSourceNo;
            dr["大类故障"] = item.ClassName1;
            dr["中类故障"] = item.ClassName2;
            dr["小类故障"] = item.ClassName3;
            dr["优先级"] = item.PriorityName;
            dr["报修详细"] = item.Details;
            dr["二线负责人"] = item.MaintaimUserName;
            dr["报修时间"] = item.ErrorDate.ToString("yyyy-MM-dd HH:mm"); ;
            dr["状态"] = Enum.GetName(typeof(SysEnum.CallStateMain), item.StateMain);
            dr["解决方案"] = item.SloveBy;
            dr["接单时间"] = "";
            dr["响应时间1（创建时间-报修时间）"] = Math.Round((item.CreateDate - item.ErrorDate).TotalHours, 2);
            dr["响应时间2（接单时间-创建时间）"] = "";
            dr["响应时间3（即接单时间-报修时间）"] = "";

            dr["上门次数"] = 0;
            dr["上门响应时间"] = "";

            dr["SLA"] = item.SLA;
            dr["SLA扩展"] = item.SLA2;
            dr["超时时间"] = item.SLADateEnd.ToString("yyyy-MM-dd HH:mm");
            dr["是否已超时"] = "false";
            if ((item.StateMain == (int)SysEnum.CallStateMain.处理中 || item.StateMain == (int)SysEnum.CallStateMain.未处理))
            {
                if (item.SLADateEnd != DicInfo.DateZone && DateTime.Now > item.SLADateEnd)
                {
                    dr["是否已超时"] = "true";
                }
            }
            else
            {
                if (item.FinishDate>item.SLADateEnd)
                {
                    dr["是否已超时"] = "true";
                }
            }

            #endregion
            if (item.FinishDate == Function.ErrorDate)
            {
                dr["用时"] = "";
            }
            else
            {
                double taketime = Math.Round((item.FinishDate - item.ErrorDate).TotalHours, 2);
                dr["用时"] = taketime > 0 ? taketime : 0;
            }
            listAssign = AssignBLL.GetList(" f_CallID=" + item.ID + " AND f_AssignType=0 order by id asc ");
            //listAssign = AssignBLL.GetList(item.ID);
            if (null == listAssign || listAssign.Count == 0)
            {
                dr["第一次接单的二线人员"] = item.MaintaimUserName;
                dr["是否转派其他二线"] = "否";
            }
            else
            {
                dr["第一次接单的二线人员"] = listAssign[0].OldName;
                dr["是否转派其他二线"] = "是";
            }

            dr["接单时间"] = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            listStep = CallStepBLL.GetList(item.ID);
            if (null != listStep && listStep.Count > 0)
            {
                if (listStep[0].StepType == (int)(SysEnum.StepType.开单))
                {
                    if (listStep.Count > 1)
                    {
                        dr["接单时间"] = listStep[1].DateBegin.ToString("yyyy-MM-dd HH:mm");
                        dr["响应时间2（接单时间-创建时间）"] = Math.Round((listStep[1].DateBegin - item.CreateDate).TotalHours, 2);
                        dr["响应时间3（即接单时间-报修时间）"] = Math.Round((listStep[1].DateBegin - item.ErrorDate).TotalHours, 2);
                        int DropInCurrCallCount = 0;    //当前的报修上门次数
                        foreach (CallStepInfo ItemStep in listStep)
                        {
                            if (ItemStep.StepType == (int)SysEnum.StepType.上门安排)
                            {
                                DropInCurrCallCount++;
                                dr["第" + DropInCurrCallCount + "次预约上门时间"] = ItemStep.DateBegin.ToString("yyyy-MM-dd HH:mm"); ;

                            }
                            if (ItemStep.StepType == (int)SysEnum.StepType.到达门店处理)
                            {

                                dr["上门次数"] = DropInCurrCallCount;
                                if (string.IsNullOrEmpty(dr["上门响应时间"].ToString()))
                                {
                                    dr["上门响应时间"] = Math.Round((ItemStep.DateBegin - item.ErrorDate).TotalHours, 2);
                                }
                                UserInfo DropInUser = UserBLL.Get(ItemStep.MajorUserID);

                                dr["第" + DropInCurrCallCount + "次上门工程师"] = ItemStep.MajorUserName;
                                dr["第" + DropInCurrCallCount + "次上门工程师所在工作组"] = WorkGroupBLL.GetWorkGroupName(DropInUser.WorkGroupID);
                                dr["第" + DropInCurrCallCount + "次到达门店时间"] = ItemStep.DateBegin.ToString("yyyy-MM-dd HH:mm");
                            }

                            if (ItemStep.StepType == (int)SysEnum.StepType.上门详细)
                            {
                                if (ItemStep.DateBegin == Function.ErrorDate)
                                {
                                    dr["第" + DropInCurrCallCount + "次离开门店时间"] = string.Empty;
                                }
                                else
                                {
                                    dr["第" + DropInCurrCallCount + "次离开门店时间"] = ItemStep.DateEnd.ToString("yyyy-MM-dd HH:mm");
                                }
                            }

                            if (ItemStep.IsSolved)
                            {
                                dr["二线最终的处理时间"] = ItemStep.DateEnd.ToString("yyyy-MM-dd HH:mm");
                                dr["二线最终的记录时间"] = ItemStep.AddDate.ToString("yyyy-MM-dd HH:mm");
                                dr["二线延时关CALL时间"] = Math.Round((ItemStep.AddDate - ItemStep.DateEnd).TotalHours, 2);
                                dr["二线最终的解决描述"] = ItemStep.Details;

                            }
                            if (ItemStep.StepType == (int)SysEnum.StepType.关单)
                            {
                                dr["一线的关单人"] = ItemStep.MajorUserName;
                                dr["关单时间"] = ItemStep.AddDate.ToString("yyyy-MM-dd HH:mm"); ;
                            }
                            if (ItemStep.StepType == (int)SysEnum.StepType.上门安排)
                            {
                                dr["jobCode"] += ItemStep.Details + "+";
                                dr["jobCode总额"] = Function.ConverToInt(dr["jobCode总额"], 0) + GetJobCodeValue(ItemStep.Details, item.WorkGroupID);
                            }
                        }
                    }
                    else {
                        dr["二线最终的记录时间"] = "";
                        dr["二线最终的解决描述"] = "";
                        dr["一线的关单人"] = "";
                        dr["关单时间"] = "";
                        dr["jobCode"] = "";
                    }

                }else{
                    dr["接单时间"] = listStep[0].DateBegin.ToString("yyyy-MM-dd HH:mm");
                    dr["响应时间2（接单时间-创建时间）"] = Math.Round((listStep[0].DateBegin - item.CreateDate).TotalHours, 2);
                    dr["响应时间3（即接单时间-报修时间）"] = Math.Round((listStep[0].DateBegin - item.ErrorDate).TotalHours, 2);
                    int DropInCurrCallCount = 0;    //当前的报修上门次数
                    foreach (CallStepInfo ItemStep in listStep)
                    {
                        if (ItemStep.StepType == (int)SysEnum.StepType.上门安排)
                        {
                            DropInCurrCallCount++;
                            dr["第" + DropInCurrCallCount + "次预约上门时间"] = ItemStep.DateBegin.ToString("yyyy-MM-dd HH:mm"); ;

                        }
                        if (ItemStep.StepType == (int)SysEnum.StepType.到达门店处理)
                        {

                            dr["上门次数"] = DropInCurrCallCount;
                            if (string.IsNullOrEmpty(dr["上门响应时间"].ToString()))
                            {
                                dr["上门响应时间"] = Math.Round((ItemStep.DateBegin - item.ErrorDate).TotalHours, 2);
                            }
                            UserInfo DropInUser = UserBLL.Get(ItemStep.MajorUserID);

                            dr["第" + DropInCurrCallCount + "次上门工程师"] = ItemStep.MajorUserName;
                            dr["第" + DropInCurrCallCount + "次上门工程师所在工作组"] = WorkGroupBLL.GetWorkGroupName(DropInUser.WorkGroupID);
                            dr["第" + DropInCurrCallCount + "次到达门店时间"] = ItemStep.DateBegin.ToString("yyyy-MM-dd HH:mm");
                        }

                        if (ItemStep.StepType == (int)SysEnum.StepType.上门详细)
                        {
                            if (ItemStep.DateBegin == Function.ErrorDate)
                            {
                                dr["第" + DropInCurrCallCount + "次离开门店时间"] = string.Empty;
                            }
                            else
                            {
                                dr["第" + DropInCurrCallCount + "次离开门店时间"] = ItemStep.DateEnd.ToString("yyyy-MM-dd HH:mm");
                            }
                        }

                        if (ItemStep.IsSolved)
                        {
                            dr["二线最终的处理时间"] = ItemStep.DateEnd.ToString("yyyy-MM-dd HH:mm");
                            dr["二线最终的记录时间"] = ItemStep.AddDate.ToString("yyyy-MM-dd HH:mm");
                            dr["二线延时关CALL时间"] = Math.Round((ItemStep.AddDate - ItemStep.DateEnd).TotalHours, 2);
                            dr["二线最终的解决描述"] = ItemStep.Details;

                        }
                        if (ItemStep.StepType == (int)SysEnum.StepType.关单)
                        {
                            dr["一线的关单人"] = ItemStep.MajorUserName;
                            dr["关单时间"] = ItemStep.AddDate.ToString("yyyy-MM-dd HH:mm"); ;
                        }
                        if (ItemStep.StepType == (int)SysEnum.StepType.上门安排)
                        {
                            dr["jobCode"] += ItemStep.Details + "+";
                            dr["jobCode总额"] = Function.ConverToInt(dr["jobCode总额"], 0) + GetJobCodeValue(ItemStep.Details, item.WorkGroupID);
                        }
                    }
                }

                
            }
            else
            {
                dr["二线最终的记录时间"] = "";
                dr["二线最终的解决描述"] = "";
                dr["一线的关单人"] = "";
                dr["关单时间"] = "";
                dr["jobCode"] = "";
            }
            dr["jobCode"] = dr["jobCode"].ToString().Trim('+');

            List<CallStepInfo> listStepConbime = Combine(item, listStep);
            if (NeedStep)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CallStepInfo Stepitem in listStepConbime)
                {
                    sb.Append(Stepitem.StepIndex);
                    sb.Append("、(").Append(Stepitem.AddDate).Append(")");
                    sb.Append(Stepitem.StepName.Trim());
                    sb.Append("，操作备注：").Append(Stepitem.Details);
                    sb.Append("\r\n");
                }
                dr["处理步骤"] = sb;
                
            }
            //判断是否有录音
            if (listStepConbime.Count > 0)
            {
                string step1Details = string.Empty;
                if (listStep[0].StepType == (int)(SysEnum.StepType.开单))
                {
                    if (listStep.Count > 1)
                    {
                        step1Details = listStepConbime[1].Details;
                        if (GetRecordIDFromDetails(step1Details))
                        {
                            dr["录音"] = "有";
                        }
                        else
                        {
                            dr["录音"] = "无";
                        }
                    }
                    else {
                        dr["录音"] = "无";
                    }
                }
                else {
                    step1Details = listStepConbime[0].Details;
                    if (GetRecordIDFromDetails(step1Details))
                    {
                        dr["录音"] = "有";
                    }
                    else
                    {
                        dr["录音"] = "无";
                    }
                }
               
            }
            else
            {
                dr["录音"] = "无";
            }

            dt.Rows.Add(dr);
        }
        return dt;
    }

    private static DataTable GetNewDataTable(bool NeedStep)
    {
        DataTable dt = new DataTable();
        #region 添加列
        dt.Columns.Add("系统单号");
        dt.Columns.Add("服务类型");
        dt.Columns.Add("下单一线");
        dt.Columns.Add("创建时间");
        dt.Columns.Add("报修人");
        dt.Columns.Add("客户");
        dt.Columns.Add("品牌");
        dt.Columns.Add("省份");
        dt.Columns.Add("城市");
        dt.Columns.Add("店铺号");
        dt.Columns.Add("店铺名");
        dt.Columns.Add("报修源");
        dt.Columns.Add("源单号");
        dt.Columns.Add("大类故障");
        dt.Columns.Add("中类故障");
        dt.Columns.Add("小类故障");
        dt.Columns.Add("优先级");
        dt.Columns.Add("报修详细");
        dt.Columns.Add("二线负责人");
        dt.Columns.Add("报修时间");
        dt.Columns.Add("状态");
        dt.Columns.Add("解决方案");
        dt.Columns.Add("响应时间1（创建时间-报修时间）");
        dt.Columns.Add("响应时间2（接单时间-创建时间）");
        dt.Columns.Add("响应时间3（即接单时间-报修时间）");

        dt.Columns.Add("第一次接单的二线人员");
        dt.Columns.Add("接单时间");
        dt.Columns.Add("二线最终的处理时间");
        dt.Columns.Add("二线最终的记录时间");
        dt.Columns.Add("二线延时关CALL时间");
        dt.Columns.Add("二线最终的解决描述");
        dt.Columns.Add("一线的关单人");

        dt.Columns.Add("关单时间");
        dt.Columns.Add("SLA");
        dt.Columns.Add("SLA扩展");
        dt.Columns.Add("超时时间");
        dt.Columns.Add("是否已超时");

        dt.Columns.Add("用时");
        dt.Columns.Add("jobCode");
        dt.Columns.Add("jobCode总额");
        dt.Columns.Add("是否转派其他二线");
        dt.Columns.Add("上门次数");
        dt.Columns.Add("上门响应时间");

        #endregion
        if (NeedStep)
        {
            dt.Columns.Add("处理步骤");
        }
        dt.Columns.Add("录音");
        //dt.Columns.Add("stepNo");
        for (int i = 1; i <= 15; i++)
        {
            dt.Columns.Add("第" + i + "次上门工程师");
            dt.Columns.Add("第" + i + "次上门工程师所在工作组");
            dt.Columns.Add("第" + i + "次预约上门时间");
            dt.Columns.Add("第" + i + "次到达门店时间");
            dt.Columns.Add("第" + i + "次离开门店时间");
        }

        return dt;
    }


    /// <summary>
    /// 转派换成操作步骤
    /// </summary>
    /// <param name="ListAss"></param>
    /// <returns></returns>
    private List<CallStepInfo> DropInMemoToStep(List<DropInMemoInfo> ListMemo, int CallID)
    {
        List<CallStepInfo> list = new List<CallStepInfo>();
        if (null == ListMemo) return null;
        CallStepInfo info;
        foreach (DropInMemoInfo item in ListMemo)
        {
            info = new CallStepInfo();
            info.AddDate = item.AddDate;
            info.CallID = CallID;
            info.DateBegin = info.DateEnd = item.MemoDate;
            info.Details = item.Details;
            info.IsSolved = false;
            info.MajorUserID = item.UserID;
            info.MajorUserName = item.UserName;
            info.SolutionID = 0;
            info.SolutionName = string.Empty;
            info.StepIndex = 0;
            info.StepName = "上门备注";
            info.StepType = 0;
            info.UserID = item.UserID;
            info.UserName = item.UserName;
            list.Add(info);

        }

        return list;
    }

    private List<CallStepInfo> Combine(CallInfo info, List<CallStepInfo> listStep)
    {
        //List<CallStepInfo> liststep = CallStepBLL.GetList(info.ID);
        //List<CallStepInfo> listass = AssignToStep(AssignBLL.GetList(info.ID));
        List<CallStepInfo> listdrop = DropInMemoToStep(DropInMemoBLL.GetListOrderByID(info.ID), info.ID);
        //liststep.AddRange(listass);
        listStep.AddRange(listdrop);
        return listStep.OrderBy(CallStepInfo => CallStepInfo.AddDate).ToList();
    }



    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerChange();
    }

    private void CustomerChange()
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            DdlBrand.DataSource = BrandBLL.GetList(ID);
            DdlBrand.DataBind();
        }
        else
        {
            DdlBrand.DataSource = null;//TODO::把它改正确过来
            DdlBrand.DataBind();
        }
        DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
    }


    /// <summary>
    /// 根据用户输入的jobcode返回其金额
    /// </summary>
    /// <param name="JobCodeInType">用户输入的JobCode</param>
    /// <returns>如果直接填金额则返回其数字类型，否则查JobCode表，没有合适记录则返回0</returns>
    private decimal GetJobCodeValue(string JobCodeInType, int WorkGroupID)
    {
        decimal JobcodeValue = Function.ConverToDecimal(JobCodeInType);
        if (JobcodeValue > 0)
        {
            return JobcodeValue;
        }
        JobcodeInfo jobcodeInfo = JobcodeBLL.Get(JobCodeInType, WorkGroupID);
        if (null != jobcodeInfo)
        {
            return jobcodeInfo.Money;
        }
        return 0;
    }
    /// <summary>
    /// 判断stepindex 1是否有录音
    /// </summary>
    /// <param name="Details"></param>
    /// <returns></returns>
    protected bool GetRecordIDFromDetails(string Details)
    {
        string recordid = "";
        int POS1 = Details.IndexOf("A$B$C");
        int POS2 = Details.IndexOf("D$E$F");
        if (POS1 != -1 && POS2 != -1 && POS2 > POS1)
            recordid = Details.Substring(POS1 + 5, POS2 - POS1 - 5);
        if (string.IsNullOrEmpty(recordid))
            return false;
        else
            return true;
    }

}
