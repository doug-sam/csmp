using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool
{
    public class KeyValueDictionary:Dictionary<string, string>
    {
        public KeyValueDictionary() { }

        public KeyValueDictionary(IDictionary<string, string> dictionary)
            : base(dictionary)
        { }

        /// <summary>
        /// 添加一个新的键值对。空键或者空值的键值对将会被忽略。
        /// </summary>
        /// <param name="key">键名称</param>
        /// <param name="value">键对应的值，目前支持：string, int, long, double, bool, DateTime类型</param>
        public void Add(string key, object value)
        {
            string strValue;

            if (value == null)
            {
                strValue = null;
            }
            else if (value is string)
            {
                strValue = (string)value;
            }
            else if (value is DateTime)
            {
                strValue = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (value is Boolean)
            {
                strValue = ((Boolean)value).ToString().ToLower();
            }
            else if (value is Nullable<DateTime>)
            {
                Nullable<DateTime> dateTime = value as Nullable<DateTime>;
                strValue = dateTime.HasValue ? ((DateTime)dateTime.Value).ToString("yyyy-MM-dd HH:mm:ss") : null;
            }
            else if (value is Nullable<Int16>)
            {
                Nullable<Int16> v = value as Nullable<Int16>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Int32>)
            {
                Nullable<Int32> v = value as Nullable<Int32>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Int64>)
            {
                Nullable<Int64> v = value as Nullable<Int64>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Single>)
            {
                Nullable<Single> v = value as Nullable<Single>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Decimal>)
            {
                Nullable<Decimal> v = value as Nullable<Decimal>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Double>)
            {
                Nullable<Double> v = value as Nullable<Double>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Boolean>)
            {
                Nullable<Boolean> v = value as Nullable<Boolean>;
                strValue = v.HasValue ? v.Value.ToString().ToLower() : null;
            }
            else if (value is Byte[])
            {
                strValue = Convert.ToBase64String((Byte[])value);
            }
            else
            {
                strValue = value.ToString();
            }

            this.Add(key, strValue);
        }
        //public new void Add(string key, string value)
        //{
        //    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
        //    {
        //        base.Add(key, value);
        //    }
        //}

        public new void Add(string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                base.Add(key, value);
            }
        }
    }
}
