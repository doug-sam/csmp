using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class page_CallStep_listview : BasePage
{

    private const string SlnTxt = "最后在{0}解决，共经过{1}次处理，解决方案是：{2}";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            CallInfo info = GetInfo();
            if (null == info)
            {
                return;
            }
            if (info.StateMain == (int)SysEnum.CallStateMain.未处理)
            {
                Response.Write("此报修未有任何处理记录！");
                Response.End(); return;
            }
            List<CallStepInfo> list = CallStepBLL.GetListJoin(info);
            GridView1.DataSource = list;
            GridView1.DataBind();
            if (list.Count > 0)
            {
                CallStepInfo csinfoSlove = null;
                int StepCount = list.Count;
                foreach (CallStepInfo item in list)
                {
                    if (item.IsSolved)
                    {
                        csinfoSlove = item;
                    }
                    if (item.StepType == (int)SysEnum.StepType.回访)
                    {
                        StepCount--;
                    }
                }
                if (null != csinfoSlove)
                {
                    LtlSolution.Text = string.Format(SlnTxt, csinfoSlove.DateBegin.ToString("yyyy-MM-dd HH:mm"), StepCount, info.SloveBy);
                }
            }

            if (RockBackPowerCheck(info)&&CurrentUser.Name=="林华夏")
            {
                BtnRockBack.Visible = true;
            }
        }
    }



    public CallInfo GetInfo()
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


    protected string IsSloved(string IsSloved, string StepType)
    {
        int StepTypeInt = Function.ConverToInt(StepType);
        if (StepTypeInt == (int)SysEnum.StepType.回访 || StepTypeInt == (int)SysEnum.StepType.关单)
        {
            return string.Empty;
        }
        return IsSloved.Trim().ToLower() == "true" ? "已解决" : "未解决";
    }


    /// <summary>
    /// 用户是否有权限处理该报修
    /// </summary>
    /// <param name="CallID"></param>
    public bool RockBackPowerCheck(CallInfo info)
    {//这里的if判断顺序很重要的，不要上下动
        if (IsAdmin)
        {
            return true;
        }
        if (null == info)
        {
            return false;
        }
        if (info.StateMain == (int)SysEnum.CallStateMain.已关闭 || info.StateMain == (int)SysEnum.CallStateMain.未处理)
        {
            return false;
        }
        if (!WorkGroupBrandBLL.HasRelaction(CurrentUser.WorkGroupID, info.BrandID))
        {
            return false;
        }
        if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.操作回滚))
        {
            return false;
        }
        if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.处理组内所有报修))
        {
            return false;
        }
        return true;
    }

    protected void BtnRockBack_Click(object sender, EventArgs e)
    {
        CallInfo info = GetInfo();
        if (null == info)
        {
            Function.AlertMsg("数据有误");
            return;
        }
        string Result = CallStepBLL.DeleteCallStep_UpdateCall(info);
        if (!string.IsNullOrEmpty(Result))
        {
            Function.AlertRefresh(Result);
        }
        else
        {
            Function.AlertRefresh("操作成功");
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        int ID = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "ID").ToString());
        int StepType = Function.ConverToInt(DataBinder.Eval(e.Row.DataItem, "StepType").ToString());
        HyperLink HyperLink1 = (HyperLink)e.Row.FindControl("HyperLink1");

        if (StepType == (int)SysEnum.StepType.上门安排)//&& GetInfo().StateMain != (int)SysEnum.CallStateMain.已关闭
        {
            HyperLink1.NavigateUrl = "javascript:tb_show('编辑', '/page/CallStep/Edit.aspx?ID=" + ID + "&TB_iframe=true&height=400&width=700', false);";
            HyperLink1.ToolTip = "点击修改";
        }
        else
        {
            HyperLink1.NavigateUrl = "#";
            HyperLink1.Enabled = false;
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

    public string GenerateATagPrefix(string index, string details)
    {
        CallInfo info = GetInfo();
        if (null == info)
            return "";
        string recID = ProcessDetails(details);
        if (index == "0")
            return "<a href='/page/Record/Play.aspx?ID=" + info.ID + "&RecID=" + recID + "'>";
        return "";
    }

    public string GenerateATagSufix(string index)
    {
        CallInfo info = GetInfo();
        if (null == info)
            return "";
        if (index == "0")
            return "</a>";
        return "";
    }
}
