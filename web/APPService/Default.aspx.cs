﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;

public partial class APPService_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string parameter = Request["param"];
        string result = string.Empty;
        parameter = HttpUtility.UrlDecode(parameter);
        Logger.GetLogger(this.GetType()).Info("APP接口被调用,参数内容：" + parameter + "\r\n", null);
        if (string.IsNullOrEmpty(parameter))
        {
            result = "{\"code\":\"Unknown\",\"result\":\"101\",\"desc\":\"参数param值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP接口被调用" + result+"\r\n", null);
        }
        else
        {
            Logger.GetLogger(this.GetType()).Info("APP接口被调用，传入参数为" + parameter + "\r\n", null);
            result = DoComplete(parameter);
        }
        Context.Response.ContentType = "application/json";
        Context.Response.Write(result);
    }
    /// <summary>
    /// 处理参数
    /// </summary>
    /// <param name="JSONInfo"></param>
    /// <returns></returns>
    protected string DoComplete(string JSONInfo)
    {
        string result = string.Empty;
        //if (result.Contains("cNumber") && result.Contains("solutionDetails") && result.Contains("TSI"))
        if (JSONInfo.Contains("code"))
        {
            JObject obj = null;
            try
            {
                obj = JObject.Parse(JSONInfo);
            }
            catch (Exception ex)
            {
                result = "{\"code\":\"Unknown\",\"result\":\"103\",\"desc\":\"JSON格式不正确\"}";
                Logger.GetLogger(this.GetType()).Info("APP接口被调用，解析JSON数据错误，原因为" + result + "异常：" + ex.Message + "\r\n", null);
                return result;
            }
            string code = string.Empty;
            try
            {
                code = obj["code"].ToString();
            }
            catch (Exception ex)
            {
                result = "{\"code\":\"Unknown\",\"result\":\"102\",\"desc\":\"param值中的参数无效\"}";
                Logger.GetLogger(this.GetType()).Info("APP接口被调用，解析JSON数据获取code值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
                return result;
            }
            code = code.Trim('"');
            if (string.IsNullOrEmpty(code))
            {
                result = "{\"code\":\"Unknown\",\"result\":\"102\",\"desc\":\"param值中的参数无效\"}";
                Logger.GetLogger(this.GetType()).Info("APP接口被调用，解析JSON数据获取code值为空.错误原因" + result + "\r\n", null);
                return result;
            }
            switch (code)
            {
                case "Login":
                    result=LoginInterface(obj);
                    break;
                case "PersonalInfo":
                    result = PersonalInfo(obj);
                    break;
                case "AddressBook":
                    result = AddressBook(obj);
                    break;
                case "MyOrder":
                    result = MyOrder(obj);
                    break;
                case "OrderDetail":
                    result = OrderDetail(obj);
                    break;
                case "HistoryOrder":
                    result = HistoryOrder(obj);
                    break;
                case "SearchOrder":
                    result = SearchOrder(obj);
                    break;
                default:
                    result = "{\"code\":\"Unknown\",\"result\":\"102\",\"desc\":\"param值中的参数无效\"}";
                    Logger.GetLogger(this.GetType()).Info("APP接口被调用，解析JSON数据获取code值为："+code+" 没有对应的接口，错误原因" + result + "\r\n", null);
                    return result;

            }
            return result;
            
        }
        else
        {
            result = "{\"code\":\"Unknown\",\"result\":\"102\",\"desc\":\"param值中的参数无效\"}";
            Logger.GetLogger(this.GetType()).Info("APP接口被调用，错误原因为" + result + "\r\n", null);
            return result;
        }
    }
    /// <summary>
    /// 登录接口
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string LoginInterface(JObject obj)
    {
        string userName = string.Empty;
        string password = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();        
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"Login\",\"result\":\"0\",\"leader\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP Login接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        try
        {
            password = obj["password"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"Login\",\"result\":\"0\",\"leader\":\"0\",\"desc\":\"解析JSON数据获取password值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP Login接口被调用，解析JSON数据获取password值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        password = password.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"Login\",\"result\":\"0\",\"leader\":\"0\",\"desc\":\"解析JSON数据获取的userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP Login接口被调用，解析JSON数据获取的userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"Login\",\"result\":\"0\",\"leader\":\"0\",\"desc\":\"用户" + userName + "不存在！\"}";
            Logger.GetLogger(this.GetType()).Info("APP Login接口被调用，用户名" + userName + "不存在\r\n", null);
            return result;
        }
        if (!user.Enable)
        {
            result = "{\"code\":\"Login\",\"result\":\"0\",\"leader\":\"0\",\"desc\":\"用户" + userName + "已被禁用\"}";
            Logger.GetLogger(this.GetType()).Info("APP Login接口被调用，用户名" + userName + "已被禁用\r\n", null);
            return result;
        }
        if (user.PassWord == password.ToLower())
        {
            if (checkLeader(user))
            {
                result = "{\"code\":\"Login\",\"result\":\"1\",\"leader\":\"1\",\"desc\":\"用户" + userName + "登录成功\"}";
                Logger.GetLogger(this.GetType()).Info("APP Login接口被调用，用户名" + userName + "登录成功\r\n", null);
            }
            else {
                result = "{\"code\":\"Login\",\"result\":\"1\",\"leader\":\"0\",\"desc\":\"用户" + userName + "登录成功\"}";
                Logger.GetLogger(this.GetType()).Info("APP Login接口被调用，用户名" + userName + "登录成功\r\n", null);
            }
            return result;
        }
        else {
            result = "{\"code\":\"Login\",\"result\":\"0\",\"leader\":\"0\",\"desc\":\"密码错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP Login接口被调用，密码错误\r\n", null);
            return result;
        }
    }

    /// <summary>
    /// 查看个人信息接口
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string PersonalInfo(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"PersonalInfo\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP PersonalInfo接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"PersonalInfo\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP PersonalInfo接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"PersonalInfo\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP PersonalInfo接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }
        WorkGroupInfo workGroup = WorkGroupBLL.Get(user.WorkGroupID);
        KeyValueDictionary paramDic = new KeyValueDictionary();
        paramDic.Add("code", "PersonalInfo");
        paramDic.Add("employeeId",user.ID);
        paramDic.Add("headUrl", "");
        paramDic.Add("trueName", user.Name);
        paramDic.Add("company", "");
        paramDic.Add("group", workGroup == null ? "" : workGroup.Name);
        paramDic.Add("phoneNo", user.Tel);
        paramDic.Add("desc", "");
        result = WebUtil.BuildQueryJson(paramDic);
        if (result.EndsWith(","))
        {
            result = result.Remove(result.Length - 1);
        }
        result = "{" +result+ "}";
        return result;
    
    }

    /// <summary>
    /// 查看通讯录，查看本组其他人的信息
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string AddressBook(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"AddressBook\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP AddressBook接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"AddressBook\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP AddressBook接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"AddressBook\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP AddressBook接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }
        WorkGroupInfo workGroup = WorkGroupBLL.Get(user.WorkGroupID);
        List<UserInfo> userList = UserBLL.GetList(workGroup.ID, "%现场工程师%");
        if (userList != null)
        {
            foreach (UserInfo item in userList)
            {
                if (item.Code == userName)
                {
                    userList.Remove(item); break;
                }
            }
        }
        if (userList.Count > 0)
        {
            string coUsers = string.Empty;
            for (int i = 0; i < userList.Count; i++)
            {
                if (i+1== userList.Count)
                {
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("userName", userList[i].Name);
                    paramDic.Add("employeeId", userList[i].ID);
                    paramDic.Add("headUrl", "");
                    paramDic.Add("trueName", userList[i].Name);
                    paramDic.Add("phoneNo", userList[i].Tel);
                    paramDic.Add("state", "");
                    coUsers = WebUtil.BuildQueryJson(paramDic);
                    if (coUsers.EndsWith(","))
                    {
                        coUsers = coUsers.Remove(coUsers.Length - 1);
                    }
                    coUsers = "{" + coUsers + "}";
                    result += coUsers;
                }
                else {
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("userName", userList[i].Name);
                    paramDic.Add("employeeId", userList[i].ID);
                    paramDic.Add("headUrl", "");
                    paramDic.Add("trueName", userList[i].Name);
                    paramDic.Add("phoneNo", userList[i].Tel);
                    paramDic.Add("state", "");
                    coUsers = WebUtil.BuildQueryJson(paramDic);
                    if (coUsers.EndsWith(","))
                    {
                        coUsers = coUsers.Remove(coUsers.Length - 1);
                    }
                    coUsers = "{" + coUsers + "},";
                    result += coUsers;
                }         
            }
            result = "{\"code\":\"AddressBook\",\"list\":[" + result + "],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP AddressBook接口被调用，用户：" + userName + "通讯录信息为：" + result + "\r\n", null);
        }
        else {
            result = "{\"code\":\"AddressBook\",\"list\":[],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP AddressBook接口被调用，用户：" + userName + "通讯录为空\r\n", null);
        }
        return result;
    }

    /// <summary>
    /// 我的工单接口
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string MyOrder(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"MyOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"MyOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"MyOrder\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }
        //WorkGroupInfo workGroup = WorkGroupBLL.Get(user.WorkGroupID);
        List<CallInfo> callList = CallBLL.GetMyCallsForOnsiteEngineer((int)SysEnum.CallStateMain.处理中, user.ID, 0);

        if (callList.Count > 0)
        {
            string callInfo = string.Empty;
            for (int i = 0; i < callList.Count; i++)
            {
                if (i + 1 == callList.Count)
                {
                    string stateDetail = string.Empty;
                    if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应)
                    {
                        //待上门
                        stateDetail = "0";
                    }
                    else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
                    {
                        //处理中
                        stateDetail = "1";
                    }
                    else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.上门支持)
                    {
                        //已离场
                        stateDetail = "2";
                    }
                    else {
                        stateDetail = "";
                    }
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("orderId", callList[i].No);
                    paramDic.Add("gps", "");
                    StoreInfo store = StoresBLL.Get(callList[i].StoreID);
                    paramDic.Add("address",store==null?"":store.Address);
                    paramDic.Add("customerName", callList[i].CustomerName);
                    paramDic.Add("customerLogo", "");
                    paramDic.Add("orderDescribe", callList[i].Details);

                    paramDic.Add("state", stateDetail);
                    paramDic.Add("desc", callList[i].Details);
                    callInfo = WebUtil.BuildQueryJson(paramDic);
                    if (callInfo.EndsWith(","))
                    {
                        callInfo = callInfo.Remove(callInfo.Length - 1);
                    }
                    callInfo = "{" + callInfo + "}";
                    result += callInfo;
                }
                else
                {
                    string stateDetail = string.Empty;
                    if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应)
                    {
                        //待上门
                        stateDetail = "0";
                    }
                    else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
                    {
                        //处理中
                        stateDetail = "1";
                    }
                    else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.上门支持)
                    {
                        //已离场
                        stateDetail = "2";
                    }
                    else
                    {
                        stateDetail = "";
                    }
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("orderId", callList[i].No);
                    paramDic.Add("gps", "");
                    StoreInfo store = StoresBLL.Get(callList[i].StoreID);
                    paramDic.Add("address", store == null ? "" : store.Address);
                    paramDic.Add("customerName", callList[i].CustomerName);
                    paramDic.Add("customerLogo", "");
                    paramDic.Add("orderDescribe", callList[i].Details);
                    paramDic.Add("state", stateDetail);
                    paramDic.Add("desc", callList[i].Details);
                    callInfo = WebUtil.BuildQueryJson(paramDic);
                    if (callInfo.EndsWith(","))
                    {
                        callInfo = callInfo.Remove(callInfo.Length - 1);
                    }
                    callInfo = "{" + callInfo + "},";
                    result += callInfo;
                }
            }
            result = "{\"code\":\"MyOrder\",\"list\":[" + result + "],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "我的工单为：" + result + "\r\n", null);
        }
        else
        {
            result = "{\"code\":\"MyOrder\",\"list\":[],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "我的工单为空\r\n", null);
        }
        return result;
    }


    /// <summary>
    /// 历史工单
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string HistoryOrder(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        string page = string.Empty;
        int pageIndex = 1;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"HistoryOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP HistoryOrder接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"HistoryOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP HistoryOrder接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"HistoryOrder\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP HistoryOrder接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }
        try
        {
            page = obj["page"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"HistoryOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP HistoryOrder接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        page = page.Trim('"');
        if (!string.IsNullOrEmpty(page))
        {
            pageIndex = Convert.ToInt32(page);
        }

        //WorkGroupInfo workGroup = WorkGroupBLL.Get(user.WorkGroupID);
        List<CallInfo> callList = CallBLL.GetHistoryCallsForOnsiteEngineer(pageIndex, 10, user.ID, 0);
        if (callList!=null&&callList.Count > 0)
        {
            string callInfo = string.Empty;
            for (int i = 0; i < callList.Count; i++)
            {
                if (i + 1 == callList.Count)
                {
                   
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("orderId", callList[i].No);
                    paramDic.Add("gps", "");
                    StoreInfo store = StoresBLL.Get(callList[i].StoreID);
                    paramDic.Add("address", store == null ? "" : store.Address);
                    paramDic.Add("customerName", callList[i].CustomerName);
                    paramDic.Add("customerLogo", "");
                    paramDic.Add("orderDescribe", callList[i].Details);
                    paramDic.Add("desc", callList[i].Details);
                    callInfo = WebUtil.BuildQueryJson(paramDic);
                    if (callInfo.EndsWith(","))
                    {
                        callInfo = callInfo.Remove(callInfo.Length - 1);
                    }
                    callInfo = "{" + callInfo + "}";
                    result += callInfo;
                }
                else
                {
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("orderId", callList[i].No);
                    paramDic.Add("gps", "");
                    StoreInfo store = StoresBLL.Get(callList[i].StoreID);
                    paramDic.Add("address", store == null ? "" : store.Address);
                    paramDic.Add("customerName", callList[i].CustomerName);
                    paramDic.Add("customerLogo", "");
                    paramDic.Add("orderDescribe", callList[i].Details);
                    paramDic.Add("desc", callList[i].Details);
                    callInfo = WebUtil.BuildQueryJson(paramDic);
                    if (callInfo.EndsWith(","))
                    {
                        callInfo = callInfo.Remove(callInfo.Length - 1);
                    }
                    callInfo = "{" + callInfo + "},";
                    result += callInfo;
                }
            }
            result = "{\"code\":\"HistoryOrder\",\"list\":[" + result + "],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP HistoryOrder接口被调用，用户：" + userName + "历史工单为：" + result + "\r\n", null);
        }
        else
        {
            result = "{\"code\":\"HistoryOrder\",\"list\":[],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP HistoryOrder接口被调用，用户：" + userName + "历史工单为空\r\n", null);
        }
        return result;
    }
    /// <summary>
    /// 订单详情接口
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string OrderDetail(JObject obj)
    {
        string callNo = string.Empty;
        string result = string.Empty;
        string stateDetail = string.Empty;
        string accessTime = string.Empty;
        string singInTime = string.Empty;
        string picUrlList =string.Empty;
        try
        {
            callNo = obj["orderId"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"OrderDetail\",\"result\":\"0\",\"desc\":\"解析JSON数据获取orderId值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP OrderDetail接口被调用，解析JSON数据获取orderId值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        callNo = callNo.Trim('"');
        if (string.IsNullOrEmpty(callNo))
        {
            result = "{\"code\":\"OrderDetail\",\"result\":\"0\",\"desc\":\"解析JSON数据获取orderId值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP OrderDetail接口被调用，解析JSON数据获取orderId值为空\r\n", null);
            return result;
        }
        CallInfo callInfo = CallBLL.Get(callNo);
        if (callInfo == null)
        {
            result = "{\"code\":\"OrderDetail\",\"result\":\"0\",\"desc\":\"单号：" + callNo + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP OrderDetail接口被调用，单号" + callNo + "不存在\r\n", null);
            return result;
        }

        
        KeyValueDictionary paramDic = new KeyValueDictionary();
        paramDic.Add("code", "OrderDetail");
        paramDic.Add("orderId", callNo);
        paramDic.Add("gps", "");
        StoreInfo store = StoresBLL.Get(callInfo.StoreID);
        paramDic.Add("address", store == null ? "" : store.Address);
        paramDic.Add("customerName", callInfo.CustomerName);
        paramDic.Add("customerLogo", "");
        paramDic.Add("orderDescribe", callInfo.Details);
        CallStepInfo callstep = CallStepBLL.GetLast(callInfo.ID, SysEnum.StepType.上门安排);
        if (callstep!=null)
        {
            accessTime = callstep.AddDate.ToString("yyyy-MM-dd HH:mm");        }
        else {
            accessTime = "";
        }
        paramDic.Add("acceptTime", accessTime);
        callstep = CallStepBLL.GetLast(callInfo.ID, SysEnum.StepType.到达门店处理);
        if (callstep!=null)
        {
            singInTime = callstep.AddDate.ToString("yyyy-MM-dd HH:mm");
        }
        else
        {
            singInTime = "";
        }
        paramDic.Add("singInTime", singInTime);

        List<CommentInfo> commentInfoList = CommentBLL.GetList(" f_CallID = " + callInfo.ID+ " order by f_AddDate desc ");

        paramDic.Add("technologyScore", commentInfoList.Count>0?commentInfoList[0].Score2.ToString():"");
        paramDic.Add("attitudeScore", commentInfoList.Count > 0 ? commentInfoList[0].Score3.ToString() : "");

        KeyValueDictionary paramUrlDic = new KeyValueDictionary();
        List<AttachmentInfo> urlList = AttachmentBLL.GetList(callInfo.ID, AttachmentInfo.EUserFor.Call);
        if (urlList.Count > 0)
        {
            string url = string.Empty;
            for (int i = 0; i < urlList.Count; i++)
            {
                if (i + 1 == urlList.Count)
                {
                    paramUrlDic.Add("url", urlList[i].FilePath);
                    url =WebUtil.BuildQueryJson(paramDic);
                    if (url.EndsWith(","))
                    {
                        url = url.Remove(url.Length - 1);
                    }
                    url = "{" + url + "}";
                    
                }
                else
                {

                    paramUrlDic.Add("url", urlList[i].FilePath);
                    url = WebUtil.BuildQueryJson(paramDic);
                    if (url.EndsWith(","))
                    {
                        url = url.Remove(url.Length - 1);
                    }
                    url = "{" + url + "},";
                }
            }
            picUrlList = "[" + url + "]";
        }
        else
        {
            picUrlList = "[]";
        }

        if (callInfo.StateMain == 2 && (callInfo.StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callInfo.StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应))
        {
            //待上门
            stateDetail = "0";
        }
        else if (callInfo.StateMain == 2 && callInfo.StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
        {
            //处理中
            stateDetail = "1";
        }
        else if (callInfo.StateMain == 2 && callInfo.StateDetail == (int)SysEnum.CallStateDetails.上门支持)
        {
            //已离场
            stateDetail = "2";
        }
        else if (callInfo.StateMain == 3)
        {

            //已完成
            stateDetail = "3";
        }
        else
        {
            stateDetail = "";
        }

        result = "{" + WebUtil.BuildQueryJson(paramDic) + "\"picList\":" + picUrlList + ",\"state\":\"" + stateDetail + "\",\"desc\":\"\"}";
        return result;       
    }

    /// <summary>
    /// 工单查询
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string SearchOrder(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        string state = string.Empty;
        int stateMain = 0;
        int stateDetail = 0;
        string getWorkgroup = string.Empty;
        string customName = string.Empty;
        string brandName = string.Empty;
        int userID = 0;
        int workgroupID = 0;

        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }

        try
        {
            state = obj["state"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取state值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取state值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        state = state.Trim('"');
        if (state == "0")
        {
            //因为待分配分两种，SysEnum.CallStateDetails.等待工程师上门和SysEnum.CallStateDetails.等待第三方响应
            stateMain = 2;
            stateDetail=100;
    
        }
        else if (state == "1")
        {
            stateMain = 2;
            stateDetail = (int)SysEnum.CallStateDetails.到达门店处理;
        }
        else if (state == "2")
        {
            stateMain = 2;
            stateDetail = (int)SysEnum.CallStateDetails.上门支持;
        }
        else if (state == "3")
        {
            stateMain = 3;
            
        }
        else {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取state值为" + state + "系统中不存在该状态\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取state值为" + state + "系统中不存在该状态，错误原因：" + result  + "\r\n", null);
            return result;
        }

        try
        {
            customName = obj["companyName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取companyName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取companyName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        customName = customName.Trim('"');
        customName = customName.Trim();
       
        
        try
        {
            brandName = obj["productName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取brandName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取brandName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        brandName = brandName.Trim('"');
        brandName = brandName.Trim();

        try
        {
            brandName = obj["productName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取brandName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取brandName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        brandName = brandName.Trim('"');
        brandName = brandName.Trim();

        try
        {
            getWorkgroup = obj["all"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取all值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取all值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        getWorkgroup = getWorkgroup.Trim('"');
        getWorkgroup = getWorkgroup.Trim();
        if (string.IsNullOrEmpty(getWorkgroup))
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取getWorkgroup值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取getWorkgroup值为空\r\n", null);
            return result;
        }
        if (getWorkgroup == "0")
        {
            userID = user.ID;
        }
        else {

            workgroupID = user.WorkGroupID;
        }


        List<CallInfo> callList = CallBLL.GetMyCallsForOnsiteEngineer(stateMain, stateDetail, userID, workgroupID, customName, brandName);

        if (callList.Count > 0)
        {
            string callInfo = string.Empty;
            for (int i = 0; i < callList.Count; i++)
            {
                if (i + 1 == callList.Count)
                {
                    //string callStateDetail = string.Empty;
                    //if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应)
                    //{
                    //    //待上门
                    //    callStateDetail = "0";
                    //}
                    //else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
                    //{
                    //    //处理中
                    //    callStateDetail = "1";
                    //}
                    //else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.上门支持)
                    //{
                    //    //已离场
                    //    callStateDetail = "2";
                    //}
                    //else
                    //{
                    //    callStateDetail = "";
                    //}
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("orderId", callList[i].No);
                    paramDic.Add("gps", "");
                    StoreInfo store = StoresBLL.Get(callList[i].StoreID);
                    paramDic.Add("address", store == null ? "" : store.Address);
                    paramDic.Add("customerName", callList[i].CustomerName);
                    paramDic.Add("customerLogo", "");
                    paramDic.Add("orderDescribe", callList[i].Details);

                    //paramDic.Add("state", callStateDetail);
                    paramDic.Add("desc", callList[i].Details);
                    callInfo = WebUtil.BuildQueryJson(paramDic);
                    if (callInfo.EndsWith(","))
                    {
                        callInfo = callInfo.Remove(callInfo.Length - 1);
                    }
                    callInfo = "{" + callInfo + "}";
                    result += callInfo;
                }
                else
                {
                    //string callStateDetail = string.Empty;
                    //if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应)
                    //{
                    //    //待上门
                    //    callStateDetail = "0";
                    //}
                    //else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
                    //{
                    //    //处理中
                    //    callStateDetail = "1";
                    //}
                    //else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.上门支持)
                    //{
                    //    //已离场
                    //    callStateDetail = "2";
                    //}
                    //else
                    //{
                    //    callStateDetail = "";
                    //}
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("orderId", callList[i].No);
                    paramDic.Add("gps", "");
                    StoreInfo store = StoresBLL.Get(callList[i].StoreID);
                    paramDic.Add("address", store == null ? "" : store.Address);
                    paramDic.Add("customerName", callList[i].CustomerName);
                    paramDic.Add("customerLogo", "");
                    paramDic.Add("orderDescribe", callList[i].Details);
                    //paramDic.Add("state", callStateDetail);
                    paramDic.Add("desc", callList[i].Details);
                    callInfo = WebUtil.BuildQueryJson(paramDic);
                    if (callInfo.EndsWith(","))
                    {
                        callInfo = callInfo.Remove(callInfo.Length - 1);
                    }
                    callInfo = "{" + callInfo + "},";
                    result += callInfo;
                }
            }
            result = "{\"code\":\"SearchOrder\",\"list\":[" + result + "],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，用户：" + userName + "工单查询列表为：" + result + "\r\n", null);
        }
        else
        {
            result = "{\"code\":\"SearchOrder\",\"list\":[],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，用户：" + userName + "工单查询列表为空\r\n", null);
        }
        return result;
    }

    /// <summary>
    /// 判断用户是不是组长，现场工程师判断方法为判断是否有更换现场工程师的权限
    /// </summary>
    /// <param name="userInfo"></param>
    /// <returns></returns>
    protected bool checkLeader(UserInfo userInfo)
    {
        string Comma = ",";
        int powerID = (int)PowerInfo.P1_Call.更换上门人;
        GroupInfo Ginfo = GroupBLL.Get(userInfo.PowerGroupID);
        string Powers = Comma + Ginfo.PowerList.Trim(',') + Comma;
        bool b = Powers.Contains(Comma + powerID + Comma);
        return b;
    }
}
