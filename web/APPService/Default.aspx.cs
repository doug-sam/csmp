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
                case "PersionalInfo":
                    result = PersionalInfo(obj);
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
            Logger.GetLogger(this.GetType()).Info("BK调用汉堡王接口，错误原因为" + result+"\r\n", null);
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
    protected string PersionalInfo(JObject obj)
    {
        string userName = string.Empty;
        string result = string.Empty;
        try
        {
            userName = obj["userName"].ToString();
        }
        catch (Exception ex)
        {
            result = "{\"code\":\"PersionalInfo\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值时错误\"}";
            Logger.GetLogger(this.GetType()).Info("APP PersonalInfo接口被调用，解析JSON数据获取userName值错误，错误原因为" + result + "异常：" + ex.Message + "\r\n", null);
            return result;
        }
        userName = userName.Trim('"');
        if (string.IsNullOrEmpty(userName))
        {
            result = "{\"code\":\"PersionalInfo\",\"result\":\"0\",\"desc\":\"解析JSON数据获取userName值为空\"}";
            Logger.GetLogger(this.GetType()).Info("APP PersonalInfo接口被调用，解析JSON数据获取userName值为空\r\n", null);
            return result;
        }
        UserInfo user = UserBLL.Get(userName);
        if (user == null)
        {
            result = "{\"code\":\"PersionalInfo\",\"result\":\"0\",\"desc\":\"用户：" + userName + "不存在\"}";
            Logger.GetLogger(this.GetType()).Info("APP PersonalInfo接口被调用，用户：" + userName + "不存在\r\n", null);
            return result;
        }
        WorkGroupInfo workGroup = WorkGroupBLL.Get(user.WorkGroupID);
        KeyValueDictionary paramDic = new KeyValueDictionary();
        paramDic.Add("code", "PersionalInfo");
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
