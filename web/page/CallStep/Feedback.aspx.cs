using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_callStep_Feedback : _Call_Feedback
{
    private const string JS = "window.clipboardData.setData('Text','AAAA$$$$BBBB$$$$CCCC::{0}');";
    public string CTIWSIP
    {
        get { return ConfigurationManager.AppSettings["CTIWSIP"].ToString(); }
    }
    public string CTIWSPort
    {
        get { return ConfigurationManager.AppSettings["CTIWSPort"].ToString(); }
    }
    public string CTIWSObAniName
    {
        get { return ConfigurationManager.AppSettings["CTIWSObAniName"].ToString(); }
    }
    public string CTIWSObDnisName
    {
        get { return ConfigurationManager.AppSettings["CTIWSObDnisName"].ToString(); }
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            NotMyCallCheck(info.ID);


            List<FeedbackPaperInfo> listinfo = FeedbackPaperBLL.GetList();
            if (listinfo == null || listinfo.Count == 0)
            {
                Function.AlertBack("没有调查问卷资料。没法回访");
                return;
            }
            FeedbackPaperInfo infopaper = listinfo[0];
            LabPaperMeno.Text = infopaper.Memo;
            ViewState["PaperID"] = infopaper.ID;
            List<FeedbackQuestionInfo> listQRadio = FeedbackQuestionBLL.GetList(infopaper.ID, SysEnum.QuestionType.Radio);
            RpRadio.DataSource = listQRadio;
            RpRadio.DataBind();

            RpEssay.DataSource = FeedbackQuestionBLL.GetList(infopaper.ID, SysEnum.QuestionType.Essay);
            RpEssay.DataBind();



            
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            if (info.StateMain != (int)SysEnum.CallStateMain.已完成)
            {
                Function.AlertBack("数据有误，该call未完成");
                return;
            }

            CallStepInfo sinfo = GetFeedback(info);
            if (null != sinfo)
            {
                BtnSubmit.Visible = false;
                TableCall.Visible = false;
                TxbFeedbackUserName.Text = ProcessDetails(sinfo.Details);
            }


            StoreInfo stinfo = StoresBLL.Get(info.StoreID);
            if (null != stinfo)
            {
                TxbTel.Text = stinfo.Tel;
                //这里听说放进剪切板就可以打出电话了。神奇
                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "make a call", string.Format(JS, stinfo.Tel), true);
            }
        }
    }

    private CallStepInfo GetFeedback(CallInfo info)
    {
        if (ViewState["FeedbackInfo"] == null)
        {
            CallStepInfo sinfo = CallStepBLL.GetLast(info.ID, SysEnum.StepType.回访);
            if (null == sinfo)
            {
                ViewState["FeedbackInfo"] = "";
            }
            else
            {
                ViewState["FeedbackInfo"] = sinfo;
            }
            return sinfo;
        }
        return string.IsNullOrEmpty(ViewState["FeedbackInfo"].ToString()) ? null : (CallStepInfo)ViewState["FeedbackInfo"];
    }


    protected List<FeedbackAnswerInfo> GetRadio()
    {
        List<FeedbackAnswerInfo> listanswer = new List<FeedbackAnswerInfo>();
        FeedbackQuestionInfo qinfo = new FeedbackQuestionInfo();
        for (int i = 0; i < RpRadio.Items.Count; i++)
        {
            
            RadioButtonList ct1 = (RadioButtonList)RpRadio.Items[i].FindControl("RadioButtonList1");
            Label Ct2 = ((Label)RpRadio.Items[i].FindControl("LabQuestionID"));
            int QuestionID = Function.ConverToInt(Ct2.Text, 0);
            if (QuestionID <= 0)
            {
                return null;
            }
            if (ct1.SelectedValue==null||Function.ConverToInt(ct1.SelectedValue)<=0)
            {
                return null;
            }
            FeedbackAnswerInfo ansinfo = new FeedbackAnswerInfo();
            ansinfo.PaperID = Function.ConverToInt(ViewState["PaperID"], 0);
            ansinfo.QuestionID = QuestionID;
            ansinfo.Answer = ct1.SelectedItem.Text;
            ansinfo.Answer2 = Function.ConverToInt(ct1.SelectedValue, 0);
            ansinfo.RecorderID = CurrentUserID;
            ansinfo.RecorderName = CurrentUserName;
            ansinfo.FeedbackUserName = TxbFeedbackUserName.Text.Trim();
            ansinfo.CallID = GetInfo().ID;
            ansinfo.CallStepID = 0;
            ansinfo.AddDate = DateTime.Now;

            qinfo = FeedbackQuestionBLL.Get(ansinfo.QuestionID);
            if (null == qinfo)
            {
                continue;
            }
            ansinfo.QuestionName = qinfo.Name;

            if (string.IsNullOrEmpty(ansinfo.Answer))
            {
                return null;
            }
            listanswer.Add(ansinfo);
        }
        return listanswer;
    }

    protected List<FeedbackAnswerInfo> GetEssay()
    {
        List<FeedbackAnswerInfo> listanswer = new List<FeedbackAnswerInfo>();
        FeedbackQuestionInfo qinfo = new FeedbackQuestionInfo();
        for (int i = 0; i < RpEssay.Items.Count; i++)
        {
            TextBox Ct1 = (TextBox)RpEssay.Items[i].FindControl("TxbAnswer");
            Label Ct2 = ((Label)RpEssay.Items[i].FindControl("LabQuestionID"));
            int QuestionID = Function.ConverToInt(Ct2.Text, 0);
            if (QuestionID <= 0)
            {
                return null;
            } 
            FeedbackAnswerInfo ansinfo = new FeedbackAnswerInfo();
            ansinfo.PaperID = Function.ConverToInt(ViewState["PaperID"], 0);
            ansinfo.QuestionID = QuestionID;
            ansinfo.Answer = Ct1.Text.Trim();
            ansinfo.Answer2 = 0;
            ansinfo.RecorderID = CurrentUserID;
            ansinfo.RecorderName = CurrentUserName;
            ansinfo.FeedbackUserName = TxbFeedbackUserName.Text.Trim();
            ansinfo.CallID = GetInfo().ID;
            ansinfo.CallStepID = 0;
            ansinfo.AddDate = DateTime.Now;

            qinfo = FeedbackQuestionBLL.Get(ansinfo.QuestionID);
            if (null == qinfo)
            {
                continue;
            }
            ansinfo.QuestionName = qinfo.Name;
            if (string.IsNullOrEmpty(ansinfo.Answer))
            {
                return null;
            }
            listanswer.Add(ansinfo);
        }
        return listanswer;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        string callbackrecordid = Request["callbackrecordid"];
        int POS1 = callbackrecordid.IndexOf("#");
        if ((POS1 + 1) >= callbackrecordid.Length)
            POS1 = -1;
        int POS2 = callbackrecordid.IndexOf("#", POS1 + 1);

        if (POS1 != -1 && POS2 != -1 && POS2 > POS1)
        {
            callbackrecordid = callbackrecordid.Substring(POS1 + 1, POS2 - POS1 - 1);
            POS1 = callbackrecordid.IndexOf("=");
            if ( POS1 != -1 )
                callbackrecordid = callbackrecordid.Substring(POS1 + 1);
            else
                callbackrecordid = "";
        }
        else
            callbackrecordid = "";
        if (string.IsNullOrEmpty(TxbFeedbackUserName.Text.Trim()))
        {
            Function.AlertMsg("受访人不能为空"); return;
        }
        if (TxbFeedbackUserName.Text.Trim().Length > 50)
        {
            Function.AlertMsg("受访人长度不能超50字"); return;
        }



        CallInfo cinfo = GetInfo();


        CallStepInfo info = new CallStepInfo();
        info.StepType = (int)SysEnum.StepType.回访;
        info.AddDate = DateTime.Now;
        info.CallID = cinfo.ID;
        info.DateBegin = info.DateEnd = DateTime.Now;
        if ( string.IsNullOrEmpty(callbackrecordid) )
            info.Details = TxbFeedbackUserName.Text.Trim();
        else
            info.Details = TxbFeedbackUserName.Text.Trim() + "  A$B$C" + callbackrecordid + "D$E$F";
        info.MajorUserID = CurrentUserID;
        info.MajorUserName = CurrentUserName;
        info.SolutionID = 0;
        info.SolutionName = "";
        info.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
        info.StepName = SysEnum.StepType.回访.ToString();
        info.UserID = CurrentUserID;
        info.UserName = CurrentUserName;
        cinfo.StateDetail = (int)SysEnum.CallStateDetails.已回访;
        //cinfo.StateMain = (int)SysEnum.CallStateMain.已关闭;
        info.IsSolved = false;
        List<FeedbackAnswerInfo> listradio = GetRadio();
        List<FeedbackAnswerInfo> listessay = GetEssay();
        if (null == listradio || null == listessay)
        {
            Function.AlertMsg("问卷未全部回答"); return;
        }
        if (CallStepBLL.AddCallStep_UpdateCall(cinfo, info))
        {
            int FlagFailed = 0;

            foreach (FeedbackAnswerInfo item in listradio)
            {
                FlagFailed += FeedbackAnswerBLL.Add(item) > 0 ? 0 : 1;
            }
            foreach (FeedbackAnswerInfo item in listessay)
            {
                FlagFailed += FeedbackAnswerBLL.Add(item) > 0 ? 0 : 1;
            }

            string NextUrl = "/page/call/list.aspx?state=" + (int)SysEnum.CallStateMain.已完成;
            if (CbNewCall.Checked)
            {
                StoreInfo sinfo = StoresBLL.Get(cinfo.StoreID);
                if (null != sinfo)
                    NextUrl = "/page/call/add.aspx?CallNumber=" + sinfo.Tel;
            }
            Function.AlertRedirect("提交成功" + (FlagFailed == 0 ? "" : FlagFailed + "条回答没有记录"), NextUrl, "main");
        }
        else
        {
            Function.AlertBack("提交失败");
        }
    }
    protected void RpRadio_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item == null) return;
        int id = Function.ConverToInt(DataBinder.Eval(e.Item.DataItem, "ID"));
        RadioButtonList Ct1 = ((RadioButtonList)e.Item.FindControl("RadioButtonList1"));
        Ct1.DataSource = FeedbackChooseBLL.GetList(id);
        Ct1.DataBind();
        if (null != GetFeedback(GetInfo()))
        {
            FeedbackAnswerInfo ainfo = FeedbackAnswerBLL.Get(id, GetInfo().ID);
            if (null != ainfo)
            {
                Ct1.SelectedValue = ainfo.Answer2.ToString();
            }
        }
    }
    protected void RpEssay_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item == null) return;
        if (null != GetFeedback(GetInfo()))
        {
            int id = Function.ConverToInt(DataBinder.Eval(e.Item.DataItem, "ID"));
            TextBox Ct1 = ((TextBox)e.Item.FindControl("TxbAnswer"));
            FeedbackAnswerInfo ainfo = FeedbackAnswerBLL.Get(id, GetInfo().ID);
            if (null != ainfo)
            {
                Ct1.Text = ainfo.Answer;
            }
        }
    }
    protected string ProcessDetails(string Details)
    {
        string recordid = Details;
        int POS1 = recordid.IndexOf("A$B$C");
        int POS2 = recordid.IndexOf("D$E$F");
        if (POS1 != -1 && POS2 != -1 && POS2 > POS1)
            recordid = recordid.Remove(POS1);
        return recordid;
    }



}
