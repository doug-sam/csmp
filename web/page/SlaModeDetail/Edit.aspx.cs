using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_SlaModeDetail_Edit : _BaseData_SLAModel
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DdlDayOfWeek.Items.Add(new ListItem("请选择", ""));
            DdlDayOfWeek.Items.Add(new ListItem("星期一", "星期一"));
            DdlDayOfWeek.Items.Add(new ListItem("星期二", "星期二"));
            DdlDayOfWeek.Items.Add(new ListItem("星期三", "星期三"));
            DdlDayOfWeek.Items.Add(new ListItem("星期四", "星期四"));
            DdlDayOfWeek.Items.Add(new ListItem("星期五", "星期五"));
            DdlDayOfWeek.Items.Add(new ListItem("星期六", "星期六"));
            DdlDayOfWeek.Items.Add(new ListItem("星期日", "星期日"));

            
            SlaModeDetailInfo info = GetInfo();
            if (null == info)
            {
                
            }
            else
            {
                LabAction.Text = "编辑";
                DdlDayOfWeek.SelectedValue = info.DayOfWeek;
                TxbDateStart.Text = info.TimerStart.ToString("HH:mm");
                TxbDateEnd.Text = info.TimeEnd.ToString("HH:mm");
            }
        }
    }

    private SlaModeDetailInfo GetInfo()
    {
        SlaModeDetailInfo info;
        if (ViewState["info"] != null)
        {
            info = (SlaModeDetailInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            
            info = SlaModeDetailBLL.Get(ID);
            if (null != info)
            {
                ViewState["info"] = info;
            }
            return info;
        }
        return null;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        SlaModeDetailInfo info = GetInfo();
        if (null == info)
        {
            info = new SlaModeDetailInfo();
            info.SlaModeID = Function.GetRequestInt("ModeID");
        }

        int startHour=Function.ConverToInt(TxbDateStart.Text.Substring(0,2));
        int startMinute=Function.ConverToInt(TxbDateStart.Text.Substring(3,2));
        int endHour=Function.ConverToInt(TxbDateEnd.Text.Substring(0,2));
        int endMinute=Function.ConverToInt(TxbDateEnd.Text.Substring(3,2));
        
        
        info.DayOfWeek = DdlDayOfWeek.SelectedValue;
        info.TimerStart = new DateTime(DicInfo.DateZone.Year, DicInfo.DateZone.Month, DicInfo.DateZone.Day, startHour, startMinute,0);
        info.TimeEnd = new DateTime(DicInfo.DateZone.Year, DicInfo.DateZone.Month, DicInfo.DateZone.Day, endHour, endMinute, 0);

        if (string.IsNullOrEmpty(info.DayOfWeek))
        {
            Function.AlertMsg("请选择工作日");
        }

        if (GetInfo() == null)
        {
            if (SlaModeDetailBLL.Add(info) > 0)
            {
                Function.AlertRefresh("添加成功","main");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (SlaModeDetailBLL.Edit(info))
            {
                Function.AlertRefresh("修改成功","main");
            }
            else
            {
                Function.AlertMsg("修改失败");
            }
        }

    }
}
