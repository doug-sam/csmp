using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

public partial class StepSwitchAfterFinish : _Call_Step
{
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CallInfo info = GetInfo();
            if (null == info)
            {
                Function.AlertBack("数据有误。");
            }
            if (info.StateMain != (int)SysEnum.CallStateMain.已完成)
            {
                Function.AlertBack("数据有误，无法处理已完成的call");
                return;
            }

            string js = string.Empty;
            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.回访))
            {
                js="$('#BtnFeedback').hide();";
                
            }
            else
            {
                CallStepInfo stepFeedback = CallStepBLL.GetLast(info.ID, SysEnum.StepType.回访);
                if (null != stepFeedback)
                {
                    js += "SetHaveFeedback();";
                }
            }


            if (!GroupBLL.PowerCheck((int)PowerInfo.P1_Call.关闭报修))
            {
                js += "$('#BtnClose').hide();";
            }

           //List<CallStepInfo> listReplacement = CallStepBLL.GetList(info.ID, SysEnum.StepType.备件维修);
           //if (listReplacement.Count>0)
           //{
           //    js += "SetReplacementCount(" + listReplacement.Count + ");";

           //}
           ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "dd", js, true);
            
            #region 用户检查
            NotMyCallCheck(info.ID);
            #endregion

        }
    }

}

