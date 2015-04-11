using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Xml.Xsl;
using System.Text.RegularExpressions;
using System.Data.OleDb;

namespace Tool
{
    /// <summary>
    /// 基本工具类
    /// </summary>
    public abstract class Function
    {
        public static readonly DateTime ErrorDate = Convert.ToDateTime("1900-01-01");
        public static readonly int ErrorNumber = -999999999;
        public const string Admin = "admin";


        /// <summary>
        /// 格式化IP地址
        /// </summary>
        /// <param name="IP地址">IP地址</param>
        public static string FormatIP(string IP)
        {
            try
            {
                IPAddress NIP = IPAddress.Parse(IP);
                return NIP.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 记录分页函数
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="Recount">记录总数</param>
        /// <param name="file">分页文件</param>
        /// <param name="page">页码数</param>
        /// <param name="CurPage">当前页码</param>
        /// <param name="url">分页条件url</param>
        public static StringBuilder Paging(int PageSize, int Recount, int page, int CurPage, string file, string url)
        {
            StringBuilder SB = new StringBuilder();
            int MaxPage = 0; //总页数
            if (Recount % PageSize == 0)
            {
                MaxPage = Recount / PageSize;
            }
            else
            {
                MaxPage = Recount / PageSize + 1;
            }

            int Cpage = 1;
            if (MaxPage % page == 0)
            {
                Cpage = MaxPage / page;
            }
            else
            {
                Cpage = MaxPage / page + 1;
            }

            if (Cpage <= 1)
            {
                for (int i = 1; i <= MaxPage; i++)
                {
                    if (i == CurPage)
                    {
                        SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"" + file + "?page=" + i + "&" + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                    }
                }
            }
            else
            {
                int ii = 0;
                if (CurPage % page == 0)
                {
                    ii = CurPage / page - 1;
                }
                else
                {
                    ii = CurPage / page;
                }
                if (ii >= 1)
                {
                    SB.Append("&nbsp;<a href=\"" + file + "?page=" + ii * page + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                }
                for (int i = ii * page + 1; i <= (ii + 1) * page; i++)
                {
                    if (i > MaxPage)
                    {
                        break;
                    }
                    if (i == CurPage)
                    {
                        SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"" + file + "?page=" + i + "&" + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                    }
                }
                if (((ii + 1) * page + 1) <= MaxPage)
                {
                    if (ii < 1)
                    {
                        SB.Append("&nbsp;<a href=\"" + file + "?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"" + file + "?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                    }
                }
            }
            return SB;
        }

        /// <summary>
        /// 记录分页函数
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="Recount">记录总数</param>
        /// <param name="file">分页文件</param>
        /// <param name="page">页码数</param>
        /// <param name="CurPage">当前页码</param>
        /// <param name="url">分页条件url</param>
        public static string Paging4(int PageSize, int Recount, int page, int CurPage, string url)
        {

            CurPage = CurPage <= 0 ? 1 : CurPage;//传过来页码本身修正一下
            StringBuilder SB = new StringBuilder();
            int MaxPage = 0; //总页数
            if (Recount % PageSize == 0)
                MaxPage = Recount / PageSize;
            else
                MaxPage = Recount / PageSize + 1;
            int Cpage = 1;
            if (MaxPage % page == 0)
                Cpage = MaxPage / page;
            else
                Cpage = MaxPage / page + 1;
            if (Recount <= 0)
            {
                return "<div class='P_PageNoRecord'>没有合适条件数据</div>";
            }
            SB.Append("<span class='P_PageRecord'>[").Append(CurPage).Append("/").Append(MaxPage).Append("]&nbsp;&nbsp;");
            SB.Append("共 ").Append(Recount).Append("条数据&nbsp;&nbsp;&nbsp;&nbsp;</span>");

            if (CurPage == 1) SB.Append("<span class='P_PageUp'>上一页</span>");
            else SB.Append("<span class='P_PageUp'><a href=\"?page=" + (CurPage - 1) + url + "\">上一页</a></span>");


            if (Cpage <= 1)//一组页码能完全搞完的情况下
            {
                for (int i = 1; i <= MaxPage; i++)
                {
                    if (i == CurPage)
                        SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    else
                        SB.Append("&nbsp;<a href=\"?page=" + i + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                }
            }
            else
            {
                int ii = 0;
                if (CurPage % page == 0) ii = CurPage / page - 1;
                else ii = CurPage / page;
                if (ii >= 1)
                {
                    SB.Append("&nbsp;<a href=\"?page=" + ii * page + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                }
                for (int i = ii * page + 1; i <= (ii + 1) * page; i++)
                {
                    if (i > MaxPage) break;
                    if (i == CurPage) SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    else SB.Append("&nbsp;<a href=\"?page=" + i + "&" + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                }
                if (((ii + 1) * page + 1) <= MaxPage)
                {
                    if (ii < 1) SB.Append("&nbsp;<a href=\"?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                    else SB.Append("&nbsp;<a href=\"?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                }
            }
            if (CurPage == MaxPage) SB.Append("<span class='P_PageDown'>下一页</span>");
            else SB.Append("<span class='P_PageDown'><a href=\"?page=" + (CurPage + 1) + url + "\">下一页</a></span>");

            return SB.ToString();
        }
        /// <summary>
        /// 记录分页函数
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="Recount">记录总数</param>
        /// <param name="file">分页文件</param>
        /// <param name="page">页码数</param>
        /// <param name="CurPage">当前页码</param>
        /// <param name="url">分页条件url</param>
        public static string Paging2(int PageSize, int Recount, int page, int CurPage, string url)
        {

            CurPage = CurPage <= 0 ? 1 : CurPage;//传过来页码本身修正一下
            StringBuilder SB = new StringBuilder();
            int MaxPage = 0; //总页数
            if (Recount % PageSize == 0)
                MaxPage = Recount / PageSize;
            else
                MaxPage = Recount / PageSize + 1;
            int Cpage = 1;
            if (MaxPage % page == 0)
                Cpage = MaxPage / page;
            else
                Cpage = MaxPage / page + 1;
            if (Recount <= 0)
            {
                return "<div class='P_PageNoRecord'>没有合适条件数据</div>";
            }
            SB.Append("<span class='P_PageRecord'>[").Append(CurPage).Append("/").Append(MaxPage).Append("]&nbsp;&nbsp;");
            SB.Append("共 ").Append(Recount).Append("条数据&nbsp;&nbsp;&nbsp;&nbsp;</span>");

            if (CurPage == 1) SB.Append("<span class='P_PageUp'>上一页</span>");
            else SB.Append("<span class='P_PageUp'><a href=\"?page=" + (CurPage - 1) + url + "\">上一页</a></span>");


            if (Cpage <= 1)//一组页码能完全搞完的情况下
            {
                for (int i = 1; i <= MaxPage; i++)
                {
                    if (i == CurPage)
                        SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    else
                        SB.Append("&nbsp;<a href=\"?page=" + i + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                }
            }
            else
            {
                int ii = 0;
                if (CurPage % page == 0) ii = CurPage / page - 1;
                else ii = CurPage / page;
                if (ii >= 1)
                {
                    SB.Append("&nbsp;<a href=\"?page=" + ii * page + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                }
                for (int i = ii * page + 1; i <= (ii + 1) * page; i++)
                {
                    if (i > MaxPage) break;
                    if (i == CurPage) SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    else SB.Append("&nbsp;<a href=\"?page=" + i + "&" + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                }
                if (((ii + 1) * page + 1) <= MaxPage)
                {
                    if (ii < 1) SB.Append("&nbsp;<a href=\"?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                    else SB.Append("&nbsp;<a href=\"?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                }
            }
            if (CurPage == MaxPage) SB.Append("<span class='P_PageDown'>下一页</span>");
            else SB.Append("<span class='P_PageDown'><a href=\"?page=" + (CurPage + 1) + url + "\">下一页</a></span>");
            
            SB.Append("\n &nbsp;&nbsp;跳到第<input type='text' id='PagerTxbPage' style='width:30px' onclick='this.select();'  value='").Append(CurPage).Append("'/>页");
            SB.Append("\n &nbsp;<input type='button' value='确定' onclick='EnterPress();'/>");
            SB.Append("\n    <script type='text/javascript'>");
            SB.Append("\n        function EnterPress(){");
            SB.Append("\n            var vhref = location.href;");
            SB.Append("\n            var GoToPage = document.getElementById('PagerTxbPage').value;");
            SB.Append("\n            if(GoToPage>").Append(MaxPage).Append(") GoToPage=").Append(MaxPage);
            SB.Append("\n            vhref = vhref.split('?')[0];");
            SB.Append("\n            vhref = vhref + '?page=' + GoToPage+'").Append(url).Append("';");
            SB.Append("\n            location.href=vhref;");
            SB.Append("\n         }");
            SB.Append("\n    </script>");
            return SB.ToString();
        }

        /// <summary>
        /// 记录分页函数
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="Recount">记录总数</param>
        /// <param name="file">分页文件</param>
        /// <param name="page">页码数</param>
        /// <param name="CurPage">当前页码</param>
        /// <param name="url">分页条件url</param>
        public static string Paging1(int PageSize, int Recount, int page, int CurPage, string url)
        {
            StringBuilder SB = new StringBuilder();
            CurPage = CurPage < 1 ? 1 : CurPage;
            int MaxPage = 0; //总页数
            if (Recount % PageSize == 0)
            {
                MaxPage = Recount / PageSize;
            }
            else
            {
                MaxPage = Recount / PageSize + 1;
            }
            int Cpage = 1;
            if (MaxPage % page == 0)
            {
                Cpage = MaxPage / page;
            }
            else
            {
                Cpage = MaxPage / page + 1;
            }

            if (Cpage <= 1)
            {
                for (int i = 1; i <= MaxPage; i++)
                {
                    if (i == CurPage)
                    {
                        SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"?page=" + i + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                    }
                }
            }
            else
            {
                int ii = 0;
                if (CurPage % page == 0)
                {
                    ii = CurPage / page - 1;
                }
                else
                {
                    ii = CurPage / page;
                }
                if (ii >= 1)
                {
                    SB.Append("&nbsp;<a href=\"?page=" + ii * page + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                }
                for (int i = ii * page + 1; i <= (ii + 1) * page; i++)
                {
                    if (i > MaxPage)
                    {
                        break;
                    }
                    if (i == CurPage)
                    {
                        SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"?page=" + i + "&" + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                    }
                }
                if (((ii + 1) * page + 1) <= MaxPage)
                {
                    if (ii < 1)
                    {
                        SB.Append("&nbsp;<a href=\"?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                    }
                }
            }
            //if (Recount > 0)
            //{
            //    SB.Append("　<span style=\"color:#fff;\">共" + MaxPage + "页　" + Recount + "条记录</span>");
            //}
            return SB.ToString();
        }

        /// <summary>
        /// 记录分页函数
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="Recount">记录总数</param>
        /// <param name="file">分页文件</param>
        /// <param name="page">页码数</param>
        /// <param name="CurPage">当前页码</param>
        /// <param name="url">分页条件url</param>
        public static string Paging3(int PageSize, int Recount, int page, int CurPage, string url)
        {
            StringBuilder SB = new StringBuilder();
            CurPage = CurPage < 1 ? 1 : CurPage;

            int MaxPage = 0; //总页数
            if (Recount % PageSize == 0)
            {
                MaxPage = Recount / PageSize;
            }
            else
            {
                MaxPage = Recount / PageSize + 1;
            }
            int Cpage = 1;
            if (MaxPage % page == 0)
            {
                Cpage = MaxPage / page;
            }
            else
            {
                Cpage = MaxPage / page + 1;
            }

            if (Cpage <= 1)
            {
                for (int i = 1; i <= MaxPage; i++)
                {
                    if (i == CurPage)
                    {
                        SB.Append("&nbsp;<span class=\"Pageing_Current\">" + i + "</span>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"?page=" + i + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                    }
                }
            }
            else
            {
                int ii = 0;
                if (CurPage % page == 0)
                {
                    ii = CurPage / page - 1;
                }
                else
                {
                    ii = CurPage / page;
                }
                if (ii >= 1)
                {
                    SB.Append("&nbsp;<a href=\"?page=" + ii * page + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                }
                for (int i = ii * page + 1; i <= (ii + 1) * page; i++)
                {
                    if (i > MaxPage)
                    {
                        break;
                    }
                    if (i == CurPage)
                    {
                        SB.Append("&nbsp;<span class=\"Pageing_Current\" >" + i + "</span>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"?page=" + i + "&" + url + "\" class=\"a1\">" + i + "</a>&nbsp;");
                    }
                }
                if (((ii + 1) * page + 1) <= MaxPage)
                {
                    if (ii < 1)
                    {
                        SB.Append("&nbsp;<a href=\"?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                    }
                    else
                    {
                        SB.Append("&nbsp;<a href=\"?page=" + ((ii + 1) * page + 1) + "&" + url + "\" class=\"a1\">...</a>&nbsp;");
                    }
                }
            }
            #region 上一页 下一页
            if (MaxPage > 1)
            {
                if (CurPage > 1)
                {
                    SB.Insert(0, "<a href='?page=" + (CurPage - 1) + "&" + url + "' class='Pageing_up'>上一页</a>&nbsp;");
                }
                else
                {
                    SB.Insert(0, "<span class='Pageing_up'>上一页</span>&nbsp;");
                }
                if (CurPage < MaxPage)
                {
                    SB.Append("<a href='?page=" + (CurPage + 1) + "&" + url + "' class='Pageing_down'>下一页</a>&nbsp;");
                }
                else
                {
                    SB.Append("<span class='Pageing_down'>下一页</span>&nbsp;");
                }
            }

            #endregion
            if (Recount > 0)
            {
                SB.Append("　<span>共").Append(MaxPage).Append("页　当前第").Append(CurPage).Append("页  总记录").Append(Recount).Append("条</span>");
            }

            return SB.ToString();
        }




        /// <summary>
        /// 获取传递参数(字符串)
        /// </summary>
        public static string GetRequestSrtring(string input)
        {
            string temp = HttpContext.Current.Request.Form[input];
            if (string.IsNullOrEmpty(temp)) temp = HttpContext.Current.Request.QueryString[input];
            if (string.IsNullOrEmpty(temp)) temp = string.Empty;

            //temp = ReplaceText(temp, @"\s+", " ").Trim();

            return System.Web.HttpUtility.HtmlDecode(temp);
        }
        /// <summary>
        /// 用于获取地址栏字符串参数，当为空时警告并返回一上页面
        /// </summary>
        /// <param name="input">地址栏里得到的值</param>
        /// <param name="AlertMsg">警告信息字样</param>
        /// <returns>当地址栏字符不为空时返回该地符</returns>
        public static string GetRequestSrtring(string input, string AlertMsg)
        {
            string temp = HttpContext.Current.Request.Form[input];
            if (string.IsNullOrEmpty(temp)) temp = HttpContext.Current.Request.QueryString[input];
            if (string.IsNullOrEmpty(temp)) temp = string.Empty;
            if (string.IsNullOrEmpty(temp))
            { AlertBack(AlertMsg); }
            return temp;
        }

        /// <summary>
        /// 查看用户是否输入正确的日期时间
        /// </summary>
        /// <param name="input">用户输入（从地址栏得到）的字符串</param>
        /// <param name="AlertMsg">例如“请输入正确的时间！”字样</param>
        /// <returns></returns>
        public static DateTime GetRequestDateTime(string input, string AlertMsg, bool CanBeTrue)
        {
            DateTime Retu_Date = GetRequestDateTime(input);
            if (Retu_Date == Function.ErrorDate && !CanBeTrue)
            {
                AlertBack(AlertMsg);
            }
            return Retu_Date;
        }

        public static DateTime GetRequestDateTime(string input)
        {
            DateTime Retu_Date = Function.ErrorDate;
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    Retu_Date = Convert.ToDateTime(GetRequestSrtring(input));
                }
                catch (Exception)
                {
                    Retu_Date = Function.ErrorDate;
                }
            }
            return Retu_Date;
        }


        /// <summary>
        /// 改变地址栏参数,
        /// </summary>
        /// <param name="Url">传过来要改变的地址栏</param>
        /// <param name="Key">参数名</param>
        /// <param name="NewValue">参数新值</param>
        /// <returns>要求如果地址栏中没有参数则自动加上，有则变成新值返回</returns>
        public static string ChagePara(string Url, string Key, string NewValue)
        {
            if (string.IsNullOrEmpty(Url) || string.IsNullOrEmpty(Key)) return string.Empty;
            if (!Url.ToLower().Contains(Key.ToLower()))
            {
                if (Url.Contains("?"))
                {
                    return Url.Trim('&') + "&" + Key + "=" + NewValue;
                }
                return Url + "?" + Key + "=" + NewValue;
            }
            string[] arrUrl = Url.Split('?');
            string[] arrPara = arrUrl[1].Split('&');
            string strReturn = arrUrl[0].ToString() + "?";
            bool isFindKey = false;
            foreach (string p in arrPara)
            {
                if (("," + p + ",").IndexOf("," + Key + "=") > -1)
                {
                    strReturn += Key + "=" + NewValue + "&";
                    isFindKey = true;
                }
                else
                {
                    strReturn += p + "&";
                }
            }
            if (!isFindKey)
                return strReturn + Key + "=" + NewValue;
            return strReturn.TrimEnd('&');
        }

        /// <summary>
        /// 用于获取地址栏整型参数，当为空时警告并返回一上页面
        /// </summary>
        /// <param name="input">地址栏里得到的值</param>
        /// <param name="AlertMsg">警告信息字样</param>
        /// <returns>当地址获取内容符合要求时返回该地符</returns>
        public static int GetRequestInt(string input, string AlertMsg)
        {
            try
            {
                ConverToInt(GetRequestSrtring(input, AlertMsg));
                return ConverToInt(GetRequestSrtring(input));
            }
            catch (Exception)
            {
                AlertBack(AlertMsg);
            }
            return Function.ErrorNumber;
        }

        /// <summary>
        /// 获取传递参数(数字)
        /// </summary>
        public static int GetRequestInt(string input)
        {
            return ConverToInt(GetRequestSrtring(input));
        }

        /// <summary>
        /// 整型转换
        /// </summary>
        public static Int32 ConverToInt<T>(T input)
        {
            Int32 temp = 0;

            try
            {
                temp = Convert.ToInt32(input);
            }
            catch (Exception e)
            {
                temp = Function.ErrorNumber;
            }

            return temp;
        }

        /// <summary>
        /// 整型转换
        /// </summary>
        public static Int32 ConverToInt<T>(T input,int DefalutValue)
        {
            Int32 temp = DefalutValue;

            try
            {
                temp = Convert.ToInt32(input);
            }
            catch (Exception e)
            {
                temp = DefalutValue;
            }

            return temp;
        }

        /// <summary>
        /// 小数转换
        /// </summary>
        public static float ConverToSingle<T>(T input)
        {
            float temp = 0;

            try
            {
                temp = Convert.ToSingle(input);
            }
            catch (Exception e)
            {
                temp = Function.ErrorNumber;
            }

            return temp;
        }

        /// <summary>
        /// decimal转换
        /// </summary>
        public static decimal ConverToDecimal<T>(T input)
        {
            decimal temp = 0;

            try
            {
                temp =Convert.ToDecimal(Convert.ToDecimal(input).ToString("G0"));
            }
            catch (Exception e)
            {
                temp = Function.ErrorNumber;
            }

            return temp;
        }

        /// <summary>
        /// 字符转换
        /// </summary>
        public static string ConverToString<T>(T input)
        {
            string temp = string.Empty;

            try
            {
                temp = Convert.ToString(input);
            }
            catch (Exception e)
            {
                temp = string.Empty;
            }

            return temp;
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        public static DateTime ConverToDateTime<T>(T input)
        {
            DateTime temp = new DateTime();
            try
            {
                temp = Convert.ToDateTime(input);
            }
            catch (Exception e)
            {
                temp = Function.ErrorDate;
            }

            return temp;
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        public static void Refresh()
        {
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl, true);
        }

        /// <summary>
        /// 提示并刷新
        /// </summary>
        public static void AlertRefresh(string msg)
        {
            AlertRedirect(msg, HttpContext.Current.Request.RawUrl);
        }
        /// <summary>
        /// 提示并刷新
        /// </summary>
        public static void AlertRefresh(string msg, bool Parent)
        {
            msg = msg.Replace("\"", "'");
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(msg))
            {
                sb.Append("alert(\"").Append(msg).Append("\");");
            }
            if (Parent)
            {
                sb.Append("top.");
            }
            sb.Append("location.reload();");
            ScriptManager.RegisterClientScriptBlock((System.Web.UI.Page)HttpContext.Current.Handler, typeof(System.Web.UI.Page), "", sb.ToString(), true);
        }
        /// <summary>
        /// 提示并刷新
        /// </summary>
        public static void AlertRefresh(string msg, string Target)
        {
            msg = msg.Replace("\"", "'");
            //window.parent.frames["列表iframe名字"].location.reload();

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(msg))
            {
                sb.Append("alert(\"").Append(msg).Append("\");");
            }
            sb.Append("top.window.parent.frames['").Append(Target).Append("'].location.reload();");
            ScriptManager.RegisterClientScriptBlock((System.Web.UI.Page)HttpContext.Current.Handler, typeof(System.Web.UI.Page), "", sb.ToString(), true);
        }

        /// <summary>
        /// 弹出警告窗
        /// </summary>
        /// <param name="msg"></param>
        public static void AlertMsg(string msg)
        {
            ScriptManager.RegisterClientScriptBlock((System.Web.UI.Page)HttpContext.Current.Handler, typeof(System.Web.UI.Page), "", "alert('" + msg + "');", true);
        }


        /// <summary>
        /// 提示并后退
        /// </summary>
        public static void AlertBack(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>");
            if (!string.IsNullOrEmpty(msg))
            {
                sb.Append("alert(\"").Append(msg).Append("\");");
            }
            sb.Append("history.back();</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 提示并跳转
        /// </summary>
        public static void AlertRedirect(string msg,string url, string Target)
        {
            msg = msg.Replace("\"", "'");
            //window.parent.frames["列表iframe名字"].location.reload();

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(msg))
            {
                sb.Append("alert(\"").Append(msg).Append("\");");
            }
            sb.Append("top.window.parent.frames['").Append(Target).Append("'].location.href='").Append(url).Append("';");
            ScriptManager.RegisterClientScriptBlock((System.Web.UI.Page)HttpContext.Current.Handler, typeof(System.Web.UI.Page), "", sb.ToString(), true);
        }

        /// <summary>
        /// 提示并跳转
        /// </summary>
        public static void AlertRedirect(string msg, string url)
        {
            AlertRedirect(msg, url, false);
        }

        /// <summary>
        /// 提示并跳转
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转地址</param>
        /// <param name="Parent">是否为父窗口跳转</param>
        public static void AlertRedirect(string msg, string url, bool Parent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>");
            if (!string.IsNullOrEmpty(msg))
            {
                sb.Append("alert(\"").Append(msg).Append("\");");
            }
            if (Parent)
            {
                sb.Append("top.");
            }
            sb.Append("location.href='").Append(url).Append("';</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 排除符号
        /// </summary>
        public static string ClearText(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = ReplaceText(input, @"[^a-zA-Z0-9 \u4e00-\u9fa5\.]", string.Empty);
                return string.IsNullOrEmpty(input) ? string.Empty : input.Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 正则替换
        /// </summary>
        /// <param name="input">要替换的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="replacement">替换成的字符</param>
        /// <returns></returns>
        public static string ReplaceText(string input, string pattern, string replacement)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string temp = rgx.Replace(input, replacement);
            return temp;
        }

        /// <summary>
        /// 多出几个字条就用。。代替距
        /// </summary>
        /// <param name="old">要截的字符串</param>
        /// <param name="cutmuch">截几个</param>
        /// <returns></returns>
        public static string Cuter<T>(T input, int cutmuch)
        {
            if (null==input||string.IsNullOrEmpty(input.ToString()))
            {
                return string.Empty;
            }
            string old = input.ToString();
            if (cutmuch > old.Length || string.IsNullOrEmpty(old))
            {
                return old;
            }
            return old.Substring(0, cutmuch) + "...";

        }

        /// <summary>
        /// 去掉html
        /// </summary>
        /// <param name="HtmlCode"></param>
        /// <returns></returns>
        public static string RemoveHTML<T>(T input)
        {
            string HtmlCode = input.ToString();
            Regex re = new Regex(@"<img([\s\S]*?)>", RegexOptions.IgnoreCase);
            string MatchVale = HtmlCode;
            foreach (Match s in Regex.Matches(HtmlCode, "<.+?>"))
            {
                if (re.IsMatch(s.Value))
                {
                    MatchVale = MatchVale.Replace(s.Value, "[图片]");
                }
                MatchVale = MatchVale.Replace(s.Value, "");
            }
            return MatchVale;
        }

        /// <summary>
        /// 验证内容区域中是否符合表达式
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="strRegex">正则表达式</param>
        /// <returns>符合返回真，不符合返回假</returns>
        public static bool IsHasMatch(string content, string strRegex)
        {
            if (!Regex.IsMatch(content, strRegex))
            {
                return false;
            }
            return true;
        }

        private static Random rand = new Random();
        public static string GetRand()
        {
            return DateTime.Now.ToString("ddHHmmss") + rand.Next(100, 999).ToString();
        }

        #region --读取Excel填充到DS ExcelToDS(string filenameurl, string tablename)
        /// <summary>
        /// 读取Excel填充到DS
        /// </summary>
        /// <param name="filenameurl"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataTable ExcelToDatatable(string filenameurl, string tablename)
        {
            if (string.IsNullOrEmpty(tablename)) tablename = "[Sheet1$]";
            string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + filenameurl + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter odda = new OleDbDataAdapter("select * from " + tablename, conn);
                odda.Fill(ds, tablename);
                conn.Close();//原来的竟然没关闭。。。
                GC.Collect();
                foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                { process.Kill(); }
                return ds.Tables[0];
            } 
            
        }
        #endregion

        public abstract class Request
        {
            /// <summary>
            /// 获取传递参数(字符串)
            /// </summary>
            public static string GetRequestSrtring(HttpContext context, string input)
            {
                string temp = context.Request.Form[input];
                if (string.IsNullOrEmpty(temp)) temp = context.Request.QueryString[input];
                if (string.IsNullOrEmpty(temp)) temp = string.Empty;

                //temp = ReplaceText(temp, @"\s+", " ").Trim();

                return System.Web.HttpUtility.HtmlDecode(temp);
            }



            public static DateTime GetRequestDateTime(HttpContext context, string input)
            {
                DateTime Retu_Date = Function.ErrorDate;
                if (!string.IsNullOrEmpty(input))
                {
                    try
                    {
                        Retu_Date = Convert.ToDateTime(GetRequestSrtring(context, input));
                    }
                    catch (Exception)
                    {
                        Retu_Date = Function.ErrorDate;
                    }
                }
                return Retu_Date;
            }




            /// <summary>
            /// 获取传递参数(数字)
            /// </summary>
            public static int GetRequestInt(HttpContext context, string input)
            {
                return ConverToInt(GetRequestSrtring(context, input));
            }

        }

    }
}
