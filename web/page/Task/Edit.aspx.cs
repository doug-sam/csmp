using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_Task_Edit : AdminPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DdlCycleMode.DataSource = SysEnum.ToDictionary(typeof(TaskInfo.CycleModeInfo));
            DdlCycleMode.DataBind();
            DdlCycleMode.Items.Insert(0, new ListItem("请选择", "0"));
            

            TaskInfo info = GetInfo();
            if (null == info)
            {
                TxbExcuteTimeLast.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {

                LabAction.Text = "编辑";
                foreach (ListItem item in DdlCycleMode.Items)
                {
                    if (item.Text==info.CycleMode.Trim())
                    {
                        DdlCycleMode.SelectedValue = item.Value;
                        break;
                    }
                }
                TxbName.Text = info.Name;
                TxbIntervalTime.Text = info.IntervalTime.ToString("G0");
                TxbExcuteTime.Text = info.ExcuteTime.ToString("yyyy-MM-dd HH:mm");
                TxbExcuteTimeLast.Text = info.ExcuteTimeLast.ToString("yyyy-MM-dd HH:mm");
                TxbURL.Text = info.URL;
                CbEnable.Checked = info.Enable;
            }
        }
    }

    private TaskInfo GetInfo()
    {
        TaskInfo info;
        if (ViewState["info"] != null)
        {
            info = (TaskInfo)ViewState["info"];
        }
        int ID = Function.GetRequestInt("ID");
        if (ID > 0)
        {
            info = TaskBLL.Get(ID);
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
        TaskInfo info = GetInfo();
        if (null == info)
        {
            info = new TaskInfo();
            info.ID = 0;
        }

        info.Name = TxbName.Text;
        info.IntervalTime = Function.ConverToDecimal(TxbIntervalTime.Text);
        info.ExcuteTime = Function.ConverToDateTime(TxbExcuteTime.Text);
        info.ExcuteTimeLast = Function.ConverToDateTime(TxbExcuteTimeLast.Text);
        info.CycleMode = DdlCycleMode.SelectedItem.Text;
        info.URL = TxbURL.Text;
        info.Enable = CbEnable.Checked;


        if (info.IntervalTime<=0)
        {
            Function.AlertMsg("时间间隔是无法识别的数字");
            return;
        }
        if (info.ExcuteTime == Function.ErrorDate)
        {
            Function.AlertMsg("执行时间无法识别");
            return;
        }
        if (info.ExcuteTimeLast == Function.ErrorDate)
        {
            Function.AlertMsg("最后执行时间无法识别");
            return;
        }
        if (DdlCycleMode.SelectedValue=="0")
        {
            Function.AlertMsg("请选择循环周期");
            return;
        }


        if (GetInfo() == null)
        {
            if (TaskBLL.Add(info) > 0)
            {
                Function.AlertMsg("添加成功");
            }
            else
            {
                Function.AlertMsg("添加失败");
            }
        }
        else
        {
            if (TaskBLL.Edit(info))
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
