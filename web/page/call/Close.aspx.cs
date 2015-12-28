using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_call_Close : _Call_Close
{
    public CallInfo info;
    protected void Page_Load(object sender, EventArgs e)
    {
        GetInfo();
        NotMyCallCheck(info.ID);
        if (!IsPostBack)
        {
            if (info.StateMain != (int)SysEnum.CallStateMain.已完成)
            {
                Function.AlertBack("该call状态不对。你是怎么进来的？！。");
            }

            TxbNo2.Text = info.No;

            StoreInfo sinfo = StoresBLL.Get(info.StoreID);
            if (null == sinfo)
            {
                Response.End();
            }
            LtlStoreNo.Text = sinfo.No;
            LtlTel.Text = sinfo.Tel;
            LtlAddress.Text = sinfo.Address;

            Class3Info c3info = Class3BLL.Get(info.Class3);
            if (null != c3info)
            {
                LabSLA.Text = c3info.SLA.ToString() + "Hour";
            }

            CallStepInfo stepBill = CallStepBLL.GetLast(info.ID, SysEnum.StepType.回收服务单);
            if (stepBill != null )
            {
                TxbNo2.Visible = false;
                CbClose.Checked = true;
                CbClose.Enabled = false;
            }

        }

    }

    private void GetInfo()
    {
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            Response.End();
        }
        info = CallBLL.Get(ID);
        if (null == info)
        {
            Response.End();
        }
    }

    private string Call_ReciveBill(CallInfo info, string CallNo2)
    {
        CallNo2 = CallNo2.Trim();
        if (null == info)
        {
            return "无法获取数据";
        }
        if (CallNo2.Length > 100)
        {
            return "服务单号不应超过100个字";
        }


        //info.StateMain = (int)SysEnum.CallStateMain.已关闭;
        info.StateDetail = (int)SysEnum.CallStateDetails.已回收服务单;
        info.CallNo2 = CallNo2;


        CallStepInfo sinfo = new CallStepInfo();
        sinfo.CallID = info.ID;
        sinfo.AddDate = DateTime.Now;
        sinfo.DateBegin = sinfo.DateEnd = DateTime.Now;
        sinfo.Details = string.Format("由{0}回收服务单号:{1}", CurrentUserName, TxbNo2.Text.Trim());
        sinfo.IsSolved = false;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = string.Empty;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(info.ID) + 1;
        sinfo.StepName = SysEnum.StepType.回收服务单.ToString();
        sinfo.StepType = (int)SysEnum.StepType.回收服务单;
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        if (CallStepBLL.AddCallStep_UpdateCall(info, sinfo))
        {
            return string.Empty;
        }
        return "系统已接收服务单，但无法更新状态。这可能由网络原因引起的，请稍候再试。";

    }

    private bool Call_Close(CallInfo info)
    {
        info.StateMain = (int)SysEnum.CallStateMain.已关闭;

        CallStepInfo sinfo = new CallStepInfo();
        sinfo.CallID = info.ID;
        sinfo.AddDate = DateTime.Now;
        sinfo.DateBegin = sinfo.DateEnd = DateTime.Now;
        sinfo.Details = string.Format("由{0}关call", CurrentUserName);
        sinfo.IsSolved = false;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = string.Empty;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(info.ID) + 1;
        sinfo.StepName = SysEnum.StepType.关单.ToString();
        sinfo.StepType = (int)SysEnum.StepType.关单;
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        bool Result = CallStepBLL.AddCallStep_UpdateCall(info, sinfo);

        
        #region 判断是汉堡王时插入“关闭”报文
        //if (info.BrandName == "汉堡王" || info.CustomerName == "汉堡王")
        //{
        //    //判断是否为上门完成（合胜自己的工程师或者合胜自己的第三方工程师）
        //    if (info.SloveBy.Trim() == SysEnum.SolvedBy.上门.ToString().Trim())
        //    {
        //        #region 上门完成调用 关闭
        //        KeyValueDictionary paramDic = new KeyValueDictionary();
        //        paramDic.Add("Action", "关闭");
        //        paramDic.Add("cNumber", info.No);
        //        paramDic.Add("Supplier", "MVSHD");
        //        paramDic.Add("Agent", sinfo.UserName);
        //        paramDic.Add("TSI", "MVSL2");
        //        paramDic.Add("Engineer", sinfo.MajorUserName);
        //        paramDic.Add("stMgr", info.ReporterName);
        //        paramDic.Add("Solution", sinfo.Details);
        //        paramDic.Add("Attachment", "");

        //        string paramStr = WebUtil.BuildQueryJson(paramDic);
        //        //string sqlStrHK = "INSERT INTO sys_WebServiceTask VALUES ('" + paramStr + "',0," + info.CustomerID.ToString() + "," + info.BrandID.ToString() + ");";
        //        //int records = CallBLL.AddBurgerKingTask(sqlStrHK);
        //        WebServiceTaskInfo bkWebSvrTask = new WebServiceTaskInfo();
        //        bkWebSvrTask.CallNo = info.No;
        //        bkWebSvrTask.TaskUrl = paramStr;
        //        bkWebSvrTask.CustomerID = info.CustomerID;
        //        bkWebSvrTask.CustomerName = info.CustomerName;
        //        bkWebSvrTask.BrandID = info.BrandID;
        //        bkWebSvrTask.BrandName = info.BrandName;
        //        bkWebSvrTask.IsDone = false;
        //        bkWebSvrTask.Remark = string.Empty;
        //        Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask开始 动作：关闭，参数信息：" + paramStr + "，callid=" + info.ID + "，CustomerName:" + info.CustomerName + "，BrandName:" + info.BrandName + "，操作人：" + CurrentUserName, null);
        //        if (WebServiceTaskBLL.Add(bkWebSvrTask) > 0)
        //        {
        //            Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask成功 动作：关闭" + "，callid=" + info.ID + "，CustomerName:" + info.CustomerName + "，BrandName:" + info.BrandName + "，操作人：" + CurrentUserName, null);
        //        }
        //        else
        //        {
        //            Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask失败 动作：关闭" + "，callid=" + info.ID + "，CustomerName:" + info.CustomerName + "，BrandName:" + info.BrandName + "，操作人：" + CurrentUserName, null);
        //        }
        //        #endregion
        //    }
        //    //判断是否为远程支持完成
        //    else if (info.SloveBy.Trim() == SysEnum.SolvedBy.远程支持.ToString().Trim())
        //    {
        //        #region 远程支持完成调用 关闭HD
        //        KeyValueDictionary paramDic = new KeyValueDictionary();
        //        paramDic.Add("Action", "关闭HD");
        //        paramDic.Add("cNumber", info.No);
        //        paramDic.Add("Supplier", "MVSHD");
        //        paramDic.Add("Agent", sinfo.UserName);
        //        paramDic.Add("TSI", "MVSL2");
        //        paramDic.Add("Engineer", sinfo.MajorUserName);
        //        paramDic.Add("stMgr", info.ReporterName);
        //        paramDic.Add("Solution", sinfo.Details);
        //        paramDic.Add("Attachment", "");

        //        string paramStr = WebUtil.BuildQueryJson(paramDic);
        //        //string sqlStrHK = "INSERT INTO sys_WebServiceTask VALUES ('" + paramStr + "',0," + info.CustomerID.ToString() + "," + info.BrandID.ToString() + ");";
        //        //int records = CallBLL.AddBurgerKingTask(sqlStrHK);
        //        WebServiceTaskInfo bkWebSvrTask = new WebServiceTaskInfo();
        //        bkWebSvrTask.CallNo = info.No;
        //        bkWebSvrTask.TaskUrl = paramStr;
        //        bkWebSvrTask.CustomerID = info.CustomerID;
        //        bkWebSvrTask.CustomerName = info.CustomerName;
        //        bkWebSvrTask.BrandID = info.BrandID;
        //        bkWebSvrTask.BrandName = info.BrandName;
        //        bkWebSvrTask.IsDone = false;
        //        bkWebSvrTask.Remark = string.Empty;
        //        Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask开始 动作：关闭(远程支持)，参数信息：" + paramStr + "，callid=" + info.ID + "，CustomerName:" + info.CustomerName + "，BrandName:" + info.BrandName + "，操作人：" + CurrentUserName, null);
        //        if (WebServiceTaskBLL.Add(bkWebSvrTask) > 0)
        //        {
        //            Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask成功 动作：关闭(远程支持)" + "，callid=" + info.ID + "，CustomerName:" + info.CustomerName + "，BrandName:" + info.BrandName + "，操作人：" + CurrentUserName, null);
        //        }
        //        else
        //        {
        //            Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask失败 动作：关闭(远程支持)" + "，callid=" + info.ID + "，CustomerName:" + info.CustomerName + "，BrandName:" + info.BrandName + "，操作人：" + CurrentUserName, null);
        //        }

        //        #endregion

        //    }
        //    //判断是否为第三方完成（转呈给BK的）
        //    else if (info.SloveBy.Trim() == SysEnum.SolvedBy.第三方.ToString().Trim())
        //    {
        //        Logger.GetLogger(this.GetType()).Info("判断为第三方完成，不再调用汉堡王关闭接口" + "，callid=" + info.ID + "，CustomerName:" + info.CustomerName + "，BrandName:" + info.BrandName + "，操作人：" + CurrentUserName, null);
        //    }

        //}
        #endregion

        #region 读写缓存
        try
        {
            List<LeftMenuData> list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
            if (list == null || list.Count <= 0)
            {
                
                LeftMenuDataBLL.InsertLeftMenuDataCache();
                list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
                Logger.GetLogger(this.GetType()).Info("关闭工单时缓存数据为空，从数据库中同步一次数据到缓存中。\r\n", null);
            }
            //UserInfo currentUser = UserBLL.Get(845);
            int index = list.FindIndex(s => s.UserID == CurrentUserID);
            list[index].Closed += 1;
            //list[index].Complete -= 1;
            CacheManage.InsertCache("leftMenuKey", list);
            Logger.GetLogger(this.GetType()).Info("关闭工单时更新缓存中对应的close数成功，操作人"+CurrentUserName+"。\r\n", null);
        }
        catch(Exception ex) {
            Logger.GetLogger(this.GetType()).Info("关闭工单时更新缓存中对应的close数失败，失败原因："+ex.Message+"\r\n", null);
        }
        #endregion

        return Result;

    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (null == info)
        {
            GetInfo();
        }
        if (CbClose.Checked)
        {
            if (info.ReplacementStatus == (int)SysEnum.ReplacementStatus.备件跟进中)
            {
                Function.AlertMsg("备件状态有误");
                return;
            }
        }
        string Result = string.Empty;
        if (TxbNo2.Visible)
        {
            Result = Call_ReciveBill(info, TxbNo2.Text);
            if (!string.IsNullOrEmpty(Result))
            {
                Function.AlertMsg(Result);
            }
        }

        if (CbClose.Checked)
        {
            if (!Call_Close(info))
            {
                Function.AlertMsg("报修关闭失败了，请重试");
                return;
            }
        }

        Function.AlertRefresh("操作成功", "main");
        return;
    }
}
