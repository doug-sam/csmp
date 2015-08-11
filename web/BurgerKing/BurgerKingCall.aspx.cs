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

public partial class BurgerKing_BurgerKingCall : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string result = Request["param"];
        result = HttpUtility.UrlDecode(result);
        Logger.GetLogger(this.GetType()).Info("BK调用汉堡王完成接口，传入参数为" + result, null);
        result = DoComplete(result);
        Context.Response.ContentType = "application/json";
        Context.Response.Write(result);
    }

    protected string DoComplete(string result) 
    {
        if (result.Contains("cNumber") && result.Contains("solutionDetails") && result.Contains("TSI"))
        {
            JObject obj = null;
            try
            {
                obj = JObject.Parse(result);
            }
            catch (Exception ex)
            {
                result = "{\"status\":true,\"errNo\":101,\"Desc\":\"JSON格式不正确\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王完成接口，错误原因为" + result, null);
                return result;
            }
            string cNumber = obj["cNumber"].ToString();
            cNumber = cNumber.Trim('"');
            string solutionDetails = obj["solutionDetails"].ToString();
            string TSI = obj["TSI"].ToString();
            TSI = TSI.Trim('"');
            int TSIid = 0;
            if (string.IsNullOrEmpty(cNumber))
            {
                result = "{\"status\":true,\"errNo\":103,\"Desc\":\"执行失败，cNumber值不能为空\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王完成接口，错误原因为" + result, null);
                return result;
            }

            if (solutionDetails.Substring(0, 1) == "\"")
            {
                solutionDetails = solutionDetails.Remove(0, 1);
            }
            if (solutionDetails.Substring(solutionDetails.Length - 1, 1) == "\"")
            {
                solutionDetails = solutionDetails.Remove(solutionDetails.Length - 1, 1);
            }

            if (solutionDetails.Length > 500)
            {
                result = "{\"status\":true,\"errNo\":104,\"Desc\":\"执行失败，solutionDetails处理过程备注不能超过500字\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王完成接口，错误原因为" + result, null);
                return result;
            }
            if (string.IsNullOrEmpty(TSI))
            {
                TSI = "汉堡王";
            }
            else { 
                string tpWhere = "f_Name like '%&$&"+TSI.Trim()+"%'";
                List<ThirdPartyInfo> tpList = ThirdPartyBLL.GetList(tpWhere);
                if (tpList.Count > 0)
                {
                    TSI = tpList[0].Name;
                    TSIid = tpList[0].ID;
                }
            }

            CallInfo cinfo = CallBLL.Get(cNumber);
            if (cinfo == null)
            {
                result = "{\"status\":true,\"errNo\":105,\"Desc\":\"执行失败，单号：" + cNumber + "不存在\"}";
                Logger.GetLogger(this.GetType()).Info("BK调用汉堡王完成接口，错误原因为" + result, null);
                return result;
            }
            else
            {
                CallStepInfo sinfo = new CallStepInfo();
                sinfo.StepType = (int)SysEnum.StepType.第三方处理离场;
                sinfo.MajorUserID = TSIid;
                sinfo.MajorUserName = TSI;
                sinfo.AddDate = DateTime.Now;
                sinfo.CallID = cinfo.ID;
                sinfo.DateBegin = DateTime.Now; 
                sinfo.DateEnd = sinfo.DateBegin;
                sinfo.Details = solutionDetails;
                sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
                sinfo.IsSolved = true;
                sinfo.SolutionID = -2;
                sinfo.SolutionName = "第三方解决";
                cinfo.StateMain = (int)SysEnum.CallStateMain.已完成;
                cinfo.StateDetail = (int)SysEnum.CallStateDetails.处理完成;
                cinfo.SloveBy = SysEnum.SolvedBy.第三方.ToString();
                cinfo.FinishDate = sinfo.DateBegin;
                sinfo.StepName = SysEnum.CallStateDetails.第三方处理离场.ToString();
                //sinfo.UserID = CurrentUser.ID;
                sinfo.UserName = "汉堡王";

                //sinfo.StepType = (int)SysEnum.StepType.升级到客户;
                ////sinfo.MajorUserID = CurrentUserID;
                //sinfo.MajorUserName = "汉堡王推送";
                //sinfo.AddDate = DateTime.Now;
                //sinfo.CallID = cinfo.ID;
                //sinfo.DateBegin = DateTime.Now;
                //sinfo.DateEnd = sinfo.DateBegin;
                //sinfo.Details = solutionDetails;
                //sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(cinfo.ID) + 1;
                //sinfo.IsSolved = true;
                //sinfo.SolutionID = -1;
                //sinfo.SolutionName = "客户自行解决";
                //cinfo.StateMain = (int)SysEnum.CallStateMain.已完成;
                //cinfo.StateDetail = (int)SysEnum.CallStateDetails.处理完成;
                //cinfo.SloveBy = SysEnum.SolvedBy.客户解决.ToString();
                //cinfo.FinishDate = sinfo.DateBegin;
                //sinfo.StepName = SysEnum.CallStateDetails.升级到客户.ToString();
                ////sinfo.UserID = CurrentUser.ID;
                //sinfo.UserName = "汉堡王推送";

                if (CallStepBLL.AddCallStep_UpdateCall(cinfo, sinfo))
                {
                    result = "{\"status\":true,\"errNo\":0,\"Desc\":\"执行成功\"}";
                    Logger.GetLogger(this.GetType()).Info("BK调用汉堡王完成接口，执行成功", null);
                }
                else
                {
                    result = "{\"status\":true,\"errNo\":106,\"Desc\":\"执行失败,提交失败。请联系管理员\"}";
                    Logger.GetLogger(this.GetType()).Info("BK调用汉堡王完成接口，错误原因为" + result, null);
                    
                }
                return result;
            }
        }
        else
        {
            result = "{\"status\":true,\"errNo\":102,\"Desc\":\"执行失败，无效的url参数\"}";
            Logger.GetLogger(this.GetType()).Info("BK调用汉堡王完成接口，错误原因为" + result, null);
            return result;
        }
    
    }
}
