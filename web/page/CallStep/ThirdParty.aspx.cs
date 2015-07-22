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
            DdlThirdParty.DataSource = ThirdPartyBLL.GetList(CurrentUser.WorkGroupID);
            DdlThirdParty.DataBind();
            DdlThirdParty.Items.Insert(0, new ListItem("请选择", "0"));

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


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {

        CallInfo cinfo = GetInfo();
        if (!RightState.Contains(cinfo.StateDetail))
        {
            Function.AlertMsg("数据失效，请刷新");
            return;
        }

        CallStepInfo sinfo = new CallStepInfo();
        sinfo.StepType = (int)SysEnum.StepType.第三方预约上门;
        sinfo.MajorUserID = CurrentUserID;
        sinfo.MajorUserName = CurrentUserName+(DdlThirdParty.SelectedValue);
        sinfo.AddDate = DateTime.Now;
        sinfo.CallID = cinfo.ID;
        sinfo.DateBegin = Function.ConverToDateTime(TxbDate.Text.Trim());
        sinfo.DateEnd = sinfo.DateBegin;
        sinfo.Details = TxbDetails.Text.Trim();
        sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        sinfo.IsSolved = false;
        sinfo.SolutionID = 0;
        sinfo.SolutionName = "";
        cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
        cinfo.StateDetail = (int)SysEnum.CallStateDetails.第三方预约上门;

        sinfo.StepName = SysEnum.CallStateDetails.第三方预约上门.ToString();
        sinfo.UserID = CurrentUser.ID;
        sinfo.UserName = CurrentUser.Name;

        if (sinfo.DateBegin == Tool.Function.ErrorDate)
        {
            Function.AlertBack("预约日期有误");
            return;
        }

        if (sinfo.Details.Length > 500)
        {
            Function.AlertBack("处理过程备注不能超过500字");
            return;
        }
        if (Function.ConverToInt(DdlThirdParty.SelectedValue) <= 0)
        {
            Function.AlertBack("请选择第三方");
            return;            
        }
        //如果是汉堡王品牌选择的第三方需要是指定的第三方
        if (cinfo.BrandName == "汉堡王" || cinfo.CustomerName == "汉堡王")
        {
            if (!DdlThirdParty.SelectedItem.Text.Contains("&$&")) 
            {
                Function.AlertBack("请选择汉堡王对应的第三方");
                return;
            }
        
        }

        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
        {
            string APImsg = string.Empty;

            #region 汉堡王升级到客户处理完成时
            if (cinfo.BrandName == "汉堡王" || cinfo.CustomerName == "汉堡王")
            {
                //string url = "http://helpdesk.bkchina.cn/siweb/ws_hesheng.ashx?";
                KeyValueDictionary paramDic = new KeyValueDictionary();
                paramDic.Add("Action", "转呈");
                paramDic.Add("cNumber", cinfo.No);
                paramDic.Add("Supplier", "MVS");
                paramDic.Add("Agent", CurrentUserName);
                //汉堡王的TSI取"&$&"后的字段
                string tsi = DdlThirdParty.SelectedItem.Text;
                int startIndex = tsi.IndexOf("&$&");
                tsi = tsi.Substring(startIndex + 3);
                paramDic.Add("TSI", tsi);
                paramDic.Add("stCode", cinfo.StoreName);//由于addcall的时候calls表storeNO和StoreName赋值赋反了
                paramDic.Add("stMgr", cinfo.ReporterName);
                paramDic.Add("Time1", DateTime.Now);
                paramDic.Add("Issue", cinfo.Details);
                paramDic.Add("Priority", cinfo.PriorityName);
                paramDic.Add("Category1", cinfo.ClassName1);
                paramDic.Add("Category2", cinfo.ClassName2);
                paramDic.Add("Category3", cinfo.ClassName3);
                paramDic.Add("Solution", "");
                paramDic.Add("Attachment", "");
                //WebUtil webtool = new WebUtil();
                //string result = webtool.DoPost(url, paramDic);
                //JObject obj = JObject.Parse(result);
                //string errNo = obj["errNo"].ToString();

                //if (errNo == "0")
                //{
                //    APImsg = " 接口调用成功";
                //}
                //else
                //{
                //    APImsg = " 接口调用失败" + obj["Desc"].ToString();
                //}
                string paramStr = WebUtil.BuildQueryJson(paramDic);
                string sqlStrHK = "INSERT INTO sys_WebServiceTask VALUES ('" + paramStr + "',0," + cinfo.CustomerID.ToString() + "," + cinfo.BrandID.ToString() + ");";
                int records = CallBLL.AddBurgerKingTask(sqlStrHK);
                if (records <= 0)
                    APImsg = " 汉堡王任务记录失败，请联系管理员";
            }
            #endregion

            string js = "top.ReloadLeft();alert('成功记录 " + APImsg + "');location.href='/page/Call/sln.aspx?id=" + cinfo.ID + "';";
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
