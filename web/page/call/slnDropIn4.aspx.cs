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
using Newtonsoft.Json.Linq;

public partial class page_call_slnDropIn4 : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.上门支持
    };


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            if (null == info)
            {
                Response.Write("数据有误");
                Response.End();
                return;
            }
            CallState1.CallID = info.ID;

            #region 状态跳转
            if (!RightState.Contains(info.StateDetail))
            {
                Function.AlertBack("数据状态有误");
                return;
            }
            #endregion

            CallStepInfo csinfo = GetLastStepID();
            if (null == csinfo)
            {
                Function.AlertBack("没有到达门店记录，你作弊？！");
            }
            UserInfo uinfo = UserBLL.Get(csinfo.MajorUserID);
            if (null == uinfo)
            {
                Function.AlertBack("系统发生严重错误，帐户信息丢失");
            }
            LabUser.Text = csinfo.MajorUserName;

            TxbDateEnd.Text = csinfo.DateEnd.ToString("yyyy-MM-dd HH:mm");
            DdlSln.DataSource = SolutionBLL.GetList(info.Class3);
            DdlSln.DataBind();
            DdlSln.Items.Insert(0, new ListItem("其它", "0"));
            DdlSln.Items.Insert(0, new ListItem("请选择", "-1"));

            #region 用户检查
            NotMyCallCheck(info.ID);
            #endregion


            YumRecordInfo yumrecordInfo = YumRecordBLL.Get(info.ID, SysEnum.CallStateMain.已完成.ToString());
            if (CallStepActionBLL.ActionInfo.IsZara(GetInfo()) && CallStepActionBLL.ActionZara.Enable(GetInfo()) && (null == yumrecordInfo || yumrecordInfo.ID <= 0))
            {
                CbSendZara.Visible = true;
            }
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

    public CallStepInfo GetLastStepID()
    {
        CallStepInfo csinfo = CallStepBLL.GetLast(GetInfo().ID, SysEnum.StepType.上门详细);
        return csinfo;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(RblSolved.SelectedValue))
        {
            Function.AlertMsg("问题是否已经解决了？"); return;
        }

        CallInfo cinfo = GetInfo();
        if (!RightState.Contains(cinfo.StateDetail))
        {
            Function.AlertMsg("数据失效，请刷新");
            return;
        }


        CallStepInfo sinfo = new CallStepInfo();//当前步
        CallStepInfo csinfo = GetLastStepID();//上一步

        sinfo.StepType = (int)SysEnum.StepType.二线离场确认;
        sinfo.MajorUserID = csinfo.MajorUserID;
        sinfo.MajorUserName = csinfo.MajorUserName;
        sinfo.UserID = CurrentUserID;
        sinfo.UserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.DateBegin = csinfo.DateEnd;
        sinfo.DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        sinfo.Details = TxbDetails.Text.Trim();
        sinfo.StepName = SysEnum.CallStateDetails.二线离场确认.ToString();
        sinfo.IsSolved = RblSolved.SelectedValue == "1";

        #region 验证
        if (sinfo.DateEnd == Tool.Function.ErrorDate)
        {
            Function.AlertMsg("请认真填写抵达和离场时间"); return;
        }
        if (sinfo.DateBegin > sinfo.DateEnd.AddMinutes(1))
        {
            Function.AlertMsg("工程师离场时间比你的确认时间还晚。你填错了吧？"); return;
        }
        if (sinfo.Details.Length > 500)
        {
            Function.AlertMsg("过程备注太长。不应超500字"); return;
        }
        if (RblSolved.SelectedValue == "1")
        {
            if (DdlSln.SelectedValue == "-1")
            {
                Function.AlertMsg("请选择解决方案"); return;
            }
            if (DdlSln.SelectedValue == "0" && string.IsNullOrEmpty(TxbSln.Text.Trim()))
            {
                Function.AlertMsg("请输入解决方案"); return;
            }
        }
        #endregion
        if (RblSolved.SelectedValue == "1")
        {
            if (TxbSln.Visible)
            {
                SolutionInfo slninfo = SolutionBLL.Get(TxbSln.Text.Trim(), cinfo.Class2);
                if (null == slninfo)
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
                    slninfo.SolveCount = 0;
                    slninfo.SuggestCount = 1;
                    int SID = SolutionBLL.Add(slninfo);
                    if (SID <= 0)
                    {
                        Function.AlertMsg("系统出错，请联系管理员"); return;
                    }
                    slninfo.ID = SID;
                    #endregion
                }
                sinfo.SolutionID = slninfo.ID;
                sinfo.SolutionName = slninfo.Name;
            }
            else
            {
                sinfo.SolutionID = Function.ConverToInt(DdlSln.SelectedValue);
                sinfo.SolutionName = DdlSln.SelectedItem.Text.Trim();
            }
            cinfo.StateMain = (int)SysEnum.CallStateMain.已完成;
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.处理完成;
            cinfo.SloveBy = SysEnum.SolvedBy.上门.ToString();
            cinfo.FinishDate = sinfo.DateBegin;
            
        }
        else
        {
            sinfo.SolutionID = 0;
            sinfo.SolutionName = "";
            cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.二线离场确认;
        }


        UpdateData(cinfo, sinfo);
        BtnSubmit.Visible = false;


    }

    private void UpdateData(CallInfo cinfo, CallStepInfo sinfo)
    {
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            string APImsg = CallStepActionBLL.StepDropInLeave(cinfo, sinfo);//跟旧版本对比下，确定这个是不是yum才有的。bug现在变成zara也发了
            if (APImsg != null)
            {
                if (APImsg == string.Empty)
                {
                    APImsg += "《离场》邮件发送成功";
                }
                else
                {
                    APImsg = "《离场》邮件发送失败" + APImsg;
                }
                APImsg = APImsg.Replace("\"", " ").Replace("'", " ");

            }
            #region 汉堡王处理完成时
            if ((int)SysEnum.CallStateMain.已完成 == cinfo.StateMain&&(cinfo.BrandName == "汉堡王" || cinfo.CustomerName == "汉堡王"))
            {
                KeyValueDictionary paramDic = new KeyValueDictionary();
                paramDic.Add("Action", "完成");
                paramDic.Add("cNumber", cinfo.No);
                //paramDic.Add("Supplier", "MVS");
                paramDic.Add("TSI", "MVSL2");
                paramDic.Add("Engineer", sinfo.MajorUserName);
                //paramDic.Add("Agent", sinfo.MajorUserName);
                paramDic.Add("stMgr", cinfo.ReporterName);
                paramDic.Add("Solution", sinfo.Details);
                paramDic.Add("Attachment", "");
                paramDic.Add("tTime", DateTime.Now);
                paramDic.Add("Method", "ONSITE");
                
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
                Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask开始 动作：完成（上门），参数信息：" + paramStr + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                if (WebServiceTaskBLL.Add(bkWebSvrTask) > 0)
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask成功 动作：完成（上门）" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }
                else
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask失败 动作：完成（上门）" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }

                #region 上门完成调用 关闭
                KeyValueDictionary paramCloseDic = new KeyValueDictionary();
                paramCloseDic.Add("Action", "关闭");
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
                WebServiceTaskInfo bkWebSvrTaskClose = new WebServiceTaskInfo();
                bkWebSvrTaskClose.CallNo = cinfo.No;
                bkWebSvrTaskClose.TaskUrl = paramCloseStr;
                bkWebSvrTaskClose.CustomerID = cinfo.CustomerID;
                bkWebSvrTaskClose.CustomerName = cinfo.CustomerName;
                bkWebSvrTaskClose.BrandID = cinfo.BrandID;
                bkWebSvrTaskClose.BrandName = cinfo.BrandName;
                bkWebSvrTaskClose.IsDone = false;
                bkWebSvrTaskClose.Remark = string.Empty;
                Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask开始 动作：关闭（上门），参数信息：" + paramCloseStr + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                if (WebServiceTaskBLL.Add(bkWebSvrTaskClose) > 0)
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask成功 动作：关闭（上门）" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }
                else
                {
                    Logger.GetLogger(this.GetType()).Info("插入一条WebServiceTask失败 动作：关闭（上门）" + "，callid=" + cinfo.ID + "，CustomerName:" + cinfo.CustomerName + "，BrandName:" + cinfo.BrandName + "，操作人：" + CurrentUserName, null);
                }
                #endregion
            }
            #endregion

            if (CbSendZara.Visible &&(CbSendZara.Checked || (int)SysEnum.CallStateMain.已完成 == cinfo.StateMain))
            {
                APImsg += CallStepActionBLL.StepDropInSlove(cinfo, sinfo);

                string SuccessMsg = CallStepActionBLL.StepSloved(cinfo, sinfo);
                if (SuccessMsg != null)
                {
                    if (SuccessMsg == string.Empty)
                    {
                        APImsg += "《已完成》邮件发送成功";
                        YumRecordInfo yumrecordInfo = new YumRecordInfo();
                        yumrecordInfo.CallID = cinfo.ID;
                        yumrecordInfo.CallNo = cinfo.ID;
                        yumrecordInfo.Flag = "ZARA";
                        yumrecordInfo.IsSuccess = true;
                        yumrecordInfo.SendDate = DateTime.Now;
                        yumrecordInfo.Step = SysEnum.CallStateMain.已完成.ToString();
                        YumRecordBLL.Add(yumrecordInfo);
                    }
                    else
                    {
                        APImsg = "《已完成》邮件发送失败" + APImsg;
                    }
                }
            }
            if (!Comment_Add(cinfo))
            {
                APImsg += "评价工程师失败!";
            }
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
            return;
        }
        else
        {
            Function.AlertMsg("处理出错。请联系管理员");
            return;
        }
    }

    //添加评论
    private bool Comment_Add(CallInfo cinfo)
    {
        CallStepInfo stepinfo = CallStepBLL.GetLast(cinfo.ID, SysEnum.StepType.上门详细);
        CommentInfo info = new CommentInfo();
        info.AddDate = DateTime.Now;
        info.ByMachine = CommentInfo.MachineType.web.ToString();
        info.CallID = cinfo.ID;
        info.CallStepID = stepinfo.ID;
        info.Details = string.Format("二线人员{0}对现场工程{1} 技术评分值:{2}；态度评分值{3}",
            cinfo.MaintaimUserName,
            stepinfo.MajorUserName,
            RblStar2.SelectedValue,
            RblStar3.SelectedValue);
        info.DropInUserID = stepinfo.MajorUserID;
        info.IsDropInUserDoIt = false;
        info.Score2 = Function.ConverToInt(RblStar2.SelectedValue, 0);
        info.Score3 = Function.ConverToInt(RblStar3.SelectedValue, 0);
        info.Score = info.Score2 + info.Score3;
        info.SupportUserID = cinfo.MaintainUserID;
        info.WorkGroupID = CurrentUser.WorkGroupID;
        info.ID = CommentBLL.Add(info);
        return info.ID > 0;


    }

    #region 改变状态

    protected void RblSolved_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RblSolved.SelectedValue == "1")
        {
             Tr_Sln.Visible = true;
        }
        else
        {
             Tr_Sln.Visible = false;
        }

    }

    protected void DdlSln_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxbSln.Visible = DdlSln.SelectedValue == "0";
    }

    #endregion

}
