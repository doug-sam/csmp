using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_FeedbackChoose_Add : _Feedback
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            FeedbackQuestionInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("参数有误");
                return;
            }
            if (info.Type != (int)SysEnum.QuestionType.Check && info.Type != (int)SysEnum.QuestionType.Radio)
            {
                Function.AlertBack("非选择题！");
                return;
            }


        }
    }
   
    
    private FeedbackQuestionInfo GetInfo()
    {
        if (ViewState["INFO"] != null)
        {
            return (FeedbackQuestionInfo)ViewState["INFO"];
        }
        int ID = Function.GetRequestInt("QuestionID");
        if (ID <= 0)
        {
            return null;
        }
        FeedbackQuestionInfo info = FeedbackQuestionBLL.Get(ID);
        if (info != null)
        {
            ViewState["INFO"] = info;
        }
        return info;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {


        if (!DataValidator.IsLen(TxbName.Text.Trim(), UpdatePanel1, this.GetType(), "选项", 2, 50))
        {
            return;
        }
        if (!DataValidator.IsNumber(TxbOrderNumber.Text.Trim(), UpdatePanel1, GetType(), "OrderNumber", 0, 999))
        {
            return;
        }
        FeedbackQuestionInfo infoq = GetInfo();

        FeedbackChooseInfo infoc = new FeedbackChooseInfo();
        infoc.AddDate = DateTime.Now;
        infoc.Enable = CbEnable.Checked;
        infoc.IsDefault = false;
        infoc.Name = TxbName.Text.Trim();
        infoc.OrderNumber = Function.ConverToInt(TxbOrderNumber.Text.Trim(), 0);
        infoc.QuestionID = infoq.ID;
        

        if (FeedbackChooseBLL.Add(infoc)>0)
        {

            Function.AlertRefresh( "添加成功", "main");
        }
        else
        {
            Function.AlertMsg("添加失败");
        }
    }
}
