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
using System.Data;

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
                //case "MyOrder":
                //    result = MyOrder(obj);
                //    break;
                case "MyOrder":
                    result = SPMyOrder(obj);
                    break;
                case "OrderDetail":
                    result = OrderDetail(obj);
                    break;
                case "HistoryOrder":
                    result = HistoryOrder(obj);
                    break;
                //case "SearchOrder":
                //    result = SearchOrder(obj);
                //    break;
                case "SearchOrder":
                    result = SPSearchOrder(obj);
                    break;
                case "AssignOrder":
                    result = AssignOrder(obj);
                    break;
                case "WorkRecord":
                    result = WorkRecord(obj);
                    break;
                case "UploadImageUrl":
                    result = UploadImageUrl(obj);
                    break;
                case "CompanyList":
                    result = CompanyList(obj);
                    break;
                case "ProductList":
                    result = ProductList(obj);
                    break;
                case "SignIn":
                    //result = SignIn(obj);
                    result = SPSignIn(obj);
                    break;
                case "SignRecord":
                    result = SignRecord(obj);
                    break;
                case "Knowledge":
                    result = Knowledge(obj);
                    break;
                case "TodayClock":
                    result = TodayClock(obj);
                    break;
                case "ChangePWD":
                    result=ChangePWD(obj);
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
        //if (userList != null)
        //{
        //    foreach (UserInfo item in userList)
        //    {
        //        if (item.Code == userName)
        //        {
        //            userList.Remove(item); break;
        //        }
        //    }
        //}
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
    #region 我的工单旧代码
    /// <summary>
    /// 我的工单接口
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    //protected string MyOrder(JObject obj)
    //{
    //    string userName = string.Empty;
    //    string result = string.Empty;
    //    try
    //    {
    //        userName = obj["userName"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"MyOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    userName = userName.Trim('"');
    //    if (string.IsNullOrEmpty(userName))
    //    {
    //        result = "{\"code\":\"MyOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，解析JSON数据获取userName值为空\r\n", null);
    //        return result;
    //    }
    //    UserInfo user = UserBLL.Get(userName);
    //    if (user == null)
    //    {
    //        result = "{\"code\":\"MyOrder\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "不存在\r\n", null);
    //        return result;
    //    }
    //    //WorkGroupInfo workGroup = WorkGroupBLL.Get(user.WorkGroupID);
    //    List<CallInfo> callList = CallBLL.GetMyCallsForOnsiteEngineer((int)SysEnum.CallStateMain.处理中, user.ID, 0);

    //    if (callList.Count > 0)
    //    {
    //        string callInfo = string.Empty;
    //        for (int i = 0; i < callList.Count; i++)
    //        {
    //            if (i + 1 == callList.Count)
    //            {
    //                string stateDetail = string.Empty;
    //                if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应)
    //                {
    //                    //待上门
    //                    stateDetail = "0";
    //                }
    //                else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
    //                {
    //                    //处理中
    //                    stateDetail = "1";
    //                }
    //                else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.上门支持)
    //                {
    //                    //已离场
    //                    stateDetail = "2";
    //                }
    //                else {
    //                    stateDetail = "";
    //                }
    //                KeyValueDictionary paramDic = new KeyValueDictionary();
    //                paramDic.Add("orderId", callList[i].No);
    //                paramDic.Add("gps", "");
    //                StoreInfo store = StoresBLL.Get(callList[i].StoreID);
    //                paramDic.Add("address",store==null?"":store.Address);
    //                paramDic.Add("customerName", callList[i].CustomerName);
    //                paramDic.Add("customerLogo", "");
    //                paramDic.Add("orderDescribe", callList[i].Details);

    //                paramDic.Add("state", stateDetail);
    //                paramDic.Add("desc", callList[i].Details);
    //                callInfo = WebUtil.BuildQueryJson(paramDic);
    //                if (callInfo.EndsWith(","))
    //                {
    //                    callInfo = callInfo.Remove(callInfo.Length - 1);
    //                }
    //                callInfo = "{" + callInfo + "}";
    //                result += callInfo;
    //            }
    //            else
    //            {
    //                string stateDetail = string.Empty;
    //                if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应)
    //                {
    //                    //待上门
    //                    stateDetail = "0";
    //                }
    //                else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
    //                {
    //                    //处理中
    //                    stateDetail = "1";
    //                }
    //                else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.上门支持)
    //                {
    //                    //已离场
    //                    stateDetail = "2";
    //                }
    //                else
    //                {
    //                    stateDetail = "";
    //                }
    //                KeyValueDictionary paramDic = new KeyValueDictionary();
    //                paramDic.Add("orderId", callList[i].No);
    //                paramDic.Add("gps", "");
    //                StoreInfo store = StoresBLL.Get(callList[i].StoreID);
    //                paramDic.Add("address", store == null ? "" : store.Address);
    //                paramDic.Add("customerName", callList[i].CustomerName);
    //                paramDic.Add("customerLogo", "");
    //                paramDic.Add("orderDescribe", callList[i].Details);
    //                paramDic.Add("state", stateDetail);
    //                paramDic.Add("desc", callList[i].Details);
    //                callInfo = WebUtil.BuildQueryJson(paramDic);
    //                if (callInfo.EndsWith(","))
    //                {
    //                    callInfo = callInfo.Remove(callInfo.Length - 1);
    //                }
    //                callInfo = "{" + callInfo + "},";
    //                result += callInfo;
    //            }
    //        }
    //        result = "{\"code\":\"MyOrder\",\"list\":[" + result + "],\"desc\":\"\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "我的工单为：" + result + "\r\n", null);
    //    }
    //    else
    //    {
    //        result = "{\"code\":\"MyOrder\",\"list\":[],\"desc\":\"\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "我的工单为空\r\n", null);
    //    }
    //    return result;
    //}
    #endregion

    /// <summary>
    /// 我的工单接口,使用SP完成
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string SPMyOrder(JObject obj)
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
        
        DataTable dt = new DataTable();
        string spReturnValue = string.Empty;
        dt = CallBLL.GetMyCallsForOnsiteEngineerBySp(userName, out spReturnValue);

        if (spReturnValue == "not binding")
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }
        if (dt.Rows.Count > 0)
        {
            string callInfo = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                
                string stateDetail = string.Empty;
                if (dr["f_StateDetail"].ToString() == ((int)SysEnum.CallStateDetails.等待工程师上门).ToString() || dr["f_StateDetail"].ToString() == ((int)SysEnum.CallStateDetails.等待第三方响应).ToString())
                {
                    //待上门
                    stateDetail = "0";
                }
                else if (dr["f_StateDetail"].ToString() == ((int)SysEnum.CallStateDetails.到达门店处理).ToString())
                {
                    //处理中
                    stateDetail = "1";
                }
                else if (dr["f_StateDetail"].ToString() == ((int)SysEnum.CallStateDetails.上门支持).ToString())
                {
                    //已离场
                    stateDetail = "2";
                }
                else
                {
                    stateDetail = "2";
                }
                KeyValueDictionary paramDic = new KeyValueDictionary();
                paramDic.Add("orderId", dr["f_No"].ToString());
                paramDic.Add("gps", dr["f_GPS"].ToString());
                paramDic.Add("address", DeleteSpecialChar(dr["f_Address"].ToString()));
                paramDic.Add("storeName", DeleteSpecialChar(dr["f_Name"].ToString()));
                paramDic.Add("majorUserName", dr["f_MajorUserName"].ToString());
                paramDic.Add("customerName", dr["f_CustomerName"].ToString());
                paramDic.Add("productName", dr["f_BrandName"].ToString());
                paramDic.Add("customerLogo", "");
                string callDetails = dr["f_Details"].ToString();
                callDetails = DeleteSpecialChar(callDetails);
                paramDic.Add("orderDescribe", callDetails);
                paramDic.Add("state", stateDetail);
                //到期时间
                string slaDateEndStr =string.Empty;
                if (dr["f_SLADateEnd"] != null && !string.IsNullOrEmpty(dr["f_SLADateEnd"].ToString()))
                {
                    DateTime slaDateEnd = Convert.ToDateTime(dr["f_SLADateEnd"].ToString());
                    if (slaDateEnd.Year != 1970)
                    {
                        slaDateEndStr = slaDateEnd.ToString("yyyy-MM-dd HH:mm");
                    }
                }
                paramDic.Add("SLA", slaDateEndStr);
                paramDic.Add("desc", callDetails);


                callInfo = WebUtil.BuildQueryJson(paramDic);
                if (callInfo.EndsWith(","))
                {
                    callInfo = callInfo.Remove(callInfo.Length - 1);
                }
                callInfo = "{" + callInfo + "},";
                result += callInfo;
            }
            if (result.EndsWith(","))
            {
                result = result.Remove(result.Length - 1);
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
            try 
            {
                pageIndex = Convert.ToInt32(page);
            }
            catch(Exception ex)
            {

                result = "{\"code\":\"HistoryOrder\",\"result\":\"0\",\"desc\":\"参数page的值必须为整数\"}";
                Logger.GetLogger(this.GetType()).Info("APP HistoryOrder接口被调用，参数page的值必须为整数，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
                return result;
            }
            
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
                    StoreInfo store = StoresBLL.Get(callList[i].StoreID);
                    paramDic.Add("gps", store == null ? "" : store.GPS);
                    paramDic.Add("address", store == null ? "" : DeleteSpecialChar(store.Address));
                    paramDic.Add("storeName",store == null ? "" : DeleteSpecialChar(store.Name));
                    paramDic.Add("majorUserName", userName);
                    paramDic.Add("customerName", callList[i].CustomerName);
                    paramDic.Add("productName", callList[i].BrandName);
                    paramDic.Add("customerLogo", "");
                    string callDetails = callList[i].Details;
                    callDetails = DeleteSpecialChar(callDetails);
                    paramDic.Add("orderDescribe", callDetails);
                    string stateMain = string.Empty;
                    if (callList[i].StateMain == 3)
                    {
                        stateMain = "0";
                    }
                    if (callList[i].StateMain == 4)
                    {
                        stateMain = "1";
                    }
                    paramDic.Add("state", stateMain);
                    //到期时间
                    string slaDateEndStr = string.Empty;
                    if (callList[i].SLADateEnd != null)
                    {
                        DateTime slaDateEnd = Convert.ToDateTime(callList[i].SLADateEnd);
                        if (slaDateEnd.Year != 1970)
                        {
                            slaDateEndStr = slaDateEnd.ToString("yyyy-MM-dd HH:mm");
                        }
                    }
                    paramDic.Add("SLA", slaDateEndStr);
                    paramDic.Add("desc", callDetails);
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
                    StoreInfo store = StoresBLL.Get(callList[i].StoreID);
                    paramDic.Add("gps", store == null ? "" : store.GPS);
                    paramDic.Add("address", store == null ? "" : DeleteSpecialChar(store.Address));
                    paramDic.Add("storeName", store == null ? "" : DeleteSpecialChar(store.Name));
                    paramDic.Add("majorUserName", userName);
                    paramDic.Add("customerName", callList[i].CustomerName);
                    paramDic.Add("productName", callList[i].BrandName);
                    paramDic.Add("customerLogo", "");
                    string callDetails = callList[i].Details;
                    callDetails = DeleteSpecialChar(callDetails);
                    paramDic.Add("orderDescribe", callDetails);
                    string stateMain = string.Empty;
                    if (callList[i].StateMain == 3)
                    {
                        stateMain = "0";
                    }
                    if (callList[i].StateMain == 4)
                    {
                        stateMain = "1";
                    }
                    paramDic.Add("state", stateMain);
                    //到期时间
                    string slaDateEndStr = string.Empty;
                    if (callList[i].SLADateEnd != null)
                    {
                        DateTime slaDateEnd = Convert.ToDateTime(callList[i].SLADateEnd);
                        if (slaDateEnd.Year != 1970)
                        {
                            slaDateEndStr = slaDateEnd.ToString("yyyy-MM-dd HH:mm");
                        }
                    }
                    paramDic.Add("SLA", slaDateEndStr);
                    paramDic.Add("desc", callDetails);
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
        string signOutTime = string.Empty;
        string picUrlList =string.Empty;
        string majorUserName = string.Empty;
        //工程师评价的说明desc
        string recordDesc = string.Empty;
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
        
        StoreInfo store = StoresBLL.Get(callInfo.StoreID);
        paramDic.Add("gps", store == null ? "" : store.GPS);
        paramDic.Add("address", store == null ? "" :store.CityName.Trim()+ DeleteSpecialChar(store.Address));
        paramDic.Add("storeName", store == null ? "" : DeleteSpecialChar(store.Name));
        paramDic.Add("customerName", callInfo.CustomerName);
        paramDic.Add("productName", callInfo.BrandName);
        paramDic.Add("customerLogo", "");

        //到期时间
        string slaDateEndStr = string.Empty;
        if (callInfo.SLADateEnd != null)
        {
            DateTime slaDateEnd = Convert.ToDateTime(callInfo.SLADateEnd);
            if (slaDateEnd.Year != 1970)
            {
                slaDateEndStr = slaDateEnd.ToString("yyyy-MM-dd HH:mm");
            }
        }
        paramDic.Add("SLA", slaDateEndStr);
        string callDetails = DeleteSpecialChar(callInfo.Details);
        paramDic.Add("orderDescribe", callDetails);
        CallStepInfo callstep = CallStepBLL.GetLast(callInfo.ID, SysEnum.StepType.上门安排);
        if (callstep!=null)
        {
            accessTime = callstep.AddDate.ToString("yyyy-MM-dd HH:mm");        }
        else {
            accessTime = "";
        }
        if (callstep!=null)
        {
            majorUserName = callstep.MajorUserName;
        }
        else {
            majorUserName = "";
        }
        paramDic.Add("acceptTime", accessTime);
        paramDic.Add("majorUserName", majorUserName);
        callstep = CallStepBLL.GetLast(callInfo.ID, SysEnum.StepType.上门准备);
        paramDic.Add("maintaimUserName", callInfo.MaintaimUserName);
        if (callstep == null)
        {
            paramDic.Add("disposeSuggest", "");
        }
        else {
            paramDic.Add("disposeSuggest", DeleteSpecialChar(callstep.Details));
        }
        callstep = CallStepBLL.GetLast(callInfo.ID, SysEnum.StepType.到达门店处理);
        List<CommentInfo> commentInfoList = new List<CommentInfo>();
        if (callstep!=null)
        {
            singInTime = callstep.AddDate.ToString("yyyy-MM-dd HH:mm");
            commentInfoList = CommentBLL.GetList(" f_CallID = " + callInfo.ID + " AND f_CallStepID=" + callstep.ID + " AND f_IsDropInUserDoIt=1 AND f_ByMachine='APP' order by f_AddDate desc ");
        }
        else
        {
            singInTime = "";
        }
        paramDic.Add("signInTime", singInTime);


        callstep = CallStepBLL.GetLast(callInfo.ID, SysEnum.StepType.上门详细);
        
        if (callstep != null)
        {
            signOutTime = callstep.AddDate.ToString("yyyy-MM-dd HH:mm");
            
        }
        else
        {
            signOutTime = "";
        }
        paramDic.Add("signOutTime", signOutTime);
        paramDic.Add("technologyScore", commentInfoList.Count>0?commentInfoList[0].Score2.ToString():"");
        paramDic.Add("attitudeScore", commentInfoList.Count > 0 ? commentInfoList[0].Score3.ToString() : "");
        if (commentInfoList.Count > 0)
        {
            recordDesc = commentInfoList[0].Details;
        }
        
        //List<AttachmentInfo> urlList = AttachmentBLL.GetList(callInfo.ID, AttachmentInfo.EUserFor.Call);
        List<AttachmentInfo> urlList = AttachmentBLL.GetList(" f_CallID=" + callInfo.ID + " AND f_Title='APP上传的图片列表'");
        if (urlList.Count > 0)
        {
            string url = string.Empty;
            for (int i = 0; i < urlList.Count; i++)
            {
                KeyValueDictionary paramUrlDic = new KeyValueDictionary();
                if (i + 1 == urlList.Count)
                {
                    paramUrlDic.Add("url", urlList[i].FilePath);
                    url = WebUtil.BuildQueryJson(paramUrlDic);
                    if (url.EndsWith(","))
                    {
                        url = url.Remove(url.Length - 1);
                    }
                    picUrlList = picUrlList+"{" + url + "}";
                    
                }
                else
                {

                    paramUrlDic.Add("url", urlList[i].FilePath);
                    url = WebUtil.BuildQueryJson(paramUrlDic);
                    if (url.EndsWith(","))
                    {
                        url = url.Remove(url.Length - 1);
                    }
                    picUrlList = picUrlList + "{" + url + "},";
                }
            }
            picUrlList = "[" + picUrlList + "]";
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
        else if (callInfo.StateMain == 4)
        {

            //已关闭
            stateDetail = "4";
        }
        else
        {
            stateDetail = "2";
        }

        result = "{" + WebUtil.BuildQueryJson(paramDic) + "\"picList\":" + picUrlList + ",\"state\":\"" + stateDetail + "\",\"desc\":\"" + recordDesc + "\"}";
        return result;
    }
    #region 工查询旧代码
    /// <summary>
    /// 工单查询
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    //protected string SearchOrder(JObject obj)
    //{
    //    string userName = string.Empty;
    //    string result = string.Empty;
    //    string state = string.Empty;
    //    int stateMain = 0;
    //    int stateDetail = 0;
    //    string getWorkgroup = string.Empty;
    //    string customName = string.Empty;
    //    string brandName = string.Empty;
    //    int userID = 0;
    //    int workgroupID = 0;

    //    try
    //    {
    //        userName = obj["userName"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    userName = userName.Trim('"');
    //    if (string.IsNullOrEmpty(userName))
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取userName值为空\r\n", null);
    //        return result;
    //    }
    //    UserInfo user = UserBLL.Get(userName);
    //    if (user == null)
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "不存在\r\n", null);
    //        return result;
    //    }

    //    try
    //    {
    //        state = obj["state"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取state值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取state值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    state = state.Trim('"');
    //    if (state == "0")
    //    {
    //        //因为待分配分两种，SysEnum.CallStateDetails.等待工程师上门和SysEnum.CallStateDetails.等待第三方响应
    //        stateMain = 2;
    //        stateDetail=100;
    
    //    }
    //    else if (state == "1")
    //    {
    //        stateMain = 2;
    //        stateDetail = (int)SysEnum.CallStateDetails.到达门店处理;
    //    }
    //    else if (state == "2")
    //    {
    //        stateMain = 2;
    //        stateDetail = (int)SysEnum.CallStateDetails.上门支持;
    //    }
    //    else if (state == "3")
    //    {
    //        stateMain = 3;
            
    //    }
    //    else {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取state值为" + state + "系统中不存在该状态\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取state值为" + state + "系统中不存在该状态，错误原因：" + result  + "\r\n", null);
    //        return result;
    //    }

    //    try
    //    {
    //        customName = obj["companyName"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取companyName值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取companyName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    customName = customName.Trim('"');
    //    customName = customName.Trim();
       
        
    //    try
    //    {
    //        brandName = obj["productName"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取brandName值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取brandName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    brandName = brandName.Trim('"');
    //    brandName = brandName.Trim();

    //    try
    //    {
    //        brandName = obj["productName"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取brandName值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取brandName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    brandName = brandName.Trim('"');
    //    brandName = brandName.Trim();

    //    try
    //    {
    //        getWorkgroup = obj["all"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取all值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取all值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    getWorkgroup = getWorkgroup.Trim('"');
    //    getWorkgroup = getWorkgroup.Trim();
    //    if (string.IsNullOrEmpty(getWorkgroup))
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取getWorkgroup值为空\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取getWorkgroup值为空\r\n", null);
    //        return result;
    //    }
    //    if (getWorkgroup == "0")
    //    {
    //        userID = user.ID;
    //    }
    //    else {

    //        workgroupID = user.WorkGroupID;
    //    }


    //    List<CallInfo> callList = CallBLL.GetMyCallsForOnsiteEngineer(stateMain, stateDetail, userID, workgroupID, customName, brandName);

    //    if (callList.Count > 0)
    //    {
    //        string callInfo = string.Empty;
    //        for (int i = 0; i < callList.Count; i++)
    //        {
    //            if (i + 1 == callList.Count)
    //            {
    //                //string callStateDetail = string.Empty;
    //                //if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应)
    //                //{
    //                //    //待上门
    //                //    callStateDetail = "0";
    //                //}
    //                //else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
    //                //{
    //                //    //处理中
    //                //    callStateDetail = "1";
    //                //}
    //                //else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.上门支持)
    //                //{
    //                //    //已离场
    //                //    callStateDetail = "2";
    //                //}
    //                //else
    //                //{
    //                //    callStateDetail = "";
    //                //}
    //                KeyValueDictionary paramDic = new KeyValueDictionary();
    //                paramDic.Add("orderId", callList[i].No);
    //                paramDic.Add("gps", "");
    //                StoreInfo store = StoresBLL.Get(callList[i].StoreID);
    //                paramDic.Add("address", store == null ? "" : store.Address);
    //                paramDic.Add("customerName", callList[i].CustomerName);
    //                paramDic.Add("customerLogo", "");
    //                paramDic.Add("orderDescribe", callList[i].Details);

    //                //paramDic.Add("state", callStateDetail);
    //                paramDic.Add("desc", callList[i].Details);
    //                callInfo = WebUtil.BuildQueryJson(paramDic);
    //                if (callInfo.EndsWith(","))
    //                {
    //                    callInfo = callInfo.Remove(callInfo.Length - 1);
    //                }
    //                callInfo = "{" + callInfo + "}";
    //                result += callInfo;
    //            }
    //            else
    //            {
    //                //string callStateDetail = string.Empty;
    //                //if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待工程师上门 || callList[i].StateDetail == (int)SysEnum.CallStateDetails.等待第三方响应)
    //                //{
    //                //    //待上门
    //                //    callStateDetail = "0";
    //                //}
    //                //else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.到达门店处理)
    //                //{
    //                //    //处理中
    //                //    callStateDetail = "1";
    //                //}
    //                //else if (callList[i].StateDetail == (int)SysEnum.CallStateDetails.上门支持)
    //                //{
    //                //    //已离场
    //                //    callStateDetail = "2";
    //                //}
    //                //else
    //                //{
    //                //    callStateDetail = "";
    //                //}
    //                KeyValueDictionary paramDic = new KeyValueDictionary();
    //                paramDic.Add("orderId", callList[i].No);
    //                paramDic.Add("gps", "");
    //                StoreInfo store = StoresBLL.Get(callList[i].StoreID);
    //                paramDic.Add("address", store == null ? "" : store.Address);
    //                paramDic.Add("customerName", callList[i].CustomerName);
    //                paramDic.Add("customerLogo", "");
    //                paramDic.Add("orderDescribe", callList[i].Details);
    //                //paramDic.Add("state", callStateDetail);
    //                paramDic.Add("desc", callList[i].Details);
    //                callInfo = WebUtil.BuildQueryJson(paramDic);
    //                if (callInfo.EndsWith(","))
    //                {
    //                    callInfo = callInfo.Remove(callInfo.Length - 1);
    //                }
    //                callInfo = "{" + callInfo + "},";
    //                result += callInfo;
    //            }
    //        }
    //        result = "{\"code\":\"SearchOrder\",\"list\":[" + result + "],\"desc\":\"\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，用户：" + userName + "工单查询列表为：" + result + "\r\n", null);
    //    }
    //    else
    //    {
    //        result = "{\"code\":\"SearchOrder\",\"list\":[],\"desc\":\"\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，用户：" + userName + "工单查询列表为空\r\n", null);
    //    }
    //    return result;
    //}
    #endregion

    /// <summary>
    /// 工单查询,调用SP实现
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string SPSearchOrder(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        string state = string.Empty;
        string getWorkgroup = string.Empty;
        string customName = string.Empty;
        string brandName = string.Empty;
        string errorStartTime = string.Empty;
        string errorEndTime = string.Empty;
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
            //stateMain = 2;
            //stateDetail = 100;
            state = "w";

        }
        else if (state == "1")
        {
            //stateMain = 2;
            //stateDetail = (int)SysEnum.CallStateDetails.到达门店处理;
            state = "p";
        }
        else if (state == "2")
        {
            //stateMain = 2;
            //stateDetail = (int)SysEnum.CallStateDetails.上门支持;
            state = "o";
        }
        else if (state == "3")
        {
            //stateMain = 3;
            state = "f";

        }
        else
        {
            state = "";
            //result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取state值为" + state + "系统中不存在该状态\"}";
            //Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取state值为" + state + "系统中不存在该状态，错误原因：" + result + "\r\n", null);
            //return result;
        }
        #region 注释掉获取客户、品牌信息
        //try
        //{
        //    customName = obj["companyName"].ToString();
        //}
        //catch (Exception ex)
        //{
        //    result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取companyName值时错误\"}";
        //    Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取companyName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
        //    return result;
        //}
        //customName = customName.Trim('"');
        //customName = customName.Trim();


        //try
        //{
        //    brandName = obj["productName"].ToString();
        //}
        //catch (Exception ex)
        //{
        //    result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取brandName值时错误\"}";
        //    Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取brandName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
        //    return result;
        //}
        //brandName = brandName.Trim('"');
        //brandName = brandName.Trim();
        #endregion
        try
        {
            errorStartTime = obj["startTime"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取startTime值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取startTime值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        errorStartTime = errorStartTime.Trim('"');
        errorStartTime = errorStartTime.Trim();
        //if (string.IsNullOrEmpty(errorStartTime))
        //{
        //    result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"startTime值不能为空\"}";
        //    Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，startTime值为空，错误原因为\r\n", null);
        //    return result;
        //}

        try
        {
            errorEndTime = obj["endTime"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取endTime值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，解析JSON数据获取endTime值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        errorEndTime = errorEndTime.Trim('"');
        errorEndTime = errorEndTime.Trim();
        //if (string.IsNullOrEmpty(errorEndTime))
        //{
        //    result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"endTime值不能为空\"}";
        //    Logger.GetLogger(this.GetType()).Info("APP SearchOrder接口被调用，endTime值为空，错误原因为\r\n", null);
        //    return result;
        //}

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
            getWorkgroup = "false";
        }
        else
        {
            getWorkgroup = "true";
        }
        DataTable dt = new DataTable();
        string spReturnValue =string.Empty;
        dt = CallBLL.GetMyCallsForOnsiteEngineerBySp(userName, state, getWorkgroup, customName, brandName,errorStartTime,errorEndTime, out  spReturnValue);
        //List<CallInfo> callList = CallBLL.GetMyCallsForOnsiteEngineer(stateMain, stateDetail, userID, workgroupID, customName, brandName);
        if (spReturnValue == "not binding")
        {
            result = "{\"code\":\"SearchOrder\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP MyOrder接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }

        if (dt.Rows.Count > 0)
        {
            string callInfo = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                KeyValueDictionary paramDic = new KeyValueDictionary();
                paramDic.Add("orderId", dr["f_No"].ToString());
                paramDic.Add("gps", dr["f_GPS"].ToString());
                paramDic.Add("address", DeleteSpecialChar(dr["f_Address"].ToString()));
                paramDic.Add("storeName", DeleteSpecialChar(dr["f_Name"].ToString()));
                paramDic.Add("customerName", dr["f_CustomerName"].ToString());
                paramDic.Add("productName", dr["f_BrandName"].ToString());
                paramDic.Add("customerLogo", "");
                string callDetails = DeleteSpecialChar(dr["f_Details"].ToString());
                paramDic.Add("orderDescribe", callDetails);
                //paramDic.Add("state", callStateDetail);
                //到期时间
                string slaDateEndStr = string.Empty;
                if (dr["f_SLADateEnd"] != null && !string.IsNullOrEmpty(dr["f_SLADateEnd"].ToString()))
                {
                    DateTime slaDateEnd = Convert.ToDateTime(dr["f_SLADateEnd"].ToString());
                    if (slaDateEnd.Year != 1970)
                    {
                        slaDateEndStr = slaDateEnd.ToString("yyyy-MM-dd HH:mm");
                    }
                }
                paramDic.Add("SLA", slaDateEndStr);
                paramDic.Add("desc", callDetails);
                string callState = dr["f_CallStateA"].ToString().Trim();
                if (callState == "2-6" || callState == "2-7")
                { 
                    callState="0";
                    paramDic.Add("state", callState);
                }
                else if (callState == "2-12")
                {
                    callState = "1";
                    paramDic.Add("state", callState);
                }
                else if (callState == "2-9")
                {
                    callState = "2";
                    paramDic.Add("state", callState);
                }
                else if (callState == "3-")
                {
                    callState = "3";
                    paramDic.Add("state", callState);
                }
                else {
                    callState = "2";
                    paramDic.Add("state", callState);
                }
                paramDic.Add("majorUserName", dr["f_MajorUserName"].ToString().Trim());
                callInfo = WebUtil.BuildQueryJson(paramDic);
                if (callInfo.EndsWith(","))
                {
                    callInfo = callInfo.Remove(callInfo.Length - 1);
                }
                callInfo = "{" + callInfo + "},";
                result += callInfo;

            }
            if (result.EndsWith(","))
            {
                result = result.Remove(result.Length - 1);
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
    /// 签到、离场、更换现场工程师
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string AssignOrder(JObject obj)
    {
        string userName = string.Empty;
        string callNo = string.Empty;
        string oper = string.Empty;
        string operTarget = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }

        try
        {
            callNo = obj["orderId"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取orderId值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取orderId值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        callNo = callNo.Trim('"');
        if (string.IsNullOrEmpty(callNo))
        {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取orderId值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取orderId值为空\r\n", null);
            return result;
        }
        CallInfo callInfo = CallBLL.Get(callNo);
        if (callInfo == null)
        {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"单号：" + callNo + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，单号" + callNo + "不存在\r\n", null);
            return result;
        }


        try
        {
            oper = obj["oper"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取oper值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        oper = oper.Trim('"');
        if (string.IsNullOrEmpty(oper))
        {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取oper值为空\r\n", null);
            return result;
        }
        //到达门店
        if (oper == "0") {
            if ((SysEnum.CallStateDetails)callInfo.StateDetail == SysEnum.CallStateDetails.等待第三方响应 || (SysEnum.CallStateDetails)callInfo.StateDetail == SysEnum.CallStateDetails.等待工程师上门 || (SysEnum.CallStateDetails)callInfo.StateDetail == SysEnum.CallStateDetails.等待厂商响应)
            {
                CallStepInfo sinfo = new CallStepInfo();
                sinfo.StepType = (int)SysEnum.StepType.到达门店处理;
                sinfo.UserID = user.ID;
                sinfo.UserName = user.Name;
                sinfo.AddDate = DateTime.Now;
                sinfo.CallID = callInfo.ID;
                sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(callInfo.ID) + 1;
                sinfo.DateBegin = DateTime.Now;
                sinfo.DateEnd = sinfo.DateBegin;
                sinfo.StepName = SysEnum.CallStateDetails.到达门店处理.ToString();
                sinfo.IsSolved = false;
                sinfo.SolutionID = 0;
                sinfo.SolutionName = "";
                sinfo.Details = "工程师使用APP签到；";

                sinfo.MajorUserID = user.ID;
                sinfo.MajorUserName = user.Name;

                callInfo.StateDetail = (int)SysEnum.CallStateDetails.到达门店处理;
                if (CallStepBLL.AddCallStep_UpdateCall(callInfo, sinfo))
                {
                    #region 插入一条记录到跑马灯
                    MarqueeMessage mInfo = new MarqueeMessage();
                    mInfo.No = callInfo.No;
                    mInfo.MaintainUserID = callInfo.MaintainUserID;
                    mInfo.MaintainUserName = callInfo.MaintaimUserName.Trim();
                    mInfo.MajorUserID = user.ID;
                    mInfo.MajorUserName = user.Code.Trim();
                    mInfo.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " APP操作到场签到";
                    mInfo.IsRead = false;
                    MarqueeMessageBLL.AddBySP(mInfo);

                    MarqueeMessageReport mInfoReport = new MarqueeMessageReport();
                    mInfoReport.No = callInfo.No;
                    mInfoReport.MaintainUserID = callInfo.MaintainUserID;
                    mInfoReport.MaintainUserName = callInfo.MaintaimUserName.Trim();
                    mInfoReport.MajorUserID = user.ID;
                    mInfoReport.MajorUserName = user.Code.Trim();
                    mInfoReport.ActionTime = DateTime.Now;
                    mInfoReport.Content = "到场签到";
                    MarqueeMessageReportBLL.Add(mInfoReport);
                    #endregion
                    result = "{\"code\":\"AssignOrder\",\"result\":\"1\",\"desc\":\"\"}";
                    Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，工程师："+userName+"签到成功,单号："+callNo+"。\r\n", null);
                    return result;
                
                }
                else {
                    result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"提交签到信息失败！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，工程师：" + userName + "签到失败,单号：" + callNo + "。\r\n", null);
                    return result;
                }
            
            }
            else {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"该工单现在的状态不能做签到处理。\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，该工单现在的状态不能做签到处理。\r\n", null);
                return result;
            }
        }
        //离场
        else if (oper == "1")
        {
            if ((SysEnum.CallStateDetails)callInfo.StateDetail == SysEnum.CallStateDetails.到达门店处理 )
            {
                CallStepInfo sinfo = new CallStepInfo();
                sinfo.StepType = (int)SysEnum.StepType.上门详细;
                sinfo.MajorUserID = user.ID;
                sinfo.MajorUserName = user.Name;
                sinfo.UserID = user.ID;
                sinfo.UserName = user.Name;
                sinfo.AddDate = DateTime.Now;
                sinfo.CallID = callInfo.ID;
                sinfo.StepIndex = CallStepBLL.GetMaxStepIndex(callInfo.ID) + 1;
                sinfo.DateBegin = sinfo.DateEnd = DateTime.Now;
                sinfo.Details = "工程师使用APP离场；";
                sinfo.StepName = SysEnum.CallStateDetails.上门支持.ToString();
                sinfo.IsSolved = false;
                
                sinfo.SolutionID = 0;
                sinfo.SolutionName = "";
                callInfo.StateMain = (int)SysEnum.CallStateMain.处理中;
                callInfo.StateDetail = (int)SysEnum.CallStateDetails.上门支持;
                if (CallStepBLL.AddCallStep_UpdateCall(callInfo, sinfo))
                {
                    #region 插入一条记录到跑马灯
                    MarqueeMessage mInfo = new MarqueeMessage();
                    mInfo.No = callInfo.No;
                    mInfo.MaintainUserID = callInfo.MaintainUserID;
                    mInfo.MaintainUserName = callInfo.MaintaimUserName.Trim();
                    mInfo.MajorUserID = user.ID;
                    mInfo.MajorUserName = user.Code.Trim();
                    mInfo.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " APP操作离场";
                    mInfo.IsRead = false;
                    MarqueeMessageBLL.AddBySP(mInfo);

                    MarqueeMessageReport mInfoReport = new MarqueeMessageReport();
                    mInfoReport.No = callInfo.No;
                    mInfoReport.MaintainUserID = callInfo.MaintainUserID;
                    mInfoReport.MaintainUserName = callInfo.MaintaimUserName.Trim();
                    mInfoReport.MajorUserID = user.ID;
                    mInfoReport.MajorUserName = user.Code.Trim();
                    mInfoReport.ActionTime = DateTime.Now;
                    mInfoReport.Content = "离场";
                    MarqueeMessageReportBLL.Add(mInfoReport);
                    #endregion
                    result = "{\"code\":\"AssignOrder\",\"result\":\"1\",\"desc\":\"\"}";
                    Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，工程师："+userName+"离场成功,单号："+callNo+"。\r\n", null);
                    return result;
                
                }
                else {
                    result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"提交离场信息失败！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，工程师：" + userName + "离场失败,单号：" + callNo + "。\r\n", null);
                    return result;
                }
            
            }
            else {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"该工单现在的状态不能做离场处理。\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，该工单现在的状态不能做离场处理。\r\n", null);
                return result;
            }

        }
        //更换现场工程师
        else if (oper == "2")
        {
            UserInfo newEngineer = new UserInfo();
            int engineerID = 0;
            try
            {
                operTarget = obj["operTarget"].ToString();
            }
            catch (Exception ex)
            {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取operTarget值时错误\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取operTarget值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
                return result;
            }
            operTarget = operTarget.Trim('"');
            if (string.IsNullOrEmpty(operTarget))
            {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取operTarget的值为空\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取operTarget的值为空\r\n", null);
                return result;
            }
            try
            {
                engineerID = Convert.ToInt32(operTarget);
            }
            catch (Exception ex)
            {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取operTarget值为" + operTarget + "，不能转换为用户编号\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取operTarge值为" + operTarget + "，不能转换为用户编号，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
                return result;
            }
            newEngineer = UserBLL.Get(engineerID);
            if (newEngineer==null)
            {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取operTarget值为" + operTarget + "，未能找到对应的用户信息。\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取operTarget值为" + operTarget + "，未能找到对应的用户信息。\r\n", null);
                return result;
            }


            CallStepInfo oldcsinfo  = CallStepBLL.GetLast(callInfo.ID, SysEnum.StepType.上门安排);
            if (null == oldcsinfo)
            {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"该工单没有上门安排记录。\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，该工单没有上门安排记录。\r\n", null);
                return result;
            }
            if (oldcsinfo.StepName.Trim() != SysEnum.CallStateDetails.等待工程师上门.ToString().Trim()&&oldcsinfo.StepName.Trim() !=  SysEnum.CallStateDetails.等待第三方响应.ToString().Trim())
            {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"该工单不符合重新指派工程师上门的条件！\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，该工单不符合重新指派工程师上门的条件！\r\n", null);
                return result;
            }
            if (!checkLeader(user))
            {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"当前用户"+userName+"没有权限安排工单！\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，当前用户" + userName + "没有权限安排工单！\r\n", null);
                return result;
            }

           
            AssignInfo asinfo = new AssignInfo();
            AssignInfo asold = AssignBLL.GetChangeEngineerMax(oldcsinfo.CallID);
            asinfo.AddDate = DateTime.Now;
            asinfo.CallID = oldcsinfo.CallID;
            asinfo.UseID = newEngineer.ID;
            asinfo.UserName = newEngineer.Name;
            asinfo.CreatorID = user.ID;
            asinfo.CreatorName = user.Name;
            asinfo.WorkGroupID = newEngineer.WorkGroupID;
            asinfo.CrossWorkGroup = false;
            asinfo.AssignType = 1;

            if (null == asold || asold.ID == 0)
            {
                asinfo.OldID = oldcsinfo.MajorUserID;
                asinfo.OldName = oldcsinfo.MajorUserName;
                asinfo.Step = 1;
            }
            else
            {
                asinfo.OldID = oldcsinfo.MajorUserID;
                asinfo.OldName = oldcsinfo.MajorUserName;
                asinfo.Step = asold.Step + 1;
            }

            CallStepInfo newcsinfo = new CallStepInfo();
            int newcsIndex = CallStepBLL.GetMaxStepIndex(oldcsinfo.CallID);
            newcsinfo.CallID = oldcsinfo.CallID;
            newcsinfo.MajorUserID = newEngineer.ID;
            newcsinfo.MajorUserName = newEngineer.Name;
            newcsinfo.StepIndex = newcsIndex + 1;
            newcsinfo.UserID = user.ID;
            newcsinfo.UserName = user.Name;
            newcsinfo.AddDate = DateTime.Now.AddSeconds(1);
            newcsinfo.DateBegin = DateTime.Now;
            newcsinfo.DateEnd = DateTime.Now;
            newcsinfo.StepName = SysEnum.CallStateDetails.等待工程师上门.ToString();
            newcsinfo.StepType = (int)SysEnum.StepType.上门安排;
            newcsinfo.IsSolved = false;
            newcsinfo.SolutionID = 0;
            newcsinfo.SolutionName = "";
            newcsinfo.Details = "";


            if (CallStepBLL.Add(newcsinfo) > 0)
            {
                AssignBLL.Add(asinfo);
                #region 插入一条记录到跑马灯
                MarqueeMessage mInfo = new MarqueeMessage();
                mInfo.No = callInfo.No;
                mInfo.MaintainUserID = callInfo.MaintainUserID;
                mInfo.MaintainUserName = callInfo.MaintaimUserName.Trim();
                mInfo.MajorUserID = user.ID;
                mInfo.MajorUserName = user.Code.Trim();
                mInfo.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " APP操作更换现场工程师";
                mInfo.IsRead = false;
                MarqueeMessageBLL.AddBySP(mInfo);

                MarqueeMessageReport mInfoReport = new MarqueeMessageReport();
                mInfoReport.No = callInfo.No;
                mInfoReport.MaintainUserID = callInfo.MaintainUserID;
                mInfoReport.MaintainUserName = callInfo.MaintaimUserName.Trim();
                mInfoReport.MajorUserID = user.ID;
                mInfoReport.MajorUserName = user.Code.Trim();
                mInfoReport.ActionTime = DateTime.Now;
                mInfoReport.Content = "更换现场工程师";
                MarqueeMessageReportBLL.Add(mInfoReport);
                #endregion
                result = "{\"code\":\"AssignOrder\",\"result\":\"1\",\"desc\":\"\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，工程师：" + userName + "操作更换现场工程师成功,单号：" + callNo + "。\r\n", null);
                return result;
                
            }
            else
            {
                result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"提交安排工单数据失败\"}";
                Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，工程师：" + userName + "操作更换现场工程师失败,单号：" + callNo + "。\r\n", null);
                return result;
              
            }

        }
        else {
            result = "{\"code\":\"AssignOrder\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper的值为"+oper+" 对应的操作不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP AssignOrder接口被调用，解析JSON数据获取oper的值为" + oper + " 对应的操作不存在\r\n", null);
            return result;
        }
    }

    /// <summary>
    /// 工作记录，工程师评价
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string WorkRecord(JObject obj)
    {
        string userName = string.Empty;
        string callNo = string.Empty;
        string desc = string.Empty;
        string technologyScore = string.Empty;
        int tScore = 0;
        string attitudeScore = string.Empty;
        int aScore = 0;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }

        try
        {
            callNo = obj["orderId"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取orderId值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，解析JSON数据获取orderId值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        callNo = callNo.Trim('"');
        if (string.IsNullOrEmpty(callNo))
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取orderId值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，解析JSON数据获取orderId值为空\r\n", null);
            return result;
        }
        CallInfo callInfo = CallBLL.Get(callNo);
        if (callInfo == null)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"单号：" + callNo + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，单号" + callNo + "不存在\r\n", null);
            return result;
        }

        try
        {
            technologyScore = obj["technologyScore"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取technologyScore值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，解析JSON数据获取technologyScore值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        technologyScore = technologyScore.Trim('"');
        try 
        {
            tScore = Convert.ToInt32(technologyScore);
        }
        catch (Exception ex) {

            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"technologyScore的值必须为整数\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，technologyScore的值必须为整数，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }

        try
        {
            attitudeScore = obj["attitudeScore"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取attitudeScore值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，解析JSON数据获取attitudeScore值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        attitudeScore = attitudeScore.Trim('"');
        try
        {
            aScore = Convert.ToInt32(attitudeScore);
        }
        catch (Exception ex)
        {

            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"attitudeScore的值必须为整数\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，attitudeScore的值必须为整数，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }

        try
        {
            desc = obj["desc"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取desc值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，解析JSON数据获取desc值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        desc = desc.Trim('"');

        if (desc.Length > 50)
        {

            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"desc值长度过长\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，desc值长度过长\r\n", null);
            return result;

        }


        CallStepInfo stepinfo = CallStepBLL.GetLast(callInfo.ID, SysEnum.StepType.到达门店处理);
        if (stepinfo == null)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"该工单没有上门处理记录，不能做评价\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，该工单没有上门处理记录，不能做评价" + result + "\r\n", null);
            return result;
        }
        List<CommentInfo> commentInfoList = CommentBLL.GetList(" f_CallID = " + callInfo.ID + " AND f_CallStepID=" + stepinfo.ID + " AND f_IsDropInUserDoIt=1 AND f_ByMachine='APP' order by f_AddDate desc ");
        if (commentInfoList.Count > 0)
        { 
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"该工单的本次上门处理记录已做过评价，不能做重复评价\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，该工单的本次上门处理记录已做过评价，不能做重复评价" + result + "\r\n", null);
            return result;
        }
        CommentInfo info = new CommentInfo();
        info.AddDate = DateTime.Now;
        info.ByMachine = "APP";
        info.CallID = callInfo.ID;
        info.CallStepID = stepinfo.ID;
        info.Details = desc;
        info.DropInUserID = stepinfo.MajorUserID;
        info.IsDropInUserDoIt = true;
        info.Score2 = tScore;
        info.Score3 =aScore;
        info.Score = info.Score2 + info.Score3;
        info.SupportUserID = callInfo.MaintainUserID;
        info.WorkGroupID = user.WorkGroupID;
        info.ID = CommentBLL.Add(info);
        #region 插入一条记录到跑马灯
        MarqueeMessage mInfo = new MarqueeMessage();
        mInfo.No = callInfo.No;
        mInfo.MaintainUserID = callInfo.MaintainUserID;
        mInfo.MaintainUserName = callInfo.MaintaimUserName.Trim();
        mInfo.MajorUserID = user.ID;
        mInfo.MajorUserName = user.Code.Trim();
        mInfo.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm")+" APP提交工作记录";
        mInfo.IsRead = false;
        MarqueeMessageBLL.AddBySP(mInfo);

        MarqueeMessageReport mInfoReport = new MarqueeMessageReport();
        mInfoReport.No = callInfo.No;
        mInfoReport.MaintainUserID = callInfo.MaintainUserID;
        mInfoReport.MaintainUserName = callInfo.MaintaimUserName.Trim();
        mInfoReport.MajorUserID = user.ID;
        mInfoReport.MajorUserName = user.Code.Trim();
        mInfoReport.ActionTime = DateTime.Now;
        mInfoReport.Content = "提交工作记录";
        MarqueeMessageReportBLL.Add(mInfoReport);
        #endregion
        if (info.ID > 0)
        {
            result = "{\"code\":\"WorkRecord\",\"result\":\"1\",\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，工程师：" + userName + "对工单"+callNo+"评价成功\r\n", null);
            return result;
        }
        else {
            result = "{\"code\":\"WorkRecord\",\"result\":\"0\",\"desc\":\"工作记录数据写入数据库失败\"}";
            Logger.GetLogger(this.GetType()).Info("APP WorkRecord接口被调用，工程师：" + userName + "对工单" + callNo + "评价失败\r\n", null);
            return result;
        }


    }

    /// <summary>
    /// 图片上传
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string UploadImageUrl(JObject obj)
    {
        string userName = string.Empty;
        string callNo = string.Empty;
        string urlStr = string.Empty;

        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }

        try
        {
            callNo = obj["orderId"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"解析JSON数据获取orderId值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，解析JSON数据获取orderId值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        callNo = callNo.Trim('"');
        if (string.IsNullOrEmpty(callNo))
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"解析JSON数据获取orderId值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，解析JSON数据获取orderId值为空\r\n", null);
            return result;
        }
        CallInfo callInfo = CallBLL.Get(callNo);
        if (callInfo == null)
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"单号：" + callNo + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，单号" + callNo + "不存在\r\n", null);
            return result;
        }

        try
        {
            urlStr = obj["url"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"解析JSON数据获取url值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，解析JSON数据获取url值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        urlStr = urlStr.Trim('"');
        if (string.IsNullOrEmpty(urlStr))
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"解析JSON数据获取url值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，解析JSON数据获取url值为空\r\n", null);
            return result;
        }

        AttachmentInfo.EUserFor eUserFor = AttachmentInfo.EUserFor.Call;
        AttachmentInfo info = new AttachmentInfo();
        info.Addtime = DateTime.Now;
        info.CallID = callInfo.ID;
        //插入的图片与该次上门记录关联 2015.12.18 ZQL加
        List<CallStepInfo> callstepList = CallStepBLL.GetList(callInfo.ID, SysEnum.StepType.到达门店处理);
        if (callstepList.Count > 0)
        {
            callstepList.OrderByDescending(step => step.ID);
            if (callstepList[0].Details.Contains("工程师使用APP签到") && callstepList[0].MajorUserID == user.ID)
            {
                info.CallStepID = callstepList[0].ID;
            }
            else {
                info.CallStepID = 0;
            }
        }
        
        info.ContentType = "image";
        info.DirID = 0;
        info.Ext = "";
        info.UserID = user.ID;
        info.UserName = user.Name;

        info.FilePath = urlStr;
        info.FileSize = 0;
        info.Memo = "";
        info.Title = "APP上传的图片列表";
        info.UseFor = eUserFor.ToString();
        
        info.ID = AttachmentBLL.Add(info);
        if (info.ID > 0)
        {
            #region 插入一条记录到跑马灯
            MarqueeMessage mInfo = new MarqueeMessage();
            mInfo.No = callInfo.No;
            mInfo.MaintainUserID = callInfo.MaintainUserID;
            mInfo.MaintainUserName = callInfo.MaintaimUserName.Trim();
            mInfo.MajorUserID = user.ID;
            mInfo.MajorUserName = user.Code.Trim();
            mInfo.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " APP操作上传图片";
            mInfo.IsRead = false;
            MarqueeMessageBLL.AddBySP(mInfo);
            #endregion
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"1\",\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，保存url连接成功\r\n", null);
            return result;
        }
        else
        {
            result = "{\"code\":\"UploadImageUrl\",\"result\":\"0\",\"desc\":\"提交数据到数据库失败\"}";
            Logger.GetLogger(this.GetType()).Info("APP UploadImageUrl接口被调用，提交数据到数据库失败\r\n", null);
            return result;
        }


    }

    /// <summary>
    /// 客户公司列表
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string CompanyList(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"CompanyList\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP CompanyList接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"CompanyList\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP CompanyList接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"CompanyList\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP CompanyList接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }

        List<CustomersInfo> customerList = CustomersBLL.GetList(user);

        if (customerList.Count > 0)
        {
            string customerInfo = string.Empty;
            for (int i = 0; i < customerList.Count; i++)
            {
                if (i + 1 == customerList.Count)
                {
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("companyId", customerList[i].ID);
                    paramDic.Add("companyName", customerList[i].Name);

                    customerInfo = WebUtil.BuildQueryJson(paramDic);
                    if (customerInfo.EndsWith(","))
                    {
                        customerInfo = customerInfo.Remove(customerInfo.Length - 1);
                    }
                    customerInfo = "{" + customerInfo + "}";
                    result += customerInfo;
                }
                else
                {

                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("companyId", customerList[i].ID);
                    paramDic.Add("companyName", customerList[i].Name);

                    customerInfo = WebUtil.BuildQueryJson(paramDic);
                    if (customerInfo.EndsWith(","))
                    {
                        customerInfo = customerInfo.Remove(customerInfo.Length - 1);
                    }
                    customerInfo = "{" + customerInfo + "},";
                    result += customerInfo;
                }
            }
            result = "{\"code\":\"CompanyList\",\"list\":[" + result + "],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP CompanyList接口被调用，用户：" + userName + "客户列表为：" + result + "\r\n", null);
        }
        else
        {
            result = "{\"code\":\"CompanyList\",\"list\":[],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP CompanyList接口被调用，用户：" + userName + "客户列表为空\r\n", null);
        }
        return result;
    }
    /// <summary>
    /// 品牌列表
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string ProductList(JObject obj)
    {
        string userName = string.Empty;
        string companyName = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"ProductList\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP ProductList接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"ProductList\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP ProductList接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"ProductList\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP ProductList接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }


        try
        {
            companyName = obj["companyName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"ProductList\",\"result\":\"0\",\"desc\":\"解析JSON数据获取companyName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP ProductList接口被调用，解析JSON数据获取companyName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        companyName = companyName.Trim('"');
        if (string.IsNullOrEmpty(companyName))
        {
            result = "{\"code\":\"ProductList\",\"result\":\"0\",\"desc\":\"解析JSON数据获取companyName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP ProductList接口被调用，解析JSON数据获取companyName值为空\r\n", null);
            return result;
        }
        CustomersInfo customer = CustomersBLL.Get(companyName);
        if (customer == null)
        {
            result = "{\"code\":\"ProductList\",\"result\":\"0\",\"desc\":\"公司名称：" + companyName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP ProductList接口被调用，公司名称：" + companyName + "不存在\r\n", null);
            return result;
        }

        List<BrandInfo> brandList = BrandBLL.GetList(customer.ID);

        if (brandList.Count > 0)
        {
            string brandInfo = string.Empty;
            for (int i = 0; i < brandList.Count; i++)
            {
                if (i + 1 == brandList.Count)
                {
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("productId", brandList[i].ID);
                    paramDic.Add("productName", brandList[i].Name);

                    brandInfo = WebUtil.BuildQueryJson(paramDic);
                    if (brandInfo.EndsWith(","))
                    {
                        brandInfo = brandInfo.Remove(brandInfo.Length - 1);
                    }
                    brandInfo = "{" + brandInfo + "}";
                    result += brandInfo;
                }
                else
                {

                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    paramDic.Add("productId", brandList[i].ID);
                    paramDic.Add("productName", brandList[i].Name);

                    brandInfo = WebUtil.BuildQueryJson(paramDic);
                    if (brandInfo.EndsWith(","))
                    {
                        brandInfo = brandInfo.Remove(brandInfo.Length - 1);
                    }
                    brandInfo = "{" + brandInfo + "},";
                    result += brandInfo;
                }
            }
            result = "{\"code\":\"ProductList\",\"list\":[" + result + "],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP ProductList接口被调用，用户：" + userName + "品牌列表为：" + result + "\r\n", null);
        }
        else
        {
            result = "{\"code\":\"ProductList\",\"list\":[],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP ProductList接口被调用，用户：" + userName + "品牌列表为空\r\n", null);
        }
        return result;
    }

    #region 原打卡签到、签退
    /// <summary>
    /// 打卡签到、签退
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    //protected string SignIn(JObject obj)
    //{
    //    string userName = string.Empty;
    //    string oper = string.Empty;
    //    string gps = string.Empty;
    //    string result = string.Empty;
    //    try
    //    {
    //        userName = obj["userName"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    userName = userName.Trim('"');
    //    if (string.IsNullOrEmpty(userName))
    //    {
    //        result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取userName值为空\r\n", null);
    //        return result;
    //    }
    //    UserInfo user = UserBLL.Get(userName);
    //    if (user == null)
    //    {
    //        result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + "不存在\r\n", null);
    //        return result;
    //    }
    //    try
    //    {
    //        gps = obj["gps"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取gps值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取gps值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    gps = gps.Trim('"');
    //    //if (string.IsNullOrEmpty(gps))
    //    //{
    //    //    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取gps值为空\"}";
    //    //    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取gps值为空\r\n", null);
    //    //    return result;
    //    //}
    //    try
    //    {
    //        oper = obj["oper"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper值时错误\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取oper值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
    //        return result;
    //    }
    //    oper = oper.Trim('"');
    //    if (string.IsNullOrEmpty(oper))
    //    {
    //        result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper值为空\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取oper值为空\r\n", null);
    //        return result;
    //    }
    //    if (oper == "0")
    //    {
    //        APPLoginLogoutLog login = APPLoginLogoutLogBLL.Get(user.ID.ToString());
    //        if (login == null||login.LoginTime.Date!=DateTime.Now.Date)
    //        {
    //            result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"今天还没有签到，不能签退！\"}";
    //            Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，今天还没有签到，不能签退！\r\n", null);
    //            return result;
    //        }else{
    //            if (login.LogoutTime.Date==DateTime.Now.Date)
    //            {
    //                result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"今天已经签退过了，不能重复签退！\"}";
    //                Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，今天已经签退过了，不能重复签退！\r\n", null);
    //                return result;
    //            }
    //            else {
    //                login.LogoutTime = DateTime.Now;
    //                login.LogoutLocation = gps;
    //                login.Status = 1;
    //                if (APPLoginLogoutLogBLL.Edit(login))
    //                {
    //                    result = "{\"code\":\"SignIn\",\"result\":\"1\",\"desc\":\"\"}";
    //                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + "签退成功\r\n", null);
    //                    return result;
    //                }
    //                else
    //                {
    //                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"签退数据保存到数据库时失败\"}";
    //                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + " 签退数据保存到数据库时失败\r\n", null);
    //                    return result;
    //                }
    //            }

    //        }
    //    }else if(oper=="1"){
    //        APPLoginLogoutLog login = APPLoginLogoutLogBLL.Get(user.ID.ToString());
    //        if (login == null)
    //        {
    //            login = new APPLoginLogoutLog();
    //            login.LoginTime = DateTime.Now;
    //            login.OpenID = user.ID.ToString();
    //            login.LoginLocation = gps;
    //            login.Status = 0;
    //            login.LogoutTime = DateTime.Now.AddYears(-100);
    //            login.LogoutLocation = "";
    //            if (APPLoginLogoutLogBLL.Add(login) > 0)
    //            {
    //                result = "{\"code\":\"SignIn\",\"result\":\"1\",\"desc\":\"\"}";
    //                Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户："+userName+" 签到成功\r\n", null);
    //                return result;
    //            }
    //            else {
    //                result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"签到数据保存到数据库时失败\"}";
    //                Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + " 签到数据保存到数据库时失败\r\n", null);
    //                return result;
    //            }
                
    //        }
    //        else { 
    //            if(login.LoginTime.Date==DateTime.Now.Date)
    //            {
    //                result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"今天已经做过签到了，不能再签到！\"}";
    //                Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，今天已经做过签到了，不能再签到！\r\n", null);
    //                return result;
                
    //            }else{
    //                login = new APPLoginLogoutLog();
    //                login.LoginTime = DateTime.Now;
    //                login.OpenID = user.ID.ToString();
    //                login.LoginLocation = gps;
    //                login.Status = 1;
    //                login.LogoutTime = DateTime.Now.AddYears(-100);
    //                login.LogoutLocation = "";
    //                if (APPLoginLogoutLogBLL.Add(login) > 0)
    //                {
    //                    result = "{\"code\":\"SignIn\",\"result\":\"1\",\"desc\":\"\"}";
    //                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + " 签到成功\r\n", null);
    //                    return result;
    //                }
    //                else
    //                {
    //                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"签到数据保存到数据库时失败\"}";
    //                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + " 签到数据保存到数据库时失败\r\n", null);
    //                    return result;
    //                }
    //            }
            
    //        }
    //    }else{
    //        result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper值为"+oper+"，没有对应的值\"}";
    //        Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取oper值为" + oper + "，没有对应的值\r\n", null);
    //        return result;
    //    }
    //}
    #endregion

    protected string SignRecord(JObject obj)
    {
        string userName = string.Empty;
        string page = string.Empty;
        int pageIndex = 1;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SignRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignRecord接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"SignRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignRecord接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"SignRecord\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignRecord接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }

        try
        {
            page = obj["page"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SignRecord\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignRecord接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        page = page.Trim('"');
        if (!string.IsNullOrEmpty(page))
        {
            try
            {
                pageIndex = Convert.ToInt32(page);
            }
            catch (Exception ex)
            {
                result = "{\"code\":\"SignRecord\",\"result\":\"0\",\"desc\":\"参数page的值必须为整数\"}";
                Logger.GetLogger(this.GetType()).Info("APP SignRecord接口被调用，参数page的值必须为整数，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
                return result;
            }
        }
        string StrWhere = " f_openid= '" + user.ID + "' order by f_logintime desc";
        int Count=0;
        List<APPLoginLogoutLog> records = APPLoginLogoutLogBLL.GetList(10, pageIndex, StrWhere,out Count);

        if (records != null && records.Count > 0)
        {
            string record = string.Empty;
            for (int i = 0; i < records.Count; i++)
            {
                if (i + 1 == records.Count)
                {

                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    //paramDic.Add("time", records[i].LoginTime.ToString("yyyy-MM-dd"));
                    paramDic.Add("gps", records[i].LoginLocation);
                    //paramDic.Add("signInTime", records[i].LoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (records[i].LoginTime != null)
                    {
                        paramDic.Add("signInTime", records[i].LoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {

                        paramDic.Add("signInTime", "");
                    }
                    if (records[i].LogoutTime != null)
                    {
                        paramDic.Add("signOutTime", records[i].LogoutTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else {

                        paramDic.Add("signOutTime", "");
                    }

                    record = WebUtil.BuildQueryJson(paramDic);
                    if (record.EndsWith(","))
                    {
                        record = record.Remove(record.Length - 1);
                    }
                    record = "{" + record + "}";
                    result += record;
                }
                else
                {
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    //paramDic.Add("time", records[i].LoginTime.ToString("yyyy-MM-dd"));
                    paramDic.Add("gps", records[i].LoginLocation);
                    //paramDic.Add("signInTime", records[i].LoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (records[i].LoginTime != null)
                    {
                        paramDic.Add("signInTime", records[i].LoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {

                        paramDic.Add("signInTime", "");
                    }
                    if (records[i].LogoutTime != null)
                    {
                        paramDic.Add("signOutTime", records[i].LogoutTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {

                        paramDic.Add("signOutTime", "");
                    }
                    record = WebUtil.BuildQueryJson(paramDic);
                    if (record.EndsWith(","))
                    {
                        record = record.Remove(record.Length - 1);
                    }
                    record = "{" + record + "},";
                    result += record;
                }
            }
            result = "{\"code\":\"SignRecord\",\"list\":[" + result + "],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignRecord接口被调用，用户：" + userName + "签到记录：" + result + "\r\n", null);
        }
        else
        {
            result = "{\"code\":\"SignRecord\",\"list\":[],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignRecord接口被调用，用户：" + userName + "签到记录为空\r\n", null);
        }
        return result;
    }

    /// <summary>
    /// 分页查询资料库
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string Knowledge(JObject obj)
    {
        string userName = string.Empty;
        string page = string.Empty;
        int pageIndex = 1;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"Knowledge\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP Knowledge接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"Knowledge\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP Knowledge接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"Knowledge\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP Knowledge接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }

        try
        {
            page = obj["page"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"Knowledge\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP Knowledge接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        page = page.Trim('"');
        if (!string.IsNullOrEmpty(page))
        {
            try
            {
                pageIndex = Convert.ToInt32(page);
            }
            catch (Exception ex)
            {
                result = "{\"code\":\"Knowledge\",\"result\":\"0\",\"desc\":\"参数page的值必须为整数\"}";
                Logger.GetLogger(this.GetType()).Info("APP Knowledge接口被调用，参数page的值必须为整数，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
                return result;
            }
        }
        //string StrWhere = " f_openid= '" + user.ID + "' order by f_logintime desc";
        int Count = 0;
        BrandInfo brandInfo = BrandBLL.Get("YUM");
        string sqlWhere = " 1=1 ";
        if (brandInfo != null)
        {
            sqlWhere = " ID in( select f_KnowledgeID from sys_KnowkedgeBaseBrand where f_BrandID= " + brandInfo.ID + ") ";
        }
        List<KnowledgeBaseInfo> knowledge = KnowledgeBaseBLL.GetList(10, pageIndex, sqlWhere, out Count);
        if (knowledge != null && knowledge.Count > 0)
        {
            string record = string.Empty;
            for (int i = 0; i < knowledge.Count; i++)
            {
                if (i + 1 == knowledge.Count)
                {

                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    if (knowledge[i].Title.Contains("\\"))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\\","\\\\");
                    }
                    if (knowledge[i].Content.Contains("\\"))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\\", "\\\\");
                    }
                    if (knowledge[i].Title.Contains("\""))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\"", "'");
                    }
                    if (knowledge[i].Content.Contains("\""))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\"", "'");
                    }

                    if (knowledge[i].Title.Contains("\r"))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\r", "");
                    }
                    if (knowledge[i].Content.Contains("\r"))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\r", "");
                    }

                    if (knowledge[i].Title.Contains("\n"))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\n", "");
                    }
                    if (knowledge[i].Content.Contains("\n"))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\n", "");
                    }

                    if (knowledge[i].Title.Contains("\t"))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\t", "");
                    }
                    if (knowledge[i].Content.Contains("\t"))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\t", "");
                    }
                    paramDic.Add("question", knowledge[i].Title);
                    paramDic.Add("answer", knowledge[i].Content);

                    record = WebUtil.BuildQueryJson(paramDic);
                    if (record.EndsWith(","))
                    {
                        record = record.Remove(record.Length - 1);
                    }
                    record = "{" + record + "}";
                    result += record;
                }
                else
                {
                    KeyValueDictionary paramDic = new KeyValueDictionary();
                    if (knowledge[i].Title.Contains("\\"))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\\", "\\\\");
                    }
                    if (knowledge[i].Content.Contains("\\"))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\\", "\\\\");
                    }
                    if (knowledge[i].Title.Contains("\""))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\"", "'");
                    }
                    if (knowledge[i].Content.Contains("\""))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\"", "'");
                    }
                    if (knowledge[i].Title.Contains("\r"))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\r", "");
                    }
                    if (knowledge[i].Content.Contains("\r"))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\r", "");
                    }

                    if (knowledge[i].Title.Contains("\n"))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\n", "");
                    }
                    if (knowledge[i].Content.Contains("\n"))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\n", "");
                    }

                    if (knowledge[i].Title.Contains("\t"))
                    {
                        knowledge[i].Title = knowledge[i].Title.Replace("\t", "");
                    }
                    if (knowledge[i].Content.Contains("\t"))
                    {
                        knowledge[i].Content = knowledge[i].Content.Replace("\t", "");
                    }
                    paramDic.Add("question", knowledge[i].Title);
                    paramDic.Add("answer", knowledge[i].Content);
                    record = WebUtil.BuildQueryJson(paramDic);
                    if (record.EndsWith(","))
                    {
                        record = record.Remove(record.Length - 1);
                    }
                    record = "{" + record + "},";
                    result += record;
                }
            }
            result = "{\"code\":\"Knowledge\",\"list\":[" + result + "],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP Knowledge接口被调用，用户：" + userName + "查询资料库列表：" + result + "\r\n", null);
        }
        else
        {
            result = "{\"code\":\"Knowledge\",\"list\":[],\"desc\":\"\"}";
            Logger.GetLogger(this.GetType()).Info("APP Knowledge接口被调用，用户：" + userName + "查询资料库列表为空\r\n", null);
        }
        return result;
    }
    /// <summary>
    /// 今日签到记录
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string TodayClock(JObject obj)
    {
        string userName = string.Empty;
        
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"TodayClock \",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP TodayClock 接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"TodayClock \",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP TodayClock 接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"TodayClock \",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP TodayClock 接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }


        string StrWhere = " and f_openid= '" + user.ID + "' and (Convert(Varchar(10),f_logintime,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "' or Convert(Varchar(10),f_logouttime,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
        List<APPLoginLogoutLog> records = APPLoginLogoutLogBLL.GetList(StrWhere);
        if (records.Count > 0)
        {
            KeyValueDictionary paramDic = new KeyValueDictionary();
            //paramDic.Add("time", records[i].LoginTime.ToString("yyyy-MM-dd"));
            //paramDic.Add("gps", records[i].LoginLocation);
            if (records[0].LoginTime != null)
            {
                paramDic.Add("signInTime", records[0].LoginTime.Value.ToString("HH:mm"));
            }
            else
            {
                paramDic.Add("signInTime", "");
            }
            //paramDic.Add("signInTime", records[0].LoginTime.ToString("HH:mm:ss"));
            if (records[0].LogoutTime!=null)
            {
                paramDic.Add("signOutTime", records[0].LogoutTime.Value.ToString("HH:mm"));
            }
            else
            {
                paramDic.Add("signOutTime", "");
            }
            result = WebUtil.BuildQueryJson(paramDic);
            
            result = "{\"code\":\"TodayClock\"," + result + "\"desc\":\"\"}";
        }
        else {
            result = "{\"code\":\"TodayClock\",\"signInTime\":\"\",\"signOutTime\":\"\",\"desc\":\"\"}";       
        }
        return result;
    }


    /// <summary>
    /// 打卡签到、签退，使用存储过程
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string SPSignIn(JObject obj)
    {
        string userName = string.Empty;
        string oper = string.Empty;
        string gps = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }
        try
        {
            gps = obj["gps"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取gps值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取gps值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        gps = gps.Trim('"');
        //if (string.IsNullOrEmpty(gps))
        //{
        //    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取gps值为空\"}";
        //    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取gps值为空\r\n", null);
        //    return result;
        //}
        try
        {
            oper = obj["oper"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取oper值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        oper = oper.Trim('"');
        if (string.IsNullOrEmpty(oper))
        {
            result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取oper值为空\r\n", null);
            return result;
        }
        if (oper == "0")
        {
            result = APPLoginLogoutLogBLL.AddBySP(user.ID.ToString(), "0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), gps);
            switch (result)
            {
                case "0":
                    result = "{\"code\":\"SignIn\",\"result\":\"1\",\"desc\":\"\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + " 签到成功\r\n", null);
                    return result;
                case "5":
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"本次签退时间早于上一次签到记录时间，不能签退！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，本次签退时间早于上一次签到记录时间，不能签退！\r\n", null);
                    return result;
                case "6":
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"今天已经签退多了，不能重复签退！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，今天已经签退多了，不能重复签退！\r\n", null);
                    return result;
                case "7":
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"本次签退时间早于上一次签退记录时间，不能签退！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，本次签退时间早于上一次签退记录时间，不能签退！\r\n", null);
                    return result;
                default :
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"签退失败！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，签退失败！\r\n", null);
                    return result;
            }
                
        }
        else if (oper == "1")
        {
            result = APPLoginLogoutLogBLL.AddBySP(user.ID.ToString(), "1", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),gps);
            switch (result)
            {
                case "0":
                    result = "{\"code\":\"SignIn\",\"result\":\"1\",\"desc\":\"\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，用户：" + userName + " 签到成功\r\n", null);
                    return result;
                case "1":
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"今天已经签退过了，不能再签到！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，今天已经签退过了，不能再签到！\r\n", null);
                    return result;
                case "2":
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"本次签到时间早于上次签退记录时间，不能签到！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，本次签到时间早于上次签退记录时间，不能签到！\r\n", null);
                    return result;
                case "3":
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"今天已经签到过了，不能再签到！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，今天已经签到过了，不能再签到！\r\n", null);
                    return result;
                case "4":
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"本次签到时间早于上次签到记录时间，不能签到！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，本次签到时间早于上次签到记录时间，不能签到！\r\n", null);
                    return result;
                default:
                    result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"签到失败！\"}";
                    Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，签到失败！\r\n", null);
                    return result;
            }
        }
        else
        {
            result = "{\"code\":\"SignIn\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oper值为" + oper + "，没有对应的值\"}";
            Logger.GetLogger(this.GetType()).Info("APP SignIn接口被调用，解析JSON数据获取oper值为" + oper + "，没有对应的值\r\n", null);
            return result;
        }
    }
    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string ChangePWD(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        string oldPassword = string.Empty;
        string newPassword = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"ChangePWD\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"ChangePWD\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }

        try
        {
            oldPassword = obj["oldPassword"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"ChangePWD\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oldPassword值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，解析JSON数据获取oldPassword值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        oldPassword = oldPassword.Trim('"');
        if (string.IsNullOrEmpty(oldPassword))
        {
            result = "{\"code\":\"ChangePWD\",\"result\":\"0\",\"desc\":\"解析JSON数据获取oldPassword值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，解析JSON数据获取oldPassword值为空\r\n", null);
            return result;
        }

        try
        {
            newPassword = obj["newPassword"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"ChangePWD\",\"result\":\"0\",\"desc\":\"解析JSON数据获取newPassword值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，解析JSON数据获取newPassword值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        newPassword = newPassword.Trim('"');
        if (string.IsNullOrEmpty(newPassword))
        {
            result = "{\"code\":\"ChangePWD\",\"result\":\"0\",\"desc\":\"解析JSON数据获取newPassword值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，解析JSON数据获取newPassword值为空\r\n", null);
            return result;
        }

        result = UserBLL.ChangePWDBySP(userName, oldPassword.ToLower(), newPassword.ToLower());
        switch (result)
        {
            case "success":
                result = "{\"code\":\"ChangePWD\",\"result\":\"1\",\"desc\":\"\"}";
                Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，用户：" + userName + " 密码修改成功\r\n", null);
                break;
            case "not binding":
                result = "{\"code\":\"ChangePWD\",\"result\":\"0\",\"desc\":\"该用户不存在\"}";
                Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，用户：" + userName + " 不存在\r\n", null);
                break;
            case "wrong oldPassword":
                result = "{\"code\":\"ChangePWD\",\"result\":\"0\",\"desc\":\"原密码错误，密码修改失败\"}";
                Logger.GetLogger(this.GetType()).Info("APP ChangePWD接口被调用，用户：" + userName + "修改密码时，原密码错误！\r\n", null);
                break;
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
    /// <summary>
    /// 去除参数中的特殊符号包括\ " 回车 换行
    /// </summary>
    /// <param name="paramer"></param>
    /// <returns></returns>
    protected string DeleteSpecialChar(string paramer)
    {
        if (paramer.Contains("\\"))
        {
            paramer = paramer.Replace("\\", "\\\\");
        }
        if (paramer.Contains("\""))
        {
            paramer = paramer.Replace("\"", "'");
        }
        if (paramer.Contains("\r"))
        {
            paramer = paramer.Replace("\r", " ");
        }
        if (paramer.Contains("\n"))
        {
            paramer = paramer.Replace("\n", " ");
        }
        if (paramer.Contains("\t"))
        {
            paramer = paramer.Replace("\t", " ");
        }
        return paramer;
   
    }
    
}
