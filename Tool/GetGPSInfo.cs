using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool
{
    public class GetGPSInfo
    {
        public static string GetGPSInfoByBaiDuAPI(string address, string city)
        {
            string result = string.Empty;
            string baiduURL = "http://api.map.baidu.com/geocoder/v2/";
            string param = "ak=83aa33656a8da19e057bcbde00272e04&output=json&address=" + address + "&city=" + city;
            result = WebUtil.DoPost(baiduURL, param, 1);
            return result;
        
        }
    }
}
