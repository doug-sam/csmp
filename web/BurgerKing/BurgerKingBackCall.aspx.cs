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
        Logger.GetLogger(this.GetType()).Info("BK调用汉堡王转回接口，传入参数为"+result,null);
        result = DoComplete(result);
        Context.Response.ContentType = "application/json";
        Context.Response.Write(result);

    }
    protected string DoComplete(string result)
    {
        if (result.Contains("cNumber") && result.Contains("backReason"))
        {
            JObject obj = null;
            try
            {
                obj = JObject.Parse(result);
            }
            catch (Exception ex)
            {
                result = "{\"status\":true,\"errNo\":101,\"Desc\":\"JSON格式不正确\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王转回接口，错误原因为" + result, null);
                return result;
            }
            
            string cNumber = obj["cNumber"].ToString();
            cNumber = cNumber.Trim('"');
            string backReason = obj["backReason"].ToString();

            if (string.IsNullOrEmpty(cNumber))
            {
                result = "{\"status\":true,\"errNo\":103,\"Desc\":\"执行失败，cNumber值不能为空\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王转回接口，错误原因为" + result, null);
                return result;
            }

            if (backReason.Substring(0, 1) == "\"")
            {
                backReason = backReason.Remove(0, 1);
            }
            if (backReason.Substring(backReason.Length - 1, 1) == "\"")
            {
                backReason = backReason.Remove(backReason.Length - 1, 1);
            }

            if (backReason.Length > 500)
            {
                result = "{\"status\":true,\"errNo\":104,\"Desc\":\"执行失败，backReason转回原因不能超过500字\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王转回接口，错误原因为" + result, null);
                return result;
            }
            CallInfo cinfo = CallBLL.Get(cNumber);
            if (cinfo == null)
            {
                result = "{\"status\":true,\"errNo\":105,\"Desc\":\"执行失败，单号：" + cNumber + "不存在\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王转回接口，错误原因为" + result, null);
                return result;
            }
            else
            {
                CallStepInfo sinfo = new CallStepInfo();
                sinfo.StepType = (int)SysEnum.StepType.升级到客户;
                //sinfo.MajorUserID = CurrentUserID;
                sinfo.MajorUserName = "汉堡王";
                sinfo.AddDate = DateTime.Now;
                sinfo.CallID = cinfo.ID;
                sinfo.DateBegin = DateTime.Now;
                sinfo.DateEnd = sinfo.DateBegin;
                sinfo.Details = "汉堡王转回。" + backReason;
                sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
                
                sinfo.IsSolved = false;
                sinfo.SolutionID = 0;
                sinfo.SolutionName = "";
                cinfo.StateMain = (int)SysEnum.CallStateMain.处理中;
                cinfo.StateDetail = (int)SysEnum.CallStateDetails.第三方处理离场;

                sinfo.StepName = SysEnum.CallStateDetails.第三方处理离场.ToString();
                //sinfo.UserID = CurrentUser.ID;
                sinfo.UserName = "汉堡王";

                if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
                {
                    result = "{\"status\":true,\"errNo\":0,\"Desc\":\"执行成功\"}";
                    Logger.GetLogger(this.GetType()).Info("BK调用汉堡王转回接口，执行成功", null);
                }
                else
                {
                    result = "{\"status\":true,\"errNo\":106,\"Desc\":\"执行失败,提交失败。请联系管理员\"}";
                    Logger.GetLogger(this.GetType()).Info("BK调用汉堡王转回接口，错误原因为" + result, null);

                }
                return result;
            }
        }
        else
        {
            result = "{\"status\":true,\"errNo\":102,\"Desc\":\"执行失败，无效的url参数\"}";
            Logger.GetLogger(this.GetType()).Info("BK调用汉堡王转回接口，错误原因为" + result, null);
            return result;
        }

    }
}
