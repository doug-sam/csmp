using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Net.Mail;
using System.IO;

public partial class CallStep_SendEmail : _Call_Sln1
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            UserInfo TargetUserInfo = GetTargetUserInfo();
            if (null == info)
            {
                Function.AlertBack("参数有误");
                return;
            }
            string JobCode = Function.GetRequestSrtring("JobCode");

            DateTime DateDate = Function.GetRequestDateTime("DateDate");
            UserInfo MaintainUserInfo = UserBLL.Get(info.MaintainUserID);
            StoreInfo stInfo = StoresBLL.Get(info.StoreID);

            TxbSubject.Text = info.No + " " + info.CityName + " " + info.StoreNo + " " + info.BrandName;
            TxbContent.Text = ProfileBLL.GetValue(ProfileInfo.UserKey.邮件模板);
            TxbContent.Text = TxbContent.Text.Replace("(((((对方名称)))))", null == TargetUserInfo ? "  " : TargetUserInfo.Name);
            TxbContent.Text = TxbContent.Text.Replace("(((((单号)))))", info.No);
            TxbContent.Text = TxbContent.Text.Replace("(((((来源单号)))))", info.ReportSourceNo);
            TxbContent.Text = TxbContent.Text.Replace("(((((店铺电话)))))", stInfo.Tel);
            TxbContent.Text = TxbContent.Text.Replace("(((((店铺地址)))))", stInfo.Address);
            TxbContent.Text = TxbContent.Text.Replace("(((((店铺名)))))", info.StoreNo);
            TxbContent.Text = TxbContent.Text.Replace("(((((品牌名)))))", info.BrandName);
            TxbContent.Text = TxbContent.Text.Replace("(((((城市)))))", info.CityName);
            TxbContent.Text = TxbContent.Text.Replace("(((((小类故障)))))", info.ClassName3);
            TxbContent.Text = TxbContent.Text.Replace("(((((预约上门时间)))))", DateDate == Function.ErrorDate ? string.Empty : DateDate.ToString("MM月dd日 HH点"));
            TxbContent.Text = TxbContent.Text.Replace("(((((小类故障)))))", info.ClassName3);
            TxbContent.Text = TxbContent.Text.Replace("(((((报修时间)))))", info.ErrorDate.ToString("yyyy-MM-dd HH:mm"));
            TxbContent.Text = TxbContent.Text.Replace("(((((二线人员)))))", info.MaintaimUserName);
            TxbContent.Text = TxbContent.Text.Replace("(((((当前登录用户)))))", CurrentUser.Name);
            TxbContent.Text = TxbContent.Text.Replace("(((((当前登录用户邮箱)))))", CurrentUser.Email);
            TxbContent.Text = TxbContent.Text.Replace("(((((二线电话)))))", null == MaintainUserInfo ? string.Empty : MaintainUserInfo.Tel);
            TxbContent.Text = TxbContent.Text.Replace("(((((备注工作说明)))))", Function.GetRequestSrtring("Details"));
            TxbContent.Text = TxbContent.Text.Replace("(((((jobcode)))))", JobCode);
            TxbContent.Text = TxbContent.Text.Replace("(((((服务内容)))))", Function.GetRequestSrtring("Details"));
            TxbContent.Text = TxbContent.Text.Replace("(((((故障描述)))))", info.Details);

        }
    }

    public CallInfo GetInfo()
    {
        if (null != ViewState["info"])
        {
            return (CallInfo)ViewState["info"];
        }
        int CallID = Function.GetRequestInt("CallID");
        if (CallID <= 0)
        {
            return null;
        }
        CallInfo info = CallBLL.Get(CallID);
        if (null != info)
        {
            ViewState["info"] = info;
            return info;
        }
        return null;
    }

    private UserInfo GetTargetUserInfo()
    {
        if (null != ViewState["uinfo"])
        {
            return (UserInfo)ViewState["uinfo"];
        }
        int ID = Function.GetRequestInt("DropInUserID");
        if (ID <= 0)
        {
            return null;
        }
        UserInfo info = UserBLL.Get(ID);
        if (null != info)
        {
            ViewState["uinfo"] = info;
            return info;
        }
        return null;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    { 

        LabError.Text = string.Empty;

        CallInfo info = GetInfo();
        CallStepInfo StepJobCode = new CallStepInfo();
        StepJobCode.Details = Function.GetRequestSrtring("Details");
        StepJobCode.UserName = GetTargetUserInfo() == null ? string.Empty : GetTargetUserInfo().Name;
        StepJobCode.ID = CallStepBLL.GetLast(info.ID).ID;
        StepJobCode.StepName = Function.GetRequestSrtring("JobCode");//借个字段记录个jobcode，我知道这样是大忌，但为了方便，就这样搞一次吧
        StepJobCode.DateBegin = StepJobCode.DateEnd = Function.GetRequestDateTime("DateDate");
        EmailInfo einfo = new EmailInfo();
        einfo.Attachment = null;
        einfo.Body = TxbContent.Text.Trim();

        einfo.Subject = TxbSubject.Text.Trim();
        einfo.MailAddress = new List<MailAddress>();
        einfo.CC = new List<MailAddress>();
        #region 收件人，抄送人，回复到
        foreach (string item in TxbToAddress.Text.Trim().Split(';').ToList())
        {
            if (string.IsNullOrEmpty(item.Trim()))
            {
                continue;
            }
            MailAddress maddress = new MailAddress(item.Trim());
            try
            {
                einfo.MailAddress.Add(maddress);
            }
            catch (Exception)
            {
                Function.AlertMsg("邮件地址错误：" + maddress);
                return;
            }

        }
        foreach (string item in TxbCC.Text.Trim().Split(';').ToList())
        {
            if (string.IsNullOrEmpty(item.Trim()))
            {
                continue;
            }
            MailAddress maddress = new MailAddress(item.Trim());
            try
            {
                einfo.CC.Add(maddress);
            }
            catch (Exception)
            {
                Function.AlertMsg("抄送邮件地址错误：" + maddress);
                return;
            }

        }
        if (string.IsNullOrEmpty(CurrentUser.Email))
        {
            CurrentUser.Email = "services@mvs.com.cn";
        }
        einfo.ReplayTo = new MailAddress(CurrentUser.Email, CurrentUser.Name);
        #endregion
        einfo.Attachment = AddAttachment(info, StepJobCode);


        string result = EmailBLL.Email_Send(einfo);//发送邮件。返回返回空字符串
        if (string.IsNullOrEmpty(result))
        {
            EmailRecordInfo eRecordinfo = new EmailRecordInfo();    //邮件发送记录对象
            eRecordinfo.UserID = CurrentUserID;
            eRecordinfo.UserName = CurrentUserName;
            eRecordinfo.CallNo = GetInfo().No;
            eRecordinfo.CallID = GetInfo().ID;
            eRecordinfo.DateAdd = DateTime.Now;
            eRecordinfo.ToUser = string.Format("主：{0}<br/>抄:{1}", TxbToAddress.Text.Trim(), TxbCC.Text.Trim());

            if (EmailRecordBLL.Add(eRecordinfo) <= 0)   //记录邮件发送记录
            {
                Response.Write("<div style='text-align:center;margin-top:150px;font-size:54px;font-color:red;color:red;'>你必需将这界面给李春看！！<br/>糟糕，邮件虽然发送成功，但没有记录下邮件记录，为方便统计，你应该将这个页面给领导看；</div>");
                Response.End();
                return;
            }

            Session["EmailGroupTo"] = null;
            Session["EmailGroupCC"] = null;
            Session["To"] = null;
            Session["CC"] = null;


            Response.Write("<div style='text-align:center;margin-top:150px;font-size:24px;'>邮件发送成功，3秒后这个页面将自杀</div>");
            Response.Write("<script>setTimeout('this.close();', 3000);</script>");
            Response.End();
        }
        else
        {
            LabError.Text = "发送失败！！！！<br/>具体信息如下<br/>" + result;
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "eorr", "BtnSubmit_Clicked();", true);
        }



    }

    private List<Attachment> AddAttachment(CallInfo info, CallStepInfo StepJobCode)
    {
        List<Attachment> list = new List<Attachment>();
        string FileFullName = string.Empty;
        foreach (string item in TxbAttachment.Text.Trim().Split(','))
        {
            int ID = Function.ConverToInt(item);
            AttachmentInfo ainfo = AttachmentBLL.Get(ID);
            if (null == ainfo)
            {
                continue;
            }
            FileFullName = ainfo.FilePath + ainfo.Title + ainfo.Ext;
            if (!File.Exists(FileFullName))
            {
                continue;
            }
            string SavePath = AttachmentBLL.GetAttachmentPathTemp() + DateTime.Now.ToFileTime() + "\\";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            SavePath += ainfo.Memo;
            //Response.Write(string.Format("FileFullName is:{0};<br/>SavePath is:{1}", FileFullName, SavePath));
            //Response.End();
            File.Copy(FileFullName, SavePath, true);
            Attachment attinfo = new Attachment(SavePath);
            list.Add(attinfo);
        }

        if (RblTemplateID.SelectedValue != "0")
        {
            string DocResult = BillRecBLL.ReduceBill(info, StepJobCode, Function.ConverToInt(RblTemplateID.SelectedValue));
            if (string.IsNullOrEmpty(DocResult))
            {
                LabError.Text = "服务单生成失败";
            }
            list.Add(new Attachment(DocResult));
        }

        return list;
    }

}
