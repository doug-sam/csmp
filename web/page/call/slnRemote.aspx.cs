using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using Newtonsoft.Json.Linq;

public partial class page_call_slnRemote : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.升级到客户,
        (int)SysEnum.CallStateDetails.二线离场确认,
        (int)SysEnum.CallStateDetails.上门取消,
        (int)SysEnum.CallStateDetails.开始处理,
        (int)SysEnum.CallStateDetails.电话支持,
         (int)SysEnum.CallStateDetails.第三方预约取消,
       (int)SysEnum.CallStateDetails.第三方处理离场
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            CallState1.CallID = info.ID;
            if (info.StateMain == (int)SysEnum.CallStateMain.已完成)
            {
                Function.AlertMsg("数据有误，无法处理已完成的call");
                return;
            }
            #region 状态跳转
            if (!RightState.Contains(info.StateDetail))
            {
                Function.AlertBack("数据状态有误");
                return;
            }
            #endregion


            TxbDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            DdlSln.DataSource = SolutionBLL.GetList(info.Class3);
            DdlSln.DataBind();
            DdlSln.Items.Insert(0, new ListItem("其它", "0"));
            DdlSln.Items.Insert(0, new ListItem("请选择", "-1"));

            #region 用户检查
            NotMyCallCheck(info.ID);
            #endregion


        }
    }

    private CallInfo GetInfo()
    {
        if (ViewState["info"] != null)
        {
            return (CallInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("id");
        if (ID <= 0)
        {
            return null;
        }
        CallInfo info = CallBLL.Get(ID);
        if (null == info)
        {
            return null;
        }
        ViewState["info"] = info;
        return info;
    }


    protected void RblSolved_SelectedIndexChanged(object sender, EventArgs e)
    {
        TrSln.Visible = true;
        LtlSln.Text = RblSolved.SelectedValue == "1" ? "" : "建议";
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(RblSolved.SelectedValue))
        {
            Function.AlertMsg("问题是否已经解决了？"); return;
        }

        if (RblSolved.SelectedValue == "1" && DdlSln.SelectedValue == "-1")
        {
            Function.AlertMsg("亲，既然解决了，请选择解决方案"); return;
        }

        if (DdlSln.SelectedValue == "0" && string.IsNullOrEmpty(TxbSln.Text.Trim()))
        {
            Function.AlertMsg("请输入解决方案"); return;
        }

        CallInfo cinfo = GetInfo();
        if (!RightState.Contains(cinfo.StateDetail))
        {
            Function.AlertMsg("数据失效，请刷新");
            return;
        }


        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.远程支持;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName =CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.DateBegin = Function.ConverToDateTime(TxbDate.Text.Trim());
        sinfo.DateEnd = sinfo.DateBegin;
        sinfo.Details = TxbDetails.Text.Trim();
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.IsSolved = RblSolved.SelectedValue == "1";
        sinfo.StepName = "由" + CurrentUserName + "进行" + SysEnum.CallStateDetails.电话支持.ToString();
        sinfo.UserID = CurrentUser.ID;
        sinfo.UserName = CurrentUser.Name;
        sinfo.IsSolved = RblSolved.SelectedValue == "1";
        #region 获得解决方案
        SolutionInfo slninfo = new SolutionInfo();
        if (Function.ConverToInt(DdlSln.SelectedValue) > 0)
        {
            slninfo = SolutionBLL.Get(Function.ConverToInt(DdlSln.SelectedValue));
        }
        if (TxbSln.Text.Trim().Length > 0)
        {
            slninfo = SolutionBLL.Get(TxbSln.Text.Trim(), cinfo.Class3);
            if (null == slninfo || slninfo.ID <= 0)
            {
                #region 添加解决方案
                slninfo = new SolutionInfo();
                slninfo.Class1 = cinfo.Class1;
                slninfo.Class1Name = cinfo.ClassName1;
                slninfo.Class2 = cinfo.Class2;
                slninfo.Class2Name = cinfo.ClassName2;
                slninfo.Class3 = cinfo.Class3;
                slninfo.Class3Name = cinfo.ClassName3;
                slninfo.EnableBy = "";
                slninfo.EnableFlag = true;
                slninfo.Name = TxbSln.Text.Trim();
                slninfo.SolveCount = RblSolved.SelectedValue == "1"?1:0;
                slninfo.SuggestCount = 1;
                int SID = SolutionBLL.Add(slninfo);
                if (SID <= 0)
                {
                    Function.AlertMsg("系统出错，请联系管理员"); return;
                }
                slninfo.ID = SID;
                #endregion
            }
        } 
        #endregion
        sinfo.SolutionID = slninfo.ID;
        sinfo.SolutionName =string.IsNullOrEmpty(slninfo.Name)?"": slninfo.Name;

        if (RblSolved.SelectedValue == "1")
        {
            sinfo.IsSolved = true;
            cinfo.StateMain = (int)SysEnum.CallStateMain.已完成;
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.处理完成;
            cinfo.SloveBy = SysEnum.SolvedBy.远程支持.ToString();
            cinfo.FinishDate = sinfo.DateBegin;
        }
        else
        {
            sinfo.IsSolved = false;
            cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.电话支持;
        }



        if (sinfo.DateBegin == Tool.Function.ErrorDate)
        {
            Function.AlertMsg("实际远程日期有误"); return;
        }
        if (sinfo.DateBegin > DateTime.Now)
        {
            Function.AlertMsg("实际操作日期不能大于当前时间");
            return;
        }
        if (sinfo.Details.Length > 500)
        {
            Function.AlertMsg("处理过程备注不能超过500字"); return;
        }
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {string APImsg=string.Empty;
            if ((int)SysEnum.CallStateMain.已完成 == cinfo.StateMain)
            {
               APImsg = CallStepActionBLL.StepSloved(cinfo, sinfo);
                if (APImsg != null)
                {
                    if (APImsg == string.Empty)
                    {
                        APImsg += "《已完成》邮件发送成功";
                    }
                    else
                    {
                        APImsg = "《已完成》邮件发送失败" + APImsg;
                    }
                    APImsg = APImsg.Replace("\"", " ").Replace("'", " ");
                }
            }

            #region 汉堡王升级到客户处理完成时
            if ((int)SysEnum.CallStateMain.已完成 == cinfo.StateMain&&(cinfo.BrandName == "汉堡王" || cinfo.CustomerName == "汉堡王"))
            {
                KeyValueDictionary paramDic = new KeyValueDictionary();
                paramDic.Add("Action", "完成HD");
                paramDic.Add("cNumber", cinfo.No);
                paramDic.Add("Supplier", "MVSHD");
                paramDic.Add("Agent", sinfo.MajorUserName);
                paramDic.Add("stMgr", cinfo.ReporterName);
                paramDic.Add("Priority", cinfo.PriorityName);
                paramDic.Add("Category1", cinfo.ClassName1);
                paramDic.Add("Category2", cinfo.ClassName2);
                paramDic.Add("Category3", cinfo.ClassName3);
                paramDic.Add("Solution", sinfo.Details);
                paramDic.Add("Attachment", "");
                string paramStr = WebUtil.BuildQueryJson(paramDic);
                //string sqlStrHK = "INSERT INTO sys_WebServiceTask VALUES ('" + paramStr + "',0," + cinfo.CustomerID.ToString() + "," + cinfo.BrandID.ToString() + ");";
                //int records = CallBLL.AddBurgerKingTask(sqlStrHK);
                WebServiceTaskInfo bkWebSvrTask = new WebServiceTaskInfo();
                bkWebSvrTask.CallNo = cinfo.No;
                bkWebSvrTask.TaskUrl = paramStr;
                bkWebSvrTask.CustomerID = cinfo.CustomerID;
                bkWebSvrTask.CustomerName = cinfo.CustomerName;
                bkWebSvrTask.BrandID = cinfo.BrandID;
                bkWebSvrTask.BrandName = cinfo.BrandName;
                bkWebSvrTask.IsDone = false;
                bkWebSvrTask.Remark = string.Empty;
                Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask开始 动作：完成（远程），参数信息：" + paramStr + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                if (WebServiceTaskBLL.Add(bkWebSvrTask) > 0)
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask成功 动作：完成（远程）" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }
                else
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask失败 动作：完成（远程）" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }

                #region 远程支持完成调用 关闭HD
                KeyValueDictionary paramCloseDic = new KeyValueDictionary();
                paramCloseDic.Add("Action", "关闭HD");
                paramCloseDic.Add("cNumber", cinfo.No);
                paramCloseDic.Add("Supplier", "MVSHD");
                paramCloseDic.Add("Agent", sinfo.UserName);
                paramCloseDic.Add("TSI", "MVSL2");
                paramCloseDic.Add("Engineer", sinfo.MajorUserName);
                paramCloseDic.Add("stMgr", cinfo.ReporterName);
                paramCloseDic.Add("Solution", sinfo.Details);
                paramCloseDic.Add("Attachment", "");

                string paramCloseStr = WebUtil.BuildQueryJson(paramCloseDic);
                //string sqlStrHK = "INSERT INTO sys_WebServiceTask VALUES ('" + paramStr + "',0," + info.CustomerID.ToString() + "," + info.BrandID.ToString() + ");";
                //int records = CallBLL.AddBurgerKingTask(sqlStrHK);
                WebServiceTaskInfo bkWebSvrCloseTask = new WebServiceTaskInfo();
                bkWebSvrCloseTask.CallNo = cinfo.No;
                bkWebSvrCloseTask.TaskUrl = paramCloseStr;
                bkWebSvrCloseTask.CustomerID = cinfo.CustomerID;
                bkWebSvrCloseTask.CustomerName = cinfo.CustomerName;
                bkWebSvrCloseTask.BrandID = cinfo.BrandID;
                bkWebSvrCloseTask.BrandName = cinfo.BrandName;
                bkWebSvrCloseTask.IsDone = false;
                bkWebSvrCloseTask.Remark = string.Empty;
                Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask开始 动作：关闭(远程支持)，参数信息：" + paramCloseStr + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                if (WebServiceTaskBLL.Add(bkWebSvrCloseTask) > 0)
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask成功 动作：关闭(远程支持)" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }
                else
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask失败 动作：关闭(远程支持)" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }
                #endregion
            }
            #endregion

            #region 当已完成时删除跑马灯表中的记录 2015.11.18 ZQL加
            MarqueeMessageBLL.Delete(cinfo.No);
            #endregion

            string js = "top.ReloadLeft();alert('成功记录" + APImsg + "');location.href='";
            switch (cinfo.StateMain)
            {
                case (int)SysEnum.CallStateMain.处理中:
                    js += "sln.aspx?id=" + cinfo.ID;
                    break;
                case (int)SysEnum.CallStateMain.已完成:
                    js += "list.aspx?state=" + (int)SysEnum.CallStateMain.已完成;
                    break;

                default:
                    break;
            }
            js += "';";
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", js, true);
        }
        else
        {
            Function.AlertMsg("提交失败。请联系管理员"); return;
        }
        BtnSubmit.Visible = false;

    }

    protected void DdlSln_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxbSln.Visible = DdlSln.SelectedValue == "0";
    }
}
