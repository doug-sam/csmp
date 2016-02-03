<%@ WebHandler Language="C#" Class="GetListByOpenIdAndCustomerName" %>

using System;
using System.Web;

using System.Collections.Generic;
using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Data;
public class GetListByOpenIdAndCustomerName : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string openId = context.Request["openId"];
        string customerName = context.Request["customerName"];

        string result = "{\"resultInfo\":\"<option value='" + 0 + "'>请选择</option>";
        if (!string.IsNullOrEmpty(openId)&&!string.IsNullOrEmpty(customerName)&&customerName!="请选择")
        {
            List<BrandInfo> bList = BrandBLL.GetList(" AND f_CustomerName = '" + customerName + "' ORDER BY f_Name");
            if (bList.Count > 0)
            {
                foreach (BrandInfo bInfo in bList)
                {
                    //result += "<option value=\"" + dr["ID"].ToString() + "\">" + dr["f_Name"].ToString() + "</option>";
                    result += "<option value='" +bInfo.ID.ToString() + "'>" + bInfo.Name.Trim() + "</option>";
                    
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