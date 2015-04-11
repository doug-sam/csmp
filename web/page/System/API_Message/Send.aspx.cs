using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;
using EMPPLib;

public partial class system_API_Message_Send : _Sys_Profile
{
    public EMPPLib.emptcl empp;

    public system_API_Message_Send()
    {
        empp = new EMPPLib.emptclClass();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           empp.SubmitRespInterface += (new _IemptclEvents_SubmitRespInterfaceEventHandler(SubmitRespInterface));

        }
    }

    public void SubmitRespInterface(SubmitResp sm)
    {
        try
        {
            string str = "收到submitResp:msgId=" + sm.MsgID + ",seqId=" + sm.SequenceID + ",result=" + sm.Result;
            LabResult.Text += str + "<br/>";
        }
        catch (Exception)
        {

            throw;
        }

    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
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
        shortMsg.content = TxbContent.Text.Trim();
        shortMsg.DestMobiles = mobs;
        shortMsg.SendNow = true;
        empp.needStatus = true;

        try
        {
            if (empp.connect(host, port, accountId, password) == ConnectResultEnum.CONNECT_OK)
            {
                empp.submit(shortMsg);
                LabResult.Text += "连接短信服务器成功。短信请求已提交";
            }
            else
            {
                LabResult.Text += "连接短信服务器失败。";
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
}
