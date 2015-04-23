using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Text;


public partial class page_call_list : _Call_list
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int State = Function.GetRequestInt("State");
            bool Flag = false;
            foreach (int item in Enum.GetValues(typeof(SysEnum.CallStateMain)))
            {
                if (State == item)
                {
                    LabState.Text = Enum.GetName(typeof(SysEnum.CallStateMain), item);
                    Flag = true; break;
                }
            }
            if (!Flag)
            {
                return;
            }

            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));
            DdlL2.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.二线.ToString());
            DdlL2.DataBind();
            DdlL2.Items.Insert(0, new ListItem("不限", "0"));
            DdlL1.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.一线.ToString());
            DdlL1.DataBind();
            DdlL1.Items.Insert(0, new ListItem("不限", "0"));

            DdlSloveBy.DataSource = SysEnum.ToDictionary(typeof(SysEnum.SolvedBy));
            DdlSloveBy.DataBind();
            DdlSloveBy.Items.Insert(0, new ListItem("不限", "0"));

            DdlReplacementStatus.DataSource = SysEnum.ToDictionary(typeof(SysEnum.ReplacementStatus));
            DdlReplacementStatus.DataBind();
            DdlReplacementStatus.Items.Insert(0, new ListItem("不限", "-1"));

            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));

            DdlCategory.DataSource = CallCategoryBLL.GetListEnable();
            DdlCategory.DataBind();
            DdlCategory.Items.Insert(0, new ListItem("不限", "0"));


            BindData(State);
            #region 根据状态隐藏列以及其它操作
            List<string> HideHead = new List<string>();
            switch (State)
            {
                case (int)SysEnum.CallStateMain.未处理:
                    HideHead.Add("处理记录"); HideHead.Add("后续处理"); HideHead.Add("解决方式"); HideHead.Add("预约时间/人员");
                    break;
                case (int)SysEnum.CallStateMain.处理中:
                    HideHead.Add("解决方式"); HideHead.Add("后续处理");
                    break;
                case (int)SysEnum.CallStateMain.已完成:
                    HideHead.Add("处理"); HideHead.Add("超时时间"); HideHead.Add("预约时间/人员");
                    BtnClose.Visible = true;
                    break;
                case (int)SysEnum.CallStateMain.已关闭:
                    HideHead.Add("后续处理"); HideHead.Add("处理"); HideHead.Add("超时时间"); HideHead.Add("预约时间/人员");
                    break;
                default:
                    break;
            }
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.跟进处理))
            {
                HideHead.Add("处理");
            }
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.转派))
            {
                HideHead.Add("转派");
            }
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.更换上门人))
            {
                HideHead.Add("更换上门人");
            }
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.回访))
            {
                HideHead.Add("回访");
            }

            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.备件记录))
            {
                HideHead.Add("备件管理");
            }

            bool PowerDelete = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.删除记录);
            bool PowerClose = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.关闭报修);
            if (!PowerClose)
            {
                BtnClose.Visible = false;
            }
            if (!PowerDelete)
            {
                BtnDelete.Visible = false;
            }
            if (!PowerDelete && !PowerClose)
            {
                HideHead.Add("选择");
                PhSelectAll.Visible = false;
            }

            HideHead.AddRange(CallBLL.GetListItem(CurrentUser));

            GridViewHide(HideHead);

            #endregion

        }
    }

    private void BindData(int State)
    {

        #region 设置好下拉选项
        if (State == (int)SysEnum.CallStateMain.处理中)
        {
            DdlState.DataSource = SysEnum.ToDictionary(typeof(SysEnum.CallStateDetails));
            DdlState.DataBind();
            DdlState.Items.Insert(0, new ListItem("不限", "0"));
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
            DdlState.Enabled = true;
        }

        if (State == (int)SysEnum.CallStateMain.已完成)
        {
            DdlSloveBy.Enabled = DdlReplacementStatus.Enabled = true;
        }

        #endregion



        int PageSize = 20;  //每页记录数
        int PageIndex = Function.GetRequestInt("page");  //当前页码
        int page = 10;      //分页显示数
        int count = 0;     //记录总数
        string url = "&state=" + State;
        StringBuilder strWhere = new StringBuilder();
        strWhere.Append(" 1=1  and f_StateMain=").Append(State);

        #region 搜索条件
        SchStateDetail(State, ref url, ref strWhere);

        SchDate(ref url, ref strWhere);

        SchUser(State, ref url, ref strWhere);

        SchStoreBrand(ref url, ref strWhere);

        SchPrivinceCity(ref url, ref strWhere);
        SchNo(ref url, ref strWhere);

        SchAssign(ref url, ref strWhere);

        SchSloveby(ref url, ref strWhere);

        SchReplacementStatus(ref url, ref strWhere);

        SchCategory(ref url, ref strWhere);
        #endregion



        strWhere.Append(" order by id desc ");
        List<CallInfo> list = CallBLL.GetList(PageSize, PageIndex, strWhere.ToString(), out count);
        GridView1.DataSource = list;
        GridView1.DataBind();
        InitTableStype(list);
        this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);
    }



    #region 搜索条件
    private void SchNo(ref string url, ref StringBuilder strWhere)
    {
        string CallNo = Function.ClearText(Function.GetRequestSrtring("CallNo"));
        if (!string.IsNullOrEmpty(CallNo))
        {
            strWhere.Append(" AND F_No LIKE '%").Append(CallNo).Append("%' ");
            url += "&CallNo=" + CallNo;
            TxbCallNo.Text = CallNo;
        }

        string No2 = Function.ClearText(Function.GetRequestSrtring("No2"));
        if (!string.IsNullOrEmpty(No2))
        {
            strWhere.Append(" AND f_ReportSourceNo LIKE '%").Append(No2).Append("%' ");
            url += "&No2=" + No2;
            TxbNo2.Text = No2;
        }
    }

    private void SchPrivinceCity(ref string url, ref StringBuilder strWhere)
    {
        int PrivinceID = Function.GetRequestInt("PrivinceID");
        int CityID = Function.GetRequestInt("CityID");
        if (PrivinceID > 0)
        {
            DdlProvince.SelectedValue = PrivinceID.ToString();
            strWhere.Append(" and f_ProvinceID = ").Append(PrivinceID);
            url += "&PrivinceID=" + PrivinceID;
            DdlCity.DataSource = CityBLL.GetList(PrivinceID);
            DdlCity.DataBind();
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));

            if (CityID > 0)
            {
                DdlCity.SelectedValue = CityID.ToString();
                strWhere.Append(" and f_CityID =").Append(CityID);
                url += "&CityID=" + CityID;
            }
        }
    }
    private void SchStoreBrand(ref string url, ref StringBuilder strWhere)
    {
        string StoreNo = Function.ClearText(Function.GetRequestSrtring("StoreNo"));
        if (!string.IsNullOrEmpty(StoreNo))
        {
            TxbStoreNo.Text = StoreNo;
            strWhere.Append(string.Format(" and( f_StoreName  like '%{0}%' or f_StoreNo like '%{0}%' ) ", StoreNo));
            url += "&StoreNo=" + StoreNo;
        }
        int CustomerID = Function.GetRequestInt("CustomerID");
        if (CustomerID > 0)
        {
            DdlCustomer.SelectedValue = CustomerID.ToString();
            DdlCustomer_SelectedIndexChanged(null, null);
            strWhere.Append(" AND f_CustomerID=").Append(CustomerID);
            url += "&CustomerID=" + CustomerID;

            int BrandID = Function.GetRequestInt("BrandID");
            if (BrandID > 0)
            {
                DdlBrand.SelectedValue = BrandID.ToString();
                strWhere.Append(" and f_BrandID =").Append(BrandID);
                url += "&BrandID=" + BrandID;
            }
        }

    }

    private void SchStateDetail(int State, ref string url, ref StringBuilder strWhere)
    {
        int StateDetail = Function.GetRequestInt("StateDetail");
        if (StateDetail > 0 && State == (int)SysEnum.CallStateMain.处理中)
        {
            DdlState.SelectedValue = StateDetail.ToString();
            strWhere.Append(" and f_StateDetail=").Append(StateDetail);
            url += "&StateDetail=" + StateDetail;
        }
    }

    private void SchUser(int State, ref string url, ref StringBuilder strWhere)
    {
        int L1 = Function.GetRequestInt("L1");
        if (L1 > 0)
        {
            DdlL1.SelectedValue = L1.ToString();
            strWhere.Append(" and f_CreatorID=").Append(L1);
            url += "&L1=" + L1;
        }
        int L2 = Function.GetRequestInt("L2");
        if (L2 > 0)
        {
            DdlL2.SelectedValue = L2.ToString();
            strWhere.Append(" and f_MaintainUserID=").Append(L2);
            url += "&L2=" + L2;
        }
        string DropInUser = Function.ClearText(Function.GetRequestSrtring("DropInUser"));
        if (!string.IsNullOrEmpty(DropInUser) && State != (int)SysEnum.CallStateMain.未处理)
        {
            TxbDropInUser.Text = DropInUser;
            strWhere.Append(string.Format(" and ID IN (SELECT f_CallID FROM sys_CallStep WHERE f_StepType in({0},{1}) and f_MajorUserName like '%{2}%' )  ", (int)SysEnum.StepType.上门安排, (int)SysEnum.StepType.上门详细, DropInUser));
            url += "&DropInUser=" + DropInUser;
        }
    }

    private void SchDate(ref string url, ref StringBuilder strWhere)
    {
        DateTime DtBegin = Function.GetRequestDateTime("DtBegin");
        DateTime DtEnd = Function.GetRequestDateTime("DtEnd");
        if (DtBegin > DtEnd)
        {
            Function.AlertBack("开始日期必需小于结束日期");
        }
        if (DtBegin != Function.ErrorDate)
        {
            TxtDateBegin.Text = DtBegin.ToString("yyyy-MM-dd");
            strWhere.Append(" and DATEDIFF(day,'").Append(DtBegin).Append("',f_ErrorDate)>=0 ");
            url += "&DtBegin=" + TxtDateBegin.Text;
        }
        if (DtEnd != Function.ErrorDate)
        {
            TxbDateEnd.Text = DtEnd.ToString("yyyy-MM-dd");
            strWhere.Append(" and DATEDIFF(day,'").Append(DtEnd).Append("',f_ErrorDate)<=0 ");
            url += "&DtEnd=" + TxbDateEnd.Text;
        }
    }

    private void SchAssign(ref string url, ref StringBuilder strWhere)
    {
        if (!IsAdmin)
        {
            if (GroupBLL.PowerCheck((int)PowerInfo.P1_Call.查看组内所有报修))
            {
                strWhere.Append(" AND f_BrandID in(SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID=").Append(CurrentUser.WorkGroupID).Append(") ");
            }
            else
            {
                strWhere.Append(string.Format(" AND (f_CreatorID={0} OR f_MaintainUserID={0} OR f_AssignUserID={0})", CurrentUserID));
            }
        }
    }
    private void SchSloveby(ref string url, ref StringBuilder strWhere)
    {
        int Sloveby = Function.GetRequestInt("Sloveby");
        if (Sloveby <= 0)
        {
            return;
        }
        try
        {
            SysEnum.SolvedBy sloveby = (SysEnum.SolvedBy)Sloveby;
            DdlSloveBy.SelectedValue = Sloveby.ToString();
            strWhere.Append(" and f_SloveBy='").Append(sloveby.ToString()).Append("' ");
            url += "&Sloveby=" + sloveby;
        }
        catch (Exception)
        {
        }

    }

    private void SchReplacementStatus(ref string url, ref StringBuilder strWhere)
    {
        int ReplacementStatus = Function.GetRequestInt("ReplacementStatus");
        if (ReplacementStatus < 0)
        {
            return;
        }
        DdlReplacementStatus.SelectedValue = ReplacementStatus.ToString();
        strWhere.Append(" and f_ReplacementStatus=").Append(ReplacementStatus).Append(" ");
        url += "&ReplacementStatus=" + ReplacementStatus;

    }

    private void SchCategory(ref string url, ref StringBuilder strWhere)
    {
        int Category = Function.GetRequestInt("Category");
        if (Category <= 0)
        {
            return;
        }

        strWhere.Append(" and f_Category=").Append(Category).Append(" ");
        url += "&Category=" + Category;
        DdlCategory.SelectedValue = Category.ToString();

    }

    #endregion

    private void InitTableStype(List<CallInfo> list)
    {
        if (list == null || list.Count == 0)
        {
            list.Add(new CallInfo());
            GridView1.DataSource = list;
            GridView1.DataBind();
            GridView1.Rows[0].Cells.Clear();
            GridView1.Rows[0].Cells.Add(new TableCell());
            GridView1.Rows[0].Visible = false;
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
            CallInfo info = CallBLL.Get(Function.ConverToInt(item));
            if (null == info)
            {
                continue;
            }
            if (info.AssignID == CurrentUser.WorkGroupID && !IsAdmin)
            {
                continue;
            }
            info.IsClosed = true;

            CallBLL.Delete(info.ID);


        }
        Function.Refresh();

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;

        CallInfo cinfo = (CallInfo)e.Row.DataItem;


        if (cinfo.StateMain == (int)SysEnum.CallStateMain.处理中 || cinfo.StateMain == (int)SysEnum.CallStateMain.未处理)
        {
            if (cinfo.SLADateEnd != DicInfo.DateZone && DateTime.Now > cinfo.SLADateEnd)
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
            }
            if (CallBLL.EnableEdit(cinfo, CurrentUser))
            {
                HyperLink HlEditCall = (HyperLink)e.Row.FindControl("HlEditCall");
                HlEditCall.NavigateUrl = "javascript:tb_show('编辑报修', 'Edit.aspx?ID=" + cinfo.ID + "&TB_iframe=true&height=450&width=730', false);";
            }
        }

        Label ctDropIn1Date = (Label)e.Row.FindControl("LabDropIn1Date");
        Label ctDropInUser = (Label)e.Row.FindControl("LabDropInUser");
        if (cinfo.StateMain == (int)SysEnum.CallStateMain.处理中 && (cinfo.StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || cinfo.StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应))
        {
            CallStepInfo stinfo = CallStepBLL.GetLast(cinfo.ID);
            if (null != stinfo && stinfo.ID > 0)
            {
                ctDropIn1Date.Text = stinfo.DateBegin.ToString("yyyy-MM-dd HH:mm");
                ctDropInUser.Text = stinfo.MajorUserName;
            }
        }
    }

    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "list.aspx";
        Url += "?State=" + Function.GetRequestInt("State");
        Url += "&DtBegin=" + TxtDateBegin.Text.Trim();
        Url += "&DtEnd=" + TxbDateEnd.Text.Trim();
        Url += "&L2=" + DdlL2.SelectedValue;
        Url += "&L1=" + DdlL1.SelectedValue;
        Url += "&DropInUser=" + TxbDropInUser.Text.Trim();
        Url += "&PrivinceID=" + DdlProvince.SelectedValue;
        Url += "&CityID=" + DdlCity.SelectedValue;
        Url += "&StoreNo=" + TxbStoreNo.Text.Trim();
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Url += "&CustomerID=" + DdlCustomer.SelectedValue;
        Url += "&CallNo=" + TxbCallNo.Text.Trim();
        Url += "&StateDetail=" + DdlState.SelectedValue;
        Url += "&Sloveby=" + DdlSloveBy.SelectedValue;
        Url += "&ReplacementStatus=" + DdlReplacementStatus.SelectedValue;
        Url += "&No2=" + TxbNo2.Text.Trim();
        Url += "&Category=" + DdlCategory.SelectedValue;
        Response.Redirect(Url);
    }

    #region 异步响应
    protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlProvince.SelectedValue);
        DdlCity.DataSource = (ID > 0) ? CityBLL.GetList(ID) : null;
        DdlCity.DataBind();
        DdlCity.Items.Insert(0, new ListItem("不限", "0"));

    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
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

    #endregion

    protected void BtnClose_Click(object sender, EventArgs e)
    {
        string delList = Function.GetRequestSrtring("ckDel");
        if (string.IsNullOrEmpty(delList))
        {
            Function.AlertBack("没有选中数据");
            return;
        }
        foreach (string item in delList.Split(','))
        {
            CallInfo info = CallBLL.Get(Function.ConverToInt(item));
            if (null == info)
            {
                continue;
            }
            if (info.StateMain != (int)SysEnum.CallStateMain.已完成)
            {
                continue;
            }
            Call_Close(info, info.No);
        }
        Function.Refresh();

    }


    public string Call_Close(CallInfo info, string NoClose)
    {
        NoClose = NoClose.Trim();
        if (null == info)
        {
            return "无法获取数据";
        }
        if (NoClose.Length > 100)
        {
            return "服务单号不应超过100个字";
        }


        info.StateMain = (int)SysEnum.CallStateMain.已关闭;
        info.CallNo2 = NoClose;
        if (CallBLL.Edit(info))
        {
            CallStepInfo sinfo = new CallStepInfo();
            sinfo.CallID = info.ID;
            sinfo.AddDate = DateTime.Now;
            sinfo.DateBegin = sinfo.DateEnd = DateTime.Now;
            sinfo.Details = string.Format("由{0}进行关单，回收服务单号为{1}", CurrentUserName, NoClose);
            sinfo.IsSolved = false;
            sinfo.MajorUserID = CurrentUserID;
            sinfo.MajorUserName = CurrentUserName;
            sinfo.SolutionID = 0;
            sinfo.SolutionName = SysEnum.StepType.关单.ToString();
            sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(info.ID) + 1;
            sinfo.StepName = SysEnum.StepType.关单.ToString();
            sinfo.StepType = (int)SysEnum.StepType.关单;
            sinfo.UserID = CurrentUserID;
            sinfo.UserName = CurrentUserName;
            if (CallStepBLL.AddCallStep_UpdateCall(info, sinfo))
            {
                return string.Empty;
            }
            return "系统已接收服务单，但无法更新状态。这可能由网络原因引起的，请稍候再试。";
        }
        return "关闭失败，请重试或联系管理员";

    }

}
