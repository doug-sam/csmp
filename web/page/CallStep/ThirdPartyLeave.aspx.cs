using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Collections;
using System.Net.Mail;
using EMPPLib;
using System.Threading;
using Newtonsoft.Json.Linq;


public partial class page_CallStep_ThirdParty : _Call_Sln1
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.第三方预约上门
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        CallInfo info = GetInfo();
        if (null == info)
        {
            Function.AlertBack("数据有误。");
        }
        CallState1.CallID = info.ID;
        if (info.StateMain == (int)SysEnum.CallStateMain.已完成)
        {
            Function.AlertBack("数据有误，无法处理已完成的call");
            return;
        }

        #region 状态跳转
        if (!RightState.Contains(info.StateDetail))
        {
            Function.AlertBack("数据状态有误");
            return;
        }
        #endregion

        CallStepInfo stepDateinfo = CallStepBLL.GetLast(info.ID, SysEnum.StepType.第三方预约上门);
        //TxbDateArrive.Text = stepDateinfo.DateBegin.ToString("yyyy-MM-dd HH:mm");
        TxbDateLeave.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        #region 用户检查
        NotMyCallCheck(info.ID);
        #endregion

    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(RblSolved.SelectedValue))
        {
            Function.AlertBack("问题是否已经解决了？");
        }

        CallInfo cinfo = GetInfo();
        if (!RightState.Contains(cinfo.StateDetail))
        {
            Function.AlertMsg("数据失效，请刷新");
            return;
        }

        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.第三方处理离场;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.DateBegin = Function.ConverToDateTime(TxbDateLeave.Text.Trim()); //Function.ConverToDateTime(TxbDateArrive.Text.Trim());
        sinfo.DateEnd = sinfo.DateBegin;
        sinfo.Details = TxbDetail.Text.Trim();
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        if (RblSolved.SelectedValue == "1")
        {
            sinfo.IsSolved = true;
            sinfo.SolutionID = -2;
            sinfo.SolutionName = "第三方解决";
            cinfo.StateMain = (int)SysEnum.CallStateMain.已完成;
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.处理完成;
            cinfo.SloveBy = SysEnum.SolvedBy.第三方.ToString();
            cinfo.FinishDate = sinfo.DateBegin;
        }
        else
        {
            sinfo.IsSolved = false;
            sinfo.SolutionID = 0;
            sinfo.SolutionName = "";
            cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.第三方处理离场;
        }
        sinfo.StepName = SysEnum.CallStateDetails.第三方处理离场.ToString();
        sinfo.UserID = CurrentUser.ID;
        sinfo.UserName = CurrentUser.Name;

        if (sinfo.DateBegin == Tool.Function.ErrorDate)
        {
            Function.AlertBack("到达日期有误");
        }
        if (sinfo.DateEnd == Tool.Function.ErrorDate)
        {
            Function.AlertBack("离开日期有误");
        }
        if (sinfo.Details.Length > 500)
        {
            Function.AlertBack("处理过程备注不能超过500字");
        }


        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            string APImsg = string.Empty;
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
            #region 汉堡王升级到客户处理完成原代码
            //if ((int)SysEnum.CallStateMain.已完成 == cinfo.StateMain && (cinfo.BrandName == "汉堡王" || cinfo.CustomerName == "汉堡王"))
            //{
            //    KeyValueDictionary paramDic = new KeyValueDictionary();
            //    paramDic.Add("Action", "完成HD");
            //    paramDic.Add("cNumber", cinfo.No);
            //    paramDic.Add("Supplier", "MVSHD");
            //    paramDic.Add("Agent", sinfo.MajorUserName);
            //    paramDic.Add("stMgr", cinfo.ReporterName);

            //    paramDic.Add("Priority", cinfo.PriorityName);
            //    paramDic.Add("Category1", cinfo.ClassName1);
            //    paramDic.Add("Category2", cinfo.ClassName2);
            //    paramDic.Add("Category3", cinfo.ClassName3);

            //    paramDic.Add("Solution", sinfo.Details);
            //    paramDic.Add("Attachment", "");

            //    string paramStr = WebUtil.BuildQueryJson(paramDic);
            //    //string sqlStrHK = "INSERT INTO sys_WebServiceTask VALUES ('" + paramStr + "',0," + cinfo.CustomerID.ToString() + "," + cinfo.BrandID.ToString() + ");";
            //    //int records = CallBLL.AddBurgerKingTask(sqlStrHK);
            //    WebServiceTaskInfo bkWebSvrTask = new WebServiceTaskInfo();
            //    bkWebSvrTask.CallNo = cinfo.No;
            //    bkWebSvrTask.TaskUrl = paramStr;
            //    bkWebSvrTask.CustomerID = cinfo.CustomerID;
            //    bkWebSvrTask.CustomerName = cinfo.CustomerName;
            //    bkWebSvrTask.BrandID = cinfo.BrandID;
            //    bkWebSvrTask.BrandName = cinfo.BrandName;
            //    bkWebSvrTask.IsDone = false;
            //    bkWebSvrTask.Remark = string.Empty;
            //    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask开始 动作：完成（转第三方），参数信息：" + paramStr + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
            //    if (WebServiceTaskBLL.Add(bkWebSvrTask) > 0)
            //    {
            //        Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask成功 动作：完成（转第三方）" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
            //    }
            //    else
            //    {
            //        Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask失败 动作：完成（转第三方）" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
            //    }
            //}
            #endregion

            #region 汉堡王转呈回来点击已解决时，拼凑关闭的参数
            if ((int)SysEnum.CallStateMain.已完成 == cinfo.StateMain && (cinfo.BrandName == "汉堡王" || cinfo.CustomerName == "汉堡王"))
            {
                KeyValueDictionary paramDic = new KeyValueDictionary();
                paramDic.Add("Action", "关闭");
                paramDic.Add("cNumber", cinfo.No);
                paramDic.Add("Supplier", "MVSHD");
                paramDic.Add("Agent", sinfo.UserName);
                paramDic.Add("TSI", "MVSL2");
                paramDic.Add("Engineer", sinfo.MajorUserName);
                paramDic.Add("stMgr", cinfo.ReporterName);
                paramDic.Add("Solution", sinfo.Details);
                paramDic.Add("Attachment", "");

                string paramStr = WebUtil.BuildQueryJson(paramDic);
                //string sqlStrHK = "INSERT INTO sys_WebServiceTask VALUES ('" + paramStr + "',0," + info.CustomerID.ToString() + "," + info.BrandID.ToString() + ");";
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
                Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask开始 动作：关闭(第三方转呈回来点已解决)，参数信息：" + paramStr + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                if (WebServiceTaskBLL.Add(bkWebSvrTask) > 0)
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask成功 动作：关闭(第三方转呈回来点已解决)" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }
                else
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask失败 动作：关闭(第三方转呈回来点已解决)" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }
            }
            #endregion
            string js = "top.ReloadLeft();alert('成功记录" + APImsg + "');location.href='/page/call/";
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
            Function.AlertBack("提交失败。请联系管理员");
        }
        BtnSubmit.Visible = false;

    }

    public CallInfo GetInfo()
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


}
