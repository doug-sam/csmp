using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CSMP.Model;
using CSMP.BLL;
using Tool;

public partial class page_SpanTime_Edit : _BaseData_ProvinceCity
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TxbDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            CallInfo info = GetInfo();
            if (null==info)
            {
                Function.AlertBack("参数有误");
            }
            if (info.StateMain==(int)SysEnum.CallStateMain.已关闭||info.StateMain==(int)SysEnum.CallStateMain.已完成)
            {
                Response.End(); return;
            }

            SpanTimeInfo StopIngInfo = SpanTimeBLL.GetStopInfo(info.ID);
            if (StopIngInfo == null)
            {
                Response.Redirect("Add.aspx?ID=" + info.ID);
            }
            ViewState["StopIngInfo"] = StopIngInfo;
            LabDateBegin.Text = StopIngInfo.DateBegin.ToString("yyyy-MM-dd HH:mm");
            LabDateEnd.Text = StopIngInfo.DateEnd.ToString("yyyy-MM-dd HH:mm");
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
        SpanTimeInfo StopIngInfo = (SpanTimeInfo)ViewState["StopIngInfo"];

        DateTime DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        if (DateEnd == Function.ErrorDate)
        {
            Function.AlertBack("请认真填写开始时间");
        }
        if (StopIngInfo.DateBegin>= DateEnd)
        {
            Function.AlertBack("开始日期不能大于结束日期");
        }
        StopIngInfo.DateEnd = Function.ConverToDateTime(TxbDateEnd.Text.Trim());
        StopIngInfo.Hours = (decimal)Math.Round((StopIngInfo.DateEnd - StopIngInfo.DateBegin).TotalHours, 2);
        StopIngInfo.StartupBy = 1;
        StopIngInfo.TotalMinutes = (int)(StopIngInfo.DateEnd - StopIngInfo.DateBegin).TotalMinutes;
        StopIngInfo.UserIDStart = CurrentUser.ID;
        StopIngInfo.UserNameStart = CurrentUser.Name;
        if (SpanTimeBLL.Edit(StopIngInfo))
        {
            Function.AlertRefresh("提交成功");
        }
        else
        {
            Function.AlertBack("提交失败");
        }
    }
}
