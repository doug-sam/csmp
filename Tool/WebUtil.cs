using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace Tool
{
    public class WebUtil
    {
        private int _timeout = 100000;

        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        //public String DoPost(String url, IDictionary<String, String> parameters)
        //{
        //    HttpWebRequest req = GetWebRequest(url, "POST");
        //    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

        //    Byte[] postData = Encoding.UTF8.GetBytes(BuildQueryJson(parameters));
        //    Stream reqStream = req.GetRequestStream();
        //    reqStream.Write(postData, 0, postData.Length);
        //    reqStream.Close();

        //    HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        //    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
        //    return GetResponseAsString(rsp, encoding);
        //}

        //public String DoPost(String url, IDictionary<String, String> parameters)
        //{
        //    HttpWebRequest req = GetWebRequest(url, "POST");
        //    req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";

        //    Byte[] postData = Encoding.GetEncoding("GB2312").GetBytes(BuildQueryJson(parameters));
        //    Stream reqStream = req.GetRequestStream();
        //    reqStream.Write(postData, 0, postData.Length);
        //    reqStream.Close();

        //    HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        //    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
        //    return GetResponseAsString(rsp, encoding);
        //}


        public String DoPost(String url, IDictionary<String, String> parameters)
        {
            //string url = "http://helpdesk.bkchina.cn/siweb/ws_hesheng.ashx?";
            //string paramStr = "\"Action\":\"新建\",\"cNumber\":\"20150210325989\",\"Supplier\":\"MVS\",\"Agent\":\"Simon\",\"stCode\":\"16594\",\"stMgr\":\"王先生\",\"Time1\":\"2015-7-1 15:23:33\",\"Issue\":\"无法打开电脑\",\"Priority\":\"P2\",\"Category1\":\"经理室硬件\",\"Category2\":\"PC机\",\"Category3\":\"显示屏故障\",\"Solution\":\"\",\"Attachment\":\"\"";
            string paramStr = BuildQueryJson(parameters);

            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
            string content = HttpUtility.UrlEncode(paramStr, encode);
            url = url + "para={" + content + "}";
            string targeturl = url.Trim().ToString();
            HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
            hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
            hr.Method = "GET";
            hr.Timeout = 30 * 60 * 1000;
            WebResponse hs = hr.GetResponse();
            Stream sr = hs.GetResponseStream();
            StreamReader ser = new StreamReader(sr, System.Text.Encoding.UTF8);
            string sendResult = ser.ReadToEnd();
            return sendResult;

        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public String DoGet(String url, IDictionary<String, String> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }

            HttpWebRequest req = GetWebRequest(url, "GET");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        public String DoGet(String url, String refUrl, CookieContainer cookies, IDictionary<String, String> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }

            HttpWebRequest req = GetWebRequest(url, "GET");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }



        public HttpWebRequest GetWebRequest(String url, String method)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "CSMP";
            req.Timeout = this._timeout;
            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static String GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// 组装GET请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>带参数的GET请求URL</returns>
        public String BuildGetUrl(String url, IDictionary<String, String> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }
            return url;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static String BuildQuery(IDictionary<String, String> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<String, String>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                String name = dem.Current.Key;
                String value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(HttpUtility.UrlEncode(value));
                    hasParam = true;
                }
            }
            //Logger.GetLogger("MyLogger").Info(HttpUtility.UrlDecode(postData.ToString()));
            return postData.ToString();
        }

        /// <summary>
        /// 组装普通文本请求参数（json格式)。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static String BuildQueryJson(IDictionary<String, String> parameters)
        {
            StringBuilder postData = new StringBuilder();
            //postData.Append("para={");
            IEnumerator<KeyValuePair<String, String>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                String name = dem.Current.Key;
                String value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!String.IsNullOrEmpty(name))
                {
                    postData.Append("\""+name+"\"");
                    postData.Append(":");
                    //postData.Append("\"" + HttpUtility.UrlEncode(value) + "\"");
                    postData.Append("\"" + value + "\"");
                    
                    postData.Append(",");

                }
            }
            //postData.Append("}");
            //Logger.GetLogger("MyLogger").Info(HttpUtility.UrlDecode(postData.ToString()));
            return postData.ToString();
        }
    }
}
