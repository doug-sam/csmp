using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Threading;
using Newtonsoft.Json.Linq;
using DBUtility;

public partial class page_call_add : _Call_Add
{
    /* 页面可接收参数 
     * callNumber:来电号码，系统会把前面的0去掉。然后去找店铺表有没有这个电话，有则获得其店铺信息
     * CustomerRequestID:客户请求ID，
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlCustomer.DataSource = CustomersBLL.GetList(CurrentUser);
            ddlCustomer.DataBind();

            DdlCategory.DataSource = CallCategoryBLL.GetListEnable();
            DdlCategory.DataBind();


            ddlReportSource.DataSource = ReportSourceBLL.GetList();
            ddlReportSource.DataBind();
            TxtErrorDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            #region 2015.5.25ZQL添加 获取called
            string Called = Function.GetRequestSrtring("called");
            if (!string.IsNullOrEmpty(Called))
            {
                TrunkNOInfo trunkNOInfo = TrunkNO.Get(Called);
                if (null != trunkNOInfo)
                {
                    labCalled.Text = "客户：" + trunkNOInfo.VirtualNo.Trim() + "(" + trunkNOInfo.Description + ")";
                }
                else {
                    labCalled.Text = "客户：" + Called;
                }
            }
            #endregion

            StoreInfo sinfo = GetStoreInfo();
            if (null != sinfo)
            {
                BtnStore.Visible = false;
                TxtStoreNo.Enabled = ddlCustomer.Enabled = false;
                TxtStoreNo.Text = sinfo.ID.ToString();
                BtnStore_Click(sender, e);
                ddlReportSource.ClearSelection();
                foreach (ListItem item in ddlReportSource.Items)
                {
                    if (item.Text.Trim() == "电话")
                    {
                        ddlReportSource.SelectedValue = item.Value; break;
                    }
                }
            }

            ddlReportSource.Items.Insert(0, new ListItem("请选择", "0"));
            ddlCustomer.Items.Insert(0, new ListItem("请选择", "0"));
            if (CurrentUser.Rule.Contains(SysEnum.Rule.客户.ToString()))
            {
                txtReporter.Text = CurrentUser.Name;
                foreach (ListItem item in ddlReportSource.Items)
                {
                    if (item.Text.Trim() == "BBS")
                    {
                        ddlReportSource.SelectedValue = item.Value; break;
                    }
                }
            }

            CustomerRequestInfo crinfo = GetCustomerRequestInfo();
            if (null != crinfo)
            {
                TxtDetails.Text = crinfo.Details;
                txtReporter.Text = crinfo.ErrorReportUserName;
            }
        }
    }

    /// <summary>
    /// 根据地址栏电话获取店铺信息
    /// </summary>
    /// <returns></returns>
    public StoreInfo GetStoreInfo()
    {
        string CallNo = Function.GetRequestSrtring("callNumber");
        if (!string.IsNullOrEmpty(CallNo))
        {
            CallNo = CallNo.Trim().TrimStart('0').Trim();

            //StoreInfo sinfo = StoresBLL.Get(CallNo);
            StoreInfo sinfo = StoresBLL.GetByCallNO(CallNo);
            if (null == sinfo)
            {
                //sinfo = StoresBLL.Get(CallNo);
                sinfo = StoresBLL.GetByCallNO(CallNo);
            }
            return sinfo;
        }
        CustomerRequestInfo crinfo = GetCustomerRequestInfo();
        if (null != crinfo)
        {
            StoreInfo sinfo = StoresBLL.Get(crinfo.StoreID);
            return sinfo;
        }
        return null;
    }


    #region 客户下拉改变选项
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlCustomer.SelectedValue);
        if (ID <= 0)
        {
            ddlBrand.DataSource = null;
            ddlBrand.DataBind();
        }
        else
        {
            ddlBrand.DataSource = BrandBLL.GetList(ID);
            ddlBrand.DataBind();
            ddlBrand.Items.Insert(0, new ListItem("请选择", "0"));

            ddlClass1.DataSource = Class1BLL.GetList(ID);
            ddlClass1.DataBind();
            ddlClass1.Items.Insert(0, new ListItem("请选择", "0"));

            ddlClass1.Enabled = true;


        }
        ddlClass1.Enabled = ddlBrand.Enabled = ID > 0;
    }


    protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlBrand.SelectedValue);
        ddlProvince.Enabled = ID > 0;
        if (ID <= 0)
        {
            ddlProvince.DataSource = new List<ProvincesInfo>();
            ddlProvince.DataBind();

            ddlL2Id.DataSource = new List<UserInfo>();
            ddlL2Id.DataBind();
            ddlProvince.Enabled = false;
        }
        else
        {
            ddlProvince.Enabled = true;

            ddlProvince.DataSource = ProvincesBLL.GetList();
            ddlProvince.DataBind();
            ddlProvince.Items.Insert(0, new ListItem("请选择", "0"));
        }
        SetKnowledgebaseLink();
    }

    protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlProvince.SelectedValue);
        ddlCity.Enabled = ID > 0;
        if (ID <= 0)
        {
            ddlCity.DataSource = null;
            ddlCity.DataBind();
        }
        else
        {
            ddlCity.DataSource = CityBLL.GetList(ID);
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("请选择", "0"));
            BindL2(ID);
        }
    }
    private void BindL2(int ID)
    {
        if (ViewState["L2List"] == null)
        {
            List<UserInfo> list = UserBLL.GetList(CurrentUser.WorkGroupID, SysEnum.Rule.二线.ToString());
            foreach (UserInfo item in list)
            {
                item.Name = Hz2Py.JoinFirstPy(item.Name);
            }
            ViewState["L2List"] = list;
        }
        ddlL2Id.DataSource = (List<UserInfo>)ViewState["L2List"];
        ddlL2Id.DataBind();
        ddlL2Id.Items.Insert(0, new ListItem("不限", "0"));
        foreach (ListItem item in ddlL2Id.Items)
        {
            if (item.Value == CurrentUser.ID.ToString())
            {
                item.Selected = true;
            }
        }
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlCity.SelectedValue);
        ddlStore.Enabled = ID > 0;
        if (ID <= 0)
        {
            ddlStore.DataSource = null;
            ddlStore.DataBind();
        }
        else
        {
            ddlStore.DataSource = StoresBLL.GetList(ID, Convert.ToInt16(ddlBrand.SelectedValue));
            ddlStore.DataBind();
            ddlStore.Items.Insert(0, new ListItem("请选择", "0"));
        }
    }

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlStore.SelectedValue);
        if (ID > 0)
        {
            StoreInfo info = StoresBLL.Get(ID);
            if (null != info)
            {
                TxtStoreNo.Text = info.No;
                LtlTel.Text = string.IsNullOrEmpty(info.Tel) ? "暂无数据" : info.Tel;
                LtlAddress.Text = string.IsNullOrEmpty(info.Address) ? "暂无数据" : info.Address;
                LtlStoreType.Text =info.StoreType;
                BindHistoryCall(info.ID);
            }
        }
    }

    #endregion

    protected void ddlClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlClass1.SelectedValue);
        if (ID > 0)
        {

            ddlClass2.DataSource = Class2BLL.GetList(ID);
            ddlClass2.DataBind();
            ddlClass2.Items.Insert(0, new ListItem("请选择", "0"));
            ddlClass2.Enabled = true;
        }
        else
        {
            ddlClass2.DataSource = null;
            ddlClass2.DataBind();
        }
    }

    protected void ddlClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlClass2.SelectedValue);
        if (ID > 0)
        {
            ddlClass3.DataSource = Class3BLL.GetList(ID);
            ddlClass3.DataBind();
            ddlClass3.Items.Insert(0, new ListItem("请选择", "0"));
            ddlClass3.Enabled = true;
        }
        else
        {
            ddlClass3.DataSource = null;
            ddlClass3.DataBind();
        }
    }

    protected void ddlClass3_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ID = Convert.ToInt16(ddlClass3.SelectedValue);
        if (ID > 0)
        {
            Repeater1.DataSource = SolutionBLL.GetList(ID);
            Repeater1.DataBind();


            Class3Info info = Class3BLL.Get(ID);
            if (null != info)
            {
                PrioritiesInfo pinfo = PrioritiesBLL.Get(info.PriorityID);
                if (pinfo != null)
                {
                    LtlPriority.Text = pinfo.Name;
                    TxbSLA.Text = info.SLA.ToString();
                    if (string.IsNullOrEmpty(TxtDetails.Text)) TxtDetails.Text = info.Name;
                }
            }
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        #region 数据验证
        StoreInfo sinfo = StoresBLL.Get(Function.ConverToInt(ddlStore.SelectedValue));
        if (sinfo == null)
        {
            Function.AlertMsg("店铺信息有误。请认真填写，如直接填写店铺号，请先点击确定"); return;
        }


        Class3Info c3info = Class3BLL.Get(Function.ConverToInt(ddlClass3.SelectedValue));
        if (c3info == null)
        {
            Function.AlertMsg("请认真选择报修的大、中、小类"); return;
        }
        Class2Info c2info = Class2BLL.Get(c3info.Class2ID);
        if (c2info == null)
        {
            Function.AlertMsg("请认真选择报修的大、中、小类"); return;
        }
        Class1Info c1info = Class1BLL.Get(c2info.Class1ID);
        if (c1info == null)
        {
            Function.AlertMsg("请认真选择报修的大、中、小类"); return;
        }

        CityInfo ctinfo = CityBLL.Get(Function.ConverToInt(ddlCity.SelectedValue));
        if (ctinfo == null)
        {
            Function.AlertMsg("请选择城市"); return;
        }
        UserInfo uinfo = UserBLL.Get(Function.ConverToInt(ddlL2Id.SelectedValue));
        if (uinfo == null)
        {
            Function.AlertMsg("请选择技术工程师"); return;
        }
        //TODO:应该进行工程师跟客户是不是有关系的判断

        if (CurrentUserID <= 0 || string.IsNullOrEmpty(CurrentUserName))
        {
            Function.AlertMsg("用户登录数据丢失，请重新登录"); return;
        }
        if (ddlReportSource.SelectedValue == "0")
        {
            Function.AlertMsg("请选择报修源"); return;
        }
        if (txtSourceNo.Text.Length > 100)
        {
            Function.AlertMsg("源单号不能多于100个字"); return;
        }
        if (Function.ConverToDateTime(TxtErrorDate.Text) == Function.ErrorDate)
        {
            Function.AlertMsg("报修日期有误"); return;
        }
        if (string.IsNullOrEmpty(txtReporter.Text.Trim()))
        {
            Function.AlertMsg("请填写报修人"); return;
        }
        if (txtReporter.Text.Trim().Length > 50)
        {
            Function.AlertMsg("报修人不能多于50字"); return;
        }
        if (TxbSLA2.Text.Trim().Length > 50)
        {
            Function.AlertMsg("扩展SLA不能超50字"); return;
        }
        if (Function.ConverToInt(TxbSLA.Text.Trim()) < 0)
        {
            Function.AlertMsg("SLA需要是一个正整数"); return;
        }

        #endregion
        CallInfo info = new CallInfo();

        info.CreatorID = CurrentUserID;
        info.CreatorName = CurrentUserName;
        info.CreateDate = DateTime.Now;
        info.No = CallBLL.GetCallNoNew();
        info.ErrorReportUser = txtReporter.Text.Trim();
        info.CustomerID = sinfo.CustomerID;
        info.CustomerName = sinfo.CustomerName;
        info.BrandID = sinfo.BrandID;
        info.BrandName = sinfo.BrandName;
        info.ProvinceID = sinfo.ProvinceID;
        info.ProvinceName = sinfo.ProvinceName;
        info.CityID = sinfo.CityID;
        info.CityName = sinfo.CityName;
        info.StoreID = sinfo.ID;
        info.StoreName = sinfo.No;//赋值赋反了？？？？？？？？？？？？？
        info.StoreNo = sinfo.Name;//赋值赋反了？？？？？？？？？？？？？
        info.ReportSourceID = Function.ConverToInt(ddlReportSource.SelectedValue);
        info.ReportSourceName = ddlReportSource.SelectedItem.Text;
        info.ReportSourceNo = txtSourceNo.Text.Trim();
        info.ReporterName = txtReporter.Text.Trim();
        info.ErrorDate = Function.ConverToDateTime(TxtErrorDate.Text);
        info.Class1 = c1info.ID;
        info.Class2 = c2info.ID;
        info.Class3 = c3info.ID;
        info.ClassName1 = c1info.Name;
        info.ClassName2 = c2info.Name;
        info.ClassName3 = c3info.Name;
        info.PriorityID = c3info.PriorityID;
        info.PriorityName = c3info.PriorityName;
        info.Details = TxtDetails.Text.Trim();
        info.MaintainUserID = uinfo.ID;
        info.MaintaimUserName = uinfo.Name;
        info.StateMain = (int)SysEnum.CallStateMain.未处理;
        info.StateDetail = (int)SysEnum.CallStateDetails.系统接单_未处理;
        info.SuggestSlnID = 0;
        info.SuggestSlnName = "";
        info.SlnID = 0;
        info.SlnName = "";
        info.SLA = Function.ConverToInt(TxbSLA.Text.Trim(), 0);
        info.SloveBy = "";
        info.WorkGroupID = uinfo.WorkGroupID;
        info.AssignID = 0;
        info.AssignUserID = 0;
        info.AssignUserName = "";
        info.IsClosed = false;
        info.Category = Function.ConverToInt(DdlCategory.SelectedValue, 0);
        

        #region 扩展信息
        string VideoID = Function.GetRequestSrtring("CallID");//这是一个录音id
        info.VideoID = VideoID;
        info.CallNo2 = string.Empty;
        info.CallNo3 = string.Empty;
        info.VideoSrc = string.Empty;
        info.FinishDate = Function.ErrorDate;
        info.SLA2 = TxbSLA2.Text.Trim();

        try
        {
            if (!string.IsNullOrEmpty(VideoID))
            {
                RecordInfo recinfo = RecordBLL.Get(VideoID);
                if (null != recinfo)
                {
                    info.VideoSrc = recinfo.filepath + recinfo.recordname;
                }
            }
        }
        catch (Exception) { }//这里是防别人乱打个VideoID过来
        #endregion
        if (null != ViewState["suggestSln"])
        {
            SolutionInfo slninfo = SolutionBLL.Get((int)ViewState["suggestSln"]);
            if (null != slninfo && slninfo.Class2 == info.Class2)//第二个判断条件很重要。好彩我够精
            {
                info.SuggestSlnID = slninfo.ID;
                info.SuggestSlnName = slninfo.Name;
            }
        }


        string js = "";
        info.ID = CallBLL.Add(info);
        #region ZQL新增插入callstep一条记录
        if (info.ID > 0)
        {
            CallStepInfo callStepInfo = new CallStepInfo();
            callStepInfo.StepType = (int)SysEnum.StepType.开单;
            callStepInfo.AddDate = DateTime.Now;
            callStepInfo.CallID = info.ID;
            callStepInfo.DateBegin = DateTime.Now;
            callStepInfo.DateEnd = DateTime.Now;
            callStepInfo.Details = "";
            callStepInfo.IsSolved = false;
            callStepInfo.MajorUserID = uinfo.ID;
            callStepInfo.MajorUserName = uinfo.Name;
            callStepInfo.SolutionID = 0;
            callStepInfo.SolutionName = "";
            callStepInfo.StepIndex = CallStepBLL.GetMaxStepIndex(info.ID) + 1;
            callStepInfo.UserID = CurrentUserID;
            callStepInfo.UserName = CurrentUserName; 
            callStepInfo.IsSolved = false;
            callStepInfo.StepName = SysEnum.CallStateDetails.系统接单_未处理.ToString();
            CallStepBLL.Add(callStepInfo);
        }

        #endregion
        if (info.ID > 0)
        {

            string APImsg = CallStepActionBLL.StepNewCall(info);
            if (APImsg!=null)
            {
                if (APImsg==string.Empty)
                {
                    APImsg += "邮件发送成功";
                }
                else
                {
                    APImsg = "邮件发送失败" + APImsg;
                }
            }
            js += EditCustomerRequestInfo(info.ID);
            js = string.Format("alert('{0}');location.href=this.location.href;", "成功交给了技术中心！" + APImsg);

            #region 判断是汉堡王时调用接口
            if (info.BrandName.Trim() == "汉堡王" || info.CustomerName.Trim() == "汉堡王")
            {
                string url = "http://helpdesk.bkchina.cn/siweb/ws_hesheng.ashx?";
                //string url = "http://192.168.1.112:8088/BurgerKing/BurgerKingCall.aspx?";
                KeyValueDictionary paramDic = new KeyValueDictionary();
                paramDic.Add("Action", "新建");
                paramDic.Add("cNumber",info.No);
                paramDic.Add("Supplier","MVS");
                paramDic.Add("Agent", info.CreatorName);
                paramDic.Add("stCode", info.StoreName);//由于addcall的时候calls表storeNO和StoreName赋值赋反了
                paramDic.Add("stMgr", info.ReporterName);
                paramDic.Add("Time1", info.ErrorDate); 
                paramDic.Add("Issue", info.Details);
                paramDic.Add("Priority", info.PriorityName);
                paramDic.Add("Category1", info.ClassName1);
                paramDic.Add("Category2", info.ClassName2);
                paramDic.Add("Category3", info.ClassName3);
                paramDic.Add("Solution","开案");
                paramDic.Add("Attachment", "");

                //WebUtil webtool = new WebUtil();
                string paramStr = WebUtil.BuildQueryJson(paramDic);
                //string result = webtool.DoPost(url, paramDic);
                //JObject obj = JObject.Parse(result);
                //string errNo = obj["errNo"].ToString();
                string Description = string.Empty;
                //if (errNo == "0")
                //{
                //    Description = "接口调用成功";
                //    //记日志
                //    LogInfo logInfo = new LogInfo();
                //    logInfo.AddDate = DateTime.Now;
                //    logInfo.Category = Enum.GetName(typeof(SysEnum.LogType), SysEnum.LogType.普通日志);
                //    logInfo.Content = "汉堡王开案接口调用成功C_NO=" + info.No;
                //    logInfo.ErrorDate = Tool.Function.ErrorDate;
                //    logInfo.SendEmail = true;
                //    logInfo.UserName = DicInfo.Admin;
                //    logInfo.Serious = 1;
                //    LogBLL.Add(logInfo);

                //}
                //else {
                //    Description = "接口调用失败"+obj["Desc"].ToString();
                //    //记日志
                //    LogInfo logInfo = new LogInfo();
                //    logInfo.AddDate = DateTime.Now;
                //    logInfo.Category = Enum.GetName(typeof(SysEnum.LogType), SysEnum.LogType.普通日志);
                //    logInfo.Content = "汉堡王开案接口调用失败C_NO=" + info.No + Description;
                //    logInfo.ErrorDate = Tool.Function.ErrorDate;
                //    logInfo.SendEmail = true;
                //    logInfo.UserName = DicInfo.Admin;
                //    logInfo.Serious = 1;
                //    LogBLL.Add(logInfo);
                //}
                
                string sqlStrHK = "INSERT INTO sys_WebServiceTask VALUES ('" + paramStr + "',0," + info.CustomerID.ToString() + "," + info.BrandID.ToString() + ");";
                int records = CallBLL.AddBurgerKingTask(sqlStrHK);
                if (records <= 0)
                    Description = " 汉堡王任务记录失败，请联系管理员";
                js = string.Format("alert('{0}');location.href=this.location.href;", "成功交给了技术中心！" + Description);
            }
            #endregion
        }
        else
        {
            js = "alert('提交失败，请联系管理员！');";
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", js, true);
        return;
    }

    protected void BtnStore_Click(object sender, EventArgs e)
    {
        int ID = 0;
        StoreInfo sinfo = new StoreInfo();
        if (string.IsNullOrEmpty(TxtStoreNo.Text.Trim()))
        {
            Function.AlertMsg("店铺编号信息有误");
            return;
        }
        ID = Function.ConverToInt(TxtStoreNo.Text.Trim());
        if (ID > 0)
        {
            sinfo = StoresBLL.Get(ID);
            if (null == sinfo)
            {
                Function.AlertMsg("店铺编号信息有误");
                return;
            }
        }
        else
        {
            int IndexStart = TxtStoreNo.Text.IndexOf("<") + 4;
            int IndexEnd = TxtStoreNo.Text.IndexOf(">");
            if (IndexStart - 4 > 0 && IndexEnd > IndexStart)
            {
                ID = Function.ConverToInt(TxtStoreNo.Text.Substring(IndexStart, IndexEnd - IndexStart));
                if (ID <= 0)
                {
                    Function.AlertMsg("店铺编号信息有误");
                    return;
                }
                sinfo = StoresBLL.Get(ID);
                if (null == sinfo)
                {
                    Function.AlertMsg("店铺编号信息有误");
                    return;
                }
            }
            else
            {
                Function.AlertMsg("店铺编号信息有误");
                return;
            }
        }
        CityInfo cinfo = CityBLL.Get(sinfo.CityID);
        ProvincesInfo pinfo = ProvincesBLL.Get(sinfo.ProvinceID);
        BrandInfo binfo = BrandBLL.Get(sinfo.BrandID);
        if (binfo == null) return;
        CustomersInfo cusinfo = CustomersBLL.Get(binfo.CustomerID);
        if (cinfo == null || pinfo == null || binfo == null || cusinfo == null)
        {
            return;
        }
        ddlCustomer.SelectedValue = binfo.CustomerID.ToString();

        ddlBrand.DataSource = BrandBLL.GetList(binfo.CustomerID);
        ddlBrand.DataBind();
        ddlBrand.SelectedValue = binfo.ID.ToString();
        BindL2(binfo.ID);
        SetKnowledgebaseLink();

        ddlProvince.DataSource = ProvincesBLL.GetList();
        ddlProvince.DataBind();
        ddlProvince.SelectedValue = pinfo.ID.ToString();

        ddlCity.DataSource = CityBLL.GetList(pinfo.ID);
        ddlCity.DataBind();
        ddlCity.SelectedValue = cinfo.ID.ToString();

        ddlStore.DataSource = StoresBLL.GetList(cinfo.ID, binfo.ID);
        ddlStore.DataBind();
        ddlStore.SelectedValue = sinfo.ID.ToString();
        ddlStore_SelectedIndexChanged(sender, e);


        ddlClass1.DataSource = Class1BLL.GetList(cusinfo.ID);
        ddlClass1.DataBind();
        ddlClass1.Items.Insert(0, new ListItem("请选择", "0"));



        BindHistoryCall(sinfo.ID);
    }


    private void BindHistoryCall(int StoreID)
    {
        ListRec1.StoreID = StoreID;
        ListRec1.Update(StoreID);

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        int ID = Function.ConverToInt(lb.CommandArgument);
        lb.CssClass = "Focus";
        ViewState["suggestSln"] = ID;
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "", "FucusSln('" + lb.ToolTip + "');", true);
    }


    private CustomerRequestInfo GetCustomerRequestInfo()
    {
        //CustomerRequestID
        CustomerRequestInfo info = null;
        if (ViewState["CustomerRequestInfo"] != null)
        {
            return (CustomerRequestInfo)ViewState["CustomerRequestInfo"];
        }
        int CustomerRequestID = Function.GetRequestInt("CustomerRequestID");
        if (CustomerRequestID > 0)
        {
            info = CustomerRequestBLL.Get(CustomerRequestID);
            if (info.CallID > 0)
            {
                return null;
            }
            ViewState["CustomerRequestInfo"] = info;
        }
        return info;
    }
    private string EditCustomerRequestInfo(int CallID)
    {
        CustomerRequestInfo crinfo = GetCustomerRequestInfo();
        if (null == crinfo || !crinfo.Enable)
        {
            return string.Empty;
        }
        crinfo.CallID = CallID;
        if (!CustomerRequestBLL.Edit(crinfo))
        {
            return "系统没有更新对应的客户报修请求。";
        }
        return string.Empty;
    }
    protected void TxbL2Name_TextChanged(object sender, EventArgs e)
    {
        TxbL2Name.Text = TxbL2Name.Text.Trim().ToLower();
        if (null == ViewState["L2List"])
        {
            return;
        }
        List<UserInfo> list = (List<UserInfo>)ViewState["L2List"];
        List<UserInfo> listResult = new List<UserInfo>();
        if (string.IsNullOrEmpty(TxbL2Name.Text.Trim()))
        {
            listResult = list;
        }
        else
        {
            foreach (UserInfo item in list)
            {
                if (item.Name.ToLower().Contains(TxbL2Name.Text))
                {
                    listResult.Add(item);
                }
            }
        }
        ddlL2Id.DataSource = listResult;
        ddlL2Id.DataBind();
        ddlL2Id.Items.Insert(0, new ListItem("不限", "0"));

    }

    /// <summary>
    /// 设置品牌变动后相关的知识库
    /// </summary>
    private void SetKnowledgebaseLink()
    {
        int ID = Convert.ToInt16(ddlBrand.SelectedValue);
        if (ID <= 0)
        {
            LtlBrandKnowledgeBase.Text = string.Empty;
        }
        else
        {
            LtlBrandKnowledgeBase.Text = string.Format("<a href='/page/KnowledgeBase/list.aspx?CustomerID={0}&BrandID={1}' target='_black' >查看相关知识库</a>", ddlCustomer.SelectedValue, ID);
        }
    }

}
