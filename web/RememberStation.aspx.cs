using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RememberStation : System.Web.UI.Page
{
    public string Info = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string station = Request.QueryString["station"];
        string agent = Request.QueryString["agent"];
        if (string.IsNullOrEmpty(station) || string.IsNullOrEmpty(agent))
        {
            Info = "未获取到分机或工号信息，请重新登陆！";
            return;
        }
        //写cookie
        UserState.WriteCookie("Station", station);
        //读取cookie
        string Station = UserState.GetCookie("Station");
        if (station == Station || station.Equals(Station))
        {
            Info += "<h3>登陆成功！</h3>";
            Info += "<h3>分机：" + Station+"</h3>";
            Info += "<h3>工号：" + agent + "</h3>"; 
        }
        else {
            Info = "请设置浏览器允许Cookie设置本地数据";
        }


    }
}

public class UserState
{
    /// <summary>
    /// 写cookie值
    /// </summary>
    /// <param name="strName">名称</param>
    /// <param name="strValue">值</param>
    public static void WriteCookie(string strName, string strValue)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
        if (cookie == null)
        {
            cookie = new HttpCookie(strName);
        }
        cookie.Value = strValue;
        HttpContext.Current.Response.AppendCookie(cookie);
    }
    /// <summary>
    /// 写cookie值
    /// </summary>
    /// <param name="strName">名称</param>
    /// <param name="strValue">值</param>
    /// <param name="strValue">过期时间(分钟)</param>
    public static void WriteCookie(string strName, string strValue, int expires)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
        if (cookie == null)
        {
            cookie = new HttpCookie(strName);
        }
        cookie.Value = strValue;
        cookie.Expires = DateTime.Now.AddMinutes(expires);
        HttpContext.Current.Response.AppendCookie(cookie);
    }
    /// <summary>
    /// 读cookie值
    /// </summary>
    /// <param name="strName">名称</param>
    /// <returns>cookie值</returns>
    public static string GetCookie(string strName)
    {
        if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
        {
            return HttpContext.Current.Request.Cookies[strName].Value.ToString();
        }
        return "";
    }
}