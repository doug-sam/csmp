using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Tool
{
    /// <summary>
    /// 数据验证类
    /// </summary>
    public abstract class DataValidator
    {
        /// <summary>
        /// 正整数验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input, UpdatePanel up, Type ty, string name, int minlen, int maxlen)
        {
            bool rel = true;
            if (minlen == 0)
            {
                return rel;
            }
            int len = input.Length;
            if (len < minlen || len > maxlen)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入长度应该在" + minlen.ToString() + "和" + maxlen.ToString() + "之间！');", true);
                rel = false;
                return rel;
            }
            if (string.IsNullOrEmpty(input))
            {
                rel = false;
            }
            rel = Regex.IsMatch(input, "^[0-9]+$");
            if (!rel)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入整数！');", true);
            }
            return rel;
        }
        public static bool IsNumber(string input, UpdatePanel up, Type ty, string name, int minlen, int maxlen, Control c)
        {
            if (!IsNumber( input,  up, ty, name, minlen, maxlen))
            {
                SetFocus(up, c);
                return false;
            }
            return true;
        }
        public static bool IsDecimal(string input, UpdatePanel up, Type ty, string name)
        {
            bool rel = IsDecimal(input);
            if (!rel)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入数字！');", true);
            }
            return rel;
        }

        /// <summary>
        /// 邮编验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPostCode(string input, UpdatePanel up, Type ty, string name)
        {
            bool rel = IsNumber(input, up, ty, name, 6, 6);
            if (!rel)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('请输入正确的邮政编码！');", true);
                rel = false;
            }
            return rel;
        }
        public static bool IsPostCode(string input, UpdatePanel up, Type ty, string name, Control c)
        {
            if (!IsPostCode(input, up, ty, name))
            {
                SetFocus(up, c);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 邮箱地址格式验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        /// <summary>
        /// 邮箱地址格式验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input, UpdatePanel up, Type ty, string name, int minlen, int maxlen)
        {
            bool rel = true;
            if (minlen == 0)
            {
                return rel;
            }
            int len = input.Length;
            if (len < minlen || len > maxlen)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入长度应该在" + minlen.ToString() + "和" + maxlen.ToString() + "之间！');", true);
                rel = false;
                return rel;
            }
            if (string.IsNullOrEmpty(input))
            {
                rel = false;
            }
            rel= IsEmail(input);
            if (!rel)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入正确的邮箱地址！');", true);
            }
            return rel;
        }

        /// <summary>
        /// URL验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(string input, UpdatePanel up, Type ty, string name, int minlen, int maxlen)
        {
            bool rel = true;
            if (minlen == 0)
            {
                return rel;
            }
            int len = input.Length;
            if (len < minlen || len > maxlen)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入长度应该在" + minlen.ToString() + "和" + maxlen.ToString() + "之间！');", true);
                rel = false;
                return rel;
            }
            if (string.IsNullOrEmpty(input))
            {
                rel = false;
            }
            rel = Regex.IsMatch(input, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
            if (!rel)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入正确的URL地址！');", true);
            }
            return rel;
        }

        /// <summary>
        /// 时间格式验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsTime(string input, UpdatePanel up, Type ty, string name)
        {
            bool rel = DataValidator.IsDateTime(input);           
            if (!DataValidator.IsDateTime(input))
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入正确日期时间格式，如1999-9-1 12:45:20！');", true);
            }
            return rel;
        }
        /// <summary>
        /// 验证时间格式是否正确，这个验证可以让时间为空或正确时间
        /// </summary>
        /// <param name="input">获得的输入值</param>
        /// <param name="up">UpdatePanel</param>
        /// <param name="ty">类型，用this.GetType()行了</param>
        /// <param name="name">出错时指定的错误信息</param>
        /// <param name="canBeEmpty">一个无意义的参数，只为与其不能为空的参数进行重载，默认用true就是时间可以为空</param>
        /// <returns></returns>
        public static bool IsTime(string input, UpdatePanel up, Type ty, string name, bool canBeEmpty)
        {
            bool rel = true;
            if (string.IsNullOrEmpty(input))
            {
                if (canBeEmpty)
                { return true; }
                return false;
            }
            return IsTime(input, up, ty, name);
        }
        /// <summary>
        /// 时间格式验证
        /// </summary>
        public static bool IsTime(string input, UpdatePanel up, Type ty, string name, Control c)
        {
            if (!IsTime(input, up, ty, name))
            {
                SetFocus(up, c);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证时间格式是否正确，这个验证可以让时间为空或正确时间
        /// </summary>
        /// <param name="input">获得的输入值</param>
        /// <param name="up">UpdatePanel</param>
        /// <param name="ty">类型，用this.GetType()行了</param>
        /// <param name="name">出错时指定的错误信息</param>
        /// <param name="canBeEmpty">一个无意义的参数，只为与其不能为空的参数进行重载，默认用true就是时间可以为空</param>
        /// <returns></returns>
        public static bool IsTime(string input, UpdatePanel up, Type ty, string name, bool canBeEmpty, Control c)
        {
            if (!IsTime(input, up, ty, name, canBeEmpty))
            {
                SetFocus(up, c);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 长度验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLen(string input, UpdatePanel up, Type ty, string name, int minlen, int maxlen)
        {
            int len = input.Length;

            bool rel = true;
            if (minlen == 0 && len == 0)
            {
                return rel;
            }
            if (string.IsNullOrEmpty(input))
            {
                rel = false;
            }

            if (len < minlen || len > maxlen)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入长度应该在" + minlen.ToString() + "和" + maxlen.ToString() + "之间！');", true);
                rel = false;
            }
            return rel;
        }
        /// <summary>
        /// 长度验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLen(string input, UpdatePanel up, Type ty, string name, int minlen, int maxlen, Control c)
        {
            if (!IsLen(input, up, ty, name, minlen, maxlen))
            {
                SetFocus(up, c);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 范围验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValue(string input, UpdatePanel up, Type ty, string name, int min, int max)
        {
            bool rel = true;
            if (string.IsNullOrEmpty(input))
            {
                rel = false;
            }
            int value = Convert.ToInt32(input);
            if (value < min || value > max)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，请输入应大于" + min.ToString() + "小于" + max.ToString() + "！');", true);
                rel = false;
            }
            return rel;
        }
        /// <summary>
        /// 范围验证
        /// </summary>
        public static bool IsValue(string input, UpdatePanel up, Type ty, string name, int min, int max,Control c)
        {
            if (!IsValue(input, up, ty, name, min, max))
            {
                SetFocus(up, c);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 用户名格式验证,不能含有\\/\"[]:|<>+=;,?*@
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string input, UpdatePanel up, Type ty, string name)
        {
            bool rel = true;
            if (string.IsNullOrEmpty(input))
            {
                rel = false;
            }
            if (input.Trim(new char[] { '.' }).Length == 0)
            {
                rel = false;
            }
            string str = "\\/\"[]:|<>+=;,?*@";
            for (int i = 0; i < input.Length; i++)
            {
                if (str.IndexOf(input[i]) >= 0)
                {
                    rel = false;
                    break;
                }
            }
            if (!rel)
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('用户名格式不正确！');", true);
            }
            return rel;
        }

        /// <summary>
        /// 仔细地验证18位身份证是否正确
        /// </summary>
        /// <param name="input">传过来的身份证号</param>
        /// <param name="up">UpdatePanel</param>
        /// <param name="ty">Type</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsIDCard(string input, UpdatePanel up, Type ty, string name)
        {
            return IsIDCard(input, up, ty, name, false);
        }
        /// <summary>
        /// 仔细地验证18位身份证是否正确，这里要么为空，要么就得认真写好整个身份证
        /// </summary>
        public static bool IsIDCard(string input, UpdatePanel up, Type ty, string name, bool canBeEmpty)
        {
            if (string.IsNullOrEmpty(input) && canBeEmpty)
            {
                if (canBeEmpty)
                {
                    return true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('填写身份证号！');", true);
                    return false;
                }
            }
            input = input.Trim();
            if (CheckCidInfo18(input) != "")
            {
                ScriptManager.RegisterClientScriptBlock(up, ty, "click", "alert('" + name + "，" + CheckCidInfo18(input) + "！');", true);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 仔细地验证18位身份证是否正确，这里要么为空，要么就得认真写好整个身份证
        /// </summary>
        public static bool IsIDCard(string input, UpdatePanel up, Type ty, string name, Control c)
        {
            if (!IsIDCard(input, up, ty, name))
            {
                SetFocus(up, c);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 仔细地验证18位身份证是否正确，这里要么为空，要么就得认真写好整个身份证
        /// </summary>
        public static bool IsIDCard(string input, UpdatePanel up, Type ty, string name, bool canBeEmpty, Control c)
        {
            if (!IsIDCard(input, up, ty, name, canBeEmpty))
            {
                SetFocus(up, c);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证18位身份证格式
        /// </summary>
        /// <param name="cid"></param>
        /// <returns>返回字符串,出错信息</returns>
        public static string CheckCidInfo18(string cid)
        {
            string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null, null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", "贵州", "云南", "西藏", null, null, null, null, null, null, "陕西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾", null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null, null, "国外" };
            double iSum = 0;
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|X|x)$");
            System.Text.RegularExpressions.Match mc = rg.Match(cid);
            if (!mc.Success)
            {
                return "- 您的身份证号码格式有误!";
            }
            cid = cid.ToLower();
            cid = cid.Replace("x", "a");
            if (aCity[int.Parse(cid.Substring(0, 2))] == null)
            {
                return "- 您的身份证号码格式有误!";//非法地区
            }
            try
            {
                DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));
            }
            catch
            {
                return "- 您的身份证号码格式有误!";//非法生日
            }
            for (int i = 17; i >= 0; i--)
            {
                iSum += (System.Math.Pow(2, i) % 11) * int.Parse(cid[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);

            }
            if (iSum % 11 != 1)
                return ("- 您的身份证号码格式有误!");//非法证号

            return "";

        }

        private static void SetFocus(UpdatePanel up, Control c)
        {
            ScriptManager scriptManager1 = (ScriptManager)up.Parent.FindControl("ScriptManager1");
            scriptManager1.SetFocus(c);
        }

        public static bool IsDateTime<T>(T input)
        {
            try
            {
                DateTime dt1 = Convert.ToDateTime(input);
                if (dt1 == Function.ErrorDate)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public static bool IsDecimal<T>(T input)
        {
            try
            {
                decimal dc1 = Convert.ToDecimal(input);
                if (dc1 == Function.ErrorNumber)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

    }
}
