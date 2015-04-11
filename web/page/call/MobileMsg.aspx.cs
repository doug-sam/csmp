using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using EMPPLib;
using Tool;

public partial class page_call_MobileMsg : BasePage
{
    public EMPPLib.emptcl empp;

    public page_call_MobileMsg()
    {

        try
        {
            empp = new EMPPLib.emptclClass();
            empp.SubmitRespInterface += (new _IemptclEvents_SubmitRespInterfaceEventHandler(SubmitRespInterface));
            empp.StatusReceivedInterface += (new _IemptclEvents_StatusReceivedInterfaceEventHandler(StatusReceivedInterface));
        }
        catch (Exception ex)
        {
            //BtnSend.Enabled = false;
            //BtnSend.Text = "短信功能调用失败，暂不能用.";
            //LabResult.Text = ex.Message;
            throw ex;
        }

    }
    /// <summary>
    /// 短信发送结果委托
    /// </summary>
    public void SubmitRespInterface(SubmitResp sm)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "success", "alert('短信发送成功啦A！');", true);
            //string str = "收到submitResp:msgId=" + sm.MsgID + ",seqId=" + sm.SequenceID + ",result=" + sm.Result;
            LabResult.Text += "对方已收到短信。<br/>";
        }
        catch (Exception)
        {

            throw;
        }

    }

    public void StatusReceivedInterface(StatusReport sm)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "success", "alert('短信发送成功啦B！');", true);
        string str = "收到状态报告:seqId=" + sm.SeqID + ",msgId=" + sm.MsgID + ",mobile=" + sm.DestID + ",destId=" + sm.SrcTerminalId + ",stat=" + sm.Status;
        Response.Write(str);
        Response.Flush();

    }



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BtnSend.Enabled = false;
            string Tel = Function.GetRequestSrtring("Tel").Trim();
            if (!string.IsNullOrEmpty(Tel))
            {
                TxbTel.Text = Tel;

            }
            DateTime DateDate = Function.GetRequestDateTime("DateDate");
            if (DateDate == Tool.Function.ErrorDate)
            {
                Function.AlertMsg("没有预约上门时间，请先认真填写");
                return;
            }
            string Details = Function.GetRequestSrtring("Details").Trim();
            string DropInUserName = Function.GetRequestSrtring("DropInUserName").Trim();
            if (string.IsNullOrEmpty(DropInUserName))
            {
                Function.AlertMsg("没有上门工程师名字，请先认真填写");
                return;

            }
            int CallID = Function.GetRequestInt("CallID");
            CallInfo info = CallBLL.Get(CallID);
            if (null == info)
            {
                Function.AlertMsg("报修记录丢失。请联系管理员");
                return;
            }
            TxbContent.Text = GetMessageContent(info, DateDate, Details, DropInUserName);
            BtnSend.Enabled = true;

        }
    }
    /// <summary>
    /// 发送短信给对方 
    /// </summary>
    protected void BtnSend_Click(object sender, EventArgs e)
    {
        
        LabResult.Text = "";
        TxbTel.Text = TxbTel.Text.Trim().Trim(';');
        List<string> Tels = TxbTel.Text.Split(';').ToList();

        string host = ProfileBLL.GetValue(ProfileInfo.API_Message.Host, true);
        int port = Function.ConverToInt(ProfileBLL.GetValue(ProfileInfo.API_Message.Port, true), 9981);
        string accountId = ProfileBLL.GetValue(ProfileInfo.API_Message.accountId, true);
        string serviceId = ProfileBLL.GetValue(ProfileInfo.API_Message.serviceId, true);
        string password = ProfileBLL.GetValue(ProfileInfo.API_Message.password, true);


        EMPPLib.Mobiles mobs = new EMPPLib.MobilesClass();
        foreach (string item in Tels)
        {
            if (string.IsNullOrEmpty(item))
            {
                continue;
            }
            mobs.Add(item.Trim());
        }
        if (mobs.count == 0)
        {
            Function.AlertBack("发送手机号有误");
        }

        EMPPLib.ShortMessage shortMsg = new EMPPLib.ShortMessageClass();
        shortMsg.srcID = accountId;
        shortMsg.ServiceID = serviceId;
        shortMsg.needStatus = true;
        shortMsg.content =TxbContent.Text.Trim();
        shortMsg.DestMobiles = mobs;
        shortMsg.SendNow = true;
        empp.needStatus = true;

        try
        {
            ConnectResultEnum ConnResult=empp.connect(host, port, accountId, password);
            if (ConnResult == ConnectResultEnum.CONNECT_OK || ConnResult == ConnectResultEnum.CONNECT_KICKLAST)
            {
                empp.submit(shortMsg);
                LabResult.Text += "短信请求已提交";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "success", "AlertAndExit();", true);

            }
            else
            {
                LabResult.Text += "连接短信服务器失败。连接状态为:" + ConnResult.ToString();
            }
        }
        catch (Exception ex)
        {
            LabResult.Text = "系统出错。" + ex.Message;
        }
        finally
        {
            // empp.disconnect();
        }
    }

    /// <summary>
    /// 根据模板得具出具短信发送内容
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private string GetMessageContent(CallInfo info, DateTime DateDate, string Details, string DropInUserName)
    {
        StoreInfo sinfo = StoresBLL.Get(info.StoreID);
        if (null == sinfo)
        {
            sinfo = new StoreInfo();
        }
        UserInfo uinfo = UserBLL.Get(info.MaintainUserID);
        if (uinfo == null)
        {
            uinfo = new UserInfo();
        }
        WorkGroupInfo winfo = WorkGroupBLL.Get(uinfo.WorkGroupID);
        if (null == winfo)
        {
            winfo = new WorkGroupInfo();
        }
        string MessageContent = ProfileBLL.GetValue(ProfileInfo.API_Message.MsgTemplate1, true);
        List<string> ReplaceItem = new List<string>();
        MessageContent = MessageContent.Replace("(((((系统单号)))))", info.No);
        MessageContent = MessageContent.Replace("(((((客户)))))", info.CustomerName);
        MessageContent = MessageContent.Replace("(((((品牌)))))", info.BrandName);
        MessageContent = MessageContent.Replace("(((((店铺号)))))", info.StoreName);
        MessageContent = MessageContent.Replace("(((((店铺名)))))", info.StoreNo);
        MessageContent = MessageContent.Replace("(((((店铺电话)))))", sinfo.Tel);
        MessageContent = MessageContent.Replace("(((((预约上门时间)))))", DateDate.ToString("yyyy-MM-dd HH:mm"));
        MessageContent = MessageContent.Replace("(((((备件详细及工作说明)))))", Details);
        MessageContent = MessageContent.Replace("(((((二线工程师名)))))", info.MaintaimUserName);
        MessageContent = MessageContent.Replace("(((((二线工程师电话)))))", uinfo.Tel);
        MessageContent = MessageContent.Replace("(((((二线工程师邮箱)))))", uinfo.Email);
        MessageContent = MessageContent.Replace("(((((单号工作组)))))", winfo.Name);
        MessageContent = MessageContent.Replace("(((((上门工程师名)))))", DropInUserName);
        return MessageContent;

    }

}