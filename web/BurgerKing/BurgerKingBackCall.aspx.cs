using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using Newtonsoft.Json.Linq;

public partial class BurgerKing_BurgerKingBackCall : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string result = Request["param"];
        result = HttpUtility.UrlDecode(result);
        result = DoComplete(result);
        Context.Response.ContentType = "application/json";
        Context.Response.Write(result);

    }
    protected string DoComplete(string result)
    {
        if (result.Contains("cNumber") && result.Contains("backReason"))
        {
            JObject obj = JObject.Parse(result);
            string cNumber = obj["cNumber"].ToString();
            cNumber = cNumber.Trim('"');
            string solutionDetails = obj["backReason"].ToString();
            if (string.IsNullOrEmpty(cNumber))
            {
                result = "{\"status\":true,\"errNo\":103,\"Desc\":\"执行失败，cNumber值不能为空\"}";
                return result;
            }
            if (solutionDetails.Length > 500)
            {
                result = "{\"status\":true,\"errNo\":104,\"Desc\":\"执行失败，backReason转回原因不能超过500字\"}";
                return result;
            }
            CallInfo cinfo = CallBLL.Get(cNumber);
            if (cinfo == null)
            {
                result = "{\"status\":true,\"errNo\":105,\"Desc\":\"执行失败，单号：" + cNumber + "不存在\"}";
                return result;
            }
            else
            {
                CallStepInfo sinfo = new CallStepInfo();
                sinfo.StepType = (int)SysEnum.StepType.升级到客户;
                //sinfo.MajorUserID = CurrentUserID;
                sinfo.MajorUserName = "汉堡王转回";
                sinfo.AddDate = DateTime.Now;
                sinfo.CallID = cinfo.ID;
                sinfo.DateBegin = DateTime.Now;
                sinfo.DateEnd = sinfo.DateBegin;
                sinfo.Details = solutionDetails;
                sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
                
                sinfo.IsSolved = false;
                sinfo.SolutionID = 0;
                sinfo.SolutionName = "";
                cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
                cinfo.StateDetail = (int)SysEnum.CallStateDetails.第三方处理离场;

                //sinfo.UserID = CurrentUser.ID;
                sinfo.UserName = "汉堡王转回";

                if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
                {
                    result = "{\"status\":true,\"errNo\":0,\"Desc\":\"执行成功\"}";
                }
                else
                {
                    result = "{\"status\":true,\"errNo\":106,\"Desc\":\"执行失败,提交失败。请联系管理员\"}";

                }
                return result;
            }
        }
        else
        {
            result = "{\"status\":true,\"errNo\":102,\"Desc\":\"执行失败，无效的url参数\"}";
            return result;
        }

    }
}
