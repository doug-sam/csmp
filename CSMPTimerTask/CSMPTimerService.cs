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
            string strTime = "定时器开始" + DateTime.Now.ToString();
            StreamWriter swtime = new StreamWriter("D:\\1.txt", false);
            swtime.WriteLine(strTime);
            swtime.Close();//写入

            //执行SQL语句或其他操作

            string sqlListStr = "select * from sys_WebServiceTask where f_IsDone = 0;";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.SqlconnString, CommandType.Text, sqlListStr, null))
            {
                while (rdr.Read())
                {
                    int id = Convert.ToInt32(rdr["ID"]);
                    string paramStr = rdr["f_TaskUrl"].ToString();

                    string strTime1 = paramStr;
                    StreamWriter swtime1 = new StreamWriter("D:\\2.txt", false);
                    swtime1.WriteLine(strTime1);
                    swtime1.Close();//写入

                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
                    string content = HttpUtility.UrlEncode(paramStr, encode);
                    string url = "http://helpdesk.bkchina.cn/siweb/ws_hesheng.ashx?";
                    url = url + "para={" + content + "}";

                    string strTime2 = url;
                    StreamWriter swtime2 = new StreamWriter("D:\\3.txt", false);
                    swtime2.WriteLine(strTime2);
                    swtime2.Close();//写入

                    string targeturl = url.Trim().ToString();
                    HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                    hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                    hr.Method = "GET";
                    hr.Timeout = 30 * 60 * 1000;
                    WebResponse hs = hr.GetResponse();
                    Stream sr = hs.GetResponseStream();
                    StreamReader ser = new StreamReader(sr, System.Text.Encoding.UTF8);
                    string sendResult = ser.ReadToEnd();
                    JObject obj = JObject.Parse(sendResult);
                    string errNo = obj["errNo"].ToString();

                    string str3 = errNo;
                    StreamWriter sw3 = new StreamWriter("D:\\4.txt", false);
                    sw3.WriteLine(str3);
                    sw3.Close();//写入

                    if (errNo == "0")
                    {
                        string updateStr = "UPDATE sys_WebServiceTask SET f_IsDone = 1 WHERE ID=" + id;
                        SqlHelper.ExecuteNonQuery(CommandType.Text, updateStr, null);
                    }
                    string str = sendResult + DateTime.Now.ToString();
                    StreamWriter sw = new StreamWriter("D:\\5.txt", false);
                    sw.WriteLine(str);
                    sw.Close();//写入

                }
            }
            //string paramStr = "\"Action\":\"新建\",\"cNumber\":\"20150210325995\",\"Supplier\":\"MVS\",\"Agent\":\"Simon\",\"stCode\":\"16594\",\"stMgr\":\"王先生\",\"Time1\":\"2015-7-1 15:23:33\",\"Issue\":\"无法打开电脑\",\"Priority\":\"P2\",\"Category1\":\"经理室硬件\",\"Category2\":\"PC机\",\"Category3\":\"显示屏故障\",\"Solution\":\"\",\"Attachment\":\"\"";


        }
    }
}
