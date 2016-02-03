<%@ WebHandler Language="C#" Class="GetListForWeixin" %>

using System;
using System.Web;

using CSMP.Model;
using CSMP.BLL;
using Tool;
using System.Data;
using System.Collections.Generic;

public class GetListForWeixin : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string customerId = context.Request["customerId"];
        string brandId = context.Request["brandId"];
        string openId = context.Request["openId"];
        string result = "{\"resultInfo\":\"";
      
        DataTable dt =KnowledgeBaseBLL.GetListByBothId(openId,customerId,brandId);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                //result += "<option value=\"" + dr["ID"].ToString() + "\">" + dr["f_Name"].ToString() + "</option>";

                
                    if (dr["AttType"].ToString().StartsWith("image"))
                    {

                        result += "<li data-corners='false' data-shadow='false' data-iconshadow='true' data-wrapperels='div' data-icon='arrow-r' data-iconpos='right' data-theme='c' class='ui-btn ui-btn-icon-right ui-li-has-arrow ui-li ui-btn-up-c'><div class='ui-btn-inner ui-li'><div class='ui-btn-text'><a href='/page/Attachment/ViewImgForWeiXin.aspx?ID=" + dr["AttId"].ToString() + "' target='_blank' class='ui-link-inherit'>" + dr["Title"].ToString() + "</a></div><span class='ui-icon ui-icon-arrow-r ui-icon-shadow'>&nbsp;</span></div></li>";
                        
                       // result += "<li><a href='/page/Attachment/ViewImg.aspx?ID=" + dr["AttId"].ToString() + ">" + dr["Title"].ToString() + "</a></li>";

                    }
                    else if (dr["AttExt"].ToString() == ".doc" || dr["AttExt"].ToString() == ".docx")
                    {
                        result += "<li data-corners='false' data-shadow='false' data-iconshadow='true' data-wrapperels='div' data-icon='arrow-r' data-iconpos='right' data-theme='c' class='ui-btn ui-btn-icon-right ui-li-has-arrow ui-li ui-btn-up-c'><div class='ui-btn-inner ui-li'><div class='ui-btn-text'><a href='/page/sys/WordToHTML.ashx?ID=" + dr["AttId"].ToString() + "' target='_blank' class='ui-link-inherit'>" + dr["Title"].ToString() + "</a></div><span class='ui-icon ui-icon-arrow-r ui-icon-shadow'>&nbsp;</span></div></li>";
                        //result += "<li><a href='/page/sys/WordToHTML.ashx?ID=" + dr["AttId"].ToString() + ">" + dr["Title"].ToString() + "</a></li>";
                    }
                    else
                    {
                        result += "<li data-corners='false' data-shadow='false' data-iconshadow='true' data-wrapperels='div' data-icon='arrow-r' data-iconpos='right' data-theme='c' class='ui-btn ui-btn-icon-right ui-li-has-arrow ui-li ui-btn-up-c'><div class='ui-btn-inner ui-li'><div class='ui-btn-text'><a href='/page/sys/DownLoadFile.ashx?ID=" + dr["AttId"].ToString() + "' target='_blank' class='ui-link-inherit'>" + dr["Title"].ToString() + "</a></div><span class='ui-icon ui-icon-arrow-r ui-icon-shadow'>&nbsp;</span></div></li>";
                       //result += "<li><a href='/page/sys/DownLoadFile.ashx?ID=" + dr["AttId"].ToString() + ">" + dr["Title"].ToString() + "</a></li>";
                    }
               
            }
        }      
        else
        {
            result += "<br/><h2>暂无数据</h2>";
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