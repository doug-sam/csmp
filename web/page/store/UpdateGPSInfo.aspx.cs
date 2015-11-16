using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSMP.BLL;
using CSMP.Model;
using Tool;
using Newtonsoft.Json.Linq;

public partial class page_store_UpdateGPSInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        List<StoreInfo> storeList = new List<StoreInfo>();
        storeList = StoresBLL.GetList(" 1=1 and f_GPS is null ");
        string storeAddress =string.Empty;
        string city = string.Empty;
        string result = string.Empty;
        string weidu = string.Empty;
        string jingdu = string.Empty;
        string gpsInfo = string.Empty;
        if (storeList.Count > 0)
        {
            foreach (StoreInfo item in storeList)
            {
                storeAddress=item.Address.Trim();
                city= item.CityName.Trim();
                if (!string.IsNullOrEmpty(storeAddress))
                {
                    try
                    {
                        result = GetGPSInfo.GetGPSInfoByBaiDuAPI(DeleteSpecialChar(storeAddress), city);
                    }
                    catch {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(result))
                    {
                        JObject obj = null;
                        try
                        {
                            obj = JObject.Parse(result);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        
                        try
                        {
                            result = obj["result"].ToString();
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        try
                        {
                            obj = JObject.Parse(result);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        try
                        {
                            result = obj["location"].ToString();
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        try
                        {
                            obj = JObject.Parse(result);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        try
                        {
                            jingdu = obj["lng"].ToString();
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        try
                        {
                            weidu = obj["lat"].ToString();
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        gpsInfo = jingdu + "," + weidu;
                        StoreInfo store = item;
                        store.GPS = gpsInfo;
                        try {
                            StoresBLL.Edit(store);
                        }
                        catch {
                            continue;
                        }
                        
                    }
                }
                
            }
        }
    }
    // <summary>
    /// 去除参数中的特殊符号包括\ " 回车 换行
    /// </summary>
    /// <param name="paramer"></param>
    /// <returns></returns>
    protected string DeleteSpecialChar(string paramer)
    {
        if (paramer.Contains("\\"))
        {
            paramer = paramer.Replace("\\", "\\\\");
        }
        if (paramer.Contains("\""))
        {
            paramer = paramer.Replace("\"", "'");
        }
        if (paramer.Contains("\r"))
        {
            paramer = paramer.Replace("\r", " ");
        }
        if (paramer.Contains("\n"))
        {
            paramer = paramer.Replace("\n", " ");
        }
        if (paramer.Contains("\t"))
        {
            paramer = paramer.Replace("\t", " ");
        }
        return paramer;

    }
}
