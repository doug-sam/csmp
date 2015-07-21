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


public partial class page_call_slnCustomer : _Call_Step
{
    int[] RightState = new int[] {
        (int)SysEnum.CallStateDetails.升级到客户,
        (int)SysEnum.CallStateDetails.上门取消,
        (int)SysEnum.CallStateDetails.二线离场确认,
        (int)SysEnum.CallStateDetails.电话支持,
        (int)SysEnum.CallStateDetails.开始处理,
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


            TxbDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
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
        sinfo.StepType = (int)SysEnum.StepType.升级到客户;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName;
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.DateBegin = Function.ConverToDateTime(TxbDate.Text.Trim());
        sinfo.DateEnd = sinfo.DateBegin;
        sinfo.Details = TxbDetails.Text.Trim();
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        if (RblSolved.SelectedValue == "1")
        {
            sinfo.IsSolved = true;
            sinfo.SolutionID = -1;
            sinfo.SolutionName = "客户自行解决";
            cinfo.StateMain = (int)SysEnum.CallStateMain.已完成;
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.处理完成;
            cinfo.SloveBy = SysEnum.SolvedBy.客户解决.ToString();
            cinfo.FinishDate = sinfo.DateBegin;
        }
        else
        {
            sinfo.IsSolved = false;
            sinfo.SolutionID = 0;
            sinfo.SolutionName = "";
            cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
            cinfo.StateDetail = (int)SysEnum.CallStateDetails.升级到客户;
            
        }
        sinfo.StepName = SysEnum.CallStateDetails.升级到客户.ToString();
        sinfo.UserID = CurrentUser.ID;
        sinfo.UserName = CurrentUser.Name;

        if (sinfo.DateBegin == Tool.Function.ErrorDate)
        {
            Function.AlertBack("实际升级客户日期有误");
        }
        if (sinfo.DateBegin > DateTime.Now)
        {
            Function.AlertMsg("实际操作日期不能大于当前时间");
            return;
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

                #region 汉堡王升级到客户处理完成时
                //if (cinfo.BrandName=="汉堡王"||cinfo.CustomerName=="汉堡王")
                //{
                //    string url = "http://helpdesk.bkchina.cn/siweb/ws_hesheng.ashx?";
                //    KeyValueDictionary paramDic = new KeyValueDictionary();
                //    paramDic.Add("Action", "HD完成");
                //    paramDic.Add("cNumber", cinfo.No);
                //    paramDic.Add("Supplier", "MVS");
                //    paramDic.Add("Agent", sinfo.MajorUserName);
                //    paramDic.Add("stMgr", cinfo.ReporterName);
                //    paramDic.Add("Solution", "");
                //    paramDic.Add("Attachment", "");
                //    WebUtil webtool = new WebUtil();
                //    string result = webtool.DoPost(url, paramDic);
                //    JObject obj = JObject.Parse(result);
                //    string errNo = obj["errNo"].ToString();
                //    if (errNo == "0")
                //    {
                //        APImsg = " 接口调用成功";
                //    }
                //    else
                //    {
                //        APImsg = " 接口调用失败" + obj["Desc"].ToString();
                //    }
                //}
                #endregion
            }

            #region RblSolved.SelectedValue为0，判断是汉堡王时调用接口
            //if (RblSolved.SelectedValue == "0" && (cinfo.BrandName == "汉堡王" || cinfo.CustomerName == "汉堡王"))
            //{
            //    string url = "http://helpdesk.bkchina.cn/siweb/ws_hesheng.ashx?";
            //    KeyValueDictionary paramDic = new KeyValueDictionary();
            //    paramDic.Add("Action", "转呈");
            //    paramDic.Add("cNumber", cinfo.No);
            //    paramDic.Add("Supplier", "MVS");
            //    paramDic.Add("Agent", CurrentUserName);
            //    paramDic.Add("TSI", "");
            //    paramDic.Add("stCode", cinfo.StoreName);//由于addcall的时候calls表storeNO和StoreName赋值赋反了
            //    paramDic.Add("stMgr", cinfo.ReporterName);
            //    paramDic.Add("Time1",DateTime.Now);
            //    paramDic.Add("Issue", cinfo.Details);
            //    paramDic.Add("Priority", cinfo.PriorityName);
            //    paramDic.Add("Category1", cinfo.ClassName1);
            //    paramDic.Add("Category2", cinfo.ClassName2);
            //    paramDic.Add("Category3", cinfo.ClassName3);
            //    paramDic.Add("Solution", "");
            //    paramDic.Add("Attachment", "");
            //    WebUtil webtool = new WebUtil();
            //    string result = webtool.DoPost(url, paramDic);
            //    JObject obj = JObject.Parse(result);
            //    string errNo = obj["errNo"].ToString();

            //    if (errNo == "0")
            //    {
            //        APImsg = "接口调用成功";
            //    }
            //    else
            //    {
            //        APImsg = "接口调用失败" + obj["Desc"].ToString();
            //    }
            //}
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
            Function.AlertBack("提交失败。请联系管理员");
        }
        BtnSubmit.Visible = false;

    }
}
