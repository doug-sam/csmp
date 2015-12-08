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

public partial class page_call_Sch : _Call_Sch
{
    // private static ListItem DefItem = new ListItem("不限", "0");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TR_State.Visible = GroupBLL.PowerCheck((int)PowerInfo.P1_Call.查询不同状态报修);
            LtlL1.Visible = DdlL1.Visible = TR_User.Visible = !CurrentUser.Rule.Contains( SysEnum.Rule.客户.ToString());

            DdlProvince.DataSource = ProvincesBLL.GetList();
            DdlProvince.DataBind();
            DdlProvince.Items.Insert(0, new ListItem("不限", "0"));
            DdlCity.Items.Insert(0, new ListItem("不限", "0"));

            DdlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            DdlCustomer.DataBind();
            if (DdlCustomer.Items.Count == 1)
            {
                DdlCustomer.SelectedValue = DdlCustomer.Items[0].Value;
                DdlCustomer_SelectedIndexChanged(sender, e);
                DdlCustomer.Visible = false;
            }
            else
            {
                DdlCustomer.Items.Insert(0, new ListItem("不限", "0"));
            }

            DdlState.DataSource = SysEnum.ToDictionary(typeof(SysEnum.CallStateMain));
            DdlState.DataBind();
            //DdlStateDetail.DataSource = SysEnum.ToDictionary(typeof(SysEnum.CallStateDetails));
            //DdlStateDetail.DataBind();
            DdlState.Items.Insert(0, new ListItem("不限", "0"));
            //DdlStateDetail.Items.Insert(0, new ListItem("不限", "0"));
            DdlSolvedBy.DataSource = SysEnum.ToDictionary(typeof(SysEnum.SolvedBy));
            DdlSolvedBy.DataBind();
            DdlSolvedBy.Items.Insert(0, new ListItem("不限", "0"));
            DdlL2.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.二线.ToString());
            DdlL2.DataBind();
            DdlL2.Items.Insert(0, new ListItem("不限", "0"));
            DdlL1.DataSource = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.一线.ToString());
            DdlL1.DataBind();
            DdlL1.Items.Insert(0, new ListItem("不限", "0"));


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
            HideHead.AddRange(CallBLL.GetListItem2(CurrentUser));
            
            GridViewHide(HideHead);
            #region 如果是APP消息查看跳转过来的，执行如下步骤 ZQL 2015.11.30新增
            string isRead = Function.GetRequestSrtring("isRead");
            if (isRead == "1")
            {
                string callNo = Function.GetRequestSrtring("CallNo");
                List<MarqueeMessage> marqueeList = MarqueeMessageBLL.GetList(" AND f_No='" + callNo + "'");
                if (marqueeList.Count > 0)
                {
                    MarqueeMessage mInfo = marqueeList[0];
                    mInfo.IsRead = true;
                    MarqueeMessageBLL.AddBySP(mInfo);
                }
                
            }

            #endregion
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
        this.List_Page.Text = Function.Paging2(PageSize, count, page, PageIndex, url);
    }

    private void GetSQL(out string url, out string strWhere)
    {
        url = "";
        strWhere = " 1=1 ";
        #region 搜索条件

        SchState(ref url, ref strWhere);

        SchDate(ref url, ref strWhere);

        SchFinishDate(ref url, ref strWhere);

        SchStoreBrand(ref url, ref strWhere);

        SchPrivinceCity(ref url, ref strWhere);

        SchNo_SolvedBy(ref url, ref strWhere);

        SchUser(ref url, ref strWhere);

        SchClass(ref url, ref strWhere);

        //SchSolvedBy(ref url, ref strWhere);

        SchCategory(ref url, ref strWhere);

        SchErrorReportUser(ref url, ref strWhere);//ZQL 15.4.15新增

        #endregion
        if (!IsAdmin)
        {
           // strWhere += string.Format(" AND (f_WorkGroupID={0} OR f_AssignID={0})", CurrentUser.WorkGroupID);
            strWhere += string.Format(" AND  f_BrandID IN(SELECT f_MID FROM sys_WorkGroupBrand WHERE f_WorkGroupID={0} )", CurrentUser.WorkGroupID);
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
        string StoreNo = Function.ClearText(Function.GetRequestSrtring("StoreNo"));
        if (!string.IsNullOrEmpty(StoreNo))
        {
            TxbStoreNo.Text = StoreNo;
            strWhere += string.Format(" and ( f_StoreName like '%{0}%' or  f_StoreNo like '%{0}%' ) ", StoreNo);
            url += "&StoreNo=" + StoreNo;
        }
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
                    //DdlStateDetail.SelectedValue = StateDetail.ToString();
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
        if (DtBegin > DtEnd)
        {
            Function.AlertBack("开始日期必需小于结束日期");
        }
        if (DtBegin != Tool.Function.ErrorDate)
        {
            TxtDateBegin.Text = DtBegin.ToString("yyyy-MM-dd");
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_ErrorDate)>=0 ", DtBegin);
            url += "&DtBegin=" + TxtDateBegin.Text;
        }
        if (DtEnd != Tool.Function.ErrorDate)
        {
            TxbDateEnd.Text = DtEnd.ToString("yyyy-MM-dd");
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_ErrorDate)<=0 ", DtEnd);
            url += "&DtEnd=" + TxbDateEnd.Text;
        }
    }

    /// <summary>
    /// 完成 时间范围
    /// </summary>
    /// <param name="url"></param>
    /// <param name="strWhere"></param>
    private void SchFinishDate(ref string url, ref string strWhere)
    {
        DateTime DtBegin = Function.GetRequestDateTime("FinishDateBegin");
        DateTime DtEnd = Function.GetRequestDateTime("FinishDateEnd");
        if (DtBegin != Tool.Function.ErrorDate)
        {
            TxbFinishDateBegin.Text = DtBegin.ToString("yyyy-MM-dd");
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_FinishDate)>=0 ", DtBegin);
            url += "&FinishDateBegin=" + TxbFinishDateBegin.Text;
        }
        if (DtEnd != Tool.Function.ErrorDate)
        {
            TxbFinishDateEnd.Text = DtEnd.ToString("yyyy-MM-dd");
            strWhere += string.Format(" and DATEDIFF(day,'{0}',f_FinishDate)<=0 ", DtEnd);
            url += "&FinishDateEnd=" + TxbFinishDateEnd.Text;
        }
    }

    private void SchNo_SolvedBy(ref string url, ref string strWhere)
    {
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

    private void SchUser(ref string url, ref string strWhere)
    {
        if (!TR_User.Visible)
        {
            return;
        }

        int L1 = Function.GetRequestInt("L1");
        if (L1 > 0)
        {
            DdlL1.SelectedValue = L1.ToString();
            strWhere += string.Format(" and f_CreatorID={0} ", L1);
            url += "&L1=" + L1;
        }
        int L2 = Function.GetRequestInt("L2");
        if (L2 > 0)
        {
            DdlL2.SelectedValue = L2.ToString();
            strWhere += string.Format(" and f_MaintainUserID={0} ", L2);
            url += "&L2=" + L2;
        }
        string DropInUser = Function.ClearText(Function.GetRequestSrtring("DropInUser"));
        if (!string.IsNullOrEmpty(DropInUser))
        {
            TxbDropInUser.Text = DropInUser;
            strWhere += string.Format(" and ID IN (SELECT f_CallID FROM sys_CallStep WHERE f_StepType ={0} and f_MajorUserName like '%{1}%' )  ", (int)SysEnum.StepType.上门详细, Function.ClearText(DropInUser));
            url += "&DropInUser=" + DropInUser;
        }
    }

    private void SchClass(ref string url, ref string strWhere)
    {
        if (!TR_User.Visible)
        {
            return;
        }

        int C1 = Function.GetRequestInt("C1");
        if (C1 > 0)
        {
            DdlClass1.SelectedValue = C1.ToString();
            strWhere += string.Format(" and f_Class1={0} ", C1);
            url += "&C1=" + C1;
            DdlClass1_SelectedIndexChanged(null, null);

            int C2 = Function.GetRequestInt("C2");
            if (C2 > 0)
            {
                DdlClass2.SelectedValue = C2.ToString();
                strWhere += string.Format(" and f_Class2={0} ", C2);
                url += "&C2=" + C2;
            }
        }
    }

    //private void SchSolvedBy(ref string url, ref string strWhere)
    //{
    //    int SolvedBy = Function.GetRequestInt("SolvedBy");
    //    if (SolvedBy > 0)
    //    {
    //        DdlSolvedBy.SelectedValue = SolvedBy.ToString();
    //        strWhere += string.Format(" and f_SloveBy='{0}' ",Function.ClearText(DdlSolvedBy.SelectedItem.Text));
    //        url += "&SolvedBy=" + SolvedBy;
    //    }
    //}
    
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
    /// <summary>
    /// 报修人 查询条件
    /// ZQL 15/4/15新增
    /// </summary>
    /// <param name="url"></param>
    /// <param name="strWhere"></param>
    private void SchErrorReportUser(ref string url, ref string strWhere)
    {
        string ErrorReportUser = Function.GetRequestSrtring("ErrorReportUser");
        if (!string.IsNullOrEmpty(ErrorReportUser))
        {
            strWhere += string.Format(" AND f_ErrorReportUser LIKE '%{0}%' ", ErrorReportUser);
            url += "&ErrorReportUser=" + ErrorReportUser;
            TxbErrorReportUser.Text = ErrorReportUser;
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
                CallBLL.Delete(Function.ConverToInt(item));
            }
        }
        Function.Refresh();

    }


    protected void BtnSch_Click(object sender, EventArgs e)
    {
        string Url = "Sch.aspx";
        Url += "?State=" + DdlState.SelectedValue;
        //Url += "&StateDetail=" + DdlStateDetail.SelectedValue;ZQL2015.4.15注释
        Url += "&ErrorReportUser=" + TxbErrorReportUser.Text.Trim();//ZQL2015.4.15新增
        Url += "&DtBegin=" + TxtDateBegin.Text.Trim();
        Url += "&DtEnd=" + TxbDateEnd.Text.Trim();
        Url += "&FinishDateBegin=" + TxbFinishDateBegin.Text.Trim();
        Url += "&FinishDateEnd=" + TxbFinishDateEnd.Text.Trim();
        Url += "&PrivinceID=" + DdlProvince.SelectedValue;
        Url += "&CityID=" + DdlCity.SelectedValue;
        Url += "&StoreNo=" + TxbStoreNo.Text.Trim();
        Url += "&BrandID=" + DdlBrand.SelectedValue;
        Url += "&CustomerID=" + DdlCustomer.SelectedValue;
        Url += "&CallNo=" + TxbCallNo.Text.Trim();
        Url += "&L2=" + DdlL2.SelectedValue;
        Url += "&L1=" + DdlL1.SelectedValue;
        Url += "&C1=" + DdlClass1.SelectedValue;
        Url += "&C2=" + DdlClass2.SelectedValue;
        Url += "&DropInUser=" + TxbDropInUser.Text.Trim();
        Url += "&SolvedBy=" + DdlSolvedBy.SelectedValue;
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
        //DdlStateDetail.Visible = (ID == (int)SysEnum.CallStateMain.处理中);
    }

    protected void DdlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlCustomer.SelectedValue);
        if (ID > 0)
        {
            DdlClass1.DataSource = Class1BLL.GetList(ID);

            List<BrandInfo> blist = BrandBLL.GetList(ID, CurrentUser.WorkGroupID);
            DdlBrand.DataSource = blist;
            DdlBrand.DataBind();
            if (blist.Count == 1)
            {
                DdlBrand.SelectedValue = DdlBrand.Items[0].Value;
            }
            else
            {
                DdlBrand.Items.Insert(0, new ListItem("不限", "0"));
            }
        }
        else
        {
            DdlBrand.DataSource = null;//TODO::把它改正确过来
            DdlBrand.DataBind();
            DdlBrand.Items.Insert(0, new ListItem("不限", "0"));

            DdlClass1.DataSource = null;
            DdlClass2.DataSource = null;
            DdlClass2.DataBind();
        }
        DdlClass1.DataBind();
        DdlClass1.Items.Insert(0, new ListItem("不限", "0"));

    }

    protected void DdlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Function.ConverToInt(DdlClass1.SelectedValue);
        if (ID > 0)
        {
            DdlClass2.DataSource = Class2BLL.GetList(ID);
        }
        else
        {
            DdlClass2.DataSource = null;
        }
        DdlClass2.DataBind();
        DdlClass2.Items.Insert(0, new ListItem("不限", "0"));
    }
}
