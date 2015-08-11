using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using DBUtility;
using Newtonsoft.Json.Linq;
using Tool;

namespace CSMPTimerTask
{
    partial class CSMPTimerService : ServiceBase
    {
        System.Timers.Timer taskTimer;  //计时器
        public CSMPTimerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            taskTimer = new System.Timers.Timer();
            taskTimer.Interval = 3*60*1000;  //设置计时器事件间隔执行时间,毫秒数1s=1000ms
            taskTimer.Elapsed += new System.Timers.ElapsedEventHandler(taskTimer_Elapsed);
            taskTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            this.taskTimer.Enabled = false;
        }

        private void taskTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            //执行SQL语句或其他操作
            Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务开始。", null);
            string sqlListStr = "select * from sys_WebServiceTask where f_IsDone = 0 order by id asc;";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, sqlListStr, null))
            {
                
                while (rdr.Read())
                {
                    int id = Convert.ToInt32(rdr["ID"]);
                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务,任务ID：" + id, null);
                    string paramStr = rdr["f_TaskUrl"].ToString();

                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
                    string content = HttpUtility.UrlEncode(paramStr, encode);
                    string url = "http://helpdesk.bkchina.cn/siweb/ws_hesheng.ashx?";
                    url = url + "para={" + content + "}";

                    string targeturl = url.Trim().ToString();
                    HttpWebRequest hr = null;
                    try {
                        //Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，httpcreate开始", null);
                        hr = (HttpWebRequest)WebRequest.Create(targeturl);
                        //Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，httpcreate成功", null);
                    }catch(Exception ex){
                        Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：" + ex.Message, null);
                        continue;
                    }
                    
                    hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                    hr.Method = "GET";
                    hr.Timeout = 30 * 60 * 1000;
                    StreamReader ser =null;
                    //Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：开始stream", null);
                    try
                    {
                        WebResponse hs = hr.GetResponse();
                        Stream sr = hs.GetResponseStream();
                        ser = new StreamReader(sr, System.Text.Encoding.UTF8);
                    }
                    catch (Exception ex) {
                        Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id+"，错误原因："+ex.Message, null);
                        continue;
                    }
                    
                    string sendResult = ser.ReadToEnd();
                    Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务,任务ID：" + id + "，接口返回结果：" + sendResult, null);

                    JObject obj = null;
                    try
                    {
                        obj = JObject.Parse(sendResult);
                    }
                    catch (Exception ex)
                    {
                        Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行失败，任务ID=" + id + "，错误原因：" + ex.Message, null);
                        continue;
                    }
                    string errNo = obj["errNo"].ToString();

                    if (errNo == "0")
                    {
                        
                        string updateStr = "UPDATE sys_WebServiceTask SET f_IsDone = 1 WHERE ID=" + id;
                        try {
                            SqlHelper.ExecuteNonQuery(CommandType.Text, updateStr, null);
                            Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行成功，任务ID=" + id, null);
                        }catch(Exception ex){
                            Logger.GetLogger(this.GetType()).Info("Windows定时调用接口任务执行成功，任务ID=" + id + "，但更新本地数据库任务状态失败，错误原因：" + ex.Message, null);
                        }
                        
                    }

                }
            }
            //string paramStr = "\"Action\":\"新建\",\"cNumber\":\"20150210325995\",\"Supplier\":\"MVS\",\"Agent\":\"Simon\",\"stCode\":\"16594\",\"stMgr\":\"王先生\",\"Time1\":\"2015-7-1 15:23:33\",\"Issue\":\"无法打开电脑\",\"Priority\":\"P2\",\"Category1\":\"经理室硬件\",\"Category2\":\"PC机\",\"Category3\":\"显示屏故障\",\"Solution\":\"\",\"Attachment\":\"\"";


        }
    }
}
