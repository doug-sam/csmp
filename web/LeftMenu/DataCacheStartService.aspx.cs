using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;


public partial class LeftMenu_DataCacheStartService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Logger.GetLogger(this.GetType()).Info("左侧菜单写缓存接口被调用！\r\n", null);
        string param = Request["param"];
        if (param == "getCache")
        {
            try
            {
                List<UserInfo> userList = UserBLL.GetList("");
                List<LeftMenuData> dataList = new List<LeftMenuData>();
                Logger.GetLogger(this.GetType()).Info("左侧菜单写缓存接口被调用！开始循环计算用户各数据，用户数：" + userList .Count+ "\r\n", null);
                //循环所有用户count LeftMenu数据 并写入数据库
                foreach (UserInfo currentUser in userList)
                {
                    LeftMenuData data = new LeftMenuData();
                    data.UserID = currentUser.ID;
                    data.HaveGroupPower = GroupBLL.PowerCheckForLeftMenuData((int)PowerInfo.P1_Call.查看组内所有报修, currentUser);
                    data.WorkGroupID = currentUser.WorkGroupID;
                    //等待安排上门
                    data.ToBeOnSite = CallBLL.GetCountSln1(currentUser.WorkGroupID);
                    //未处理
                    data.ToBeDisposed = CSMP.BLL.CallBLL.GetCountForLeftMenuData(data.HaveGroupPower, (int)SysEnum.CallStateMain.未处理, currentUser);
                    //处理中
                    data.Disposing = CSMP.BLL.CallBLL.GetCountForLeftMenuData(data.HaveGroupPower,(int)SysEnum.CallStateMain.处理中, currentUser);
                    //已完成
                    data.Complete = CSMP.BLL.CallBLL.GetCountForLeftMenuData(data.HaveGroupPower, (int)SysEnum.CallStateMain.已完成, currentUser);
                    //已关闭
                    data.Closed = CSMP.BLL.CallBLL.GetCountForLeftMenuData(data.HaveGroupPower, (int)SysEnum.CallStateMain.已关闭, currentUser);
                    //店铺催促
                    List<CallInfo> Tracelist = LeftMenuDataBLL.GetListTraceByCurrentUser(data.HaveGroupPower, currentUser);
                    if (null == Tracelist || Tracelist.Count == 0)
                    {
                        data.StoreUrgency = 0;
                    }
                    else
                    {
                        data.StoreUrgency = Tracelist.Count;
                    }
                    data.StoreRequest = CSMP.BLL.CustomerRequestBLL.GetConut(currentUser.WorkGroupID);
                    //data.HaveGroupPower = false;
                    
                    if (LeftMenuDataBLL.Get(currentUser.ID) == null)
                    {
                        LeftMenuDataBLL.Add(data);
                    }
                    else
                    {
                        LeftMenuDataBLL.Edit(data);
                    }
                }
                Logger.GetLogger(this.GetType()).Info("左侧菜单写缓存接口被调用！循环计算用户各数据插入数据库完成\r\n", null);
                dataList = LeftMenuDataBLL.GetListBySP();
                CacheManage.InsertCache("leftMenuKey", dataList);
                Logger.GetLogger(this.GetType()).Info("左侧菜单写缓存接口被调用！循环计算用户各数据读数据库后写入缓存完成\r\n", null);
         
            }
            catch (Exception ex)
            {
                Logger.GetLogger(this.GetType()).Info("左侧菜单写缓存接口被调用！循环计算用户各数据出错，错误原因：" + ex.Message + "\r\n", null);
            }
            LeftMenuDataBLL.InsertLeftMenuDataCache();
            Context.Response.ContentType = "application/json";
            Context.Response.Write("OK");
            
        }

    }
}
