using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;

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
        List<LeftMenuData> list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
        UserInfo currentUser = UserBLL.Get(845);
        int index = list.FindIndex(s => s.UserID == currentUser.ID);
        list[index].Closed += 1;
        CacheManage.InsertCache("leftMenuKey", list);
        LeftMenuData currentData = LeftMenuDataBLL.GetLeftMenuCacheByName(currentUser.ID);
        tboxResult.Text = currentData.Closed.ToString();

    }
    protected void BtnCacheMinus_Click(object sender, EventArgs e)
    {
        List<LeftMenuData> list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
        UserInfo currentUser = UserBLL.Get(845);
        int index = list.FindIndex(s => s.UserID == currentUser.ID);
        list[index].Closed -= 1;
        CacheManage.InsertCache("leftMenuKey", list);
        LeftMenuData currentData = LeftMenuDataBLL.GetLeftMenuCacheByName(currentUser.ID);
        tboxResult.Text = currentData.Closed.ToString();
    }
    protected void BtnShowCache_Click(object sender, EventArgs e)
    {
        List<LeftMenuData> list = CacheManage.GetSearch("leftMenuKey") as List<LeftMenuData>;
        LeftMenuData currentData = new LeftMenuData();
        foreach (LeftMenuData item in list)
        {
            if (item.UserID == 845)
                currentData = item;
        
        }
        tboxResult.Text = currentData.Closed.ToString();
    }
    protected void postCachePage_Click(object sender, EventArgs e)
    {

        try
        {

            string urlStr = "http://124.74.9.202:820/LeftMenu/DataCacheStartService.aspx";
            string param = "ak=83aa33656a8da19e057bcbde00272e04" ;
            WebUtil.DoPost(urlStr, param, 1);

            Logger.GetLogger(this.GetType()).Info("CSMP左侧菜单数据缓存读写任务凌晨2点执行完成\r\n", null);
        }
        catch (Exception ex)
        {
            Logger.GetLogger(this.GetType()).Info("CSMP左侧菜单数据缓存读写任务凌晨2点执行错误" + ex.Message + "\r\n", null);
        }

    }
}
