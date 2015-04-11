<%@ Application Language="C#" %>
<%@ Import Namespace="CSMP.BLL" %>
<%@ Import Namespace="CSMP.Model" %>
<%@ Import Namespace="Tool" %>
<%@ Import Namespace="System.Collections.Generic" %>
<script RunAt="server">

    private static int CheckEmailInterval =12000;
    void Application_Start(object sender, EventArgs e)
    {
        //在应用程序启动时运行的代码
        if (CheckEmailInterval <= 0)
        {
            return;
        }
        System.Timers.Timer tr1 = new System.Timers.Timer(CheckEmailInterval);
        tr1.AutoReset = true;
        tr1.Enabled = true;
        tr1.Start();
        tr1.Elapsed += new System.Timers.ElapsedEventHandler(tr1_Elapsed);

    }

    public static int MyCount = 1;
    void tr1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (CheckEmailInterval <= 0) return;
        List<TaskInfo> list = TaskBLL.GetList();
        foreach (TaskInfo item in list)
        {
            if (!item.Enable||string.IsNullOrEmpty(item.URL))
            {
                continue;
            }
            bool NeedExcute = NeedExcuteCheck(item);

            if (NeedExcute)
            {
                string RequestURL = item.URL;
                if (!RequestURL.ToLower().Contains("http://")&&!RequestURL.ToLower().Contains("https://"))
                {
                    if (!RequestURL.StartsWith("/"))
                    {
                        RequestURL = "/" + RequestURL;
                    }
                    RequestURL = "http://localhost" + RequestURL;
                }
                
                LogInfo info = new LogInfo();
                info.Category = SysEnum.LogType.系统任务执行监视.ToString();
                info.Content = "执行结果：<br/>" + DataHelper.GetHttpData(RequestURL,60000);
                LogBLL.Add(info);
                
                item.ExcuteTimeLast = DateTime.Now;
                TaskBLL.Edit(item);
            }
        }
    }

    private static bool NeedExcuteCheck(TaskInfo item)
    {
        bool NeedExcute = false;
        TaskInfo.CycleModeInfo cyclemode = (TaskInfo.CycleModeInfo)Enum.Parse(typeof(TaskInfo.CycleModeInfo), item.CycleMode);
        switch (cyclemode)
        {
            case TaskInfo.CycleModeInfo.Hour://hour超过interval则执行
                NeedExcute = (DateTime.Now - item.ExcuteTimeLast).TotalHours >= (double)item.IntervalTime;
                break;
            case TaskInfo.CycleModeInfo.Day://day超过interval,并到相同的hour
                NeedExcute = (DateTime.Now.Date - item.ExcuteTimeLast.Date).TotalDays >= (double)item.IntervalTime && DateTime.Now.Hour >= item.ExcuteTimeLast.Hour;
                break;
            case TaskInfo.CycleModeInfo.Week://week超过interval，相同的dayOfWeek，相同的hour
                NeedExcute = (DateTime.Now.Date - item.ExcuteTimeLast.Date).TotalDays * 7 >= (double)item.IntervalTime && DateTime.Now.DayOfWeek == item.ExcuteTimeLast.DayOfWeek && DateTime.Now.Hour >= item.ExcuteTimeLast.Hour;
                break;
            case TaskInfo.CycleModeInfo.Month://month超interval,相同的day,相同的hour
                int MonthInterVal = (DateTime.Now.Year - item.ExcuteTimeLast.Year) * 12 + (DateTime.Now.Month - item.ExcuteTimeLast.Month);
                NeedExcute = MonthInterVal >= item.IntervalTime && DateTime.Now.Day == item.ExcuteTimeLast.Day && DateTime.Now.Hour >= item.ExcuteTimeLast.Hour;
                break;
            default:
                break;
        }
        return NeedExcute;
    }





    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码
        Exception objErr = Server.GetLastError().GetBaseException();
        LogInfo Log = new LogInfo();
        Log.AddDate = DateTime.Now;
        Log.ErrorDate = DateTime.Now;
        Log.Category = Enum.GetName(typeof(SysEnum.LogType), SysEnum.LogType.系统出错) ;
        Log.Content = "发生异常页:{0}<br/>      异常信息:{1}<br/>       错误源:{2}<br/>        堆栈信息:{3}<br/>        引错用户{4}<br/>        引错用户IP{5}<br/>";
        Log.Content = string.Format(Log.Content, Request.Url.ToString(), objErr.Message, objErr.Source, objErr.StackTrace, UserBLL.GetCurrentEmployeeName(), Request.UserHostAddress);
        Log.SendEmail = true;
        Log.Serious = 1;
        Log.UserName = UserBLL.GetCurrentEmployeeName();
        
        
        try
        {
            LogBLL.Add(Log);
        }
        finally
        {
           // Server.Transfer("~/system/error.html");

        }

    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

        
    }
       
</script>

