using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Security;
using System.Xml;
using CSMP.Model;
using Tool;

namespace CSMP.BLL
{
    public class WeChatHelper
    {
        /// <summary>
        /// 获取access token
        /// </summary>
        /// <returns>成功时返回access_token，失败时则返回错误码，可通过判断其返回字符串长度来断定是错误码还是access token</returns>
        public static string GetAccessTonken()
        {
            string URL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=((APPID))&secret=((APPSECRET))";
            URL = URL.Replace("((APPID))", ConfigHelper.GetAppendSettingValue(ConfigHelper.WeChatAppId));
            URL = URL.Replace("((APPSECRET))", ConfigHelper.GetAppendSettingValue(ConfigHelper.WeChatSecret));
            string Result = DataHelper.GetHttpData(URL);
            Dictionary<string, object> dic = JSONHelper.DataRowFromJSON(Result);
            if (dic.Keys.Contains("errcode"))
            {
                return dic["errcode"].ToString();
            }
            return dic["access_token"].ToString();
        }

        public static class ConstKey
        {
            public static readonly string 订阅 = "subscribe";
            public static readonly string 取消订阅 = "unsubscribe";
            public static readonly string 没有合适返回消息 = "keyNoKeyResponse";
        }



        /// <summary>
        /// 微信公众平台操作类
        /// </summary>
        public class WeChat
        {
            private string Token = ConfigHelper.GetAppendSettingValue(ConfigHelper.WeChatToken);

            /// <summary>
            /// 来源Oauth验证
            /// </summary>
            public void OauthCheck()
            {
                string echoStr = System.Web.HttpContext.Current.Request.QueryString["echoStr"];
                if (CheckSignature()) //校验签名是否正确
                {
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        System.Web.HttpContext.Current.Response.Write(echoStr); //返回原值表示校验成功
                        System.Web.HttpContext.Current.Response.End();
                    }
                }
            }


            public string Handle(string postStr)
            {

                //封装请求类
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(postStr);
                XmlElement rootElement = doc.DocumentElement;
                //MsgType
                XmlNode MsgType = rootElement.SelectSingleNode("MsgType");
                //接收的值--->接收消息类(也称为消息推送)
                WeChatXMLInfo requestXML = new WeChatXMLInfo();
                requestXML.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                requestXML.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                requestXML.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                requestXML.MsgType = MsgType.InnerText;

                //LogInfo info = new LogInfo();
                //info.AddDate = DateTime.Now;
                //info.Category = "from";
                //info.Content = postStr;
                //info.ErrorDate = DateTime.Now;
                //info.SendEmail = false;
                //info.Serious = 1;
                //info.UserName = "";
                //LogBLL.Add(info);




                //根据不同的类型进行不同的处理
                switch (requestXML.MsgType)
                {
                    case "text": //文本消息
                        requestXML.Content = rootElement.SelectSingleNode("Content").InnerText;
                        break;
                    case "image": //图片
                        requestXML.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                        break;
                    case "location": //位置
                        requestXML.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
                        requestXML.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
                        requestXML.Scale = rootElement.SelectSingleNode("Scale").InnerText;
                        requestXML.Label = rootElement.SelectSingleNode("Label").InnerText;
                        break;
                    case "link": //链接
                        break;
                    case "event": //事件推送 支持V4.5+
                        requestXML.Event = rootElement.SelectSingleNode("Event").InnerText;
                        requestXML.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                        break;
                }


                //消息回复
                return ResponseMsg(requestXML);
            }


            /// <summary>
            /// 验证微信签名
            /// * 将token、timestamp、nonce三个参数进行字典序排序
            /// * 将三个参数字符串拼接成一个字符串进行sha1加密
            /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
            /// </summary>
            /// <returns></returns>
            private bool CheckSignature()
            {
                string signature = System.Web.HttpContext.Current.Request.QueryString["signature"];
                string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];
                string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];
                //加密/校验流程：
                //1. 将token、timestamp、nonce三个参数进行字典序排序
                string[] ArrTmp = { Token, timestamp, nonce };
                Array.Sort(ArrTmp);//字典排序
                //2.将三个参数字符串拼接成一个字符串进行sha1加密
                string tmpStr = string.Join("", ArrTmp);
                tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
                tmpStr = tmpStr.ToLower();
                //3.开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
                if (tmpStr == signature)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 消息回复(微信信息返回)
            /// </summary>
            /// <param name="requestXML">The request XML.</param>
            private string ResponseMsg(WeChatXMLInfo requestXML)
            {
                try
                {
                    string resxml = "";
                    //主要是调用数据库进行关键词匹配自动回复内容，可以根据自己的业务情况编写。
                    //1.通常有，没有匹配任何指令时，返回帮助信息
                    // AutoResponse mi = new AutoResponse(requestXML.Content, requestXML.FromUserName);

                    switch (requestXML.MsgType)
                    {
                        case "text":
                            resxml = MsgText(requestXML);
                            break;
                        case "location":
                            resxml = MsgLocation(requestXML);
                            break;
                        case "image":
                            //图文混合的消息 具体格式请见官方API“回复图文消息” 
                            break;
                        case "event":
                            resxml = MsgEvent(requestXML);
                            break;
                    }

                    //LogInfo info = new LogInfo();
                    //info.AddDate = DateTime.Now;
                    //info.Category = "info";
                    //info.Content = resxml;
                    //info.ErrorDate = DateTime.Now;
                    //info.SendEmail = false;
                    //info.Serious = 1;
                    //info.UserName = "";
                    //LogBLL.Add(info);

                    return resxml;
                }
                catch (Exception ex)
                {
                    LogInfo info = new LogInfo();
                    info.AddDate = DateTime.Now;
                    info.Category = "error";
                    info.Content = ex.Message;
                    info.ErrorDate = DateTime.Now;
                    info.SendEmail = false;
                    info.Serious = 1;
                    info.UserName = "";
                    LogBLL.Add(info);

                    return "<xml><ToUserName><![CDATA[o_cUAuOAjqSlWrE_PIJDQeH08hKY]]></ToUserName><FromUserName><![CDATA[gh_39c476792a86]]></FromUserName><CreateTime>1383555573</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + ex.Message + "]]></Content></xml>";
                }
            }

            #region 具体不同类型的消息回复

            /// <summary>
            /// 当接收到事件时回复
            /// </summary>
            /// <param name="requestXML"></param>
            /// <returns></returns>
            private string MsgEvent(WeChatXMLInfo requestXML)
            {
                string resxml = string.Empty;
                List<WeixinNewsInfo> list = new List<WeixinNewsInfo>();
                switch (requestXML.Event)
                {
                    case "subscribe":   //订阅
                        list = WeixinNewsBLL.Get(ConstKey.订阅, true);
                        resxml=Responser(requestXML, list);
                        break;
                    case "unsubscribe":   //取消订阅
                        break;
                    case "CLICK":
                        list = WeixinNewsBLL.Get(requestXML.EventKey, true);
                       resxml= Responser(requestXML, list);
                        break;
                    default:
                        break;
                }

                return resxml;
            }
            private string MsgText(WeChatXMLInfo requestXML)
            {
                string resxml = string.Empty;
                List<WeixinNewsInfo> list = WeixinNewsBLL.Get(requestXML.Content, false);
                return Responser(requestXML, list);
            }
            private string MsgLocation(WeChatXMLInfo requestXML)
            {
                //List<StoreInfo> list = StoreBLL.GetList();
                //List<double> Roader = new List<double>();
                //foreach (StoreInfo item in list)
                //{
                //    double d = GPSHelper.GetDistance((double)Tool.Function.ConverToDecimal(requestXML.Location_X),
                //        (double)Tool.Function.ConverToDecimal(requestXML.Location_Y),
                //        (double)item.LocationX, (double)item.LocationY);
                //    Roader.Add(d);
                //}
                //int NearIndexMost = 0;
                //int NearIndexNext = 0;
                //double Flag=0;
                //int i = -1;
                //foreach (double item in Roader)
                //{
                //    i++;
                //    if (Flag==0)
                //    {
                //        Flag = item;
                //        continue;
                //    }
                //    if (item<Flag)
                //    {
                //        NearIndexNext = NearIndexMost;
                //        NearIndexMost = i;                        
                //        Flag = item;
                //    }
                //}

                string vResponseText = string.Format("我知道你的位置在:lon:{0};lat:{1}", requestXML.Location_Y, requestXML.Location_X);
                //if (NearIndexMost>0)
                //{
                //    vResponseText = "距离你最近的店铺:";
                //    vResponseText += list[NearIndexMost].BrandName + " 它位于:" + list[NearIndexMost].LocationX + "  " + list[NearIndexMost].LocationY;
                //}
                //else
                //{
                //    vResponseText+= "暂时没有发现离得你很近的店";
                //}

                //if (NearIndexNext>0)
                //{
                //    vResponseText = "还有一家铺离你不远:";
                //    vResponseText += list[NearIndexNext].BrandName + " 它位于:" + list[NearIndexNext].LocationX + "  " + list[NearIndexNext].LocationY;
                //}
                string ShopListURL = ConfigHelper.GetAppendSettingValue(ConfigHelper.DoMain) + "/shoplist.aspx";
                vResponseText += string.Format("<a href='{0}?x={1}&y={2}'>点我进去看详细</a>", ShopListURL, requestXML.Location_X, requestXML.Location_Y);
                string resxml = string.Empty;
                resxml = ResponseText(requestXML, vResponseText);
                return resxml;
            }




            #endregion

            #region 回复函数

            private string Responser(WeChatXMLInfo requestXML, List<WeixinNewsInfo> list)
            {
                if (null == list || list.Count == 0)
                {
                    list = WeixinNewsBLL.Get(ConstKey.没有合适返回消息, true);
                }
                switch (list[0].MsgType)
                {
                    case 0:
                        return ResponseText(requestXML, list[0].Content);
                    case 1:
                        return ResponseTuWen(requestXML, list);
                    default:
                        return string.Empty;
                }
            }

            /// <summary>
            /// 回复文本
            /// </summary>
            /// <param name="requestXML"></param>
            /// <param name="_reMsg"></param>
            /// <returns></returns>
            private string ResponseText(WeChatXMLInfo requestXML, string _reMsg)
            {
                string ResTxt = "<xml>"
                    + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
                    + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
                    + "<CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime>"
                    + "<MsgType><![CDATA[text]]></MsgType>"
                    + "<Content><![CDATA[" + _reMsg + "]]></Content>"
                    + "</xml>";
                return ResTxt;
            }

            /// <summary>
            /// 回复图文
            /// </summary>
            /// <param name="requestXML"></param>
            /// <param name="list"></param>
            /// <returns></returns>
            private string ResponseTuWen(WeChatXMLInfo requestXML, List<WeixinNewsInfo> list)
            {
                if (null == list || list.Count <= 0)
                {
                    return string.Empty;
                }
                string ResTxt = "<xml>";
                ResTxt += "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>";
                ResTxt += "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>";
                ResTxt += "<CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";
                ResTxt += "<MsgType><![CDATA[news]]></MsgType>";
                ResTxt += "<ArticleCount>" + list.Count + "</ArticleCount>";
                ResTxt += "<Articles>";

                foreach (var item in list)
                {
                    ResTxt += "<item>";
                    ResTxt += "     <Title><![CDATA[" + item.Title + "]]></Title> ";
                    ResTxt += "     <Description><![CDATA[" + item.Description + "]]></Description>";
                    ResTxt += "     <PicUrl><![CDATA[" + item.Pic + "]]></PicUrl>";
                    ResTxt += "     <Url><![CDATA[" + item.LinkURL + "]]></Url>";
                    ResTxt += "</item>";

                }

                ResTxt += "</Articles>";
                ResTxt += "</xml> ";
                return ResTxt;
            }

            #endregion

            /// <summary>
            /// unix时间转换为datetime
            /// </summary>
            /// <param name="timeStamp"></param>
            /// <returns></returns>
            private DateTime UnixTimeToTime(string timeStamp)
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000000");
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }


            /// <summary>
            /// datetime转换为unixtime
            /// </summary>
            /// <param name="time"></param>
            /// <returns></returns>
            private int ConvertDateTimeInt(System.DateTime time)
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                return (int)(time - startTime).TotalSeconds;
            }




            /// <summary>
            /// Post 提交调用抓取
            /// </summary>
            /// <param name="url">提交地址</param>
            /// <param name="param">参数</param>
            /// <returns>string</returns>
            public string webRequestPost(string url, string param)
            {
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(param);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + "?" + param);
                req.Method = "Post";
                req.Timeout = 120 * 1000;
                req.ContentType = "application/x-www-form-urlencoded;";
                req.ContentLength = bs.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Flush();
                }

                using (WebResponse wr = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    Stream strm = wr.GetResponseStream();
                    StreamReader sr = new StreamReader(strm, System.Text.Encoding.UTF8);

                    string line;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line + System.Environment.NewLine);
                    }
                    sr.Close();
                    strm.Close();
                    return sb.ToString();
                }
            }

        }
    }

}
