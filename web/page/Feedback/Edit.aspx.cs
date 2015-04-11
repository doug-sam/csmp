using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Feedback_Edit : _Feedback
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<FeedbackPaperInfo> listinfo = FeedbackPaperBLL.GetList();
            if (listinfo == null || listinfo.Count == 0)
            {
                Function.AlertBack("没有调查问卷资料。没法回访");
                return;
            }

            FeedbackPaperInfo infopaper = listinfo[0];
            ViewState["PaperID"] = infopaper.ID;
            List<FeedbackQuestionInfo> listQRadio = FeedbackQuestionBLL.GetList(infopaper.ID, SysEnum.QuestionType.Radio);
            RpRadio.DataSource = listQRadio;
            RpRadio.DataBind();

            RpEssay.DataSource = FeedbackQuestionBLL.GetList(infopaper.ID, SysEnum.QuestionType.Essay);
            RpEssay.DataBind();

        }
    }

    private CallInfo GetInfo()
    {
        CallInfo info;
        if (ViewState["info"] != null)
        {
            info = (CallInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = CallBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
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

    protected void RpRadio_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item == null) return;
       
        if (null != GetFeedback(GetInfo()))
        {
          int id = Function.ConverToInt(DataBinder.Eval(e.Item.DataItem, "ID"));
          Label Ct1 = ((Label)e.Item.FindControl("LabAnswer"));
          FeedbackAnswerInfo ainfo = FeedbackAnswerBLL.Get(id, GetInfo().ID);
            if (null != ainfo)
            {
                Ct1.Text = ainfo.Answer;
            }
        }
    }
    protected void RpEssay_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item == null) return;
        if (null != GetFeedback(GetInfo()))
        {
            int id = Function.ConverToInt(DataBinder.Eval(e.Item.DataItem, "ID"));
            Label Ct1 = ((Label)e.Item.FindControl("LabAnswer"));
            FeedbackAnswerInfo ainfo = FeedbackAnswerBLL.Get(id, GetInfo().ID);
            if (null != ainfo)
            {
                Ct1.Text = ainfo.Answer;
            }
        }
    }


}
