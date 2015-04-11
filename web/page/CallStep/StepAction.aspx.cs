using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class page_CallStep_StepAction : _Call_Step
{
    private void OutPutError(string Content)
    {
        Response.Write("error:"+Content);
        Response.End();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        int CallID = Function.GetRequestInt("ID");
        int CallStepID = Function.GetRequestInt("StepID");
        CallInfo info = CallBLL.Get(CallID);
        CallStepInfo sinfo = null;
        if (CallStepID>0)
        {
            sinfo= CallStepBLL.Get(CallStepID);
        }
        else
        {
            sinfo = CallStepBLL.GetLast(CallID);
        }
        if (null==info||info.StateMain==(int)SysEnum.CallStateMain.已关闭)
        {
            OutPutError("数据有误");
            return;
        }
        if ((null==sinfo &&info.StateMain!=(int)SysEnum.CallStateMain.未处理)||sinfo.ID<=0||sinfo.CallID!=CallID)
        {
            OutPutError("数据有误");
            return;
        }


    }


}