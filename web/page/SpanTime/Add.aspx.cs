using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_SpanTime_Add : _BaseData_ProvinceCity
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbDateBegin.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            TxbDateEnd.Text = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm");
            CallInfo info = GetInfo();
            if (null==info)
            {
                Function.AlertBack("参数有误");
            }
            if (SpanTimeBLL.IsInStop(info.ID))
            {
                Response.Redirect("Edit.aspx?ID="+info.ID);
            }
            if (info.StateMain==(int)SysEnum.CallStateMain.已关闭||info.StateMain==(int)SysEnum.CallStateMain.已完成)
            {
                Tb_Add.Visible = false;
                UClist1.BtnDelete_Hide();
            }
            UClist1.BindData(info.ID);
        }
    }

    public CallInfo GetInfo()
    {
        if (null != ViewState["info"])
        {
            return (CallInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID <= 0)
        {
            return null;
        }
        return CallBLL.Get(ID);
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        DateTime DateBegin = Function.ConverToDateTime(TxbDateBegin.Text.Trim());
        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        if (DateBegin == Function.ErrorDate)
        {
            Function.AlertBack("请认真填写开始时间");
        }
        if (DateEnd == Function.ErrorDate)
        {
            Function.AlertBack("请认真填写结束时间");
        }
        if (DateBegin>=DateEnd)
        {
            Function.AlertBack("开始日期不能大于结束日期");
        }
        string Reason = Function.GetRequestSrtring("inputReason").Trim(',');
        Reason = Reason.Replace("其它原因,", "");
        if (Reason.Length==0)
        {
            Function.AlertBack("请填写暂停原因");
        }
        if (Reason.Length>50)
        {
            Function.AlertBack("暂停原因不能超过50字");
        }
        if (TxbMemo.Text.Trim().Length>100)
        {
            Function.AlertBack("备注不能超过100字");
        }
        SpanTimeInfo info = new SpanTimeInfo();
        info.AddDate = DateTime.Now;
        info.CallID = GetInfo().ID;
        info.CallNo = GetInfo().No;
        info.DateBegin = Function.ConverToDateTime(TxbDateBegin.Text.Trim());
        info.DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        info.Hours =(decimal) Math.Round((info.DateEnd - info.DateBegin).TotalHours, 2);
        info.Memo = TxbMemo.Text.Trim();
        info.Reason = Reason;
        info.StartupBy = 0;
        info.TotalMinutes = (int)(info.DateEnd - info.DateBegin).TotalMinutes;
        info.UserIDStart = CurrentUser.ID;
        info.UserIDStop = CurrentUser.ID;
        info.UserNameStart = CurrentUser.Name;
        info.UserNameStop = CurrentUser.Name;
        if (SpanTimeBLL.Add(info)>0)
        {
            Function.AlertRefresh("提交成功");
        }
        else
        {
            Function.AlertBack("提交失败");
        }
    }
}
