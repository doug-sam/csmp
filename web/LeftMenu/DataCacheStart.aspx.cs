using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tool;
using CSMP.Model;
using CSMP.BLL;
using System.Threading;


public partial class LeftMenu_DataCacheStart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void StartThread_Click(object sender, EventArgs e)
    {
        Logger.GetLogger(this.GetType()).Info("CSMP左侧菜单数据缓存读写任务启动任务按键被点击\r\n", null);
        ThreadPool.QueueUserWorkItem(new WaitCallback(DoTimerTask));
    }
    protected void WriteLog_Click(object sender, EventArgs e)
    {
        Logger.GetLogger(this.GetType()).Info("CSMP左侧菜单数据缓存读写任务写日志按键被点击\r\n", null);
    }
    private void DoTimerTask(Object stateInfo)
    {
        while (true)
        {
            Logger.GetLogger(this.GetType()).Info("CSMP左侧菜单数据缓存读写任务执行循环1次\r\n", null);
            try
            {
                if (DateTime.Now.ToString("HH:mm") == "02:01")
                {
                    Logger.GetLogger(this.GetType()).Info("CSMP左侧菜单数据缓存读写任务凌晨2点执行！！！\r\n", null);
                    Thread.Sleep(5 * 60 * 1000);
                }
                
            }
            finally {
                Thread.Sleep(1 *60 * 1000);
            }
        
        }
    }
}
