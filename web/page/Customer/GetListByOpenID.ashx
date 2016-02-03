<%@ WebHandler Language="C#" Class="GetListByOpenID" %>

using System;
using System.Web;

using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Data;

public class GetListByOpenID : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string openId = context.Request["openId"];

        string result = "{\"resultInfo\":\"<option value='" + 0 + "'>请选择</option>";
        if (!string.IsNullOrEmpty(openId))
        {
            DataTable dt = CustomersBLL.GetCustomerByOpenID(openId);
            int i = 1;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //result += "<option value=\"" + dr["ID"].ToString() + "\">" + dr["f_Name"].ToString() + "</option>";
                    result += "<option value='" + i.ToString() + "'>" + dr["f_Name"].ToString() + "</option>";
                    i++;
                }
            }
        
        }
        result += "\"}";
        context.Response.Write(result);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}