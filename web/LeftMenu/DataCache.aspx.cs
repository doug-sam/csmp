using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using System.Configuration;

public partial class LeftMenu_DataCache : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnGetFromData_Click(object sender, EventArgs e)
    {
        
        LeftMenuDataBLL.InsertLeftMenuDataCache();
        UserInfo currentUser = UserBLL.Get(845);


        LeftMenuData data = LeftMenuDataBLL.GetLeftMenuCacheByName(currentUser.ID);
        tboxResult.Text = data.Closed.ToString();

    }


    protected void BtnCacheAdd_Click(object sender, EventArgs e)
    {
        string userName = tboxResult.Text.Trim();
        if (string.IsNullOrEmpty(userName))
        {
            Label1.Text = "请输入用户名称";
            return;
        }
        UserInfo currentUser = UserBLL.Get(userName);
        if (currentUser == null)
        {
            Label1.Text = "无此用户";
            return;

        }
        List<LeftMenuData> list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
        int index = list.FindIndex(s => s.UserID == currentUser.ID);
        list[index].Closed += 1;
        CacheManage.InsertCache("leftMenuKey", list);
        LeftMenuData currentData = LeftMenuDataBLL.GetLeftMenuCacheByName(currentUser.ID);
        Label1.Text = " 已关闭数：" + currentData.Closed;

    }
    protected void BtnCacheMinus_Click(object sender, EventArgs e)
    {
        string userName = tboxResult.Text.Trim();
        if (string.IsNullOrEmpty(userName))
        {
            Label1.Text = "请输入用户名称";
            return;
        }
        UserInfo currentUser = UserBLL.Get(userName);
        if (currentUser == null)
        {
            Label1.Text = "无此用户";
            return;

        }
        List<LeftMenuData> list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
        int index = list.FindIndex(s => s.UserID == currentUser.ID);
        list[index].Closed -= 1;
        CacheManage.InsertCache("leftMenuKey", list);
        LeftMenuData currentData = LeftMenuDataBLL.GetLeftMenuCacheByName(currentUser.ID);
        Label1.Text = " 已关闭数：" + currentData.Closed;
    }
    protected void BtnShowCache_Click(object sender, EventArgs e)
    {
        string userName = tboxResult.Text.Trim();
        if (string.IsNullOrEmpty(userName))
        {
            Label1.Text = "请输入用户名称";
            return;
        }
        UserInfo userInfo = UserBLL.Get(userName);
        if (userInfo == null)
        {
            Label1.Text = "无此用户";
            return;
            
        }
        List<LeftMenuData> list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
        if (list == null||list.Count<=0)
        {
            Label1.Text = "缓存数据不存在";
            return;

        }
        LeftMenuData currentData = new LeftMenuData();
        foreach (LeftMenuData item in list)
        {
            if (item.UserID == userInfo.ID)
                currentData = item;
        
        }
        if (currentData == null)
        {
            Label1.Text = "缓存中无此用户的数据";
            return;

        }
        Label1.Text = "已完成数：" + currentData .Complete+" 已关闭数："+ currentData.Closed;
    }
    protected void postCachePage_Click(object sender, EventArgs e)
    {

        try
        {
            //string AbsoluteUri = Request.Url.AbsoluteUri;
            //Logger.GetLogger(this.GetType()).Info("左侧菜单测试页面：获取当前网页完整地址" + AbsoluteUri + "\r\n", null);
            //string urlAbsolutePath = Request.Url.AbsolutePath;
            //Logger.GetLogger(this.GetType()).Info("左侧菜单测试页面：获取当前网页绝对地址为" + urlAbsolutePath + "\r\n", null);
            //string urlHost = AbsoluteUri.Remove(AbsoluteUri.IndexOf(urlAbsolutePath));
            //Logger.GetLogger(this.GetType()).Info("左侧菜单测试页面：获取网站地址为" + urlHost + "\r\n", null);
            //string urlStr = urlHost+"/LeftMenu/DataCacheStartService.aspx";
            string urlStr = ConfigurationManager.AppSettings["WebServerUrl"].ToString() + "/LeftMenu/DataCacheStartService.aspx";
            Logger.GetLogger(this.GetType()).Info("左侧菜单测试页面：写缓存的网页地址为" + urlStr + "\r\n", null);
            string param = "param=getCache" ;
            WebUtil.DoPost(urlStr, param, 1);

            Label1.Text = "CSMP左侧菜单数据缓存读写任务凌晨2点执行完成";

        }
        catch (Exception ex)
        {
            Label1.Text = "CSMP左侧菜单数据缓存读写任务凌晨2点执行错误" + ex.Message;
        }

    }
}
