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

public partial class page_call_StatList : _Report_ReportF
{
    // private static ListItem DefItem = new ListItem("不限", "0");
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write("为缓解服务器压力，导出功能移到此地址中：<br/>");
        //string Href=string.Empty;
        //if (Request.Url.Host.ToString()=="10.0.32.240")
        //{
        //    Href="http://10.0.32.239:8888/page/report/statlist.aspx";
        //}
        //else
        //{
        //    Href="http://"+Request.Url.Host.ToString() + ":8888/page/report/statlist.aspx";
        //}
        //Response.Write(Href+"<br/>如果此页面窗口没有打开，请复制该地址，贴上地址栏上打开。");
        
        //Response.Write("<script>window.open('" + Href + "', '_black');</script>");
        //Response.End();
        //return;
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
        SchStr_UrlStr(out url, out strWhere);
        GridView1.DataSource = CallBLL.GetList(PageSize, PageIndex, strWhere, out count);
        GridView1.DataBind();
        this.List_Page.Text = Function.Paging3(PageSize, count, page, PageIndex, url);
    }

    private void SchStr_UrlStr(out string url, out string strWhere)
    {
        url = "";
        strWhere = " 1=1 ";
        #region 搜索条件

        SchState(ref url, ref strWhere);

        SchDate(ref url, ref strWhere);


        SchStoreBrand(ref url, ref strWhere);

        SchPrivinceCity(ref url, ref strWhere);

        SchStoreNo_No_SolvedBy(ref url, ref strWhere);

        Sch_SLA(ref url, ref strWhere);
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
            if (StateMain > 0)
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
        if (DtBegin > DtEnd)
        {
            Function.AlertBack("报修的开始日期必需小于结束日期");
        }
        if (DtBeginCreate>DtEndCreate)
        {
            Function.AlertBack("创建单时的开始日期必需小于结束日期");
        }
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
        string Url = "StatList.aspx";
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
        LogInfo log = new LogInfo();
        log.AddDate = DateTime.Now;
        log.Category = SysEnum.LogType.导出数据.ToString();
        log.ErrorDate = DateTime.Now;
        log.SendEmail = false;
        log.Serious = 0;
        log.UserName = CurrentUser.Name;
        log.Content = "用户 " + CurrentUser.Name + " 在 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " 开始导出数据。<br/>";
        log.ID= LogBLL.Add(log);
        string url;
        string strWhere;
        SchStr_UrlStr(out url, out strWhere);
        List<CallInfo> list = CallBLL.GetList(strWhere);
        if (list.Count == 0)
        {
            Function.AlertMsg("没有符合条件的数据"); return;
        }
        if (list.Count>10000)
        {
            Function.AlertMsg("数据量"+list.Count+"多于10000条。请分次导出");
            log.Content += "数据量”"+list.Count+"“过多，服务器拒绝请求。<br/>";
            LogBLL.Edit(log);
            return;

        }
        log.Content += "于 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " 数据获取完成。数据量共有" + list.Count + " 条。<br/>";
        LogBLL.Edit(log);
        //DataTable dt = ModelHelper.ToDataTable(list);
        DataTable dt = ListToTable(list);

        string InnerPath = "~/file/download/" + DateTime.Now.ToString("yyyy-MM") + "/";
        string InnerName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        string FilePath = Server.MapPath(InnerPath);
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        string FileAll = FilePath + InnerName;
        Tool.DocHelper.ExportExcel(dt, FileAll);
        log.Content += "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " 完成导出。导出数据备份在:系统地址+" + InnerPath.Trim('~') + InnerName;
        LogBLL.Add(log);
        Response.Write(string.Format("<a href='{0}'>点击下载{0}</a>",InnerPath.Trim('~') + InnerName));
        Response.End();
    }

    private DataTable ListToTable(List<CallInfo> list)
    {
        bool NeedStep = RblStep.SelectedValue == "1";

        DataTable dt = new DataTable();
        dt.Columns.Add("系统单号");
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
        dt.Columns.Add("响应时间1(创建时间-报修时间)");
        dt.Columns.Add("响应时间2(接单时间-创建时间)");
        dt.Columns.Add("响应时间3(即 接单时间-报修时间)");

        dt.Columns.Add("第一次接单的二线人员");
        dt.Columns.Add("接单时间");
        dt.Columns.Add("二线最终的处理时间");
        dt.Columns.Add("二线最终的记录时间");
        dt.Columns.Add("二线延时关CALL时间");
        dt.Columns.Add("二线最终的解决描述");
        dt.Columns.Add("一线的关单人");
        
        dt.Columns.Add("关单时间");
        dt.Columns.Add("SLA");
        
        dt.Columns.Add("用时");
        dt.Columns.Add("job code");
        dt.Columns.Add("是否转派其他二线");

        if (NeedStep)
        {
            dt.Columns.Add("处理步骤");
        }

        DataRow dr = dt.NewRow();
        StoreInfo info;
        List<AssignInfo> listAssign;
        List<CallStepInfo> listStep;
        int DropInCountMax = 0; //标记报修中最多的上门次数

        foreach (CallInfo item in list)
        {
            if (null==item)
            {
                throw new Exception(item.No);
            }
            dr = dt.NewRow();
            info = StoresBLL.Get(item.StoreID);
            dr["系统单号"] = item.No;
            dr["下单一线"] = item.CreatorName;
            dr["创建时间"] = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            dr["报修人"] = item.ErrorReportUser;
            dr["客户"] = item.CustomerName;
            dr["品牌"] = item.BrandName;
            dr["省份"] = item.ProvinceName;
            dr["城市"] = item.CityName;
            dr["店铺号"] = item.StoreName.Trim();
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
            dr["响应时间1(创建时间-报修时间)"] = Math.Round((item.CreateDate - item.ErrorDate).TotalHours, 2);
            dr["响应时间2(接单时间-创建时间)"] = "";
            dr["响应时间3(即 接单时间-报修时间)"] ="";

            dr["SLA"]=item.SLA;
            if (item.FinishDate==Function.ErrorDate)
            {
                dr["用时"] = "";
            }
            else
            {
                double taketime=Math.Round((item.FinishDate-item.ErrorDate).TotalHours,2);
                dr["用时"] =taketime>0?taketime:0;
            }

            listAssign = AssignBLL.GetList(item.ID);
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
            listStep = CallStepBLL.GetListJoin(item);
            if (null != listStep && listStep.Count > 0)
            {
                dr["接单时间"] = listStep[0].DateBegin.ToString("yyyy-MM-dd HH:mm");
                dr["响应时间2(接单时间-创建时间)"] = Math.Round((listStep[0].DateBegin - item.CreateDate).TotalHours, 2);
                dr["响应时间3(即 接单时间-报修时间)"] = Math.Round((listStep[0].DateBegin - item.ErrorDate).TotalHours, 2);

                int DropInCurrCallCount = 0;    //当前的报修上门次数
                foreach (CallStepInfo ItemStep in listStep)
                {
                    if (ItemStep.StepType==(int)SysEnum.StepType.到达门店处理)
                    {
                        DropInCurrCallCount++;
                        if (DropInCurrCallCount>DropInCountMax)
                        {
                            DropInCountMax = DropInCurrCallCount;
                            dt.Columns.Add("第" + DropInCountMax + "次上门工程师");
                            dt.Columns.Add("第" + DropInCountMax + "次到达门店时间");
                        }
                        dr["第" + DropInCurrCallCount + "次上门工程师"] = ItemStep.MajorUserName;
                        dr["第" + DropInCurrCallCount + "次到达门店时间"] = ItemStep.DateBegin.ToString("yyyy-MM-dd HH:mm");
                    }
                    if (ItemStep.IsSolved)
                    {
                        dr["二线最终的处理时间"] = ItemStep.DateEnd.ToString("yyyy-MM-dd HH:mm");
                        dr["二线最终的记录时间"] = ItemStep.AddDate.ToString("yyyy-MM-dd HH:mm");
                        dr["二线延时关CALL时间"] = Math.Round((ItemStep.AddDate-ItemStep.DateEnd).TotalHours, 2);
                        dr["二线最终的解决描述"] = ItemStep.Details;
                        
                    }
                    if (ItemStep.StepType == (int)SysEnum.StepType.关单)
                    {
                        dr["一线的关单人"] = ItemStep.MajorUserName;
                        dr["关单时间"] = ItemStep.AddDate.ToString("yyyy-MM-dd HH:mm"); ;
                    }
                    if (ItemStep.StepType == (int)SysEnum.StepType.上门安排)
                    {
                        dr["job code"] += ItemStep.Details + "+";
                    }
                }
            }
            else
            {
                dr["二线最终的记录时间"] = "";
                dr["二线最终的解决描述"] = "";
                dr["一线的关单人"] = "";
                dr["关单时间"] = "";
                dr["job code"] = "";
            }
            dr["job code"] = dr["job code"].ToString().Trim('+');
            if (NeedStep)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CallStepInfo Stepitem in listStep)
                {
                    sb.Append(Stepitem.StepIndex);
                    sb.Append("、(").Append(Stepitem.AddDate).Append(")");
                    sb.Append(Stepitem.StepName);
                    sb.Append("，操作备注：").Append(Stepitem.Details);
                    sb.Append("\r\n");
                }
                dr["处理步骤"] = sb;
            }

            dt.Rows.Add(dr);
        }
        return dt;
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

}
